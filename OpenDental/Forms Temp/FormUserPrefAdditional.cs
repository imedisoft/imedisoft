using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using Imedisoft.Data.Models;

namespace OpenDental {
	public partial class FormUserPrefAdditional:ODForm {
		/// <summary>This is a list of providerclinic rows that were given to this form, containing any modifications that were made while in FormProvAdditional.</summary>
		//public List<UserOdPref> ListUserPrefOut=new List<UserOdPref>();
		private User _userCur;
		//private List<UserOdPref> _listUserPref;

		public FormUserPrefAdditional(/*List<UserOdPref> listUserPref,*/User userCur) {
			InitializeComponent();
			//
			//_listUserPref=listUserPref.Select(x => x.Clone()).ToList();
			_userCur=userCur.Copy();
		}

		private void FormProvAdditional_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			Cursor=Cursors.WaitCursor;
			gridUserProperties.BeginUpdate();
			gridUserProperties.Columns.Clear();
			GridColumn col=new GridColumn("Clinic",120);
			gridUserProperties.Columns.Add(col);
			col=new GridColumn("DoseSpot User ID",120,true);
			gridUserProperties.Columns.Add(col);
			gridUserProperties.Rows.Clear();
			GridRow row;
			//UserOdPref userPrefDefault=_listUserPref.Find(x => x.ClinicNum==0);
			//Doesn't exist in Db, create one

			//if(userPrefDefault==null) {
			//	userPrefDefault=UserOdPrefs.GetByCompositeKey(_userCur.Id,Programs.GetCur(ProgramName.eRx).Id,UserOdFkeyType.Program,0);
			//	//Doesn't exist in db, add to list to be synced later
			//	_listUserPref.Add(userPrefDefault);
			//}

			//row=new GridRow();
			//row.Cells.Add("Default");
			//row.Cells.Add(userPrefDefault.ValueString);
			//row.Tag=userPrefDefault;
			//gridUserProperties.ListGridRows.Add(row);
			//foreach(Clinic clinicCur in Clinics.GetForUserod(Security.CurrentUser)) {
				//row=new GridRow();
				//UserOdPref userPrefCur=_listUserPref.Find(x => x.ClinicNum==clinicCur.ClinicNum);

				//wasn't in list, check Db and create a new one if needed
				//if(userPrefCur==null) {
				//	userPrefCur=UserOdPrefs.GetByCompositeKey(_userCur.Id,Programs.GetCur(ProgramName.eRx).Id,UserOdFkeyType.Program,clinicCur.ClinicNum);
				//	//Doesn't exist in db, add to list to be synced later
				//	_listUserPref.Add(userPrefCur);
				//}

				//row.Cells.Add(clinicCur.Abbr);		
				//row.Cells.Add(userPrefCur.ValueString);
				//row.Tag=userPrefCur;
				//gridUserProperties.ListGridRows.Add(row);
			//}
			//gridUserProperties.EndUpdate();
			Cursor=Cursors.Default;
		}

		private void gridProvProperties_CellLeave(object sender,ODGridClickEventArgs e) {
			//string newDoseSpotID=PIn.String(gridUserProperties.ListGridRows[e.Row].Cells[e.Col].Text);
			//UserOdPref userPref=(UserOdPref)gridUserProperties.ListGridRows[e.Row].Tag;
			//userPref.ValueString=newDoseSpotID;
		}

		private void butOK_Click(object sender,EventArgs e) {
			//ListUserPrefOut=new List<UserOdPref>();
			//foreach(GridRow row in gridUserProperties.ListGridRows) {
			//	ListUserPrefOut.Add((UserOdPref)row.Tag);
			//}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}