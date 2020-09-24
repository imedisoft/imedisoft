using Imedisoft.Data.Models;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class EhrSummaryCcds
	{
		public static IEnumerable<EhrSummaryCcd> Refresh(long patientId) 
			=> SelectMany(
				"SELECT * FROM `ehr_summary_ccds` " +
				"WHERE `patient_id` = " + patientId + " " +
				"ORDER BY `date`");

		public static EhrSummaryCcd GetOneForEmailAttach(long emailAttachmentId) 
			=> SelectOne(
				"SELECT * FROM `ehr_summary_ccds` " +
				"WHERE `email_attachment_id` = " + emailAttachmentId + " LIMIT 1");
		
		public static void Save(EhrSummaryCcd ehrSummaryCcd)
		{
			if (ehrSummaryCcd.Id == 0) ExecuteInsert(ehrSummaryCcd);
            else
            {
				ExecuteUpdate(ehrSummaryCcd);
            }
		}
	}
}
