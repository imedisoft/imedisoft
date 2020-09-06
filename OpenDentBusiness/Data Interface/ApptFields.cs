using Imedisoft.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace OpenDentBusiness
{
	public class ApptFields
	{
		public static ApptField GetOne(long apptFieldId) 
			=> Crud.ApptFieldCrud.SelectOne(apptFieldId);

		public static List<ApptField> GetForAppt(long apptId) 
			=> Crud.ApptFieldCrud.SelectMany("SELECT * FROM `appt_fields` WHERE `appt_id` = " + apptId);

		public static long Insert(ApptField apptField) 
			=> Crud.ApptFieldCrud.Insert(apptField);

		public static void Update(ApptField apptField) 
			=> Crud.ApptFieldCrud.Update(apptField);

		/// <summary>
		/// Deletes any pre-existing appt fields for the AptNum and FieldName combo and then inserts the apptField passed in.
		/// </summary>
		public static long Upsert(ApptField apptField)
		{
			// There could already be an appt field in the database due to concurrency. 
			// Delete all entries prior to inserting the new one.

			DeleteFieldForAppt(apptField.FieldName, apptField.AptNum); // Last in wins.

			return Insert(apptField);
		}

		public static void Delete(long apptFieldId) 
			=> Database.ExecuteNonQuery("DELETE FROM `appt_fields` WHERE `id` = " + apptFieldId);

		/// <summary>
		/// Deletes all fields for the appointment and field name passed in.
		/// </summary>
		public static void DeleteFieldForAppt(string fieldName, long apptId) 
			=> Database.ExecuteNonQuery("DELETE FROM `appt_fields` WHERE `appt_id` = " + apptId + " AND `name` = @name",
				new MySqlParameter("name", fieldName));
	}
}
