﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests.IntegrationTests.UserControlTasks_Tests
{
	[TestClass]
	public class UserControlTasksTests : TestBase
	{
		//private TaskList _taskListParent;
		//private TaskList _taskListChild;
		//private TaskList _taskListGrandchild;
		//private OpenDentBusiness.Task _task;
		//private UserControlTasks _userControlTasksInstance;
		//private PrivateObject _userControlTasksAccessor;

		//[ClassInitialize]
		//public static void SetupClass(TestContext testContext)
		//{
		//	//Add anything here that you want to run once before the tests in this class run.
		//}

		//[TestInitialize]
		//public void SetupTest()
		//{
		//	TaskListT.ClearTaskListTable();
		//	TaskT.ClearTaskTable();
		//	TaskSubscriptionT.ClearTaskSubscriptionTable();
		//	SignalodT.ClearSignalodTable();
		//	_taskListParent = TaskListT.CreateTaskList(description: "TaskListParent");
		//	_taskListChild = TaskListT.CreateTaskList(description: "TaskListChild", parentId: _taskListParent.Id, parentDescription: _taskListParent.Description);
		//	_taskListGrandchild = TaskListT.CreateTaskList(description: "TaskListGrandchild", parentId: _taskListChild.Id,
		//		parentDescription: _taskListChild.Description);
		//	_task = TaskT.CreateTask(_taskListGrandchild.Id, descript: "Test Task", fromNum: Security.CurrentUser.Id);//Starts in _taskListGrandchild
		//	Security.CurrentUser.InboxTaskListId = _taskListParent.Id;//Set inbox for current user to _taskListParent.
		//	try
		//	{
		//		Userods.Update(Security.CurrentUser);
		//	}
		//	catch
		//	{
		//		Assert.Fail("Failed to update current user task list inbox.");//Error updating user.
		//	}
		//	_userControlTasksInstance = new UserControlTasks();
		//	_userControlTasksAccessor = new PrivateObject(_userControlTasksInstance);
		//	//Artificially set that we are viewing _listTaskListParent.
		//	_userControlTasksAccessor.SetField("_listTaskListTreeHistory", new List<TaskList>() { _taskListParent });
		//	_userControlTasksAccessor.SetField("_dictTaskLists", new Dictionary<long, TaskList>());
		//}

		//[TestCleanup]
		//public void TearDownTest()
		//{
		//	//Add anything here that you want to run after every test in this class.
		//}

		//[ClassCleanup]
		//public static void TearDownClass()
		//{
		//	//Add anything here that you want to run after all the tests in this class have been run.
		//}

		//[TestMethod]
		/////<summary>Context menu right-click and select the "Done" option.</summary>
		//public void UserControlTasks_Done_Clicked()
		//{
		//	_userControlTasksAccessor.SetField("_clickedTask", _task);//Directly set which task is clicked.
		//	_userControlTasksAccessor.Invoke("Done_Clicked");//Context menu "Done" option click.
		//	List<Signalod> listSignals = SignalodT.GetAllSignalods();
		//	OpenDentBusiness.Task taskDb = Tasks.GetOne(_task.Id);
		//	Assert.AreEqual(1, listSignals.Count);
		//	Assert.IsTrue(listSignals.Exists(x => x.InvalidType == InvalidType.Task && x.Name == KeyType.Task && x.InvalidForeignKey == _task.Id));
		//	Assert.AreEqual(OpenDentBusiness.TaskStatus.Done, taskDb.Status);
		//}

		//[TestMethod]
		/////<summary>Context menu right-click and select the "Paste" option with a cut TaskList on the clipboard.</summary>
		//public void UserControlTasks_Paste_Clicked_TaskList()
		//{
		//	_userControlTasksAccessor.SetField("_wasCut", true);//taskList is cut, not copied.
		//														//Destination task list is the root node (item 0) from _listTaskListTreeHistory which was initialized in SetupTest().
		//	_userControlTasksAccessor.SetField("_clipTaskList", _taskListGrandchild);//Directly set which taskList is cut.
		//	_userControlTasksAccessor.Invoke("Paste_Clicked");//Context menu "Paste" option click.
		//	List<Signalod> listSignals = SignalodT.GetAllSignalods();
		//	Assert.AreEqual(2, listSignals.Count);
		//	Assert.IsTrue(listSignals.Exists(x => x.InvalidType == InvalidType.TaskList && x.Name == KeyType.Undefined
		//		&& x.InvalidForeignKey == _taskListGrandchild.ParentId));//Signal for source taskList.
		//	Assert.IsTrue(listSignals.Exists(x => x.InvalidType == InvalidType.TaskList && x.Name == KeyType.Undefined
		//		&& x.InvalidForeignKey == _taskListParent.Id));//Signal for destination taskList.
		//	TaskList taskListDb = TaskLists.GetOne(_taskListGrandchild.Id);
		//	Assert.AreEqual(_taskListParent.Id, taskListDb.ParentId);//Db was properly updated with new Parent taskList.
		//}

		//[TestMethod]
		/////<summary>Context menu right-click and select the "Paste" option with a cut Task on the clipboard.</summary>
		//public void UserControlTasks_Paste_Clicked_Task_Cut()
		//{
		//	_userControlTasksAccessor.SetField("_wasCut", true);//task is cut, not copied.
		//														//Destination task list is the root node (item 0) from _listTaskListTreeHistory which was initialized in SetupTest().
		//	_userControlTasksAccessor.SetField("_clipTask", _task);//Directly set which task is cut.
		//	_userControlTasksAccessor.Invoke("Paste_Clicked");//Context menu "Paste" option click.
		//	List<Signalod> listSignals = SignalodT.GetAllSignalods();
		//	Assert.AreEqual(4, listSignals.Count);
		//	Assert.IsTrue(listSignals.Exists(x => x.InvalidType == InvalidType.TaskList && x.Name == KeyType.Undefined
		//		&& x.InvalidForeignKey == _taskListGrandchild.Id));//Signal for source taskList.
		//	Assert.IsTrue(listSignals.Exists(x => x.InvalidType == InvalidType.TaskList && x.Name == KeyType.Undefined
		//		&& x.InvalidForeignKey == _taskListParent.Id));//Signal for destination taskList.
		//	Assert.IsTrue(listSignals.Exists(x => x.InvalidType == InvalidType.TaskPopup && x.Name == KeyType.Task
		//		&& x.InvalidForeignKey == _task.Id));//Signal for Task Popup.
		//	OpenDentBusiness.Task taskDb = Tasks.GetOne(_task.Id);
		//	Assert.AreEqual(_taskListParent.Id, taskDb.TaskListId);//Db was properly updated with new taskListNum on _task.
		//}

		//[TestMethod]
		/////<summary>Context menu right-click and select the "Paste" option with a copied Task on the clipboard.</summary>
		//public void UserControlTasks_Paste_Clicked_Task_Copied()
		//{
		//	_userControlTasksAccessor.SetField("_wasCut", false);//task is copied.
		//														 //Destination task list is the root node (item 0) from _listTaskListTreeHistory which was initialized in SetupTest().
		//	_userControlTasksAccessor.SetField("_clipTask", _task);//Directly set which task is cut.
		//	_userControlTasksAccessor.Invoke("Paste_Clicked");//Context menu "Paste" option click.
		//	List<Signalod> listSignals = SignalodT.GetAllSignalods();
		//	Assert.AreEqual(3, listSignals.Count);
		//	Assert.IsTrue(!listSignals.Exists(x => x.InvalidType == InvalidType.TaskList && x.Name == KeyType.Undefined
		//		&& x.InvalidForeignKey == _taskListGrandchild.Id));//Should not have signal for source taskList.
		//	Assert.IsTrue(listSignals.Exists(x => x.InvalidType == InvalidType.TaskList && x.Name == KeyType.Undefined
		//		&& x.InvalidForeignKey == _taskListParent.Id));//Signal for destination taskList.
		//													   //Signal for Task Popup. Don't know the primary key for this.
		//	Assert.IsTrue(listSignals.Exists(x => x.InvalidType == InvalidType.TaskPopup && x.Name == KeyType.Task));
		//	long pastedTaskPK = listSignals.FirstOrDefault(x => x.InvalidType == InvalidType.TaskPopup)?.InvalidForeignKey ?? 0;//Only way to get new key is by reading the associated signal.
		//	OpenDentBusiness.Task taskCopiedDb = Tasks.GetOne(_task.Id);//Copied task.
		//	OpenDentBusiness.Task taskPastedDb = Tasks.GetOne(pastedTaskPK);//Pasted task.
		//	Assert.AreEqual(_taskListGrandchild.Id, taskCopiedDb.TaskListId);//Db did not change on copied task.
		//	Assert.AreEqual(_taskListParent.Id, taskPastedDb.TaskListId);//Db has pasted task in the correct tasklist.
		//}

		//[TestMethod]
		/////<summary>Context menu right-click and select the "Send to Me" option.</summary>
		//public void UserControlTasks_SendToMe_Clicked()
		//{
		//	_userControlTasksAccessor.SetField("_clickedTask", _task);
		//	_userControlTasksAccessor.Invoke("SendToMe_Clicked", false);
		//	List<Signalod> listSignals = SignalodT.GetAllSignalods();
		//	Assert.AreEqual(3, listSignals.Count);
		//	Assert.IsTrue(listSignals.Exists(x => x.InvalidType == InvalidType.TaskList && x.Name == KeyType.Undefined
		//		&& x.InvalidForeignKey == _taskListGrandchild.Id));//Signal for source taskList.
		//	Assert.IsTrue(listSignals.Exists(x => x.InvalidType == InvalidType.TaskList && x.Name == KeyType.Undefined
		//		&& x.InvalidForeignKey == _taskListParent.Id));//Signal for destination taskList.
		//	OpenDentBusiness.Task taskDb = Tasks.GetOne(_task.Id);
		//	Assert.AreEqual(taskDb.TaskListId, _taskListParent.Id);//Db was properly updated with new taskListNum on _task.
		//}

		//[TestMethod]
		/////<summary>Context menu right-click and select the "Mark Read" with TaskNewTrackedByUser preference turned off.</summary>
		//public void UserControlTasks_MarkRead()
		//{
		//	Prefs.Set(PrefName.TasksNewTrackedByUser, false);//TaskNewTrackedByUser=false;
		//	_userControlTasksAccessor.Invoke("MarkRead", _task);
		//	List<Signalod> listSignals = SignalodT.GetAllSignalods();
		//	Assert.AreEqual(1, listSignals.Count);
		//	Assert.IsTrue(listSignals.Exists(x => x.InvalidType == InvalidType.Task && x.Name == KeyType.Task
		//		&& x.InvalidForeignKey == _task.Id));//Signal for _task.
		//	OpenDentBusiness.Task taskDb = Tasks.GetOne(_task.Id);
		//	//Assert.AreEqual(OpenDentBusiness.TaskStatus.Viewed, taskDb.Status);
		//}

		//[TestMethod]
		/////<summary>Context menu right-click and select the "Mark Read" with TaskNewTrackedByUser preference turned on.</summary>
		//public void UserControlTasks_MarkRead_TasksNewTrackedByUser()
		//{
		//	Prefs.Set(PrefName.TasksNewTrackedByUser, true);//TaskNewTrackedByUser=true;
		//	TaskUnreads.SetUnread(Security.CurrentUser.Id, _task);//Set the task to unread for our user.
		//	_userControlTasksAccessor.Invoke("MarkRead", _task);//TaskNewTrackedByUser=false;
		//	List<Signalod> listSignals = SignalodT.GetAllSignalods();
		//	Assert.AreEqual(1, listSignals.Count);
		//	Assert.IsTrue(listSignals.Exists(x => x.InvalidType == InvalidType.Task && x.Name == KeyType.Task
		//		&& x.InvalidForeignKey == _task.Id));//Signal for _task.
		//	Assert.IsFalse(_task.IsUnread);
		//}

		//[TestMethod]
		/////<summary>Context menu right-click and drill down in the "Set Priority" option, selecting one of the priority options.</summary>
		//public void UserControlTasks_menuTaskPriority_Click()
		//{
		//	Def newDef = new Def() { DefNum = 5 };
		//	_userControlTasksAccessor.Invoke("menuTaskPriority_Click", _task, newDef);
		//	List<Signalod> listSignals = SignalodT.GetAllSignalods();
		//	Assert.AreEqual(1, listSignals.Count);
		//	Assert.IsTrue(listSignals.Exists(x => x.InvalidType == InvalidType.Task && x.Name == KeyType.Task
		//		&& x.InvalidForeignKey == _task.Id));//Signal for _task.
		//	OpenDentBusiness.Task taskDb = Tasks.GetOne(_task.Id);
		//	Assert.AreEqual(newDef.DefNum, taskDb.PriorityId);
		//}

		//[TestMethod]
		/////<summary>Correct TaskLists are returned when a parent TaskList is subscribed to in a parent->child->grandchild TaskList hierarchy.</summary>
		//public void UserControlTasks_GetSubscribedTaskLists()
		//{
		//	//Subscribe to _taskListParent. This will result in a subscription list of _taskListParent,_taskListChild,_taskListGrandchild
		//	TaskSubscriptionT.CreateTaskSubscription(Security.CurrentUser.Id, _taskListParent.Id);
		//	List<TaskList> listSubscribedTaskLists = UserControlTasks.GetSubscribedTaskLists(Security.CurrentUser.Id);
		//	List<TaskList> listExpectedSubscribedTaskLists = new List<TaskList>() { _taskListParent, _taskListChild, _taskListGrandchild };
		//	Assert.AreEqual(listExpectedSubscribedTaskLists.Count, listSubscribedTaskLists.Count);//Same number of subscriptions.
		//	foreach (TaskList taskListSubscribed in listSubscribedTaskLists)
		//	{
		//		//Every subscribed tasklist should be in the expected list of subscribed tasklists.
		//		Assert.IsTrue(listExpectedSubscribedTaskLists.Exists(x => x.Id == taskListSubscribed.Id));
		//	}
		//}

		//[TestMethod]
		/////<summary>List of signalNums tracking signals sent from this machine gets cleared on a full refresh.</summary>
		//public void UserControlTasks_RefreshMainLists_ListSentSignalsClear()
		//{
		//	long signalNum = 1;
		//	List<long> listSentSignalNums = new List<long>() { signalNum };
		//	//Adds a signalNum to the list of sent signalNums.
		//	UserControlTasks.RefillLocalTaskGrids(_task, listSentSignalNums);
		//	List<long> listTrackedSignalNums = (List<long>)_userControlTasksAccessor.GetField("_listSentTaskSignalNums");
		//	//Assert.AreEqual(listSentSignalNums.Count,listTrackedSignalNums.Count); Add back when local memory refresh has been re-implemented.
		//	//Assert.AreEqual(signalNum,listTrackedSignalNums[0]); Add back when local memory refresh has been re-implemented.
		//	long parent = 0;
		//	_userControlTasksAccessor.Invoke("RefreshMainLists", parent, DateTime.Today);//A full refresh should clear the list of sent signalNums.
		//	listTrackedSignalNums = (List<long>)_userControlTasksAccessor.GetField("_listSentTaskSignalNums");
		//	Assert.AreEqual(0, listTrackedSignalNums.Count);
		//}
	}
}
