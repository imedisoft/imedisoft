using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;
using Imedisoft.Data;
using Imedisoft.Data.Cache;
using OpenDentBusiness.DoseSpotService;

namespace OpenDentBusiness
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

		public static List<UserGroup> GetDeepCopy() 
			=> cache.GetAll();

		public static UserGroup GetFirstOrDefault(Predicate<UserGroup> match) 
			=> cache.FirstOrDefault(match);

		public static List<UserGroup> GetWhere(Predicate<UserGroup> match) 
			=> cache.Find(match);

		public static void RefreshCache() 
			=> cache.Refresh();

		/// <summary>
		/// A list of all user groups, ordered by description. Set includeCEMT to true if you want CEMT user groups included.
		/// </summary>
		public static List<UserGroup> GetList(bool includeCEMT = false) 
			=> GetWhere(x => includeCEMT || x.CentralUserGroupId == 0);

		///<summary>Only called from the CEMT in order to update a remote database with changes.  
		///This method will update rows based on the UserGroupNumCEMT instead of the typical UserGroupNum column.</summary>
		public static void UpdateCEMTNoCache(UserGroup userGroupCEMT)
		{
			if (userGroupCEMT.Id == 0)
			{
				throw new Exception(userGroupCEMT.Description + " has a UserGroupNum of 0 and cannot be synced.");
			}

			Database.ExecuteNonQuery(
				"UPDATE usergroup SET Description = '" + POut.String(userGroupCEMT.Description) + "' " +
				"WHERE UserGroupNumCEMT = " + userGroupCEMT.Id);
		}

		public static List<UserGroup> GetCEMTGroups() 
			=> GetWhere(x => x.CentralUserGroupId != 0);

		/// <summary>
		/// Gets a list of CEMT usergroups without using the cache. Useful for multithreaded connections.
		/// </summary>
		public static IEnumerable<UserGroup> GetCEMTGroupsNoCache() 
			=> SelectMany("SELECT * FROM usergroup WHERE UserGroupNumCEMT != 0");

		/// <summary>
		/// Checks for dependencies first
		/// </summary>
		public static void Delete2(UserGroup group)
		{
			var count = Database.ExecuteLong("SELECT COUNT(*) FROM usergroupattach WHERE UserGroupNum=" + group.Id);
			if (count != 0)
			{
				throw new Exception(
					"Must move users to another group first.");
			}

			if (Prefs.GetLong(PrefName.SecurityGroupForStudents) == group.Id)
			{
				throw new Exception(
					"Group is the default group for students and cannot be deleted. Change the default student group before deleting.");
			}

			if (Prefs.GetLong(PrefName.SecurityGroupForInstructors) == group.Id)
			{
				throw new Exception(
					"Group is the default group for instructors and cannot be deleted. Change the default instructors group before deleting.");
			}

			Database.ExecuteNonQuery("DELETE FROM usergroup WHERE UserGroupNum=" + group.Id);
			Database.ExecuteNonQuery("DELETE FROM grouppermission WHERE UserGroupNum=" + group.Id);
		}

		/// <summary>
		/// Deletes without using the cache. Doesn't check dependencies. Useful for multithreaded connections.
		/// </summary>
		public static void DeleteNoCache(UserGroup group)
		{
			Database.ExecuteNonQuery("DELETE FROM usergroup WHERE UserGroupNum=" + group.Id);
			Database.ExecuteNonQuery("DELETE FROM grouppermission WHERE UserGroupNum=" + group.Id);
		}

		public static UserGroup GetGroup(long userGroupId) 
			=> GetFirstOrDefault(x => x.Id == userGroupId);

		/// <summary>
		/// Returns a list of usergroups given a list of usergroupnums.
		/// </summary>
		public static IEnumerable<UserGroup> GetList(List<long> userGroupIds, bool includeCemt)
		{
			var userGroups = includeCemt ? GetDeepCopy() : GetList();

			return userGroupIds
				.Select(userGroupId => userGroups.FirstOrDefault(userGroup => userGroup.Id == userGroupId))
				.Where(userGroup => userGroup != null);
		}

		/// <summary>
		/// Returns a list of usergroups for a given user. 
		/// Returns an empty list if the user is not associated to any user groups. (should never happen)
		/// </summary>
		public static IEnumerable<UserGroup> GetForUser(long userId, bool includeCemt) 
			=> GetList(UserGroupAttaches.GetForUser(userId).Select(x => x.UserGroupId).ToList(), includeCemt);

		/// <summary>
		/// Returns true if at least one of the usergroups passed in has the SecurityAdmin permission.
		/// </summary>
		public static bool IsAdminGroup(List<long> userGroupIds)
		{
			var securityAdminPermissions = GroupPermissions.GetWhere(x => x.Permission == Permissions.SecurityAdmin);

			return userGroupIds.Any(x => securityAdminPermissions.Any(y => y.UserGroupId == x));
		}
	}
}
