using Imedisoft.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OpenDentBusiness
{
	public class EmailHostingTemplates
	{
		public static List<EmailHostingTemplate> Refresh()
		{
			return Crud.EmailHostingTemplateCrud.SelectMany("SELECT * FROM emailhostingtemplate");
		}

		public static EmailHostingTemplate GetOne(long emailHostingTemplateNum)
		{
			return Crud.EmailHostingTemplateCrud.SelectOne(emailHostingTemplateNum);
		}

		public static long Insert(EmailHostingTemplate emailHostingTemplate)
		{
			return Crud.EmailHostingTemplateCrud.Insert(emailHostingTemplate);
		}

		public static void Update(EmailHostingTemplate emailHostingTemplate)
		{
			Crud.EmailHostingTemplateCrud.Update(emailHostingTemplate);
		}

		public static void Delete(long emailHostingTemplateNum)
		{
			Crud.EmailHostingTemplateCrud.Delete(emailHostingTemplateNum);
		}

		public static bool AreEqual(EmailHostingTemplate template1, EmailHostingTemplate template2)
		{
			if ((template1 == null && template2 != null) || (template2 == null && template1 != null))
			{
				return false;//most likely a new template being created. The original will be null and the copy will be a new template. 
			}
			if (template1.BodyHTML != template2.BodyHTML)
			{
				return false;
			}
			if (template1.BodyPlainText != template2.BodyPlainText)
			{
				return false;
			}
			if (template1.TemplateName != template2.TemplateName)
			{
				return false;
			}
			if (template1.Subject != template2.Subject)
			{
				return false;
			}
			return true;
		}

		public static bool IsBlank(EmailHostingTemplate template)
		{
			return string.IsNullOrEmpty(template.Subject) && string.IsNullOrEmpty(template.BodyHTML) && string.IsNullOrEmpty(template.BodyPlainText);
		}

		///<summary>Returns a list of the replacements in the given string. Will return the inner key without the outside brackets.</summary>
		public static List<string> GetListReplacements(string subjectOrBody)
		{
			if (string.IsNullOrWhiteSpace(subjectOrBody))
			{
				return new List<string>();
			}
			List<string> retVal = new List<string>();
			foreach (Match match in Regex.Matches(subjectOrBody, @"\[{\[{\s?([A-Za-z0-9]*)\s?}\]}\]"))
			{
				retVal.Add(match.Groups[1].Value.Trim());
			}
			return retVal;
		}
	}
}
