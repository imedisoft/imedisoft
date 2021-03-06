using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EClipboardSheetDefs{
		//If this table type will exist as cached data, uncomment the Cache Pattern region below and edit.
		/*
		#region Cache Pattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add GetTableFromCache and FillCacheFromTable to the Cache.cs file with all the other Cache types.
		//Also, consider making an invalid type for this class in Cache.GetAllCachedInvalidTypes() if needed.

		private class EClipboardSheetDefCache : CacheListAbs<EClipboardSheetDef> {
			protected override List<EClipboardSheetDef> GetCacheFromDb() {
				string command="SELECT * FROM eclipboardsheetdef";
				return Crud.EClipboardSheetDefCrud.SelectMany(command);
			}
			protected override List<EClipboardSheetDef> TableToList(DataTable table) {
				return Crud.EClipboardSheetDefCrud.TableToList(table);
			}
			protected override EClipboardSheetDef Copy(EClipboardSheetDef eClipboardSheetDef) {
				return eClipboardSheetDef.Copy();
			}
			protected override DataTable ListToTable(List<EClipboardSheetDef> listEClipboardSheetDefs) {
				return Crud.EClipboardSheetDefCrud.ListToTable(listEClipboardSheetDefs,"EClipboardSheetDef");
			}
			protected override void FillCacheIfNeeded() {
				EClipboardSheetDefs.GetTableFromCache(false);
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static EClipboardSheetDefCache _eClipboardSheetDefCache=new EClipboardSheetDefCache();

		public static List<EClipboardSheetDef> GetDeepCopy(bool isShort=false) {
			return _eClipboardSheetDefCache.GetDeepCopy(isShort);
		}

		public static int GetCount(bool isShort=false) {
			return _eClipboardSheetDefCache.GetCount(isShort);
		}

		public static bool GetExists(Predicate<EClipboardSheetDef> match,bool isShort=false) {
			return _eClipboardSheetDefCache.GetExists(match,isShort);
		}

		public static int GetFindIndex(Predicate<EClipboardSheetDef> match,bool isShort=false) {
			return _eClipboardSheetDefCache.GetFindIndex(match,isShort);
		}

		public static EClipboardSheetDef GetFirst(bool isShort=false) {
			return _eClipboardSheetDefCache.GetFirst(isShort);
		}

		public static EClipboardSheetDef GetFirst(Func<EClipboardSheetDef,bool> match,bool isShort=false) {
			return _eClipboardSheetDefCache.GetFirst(match,isShort);
		}

		public static EClipboardSheetDef GetFirstOrDefault(Func<EClipboardSheetDef,bool> match,bool isShort=false) {
			return _eClipboardSheetDefCache.GetFirstOrDefault(match,isShort);
		}

		public static EClipboardSheetDef GetLast(bool isShort=false) {
			return _eClipboardSheetDefCache.GetLast(isShort);
		}

		public static EClipboardSheetDef GetLastOrDefault(Func<EClipboardSheetDef,bool> match,bool isShort=false) {
			return _eClipboardSheetDefCache.GetLastOrDefault(match,isShort);
		}

		public static List<EClipboardSheetDef> GetWhere(Predicate<EClipboardSheetDef> match,bool isShort=false) {
			return _eClipboardSheetDefCache.GetWhere(match,isShort);
		}

		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_eClipboardSheetDefCache.FillCacheFromTable(table);
		}

		///<summary>Returns the cache in the form of a DataTable. Always refreshes the ClientWeb's cache.</summary>
		///<param name="doRefreshCache">If true, will refresh the cache if RemotingRole is ClientDirect or ServerWeb.</param> 
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _eClipboardSheetDefCache.GetTableFromCache(doRefreshCache);
		}
		#endregion Cache Pattern
		*/
		
		#region Get Methods
		///<summary></summary>
		public static List<EClipboardSheetDef> Refresh(){
			
			string command="SELECT * FROM eclipboardsheetdef";
			return Crud.EClipboardSheetDefCrud.SelectMany(command);
		}
		
		///<summary>Gets one EClipboardSheetDef from the db.</summary>
		public static EClipboardSheetDef GetOne(long eClipboardSheetDefNum){
			
			return Crud.EClipboardSheetDefCrud.SelectOne(eClipboardSheetDefNum);
		}

		public static List<EClipboardSheetDef> GetForClinic(long clinicNum) {
			
			string command="SELECT * FROM eclipboardsheetdef WHERE ClinicNum="+POut.Long(clinicNum);
			return Crud.EClipboardSheetDefCrud.SelectMany(command);
		}

		public static bool IsSheetDefInUse(long sheetDefNum) {
			
			string command="SELECT COUNT(*) FROM eclipboardsheetdef WHERE SheetDefNum="+POut.Long(sheetDefNum);
			return PIn.Int(Database.ExecuteString(command))>0;
		}

		#endregion Get Methods
		#region Modification Methods
		#region Insert
		///<summary></summary>
		public static long Insert(EClipboardSheetDef eClipboardSheetDef){
			
			return Crud.EClipboardSheetDefCrud.Insert(eClipboardSheetDef);
		}
		#endregion Insert
		#region Update
		///<summary></summary>
		public static void Update(EClipboardSheetDef eClipboardSheetDef){
			
			Crud.EClipboardSheetDefCrud.Update(eClipboardSheetDef);
		}
		#endregion Update
		#region Delete
		///<summary></summary>
		public static void Delete(long eClipboardSheetDefNum) {
			
			Crud.EClipboardSheetDefCrud.Delete(eClipboardSheetDefNum);
		}
		#endregion Delete
		#endregion Modification Methods
		#region Misc Methods

		///<summary>Inserts, updates, or deletes db rows to match listNew.  No need to pass in userNum, it's set before remoting role check and passed to
		///the server if necessary.</summary>
		public static bool Sync(List<EClipboardSheetDef> listNew,List<EClipboardSheetDef> listOld) {
			
			return Crud.EClipboardSheetDefCrud.Sync(listNew,listOld);
		}
		
		#endregion Misc Methods
	}
}