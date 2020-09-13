using System;
using System.Collections.Generic;
using System.Data;

namespace OpenDentBusiness
{
	public class Ebills
	{
		#region CachePattern

		private class EbillCache : CacheListAbs<Ebill>
		{
			protected override List<Ebill> GetCacheFromDb()
			{
				string command = "SELECT * FROM ebill";
				return Crud.EbillCrud.SelectMany(command);
			}
			protected override List<Ebill> TableToList(DataTable table)
			{
				return Crud.EbillCrud.TableToList(table);
			}
			protected override Ebill Copy(Ebill Ebill)
			{
				return Ebill.Copy();
			}
			protected override DataTable ListToTable(List<Ebill> listEbills)
			{
				return Crud.EbillCrud.ListToTable(listEbills, "Ebill");
			}
			protected override void FillCacheIfNeeded()
			{
				Ebills.GetTableFromCache(false);
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static EbillCache _EbillCache = new EbillCache();

		public static List<Ebill> GetDeepCopy(bool isShort = false)
		{
			return _EbillCache.GetDeepCopy(isShort);
		}

		public static Ebill GetFirstOrDefault(Func<Ebill, bool> match, bool isShort = false)
		{
			return _EbillCache.GetFirstOrDefault(match, isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache()
		{
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table)
		{
			_EbillCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache)
		{

			return _EbillCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		public static Ebill GetForClinic(long clinicNum)
		{
			return GetFirstOrDefault(x => x.ClinicNum == clinicNum);
		}

		public static bool Sync(List<Ebill> listNew, List<Ebill> listOld)
		{
			return Crud.EbillCrud.Sync(listNew, listOld);
		}
	}
}
