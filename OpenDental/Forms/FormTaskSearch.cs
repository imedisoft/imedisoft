using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormTaskSearch : FormBase
	{
		private readonly List<Definition> taskPriorities;
		private readonly List<long> preloadedTaskIds = new List<long>();
		private List<Userod> users;
		private List<Tasks.TaskSearchResult> searchResults;

		public bool IsSelectionMode { get; set; }

		/// <summary>
		/// Gets the ID of the selected task.
		/// </summary>
		public long SelectedTaskId { get; set; }

		/// <summary>
		/// Gets the ID of the task.
		/// </summary>
		public long? TaskId { get; set; }

		public FormTaskSearch(List<long> preloadedTaskIds = null)
		{
			InitializeComponent();

			taskPriorities = Definitions.GetByCategory(DefinitionCategory.TaskPriorities, true);

			if (preloadedTaskIds != null)
			{
				this.preloadedTaskIds.AddRange(preloadedTaskIds);
			}
		}

		private void FormTaskSearch_Load(object sender, EventArgs e)
		{
			if (IsSelectionMode) cancelButton.Text = "&Cancel";

			users = Userods.GetAll();
			userComboBox.Items.Add("All");
			userComboBox.Items.Add("Me");
			userComboBox.SelectedIndex = 0;
			foreach (var user in users)
            {
				userComboBox.Items.Add(user);
            }

			priorityComboBox.Items.Add("All");
			priorityComboBox.SelectedIndex = 0;
			foreach (var priority in taskPriorities)
			{
				priorityComboBox.Items.Add(priority);
			}

			if (TaskId.HasValue && !preloadedTaskIds.Contains(TaskId.Value)) 
				preloadedTaskIds.Add(TaskId.Value);

			if (preloadedTaskIds.Count > 0)
				taskIdTextBox.Text = string.Join(", ", preloadedTaskIds);

			RefreshTable();
		}

		private void FillGrid()
		{
			tasksGrid.BeginUpdate();
			tasksGrid.Columns.Clear();
			tasksGrid.Rows.Clear();
			tasksGrid.Columns.Add(new GridColumn("Created", 70, HorizontalAlignment.Left));
			tasksGrid.Columns.Add(new GridColumn("Completed", 70, HorizontalAlignment.Left));
			tasksGrid.Columns.Add(new GridColumn("Description", 70) { IsWidthDynamic = true });

			foreach (var searchResult in searchResults)
			{
				var gridRow = new GridRow();

				gridRow.Cells.Add(searchResult.DateAdded);
				gridRow.Cells.Add(searchResult.DateCompleted);
				gridRow.Cells.Add(searchResult.Description);
				gridRow.LowerBorderColor = Color.Black;
				gridRow.ForeColor = Color.FromArgb(searchResult.Color);
				gridRow.Tag = searchResult;

				tasksGrid.Rows.Add(gridRow);
			}

			tasksGrid.EndUpdate();
		}

		private void RefreshButton_Click(object sender, EventArgs e) 
			=> RefreshTable();

		private void RefreshTable()
		{
			long? priorityId = null;
			if (priorityComboBox.SelectedItem is Definition priority)
            {
				priorityId = priority.Id;
            }

			long? userId = null;
			if (userComboBox.SelectedIndex == 1) userId = Security.CurrentUser.Id;
			else if (userComboBox.SelectedItem is Userod user)
            {
				userId = user.Id;
            }

			IEnumerable<long> taskListIds = null;
			if (taskListTextBox.Text != "")
			{
				taskListIds = TaskLists.GetIdsByDescription(taskListTextBox.Text);
				if (!taskListIds.Any())
				{
					ShowError("Task List not found.");

					return;
				}
			}

			IEnumerable<long> taskIds = null;
			if (taskIdTextBox.Text != "")
			{
				try
				{
					taskIds = taskIdTextBox.Text
						.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
						.Select(long.Parse)
						.ToList();
				}
				catch
				{
					ShowError("Invalid task number format.");

					return;
				}
			}

			long? patientId = null;

			var patientIdStr = patientIdTextBox.Text.Trim();
			if (patientIdStr.Length > 0)
			{
				if (long.TryParse(patientIdStr, out var result))
                {
					ShowError("Invalid patient number format.");

					return;
				}

				patientId = result;
			}

			searchResults = Tasks.GetDataSet(
				userId,
				taskListIds,
				taskIds,
				dateCreatedFromDateTimePicker.Text,
				dateCreatedToDateTimePicker.Text,
				dateCompletedFromDateTimePicker.Text,
				dateCompletedToDateTimePicker.Text,
				descriptionTextBox.Text,
				priorityId, patientId,
				includeCompletedCheckBox.Checked,
				limitCheckBox.Checked).ToList();

			FillGrid();
		}

		private void TasksGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (tasksGrid.Rows[e.Row].Tag is Tasks.TaskSearchResult searchResult)
			{
				if (IsSelectionMode)
				{
					SelectedTaskId = searchResult.Id;

					DialogResult = DialogResult.OK;

					return;
				}

				var task = Tasks.GetOne(searchResult.Id);
				if (task != null)
				{
					var formTaskEdit = new FormTaskEdit(task);

					formTaskEdit.Show();
				}
				else
				{
					ShowError("The task no longer exists.");
				}
			}
		}

		private void PickUserButton_Click(object sender, EventArgs e)
		{
            using var formUserPick = new FormUserPick
            {
                ListUserodsFiltered = Userods.GetAll()
            };

            if (formUserPick.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            userComboBox.SelectedIndex = users.FindIndex(x => x.Id == formUserPick.SelectedUserNum) + 2;
        }

		private void PickPatientButton_Click(object sender, EventArgs e)
		{
            using var formPatientSelect = new FormPatientSelect
            {
                SelectionModeOnly = true
            };

            if (formPatientSelect.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            patientIdTextBox.Text = formPatientSelect.SelectedPatientId.ToString();
        }

		private void DateCreatedFromDateTimePicker_ValueChanged(object sender, EventArgs e) 
			=> dateCreatedFromDateTimePicker.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

		private void DateCreatedToDateTimePicker_ValueChanged(object sender, EventArgs e) 
			=> dateCreatedToDateTimePicker.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

		private void ClearCreatedButton_Click(object sender, EventArgs e)
		{
			dateCreatedFromDateTimePicker.Value = DateTime.UtcNow;
			dateCreatedToDateTimePicker.Value = DateTime.UtcNow;
			dateCreatedFromDateTimePicker.CustomFormat = " ";
			dateCreatedToDateTimePicker.CustomFormat = " ";
		}

		private void DateCompletedFromDateTimePicker_ValueChanged(object sender, EventArgs e) 
			=> dateCompletedFromDateTimePicker.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

		private void DateCompletedToDateTimePicker_ValueChanged(object sender, EventArgs e) 
			=> dateCompletedToDateTimePicker.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

		private void ClearCompletedButton_Click(object sender, EventArgs e)
		{
			dateCompletedFromDateTimePicker.Value = DateTime.UtcNow;
			dateCompletedToDateTimePicker.Value = DateTime.UtcNow;
			dateCompletedFromDateTimePicker.CustomFormat = " ";
			dateCompletedToDateTimePicker.CustomFormat = " ";
		}

		private void NewTaskButton_Click(object sender, EventArgs e)
		{
			var task = new Task()
			{
				UserId = Security.CurrentUser.Id
			};

			using (var formTaskListSelect = new FormTaskListSelect())
			{
				formTaskListSelect.Text = "Add Task - " + formTaskListSelect.Text;

				if (formTaskListSelect.ShowDialog(this) != DialogResult.OK ||
					formTaskListSelect.SelectedList == null)
				{
					return;
				}

				task.TaskListId = formTaskListSelect.SelectedList.Id;
			}

			using (var formTaskEdit = new FormTaskEdit(task))
			{
				if (formTaskEdit.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}

				Tasks.Insert(task);

				SelectedTaskId = task.Id;
			}

			DialogResult = DialogResult.OK;

			Close();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;

			Close();
		}
	}
}
