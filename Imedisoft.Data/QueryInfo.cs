using System;

namespace Imedisoft.Data
{
    /// <summary>
    /// Helper object that holds metadata regarding a query that was executed by the program.
    /// </summary>
    public class QueryInfo
	{
		/// <summary>
		/// Gets a globally unique identifier for this particular query.
		/// </summary>
		/// <remarks>
		/// Useful for updating <see cref="StopTime"/> when query finishes executing.
		/// </remarks>
		public Guid GUID { get; }

		/// <summary>
		/// Gets or sets the full query command text.
		/// </summary>
		public string Command { get; set; }

		/// <summary>
		/// Gets or sets the starting date and time of the query.
		/// </summary>
		public DateTime StartTime { get; } = DateTime.Now;

		/// <summary>
		/// Gets or sets the date on which the query finished executing.
		/// </summary>
		public DateTime? StopTime { get; set; }

		/// <summary>
		/// Gets or sets the name of the method that started the query.
		/// </summary>
		public string MethodName { get; set; }

		/// <summary>
		/// Returns the difference between DateTimeStop and DateTimeStart if both are valid date times.
		/// </summary>
		public TimeSpan Elapsed
		{
			get
			{
				if (StopTime.HasValue)
                {
					return StopTime.Value - StartTime;
                }

				return TimeSpan.Zero;
			}
		}

		/// <summary>
		/// Creates a query object that stores the command passed in and automatically sets a GUID and DateTimeStart.
		/// </summary>
		public QueryInfo(string command)
		{
			GUID = Guid.NewGuid();
			Command = command;
		}

		/// <summary>
		/// A string representation of what this query object should look like in the log file.
		/// </summary>
		public override string ToString() => 
			$"# GUID: {GUID}  Method Name: {MethodName ?? "<Unknown>"}\r\n" + 
			$"# DateTimeStart: {StartTime:MM/dd/yyyy hh:mm:ss.fffffff tt}\r\n" + 
			$"# DateTimeStop: {(StopTime.HasValue ? StopTime.Value.ToString("MM/dd/yyyy hh:mm:ss.fffffff tt") : "Still Running")}\r\n" + 
			$"# Elapsed: {Elapsed:G}\r\n" + 
			$"{Command}";
	}
}
