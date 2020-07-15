using CodeBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnitTestsCore;

namespace UnitTests.SecurityLogs_Tests
{
    [TestClass]
	public class SecurityLogsTests : TestBase
	{
		[ClassInitialize]
		public static void SetupClass(TestContext testContext)
		{
		}

		[TestInitialize]
		public void SetupTest()
		{
		}

		[TestCleanup]
		public void TearDownTest()
		{
		}

		[ClassCleanup]
		public static void TearDownClass()
		{
		}

		[TestMethod]
		public void SecurityLogs_MakeLogEntry_DuplicateEntry()
		{
			Patient patient = PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			//There are lots of bug submissions with exception text like "Duplicate entry 'XXXXX' for key 'PRIMARY'".
			//OpenDentBusiness.SecurityLogs.MakeLogEntry() seems to be the common theme for most of the submissions.
			//Loop as fast as we can and insert 200 security logs trying to get a duplicate entry exception.
			for (int i = 0; i < 200; i++)
			{
				try
				{
					SecurityLogs.MakeLogEntry(Permissions.Accounting, patient.PatNum, "", 0, DateTime.Now.AddDays(-7));
				}
				catch (Exception ex)
				{
					Assert.Fail(ex.Message);
					break;
				}
			}
		}

		[TestMethod]
		public void SecurityLogs_MakeLogEntry_DuplicateEntryParallel()
		{
			Patient patient = PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);

			// There are lots of bug submissions with exception text like "Duplicate entry 'XXXXX' for key 'PRIMARY'".
			// OpenDentBusiness.SecurityLogs.MakeLogEntry() seems to be the common theme for most of the submissions.
			// Spawn parallel threads to insert 200 security logs trying to get a duplicate entry exception.
			List<Action> listActions = new List<Action>();
			for (int i = 0; i < 200; i++)
			{
				listActions.Add(() => SecurityLogs.MakeLogEntry(Permissions.Accounting, patient.PatNum, "", 0, DateTime.Now.AddDays(-7)));
			}

			// Parallel threads do not support Middle Tier mode when unit testing due to how we have to fake being both the client and the server.
			ODThread.RunParallel(listActions, exceptionHandler: (ex) =>
			{
				Assert.Fail(ex.Message);
			});
		}
	}
}
