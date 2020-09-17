using Imedisoft.Data;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public class AlertReads
	{
		public static IEnumerable<long> Refresh(long userId)
		{
			return Database.SelectMany(
				"SELECT `alert_item_id` FROM `alerts_read` WHERE user_id = " + userId, 
				Database.ToScalar<long>);
		}

		public static IEnumerable<long> RefreshForAlertNums(long userId, List<long> alertItemIds)
		{
			if (alertItemIds == null || alertItemIds.Count == 0)
			{
				return new List<long>();
			}

			return Database.SelectMany(
				"SELECT `alert_item_id` FROM `alerts_read` " +
				"WHERE `user_id` = " + userId + " AND `alert_item_id` IN (" + string.Join(", ", alertItemIds) + ")", 
					Database.ToScalar<long>);
		}

		public static void Insert(long userId, long alertItemId)
			=> Database.ExecuteNonQuery(
				"INSERT INTO alerts_read (user_id, alert_item_id) " +
				"VALUES (" + userId + ", " + alertItemId + ") " +
				"ON DUPLICATE KEY IGNORE");

		/// <summary>
		/// Deletes all alertreads for the listAlertItemNums. Used by the OpenDentalService AlertRadiologyProceduresThread.
		/// </summary>
		public static void DeleteForAlertItems(List<long> alertItemIds)
		{
			if (alertItemIds != null && alertItemIds.Count > 0)
			{
				Database.ExecuteNonQuery("DELETE FROM `alerts_read` WHERE `alert_item_id` IN (" + string.Join(", ", alertItemIds) + ")");
			}
		}
	}
}
