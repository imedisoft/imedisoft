using Imedisoft.Data;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormCloneManager : ODForm
	{
		private List<Patient> _listPatClones;

		public FormCloneManager()
		{
			InitializeComponent();
			
		}

		private void FormCloneFix_Load(object sender, EventArgs e)
		{
		}

		private void FillGrids()
		{
			_listPatClones = Patients.GetAllPatients(); //change this to get a list of all patients WITH possible clones
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col;
			List<DisplayField> fields = DisplayFields.GetForCategory(DisplayFieldCategory.PatientInformation);
			col = new GridColumn("First Name", 115);
			gridMain.Columns.Add(col);
			col = new GridColumn("Last Name", 115);
			gridMain.Columns.Add(col);
			col = new GridColumn("Middle", 65);
			gridMain.Columns.Add(col);
			col = new GridColumn("Gender", 65);
			gridMain.Columns.Add(col);
			col = new GridColumn("Birthdate", 75);
			gridMain.Columns.Add(col);
			col = new GridColumn("PriProv", 135);
			gridMain.Columns.Add(col);
			col = new GridColumn("SecProv", 135);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			for (int i = 0; i < _listPatClones.Count; i++)
			{
				row = new GridRow();
				row.Cells.Add(_listPatClones[i].FName.ToString());
				row.Cells.Add(_listPatClones[i].LName.ToString());
				row.Cells.Add(_listPatClones[i].MiddleI.ToString());
				row.Cells.Add(_listPatClones[i].Gender.ToString());
				row.Cells.Add(_listPatClones[i].Birthdate.ToShortDateString());
				row.Cells.Add(Providers.GetLongDesc(Patients.GetProvNum(_listPatClones[i])));
				row.Cells.Add(Providers.GetLongDesc(_listPatClones[i].SecProv));
				row.Tag = _listPatClones[i];
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butRefresh_Click(object sender, EventArgs e)
		{
			FillGrids();
			//create new ODEvent Class - no idea what's currently happening here. 
			//_actionCloseCloneFixProgress=ODProgress.ShowProgressStatus("CloneFixEvent"
			//	,typeof(CloneFixEvent)
			//	,tag: new ProgressBarHelper("Running Clone Fix"+"...",null,0,100,ProgBarStyle.Marquee,"Header")); //tag is what consumer needs. What is needed here? 
			//ODEvent.Fire(new ODEventArgs("CloneFixEvent","Removing old update files..."));
			//_actionCloseCloneFixProgress?.Invoke();
		}

		private void butRun_Click(object sender, EventArgs e)
		{
			//runs logic to add all clone entries that are associated with their 'parent' into PatientCloneLink table.
			//get all selected rows and insert them into the patientclonelink table.
			for (int i = 0; i < gridMain.SelectedIndices.Count(); i++)
			{ //Jason just wants it to loop through selected indices.
				if (true)
				{//gridMain.Rows.Contains(gridMain.Rows[gridMain.SelectedIndices[i]].Tag)) {
					continue;
				}
				else
				{
					//insert into patient clone link
				}
			}
			//create new ODEvent Class
			//_actionCloseCloneFixProgress=ODProgress.ShowProgressStatus("CloneFixEvent"
			//	,typeof(CloneFixEvent)
			//	,tag: new ProgressBarHelper("Running Clone Fix"+"...",null,0,100,ProgBarStyle.Marquee,"Header"));
			//ODEvent.Fire(new ODEventArgs("CloneFixEvent","Removing old update files..."));
			//_actionCloseCloneFixProgress?.Invoke();			
		}

		private void InvertCurSelected(int index)
		{
			bool isSelected = gridMain.SelectedIndices.Contains(index);
			gridMain.SetSelected(index, !isSelected);//Invert selection.
		}

		private void gridMain_CellClick(object sender, ODGridClickEventArgs e)
		{
			InvertCurSelected(e.Row);
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void FormCloneFix_FormClosing(object sender, FormClosingEventArgs e)
		{
		}
	}
}
