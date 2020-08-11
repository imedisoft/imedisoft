using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests.Userods_Tests
{
	[TestClass]
	public class UserodsTests : TestBase
	{
		///<summary>This method will get invoked after every single test.</summary>
		[TestCleanup]
		public void Cleanup()
		{
			CredentialsFailedAfterLoginEvent.Fired -= CredentialsFailedAfterLoginEvent_Fired1;
			Security.CurrentUser = Userods.GetFirstOrDefault(x => x.UserName == UnitTestUserName);
		}

		private void CredentialsFailedAfterLoginEvent_Fired1(ODEventArgs e)
		{
			//If we don't subscribe to this event then the failed event will keep firing over and over.
			throw new Exception("Incorrect username and password");
		}

		//Test #2 Attempt 6 times in total and validate that count increases to 5 and then get message that account has been locked. 
		[TestMethod]
		public void Userods_CheckUserAndPassword_LockoutAfterUserHasLoggedInButPasswordIsNotCorrectAfter5Attempts()
		{
			//First, setup the test scenario.
			long group1 = UserGroupT.CreateUserGroup("usergroup1");
			bool isAccountLocked = false;
			Userod myUser = UserodT.CreateUser(MethodBase.GetCurrentMethod().Name + DateTime.Now.Ticks, "reallystrongpassword", userGroupNumbers: new List<long>() { group1 });
			//Make 5 bad password attempts
			for (int i = 1; i < 6; i++)
			{
				ODException.SwallowAnyException(() =>
				{
					Userods.CheckUserAndPassword(myUser.UserName, "passwordguess#" + i, false);
				});
			}
			try
			{
				//the 6th bad attempt should kick us with a message saying that our account has been locked.
				Userods.CheckUserAndPassword(myUser.UserName, "passwordguess#6", false);
			}
			catch (Exception e)
			{
				if (e.Message.Contains("Account has been locked due to failed log in attempts"))
				{
					isAccountLocked = true;
				}
			}
			//Get our updated user from the DB. 
			myUser = Userods.GetUserByNameNoCache(myUser.UserName);
			//Assert that we got to 5 failed attempts and that the account has been locked. 
			Assert.AreEqual(5, myUser.FailedAttempts);
			Assert.AreEqual(true, isAccountLocked);
		}

		/// <summary>
		/// This test is to make sure that all values are being copied according to specs of F15854
		/// A demo user and clinic is being created and then a copy of said user is made
		/// </summary>
		[TestMethod]
		public void Userods_CopyUser_HappyPath()
		{
			Userod user = UserodT.CreateUser();
			Clinic clinic = ClinicT.CreateClinic();
			UserClinics.Insert(new UserClinic(clinic.ClinicNum, user.Id));
			AlertSubs.Insert(new AlertSub(user.Id, clinic.ClinicNum, 1));
			UserPreference.Set(user.Id, UserPreferenceName.ClinicLast, clinic.ClinicNum);
			//Setup user
			//Fields given by method caller
			string passwordHashNotExpected = user.PasswordHash;
			string password = "Asdf1234!@#$";
			string passwordHash = Password.Hash(password);
			bool isPasswordStrong = string.IsNullOrEmpty(Userods.IsPasswordStrong(password));
			string copiedUserName = user.UserName + "(Copy)";
			//Fields directly copied
			bool clinicIsRestrictedExpected = user.ClinicIsRestricted;
			long clinicNumExpected = user.ClinicId;
			List<UserGroupAttach> listAttachesExpected = UserGroupAttaches.GetForUser(user.Id);//Mimics Userods.Insert(...)
			List<UserClinic> listUserClinicsExpected = UserClinics.GetForUser(user.Id);//Mimics 
			List<AlertSub> listAlertSubsExpected = AlertSubs.GetAllForUser(user.Id);
			//Copy User
			long copiedUserNum = Userods.CopyUser(user, passwordHash, isPasswordStrong, copiedUserName, isForCemt: false).Id;
			Cache.Refresh(InvalidType.AllLocal);
			Userod copy = Userods.GetUser(copiedUserNum);
			//Assert
			//Fields given by method caller
			Assert.AreEqual(copiedUserName, copy.UserName);
			Assert.AreEqual(isPasswordStrong, copy.PasswordIsStrong);
			Assert.AreEqual(passwordHash.ToString(), copy.PasswordHash);
			Assert.AreNotEqual(passwordHashNotExpected, copy.PasswordHash);//Source user's password should not have been copied.
																				  //Fields directly copied
			Assert.AreEqual(clinicIsRestrictedExpected, copy.ClinicIsRestricted);
			Assert.AreEqual(clinicNumExpected, copy.ClinicId);
			List<UserGroupAttach> listAttaches = UserGroupAttaches.GetForUser(copy.Id);
			Assert.AreEqual(listAttachesExpected.Count, listAttaches.Count);
			foreach (UserGroupAttach expected in listAttachesExpected)
			{
				Assert.IsTrue(listAttaches.Exists(x => x.UserGroupNum == expected.UserGroupNum));
			}
			List<UserClinic> listUserClinics = UserClinics.GetForUser(copy.Id);
			Assert.AreEqual(listUserClinicsExpected.Count, listUserClinics.Count);
			foreach (UserClinic expected in listUserClinicsExpected)
			{
				Assert.IsTrue(listUserClinics.Exists(x => x.ClinicId == expected.ClinicId));
			}
			List<AlertSub> listAlertSubs = AlertSubs.GetAllForUser(copy.Id);
			Assert.AreEqual(listAlertSubsExpected.Count, listAlertSubs.Count);
			foreach (AlertSub expected in listAlertSubsExpected)
			{
				Assert.IsTrue(listAlertSubs.Exists(x => x.ClinicNum == expected.ClinicNum && x.Type == expected.Type
					&& x.AlertCategoryNum == expected.AlertCategoryNum));
			}
			//Fields not copied (set to default)
			Assert.AreEqual(0, copy.EmployeeId);
			Assert.AreEqual(0, copy.ProviderId);
			Assert.AreEqual(false, copy.IsHidden);
			Assert.AreEqual(0, copy.InboxTaskListId);
			Assert.AreEqual(0, copy.AnesthProvType);
			Assert.AreEqual(false, copy.DefaultHidePopups);
			Assert.AreEqual(0, copy.UserNumCEMT);
			Assert.AreEqual(DateTime.MinValue, copy.FailedLoginDateTime);
			Assert.AreEqual(0, copy.FailedAttempts);
			Assert.AreEqual("", copy.DomainUser);
			Assert.AreEqual("", copy.MobileWebPin);
			Assert.AreEqual(0, copy.MobileWebPinFailedAttempts);
			Assert.AreEqual(DateTime.MinValue, copy.DateTLastLogin);
			//List<UserOdPref> listUserOdPrefs = UserOdPrefT.GetByUser(copy.Id);
			//Assert.AreEqual(0, listUserOdPrefs.Count);
		}

		/// <summary>
		/// Tests if GetUniqueUserName called in CopyUser is performing as expected.
		/// </summary>
		[TestMethod]
		public void Userods_CopyUser_NameInUse()
		{
			Userod user = UserodT.CreateUser();
			//Setup user
			//Fields given by method caller
			Userods.CopyUser(user, user.PasswordHash, false, isForCemt: false);//First copy
			long copiedUserNum2 = Userods.CopyUser(user, user.PasswordHash, false, isForCemt: false).Id;//Second copy
			Cache.Refresh(InvalidType.Security);
			Userod copy = Userods.GetUser(copiedUserNum2);
			Assert.AreEqual(user.UserName + "(Copy)(2)", copy.UserName);
		}
	}
}
