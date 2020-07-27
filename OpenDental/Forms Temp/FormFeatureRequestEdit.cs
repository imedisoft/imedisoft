using OpenDentBusiness.Services.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormRequestEdit : FormBase
	{
		private readonly FeatureRequestDto featureRequest;
		private bool allowModification = false;

		public FormRequestEdit(FeatureRequestDto featureRequest)
		{
			InitializeComponent();

			this.featureRequest = featureRequest;
		}

		private void FormRequestEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = featureRequest.Description;
			detailsTextBox.Text = "";
			submitterTextBox.Text = featureRequest.Submitter;
			difficultyTextBox.Text = featureRequest.Difficulty.ToString();
			priorityTextBox.Text = featureRequest.Priority.ToString();
			statusTextBox.Text = FeatureRequestDto.GetStatusString(featureRequest.Status);
			pledgeTextBox.Text = FeatureRequestDto.FormatAmount(featureRequest.Pledge);
			criticalCheckBox.Checked = featureRequest.IsCritical;
			bountyTextBox.Text = FeatureRequestDto.FormatAmount(featureRequest.Bounty);

			if (featureRequest.Id == 0)
			{
				idTextBox.Text = "(new)";

				pledgeGroupBox.Visible = false;
			}
			else
			{
				idTextBox.Text = featureRequest.Id.ToString();

				allowModification = 
					featureRequest.Status == FeatureRequestStatus.NeedsClarification && 
					featureRequest.Status == FeatureRequestStatus.New;

				if (!featureRequest.IsMine || !allowModification)
                {
					descriptionTextBox.ReadOnly = detailsTextBox.ReadOnly = submitterTextBox.ReadOnly = true;
                }

				if (featureRequest.IsMine)
                {
					deleteButton.Visible = true;
                }

				FetchFeatureRequestNotesFromServer();
			}
		}

		private void CriticalCheckBox_Click(object sender, EventArgs e)
		{
			if (criticalCheckBox.Checked)
			{
				if (Prompt(
					"Are you sure this is really critical? " +
					"To qualify as critical, there would be no possible workarounds. " +
					"The missing feature would probably be seriously impacting the financial status of the office. " +
					"It would be serious enough that you might be considering using another software.") == DialogResult.No)
				{
					criticalCheckBox.Checked = false;

					return;
				}
			}
		}

		private void SaveNoteButton_Click(object sender, EventArgs e)
		{
			var note = noteTextBox.Text.Trim();

			if (note.Length == 0)
			{
				ShowInfo("Please enter some text first.");

				return;
			}

			if (!AddNoteToFeatureRequest())
			{
				return;
			}

			noteTextBox.Text = "";

			FetchFeatureRequestNotesFromServer();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (Prompt("Delete this entire request?") == DialogResult.No)
			{
				return;
			}

			if (!DeleteFeatureRequestFromServer())
			{
				return;
			}

			DialogResult = DialogResult.OK;

			Close();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var note = noteTextBox.Text.Trim();

			if (note.Length == 0)
			{
				ShowInfo("You need to save your note first.");

				return;
			}

			if (allowModification)
			{
				if (Prompt("Only continue if you have added notes to the original request to comply better with submission guidelines.") == DialogResult.No)
				{
					return;
				}
			}

			if (!SaveFeatureRequestToServer())
			{
				return;
			}

			DialogResult = DialogResult.OK;
			Close();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;

			Close();
		}

		private void FetchFeatureRequestNotesFromServer()
			=> throw new NotImplementedException();

		private bool AddNoteToFeatureRequest()
			=> throw new NotImplementedException();

		private bool SaveFeatureRequestToServer()
			=> throw new NotImplementedException();

		private bool DeleteFeatureRequestFromServer()
			=> throw new NotImplementedException();
	}
}
