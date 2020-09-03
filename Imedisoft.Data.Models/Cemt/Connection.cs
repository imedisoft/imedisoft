using Imedisoft.Data.Annotations;
using System;

namespace Imedisoft.Data.Models.Cemt
{
    /// <summary>
    ///		<para>
    ///			Used by the Central Manager.
    ///		</para>
    ///		<para>
    ///			Stores the information needed to establish a connection to a remote database.
    ///		</para>
    /// </summary>
    [Table("cemt_connections")]
	public class Connection
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The hostname or IP address of the database server.
		/// </summary>
		public string DatabaseServer;

		/// <summary>
		/// The name of the database.
		/// </summary>
		public string DatabaseName;

		/// <summary>
		/// The name of the user.
		/// </summary>
		public string DatabaseUser;

		/// <summary>
		/// The database password.
		/// </summary>
		public string DatabasePassword;

		public string Note;
		public int ItemOrder;

		/// <summary>
		/// Contains the most recent information about this connection. 
		/// OK if no problems, version information if version mismatch, nothing for not checked, and OFFLINE if previously couldn't connect.
		/// </summary>
		public string ConnectionStatus;

		/// <summary>
		/// If set to True, display clinic breakdown in reports, else only show practice totals.
		/// </summary>
		public bool HasClinicBreakdownReports;

		/// <summary>
		/// Set when reading from the config file. Not an actual DB column.
		/// </summary>
		[Ignore]
		public bool IsAutomaticLogin;

		/// <summary>
		/// Gets a value indicating whether the connection is vaid.
		/// </summary>
		public bool IsConnectionValid() => ConnectionStatus == "OK";

		/// <summary>
		/// Gets a description of the connection.
		/// </summary>
		public string Description => $"{DatabaseServer}, {DatabaseName}";

		/// <summary>
		/// Returns a string representation of the connection.
		/// </summary>
		public override string ToString() => Description;

        public static object GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
