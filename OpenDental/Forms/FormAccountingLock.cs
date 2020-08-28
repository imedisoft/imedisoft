using Imedisoft.Data.Cache;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAccountingLock : FormBase
	{
		public FormAccountingLock()
		{
			InitializeComponent();
		}

		private void FormAccountingLock_Load(object sender, EventArgs e)
		{
			var accountingLockDate = Prefs.GetDateTimeOrNull(PrefName.AccountingLockDate);
			if (accountingLockDate.HasValue)
            {
				dateTextBox.Text = accountingLockDate.Value.ToShortDateString();
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			DateTime? accountingLockDate = null;
			if (!string.IsNullOrEmpty(dateTextBox.Text))
            {
				if (DateTime.TryParse(dateTextBox.Text, out var result))
                {
					accountingLockDate = result;
                }
                else
                {
					ShowError(Translation.Common.PleaseEnterValidDate);

					return;
                }
            }

			if (Prefs.Set(PrefName.AccountingLockDate, accountingLockDate))
			{
				CacheManager.Refresh(nameof(InvalidType.Prefs));
			}

			DialogResult = DialogResult.OK;
		}
	}
}
