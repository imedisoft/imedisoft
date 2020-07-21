using Imedisoft.Data;
using OpenDentBusiness.HL7;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{
    public class HL7Defs
	{
		private class HL7DefCache : CacheListAbs<HL7Def>
		{
			protected override List<HL7Def> GetCacheFromDb()
			{
				return Crud.HL7DefCrud.SelectMany("SELECT * FROM hl7def ORDER BY Description");
			}

			protected override List<HL7Def> TableToList(DataTable table)
			{
				return Crud.HL7DefCrud.TableToList(table);
			}

			protected override HL7Def Copy(HL7Def HL7Def)
			{
				return HL7Def.Clone();
			}

			protected override DataTable ListToTable(List<HL7Def> listHL7Defs)
			{
				return Crud.HL7DefCrud.ListToTable(listHL7Defs, "HL7Def");
			}

			protected override void FillCacheIfNeeded()
			{
				HL7Defs.GetTableFromCache(false);
			}
		}

		private static readonly HL7DefCache cache = new HL7DefCache();

		public static HL7Def GetFirstOrDefault(Func<HL7Def, bool> match, bool isShort = false)
		{
			return cache.GetFirstOrDefault(match, isShort);
		}

		public static DataTable RefreshCache()
		{
			return GetTableFromCache(true);
		}

		/// <summary>
		/// Always refreshes the ClientWeb's cache.
		/// </summary>
		public static DataTable GetTableFromCache(bool doRefreshCache)
		{
			return cache.GetTableFromCache(doRefreshCache);
		}

		/// <summary>
		/// Gets an internal HL7Def from the database of the specified type.
		/// </summary>
		public static HL7Def GetInternalFromDb(HL7InternalType internalType)
		{
			return Crud.HL7DefCrud.SelectOne(
				"SELECT * FROM hl7def WHERE IsInternal=1 AND InternalType='" + POut.String(internalType.ToString()) + "'");
		}

		public static List<HL7Def> GetListInternalFromDb()
		{
			return Crud.HL7DefCrud.SelectMany(
				"SELECT * FROM hl7def WHERE IsInternal=1");
		}

		/// <summary>
		/// Gets from cache.
		/// Will get all enabled defs that are not InternalType HL7InternalType.MedLabv2_3.
		/// Only one def that is not MedLabv2_3 can be enabled so this is guaranteed to return only one def.
		/// </summary>
		public static HL7Def GetOneDeepEnabled()
		{
			return GetOneDeepEnabled(false);
		}

		/// <summary>
		/// Gets from cache. 
		/// If isMedLabHL7 is true, this will only return the enabled def if it is HL7InternalType.MedLabv2_3.
		/// If false, then only those defs not of that type. 
		/// This will return null if no HL7defs are enabled. 
		/// Since only one can be enabled, this will return only one.
		/// </summary>
		public static HL7Def GetOneDeepEnabled(bool isMedLabHL7)
		{
			HL7Def result = GetFirstOrDefault(x => x.IsEnabled && isMedLabHL7 == (x.InternalType == HL7InternalType.MedLabv2_3));
			if (result == null)
			{
				return null;
			}

			if (result.IsInternal) // if internal, messages, segments, and fields will not be in the database
			{
				GetDeepForInternal(result);
			}
			else
			{
				result.hl7DefMessages = HL7DefMessages.GetDeepFromCache(result.HL7DefNum);
			}

			return result;
		}

		/// <summary>
		/// Gets a full deep list of all internal defs. 
		/// If one is enabled, then it might be in database.
		/// </summary>
		public static List<HL7Def> GetDeepInternalList()
		{
			List<HL7Def> listInternalDb = GetListInternalFromDb();
			List<HL7Def> retval = new List<HL7Def>();
			HL7Def def;

			// Whether or not the def was in the db, internal def messages, segments, and fields will not be in the db.
			foreach (HL7InternalType defType in Enum.GetValues(typeof(HL7InternalType)))
			{
				def = listInternalDb.Find(x => x.InternalType == defType); // might be null
				switch (defType)
				{
					case HL7InternalType.eCWFull:
						def = InternalEcwFull.GetDeepInternal(def);
						continue;

					case HL7InternalType.eCWStandalone:
						retval.Add(InternalEcwStandalone.GetDeepInternal(def));
						continue;

					case HL7InternalType.eCWTight:
						retval.Add(InternalEcwTight.GetDeepInternal(def));
						continue;

					case HL7InternalType.Centricity:
						retval.Add(InternalCentricity.GetDeepInternal(def));
						continue;

					case HL7InternalType.HL7v2_6:
						retval.Add(InternalHL7v2_6.GetDeepInternal(def));
						continue;

					case HL7InternalType.MedLabv2_3:
						retval.Add(MedLabv2_3.GetDeepInternal(def));
						continue;

					default:
						continue;
				}
			}
			return retval;
		}

		/// <summary>
		/// Gets from C# internal code rather than db
		/// </summary>
		private static void GetDeepForInternal(HL7Def def)
		{
			if (def.InternalType == HL7InternalType.eCWFull)
			{
				InternalEcwFull.GetDeepInternal(def); // def that we're passing in is guaranteed to not be null
			}
			else if (def.InternalType == HL7InternalType.eCWStandalone)
			{
				InternalEcwStandalone.GetDeepInternal(def);
			}
			else if (def.InternalType == HL7InternalType.eCWTight)
			{
				InternalEcwTight.GetDeepInternal(def);
			}
			else if (def.InternalType == HL7InternalType.Centricity)
			{
				InternalCentricity.GetDeepInternal(def);
			}
			else if (def.InternalType == HL7InternalType.HL7v2_6)
			{
				InternalHL7v2_6.GetDeepInternal(def);
			}
			else if (def.InternalType == HL7InternalType.MedLabv2_3)
			{
				MedLabv2_3.GetDeepInternal(def);
			}
		}

		/// <summary>
		/// Tells us whether there is an existing enabled HL7Def, excluding the def with excludeHL7DefNum.
		/// If isMedLabHL7 is true, this will only check to see if a def of type HL7InternalType.MedLabv2_3 is enabled.
		/// Otherwise, only defs not of that type will be checked.
		/// </summary>
		public static bool IsExistingHL7Enabled(long excludeHL7DefNum, bool isMedLabHL7)
		{

			string command = "SELECT COUNT(*) FROM hl7def WHERE IsEnabled=1 AND HL7DefNum != " + excludeHL7DefNum;
			if (isMedLabHL7)
			{
				command += " AND InternalType='" + POut.String(HL7InternalType.MedLabv2_3.ToString()) + "'";
			}
			else
			{
				command += " AND InternalType!='" + POut.String(HL7InternalType.MedLabv2_3.ToString()) + "'";
			}

			if (Database.ExecuteLong(command) == 0)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Tells us whether there is an existing enabled HL7Def that is not HL7InternalType.MedLabv2_3.
		/// </summary>
		public static bool IsExistingHL7Enabled()
		{
			return cache.GetWhere(x => x.IsEnabled && x.InternalType != HL7InternalType.MedLabv2_3).Count > 0;
		}

		/// <summary>
		/// Gets a full deep list of all defs that are not internal from the database.
		/// </summary>
		public static List<HL7Def> GetDeepCustomList()
		{
			List<HL7Def> customList = GetShallowFromDb();

			for (int d = 0; d < customList.Count; d++)
			{
				customList[d].hl7DefMessages = HL7DefMessages.GetDeepFromDb(customList[d].HL7DefNum);
			}

			return customList;
		}

		/// <summary>
		/// Gets shallow list of all defs that are not internal from the database
		/// </summary>
		public static List<HL7Def> GetShallowFromDb()
		{
			return Crud.HL7DefCrud.SelectMany("SELECT * FROM hl7def WHERE IsInternal=0");
		}

		/// <summary>
		/// Only used from Unit Tests. 
		/// Since we clear the db of hl7Defs we have to insert this internal def not update it.
		/// </summary>
		public static void EnableInternalForTests(HL7InternalType internalType)
		{
			HL7Def hl7Def = null;
			List<HL7Def> defList = GetDeepInternalList();

			for (int i = 0; i < defList.Count; i++)
			{
				if (defList[i].InternalType == internalType)
				{
					hl7Def = defList[i];
					break;
				}
			}

			if (hl7Def == null)
			{
				return;
			}

			hl7Def.IsEnabled = true;
			Insert(hl7Def);
		}

		public static long Insert(HL7Def hL7Def)
		{
			return Crud.HL7DefCrud.Insert(hL7Def);
		}

		public static void Update(HL7Def hL7Def)
		{
			Crud.HL7DefCrud.Update(hL7Def);
		}

		public static void Delete(long hL7DefNum)
		{
			Database.ExecuteNonQuery("DELETE FROM hl7def WHERE HL7DefNum = " + hL7DefNum);
		}
	}
}
