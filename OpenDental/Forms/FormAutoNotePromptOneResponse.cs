using Imedisoft.Data.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAutoNotePromptOneResponse : FormAutoNotePromptBase
	{
		private readonly AutoNotePrompt autoNotePrompt;
		private readonly string autoNotePromptValue;
		private readonly bool isFirst;

		public FormAutoNotePromptOneResponse(AutoNotePrompt autoNotePrompt, bool isFirst, string autoNotePromptValue = null)
		{
			InitializeComponent();

			this.autoNotePrompt = autoNotePrompt;
			this.autoNotePromptValue = autoNotePromptValue ?? autoNotePrompt.Options;
			this.isFirst = isFirst;
		}

		private void FormAutoNotePromptOneResp_Load(object sender, EventArgs e)
		{
			Location = new Point(Left, Top + 150);

			promptLabel.Text = autoNotePrompt.Label;

			var options = autoNotePrompt.Options.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var option in options)
			{
				promptListBox.Items.Add(option);
				if (option == autoNotePromptValue)
				{
					promptListBox.SelectedItem = option;
				}
			}

			backButton.Visible = !isFirst;
		}

		private void PromptListBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (promptListBox.SelectedItem == null)
			{
				return;
			}

			NextButton_Click(this, EventArgs.Empty);
		}

		private void BackButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Retry;
		}

		private void RemoveButton_Click(object sender, EventArgs e)
		{
			Value = "";

			DialogResult = DialogResult.OK;
		}

		private void PreviewButton_Click(object sender, EventArgs e)
		{
			if (!(promptListBox.SelectedItem is string option))
			{
				ShowError(Translation.Common.PleaseSelectResponse);

				return;
			}

			using var formAutoNotePromptPreview = new FormAutoNotePromptPreview(option);
			if (formAutoNotePromptPreview.ShowDialog(this) == DialogResult.OK)
			{
				Value = formAutoNotePromptPreview.Preview;

				DialogResult = DialogResult.OK;
			}
		}

		private void NextButton_Click(object sender, EventArgs e)
		{
			var option = promptListBox.SelectedItem as string;
			if (option == null)
            {
				ShowError(Translation.Common.PleaseSelectResponse);
            }

			Value = option;

			DialogResult = DialogResult.OK;
		}
    }
}
