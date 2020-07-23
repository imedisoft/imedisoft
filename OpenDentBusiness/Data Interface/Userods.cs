using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;
using ODCrypt;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Services;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using Imedisoft.Data.Cache;
using OpenDentBusiness.Crud;

namespace OpenDentBusiness
{
    ///<summary>(Users OD)</summary>
    public class Userods
	{
		#region Get Methods

		///<summary>Returns the UserNum of the first non-hidden admin user if they have no password set.
		///It is very important to order by UserName in order to preserve old behavior of only considering the first Admin user we come across.
		///This method does not simply return the first admin user with no password.  It is explicit in only considering the FIRST admin user.
		///Returns 0 if there are no admin users or the first admin user found has a password set.</summary>
		public static long GetFirstSecurityAdminUserNumNoPasswordNoCache()
		{
			//The query will order by UserName in order to preserve old behavior (mimics the cache).
			string command = @"SELECT userod.UserNum,CASE WHEN COALESCE(userod.Password,'')='' THEN 0 ELSE 1 END HasPassword 
				FROM userod
				INNER JOIN usergroupattach ON userod.UserNum=usergroupattach.UserNum
				INNER JOIN grouppermission ON usergroupattach.UserGroupNum=grouppermission.UserGroupNum 
				WHERE userod.IsHidden=0
				AND grouppermission.PermType=" + POut.Int((int)Permissions.SecurityAdmin) + @"
				GROUP BY userod.UserNum
				ORDER BY userod.UserName
				LIMIT 1";
			DataTable table = Database.ExecuteDataTable(command);
			long userNumAdminNoPass = 0;
			if (table != null && table.Rows.Count > 0 && table.Rows[0]["HasPassword"].ToString() == "0")
			{
				//The first admin user in the database does NOT have a password set.  Return their UserNum.
				userNumAdminNoPass = PIn.Long(table.Rows[0]["UserNum"].ToString());
			}
			return userNumAdminNoPass;
		}

		///<summary>Gets the corresponding user for the userNum passed in without using the cache.</summary>
		public static Userod GetUserNoCache(long userNum)
		{
			string command = "SELECT * FROM userod WHERE userod.UserNum=" + POut.Long(userNum);
			return Crud.UserodCrud.SelectOne(command);
		}

		///<summary>Gets the user name for the userNum passed in.  Returns empty string if not found in the database.</summary>
		public static string GetUserNameNoCache(long userNum)
		{
			string command = "SELECT userod.UserName FROM userod WHERE userod.UserNum=" + POut.Long(userNum);
			return Database.ExecuteString(command);
		}

		///<summary>Returns a list of non-hidden, non-CEMT user names.  Set hasOnlyCEMT to true if you only want non-hidden CEMT users.
		///Always returns all non-hidden users if PrefName.UserNameManualEntry is true.</summary>
		public static List<string> GetUserNamesNoCache(bool hasOnlyCEMT)
		{
			string command = $@"SELECT userod.UserName FROM userod 
				WHERE userod.IsHidden=0 
				{ (PrefC.GetBool(PrefName.UserNameManualEntry) ? " " : " AND userod.UserNumCEMT" + (hasOnlyCEMT ? "!=" : "=") + @"0 ") }
				ORDER BY userod.UserName";
			return Database.GetListString(command);
		}

		///<summary>Returns all non-hidden UserNums (key) and UserNames (value) associated with the domain user name passed in.
		///Returns an empty dictionary if no matches were found.</summary>
		public static SerializableDictionary<long, string> GetUsersByDomainUserNameNoCache(string domainUser)
		{
			string command = @"SELECT userod.UserNum, userod.UserName, userod.DomainUser 
				FROM userod 
				WHERE IsHidden=0";
			//Not sure how to do an InvariantCultureIgnoreCase via a query so doing it over in C# in order to preserve old behavior.
			return Database.ExecuteDataTable(command).Select()
				.Where(x => PIn.String(x["DomainUser"].ToString()).Equals(domainUser, StringComparison.InvariantCultureIgnoreCase))
				.ToSerializableDictionary(x => PIn.Long(x["UserNum"].ToString()), x => PIn.String(x["UserName"].ToString()));
		}

		#endregion

		#region Misc Methods

		///<summary>Returns true if at least one admin user is present within the database.  Otherwise; false.</summary>
		public static bool HasSecurityAdminUserNoCache()
		{
			string command = @"SELECT COUNT(*) FROM userod
				INNER JOIN usergroupattach ON userod.UserNum=usergroupattach.UserNum
				INNER JOIN grouppermission ON usergroupattach.UserGroupNum=grouppermission.UserGroupNum 
				WHERE userod.IsHidden=0
				AND grouppermission.PermType=" + POut.Int((int)Permissions.SecurityAdmin);
			return (Database.ExecuteString(command) != "0");
		}

