using System.ComponentModel;

namespace Imedisoft.Data
{
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
}
