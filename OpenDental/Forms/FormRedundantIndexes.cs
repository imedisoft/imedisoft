using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormRedundantIndexes:ODForm {

		public FormRedundantIndexes() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormRedundantIndexes_Load(object sender,EventArgs e) {
			FillGrid();
			if(gridMain.ListGridRows.Count==0) {
				MessageBox.Show("There are no redundant indexes in the current database.");
				DialogResult=DialogResult.OK;
			}
		}

		private void FillGrid() {
			DataTable table=DatabaseMaintenances.GetRedundantIndexesTable();
			if(table.Rows.Count==0) {
				return;
			}
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			gridMain.ListGridColumns.Add(new GridColumn(Lan.G(this,"Table"),165));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.G(this,"Index"),165));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.G(this,"Index Columns"),200));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.G(this,"Redundant Of"),615));
			gridMain.ListGridRows.Clear();
			gridMain.ListGridRows.AddRange(table.Select().Select(x =>
				new GridRow(new[] { "TABLE_NAME","INDEX_NAME","INDEX_COLS","REDUNDANT_OF" }.Select(y => PIn.String(x[y].ToString())).ToArray()) { Tag=x }));
			gridMain.EndUpdate();
		}

		private void checkLogAddStatements_CheckedChanged(object sender,EventArgs e) {
			if(!checkLogAddStatements.Checked) {
				string msgText="It is recommended that you log the statements to add the indexes back in case you wish to undo the actions performed by this "
					+"tool.  Without the log the indexes dropped by this tool cannot be recovered.\r\n"
					+"Do you want to re-enable logging of the statements?";
				checkLogAddStatements.Checked=MsgBox.Show(MsgBoxButtons.YesNo,msgText);
			}
		}

		private void butAll_Click(object sender,EventArgs e) {
			gridMain.SetSelected(true);
		}

		private void butNone_Click(object sender,EventArgs e) {
			gridMain.SetSelected(false);
		}

		private void butDrop_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MessageBox.Show("Please select the index(es) to drop first.");
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"This tool could take a long time to finish.  Do you wish to continue?")) {
				return;
			}
			MsgBoxCopyPaste msgBox;
			if(checkLogAddStatements.Checked) {
				string path="";
				try {
					path=GetLogFilePath();
					msgBox=new MsgBoxCopyPaste(Lan.G(this,"SQL statements to add the selected index(es) back will be in the following location")+":\r\n"+path);
					msgBox.ShowDialog();
				}
				catch(Exception ex) {
					MsgBox.Show(ex.Message+"\r\n"+(ex.InnerException?.Message??""));
				}
			}
			string logText="";
			ODProgress.ShowAction(() => logText=DatabaseMaintenances.DropRedundantIndexes(gridMain.SelectedTags<DataRow>()),
				actionException:ex => { MsgBox.Show(Lan.G(this,"There was an error dropping redundant indexes")+":\r\n"+ex.Message); },
				eventType:typeof(DatabaseMaintEvent),
				odEventType:EventCategory.DatabaseMaint);
			if(checkLogAddStatements.Checked) {
				try {
					SaveLogToFile(logText);
				}
				catch {
					msgBox=new MsgBoxCopyPaste(Lan.G(this,"Could not create or modify the log file. Copy and paste the following queries into a text file and "
						+"save it in case you ever want to undo the actions performed by this tool.")+"\r\n\r\n"+logText);
					msgBox.ShowDialog();
				}
			}
			MessageBox.Show("Done.");
			DialogResult=DialogResult.OK;
		}
		
		///<summary>Adds the logText to a centralized log file for the current day if the current data storage type is LocalAtoZ.
		///Throws exceptions to be displayed to the user.</summary>
		private void SaveLogToFile(string logText) {
			string machineName="~INVALID~";
			ODException.SwallowAnyException(() => { machineName=Environment.MachineName; });
			try {
				//will append to existing file or create new one for the date if it doesn't exist
				File.AppendAllText(GetLogFilePath(),DateTime.Now.ToString()+" - Computer Name: "+machineName+new string('-',45)+Environment.NewLine+logText);
			}
			catch(SecurityException se) {
				throw new ODException(Lan.G(this,"Log not saved to Drop Index Logs folder because user does not have permission to access that file."),se);
			}
			catch(UnauthorizedAccessException uae) {
				throw new ODException(Lan.G(this,"Log not saved to Drop Index Logs folder because user does not have permission to access that file."),uae);
			}
			//Throw all other types of exceptions like usual.
		}

		private string GetLogFilePath() {
			string path=ODFileUtils.CombinePaths(OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath(),"DropIndexLogs");
			try {
				if(!Directory.Exists(path)) {
					Directory.CreateDirectory(path);//Create DropIndexLogs folder if it doesn't exist
				}
			}
			catch(Exception ex) {
				throw new ODException(Lan.G(this,"Could not create or access the directory for saving the Drop Index Log file."),ex);
			}
			return ODFileUtils.CombinePaths(path,DateTime.Now.ToString("M_d_yyyy")+".txt");//One file per date
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}