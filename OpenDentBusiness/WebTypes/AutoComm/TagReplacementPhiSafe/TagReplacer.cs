using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness.AutoComm
{
	public class TagReplacer
	{
		/// <summary>
		/// Replaces any tags in the AutoCommObj.
		/// </summary>
		protected virtual void ReplaceTagsChild(StringBuilder retVal, AutoCommObj autoCommObj, bool isEmail) { }
		
		/// <summary>
		/// Replaces any aggregate tags.
		/// </summary>
		protected virtual void ReplaceTagsAggregateChild(StringBuilder sbTemplate, StringBuilder sbAutoCommObjsAggregate) { }
		
		/// <summary>
		/// Replaces one individual tag. Case insensitive.
		/// </summary>
		protected void ReplaceOneTag(StringBuilder template, string tagToReplace, string replaceValue, bool isEmailBody)
		{
			OpenDentBusiness.ReplaceTags.ReplaceOneTag(template, tagToReplace, replaceValue, isEmailBody);
		}

		/// <summary>
		/// Replaces all tags with appropriate values if possible.
		/// </summary>
		public string ReplaceTags(string template, AutoCommObj autoCommObj, Clinic clinic, bool isEmailBody)
		{
			if (string.IsNullOrEmpty(template))
			{
				return template;
			}

			var stringBuilder = new StringBuilder();

			stringBuilder.Append(template);
			ReplaceOneTag(stringBuilder, "[Namef]", autoCommObj.NameF, isEmailBody);
			ReplaceOneTag(stringBuilder, "[ClinicName]", clinic.Description, isEmailBody);
			ReplaceOneTag(stringBuilder, "[ClinicPhone]", TelephoneNumbers.ReFormat(clinic.Phone.ToString()), isEmailBody);
			ReplaceOneTag(stringBuilder, "[OfficePhone]", TelephoneNumbers.ReFormat(Clinics.GetOfficePhone(clinic)), isEmailBody);
			ReplaceOneTag(stringBuilder, "[OfficeName]", Clinics.GetOfficeName(clinic), isEmailBody);
			ReplaceOneTag(stringBuilder, "[PracticeName]", Prefs.GetString(PrefName.PracticeTitle), isEmailBody);
			ReplaceOneTag(stringBuilder, "[PracticePhone]", TelephoneNumbers.ReFormat(Prefs.GetString(PrefName.PracticePhone)), isEmailBody);
			ReplaceOneTag(stringBuilder, "[ProvName]", Providers.GetFormalName(autoCommObj.ProvNum), isEmailBody);
			ReplaceOneTag(stringBuilder, "[ProvAbbr]", Providers.GetAbbr(autoCommObj.ProvNum), isEmailBody);
			ReplaceOneTag(stringBuilder, "[EmailDisclaimer]", EmailMessages.GetEmailDisclaimer(clinic.Id), isEmailBody);
			ReplaceTagsChild(stringBuilder, autoCommObj, isEmailBody);

			return stringBuilder.ToString();
		}
	}
}
