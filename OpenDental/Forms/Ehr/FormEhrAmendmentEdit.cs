using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrAmendmentEdit : FormBase
	{
		private readonly EhrAmendment ehrAmendment;

		public FormEhrAmendmentEdit(EhrAmendment ehrAmendment)
		{
			InitializeComponent();

			this.ehrAmendment = ehrAmendment;
		}

		private void FormEhrAmendmentEdit_Load(object sender, EventArgs e)
		{
			foreach (var source in EhrAmendments.GetSources())
            {
				sourceComboBox.Items.Add(source);
				if (source.Value == ehrAmendment.Source)
                {
					sourceComboBox.SelectedItem = source;
                }
            }

			if (ehrAmendment.Id == 0)
			{
				ehrAmendment.RequestedOn = DateTime.Now;
			}

			if (ehrAmendment.AppendedOn.HasValue)
			{
				scanLabel.Visible = true;
				scanButton.Text = Translation.Common.View;
			}

			if (ehrAmendment.IsAccepted == true)
			{
				statusAcceptedRadioButton.Checked = true;
			}
			else if (ehrAmendment.IsAccepted == false)
			{
				statusDeniedRadioButton.Checked = false;
			}

			sourceComboBox.SelectedIndex = (int)ehrAmendment.Source;
			sourceNameTextBox.Text = ehrAmendment.SourceName;
			descriptionTextBox.Text = ehrAmendment.Description;

			if (ehrAmendment.RequestedOn.HasValue)
			{
				dateRequestedTextBox.Text = ehrAmendment.RequestedOn.ToString();
			}

			if (ehrAmendment.AcceptedDeniedOn.HasValue)
			{
				dateAcceptedTextBox.Text = ehrAmendment.AcceptedDeniedOn.ToString();
			}

			if (ehrAmendment.AppendedOn.HasValue)
			{
				dateAppendedTextBox.Text = ehrAmendment.AppendedOn.ToString();
			}
		}

		private void DateRequestedButton_Click(object sender, EventArgs e)
		{
			dateRequestedTextBox.Text = DateTime.Now.ToString();
		}

		private void DateAcceptedButton_Click(object sender, EventArgs e)
		{
			dateAcceptedTextBox.Text = DateTime.Now.ToString();
		}

		private void DateAppendedButton_Click(object sender, EventArgs e)
		{
			dateAppendedTextBox.Text = DateTime.Now.ToString();
		}

		private void ScanButton_Click(object sender, EventArgs e)
		{
            using var formImages = new FormImages
            {
                EhrAmendmentCur = ehrAmendment
            };

            if (formImages.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			// TODO: This is currently broken because of the awkward way in whcih FormImages works...

			if (string.IsNullOrEmpty(ehrAmendment.FileName))
			{
				scanLabel.Visible = false;
				scanButton.Text = Translation.Common.Scan;
			}
			else
			{
				scanLabel.Visible = true;
				scanButton.Text = Translation.Common.View;

				dateAppendedTextBox.Text = DateTime.Now.ToString();
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			static bool TryParseDate(TextBox textBox, out DateTime? result)
            {
				result = null;

				if (string.IsNullOrEmpty(textBox.Text))
                {
					if (!DateTime.TryParse(textBox.Text, out var date))
                    {
						return false;
                    }

					result = date;
                }

				return true;
            }

			if (!TryParseDate(dateRequestedTextBox, out var dateRequested) || 
				!TryParseDate(dateAcceptedTextBox, out var dateAccepted) || 
				!TryParseDate(dateAppendedTextBox, out var dateAppended))
            {
				ShowError(Translation.Common.PleaseEnterValidDate);

				return;
			}

            if (!(sourceComboBox.SelectedItem is DataItem<EhrAmendmentSource> source))
            {
                ShowError(Translation.Ehr.PleaseSelectAmendmentSource);

                return;
            }

            var sourceName = sourceNameTextBox.Text.Trim();
			if (sourceName.Length == 0)
            {
				ShowError(Translation.Ehr.PleaseEnterAmendmentSourceName);

				return;
			}

			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterDescription);

				return;
			}

			bool? status = null;
			if (statusAcceptedRadioButton.Checked) status = true;
			else if (statusDeniedRadioButton.Checked) status = false;

			if (ehrAmendment.Id == 0 && !dateRequested.HasValue)
			{
				dateRequested = DateTime.Now;
			}

			if (status.HasValue && !dateAccepted.HasValue)
			{
				if (ehrAmendment.Id == 0 || ehrAmendment.IsAccepted != status)
				{
					dateAccepted = DateTime.Now;
				}
			}

			ehrAmendment.IsAccepted = status;
			ehrAmendment.Source = source.Value;
			ehrAmendment.SourceName = sourceNameTextBox.Text;
			ehrAmendment.Description = descriptionTextBox.Text;
			ehrAmendment.RequestedOn = dateRequested;
			ehrAmendment.AcceptedDeniedOn = dateAccepted;
			ehrAmendment.AppendedOn = dateAppended;

			EhrAmendments.Update(ehrAmendment);

			DialogResult = DialogResult.OK;
		}
	}
}
