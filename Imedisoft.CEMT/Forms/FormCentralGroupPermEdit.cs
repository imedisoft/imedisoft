using OpenDentBusiness;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralGroupPermEdit : FormBase
	{
		private readonly GroupPermission groupPermission;

		public FormCentralGroupPermEdit(GroupPermission groupPermission)
		{
			InitializeComponent();

			this.groupPermission = groupPermission.Copy();
		}

		private void FormCentralGroupPermEdit_Load(object sender, EventArgs e)
		{
			nameTextBox.Text = GroupPermissions.GetDesc(groupPermission.PermType);

			dateTextBox.Text = groupPermission.NewerDate.Year < 1880 ? "" : groupPermission.NewerDate.ToShortDateString();
			daysTextBox.Text = groupPermission.NewerDays == 0 ? "" : groupPermission.NewerDays.ToString();
		}

		private void DateTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			daysTextBox.Text = "";
		}

		private void DateTextBox_Validating(object sender, CancelEventArgs e)
		{
			var value = dateTextBox.Text.Trim();

			if (value.Length > 0)
			{
				if (!DateTime.TryParse(value, out var dateTime))
                {
					dateTextBox.Text = "";
				}
                else
                {
					dateTextBox.Text = dateTime.ToShortDateString();
				}
			}
		}

		private void DaysTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			dateTextBox.Text = "";
		}

		private void DaysTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
				e.Handled = true;
            }
		}

		private void DaysTextBox_Validating(object sender, CancelEventArgs e)
		{
			var value = daysTextBox.Text.Trim();

			if (value.Length > 0 && int.TryParse(value, out _))
			{
				return;
			}

			daysTextBox.Text = "";
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var newerDate = DateTime.MinValue;
			if (dateTextBox.Text.Length > 0)
            {
				if (!DateTime.TryParse(dateTextBox.Text, out newerDate))
                {
					ShowError("Please enter a valid date.");

					return;
				}
            }

			if (int.TryParse(daysTextBox.Text.Trim(), out var days) && days > GroupPermissions.NewerDaysMax)
            {
				ShowError($"Days must be less than {GroupPermissions.NewerDaysMax}.");

				return;
			}

			groupPermission.NewerDays = days;
			groupPermission.NewerDate = newerDate;

			try
			{
				if (groupPermission.IsNew)
				{
					GroupPermissions.Insert(groupPermission);
				}
				else
				{
					GroupPermissions.Update(groupPermission);
				}
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			DialogResult = DialogResult.OK;
		}
    }
}
