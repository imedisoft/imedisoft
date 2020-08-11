using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests.TaskSubscriptions_Tests
{
	[TestClass]
	public class TaskSubscriptionsTests : TestBase
	{
		private long userId = 1;
		private TaskList taskList;
		private TaskList taskListChild;
		private TaskList taskListGrandchild;

		[ClassInitialize]
		public static void SetupClass(TestContext testContext)
		{
		}

		[TestInitialize]
		public void SetupTest()
		{
			TaskListT.ClearTaskListTable();

			TaskT.ClearTaskTable();

			TaskSubscriptionT.ClearTaskSubscriptionTable();

			taskList = 
				TaskListT.CreateTaskList(
					description: "TaskList1");

			taskListChild = 
				TaskListT.CreateTaskList(
					description: "TaskList1Child", 
					parentId: taskList.Id, 
					parentDescription: taskList.Description);
			
			taskListGrandchild = 
				TaskListT.CreateTaskList(
					description: "TaskList1Grandchild", 
					parentId: taskListChild.Id,
					parentDescription: taskListChild.Description);
		}
	}
}
