using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ToothGridDefs{
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		/*
		#region CachePattern

		private class ToothGridDefCache : CacheListAbs<ToothGridDef> {
			protected override List<ToothGridDef> GetCacheFromDb() {
				string command="SELECT * FROM ToothGridDef ORDER BY ItemOrder";
				return Crud.ToothGridDefCrud.SelectMany(command);
			}
			protected override List<ToothGridDef> TableToList(DataTable table) {
				return Crud.ToothGridDefCrud.TableToList(table);
			}
			protected override ToothGridDef Copy(ToothGridDef ToothGridDef) {
				return ToothGridDef.Clone();
			}
			protected override DataTable ListToTable(List<ToothGridDef> listToothGridDefs) {
				return Crud.ToothGridDefCrud.ListToTable(listToothGridDefs,"ToothGridDef");
			}
			protected override void FillCacheIfNeeded() {
				ToothGridDefs.GetTableFromCache(false);
			}
			protected override bool IsInListShort(ToothGridDef ToothGridDef) {
				return !ToothGridDef.IsHidden;
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static ToothGridDefCache _ToothGridDefCache=new ToothGridDefCache();

		///<summary>A list of all ToothGridDefs. Returns a deep copy.</summary>
		public static List<ToothGridDef> ListDeep {
			get {
				return _ToothGridDefCache.ListDeep;
			}
		}

		///<summary>A list of all visible ToothGridDefs. Returns a deep copy.</summary>
		public static List<ToothGridDef> ListShortDeep {
			get {
				return _ToothGridDefCache.ListShortDeep;
			}
		}

		///<summary>A list of all ToothGridDefs. Returns a shallow copy.</summary>
		public static List<ToothGridDef> ListShallow {
			get {
				return _ToothGridDefCache.ListShallow;
			}
		}

		///<summary>A list of all visible ToothGridDefs. Returns a shallow copy.</summary>
		public static List<ToothGridDef> ListShort {
			get {
				return _ToothGridDefCache.ListShallowShort;
			}
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_ToothGridDefCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _ToothGridDefCache.GetTableFromCache(doRefreshCache);
		}

		#endregion*/

		public static List<ToothGridDef> Refresh(long patNum) {
			
			string command="SELECT * FROM toothgriddef WHERE toothgriddefnum = "+POut.Long(patNum);
			return Crud.ToothGridDefCrud.SelectMany(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<ToothGridDef> Refresh(long patNum){
			
			string command="SELECT * FROM toothgriddef WHERE PatNum = "+POut.Long(patNum);
			return Crud.ToothGridDefCrud.SelectMany(command);
		}

		///<summary>Gets one ToothGridDef from the db.</summary>
		public static ToothGridDef GetOne(long toothGridDefNum){
			
			return Crud.ToothGridDefCrud.SelectOne(toothGridDefNum);
		}

		///<summary></summary>
		public static long Insert(ToothGridDef toothGridDef){
			
			return Crud.ToothGridDefCrud.Insert(toothGridDef);
		}

		///<summary></summary>
		public static void Update(ToothGridDef toothGridDef){
			
			Crud.ToothGridDefCrud.Update(toothGridDef);
		}

		///<summary></summary>
		public static void Delete(long toothGridDefNum) {
			
			string command= "DELETE FROM toothgriddef WHERE ToothGridDefNum = "+POut.Long(toothGridDefNum);
			Db.NonQ(command);
		}
		*/



	}
}