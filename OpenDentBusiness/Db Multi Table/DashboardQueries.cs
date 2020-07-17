using System;
using System.Collections.Generic;
//using System.Windows.Controls;//need a reference for this dll, or get msgbox into UI layer.
using System.Data;
using System.Text;
using System.Reflection;
using Imedisoft.Data;

namespace OpenDentBusiness
{
	public class DashboardQueries
	{
#if DEBUG
		///<summary>Set this boolean to true if you want to have message boxes pop up after each method is run when in debug mode.  Used to time long computations before loading the dashboard.</summary>
		private static bool _showElapsedTimesForDebug = false;
		private static string _elapsedTimeAR = "";
#endif

		///<summary>Returns all DashbaordAR(s) for the given time period. Caution, this will run aging and calculate a/r if a month within the given range is missing.
		///This can take several seconds per month missing.</summary>
		public static List<DashboardAR> GetAR(DateTime dateFrom, DateTime dateTo, List<DashboardAR> listDashAR)
		{
			//assumes that dateFrom is the first of the month.
			string command;
			List<DashboardAR> listRet = new List<DashboardAR>();
#if DEBUG
			_elapsedTimeAR = "";
			System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
			System.Diagnostics.Stopwatch stopWatchTotal = new System.Diagnostics.Stopwatch();
			_elapsedTimeAR = "Elapsed time for GetAR:\r\n";
			stopWatchTotal.Start();
#endif
			int months = 0;
			while (dateTo >= dateFrom.AddMonths(months))
			{ //calculate the number of months between the two dates.
				months++;
			}
			for (int i = 0; i < months; i++)
			{
				DateTime dateLastOfMonth = dateFrom.AddMonths(i + 1).AddDays(-1);
				DashboardAR dash = null;
				for (int d = 0; d < listDashAR.Count; d++)
				{
					if (listDashAR[d].DateCalc != dateLastOfMonth)
					{
						continue;
					}
					dash = listDashAR[d];
				}
				if (dash != null)
				{//we found a DashboardAR object from the database for this month, so use it.
					listRet.Add(dash);
					continue;
				}
#if DEBUG
				stopWatch.Restart();
#endif
				//run historical aging on all patients based on the date entered.
				command = "SELECT SUM(Bal_0_30+Bal_31_60+Bal_61_90+BalOver90),SUM(InsEst) "
					+ "FROM (" + Ledgers.GetAgingQueryString(dateLastOfMonth, isHistoric: true) + ") guarBals";
				DataTable table = ReportsComplex.RunFuncOnReportServer(() => Database.ExecuteDataTable(command));
#if DEBUG
				stopWatch.Stop();
				_elapsedTimeAR += "Aging using Ledgers.GetHistoricAgingQueryString() #" + i + " : " + stopWatch.Elapsed.ToString() + "\r\n";
#endif
				dash = new DashboardAR();
				dash.DateCalc = dateLastOfMonth;
				dash.BalTotal = PIn.Double(table.Rows[0][0].ToString());
				dash.InsEst = PIn.Double(table.Rows[0][1].ToString());
				DashboardARs.Insert(dash);//save it to the db for later. 
				if (!string.IsNullOrEmpty(PrefC.ReportingServer.Server))
				{ //only attempt to insert into the reporting server if the reporting server is set up.
					ReportsComplex.RunFuncOnReportServer(() => (DashboardARs.Insert(dash)));//save it to the db for later.
				}
				listRet.Add(dash); //and also use it now.
			}
#if DEBUG
			stopWatchTotal.Stop();
			_elapsedTimeAR += "Total: " + stopWatchTotal.Elapsed.ToString();
			if (_showElapsedTimesForDebug)
			{
				System.Windows.Forms.MessageBox.Show(_elapsedTimeAR);
			}
#endif
			return listRet;
		}

		#region OpenDentalGraph Queries
		public static DataTable GetTable(string command, bool doRunOnReportServer = true)
		{
			return ReportsComplex.RunFuncOnReportServer(() => Database.ExecuteDataTable(command), doRunOnReportServer);
		}
		#endregion
	}
}
