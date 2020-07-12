using CodeBase;
using DataConnectionBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace OpenDentBusiness
{
    /// <summary>
    /// Handles database commands for the language table in the database.
    /// </summary>
    public class Lans
	{
		/// <summary>
		/// Utilizes the NonPkAbs version of CacheDict because it uses a custom Key instead of the PK LanguageNum.
		/// </summary>
		private class LanguageCache : CacheDictNonPkAbs<Language, string, Language>
		{
			protected override List<Language> GetCacheFromDb() 
				=> Crud.LanguageCrud.SelectMany("SELECT * FROM language");

			protected override List<Language> TableToList(DataTable table) 
				=> Crud.LanguageCrud.TableToList(table);

			protected override Language Copy(Language language) 
				=> language.Copy();

			protected override DataTable DictToTable(Dictionary<string, Language> dictLanguages) 
				=> Crud.LanguageCrud.ListToTable(dictLanguages.Values.ToList(), "Language");

			protected override void FillCacheIfNeeded() 
				=> Lans.GetTableFromCache(false);

			protected override string GetDictKey(Language language) 
				=> language.ClassType + language.English;

			protected override Language GetDictValue(Language language) 
				=> language;

			protected override Language CopyDictValue(Language language) 
				=> language.Copy();

			protected override DataTable ListToTable(List<Language> listAllItems) 
				=> Crud.LanguageCrud.ListToTable(listAllItems);
		}

		/// <summary>
		/// The object that accesses the cache in a thread-safe manner.
		/// </summary>
		private static readonly LanguageCache cache = new LanguageCache();

		public static bool DictIsNull() => cache.DictIsNull();

		/// <summary>
		/// Does not do anything if the current region is US.
		/// Refreshes the cache and returns it as a DataTable.
		/// This will refresh the ClientWeb's cache and the ServerWeb's cache.
		/// </summary>
		public static DataTable RefreshCache() => 
			CultureInfo.CurrentCulture.Name == "en-US" ? null : GetTableFromCache(true);

		/// <summary>
		/// Fills the local cache with the passed in DataTable.
		/// </summary>
		public static void FillCacheFromTable(DataTable table)
		{
			if (CultureInfo.CurrentCulture.Name == "en-US")
			{
				return;
			}

			cache.FillCacheFromTable(table);
		}

		/// <summary>
		/// Always refreshes the ClientWeb's cache.
		/// </summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) 
			=> cache.GetTableFromCache(doRefreshCache);

		/// <summary>
		/// Converts a string to the current language.
		/// </summary>
		public static string g(string classType, string text) 
			=> ConvertString(classType, text);

		/// <summary>
		/// Converts a string to the current language.
		/// </summary>
		public static string g(object sender, string text) 
			=> ConvertString(sender.GetType().Name, text);

		/// <summary>
		/// This is where all the action happens.
		/// This method is used by all the others.
		/// This is always run on the client rather than the server, unless, of course, it's being called from the server.
		/// If it inserts an item into the db table, it will also add it to the local cache, but will not trigger a refresh on both ends.
		/// </summary>
		public static string ConvertString(string classType, string text)
		{
			if (classType == null || text == null) 
				return "";

			if (CultureInfo.CurrentCulture.Name == "en-US")
				return text;
			
			if (text.Trim() == "")
				return "";

			if (DictIsNull()) return text;

            var language = new Language
            {
                ClassType = classType,
                English = text
            };

            if (cache.AddValueForKey(classType + text, language))
			{
				Insert(language);
				return text;
			}

			if (LanguageForeigns.GetContainsKey(classType + text))
			{
				if (LanguageForeigns.GetOne(classType + text).Translation == "")
				{
					return text;
				}
				return LanguageForeigns.GetOne(classType + text).Translation;
			}
			else
			{
				return text;
			}
		}

		public static long Insert(Language language)
			=> Crud.LanguageCrud.Insert(language);

		/// <summary>
		/// No need to refresh after this.
		/// </summary>
		public static void DeleteItems(string classType, List<string> englishList)
		{
			string command = "DELETE FROM language WHERE ClassType='" + DataConnectionBase.SOut.String(classType) + "' AND (";

			for (int i = 0; i < englishList.Count; i++)
			{
				if (i > 0)
				{
					command += "OR ";
				}

				command += "English='" + SOut.String(englishList[i]) + "' ";

				cache.RemoveKey(classType + englishList[i]);
			}

			command += ")";

			Db.NonQ(command);
		}

		public static string[] GetListCat()
		{
			DataTable table = Db.GetTable("SELECT Distinct ClassType FROM language ORDER BY ClassType");

			string[] ListCat = new string[table.Rows.Count];
			for (int i = 0; i < table.Rows.Count; i++)
			{
				ListCat[i] = SIn.String(table.Rows[i][0].ToString());
			}

			return ListCat;
		}

		/// <summary>
		/// Only used in translation tool to get list for one category
		/// </summary>
		public static Language[] GetListForCat(string classType) =>
			Crud.LanguageCrud.SelectMany(
				"SELECT * FROM language WHERE ClassType = BINARY '" + SOut.String(classType) + "' ORDER BY English").ToArray();

		/// <summary>
		/// This had to be added because SilverLight does not allow globally setting the current culture format.
		/// </summary>
		public static string GetShortDateTimeFormat()
		{
			if (CultureInfo.CurrentCulture.Name == "en-US")
			{
				return "MM/dd/yyyy";
			}
			else
			{
				return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
			}
		}

		/// <summary>
		/// Gets a short time format for displaying in appt and schedule along the sides.
		/// Pass in a clone of the current culture; it will get altered.
		/// Returns a string format.
		/// </summary>
		public static string GetShortTimeFormat(CultureInfo ci)
		{
			string hFormat = "";
			ci.DateTimeFormat.AMDesignator = ci.DateTimeFormat.AMDesignator.ToLower();
			ci.DateTimeFormat.PMDesignator = ci.DateTimeFormat.PMDesignator.ToLower();
			string shortTimePattern = ci.DateTimeFormat.ShortTimePattern;
			if (shortTimePattern.IndexOf("hh") != -1)
			{//if hour is 01-12
				hFormat += "hh";
			}
			else if (shortTimePattern.IndexOf("h") != -1)
			{//or if hour is 1-12
				hFormat += "h";
			}
			else if (shortTimePattern.IndexOf("HH") != -1)
			{//or if hour is 00-23
				hFormat += "HH";
			}
			else
			{//hour is 0-23
				hFormat += "H";
			}
			if (shortTimePattern.IndexOf("t") != -1)
			{//if there is an am/pm designator
				hFormat += "tt";
			}
			else
			{//if no am/pm designator, then use :00
				hFormat += ":00";//time separator will actually change according to region
			}
			return hFormat;
		}

		/// <summary>
		/// This is one rare situation where queries can be passed.
		/// But it will always fail for client web and server web.
		/// </summary>
		public static void LoadTranslationsFromTextFile(string content) => Db.NonQ(content);
	}
}
