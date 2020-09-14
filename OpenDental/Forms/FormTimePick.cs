using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormTimePick : FormBase
	{
		private readonly bool enableDatePicker;
		private readonly DateTime date;

		public DateTime SelectedDTime { get; private set; }

		public FormTimePick(bool enableDatePicker, DateTime? value)
		{
			InitializeComponent();

			this.enableDatePicker = enableDatePicker;
			date = value ?? DateTime.Now;
		}

		private void FormTimePick_Load(object sender, EventArgs e)
		{
			if (!enableDatePicker)
            {
				dateGroupBox.Visible = false;
				timeGroupBox.Top = dateGroupBox.Top;

				Height -= dateGroupBox.Height;
            }

			if (date.Hour >= 12) pmRadioButton.Checked = true;
			else
			{
				amRadioButton.Checked = true;
			}

			if (date.Hour > 12) hourComboBox.SelectedText = (date.Hour - 12).ToString();
			else
			{
				hourComboBox.SelectedText = date.Hour > 0 ? date.Hour.ToString() : "12";
			}

			minuteComboBox.SelectedText = date.Minute.ToString();
			dateTimePicker.Value = date.Date;
		}

		private void TimeComboBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!int.TryParse(hourComboBox.Text, out var hour) ||
				!int.TryParse(minuteComboBox.Text, out var minute))
			{
				ShowError("Please enter a valid time.");

				return;
			}

			if (pmRadioButton.Checked && hour < 12)
			{
				hour += 12;
			}

			if (amRadioButton.Checked && hour == 12) hour = 0;

			if (hour < 0 || hour >= 23 || minute < 0 || minute > 59)
			{
				ShowError("Please enter a valid time.");

				return;
			}

			SelectedDTime = new DateTime(
				dateTimePicker.Value.Year, dateTimePicker.Value.Month, dateTimePicker.Value.Day,
				hour, minute, 0);

			DialogResult = DialogResult.OK;
		}
    }
}
