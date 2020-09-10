using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormWikiSetup : ODForm
	{
		public FormWikiSetup()
		{
			InitializeComponent();
		}

		private void FormWikiSetup_Load(object sender, EventArgs e)
		{
			textMaster.Text = WikiPages.MasterPage.PageContent;
			checkDetectLinks.Checked = Preferences.GetBool(PreferenceName.WikiDetectLinks);
			checkCreatePageFromLinks.Checked = Preferences.GetBool(PreferenceName.WikiCreatePageFromLink);
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			//Prefs
			if (Preferences.Set(PreferenceName.WikiDetectLinks, checkDetectLinks.Checked)
				| Preferences.Set(PreferenceName.WikiCreatePageFromLink, checkCreatePageFromLinks.Checked))
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			//Master Page
			WikiPage masterPage = WikiPages.MasterPage;
			masterPage.PageContent = textMaster.Text;
			masterPage.UserNum = Security.CurrentUser.Id;
			WikiPages.InsertAndArchive(masterPage);
			DataValid.SetInvalid(InvalidType.Wiki);
			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
