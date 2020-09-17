using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormStudentResultEdit : FormBase
	{
		private readonly StudentResult studentResult;
		private long? patientId;
		private long? apptId;

		public FormStudentResultEdit(StudentResult studentResult)
		{
			InitializeComponent();

			this.studentResult = studentResult;
		}

		private void EnableControlsForStudent()
        {
			patientDetachButton.Enabled = true;
			patientSelectButton.Enabled = true;
			appointmentDetachButton.Enabled = true;
			acceptButton.Enabled = true;
		}

		private void EnableControlsForInstructor()
        {
			EnableControlsForStudent();

			completionDateTextBox.ReadOnly = false;
			completionDateButton.Enabled = true;
		}

		private void FormStudentResultEdit_Load(object sender, EventArgs e)
		{
			studentTextBox.Text = Providers.GetLongDesc(studentResult.ProviderId);
			courseTextBox.Text = SchoolCourses.GetDescription(studentResult.SchoolCourseId);
			requirementTextBox.Text = studentResult.Description;

			if (studentResult.CompletionDate.HasValue)
			{
				completionDateTextBox.Text = studentResult.CompletionDate.Value.ToShortDateString();
			}


			patientId = studentResult.PatientId;
			if (patientId.HasValue)
			{
				patientTextBox.Text = Patients.GetPat(patientId.Value)?.GetNameFL();
			}

			apptId = studentResult.ApptId;
			if (apptId.HasValue)
            {
				var appt = Appointments.GetOneApt(apptId.Value);
				if (appt != null)
                {
					if (appt.AptStatus == ApptStatus.UnschedList)
					{
						appointmentTextBox.Text = "Unscheduled";
					}
					else
					{
						appointmentTextBox.Text = appt.AptDateTime.ToShortDateString() + " " + appt.AptDateTime.ToShortTimeString();
					}
					appointmentTextBox.Text += ", " + appt.ProcDescript;
				}
            }

			instructorComboBox.Items.Add(Translation.Common.None);
			instructorComboBox.SelectedIndex = 0;

			foreach (var provider in Providers.GetAll(true))
            {
				instructorComboBox.Items.Add(provider);
				if (provider.Id == studentResult.InstructorId)
                {
					instructorComboBox.SelectedItem = provider;
                }
            }

			var userProvider = Providers.GetById(Security.CurrentUser.ProviderId);
			if (userProvider != null)
			{
				if (!userProvider.IsInstructor)
				{
					if (userProvider.Id != studentResult.ProviderId)
					{
						ShowInfo(Translation.DentalSchools.StudentsMayOnlyEditTheirOwnResults);
					}
					else
					{
						EnableControlsForStudent();
					}
				}
				else
				{
					EnableControlsForInstructor();
				}
			}
		}

		private void CompletionDateButton_Click(object sender, EventArgs e)
		{
			completionDateTextBox.Text = DateTime.UtcNow.ToShortDateString();
		}

		private void PatientDetachButton_Click(object sender, EventArgs e)
		{
			patientId = null;
			patientTextBox.Text = "";
		}

		private void PatientSelectButton_Click(object sender, EventArgs e)
		{
            using var formPatientSelect = new FormPatientSelect
            {
                SelectionModeOnly = true
            };

			if (formPatientSelect.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			patientId = formPatientSelect.SelectedPatientId;
			patientTextBox.Text = patientId.HasValue ? Patients.GetPat(patientId.Value).GetNameFL() : "";

			AppointmentDetachButton_Click(this, EventArgs.Empty);
		}

		private void AppointmentDetachButton_Click(object sender, EventArgs e)
		{
			apptId = null;
			appointmentTextBox.Text = "";
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (!Confirm(Translation.Common.ConfirmDelete))
            {
				return;
            }

			try
			{
				StudentResults.Delete(studentResult.Id);

				DialogResult = DialogResult.OK;
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			DateTime? completionDate = null;
			if (completionDateTextBox.Text != "")
            {
				if (!DateTime.TryParse(completionDateTextBox.Text, out var date))
                {
					ShowError(Translation.Common.PleaseEnterValidDate);

					return;
                }

				completionDate = date;
            }

			long? instructorId = null;
			if (instructorComboBox.SelectedItem is Provider instructor)
            {
				instructorId = instructor.Id;
			}

			studentResult.InstructorId = instructorId;
			studentResult.CompletionDate = completionDate;
			studentResult.ApptId = apptId;
			studentResult.PatientId = patientId;

			StudentResults.Save(studentResult);

			DialogResult = DialogResult.OK;
		}
	}
}
