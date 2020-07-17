using CodeBase;
using DataConnectionBase;
using System;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace Imedisoft.Data
{
    public class QueryMonitor
	{
		/// <summary>
		/// Set to true if monitoring queries from FormQueryMonitor.
		/// This will cause the query log to contain runnable queries (i.e. with query parameters replaced with actual parameter value),
		/// and will include the method name of the calling method in DataConnection (i.e. GetTable, ExecuteNonQuery, etc).
		/// </summary>
		public static bool IsMonitoring { get; set; } = false;

		public static void RunMonitoredQuery(Action queryAction, DbCommand command)
		{
			QueryInfo queryInfo = null;

			Stopwatch stopwatch = null;

			if (IsMonitoring)
			{
				queryInfo = new QueryInfo(command.CommandText);

				var parameters = command.Parameters
					.OfType<DbParameter>()
					.OrderByDescending(
						parameter => parameter.ParameterName.Length);

				foreach (var param in parameters)
                {
					queryInfo.Command = queryInfo.Command.Replace(
						param.ParameterName, "'" + SOut.String(param.Value.ToString()) + "'");
				}

				queryInfo.MethodName = new StackTrace().GetFrame(3).GetMethod().Name;

				// Synchronously notify anyone that cares that the query has started to execute.
				QueryMonitorEvent.Fire(EventCategory.QueryMonitor, queryInfo);

				// Using stopwatch to time queries because the resolution of DateTime.Now is between 0.5 and 15 milliseconds
				// which makes it not suitable for use as a benchmarking tool. 
				// See https://docs.microsoft.com/en-us/dotnet/api/system.datetime.now
				stopwatch = Stopwatch.StartNew();
			}

			queryAction();

			if (IsMonitoring)
			{
				stopwatch.Stop();

				queryInfo.StopTime = queryInfo.StartTime.Add(stopwatch.Elapsed);

				QueryMonitorEvent.Fire(EventCategory.QueryMonitor, queryInfo);
			}
		}
	}

	public class QueryMonitorEvent
	{
		public static event ODEventHandler Fired;

		public static void Fire(EventCategory odEventType, object tag)
		{
			Fired?.Invoke(new ODEventArgs(odEventType, tag));
		}
	}
}
