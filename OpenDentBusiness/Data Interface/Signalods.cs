using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using CodeBase;
using DataConnectionBase;
using OpenDentBusiness.UI;
using System.Windows;
using OpenDentBusiness.WebTypes;
using Imedisoft.Data;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices.WindowsRuntime;

namespace OpenDentBusiness
{
	public class Signalods
	{
		/// <summary>
		/// This is not the actual date/time last refreshed.
		/// It is really the server based date/time of the last item in the database retrieved on previous refreshes.
		/// That way, the local workstation time is irrelevant.
		/// </summary>
		public static DateTime SignalLastRefreshed { get; set; }

		/// <summary>
		/// Mimics the behavior of SignalLastRefreshed, but is used exclusively in ContrAppt.TickRefresh().
		/// The root issue was that when a client came back from being inactive 
		/// ContrAppt.TickRefresh() was using SignalLastRefreshed, which is only set after we process signals.
		/// Therefore, when a client went inactive, we could potentially query the SignalOD table for a much larger dataset than intended.
		/// E.g.- Client goes inactive for 3 hours, comes back, ContrAppt.TickRefresh() is called and calls RefreshTimed() with a 3 hour old datetime.
		/// </summary>
		public static DateTime ApptSignalLastRefreshed { get; set; }
		

		public static Signalod FromReader(MySqlDataReader dataReader)
        {
			return new Signalod
			{
				Id = (long)dataReader["id"],
				Date = (DateTime)dataReader["date"],
				Name = (string)dataReader["name"],
				Param1 = dataReader["param1"] as string,
				Param2 = dataReader["param2"] as long?,
				Param3 = dataReader["param3"] as DateTime?
			};
        }

		private static Signalod MakeSignal(string name, string param1 = null, long? param2 = null, DateTime? param3 = null)
		{
			return new Signalod
			{
				Name = name,
				Date = DateTime.UtcNow,
				Param1 = param1,
				Param2 = param2,
				Param3 = param3
			};
		}

		public static object ValueOrDbNull(string value)
			=> value == null ? (object)DBNull.Value : value;

        public static object ValueOrDbNull<T>(T? value) where T : struct
			=> value.HasValue ? (object)value.Value : DBNull.Value;
        
        private static long Insert(Signalod signal)
			=> signal.Id = Database.ExecuteInsert(
				"INSERT INTO `signals` (`date`, `name`, `param1`, `param2`, `param3`) VALUES (@date, @name, @param1, @param2, @param3)",
					new MySqlParameter("date", signal.Date),
					new MySqlParameter("name", signal.Name ?? ""),
					new MySqlParameter("param1", ValueOrDbNull(signal.Param1)),
					new MySqlParameter("param2", ValueOrDbNull(signal.Param2)),
					new MySqlParameter("param3", ValueOrDbNull(signal.Param3)));


		/// <summary>
		/// Gets all Signals since a given DateTime.
		/// If it can't connect to the database, then it returns a list of length 0.
		/// Remeber that the supplied dateTime is server time.  This has to be accounted for.
		/// ListITypes is an optional parameter for querying specific signal types.
		/// </summary>
		public static IEnumerable<Signalod> RefreshTimed(DateTime sinceDateT, params string[] signalNames)
		{
			string command = "SELECT * FROM `signals` WHERE (`date` > " + POut.DateT(sinceDateT) + " AND `date` < NOW())";

			if (signalNames != null && signalNames.Length > 0)
			{
				command += " AND `name` IN (" + string.Join(",", signalNames.Select(name => MySqlHelper.EscapeString(name))) + ")";
			}

			command += " ORDER BY `date`";

			return Database.SelectMany(command, FromReader);
		}

		public static void Insert(params Signalod[] signals)
		{
			foreach (var signal in signals)
			{
				Insert(signal);
			}
		}

		/// <summary>
		/// Simplest way to use the new fKey and FKeyType. Set isBroadcast=true to process signals immediately on workstation.
		/// </summary>
		public static long SetInvalid(InvalidType iType, KeyType fKeyType, long fKey) 
			=> Send(SignalName.Invalidate, iType.ToString(), fKey);

