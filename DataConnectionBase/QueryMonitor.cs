using System;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using CodeBase;

namespace DataConnectionBase {
	public class QueryMonitor {
		///<summary>Set to true if monitoring queries from FormQueryMonitor.  This will cause the query log to contain runnable queries (i.e. with query
		///parameters replaced with actual parameter value), and will include the method name of the calling method in DataConnection (i.e. GetTable, NonQ, etc).</summary>
		public static bool IsMonitoring=false;

		public static void RunMonitoredQuery(Action queryAction,DbCommand cmd) {
			DbQueryObj dbQueryObj=null;
			Stopwatch s=null;
			if(IsMonitoring) {
				dbQueryObj=new DbQueryObj(cmd.CommandText);
				//order by descending length of parameter name so that replacing parameter '@Note' doesn't replace part of parameter '@NoteBold'
				cmd.Parameters.OfType<DbParameter>().OrderByDescending(x => x.ParameterName.Length)
					.ForEach(x => dbQueryObj.Command=dbQueryObj.Command.Replace(x.ParameterName,"'"+SOut.String(x.Value.ToString())+"'"));
				dbQueryObj.MethodName=new StackTrace().GetFrame(3).GetMethod().Name;
				//Synchronously notify anyone that cares that the query has started to execute.
				QueryMonitorEvent.Fire(ODEventType.QueryMonitor,dbQueryObj);
				dbQueryObj.DateTimeStart=DateTime.Now;
				//using stopwatch to time queries because the resolution of DateTime.Now is between 0.5 and 15 milliseconds which makes it not suitable for use
				//as a benchmarking tool.  See https://docs.microsoft.com/en-us/dotnet/api/system.datetime.now
				s=Stopwatch.StartNew();
			}
			queryAction();
			if(IsMonitoring) {
				s.Stop();
				dbQueryObj.DateTimeStop=dbQueryObj.DateTimeStart.Add(s.Elapsed);
				//Synchronously notify anyone that cares that the query has finished executing.
				QueryMonitorEvent.Fire(ODEventType.QueryMonitor,dbQueryObj);
			}
		}
	}


	public class QueryMonitorEvent {
		public static event ODEventHandler Fired;
		public static void Fire(ODEventType odEventType,object tag) {
			Fired?.Invoke(new ODEventArgs(odEventType,tag));
		}
	}

}
