using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class EhrProviderKeys
	{
		public static IEnumerable<EhrProviderKey> GetAll() 
			=> SelectMany("SELECT * FROM `ehr_provider_keys` ORDER BY `last_name`, `year`");

		public static IEnumerable<EhrProviderKey> GetByProviderName(string lastName, string firstName)
		{ 
			if (lastName == null || firstName == null || lastName.Trim() == "" || firstName.Trim() == "")
			{
				return new List<EhrProviderKey>();
			}

			return SelectMany(
				"SELECT * FROM ehr_provider_keys " +
				"WHERE last_name = @last_name AND first_name = @first_name " +
				"ORDER BY `year` DESC",
					new MySqlParameter("last_name", lastName ?? ""),
					new MySqlParameter("first_name", firstName ?? ""));
		}

		public static bool HasProviderHadKey(string lastName, string firstName) 
			=> Database.ExecuteLong(
				"SELECT COUNT(*) FROM ehr_provider_keys WHERE last_name = @last_name AND first_name = @first_name",
					new MySqlParameter("last_name", lastName ?? ""),
					new MySqlParameter("first_name", firstName ?? "")) > 0;

		private static bool HasKeys() 
			=> Database.ExecuteLong("SELECT COUNT(*) FROM ehr_provider_keys") > 0;

		public static void Save(EhrProviderKey ehrProviderKey)
        {
			if (ehrProviderKey.Id == 0)
            {
				var isAlertRadiologyProcsEnabled = Preferences.GetBool(PreferenceName.IsAlertRadiologyProcsEnabled);
				var isFirstKey = false;

				if (!isAlertRadiologyProcsEnabled)
				{
					isFirstKey = !HasKeys();
				}

				ExecuteInsert(ehrProviderKey);

				if (!isAlertRadiologyProcsEnabled && isFirstKey)
				{
					Preferences.Set(PreferenceName.IsAlertRadiologyProcsEnabled, true);

					CacheManager.RefreshGlobal(nameof(InvalidType.Prefs));
				}
			}
            else
            {
				ExecuteUpdate(ehrProviderKey);
            }
		}

		public static void Delete(long ehrProviderKeyId)
		{
			ExecuteDelete(ehrProviderKeyId);

			if (Preferences.GetBool(PreferenceName.IsAlertRadiologyProcsEnabled) && !HasKeys())
			{
				Preferences.Set(PreferenceName.IsAlertRadiologyProcsEnabled, false);

				CacheManager.RefreshGlobal(nameof(InvalidType.Prefs));
			}
		}
	}
}
