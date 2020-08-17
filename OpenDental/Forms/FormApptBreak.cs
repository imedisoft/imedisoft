using CodeBase;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormApptBreak : FormBase
	{
		private readonly Appointment appointment;

		public ApptBreakSelection SelectedAction { get; private set; }

		public ProcedureCode SelectedProcedureCode { get; private set; }

		public FormApptBreak(Appointment appointment)
		{
			InitializeComponent();

			this.appointment = appointment;
		}

		private void FormApptBreak_Load(object sender, EventArgs e)
		{
			var brokenApptProcs = Prefs.GetEnum<BrokenApptProcedure>(PrefName.BrokenApptProcedure);

			missedRadioButton.Enabled = brokenApptProcs == BrokenApptProcedure.Missed || brokenApptProcs == BrokenApptProcedure.Both;
			cancelledRadioButton.Enabled = brokenApptProcs == BrokenApptProcedure.Cancelled || brokenApptProcs == BrokenApptProcedure.Both;

			if (missedRadioButton.Enabled && !cancelledRadioButton.Enabled)
			{
				missedRadioButton.Checked = true;
			}
			else if (!missedRadioButton.Enabled && cancelledRadioButton.Enabled)
			{
				missedRadioButton.Checked = true;
			}
		}

		private bool ValidateSelection()
		{
			if (!missedRadioButton.Checked && !cancelledRadioButton.Checked)
			{
				ShowError("Please select a broken procedure type.");

				return false;
			}

			return true;
		}

		private void PromptTextASAPList()
		{
			if (!Prefs.GetBool(PrefName.WebSchedAsapEnabled)) return;

			if (Appointments.RefreshASAP(0, 0, appointment.ClinicNum, new List<ApptStatus>()).Count == 0 || Prompt("Text patients on the ASAP List and offer them this opening?") == DialogResult.No)
			{
				return;
			}

			var dateTimeSlotStart = appointment.AptDateTime.Date; // Midnight
			var dateTimeSlotEnd = appointment.AptDateTime.Date.AddDays(1); // Midnight tomorrow

			// Loop through all other appts in the op to find a slot that will not overlap.
			var appointmentsInOperatory = Appointments.GetAppointmentsForOpsByPeriod(new List<long> { appointment.Op }, appointment.AptDateTime);
			foreach (var otherAppt in appointmentsInOperatory.Where(x => x.AptNum != appointment.AptNum))
			{
				var dateEndApt = otherAppt.AptDateTime.AddMinutes(otherAppt.Pattern.Length * 5);
				if (dateEndApt.Between(dateTimeSlotStart, appointment.AptDateTime))
				{
					dateTimeSlotStart = dateEndApt;
				}

				if (otherAppt.AptDateTime.Between(appointment.AptDateTime, dateTimeSlotEnd))
				{
					dateTimeSlotEnd = otherAppt.AptDateTime;
				}
			}

			dateTimeSlotStart = ODMathLib.Max(dateTimeSlotStart, appointment.AptDateTime.AddHours(-1));
			dateTimeSlotEnd = ODMathLib.Min(dateTimeSlotEnd, appointment.AptDateTime.AddHours(3));

			using var formASAP = new FormASAP(appointment.AptDateTime, dateTimeSlotStart, dateTimeSlotEnd, appointment.Op);

			formASAP.ShowDialog(this);
		}

		private void UnschedButton_Click(object sender, EventArgs e)
		{
			if (!ValidateSelection()) return;

			if (Prefs.GetBool(PrefName.UnscheduledListNoRecalls, true) && Appointments.IsRecallAppointment(appointment))
			{
				if (Prompt("Recall appointments cannot be sent to the Unscheduled List.\r\nDelete appointment instead?") == DialogResult.Yes)
				{
					SelectedAction = ApptBreakSelection.Delete; // Set to delete so the parent form can handle the delete.

					DialogResult = DialogResult.Cancel;
				}

				return;
			}

			PromptTextASAPList();

			SelectedAction = ApptBreakSelection.Unsched;
			DialogResult = DialogResult.OK;
		}

		private void PinboardButton_Click(object sender, EventArgs e)
		{
			if (!ValidateSelection()) return;

			PromptTextASAPList();

			SelectedAction = ApptBreakSelection.Pinboard;

			DialogResult = DialogResult.OK;
		}

		private void ApptBookButton_Click(object sender, EventArgs e)
		{
			if (!ValidateSelection()) return;

			SelectedAction = ApptBreakSelection.ApptBook;

			DialogResult = DialogResult.OK;
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			SelectedAction = ApptBreakSelection.None;

			DialogResult = DialogResult.Cancel;
		}

		private void FormApptBreak_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult != DialogResult.OK)
			{
				return;
			}

			SelectedProcedureCode = 
				missedRadioButton.Checked ? 
					ProcedureCodes.GetProcCode("D9986") : 
					ProcedureCodes.GetProcCode("D9987");
		}
	}

	public enum ApptBreakSelection
	{
		None,
		Unsched,
		Pinboard,
		ApptBook,
		Delete,
	}
}
