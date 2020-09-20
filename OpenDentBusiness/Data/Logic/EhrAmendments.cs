using Imedisoft.Data.Models;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class EhrAmendments
	{
		public static IEnumerable<EhrAmendment> Refresh(long patientId) 
			=> SelectMany("SELECT * FROM `ehr_amendments` WHERE `patient_id` = " + patientId + " ORDER BY `requested_on`");

		public static EhrAmendment GetOne(long ehrAmendmentNum) 
			=> SelectOne(ehrAmendmentNum);

		public static long Insert(EhrAmendment ehrAmendment) 
			=> ExecuteInsert(ehrAmendment);

		public static void Update(EhrAmendment ehrAmendment) 
			=> ExecuteUpdate(ehrAmendment);

		public static void Delete(EhrAmendment ehrAmendment) 
			=> ExecuteDelete(ehrAmendment);

		public static IEnumerable<DataItem<EhrAmendmentSource>> GetSources()
		{
			yield return new DataItem<EhrAmendmentSource>(EhrAmendmentSource.Patient, Translation.Common.Patient);
			yield return new DataItem<EhrAmendmentSource>(EhrAmendmentSource.Provider, Translation.Common.Provider);
			yield return new DataItem<EhrAmendmentSource>(EhrAmendmentSource.Organization, Translation.Common.Organization);
			yield return new DataItem<EhrAmendmentSource>(EhrAmendmentSource.Other, Translation.Common.Other);
		}

		public static string GetSourceDescription(EhrAmendmentSource source)
        {
			return source switch
            {
                EhrAmendmentSource.Patient => Translation.Common.Patient,
                EhrAmendmentSource.Provider => Translation.Common.Provider,
                EhrAmendmentSource.Organization => Translation.Common.Organization,
                EhrAmendmentSource.Other => Translation.Common.Other,
				_ => source.ToString()
            };
        }
	}
}
