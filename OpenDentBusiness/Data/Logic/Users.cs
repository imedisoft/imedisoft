using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class Users
	{
		public static long GetFirstSecurityAdminUserIdNoPasswordNoCache()
		{
			var command = 
				"SELECT u.`id`, CASE WHEN COALESCE(u.`password_hash`, '') = '' THEN 0 ELSE 1 END `has_password` " +
				"FROM `users` u " +
				"INNER JOIN `user_group_users` ugu ON u.`id` = ugu.`user_id` " +
				"INNER JOIN `group_permissions` gp ON ugu.`user_group_id` = gp.`user_group_id` " +
				"WHERE u.`is_hidden` = 0 AND gp.`permission` = " + (int)Permissions.SecurityAdmin + " " +
				"GROUP BY u.`id` ORDER BY u.`user_name` LIMIT 1";

			long userId = 0;

			Database.ExecuteReader(command, dataReader =>
			{
				if (dataReader.Read())
                {
					if (Convert.ToInt32(dataReader["has_password"]) == 0)
                    {
						userId = (long)dataReader["id"];
                    }
                }
			});

			return userId;
		}

		public static Dictionary<long, string> GetUsersByDomainUserNameNoCache(string domainUser)
		{
			var domainUsers = new Dictionary<long, string>();
			if (string.IsNullOrEmpty(domainUser))
            {
				return domainUsers;
            }

			Database.ExecuteReader("SELECT `id`, `user_name` FROM `users` WHERE `domain_user` = @domain_user `is_hidden` = 0", 
				dataReader =>
				{
					while (dataReader.Read())
					{
						domainUsers[(long)dataReader["id"]] = (string)dataReader["user_name"];
					}
				}, 
				new MySqlParameter("domain_user", domainUser));

			return domainUsers;
		}

		/// <summary>
		/// Checks whether there is at least one active user with the 'Security Admin' permission.
		/// </summary>
		/// <returns>True if there is at least one user with the 'Security Admin' permission; otherwise, false.</returns>
		public static bool HasSecurityAdminUserNoCache() 
			=> Database.ExecuteLong(
				"SELECT COUNT(*) FROM `users` u " +
				"INNER JOIN `user_group_users` ugu ON u.`id` = ugu.user_id " +
				"INNER JOIN `group_permissions` gp ON ugu.`user_group_id` = gp.`user_group_id` " +
				"WHERE u.`is_hidden` = 0 AND gp.`permission` = " + (int)Permissions.SecurityAdmin) != 0;

		/// <summary>
		/// Determines whether the database contain users that origate from the CEMT database.
		/// </summary>
		/// <returns>True if the database contains users from the CEMT database; otherwise, false.</returns>
		public static bool HasUsersForCemtNoCache() 
			=> Database.ExecuteLong("SELECT COUNT(*) FROM `users` WHERE `central_user_id` IS NOT NULL") > 0;

		/// <summary>
		/// Determines whether the specified user is allowed to sign notes.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns>True if the user can sign notes; otherwise, false.</returns>
		public static bool CanUserSignNote(User user = null)
		{
			user ??= Security.CurrentUser;

			if (Preferences.GetBool(PreferenceName.NotesProviderSignatureOnly) && user.ProviderId == null)
			{
				return false;
			}

			return true;
		}





		[CacheGroup(nameof(InvalidType.Security))]
		private class UserGroupUsersCache : ListCache<(long UserGroupId, long UserId)>
        {
			protected override IEnumerable<(long UserGroupId, long UserId)> Load()
				=> Database.SelectMany("SELECT * FROM `user_group_users`", 
					dataReader => ((long)dataReader["user_group_id"], (long)dataReader["user_id"]));
        }

        [CacheGroup(nameof(InvalidType.Security))]
        private class UsersCache : ListCache<User>
        {
            protected override IEnumerable<User> Load()
                => SelectMany("SELECT * FROM `users` ORDER BY `user_name`");
        }

        private static readonly UsersCache cache = new UsersCache();
		private static readonly UserGroupUsersCache cacheUserGroupUsers = new UserGroupUsersCache();

		public static void RefreshCache()
		{
			cache.Refresh();
			cacheUserGroupUsers.Refresh();
		}

		public static List<User> Find(Predicate<User> predicate)
			=> cache.Find(predicate).ToList();

		public static User FirstOrDefault(Predicate<User> predicate) 
			=> cache.FirstOrDefault(predicate);

		public static User GetById(long userId)
			=> FirstOrDefault(x => x.Id == userId);

		public static User GetByIdNoCache(long userId)
			=> SelectOne(userId);

		public static List<User> GetById(List<long> userIds)
			=> cache.Find(user => userIds.Contains(user.Id)).ToList();

		public static List<User> GetAll()
			=> GetAll(false);

		public static List<User> GetAll(bool excludeHidden)
        {
            if (excludeHidden)
            {
                return SelectMany("SELECT * FROM users WHERE is_hidden = 0 ORDER BY user_name").ToList();
            }

            return SelectMany("SELECT * FROM users ORDER BY user_name").ToList();
        }

		public static List<User> GetUsers()
			=> cache.Find(user => !user.IsHidden && user.CentralUserId == null);

		///<summary>Returns a list of non-hidden, non-CEMT user names.  Set hasOnlyCEMT to true if you only want non-hidden CEMT users.
		///Always returns all non-hidden users if PrefName.UserNameManualEntry is true.</summary>
		public static List<string> GetUserNamesNoCache(bool hasOnlyCEMT)
		{
			string command = $@"SELECT userod.UserName FROM userod 
				WHERE userod.IsHidden=0 
				{ (Preferences.GetBool(PreferenceName.UserNameManualEntry) ? " " : " AND userod.UserNumCEMT" + (hasOnlyCEMT ? "!=" : "=") + @"0 ") }
				ORDER BY userod.UserName";
			return Database.GetListString(command);
		}

		public static User GetByUserName(string userName) 
			=> cache.FirstOrDefault(user => !user.IsHidden && user.UserName.ToLower() == userName.ToLower());

		public static User GetByUserNameNoCache(string userName)
			=> SelectOne("SELECT * FROM `users` WHERE `user_name` = @user_name AND `is_hidden` = 0",
				new MySqlParameter("user_name", userName ?? ""));

		public static List<User> GetByUserGroup(long userGroupId)
			=> cache.Find(user => IsInUserGroup(user.Id, userGroupId)).ToList();

		public static List<User> GetByEmployee(long employeeId) 
			=> cache.Find(user => user.EmployeeId == employeeId);

		public static List<User> GetByPermission(Permissions permission, bool includeHidden) 
			=> cache.Find(user => (includeHidden || !user.IsHidden) && GroupPermissions.HasPermission(user, permission));

		public static List<User> GetWithProvider() 
			=> cache.Find(user => !user.IsHidden && user.ProviderId.HasValue);

		public static List<User> GetByProvider(long providerId) 
			=> cache.Find(user => user.ProviderId == providerId);

		public static List<User> GetByInboxTaskList(long taskListId) 
			=> cache.Find(user => user.InboxTaskListId == taskListId);


		/// <summary>
		/// Returns all users selectable for the insurance verification list.
		/// Pass in an empty list to not filter by clinic.
		/// Set isAssigning to false to return only users who have an insurance already assigned.
		/// </summary>
		public static List<User> GetUsersForVerifyList(List<long> listClinicNums, bool isAssigning)
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
				listUserNumsInInsVerify.AddRange(GetById(listUserNumsInInsVerify).FindAll(x => !x.ClinicIsRestricted).Select(x => x.Id).Distinct().ToList());//Always add unrestricted users into the list.
				listUserNumsInInsVerify = listUserNumsInInsVerify.Distinct().ToList();
			}
			List<User> listUsersWithPerm = GetByPermission(Permissions.InsPlanVerifyList, false);
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




		public static List<User> GetByDomainUserName(string domainUser) 
			=> cache.Find(user => user.DomainUser.Equals(domainUser, StringComparison.InvariantCultureIgnoreCase));

		public static string GetUserName(long userId) 
			=> cache.FirstOrDefault(x => x.Id == userId)?.UserName ?? "";

		public static long GetInboxTaskList(long userId) // TODO: Should return nullable...
			=> cache.FirstOrDefault(x => x.Id == userId)?.InboxTaskListId ?? 0;




		/// <summary>
		/// Returns true if the user passed in is associated with a provider that has (or had) an EHR prov key.
		/// </summary>
		public static bool IsUserCpoe(User user)
		{
			if (user == null || !user.ProviderId.HasValue)
			{
				return false;
			}

			var provider = Providers.GetById(user.ProviderId.Value);
			if (provider == null)
			{
				return false;
			}

			// Check to see if this provider has had a valid key at any point in history.
			return EhrProviderKeys.HasProviderHadKey(provider.LastName, provider.FirstName);
		}

		/// <summary>
		///		<para>
		///			Validates the specified <paramref name="username"/> and <paramref name="password"/> combination.
		///		</para>
		///		<para>
		///			Throws an exception if the specified credentials are invalid.
		///		</para>
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The (plaintext) password.</param>
		/// <returns>The user associated with the given username and password.</returns>
		/// <exception cref="Exception">If the specified credentials are invalid.</exception>
		public static User CheckUserAndPassword(string username, string password)
		{
			var user = GetByUserNameNoCache(username);
			if (user == null)
			{
				throw new Exception(Imedisoft.Translation.Common.InvalidUserNameOrPassword);
			}

			// We found a user via matching just the username passed in. Now we need to check to see if they have exceeded the log in failure attempts.
			// For now we are hardcoding a 5 minute delay when the user has failed to log in 5 times in a row.
			// An admin user can reset the password or the failure attempt count for the user failing to log in via the Security window.
			var serverTime = DateTime.UtcNow;
			if (user.FailedLoginDate.HasValue && serverTime.Subtract(user.FailedLoginDate.Value) < TimeSpan.FromMinutes(5) && user.FailedAttempts >= 5)
			{
				throw new Exception(Translation.Common.TooManyFailedLogonAttempts);
			}

			var passwordOk = Password.Verify(password, user.PasswordHash);
			if (!passwordOk)
			{
				user.FailedLoginDate = serverTime;
				user.FailedAttempts += 1;
			}
            else
            {
				user.FailedAttempts = 0;
				user.FailedLoginDate = DateTime.MinValue;
			}

			ExecuteUpdate(user);

			if (!passwordOk)
			{
				throw new Exception(Translation.Common.InvalidUserNameOrPassword);
			}

			return user;
		}

		/// <summary>
		/// Updates all students/instructors to the specified user group. 
		/// Surround with try/catch because it can throw exceptions.
		/// </summary>
		public static void UpdateUserGroupsForDentalSchools(UserGroup userGroup, bool isInstructor)
		{
			// Check if the user group that the students or instructors are trying to go to has the SecurityAdmin permission.
			if (!GroupPermissions.HasPermission(userGroup.Id, Permissions.SecurityAdmin))
			{
				var condition = isInstructor ?
					"AND prov.`is_instructor` = 1" : 
					"AND prov.`is_instructor` = 0 AND prov.`school_class_id` IS NULL";

				if (Database.ExecuteLong(
					"SELECT COUNT(*) FROM `user_group_users` ugu " +
					"INNER JOIN `user_groups` ug ON ugu.`user_group_id` = ug.`id` " +
					"INNER JOIN `group_permissions` gp ON gp.`user_group_id` = ug.`id` " +
					"WHERE ugu.`user_id` NOT IN " +
						"(SELECT u.`id` FROM `users` u, `providers` prov WHERE u.`provider_id` = prov.`id` " + condition + ") " +
					"AND gp.`permission` = " + (int)Permissions.SecurityAdmin) == 0)
				{
					throw new Exception("Cannot move students or instructors to the new user group because it would leave no users with the SecurityAdmin permission.");
				}
			}

			var command = 
				"UPDATE `users` u " +
				"INNER JOIN `providers` p ON u.`provider_id` = p.`id` " +
				"SET user_group_id = " + userGroup.Id + " " +
				"WHERE p.is_instructor = " + (isInstructor ? 1 : 0);

			if (!isInstructor)
			{
				command += " AND p.`school_class_id` IS NOT NULL";
			}

			Database.ExecuteNonQuery(command);
		}

		public static void Update(User user, IEnumerable<long> userGroupIds = null)
		{
			Validate(user, userGroupIds);

			ExecuteUpdate(user);

			if (userGroupIds == null)
			{
				return;
			}

			var userGroupIdsList = userGroupIds.ToList();
			if (userGroupIdsList.Count > 0)
			{
				Database.ExecuteNonQuery(
					"DELETE FROM `user_group_users` " +
					"WHERE `user_group_id` NOT IN (" + string.Join(", ", userGroupIdsList) + ")");

				foreach (var userGroupId in userGroupIdsList)
				{
					AddToGroup(user.Id, userGroupId);
				}
			}
		}

		public static void UpdatePassword(User user, string passwordHash, bool isPasswordStrong)
		{
			var userGroups = GetGroups(user.Id, false).ToList();
			if (userGroups.Count < 1)
			{
				throw new Exception("The current user must be in at least one user group.");
			}

			user.PasswordHash = passwordHash;
			user.PasswordIsStrong = isPasswordStrong;

			Validate(user, userGroups.Select(x => x.Id).ToList());

			ExecuteUpdate(user);
		}

		public static void DisassociateTaskListInBox(long taskListId) 
			=> Database.ExecuteNonQuery(
				"UPDATE `users` SET `inbox_task_list_id` = NULL WHERE `inbox_task_list_id` = " + taskListId);

		public static long Insert(User user, IEnumerable<long> userGroupIds)
		{
			if (user.IsHidden && UserGroups.IsAdminGroup(userGroupIds))
			{
				throw new Exception("Administrators cannot be hidden.");
			}

			Validate(user, userGroupIds);

			long userId = ExecuteInsert(user);

			if (userGroupIds != null)
			{
				foreach (var userGroupId in userGroupIds)
				{
					AddToGroup(userId, userGroupId);
				}
			}

			return userId;
		}

		/// <summary>
		/// Adds the user with the specified ID to the group with the specified ID.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <param name="userGroupId">The ID of the user group.</param>
		public static void AddToGroup(long userId, long userGroupId) 
			=> Database.ExecuteNonQuery(
				"INSERT INTO user_group_users (user_id, user_group_id) " +
				"VALUES (" + userId + ", " + userGroupId + ") " +
				"ON DUPLICATE KEY IGNORE");

		private static void Validate(User user, IEnumerable<long> userGroupIds)
		{
			if (!IsUserNameUnique(user.UserName, user.Id))
			{
				throw new Exception("Username already in use.");
			}

			if (userGroupIds == null) return;

			var userGroupIdsList = userGroupIds.ToList();
			if (userGroupIdsList.Count < 1)
			{
				throw new Exception("The current user must be in at least one user group.");
			}

			var isSecurityAdmin = Database.ExecuteLong(
				"SELECT COUNT(*) FROM group_permissions " +
				"WHERE permission = " + (int)Permissions.SecurityAdmin + " " +
				"AND user_group_id IN (" + string.Join(", ", userGroupIdsList) + ")") > 0;

			if (user.Id > 0 && !isSecurityAdmin && !IsSomeoneElseSecurityAdmin(user))
			{
				throw new Exception("At least one user must have Security Admin permission.");
			}

			if (user.IsHidden && isSecurityAdmin)
			{
				throw new Exception("Administrators cannot be hidden.");
			}
		}

		/// <summary>
		/// Checks whether any user other then the specified <paramref name="user"/> has the 'Security Admin' permission.
		/// </summary>
		/// <param name="user">The user to exclude from the check.</param>
		/// <returns>True if another user has the 'Security Admin' permission; otherwise, false.</returns>
		public static bool IsSomeoneElseSecurityAdmin(User user) 
			=> Database.ExecuteLong(
				"SELECT COUNT(*) FROM `users` u " +
				"INNER JOIN `user_group_users` ugu ON ugu.`user_id` = u.`id` " +
				"INNER JOIN `group_permissions` gp ON ugu.`user_group_id` = gp.`user_group_id` " +
				"WHERE gp.`permission` = " + (int)Permissions.SecurityAdmin + " " +
				"AND u.`is_hidden` = 0 AND u.`id` != " + user.Id) > 0;

		/// <summary>
		/// Checks whether the specified <paramref name="userName"/> is unique.
		/// </summary>
		/// <param name="userName">The user name to check.</param>
		/// <param name="excludeUserId">The ID of the user to exclude from the check.</param>
		/// <returns>True if the username is unique; otherwise, false.</returns>
		public static bool IsUserNameUnique(string userName, long excludeUserId = 0)
		{
			if (string.IsNullOrEmpty(userName)) return false;

			return Database.ExecuteLong(
				"SELECT COUNT(*) FROM `users` WHERE `user_name` = @user_name AND `id` !=" + excludeUserId, 
					new MySqlParameter("user_name", userName)) == 0;
		}

		public static bool TryGetUniqueUsername(string userName, long excludeUserId, out string uniqueUserName)
		{
			int attempt = 1;

			uniqueUserName = userName;

			while (!IsUserNameUnique(uniqueUserName, excludeUserId))
			{
				if (attempt > 100)
				{
					uniqueUserName = null;

					return false;
				}

				uniqueUserName = userName + $"({++attempt})";
			}
			return true;
		}

		/// <summary>
		/// Inserts a new user into table and returns that new user. Not all fields are copied from original user.
		/// </summary>
		/// <param name="user">The user that we will be copying from, not all fields are copied.</param>
		/// <param name="passwordHash"></param>
		/// <param name="isPasswordStrong"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		public static User CopyUser(User user, string passwordHash, bool isPasswordStrong, string username = null)
		{
			if (!TryGetUniqueUsername(username ?? (user.UserName + " (Copy)"), 0, out string uniqueUserName))
			{
				return null;
			}

            var newUser = new User
			{
                UserName = uniqueUserName,
                PasswordHash = passwordHash,
                PasswordIsStrong = isPasswordStrong,
                ClinicIsRestricted = user.ClinicIsRestricted,
                ClinicId = user.ClinicId
            };

            newUser.Id = Insert(newUser, GetGroups(user.Id, false).Select(x => x.Id).ToList());

			var userClinics = UserClinics.GetForUser(user.Id);

			userClinics.ForEach(x => x.UserId = newUser.Id);

			UserClinics.Sync(userClinics, newUser.Id);

			AlertSubs.GetAllForUser(user.Id).ForEach(x =>
			{
				x.UserId = newUser.Id;

				AlertSubs.Insert(x);
			});

			return newUser;
		}





		public static IEnumerable<User> GetUsersOnlyThisClinic(long clinicId) 
			=> SelectMany(
				"SELECT users.* " +
				"FROM (" +
					"SELECT uc.`user_id`, COUNT(uc.`clinic_id`) AS `clinics` FROM `user_clinics` uc " +
					"GROUP BY uc.`user_id` " +
					"HAVING clinics = 1" +
				") unq " +
				"INNER JOIN `user_clinics` uc ON (uc.`user_id` = unq.`user_id` AND uc.`clinic_id` = " + clinicId + ") " +
				"INNER JOIN `users` u ON u.id = uc.user_id");

		/// <summary>
		///		<para>
		///			Checks the specified <paramref name="password"/> to ensure it meets the 
		///			criteria to qualify as a 'strong' password.
		///		</para>
		///		<para>
		///			Throws an exception if the specified <paramref name="password"/> is not a 
		///			strong password.
		///		</para>
		/// </summary>
		/// <param name="password">The password to check.</param>
		/// <exception cref="Exception">If the password is not strong.</exception>
		public static void EnsurePasswordStrong(string password)
		{
			if (string.IsNullOrEmpty(password))
			{
				throw new Exception(Translation.Common.PasswordMayNotBeBlank);
			}

			if (password.Length < 8)
			{
				throw new Exception(Translation.Common.PasswordMustBeAtLeastEightCharacters);
			}

			bool containsUpper = false;

			foreach (var c in password)
			{
				if (char.IsUpper(c))
				{
					containsUpper = true;

					break;
				}
			}

			if (!containsUpper)
			{
				throw new Exception(Translation.Common.PasswordMustContainAtLeastOneUpperCaseLetter);
			}

			bool containsLower = false;

			foreach (var c in password)
			{
				if (char.IsLower(c))
				{
					containsLower = true;

					break;
				}
			}

			if (!containsLower)
			{
				throw new Exception(Translation.Common.PasswordMustContainAtLeastOneLowerCaseLetter);
			}

			if (Preferences.GetBool(PreferenceName.PasswordsStrongIncludeSpecial))
			{
				bool containsSpecial = false;

				foreach (var c in password)
				{
					if (!char.IsLetterOrDigit(c))
					{
						containsSpecial = true;

						break;
					}
				}

				if (!containsSpecial)
				{
					throw new Exception(Translation.Common.PasswordMustContainAtLeastOneSpecialCharacter);
				}
			}

			bool containerNumber = false;

			foreach (var c in password)
			{
				if (char.IsNumber(c))
				{
					containerNumber = true;

					break;
				}
			}

			if (!containerNumber)
			{
				throw new Exception(Translation.Common.PasswordMustContainAtLeastOneNumber);
			}
		}

		/// <summary>
		/// Determines whether the specified password qualifies as a strong password.
		/// </summary>
		/// <param name="password">The password to check.</param>
		/// <returns>True if the password is strong; otherwise, false.</returns>
		public static bool IsPasswordStrong(string password)
        {
            try
            {
				EnsurePasswordStrong(password);

				return true;
			}
            catch
            {
				return false;
            }
        }

		public static void ResetStrongPasswordFlags() 
			=> Database.ExecuteNonQuery("UPDATE `users` SET `password_is_strong` = 0");

		public static bool IsInUserGroup(long userId, long userGroupId) 
			=> cacheUserGroupUsers.Any(ugu => ugu.UserId == userId && ugu.UserGroupId == userGroupId);

		public static IEnumerable<UserGroup> GetGroups(long userId, bool includeCemt)
			=> UserGroups.GetById(cacheUserGroupUsers.Find(ugu => ugu.UserId == userId).Select(ugu => ugu.UserGroupId).ToList(), includeCemt);
	}
}
