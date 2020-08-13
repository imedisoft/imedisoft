using System.Text;
using CodeBase;

namespace OpenDentBusiness.AutoComm
{
	public class ApptTagReplacer : TagReplacer
	{
		protected override void ReplaceTagsChild(StringBuilder stringBuilder, AutoCommObj autoCommObj, bool isEmail)
		{
            if (autoCommObj is ApptLite appt)
            {
                ReplaceOneTag(stringBuilder, "[ApptTime]", appt.AptDateTime.ToShortTimeString(), isEmail);
                ReplaceOneTag(stringBuilder, "[ApptDate]", appt.AptDateTime.ToString(PrefC.PatientCommunicationDateFormat), isEmail);
                ReplaceOneTag(stringBuilder, "[ApptTimeAskedArrive]", appt.DateTimeAskedToArrive.ToShortTimeString(), isEmail);
            }
        }

		protected override void ReplaceTagsAggregateChild(StringBuilder sbTemplate, StringBuilder sbAutoCommObjsAggregate)
		{
			sbTemplate.RegReplace("\\[Appts]", sbAutoCommObjsAggregate.ToString());
		}
	}
}
