using CodeBase;
using DataConnectionBase;
using Imedisoft.CEMT.Forms;
using Imedisoft.Data;
using Newtonsoft.Json;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using Task = System.Threading.Tasks.Task;

namespace CentralManager
{
    public partial class FormCentralManager : FormBase
	{
		private List<CentralConnection> connections;
		private List<GroupPermission> reportPermissions;
		private readonly Dictionary<long, Process> processes = new Dictionary<long, Process>();



		private readonly DataSet _dataSetPats = new DataSet();
		private string _invalidConnsLog = "";
		private List<CentralConnection> _listConnsOK;
		private List<DisplayReport> _listDisplayReports_ProdInc;
		private readonly object _lockObj = new object();




		const int SW_SHOW = 5;
		const int SW_SHOWDEFAULT = 10;

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool ShowWindowAsync(IntPtr windowHandle, int nCmdShow);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool SetForegroundWindow(IntPtr windowHandle);

		public FormCentralManager() => InitializeComponent();

		private static bool ConnectAndFilter(CentralConnection connection, string providerFilter, string clinicFilter)
		{
			if (!CentralConnectionHelper.SetCentralConnection(connection, false))
			{
				connection.ConnectionStatus = "OFFLINE";

				return false;
			}

			if (providerFilter.Length > 0)
            {
				var providerFound = false;

				foreach (var provider in Providers.GetProvsNoCache())
                {
					var providerName = provider.Abbr + provider.LName + provider.FName;
					if (providerName.Contains(providerFilter))
                    {
						providerFound = true;

						break;
                    }
                }

				if (!providerFound) return false;
            }

			if (clinicFilter.Length > 0)
            {
				var clinicFound = false;

				foreach (var clinic in Clinics.GetClinicsNoCache())
                {
					if (clinic.Description.ToLower().Contains(clinicFilter))
                    {
						clinicFound = true;

						break;
					}
                }

				if (!clinicFound) return false;
            }

			return true;
		}

		private async void FilterButton_Click(object sender, EventArgs e)
		{
			var filterProvider = filterProviderTextBox.Text.Trim();
			var filterClinic = filterClinicTextBox.Text.Trim();

			if (filterProvider.Length == 0 && filterClinic.Length == 0)
			{
				FillConnectionsGrid();

				return;
			}

			var filteredConnectionIds = new List<long>();

			foreach (var gridRow in connectionsGrid.ListGridRows)
            {
				if (gridRow.Tag is CentralConnection connection)
                {
					if (connection.ConnectionStatus != "OK")
                    {
						continue;
                    }

					var result = await Task.Run(
						() => ConnectAndFilter(connection, 
							filterProvider, 
							filterClinic));

					if (result) filteredConnectionIds.Add(connection.CentralConnectionNum);
				}
            }

			FillConnectionsGrid(filteredConnectionIds);
		}

		private async void RefreshStatusesButton_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			if (connectionsGrid.SelectedIndices.Length == 0)
			{
				connectionsGrid.SetSelected(true);
			}

			for (int i = 0; i < connectionsGrid.SelectedIndices.Length; i++)
			{
				if (connectionsGrid.ListGridRows[connectionsGrid.SelectedIndices[i]].Tag is CentralConnection connection)
                {
					await Task.Run(() =>
					{
						if (!CentralConnectionHelper.SetCentralConnection(connection, false))
						{
							connection.ConnectionStatus = "OFFLINE";

							return;
						}

						connection.ConnectionStatus = "OK";
					});
                }
			}

			Cursor = Cursors.Default;

			FillConnectionsGrid();
		}

