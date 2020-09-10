using DataConnectionBase;
using Imedisoft.Data.Cache;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Imedisoft.Data
{
    public class Preferences
	{
		private class Preference
        {
			public string Key;
			public string Value;
        }

		[CacheGroup(nameof(InvalidType.Prefs))]
        private class PreferenceCache : DictionaryCache<string, Preference>
        {
            protected override string GetKey(Preference item) 
				=> item.Key.ToLower();

            protected override IEnumerable<Preference> Load() 
				=> Database.SelectMany("SELECT * FROM preferences", 
					dataReader => new Preference { 
						Key = (string)dataReader["key"], 
						Value = (string)dataReader["value"] });
        }

		private static readonly PreferenceCache cache = new PreferenceCache();
		private static readonly NumberFormatInfo numberFormat = new NumberFormatInfo { NumberDecimalSeparator = ".", NumberGroupSeparator = "," };
		private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

		/// <summary>
		/// Invalidates and refreshes the preference cache.
		/// </summary>
		public static void RefreshCache()
			=> cache.Refresh();

		/// <summary>
		/// Checks whether a preference with the specified key exists.
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <returns>True if the preference exists; otherwise, false.</returns>
		public static bool Exists(string key)
			=> cache.Find(item => item.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) != null;

		/// <summary>
		/// Gets the value of the preference with the specified key.
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <returns>The value of the preference.</returns>
		public static string GetString(string key, string defaultValue = null)
			=> cache.Find(key?.ToLower())?.Value ?? defaultValue;

		/// <summary>
		/// Gets the value of the preference with the specified key without using the key.
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <returns>The value of the preference.</returns>
		public static string GetStringNoCache(string key) =>
			Database.ExecuteString("SELECT `value` FROM `preferences` WHERE `key` = @key",
				new MySqlParameter("key", key?.ToLower() ?? ""));

		/// <summary>
		/// Converts the specified string value to a boolean.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The specified value as a boolean.</returns>
		private static bool ToBool(string value)
			=> value == "true" || value == "yes" || value == "1";

		/// <summary>
		/// Gets the value of the preference with the specified key.
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <returns>The value of the preference.</returns>
		public static bool GetBool(string key, bool defaultValue = false)
        {
			var preference = cache.Find(key?.ToLower());
			if (preference == null)
            {
				return defaultValue;
            }

			return ToBool(preference.Value.ToLower());
		}

		/// <summary>
		/// Gets the value of the preference with the specified key without using the key.
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <returns>The value of the preference.</returns>
		public static bool GetBoolNoCache(string key) 
			=> ToBool(GetStringNoCache(key));

		/// <summary>
		/// Gets the value of the preference with the specified key.
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <returns>The value of the preference.</returns>
		public static DateTime GetDateTime(string key, DateTime defaultValue)
		{
			var preference = cache.Find(key?.ToLower());
			if (preference == null)
			{
				return defaultValue;
			}

			if (DateTime.TryParseExact(preference.Value, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dateTime))
			{
				return dateTime;
			}

			return defaultValue;
		}

		/// <summary>
		/// Gets the value of the preference with the specified key.
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <returns>The value of the preference.</returns>
		public static DateTime GetDateTime(string key) 
			=> GetDateTime(key, DateTime.MinValue);

		/// <summary>
		/// Gets the value of the preference with the specified key.
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <returns>The value of the preference.</returns>
		public static DateTime? GetDateTimeOrNull(string key, DateTime? defaultValue = null)
        {
			var preference = cache.Find(key?.ToLower());
			if (preference == null)
			{
				return defaultValue;
			}

			if (DateTime.TryParseExact(preference.Value, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dateTime))
            {
				return dateTime;
            }

			return defaultValue;
		}

		/// <summary>
		/// Gets the value of the preference with the specified key.
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <returns>The value of the preference.</returns>
		public static byte GetByte(string key, byte defaultValue = 0)
		{
			if (byte.TryParse(GetString(key), out var result))
			{
				return result;
			}

			return defaultValue;
		}

		/// <summary>
		/// Gets the value of the preference with the specified key.
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <returns>The value of the preference.</returns>
		public static int GetInt(string key, int defaultValue = 0)
		{
			if (int.TryParse(GetString(key), out var result))
			{
				return result;
			}

			return defaultValue;
		}

		/// <summary>
		/// Gets the value of the preference with the specified key.
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <returns>The value of the preference.</returns>
		public static long GetLong(string key, long defaultValue = 0)
		{
			if (long.TryParse(GetString(key), out var result))
            {
				return result;
            }

			return defaultValue;
		}

		public static string GetLongHideNegOne(string key)
		{
			if (long.TryParse(GetString(key), out var result) && result > 0)
            {
				return result.ToString();
            }

			return "";
		}

		/// <summary>
		/// Gets the value of the preference with the specified key.
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <returns>The value of the preference.</returns>
		public static double GetDouble(string key, double defaultValue = 0)
        {
			if (double.TryParse(GetString(key), NumberStyles.Float, numberFormat, out var result))
            {
				return result;
            }

			return defaultValue;
        }

		/// <summary>
		/// Gets the value of the preference with the specified key.
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <returns>The value of the preference.</returns>
		public static T GetEnum<T>(string key, T defaultValue = default) where T : Enum
        {
			if (int.TryParse(GetString(key), out var result))
            {
				return (T)Enum.ToObject(typeof(T), result);
			}

			return defaultValue;
		}

		/// <summary>
		///		<para>
		///			Sets the value of the preference with the specified <paramref name="key"/>.
		///		</para>
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <returns>True if the preference value changed; otherwise, false.</returns>
		public static bool Set(string key, string value)
		{
			key ??= "";
			value ??= "";

			if (GetString(key) == value) return false;

			Database.ExecuteNonQuery(
				"INSERT INTO `preferences` (`key`, `value`) VALUES (@key, @value) " +
				"ON DUPLICATE KEY UPDATE `value` = @value",
					new MySqlParameter("key", key),
					new MySqlParameter("value", value));

			cache.Set(key, new Preference { Key = key, Value = value });

			return true;
		}

		/// <summary>
		///		<para>
		///			Sets the value of the preference with the specified <paramref name="key"/>.
		///		</para>
		///		<para>
		///			Always writes the specified <paramref name="value"/> to the database without a cache lookup.
		///		</para>
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		public static void SetNoCache(string key, string value)
		{
			key ??= "";
			value ??= "";

			Database.ExecuteNonQuery(
				"INSERT INTO `preferences` (`key`, `value`) VALUES (@key, @value) " +
				"ON DUPLICATE KEY UPDATE `value` = @value",
					new MySqlParameter("key", key),
					new MySqlParameter("value", value));

			cache.Set(key, new Preference { Key = key, Value = value });
		}

		/// <summary>
		///		<para>
		///			Sets the value of the preference with the specified <paramref name="key"/>.
		///		</para>
		///		<para>
		///			All dates are converted to UTC before they are stored in the database.
		///		</para>
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <returns>True if the preference value changed; otherwise, false.</returns>
		public static bool Set(string key, DateTime? value)
		{
			if (value.HasValue && value.Value.Kind != DateTimeKind.Utc)
            {
				value = value.Value.ToUniversalTime();
            }

			return Set(key, value.HasValue ? value.Value.ToString(DateTimeFormat, CultureInfo.InvariantCulture) : "");
		}

		/// <summary>
		///		<para>
		///			Sets the value of the preference with the specified <paramref name="key"/>.
		///		</para>
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <param name="round">Set to true if the value should be rounded.</param>
		/// <param name="roundDigits">The number of fractional digits in the return value.</param>
		/// <param name="roundingMode">Specification for how to round value if it is midway between two other numbers.</param>
		/// <returns>True if the preference value changed; otherwise, false.</returns>
		public static bool Set(string key, double? value, bool round = true, int roundDigits = 2, MidpointRounding roundingMode = MidpointRounding.AwayFromZero)
		{
			var str = "";

			if (value.HasValue)
			{
				if (round)
				{
					value = Math.Round(value.Value, roundDigits, roundingMode);
				}

				str = value.Value.ToString(numberFormat);
			}

			return Set(key, str);
		}

		/// <summary>
		///		<para>
		///			Sets the value of the preference with the specified <paramref name="key"/>.
		///		</para>
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <returns>True if the preference value changed; otherwise, false.</returns>
		public static bool Set(string key, bool? value)
			=> Set(key, value.HasValue ? (value.Value ? "true" : "false") : "");

		/// <summary>
		///		<para>
		///			Sets the value of the preference with the specified <paramref name="key"/>.
		///		</para>
		///		<para>
		///			Always writes the specified <paramref name="value"/> to the database without a cache lookup.
		///		</para>
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		[Obsolete("Use the regular (cache) version.")]
		public static void SetNoCache(string key, bool? value)
			=> SetNoCache(key, value.HasValue ? (value.Value ? "true" : "false") : "");

		/// <summary>
		///		<para>
		///			Sets the value of the preference with the specified <paramref name="key"/>.
		///		</para>
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <returns>True if the preference value changed; otherwise, false.</returns>
		public static bool Set(string key, long? value) 
			=> Set(key, value?.ToString() ?? "");

		/// <summary>
		///		<para>
		///			Sets the value of the preference with the specified <paramref name="key"/>.
		///		</para>
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <returns>True if the preference value changed; otherwise, false.</returns>
		public static bool Set(string key, int? value)
			=> Set(key, value?.ToString() ?? "");

		/// <summary>
		///		<para>
		///			Sets the value of the preference with the specified <paramref name="key"/>.
		///		</para>
		///		<para>
		///			Always writes the specified <paramref name="value"/> to the database without a cache lookup.
		///		</para>
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		[Obsolete("Use the regular (cache) version.")]
		public static void SetNoCache(string key, int? value)
			=> SetNoCache(key, value?.ToString());

		///// <summary>
		/////		<para>
		/////			Sets the value of the preference with the specified <paramref name="key"/>.
		/////		</para>
		///// </summary>
		///// <param name="key">The preference key.</param>
		///// <param name="value">The new value of the preference.</param>
		///// <returns>True if the preference value changed; otherwise, false.</returns>
		//[Obsolete("YN enum is being phased out. Use bool? instead.")]
		//public static bool Set(string key, YN value)
		//	=> Set(key, (int)value);

		/// <summary>
		///		<para>
		///			Sets the value of the preference with the specified <paramref name="key"/>.
		///		</para>
		/// </summary>
		/// <param name="key">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <returns>True if the preference value changed; otherwise, false.</returns>
		public static bool Set(string key, byte value)
			=> Set(key, value.ToString());

















		/// <summary>
		/// Returns a deep copy list of the corresponding preferences by name from the cache.
		/// </summary>
		private static List<string> GetPrefs(List<string> preferenceNames)
		{
			if (preferenceNames == null || preferenceNames.Count == 0)
			{
				return new List<string>();
			}

			return cache.Find(pref => preferenceNames.Contains(pref.Key)).Select(pref => pref.Value).ToList();
		}


		/// <summary>
		/// For saving from the UI when we want "0" or "" to be saved as a -1 in the database.
		/// </summary>
		public static bool UpdateLongAsNegOne(string preferenceName, string newValue)
		{
			long val = -1;
			if (!string.IsNullOrWhiteSpace(newValue))
			{
				val = SIn.Long(newValue) > 0 ? SIn.Long(newValue) : -1;
			}

			return Set(preferenceName, val);
		}

		/// <summary>
		/// Gets the practice wide default PrefName that corresponds to the passed in sheet type.
		/// The PrefName must follow the pattern "SheetsDefault"+PrefName.
		/// </summary>
		public static string GetSheetDefPref(SheetTypeEnum sheetType)
		{
			return "SheetsDefault" + sheetType.ToString();
		}

		///<summary>Returns a list of all of the InsHist preferences.</summary>
		public static List<string> GetInsHistPrefs() 
			=> GetPrefs(new List<string> { 
				PreferenceName.InsHistBWCodes,
				PreferenceName.InsHistDebridementCodes,
				PreferenceName.InsHistExamCodes,
				PreferenceName.InsHistPanoCodes,
				PreferenceName.InsHistPerioLLCodes,
				PreferenceName.InsHistPerioLRCodes,
				PreferenceName.InsHistPerioMaintCodes,
				PreferenceName.InsHistPerioULCodes,
				PreferenceName.InsHistPerioURCodes,
				PreferenceName.InsHistProphyCodes 
			});

		/// <summary>
		/// Returns a list of all of the InsHist PrefNames.
		/// </summary>
		public static List<string> GetInsHistPrefNames() 
			=> new List<string> { 
				PreferenceName.InsHistBWCodes,
				PreferenceName.InsHistPanoCodes,
				PreferenceName.InsHistExamCodes,
				PreferenceName.InsHistProphyCodes,
				PreferenceName.InsHistPerioURCodes,
				PreferenceName.InsHistPerioULCodes,
				PreferenceName.InsHistPerioLRCodes,
				PreferenceName.InsHistPerioLLCodes,
				PreferenceName.InsHistPerioMaintCodes,
				PreferenceName.InsHistDebridementCodes 
			};

		/// <summary>
		/// Same as <see cref="PrefC.HasClinicsEnabled"/> but doesn't use the cache.
		/// </summary>
		public static bool HasClinicsEnabledNoCache => 
			!GetBoolNoCache(nameof(PreferenceName.EasyNoClinics)) && Clinics.GetClinicsNoCache().Count(x => !x.IsHidden) > 0;
	}
}
