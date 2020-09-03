using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{
	public class ZipCodes
	{
		#region CachePattern

		private class ZipCodeCache : CacheListAbs<ZipCode>
		{
			protected override List<ZipCode> GetCacheFromDb()
			{
				return Crud.ZipCodeCrud.SelectMany("SELECT * from zipcode ORDER BY ZipCodeDigits");
			}

			protected override List<ZipCode> TableToList(DataTable table)
			{
				return Crud.ZipCodeCrud.TableToList(table);
			}

			protected override ZipCode Copy(ZipCode zipCode)
			{
				return zipCode.Copy();
			}

			protected override DataTable ListToTable(List<ZipCode> listZipCodes)
			{
				return Crud.ZipCodeCrud.ListToTable(listZipCodes, "ZipCode");
			}

			protected override void FillCacheIfNeeded()
			{
				ZipCodes.GetTableFromCache(false);
			}

			/// <summary>The zipcode "Short" list is for zipcodes marked frequent, not hidden.</summary>
			protected override bool IsInListShort(ZipCode zipCode)
			{
				return zipCode.IsFrequent;
			}
		}

		private static readonly ZipCodeCache cache = new ZipCodeCache();

		public static List<ZipCode> GetWhere(Predicate<ZipCode> match, bool isShort = false)
		{
			return cache.GetWhere(match, isShort);
		}

		public static List<ZipCode> GetDeepCopy(bool isShort = false)
		{
			return cache.GetDeepCopy(isShort);
		}

		/// <summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache()
		{
			return GetTableFromCache(true);
		}

		public static DataTable GetTableFromCache(bool doRefreshCache)
		{
			return cache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		public static long Insert(ZipCode zipCode) 
			=> Crud.ZipCodeCrud.Insert(zipCode);

		public static void Update(ZipCode zipCode) 
			=> Crud.ZipCodeCrud.Update(zipCode);

		public static void Save(ZipCode zipCode)
		{
			if (zipCode.ZipCodeNum == 0) Insert(zipCode);
			else
			{
				Update(zipCode);
			}
		}

		public static void Delete(ZipCode zipCode) 
			=> Database.ExecuteNonQuery("DELETE from zipcode WHERE zipcodenum = " + zipCode.ZipCodeNum);

		public static List<ZipCode> GetByZipCodeDigits(string zipCodeDigits) 
			=> GetWhere(x => x.ZipCodeDigits == zipCodeDigits);
	}
}