		private void SearchPatientsButton_Click(object sender, EventArgs e)
		{
			_listConnsOK = new List<CentralConnection>();
			if (connectionsGrid.SelectedIndices.Length == 0)
			{
				for (int i = 0; i < connectionsGrid.ListGridRows.Count; i++)
				{
					if (((CentralConnection)connectionsGrid.ListGridRows[i].Tag).ConnectionStatus != "OK")
					{
						continue;
					}
					_listConnsOK.Add((CentralConnection)connectionsGrid.ListGridRows[i].Tag);
				}
			}
			else
			{
				for (int i = 0; i < connectionsGrid.SelectedIndices.Length; i++)
				{
					if (((CentralConnection)connectionsGrid.ListGridRows[connectionsGrid.SelectedIndices[i]].Tag).ConnectionStatus != "OK")
					{
						continue;
					}
					_listConnsOK.Add((CentralConnection)connectionsGrid.ListGridRows[connectionsGrid.SelectedIndices[i]].Tag);
				}
			}
			ODThread.JoinThreadsByGroupName(1, "FetchPats");//Stop fetching immediately
			lock (_lockObj)
			{
				_invalidConnsLog = "";
			}
			if (butSearchPats.Text != "Search")
			{//in middle of previous search
				butSearchPats.Text = "Search";
				labelFetch.Visible = false;
				return;
			}
			Cursor = Cursors.WaitCursor;
			//_dataSetPats.Clear();
			butSearchPats.Text = "Stop Search";
			labelFetch.Visible = true;
			//Loops through all connections passed in and spawns a thread for each to go fetch patient data from each db using the given filters.
			//StartThreadsForConns();
			_dataSetPats.Tables.Clear();
			for (int i = 0; i < _listConnsOK.Count; i++)
			{
				//Filter the threads by their connection name
				string connName = "";

				connName = _listConnsOK[i].ServerName + ", " + _listConnsOK[i].DatabaseName;

				if (!connName.Contains(textConnPatSearch.Text))
				{
					//Do NOT spawn a thread to go fetch data for this connection because the user has filtered it out.
					//Increment the completed thread count and continue.
					continue;
				}
				//At this point we know the connection has not been filtered out, so fire up a thread to go get the patient data table for the search.
				ODThread odThread = new ODThread(GetDataTablePatForConn, new object[] { _listConnsOK[i] });
				odThread.GroupName = "FetchPats";
				odThread.Start();
			}
			ODThread.JoinThreadsByGroupName(Timeout.Infinite, "FetchPats");
			FillGridPats();
			butSearchPats.Text = "Search";
			labelFetch.Visible = false;

			Cursor = Cursors.Default;
			if (_invalidConnsLog != "")
			{
				ShowError("Could not connect to the following servers:" + _invalidConnsLog);
			}
		}

