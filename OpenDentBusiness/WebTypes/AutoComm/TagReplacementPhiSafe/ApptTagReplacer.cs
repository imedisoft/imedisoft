using System.Text;
using CodeBase;

namespace OpenDentBusiness.AutoComm
{
	public class ApptTagReplacer : TagReplacer
	{
		protected override void ReplaceTagsChild(StringBuilder stringBuilder, AutoCommObj autoCommObj, bool isEmail)
		{
			ApptLite appt = autoCommObj as ApptLite;
			if (appt != null)
			{
				ReplaceOneTag(stringBuilder, "[ApptTime]", appt.AptDateTime.ToShortTimeString(), isEmail);
				ReplaceOneTag(stringBuilder, "[ApptDate]", appt.AptDateTime.ToString(PrefC.PatientCommunicationDateFormat), isEmail);
				ReplaceOneTag(stringBuilder, "[ApptTimeAskedArrive]", appt.DateTimeAskedToArrive.ToShortTimeString(), isEmail);
			}
		}

		///<summary>Replaces appointment related tags.</summary>
		protected override void ReplaceTagsAggregateChild(StringBuilder sbTemplate, StringBuilder sbAutoCommObjsAggregate)
		{
			sbTemplate.RegReplace("\\[Appts]", sbAutoCommObjsAggregate.ToString());//We don't need to escape '<' here.
		}
	}
}