		/// <summary>
		/// Creates up to 3 signals for each supplied appt.
		/// The signals are needed for many different kinds of changes to the appointment, but the signals only specify Provs and Ops because that's what's needed to tell workstations which views to refresh.
		/// Always call a refresh of the appointment module before calling this method. 
		/// apptNew cannot be null.
		/// apptOld is only used when making changes to an existing appt and Provs or Ops have changed.
		/// Generally should not be called outside of Appointments.cs
		/// </summary>
		public static void SetInvalidAppt(Appointment apptNew, Appointment apptOld = null)
		{
			// TODO: Fix me...

			//if (apptNew == null)
			//{
			//	//If apptOld is not null then use it as the apptNew so we can send signals
			//	//Most likely occurred due to appointment delete.
			//	if (apptOld != null)
			//	{
			//		apptNew = apptOld;
			//		apptOld = null;
			//	}
			//	else
			//	{
			//		return;//should never happen. Both apptNew and apptOld are null in this scenario
			//	}
			//}

			//bool addSigForNewApt = IsApptInRefreshRange(apptNew);
			//bool addSignForOldAppt = IsApptInRefreshRange(apptOld);
			////The six possible signals are:
			////  1.New Provider
			////  2.New Hyg
			////  3.New Op
			////  4.Old Provider
			////  5.Old Hyg
			////  6.Old Op
			////If there is no change between new and old, or if there is not an old appt provided, then fewer than 6 signals may be generated.
			//List<Signalod> listSignals = new List<Signalod>();
			//if (addSigForNewApt)
			//{
			//	//  1.New Provider
			//	listSignals.Add(
			//		new Signalod()
			//		{
			//			InvalidDate = apptNew.AptDateTime,
			//			InvalidType = InvalidType.Appointment,
			//			InvalidForeignKey = apptNew.ProvNum,
			//			Name = KeyType.Provider,
			//		});
			//	//  2.New Hyg
			//	if (apptNew.ProvHyg > 0)
			//	{
			//		listSignals.Add(
			//			new Signalod()
			//			{
			//				InvalidDate = apptNew.AptDateTime,
			//				InvalidType = InvalidType.Appointment,
			//				InvalidForeignKey = apptNew.ProvHyg,
			//				Name = KeyType.Provider,
			//			});
			//	}
			//	//  3.New Op
			//	if (apptNew.Op > 0)
			//	{
			//		listSignals.Add(
			//			new Signalod()
			//			{
			//				InvalidDate = apptNew.AptDateTime,
			//				InvalidType = InvalidType.Appointment,
			//				InvalidForeignKey = apptNew.Op,
			//				Name = KeyType.Operatory,
			//			});
			//	}
			//}
			//if (addSignForOldAppt)
			//{
			//	//  4.Old Provider
			//	if (apptOld != null && apptOld.ProvNum > 0 && (apptOld.AptDateTime.Date != apptNew.AptDateTime.Date || apptOld.ProvNum != apptNew.ProvNum))
			//	{
			//		listSignals.Add(
			//			new Signalod()
			//			{
			//				InvalidDate = apptOld.AptDateTime,
			//				InvalidType = InvalidType.Appointment,
			//				InvalidForeignKey = apptOld.ProvNum,
			//				Name = KeyType.Provider,
			//			});
			//	}
			//	//  5.Old Hyg
			//	if (apptOld != null && apptOld.ProvHyg > 0 && (apptOld.AptDateTime.Date != apptNew.AptDateTime.Date || apptOld.ProvHyg != apptNew.ProvHyg))
			//	{
			//		listSignals.Add(
			//			new Signalod()
			//			{
			//				InvalidDate = apptOld.AptDateTime,
			//				InvalidType = InvalidType.Appointment,
			//				InvalidForeignKey = apptOld.ProvHyg,
			//				Name = KeyType.Provider,
			//			});
			//	}
			//	//  6.Old Op
			//	if (apptOld != null && apptOld.Op > 0 && (apptOld.AptDateTime.Date != apptNew.AptDateTime.Date || apptOld.Op != apptNew.Op))
			//	{
			//		listSignals.Add(
			//			new Signalod()
			//			{
			//				InvalidDate = apptOld.AptDateTime,
			//				InvalidType = InvalidType.Appointment,
			//				InvalidForeignKey = apptOld.Op,
			//				Name = KeyType.Operatory,
			//			});
			//	}
			//}
			//listSignals.ForEach(x => Insert(x));
			////There was a delay when using this method to refresh the appointment module due to the time it takes to loop through the signals that iSignalProcessors need to loop through.
			////BroadcastSignals(listSignals);//for immediate update. Signals will be processed again at next tick interval.
		}

		/// <summary>
		/// Returns true if the Apppointment.AptDateTime is between DateTime.Today and the number of ApptAutoRefreshRange preference days.
		/// </summary>
		public static bool IsApptInRefreshRange(Appointment appt)
		{
			if (appt == null) return false;

			int days = PrefC.GetInt(PreferenceName.ApptAutoRefreshRange);
			if (days == -1)
			{
				// ApptAutoRefreshRange preference is -1, so all appointments are in range
				return true;
			}

			// Returns true if the appointment is between today and today + the auto refresh day range preference.
			return appt.AptDateTime.Between(DateTime.Today, DateTime.Today.AddDays(days));
		}

