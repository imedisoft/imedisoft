using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class AppointmentFieldDefinitions
	{
		[CacheGroup(nameof(InvalidType.CustomFields))]
		private class AppointmentFieldDefinitionCache : ListCache<AppointmentFieldDefinition>
		{
			protected override IEnumerable<AppointmentFieldDefinition> Load()
				=> SelectMany("SELECT * FROM `appt_field_defs` ORDER BY `name`");
        }

		private static readonly AppointmentFieldDefinitionCache cache = new AppointmentFieldDefinitionCache();

		public static bool GetExists(Predicate<AppointmentFieldDefinition> predicate) 
			=> cache.Any(predicate);

		public static List<AppointmentFieldDefinition> All
			=> cache.GetAll();

		public static AppointmentFieldDefinition GetFirstOrDefault(Predicate<AppointmentFieldDefinition> predicate)
			=> cache.FirstOrDefault(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static void Update(AppointmentFieldDefinition apptFieldDefinition)
		{
			var count = Database.ExecuteLong(
				"SELECT COUNT(*) FROM `appt_field_defs` WHERE `name` = @name AND `id` != " + apptFieldDefinition.Id,
					new MySqlParameter("name", apptFieldDefinition.Name));

			if (count != 0)
			{
				throw new ApplicationException(
					Translation.Common.FieldNameAlreadyInUse);
			}

			ExecuteUpdate(apptFieldDefinition);
		}

		public static long Insert(AppointmentFieldDefinition apptFieldDefinition)
		{
			var count = Database.ExecuteLong(
				"SELECT COUNT(*) FROM `appt_field_defs` WHERE `name` = @name",
					new MySqlParameter("name", apptFieldDefinition.Name));

			if (count != 0)
			{
				throw new ApplicationException(
					Translation.Common.FieldNameAlreadyInUse);
			}

			return ExecuteInsert(apptFieldDefinition);
		}

		public static void Save(AppointmentFieldDefinition apptFieldDefinition)
        {
			if (apptFieldDefinition.Id == 0) Insert(apptFieldDefinition);
            else
            {
				Update(apptFieldDefinition);
            }
        }

		public static void Delete(AppointmentFieldDefinition apptFieldDefinition)
		{
			string command = "SELECT LName,FName,AptDateTime "
				+ "FROM patient,apptfield,appointment WHERE "
				+ "patient.PatNum=appointment.PatNum "
				+ "AND appointment.AptNum=apptfield.AptNum "
				+ "AND FieldName='" + POut.String(apptFieldDefinition.Name) + "'";

			DataTable table = Database.ExecuteDataTable(command);

			DateTime aptDateTime;
			if (table.Rows.Count > 0)
			{
				string s = "Not allowed to delete. Already in use by " + table.Rows.Count.ToString() + " appointments, including\r\n";

				for (int i = 0; i < table.Rows.Count; i++)
				{
					if (i > 5)
					{
						break;
					}

					aptDateTime = PIn.Date(table.Rows[i]["AptDateTime"].ToString());

					s += table.Rows[i]["LName"].ToString() + ", " + table.Rows[i]["FName"].ToString() + POut.DateT(aptDateTime, false) + "\r\n";
				}

				throw new ApplicationException(s);
			}

			ExecuteDelete(apptFieldDefinition);
		}

		public static string GetFieldName(long apptFieldDefinitionId) 
			=> GetFirstOrDefault(x => x.Id == apptFieldDefinitionId)?.Name ?? "";

		public static string GetPickListByFieldName(string apptFieldName) 
			=> GetFirstOrDefault(x => x.Name == apptFieldName)?.PickList ?? "";

		public static AppointmentFieldDefinition GetFieldDefByFieldName(string apptFieldName) 
			=> GetFirstOrDefault(x => x.Name == apptFieldName);
	}
}
