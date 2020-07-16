using CodeBase;
using System.ComponentModel;

namespace DataConnectionBase
{
    /// <summary>
    /// Specific ODEvent for when communication to the database is unavailable.
    /// </summary>
    public class DataConnectionEvent
	{
		/// <summary>
		/// This event will get fired whenever communication to the database is attempted and fails.
		/// </summary>
		public static event DataConnectionEventHandler Fired;

		/// <summary>
		/// Call this method only when communication to the database is not possible.
		/// </summary>
		public static void Fire(DataConnectionEventArgs e) => Fired?.Invoke(e);
	}

	public class DataConnectionEventArgs : ODEventArgs
	{
		/// <summary>
		/// This will be set to true once the connection to the database has been restored.
		/// </summary>
		public bool IsConnectionRestored;

		/// <summary>
		/// The connection string of the database that this event is for.
		/// </summary>
		public string ConnectionString;

		public DataConnectionEventArgs(DataConnectionEventType error, bool isConnectionRestored, string connectionString)
			: base(EventCategory.DataConnection, error.GetDescription())
		{
			IsConnectionRestored = isConnectionRestored;
			ConnectionString = connectionString;
		}
	}

	/// <summary>
	/// A list of the types of mysql errors handled through FormConnectionLost
	/// </summary>
	public enum DataConnectionEventType
	{
		/// <summary>
		/// Occurs when the connection is lost with the MySQL server.
		/// </summary>
		[Description("Connection to the MySQL server has been lost. Connectivity will be retried periodically. Click Retry to attempt to connect manually or Exit Program to close the program.")]
		ConnectionLost,

		/// <summary>
		/// Occurs when the connection refuses to connect due to too many connections to the server.
		/// </summary>
		[Description("Too many connections have been made to the database. Consider increasing the max_connections variable in your my.ini file. Connectivity will be retried periodically.  Click Retry to attempt to connect manually or Exit Program to close the program.")]
		TooManyConnections,

		/// <summary>
		/// Occurs when the connection has successfully restored.
		/// </summary>
		[Description("Connection Restored.")]
		ConnectionRestored,

		/// <summary>
		/// Occurs when unable to read from the MySQL data adapter.
		/// </summary>
		[Description("Reading from MySQL has failed. Connectivity will be retried periodically. Click Retry to attempt to retry manually or Exit Program to close the program.")]
		DataReaderNull,
	}

	public delegate void DataConnectionEventHandler(DataConnectionEventArgs e);
}