		/// <summary>
		/// The given dateStart must be less than or equal to dateEnd. Both dates must be valid dates (not min date, etc).
		/// </summary>
		public static void SetInvalidSchedForOps(Dictionary<DateTime, List<long>> operatoriesByDateTime)
		{
			foreach (var kvp in operatoriesByDateTime)
            {
				foreach (var operatoryId in kvp.Value.Distinct())
                {
					Send(SignalName.Schedules, null, operatoryId, kvp.Key);
                }
            }
		}

		/// <summary>
		///		<para>
		///			Sends a signal with the specified name and value.
		///		</para>
		/// </summary>
		/// <param name="name">The name of the signal.</param>
		/// <param name="param1"></param>
		/// <param name="param2"></param>
		/// <param name="param3"></param>
		public static long Send(string name, string param1 = null, long? param2 = null, DateTime? param3 = null) 
			=> Insert(MakeSignal(name, param1, param2, param3));

		/// <summary>
		///		<para>
		///			Invalidates the cache group with the specified name.
		///		</para>
		///		<para>
		///			Triggers a cache refresh on every workstation.
		///		</para>
		/// </summary>
		/// <param name="cacheGroup"></param>
		public static long Invalidate(string cacheGroup)
			=> Send("invalidate", cacheGroup);

		public static long InvalidateDate(DateTime dateTime)
			=> Send("invalidate_date", dateTime.ToString());

		/// <summary>
		/// Inserts a signal for each operatory in the schedule that has been changed, and for the provider the schedule is for.
		/// This only inserts a signal for today's schedules. 
		/// Generally should not be called outside of Schedules.cs (then why is it no declared there? -_-....)
		/// </summary>
		public static void SetInvalidSched(params Schedule[] schedules)
		{
			var signalOperatories = schedules
				.Where(
					schedule => schedule.SchedDate == DateTime.UtcNow.Date)
				.SelectMany(
					schedule => schedule.Ops.Select(
						operatoryId => MakeSignal(SignalName.Schedules, "operatory", operatoryId, schedule.SchedDate)));

			var signalProviders = schedules
				.Where(
					schedule => schedule.ProvNum > 0 && schedule.SchedDate == DateTime.UtcNow.Date)
				.Select(
					schedule => MakeSignal(SignalName.Schedules, "provider", schedule.ProvNum, param3: schedule.SchedDate));

			var signals = signalOperatories.Union(signalProviders).ToList();
			if (signals.Count > 1000)
            {
				Insert(MakeSignal(SignalName.Schedules));

				return;
            }

			Insert(signals.ToArray());
		}

		/// <summary>
		/// Schedules, when we don't have a specific FKey and want to set an invalid for the entire type. 
		/// Includes the dateViewing parameter for Refresh.
		/// A dateViewing of 01-01-0001 will be ignored because it would otherwise cause a full refresh for all connected client workstations.
		/// </summary>
		public static void SetInvalidSched(DateTime dateViewing)
		{
			if (dateViewing == DateTime.MinValue) return;

			Insert(MakeSignal(SignalName.Schedules, null, null, dateViewing));
		}


		/// <summary>
		/// Check for appointment signals for a single date.
		/// </summary>
		public static bool IsApptRefreshNeeded(DateTime dateTimeShowing, List<Signalod> signals, List<long> visibleOperatoryIds, List<long> listProvNumsVisible)
		{
			return IsApptRefreshNeeded(dateTimeShowing, dateTimeShowing, signals, visibleOperatoryIds, listProvNumsVisible);
		}

		///<summary>After a refresh, this is used to determine whether the Appt Module needs to be refreshed. Returns true if there are any signals
		///with InvalidType=Appointment where the DateViewing time of the signal falls within the provided daterange, and the signal matches either
		///the list of visible operatories or visible providers in the current Appt Module View. Always returns true if any signals have
		///DateViewing=DateTime.MinVal.</summary>
		public static bool IsApptRefreshNeeded(DateTime startDate, DateTime endDate, List<Signalod> signalList, List<long> listOpNumsVisible, List<long> listProvNumsVisible)
		{
			// TODO: Fix me...

			////A date range was refreshed.  Easier to refresh all without checking.
			//if (signalList.Exists(x => x.InvalidDate.Date == DateTime.MinValue.Date && x.InvalidType == InvalidType.Appointment))
			//{
			//	return true;
			//}
			//List<Signalod> listApptSignals = signalList.FindAll(x => x.InvalidType == InvalidType.Appointment &&
			//	  x.InvalidDate.Date >= startDate.Date && x.InvalidDate.Date <= endDate.Date);
			//if (listApptSignals.Count == 0)
			//{
			//	return false;
			//}

			////List<long> visibleOps = ApptDrawing.VisOps.Select(x => x.OperatoryNum).ToList();
			////List<long> visibleProvs = ApptDrawing.VisProvs.Select(x => x.ProvNum).ToList();
			//if (listApptSignals.Any(x => x.Name == KeyType.Operatory && listOpNumsVisible.Contains(x.InvalidForeignKey)) || 
			//	listApptSignals.Any(x => x.Name == KeyType.Provider && listProvNumsVisible.Contains(x.InvalidForeignKey)))
			//{
			//	return true;
			//}
			return false;
		}

