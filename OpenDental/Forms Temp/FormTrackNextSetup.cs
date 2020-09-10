using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Imedisoft.Data;
using OpenDentBusiness;

namespace OpenDental
{
	public partial class FormTrackNextSetup : ODForm
	{

		public FormTrackNextSetup()
		{
			InitializeComponent();

		}

		private void FormTrackNextSetup_Load(object sender, EventArgs e)
		{
			textDaysPast.Text = Preferences.GetLong(PreferenceName.PlannedApptDaysPast).ToString();
			textDaysFuture.Text = Preferences.GetLong(PreferenceName.PlannedApptDaysFuture).ToString();
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			if (!textDaysPast.IsValid || !textDaysFuture.IsValid)
			{
				MsgBox.Show("Please fix data entry errors first.");
				return;
			}
			int uschedDaysPastValue = PIn.Int(textDaysPast.Text, false);
			int uschedDaysFutureValue = PIn.Int(textDaysFuture.Text, false);
			if (Preferences.Set(PreferenceName.PlannedApptDaysPast, uschedDaysPastValue)
				| Preferences.Set(PreferenceName.PlannedApptDaysFuture, uschedDaysFutureValue))
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

	}
}