using Imedisoft.Data.Cache;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
	/// <summary>
	///		<para>
	///			Keeps track of the computers in an office.
	///		</para>
	///		<para>
	///			The list will eventually become cluttered with the names of old computers that are 
	///			no longer in service. Old rows can be safely deleted.
	///		</para>
	/// </summary>
	public partial class Computers
	{
		[CacheGroup(nameof(InvalidType.Computers))]
		private class ComputerCache : ListCache<Computer>
		{
			protected override IEnumerable<Computer> Load()
				=> SelectMany("SELECT * FROM `computers` ORDER BY `machine_name`");

			protected override void Initialize() 
				=> UpdateHeartBeat(Environment.MachineName);
		}

		private static readonly ComputerCache cache = new ComputerCache();

		public static void RefreshCache() 
			=> cache.Refresh();

		public static long Insert(Computer computer) 
			=> ExecuteInsert(computer);

		public static void Delete(Computer computer) 
			=> ExecuteDelete(computer);

		public static Computer GetCurrent() 
			=> cache.FirstOrDefault(computer 
				=> computer.MachineName.Equals(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase));

		public static List<Computer> GetAll()
			=> cache.GetAll();

		/// <summary>
		///		<para>
		///			Returns the machine names of all computers with an active heartbeat.
		///		</para>
		///		<para>
		///			A heartbeat less than 4 minutes old is considered active.
		///		</para>
		/// </summary>
		public static IEnumerable<string> GetRunningComputers() 
			=> Database.SelectMany(
				"SELECT `machine_name` FROM `computers` WHERE `last_heartbeat` > SUBTIME(NOW(),'00:04:00')", 
					Database.ToScalar<string>);

		/// <summary>
		/// Updates the heartbeat of the computer with the specified machine name.
		/// </summary>
		/// <param name="machineName">The machine name of the computer.</param>
		public static void UpdateHeartBeat(string machineName) 
			=> Database.ExecuteNonQuery(
				"INSERT INTO `computers` (`machine_name`, `last_heartbeat`) VALUES (@machine_name, NOW()) ON DUPLICATE KEY UPDATE `last_heartbeat` = NOW()",
					new MySqlParameter("machine_name", machineName));

		/// <summary>
		/// Clears the heartbeat for the computer with the specified machine name.
		/// </summary>
		/// <param name="machineName">The machine name of the computer.</param>
		public static void ClearHeartBeat(string machineName)
		{
			if (string.IsNullOrEmpty(machineName))
				return;

			Database.ExecuteNonQuery(
				"UPDATE `computers` SET `last_heartbeat` = NULL " +
				"WHERE `machine_name` = @machine_name",
					new MySqlParameter("machine_name", machineName));
		}

		/// <summary>
		/// Clears the heartbeat for every computer.
		/// </summary>
		/// <param name="excludeMachineName"></param>
		public static void ClearAllHeartBeats(string excludeMachineName = null) 
			=> Database.ExecuteNonQuery(
				"UPDATE `computers` SET `last_heartbeat` = NULL " +
				"WHERE `machine_name` != @exclude_machine_name",
					new MySqlParameter("exclude_machine_name", excludeMachineName ?? ""));

		/// <summary>
		/// Gets details of the database service.
		///</summary>
		public static (string name, string comment, string hostname, string version) GetServiceInfo()
			=> (Database.ExecuteString("SELECT @@socket"),
				Database.ExecuteString("SELECT @@version_comment"),
				Database.ExecuteString("SELECT @@hostname"),
				Database.ExecuteString("SELECT @@version"));
	}
}
