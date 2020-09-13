using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class UserGroups
	{
		[CacheGroup(nameof(InvalidType.Security))]
		private class UserGroupCache : ListCache<UserGroup>
		{
			protected override IEnumerable<UserGroup> Load()
				=> SelectMany("SELECT * from `user_groups` ORDER BY `description`");
		}

		private static readonly UserGroupCache cache = new UserGroupCache();

		public static UserGroup FirstOrDefault(Predicate<UserGroup> predicate)
			=> cache.FirstOrDefault(predicate);

		public static List<UserGroup> Find(Predicate<UserGroup> predicate)
			=> cache.Find(predicate);

		public static void RefreshCache()
			=> cache.Refresh();

		public static UserGroup GetById(long userGroupId)
			=> FirstOrDefault(x => x.Id == userGroupId);

		public static IEnumerable<UserGroup> GetById(List<long> userGroupIds, bool includeCemt)
		{
			var userGroups = GetAll(includeCemt);

			return userGroupIds
				.Select(userGroupId => userGroups.FirstOrDefault(userGroup => userGroup.Id == userGroupId))
				.Where(userGroup => userGroup != null);
		}

		public static List<UserGroup> GetAll() 
			=> cache.GetAll();

		public static List<UserGroup> GetAll(bool includeCemt = false)
			=> includeCemt ? 
				GetAll() : Find(x => !x.CentralUserGroupId.HasValue);

		public static List<UserGroup> GetAllCemt()
			=> Find(x => x.CentralUserGroupId != 0);

		public static IEnumerable<UserGroup> GetAllCemtNoCache()
			=> SelectMany("SELECT * FROM `user_groups` WHERE `central_user_group_id` IS NOT NULL");

		public static void Save(UserGroup userGroup)
        {
			if (userGroup.Id == 0) ExecuteInsert(userGroup);
            else
            {
				ExecuteUpdate(userGroup);
            }
        }

		public static void Delete(UserGroup userGroup)
		{
			var count = Database.ExecuteLong("SELECT COUNT(*) FROM usergroupattach WHERE UserGroupNum=" + userGroup.Id);
			if (count != 0)
			{
				throw new Exception(
					"Must move users to another group first.");
			}

			if (Preferences.GetLong(PreferenceName.SecurityGroupForStudents) == userGroup.Id)
			{
				throw new Exception(
					"Group is the default group for students and cannot be deleted. Change the default student group before deleting.");
			}

			if (Preferences.GetLong(PreferenceName.SecurityGroupForInstructors) == userGroup.Id)
			{
				throw new Exception(
					"Group is the default group for instructors and cannot be deleted. Change the default instructors group before deleting.");
			}

			ExecuteDelete(userGroup);
		}

		/// <summary>
		/// Returns true if at least one of the usergroups passed in has the SecurityAdmin permission.
		/// </summary>
		public static bool IsAdminGroup(IEnumerable<long> userGroupIds)
		{
			var securityAdminPermissions = GroupPermissions.GetWhere(x => x.Permission == Permissions.SecurityAdmin);

			return userGroupIds.Any(x => securityAdminPermissions.Any(y => y.UserGroupId == x));
		}
	}
}
