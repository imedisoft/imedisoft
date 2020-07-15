using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
	public class ApptThankYouSents
	{
		public const string ADD_TO_CALENDAR = "[AddToCalendar]";

		public static List<ApptThankYouSent> GetForApt(long aptNum)
			=> Crud.ApptThankYouSentCrud.SelectMany(
				"SELECT * FROM apptthankyousent WHERE ApptNum=" + POut.Long(aptNum));

		public static void Update(ApptThankYouSent apptThankYouSent)
			=> Crud.ApptThankYouSentCrud.Update(apptThankYouSent);

		public static void Delete(params long[] arrApptThankYouSentNums)
		{
			if (arrApptThankYouSentNums.IsNullOrEmpty()) return;

			Db.NonQ(
				"DELETE FROM apptthankyousent " +
				"WHERE ApptThankYouSentNum IN (" + string.Join(",", arrApptThankYouSentNums) + ")");
		}
	}
}
