using Imedisoft.Data.Models;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class EhrPatientVaccines
	{
		public static IEnumerable<EhrPatientVaccine> GetByPatient(long patientId) 
			=> SelectMany("SELECT * FROM `ehr_patient_vaccines` WHERE `patient_id` = " + patientId + " ORDER BY `date_start`");

		public static void Save(EhrPatientVaccine patientVaccine)
		{
			if (patientVaccine.Id == 0) ExecuteInsert(patientVaccine);
            else
            {
				ExecuteUpdate(patientVaccine);
			}
		}

		public static void Delete(EhrPatientVaccine ehrPatientVaccine) 
			=> ExecuteDelete(ehrPatientVaccine);
	}
}
