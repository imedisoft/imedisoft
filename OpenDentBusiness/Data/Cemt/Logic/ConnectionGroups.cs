using Imedisoft.Data.Models.Cemt;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Data.Cemt
{
    public partial class ConnectionGroups
	{
		public static IEnumerable<ConnectionGroup> GetAll() 
			=> SelectMany("SELECT * FROM `cemt_connection_groups` ORDER BY `description`");

		public static long Insert(ConnectionGroup connectionGroup) 
			=> ExecuteInsert(connectionGroup);

		public static void Update(ConnectionGroup connectionGroup) 
			=> ExecuteUpdate(connectionGroup);

		public static void Save(ConnectionGroup connectionGroup)
        {
			if (connectionGroup.Id == 0) Insert(connectionGroup);
            else
            {
				Update(connectionGroup);
            }
        }

		public static void Delete(long connectionGroupId) 
			=> ExecuteDelete(connectionGroupId);

		/// <summary>
		/// Adds the connections with the specified ID's to the specified group.
		/// </summary>
		/// <param name="connectionGroup">The connection group.</param>
		/// <param name="connectionIds">The ID's of the connections to the add to the group.</param>
		public static void AddConnectionsToGroup(ConnectionGroup connectionGroup, IEnumerable<long> connectionIds)
        {
			var ids = connectionIds.ToList();
			if (ids.Count == 0)
            {
				return;
            }

			foreach (var connectionId in connectionIds)
            {
				Database.ExecuteNonQuery(
					"INSERT INTO `cemt_connection_group_connections` (`connection_group_id`, `connection_id`) " +
					"VALUES (" + connectionGroup.Id + ", " + connectionId + ") " +
					"ON DUPLICATE KEY IGNORE");
            }
        }

		/// <summary>
		/// Removes the connections with the specified ID's from the specified group.
		/// </summary>
		/// <param name="connectionGroup">The connection group.</param>
		/// <param name="connectionIds">The ID's of the connections to the remove from the group.</param>
		public static void RemoveConnectionsFromGroup(ConnectionGroup connectionGroup, IEnumerable<long> connectionIds)
        {
			var connectionIdsList = connectionIds.ToList();
			if (connectionIdsList.Count == 0)
            {
				return;
            }

			Database.ExecuteNonQuery(
				"DELETE FROM `cemt_connection_group_connections` " +
				"WHERE `connection_group_id` = " + connectionGroup.Id + " " +
				"AND `connection_id` IN (" + string.Join(", ", connectionIdsList) + ")");
        }

		public static Dictionary<long, long> GetConnectionGroupConnectionCounts()
        {
			static (long id, long connections) FromReader(MySqlDataReader dataReader) 
				=> ((long)dataReader[0], (long)dataReader[1]);

			var results = Database.SelectMany(
				"SELECT cg.`id`, cg.`description`, COUNT(cgc.`connection_id`) AS `connections` " +
				"FROM `cemt_connection_groups` cg " +
				"LEFT JOIN `cemt_connection_group_connections` cgc ON cgc.`connection_group_id` = cg.`id`",
					FromReader);

			var dictionary = new Dictionary<long, long>();
			foreach (var result in results)
            {
				dictionary[result.id] = result.connections;
            }

			return dictionary;
        }
	}
}
