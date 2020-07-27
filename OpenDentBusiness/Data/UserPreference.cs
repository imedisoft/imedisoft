using Imedisoft.Data;
using MySql.Data.MySqlClient;

namespace OpenDentBusiness
{
    public class UserPreference
	{
		/// <summary>
		/// Generates a composite key for the given parameters.
		/// </summary>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		/// <returns></returns>
		private static string MakeKey(string preferenceKey, long? foreignKey, long? clinicId)
        {
			// If a clinic ID has been specified, scope the key to that clinic.
			if (clinicId.HasValue)
				preferenceKey = $"_c.{clinicId.Value}.{preferenceKey}";

			// If a foreign key has been specified append it to the preference key.
			if (foreignKey.HasValue)
				preferenceKey = $"{preferenceKey}_{foreignKey.Value}";

			return preferenceKey;
		}

		/// <summary>
		/// Gets the value of a user preference as a string.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		/// <returns>The string value of the preference.</returns>
		public static string GetString(long userId, string preferenceKey, long? foreignKey = null, long? clinicId = null)
        {
			// If a clinic ID has been specified, scope the key to that clinic.
			if (clinicId.HasValue) 
				preferenceKey = $"_c.{clinicId.Value}.{preferenceKey}";

			// If a foreign key has been specified append it to the preference key.
			if (foreignKey.HasValue)
				preferenceKey = $"{preferenceKey}_{foreignKey.Value}";

			return Database.ExecuteString(
				"SELECT `value` FROM `user_preferences` WHERE `user_id` = " + userId + " AND `key` = @key", 
					new MySqlParameter("key", MakeKey(preferenceKey, foreignKey, clinicId)));
        }

		/// <summary>
		/// Gets the value of a user preference as a 32-bit integer.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		/// <returns>The integer value of the preference.</returns>
		public static int GetInt(long userId, string preferenceKey, int defaultValue = 0, long? foreignKey = null, long? clinicId = null)
		{
			if (int.TryParse(GetString(userId, preferenceKey, foreignKey, clinicId), out var result))
			{
				return result;
			}

			return defaultValue;
		}

		/// <summary>
		/// Gets the value of a user preference as a 64-bit integer.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		/// <returns>The long value of the preference.</returns>
		public static long GetLong(long userId, string preferenceKey, long defaultValue = 0, long? foreignKey = null, long? clinicId = null)
		{
			if (long.TryParse(GetString(userId, preferenceKey, foreignKey, clinicId), out var result))
			{
				return result;
			}

			return defaultValue;
		}

		/// <summary>
		/// Gets the value of a user preference as a boolean.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		/// <returns>The boolean value of the preference.</returns>
		public static bool GetBool(long userId, string preferenceKey, bool defaultValue = false, long? foreignKey = null, long? clinicId = null)
		{
			if (int.TryParse(GetString(userId, preferenceKey, foreignKey, clinicId), out var result))
			{
				return result != 0;
			}

			return defaultValue;
		}

		/// <summary>
		/// Sets a new value for the preference with the specified key of the specified user.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		public static void Set(long userId, string preferenceKey, string value, long? foreignKey = null, long? clinicId = null) 
			=> Database.ExecuteNonQuery(
				"INSERT INTO `user_preferences` (`user_id`, `key`, `value`) VALUES (" + userId + ", @key, @value) ON DUPLICATE KEY UPDATE `value` = @value",
					new MySqlParameter("value", value ?? ""),
					new MySqlParameter("key", MakeKey(preferenceKey, foreignKey, clinicId)));

		/// <summary>
		/// Sets a new value for the preference with the specified key of the specified user.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		public static void Set(long userId, string preferenceKey, int value, long? foreignKey = null, long? clinicId = null)
			=> Set(userId, preferenceKey, value.ToString(), foreignKey, clinicId);

		/// <summary>
		/// Sets a new value for the preference with the specified key of the specified user.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		public static void Set(long userId, string preferenceKey, long value, long? foreignKey = null, long? clinicId = null)
			=> Set(userId, preferenceKey, value.ToString(), foreignKey, clinicId);

		/// <summary>
		/// Sets a new value for the preference with the specified key of the specified user.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		public static void Set(long userId, string preferenceKey, bool value, long? foreignKey = null, long? clinicId = null)
			=> Set(userId, preferenceKey, value ? 1 : 0, foreignKey, clinicId);

		/// <summary>
		/// Deletes the preference with the specified key for the specified user.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		public static void Delete(long userId, string preferenceKey, long? foreignKey = null, long? clinicId = null)
			=> Database.ExecuteNonQuery(
				"DELETE FROM `user_preferences` WHERE `user_id` = " + userId + " AND `key` = @key",
					new MySqlParameter("key", MakeKey(preferenceKey, foreignKey, clinicId)));