		/// <summary>
		/// Check for schedule signals for a single date.
		/// </summary>
		public static bool IsSchedRefreshNeeded(DateTime date, List<Signalod> signals, List<long> operatoryIds, List<long> providerIds) 
			=> IsSchedRefreshNeeded(date, date, signals, operatoryIds, providerIds);

		/// <summary>
		/// After a refresh, this is used to determine whether the Appt Module needs to be refreshed.
		/// Returns true if there are any signals with InvalidType=Appointment where the DateViewing time of the signal 
		/// falls within the provided daterange, and the signal matches either the list of visible operatories or visible 
		/// providers in the current Appt Module View. 
		/// Always returns true if any signals have DateViewing=DateTime.MinVal.
		/// </summary>
		public static bool IsSchedRefreshNeeded(DateTime startDate, DateTime endDate, List<Signalod> signals, List<long> operatoryIds, List<long> providerIds)
		{
			// If there is a scheduler signal without a date attached, refresh all...
			if (signals.Any(signal => signal.Name == SignalName.Schedules && !signal.Param3.HasValue)) 
				return true;

			// Get only the signals within the specified date range...
			signals = signals.Where(
				signal => 
					signal.Param3.HasValue && 
					signal.Param3 >= startDate.Date && 
					signal.Param3 <= endDate.Date).ToList();

			if (signals.Count > 0)
            {
				foreach (var signal in signals)
                {
					switch (signal.Param1)
                    {
						case "operatory":
							if (signal.Param2.HasValue && operatoryIds.Contains(signal.Param2.Value))
							{
								return true;
							}
							break;

						case "provider":
							if (signal.Param2.HasValue && providerIds.Contains(signal.Param2.Value))
                            {
								return true;
                            }
							break;

						default: 
							return true;
                    }
                }
            }

			return false;
		}

		/// <summary>
		/// After a refresh, this is used to determine whether the buttons and listboxes need to be refreshed on the ContrApptPanel. 
		/// Will return true with InvalidType==Defs.
		/// </summary>
		public static bool IsContrApptButtonRefreshNeeded(IEnumerable<Signalod> signals) 
			=> signals.Any(s => s.Name == SignalName.Invalidate && s.Param1 == nameof(InvalidType.Defs));

		/// <summary>
		/// After a refresh, this is used to get a list containing all flags of types that need to be refreshed. 
		/// The FKey must be 0 and the FKeyType must Undefined. Types of Task and SmsTextMsgReceivedUnreadCount are not included.
		/// </summary>
		public static IEnumerable<InvalidType> GetInvalidTypes(List<Signalod> signals) 
			=> signals
				.Where(
					signal => signal.Name == SignalName.Invalidate && signal.Param1 != null)
				.Select(
					signal => (InvalidType)Enum.Parse(typeof(InvalidType), signal.Param1));

		/// <summary>
		/// Won't work with InvalidType.Date, InvalidType.Task, or InvalidType.TaskPopup yet.
		/// </summary>
		public static void SetInvalid(params InvalidType[] invalidTypes)
		{
			foreach (var invalidType in invalidTypes)
			{
				Send(SignalName.Invalidate, invalidType.ToString());
			}
		}

		public static void SetInvalidNoCache(params InvalidType[] invalidTypes)
			=> SetInvalid(invalidTypes);

		/// <summary>
		/// Must be called after Preference cache has been filled.
		/// Deletes all signals older than 2 days if this has not been run within the last week.
		/// Will fail silently if anything goes wrong.
		/// </summary>
		public static void ClearOldSignals()
		{
			try
			{
				var dateLastCleared = Preferences.GetDateTimeOrNull(nameof(PreferenceName.SignalLastClearedDate));
				if (dateLastCleared.HasValue && dateLastCleared > DateTime.UtcNow.AddDays(-7))
                {
					return;
                }

				Database.ExecuteNonQuery("CALL `clear_signals`");

				SigMessages.ClearOldSigMessages();

				Preferences.Set(nameof(PreferenceName.SignalLastClearedDate), DateTime.UtcNow);
			}
			catch
			{
			}
		}
	}
}