		///<summary>Returns true if there are any users (including hidden) with a UserNumCEMT set.  Otherwise; false.</summary>
		public static bool HasUsersForCEMTNoCache()
		{
			string command = @"SELECT COUNT(*) FROM userod
				WHERE userod.UserNumCEMT > 0";
			return (Database.ExecuteString(command) != "0");
		}

		///<summary>Returns true if the user can sign notes. Uses the NotesProviderSignatureOnly preference to validate.</summary>
		public static bool CanUserSignNote(Userod user = null)
		{
			//No need to check RemotingRole; no call to db.
			Userod userSig = user == null ? Security.CurUser : user;
			if (PrefC.GetBool(PrefName.NotesProviderSignatureOnly) && userSig.ProvNum == 0)
			{
				return false;//Prefernce is on and our user is not a provider.
			}
			return true;//Either pref is off or it is on and user is a provider.
		}

		#endregion

        private class UserodsCache : ListCache<Userod>
        {
            protected override IEnumerable<Userod> Load()
                => UserodCrud.SelectMany("SELECT * FROM userod ORDER BY UserName");
        }

        private static readonly UserodsCache cache = new UserodsCache();

        public static void RefreshCache() 
			=> cache.Refresh();

        public static List<Userod> All => cache.GetAll();

        public static Userod GetFirstOrDefault(Predicate<Userod> predicate) 
			=> cache.FirstOrDefault(predicate);

		public static List<Userod> GetAll(bool isShort = false)
        {
            if (isShort)
            {
                return UserodCrud.SelectMany("SELECT * FROM userod WHERE IsHidden = 0 ORDER BY UserName").ToList();
            }

            return UserodCrud.SelectMany("SELECT * FROM userod ORDER BY UserName").ToList();
        }

		public static Userod GetUser(long userNum)
		{
            return GetFirstOrDefault(x => x.Id == userNum);
		}

		/// <summary>
		/// Returns a list of users from the list of usernums.
		/// </summary>
		public static List<Userod> GetUsers(List<long> listUserNums)
        {
            return cache.Find(user => listUserNums.Contains(user.Id)).ToList();
        }

		/// <summary>
		/// Returns a list of all non-hidden users.
		/// Set includeCEMT to true if you want CEMT users included.
		/// </summary>
		public static List<Userod> GetUsers(bool includeCEMT = false)
		{
            return cache.Find(user => !user.IsHidden && (includeCEMT || user.UserNumCEMT == 0)).ToList();
        }

		/// <summary>
		/// Returns a list of all non-hidden users.  Does not include CEMT users.
		/// </summary>
		public static List<Userod> GetUsersByClinic(long clinicNum)
        {
            return cache.Find(
                user => !user.IsHidden && (user.ClinicIsRestricted || user.ClinicNum == clinicNum)).ToList();
        }

        public static List<Userod> GetWhere(Predicate<Userod> predicate)
            => cache.Find(predicate).ToList();

		/// <summary>
		/// Returns a list of all users without using the local cache.
		/// Useful for multithreaded connections.
		/// </summary>
		public static List<Userod> GetUsersNoCache()
        {
            return UserodCrud.SelectMany("SELECT * FROM userod").ToList();
        }

		/// <summary>
		/// Returns a list of all CEMT users.
		/// </summary>
		public static List<Userod> GetUsersForCEMT()
        {
            return cache.Find(user => user.UserNumCEMT != 0).ToList();
        }

		///<summary>Returns null if not found.  Is not case sensitive.  isEcwTight isn't even used.</summary>
		public static Userod GetUserByName(string userName, bool isEcwTight)
		{
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => !x.IsHidden && x.UserName.ToLower() == userName.ToLower());
		}

		///<summary>Gets the first user with the matching userName passed in.  Not case sensitive.  Returns null if not found.
		///Does not use the cache to find a corresponding user with the passed in userName.  Every middle tier call passes through here.</summary>
		public static Userod GetUserByNameNoCache(string userName)
        {
            return UserodCrud.SelectMany("SELECT * FROM userod WHERE UserName='" + POut.String(userName) + "'")
                .FirstOrDefault(user => !user.IsHidden && user.UserName.ToLower() == userName.ToLower());
        }

