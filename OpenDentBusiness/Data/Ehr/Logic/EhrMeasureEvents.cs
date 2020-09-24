using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class EhrMeasureEvents
	{
		public static IEnumerable<EhrMeasureEvent> GetByPatient(long patientId)
			=> SelectMany(
				"SELECT * FROM `ehr_measure_events` " +
				"WHERE `patient_id` = " + patientId + " " +
				"ORDER BY `date`");

		public static IEnumerable<EhrMeasureEvent> GetByPatient(long patientId, params EhrMeasureEventType[] ehrMeasureEventTypes)
		{
			if (ehrMeasureEventTypes.Length == 0) return GetByPatient(patientId);

			return SelectMany(
				"SELECT * FROM `ehr_measure_events` " +
				"WHERE `type` IN (" + string.Join(", ", ehrMeasureEventTypes.Select(ehrMeasureEventType => (int)ehrMeasureEventType)) + ") " +
				"AND `patient_id` = " + patientId + "ORDER BY `date`");
		}

		public static IEnumerable<EhrMeasureEvent> GetByDateRange(DateTime dateStart, DateTime dateEnd, EhrMeasureEventType? ehrMeasureEventType = null)
		{
			var command = "SELECT * FROM ehr_measure_events WHERE date >= @date_start AND date <= @date_end";

			if (ehrMeasureEventType.HasValue)
			{
				command += " AND `type` = " + (int)ehrMeasureEventType.Value;
			}

			command += " ORDER BY `type`, `date`, `patient_id`";

			return SelectMany(command,
				new MySqlParameter("date_start", dateStart),
				new MySqlParameter("date_end", dateEnd));
		}

		public static string GetLatestInfoByType(EhrMeasureEventType measureEventType)
		{
			var ehrMeasureEvent = SelectOne(
				"SELECT * FROM `ehr_measure_events` " +
				"WHERE `type` = " + (int)measureEventType + " " +
				"ORDER BY `date` DESC LIMIT 1");

			return ehrMeasureEvent?.MoreInfo ?? "";
		}

		public static IEnumerable<EhrMeasureEvent> GetByType(List<EhrMeasureEvent> ehrMeasureEvents, EhrMeasureEventType ehrMeasureEventType)
			=> ehrMeasureEvents.Where(ehrMeasureEvent => ehrMeasureEvent.Type == ehrMeasureEventType);

		public static long Create(long patientId, EhrMeasureEventType ehrMeasureEventType)
		{
            return ExecuteInsert(new EhrMeasureEvent
			{
				Date = DateTime.Now,
				Type = ehrMeasureEventType,
				PatientId = patientId
			});
		}

		public static void Save(EhrMeasureEvent ehrMeasureEvent)
		{
			if (ehrMeasureEvent.Id == 0) ExecuteInsert(ehrMeasureEvent);
            else
            {
				ExecuteUpdate(ehrMeasureEvent);
            }
		}

		public static void Delete(long ehrMeasureEventId) 
			=> ExecuteDelete(ehrMeasureEventId);

		public static IEnumerable<string> GetResultCodesUsedByType(EhrMeasureEventType ehrMeasureEventType) 
			=> Database.SelectMany(
				"SELECT `result_code` FROM `ehr_measure_events` " +
				"WHERE `type` = " + (int)ehrMeasureEventType + " " +
				"AND `result_code` != '' AND DATE(`date`) >= @date " +
				"GROUP BY `result_code`", Database.ToScalar<string>, 
					new MySqlParameter("date", DateTime.Now.AddYears(-1)));
	}
}
