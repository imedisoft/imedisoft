using CodeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace OpenDentBusiness
{
    public class Security
	{
		/// <summary>
		/// The permissions that are affected by the global date lock.
		/// </summary>
		private static readonly List<Permissions> globalLockPermissions = new List<Permissions>
		{
			Permissions.AdjustmentCreate,
			Permissions.AdjustmentEdit,
			Permissions.PaymentCreate,
			Permissions.PaymentEdit,
			Permissions.ProcCompleteEdit,
			Permissions.ProcCompleteStatusEdit,
			Permissions.ProcComplCreate,
			Permissions.InsPayCreate,
			Permissions.InsPayEdit,
			Permissions.SheetEdit,
			Permissions.SheetDelete,
			Permissions.CommlogEdit,
			Permissions.PayPlanEdit
		};

		private static Userod currentUser;

		/// <summary>
		/// The last local datetime that there was any mouse or keyboard activity.
		/// Used for auto logoff comparison and for disabling signal processing due to inactivity.
		/// Must be public so that it can be accessed from multiple application level classes.
		/// </summary>
		public static DateTime DateTimeLastActivity;

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        public static Userod CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;

                UserodChangedEvent.Fire(EventCategory.Userod, currentUser?.Id ?? 0);
            }
        }

        /// <summary>
        /// Gets a value indicating whether a user has logged in.
        /// </summary>
        public static bool IsUserLoggedIn => currentUser != null;

		/// <summary>
		/// Checks whether the current user has the specified <paramref name="permission"/>.
		/// </summary>
		/// <param name="permission">The permission to check.</param>
		/// <param name="silent">A value indicating whether to supress the error message box.</param>
		/// <returns>True if the current user has the specified permission.</returns>
		public static bool IsAuthorized(Permissions permission, bool silent = false) 
			=> IsAuthorized(permission, DateTime.UtcNow, silent, currentUser, null, 0, null, null);

		/// <summary>
		/// Checks whether the current user has the specified <paramref name="permission"/> for the object with the given <paramref name="objectId"/>.
		/// </summary>
		/// <param name="permission">The permission to check.</param>
		/// <param name="objectId">The ID of the object.</param>
		/// <param name="silent">A value indicating whether to supress the error message box.</param>
		/// <returns>True if the current user has the specified permission.</returns>
		public static bool IsAuthorized(Permissions permission, long objectId, bool silent = false) 
			=> IsAuthorized(permission, DateTime.UtcNow, silent, currentUser, 0, -1, 0, objectId);

		/// <summary>
		/// Checks whether the current user had the specified <paramref name="permission"/> on the given <paramref name="date"/>.
		/// </summary>
		/// <param name="permission">The permission to check.</param>
		/// <param name="date">The date to check.</param>
		/// <param name="silent">A value indicating whether to supress the error message box.</param>
		/// <returns>True if the current user has the specified permission.</returns>
		public static bool IsAuthorized(Permissions permission, DateTime date, bool silent = false) 
			=> IsAuthorized(permission, date, silent, currentUser, null, 0, null, null);

		public static bool IsAuthorized(Permissions permission, DateTime date, long procedureCodeId, double procedureFee) 
			=> IsAuthorized(permission, date, false, currentUser, procedureCodeId, procedureFee, null, null);
		
		public static bool IsAuthorized(Permissions permission, DateTime date, bool silent, long? procedureCodeId, double procedureFee, long? sheetDefinitionId, long? objectId) 
			=> IsAuthorized(permission, date, silent, currentUser, procedureCodeId, procedureFee, sheetDefinitionId, objectId);

		/// <summary>
		/// Checks whether the specified <paramref name="user"/> has the specified <paramref name="permission"/>.
		/// </summary>
		/// <param name="permission">The permission to check.</param>
		/// <param name="date">The date to check.</param>
		/// <param name="silent">A value indicating whether to supress the error message box.</param>
		/// <param name="user">The user.</param>
		/// <param name="procedureCodeId"></param>
		/// <param name="procedureFee"></param>
		/// <param name="sheetDefinitionId"></param>
		/// <param name="objectId">The ID of the object. Set to NULL to check global permission.</param>
		/// <returns>True if the current user has the specified permission.</returns>
		public static bool IsAuthorized(Permissions permission, DateTime date, bool silent,  Userod user, long? procedureCodeId, double procedureFee, long? sheetDefinitionId, long? objectId)
		{
            try
            {
				EnsureAuthorized(permission, date, user, procedureCodeId, procedureFee, sheetDefinitionId, objectId);

				return true;
            }
			catch (Exception exception)
            {
				if (false == silent)
                {
					MessageBox.Show(exception.Message);
                }

				return false;
            }
		}

		/// <summary>
		///		<para>
		///			Ensures the specified <paramref name="user"/> is authorized for the given <paramref name="permission"/>.
		///		</para>
		///		<para>
		///			Throws an exception if the <paramref name="user"/> is not authorized.
		///		</para>
		/// </summary>
		/// <param name="permission"></param>
		/// <param name="date"></param>
		/// <param name="user"></param>
		/// <param name="procedureCodeId"></param>
		/// <param name="procedureFee"></param>
		/// <param name="sheetDefinitionId"></param>
		/// <param name="objectId"></param>
		public static void EnsureAuthorized(Permissions permission, DateTime date, Userod user, long? procedureCodeId, double procedureFee, long? sheetDefinitionId, long? objectId)
		{
			if (user == null || !GroupPermissions.HasPermission(user, permission, objectId))
			{
				throw new Exception(Imedisoft.Translation.Common.InsufficientPrivileges);
			}

			date = date.Date;
			if (permission == Permissions.AccountingCreate || permission == Permissions.AccountingEdit)
			{
				var accountingLockDate = Prefs.GetDateTimeOrNull(PrefName.AccountingLockDate);
				if (accountingLockDate.HasValue && date <= accountingLockDate)
				{
					throw new Exception(Imedisoft.Translation.Common.LockedByAdministrator);
				}
			}

			// Make sure there is no global lock date active for this permission.
			EnsureIsNotGlobalDateLock(permission, date, procedureCodeId, procedureFee, sheetDefinitionId);

			// Check date/days limits on individual permission.
			if (!GroupPermissions.PermissionTakesDates(permission))
			{
				return;
			}

			// Include CEMT users, as a CEMT user could be logged in when this is checked.
			var dateAuthorized = GroupPermissions.GetDateRestrictedForPermission(permission, user.GetGroups(true).Select(x => x.Id).ToList());
			if (!dateAuthorized.HasValue || date > dateAuthorized)
			{
				return;
			}

			throw new Exception(
				string.Format(
					Imedisoft.Translation.Common.OperationNowAllowedForReason, 
					Imedisoft.Translation.Common.DateLimitation));
		}

		/// <summary>
		///		<para>
		///			Ensures no global date lock is active for the specified <paramref name="permission"/> on the given <paramref name="date"/>.
		///		</para>
		///		<para>
		///			Throws an exception if a global date lock is active for the given <paramref name="permission"/>.
		///		</para>
		/// </summary>
		/// <param name="permission">The permission.</param>
		/// <param name="date">The date to check.</param>
		/// <param name="procedureCodeId"></param>
		/// <param name="procedureFee"></param>
		/// <param name="sheetDefinitionId"></param>
		/// <exception cref="Exception">If a global date lock is active for the given permission.</exception>"
		public static void EnsureIsNotGlobalDateLock(Permissions permission, DateTime date, long? procedureCodeId = null, double procedureFee = 0, long? sheetDefinitionId = null)
		{
			// Permission being checked is not affected by global lock date.
			if (!globalLockPermissions.Contains(permission)) return;

			// Admins are never affected by global date limitation when preference is false.
			if (!Prefs.GetBool(PrefName.SecurityLockIncludesAdmin) && GroupPermissions.HasPermission(currentUser, Permissions.SecurityAdmin, 0))
			{
				return;
			}

			if (procedureCodeId.HasValue && ProcedureCodes.CanBypassLockDate(procedureCodeId.Value, procedureFee))
			{
				return;
			}

			if (sheetDefinitionId.HasValue && permission.In(Permissions.SheetEdit, Permissions.SheetDelete) && SheetDefs.CanBypassLockDate(sheetDefinitionId.Value))
			{
				return;
			}

			// If global lock is Date based.
			var lockDate = Prefs.GetDateTimeOrNull(PrefName.SecurityLockDate);
			if (lockDate.HasValue && date <= lockDate)
			{
				throw new Exception(
					string.Format(
						Imedisoft.Translation.Common.LockedByAdministratorForDate, 
						lockDate.Value.ToShortDateString()));
			}

			// If global lock is days based.
			var lockDays = Prefs.GetInt(PrefName.SecurityLockDays);
			if (lockDays > 0 && date <= DateTime.Today.AddDays(-lockDays))
			{
				throw new Exception(
					string.Format(
						Imedisoft.Translation.Common.LockedByAdministratorForDays, 
						lockDays));
			}
		}

		public static bool IsGlobalDateLock(Permissions permission, DateTime date, bool silent = false, long procedureCodeId = 0, double procedureFee = -1, long? sheetDefinitionId = null)
        {
            try
            {
				EnsureIsNotGlobalDateLock(permission, date, procedureCodeId, procedureFee, sheetDefinitionId);

				return true;
            }
			catch (Exception exception)
            {
				if (false == silent)
                {
					MessageBox.Show(exception.Message);
                }

				return false;
            }
        }

		/// <summary>
		///		<para>
		///			Gets the index of the first module the user is authorized to use.
		///		</para>
		/// </summary>
		/// <param name="suggestedModuleIndex">The index of the module to test first.</param>
		/// <returns>
		///		<para>
		///			The index of the module the user is authorized to use. 
		///			Or -1 if the user is not authorized for any modules.
		///		</para>
		/// </returns>
		public static int GetModule(int suggestedModuleIndex)
		{
			static Permissions GetModulePermission(int moduleIndex)
				=> moduleIndex switch
				{
					0 => Permissions.AppointmentsModule,
					1 => Permissions.FamilyModule,
					2 => Permissions.AccountModule,
					3 => Permissions.TPModule,
					4 => Permissions.ChartModule,
					5 => Permissions.ImagesModule,
					6 => Permissions.ManageModule,
					_ => Permissions.None,
				};

			if (suggestedModuleIndex != -1 && IsAuthorized(GetModulePermission(suggestedModuleIndex), DateTime.MinValue, true))
			{
				return suggestedModuleIndex;
			}

			for (int i = 0; i < 7; i++)
			{
				if (IsAuthorized(GetModulePermission(i), DateTime.MinValue, true))
				{
					return i;
				}
			}

			return -1;
		}
	}
}
