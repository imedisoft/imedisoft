using Imedisoft.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenDentBusiness
{
    public class TaskSubscriptions
	{
		/// <summary>
		/// Gets the ID's of all the task lists the user with the specified ID is subscribed to.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <returns>The ID's of the task lists the user is subscribed to.</returns>
		public static IEnumerable<long> GetSubscriptionsForUser(long userId) 
			=> Database.SelectMany(
				"SELECT `task_list_id` FROM `task_subscriptions` WHERE `user_id` = " + userId,
					Database.ToScalar<long>);

		/// <summary>
		/// Gets the ID's of all the task lists the current user is subscribed to.
		/// </summary>
		/// <returns>The ID's of the task lists the user is subscribed to.</returns>
		public static IEnumerable<long> GetSubscriptionsForUser()
			=> GetSubscriptionsForUser(Security.CurrentUser.Id);

		/// <summary>
		/// Subscribes the user with the specified ID to the specified task list.
		/// </summary>
		/// <param name="taskListId">The ID of the task list.</param>
		/// <param name="userId">The ID of the user.</param>
		public static void Subscribe(long taskListId, long userId)
			=> Database.ExecuteInsert(
				"INSERT INTO `task_subscriptions` (`user_id`, `task_list_id`) " +
				"VALUES (" + userId + ", " + taskListId + ") " +
				"ON DUPLICATE KEY IGNORE");

		/// <summary>
		/// Subscribes the current user to the specified task list.
		/// </summary>
		/// <param name="taskListId">The ID of the task list.</param>
		public static void Subscribe(long taskListId)
			=> Subscribe(taskListId, Security.CurrentUser.Id);

		/// <summary>
		/// Unsubscribes the user with the specified ID from the specified task list.
		/// </summary>
		/// <param name="taskListId">The ID of the task list.</param>
		/// <param name="userId">The ID of the user.</param>
		public static void Unsubscribe(long taskListId, long userId)
			=> Database.ExecuteNonQuery(
				"DELETE FROM `task_subscriptions` " +
				"WHERE `user_id` = " + userId + " AND `task_list_id` = " + taskListId);

		/// <summary>
		/// Unsubscribes the current user from the specified task list.
		/// </summary>
		/// <param name="taskListId">The ID of the task list.</param>
		public static void Unsubscribe(long taskListId)
			=> Unsubscribe(taskListId, Security.CurrentUser.Id);

		/// <summary>
		/// Checks whether the user with the specified ID is subscribed to the specified task list.
		/// </summary>
		/// <param name="taskListId">The ID of the task list.</param>
		/// <param name="userId">The ID of the user.</param>
		/// <returns>True if the user is subscribed to the task list; otherwise, false.</returns>
		public static bool IsSubscribed(long taskListId, long userId)
			=> Database.ExecuteLong(
				"SELECT COUNT(*) " +
				"FROM `task_subscriptions` " +
				"WHERE `user_id` = " + userId + " AND `task_list_id` = " + taskListId) > 0;

		/// <summary>
		/// Checks whether the current user is subscribed to the specified task list.
		/// </summary>
		/// <param name="taskListId">The ID of the task list.</param>
		/// <returns>True if the user is subscribed to the task list; otherwise, false.</returns>
		public static bool IsSubscribed(long taskListId)
			=> IsSubscribed(taskListId, Security.CurrentUser.Id);
	}
}
