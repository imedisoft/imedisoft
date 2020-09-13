using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OpenDentBusiness
{
    public partial class GroupPermissions
	{
		/// <summary>
		/// The maximum number of days allowed for the NewerDays column.
		/// Setting a NewerDays to a value higher than this will cause an exception to be thrown in the program.
		/// There is a DBM that will correct invalid NewerDays in the database.
		/// </summary>
		public const double NewerDaysMax = 3000;

		/// <summary>
		/// Returns the Date that the user is restricted to for the passed-in permission. 
		/// Returns MinVal if the user is not restricted or does not have the permission.
		/// </summary>
		public static DateTime? GetDateRestrictedForPermission(Permissions permission, List<long> userGroupIds)
		{
			var currentDate = DateTime.UtcNow.Date;

			var groupPermissions = GetForUserGroups(userGroupIds, permission);

			var groupPermission = groupPermissions
				.OrderBy(y =>
				{
					if (y.NewerDays == 0 && y.NewerDate == DateTime.MinValue)
					{
						return DateTime.MinValue;
					}

					if (y.NewerDays == 0)
					{
						return y.NewerDate;
					}

					return currentDate.AddDays(-y.NewerDays);


				})
				.FirstOrDefault();

			if (groupPermission == null)
				return null;

			if (!groupPermission.NewerDate.HasValue && groupPermission.NewerDays == 0)
				return null;

			if (groupPermission.NewerDate.HasValue)
				return groupPermission.NewerDate;

			try
			{
				return currentDate.AddDays(-groupPermission.NewerDays);
			}
			catch (ArgumentOutOfRangeException)
            {
            }

			return null;
		}

		/// <summary>
		/// Used for procedures with status EO, EC, or C. 
		/// Returns Permissions.ProcExistingEdit for EO/EC
		/// </summary>
		public static Permissions SwitchExistingPermissionIfNeeded(Permissions permission, Procedure procedure)
		{
			if (procedure.ProcStatus.In(ProcStat.EO, ProcStat.EC))
			{
				return Permissions.ProcExistingEdit;
			}

			return permission;
		}

		[CacheGroup(nameof(InvalidType.Security))]
		private class GroupPermissionCache : ListCache<GroupPermission>
		{
			protected override IEnumerable<GroupPermission> Load() 
				=> SelectMany("SELECT * FROM `group_permissions`");
        }

		private static readonly GroupPermissionCache cache = new GroupPermissionCache();

		public static GroupPermission GetFirstOrDefault(Predicate<GroupPermission> predicate) 
			=> cache.FirstOrDefault(predicate);

		public static List<GroupPermission> GetWhere(Predicate<GroupPermission> predicate)
			=> cache.Find(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static void Update(GroupPermission groupPermission)
		{
			if (groupPermission.NewerDate.HasValue && groupPermission.NewerDays > 0)
			{
				throw new Exception("Date or days can be set, but not both.");
			}

			if (!PermissionTakesDates(groupPermission.Permission))
			{
				if (groupPermission.NewerDate.HasValue || groupPermission.NewerDays > 0)
				{
					throw new Exception("This type of permission may not have a date or days set.");
				}
			}

			UpdateInternal(groupPermission);
		}

		public static void UpdateNoCache(GroupPermission groupPermission) 
			=> UpdateInternal(groupPermission);
	
		public static long Insert(GroupPermission groupPermission)
		{
			if (groupPermission.NewerDate.HasValue && groupPermission.NewerDays > 0)
			{
				throw new Exception("Date or days can be set, but not both.");
			}

			if (!PermissionTakesDates(groupPermission.Permission))
			{
				if (groupPermission.NewerDate.HasValue || groupPermission.NewerDays > 0)
				{
					throw new Exception("This type of permission may not have a date or days set.");
				}

			}

			if (groupPermission.Permission == Permissions.SecurityAdmin)
			{
				// Make sure there are no hidden users in the group that is about to get the Security Admin permission.
				string command =
					"SELECT COUNT(*) FROM userod " +
					"INNER JOIN usergroupattach ON usergroupattach.UserNum = userod.UserNum " +
					"WHERE userod.IsHidden = 1 " +
					"AND usergroupattach.UserGroupNum=" + groupPermission.UserGroupId;


				if (Database.ExecuteLong(command) != 0)
				{
					// There are hidden users in this group.
					throw new Exception("The Security Admin permission cannot be given to a user group with hidden users.");
				}
			}

			return InsertInternal(groupPermission);
		}

		public static long InsertNoCache(GroupPermission groupPermission) 
			=> InsertInternal(groupPermission);

		public static void RemovePermission(long userGroupId, Permissions permission)
		{
			string command;

			if (permission == Permissions.SecurityAdmin)
			{
				// Need to make sure that at least one other user has this permission
				command = 
					"SELECT COUNT(*) FROM (SELECT DISTINCT grouppermission.UserGroupNum " +
					"FROM grouppermission " +
					"INNER JOIN usergroupattach ON usergroupattach.UserGroupNum=grouppermission.UserGroupNum " +
					"INNER JOIN userod ON userod.UserNum=usergroupattach.UserNum AND userod.IsHidden=0 " +
					"WHERE grouppermission.PermType='" + (int)permission + "' " +
					"AND grouppermission.UserGroupNum!=" + userGroupId + ") t";

				if (Database.ExecuteLong(command) == 0)
				{
					// No other users outside of this group have SecurityAdmin
					throw new Exception("There must always be at least one user in a user group that has the Security Admin permission.");
				}
			}

			if (permission == Permissions.Reports)
			{
				// Special case. For Reports permission type we want to delete the "base" Reports permission but not any Reports permissions with FKey
				// When they re-enable the Reports permission we want to remember all individual reports permissions for that UserGroup
				command =
					"DELETE FROM `group_permissions` " +
					"WHERE `user_group_id` = " + userGroupId + " " +
					"AND `permission` = " + (int)permission + " " +
					"AND `object_id` IS NULL";
			}
			else
			{
				command =
					"DELETE FROM `group_permissions` " +
					"WHERE `user_group_id` = " + userGroupId + " " +
					"AND `permission` = " + (int)permission;
			}

			Database.ExecuteNonQuery(command);
		}

		public static bool Sync(List<GroupPermission> listNew, List<GroupPermission> listOld)
		{
			return false; //  return Crud.GroupPermissionCrud.Sync(listNew, listOld);
		}

		public static GroupPermission GetPermission(long userGroupId, Permissions permission) 
			=> GetFirstOrDefault(x => x.UserGroupId == userGroupId && x.Permission == permission);

		public static List<GroupPermission> GetByUserGroup(long userGroupId) 
			=> GetWhere(x => x.UserGroupId == userGroupId);

		public static IEnumerable<GroupPermission> GetPermsNoCache(long userGroupId) 
			=> SelectMany("SELECT * FROM `group_permissions` WHERE `user_group_id` = " + userGroupId);
		
		public static List<GroupPermission> GetPermissionsForReports() 
			=> GetWhere(x => x.Permission == Permissions.Reports && x.ObjectId != 0);
		

		/// <summary>
		/// Used to check if user has permission to access the report. 
		/// Pass in a list of DisplayReports to avoid a call to the db.
		/// </summary>
		public static bool HasReportPermission(string reportName, User user, List<DisplayReport> reports = null)
		{
			if (!Security.IsAuthorized(Permissions.Reports, true))
			{
				return false;
			}

			var report = (reports ?? DisplayReports.GetAll(false)).FirstOrDefault(x => x.InternalName == reportName);
			if (report == null) // Report is probably hidden.
			{
				return false;
			}

			var reportPermissions = GetPermissionsForReports();
			if (reportPermissions.Exists(x => x.ObjectId == report.DisplayReportNum && Users.IsInUserGroup(user.Id, x.UserGroupId)))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Determines whether a single userGroup contains a specific permission.
		/// </summary>
		public static bool HasPermission(long userGroupId, Permissions permission, long? objectId = null)
		{
			var groupPermission = GetFirstOrDefault(x => x.UserGroupId == userGroupId && x.Permission == permission && x.ObjectId == objectId);

			return groupPermission != null;
		}

		/// <summary>
		/// Determines whether an individual user has a specific permission.
		/// </summary>
		public static bool HasPermission(User user, Permissions permission, long? objectId = null)
		{
			var groupPermission = GetFirstOrDefault(x => x.Permission == permission && x.ObjectId == objectId && Users.IsInUserGroup(user.Id, x.UserGroupId));

			return groupPermission != null;
		}

		/// <summary>
		/// Returns permissions associated to the passed-in usergroups. 
		/// Pass in a specific permType to only return GroupPermissions of that type.
		/// Otherwise, will return all GroupPermissions for the UserGroups.
		/// </summary>
		public static List<GroupPermission> GetForUserGroups(List<long> userGroupIds, Permissions permission = Permissions.None)
		{
			if (permission == Permissions.None)
			{
				return GetWhere(x => userGroupIds.Contains(x.UserGroupId));
			}

			return GetWhere(x => x.Permission == permission && userGroupIds.Contains(x.UserGroupId));
		}

		/// <summary>
		/// Gets a value indicating whether the specified permission actually generates audit trail entries.
		/// </summary>
		public static bool HasAuditTrail(Permissions permission)
		{
			switch (permission)
			{
				case Permissions.None:
				case Permissions.AppointmentsModule:
				case Permissions.ManageModule:
				case Permissions.StartupSingleUserOld:
				case Permissions.StartupMultiUserOld:
				case Permissions.TimecardsEditAll:
				case Permissions.AnesthesiaIntakeMeds:
				case Permissions.AnesthesiaControlMeds:
				case Permissions.EquipmentDelete:
				case Permissions.ProcEditShowFee:
				case Permissions.AdjustmentEditZero:
				case Permissions.EhrEmergencyAccess:
				case Permissions.EhrKeyAdd:
				case Permissions.Providers:
				case Permissions.EcwAppointmentRevise:
				case Permissions.ProcedureNoteFull:
				case Permissions.ProcedureNoteUser:
				case Permissions.GraphicalReports:
				case Permissions.EquipmentSetup:
				case Permissions.WikiListSetup:
				case Permissions.Copy:
				case Permissions.PatFamilyHealthEdit:
				case Permissions.PatientPortal:
				case Permissions.AdminDentalStudents:
				case Permissions.AdminDentalInstructors:
				case Permissions.OrthoChartEditUser:
				case Permissions.AdminDentalEvaluations:
				case Permissions.UserQueryAdmin:
				case Permissions.ProviderFeeEdit:
				case Permissions.ClaimHistoryEdit:
				case Permissions.PreAuthSentEdit:
				case Permissions.InsPlanVerifyList:
				case Permissions.ProviderAlphabetize:
				case Permissions.ClaimProcReceivedEdit:
				case Permissions.ReportProdIncAllProviders:
				case Permissions.ReportDailyAllProviders:
				case Permissions.SheetDelete:
				case Permissions.UpdateCustomTracking:
				case Permissions.InsPlanOrthoEdit:
				case Permissions.PopupEdit:
				case Permissions.InsPlanPickListExisting:
				case Permissions.WikiAdmin:
				case Permissions.ClaimView:
				case Permissions.TreatPlanSign:
				case Permissions.UnrestrictedSearch:
				case Permissions.ArchivedPatientEdit:
				case Permissions.InsuranceVerification:
				case Permissions.NewClaimsProcNotBilled:
					return false;
			}

			if (permission.In(
					//These permissions are only used at OD HQ
					Permissions.FAQEdit
				))
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Gets the description for the specified permisssion.
		/// </summary>
		public static string GetDesc(Permissions permission) 
			=> permission.GetDescription();

		public static bool PermissionTakesDates(Permissions permission) => 
			permission == Permissions.AccountingCreate || 
			permission == Permissions.AccountingEdit || 
			permission == Permissions.AdjustmentEdit || 
			permission == Permissions.ClaimDelete || 
			permission == Permissions.ClaimHistoryEdit || 
			permission == Permissions.ClaimProcReceivedEdit || 
			permission == Permissions.ClaimSentEdit || 
			permission == Permissions.CommlogEdit || 
			permission == Permissions.DepositSlips || 
			permission == Permissions.EquipmentDelete || 
			permission == Permissions.ImageDelete || 
			permission == Permissions.InsPayEdit || 
			permission == Permissions.InsWriteOffEdit || 
			permission == Permissions.NewClaimsProcNotBilled || 
			permission == Permissions.OrthoChartEditFull || 
			permission == Permissions.OrthoChartEditUser || 
			permission == Permissions.PaymentEdit || 
			permission == Permissions.PerioEdit || 
			permission == Permissions.PreAuthSentEdit || 
			permission == Permissions.ProcCompleteEdit || 
			permission == Permissions.ProcCompleteNote || 
			permission == Permissions.ProcCompleteEditMisc || 
			permission == Permissions.ProcCompleteStatusEdit || 
			permission == Permissions.ProcCompleteAddAdj || 
			permission == Permissions.ProcExistingEdit || 
			permission == Permissions.ProcDelete || 
			permission == Permissions.SheetEdit ||
			permission == Permissions.TimecardDeleteEntry || 
			permission == Permissions.TreatPlanEdit || 
			permission == Permissions.TreatPlanSign ||
			permission == Permissions.PaymentCreate;
	}
}
