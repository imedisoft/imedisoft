using Imedisoft.Data;
using Imedisoft.Data.Cache;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenDentBusiness
{
    public class ClinicPrefs
	{
		private class Preference
        {
			public string Key;
			public string Value;
			public long ClinicId;
        }

		private static string MakeKey(long clinicId, string key)
			=> clinicId + "$$" + (key ?? "").ToLower();

		[CacheGroup(nameof(InvalidType.ClinicPrefs))]
        private class PreferenceCache : DictionaryCache<string, Preference>
        {
			protected override IEnumerable<Preference> Load() 
				=> Database.SelectMany("SELECT * FROM `clinic_preferences`", dataReader => new Preference
				{
					ClinicId = (long)dataReader["clinic_id"],
					Key = (string)dataReader["key"],
					Value = (string)dataReader["value"]
				});

			protected override string GetKey(Preference item)
				=> MakeKey(item.ClinicId, item.Key);
        }

		private static readonly PreferenceCache cache = new PreferenceCache();

		public static void RefreshCache() 
			=> cache.Refresh();

		public static IEnumerable<(long, string)> GetPreferenceForAllClinics(string key)
        {
			foreach (var preference in cache.Find(p => p.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)))
            {
				yield return (preference.ClinicId, preference.Value);
            }
        }

		public static string GetString(long clinicId, string key, string defaultValue = null)
        {
			var preference = cache.Find(MakeKey(clinicId, key));
			if (preference == null)
            {
				return defaultValue;
            }

			return preference.Value;
        }

		public static long GetLong(long clinicId, string key, long defaultValue = 0)
		{
			if (long.TryParse(GetString(clinicId, key), out var result))
            {
				return result;
            }

			return defaultValue;
		}

		public static bool TryGetLong(long clinicId, string key, out long value)
		{
			if (long.TryParse(GetString(clinicId, key), out var result))
            {
				value = result;
				return true;
			}

			value = 0;
			return false;
		}

		public static int GetInt(long clinicId, string key, int defaultValue = 0)
		{
			if (int.TryParse(GetString(clinicId, key), out var result))
			{
				return result;
			}

			return defaultValue;
		}

		public static bool TryGetInt(long clinicId, string key, out int value)
		{
			if (int.TryParse(GetString(clinicId, key), out var result))
			{
				value = result;
				return true;
			}

			value = 0;
			return false;
		}

		public static bool GetBool(long clinicId, string key, bool defaultValue = false)
		{
			var preference = cache.Find(MakeKey(clinicId, key));
			if (preference == null)
			{
				return defaultValue;
			}

			var value = preference.Value.ToLower();

			return value == "true" || value == "1";
		}

		public static bool Set(long clinicId, string key, string value)
        {
			key = (key ?? "").ToLower();
			value ??= "";

			if (GetString(clinicId, key) == value) return false;

			Database.ExecuteNonQuery(
				"INSERT INTO `clinic_preferences` (`clinic_id`, `key`, `value`) " +
				"VALUES (@clinic_id, @key, @value) " +
				"ON DUPLICATE KEY UPDATE `value` = @value",
					new MySqlParameter("clinic_id", clinicId),
					new MySqlParameter("key", key),
					new MySqlParameter("value", value));

			cache.Set(MakeKey(clinicId, key), 
				new Preference { ClinicId = clinicId, Key = key, Value = value });

			return true;
        }

		public static bool Set(long clinicId, string key, long? value)
			=> Set(clinicId, key, value?.ToString());

		public static bool Set(long clinicId, string key, bool value)
			=> Set(clinicId, key, value ? "true" : "false");

		/// <summary>
		/// Deletes the preferences of the specified clinic.
		/// </summary>
		/// <param name="clinicId">The ID of the clinic.</param>
		/// <param name="keys">The keys of the preferences to delete.</param>
		public static void Delete(long clinicId, IEnumerable<string> keys)
        {
			var set = string.Join(", ", keys.Select(key => MySqlHelper.EscapeString(key)));
			if (set.Length == 0)
            {
				return;
            }

			Database.ExecuteNonQuery(
				"DELETE FROM `clinic_preferences` " +
				"WHERE `clinic_id` = " + clinicId + " " +
				"AND `key` IN (" + set + ")");

			RefreshCache();
        }
	}
}
