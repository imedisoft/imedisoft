using CodeBase;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace OpenDentBusiness
{
	public class Security
	{
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
		public static bool IsUserLoggedIn => CurrentUser != null;

		/// <summary>
		/// Checks to see if current user is authorized.
		/// It also checks any date restrictions.
		/// If not authorized, it gives a Message box saying so and returns false.
		/// </summary>
		public static bool IsAuthorized(Permissions perm)
		{
			return IsAuthorized(perm, DateTime.MinValue, false);
		}

		/// <summary>
		/// Checks to see if current user is authorized for the permission and corresponding FKey.
		/// If not authorized, it gives a Message box saying so and returns false.
		/// </summary>
		public static bool IsAuthorized(Permissions perm, long fKey, bool suppressMessage)
		{
			return IsAuthorized(perm, DateTime.MinValue, suppressMessage, true, 0, -1, 0, fKey);
		}

		/// <summary>
		/// Checks to see if current user is authorized.
		/// It also checks any date restrictions.
		/// If not authorized, it gives a Message box saying so and returns false.
		/// </summary>
		public static bool IsAuthorized(Permissions perm, DateTime date)
		{
			return IsAuthorized(perm, date, false);
		}

		/// <summary>
		/// Checks to see if current user is authorized.
		/// It also checks any date restrictions.
		/// If not authorized, it gives a Message box saying so and returns false.
		/// </summary>
		public static bool IsAuthorized(Permissions perm, bool suppressMessage)
		{
			return IsAuthorized(perm, DateTime.MinValue, suppressMessage);
		}

		/// <summary>
		/// Checks to see if current user is authorized. 
		/// It also checks any date restrictions. 
		/// If not authorized, it gives a Message box saying so and returns false.
		/// </summary>
		public static bool IsAuthorized(Permissions perm, DateTime date, bool suppressMessage)
		{
			return IsAuthorized(perm, date, suppressMessage, false);
		}

		/// <summary>
		/// Checks to see if current user is authorized.
		/// It also checks any date restrictions.
		/// If not authorized, it gives a Message box saying so and returns false.
		/// </summary>
		public static bool IsAuthorized(Permissions perm, DateTime date, bool suppressMessage, bool suppressLockDateMessage)
		{
			return IsAuthorized(perm, date, suppressMessage, suppressLockDateMessage, 0, -1, 0, 0);
		}

		public static bool IsAuthorized(Permissions perm, DateTime date, long procCodeNum, double procCodeFee)
		{
			return IsAuthorized(perm, date, false, false, procCodeNum, procCodeFee, 0, 0);
		}

		/// <summary>
		/// Checks to see if current user is authorized. 
		/// It also checks any date restrictions. 
		/// If not authorized, it gives a Message box saying so and returns false.
		/// </summary>
		public static bool IsAuthorized(Permissions perm, DateTime date, bool suppressMessage, bool suppressLockDateMessage, long procCodeNum, double procCodeFee, long sheetDefNum, long fKey)
		{
			if (CurrentUser == null)
			{
				if (!suppressMessage)
				{
					MessageBox.Show("Not authorized for\r\n" + GroupPermissions.GetDesc(perm));
				}

				return false;
			}

			try
			{
				return IsAuthorized(perm, date, suppressMessage, suppressLockDateMessage, currentUser, procCodeNum, procCodeFee, sheetDefNum, fKey);
			}
			catch (Exception ex)
			{
				if (!suppressMessage)
				{
					MessageBox.Show(ex.Message);
				}
				return false;
			}
		}

		/// <summary>
		/// Will throw an error if not authorized and message not suppressed.
		/// </summary>
		public static bool IsAuthorized(Permissions perm, DateTime date, bool suppressMessage, bool suppressLockDateMessage, Userod curUser, long procCodeNum, double procFee, long sheetDefNum, long fKey)
		{
			date = date.Date; //Remove the time portion of date so we can compare strictly as a date later.

			// Check eConnector permission first.
			if (IsValidEServicePermission(perm))
			{
				return true;
			}

			if (!GroupPermissions.HasPermission(curUser, perm, fKey))
			{
				if (!suppressMessage)
				{
					throw new Exception(
						"Not authorized.\r\n" +
						"A user with the SecurityAdmin permission must grant you access for:\r\n" + 
						GroupPermissions.GetDesc(perm));
				}

				return false;
			}

			if (perm == Permissions.AccountingCreate || perm == Permissions.AccountingEdit)
			{
				if (date <= PrefC.GetDate(PrefName.AccountingLockDate))
				{
					if (!suppressMessage && !suppressLockDateMessage)
					{
						throw new Exception(Lans.g("Security", "Locked by Administrator."));
					}
					return false;
				}
			}

			//Check the global security lock------------------------------------------------------------------------------------
			if (IsGlobalDateLock(perm, date, suppressMessage || suppressLockDateMessage, procCodeNum, procFee, sheetDefNum))
			{
				return false;
			}
			//Check date/days limits on individual permission----------------------------------------------------------------
			if (!GroupPermissions.PermTakesDates(perm))
			{
				return true;
			}
			//Include CEMT users, as a CEMT user could be logged in when this is checked.
			DateTime dateLimit = GetDateLimit(perm, curUser.GetGroups(true).Select(x => x.Id).ToList());
			if (date > dateLimit)
			{//authorized
				return true;
			}

			//Prevents certain bugs when 1/1/1 dates are passed in and compared----------------------------------------------
			//Handling of min dates.  There might be others, but we have to handle them individually to avoid introduction of bugs.
			if (perm == Permissions.ClaimDelete//older versions did not have SecDateEntry
				|| perm == Permissions.ClaimSentEdit//no date sent was entered before setting claim received	
				|| perm == Permissions.ProcCompleteEdit
				|| perm == Permissions.ProcCompleteStatusEdit
				|| perm == Permissions.ProcCompleteNote
				|| perm == Permissions.ProcCompleteAddAdj
				|| perm == Permissions.ProcCompleteEditMisc
				|| perm == Permissions.ProcExistingEdit//a completed EO or EC procedure with a min date.
				|| perm == Permissions.InsPayEdit//a claim payment with no date.
				|| perm == Permissions.InsWriteOffEdit//older versions did not have SecDateEntry or DateEntryC
				|| perm == Permissions.TreatPlanEdit
				|| perm == Permissions.AdjustmentEdit
				|| perm == Permissions.CommlogEdit//usually from a conversion
				|| perm == Permissions.ProcDelete//because older versions did not set the DateEntryC.
				|| perm == Permissions.ImageDelete//In case an image has a creation date of DateTime.MinVal.
				|| perm == Permissions.PerioEdit//In case perio chart exam has a creation date of DateTime.MinValue.
				|| perm == Permissions.PreAuthSentEdit//older versions did not have SecDateEntry
				|| perm == Permissions.ClaimProcReceivedEdit//
				|| perm == Permissions.PaymentCreate)//Older versions did not have a date limitation to PaymentCreate
			{
				if (date.Year < 1880 && dateLimit.Year < 1880)
				{
					return true;
				}
			}

			if (!suppressMessage)
			{
				throw new Exception(
					"Not authorized for\r\n" + GroupPermissions.GetDesc(perm) + "\r\nDate limitation");
			}

			return false;
		}

		/// <summary>
		/// Surrond with Try/Catch. Error messages will be thrown to caller.
		/// </summary>
		public static bool IsGlobalDateLock(Permissions perm, DateTime date, bool isSilent = false, long codeNum = 0, double procFee = -1, long sheetDefNum = 0)
		{
			if (!(new[] {
				 Permissions.AdjustmentCreate
				,Permissions.AdjustmentEdit
				,Permissions.PaymentCreate
				,Permissions.PaymentEdit
				,Permissions.ProcComplCreate
				,Permissions.ProcCompleteEdit
				,Permissions.ProcCompleteStatusEdit
				//,Permissions.ProcComplNote (corresponds to obsolete ProcComplEditLimited)
				//,Permissions.ProcComplAddAdj (corresponds to obsolete ProcComplEditLimited)
				//,Permissions.ProcComplEditMisc (corresponds to obsolete ProcComplEditLimited)
				//,Permissions.ProcExistingEdit//per Allen 6/26/2020 this should not be affected by the global date lock
			//,Permissions.ImageDelete
				,Permissions.InsPayCreate
				,Permissions.InsPayEdit
			//,Permissions.InsWriteOffEdit//per Nathan 7/5/2016 this should not be affected by the global date lock
				,Permissions.SheetEdit
				,Permissions.SheetDelete
				,Permissions.CommlogEdit
			//,Permissions.ClaimDelete //per Nathan 01/18/2018 this should not be affected by the global date lock
				,Permissions.PayPlanEdit
			//,Permissions.ClaimHistoryEdit //per Nathan & Mark 03/01/2018 this should not be affected by the global lock date, not financial data.
			}).Contains(perm))
			{
				return false;//permission being checked is not affected by global lock date.
			}
			if (date.Year == 1)
			{
				return false;//Invalid or MinDate passed in.
			}
			if (!Prefs.GetBool(PrefName.SecurityLockIncludesAdmin) && GroupPermissions.HasPermission(Security.CurrentUser, Permissions.SecurityAdmin, 0))
			{
				return false;//admins are never affected by global date limitation when preference is false.
			}
			List<Permissions> listPermissionsCanBypassLockDate = new List<Permissions>() {
				Permissions.ProcCompleteEdit,Permissions.ProcCompleteAddAdj,Permissions.ProcCompleteEditMisc,Permissions.ProcCompleteStatusEdit,Permissions.ProcCompleteNote,
				Permissions.ProcComplCreate,Permissions.ProcExistingEdit
			};
			if (perm.In(listPermissionsCanBypassLockDate) && ProcedureCodes.CanBypassLockDate(codeNum, procFee))
			{
				return false;
			}
			if (perm.In(Permissions.SheetEdit, Permissions.SheetDelete) && sheetDefNum > 0 && SheetDefs.CanBypassLockDate(sheetDefNum))
			{
				return false;
			}

			//If global lock is Date based.
			if (date <= PrefC.GetDate(PrefName.SecurityLockDate))
			{
				if (!isSilent)
				{
					MessageBox.Show("Locked by Administrator before " + PrefC.GetDate(PrefName.SecurityLockDate).ToShortDateString());
				}
				return true;
			}

			//If global lock is days based.
			int lockDays = PrefC.GetInt(PrefName.SecurityLockDays);
			if (lockDays > 0 && date <= DateTime.Today.AddDays(-lockDays))
			{
				if (!isSilent)
				{
					MessageBox.Show("Locked by Administrator before " + lockDays.ToString() + " days.");
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns the Date that the user is restricted to for the passed-in PermType. 
		/// Returns MinVal if the user is not restricted or does not have the permission.
		/// </summary>
		private static DateTime GetDateLimit(Permissions permType, List<long> userGroupIds) 
			=> GroupPermissions.GetDateRestrictedForPermission(permType, userGroupIds);

		/// <summary>
		/// Gets a module that the user has permission to use.
		/// Tries the suggestedI first.
		/// If a -1 is supplied, it tries to find any authorized module.
		/// If no authorization for any module, it returns a -1, causing no module to be selected.
		/// </summary>
		public static int GetModule(int suggestI)
		{
			if (suggestI != -1 && IsAuthorized(PermofModule(suggestI), DateTime.MinValue, true))
			{
				return suggestI;
			}

			for (int i = 0; i < 7; i++)
			{
				if (IsAuthorized(PermofModule(i), DateTime.MinValue, true))
				{
					return i;
				}
			}

			return -1;
		}

		private static Permissions PermofModule(int moduleIndex) 
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

		#region eServices

		///<summary>Returns false if the currently logged in user is not designated for the eConnector or if the user does not have permission.</summary>
		private static bool IsValidEServicePermission(Permissions perm)
		{
			//No need to check RemotingRole; no call to db.
			if (currentUser == null)
			{
				return false;
			}
			//Run specific checks against certain types of eServices.
			switch (currentUser.EServiceType)
			{
				case EServiceTypes.Broadcaster:
				case EServiceTypes.BroadcastMonitor:
				case EServiceTypes.ServiceMainHQ:
					return true;//These eServices are at HQ and we trust ourselves to have full permissions for any S class method.
				case EServiceTypes.EConnector:
					return IsPermAllowedEConnector(perm);
				case EServiceTypes.None:
				default:
					return false;//Not an eService, let IsAuthorized handle the permission checking.
			}
		}

		///<summary>Returns true if the eConnector should be allowed to run methods with the passed in permission.</summary>
		private static bool IsPermAllowedEConnector(Permissions perm)
		{
			//We are typically on the customers eConnector and need to be careful when giving access to certain permission types.
			//Engineers must EXCPLICITLY add permissions to this switch statement as they need them.
			//Be very cautious when adding permissions because the flood gates for that permission will be opened once added.
			//E.g. we should never add a permission like Setup or SecurityAdmin.  If there is a need for such a thing, we need to rethink this paradigm.
			switch (perm)
			{
				//Add additional permissions to this case as needed to grant access.
				case Permissions.EmailSend:
					return true;
				default:
					return false;
			}
		}

		#endregion
	}
}
