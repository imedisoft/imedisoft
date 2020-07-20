using CentralManager;
using CodeBase;
using DataConnectionBase;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Task = System.Threading.Tasks.Task;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralPatientTransfer : FormBase
	{
		private readonly CentralConnection sourceConnection;
		private readonly List<DataRow> patientDataRows = new List<DataRow>();
		private readonly List<CentralConnection> selectedConnections = new List<CentralConnection>();

		public FormCentralPatientTransfer(CentralConnection sourceConnection)
		{
			this.sourceConnection = sourceConnection;

			InitializeComponent();
		}

		private void FormCentralPatientTransfer_Load(object sender, EventArgs e)
		{
			sourceLabel.Text += sourceConnection.DatabaseName;

			FillPatients();
			FillDatabases();
		}

		private void FillDatabases()
		{
			Cursor = Cursors.WaitCursor;

			targetDatabasesGrid.BeginUpdate();
			targetDatabasesGrid.ListGridColumns.Clear();
			targetDatabasesGrid.ListGridColumns.Add(new GridColumn("Databases", 150));
			targetDatabasesGrid.ListGridColumns.Add(new GridColumn("Note", 200));
			targetDatabasesGrid.ListGridColumns.Add(new GridColumn("Status", 30, HorizontalAlignment.Center));
			targetDatabasesGrid.ListGridRows.Clear();

			foreach (var connection in selectedConnections)
			{
				var gridRow = new GridRow();

				gridRow.Cells.Add(connection.ToString());
				gridRow.Cells.Add(connection.Note);
				gridRow.Cells.Add(connection.ConnectionStatus);
				gridRow.Tag = connection;

				targetDatabasesGrid.ListGridRows.Add(gridRow);
			}

			targetDatabasesGrid.EndUpdate();

			Cursor = Cursors.Default;
		}

		private void FillPatients()
		{
			Cursor = Cursors.WaitCursor;

			patientsGrid.BeginUpdate();
			patientsGrid.ListGridColumns.Clear();
			patientsGrid.ListGridColumns.Add(new GridColumn("PatNum", 140));
			patientsGrid.ListGridColumns.Add(new GridColumn("LName", 140));
			patientsGrid.ListGridColumns.Add(new GridColumn("FName", 140, true));
			patientsGrid.ListGridRows.Clear();

			foreach (var dataRow in patientDataRows)
			{
				var gridRow = new GridRow();

				gridRow.Cells.Add(dataRow["PatNum"].ToString());
				gridRow.Cells.Add(dataRow["LName"].ToString());
				gridRow.Cells.Add(dataRow["FName"].ToString());
				gridRow.Tag = dataRow;

				patientsGrid.ListGridRows.Add(gridRow);
			}

			patientsGrid.EndUpdate();

			Cursor = Cursors.Default;
		}

		private void PatientsAddButton_Click(object sender, EventArgs e)
		{
			using (var formCentralPatientSearch = new FormCentralPatientSearch(new List<CentralConnection> { sourceConnection }))
			{
				if (formCentralPatientSearch.ShowDialog() == DialogResult.OK)
				{
					var patientDataRow = formCentralPatientSearch.SelectedPatientDataRow;
					if (patientDataRows.Any(dr => dr["PatNum"].ToString() == patientDataRow["PatNum"].ToString()))
                    {
						return;
                    }

					patientDataRows.Add(patientDataRow);

					FillPatients();
				}
			}
		}

		private void PatientsRemoveButton_Click(object sender, EventArgs e)
		{
			if (patientsGrid.SelectedIndices.Count() == 0)
			{
				ShowInfo("At least one patient must be selected.");
				return;
			}

			foreach (var gridRow in patientsGrid.SelectedGridRows)
			{
				patientDataRows.Remove((DataRow)gridRow.Tag);
			}

			FillPatients();
		}

		private void DatabasesAddButton_Click(object sender, EventArgs e)
		{
			using (var formCentralConnections = new FormCentralConnections())
			{
				formCentralConnections.Text = "Select Databases";
				formCentralConnections.IsSelectionMode = true;

				if (formCentralConnections.ShowDialog() == DialogResult.OK)
				{
					selectedConnections.AddRange(
						formCentralConnections.SelectedConnections.FindAll(x => 
							x.CentralConnectionNum != sourceConnection.CentralConnectionNum &&  
							!selectedConnections.Any(y => y.CentralConnectionNum == x.CentralConnectionNum)));

					FillDatabases();
				}
			}
		}

		private void DatabasesRemoveButton_Click(object sender, EventArgs e)
		{
			if (targetDatabasesGrid.SelectedIndices.Count() == 0)
			{
				ShowInfo("At least one connection must be selected.");
				return;
			}

			foreach (GridRow gridRow in targetDatabasesGrid.SelectedGridRows)
			{
				selectedConnections.Remove((CentralConnection)gridRow.Tag);
			}

			FillDatabases();
		}

		private void ExportButton_Click(object sender, EventArgs e)
		{
			if (patientsGrid.ListGridRows.Count == 0)
			{
				ShowInfo("At least one patient must be selected to transfer.");
				return;
			}

			if (targetDatabasesGrid.ListGridRows.Count == 0)
			{
				ShowInfo("At least one database must be selected to transfer.");
				return;
			}

			if (Confirm("The transfer process may take a long time. Continue?") == DialogResult.No)
			{
				return;
			}

			ODProgress.ShowAction(
				() => RunTransfer(), 
				startingMessage: "Transferring patient(s)...",
				actionException: err => this.Invoke(() => FriendlyException.Show("Error transferring patient(s).", err)));
		}

		private IEnumerable<Sheet> GetSheetsForTransfer()
		{
			var patients = Patients.GetMultPats(
				patientDataRows.Select(
					dataRow => SIn.Long(dataRow["PatNum"].ToString())).ToList());

			foreach (Patient patient in patients)
			{
				var sheet = SheetUtil.CreateSheet(SheetsInternal.GetSheetDef(SheetInternalType.PatientTransferCEMT));

				SheetFiller.FillFieldsForPatientTransferCEMT(sheet, patient);

				yield return sheet;
			}
		}

		private async void RunTransfer()
		{
			CentralConnectionHelper.SetCentralConnection(sourceConnection, false);

			var sheetsForSelectedPatients = new List<Sheet>();

			await Task.Run(() =>
			{
				sheetsForSelectedPatients.AddRange(GetSheetsForTransfer());
			});

			string failedConnections = "";
			if (sheetsForSelectedPatients.Count == 0)
			{
				failedConnections += sourceConnection.ToString() + "\r\n";
			}

			object locker = new object();

			bool InsertSheetsToConnection(CentralConnection connection, List<Sheet> sheets)
			{
				if (!CentralConnectionHelper.SetCentralConnection(connection, false))
				{
					connection.ConnectionStatus = "OFFLINE";

					return false;
				}

				foreach (Sheet sheet in sheets)
				{
					sheet.SheetNum = 0;
					sheet.PatNum = 0;
					sheet.IsNew = true;
				}

				Sheets.SaveNewSheetList(sheets);

				return true;
			}

			var tasks = new List<Task>();
			foreach (var connection in selectedConnections)
			{
				var sheets = new List<Sheet>(sheetsForSelectedPatients);

				tasks.Add(new Task(() =>
				{
					if (!InsertSheetsToConnection(connection, sheets))
					{
						lock (locker)
						{
							failedConnections += connection.ToString() + "\r\n";
						}
					}
				}));
			}

			await Task.WhenAll(tasks);

			if (failedConnections != "")
			{
				failedConnections = 
					"There were some transfers that failed due to connection issues. " +
					"Fix connections and try again.\r\nFailed Connections:\r\n" + failedConnections;

				using (var msgBoxCopyPaste = new CodeBase.MsgBoxCopyPaste(failedConnections))
				{
					msgBoxCopyPaste.ShowDialog(this);
				}
			}
			else
			{
				ShowInfo(
					"Transfers Completed Successfully\r\n" +
					"Go to each database you transferred patients to and retrieve Webforms to finish the transfer process.");
			}
		}
    }
}
