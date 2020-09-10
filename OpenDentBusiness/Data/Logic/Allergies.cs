using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class Allergies
	{ 
		/// <summary>
		/// Gets all allergies for the patient with the specified ID, including inactive allergies.
		/// </summary>
		/// <param name="patientId">The ID of the patient.</param>
		/// <returns></returns>
		public static IEnumerable<Allergy> GetByPatient(long patientId) 
			=> SelectMany("SELECT * FROM `allergies` WHERE `patient_id` = " + patientId);

		/// <summary>
		/// Gets all allergies for the patient with the specified ID.
		/// </summary>
		/// <param name="patientId">The ID of the patient.</param>
		/// <param name="includeInactive"></param>
		/// <returns></returns>
		public static IEnumerable<Allergy> GetByPatient(long patientId, bool includeInactive) 
			=> includeInactive ?
				SelectMany("SELECT * FROM `allergies` WHERE `patient_id` = " + patientId) :
				SelectMany("SELECT * FROM `allergies` WHERE `patient_id` = " + patientId + " AND `is_active` = 1");

		public static void Save(Allergy allergy)
        {
			if (allergy.Id == 0)
			{
				ExecuteInsert(allergy);

				SecurityLogs.MakeLogEntry(
					Permissions.PatAllergyListEdit, allergy.PatientId, 
					AllergyDefs.GetDescription(allergy.AllergyDefId) + " added");
			}
			else
			{
				ExecuteUpdate(allergy);

				SecurityLogs.MakeLogEntry(
					Permissions.PatAllergyListEdit, allergy.PatientId, 
					AllergyDefs.GetDescription(allergy.AllergyDefId) + " edited");
			}
        }

		public static void Delete(Allergy allergy)
		{
			if (allergy.Id == 0) return;

			ExecuteDelete(allergy);

			SecurityLogs.MakeLogEntry(
				Permissions.PatAllergyListEdit, allergy.PatientId,
				AllergyDefs.GetDescription(allergy.AllergyDefId) + " deleted");
		}

		/// <summary>
		/// Returns an array of all patient names who are using this allergy.
		/// </summary>
		public static IEnumerable<string> GetPatNamesForAllergy(long allergyDefId) 
			=> Database.SelectMany(
				"SELECT CONCAT(LName, ', ', FName, ' ', Preferred) FROM `allergies` a, `patient` pat " +
				"WHERE a.`patient_id` = pat.`PatNum` AND a.`allergy_def_id` = " + allergyDefId,
					Database.ToScalar<string>);

		/// <summary>
		/// Returns a list of PatNums that have an allergy from the PatNums that are passed in.
		/// </summary>
		public static IEnumerable<long> GetPatientsWithAllergy(List<long> patientIds)
		{
			if (patientIds == null || patientIds.Count == 0)
			{
				return new List<long>();
			}

			return Database.SelectMany(
				"SELECT DISTINCT `patient_id` FROM `allergies` " +
				"WHERE `patient_id` IN (" + string.Join(", ", patientIds) + ") " +
				"AND `allergies`.`allergy_def_id` != " + Preferences.GetLong(PreferenceName.AllergiesIndicateNone),
					Database.ToScalar<long>);
		}

		/// <summary>
		/// Changes the value of the DateTStamp column to the current time stamp for all allergies of a patient that are the status specified
		/// </summary>
		public static void ResetTimeStamps(long patientId, bool activeOnly) 
			=> Database.ExecuteNonQuery(activeOnly ?
				"UPDATE `allergies` SET `last_modified_date` = current_timestamp() WHERE `patient_id` =" + patientId + " AND `is_active` = 1" :
				"UPDATE `allergies` SET `last_modified_date` = current_timestamp() WHERE `patient_id` =" + patientId);
	}
}
