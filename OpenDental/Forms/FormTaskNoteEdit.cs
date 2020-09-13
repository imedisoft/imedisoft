using Imedisoft.Data;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormTaskNoteEdit : FormBase
	{
		private readonly TaskNote taskNote;

		/// <summary>
		/// Called when this form is closed.
		/// </summary>
		public EventHandler EditComplete;

		public FormTaskNoteEdit(TaskNote taskNote)
        {
			InitializeComponent();

			this.taskNote = taskNote;
		}

		private void FormTaskNoteEdit_Load(object sender, EventArgs e)
		{
			dateTimeTextBox.Text = taskNote.DateModified.ToString();
			userTextBox.Text = Users.GetUserName(taskNote.UserId);
			noteTextBox.Text = taskNote.Note;
			noteTextBox.Select(taskNote.Note.Length, 0);

			Top += 150;

			if (taskNote.Id == 0)
			{
				dateTimeTextBox.ReadOnly = true;
			}
			else if (!Security.IsAuthorized(Permissions.TaskNoteEdit))
			{
				acceptButton.Enabled = false;
				deleteButton.Enabled = false;
			}
		}

		protected virtual void OnEditComplete() 
			=> EditComplete?.Invoke(this, EventArgs.Empty);

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (Prompt(Translation.Common.ConfirmDelete) == DialogResult.No) return;

			if (taskNote.Id > 0)
			{
				TaskNotes.Delete(taskNote.Id);

				DialogResult = DialogResult.OK;

				OnEditComplete();

				Tasks.TaskEditCreateLog(Permissions.TaskNoteEdit, "Deleted note from task", Tasks.GetOne(taskNote.TaskId));
			}
            else
            {
				DialogResult = DialogResult.Cancel;
			}

			Close();
		}

		private void AutoNoteButton_Click(object sender, EventArgs e)
		{
			using var formAutoNoteCompose = new FormAutoNoteCompose();

			if (formAutoNoteCompose.ShowDialog() == DialogResult.OK)
			{
				noteTextBox.Text += formAutoNoteCompose.CompletedNote;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var note = noteTextBox.Text.Trim();
			if (note.Length == 0)
			{
				ShowError("Please enter a note, or delete this entry.");

				return;
			}

			if (Tasks.IsTaskDeleted(taskNote.TaskId))
			{
				ShowError("The task for this note was deleted.");

				return;
			}

			if (!DateTime.TryParse(dateTimeTextBox.Text, out var dateModified))
            {
				ShowError(Translation.Common.PleaseEnterValidDate);

				return;
			}

			if (taskNote.Id == 0)
			{
				taskNote.Note = noteTextBox.Text;
				taskNote.DateModified = dateModified;

				TaskNotes.Insert(taskNote);

				Tasks.TaskEditCreateLog(Permissions.TaskNoteEdit, "Added task note", Tasks.GetOne(taskNote.TaskId));

				DialogResult = DialogResult.OK;

				OnEditComplete();
			}
			else if (taskNote.Note != noteTextBox.Text || dateModified != taskNote.DateModified)
			{
				taskNote.Note = noteTextBox.Text;
				taskNote.DateModified = dateModified;

				TaskNotes.Update(taskNote);

				Tasks.TaskEditCreateLog(Permissions.TaskNoteEdit, "Task note changed", Tasks.GetOne(taskNote.TaskId));

				DialogResult = DialogResult.OK;

				OnEditComplete();
			}
			else
			{
				DialogResult = DialogResult.Cancel;
			}

			Close();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;

			Close();
		}
	}
}
