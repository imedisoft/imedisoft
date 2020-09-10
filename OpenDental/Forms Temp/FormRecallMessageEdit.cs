using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormRecallMessageEdit:ODForm {
		public string MessageVal;
		private string _prefName;

		public FormRecallMessageEdit(string prefName) {
			InitializeComponent();
			
			_prefName=prefName;
		}

		private void FormRecallMessageEdit_Load(object sender,EventArgs e) {
			textMain.Text=MessageVal;
		}

		private void butOK_Click(object sender,EventArgs e) {
			//We need to limit email subjects to 200 characters otherwise errors can happen in other places of the software and it's hard to track.
			//E.g. sending emails from the Recall List window and all recalls of type email will simply skip with no explanation.
			if(_prefName==PreferenceName.BillingEmailSubject
				|| _prefName==PreferenceName.ConfirmEmailSubject
				|| _prefName==PreferenceName.RecallEmailSubject
				|| _prefName==PreferenceName.RecallEmailSubject2
				|| _prefName==PreferenceName.RecallEmailSubject3
				|| _prefName==PreferenceName.ReactivationEmailSubject
				|| _prefName==PreferenceName.WebSchedSubject) 
			{
				if(textMain.Text.Length>200) {
					MessageBox.Show("Email subjects cannot be longer than 200 characters.");
					return;
				}
			}
			string urlWarning="Web Sched message does not contain the \"[URL]\" variable. Omitting the \"[URL]\" variable will prevent the "+
				"patient from visiting the WebSched portal. Are you sure you want to continue?";
			if(_prefName==PreferenceName.WebSchedMessage
				|| _prefName==PreferenceName.WebSchedMessage2
				|| _prefName==PreferenceName.WebSchedMessage3) 
			{
				if(!textMain.Text.Contains("[URL]")
					&& !MsgBox.Show(MsgBoxButtons.OKCancel,urlWarning)) 
				{
					return;
				}
			}
			if(_prefName==PreferenceName.WebSchedMessageText
				|| _prefName==PreferenceName.WebSchedMessageText2
				|| _prefName==PreferenceName.WebSchedMessageText3)
			{
				if(textMain.Text.Contains("[URL]")) {
					textMain.Text=textMain.Text.Replace("[URL].","[URL] .");//Clicking a link with a period will not get recognized. 
				}
				else if(!MsgBox.Show(MsgBoxButtons.OKCancel,urlWarning)) {
					return;
				}
			}
			MessageVal=textMain.Text;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		
	}
}