using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{
	public class LetterMerges
	{
		private class LetterMergeCache : CacheListAbs<LetterMerge>
		{
			protected override List<LetterMerge> GetCacheFromDb()
			{
				return Crud.LetterMergeCrud.SelectMany("SELECT * FROM lettermerge ORDER BY Description");
			}

			protected override List<LetterMerge> TableToList(DataTable table)
			{
				return Crud.LetterMergeCrud.TableToList(table);
			}

			protected override LetterMerge Copy(LetterMerge letterMerge)
			{
				return letterMerge.Copy();
			}

			protected override DataTable ListToTable(List<LetterMerge> listLetterMerges)
			{
				return Crud.LetterMergeCrud.ListToTable(listLetterMerges, "LetterMerge");
			}

			protected override void FillCacheIfNeeded()
			{
				LetterMerges.GetTableFromCache(false);
			}
		}

		private static readonly LetterMergeCache cache = new LetterMergeCache();

		public static List<LetterMerge> GetWhere(Predicate<LetterMerge> match, bool isShort = false)
		{
			return cache.GetWhere(match, isShort);
		}

		public static DataTable RefreshCache()
		{
			return GetTableFromCache(true);
		}

		public static DataTable GetTableFromCache(bool doRefreshCache)
		{
			return cache.GetTableFromCache(doRefreshCache);
		}

#if !DISABLE_MICROSOFT_OFFICE
		private static Word.Application wordApp;

		/// <summary>
		/// This is a static reference to a word application. 
		/// That way, we can reuse it instead of having to reopen Word each time.
		/// </summary>
		public static Word.Application WordApp
		{
			get
			{
				if (wordApp == null)
				{
                    wordApp = new Word.Application
                    {
                        Visible = true
                    };
                }
				try
				{
					wordApp.Activate();
				}
				catch
				{
					wordApp = new Word.Application();
					wordApp.Visible = true;
					wordApp.Activate();
				}
				return wordApp;
			}
		}
#endif

		public static long Insert(LetterMerge merge)
		{
			return Crud.LetterMergeCrud.Insert(merge);
		}

		public static void Update(LetterMerge merge)
		{

			Crud.LetterMergeCrud.Update(merge);
		}

		public static void Delete(LetterMerge merge)
		{
			Database.ExecuteNonQuery("DELETE FROM lettermerge WHERE LetterMergeNum = " + merge.LetterMergeNum);
		}

		/// <summary>
		/// Supply the index of the cat within Defs.Short.
		/// </summary>
		public static List<LetterMerge> GetListForCat(int catIndex)
		{
			long defNum = Definitions.GetDefsForCategory(DefinitionCategory.LetterMergeCats, true)[catIndex].Id;

			return GetWhere(x => x.Category == defNum);
		}
	}
}
