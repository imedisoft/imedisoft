using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using CodeBase;
using Imedisoft.Data;

namespace OpenDentBusiness
{
	public class GroupPermissions
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

		#region CachePattern

		private class GroupPermissionCache : CacheListAbs<GroupPermission>
		{
			protected override List<GroupPermission> GetCacheFromDb()
			{
				string command = "SELECT * FROM grouppermission";
				return Crud.GroupPermissionCrud.SelectMany(command);
			}
			protected override List<GroupPermission> TableToList(DataTable table)
			{
				return Crud.GroupPermissionCrud.TableToList(table);
			}
			protected override GroupPermission Copy(GroupPermission GroupPermission)
			{
				return GroupPermission.Copy();
			}
			protected override DataTable ListToTable(List<GroupPermission> listGroupPermissions)
			{
				return Crud.GroupPermissionCrud.ListToTable(listGroupPermissions, "GroupPermission");
			}
			protected override void FillCacheIfNeeded()
			{
				GroupPermissions.GetTableFromCache(false);
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static GroupPermissionCache _GroupPermissionCache = new GroupPermissionCache();

		public static GroupPermission GetFirstOrDefault(Func<GroupPermission, bool> match, bool isShort = false)
		{
			return _GroupPermissionCache.GetFirstOrDefault(match, isShort);
		}

		public static List<GroupPermission> GetWhere(Predicate<GroupPermission> match, bool isShort = false)
		{
			return _GroupPermissionCache.GetWhere(match, isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache()
		{
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table)
		{
			_GroupPermissionCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache)
		{

			return _GroupPermissionCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		public static void Update(GroupPermission gp)
		{
			if (gp.NewerDate.HasValue && gp.NewerDays > 0)
			{
				throw new Exception("Date or days can be set, but not both.");
			}

			if (!PermissionTakesDates(gp.Permission))
			{
				if (gp.NewerDate.HasValue || gp.NewerDays > 0)
				{
					throw new Exception("This type of permission may not have a date or days set.");
				}
			}

			Crud.GroupPermissionCrud.Update(gp);
		}

		/// <summary>
		/// Update that doesnt use the local cache. Useful for multithreaded connections.
		/// </summary>
		public static void UpdateNoCache(GroupPermission gp)
		{
			string command = "UPDATE grouppermission SET "
				+ "NewerDate   =  " + POut.Date(gp.NewerDate ?? DateTime.MinValue) + ", "
				+ "NewerDays   =  " + POut.Int(gp.NewerDays) + ", "
				+ "UserGroupNum=  " + gp.UserGroupId + ", "
				+ "PermType    =  " + (int)gp.Permission + " "
				+ "WHERE GroupPermNum = " + gp.Id;

			Database.ExecuteNonQuery(command);
		}
		/// <summary>
		/// Deletes GroupPermissions based on primary key. 
		/// Do not call this method unless you have checked specific dependencies first. 
		/// E.g. after deleting this permission, there will still be a security admin user. 
		/// This method is only called from the CEMT sync. 
		/// RemovePermission should probably be used instead.
		/// </summary>
		public static void Delete(GroupPermission groupPermission) 
			=> Database.ExecuteNonQuery(
				"DELETE FROM grouppermission WHERE GroupPermNum = " + groupPermission.Id);

		/// <summary>
		/// Deletes without using the cache. 
		/// Useful for multithreaded connections.
		/// </summary>
		public static void DeleteNoCache(GroupPermission groupPermission) 
			=> Database.ExecuteNonQuery(
				"DELETE FROM grouppermission WHERE GroupPermNum=" + groupPermission.Id);

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
					"INNER JOIN usergroupattach ON usergroupattach.UserNum=userod.UserNum " +
					"WHERE userod.IsHidden=1 " +
					"AND usergroupattach.UserGroupNum=" + groupPermission.UserGroupId;


				if (Database.ExecuteLong(command) != 0)
				{
					// There are hidden users in this group
					throw new Exception("The Security Admin permission cannot be given to a user group with hidden users.");
				}
			}

			return Crud.GroupPermissionCrud.Insert(groupPermission);
		}

		/// <summary>
		/// Insertion logic that doesn't use the cache. Has special cases for generating random PK's and handling Oracle insertions.
		/// </summary>
		public static long InsertNoCache(GroupPermission gp)
		{
			return Crud.GroupPermissionCrud.InsertNoCache(gp);
		}

		public static void RemovePermission(long userGroupId, Permissions permission)
		{
			string command;
			if (permission == Permissions.SecurityAdmin)
			{
				//need to make sure that at least one other user has this permission
				command = "SELECT COUNT(*) FROM (SELECT DISTINCT grouppermission.UserGroupNum "
					+ "FROM grouppermission "
					+ "INNER JOIN usergroupattach ON usergroupattach.UserGroupNum=grouppermission.UserGroupNum "
					+ "INNER JOIN userod ON userod.UserNum=usergroupattach.UserNum AND userod.IsHidden=0 "
					+ "WHERE grouppermission.PermType='" + (int)permission + "' "
					+ "AND grouppermission.UserGroupNum!=" + userGroupId + ") t";

				if (Database.ExecuteLong(command) == 0)
				{//no other users outside of this group have SecurityAdmin
					throw new Exception("There must always be at least one user in a user group that has the Security Admin permission.");
				}
			}

			if (permission == Permissions.Reports)
			{
				//Special case.  For Reports permission type we want to delete the "base" Reports permission but not any Reports permissions with FKey
				//When they re-enable the Reports permission we want to remember all individual reports permissions for that UserGroup
				command =
					"DELETE from grouppermission WHERE UserGroupNum='" + userGroupId + "' AND PermType='" + (int)permission + "' AND FKey=0";
			}
			else
			{
				command =
					"DELETE from grouppermission WHERE UserGroupNum='" + userGroupId + "' AND PermType='" + (int)permission + "'";
			}

			Database.ExecuteNonQuery(command);
		}

		public static bool Sync(List<GroupPermission> listNew, List<GroupPermission> listOld)
		{
			return Crud.GroupPermissionCrud.Sync(listNew, listOld);
		}

		/// <summary>
		/// Gets a GroupPermission based on the supplied userGroupNum and permType.
		/// If not found, then it returns null. 
		/// Used in FormSecurity when double clicking on a dated permission or when clicking the all button.
		/// </summary>
		public static GroupPermission GetPerm(long userGroupNum, Permissions permType)
		{
			return GetFirstOrDefault(x => x.UserGroupId == userGroupNum && x.Permission == permType);
		}

		/// <summary>
		/// Gets a list of GroupPermissions for the supplied UserGroupNum.
		/// </summary>
		public static List<GroupPermission> GetPerms(long userGroupNum)
		{
			return GetWhere(x => x.UserGroupId == userGroupNum);
		}

		/// <summary>
		/// Gets a list of GroupPermissions for the supplied UserGroupNum without using the local cache. 
		/// Useful for multithreaded connections.
		/// </summary>
		public static List<GroupPermission> GetPermsNoCache(long userGroupId)
		{
			DataTable tableGroupPerms = Database.ExecuteDataTable("SELECT * FROM grouppermission WHERE UserGroupNum=" + userGroupId);

			return Crud.GroupPermissionCrud.TableToList(tableGroupPerms);
		}

		/// <summary>
		/// Gets a list of GroupPermissions that are associated with reports.  Uses Reports (22) permission.
		/// </summary>
		public static List<GroupPermission> GetPermsForReports()
		{
			return GetWhere(x => x.Permission == Permissions.Reports && x.ObjectId != 0);
		}

		/// <summary>
		/// Used to check if user has permission to access the report. 
		/// Pass in a list of DisplayReports to avoid a call to the db.
		/// </summary>
		public static bool HasReportPermission(string reportName, Userod user, List<DisplayReport> reports = null)
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

			var reportPermissions = GetPermsForReports();
			if (reportPermissions.Exists(x => x.ObjectId == report.DisplayReportNum && Userods.IsInUserGroup(user.Id, x.UserGroupId)))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Determines whether a single userGroup contains a specific permission.
		/// </summary>
		public static bool HasPermission(long userGroupId, Permissions permission, long? objectId)
		{
			var groupPermission = GetFirstOrDefault(x => x.UserGroupId == userGroupId && x.Permission == permission && x.ObjectId == objectId);

			return groupPermission != null;
		}

		/// <summary>
		/// Determines whether an individual user has a specific permission.
		/// </summary>
		public static bool HasPermission(Userod user, Permissions permission, long? objectId)
		{
			var groupPermission = GetFirstOrDefault(x => x.Permission == permission && x.ObjectId == objectId && user.IsInUserGroup(x.UserGroupId));

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
