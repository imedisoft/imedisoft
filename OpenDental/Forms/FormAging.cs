using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Cache;
using OpenDentBusiness;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAging : FormBase
	{
		public FormAging()
		{
			InitializeComponent();
		}

		private void FormAging_Load(object sender, EventArgs e)
		{
			var lastDate = Prefs.GetDateTime(PrefName.DateLastAging);

			lastDateTextBox.Text = lastDate.ToShortDateString();

            if (Prefs.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily))
            {
                if (lastDate < DateTime.Today.AddDays(-15))
                {
                    dateTextBox.Text = lastDate.AddMonths(1).ToShortDateString();
                }
                else
                {
                    dateTextBox.Text = lastDate.ToShortDateString();
                }
            }
            else
            {
                dateTextBox.Text = DateTime.Today.ToShortDateString();
                if (Prefs.GetBool(PrefName.AgingIsEnterprise))
                {
                    dateTextBox.ReadOnly = true;
                    dateTextBox.BackColor = SystemColors.Control;
                }
            }
        }

		private bool RunAgingEnterprise(DateTime date)
		{
			DateTime dateLastAging = PrefC.GetDate(PrefName.DateLastAging);
			if (dateLastAging.Date == date.Date)
			{
				if (!Confirm( 
					"Aging has already been calculated for " + date.ToShortDateString() + " and does not normally need to run more than once per day." +
					"\r\n\r\n" +
					"Run anyway?"))
				{
					return false;
				}
			}

			Prefs.RefreshCache();

			var agingBeginDateTime = Prefs.GetDateTimeOrNull(PrefName.AgingBeginDateTime);
			if (agingBeginDateTime.HasValue)
			{
				ShowError(
					"You cannot run aging until it has finished the current calculations which began on " + agingBeginDateTime.ToString() + ".\r\n" +
					"If you believe the current aging process has finished, a user with SecurityAdmin permission can manually clear the date and time by going to Setup | Miscellaneous and pressing the 'Clear' button.");
				
				return false;
			}

			SecurityLogs.Write(Permissions.AgingRan, Translation.SecurityLog.AgingStart);

			Prefs.Set(PrefName.AgingBeginDateTime, DateTime.UtcNow);

			Cursor = Cursors.WaitCursor;

			bool result = true;
			ODProgress.ShowAction(
				() =>
				{
					Ledgers.ComputeAging(0, date);

					Prefs.Set(PrefName.DateLastAging, date);
				},
				startingMessage: "Calculating enterprise aging for all patients as of " + date.ToShortDateString() + "...",
				actionException: ex =>
				{
					Ledgers.AgingExceptionHandler(ex, this);

					result = false;
				}
			);

			Cursor = Cursors.Default;

			SecurityLogs.Write(Permissions.AgingRan, Translation.SecurityLog.AgingComplete);

			Prefs.Set(PrefName.AgingBeginDateTime, "");

			return result;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!DateTime.TryParse(dateTextBox.Text, out var date))
            {
				ShowError(Translation.Common.PleaseEnterValidDate);

				return;
            }

			if (Prefs.GetBool(PrefName.AgingIsEnterprise))
			{
				if (!RunAgingEnterprise(date))
				{
					return;
				}
			}
			else
			{
				SecurityLogs.Write(Permissions.AgingRan, Translation.SecurityLog.AgingStart);

				Cursor = Cursors.WaitCursor;

				bool result = true;
				ODProgress.ShowAction(() => Ledgers.ComputeAging(0, date),
					startingMessage: "Calculating aging for all patients as of " + date.ToShortDateString() + "...",
					actionException: ex =>
					{
						Ledgers.AgingExceptionHandler(ex, this);

						result = false;
					}
				);

				Cursor = Cursors.Default;

				SecurityLogs.Write(Permissions.AgingRan, Translation.SecurityLog.AgingComplete);
				if (!result)
				{
					DialogResult = DialogResult.Cancel;

					return;
				}

				if (Prefs.Set(PrefName.DateLastAging, date))
				{
					CacheManager.RefreshGlobal(nameof(InvalidType.Prefs));
				}
			}

			MessageBox.Show(Translation.Common.OperationComplete);

			DialogResult = DialogResult.OK;
		}
	}
}
