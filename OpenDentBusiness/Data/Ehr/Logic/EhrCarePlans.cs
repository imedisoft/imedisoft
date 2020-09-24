using Imedisoft.Data.Models;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class EhrCarePlans
	{
		public static IEnumerable<EhrCarePlan> Refresh(long patientId) 
			=> SelectMany(
				"SELECT * FROM `ehr_care_plans` " +
				"WHERE `patient_id` = " + patientId + " " +
				"ORDER BY `date_planned`");

		public static void Save(EhrCarePlan ehrCarePlan)
		{
			if (ehrCarePlan.Id == 0) ExecuteInsert(ehrCarePlan);
            else
            {
				ExecuteUpdate(ehrCarePlan);
            }
		}

		public static void Delete(EhrCarePlan ehrCarePlan) 
			=> ExecuteDelete(ehrCarePlan.Id);
	}
}
