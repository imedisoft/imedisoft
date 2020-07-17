using CentralManager;
using CodeBase;
using DataConnectionBase;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralPatientSearch : FormBase
	{
		private readonly SynchronizationContext synchronizationContext;
		private readonly List<CentralConnection> connections;
		private List<DisplayField> displayFields;
		private readonly DataSet patientsDataSet = new DataSet();
		private readonly List<Thread> patientSearchThreads = new List<Thread>();
		private string errorMessages = "";
		private bool searchCancelled = false;

		/// <summary>
		/// Gets the selected data row in the patients grid.
		/// </summary>
		public DataRow SelectedPatientDataRow { get; private set; }

		public FormCentralPatientSearch(List<CentralConnection> connections)
		{
			InitializeComponent();

			synchronizationContext = SynchronizationContext.Current;

			this.connections = connections;
		}

		private void FormCentralPatientSearch_Load(object sender, EventArgs e)
		{
			DisplayFields.RefreshCache();

			InitializePatientsGrid();

			StartSearchForPatients();
		}

		private void InitializePatientsGrid()
		{
			patientsGrid.Title = "Double Click to Select Patient";

			displayFields = DisplayFields.GetForCategory(DisplayFieldCategory.CEMTSearchPatients);

			foreach (var displayField in displayFields)
			{
				string heading = displayField.InternalName;

				if (!string.IsNullOrEmpty(displayField.Description))
				{
					heading = displayField.Description;
				}

				patientsGrid.ListGridColumns.Add(new GridColumn(heading, displayField.ColumnWidth));
			}
		}

		private PtTableSearchParams CreateSearchParams() 
			=> new PtTableSearchParams(
				limitCheckBox.Checked, 
				lastNameTextBox.Text, 
				firstNameTextBox.Text, 
				phoneTextBox.Text,
				addressTextBox.Text, 
				hideInactiveCheckBox.Checked, 
				cityTextBox.Text, 
				stateTextBox.Text, 
				ssnTextBox.Text, 
				patientNumberTextBox.Text, 
				chartNumberTextBox.Text, 
				0,
				guarantorsCheckBox.Checked, 
				!hideArchivedCheckBox.Checked,
				SIn.Date(birthdateTextBox.Text), 
				0, 
				subscriberIdTextBox.Text, 
				emailTextBox.Text, 
				countryTextBox.Text, 
				"", "", "", "",
				hasNextLastVisit: displayFields.Any(x => x.InternalName.In("NextVisit", "LastVisit")));
		
		private void ReportSearchResults(CentralConnection centralConnection, DataTable resultsDataTable)
        {
			Cursor = Cursors.WaitCursor;

			patientsGrid.BeginUpdate();
			patientsGrid.ListGridRows.Clear();

			foreach (DataRow dataRow in resultsDataTable.Rows)
            {
				var gridRow = new GridRow();

				foreach (var displayField in displayFields)
                {
					switch (displayField.InternalName)
                    {
						case "Conn":
							gridRow.Cells.Add(centralConnection.ToString());
							break;

						case "PatNum":
						case "LName":
						case "FName":
						case "SSN":
						case "PatStatus":
						case "City":
						case "State":
						case "Address":
						case "Email":
						case "Country":
						case "Birthdate":
							gridRow.Cells.Add(dataRow[displayField.InternalName].ToString());
							break;

						case "Age":
							gridRow.Cells.Add(dataRow["age"].ToString());
							break;

						case "Wk Phone":
							gridRow.Cells.Add(dataRow["WkPhone"].ToString());
							break;

						case "ChartNum":
							gridRow.Cells.Add(dataRow["ChartNumber"].ToString());
							break;

						case "MI":
							gridRow.Cells.Add(dataRow["MiddleI"].ToString());
							break;

						case "Pref Name":
							gridRow.Cells.Add(dataRow["Preferred"].ToString());
							break;

						case "Hm Phone":
							gridRow.Cells.Add(dataRow["HmPhone"].ToString());
							break;

						case "Bill Type":
							gridRow.Cells.Add(dataRow["BillingType"].ToString());
							break;

						case "Pri Prov":
							gridRow.Cells.Add(dataRow["PriProv"].ToString());
							break;

						case "Site":
							gridRow.Cells.Add(dataRow["site"].ToString());
							break;

						case "Clinic":
							gridRow.Cells.Add(dataRow["clinic"].ToString());
							break;

						case "Wireless Ph":
							gridRow.Cells.Add(dataRow["WirelessPhone"].ToString());
							break;

						case "Sec Prov":
							gridRow.Cells.Add(dataRow["SecProv"].ToString());
							break;

						case "LastVisit":
							gridRow.Cells.Add(dataRow["lastVisit"].ToString());
							break;

						case "NextVisit":
							gridRow.Cells.Add(dataRow["nextVisit"].ToString());
							break;
					}
                }

				gridRow.Tag = dataRow;

				patientsGrid.ListGridRows.Add(gridRow);
            }

			patientsGrid.EndUpdate();

			Cursor = Cursors.Default;
		}

		private void StartSearchForPatients()
		{
			patientsDataSet.Tables.Clear();

			var searchParams = CreateSearchParams();

			lock (patientSearchThreads)
			{
				errorMessages = "";

				foreach (var connection in connections)
				{
					var connectionName = $"{connection.ServerName}, {connection.DatabaseName}";
					if (!string.IsNullOrEmpty(connectionTextBox.Text))
					{
						if (!connectionName.Contains(connectionTextBox.Text))
						{
							continue;
						}
					}

					var thread = new Thread(o =>
					{
						var self = (Thread)o;

						try
						{
							if (!CentralConnectionHelper.SetCentralConnection(connection, true))
							{
								connection.ConnectionStatus = "OFFLINE";

								synchronizationContext.Post(state =>
								{
									errorMessages += "\r\n" + connection.ToString();

								}, null);

								return;
							}

							var table = Patients.GetPtDataTable(searchParams);

							synchronizationContext.Post(state =>
							{
								ReportSearchResults(connection, table);

							}, null);
						}
						catch (ThreadAbortException)
						{
						}
						catch
						{
							synchronizationContext.Post(state =>
							{
								errorMessages += "\r\n" + connection.ToString();

							}, null);
						}
                        finally
                        {
							lock (patientSearchThreads)
							{
								patientSearchThreads.Remove(self);
								if (!searchCancelled && patientSearchThreads.Count == 0)
								{
									synchronizationContext.Post(state =>
									{
										var messages = errorMessages;
										if (!string.IsNullOrEmpty(messages))
                                        {
											ShowError($"Could not connect to the following servers:{messages}");
										}

										searchButton.Text = "Search";
										fetchLabel.Visible = false;

									}, null);
								}
							}
						}
					});

					thread.Start(thread);

					patientSearchThreads.Add(thread);
				}
			}
		}

		private void PatientsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			SelectedPatientDataRow = (DataRow)patientsGrid.ListGridRows[e.Row].Tag;

			DialogResult = DialogResult.OK;
		}

		private void SearchButton_Click(object sender, EventArgs e)
		{
			lock (patientSearchThreads)
            {
				if (patientSearchThreads.Count == 0)
				{
					searchCancelled = false;
					searchButton.Text = "Stop";

					fetchLabel.Visible = true;

					StartSearchForPatients();
				}
				else
				{
					searchCancelled = true;

					foreach (var thread in patientSearchThreads)
					{
						thread.Abort();
					}

					patientSearchThreads.Clear();

					searchButton.Text = "Search";

					fetchLabel.Visible = false;
				}
            }
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (patientsGrid.GetSelectedIndex() == -1)
			{
				ShowError("Please select a patient, first.");

				return;
			}

			SelectedPatientDataRow = (DataRow)patientsGrid.ListGridRows[patientsGrid.GetSelectedIndex()].Tag;

			DialogResult = DialogResult.OK;
		}

		private void FormCentralPatientSearch_FormClosing(object sender, FormClosingEventArgs e)
		{
			searchCancelled = true;

			foreach (var thread in patientSearchThreads)
			{
				thread.Abort();
			}
		}
	}
}
