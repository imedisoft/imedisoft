using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAutoNoteCompose : FormBase
	{
		class AutoNotePromptInfo
		{
			public readonly AutoNotePrompt Prompt;

			public string Value;

			public int Start;

			public int Length;

			public AutoNotePromptInfo(AutoNotePrompt autoNotePrompt, string prompt, int start)
			{
				Prompt = autoNotePrompt;
				Value = "";
				Start = start;
				Length = prompt.Length;
			}
		}

		private readonly string initialNote;
		private string expandedAutoNoteCategories;

		public string CompletedNote => noteTextBox.Text.Trim();

		public FormAutoNoteCompose(string initialNote = null)
		{
			InitializeComponent();

			this.initialNote = initialNote;
		}

		private void FormAutoNoteCompose_Load(object sender, EventArgs e)
		{
			expandedAutoNoteCategories = UserPreference.GetString(UserPreferenceName.AutoNoteExpandedCats);

			AutoNoteL.FillTreeView(autoNotesTreeView, expandedAutoNoteCategories);
		}

		private void FormAutoNoteCompose_Shown(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(initialNote))
			{
				PromptForAutoNotes(initialNote);
			}
		}

		private void AutoNotesTreeView_DoubleClick(object sender, EventArgs e)
		{
			InsertButton_Click(this, EventArgs.Empty);
		}

		private void InsertButton_Click(object sender, EventArgs e)
		{
            if (!(autoNotesTreeView.SelectedNode?.Tag is AutoNote autoNote))
            {
                return;
            }

            PromptForAutoNotes(autoNote.Content);
		}

		private void NoteTextBox_TextChanged(object sender, EventArgs e)
		{
			acceptButton.Enabled = noteTextBox.Text.Trim() != "";
		}

		private int PromptForAutoNotes(string note)
		{
			noteTextBox.SelectedText = note;

			var noteLength = noteTextBox.SelectionLength;

			var promptStart = noteTextBox.SelectionStart;
			var promptPos = promptStart;
			var promptInfos = new List<AutoNotePromptInfo>();

			/* Find all the valid prompts in the note... */
			foreach (var prompt in AutoNotePrompts.GetPromptRegexMatches(note))
            {
				promptPos = noteTextBox.Text.IndexOf(prompt.Value, promptPos);
				if (promptPos == -1)
                {
					continue;
                }

				var autoNotePromptDescription = prompt.Value.Substring(9, prompt.Length - 11);
				var autoNotePrompt = AutoNotePrompts.GetByDescription(autoNotePromptDescription);
				if (autoNotePrompt == null)
                {
					continue;
                }

				promptInfos.Add(new AutoNotePromptInfo(autoNotePrompt, prompt.Value, promptPos));
				promptPos += prompt.Value.Length;
			}

			/* Display all the prompts... */
			for (int i = 0; i < promptInfos.Count;)
			{
				var promptInfo = promptInfos[i];
				var promptIsFirst = i == 0;

				noteTextBox.Select(promptInfo.Start, promptInfo.Length);

				Application.DoEvents();

				FormAutoNotePromptBase formAutoNotePrompt = null;
				switch (promptInfo.Prompt.Type)
				{
					case AutoNotePromptType.Text:
						formAutoNotePrompt = new FormAutoNotePromptText(promptInfo.Prompt, promptIsFirst, promptInfo.Value);
						break;

					case AutoNotePromptType.OneResponse:
						formAutoNotePrompt = new FormAutoNotePromptOneResponse(promptInfo.Prompt, promptIsFirst, promptInfo.Value);
						break;

					case AutoNotePromptType.MultiResponse:
						formAutoNotePrompt = new FormAutoNotePromptMultiResponse(promptInfo.Prompt, promptIsFirst, promptInfo.Value);
						break;
				}

				if (formAutoNotePrompt == null)
				{
					i++;
					continue;
				}

				formAutoNotePrompt.ShowDialog(this);
				switch (formAutoNotePrompt.PromptResult)
				{
					case FormAutoNotePromptResult.MoveBack:
						i--;
						break;

					case FormAutoNotePromptResult.MoveNext:
						var value = formAutoNotePrompt.Value ?? "";
						if (value != promptInfo.Value)
						{
							var s = value.Length - promptInfo.Value.Length;
							if (s != 0)
							{
								for (int j = i + 1; j < promptInfos.Count; j++)
								{
									promptInfos[j].Start += s;
								}

								noteLength += s;
							}

							promptInfo.Value = value;

							noteTextBox.SelectedText = value;

							// TODO: Handle nested auto notes...
						}

						i++;
						break;

					case FormAutoNotePromptResult.Skip:
						i++;
						break;

					case FormAutoNotePromptResult.Abort:
						noteTextBox.Select(noteTextBox.Text.Length, 0);
						return -1;
				}
			}

			return noteLength;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var note = noteTextBox.Text.Trim();
			if (note.Length == 0)
            {
				ShowError(Translation.Common.PleaseEnterNote);

				return;
            }

			DialogResult = DialogResult.OK;
		}
	}
}