		/// <summary>
		/// Returns null if not found.
		/// </summary>
		public static Userod GetUserByEmployeeNum(long employeeNum)
		{
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => x.EmployeeNum == employeeNum);
		}

		/// <summary>
		/// Returns all users that are associated to the employee passed in.
		/// Returns empty list if no matches found.
		/// </summary>
		public static List<Userod> GetUsersByEmployeeNum(long employeeNum)
		{
            return cache.Find(x => x.EmployeeNum == employeeNum).ToList();
		}

		/// <summary>
		/// Returns all users that are associated to the permission passed in.
		/// Returns empty list if no matches found.
		/// </summary>
		public static List<Userod> GetUsersByPermission(Permissions permission, bool showHidden)
        {
            return cache.Find(user =>
                (showHidden || !user.IsHidden) && GroupPermissions.HasPermission(user, permission, 0)).ToList();
        }

		/// <summary>
		/// Gets all non-hidden users that have an associated provider.
		/// </summary>
		public static List<Userod> GetUsersWithProviders()
		{
            return cache.Find(x => x.ProvNum != 0).ToList();
		}

		/// <summary>
		/// Returns all users associated to the provider passed in.
		/// Returns empty list if no matches found.
		/// </summary>
		public static List<Userod> GetUsersByProvNum(long provNum)
		{
            return cache.Find(x => x.ProvNum == provNum).ToList();
		}

		public static List<Userod> GetUsersByInbox(long taskListNum)
		{
            return cache.Find(x => x.TaskListInBox == taskListNum).ToList();
		}

		/// <summary>
		/// Returns all users selectable for the insurance verification list.
		/// Pass in an empty list to not filter by clinic.
		/// Set isAssigning to false to return only users who have an insurance already assigned.
		/// </summary>
		public static List<Userod> GetUsersForVerifyList(List<long> listClinicNums, bool isAssigning)
		{
            List<long> listUserNumsInInsVerify = InsVerifies.GetAllInsVerifyUserNums();
			List<long> listUserNumsInClinic = new List<long>();
			if (listClinicNums.Count > 0)
			{
				List<UserClinic> listUserClinics = new List<UserClinic>();
				for (int i = 0; i < listClinicNums.Count; i++)
				{
					listUserNumsInClinic.AddRange(UserClinics.GetForClinic(listClinicNums[i]).Select(y => y.UserId).Distinct().ToList());
				}
				listUserNumsInClinic.AddRange(GetUsers().FindAll(x => !x.ClinicIsRestricted).Select(x => x.Id).Distinct().ToList());//Always add unrestricted users into the list.
				listUserNumsInClinic = listUserNumsInClinic.Distinct().ToList();//Remove duplicates that could possibly be in the list.
				if (listUserNumsInClinic.Count > 0)
				{
					listUserNumsInInsVerify = listUserNumsInInsVerify.FindAll(x => listUserNumsInClinic.Contains(x));
				}
				listUserNumsInInsVerify.AddRange(GetUsers(listUserNumsInInsVerify).FindAll(x => !x.ClinicIsRestricted).Select(x => x.Id).Distinct().ToList());//Always add unrestricted users into the list.
				listUserNumsInInsVerify = listUserNumsInInsVerify.Distinct().ToList();
			}
			List<Userod> listUsersWithPerm = GetUsersByPermission(Permissions.InsPlanVerifyList, false);
			if (isAssigning)
			{
				if (listClinicNums.Count == 0)
				{
					return listUsersWithPerm;//Return unfiltered list of users with permission
				}
				//Don't limit user list to already assigned insurance verifications.
				return listUsersWithPerm.FindAll(x => listUserNumsInClinic.Contains(x.Id));//Return users with permission, limited by their clinics
			}
			return listUsersWithPerm.FindAll(x => listUserNumsInInsVerify.Contains(x.Id));//Return users limited by permission, clinic, and having an insurance already assigned.
		}

		/// <summary>
		/// Returns all non-hidden users associated with the domain user name passed in.
		/// Returns an empty list if no matches found.
		/// </summary>
		public static List<Userod> GetUsersByDomainUserName(string domainUser)
		{
			return cache.Find(x => x.DomainUser.Equals(domainUser, StringComparison.InvariantCultureIgnoreCase)).ToList();
		}

		/// <summary>
		/// This handles situations where we have a usernum, but not a user.
		/// And it handles usernum of zero.
		/// </summary>
		public static string GetName(long userNum)
		{
            return GetFirstOrDefault(x => x.Id == userNum)?.UserName ?? "";
        }

		/// <summary>
		/// Returns true if the user passed in is associated with a provider that has (or had) an EHR prov key.
		/// </summary>
		public static bool IsUserCpoe(Userod user)
		{
			//No need to check RemotingRole; no call to db.
			if (user == null)
			{
				return false;
			}
			Provider prov = Providers.GetProv(user.ProvNum);
			if (prov == null)
			{
				return false;
			}
			//Check to see if this provider has had a valid key at any point in history.
			return EhrProvKeys.HasProvHadKey(prov.LName, prov.FName);
		}

		///<summary>Searches the database for a corresponding user by username (not case sensitive).  Returns null is no match found.
		///Once a user has been found, if the number of failed log in attempts exceeds the limit an exception is thrown with a message to display to the 
		///user.  Then the hash of the plaintext password (if usingEcw is true, password needs to be hashed before passing into this method) is checked 
		///against the password hash that is currently in the database.  Once the plaintext password passed in is validated, this method will upgrade the 
		///hashing algorithm for the password (if necessary) and then returns the entire user object for the corresponding user found.  Throws exceptions 
		///with error message to display to the user if anything goes wrong.  Manipulates the appropriate log in failure columns in the db as 
		///needed.</summary>
		public static Userod CheckUserAndPassword(string username, string plaintext, bool isEcw)
		{
			return CheckUserAndPassword(username, plaintext, isEcw, true);
		}

		///<summary>Searches the database for a corresponding user by username (not case sensitive).  Returns null is no match found.
		///Once a user has been found, if the number of failed log in attempts exceeds the limit an exception is thrown with a message to display to the 
		///user.  Then the hash of the plaintext password (if usingEcw is true, password needs to be hashed before passing into this method) is checked 
		///against the password hash that is currently in the database.  Once the plaintext password passed in is validated, this method will upgrade the 
		///hashing algorithm for the password (if necessary) and then returns the entire user object for the corresponding user found.  Throws exceptions 
		///with error message to display to the user if anything goes wrong.  Manipulates the appropriate log in failure columns in the db as 
		///needed.  Null will be returned when hasExceptions is false and no matching user found, credentials are invalid, or account is locked.</summary>
		public static Userod CheckUserAndPassword(string username, string plaintext, bool isEcw, bool hasExceptions)
		{
			//Do not use the cache here because an administrator could have cleared the log in failure attempt columns for this user.
			//Also, middle tier calls this method every single time a process request comes to it.
			Userod userDb = GetUserByNameNoCache(username);
			if (userDb == null)
			{
				if (hasExceptions)
				{
					throw new ODException(Lans.g("Userods", "Invalid username or password."), ODException.ErrorCodes.CheckUserAndPasswordFailed);
				}
				return null;
			}
			DateTime dateTimeNowDb = MiscData.GetNowDateTime();
			//We found a user via matching just the username passed in.  Now we need to check to see if they have exceeded the log in failure attempts.
			//For now we are hardcoding a 5 minute delay when the user has failed to log in 5 times in a row.  
			//An admin user can reset the password or the failure attempt count for the user failing to log in via the Security window.
			if (userDb.DateTFail.Year > 1880 //The user has failed to log in recently
				&& dateTimeNowDb.Subtract(userDb.DateTFail) < TimeSpan.FromMinutes(5) //The last failure has been within the last 5 minutes.
				&& userDb.FailedAttempts >= 5) //The user failed 5 or more times.
			{
				if (hasExceptions)
				{
					throw new ApplicationException(Lans.g("Userods", "Account has been locked due to failed log in attempts."
						+ "\r\nCall your security admin to unlock your account or wait at least 5 minutes."));
				}
				return null;
			}
			bool isPasswordValid = Authentication.CheckPassword(userDb, plaintext, isEcw);
			Userod userNew = userDb.Copy();
			//If the last failed log in attempt was more than 5 minutes ago, reset the columns in the database so the user can try 5 more times.
			if (userDb.DateTFail.Year > 1880 && dateTimeNowDb.Subtract(userDb.DateTFail) > TimeSpan.FromMinutes(5))
			{
				userNew.FailedAttempts = 0;
				userNew.DateTFail = DateTime.MinValue;
			}
			if (!isPasswordValid)
			{
				userNew.DateTFail = dateTimeNowDb;
				userNew.FailedAttempts += 1;
			}
			//Synchronize the database with the results of the log in attempt above
			Crud.UserodCrud.Update(userNew, userDb);
			if (isPasswordValid)
			{
				//Upgrade the encryption for the password if this is not an eCW user (eCW uses md5) and the password is using an outdated hashing algorithm.
				if (!isEcw && !string.IsNullOrEmpty(plaintext) && userNew.LoginDetails.HashType != HashTypes.SHA3_512)
				{
					//Update the password to the default hash type which should be the most secure hashing algorithm possible.
					Authentication.UpdatePasswordUserod(userNew, plaintext, HashTypes.SHA3_512);
					//The above method is almost guaranteed to have changed the password for userNew so go back out the db and get the changes that were made.
					userNew = GetUserNoCache(userNew.Id);
				}
				return userNew;
			}
			else
			{//Password was not valid.
				if (hasExceptions)
				{
					throw new ODException(Lans.g("Userods", "Invalid username or password."), ODException.ErrorCodes.CheckUserAndPasswordFailed);
				}
				return null;
			}
		}

		///<summary>Updates all students/instructors to the specified user group.  Surround with try/catch because it can throw exceptions.</summary>
		public static void UpdateUserGroupsForDentalSchools(UserGroup userGroup, bool isInstructor)
		{
			string command;
			//Check if the user group that the students or instructors are trying to go to has the SecurityAdmin permission.
			if (!GroupPermissions.HasPermission(userGroup.Id, Permissions.SecurityAdmin, 0))
			{
				//We need to make sure that moving these users to the new user group does not eliminate all SecurityAdmin users in db.
				command = "SELECT COUNT(*) FROM usergroupattach "
					+ "INNER JOIN usergroup ON usergroupattach.UserGroupNum=usergroup.UserGroupNum "
					+ "INNER JOIN grouppermission ON grouppermission.UserGroupNum=usergroup.UserGroupNum "
					+ "WHERE usergroupattach.UserNum NOT IN "
					+ "(SELECT userod.UserNum FROM userod,provider "
						+ "WHERE userod.ProvNum=provider.ProvNum ";
				if (!isInstructor)
				{
					command += "AND provider.IsInstructor=" + POut.Bool(isInstructor) + " ";
					command += "AND provider.SchoolClassNum!=0) ";
				}
				else
				{
					command += "AND provider.IsInstructor=" + POut.Bool(isInstructor) + ") ";
				}
				command += "AND grouppermission.PermType=" + POut.Int((int)Permissions.SecurityAdmin) + " ";
				int lastAdmin = PIn.Int(Database.ExecuteString(command));
				if (lastAdmin == 0)
				{
					throw new Exception("Cannot move students or instructors to the new user group because it would leave no users with the SecurityAdmin permission.");
				}
			}
			command = "UPDATE userod INNER JOIN provider ON userod.ProvNum=provider.ProvNum "
					+ "SET UserGroupNum=" + POut.Long(userGroup.Id) + " "
					+ "WHERE provider.IsInstructor=" + POut.Bool(isInstructor);
			if (!isInstructor)
			{
				command += " AND provider.SchoolClassNum!=0";
			}
			Database.ExecuteNonQuery(command);
		}

		///<summary>Surround with try/catch because it can throw exceptions.</summary>
		public static void Update(Userod userod, List<long> listUserGroupNums = null)
		{
			Validate(false, userod, false, listUserGroupNums);
			Crud.UserodCrud.Update(userod);
			if (listUserGroupNums == null)
			{
				return;
			}
			UserGroupAttaches.SyncForUser(userod, listUserGroupNums);
		}

		///<summary>Update for CEMT only.  Used when updating Remote databases with information from the CEMT.  Because of potentially different primary keys we have to update based on UserNumCEMT.</summary>
		public static void UpdateCEMT(Userod userod)
		{
			//This should never happen, but is a failsafe to prevent the overwriting of all non-CEMT users in the remote database.
			if (userod.UserNumCEMT == 0)
			{
				return;
			}
			//Validate(false,userod,false);//Can't use this validate. it's for normal updating only.
			string command = "UPDATE userod SET "
				+ "UserName          = '" + POut.String(userod.UserName) + "', "
				+ "Password          = '" + POut.String(userod.Password) + "', "
				//+"UserGroupNum      =  "+POut.Long(userod.UserGroupNum)+", "//need to find primary key of remote user group
				+ "EmployeeNum       =  " + POut.Long(userod.EmployeeNum) + ", "
				+ "ClinicNum         =  " + POut.Long(userod.ClinicNum) + ", "
				+ "ProvNum           =  " + POut.Long(userod.ProvNum) + ", "
				+ "IsHidden          =  " + POut.Bool(userod.IsHidden) + ", "
				+ "TaskListInBox     =  " + POut.Long(userod.TaskListInBox) + ", "
				+ "AnesthProvType    =  " + POut.Int(userod.AnesthProvType) + ", "
				+ "DefaultHidePopups =  " + POut.Bool(userod.DefaultHidePopups) + ", "
				+ "PasswordIsStrong  =  " + POut.Bool(userod.PasswordIsStrong) + ", "
				+ "ClinicIsRestricted=  " + POut.Bool(userod.ClinicIsRestricted) + ", "
				+ "InboxHidePopups   =  " + POut.Bool(userod.InboxHidePopups) + " "
				+ "WHERE UserNumCEMT = " + POut.Long(userod.UserNumCEMT);
			Database.ExecuteNonQuery(command);
		}

		///<summary>DEPRICATED DO NOT USE. Use OpenDentBusiness.Authentication class instead.  For middle tier backward-compatability only.</summary>
		public static void UpdatePassword(Userod userod, string newPassHashed, bool isPasswordStrong)
		{
			//Before 18.3, we only used MD5
			UpdatePassword(userod, new PasswordContainer(HashTypes.MD5, "", newPassHashed), isPasswordStrong);
		}

		///<summary>Surround with try/catch because it can throw exceptions.
		///Same as Update(), only the Validate call skips checking duplicate names for hidden users.</summary>
		public static void UpdatePassword(Userod userod, PasswordContainer loginDetails, bool isPasswordStrong)
		{
			Userod userToUpdate = userod.Copy();
			userToUpdate.LoginDetails = loginDetails;
			userToUpdate.PasswordIsStrong = isPasswordStrong;
			List<UserGroup> listUserGroups = userToUpdate.GetGroups(); //do not include CEMT users.
			if (listUserGroups.Count < 1)
			{
				throw new Exception(Lans.g("Userods", "The current user must be in at least one user group."));
			}
			Validate(false, userToUpdate, true, listUserGroups.Select(x => x.Id).ToList());
			Crud.UserodCrud.Update(userToUpdate);
		}

		///<summary>Sets the TaskListInBox to 0 for any users that have this as their inbox.</summary>
		public static void DisassociateTaskListInBox(long taskListNum)
		{
			string command = "UPDATE userod SET TaskListInBox=0 WHERE TaskListInBox=" + POut.Long(taskListNum);
			Database.ExecuteNonQuery(command);
		}

		///<summary>A user must always have at least one associated userGroupAttach. Pass in the usergroup(s) that should be attached.
		///Surround with try/catch because it can throw exceptions.</summary>
		public static long Insert(Userod userod, List<long> listUserGroupNums, bool isForCEMT = false)
		{
			if (userod.IsHidden && UserGroups.IsAdminGroup(listUserGroupNums))
			{
				throw new Exception(Lans.g("Userods", "Admins cannot be hidden."));
			}
			Validate(true, userod, false, listUserGroupNums);
			long userNum = Crud.UserodCrud.Insert(userod);
			UserGroupAttaches.SyncForUser(userod, listUserGroupNums);
			if (isForCEMT)
			{
				userod.UserNumCEMT = userNum;
				Crud.UserodCrud.Update(userod);
			}
			return userNum;
		}

		public static long InsertNoCache(Userod userod)
		{
			return Crud.UserodCrud.Insert(userod);
		}

		///<summary>Surround with try/catch because it can throw exceptions.  
		///We don't really need to make this public, but it's required in order to follow the RemotingRole pattern.
		///listUserGroupNum can only be null when validating for an Update.</summary>
		public static void Validate(bool isNew, Userod user, bool excludeHiddenUsers, List<long> listUserGroupNum)
		{
			//should add a check that employeenum and provnum are not both set.
			//make sure username is not already taken
			string command;
			long excludeUserNum;
			if (isNew)
			{
				excludeUserNum = 0;
			}
			else
			{
				excludeUserNum = user.Id;//it's ok if the name matches the current username
			}
			//It doesn't matter if the UserName is already in use if the user being updated is going to be hidden.  This check will block them from unhiding duplicate users.
			if (!user.IsHidden)
			{//if the user is now not hidden
			 //CEMT users will not be visible from within Open Dental.  Therefore, make a different check so that we can know if the name
			 //the user typed in is a duplicate of a CEMT user.  In doing this, we are able to give a better message.
				if (!IsUserNameUnique(user.UserName, excludeUserNum, excludeHiddenUsers, true))
				{
					throw new ApplicationException(Lans.g("Userods", "UserName already in use by CEMT member."));
				}
				if (!IsUserNameUnique(user.UserName, excludeUserNum, excludeHiddenUsers))
				{
					//IsUserNameUnique doesn't care if it's a CEMT user or not.. It just gets a count based on username.
					throw new ApplicationException(Lans.g("Userods", "UserName already in use."));
				}
			}
			if (listUserGroupNum == null)
			{//Not validating UserGroup selections.
				return;
			}
			if (listUserGroupNum.Count < 1)
			{
				throw new ApplicationException(Lans.g("Userods", "The current user must be in at least one user group."));
			}
			//an admin user can never be hidden
			command = "SELECT COUNT(*) FROM grouppermission "
				+ "WHERE PermType='" + POut.Long((int)Permissions.SecurityAdmin) + "' "
				+ "AND UserGroupNum IN (" + string.Join(",", listUserGroupNum) + ") ";
			if (!isNew//Updating.
				&& Database.ExecuteString(command) == "0"//if this user would not have admin
				&& !IsSomeoneElseSecurityAdmin(user))//make sure someone else has admin
			{
				throw new ApplicationException(Lans.g("Users", "At least one user must have Security Admin permission."));
			}
			if (user.IsHidden//hidden 
				&& user.UserNumCEMT == 0//and non-CEMT
				&& Database.ExecuteString(command) != "0")//if this user is admin
			{
				throw new ApplicationException(Lans.g("Userods", "Admins cannot be hidden."));
			}
		}

		/// <summary>Returns true if there is at least one user part of the SecurityAdmin permission excluding the user passed in.</summary>
		public static bool IsSomeoneElseSecurityAdmin(Userod user)
		{
			string command = "SELECT COUNT(*) FROM userod "
				+ "INNER JOIN usergroupattach ON usergroupattach.UserNum=userod.UserNum "
				+ "INNER JOIN grouppermission ON usergroupattach.UserGroupNum=grouppermission.UserGroupNum "
				+ "WHERE grouppermission.PermType='" + POut.Long((int)Permissions.SecurityAdmin) + "'"
				+ " AND userod.IsHidden =0"
				+ " AND userod.UserNum != " + POut.Long(user.Id);
			if (Database.ExecuteString(command) == "0")
			{//there are no other users with this permission
				return false;
			}
			return true;
		}

		public static bool IsUserNameUnique(string username, long excludeUserNum, bool excludeHiddenUsers)
		{
			return IsUserNameUnique(username, excludeUserNum, excludeHiddenUsers, false);
		}

		///<summary>Supply 0 or -1 for the excludeUserNum to not exclude any.</summary>
		public static bool IsUserNameUnique(string username, long excludeUserNum, bool excludeHiddenUsers, bool searchCEMTUsers)
		{
			if (username == "")
			{
				return false;
			}
			string command = "SELECT COUNT(*) FROM userod WHERE ";
			//if(Programs.UsingEcwTight()){
			//	command+="BINARY ";//allows different usernames based on capitalization.//we no longer allow this
			//Does not need to be tested under Oracle because eCW users do not use Oracle.
			//}
			command += "UserName='" + POut.String(username) + "' "
				+ "AND UserNum !=" + POut.Long(excludeUserNum) + " ";
			if (excludeHiddenUsers)
			{
				command += "AND IsHidden=0 ";//not hidden
			}
			if (searchCEMTUsers)
			{
				command += "AND UserNumCEMT!=0";
			}
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows[0][0].ToString() == "0")
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Generates a unique username based on what is passed into it.
		/// Returns null if given userName can not be easily identified as unique.
		/// </summary>
		/// <param name="username">The username you are copying</param>
		/// <param name="excludeUserNum">The UserNum that is excluded when checking if a username is in use.</param>
		/// <param name="excludeHiddenUsers">Set to true to exclude hidden patients when checking if a username is in use, otherwise false</param>
		/// <param name="searchCEMTUsers">Set to true to include checking usernames that are associated to CEMT users.</param>
		/// <param name="uniqueUserName">When returning true this is set to a unique username, otherwise null.</param>
		/// <returns></returns>
		public static bool TryGetUniqueUsername(string username, long excludeUserNum, bool excludeHiddenUsers, bool searchCEMTUsers, out string uniqueUserName)
		{
			int attempt = 1;
			uniqueUserName = username;//Default to given username, will change if not unique.
			while (!IsUserNameUnique(uniqueUserName, excludeUserNum, excludeHiddenUsers, searchCEMTUsers))
			{
				if (attempt > 100)
				{
					uniqueUserName = null;
					return false;
				}
				uniqueUserName = username + $"({++attempt})";
			}
			return true;
		}

		/// <summary>
		/// Inserts a new user into table and returns that new user. Not all fields are copied from original user.
		/// </summary>
		/// <param name="user">The user that we will be copying from, not all fields are copied.</param>
		/// <param name="loginDetails"></param>
		/// <param name="isPasswordStrong"></param>
		/// <param name="username"></param>
		/// <param name="isForCemt">When true newly inserted user.UserNumCEMT will be set to the user.UserNum</param>
		/// <returns></returns>
		public static Userod CopyUser(Userod user, PasswordContainer loginDetails, bool isPasswordStrong, string username = null, bool isForCemt = false)
		{
			if (!TryGetUniqueUsername(username ?? (user.UserName + "(Copy)"), 0, false, isForCemt, out string uniqueUserName))
			{
				return null;
			}
			Userod copy = new Userod();
			//if function is ever called outside of the security form this ensures that we will know if a user is a copy of another user
			copy.UserName = uniqueUserName;
			copy.LoginDetails = loginDetails;
			copy.PasswordIsStrong = isPasswordStrong;
			copy.ClinicIsRestricted = user.ClinicIsRestricted;
			copy.ClinicNum = user.ClinicNum;
			//Insert also validates the user.
			copy.Id = Insert(copy, UserGroups.GetForUser(user.Id, isForCemt).Select(x => x.Id).ToList(), isForCemt);
			#region UserClinics
			List<UserClinic> listUserClinics = new List<UserClinic>(UserClinics.GetForUser(user.Id));
			listUserClinics.ForEach(x => x.UserId = copy.Id);
			UserClinics.Sync(listUserClinics, copy.Id);
			#endregion
			#region Alerts
			List<AlertSub> listUserAlert = AlertSubs.GetAllForUser(user.Id);
			listUserAlert.ForEach(x => x.UserNum = copy.Id);
			AlertSubs.Sync(listUserAlert, new List<AlertSub>());
			#endregion
			return copy;
		}

		public static List<Userod> GetForGroup(long userGroupNum)
		{
            return cache.Find(x => x.IsInUserGroup(userGroupNum)).ToList();
		}

		/// <summary>
		/// Gets a list of users for which the passed-in clinicNum is the only one they have access to.
		/// </summary>
		public static List<Userod> GetUsersOnlyThisClinic(long clinicNum)
		{
			string command = "SELECT userod.* "
			+ "FROM( "
				+ "SELECT userclinic.UserNum,COUNT(userclinic.ClinicNum) Clinics FROM userclinic "
				+ "GROUP BY userNum "
				+ "HAVING Clinics = 1 "
			+ ") users "
			+ "INNER JOIN userclinic ON userclinic.UserNum = users.UserNum "
				+ "AND userclinic.ClinicNum = " + POut.Long(clinicNum) + " "
			+ "INNER JOIN userod ON userod.UserNum = userclinic.UserNum ";

			return UserodCrud.SelectMany(command).ToList();
		}

		/// <summary>
		/// Will return 0 if no inbox found for user.
		/// </summary>
		public static long GetInbox(long userNum)
		{
            return GetFirstOrDefault(x => x.Id == userNum)?.TaskListInBox ?? 0;
        }

		/// <summary>
		/// Returns 3, which is non-admin provider type, if no match found.
		/// </summary>
		public static long GetAnesthProvType(long anesthProvType)
		{
			return GetFirstOrDefault(x => x.AnesthProvType == anesthProvType)?.AnesthProvType ?? 3;
        }

		public static List<Userod> GetUsersForJobs()
		{
			string command = "SELECT * FROM userod "
				+ "INNER JOIN jobpermission ON userod.UserNum=jobpermission.UserNum "
				+ "WHERE IsHidden=0 GROUP BY userod.UserNum ORDER BY UserName";
			return UserodCrud.SelectMany(command).ToList();
		}

		///<summary>Returns empty string if password is strong enough.  Otherwise, returns explanation of why it's not strong enough.</summary>
		public static string IsPasswordStrong(string pass)
		{
			//No need to check RemotingRole; no call to db.
			if (pass == "")
			{
				return Lans.g("FormUserPassword", "Password may not be blank when the strong password feature is turned on.");
			}
			if (pass.Length < 8)
			{
				return Lans.g("FormUserPassword", "Password must be at least eight characters long when the strong password feature is turned on.");
			}
			bool containsCap = false;
			for (int i = 0; i < pass.Length; i++)
			{
				if (Char.IsUpper(pass[i]))
				{
					containsCap = true;
				}
			}
			if (!containsCap)
			{
				return Lans.g("FormUserPassword", "Password must contain at least one capital letter when the strong password feature is turned on.");
			}
			bool containsLower = false;
			for (int i = 0; i < pass.Length; i++)
			{
				if (Char.IsLower(pass[i]))
				{
					containsLower = true;
				}
			}
			if (!containsLower)
			{
				return Lans.g("FormUserPassword", "Password must contain at least one lower case letter when the strong password feature is turned on.");
			}
			if (PrefC.GetBool(PrefName.PasswordsStrongIncludeSpecial))
			{
				bool hasSpecial = false;
				for (int i = 0; i < pass.Length; i++)
				{
					if (!Char.IsLetterOrDigit(pass[i]))
					{
						hasSpecial = true;
						break;
					}
				}
				if (!hasSpecial)
				{
					return Lans.g("FormUserPassword", "Password must contain at least one special character when the 'strong passwords require a special character' feature is turned on.");
				}
			}
			bool containsNum = false;
			for (int i = 0; i < pass.Length; i++)
			{
				if (Char.IsNumber(pass[i]))
				{
					containsNum = true;
				}
			}
			if (!containsNum)
			{
				return Lans.g("FormUserPassword", "Password must contain at least one number when the strong password feature is turned on.");
			}
			return "";
		}

		///<summary>This resets the strong password flag on all users after an admin turns off pref PasswordsMustBeStrong.  If strong passwords are again turned on later, then each user will have to edit their password in order set the strong password flag again.</summary>
		public static void ResetStrongPasswordFlags()
		{
			string command = "UPDATE userod SET PasswordIsStrong=0";
			Database.ExecuteNonQuery(command);
		}

		///<summary>Returns true if the passed-in user is apart of the passed-in usergroup.</summary>
		public static bool IsInUserGroup(long userNum, long userGroupNum)
		{
			List<UserGroupAttach> listAttaches = UserGroupAttaches.GetForUser(userNum);
			return listAttaches.Select(x => x.UserGroupNum).Contains(userGroupNum);
		}
	}
}
