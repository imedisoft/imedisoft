using Imedisoft.Data.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAutoNotePromptMultiResponse : FormAutoNotePromptBase
	{
		private readonly AutoNotePrompt autoNotePrompt;
		private readonly string autoNotePromptValue;
		private readonly bool isFirst;

		public FormAutoNotePromptMultiResponse(AutoNotePrompt autoNotePrompt, bool isFirst, string autoNotePromptValue = null)
		{
			InitializeComponent();

			this.autoNotePrompt = autoNotePrompt;
			this.autoNotePromptValue = autoNotePromptValue ?? autoNotePrompt.Options;
			this.isFirst = isFirst;
		}

		private void FormAutoNotePromptMultiResp_Load(object sender, EventArgs e)
		{
			var values = autoNotePromptValue.Split(',').Select(value => value.Trim()).ToList();

			Location = new Point(Left, Top + 150);

			promptLabel.Text = autoNotePrompt.Label;

			var options = autoNotePrompt.Options.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var option in options)
			{
				var index = promptCheckedListBox.Items.Add(option);

				if (values.Contains(option))
				{
					promptCheckedListBox.SetItemChecked(index, true);
				}
			}

			backButton.Visible = !isFirst;
		}

		private void SelectAllButton_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < promptCheckedListBox.Items.Count; i++)
			{
				promptCheckedListBox.SetItemChecked(i, true);
			}
		}

		private void SelectNoneButton_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < promptCheckedListBox.Items.Count; i++)
			{
				promptCheckedListBox.SetItemChecked(i, false);
			}
		}

		private void BackButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Retry;
		}

		private void PreviewButton_Click(object sender, EventArgs e)
		{
			var value = string.Join(", ", promptCheckedListBox.CheckedItems.OfType<string>());

			using var formAutoNotePromptPreview = new FormAutoNotePromptPreview(value);
			if (formAutoNotePromptPreview.ShowDialog(this) == DialogResult.OK)
			{
				Value = formAutoNotePromptPreview.Preview;

				DialogResult = DialogResult.OK;
			}
		}

		private void RemoveButton_Click(object sender, EventArgs e)
		{
			Value = "";

			DialogResult = DialogResult.OK;
		}

		private void NextButton_Click(object sender, EventArgs e)
		{
			if (promptCheckedListBox.CheckedItems.Count == 0)
            {
				ShowError(Translation.Common.PleaseSelectAtLeastOneResponse);

				return;
            }

			Value = string.Join(", ", promptCheckedListBox.CheckedItems.OfType<string>());

			DialogResult = DialogResult.OK;
		}
    }
}
