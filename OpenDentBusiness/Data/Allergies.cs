using Imedisoft.Data;
using Imedisoft.Data.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public partial class Allergies
	{ 
		public static List<Allergy> GetByPatient(long patientId) 
			=> SelectMany("SELECT * FROM `allergies` WHERE patient_id = " + patientId).ToList();

		/// <summary>
		/// Gets all allergies for patient whether active or not.
		/// </summary>
		public static List<Allergy> GetByPatient(long patientId, bool showInactive)
		{
			var command = "SELECT * FROM `allergies` WHERE `patient_id` = " + patientId;
			if (!showInactive)
			{
				command += " AND `status_is_active` <> 0";
			}

			return SelectMany(command).ToList();
		}

		public static IEnumerable<long> GetChangedSinceAllergyNums(DateTime changedSince) 
			=> Database.SelectMany(
				"SELECT id FROM allergies WHERE date_last_modified > " + POut.DateT(changedSince),
					Database.ToScalar<long>);

		/// <summary>
		/// Returns an array of all patient names who are using this allergy.
		/// </summary>
		public static IEnumerable<string> GetPatNamesForAllergy(long allergyDefId) 
			=> Database.SelectMany(
				"SELECT CONCAT(CONCAT(CONCAT(CONCAT(LName, ', '), FName), ' '), Preferred) FROM allergies, patient WHERE allergies.patient_id = patient.PatNum AND allergies.allergy_def_id = " + allergyDefId,
					Database.ToScalar<string>);

		/// <summary>
		/// Returns a list of PatNums that have an allergy from the PatNums that are passed in.
		/// </summary>
		public static List<long> GetPatientsWithAllergy(List<long> patientIds)
		{
			if (patientIds == null || patientIds.Count == 0)
			{
				return new List<long>();
			}

			return Database.SelectMany(
				"SELECT DISTINCT patient_id FROM allergies WHERE patient_id IN (" + string.Join(",", patientIds) + ") AND allergies.allergy_def_id != " + PrefC.GetLong(PrefName.AllergiesIndicateNone),
					dataReader => Convert.ToInt64(dataReader["id"])).ToList();
		}

		/// <summary>
		/// Changes the value of the DateTStamp column to the current time stamp for all allergies of a patient that are the status specified
		/// </summary>
		public static void ResetTimeStamps(long patientId, bool onlyActive)
		{
			var command = "UPDATE allergies SET date_last_modified = CURRENT_TIMESTAMP WHERE patient_id =" + patientId;
			if (onlyActive)
			{
				command += " AND status_is_active = <> 0";
			}

			Database.ExecuteNonQuery(command);
		}
	}
}
