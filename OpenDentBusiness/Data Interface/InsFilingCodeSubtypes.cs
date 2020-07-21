using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{
    public class InsFilingCodeSubtypes
	{
		private class InsFilingCodeSubtypeCache : CacheListAbs<InsFilingCodeSubtype>
		{
			protected override List<InsFilingCodeSubtype> GetCacheFromDb() 
				=> Crud.InsFilingCodeSubtypeCrud.SelectMany("SELECT * FROM insfilingcodesubtype ORDER BY Descript");

			protected override List<InsFilingCodeSubtype> TableToList(DataTable table) 
				=> Crud.InsFilingCodeSubtypeCrud.TableToList(table);

			protected override InsFilingCodeSubtype Copy(InsFilingCodeSubtype InsFilingCodeSubtype) 
				=> InsFilingCodeSubtype.Clone();

			protected override DataTable ListToTable(List<InsFilingCodeSubtype> listInsFilingCodeSubtypes) 
				=> Crud.InsFilingCodeSubtypeCrud.ListToTable(listInsFilingCodeSubtypes, "InsFilingCodeSubtype");

			protected override void FillCacheIfNeeded() 
				=> InsFilingCodeSubtypes.GetTableFromCache(false);
		}

		private static readonly InsFilingCodeSubtypeCache cache = new InsFilingCodeSubtypeCache();

		public static List<InsFilingCodeSubtype> GetWhere(Predicate<InsFilingCodeSubtype> match, bool isShort = false) 
			=> cache.GetWhere(match, isShort);

		public static DataTable RefreshCache() 
			=> GetTableFromCache(true);

		public static DataTable GetTableFromCache(bool doRefreshCache) 
			=> cache.GetTableFromCache(doRefreshCache);

		public static long Insert(InsFilingCodeSubtype insFilingCodeSubtype) 
			=> Crud.InsFilingCodeSubtypeCrud.Insert(insFilingCodeSubtype);

		public static void Update(InsFilingCodeSubtype insFilingCodeSubtype) 
			=> Crud.InsFilingCodeSubtypeCrud.Update(insFilingCodeSubtype);

		public static void Delete(long insFilingCodeSubtypeNum)
		{
			string command = "SELECT COUNT(*) FROM insplan WHERE FilingCodeSubtype=" + insFilingCodeSubtypeNum;
			if (Database.ExecuteLong(command) != 0)
			{
				throw new ApplicationException("Already in use by insplans.");
			}

			Crud.InsFilingCodeSubtypeCrud.Delete(insFilingCodeSubtypeNum);
		}

		public static List<InsFilingCodeSubtype> GetForInsFilingCode(long insFilingCodeNum) 
			=> GetWhere(x => x.InsFilingCodeNum == insFilingCodeNum);

		public static void DeleteForInsFilingCode(long insFilingCodeNum) 
			=> Database.ExecuteNonQuery("DELETE FROM insfilingcodesubtype WHERE InsFilingCodeNum=" + insFilingCodeNum);
	}
}
