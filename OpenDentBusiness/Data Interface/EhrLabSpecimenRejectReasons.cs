using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EhrLabSpecimenRejectReasons {
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


		///<summary></summary>
		public static List<EhrLabSpecimenRejectReason> GetForEhrLabSpecimen(long ehrLabSpecimenNum) {
			
			string command="SELECT * FROM ehrlabspecimenrejectreason WHERE EhrLabSpecimenNum = "+POut.Long(ehrLabSpecimenNum);
			return Crud.EhrLabSpecimenRejectReasonCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void DeleteForLab(long ehrLabNum) {
			
			string command="DELETE FROM ehrlabspecimenrejectreason WHERE EhrLabSpecimenNum IN (SELECT EhrLabSpecimenNum FROM ehrlabspecimen WHERE EhrLabNum="+POut.Long(ehrLabNum)+")";
			Database.ExecuteNonQuery(command);
		}

		///<summary></summary>
		public static void DeleteForLabSpecimen(long ehrLabSpecimenNum) {
			
			string command="DELETE FROM ehrlabspecimenrejectreason WHERE EhrLabSpecimenNum="+POut.Long(ehrLabSpecimenNum);
			Database.ExecuteNonQuery(command);
		}

		///<summary></summary>
		public static long Insert(EhrLabSpecimenRejectReason ehrLabSpecimenRejectReason) {
			
			return Crud.EhrLabSpecimenRejectReasonCrud.Insert(ehrLabSpecimenRejectReason);
		}

		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern

		private class EhrLabSpecimenRejectReasonCache : CacheListAbs<EhrLabSpecimenRejectReason> {
			protected override List<EhrLabSpecimenRejectReason> GetCacheFromDb() {
				string command="SELECT * FROM EhrLabSpecimenRejectReason ORDER BY ItemOrder";
				return Crud.EhrLabSpecimenRejectReasonCrud.SelectMany(command);
			}
			protected override List<EhrLabSpecimenRejectReason> TableToList(DataTable table) {
				return Crud.EhrLabSpecimenRejectReasonCrud.TableToList(table);
			}
			protected override EhrLabSpecimenRejectReason Copy(EhrLabSpecimenRejectReason EhrLabSpecimenRejectReason) {
				return EhrLabSpecimenRejectReason.Clone();
			}
			protected override DataTable ListToTable(List<EhrLabSpecimenRejectReason> listEhrLabSpecimenRejectReasons) {
				return Crud.EhrLabSpecimenRejectReasonCrud.ListToTable(listEhrLabSpecimenRejectReasons,"EhrLabSpecimenRejectReason");
			}
			protected override void FillCacheIfNeeded() {
				EhrLabSpecimenRejectReasons.GetTableFromCache(false);
			}
			protected override bool IsInListShort(EhrLabSpecimenRejectReason EhrLabSpecimenRejectReason) {
				return !EhrLabSpecimenRejectReason.IsHidden;
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static EhrLabSpecimenRejectReasonCache _EhrLabSpecimenRejectReasonCache=new EhrLabSpecimenRejectReasonCache();

		///<summary>A list of all EhrLabSpecimenRejectReasons. Returns a deep copy.</summary>
		public static List<EhrLabSpecimenRejectReason> ListDeep {
			get {
				return _EhrLabSpecimenRejectReasonCache.ListDeep;
			}
		}

		///<summary>A list of all visible EhrLabSpecimenRejectReasons. Returns a deep copy.</summary>
		public static List<EhrLabSpecimenRejectReason> ListShortDeep {
			get {
				return _EhrLabSpecimenRejectReasonCache.ListShortDeep;
			}
		}

		///<summary>A list of all EhrLabSpecimenRejectReasons. Returns a shallow copy.</summary>
		public static List<EhrLabSpecimenRejectReason> ListShallow {
			get {
				return _EhrLabSpecimenRejectReasonCache.ListShallow;
			}
		}

		///<summary>A list of all visible EhrLabSpecimenRejectReasons. Returns a shallow copy.</summary>
		public static List<EhrLabSpecimenRejectReason> ListShort {
			get {
				return _EhrLabSpecimenRejectReasonCache.ListShallowShort;
			}
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_EhrLabSpecimenRejectReasonCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _EhrLabSpecimenRejectReasonCache.GetTableFromCache(doRefreshCache);
		}

		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<EhrLabSpecimenRejectReason> Refresh(long patNum){
			
			string command="SELECT * FROM ehrlabspecimenrejectreason WHERE PatNum = "+POut.Long(patNum);
			return Crud.EhrLabSpecimenRejectReasonCrud.SelectMany(command);
		}

		///<summary>Gets one EhrLabSpecimenRejectReason from the db.</summary>
		public static EhrLabSpecimenRejectReason GetOne(long ehrLabSpecimenRejectReasonNum){
			
			return Crud.EhrLabSpecimenRejectReasonCrud.SelectOne(ehrLabSpecimenRejectReasonNum);
		}

		///<summary></summary>
		public static long Insert(EhrLabSpecimenRejectReason ehrLabSpecimenRejectReason){
			
			return Crud.EhrLabSpecimenRejectReasonCrud.Insert(ehrLabSpecimenRejectReason);
		}

		///<summary></summary>
		public static void Update(EhrLabSpecimenRejectReason ehrLabSpecimenRejectReason){
			
			Crud.EhrLabSpecimenRejectReasonCrud.Update(ehrLabSpecimenRejectReason);
		}

		///<summary></summary>
		public static void Delete(long ehrLabSpecimenRejectReasonNum) {
			
			string command= "DELETE FROM ehrlabspecimenrejectreason WHERE EhrLabSpecimenRejectReasonNum = "+POut.Long(ehrLabSpecimenRejectReasonNum);
			Db.ExecuteNonQuery(command);
		}
		*/



	}
}