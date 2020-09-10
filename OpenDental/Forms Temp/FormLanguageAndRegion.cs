using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Imedisoft.Data;
using OpenDentBusiness;

namespace OpenDental
{
	public partial class FormLanguageAndRegion : ODForm
	{
		private List<CultureInfo> _listAllCultures;

		public FormLanguageAndRegion()
		{
			InitializeComponent();

		}

		private void FormLanguageAndRegion_Load(object sender, EventArgs e)
		{
			CultureInfo cultureCur = PrefC.GetLanguageAndRegion();
			_listAllCultures = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(x => !x.IsNeutralCulture).OrderBy(x => x.DisplayName).ToList();
			textLARLocal.Text = CultureInfo.CurrentCulture.DisplayName;
			if (Preferences.GetString(PreferenceName.LanguageAndRegion) == "")
			{
				textLARDB.Text = "None";
			}
			else
			{
				textLARDB.Text = cultureCur.DisplayName;
			}
			comboLanguageAndRegion.Items.Clear();
			_listAllCultures.ForEach(x => comboLanguageAndRegion.Items.Add(x.DisplayName));
			comboLanguageAndRegion.SelectedIndex = _listAllCultures.FindIndex(x => x.DisplayName == cultureCur.DisplayName);
			checkNoShow.Checked = ComputerPrefs.LocalComputer.NoShowLanguage;
			if (!Security.IsAuthorized(Permissions.Setup, true))
			{
				comboLanguageAndRegion.Visible = false;
				labelNewLAR.Visible = false;
				butOK.Enabled = false;
				butCancel.Text = "&Close";
				checkNoShow.Enabled = false;
			}
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			if (comboLanguageAndRegion.SelectedIndex == -1)
			{
				MessageBox.Show("Select a language and region.");
				return;
			}
			if (Security.IsAuthorized(Permissions.Setup, true))
			{
				//_cultureCur=_listAllCultures[comboLanguageAndRegion.SelectedIndex];
				if (Preferences.Set(PreferenceName.LanguageAndRegion, _listAllCultures[comboLanguageAndRegion.SelectedIndex].Name))
				{
					MessageBox.Show("Program must be restarted for changes to take full effect.");
				}
				ComputerPrefs.LocalComputer.NoShowLanguage = checkNoShow.Checked;
				ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			}
			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
