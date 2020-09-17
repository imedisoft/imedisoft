using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormApptTypeEdit : FormBase
	{
		private readonly AppointmentType appointmentType;
		private readonly StringBuilder timeStringBuilder = new StringBuilder();
		private List<ProcedureCode> procedureCodes;

		private bool mouseIsDown;
		private Point mouseOrigin;
		private Point sliderOrigin;

		public FormApptTypeEdit(AppointmentType appointmentType)
		{
			InitializeComponent();

			this.appointmentType = appointmentType;
		}

		private void FormApptTypeEdit_Load(object sender, EventArgs e)
		{
			nameTextBox.Text = appointmentType.Name;
			colorButton.BackColor = appointmentType.Color;
			hiddenCheckBox.Checked = appointmentType.Hidden;

			procedureCodes = ProcedureCodes.GetFromCommaDelimitedList(appointmentType.ProcedureCodes);

			if (appointmentType.Pattern != null)
			{
				timeStringBuilder.Append(Appointments.ConvertPatternFrom5(appointmentType.Pattern));
			}

			FillTime();

			RefreshListBoxProcCodes();
		}

		private void ColorButton_Click(object sender, EventArgs e)
		{
            using var colorDialog = new ColorDialog
            {
                Color = colorButton.BackColor
            };

            if (colorDialog.ShowDialog(this) == DialogResult.OK)
			{
				colorButton.BackColor = colorDialog.Color;
			}
		}

		private void ColorClearButton_Click(object sender, EventArgs e)
		{
			colorButton.BackColor = Color.FromArgb(0);
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (appointmentType.IsNew)
			{
				DialogResult = DialogResult.Cancel;
				return;
			}
			else
			{
				string msg = AppointmentTypes.CheckInUse(appointmentType.Id);
				if (!string.IsNullOrWhiteSpace(msg))
				{
					ShowError(msg);

					return;
				}

				AppointmentTypes.Delete(appointmentType.Id);

				appointmentType.Id = 0;

				DialogResult = DialogResult.OK;
			}
		}

		private void SliderButton_MouseUp(object sender, MouseEventArgs e)
		{
			mouseIsDown = false;
		}

		private void SliderButton_MouseDown(object sender, MouseEventArgs e)
		{
			mouseIsDown = true;
			mouseOrigin = new Point(e.X + sliderButton.Location.X, e.Y + sliderButton.Location.Y);
			sliderOrigin = sliderButton.Location;
		}

		private void SliderButton_MouseMove(object sender, MouseEventArgs e)
		{
			if (!mouseIsDown) return;

			// tempPoint represents the new location of button of smooth dragging.
			Point tempPoint = new Point(sliderOrigin.X, sliderOrigin.Y + e.Y + sliderButton.Location.Y - mouseOrigin.Y);

			int step = (int)(Math.Round((Decimal)(tempPoint.Y - timeTable.Location.Y) / 14));
			if (step == timeStringBuilder.Length) return;
			if (step < 1) return;
			if (step > timeTable.MaxRows - 1) return;
			if (step > timeStringBuilder.Length)
			{
				timeStringBuilder.Append('/');
			}

			if (step < timeStringBuilder.Length)
			{
				timeStringBuilder.Remove(step, 1);
			}

			FillTime();
		}

		private void FillTime()
		{
			for (int i = 0; i < timeStringBuilder.Length; i++)
			{
				timeTable.Cell[0, i] = timeStringBuilder.ToString(i, 1);
				timeTable.BackGColor[0, i] = Color.White;
			}

			for (int i = timeStringBuilder.Length; i < timeTable.MaxRows; i++)
			{
				timeTable.Cell[0, i] = "";
				timeTable.BackGColor[0, i] = Color.FromName("Control");
			}

			timeTable.Refresh();

			sliderButton.Location = new Point(timeTable.Location.X + 2, timeTable.Location.Y + timeStringBuilder.Length * 14 + 1);
			if (timeStringBuilder.Length > 0)
			{
				timeTextBox.Text = (timeStringBuilder.Length * PrefC.GetInt(PreferenceName.AppointmentTimeIncrement)).ToString();
			}
			else
			{
				timeTextBox.Text = "Use procedure time pattern";
			}
		}

		private void TimeTable_CellClicked(object sender, CellEventArgs e)
		{
			if (e.Row < timeStringBuilder.Length)
			{
				if (timeStringBuilder[e.Row] == '/')
				{
					timeStringBuilder.Replace('/', 'X', e.Row, 1);
				}
				else
				{
					timeStringBuilder.Replace(timeStringBuilder[e.Row], '/', e.Row, 1);
				}
			}

			FillTime();
		}

		private void RefreshListBoxProcCodes()
		{
			procedureCodesListBox.Items.Clear();

			foreach (var procedureCode in procedureCodes)
			{
				if (procedureCode.Id == 0)
				{
					continue;
				}

				procedureCodesListBox.Items.Add(procedureCode.Code);
			}
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
            using var formProcCodes = new FormProcCodes
            {
                IsSelectionMode = true,
                AllowMultipleSelections = true
            };

            if (formProcCodes.ShowDialog(this) == DialogResult.OK)
			{
				procedureCodes.AddRange(formProcCodes.ListSelectedProcCodes.Select(x => x.Copy()).ToList());
			}

			RefreshListBoxProcCodes();
		}

		private void ClearButton_Click(object sender, EventArgs e)
		{
			timeStringBuilder.Clear();

			FillTime();
		}

		private void RemoveButton_Click(object sender, EventArgs e)
		{
			if (procedureCodesListBox.SelectedItems.Count < 1)
			{
				ShowError("Please select the procedures you wish to remove.");

				return;
			}

			if (Prompt("Remove selected procedure(s)?") == DialogResult.Yes)
			{
				procedureCodes.RemoveAll(x => procedureCodesListBox.SelectedItems.Contains(x.Code));

				RefreshListBoxProcCodes();
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			appointmentType.Name = nameTextBox.Text;
			appointmentType.Color = colorButton.BackColor;
			appointmentType.Hidden = hiddenCheckBox.Checked;
			appointmentType.ProcedureCodes = String.Join(",", procedureCodes.Select(x => x.Code));

			if (timeStringBuilder.Length > 0)
			{
				appointmentType.Pattern = Appointments.ConvertPatternTo5(timeStringBuilder.ToString());
			}
			else
			{
				appointmentType.Pattern = "";
			}

			DialogResult = DialogResult.OK;
		}
	}
}
