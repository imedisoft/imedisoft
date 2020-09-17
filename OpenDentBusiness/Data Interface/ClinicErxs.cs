using System;
using System.Collections.Generic;
using System.Data;

namespace OpenDentBusiness
{
    public class ClinicErxs
	{
		#region Cache Pattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add GetTableFromCache and FillCacheFromTable to the Cache.cs file with all the other Cache types.
		//Also, consider making an invalid type for this class in Cache.GetAllCachedInvalidTypes() if needed.

		private class ClinicErxCache : CacheListAbs<ClinicErx>
		{
			protected override List<ClinicErx> GetCacheFromDb()
			{
				string command = "SELECT * FROM clinicerx";
				return Crud.ClinicErxCrud.SelectMany(command);
			}
			protected override List<ClinicErx> TableToList(DataTable table)
			{
				return Crud.ClinicErxCrud.TableToList(table);
			}
			protected override ClinicErx Copy(ClinicErx clinicErx)
			{
				return clinicErx.Copy();
			}
			protected override DataTable ListToTable(List<ClinicErx> listClinicErxs)
			{
				return Crud.ClinicErxCrud.ListToTable(listClinicErxs, "ClinicErx");
			}
			protected override void FillCacheIfNeeded()
			{
				ClinicErxs.GetTableFromCache(false);
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static ClinicErxCache _clinicErxCache = new ClinicErxCache();

		public static List<ClinicErx> GetDeepCopy(bool isShort = false)
		{
			return _clinicErxCache.GetDeepCopy(isShort);
		}

		public static int GetCount(bool isShort = false)
		{
			return _clinicErxCache.GetCount(isShort);
		}

		public static bool GetExists(Predicate<ClinicErx> match, bool isShort = false)
		{
			return _clinicErxCache.GetExists(match, isShort);
		}

		public static int GetFindIndex(Predicate<ClinicErx> match, bool isShort = false)
		{
			return _clinicErxCache.GetFindIndex(match, isShort);
		}

		public static ClinicErx GetFirst(bool isShort = false)
		{
			return _clinicErxCache.GetFirst(isShort);
		}

		public static ClinicErx GetFirst(Func<ClinicErx, bool> match, bool isShort = false)
		{
			return _clinicErxCache.GetFirst(match, isShort);
		}

		public static ClinicErx GetFirstOrDefault(Func<ClinicErx, bool> match, bool isShort = false)
		{
			return _clinicErxCache.GetFirstOrDefault(match, isShort);
		}

		public static ClinicErx GetLast(bool isShort = false)
		{
			return _clinicErxCache.GetLast(isShort);
		}

		public static ClinicErx GetLastOrDefault(Func<ClinicErx, bool> match, bool isShort = false)
		{
			return _clinicErxCache.GetLastOrDefault(match, isShort);
		}

		public static List<ClinicErx> GetWhere(Predicate<ClinicErx> match, bool isShort = false)
		{
			return _clinicErxCache.GetWhere(match, isShort);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table)
		{
			_clinicErxCache.FillCacheFromTable(table);
		}

		///<summary>Returns the cache in the form of a DataTable. Always refreshes the ClientWeb's cache.</summary>
		///<param name="doRefreshCache">If true, will refresh the cache if RemotingRole is ClientDirect or ServerWeb.</param> 
		public static DataTable GetTableFromCache(bool doRefreshCache)
		{

			return _clinicErxCache.GetTableFromCache(doRefreshCache);
		}
		#endregion Cache Pattern

		public static ClinicErx GetOne(long clinicErxNum)
		{
			return Crud.ClinicErxCrud.SelectOne("SELECT * FROM clinicerx WHERE ClinicErxNum = " + clinicErxNum);
		}

		public static List<ClinicErx> Refresh(long patNum)
		{
			return Crud.ClinicErxCrud.SelectMany("SELECT * FROM clinicerx WHERE PatNum = " + patNum);
		}

		public static ClinicErx GetByClinicNum(long clinicNum)
		{
			return GetFirstOrDefault(x => x.ClinicNum == clinicNum);
		}

		public static ClinicErx GetByClinicIdAndKey(string clinicId, string clinicKey)
		{
			return GetFirstOrDefault(x => x.ClinicId == clinicId && x.ClinicKey == clinicKey);
		}

		public static long Insert(ClinicErx clinicErx)
		{
			return Crud.ClinicErxCrud.Insert(clinicErx);
		}

		public static void Update(ClinicErx clinicErx)
		{
			Crud.ClinicErxCrud.Update(clinicErx);
		}

		public static bool Update(ClinicErx clinicErx, ClinicErx oldClinicErx)
		{
			return Crud.ClinicErxCrud.Update(clinicErx, oldClinicErx);
		}

		public static void Delete(long clinicErxNum)
		{
			Crud.ClinicErxCrud.Delete(clinicErxNum);
		}
	}
}
