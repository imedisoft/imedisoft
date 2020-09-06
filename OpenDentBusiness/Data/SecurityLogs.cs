using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Data
{
    /// <summary>
    ///		<para>
    ///			 Stores an ongoing record of database activity for security purposes.
    ///		</para>
    ///		<para>
    ///			User not allowed to edit.
    ///		</para>
    /// </summary>
    public partial class SecurityLogs
	{
		public static SecurityLogSource LogSource = SecurityLogSource.None;

		public static SecurityLog GetOne(long securityLogId)
		{
			return SelectOne(securityLogId);
		}

		public static List<SecurityLog> GetMany(params SQLWhere[] whereClauses)
		{
			return GetMany(new List<SQLWhere>(whereClauses));
		}

		public static List<SecurityLog> GetMany(List<SQLWhere> whereClauses)
		{
			var command = "SELECT * FROM `security_logs` ";

			if (whereClauses != null && whereClauses.Count > 0)
			{
				command += "WHERE " + string.Join(" AND ", whereClauses);
			}

			return SelectMany(command).ToList();
		}

		public static void DeleteWithMaxPriKey(long maxSecurityLogId)
		{
			if (maxSecurityLogId == 0)
			{
				return;
			}

			Database.ExecuteNonQuery("DELETE FROM `security_logs` WHERE `id` <= " + maxSecurityLogId);
		}

		/// <summary>
		///		<para>
		///			Removes all security log entries before (and on) the given 
		///			<paramref name="date"/>. We cannot simply delete entries from the 
		///			'security_logs' table because the FK contraint on the 'security_log_hashes'
		///			table forbids this. So when deleting log entries we first have to delete
		///			the associated hashes.
		///		</para>
		/// </summary>
		/// <param name="date">The date.</param>
		public static void DeleteBeforeDateInclusive(DateTime date)
		{
			date = date.Date;

			int countDeleted = 0;

			List<long> securityLogIds;
			do
			{
				// Delete the hashes
				MiscDataEvent.Fire(CodeBase.EventCategory.MiscData,
					"Removing old data from 'security_log_hashes' table. Rows deleted so far: " + countDeleted);

				// Limiting to 500,000 to avoid out of memory exceptions
				securityLogIds = Database.SelectMany(
					"SELECT `id` FROM `security_logs` WHERE DATE(`log_date`) <= @date LIMIT 500000", Database.ToScalar<long>,
						new MySqlParameter("date", date.Date)).ToList();

				if (securityLogIds.Count == 0)
				{
					break;
				}

				SecurityLogHashes.DeleteForSecurityLogEntries(securityLogIds);

				MiscDataEvent.Fire(CodeBase.EventCategory.MiscData,
					"Removing old data from 'security_logs' table. Rows deleted so far: " + countDeleted);

				Database.ExecuteNonQuery("DELETE FROM `security_logs` WHERE `id` IN (" + string.Join(", ", securityLogIds) + ")");

				countDeleted += securityLogIds.Count;
			}
			while (securityLogIds.Count > 0);
		}



		public static SecurityLog[] Refresh(
			DateTime dateFrom, DateTime dateTo, Permissions permissionType, long? patientId, long? userId, DateTime datePreviousFrom, DateTime datePreviousTo, int limit = 0)
		{
			var command =
				"SELECT sl.*, `make_patient_id`(p.`patient_id`, FName, LName) AS `patient_name`, `hash` " +
				"FROM `security_logs` sl " +
				"LEFT JOIN `patient` p ON p.`PatNum` = sl.`patient_id` " +
				"LEFT JOIN `security_log_hashes` slh ON slh.`security_log_id` = sl.`id` " +
				"WHERE (`log_date` BETWEEN @date_from AND @date_to) " +
				"AND (`object_date` BETWEEN @date_previous_from AND @date_previous_to)";

			if (patientId.HasValue)
			{
				command += " AND sl.`patient_id` IN (" + string.Join(", ",
					PatientLinks.GetPatNumsLinkedToRecursive(patientId.Value, PatientLinkType.Merge)) + ")";
			}

			if (permissionType != Permissions.None)
			{
				command += " AND `type` = " + (int)permissionType;
			}

			if (userId.HasValue)
			{
				command += " AND `user_id` =" + userId.Value;
			}

			command += " ORDER BY `log_date` DESC";//Using DESC so that the most recent ones appear in the list
			if (limit > 0)
			{
				command += " LIMIT " + limit;
			}

			static SecurityLog FromReaderWithPatientNameAndHash(MySqlDataReader dataReader)
            {
				var securityLog = FromReader(dataReader);
				securityLog.PatientName = (string)dataReader["patient_name"];
				securityLog.Hash = (string)dataReader["hash"];

				return securityLog;
			}

			var results = Database.SelectMany(command, FromReaderWithPatientNameAndHash,
				new MySqlParameter("date_from", dateFrom),
				new MySqlParameter("date_to", dateTo.AddDays(1)),
				new MySqlParameter("date_previous_from", datePreviousFrom),
				new MySqlParameter("date_previous_to", datePreviousTo.AddDays(1)));

			return results.ToArray();
		}

		public static long Insert(SecurityLog log) 
			=> ExecuteInsert(log);

		public static long InsertNoCache(SecurityLog securityLog)
			=> ExecuteInsert(securityLog);

		public static SecurityLog[] Refresh(long? patientId, List<Permissions> permissionTypes, long objectId) 
			=> Refresh(patientId, permissionTypes, new List<long>() { objectId });

		///<summary>Used when viewing various audit trails of specific types.  This overload will return security logs for multiple objects (or fKeys).
		///Typically you will only need a specific type audit log for one type.
		///However, for things like ortho charts, each row (FK) in the database represents just one part of a larger ortho chart "object".
		///Thus, to get the full experience of a specific type audit trail window, we need to get security logs for multiple objects (FKs) that
		///comprise the larger object (what the user sees).  Only implemented with ortho chart so far.  FKeys can be null.
		///Throws exceptions.</summary>
		public static SecurityLog[] Refresh(long? patientId, List<Permissions> permissionTypes, List<long> objectIds)
		{
			string types = "";
			for (int i = 0; i < permissionTypes.Count; i++)
			{
				if (i > 0)
				{
					types += " OR";
				}
				types += " `type` =" + (int)permissionTypes[i];
			}

			var command = "SELECT * FROM `security_logs` WHERE (" + types + ") ";

			if (objectIds != null && objectIds.Count > 0)
			{
				command += "AND `object_id` IN (" + string.Join(", ", objectIds) + ") ";
			}

			if (patientId.HasValue)
			{
				command += 
					" AND `patient_id` IN (" + string.Join(",", 
						PatientLinks.GetPatNumsLinkedToRecursive(patientId.Value, PatientLinkType.Merge)) + ")";
			}

			command += "ORDER BY `log_date`";

			return SelectMany(command).OrderBy(x => x.LogDate).ToArray();
		}

		public static List<SecurityLog> GetFromFKeysAndType(List<long> objectIds, List<Permissions> permissionTypes)
		{
			if (objectIds == null || objectIds.FindAll(x => x != 0).Count == 0)
			{
				return new List<SecurityLog>();
			}

			string command = 
				"SELECT * FROM `security_logs` " +
				"WHERE `object_id` IN (" + string.Join(", ", objectIds.FindAll(x => x != 0)) + ") " +
				"AND `type` IN (" + string.Join(", ", permissionTypes.Select(x => (int)x)) + ")";

			return SelectMany(command).ToList();
		}

		public static void MakeLogEntries(Permissions permType, long patientId, List<string> logMessages)
		{
			if (logMessages == null || logMessages.Count == 0)
			{
				return;
			}

			foreach (string securityLogEntry in logMessages)
			{
				MakeLogEntry(permType, patientId, securityLogEntry);
			}
		}

		public static void MakeLogEntry(Permissions permissionType, long? patientId, string logMessage, SecurityLogSource logSource = SecurityLogSource.None) 
			=> MakeLogEntry(permissionType, patientId, logMessage, null, logSource, null);

		public static void MakeLogEntry(Permissions permissionType, long? patientId, string logMessage, long? objectId, DateTime? objectDate, string deviceName = null) 
			=> MakeLogEntry(permissionType, patientId, logMessage, objectId, LogSource, objectDate, deviceName);

		public static void MakeLogEntry(Permissions permissionType, long? patientId, string logMessage, long? objectId, SecurityLogSource logSource, DateTime? objectDate, string deviceName = null)
		{
			SecurityLog securityLog = MakeLogEntryNoInsert(permissionType, patientId, logMessage, objectId, logSource, objectDate, deviceName);

			MakeLogEntry(securityLog);
		}

		public static void MakeLogEntry(SecurityLog securityLog)
		{
			securityLog.Id = Insert(securityLog);

			SecurityLogHashes.InsertSecurityLogHash(securityLog.Id);

			if (securityLog.Type == Permissions.AppointmentCreate && securityLog.ObjectId.HasValue)
			{
				EntryLogs.Insert(
					new EntryLog(securityLog.UserId, EntryLogFKeyType.Appointment, securityLog.ObjectId.Value, securityLog.LogSource));
			}
		}

		public static void MakeLogEntry(Permissions permissionType, List<long> patientIds, string logMessage)
		{
			var securityLogs = new List<SecurityLog>();

			foreach (long patientId in patientIds)
			{
				var securityLog = MakeLogEntryNoInsert(permissionType, patientId, logMessage, 0, LogSource);

                Insert(securityLog);

				securityLogs.Add(securityLog);
			}

			var securityLogHashses = new List<SecurityLogHash>();
			var entryLogs = new List<EntryLog>();

			securityLogs = GetMany(SQLWhere.CreateIn(nameof(SecurityLog.Id),
				securityLogs.Select(x => x.Id).ToList()));

			foreach (var log in securityLogs)
			{
                var securityLogHash = new SecurityLogHash
                {
                    SecurityLogId = log.Id,
                    Hash = SecurityLogHashes.GetHashString(log)
                };

                securityLogHashses.Add(securityLogHash);

				if (log.Type == Permissions.AppointmentCreate && log.ObjectId.HasValue)
				{
					entryLogs.Add(new EntryLog(log.UserId, EntryLogFKeyType.Appointment, log.ObjectId.Value, log.LogSource));
				}
			}

			EntryLogs.InsertMany(entryLogs);

			foreach (var securityLogHash in securityLogHashses)
            {
				SecurityLogHashes.Insert(securityLogHash);
            }
		}

		public static void MakeLogEntryNoCache(Permissions permissionType, long patientId, string logMessage) 
			=> MakeLogEntryNoCache(permissionType, patientId, logMessage, 0, LogSource);

		public static void MakeLogEntryNoCache(Permissions permissionType, long patientId, string logMessage, long userId, SecurityLogSource logSource)
		{
            var securityLog = new SecurityLog
            {
                Type = permissionType,
                UserId = userId,
                LogMessage = logMessage,
                MachineName = Environment.MachineName,
                PatientId = patientId,
                ObjectId = null,
                LogSource = logSource
            };

            securityLog.Id = InsertNoCache(securityLog);

			SecurityLogHashes.InsertSecurityLogHashNoCache(securityLog.Id);
		}

		/// <summary>
		/// Creates a new security log entry with the specified details.
		/// </summary>
		/// <param name="permissionType"></param>
		/// <param name="patientId"></param>
		/// <param name="logMessage"></param>
		/// <param name="objectId"></param>
		/// <param name="logSource"></param>
		/// <param name="objectDate"></param>
		/// <param name="deviceName"></param>
		/// <returns></returns>
		public static SecurityLog MakeLogEntryNoInsert(Permissions permissionType, long? patientId, string logMessage, long? objectId, SecurityLogSource logSource, DateTime? objectDate = null, string deviceName = null)
		{
			return new SecurityLog
			{
				Type = permissionType,
				UserId = Security.CurrentUser.Id,
				LogMessage = logMessage,
				LogSource = logSource,
				MachineName = deviceName ?? Environment.MachineName,
				PatientId = patientId,
				ObjectId = objectId,
				ObjectDate = objectDate
			};
		}

		/// <summary>
		/// Writes the specified entry to the log.
		/// </summary>
		/// <param name="permissionType"></param>
		/// <param name="logMessage"></param>
		public static void Write(Permissions permissionType, string logMessage)
			=> Write(permissionType, logMessage, null);

		/// <summary>
		/// Writes the specified entry to the log.
		/// </summary>
		/// <param name="permissionType"></param>
		/// <param name="logMessage"></param>
		/// <param name="patientId">The ID of the patient.</param>
		public static void Write(Permissions permissionType, string logMessage, long? patientId = null)
			=> MakeLogEntry(permissionType, patientId, logMessage);
	}
}
