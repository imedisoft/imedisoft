using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormWikiSetup:ODForm {

		public FormWikiSetup() {
			InitializeComponent();
			
		}

		private void FormWikiSetup_Load(object sender,EventArgs e) {
			textMaster.Text=WikiPages.MasterPage.PageContent;
			checkDetectLinks.Checked=Prefs.GetBool(PrefName.WikiDetectLinks);
			checkCreatePageFromLinks.Checked=Prefs.GetBool(PrefName.WikiCreatePageFromLink);
		}

		private void butOK_Click(object sender,EventArgs e) {
			//Prefs
			if(Prefs.Set(PrefName.WikiDetectLinks,checkDetectLinks.Checked)
				| Prefs.Set(PrefName.WikiCreatePageFromLink,checkCreatePageFromLinks.Checked)) 
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			//Master Page
			WikiPage masterPage=WikiPages.MasterPage;
			masterPage.PageContent=textMaster.Text;
			masterPage.UserNum=Security.CurrentUser.Id;
			WikiPages.InsertAndArchive(masterPage);
			DataValid.SetInvalid(InvalidType.Wiki);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}