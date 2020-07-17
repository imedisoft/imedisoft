namespace Imedisoft.Data
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

	public delegate void DataConnectionEventHandler(DataConnectionEventArgs e);
}
