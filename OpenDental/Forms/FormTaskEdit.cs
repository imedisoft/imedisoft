using CodeBase;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormTaskEdit : FormBase
	{
		private List<Def> priorities;
		private List<TaskNote> taskNotes;
		private long taskListId;
		private Task task;
		private bool taskDeleted;
		private long userId;
		private long? patientId;
		private long? appointmentId;
		private long? replyToUserId;
		private Def defaultPriority;

		/// <summary>
		///		<para>
		///			Gets or sets a value indicating whether the window is a automatic popup (i.e. 
		///			the window was opened without user interaction). 
		///		</para>
		///		<para>
		///			When set to true, this window will not activate and steal focus upon opening.
		///		</para>
		/// </summary>
		public bool IsPopup { get; set; }

		protected override bool ShowWithoutActivation => IsPopup;

		/// <summary>
		/// Gets the ID of the task that is being edited.
		/// </summary>
		public long TaskId => task.Id;

		/// <summary>
		/// Initializes a new instance of the <see cref="FormTaskEdit"/> class.
		/// </summary>
		/// <param name="task">The task to edit.</param>
		public FormTaskEdit(Task task)
		{
			InitializeComponent();

			this.task = task;
		}

		private void FormTaskListEdit_Load(object sender, EventArgs e)
		{
			LoadTask();

			notesGrid.ScrollToEnd();
		}

		private void LoadTask()
		{
			// If there is no user assigned, assign the task to the current user.
			if (task.UserId == 0)
            {
				userId = task.UserId = Security.CurrentUser.Id;
            }

			// Is this an existing task?
			if (task.Id > 0)
			{
				// If the task is not assigned to the current user and the user doesn't have the task edit permission, disable the delete button.
				if (task.UserId != Security.CurrentUser.Id && !Security.IsAuthorized(Permissions.TaskNoteEdit, true))
				{
					deleteButton.Enabled = true;
				}

				// If the tsak is not assigned to the current user and the user doesn't have the 'TaskEdit' permission, set form to readonly...
				if (task.UserId != Security.CurrentUser.Id && !Security.IsAuthorized(Permissions.TaskEdit, true))
				{
					DisableMostControls();
				}
			}

			// Check whether there are priorities defined.
			priorities = Defs.GetDefsForCategory(DefCat.TaskPriorities, true);
			if (priorities.Count < 1)
			{
				ShowError("There are no task priorities in Setup | Definitions. There must be at least one in order to use the task system.");

				DialogResult = DialogResult.Cancel;

				Close();
			}

			defaultPriority = priorities.FirstOrDefault();

			foreach (var priority in priorities)
			{
				if (!priority.IsHidden && priority.ItemValue == "D")
				{
					defaultPriority = priority;

					break;
				}
			}

			priorityComboBox.Items.Clear();
			foreach (var priority in priorities)
            {
				if (priority.IsHidden && priority.DefNum != task.PriorityId) continue;

				priorityComboBox.Items.Add(priority);
				if (priority.DefNum == task.PriorityId)
                {
					priorityComboBox.SelectedItem = priority;
                }
            }

			// If no priority was selected, select the default priority.
			if (task.PriorityId == 0 || priorityComboBox.SelectedIndex == -1)
            {
				priorityComboBox.SelectedItem = defaultPriority;
            }

			userTextBox.Text = Userods.GetName(task.UserId);//might be blank.
			taskListId = task.TaskListId;
			taskListTextBox.Text = TaskLists.GetOne(taskListId)?.Description;

			dateCreatedTextBox.Text = task.DateAdded.ToString();
			dateStartTextBox.Text = task.DateStart?.ToString() ?? DateTime.Now.ToString();
			dateCompletedTextBox.Text = task.DateCompleted?.ToString() ?? "";

			descriptionTextBox.Text = task.Description;
			descriptionTextBox.Select();
			descriptionTextBox.Select(task.Description.Length, 0);
			
			if (task.Status == TaskStatus.Done)
			{
				doneCheckBox.Checked = true;
			}

			repeatIntervalComboBox.SelectedIndex = (int)task.RepeatInterval;
			if (task.RepeatDate.HasValue)
			{
				repeatDateTextBox.Text = task.RepeatDate.Value.ToShortDateString();
			}

			editAutoNoteButton.Visible = GetHasAutoNotePrompt();
			if (task.Repeat)
			{
				newCheckBox.Enabled = false;
				doneCheckBox.Enabled = false;
				repeatDateTextBox.Enabled = false;
				repeatIntervalComboBox.Enabled = false;
			}

			// Get all the available task priorities
			priorities = Defs.GetDefsForCategory(DefCat.TaskPriorities, true);
			if (priorities.Count < 1)
			{
				ShowError("There are no task priorities in Setup | Definitions.  There must be at least one in order to use the task system.");

				DialogResult = DialogResult.Cancel;

				Close();
			}

			patientId = task.PatientId;
			appointmentId = task.AppointmentId;

			FillPatient();
			FillAppointment();
			FillNotesGrid();

			// If this is a existing task, try to the ID of the user that added the last note...
			if (task.Id > 0)
            {
				replyToUserId = taskNotes.Where(tn => tn.UserId != Security.CurrentUser.Id).LastOrDefault()?.UserId;
				if (!replyToUserId.HasValue)
                {
					replyToUserId = Security.CurrentUser.Id;
                }
			}

			// If we don't know who to reply to, hide the reply to button...
			if (!replyToUserId.HasValue)
            {
				replyLabel.Visible = false;
				replyButton.Visible = false;
			}
            else
            {
				replyLabel.Text = "(Send to " + Userods.GetName(replyToUserId.Value) + ")";
            }

			// If the user is not allowed to edit tasks, hide the audit button.
			if (!Security.IsAuthorized(Permissions.TaskEdit, true))
			{
				auditButton.Visible = false;
			}

			SetStartingLocation();
		}

		/// <summary>
		///		<para>
		///			Determines the starting position of the form. If there are no other task forms
		///			open, the form will be displayed in the center of the screen.
		///		</para>
		///		<para>
		///			If there are other task forms open, the position of this form will be set to be
		///			slighly offset relative to the previous task form. This ensures that when 
		///			multiple task forms are opened, they are not displayed on top of eachother, 
		///			instead they are displayed in a cascading manner.
		///		</para>
		/// </summary>
		private void SetStartingLocation()
		{
			const int Offset = 20;

			// Default to center screen.
			StartPosition = FormStartPosition.CenterScreen;

			// Get a list of all the currently open task forms.
			var formTaskEdits = Application.OpenForms.OfType<FormTaskEdit>().ToList();
			if (formTaskEdits.Count <= 1)
			{
				return;
			}

			// Search backwards through the list to find the first visible task form.
			Form previousForm = null;
			for (int i = formTaskEdits.Count - 2; i >= 0; i--)
            {
				var form = formTaskEdits[i];

				if (form != null && !form.IsDisposed && form.WindowState != FormWindowState.Minimized)
                {
					previousForm = form;

					break;
				}
            }

			// If we couldn't find another task edit form, just display this one centered.
			if (previousForm == null) return;

			var currentScreen = System.Windows.Forms.Screen.FromControl(previousForm);
			var currentPos = previousForm.Location;

			// Determine the position of this form, if it's going offscreen reset to the top left corner.
			var startPos = new Point(currentPos.X + Offset, currentPos.Y + Offset);
			if (currentScreen.WorkingArea.Contains(new Rectangle(startPos, Size)))
            {
				startPos = new Point(
					currentScreen.WorkingArea.X, 
					currentScreen.WorkingArea.Y);
            }

			StartPosition = FormStartPosition.Manual;

			Location = startPos;
		}

		private void FillNotesGrid()
		{
			if (taskDeleted) return;

			notesGrid.BeginUpdate();
			notesGrid.ListGridColumns.Clear();
			notesGrid.ListGridColumns.Add(new GridColumn("Date Time", 120));
			notesGrid.ListGridColumns.Add(new GridColumn("User", 80));
			notesGrid.ListGridColumns.Add(new GridColumn("Note", 400));
			notesGrid.ListGridRows.Clear();

			taskNotes = TaskNotes.GetForTask(task.Id);
			foreach (var taskNote in taskNotes)
			{
				var gridRow = new GridRow();

				gridRow.Cells.Add(taskNote.DateModified.ToShortDateString() + " " + taskNote.DateModified.ToShortTimeString());
				gridRow.Cells.Add(Userods.GetName(taskNote.UserId));
				gridRow.Cells.Add(taskNote.Note);
				gridRow.Tag = taskNote;

				notesGrid.ListGridRows.Add(gridRow);
			}

			notesGrid.EndUpdate();
		}

		private void NotesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (taskDeleted) return;

			if (!(notesGrid.ListGridRows[e.Row].Tag is TaskNote taskNote))
			{
				return;
			}

			EditTaskNote(taskNote);
		}

		private void EditTaskNote(TaskNote taskNote)
        {
			using var formTaskNoteEdit = new FormTaskNoteEdit(taskNote);
			{
				if (formTaskNoteEdit.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
			}

			if (task.Status == TaskStatus.Done)
			{
				doneCheckBox.Checked = false;
			}

			FillNotesGrid();

			Signalods.SetInvalid(InvalidType.TaskPopup, KeyType.Task, task.Id);

			TaskUnreads.AddUnreads(task, Security.CurrentUser.Id);

			SendSignalsRefillLocal(task);
		}

		/// <summary>
		/// Adds a new note to the task, and opens it for editing by the user. 
		/// Will not open if the task is not visible, is deleted, or if a child form is already open.
		/// </summary>
		public void AddNoteToTaskAndEdit(string initialText = "")
		{
			if (!Visible || taskDeleted) return;

            var taskNote = new TaskNote
            {
                TaskId = task.Id,
                DateModified = DateTime.Now,
                UserId = Security.CurrentUser.Id,
                Note = initialText
            };

			EditTaskNote(taskNote);
		}

		private void AddNoteButton_Click(object sender, EventArgs e)
		{
			AddNoteToTaskAndEdit();
		}

		private void NewCheckBox_Click(object sender, EventArgs e)
		{
			if (newCheckBox.Checked && doneCheckBox.Checked)
			{
				doneCheckBox.Checked = false;
			}
		}

		private void DoneCheckBox_Click(object sender, EventArgs e)
		{
			if (newCheckBox.Checked && doneCheckBox.Checked)
			{
				newCheckBox.Checked = false;
			}
		}

		private void FillPatient()
        {
			if (patientId.HasValue)
            {
				patientTextBox.Text = $"{Patients.GetPat(patientId.Value).GetNameLF()} - {patientId}";
            }
            else
            {
				patientTextBox.Text = "(none)";
            }
        }

		private void FillAppointment()
        {
			if (appointmentId.HasValue)
            {
				var appt = Appointments.GetOneApt(appointmentId.Value);
				if (appt == null)
                {
					appointmentTextBox.Text = "(deleted)";
                }
                else
                {
					appointmentTextBox.Text = $"{Patients.GetPat(appt.PatNum).GetNameLF()}  {appt.AptDateTime}  {appt.ProcDescript}  {appt.Note}";
				}
			}
            else
            {
				appointmentTextBox.Text = "(none)";
            }
        }

		private void DateStartNowButton_Click(object sender, EventArgs e)
		{
			dateStartTextBox.Text = DateTime.Now.ToString();
		}

		private void DateCompletedNowButton_Click(object sender, EventArgs e)
		{
			dateCompletedTextBox.Text = DateTime.Now.ToString();
		}

		private void AutoNoteButton_Click(object sender, EventArgs e)
		{
			using var formAutoNoteCompose = new FormAutoNoteCompose();

			if (formAutoNoteCompose.ShowDialog() == DialogResult.OK)
			{
				descriptionTextBox.AppendText(formAutoNoteCompose.CompletedNote);

				editAutoNoteButton.Visible = GetHasAutoNotePrompt();
			}
		}

		private void TaskDescriptionNotesSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
		{
			descriptionTextBox.Invalidate();
		}

		private void PriorityComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (priorityComboBox.SelectedItem is Def priority)
            {
				colorButton.BackColor = Defs.GetColor(DefCat.TaskPriorities, priority.DefNum);
			}
		}

		private void AuditButton_Click(object sender, EventArgs e)
		{
			if (Tasks.IsTaskDeleted(task.Id))
			{
				SetFormToDeletedMode();

				ShowError("Task has been deleted, no history can be retrieved.");

				return;
			}

			var formTaskHistory = new FormTaskHistory(task.Id);

			formTaskHistory.Show();
		}

		private void PatientPickButton_Click(object sender, EventArgs e)
		{
            using var formPatientSelect = new FormPatientSelect
            {
                SelectionModeOnly = true
            };

			if (formPatientSelect.ShowDialog(this) != DialogResult.OK) return;

			patientId = formPatientSelect.SelectedPatientId;

			FillPatient();
		}

		private void PatientGoButton_Click(object sender, EventArgs e)
		{
			if (!TrySave(out var _)) return;

			// TODO: Close form and navigate to patient...
		}

		private void AppointmentPickButton_Click(object sender, EventArgs e)
		{
            using var formPatientSelect = new FormPatientSelect
            {
                SelectionModeOnly = true
            };

            if (formPatientSelect.ShowDialog(this) != DialogResult.OK) 
				return;

            var formApptsOther = new FormApptsOther(formPatientSelect.SelectedPatientId, null)
            {
                SelectionMode = true
            };

            if (formApptsOther.ShowDialog(this) != DialogResult.OK)
				return;

			appointmentId = formApptsOther.ListAptNumsSelected[0];
		}

        private void AppointmentGoButton_Click(object sender, EventArgs e)
		{
			if (!TrySave(out var _)) return;

			// TODO: Close form and navigate to appointment...
		}

		private void UserPickButton_Click(object sender, EventArgs e)
		{
			using var formLogOn = new FormLogOn(isTemporary: true);

			if (formLogOn.ShowDialog(this) == DialogResult.OK)
			{
				task.UserId = formLogOn.User.Id;

				userTextBox.Text = Userods.GetName(task.UserId);
			}
		}

		private void DescriptionTextBox_TextChanged(object sender, EventArgs e)
		{
			if (task.Status == TaskStatus.Done && descriptionTextBox.Text != task.Description)
			{
				doneCheckBox.Checked = false;
			}
		}

		private void CopyButton_Click(object sender, EventArgs e)
		{
			try
			{
				ODClipboard.Text = CreateCopyTask();
			}
			catch
			{
				ShowError("Could not copy contents to the clipboard. Please try again.");

				return;
			}

			Tasks.TaskEditCreateLog("Copied Task Note", task);
		}

		private string CreateCopyTask()
		{
			var stringBuilder = new StringBuilder();

			stringBuilder.Append($"Task #{task.Id}");

			if (patientId.HasValue)
				stringBuilder.Append($" - Patient #{patientId}");

			stringBuilder.AppendLine();
			stringBuilder.Append(task.DateStart.Value.ToShortDateString() + " " + task.DateStart.Value.ToShortTimeString());
			if (!string.IsNullOrWhiteSpace(patientTextBox.Text))
            {
				stringBuilder.Append($" - {patientTextBox.Text.Trim()}");
            }
			stringBuilder.Append($" - {userTextBox.Text.Trim()}");
			stringBuilder.AppendLine($" - {descriptionTextBox.Text.Trim()}");

			foreach (var taskNote in taskNotes)
            {
				stringBuilder.AppendLine("--------------------------------------------------");
				stringBuilder.Append($"== {Userods.GetName(taskNote.UserId)}");
				stringBuilder.Append($" - {taskNote.DateModified.ToShortDateString()} {taskNote.DateModified.ToShortTimeString()}");
				stringBuilder.Append($" - {taskNote.Note}");
				stringBuilder.AppendLine();
            }

			return stringBuilder.ToString();
		}

		public void OnTaskEdited()
		{
            var task = Tasks.GetOne(this.task.Id);
            if (task == null)
            {
                SetFormToDeletedMode();

                return;
            }

            if (!this.task.Equals(task))
            {
                refreshButton.Visible = true;
                taskChangedLabel.Visible = true;
            }

            this.task = task;
        }

		/// <summary>
		///		<para>
		///			Validates all the fields and if valid saves the changes to the database.
		///		</para>
		///		<para>
		///			Any validation errors will result in a error message being displayed to the user.
		///		</para>
		/// </summary>
		/// <returns>
		///		True if the task was updated succesfully; otherwise, false.
		/// </returns>
		private bool TrySave(out bool wasChanged)
		{
			wasChanged = false;

			var newDescription = descriptionTextBox.Text.Trim();
			if (string.IsNullOrWhiteSpace(newDescription))
			{
				ShowError("Please enter a description.");

				return false;
			}

			// Determine whether a priority has been selected.
			long newPriorityId;
			if (!(priorityComboBox.SelectedItem is Def priority))
            {
				ShowError("Please select a priority for this task.");

				return false;
			}
			newPriorityId = priority.DefNum;

			// If a start date was entered, check whether a valid date and time was entered...
			DateTime? newDateStart = null;
			if (!string.IsNullOrWhiteSpace(dateStartTextBox.Text))
            {
				if (!DateTime.TryParse(dateStartTextBox.Text, out var date))
                {
					ShowError("Please fix date/time entry.");

					return false;
                }

				newDateStart = date;
            }

			// If a completion date was entered, check whether a valid date and time was entered...
			DateTime? newDateCompleted = null;
			if (!string.IsNullOrWhiteSpace(dateCompletedTextBox.Text))
            {
				if (!DateTime.TryParse(dateCompletedTextBox.Text, out var date))
				{
					ShowError("Please fix date/time entry.");

					return false;
				}

				newDateCompleted = date;
			}


			var newStatus = doneCheckBox.Checked ? TaskStatus.Done : TaskStatus.New;

			// If the task is marked as 'done', there must be a completion date. If the user didn't enter one we use the current date and time.
			if (newStatus == TaskStatus.Done && newDateCompleted == null) newDateCompleted = DateTime.Now;

			// Determine the new repeat interval.
			var newRepeatInterval = (TaskRepeatInterval)repeatIntervalComboBox.SelectedIndex;
			var newRepeat = newRepeatInterval != TaskRepeatInterval.Never;

			// Determine the new repeat date of the task...
			var newRepeatDate = task.RepeatDate;
			if (string.IsNullOrWhiteSpace(repeatDateTextBox.Text))
			{
				if (!newRepeatDate.HasValue && newRepeatInterval == TaskRepeatInterval.Once)
                {
					ShowError("Please specify the date on which to repeat this task.");

					return false;
                }

				if (!newRepeatDate.HasValue && newRepeatInterval != TaskRepeatInterval.Never)
                {
					newRepeatDate = newRepeatInterval switch
                    {
                        TaskRepeatInterval.Daily => DateTime.UtcNow.AddDays(1),
                        TaskRepeatInterval.Weekly => DateTime.UtcNow.AddDays(7),
                        TaskRepeatInterval.Monthly => DateTime.UtcNow.AddMonths(1),
                        _ => null,
                    };
                }
			}
            else
            {
				if (!DateTime.TryParse(repeatDateTextBox.Text, out var date))
				{
					ShowError("Please enter a valid repeat date.");

					return false;
				}

				newRepeatDate = date;
			}

			// Determine the new repeat task ID, if the user modified the interval or date we'll assume the user
			// wants the task to repeat again so we clear the ID of the repeat task...
			var newRepeatTaskId = task.RepeatTaskId;
			if (newRepeatTaskId.HasValue)
            {
				if (newRepeatInterval != task.RepeatInterval || newRepeatDate != task.RepeatDate)
                {
					newRepeatTaskId = null;
                }
            }

			// Check whether any details have been changed. If there are no changes we don't need to do anything, simply return true.
			var taskChanged = 
				task.TaskListId != taskListId ||
				task.Description != newDescription ||
				task.Repeat != newRepeat ||
				task.RepeatInterval != newRepeatInterval ||
				task.RepeatDate != newRepeatDate ||
				task.RepeatTaskId != newRepeatTaskId ||
				task.PatientId != patientId ||
				task.AppointmentId != appointmentId ||
				task.UserId != userId ||
				task.PriorityId != newPriorityId ||
				task.DateStart != newDateStart ||
				task.DateCompleted != newDateCompleted ||
				task.Status != newStatus;

			if (!taskChanged) return true;

			task.TaskListId = taskListId;
			task.Description = newDescription;
			task.Repeat = newRepeat;
			task.RepeatInterval = newRepeatInterval;
			task.RepeatDate = newRepeatDate;
			task.RepeatTaskId = newRepeatTaskId;
			task.PatientId = patientId;
			task.AppointmentId = appointmentId;
			task.UserId = userId;
			task.PriorityId = newPriorityId;
			task.DateStart = newDateStart;
			task.DateCompleted = newDateCompleted;
			task.Status = newStatus;

			// Save the task to the database...
			try
			{
				if (task.Id > 0)
				{
					if (refreshButton.Visible)
					{
						if (Prompt(
							"There have been changes to the task since it has been loaded. " +
							"You must refresh before saving. Would you like to refresh now?") == DialogResult.Yes)
						{
							RefreshTask();
						}

						return false;
					}

					// TODO: Create a task history record...

					// TODO: Update the task...
				}
                else
                {
					// TODO: Create the task...
                }
			}
			catch (Exception exception)
			{
				Cursor = Cursors.Default;

				ShowError(exception.Message);

				return false;
			}

			wasChanged = true;

			return true;
		}

		private void RefreshButton_Click(object sender, EventArgs e)
		{
			RefreshTask();
		}

		private void RefreshTask()
		{
			if (task == null)
			{
				ShowError("This task is in an invalid state. The task will now be closed so it can be opened again in a valid state.");

				DialogResult = DialogResult.Abort;

				Close();

				return;
			}

			task = Tasks.GetOne(task.Id);
			if (task == null)
			{
				ShowError("This task has been deleted and must be closed.");

				DialogResult = DialogResult.Abort;

				Close();

				return;
			}

			LoadTask();

			refreshButton.Visible = false;
			taskChangedLabel.Visible = false;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (task.Id > 0)
			{
				if (Prompt("Delete?") == DialogResult.No) return;

				if (Tasks.GetOne(task.Id) == null)
				{
					ShowInfo("Task already deleted.");

					deleteButton.Enabled = false;
					acceptButton.Enabled = false;
					sendButton.Enabled = false;
					addNoteButton.Enabled = false;
					Text += " - {Deleted}";

					return;
				}

				var taskList = TaskLists.GetOne(task.TaskListId);

				Tasks.TaskEditCreateLog($"Deleted task from task list {taskList.Description}.", task);
			}

			Tasks.Delete(task.Id);

			SendSignalsRefillLocal(task, task.TaskListId);

            TaskHists.Insert(new TaskHistory(task));

			SecurityLogs.MakeLogEntry(Permissions.TaskEdit, 0, "Task " + task.Id + " deleted", 0);

			DialogResult = DialogResult.OK;

			Close();
		}

		private void ReplyButton_Click(object sender, EventArgs e)
		{
			if (!replyToUserId.HasValue) return;

			long oldTaskListId = task.TaskListId;

			long inboxTaskListId = Userods.GetInbox(replyToUserId.Value);
			if (inboxTaskListId == 0)
			{
				ShowError("No inbox has been set up for this user yet.");

				return;
			}

			if (descriptionTextBox.Text == task.Description)
			{
				var taskNote = new TaskNote
				{
					TaskId = task.Id,
					DateModified = DateTime.Now,
					UserId = Security.CurrentUser.Id,
				};

				using var formTaskNoteEdit = new FormTaskNoteEdit(taskNote);
				{
					if (formTaskNoteEdit.ShowDialog(this) != DialogResult.OK)
					{
						return;
					}
				}

				taskListId = inboxTaskListId;
				if (!TrySave(out var _))
				{
					return;
				}

				Signalods.SetInvalid(InvalidType.TaskPopup, KeyType.Task, task.Id);

				TaskUnreads.AddUnreads(task, Security.CurrentUser.Id);

				SendSignalsRefillLocal(task, task.TaskListId);

				DialogResult = DialogResult.OK;

				Close();

				return;
			}

			taskListId = inboxTaskListId;
			if (!TrySave(out var wasChanged))
			{
				return;
			}

			if (wasChanged)
			{
				Signalods.SetInvalid(InvalidType.TaskPopup, KeyType.Task, task.Id);

				TaskUnreads.AddUnreads(task, Security.CurrentUser.Id);

				SendSignalsRefillLocal(task, oldTaskListId);
			}

			DialogResult = DialogResult.OK;

			Close();
		}

		private void SendButton_Click(object sender, EventArgs e)
		{
            using var formTaskListSelect = new FormTaskListSelect();
            if (formTaskListSelect.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

			taskListId = formTaskListSelect.SelectedList.Id;
            taskListTextBox.Text = TaskLists.GetOne(taskListId)?.Description;

            if (!TrySave(out var wasChanged))
            {
                return;
            }

            if (wasChanged)
            {
                Signalods.SetInvalid(InvalidType.TaskPopup, KeyType.Task, task.Id);

                TaskUnreads.AddUnreads(task, Security.CurrentUser.Id);
            }

            SendSignalsRefillLocal(task, task.TaskListId);

            DialogResult = DialogResult.OK;

            Close();
        }

		/// <summary>
		/// Sets the form into "Deleted" mode to disallow changes to be made.
		/// </summary>
		private void SetFormToDeletedMode()
		{
			taskDeleted = true;
			taskChangedLabel.Visible = true;
			taskChangedLabel.Text = "The task has been deleted";

			DisableMostControls();
		}

		/// <summary>
		/// Sets all controls to read-only or disabled except cancel and copy button.
		/// </summary>
		private void DisableMostControls()
		{
			DisableAllExcept(cancelButton, copyButton, dateStartTextBox, dateCompletedTextBox, taskChangedLabel, taskDescriptionNotesSplitContainer);

			// Enable and set read-only for fields we want to allow copying from.
			taskDescriptionNotesSplitContainer.Enabled = true;
			autoNoteButton.Enabled = false;
			dateStartTextBox.ReadOnly = true;
			dateCompletedTextBox.ReadOnly = true;
			descriptionTextBox.ReadOnly = true;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!TrySave(out var wasChanged)) return;

			if (!wasChanged)
			{
				DialogResult = DialogResult.OK;

				Close();

				return;
			}

			Signalods.SetInvalid(InvalidType.TaskPopup, KeyType.Task, task.Id);

			TaskUnreads.AddUnreads(task, Security.CurrentUser.Id);

			SendSignalsRefillLocal(task);

			DialogResult = DialogResult.OK;

			Close();
		}

		/// <summary>
		/// Determines which signals need to be sent, and sends them.
		/// Pass in taskListNumOld if the taskListNum has possibly changed.
		/// </summary>
		private void SendSignalsRefillLocal(Task task, long? oldTaskListId = null)
		{
            var signalIds = new List<long>
            {
                Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, task.TaskListId),
                Signalods.SetInvalid(InvalidType.Task, KeyType.Task, task.Id)
            };

            if (oldTaskListId.HasValue && task.TaskListId != oldTaskListId)
            {
                signalIds.Add(Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, oldTaskListId.Value));
            }

			if (userId != task.UserId)
			{
				signalIds.Add(Signalods.SetInvalid(InvalidType.TaskAuthor, KeyType.Undefined, userId));
				signalIds.Add(Signalods.SetInvalid(InvalidType.TaskAuthor, KeyType.Undefined, task.UserId));
			}

            if (patientId != task.PatientId)
            {
				if (patientId.HasValue)
                {
					signalIds.Add(Signalods.SetInvalid(InvalidType.TaskPatient, KeyType.Undefined, patientId.Value));
				}

				if (task.PatientId.HasValue)
                {
					signalIds.Add(Signalods.SetInvalid(InvalidType.TaskPatient, KeyType.Undefined, task.PatientId.Value));
				}
            }

            UserControlTasks.RefillLocalTaskGrids(this.task, signalIds);
        }

		private void CancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;

			Close();
		}

		private void FormTaskEdit_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.Abort) return;

			if (Security.CurrentUser != null)
			{
				TaskUnreads.SetRead(Security.CurrentUser.Id, task);
			}
		}

		private void EditAutoNoteButton_Click(object sender, EventArgs e)
		{
			if (GetHasAutoNotePrompt())
			{
                using var formAutoNoteCompose = new FormAutoNoteCompose
                {
                    MainTextNote = descriptionTextBox.Text
                };

                if (formAutoNoteCompose.ShowDialog(this) == DialogResult.OK)
				{
					descriptionTextBox.Text = formAutoNoteCompose.CompletedNote;
					editAutoNoteButton.Visible = GetHasAutoNotePrompt();
				}
			}
			else
			{
				ShowError("No auto note available to edit.");
			}
		}

		private bool GetHasAutoNotePrompt()
		{
			return Regex.IsMatch(descriptionTextBox.Text, @"\[Prompt:""[a-zA-Z_0-9 ]+""\]");
		}
    }
}
