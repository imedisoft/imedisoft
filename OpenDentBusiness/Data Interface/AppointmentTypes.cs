using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class AppointmentTypes
	{
		#region Get Methods

		///<summary>Returns an empty string for invalid AppointmentTypeNum.  Appends (hidden) to the end of the name if necessary.</summary>
		public static string GetName(long AppointmentTypeNum)
		{
			//No need to check RemotingRole; no call to db.
			string retVal = "";
			AppointmentType appointmentType = GetFirstOrDefault(x => x.Id == AppointmentTypeNum);
			if (appointmentType != null)
			{
				retVal = appointmentType.Name + (appointmentType.Hidden ? " " + "(hidden)" : "");
			}
			return retVal;
		}

		///<summary>Returns the time pattern for the specified appointment type (time pattern returned will always be in 5 min increments).
		///If the Pattern variable is not set on the appointment type object then the pattern will be dynamically calculated.
		///Optionally pass in provider information in order to use specific provider time patterns.</summary>
		public static string GetTimePatternForAppointmentType(AppointmentType appointmentType, long provNumDentist = 0, long provNumHyg = 0)
		{
			//No need to check RemotingRole; no call to db.
			string timePattern = "";
			if (string.IsNullOrEmpty(appointmentType.Pattern))
			{
				//Dynamically calculate the timePattern from the procedure codes associated to the appointment type passed in.
				List<string> listProcCodeStrings = appointmentType.ProcedureCodes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
				List<ProcedureCode> listProcCodes = new List<ProcedureCode>();
				listProcCodeStrings.ForEach(x => listProcCodes.Add(ProcedureCodes.GetProcCode(x)));
				timePattern = OpenDentBusiness.Appointments.CalculatePattern(provNumDentist, provNumHyg, listProcCodes.Select(x => x.Id).ToList(), true);
			}
			else
			{
				timePattern = appointmentType.Pattern;//Already in 5 minute increment so no conversion required.
			}
			return timePattern;
		}

		///<summary>Returns the appointment type associated to the definition passed in.  Returns null if no match found.</summary>
		public static AppointmentType GetWebSchedNewPatApptTypeByDef(long defNum)
		{
			//No need to check RemotingRole; no call to db.
			List<DefLink> listDefLinks = DefLinks.GetDefLinksByType(DefLinkType.AppointmentType);
			DefLink defLink = listDefLinks.FirstOrDefault(x => x.DefinitionId == defNum);
			if (defLink == null)
			{
				return null;
			}
			return AppointmentTypes.GetFirstOrDefault(x => x.Id == defLink.FKey, true);
		}

		#endregion



		#region CachePattern

		private class AppointmentTypeCache : CacheListAbs<AppointmentType>
		{
			protected override List<AppointmentType> GetCacheFromDb()
			{
				string command = "SELECT * FROM appointmenttype ORDER BY ItemOrder";
				return Crud.AppointmentTypeCrud.SelectMany(command);
			}
			protected override List<AppointmentType> TableToList(DataTable table)
			{
				return Crud.AppointmentTypeCrud.TableToList(table);
			}
			protected override AppointmentType Copy(AppointmentType appointmentType)
			{
				return appointmentType.Copy();
			}
			protected override DataTable ListToTable(List<AppointmentType> listAppointmentTypes)
			{
				return Crud.AppointmentTypeCrud.ListToTable(listAppointmentTypes, "AppointmentType");
			}
			protected override void FillCacheIfNeeded()
			{
				AppointmentTypes.GetTableFromCache(false);
			}
			protected override bool IsInListShort(AppointmentType appointmentType)
			{
				return !appointmentType.Hidden;
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static AppointmentTypeCache _appointmentTypeCache = new AppointmentTypeCache();

		public static List<AppointmentType> GetDeepCopy(bool isShort = false)
		{
			return _appointmentTypeCache.GetDeepCopy(isShort);
		}

		public static AppointmentType GetFirstOrDefault(Func<AppointmentType, bool> match, bool isShort = false)
		{
			return _appointmentTypeCache.GetFirstOrDefault(match, isShort);
		}

		public static List<AppointmentType> GetWhere(Predicate<AppointmentType> match, bool isShort = false)
		{
			return _appointmentTypeCache.GetWhere(match, isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache()
		{
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table)
		{
			_appointmentTypeCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache)
		{
			return _appointmentTypeCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		#region Sync Pattern

		///<summary>Inserts, updates, or deletes database rows to match supplied list.</summary>
		public static void Sync(List<AppointmentType> listNew, List<AppointmentType> listOld)
		{
			Crud.AppointmentTypeCrud.Sync(listNew, listOld);
		}

		#endregion

		public static AppointmentType GetOne(long appointmentTypeNum)
		{
			return GetFirstOrDefault(x => x.Id == appointmentTypeNum);
		}

		public static long Insert(AppointmentType appointmentType)
		{
			return Crud.AppointmentTypeCrud.Insert(appointmentType);
		}

		public static void Update(AppointmentType appointmentType)
		{
			Crud.AppointmentTypeCrud.Update(appointmentType);
		}

		///<summary>Surround with try catch.</summary>
		public static void Delete(long appointmentTypeNum)
		{
			string s = CheckInUse(appointmentTypeNum);
			if (s != "")
			{
				throw new ApplicationException(s);
			}
			string command = "DELETE FROM appointmenttype WHERE AppointmentTypeNum = " + POut.Long(appointmentTypeNum);
			Database.ExecuteNonQuery(command);
		}

		///<summary>Used when attempting to delete.  Returns empty string if not in use and an untranslated string if in use.</summary>
		public static string CheckInUse(long appointmentTypeNum)
		{
			string command = "SELECT COUNT(*) FROM appointment WHERE AppointmentTypeNum = " + POut.Long(appointmentTypeNum);
			if (PIn.Int(Database.ExecuteString(command)) > 0)
			{
				return "Not allowed to delete appointment types that are in use on an appointment.";
			}
			command = "SELECT COUNT(*) FROM deflink "
				+ "WHERE LinkType = " + POut.Int((int)DefLinkType.AppointmentType) + " "
				+ "AND FKey = " + POut.Long(appointmentTypeNum) + " ";
			if (PIn.Int(Database.ExecuteString(command)) > 0)
			{
				//This message will need to change in the future if more definition categories utilize appointment types with the deflink table.
				return "Not allowed to delete appointment types that are in use by Web Sched New Pat Appt Types definitions.";
			}
			return "";
		}

		public static int SortItemOrder(AppointmentType a1, AppointmentType a2)
		{
			if (a1.ItemOrder != a2.ItemOrder)
			{
				return a1.ItemOrder.CompareTo(a2.ItemOrder);
			}
			return a1.Id.CompareTo(a2.Id);
		}
	}
}
