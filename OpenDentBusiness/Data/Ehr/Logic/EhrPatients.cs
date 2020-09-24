using OpenDentBusiness;

namespace Imedisoft.Data
{
    public partial class EhrPatients
	{
		public static EhrPatient GetById(long patientId)
		{
			if (Database.ExecuteLong("SELECT COUNT(*) FROM `ehr_patients` WHERE `patient_id` = " + patientId) == 0)
			{
				Database.ExecuteNonQuery(
					"INSERT IGNORE INTO `ehr_patients` (`patient_id`) " +
					"VALUES (" + patientId + ")");
			}

			return SelectOne(patientId);
		}

		public static EhrPatient GetOne(long patientId)
			=> SelectOne(patientId);

		public static void Update(EhrPatient ehrPatient) 
			=> ExecuteUpdate(ehrPatient);
	}
}