		/// <summary>
		/// Delete the preference with the specified key and value for all users.
		/// </summary>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="value">The preference value.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		public static void DeleteWithValue(string preferenceKey, string value, long? foreignKey = null, long? clinicId = null)
			=> Database.ExecuteNonQuery(
				"DELETE FROM `user_preferences` WHERE `key` = @key AND `value` = @value",
					new MySqlParameter("key", MakeKey(preferenceKey, foreignKey, clinicId)),
					new MySqlParameter("value", value ?? ""));

		/// <summary>
		/// Updates all users preferences with the given key and value to the specified new value.
		/// </summary>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="oldValue">The old (current) value of the preferences.</param>
		/// <param name="newValue">The new value of the preferences.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		public static void Update(string preferenceKey, string oldValue, string newValue, long? foreignKey = null, long? clinicId = null)
			=> Database.ExecuteNonQuery(
				"UPDATE `user_preferences` SET `value` = @new_value WHERE `key` = @key AND `value` = @old_value",
					new MySqlParameter("key", MakeKey(preferenceKey, foreignKey, clinicId)),
					new MySqlParameter("old_value", oldValue ?? ""),
					new MySqlParameter("new_value", newValue ?? ""));

		/// <summary>
		/// Gets the value of a user preference as a string for the current user.
		/// </summary>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		/// <returns>The string value of the preference.</returns>
		public static string GetString(string preferenceKey, long? foreignKey = null, long? clinicId = null)
			=> GetString(Security.CurrentUser.Id, preferenceKey, foreignKey, clinicId);

		/// <summary>
		/// Gets the value of a user preference as a 32-bit integer for the current user.
		/// </summary>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		/// <returns>The integer value of the preference.</returns>
		public static int GetInt(string preferenceKey, int defaultValue = 0, long? foreignKey = null, long? clinicId = null)
			=> GetInt(Security.CurrentUser.Id, preferenceKey, defaultValue, foreignKey, clinicId);

		/// <summary>
		/// Gets the value of a user preference as a 64-bit integer for the current user.
		/// </summary>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		/// <returns>The long value of the preference.</returns>
		public static long GetLong(string preferenceKey, long defaultValue = 0, long? foreignKey = null, long? clinicId = null)
			=> GetLong(Security.CurrentUser.Id, preferenceKey, defaultValue, foreignKey, clinicId);

		/// <summary>
		/// Gets the value of a user preference as a boolean for the current user.
		/// </summary>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="defaultValue">The default value of the preference.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		/// <returns>The boolean value of the preference.</returns>
		public static bool GetBool(string preferenceKey, bool defaultValue = false, long? foreignKey = null, long? clinicId = null)
			=> GetBool(Security.CurrentUser.Id, preferenceKey, defaultValue, foreignKey, clinicId);

		/// <summary>
		/// Sets a new value for the preference with the specified key of the current user.
		/// </summary>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		public static void Set(string preferenceKey, string value, long? foreignKey = null, long? clinicId = null)
			=> Set(Security.CurrentUser.Id, preferenceKey, value, foreignKey, clinicId);

		/// <summary>
		/// Sets a new value for the preference with the specified key of the current user.
		/// </summary>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		public static void Set(string preferenceKey, int value, long? foreignKey = null, long? clinicId = null)
			=> Set(preferenceKey, value.ToString(), foreignKey, clinicId);

		/// <summary>
		/// Sets a new value for the preference with the specified key of the current user.
		/// </summary>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		public static void Set(string preferenceKey, long value, long? foreignKey = null, long? clinicId = null)
			=> Set(preferenceKey, value.ToString(), foreignKey, clinicId);

		/// <summary>
		/// Sets a new value for the preference with the specified key of the current user.
		/// </summary>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="value">The new value of the preference.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		public static void Set(string preferenceKey, bool value, long? foreignKey = null, long? clinicId = null)
			=> Set(preferenceKey, value ? 1 : 0, foreignKey, clinicId);

		/// <summary>
		/// Deletes the preference with the specified key for the current user.
		/// </summary>
		/// <param name="preferenceKey">The preference key.</param>
		/// <param name="foreignKey">The (optional) ID of the data entity the preference applies to.</param>
		/// <param name="clinicId">The (optional) ID of the cinic the preference applies to.</param>
		public static void Delete(string preferenceKey, long? foreignKey = null, long? clinicId = null)
			=> Delete(Security.CurrentUser.Id, preferenceKey, foreignKey, clinicId);
	}
}
