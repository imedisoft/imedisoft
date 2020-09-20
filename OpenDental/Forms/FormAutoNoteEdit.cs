using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAutoNoteEdit : FormBase
	{
		private readonly AutoNote autoNote;
		private readonly bool isReadOnly;
		private int selectionStart;

		public FormAutoNoteEdit(AutoNote autoNote)
		{
			InitializeComponent();

			this.autoNote = autoNote;

			isReadOnly = !Security.IsAuthorized(Permissions.AutoNoteQuickNoteEdit, true);
		}

		private void FormAutoNoteEdit_Load(object sender, EventArgs e)
		{
			if (isReadOnly)
			{
				addButton.Enabled = false;
				acceptButton.Enabled = false;
				mainTextTextBox.ReadOnly = true;
				nameTextBox.ReadOnly = true;
			}
			else
			{
				autoNotePromptsGrid.CellDoubleClick += AutoNotePromptsGrid_CellDoubleClick;
				autoNotePromptsGrid.SelectionCommitted += AutoNotePromptsGrid_SelectionCommitted;
			}

			nameTextBox.Text = autoNote.Name;
			mainTextTextBox.Text = autoNote.Content;

			FillGrid();
		}

		private void FillGrid()
		{
			AutoNotePrompts.RefreshCache();

			autoNotePromptsGrid.BeginUpdate();
			autoNotePromptsGrid.Columns.Clear();
			autoNotePromptsGrid.Columns.Add(new GridColumn("", 100));
			autoNotePromptsGrid.Rows.Clear();

			foreach (var autoNotePrompts in AutoNotePrompts.GetAll())
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(autoNotePrompts.Description);
				gridRow.Tag = autoNotePrompts;

				autoNotePromptsGrid.Rows.Add(gridRow);
			}

			autoNotePromptsGrid.EndUpdate();
		}

		private void MainTextTextBox_Leave(object sender, EventArgs e)
		{
			selectionStart = mainTextTextBox.SelectionStart;
		}

		private void InsertButton_Click(object sender, EventArgs e)
		{
			var autoNotePrompt = autoNotePromptsGrid.SelectedTag<AutoNotePrompt>();
			if (autoNotePrompt == null)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			var prompt = autoNotePrompt.PromptTag;

			if (selectionStart < mainTextTextBox.Text.Length - 1)
			{
				mainTextTextBox.Text =
					mainTextTextBox.Text.Substring(0, selectionStart) + prompt +
					mainTextTextBox.Text.Substring(selectionStart);
			}
			else
			{
				mainTextTextBox.Text += prompt;
			}

			mainTextTextBox.Select(selectionStart + prompt.Length, 0);
			mainTextTextBox.Focus();
		}

		private void AutoNotePromptsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void AutoNotePromptsGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled = autoNotePromptsGrid.SelectedRows.Count > 0;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var autoNotePrompt = new AutoNotePrompt
			{
				Type = AutoNotePromptType.Text
			};

			using var formAutoNotePromptEdit = new FormAutoNotePromptEdit(autoNotePrompt);
			if (formAutoNotePromptEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var autoNotePrompt = autoNotePromptsGrid.SelectedTag<AutoNotePrompt>();
			if (autoNotePrompt == null)
			{
				return;
			}

			using var formAutoNotePromptEdit = new FormAutoNotePromptEdit(autoNotePrompt);
			if (formAutoNotePromptEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var autoNotePrompt = autoNotePromptsGrid.SelectedTag<AutoNotePrompt>();
			if (autoNotePrompt == null)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
			{
				return;
			}

			AutoNotePrompts.Delete(autoNotePrompt.Id);

			FillGrid();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!isReadOnly)
			{
				autoNote.Name = nameTextBox.Text;
				autoNote.Content = mainTextTextBox.Text;

				AutoNotes.Save(autoNote);

				CacheManager.RefreshGlobal(nameof(InvalidType.AutoNotes));
			}

			DialogResult = DialogResult.OK;
		}
    }
}
