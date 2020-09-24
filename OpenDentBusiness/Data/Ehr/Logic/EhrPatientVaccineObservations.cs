using Imedisoft.Data.Models;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class EhrPatientVaccineObservations
	{
		public static IEnumerable<EhrPatientVaccineObservation> GetByPatientVaccine(long ehrPatientVaccineId) 
			=> SelectMany(
				"SELECT * FROM `ehr_patient_vaccine_obs` " +
				"WHERE `ehr_patient_vaccine_id` = " + ehrPatientVaccineId + " " +
				"ORDER BY `group`");

		public static void Save(EhrPatientVaccineObservation ehrPatientVaccineObservation)
		{
			if (ehrPatientVaccineObservation.Id == 0) ExecuteInsert(ehrPatientVaccineObservation);
			else
			{
				ExecuteUpdate(ehrPatientVaccineObservation);
			}
		}

		public static void Delete(EhrPatientVaccineObservation ehrPatientVaccineObservation) 
			=> ExecuteDelete(ehrPatientVaccineObservation);
	}
}
