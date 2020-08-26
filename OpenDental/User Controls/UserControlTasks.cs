using CodeBase;
using Imedisoft.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class UserControlTasks : UserControl
	{
		private List<Task> tasks = new List<Task>();
		private List<TaskList> taskLists = new List<TaskList>();
		private readonly List<long> expandedTaskIds = new List<long>();



		[Category("Action"), Description("Fires towards the end of the FillGrid method.")]
		public event FillGridEventHandler FillGridEvent;
		

		
		///<summary>A TaskList that is on the 'clipboard' waiting to be pasted.  Will be null if nothing has been copied yet.</summary>
		private TaskList _clipTaskList;
		
		///<summary>A Task that is on the 'clipboard' waiting to be pasted.  Will be null if nothing has been copied yet.</summary>
		private Task _clipTask;
		
		///<summary>If there is an item on our 'clipboard', this tracks whether it was cut.</summary>
		private bool _wasCut;
		
		///<summary>The index of the last clicked item in the main list.</summary>
		private int _clickedI;
		
		///<summary>After closing, if this is not zero, then it will jump to the object specified in GotoKeyNum.</summary>
		//public TaskObjectType GotoType { get; set; }
		
		///<summary>After closing, if this is not zero, then it will jump to the specified patient.</summary>
		public long GotoKeyNum;
		
		///<summary>All notes for the showing tasks, ordered by date time.</summary>
		private List<TaskNote> _listTaskNotes = new List<TaskNote>();
		
		///<summary>A friendly string that could be used as the title of any window that has this control on it.
		///It will contain the description of the currently selected task list and a count of all new tasks within that list.</summary>
		public string ControlParentTitle;

		private bool _isTaskSortApptDateTime;//Use task AptDateTime sort setup in FormTaskOptions.
		private bool _isShowFinishedTasks = false;//Show finished task setup in FormTaskOptions.
		private bool _isShowArchivedTaskLists;//Show archived task lists in FormTaskOptions.
		private DateTime _dateTimeStartShowFinished = DateTimeOD.Today.AddDays(-7);//Show finished task date setup in FormTaskOptions.
		



		private bool _isCollapsedByDefault;
		private bool _hasListSwitched;
		
		///<summary>This can be three states: 0 for all tasks expanded, 1 for all tasks collapsed, and -1 for mixed.</summary>
		private int _taskCollapsedState;
		
		///<summary>When a task is selected via right click, we make a shallow copy of the task so menu options are performed on the correct task.</summary>
		private Task _clickedTask;
		
		///<summary>Signalnums for Task or TaskList signals sent from this machine, that have not yet been received back from 
		///FormOpenDental.OnProcessSignals(). Do not include InvalidType.TaskPopup.</summary>
		private List<long> _listSentTaskSignalNums = new List<long>();
		private static List<UserControlTasks> _listInstances = new List<UserControlTasks>();
		

		/// <summary>
		/// The ID's of the task lists the user is subscribed to.
		/// </summary>
		private static List<long> subscribedTaskListIds = new List<long>();



		///<summary>Dictionary to look up task list by primary key. Key: TaskListNum. Value: TaskList.</summary>
		private Dictionary<long, TaskList> _dictTaskLists;

		/// <summary>
		/// Creates a thread safe copy of _listSubscribedTaskListNums.
		/// </summary>
		private static List<long> ListSubscribedTaskListNums
		{
			get
			{
				lock (subscribedTaskListIds)
				{
					return new List<long>(subscribedTaskListIds);
				}
			}
		}

		public UserControlTasks()
		{
			InitializeComponent();
			tasksGrid.ContextMenu = menuEdit;

			_listInstances.Add(this);
		}

		/// <summary>
		/// Destructor.  Removes this instance from the private list of instances.
		/// </summary>
		~UserControlTasks()
		{
			_listInstances.Remove(this);
		}

		private void UserControlTasks_Resize(object sender, EventArgs e)
		{
			FillGrid(new List<Signalod>());
		}

		/// <summary>
		/// Reloads all task lists and rebuilds the tree view.
		/// </summary>
		private void RefreshTree()
        {
			var newTaskLists = TaskLists.GetAll();

			var trunk = Security.CurrentUser.RootTaskListId.HasValue ?
				newTaskLists.Where(tl => tl.Id == Security.CurrentUser.RootTaskListId.Value) :
				newTaskLists.Where(tl => !tl.ParentId.HasValue);

			taskListsTree.BeginUpdate();
			taskListsTree.Nodes.Clear();

            static TreeNode CreateTreeNode(TaskList taskList) => new TreeNode(taskList.Description)
                {
                    Tag = taskList
                };

			void CreateTreeNodes(TreeNodeCollection treeNodeCollection, IEnumerable<TaskList> taskLists)
            {
				foreach (var taskList in taskLists)
                {
					var treeNode = CreateTreeNode(taskList);

					treeNodeCollection.Add(treeNode);

					if (taskList.Id == Security.CurrentUser.InboxTaskListId)
                    {
						taskListsTree.SelectedNode = treeNode;
                    }

					CreateTreeNodes(treeNodeCollection, newTaskLists.Where(tl => tl.ParentId == taskList.Id));
                }
            }

			CreateTreeNodes(taskListsTree.Nodes, trunk);

			taskListsTree.EndUpdate();
		}
		
		public static void RefreshTasksForAllInstances(List<Signalod> signals)
		{
			foreach (var control in _listInstances)
			{
				if (!control.Visible || control.IsDisposed)
				{
					continue;
				}

				control.FillGrid(signals);
			}
		}

		public void InitializeOnStartup()
		{
			if (Security.CurrentUser == null)
			{
				return;
			}

			LayoutToolBar();

			_isTaskSortApptDateTime = Prefs.GetBool(PrefName.TaskSortApptDateTime);//This sets it for use and also for the task options default value.
			_isCollapsedByDefault = UserPreference.GetBool(UserPreferenceName.TaskCollapse);
			_hasListSwitched = true;
			_taskCollapsedState = _isCollapsedByDefault ? 1 : 0;

			SetMenusEnabled();
		}


		public void ClearLogOff()
		{
			taskListsTree.Nodes.Clear();

			tasksGrid.ListGridRows.Clear();
			tasksGrid.Invalidate();
		}

		private void UserControlTasks_Load(object sender, EventArgs e)
		{
		}

		public void LayoutToolBar()
		{
			mainToolBar.Buttons.Clear();

            mainToolBar.Buttons.Add(new ODToolBarButton
			{
				Text = "Options",
				ToolTipText = "Set session specific task options.",
				Tag = "Options"
			});

			mainToolBar.Buttons.Add(new ODToolBarButton("Add Task List", 0, "", "AddList"));

            mainToolBar.Buttons.Add(new ODToolBarButton("Add Task", 1, "", "AddTask")
			{
				Style = ODToolBarButtonStyle.DropDownButton,
				DropDownMenu = menuTask
			});

			mainToolBar.Buttons.Add(new ODToolBarButton("Search", -1, "", "Search"));

            mainToolBar.Buttons.Add(new ODToolBarButton
			{
				Text = "Manage Blocks",
				ToolTipText = "Manage which task lists will have popups blocked even when subscribed.",
				Tag = "BlockSubsc",
				Pushed = Security.CurrentUser.DefaultHidePopups
			});

			mainToolBar.Invalidate();
		}

		public void RefreshPatTicketsIfNeeded()
		{
			FillGrid();
		}

		/// <summary>
		/// Causes all instances of UserControlTasks to replace/remove the passed in Task and TaskNotes from the list of currently displayed 
		/// Tasks, then, if necessary, refills the grid without querying the database for the Task or TaskNotes. Adds signalNums for signals associated
		/// to the refreshes occurring in this method to the list of signals that have been sent, so FillGrid can ignore them if the refresh has already
		/// occurred locally. To remove task from grid in all instances, pass in canKeepTask=true.
		/// </summary>
		public static void RefillLocalTaskGrids(Task task, List<long> sentSignalIds)
		{
			DataValid.SetInvalid(InvalidType.Task); // Fires plugin hook, refreshes Chart module if visible.

            AddSentSignalNums(sentSignalIds);

			foreach (var userControl in _listInstances)
			{
				if (!userControl.Visible && !userControl.IsDisposed)
				{
					continue;
				}

				//long parentId = 0; // Default to one of the main trunks.

				if (task == null)
				{
					// Just FillGrid.
				}
				else if (task.Status == TaskStatus.Done && (!userControl._isShowFinishedTasks || userControl._isShowFinishedTasks))
				{
					userControl.tasks.RemoveAll(x => x.Id == task.Id);
					userControl._listTaskNotes.RemoveAll(x => x.TaskId == task.Id);
				}
				else
				{
					// Task is not in current TaskList, or was deleted(canKeepTask==false)
					userControl.tasks.RemoveAll(x => x.Id == task.Id);
					userControl._listTaskNotes.RemoveAll(x => x.TaskId == task.Id);
				}

				userControl.FullRefreshIfNeeded();
			}
		}

		/// <summary>
		/// Causes all instances of UserControlTasks to replace/remove the passed in TaskList from the list of currently displayed TaskLists, then, if necessary, refills the grid without querying the database for the TaskList.
		/// Adds signalNums for signals associated to the refreshes occurring in this method to the list of signals that have been sent, so FillGrid can ignore them if the refresh has already occurred locally.
		/// To remove taskList from grid in all instances, pass in canKeepTask=true.
		/// </summary>
		public static void RefillLocalTaskGrids(TaskList taskList, List<long> sentSignalIds)
		{
			AddSentSignalNums(sentSignalIds);

			var subscribedTaskListIds = ListSubscribedTaskListNums;

			foreach (var userControl in _listInstances)
			{
				if (taskList != null)
				{
					foreach (var list in userControl.taskLists)
					{
						if (list.Id == taskList.Id)
						{
							list.Status = taskList.Status;
						}
					}

					if (userControl._dictTaskLists != null && userControl._dictTaskLists.ContainsKey(taskList.Id))
					{
						userControl._dictTaskLists[taskList.Id].Status = taskList.Status;
					}
				}

				if (taskList == null)
				{
					// Just FillGrid
				}
				else if (!userControl._isShowArchivedTaskLists && taskList.Status == TaskListStatus.Archived)
				{
					if (userControl._dictTaskLists != null)
					{
						var taskListsDictionary = userControl._dictTaskLists;

						userControl.taskLists.RemoveAll(x => x.Status == TaskListStatus.Archived || TaskLists.IsAncestorTaskListArchived(ref taskListsDictionary, x));
						userControl._dictTaskLists = taskListsDictionary;
					}
				}
				else
				{
					userControl.taskLists.RemoveAll(x => x.Id == taskList.Id);
				}

				userControl.FullRefreshIfNeeded();
			}
		}

		private void FullRefreshIfNeeded()
		{
			FillGrid(new List<Signalod>());
		}

		/// <summary>
		/// Adds Signalod.SignalNums to each instance of this control's list of sent Task/TaskList related signalnums.
		/// Method is static so that each signalNum is only added once to each instance of UserControlTasks.
		/// </summary>
		private static void AddSentSignalNums(List<long> signalIds)
		{
			if (signalIds == null || signalIds.Count == 0) return;

			foreach (var userControl in _listInstances)
			{
				userControl._listSentTaskSignalNums.AddRange(signalIds);
			}
		}

		/// <summary>
		/// Removes any matching Signalod.SignalNums from this instance's list of sent Task/TaskList related signalnums.
		/// </summary>
		private List<Signalod> RemoveSentSignalNums(List<Signalod> receivedSignals)
		{
			if (receivedSignals == null || receivedSignals.Count == 0)
			{
				return new List<Signalod>();
			}

			for (int i = receivedSignals.Count - 1; i >= 0; i--)
			{
				long receivedSignalNum = receivedSignals[i].Id;

				if (receivedSignalNum.In(_listSentTaskSignalNums))
				{
					_listSentTaskSignalNums.Remove(receivedSignalNum);

					receivedSignals.RemoveAt(i);
				}
			}

			return receivedSignals;
		}

		/// <summary>
		/// If listSignals is NULL, a full refresh/query will be run for the grid.
		/// If listSignals contains one signal of InvalidType.Task for a task in _listTasks, the task is already refreshed in memory and only the one task is refreshed from the database.
		/// Otherwise, a full refresh will only be run when certain types of signals corresonding to the current selected tabs are found in listSignals.
		/// </summary>
		private void FillGrid(List<Signalod> signals = null)
		{
			if (Security.CurrentUser == null)
			{
				tasksGrid.BeginUpdate();
				tasksGrid.ListGridRows.Clear();
				tasksGrid.EndUpdate();

				return;
			}

			DateTime date = DateTime.MinValue;


			tasksGrid.Height = ClientSize.Height - tasksGrid.Top;
			if (signals == null)
			{//Full refresh.
				//RefreshMainLists(parent, date);
			}
			else
			{
				// Remove any Task related signals that originated from this instance of OpenDental.
				signals = RemoveSentSignalNums(
					signals.FindAll(x => x.Param1.In(
						nameof(InvalidType.Task),
						nameof(InvalidType.TaskList),
						nameof(InvalidType.TaskAuthor),
						nameof(InvalidType.TaskPatient))));

				// User is observing a task list for which a TaskList signal is specified, or TaskList from signal is a sublist of current view.
				if (signals.Exists(x => x.Param1 == nameof(InvalidType.TaskList) && (!x.Param2.HasValue || taskLists.Exists(y => y.Id == x.Param2.Value))))
				{
					//RefreshMainLists(parent, date);
				}
				else
				{ 
					// Individual Task signals. Only refreshes if the task is in the currently displayed list of Tasks. Add/Remove is addressed with TaskList signals.
					foreach (var signal in signals)
					{
						//if (signal.Param1.In(InvalidType.Task, InvalidType.TaskPopup) && signal.Name == KeyType.Task)
						//{
						//	if (tasks.Exists(x => x.Id == signal.Param2))
						//	{
						//		// A signal indicates that a task we are looking at has been modified.
						//		RefreshMainLists();
						//		break;
						//	}
						//}
					}
				}
			}

			tasksGrid.BeginUpdate();
			tasksGrid.ListGridColumns.Clear();
			tasksGrid.ListGridColumns.Add(new GridColumn("", 17) { ImageList = imageListTree });
			var column = new GridColumn("+/-", 17, HorizontalAlignment.Center);
			column.CustomClickEvent += GridHeaderClickEvent;
			tasksGrid.ListGridColumns.Add(column);
			tasksGrid.ListGridColumns.Add(new GridColumn("Description", 200));
			tasksGrid.ListGridRows.Clear();

			var appointmentIds = tasks.Where(x => x.AppointmentId.HasValue).Select(y => y.AppointmentId.Value).ToList();
			var appointmentDescriptions = Tasks.GetAppointmentDescriptions(appointmentIds);

			int selectedTaskIndex = -1;

			foreach (var task in tasks)
			{
				var description = "";
				if (task.Status == TaskStatus.Done)
				{
					description = "Done:" + task.DateCompleted.Value.ToShortDateString() + " - ";
				}

				if (task.PatientId.HasValue)
                {
					description += Patients.GetNameLF(task.PatientId.Value) + " - ";
				}

				if (task.AppointmentId.HasValue)
                {
					appointmentDescriptions.TryGetValue(task.AppointmentId.Value, out description);
                }

				if (!task.Description.StartsWith("==") && task.UserId != 0)
				{
					description += Userods.GetName(task.UserId) + " - ";
				}

				var notes = "";


				var taskNotes = _listTaskNotes.FindAll(x => x.TaskId == task.Id);
				if (!expandedTaskIds.Contains(task.Id) && taskNotes.Count > 1)
				{
					var lastTaskNote = taskNotes.Last();

					notes += 
						"\r\n\u22EE\r\n" + 
						"==" + Userods.GetName(lastTaskNote.UserId) + " - " + 
						lastTaskNote.DateModified.ToShortDateString() + " " + 
						lastTaskNote.DateModified.ToShortTimeString() + " - " +
						lastTaskNote.Note;
				}
				else
				{
					foreach (var taskNote in taskNotes)
					{
						notes += 
							"\r\n" + 
							"==" + Userods.GetName(taskNote.UserId) + " - " + 
							taskNote.DateModified.ToShortDateString() + " " + 
							taskNote.DateModified.ToShortTimeString() + " - " + 
							taskNote.Note;
					}
				}

				var gridRow = new GridRow();

				switch (task.Status)
				{
					case TaskStatus.New:
						gridRow.Cells.Add("4");
						break;

					case TaskStatus.Done:
						gridRow.Cells.Add("1");
						break;
				}

				if (expandedTaskIds.Contains(task.Id))
				{
					if (task.Description.Length > 250 || taskNotes.Count > 1 || (taskNotes.Count == 1 && notes.Length > 250))
					{
						gridRow.Cells.Add("-");
					}
					else
					{
						gridRow.Cells.Add("");
					}

					gridRow.Cells.Add(description + task.Description + notes);
				}
				else
				{
					// Conditions for giving collapse option: Descript is long, there is more than one note, or there is one note and it's long.
					if (task.Description.Length > 250 || taskNotes.Count > 1 || (taskNotes.Count == 1 && notes.Length > 250))
					{
						gridRow.Cells.Add("+");

						string rowString = description;
						if (task.Description.Length > 250)
						{
							rowString += task.Description.Substring(0, 250) + "(...)";//546,300 tasks have average Descript length of 142.1 characters.
						}
						else
						{
							rowString += task.Description;
						}

						if (notes.Length > 250)
						{
							rowString += notes.Substring(0, 250) + "(...)";
						}
						else
						{
							rowString += notes;
						}

						gridRow.Cells.Add(rowString);
					}
					else
					{
						gridRow.Cells.Add("");
						gridRow.Cells.Add(description + task.Description + notes);
					}
				}

				gridRow.BackColor = Defs.GetColor(DefCat.TaskPriorities, task.PriorityId);
				gridRow.Tag = task;

				tasksGrid.ListGridRows.Add(gridRow);
				if (_clickedTask is Task && task.Id == _clickedTask.Id)
				{
					selectedTaskIndex = tasksGrid.ListGridRows.Count - 1;
				}
			}

			tasksGrid.EndUpdate();
			//if(isTaskSelectedVisible) {//Only scroll the previously selected task (now reselected task) into view if it was previously visible.
			//gridMain.ScrollToIndex(selectedTaskIndex); For now, this is confusing techs, revisit later.
			//}
			tasksGrid.SetSelected(selectedTaskIndex, true);
			//Without this 'scroll value reset', drilling down into a tasklist that contains tasks will sometimes result in an empty grid, until the user 
			//interacts with the grid, example, scrolling will cause the grid to repaint and properly display the expected tasks.
			tasksGrid.ScrollValue = tasksGrid.ScrollValue;//this forces scroll value to reset if it's > allowed max.

			SetControlTitleHelper();
		}

		/// <summary>
		/// Click event for GridMain's collapse/expand column header.
		/// </summary>
		private void GridHeaderClickEvent(object sender, EventArgs e)
		{
			if (_taskCollapsedState == -1)
			{//Mixed mode
				_taskCollapsedState = _isCollapsedByDefault ? 1 : 0;
				FillGrid();//Re-do the grid with whatever their default mode is.
				return;
			}
			if (_taskCollapsedState == 0)
			{//All are NOT collapsed. Make them all collapsed.
				_taskCollapsedState = 1;
				FillGrid();
				return;
			}
			if (_taskCollapsedState == 1)
			{//All ARE collapsed.  Make them all NOT collapsed.
				_taskCollapsedState = 0;
				FillGrid();
				return;
			}
		}

		/// <summary>
		/// Updates ControlParentTitle to give more information about the currently selected task list.
		/// Currently only called in FillGrid()
		/// </summary>
		private void SetControlTitleHelper()
		{
			string taskListDescript = "";


			if (taskListDescript == "")
			{
				// Should only happen when at main trunk.
				ControlParentTitle = "Tasks";
			}
			else
			{
				int tasksNewCount = taskLists.Sum(x => x.NewTaskCount);

				tasksNewCount += tasks.Sum(x => x.Status == TaskStatus.New ? 1 : 0);

				ControlParentTitle = "Tasks - " + taskListDescript + " (" + tasksNewCount.ToString() + ")";
			}

			FillGridEvent?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// If parent=0, then this is a trunk.
		/// </summary>
		private void RefreshMainLists()
		{
			if (DesignMode)
			{
				taskLists = new List<TaskList>();
				tasks = new List<Task>();
				_listTaskNotes = new List<TaskNote>();
				return;
			}

			_listSentTaskSignalNums.Clear();//Full refresh, tracked sent signals are now irrelevant and taking up memory.

			//An old bug allowed a user to be subscribed to the same task list more than once so we need to try/catch the filling of this dictionary
			if (_dictTaskLists == null)
			{
				_dictTaskLists = new Dictionary<long, TaskList>();
				foreach (TaskList taskList in taskLists)
				{
					try
					{
						_dictTaskLists.Add(taskList.Id, taskList);
					}
					catch
					{
					}
				}
			}

			if (!_isShowArchivedTaskLists)
			{
				taskLists.RemoveAll(x => x.Status == TaskListStatus.Archived || TaskLists.IsAncestorTaskListArchived(ref _dictTaskLists, x));
			}

			//notes
			List<long> taskNums = new List<long>();
			for (int i = 0; i < tasks.Count; i++)
			{
				taskNums.Add(tasks[i].Id);
			}

			if (_hasListSwitched)
			{
				if (_isCollapsedByDefault)
				{
					expandedTaskIds.Clear();
				}
				else
				{
					expandedTaskIds.AddRange(taskNums);
				}
				_hasListSwitched = false;
			}
			else
			{
				if (_taskCollapsedState == 1)
				{//Header was clicked, make all collapsed
					expandedTaskIds.Clear();
				}
				else if (_taskCollapsedState == 0)
				{//Header was clicked, make all expanded
					expandedTaskIds.AddRange(taskNums);
				}
				else
				{
					for (int i = expandedTaskIds.Count - 1; i >= 0; i--)
					{
						if (!taskNums.Contains(expandedTaskIds[i]))
						{
							expandedTaskIds.Remove(expandedTaskIds[i]);//The Task was removed from the visual list, don't keep it around in the expanded list.
						}
					}
				}
			}

			_listTaskNotes = TaskNotes.RefreshForTasks(taskNums);
		}

		/// <summary>
		/// Gets all the task lists the user is subscribed to.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <returns>All the task lists the user is subscribed to.</returns>
		public static List<TaskList> GetSubscribedTaskLists(long userId)
		{
			var subscribedTaskListIds = TaskSubscriptions.GetSubscriptionsForUser(userId).ToList();

			return TaskLists.GetAll().Where(tl => subscribedTaskListIds.Contains(tl.Id)).ToList();
		}

		private void ToolBarMain_ButtonClick(object sender, ODToolBarButtonClickEventArgs e)
		{
			switch (e.Button.Tag.ToString())
			{
				case "Options":
					Options_Clicked();
					break;

				case "AddList":
					AddList_Clicked();
					break;

				case "AddTask":
					AddTask_Clicked();
					break;

				case "Search":
					Search_Clicked();
					break;

				case "BlockSubsc":
					BlockSubsc_Clicked();
					break;
			}
		}

		private void Options_Clicked()
		{
			var formTaskOptions = new FormTaskOptions(_isShowFinishedTasks, _dateTimeStartShowFinished, _isTaskSortApptDateTime, _isShowArchivedTaskLists);
			
			formTaskOptions.StartPosition = FormStartPosition.Manual;//Allows us to set starting form starting Location.
			Point pointFormLocation = PointToScreen(mainToolBar.Location);//Since we cant get ToolBarMain.Buttons["Options"] location directly.
			pointFormLocation.X += mainToolBar.Buttons["Options"].Bounds.Width;//Add Options button width so by default form opens along side button.
			Rectangle screenDim = SystemInformation.VirtualScreen;//Dimensions of users screen. Includes if user has more then 1 screen.
			if (pointFormLocation.X + formTaskOptions.Width > screenDim.Width)
			{//Not all of form will be on screen, so adjust.
				pointFormLocation.X = screenDim.Width - formTaskOptions.Width - 5;//5 for some padding.
			}
			if (pointFormLocation.Y + formTaskOptions.Height > screenDim.Height)
			{//Not all of form will be on screen, so adjust.
				pointFormLocation.Y = screenDim.Height - formTaskOptions.Height - 5;//5 for some padding.
			}
			formTaskOptions.Location = pointFormLocation;
			formTaskOptions.ShowDialog();

			_isShowFinishedTasks = formTaskOptions.IsShowFinishedTasks;
			_isShowArchivedTaskLists = formTaskOptions.IsShowArchivedTaskLists;
			_dateTimeStartShowFinished = formTaskOptions.DateTimeStartShowFinished;
			_isTaskSortApptDateTime = formTaskOptions.IsSortApptDateTime;
			_isCollapsedByDefault = UserPreference.GetBool(UserPreferenceName.TaskCollapse);
			_hasListSwitched = true;//To display tasks in correctly collapsed/expanded state
			FillGrid();
		}

		private void AddList_Clicked()
		{
			if (!Security.IsAuthorized(Permissions.TaskListCreate, false))
			{
				return;
			}

			//if (_listTaskListTreeHistory.Count == 0)
			//{
			//	ODMessageBox.Show(
			//		"Not allowed to add a task list to the trunk of the user tab. " +
			//		"Either use the subscription feature, or add it to a child list.");

			//	return;
			//}

			//if (tabContr.SelectedTab == tabPatientTickets)
			//{
			//	ODMessageBox.Show(
			//		"Not allowed to add a task list to the 'Patient Tasks' tab.");

			//	return;
			//}

			TaskList taskList = new TaskList();

			taskList.ParentId = 0;



			using var formTaskListEdit = new FormTaskListEdit(taskList);

			if (formTaskListEdit.ShowDialog(this) == DialogResult.OK)
			{
				long signalNum = Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, taskList.ParentId.Value);//Signal for source parent tasklist.

				RefillLocalTaskGrids(taskList, sentSignalIds: new List<long>() { signalNum });
			}
		}

		private void AddTask(bool isReminder)
		{
			if (Plugins.HookMethod(this, "UserControlTasks.AddTask_Clicked")) return;

            Task task = new Task
            {
                TaskListId = -1
            };
            Tasks.Insert(task);
			Task taskOld = task.Copy();

			task.UserId = Security.CurrentUser.Id;
			if (isReminder)
			{
				task.ReminderType = TaskReminderType.Once;
			}

			var formTaskEdit = new FormTaskEdit(task);

			formTaskEdit.Closing += new CancelEventHandler(TaskGoToEvent);
			formTaskEdit.Show();
		}

		private void AddTask_Clicked()
		{
			bool isReminder = false;

			AddTask(isReminder);
		}

		private void menuItemTaskReminder_Click(object sender, EventArgs e)
		{
			AddTask(true);
		}

		public void Search_Clicked()
		{
			var clipboardMatch = false;
			var clipboardTaskId = "";

			try
			{
				if (Clipboard.ContainsText())
				{
					var clipboardText = Clipboard.GetText();

					if (Regex.IsMatch(clipboardText, @"^TaskNum:\d+$")) // Very restrictive specific match for "TaskNum:##"
					{
						clipboardTaskId = clipboardText.Substring(8).Trim();

						if (!string.IsNullOrEmpty(clipboardTaskId))
						{
							Clipboard.Clear();

							clipboardMatch = true;
						}
					}
				}
			}
			catch
			{
			}

			var formTaskSearch = new FormTaskSearch();

			if (!clipboardMatch)
			{
				// If there is no match, open the form as it normally would
				formTaskSearch.Show(this);
				return;
			}

			if (int.TryParse(clipboardTaskId, out var taskId))
			{
				formTaskSearch.TaskId = taskId;

				var task = Tasks.GetOne(taskId);
				if (task == null)
				{
					// If the task doesn't match, open the form but pass in the task to search
					formTaskSearch.Show(this);

					return;
				}

				var formTaskEdit = new FormTaskEdit(task);

				formTaskEdit.Show();
				formTaskEdit.BringToFront();
			}
		}

		public void TaskGoToEvent(object sender, CancelEventArgs e)
		{
			var formTaskEdit = (FormTaskEdit)sender;

			//if (formTaskEdit.GotoType != TaskObjectType.None)
			//{
			//	GotoType = formTaskEdit.GotoType;
			//	GotoKeyNum = formTaskEdit.GotoKeyNum;
			//	FormOpenDental.S_TaskGoTo(GotoType, GotoKeyNum);
			//}

			if (!IsDisposed) FillGrid();
		}

		private void BlockSubsc_Clicked()
		{
			using var formTaskListBlocks = new FormTaskListBlocks();

			if (formTaskListBlocks.ShowDialog(this) == DialogResult.OK)
			{
				DataValid.SetInvalid(InvalidType.Security);
			}
		}

		private void Done_Clicked()
		{
			Task task = _clickedTask;
			Task oldTask = task.Copy();
			task.Status = TaskStatus.Done;

			if (!task.DateCompleted.HasValue)
			{
				task.DateCompleted = DateTime.Now;
			}

			try
			{
				Tasks.Update(task, oldTask);
			}
			catch (Exception exception)
			{
				// We manipulated the TaskStatus and need to set it back to what it was because something went wrong.
				int index = tasks.FindIndex(x => x.Id == oldTask.Id);
				if (index != -1)
				{
					tasks[index] = oldTask;
				}

                ODMessageBox.Show(exception.Message);

				return;
			}

			TaskUnreads.DeleteForTask(task);
			TaskHistory taskHist = new TaskHistory(oldTask);
			taskHist.HistoryUserId = Security.CurrentUser.Id;
			TaskHists.Insert(taskHist);

			long signalId = Signalods.SetInvalid(InvalidType.Task, KeyType.Task, task.Id);//Only needs to send signal for the one task.
			//RefillLocalTaskGrids(task, _listTaskNotes.FindAll(x => x.TaskId == task.Id), new List<long>() { signalNum });//No db call.
		}

		private void Edit_Clicked()
		{
			if (_clickedI < taskLists.Count)
			{
				using var formTaskListEdit = new FormTaskListEdit(taskLists[_clickedI]);

				formTaskListEdit.ShowDialog(this);

				long signalId = Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, taskLists[_clickedI].ParentId.Value);//Signal for source parent tasklist.
				
				RefillLocalTaskGrids(taskLists[_clickedI], new List<long>() { signalId });//No db call.
			}
			else
			{
				var formTaskEdit = new FormTaskEdit(_clickedTask);

				formTaskEdit.Show();
			}
		}

		private void Cut_Clicked()
		{
			if (_clickedI < taskLists.Count)
			{//is list
				_clipTaskList = taskLists[_clickedI].Copy();
				_clipTask = null;
			}
			else
			{//task
				_clipTaskList = null;
				_clipTask = _clickedTask.Copy();
			}
			_wasCut = true;
		}

		private void Copy_Clicked()
		{
			if (_clickedI < taskLists.Count)
			{//is list
				_clipTaskList = taskLists[_clickedI].Copy();
				_clipTask = null;
			}
			else
			{//task
				_clipTaskList = null;
				_clipTask = _clickedTask.Copy();
				if (!string.IsNullOrEmpty(_clipTask.ReminderGroupId))
				{
					//Any reminder tasks duplicated must have a brand new ReminderGroupId
					//so that they do not affect the original reminder task chain.
					Tasks.SetReminderGroupId(_clipTask);
				}
			}

			_wasCut = false;
		}

		/// <summary>
		/// When copying and pasting, Task hist will be lost because the pasted task has a new TaskNum.
		/// </summary>
		private void Paste_Clicked()
		{
			if (_clipTaskList != null)
			{//a taskList is on the clipboard
				if (!_wasCut)
				{
					return;//Tasklists are no longer allowed to be copied, only cut.  Code should never make it this far.
				}
				TaskList newTL = _clipTaskList.Copy();
				long clipTlParentNum = _clipTaskList.ParentId.Value;

				newTL.ParentId = null;

				if (_clipTaskList.Id == newTL.ParentId && _wasCut)
				{
					ODMessageBox.Show("Cannot cut and paste a task list into itself.  Please move it into a different task list.");
					return;
				}
				if (TaskLists.IsAncestor(_clipTaskList.Id, newTL.ParentId.Value))
				{
					//The user is attempting to cut or copy a TaskList into one of its ancestors.  We don't want to do normal movement logic for this case.
					//We move the TaskList desired to have its parent to the list they desire.  
					//We change the TaskList's direct children to have the parent of the TaskList being moved.
					MoveListIntoAncestor(newTL, _clipTaskList.ParentId.Value);
				}
				else
				{

					MoveTaskList(newTL);

				}
				List<long> listSignalNums = new List<long>();
				if (clipTlParentNum != 0)
				{
					listSignalNums.Add(Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, clipTlParentNum));//Signal for source parent tasklist.
				}
				if (newTL.ParentId != 0)
				{
					listSignalNums.Add(Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, newTL.ParentId.Value));//Signal for destination parent tasklist.
				}
				RefillLocalTaskGrids(newTL, listSignalNums);//No db call.
			}
			else if (_clipTask != null)
			{//a task is on the clipboard
				Task newT = _clipTask.Copy();
				long clipTaskTaskListNum = _clipTask.TaskListId;

				newT.TaskListId = 0;
				newT.Repeat = false;
				
				newT.RepeatTaskId = 0;//always
				if (!String.IsNullOrEmpty(newT.ReminderGroupId))
				{
					//Any reminder tasks duplicated to another task list must have a brand new ReminderGroupId
					//so that they do not affect the original reminder task chain.
					Tasks.SetReminderGroupId(newT);
				}

				if (_wasCut && Tasks.WasTaskAltered(_clipTask))
				{
                    ODMessageBox.Show("Not allowed to move because the task has been altered by someone else.");

					FillGrid();

					return;
				}

                List<TaskNote> noteList;
                var signalIds = new List<long>();

                string histDescript;
                if (_wasCut)
                { //cut
                    if (clipTaskTaskListNum == newT.TaskListId)
                    {//User cut then paste into the same task list.
                        return;//Nothing to do.
                    }

                    TaskNotes.GetForTask(newT.Id);
                    histDescript = "This task was cut from task list " + TaskLists.GetFullPath(_clipTask.TaskListId) + " and pasted into " + TaskLists.GetFullPath(newT.TaskListId);

                    Tasks.Update(newT, _clipTask);
                    signalIds.Add(Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, clipTaskTaskListNum));
                    signalIds.Add(Signalods.SetInvalid(InvalidType.Task, KeyType.Task, _clipTask.Id));
                }
                else
                { 
					// copied
                    noteList = TaskNotes.GetForTask(newT.Id);
                    newT.Id = Tasks.Insert(newT);//Creates a new PK for newT  Copy, no need to signal source.
                    signalIds.Add(Signalods.SetInvalid(InvalidType.Task, KeyType.Task, newT.Id));//Signal for new task.
                    histDescript = "This task was copied from task " + _clipTask.Id + " in task list " + TaskLists.GetFullPath(_clipTask.TaskListId);

                    for (int t = 0; t < noteList.Count; t++)
                    {
                        noteList[t].TaskId = newT.Id;
                        TaskNotes.Insert(noteList[t]);//Creates the new note with the current datetime stamp.
                        TaskNotes.Update(noteList[t]);//Restores the historical datetime for the note.
                    }
                }

                TaskHists.Insert(new TaskHistory(newT)
				{
					Description = histDescript,
				});

				Signalods.SetInvalid(InvalidType.TaskPopup, KeyType.Task, newT.Id);

				TaskUnreads.AddUnreads(newT, Security.CurrentUser.Id);

				signalIds.Add(Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, newT.TaskListId));

				//RefillLocalTaskGrids(newT, noteList, listSignalNums);//No db call.
			}

			// Turn the cut into a copy once the users has pasted at least once.
			_wasCut = false;
		}

		/// <summary>
		/// Return the FormTaskEdit that was created from showing the task.  Can return null.
		/// </summary>
		private FormTaskEdit SendToMe_Clicked(bool doOpenTask = true)
		{
			if (!Security.CurrentUser.InboxTaskListId.HasValue)
			{
				ODMessageBox.Show("You do not have an inbox.");

				return null;
			}

			Task task = _clickedTask;
			Task oldTask = task.Copy();
			task.TaskListId = Security.CurrentUser.InboxTaskListId.Value;

			Cursor = Cursors.WaitCursor;

			List<long> signalIds = new List<long>();

			try
			{
				Tasks.Update(task, oldTask);

				TaskHists.Insert(new TaskHistory(oldTask));

				signalIds.Add(Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, oldTask.TaskListId));//Signal for old TaskList containing this Task.
				signalIds.Add(Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, task.TaskListId));//Signal for new tasklist.
				signalIds.Add(Signalods.SetInvalid(InvalidType.Task, KeyType.Task, task.Id));//Signal for task.
				//RefillLocalTaskGrids(task, _listTaskNotes.FindAll(x => x.TaskId == task.Id), listSignalNums);
				
				Cursor = Cursors.Default;

				var formTaskEdit = new FormTaskEdit(task);
				formTaskEdit.IsPopup = true;

				if (doOpenTask)
				{
					formTaskEdit.Show();
				}

				return formTaskEdit;
			}
			catch (Exception ex)
			{
				Cursor = Cursors.Default;

				ODMessageBox.Show(ex.Message);

				FillGrid();//Full refresh on local machine.  This will revert/refresh the clicked task so any changes made above are ignored.

				return null;
			}
		}

		/// <summary>
		/// Sends a task to the current user, opens the task, and opens a new tasknote for the user to edit.
		/// </summary>
		private void SendToMeAndGoto_Clicked()
		{
			var formTaskEdit = SendToMe_Clicked(doOpenTask: false);
			if (formTaskEdit == null)
			{
				return;
			}

			Goto_Clicked();

			formTaskEdit.Show();
			formTaskEdit.Activate();
			formTaskEdit.AddNoteToTaskAndEdit("Returned call. ");

			Tasks.TaskEditCreateLog(Permissions.TaskNoteEdit, "Automatically added task note: Returned Call", Tasks.GetOne(formTaskEdit.TaskId));
		}

		private void Goto_Clicked()
		{
			//not even allowed to get to this point unless a valid task
			//Task task = _clickedTask;
			//GotoKeyNum = task.KeyNum;
			//FormOpenDental.S_TaskGoTo(GotoType, GotoKeyNum);
		}

		private void MarkRead(Task markedTask)
		{
			if (markedTask == null)
			{
				ODMessageBox.Show("Please select a valid task.");

				return;
			}

			markedTask.IsUnread = TaskUnreads.IsUnread(Security.CurrentUser.Id, markedTask);
		}

		private void MoveListIntoAncestor(TaskList newList, long oldListParent)
		{
			if (_wasCut)
			{
				// If the TaskList was cut, move direct children of the list "up" one in the hierarchy and then update
				var childTaskLists = TaskLists.GetChildren(newList.Id);

				foreach (var taskList in childTaskLists)
				{
					taskList.ParentId = oldListParent;

					TaskLists.Update(taskList);
				}
				TaskLists.Update(newList);
			}
			else
			{
				// Just insert a new TaskList if it was copied.
				TaskLists.Insert(newList);
			}
		}

		private void MoveTaskList(TaskList newTaskList)
		{
            TaskLists.Update(newTaskList);

			foreach (var taskList in TaskLists.GetChildren(newTaskList.Id))
			{ 
				taskList.ParentId = newTaskList.Id;

				MoveTaskList(taskList);
			}
		}

		///<summary>Only used for dated task lists. Should NOT be used for regular task lists, puts too much strain on DB with large amount of tasks.
		///A recursive function that duplicates an entire existing TaskList.  
		///For the initial loop, make changes to the original taskList before passing it in.  
		///That way, Date and type are only set in initial loop.  All children preserve original dates and types. 
		///The isRepeating value will be applied in all loops.  Also, make sure to change the parent num to the new one before calling this function.
		///The taskListNum will always change, because we are inserting new record into database. </summary>
		private void DuplicateExistingList(TaskList newTaskList, bool isInMainOrUser)
		{
			if (_wasCut) //Not making a new TaskList, just moving an old one
			{ 
				TaskLists.Update(newTaskList);
			}
			else //copied -- We are making a new TaskList, we're keeping the old one as well
			{
				TaskLists.Insert(newTaskList);
			}

			// Duplicate all child task lists
			foreach (var taskList in TaskLists.GetChildren(newTaskList.Id))
			{
				taskList.ParentId = newTaskList.Id;

				DuplicateExistingList(taskList, isInMainOrUser);
			}

			// Duplicate all child tasks
			foreach (var task in Tasks.GetByTaskList(newTaskList.Id, true))
			{ 
				//updates all the child tasks. If the task list was cut, then just update the child tasks' ancestors.
				if (_wasCut)
				{
				}
				else // Copied
				{
					task.TaskListId = newTaskList.Id;
					task.Repeat = false;
					task.RepeatTaskId = null;

					if (!isInMainOrUser)
					{
						task.RepeatDate = DateTime.MinValue;
						task.RepeatInterval = TaskRepeatInterval.Never;
					}

					if (!string.IsNullOrEmpty(task.ReminderGroupId))
					{
						// Any reminder tasks duplicated to another task list must have a brand new ReminderGroupId
						// so that they do not affect the original reminder task chain.
						Tasks.SetReminderGroupId(task);
					}

					foreach (var taskNote in TaskNotes.GetForTask(task.Id))
					{
						taskNote.TaskId = task.Id;

						TaskNotes.Insert(taskNote);
						TaskNotes.Update(taskNote);
					}
				}
			}
		}

		private void Delete_Clicked()
		{
			if (_clickedI < taskLists.Count)
			{//is list
				TaskList taskListToDelete = taskLists[_clickedI];

				//check to make sure the list is empty.  Do not filter tasks so we don't try to delete a list that still has tasks.
				var tsks = Tasks.GetByTaskList(taskListToDelete.Id, true).ToList();
				var tsklsts = TaskLists.GetChildren(taskListToDelete.Id).ToList();


				int countHiddenTasks = tsklsts.Sum(x => x.NewTaskCount) + tsks.Count - taskListToDelete.NewTaskCount;

				if (tsks.Count > 0 || tsklsts.Count > 0)
				{
					ODMessageBox.Show(
						"Not allowed to delete a list unless it's empty.  This task list contains:\r\n"
						+ tsks.FindAll(x => string.IsNullOrEmpty(x.ReminderGroupId)).Count + " normal tasks\r\n"
						+ tsks.FindAll(x => !string.IsNullOrEmpty(x.ReminderGroupId)).Count + " reminder tasks\r\n"
						+ countHiddenTasks + " filtered tasks" + "\r\n"
						+ tsklsts.Count + " task lists");
					return;
				}

				if (TaskLists.GetMailboxUserNum(taskListToDelete.Id) != 0)
				{
                    ODMessageBox.Show("Not allowed to delete task list because it is attached to a user inbox.");

					return;
				}

				if (!MsgBox.Show(MsgBoxButtons.YesNo, "Delete this empty list?"))
				{
					return;
				}

				TaskLists.Delete(taskListToDelete);
				long signalNum = Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, taskListToDelete.ParentId.Value);//Signal for source tasklist.
				//RefillLocalTaskGrids(taskListToDelete, new List<long>() { signalNum }, false);//No db calls.
			}
			else
			{//Is task
			 //This security logic should match FormTaskEdit for when we enable the delete button.
				bool isTaskForCurUser = true;
				if (_clickedTask.UserId != Security.CurrentUser.Id)
				{//current user didn't write this task, so block them.
					isTaskForCurUser = false;//Delete will only be enabled if the user has the TaskEdit and TaskNoteEdit permissions.
				}
				if (_clickedTask.TaskListId != Security.CurrentUser.InboxTaskListId)
				{//the task is not in the logged-in user's inbox
					isTaskForCurUser = false;//Delete will only be enabled if the user has the TaskEdit and TaskNoteEdit permissions.
				}
				if (isTaskForCurUser)
				{
					List<TaskNote> listTaskNotes = TaskNotes.GetForTask(_clickedTask.Id);//so we can check so see if other users have added notes
					for (int i = 0; i < listTaskNotes.Count; i++)
					{
						if (Security.CurrentUser.Id != listTaskNotes[i].UserId)
						{
							isTaskForCurUser = false;
							break;
						}
					}
				}
				//Purposefully show a popup if the user is not authorized to delete this task.
				if (!isTaskForCurUser && (!Security.IsAuthorized(Permissions.TaskEdit) || !Security.IsAuthorized(Permissions.TaskNoteEdit)))
				{
					return;
				}

				//This logic should match FormTaskEdit.butDelete_Click()
				if (!MsgBox.Show(MsgBoxButtons.OKCancel, "Delete Task?"))
				{
					return;
				}

				if (Tasks.GetOne(_clickedTask.Id) == null)
				{
					ODMessageBox.Show("Task already deleted.");
					return;
				}

				if (_clickedTask.TaskListId == 0)
				{
					Tasks.TaskEditCreateLog("Deleted task", _clickedTask);
				}
				else
				{
					string logText = "Deleted task from tasklist";

					TaskList tList = TaskLists.GetOne(_clickedTask.TaskListId);
					if (tList != null)
					{
						logText += " " + tList.Description;
					}
					else
					{
						logText += ". Task list no longer exists";
					}
					logText += ".";
					Tasks.TaskEditCreateLog(logText, _clickedTask);
				}

				Tasks.Delete(_clickedTask.Id);//always do it this way to clean up all four tables
				List<long> listSignalNums = new List<long>();
				listSignalNums.Add(Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, _clickedTask.TaskListId));//Signal for source tasklist.
				listSignalNums.Add(Signalods.SetInvalid(InvalidType.Task, KeyType.Task, _clickedTask.Id));//Signal for current task.

                //RefillLocalTaskGrids(_clickedTask, _listTaskNotes.FindAll(x => x.TaskId == _clickedTask.Id), listSignalNums, false);
                TaskHistory taskHistory = new TaskHistory(_clickedTask)
                {
                    UserId = Security.CurrentUser.Id
                };
                TaskHists.Insert(taskHistory);

				SecurityLogs.MakeLogEntry(Permissions.TaskEdit, 0, "Task " + _clickedTask.Id + " deleted", 0);
			}
		}

		/// <summary>
		/// A recursive function that deletes the specified list and all children.
		/// </summary>
		private void DeleteEntireList(TaskList taskList)
		{
			TaskLists.GetChildren(taskList.Id).ForEach(tl => DeleteEntireList(tl));

			Tasks.GetByTaskList(taskList.Id, true).ForEach(t =>
			{
				Tasks.Delete(t);

				SecurityLogs.MakeLogEntry(Permissions.TaskEdit, 0, "Task " + t.Id + " deleted", 0);
			});

			try
			{
				TaskLists.Delete(taskList);
			}
			catch (Exception e)
			{
				ODMessageBox.Show(e.Message);
			}
		}

		/// <summary>
		/// The indexing logic here could be improved to be easier to read, by modifying the fill grid to save column indexes into class-wide private varaibles.
		/// This way we will have access to the index without performing any logic.
		/// Additionally, each variable could be set to -1 when the column is not present.
		/// </summary>
		private void gridMain_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (e.Col == 0)
			{//check box column
			 //no longer allow double click on checkbox, because it's annoying.
				return;
			}
			if (e.Row >= taskLists.Count)
			{//is task
				if (IsInvalidTaskRow(e.Row))
				{
					return; //could happen if the task list refreshed while the double-click was happening.
				}
				//It's important to grab the task directly from the db because the status in this list is fake, being the "unread" status instead.
				Task task = Tasks.GetOne(tasks[e.Row - taskLists.Count].Id);
				if (task == null)
				{//Task was deleted or moved.
					return;
				}

				FormTaskEdit FormT = new FormTaskEdit(task);
				FormT.Show();//non-modal
			}
		}

		/// <summary>
		/// Necessary to use this handler to set _clickedI before menuEdit_Popup.
		/// This is due to the order MouseDown vs CellClick events fire.
		/// Only using CellClick to set these variables resulted in stale values in menuEdit_Popup.
		/// </summary>
		private void gridMain_MouseDown(object sender, MouseEventArgs e)
		{
			SetClickedIAndTask(e);
		}

		private void gridMain_CellClick(object sender, ODGridClickEventArgs e)
		{
		}

		/// <summary>
		/// Helper function to centralize _clickedI and _clickedTask logic.
		/// </summary>
		private void SetClickedIAndTask(object e)
		{
			if (e is ODGridClickEventArgs args)
			{
				_clickedI = args.Row;
			}
			else if (e is MouseEventArgs mouseEventArgs)
			{
				_clickedI = tasksGrid.PointToRow(mouseEventArgs.Y);
			}

			if (_clickedI == -1) return;

			if (_clickedI >= tasksGrid.ListGridRows.Count)
			{//Grid refreshed mid-click and _clickedI is no longer valid.
				_clickedI = -1;
				_clickedTask = null;
				SetMenusEnabled();
				return;
			}

			if (tasksGrid.ListGridRows[_clickedI].Tag is Task)
			{
				_clickedTask = (Task)tasksGrid.ListGridRows[_clickedI].Tag;//Task lists cause _clickedTask to be null
			}
			else
			{
				_clickedTask = null;
			}
		}

		private void menuEdit_Popup(object sender, EventArgs e)
		{
			SetMenusEnabled();
		}

		private void SetMenusEnabled()
		{
			//Done----------------------------------
			if (tasksGrid.SelectedIndices.Length == 0 || _clickedI < taskLists.Count)
			{//or a tasklist selected
				menuItemDone.Enabled = false;
			}
			else
			{
				menuItemDone.Enabled = true;
			}

			//Edit,Cut,Copy,Delete-------------------------
			if (tasksGrid.SelectedIndices.Length == 0)
			{
				menuItemEdit.Enabled = false;
				menuItemCut.Enabled = false;
				menuItemCopy.Enabled = false;
				menuItemDelete.Enabled = false;
			}
			else
			{
				menuItemEdit.Enabled = true;
				menuItemCut.Enabled = true;
				if (_clickedI < taskLists.Count)
				{//Is a tasklist
					menuItemCopy.Enabled = false;//We don't want users to copy tasklists, only move them by cut.
				}
				else
				{
					menuItemCopy.Enabled = true;
				}
				menuItemDelete.Enabled = true;
			}

			menuItemPriority.MenuItems.Clear();
			//SendToMe/GoTo/Task Priority/DeleteTaskTaken---------------------------------------------------------------
			if (tasksGrid.SelectedIndices.Length > 0 && _clickedI >= taskLists.Count)
			{//is task
			 //The clicked task was removed from _listTasks, could happen between FillGrid(), mouse click, and now
				if (IsInvalidTaskRow(_clickedI))
				{
					IgnoreTaskClick();
					return;
				}
				Task task = tasks[_clickedI - taskLists.Count];
				//if (task.ObjectType == TaskObjectType.None)
				//{
				//	menuItemGoto.Enabled = false;
				//}
				//else
				//{
				//	menuItemGoto.Enabled = true;
				//}
				menuDeleteTaken.Visible = false;//Without this, HQ users without Permissions.TaskEdit still see this disabled item.
				menuItemMarkRead.Enabled = true;
				menuItemSendToMe.Enabled = true;
				//Check if task has patient attached
				//if (task.ObjectType == TaskObjectType.Patient)
				//{
				//	menuItemSendAndGoto.Enabled = true;
				//}
				//else
				//{
				//	menuItemSendAndGoto.Enabled = false;
				//}
				if (Defs.GetDefsForCategory(DefCat.TaskPriorities, true).Count == 0)
				{
					menuItemPriority.Enabled = false;
				}
				else
				{
					menuItemPriority.Enabled = true;
					Def[] defs = Defs.GetDefsForCategory(DefCat.TaskPriorities, true).ToArray();
					foreach (Def def in defs)
					{
						MenuItem item = menuItemPriority.MenuItems.Add(def.ItemName);
						item.Click += (sender, e) => menuTaskPriority_Click(task, def);
					}
				}
			}
			else
			{
				menuItemGoto.Enabled = false;//not a task
				menuItemSendToMe.Enabled = false;
				menuItemSendAndGoto.Enabled = false;
				menuItemPriority.Enabled = false;
				menuItemMarkRead.Enabled = false;
				menuDeleteTaken.Enabled = false;
			}
			//Archived/Unarchived-------------------------------------------------------------
			menuArchive.Visible = false;
			menuUnarchive.Visible = false;

			if (_clickedI < 0)
			{//Not clicked on any row
				menuItemDone.Enabled = false;
				menuItemEdit.Enabled = false;
				menuItemCut.Enabled = false;
				menuItemCopy.Enabled = false;
				//menuItemPaste.Enabled=false;//Don't disable paste because this one makes sense for user to do.
				menuItemDelete.Enabled = false;
				menuItemSubscribe.Enabled = false;
				menuItemUnsubscribe.Enabled = false;
				menuItemSendToMe.Enabled = false;
				menuItemGoto.Enabled = false;
				menuItemPriority.Enabled = false;
				menuItemMarkRead.Enabled = false;
				return;
			}
		}

		private bool IsInvalidTaskRow(int row)
		{//Index out of range
			return (row - taskLists.Count < 0 || row - taskLists.Count >= tasks.Count);
		}

		private void IgnoreTaskClick()
		{
			tasksGrid.SetSelected(_clickedI, false);

			_clickedI = -1;

			foreach (MenuItem menuItem in tasksGrid.ContextMenu.MenuItems)
			{
				menuItem.Enabled = false;
			}

			Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, _clickedTask.TaskListId);

			FillGrid();
		}

		private void OnSubscribe_Click()
		{
			var taskList = taskLists[_clickedI];
			var taskListId = taskList.Id;

			if (!subscribedTaskListIds.Contains(taskListId))
            {
				TaskSubscriptions.Subscribe(taskListId);

				lock (subscribedTaskListIds)
                {
					subscribedTaskListIds.Add(taskListId);
                }
            }
            else
            {
				ODMessageBox.Show("User already subscribed.");

				return;
			}

            ODMessageBox.Show("Done");

            RefillLocalTaskGrids(taskList, null);
        }

		private void OnUnsubscribe_Click()
		{
			var taskList = taskLists[_clickedI];
			var taskListId = taskList.Id;

			TaskSubscriptions.Unsubscribe(taskListId);

			lock (subscribedTaskListIds)
            {
				subscribedTaskListIds.Remove(taskListId);
            }

			RefillLocalTaskGrids(taskList, null);
		}

		private void menuItemDone_Click(object sender, EventArgs e)
		{
			Done_Clicked();
		}

		private void menuItemEdit_Click(object sender, EventArgs e)
		{
			Edit_Clicked();
		}

		private void menuItemCut_Click(object sender, EventArgs e)
		{
			Cut_Clicked();
		}

		private void menuItemCopy_Click(object sender, EventArgs e)
		{
			Copy_Clicked();
		}

		private void menuItemPaste_Click(object sender, EventArgs e)
		{
			Paste_Clicked();
		}

		private void menuItemDelete_Click(object sender, EventArgs e)
		{
			Delete_Clicked();
		}

		private void menuItemSubscribe_Click(object sender, EventArgs e)
		{
			OnSubscribe_Click();
		}

		private void menuItemUnsubscribe_Click(object sender, EventArgs e)
		{
			OnUnsubscribe_Click();
		}

		private void menuItemSendToMe_Click(object sender, EventArgs e)
		{
			SendToMe_Clicked();
		}

		private void menuItemSendAndGoto_Click(object sender, EventArgs e)
		{
			SendToMeAndGoto_Clicked();
		}

		private void menuItemGoto_Click(object sender, System.EventArgs e)
		{
			Goto_Clicked();
		}

		private void menuItemMarkRead_Click(object sender, EventArgs e)
		{
			MarkRead(_clickedTask);
		}

		private void menuDeleteTaken_Click(object sender, EventArgs e)
		{
		}

		private void menuTaskPriority_Click(Task task, Def priorityDef)
		{
			Task taskNew = task.Copy();
			taskNew.PriorityId = priorityDef.DefNum;

			try
			{
				Tasks.Update(taskNew, task);

				TaskHists.Insert(new TaskHistory(task));

				long signalId = Signalods.SetInvalid(InvalidType.Task, KeyType.Task, taskNew.Id);

				//RefillLocalTaskGrids(taskNew, _listTaskNotes.FindAll(x => x.TaskId == taskNew.Id), new List<long>() { signalNum });
			}
			catch (Exception exception)
			{
                ODMessageBox.Show(exception.Message);
			}
		}

		private void menuArchive_Click(object sender, EventArgs e)
		{
			//Will not get here unless clicked index is an unarchived task list
			TaskLists.Archive(taskLists[_clickedI]);
			long signalNum = Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, taskLists[_clickedI].ParentId.Value);//Signal for source parent tasklist.
			RefillLocalTaskGrids(taskLists[_clickedI], new List<long>() { signalNum });//No db call.
		}

		private void menuUnarchive_Click(object sender, EventArgs e)
		{
			//Will not get here unless clicked index is an archived task list
			TaskLists.Unarchive(taskLists[_clickedI]);

			long signalId = Signalods.SetInvalid(InvalidType.TaskList, KeyType.Undefined, taskLists[_clickedI].ParentId.Value);
			//Signal for source parent tasklist.
			
			RefillLocalTaskGrids(taskLists[_clickedI], new List<long>() { signalId });
		}


		/// <summary>
		/// Gets the selected task list.
		/// </summary>
		public TaskList SelectedTaskList 
			=> taskListsTree.SelectedNode?.Tag as TaskList;

		/// <summary>
		/// Gets the selected task.
		/// </summary>
		public Task SelectedTask
			=> tasksGrid.SelectedTag<Task>();

		public delegate void FillGridEventHandler(object sender, EventArgs e);
    }
}
