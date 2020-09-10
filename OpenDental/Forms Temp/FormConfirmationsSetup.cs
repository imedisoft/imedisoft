using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormConfirmationSetup:ODForm {
		private List<Definition> _listApptConfirmedDefs;

		public FormConfirmationSetup() {
			InitializeComponent();
			
		}


		public void FormConfirmationSetup_Load(object sender,System.EventArgs e) {
			FillTabManualConfirmation();
			Plugins.HookAddCode(this,"FormConfirmationSetup.Load_end");
		}

		//===============================================================================================
		#region Confirmations

		///<summary>Called on load to initially load confirmation with values from the database.  Calls FillGrid at the end.</summary>
		private void FillTabManualConfirmation() {
			_listApptConfirmedDefs=Definitions.GetByCategory(DefinitionCategory.ApptConfirmed);
			for(int i=0;i<_listApptConfirmedDefs.Count;i++) {
				comboStatusEmailedConfirm.Items.Add(_listApptConfirmedDefs[i].Name);
				if(_listApptConfirmedDefs[i].Id==Preferences.GetLong(PreferenceName.ConfirmStatusEmailed)) {
					comboStatusEmailedConfirm.SelectedIndex=i;
				}
			}
			for(int i=0;i<_listApptConfirmedDefs.Count;i++) {
				comboStatusTextMessagedConfirm.Items.Add(_listApptConfirmedDefs[i].Name);
				if(_listApptConfirmedDefs[i].Id==Preferences.GetLong(PreferenceName.ConfirmStatusTextMessaged)) {
					comboStatusTextMessagedConfirm.SelectedIndex=i;
				}
			}
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col;
			col=new GridColumn("Mode",61);
			gridMain.Columns.Add(col);
			col=new GridColumn("",300);
			gridMain.Columns.Add(col);
			col=new GridColumn("Message",500);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			#region Confirmation
			//Confirmation---------------------------------------------------------------------------------------------
			row=new GridRow();
			row.Cells.Add("Postcard");
			row.Cells.Add("Confirmation message.  Use [date]  and [time] where you want those values to be inserted");
			row.Cells.Add(Preferences.GetString(PreferenceName.ConfirmPostcardMessage));
			row.Tag=PreferenceName.ConfirmPostcardMessage;
			gridMain.Rows.Add(row);
			//
			row=new GridRow();
			row.Cells.Add("E-mail");
			row.Cells.Add("Confirmation subject line.");
			row.Cells.Add(Preferences.GetString(PreferenceName.ConfirmEmailSubject));
			row.Tag=PreferenceName.ConfirmEmailSubject;
			gridMain.Rows.Add(row);
			//
			row=new GridRow();
			row.Cells.Add("E-mail");
			row.Cells.Add("Confirmation message. Available variables: [NameF], [date], [time].");
			row.Cells.Add(Preferences.GetString(PreferenceName.ConfirmEmailMessage));
			row.Tag=PreferenceName.ConfirmEmailMessage;
			gridMain.Rows.Add(row);
			#endregion
			#region Text Messaging
			//Text Messaging----------------------------------------------------------------------------------------------
			row=new GridRow();
			row.Cells.Add("Text");
			row.Cells.Add("Confirmation message. Available variables: [NameF], [date], [time].");
			row.Cells.Add(Preferences.GetString(PreferenceName.ConfirmTextMessage));
			row.Tag=PreferenceName.ConfirmTextMessage;
			gridMain.Rows.Add(row);
			#endregion
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			string prefName=(string)gridMain.Rows[e.Row].Tag;
			FormRecallMessageEdit FormR=new FormRecallMessageEdit(prefName);
			FormR.MessageVal=Preferences.GetString(prefName);
			FormR.ShowDialog();
			if(FormR.DialogResult!=DialogResult.OK) {
				return;
			}
			Preferences.Set(prefName,FormR.MessageVal);
			//Prefs.RefreshCache();//above line handles it.
			FillGrid();
		}

		#endregion Confirmations
		//===============================================================================================


		private void butSetup_Click(object sender,EventArgs e) {
			//FormEServicesAutoMsging formESECR=new FormEServicesAutoMsging();
			//formESECR.ShowDialog();
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(comboStatusEmailedConfirm.SelectedIndex==-1) {
				Preferences.Set(PreferenceName.ConfirmStatusEmailed,0);
			}
			else {
				Preferences.Set(PreferenceName.ConfirmStatusEmailed,_listApptConfirmedDefs[comboStatusEmailedConfirm.SelectedIndex].Id);
			}
			if(comboStatusTextMessagedConfirm.SelectedIndex==-1) {
				Preferences.Set(PreferenceName.ConfirmStatusTextMessaged,0);
			}
			else {
				Preferences.Set(PreferenceName.ConfirmStatusTextMessaged,_listApptConfirmedDefs[comboStatusTextMessagedConfirm.SelectedIndex].Id);
			}
			//If we want to take the time to check every Update and see if something changed 
			//then we could move this to a FormClosing event later.
			DataValid.SetInvalid(InvalidType.Prefs);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}

}