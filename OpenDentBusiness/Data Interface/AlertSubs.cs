using Imedisoft.Data;
using System.Collections.Generic;
using System.Linq;

namespace OpenDentBusiness
{
    public partial class AlertSubs
	{
		public static void DeleteAndInsertForSuperUsers(List<Userod> users, List<AlertSub> alertSubs)
		{
			if (users == null || users.Count < 1)
			{
				return;
			}

			Database.ExecuteNonQuery(
				"DELETE FROM `alert_subs` WHERE `user_id` IN (" + string.Join(", ", users.Select(x => x.Id)) + ")");

			foreach (var alertSub in alertSubs)
			{
				Database.ExecuteNonQuery(
					"INSERT INTO `alert_subs` (`user_id`, `clinic_id`) " +
					"VALUES (" + alertSub.UserId + "," + alertSub.ClinicId + ")");
			}
		}

		public static List<AlertSub> GetAll() 
			=> SelectMany("SELECT * FROM alert_subs").ToList();

		public static List<AlertSub> GetAllForUser(long userId, long? clinicId = null)
		{
			string command = "SELECT * FROM `alert_subs` WHERE `user_id` = " + userId;

			if (clinicId.HasValue)
			{
				command += " AND `clinic_id` = " + clinicId.Value;
			}

			return SelectMany(command).ToList();
		}

		public static long Insert(AlertSub alertSub) 
			=> ExecuteInsert(alertSub);

		public static void Update(AlertSub alertSub) 
			=> ExecuteUpdate(alertSub);

		public static void Delete(long alertSubId) 
			=> ExecuteDelete(alertSubId);
	}
}
