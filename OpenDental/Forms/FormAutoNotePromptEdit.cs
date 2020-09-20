using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAutoNotePromptEdit : FormBase
	{
		private readonly AutoNotePrompt autoNotePrompt;

		public FormAutoNotePromptEdit(AutoNotePrompt autoNotePrompt)
		{
			InitializeComponent();

			this.autoNotePrompt = autoNotePrompt;
		}

		private void FormAutoNotePromptEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = autoNotePrompt.Description;
			labelTextBox.Text = autoNotePrompt.Label;

			typeComboBox.Items.Clear();
			foreach (var item in AutoNotePromptType.GetValues())
            {
				typeComboBox.Items.Add(item);
				if (item.Value == autoNotePrompt.Type)
                {
					typeComboBox.SelectedItem = item;
                }
            }

			optionsTextBox.Text = autoNotePrompt.Options;
		}

		private void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (typeComboBox.SelectedItem.ToString())
			{
				case AutoNotePromptType.Text:
					optionsLabel.Text = Translation.Common.DefaultText;
					autoNoteResponseButton.Visible = false;
					upButton.Visible = downButton.Visible = false;
					break;

				case AutoNotePromptType.OneResponse:
					optionsLabel.Text = Translation.Common.PossibleResponsesOneLinePerItem;
					autoNoteResponseButton.Visible = true;
					upButton.Visible = downButton.Visible = true;
					break;

				case AutoNotePromptType.MultiResponse:
					optionsLabel.Text = Translation.Common.PossibleResponsesOneLinePerItem;
					autoNoteResponseButton.Visible = false;
					upButton.Visible = downButton.Visible = true;
					break;
			}
		}

		private void LabelTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				e.SuppressKeyPress = true;
			}
		}

		private void AutoNoteResponseButton_Click(object sender, EventArgs e)
		{
			if (typeComboBox.SelectedItem.ToString() != AutoNotePromptType.OneResponse)
			{
				return;
			}

			using var formAutoNoteResponsePicker = new FormAutoNoteResponsePicker();
			if (formAutoNoteResponsePicker.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			string selectedResponse = formAutoNoteResponsePicker.AutoNoteResponseText;
			if (optionsTextBox.SelectionStart < optionsTextBox.Text.Length - 1)
			{
				optionsTextBox.Text = optionsTextBox.Text.Insert(optionsTextBox.SelectionStart, selectedResponse);
			}
			else
			{
				optionsTextBox.Text += selectedResponse + "\r\n";
			}
		}

		private void UpButton_Click(object sender, EventArgs e)
		{
			if (optionsTextBox.Text == "")
			{
				return;
			}

			var line = optionsTextBox.GetLineFromCharIndex(optionsTextBox.SelectionStart);
			if (line <= 0)
            {
				return;
            }

			var lines = new string[optionsTextBox.Lines.Length];
			optionsTextBox.Lines.CopyTo(lines, 0);

			(lines[line - 1], lines[line]) = (lines[line], lines[line - 1]);

			optionsTextBox.Lines = lines;

			line--;

			optionsTextBox.Select(optionsTextBox.GetFirstCharIndexFromLine(line), 
				lines[line].Length);
		}

		private void DownButton_Click(object sender, EventArgs e)
		{
			if (optionsTextBox.Text == "")
			{
				return;
			}

			var line = optionsTextBox.GetLineFromCharIndex(optionsTextBox.SelectionStart);
			if (line >= optionsTextBox.Lines.Length - 1)
			{
				return;
			}

			var lines = new string[optionsTextBox.Lines.Length];
			optionsTextBox.Lines.CopyTo(lines, 0);

			(lines[line + 1], lines[line]) = (lines[line], lines[line + 1]);

			optionsTextBox.Lines = lines;

			line++;

			optionsTextBox.Select(optionsTextBox.GetFirstCharIndexFromLine(line),
				lines[line].Length);
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
            {
				ShowError(Translation.Common.PleaseEnterDescription);

				return;
            }

            if (!(typeComboBox.SelectedItem is DataItem<string> type))
            {
                ShowError(Translation.Common.PleaseSelectType);

                return;
            }

			if (!AutoNotePrompts.IsDescriptionValid(description))
			{
				ShowError(Translation.Common.DescriptionCannotContainSpecialCharacters);

				return;
			}

			autoNotePrompt.Description = description;
			autoNotePrompt.Type = type.Value;
			autoNotePrompt.Label = labelTextBox.Text.Trim();
			autoNotePrompt.Options = optionsTextBox.Text.Trim();
			
			AutoNotePrompts.Save(autoNotePrompt);
			
			DialogResult = DialogResult.OK;
		}
	}
}
