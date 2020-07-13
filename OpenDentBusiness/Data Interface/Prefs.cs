using CodeBase;
using DataConnectionBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness
{
    public class Prefs
	{
		/// <summary>
		/// Utilizes the NonPkAbs version of CacheDict because it uses PrefName instead of the PK PrefNum.
		/// </summary>
		private class PrefCache : CacheDictNonPkAbs<Pref, string, Pref>
		{
			protected override List<Pref> GetCacheFromDb() 
				=> Crud.PrefCrud.SelectMany("SELECT * FROM preference");

			protected override List<Pref> TableToList(DataTable table)
			{
				var preferences = new List<Pref>();

				bool containsPrefNum = table.Columns.Contains("PrefNum");

				foreach (DataRow row in table.Rows)
				{
					var preference = new Pref();
					if (containsPrefNum)
					{
						preference.PrefNum = SIn.Long(row["PrefNum"].ToString());
					}

					preference.PrefName = SIn.String(row["PrefName"].ToString());
					preference.ValueString = SIn.String(row["ValueString"].ToString());

					preferences.Add(preference);
				}

				return preferences;
			}

			protected override Pref Copy(Pref preference) 
				=> preference.Copy();
			
			protected override DataTable DictToTable(Dictionary<string, Pref> preferences) 
				=> Crud.PrefCrud.ListToTable(preferences.Values.ToList(), "Pref");
			
			protected override void FillCacheIfNeeded() 
				=> Prefs.GetTableFromCache(false);

			protected override string GetDictKey(Pref preference) 
				=> preference.PrefName;

			protected override Pref GetDictValue(Pref preference) 
				=> preference;

			protected override Pref CopyDictValue(Pref preference) 
				=> preference.Copy();
			
			protected override Dictionary<string, Pref> GetDictFromList(List<Pref> preferences)
			{
				var preferencesDict = new Dictionary<string, Pref>();
				var duplicatePreferences = new List<string>();

				foreach (var preference in preferences)
				{
					if (preferencesDict.ContainsKey(preference.PrefName))
					{
						duplicatePreferences.Add(preference.PrefName);//The current preference is a duplicate preference.
					}
					else
					{
						preferencesDict.Add(preference.PrefName, preference);
					}
				}

				if (duplicatePreferences.Count > 0)
				{
					throw new ApplicationException(
						"Duplicate preferences found in database: " + string.Join(",", duplicatePreferences));
				}

				return preferencesDict;
			}

			protected override DataTable ListToTable(List<Pref> preferences) 
				=> Crud.PrefCrud.ListToTable(preferences);
		}

		/// <summary>
		/// The default cache that will be used if _dictCachesForDbs is not being used.
		/// </summary>
		private static readonly PrefCache defaultPreferenceCache = new PrefCache();
		
		/// <summary>
		/// A dictionary that stores a different cache for each database connection.
		/// This exists here in Prefs because some applications switch back and forth between DentalOffice and Customers.
		/// </summary>
		private static readonly Dictionary<ConnectionNames, PrefCache> preferenceCaches = new Dictionary<ConnectionNames, PrefCache>();

		/// <summary>
		/// The object that accesses the cache in a thread-safe manner.
		/// </summary>
		private static PrefCache Cache
		{
			get
			{
                if (preferenceCaches.TryGetValue(ConnectionStore.CurrentConnection, out PrefCache cache))
                {
                    return cache;
                }

                return defaultPreferenceCache;
			}
		}

		public static bool GetContainsKey(string prefName) 
			=> Cache.GetContainsKey(prefName);
		
		public static bool DictIsNull() 
			=> Cache.DictIsNull();
		
		/// <summary>
		/// Returns a deep copy of the corresponding preference by name from the cache.
		/// Throws an exception indicating that the prefName passed in is invalid if it cannot be found in the cache (old behavior).
		/// </summary>
		public static Pref GetOne(PrefName preferenceName) => GetOne(preferenceName.ToString());

		/// <summary>
		/// Returns a deep copy of the corresponding preference by name from the cache.
		/// Throws an exception indicating that the prefName passed in is invalid if it cannot be found in the cache (old behavior).
		/// </summary>
		public static Pref GetOne(string preferenceName)
		{
			if (!Cache.GetContainsKey(preferenceName))
			{
				throw new Exception(preferenceName + " is an invalid pref name.");
			}

			return Cache.GetOne(preferenceName);
		}

		/// <summary>
		/// Returns a deep copy list of the corresponding preferences by name from the cache.
		/// </summary>
		public static List<Pref> GetPrefs(List<string> preferenceNames)
		{
			if (preferenceNames == null || preferenceNames.Count == 0)
			{
				return new List<Pref>();
			}

			return Cache.GetWhere(x => x.PrefName.In(preferenceNames));
		}

		public static DataTable RefreshCache() => GetTableFromCache(true);

		/// <summary>
		/// Fills the local cache with the passed in DataTable.
		/// </summary>
		public static void FillCacheFromTable(DataTable table) 
			=> Cache.FillCacheFromTable(table);

		/// <summary>
		/// Always refreshes the ClientWeb's cache.
		/// </summary>
		public static DataTable GetTableFromCache(bool refreshCache) 
			=> Cache.GetTableFromCache(refreshCache);

		public static void UpdateValueForKey(Pref preference) 
			=> Cache.SetValueForKey(preference.PrefName, preference);

		/// <summary>
		/// Gets a pref of type bool without using the cache.
		/// </summary>
		public static bool GetBoolNoCache(PrefName preferenceName) => SIn.Bool(Db.GetScalar(
			"SELECT ValueString FROM preference WHERE PrefName = '" + SOut.String(preferenceName.ToString()) + "'"));

		public static void Update(Pref preference) => Db.NonQ(
			"UPDATE preference SET ValueString = '" + SOut.String(preference.ValueString) + "' " +
			"WHERE PrefName = '" + SOut.String(preference.PrefName) + "'");

		/// <summary>
		/// Updates a pref of type int. Returns true if a change was required, or false if no change needed.
		/// </summary>
		public static bool UpdateInt(PrefName preferenceName, int newValue) 
			=> UpdateLong(preferenceName, newValue);
		
		/// <summary>
		/// Updates a pref of type YN. Returns true if a change was required, or false if no change needed.
		/// </summary>
		public static bool UpdateYN(PrefName preferenceName, YN newValue) 
			=> UpdateLong(preferenceName, (int)newValue);
		
		/// <summary>
		/// Updates a pref of type YN.  Returns true if a change was required, or false if no change needed.
		/// </summary>
		[Obsolete("UI logic shouldn't be here...")]
		public static bool UpdateYN(PrefName preferenceName, System.Windows.Forms.CheckState checkState)
		{
			YN yn = YN.Unknown;
			if (checkState == System.Windows.Forms.CheckState.Checked)
			{
				yn = YN.Yes;
			}

			if (checkState == System.Windows.Forms.CheckState.Unchecked)
			{
				yn = YN.No;
			}

			return UpdateYN(preferenceName, yn);
		}

		/// <summary>
		/// Updates a pref of type byte. Returns true if a change was required, or false if no change needed.
		/// </summary>
		public static bool UpdateByte(PrefName preferenceName, byte newValue) 
			=> UpdateLong(preferenceName, newValue);

		/// <summary>
		/// Updates a pref of type int without using the cache. Useful for multithreaded connections.
		/// </summary>
		public static void UpdateIntNoCache(PrefName preferenceName, int newValue) => Db.NonQ(
			"UPDATE preference SET ValueString='" + SOut.Long(newValue) + "' " +
			"WHERE PrefName='" + SOut.String(preferenceName.ToString()) + "'");

		/// <summary>
		/// Updates a pref of type long. 
		/// Returns true if a change was required, or false if no change needed.
		/// </summary>
		public static bool UpdateLong(PrefName prefName, long newValue)
		{
			var currentValue = PrefC.GetLong(prefName);
			if (currentValue == newValue)
			{
				return false;
			}

			Db.NonQ(
				"UPDATE preference SET ValueString = '" + SOut.Long(newValue) + "' " +
				"WHERE PrefName = '" + SOut.String(prefName.ToString()) + "'");

            UpdateValueForKey(new Pref
			{
				PrefName = prefName.ToString(),
				ValueString = newValue.ToString()
			});

			return true;
		}

		/// <summary>
		/// For saving from the UI when we want "0" or "" to be saved as a -1 in the database.
		/// </summary>
		public static bool UpdateLongAsNegOne(PrefName preferenceName, string newValue)
		{
			long val = -1;
			if (!string.IsNullOrWhiteSpace(newValue))
			{
				val = SIn.Long(newValue) > 0 ? SIn.Long(newValue) : -1;
			}

			return UpdateLong(preferenceName, val);
		}

		/// <summary>
		/// Updates a pref of type double.
		/// Returns true if a change was required, or false if no change needed.
		/// Set doRounding false when the double passed in needs to be Multiple Precision Floating-Point Reliable (MPFR).
		/// Set doUseEnUSFormat to true to use a period no matter what region.
		/// </summary>
		public static bool UpdateDouble(PrefName preferenceName, double newValue, bool doRounding = true, bool doUseEnUSFormat = false)
		{
			var currentValue = PrefC.GetDouble(preferenceName, doUseEnUSFormat);
			if (currentValue == newValue)
			{
				return false;
			}

			Db.NonQ(
				"UPDATE preference SET ValueString = '" + SOut.Double(newValue, doRounding, doUseEnUSFormat) + "' " +
				"WHERE PrefName = '" + SOut.String(preferenceName.ToString()) + "'");

            UpdateValueForKey(new Pref
			{
				PrefName = preferenceName.ToString(),
				ValueString = newValue.ToString()
			});

			return true;
		}

		/// <summary>
		/// Returns true if a change was required, or false if no change needed.
		/// </summary>
		public static bool UpdateBool(PrefName preferenceName, bool newValue, bool force = false)
		{
			var currentValue = PrefC.GetBool(preferenceName);
			if (!force && currentValue == newValue)
			{
				return false;
			}

			Db.NonQ(
				 "UPDATE preference SET ValueString = '" + SOut.Bool(newValue) + "' " +
				 "WHERE PrefName = '" + SOut.String(preferenceName.ToString()) + "'");

            UpdateValueForKey(new Pref
			{
				PrefName = preferenceName.ToString(),
				ValueString = SOut.Bool(newValue)
			});

			return true;
		}

		/// <summary>
		/// Updates a bool without using cache classes. 
		/// Useful for multithreaded connections.
		/// </summary>
		public static void UpdateBoolNoCache(PrefName preferenceName, bool newValue) => Db.NonQ(
			"UPDATE preference SET ValueString='" + SOut.Bool(newValue) + "' " +
			"WHERE PrefName='" + SOut.String(preferenceName.ToString()) + "'");

		/// <summary>
		/// Returns true if a change was required, or false if no change needed.
		/// </summary>
		public static bool UpdateString(PrefName preferenceName, string newValue)
		{
			var currentValue = PrefC.GetString(preferenceName);
			if (currentValue == newValue)
			{
				return false;
			}

			Db.NonQ(
				"UPDATE preference SET ValueString = '" + SOut.String(newValue) + "' " +
				"WHERE PrefName = '" + SOut.String(preferenceName.ToString()) + "'");

            UpdateValueForKey(new Pref
			{
				PrefName = preferenceName.ToString(),
				ValueString = newValue
			});

			return true;
		}

		/// <summary>
		/// Updates a pref string without using the cache classes. 
		/// Useful for multithreaded connections.
		/// </summary>
		public static void UpdateStringNoCache(PrefName preferenceName, string newValue) => Db.NonQ(
			"UPDATE preference SET ValueString='" + SOut.String(newValue) + "' " +
			"WHERE PrefName='" + SOut.String(preferenceName.ToString()) + "'");

		/// <summary>
		/// Used for prefs that are non-standard.
		/// Especially by outside programmers.
		/// Returns true if a change was required, or false if no change needed.
		/// </summary>
		public static bool UpdateRaw(string preferenceName, string newValue)
		{
			var currentValue = PrefC.GetRaw(preferenceName);
			if (currentValue == newValue)
			{
				return false;
			}

			Db.NonQ(
				"UPDATE preference SET ValueString = '" + SOut.String(newValue) + "' " +
				"WHERE PrefName = '" + SOut.String(preferenceName) + "'");

            UpdateValueForKey(new Pref
			{
				PrefName = preferenceName,
				ValueString = newValue
			});

			return true;
		}

		/// <summary>
		/// Returns true if a change was required, or false if no change needed.
		/// </summary>
		public static bool UpdateDateT(PrefName prefName, DateTime newValue)
		{
			var currentValue = PrefC.GetDateT(prefName);
			if (currentValue == newValue)
			{
				return false;
			}

			Db.NonQ(
				"UPDATE preference SET ValueString = '" + SOut.DateT(newValue, false) + "' " +
				"WHERE PrefName = '" + SOut.String(prefName.ToString()) + "'");

            UpdateValueForKey(new Pref
			{
				PrefName = prefName.ToString(),
				ValueString = SOut.DateT(newValue, false)
			});

			return true;
		}

		/// <summary>
		/// Gets a Pref object when the PrefName is provided.
		/// </summary>
		public static Pref GetPref(string PrefName) => GetOne(PrefName);

		/// <summary>
		/// Gets the practice wide default PrefName that corresponds to the passed in sheet type.
		/// The PrefName must follow the pattern "SheetsDefault"+PrefName.
		/// </summary>
		public static PrefName GetSheetDefPref(SheetTypeEnum sheetType)
		{
            if (!Enum.TryParse("SheetsDefault" + sheetType.GetDescription(), out PrefName result))
            {
                throw new Exception("Unsupported SheetTypeEnum\r\n" + sheetType.ToString());
            }

            return result;
		}

		///<summary>Returns a list of all of the InsHist preferences.</summary>
		public static List<Pref> GetInsHistPrefs()
		{
			return Prefs.GetPrefs(new List<string> { PrefName.InsHistBWCodes.ToString(),PrefName.InsHistDebridementCodes.ToString(),
				PrefName.InsHistExamCodes.ToString(),PrefName.InsHistPanoCodes.ToString(),PrefName.InsHistPerioLLCodes.ToString(),
				PrefName.InsHistPerioLRCodes.ToString(),PrefName.InsHistPerioMaintCodes.ToString(),PrefName.InsHistPerioULCodes.ToString(),
				PrefName.InsHistPerioURCodes.ToString(),PrefName.InsHistProphyCodes.ToString() });
		}

		///<summary>Returns a list of all of the InsHist PrefNames.</summary>
		public static List<PrefName> GetInsHistPrefNames()
		{
			return new List<PrefName> { PrefName.InsHistBWCodes,PrefName.InsHistPanoCodes,PrefName.InsHistExamCodes,PrefName.InsHistProphyCodes,
				PrefName.InsHistPerioURCodes,PrefName.InsHistPerioULCodes,PrefName.InsHistPerioLRCodes,PrefName.InsHistPerioLLCodes,
				PrefName.InsHistPerioMaintCodes,PrefName.InsHistDebridementCodes };
		}

		/// <summary>
		/// Same as <see cref="PrefC.HasClinicsEnabled"/> but doesn't use the cache.
		/// </summary>
		public static bool HasClinicsEnabledNoCache => 
			!GetBoolNoCache(PrefName.EasyNoClinics) && Clinics.GetClinicsNoCache().Count(x => !x.IsHidden) > 0;
	}
}
