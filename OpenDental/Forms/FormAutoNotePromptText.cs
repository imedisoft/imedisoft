using Imedisoft.Data.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAutoNotePromptText : FormAutoNotePromptBase
	{
		private readonly AutoNotePrompt autoNotePrompt;
		private readonly string autoNotePromptText;
		private readonly bool isFirst;

		public FormAutoNotePromptText(AutoNotePrompt autoNotePrompt, bool isFirst, string autoNotePromptText = null)
		{
			InitializeComponent();

			this.autoNotePrompt = autoNotePrompt;
			this.autoNotePromptText = autoNotePromptText ?? autoNotePrompt.Options;
			this.isFirst = isFirst;
		}

		private void FormAutoNotePromptText_Load(object sender, EventArgs e)
		{
			Location = new Point(Left, Top + 150);

			promptLabel.Text = autoNotePrompt.Label;
			promptTextBox.Text = autoNotePromptText;
			promptTextBox.SelectionStart = promptTextBox.Text.Length;

			backButton.Visible = !isFirst;
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

		private void SkipButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Ignore;
		}

		private void NextButton_Click(object sender, EventArgs e)
		{
			Value = promptTextBox.Text.Trim();

			DialogResult = DialogResult.OK;
		}
    }
}
