using System.Text;
using CodeBase;

namespace OpenDentBusiness.AutoComm {
	public class PatientPortalInviteTagReplacer : ApptTagReplacer {		
			
		protected override void ReplaceTagsChild(StringBuilder sbTemplate,AutoCommObj autoCommObj,bool isEmail) {
			base.ReplaceTagsChild(sbTemplate,autoCommObj,isEmail);//To replace [ApptDate], etc.
			PatPortalInviteLite invite=(PatPortalInviteLite)autoCommObj;
			ReplaceOneTag(sbTemplate,"[UserName]",invite.UserWeb.Item1.UserName,isEmail);
			ReplaceOneTag(sbTemplate,"[Password]",invite.UserWeb.Item2,isEmail);
			ReplaceOneTag(sbTemplate,"[PatientPortalURL]",Prefs.GetString(PrefName.PatientPortalURL),isEmail);
		}
			
		protected override void ReplaceTagsAggregateChild(StringBuilder sbTemplate,StringBuilder sbAutoCommObjsAggregate) {
			sbTemplate.RegReplace("\\[Credentials]",sbAutoCommObjsAggregate.ToString());//We don't need to escape '<' here.
		}
	}
}
