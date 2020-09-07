using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CodeBase;

namespace OpenDentBusiness{
	public class EmailHostingTemplates{
		#region Get Methods

		///<summary>Returns an instance of the account api for the given Clinic Num.</summary>
		public static IAccountApi GetAccountApi(long clinicNum)
		{
			string guid = ClinicPrefs.GetString(clinicNum, PrefName.MassEmailGuid);
			string secret = ClinicPrefs.GetString(clinicNum, PrefName.MassEmailSecret);

			if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(secret))
			{
				//Huge assumption that we have already checked that the current clinic is signed up.
				guid = ClinicPrefs.GetString(Clinics.Active.Id, PrefName.MassEmailGuid);
				secret = ClinicPrefs.GetString(Clinics.Active.Id, PrefName.MassEmailSecret);
			}
			return new AccountApi(guid, secret);
		}

		public static List<EmailHostingTemplate> Refresh(){
			
			string command="SELECT * FROM emailhostingtemplate";
			return Crud.EmailHostingTemplateCrud.SelectMany(command);
		}

		///<summary>Gets one EmailHostingTemplate from the db.</summary>
		public static EmailHostingTemplate GetOne(long emailHostingTemplateNum){
			
			return Crud.EmailHostingTemplateCrud.SelectOne(emailHostingTemplateNum);
		}
		#endregion Get Methods

		#region Modification Methods
		///<summary></summary>
		public static long Insert(EmailHostingTemplate emailHostingTemplate){
			
			return Crud.EmailHostingTemplateCrud.Insert(emailHostingTemplate);
		}

		///<summary></summary>
		public static void Update(EmailHostingTemplate emailHostingTemplate){
			
			Crud.EmailHostingTemplateCrud.Update(emailHostingTemplate);
		}

		///<summary></summary>
		public static void Delete(long emailHostingTemplateNum) {
			
			Crud.EmailHostingTemplateCrud.Delete(emailHostingTemplateNum);
		}
		#endregion Modification Methods

		#region Misc Methods
		public static bool AreEqual(EmailHostingTemplate template1,EmailHostingTemplate template2) {
			if((template1==null && template2!=null) || (template2==null && template1!=null)) {
				return false;//most likely a new template being created. The original will be null and the copy will be a new template. 
			}
			if(template1.BodyHTML!=template2.BodyHTML) {
				return false;
			}
			if(template1.BodyPlainText!=template2.BodyPlainText) {
				return false;
			}
			if(template1.TemplateName!=template2.TemplateName) {
				return false;
			}
			if(template1.Subject!=template2.Subject) {
				return false;
			}
			return true;
		}

		public static bool IsBlank(EmailHostingTemplate template) {
			return string.IsNullOrEmpty(template.Subject) && string.IsNullOrEmpty(template.BodyHTML) && string.IsNullOrEmpty(template.BodyPlainText);
		}

		///<summary>Returns a list of the replacements in the given string. Will return the inner key without the outside brackets.</summary>
		public static List<string> GetListReplacements(string subjectOrBody) {
			if(string.IsNullOrWhiteSpace(subjectOrBody)) {
				return new List<string>();
			}
			List<string> retVal=new List<string>();
			foreach(Match match in Regex.Matches(subjectOrBody,@"\[{\[{\s?([A-Za-z0-9]*)\s?}\]}\]")) {
				retVal.Add(match.Groups[1].Value.Trim());
			}
			return retVal;
		}

		#endregion Misc Methods

		#region Cache Pattern
		//If this table type will exist as cached data, uncomment the Cache Pattern region below and edit.
		/*
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add GetTableFromCache and FillCacheFromTable to the Cache.cs file with all the other Cache types.
		//Also, consider making an invalid type for this class in Cache.GetAllCachedInvalidTypes() if needed.

		private class EmailHostingTemplateCache : CacheListAbs<EmailHostingTemplate> {
			protected override List<EmailHostingTemplate> GetCacheFromDb() {
				string command="SELECT * FROM emailhostingtemplate";
				return Crud.EmailHostingTemplateCrud.SelectMany(command);
			}
			protected override List<EmailHostingTemplate> TableToList(DataTable table) {
				return Crud.EmailHostingTemplateCrud.TableToList(table);
			}
			protected override EmailHostingTemplate Copy(EmailHostingTemplate emailHostingTemplate) {
				return emailHostingTemplate.Copy();
			}
			protected override DataTable ListToTable(List<EmailHostingTemplate> listEmailHostingTemplates) {
				return Crud.EmailHostingTemplateCrud.ListToTable(listEmailHostingTemplates,"EmailHostingTemplate");
			}
			protected override void FillCacheIfNeeded() {
				EmailHostingTemplates.GetTableFromCache(false);
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static EmailHostingTemplateCache _emailHostingTemplateCache=new EmailHostingTemplateCache();

		public static List<EmailHostingTemplate> GetDeepCopy(bool isShort=false) {
			return _emailHostingTemplateCache.GetDeepCopy(isShort);
		}

		public static int GetCount(bool isShort=false) {
			return _emailHostingTemplateCache.GetCount(isShort);
		}

		public static bool GetExists(Predicate<EmailHostingTemplate> match,bool isShort=false) {
			return _emailHostingTemplateCache.GetExists(match,isShort);
		}

		public static int GetFindIndex(Predicate<EmailHostingTemplate> match,bool isShort=false) {
			return _emailHostingTemplateCache.GetFindIndex(match,isShort);
		}

		public static EmailHostingTemplate GetFirst(bool isShort=false) {
			return _emailHostingTemplateCache.GetFirst(isShort);
		}

		public static EmailHostingTemplate GetFirst(Func<EmailHostingTemplate,bool> match,bool isShort=false) {
			return _emailHostingTemplateCache.GetFirst(match,isShort);
		}

		public static EmailHostingTemplate GetFirstOrDefault(Func<EmailHostingTemplate,bool> match,bool isShort=false) {
			return _emailHostingTemplateCache.GetFirstOrDefault(match,isShort);
		}

		public static EmailHostingTemplate GetLast(bool isShort=false) {
			return _emailHostingTemplateCache.GetLast(isShort);
		}

		public static EmailHostingTemplate GetLastOrDefault(Func<EmailHostingTemplate,bool> match,bool isShort=false) {
			return _emailHostingTemplateCache.GetLastOrDefault(match,isShort);
		}

		public static List<EmailHostingTemplate> GetWhere(Predicate<EmailHostingTemplate> match,bool isShort=false) {
			return _emailHostingTemplateCache.GetWhere(match,isShort);
		}

		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_emailHostingTemplateCache.FillCacheFromTable(table);
		}

		///<summary>Returns the cache in the form of a DataTable. Always refreshes the ClientWeb's cache.</summary>
		///<param name="doRefreshCache">If true, will refresh the cache if RemotingRole is ClientDirect or ServerWeb.</param> 
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _emailHostingTemplateCache.GetTableFromCache(doRefreshCache);
		}
		*/
		#endregion

	}
}