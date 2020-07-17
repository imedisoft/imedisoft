using CodeBase;

namespace Imedisoft.Data
{
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
}
