using System;
using System.Text;
using CodeBase;
using Imedisoft.Data;

namespace OpenDentBusiness
{
	public class ReplaceTags
	{
		///<summary>Replaces one individual tag. Case insensitive.</summary>
		public static void ReplaceOneTag(StringBuilder template, string tagToReplace, string replaceWith, bool isHtmlEmail)
		{
			if (isHtmlEmail)
			{
				//Because we are sending emails as HTML, we need to escape characters that can cause our email text to become invalid HTML.
				replaceWith = (replaceWith ?? "").Replace(">", "&>").Replace("<", "&<");
			}
			//Note: RegReplace is case insensitive by default.
			template.RegReplace("\\" + tagToReplace, replaceWith ?? "");
		}

		/// <summary>
		/// Replaces all user fields in the given message with the supplied userod's information.  Returns the resulting string.
		/// Only works if the current user has a linked provider or employee, otherwise the replacements will be blank.
		/// Replaces: [UserNameF], [UserNameL], [UserNameFL].
		/// </summary>
		public static string ReplaceUser(string message, Userod userod)
		{
			string retVal = message;

			string userNameF = "";
			string userNameL = "";

			if (userod.ProviderId.HasValue)
			{
				Provider prov = Providers.GetById(userod.ProviderId.Value);
				userNameF = prov.FirstName;
				userNameL = prov.LastName;
			}
			else if (userod.EmployeeId.HasValue)
			{
				Employee emp = Employees.GetEmp(userod.EmployeeId.Value);
				userNameF = emp.FirstName;
				userNameL = emp.LastName;
			}

			retVal = retVal.Replace("[UserNameF]", userNameF);
			retVal = retVal.Replace("[UserNameL]", userNameL);
			retVal = retVal.Replace("[UserNameFL]", Patients.GetNameFL(userNameL, userNameF, "", ""));
			return retVal;
		}

		/// <summary>
		/// Replaces all miscellaneous fields in the given message.
		/// Returns the resulting string. Replaces: [CurrentMonth]
		/// </summary>
		public static string ReplaceMisc(string message) => message.Replace("[CurrentMonth]", DateTime.Today.ToString("MMMM"));
	}
}
