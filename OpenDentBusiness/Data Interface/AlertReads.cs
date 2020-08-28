using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
	public class AlertReads
	{

		#region Get Methods
		#endregion

		#region Modification Methods

		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion


		

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

		public static void DeleteForAlertItem(long alertItemId) 
			=> Database.ExecuteNonQuery(
				"DELETE FROM `alerts_read` WHERE `alert_item_id` = " + alertItemId);

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