		private void ConnectionGroupComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			FillConnectionsGrid();
		}

		private void FormCentralManager_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.ApplicationExitCall) return;

			ODThread.QuitSyncThreadsByGroupName(0, "");
			foreach (var connection in connections)
			{
				CentralConnections.UpdateStatus(connection);
			}
		}

		private void FormCentralManager_Load(object sender, EventArgs e)
		{
			if (!LoadSettingsAndConfigure())
            {
				Application.Exit();

				return;
			}
			
			string syncCode = PrefC.GetString(PrefName.CentralManagerSyncCode);
			if (string.IsNullOrEmpty(syncCode))
			{
				var random = new Random();

				string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
				for (int i = 0; i < 10; i++)
				{
					syncCode += allowedChars[random.Next(allowedChars.Length)];
				}

				Prefs.UpdateString(PrefName.CentralManagerSyncCode, syncCode);
			}

			DisplayFields.RefreshCache();

			Text += " - " + Security.CurrentUser.UserName;

			connections = CentralConnections.GetConnections();

			versionLabel.Text = "Version: " + PrefC.GetString(PrefName.ProgramVersion);

			FillComboGroups(PrefC.GetLong(PrefName.ConnGroupCEMT));
			FillConnectionsGrid();

			reportPermissions = GroupPermissions.GetPermsForReports().Where(x => Security.CurrentUser.IsInUserGroup(x.UserGroupNum)).ToList();
			_listDisplayReports_ProdInc = DisplayReports.GetForCategory(DisplayReportCategory.ProdInc, false);
		}

		private void ConnectionsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (connectionsGrid.ListGridRows[e.Row].Tag is CentralConnection connection)
            {
				LaunchConnection(connection);
            }
		}

		private void PatientsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			DataRow dataRow = (DataRow)patientsGrid.ListGridRows[e.Row].Tag;
			CentralConnection conn = _listConnsOK.Find(x => (x.ServerName + ", " + x.DatabaseName) == dataRow["Conn"].ToString());
			long patNum = PIn.Long(dataRow["PatNum"].ToString());
			LaunchConnection(conn, patNum);
		}

		private void LogoffMenuItem_Click(object sender, EventArgs e)
		{
			using (var formCentralLogOn = new FormCentralLogOn())
			{
				if (formCentralLogOn.ShowDialog() != DialogResult.OK)
				{
					Application.Exit();

					return;
				}
			}

			Text = "Central Manager - " + Security.CurrentUser.UserName;
		}

		private void PasswordMenuItem_Click(object sender, EventArgs e)
		{
			using (var formCentralUserPasswordEdit = new FormCentralUserPasswordEdit(Security.CurrentUser.UserName, false, false))
			{
				if (formCentralUserPasswordEdit.ShowDialog(this) == DialogResult.Cancel)
				{
					return;
				}

				Security.CurrentUser.PasswordHash = formCentralUserPasswordEdit.PasswordHash;
				try
				{
					Userods.Update(Security.CurrentUser);
				}
				catch (Exception ex)
				{
                    ODMessageBox.Show(ex.Message);
				}
			}
		}

		private void TransferPatientMenuItem_Click(object sender, EventArgs e)
		{
			if (connectionsGrid.SelectedIndices.Count() != 1)
			{
				ShowInfo("Please select one and only one connection to start transfer.");
				return;
			}

			var connection = connectionsGrid.SelectedTag<CentralConnection>();
			if (!connection.IsConnectionValid())
			{
				ShowError("Server Offline. Fix connection and check status again to connect.");
				return;
			}

			using (var formCentralPatientTransfer = new FormCentralPatientTransfer(connection))
			{
				formCentralPatientTransfer.ShowDialog(this);
			}
		}

		private void ConnectionsMenuItem_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup)) return;

			using (var formCentralConnections = new FormCentralConnections())
			{
				formCentralConnections.ShowDialog();
			}

			connections = CentralConnections.GetConnections();

			FillConnectionsGrid();
		}

		private void GroupsMenuItem_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup)) return;

			var connectionGroup = connectionGroupComboBox.SelectedItem as ConnectionGroup;

			using (var formCentralConnectionGroups = new FormCentralConnectionGroups())
			{
				formCentralConnectionGroups.ShowDialog();
			}

			FillComboGroups(connectionGroup?.ConnectionGroupNum ?? 0);
			FillConnectionsGrid();
		}

		private void ReportSetupMenuItem_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup)) return;

			using (var formCentralReportSetup = new FormCentralReportSetup(Security.CurrentUser.Id, true))
			{
				formCentralReportSetup.ShowDialog();
			}
		}

		private void SecurityMenuItem_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.SecurityAdmin)) return;

			using (var formCentralSecurity = new FormCentralSecurity())
			{
				formCentralSecurity.ShowDialog(this);
			}

			connections = CentralConnections.GetConnections();

			FillConnectionsGrid();
		}

		private void DisplayFieldsMenuItem_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup)) return;

			using (var formDisplayFieldCategories = new FormDisplayFieldCategories(true))
			{
				formDisplayFieldCategories.ShowDialog();
			}

			DisplayFields.RefreshCache();

			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Display Fields");
		}

		private void ProductionIncomeMenuItem_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Reports)) return;

			if (!reportPermissions.Exists(x => x.FKey == _listDisplayReports_ProdInc.FirstOrDefault(y => y.InternalName == DisplayReports.ReportNames.ODMoreOptions)?.DisplayReportNum))
			{
				ShowInfo("You do not have the 'More Options' report permission.");
				return;
			}

			if (Security.CurrentUser.ProvNum == 0 && !Security.IsAuthorized(Permissions.ReportProdIncAllProviders, true))
			{
				ShowInfo("The current user needs to have the 'All Providers' permission for this report");
				return;
			}

			var selectedConnections = new List<CentralConnection>();
			foreach (var gridRow in connectionsGrid.SelectedGridRows)
			{
				if (gridRow.Tag is CentralConnection connection)
				{
					if (connection.ConnectionStatus.Contains("OFFLINE"))
					{
						ShowInfo("One or more connections are offline. Please remove the offline connection to run this report.");
						return;
					}

					selectedConnections.Add(connection);
				}
			}

			if (selectedConnections.Count == 0)
			{
				ShowInfo("Please select at least one connection to run this report against.");
				return;
			}

			using (var formCentralProdInc = new FormCentralProdInc())
			{
				formCentralProdInc.ConnList = selectedConnections;
				formCentralProdInc.ShowDialog(this);
			}
		}

		private void FillComboGroups(long connectionGroupId)
		{
			connectionGroupComboBox.Items.Clear();
			connectionGroupComboBox.Items.Add("All");
			connectionGroupComboBox.SelectedIndex = 0;

			var connectionGroups = ConnectionGroups.GetAll();

			foreach (var connectionGroup in connectionGroups)
            {
				connectionGroupComboBox.Items.Add(connectionGroup);
				if (connectionGroup.ConnectionGroupNum == connectionGroupId)
                {
					connectionGroupComboBox.SelectedItem = connectionGroup;
				}
			}
		}

		public IEnumerable<CentralConnection> FilterConnections(IEnumerable<CentralConnection> connections, string filterText)
		{
            if (connectionGroupComboBox.SelectedItem is ConnectionGroup connectionGroup)
            {
                var connectionGroupAttaches = ConnGroupAttaches.GetForGroup(connectionGroup.ConnectionGroupNum);

                connections = connections.Where(
                    connection => connectionGroupAttaches.Exists(
                        attach => attach.CentralConnectionNum == connection.CentralConnectionNum));
            }

            return connections.Where(x => x.ToString().ToLower().Contains(filterText.ToLower()));
		}

		private void FillConnectionsGrid(List<long> connectionIds = null)
		{
			// TODO: connectionIds should be class wide to presist filter across grid refreshes...

			connectionsGrid.BeginUpdate();
			connectionsGrid.ListGridColumns.Clear();
            connectionsGrid.ListGridColumns.Add(new GridColumn("#", 20) { SortingStrategy = GridSortingStrategy.AmountParse });
			connectionsGrid.ListGridColumns.Add(new GridColumn("Status", 70, HorizontalAlignment.Center));
			connectionsGrid.ListGridColumns.Add(new GridColumn("Database", 280));
			connectionsGrid.ListGridColumns.Add(new GridColumn("Note", 280));
			connectionsGrid.ListGridRows.Clear();

			foreach (var connection in FilterConnections(connections, filterConnectionTextBox.Text))
			{
				if (connectionIds != null && !connectionIds.Contains(connection.CentralConnectionNum))
				{
					continue;
				}

				var gridRow = new GridRow();
				var gridStatusCell = new GridCell();

				switch (connection.ConnectionStatus)
				{
					case "":
						gridStatusCell.Text = "Not Checked";
						gridStatusCell.ColorText = Color.DarkGoldenrod;
						break;

					case "OK":
						gridStatusCell.Text = connection.ConnectionStatus;
						gridStatusCell.ColorText = Color.Green;
						gridStatusCell.Bold = YN.Yes;
						break;

					case "OFFLINE":
						gridStatusCell.Text = connection.ConnectionStatus;
						gridRow.Bold = true;
						gridRow.ColorText = Color.Red;
						break;

					default:
						gridStatusCell.Text = connection.ConnectionStatus;
						gridStatusCell.ColorText = Color.Red;
						break;
				}

				gridRow.Cells.Add(connection.ItemOrder.ToString());
				gridRow.Cells.Add(gridStatusCell);
				gridRow.Cells.Add(connection.ToString());
				gridRow.Cells.Add(connection.Note);
				gridRow.Tag = connection;

				connectionsGrid.ListGridRows.Add(gridRow);
			}

			connectionsGrid.EndUpdate();
		}

		private void FillGridPats()
		{
			if (_dataSetPats.Tables.Count == 0)
			{
				patientsGrid.BeginUpdate();
				patientsGrid.ListGridColumns.Clear();
				patientsGrid.ListGridRows.Clear();
				patientsGrid.EndUpdate();
				return;
			}
			//create a single table so that we can sort.
			//An alternative would be a list of DataRows.
			//List<DataRow> listRows=new List<DataRow>();
			DataTable table = _dataSetPats.Tables[0].Clone();//just structure, no data
			table.Columns.Add("Conn");
			for (int i = 0; i < _dataSetPats.Tables.Count; i++)
			{
				_dataSetPats.Tables[i].Columns.Add("Conn");
				for (int j = 0; j < _dataSetPats.Tables[i].Rows.Count; j++)
				{
					_dataSetPats.Tables[i].Rows[j]["Conn"] = _dataSetPats.Tables[i].TableName;
				}
				table.Merge(_dataSetPats.Tables[i]);
			}
			DataView dataView = table.DefaultView;
			dataView.Sort = "LName,FName";
			table = dataView.ToTable();
			patientsGrid.BeginUpdate();
			List<DisplayField> fields = DisplayFields.GetForCategory(DisplayFieldCategory.CEMTSearchPatients);
			patientsGrid.ListGridColumns.Clear();
			foreach (DisplayField field in fields)
			{
				string heading = field.InternalName;
				if (!string.IsNullOrEmpty(field.Description))
				{
					heading = field.Description;
				}
				patientsGrid.ListGridColumns.Add(new GridColumn(heading, field.ColumnWidth));
			}
			patientsGrid.ListGridRows.Clear();
			GridRow gridRow;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				gridRow = new GridRow();
				foreach (DisplayField field in fields)
				{
					switch (field.InternalName)
					{
						#region Row Cell Filling
						case "Conn":
							gridRow.Cells.Add(table.Rows[i]["Conn"].ToString());
							break;
						case "PatNum":
							gridRow.Cells.Add(table.Rows[i]["PatNum"].ToString());
							break;
						case "LName":
							gridRow.Cells.Add(table.Rows[i]["LName"].ToString());
							break;
						case "FName":
							gridRow.Cells.Add(table.Rows[i]["FName"].ToString());
							break;
						case "SSN":
							gridRow.Cells.Add(table.Rows[i]["SSN"].ToString());
							break;
						case "PatStatus":
							gridRow.Cells.Add(table.Rows[i]["PatStatus"].ToString());
							break;
						case "Age":
							gridRow.Cells.Add(table.Rows[i]["age"].ToString());
							break;
						case "City":
							gridRow.Cells.Add(table.Rows[i]["City"].ToString());
							break;
						case "State":
							gridRow.Cells.Add(table.Rows[i]["State"].ToString());
							break;
						case "Address":
							gridRow.Cells.Add(table.Rows[i]["Address"].ToString());
							break;
						case "Wk Phone":
							gridRow.Cells.Add(table.Rows[i]["WkPhone"].ToString());
							break;
						case "Email":
							gridRow.Cells.Add(table.Rows[i]["Email"].ToString());
							break;
						case "ChartNum":
							gridRow.Cells.Add(table.Rows[i]["ChartNumber"].ToString());
							break;
						case "MI":
							gridRow.Cells.Add(table.Rows[i]["MiddleI"].ToString());
							break;
						case "Pref Name":
							gridRow.Cells.Add(table.Rows[i]["Preferred"].ToString());
							break;
						case "Hm Phone":
							gridRow.Cells.Add(table.Rows[i]["HmPhone"].ToString());
							break;
						case "Bill Type":
							gridRow.Cells.Add(table.Rows[i]["BillingType"].ToString());
							break;
						case "Pri Prov":
							gridRow.Cells.Add(table.Rows[i]["PriProv"].ToString());
							break;
						case "Birthdate":
							gridRow.Cells.Add(table.Rows[i]["Birthdate"].ToString());
							break;
						case "Site":
							gridRow.Cells.Add(table.Rows[i]["site"].ToString());
							break;
						case "Clinic":
							gridRow.Cells.Add(table.Rows[i]["clinic"].ToString());
							break;
						case "Wireless Ph":
							gridRow.Cells.Add(table.Rows[i]["WirelessPhone"].ToString());
							break;
						case "Sec Prov":
							gridRow.Cells.Add(table.Rows[i]["SecProv"].ToString());
							break;
						case "LastVisit":
							gridRow.Cells.Add(table.Rows[i]["lastVisit"].ToString());
							break;
						case "NextVisit":
							gridRow.Cells.Add(table.Rows[i]["nextVisit"].ToString());
							break;
						case "Country":
							gridRow.Cells.Add(table.Rows[i]["Country"].ToString());
							break;
							#endregion
					}
				}
				gridRow.Tag = table.Rows[i];
				patientsGrid.ListGridRows.Add(gridRow);
			}
			patientsGrid.EndUpdate();
		}

		private class SettingsDto
		{
			[JsonProperty(PropertyName = "connectionString")]
			public string ConnectionString { get; set; }
		}

		/// <summary>
		/// Loads the settings from the 'cemt.json' configuration file and configures the program.
		/// </summary>
		/// <returns>True if the program was configured succesfully; otherwise, false.</returns>
		private bool LoadSettingsAndConfigure()
		{
			var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Imedisoft", "cemt.json");
			if (!File.Exists(path))
			{
				ShowInfo("Please create 'cemt.json' according to the manual before using this tool.");

				return false;
			}

			SettingsDto settings;
            try
            {
				settings = JsonConvert.DeserializeObject<SettingsDto>(File.ReadAllText(path));
				if (string.IsNullOrEmpty(settings.ConnectionString))
                {
					ShowError("The settings file contains an empty connection string.");

					return false;
                }
            }
			catch (Exception exception)
            {
				ShowError(exception.Message);

				return false;
            }

            var dataConnection = new DataConnection();

			try
			{
				dataConnection.SetDb(settings.ConnectionString);

				using (var formCentralLogOn = new FormCentralLogOn())
				{
					if (formCentralLogOn.ShowDialog(this) != DialogResult.OK)
					{
						return false;
					}
				}
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return false;
			}

			return true;
		}

		private void GetDataTablePatForConn(ODThread odThread)
		{
			CentralConnection connection = (CentralConnection)odThread.Parameters[0];
			//Filter the threads by their connection name
			string connName = "";

			connName = connection.ServerName + ", " + connection.DatabaseName;

			if (!CentralConnectionHelper.SetCentralConnection(connection, false))
			{
				lock (_lockObj)
				{
					_invalidConnsLog += "\r\n" + connName;
				}
				connection.ConnectionStatus = "OFFLINE";
				//BeginInvoke((Action)FillGridPats);
				return;
			}
			List<DisplayField> fields = DisplayFields.GetForCategory(DisplayFieldCategory.CEMTSearchPatients);
			bool hasNextLastVisit = fields.Any(x => x.InternalName.In("NextVisit", "LastVisit"));
			DataTable table = new DataTable();
			try
			{
				PtTableSearchParams ptTableSearchParams = new PtTableSearchParams(checkLimit.Checked, textLName.Text, textFName.Text, textPhone.Text,
					textAddress.Text, checkHideInactive.Checked, textCity.Text, textState.Text, textSSN.Text, textPatNum.Text, textChartNumber.Text, 0,
					checkGuarantors.Checked, !checkHideArchived.Checked,//checkHideArchived is opposite label for what this function expects, but hideArchived makes more sense
					SIn.Date(textBirthdate.Text), 0, textSubscriberID.Text, textEmail.Text, textCountry.Text, "", "", textClinicPatSearch.Text, "", hasNextLastVisit: hasNextLastVisit);
				table = Patients.GetPtDataTable(ptTableSearchParams);
			}
			catch (ThreadAbortException tae)
			{
				throw tae;//ODThread needs to clean up after an abort exception is thrown.
			}
			catch (Exception)
			{
				//This can happen if the connection to the server was severed somehow during the execution of the query.
				lock (_lockObj)
				{
					_invalidConnsLog += "\r\n" + connName + "  -GetPtDataTable";
				}
				//BeginInvoke((Action)FillGridPats);//Pops up a message box if this was the last thread to finish.
				return;
			}
			table.TableName = connName;
			odThread.Tag = table;
			lock (_lockObj)
			{
				_dataSetPats.Tables.Add((DataTable)odThread.Tag);
			}
			//BeginInvoke((Action)FillGridPats);
		}

	


		private void LaunchConnection(CentralConnection conn, long patNum = 0)
		{
			if (conn.ConnectionStatus.StartsWith("OFFLINE"))
			{
				ShowError("Server Offline. Fix connection and check status again to connect.");
				return;
			}

			if (conn.ConnectionStatus != "OK")
			{
				ShowError("Version mismatch. Either update your program or update the remote server's program and check status again to connect.");
				return;
			}

			if (string.IsNullOrEmpty(conn.DatabaseName))
			{
				ShowError("database must be specified in the connection.");
				return;
			}

			// Remove all processes that have exited from the dictionary...
			foreach (var kvp in processes.Where(kvp => kvp.Value.HasExited).ToList())
            {
				processes.Remove(kvp.Key);
			}

			// If there is no process for the connection or the existing process as exited, spawn a new process...
			if (!processes.TryGetValue(conn.CentralConnectionNum, out var process) || process.HasExited)
			{
				process = CentralConnectionHelper.LaunchProgram(conn, patNum);
				processes[conn.CentralConnectionNum] = process;
			}

			// Bring the window to the front.
			if (process != null)
            {
				var windowHandle = process.MainWindowHandle;
				if (windowHandle != IntPtr.Zero)
				{
					ShowWindowAsync(process.MainWindowHandle, SW_SHOWDEFAULT);
					ShowWindowAsync(process.MainWindowHandle, SW_SHOW);

					SetForegroundWindow(process.MainWindowHandle);
				}
			}
		}
	}
}
