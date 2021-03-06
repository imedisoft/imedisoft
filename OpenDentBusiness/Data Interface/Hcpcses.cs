using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Hcpcses{
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

		//If this table type will exist as cached data, uncomment the CachePattern region below.
		/*
		#region CachePattern

		private class HcpcsCache : CacheListAbs<Hcpcs> {
			protected override List<Hcpcs> GetCacheFromDb() {
				string command="SELECT * FROM Hcpcs ORDER BY ItemOrder";
				return Crud.HcpcsCrud.SelectMany(command);
			}
			protected override List<Hcpcs> TableToList(DataTable table) {
				return Crud.HcpcsCrud.TableToList(table);
			}
			protected override Hcpcs Copy(Hcpcs Hcpcs) {
				return Hcpcs.Clone();
			}
			protected override DataTable ListToTable(List<Hcpcs> listHcpcss) {
				return Crud.HcpcsCrud.ListToTable(listHcpcss,"Hcpcs");
			}
			protected override void FillCacheIfNeeded() {
				Hcpcss.GetTableFromCache(false);
			}
			protected override bool IsInListShort(Hcpcs Hcpcs) {
				return !Hcpcs.IsHidden;
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static HcpcsCache _HcpcsCache=new HcpcsCache();

		///<summary>A list of all Hcpcss. Returns a deep copy.</summary>
		public static List<Hcpcs> ListDeep {
			get {
				return _HcpcsCache.ListDeep;
			}
		}

		///<summary>A list of all visible Hcpcss. Returns a deep copy.</summary>
		public static List<Hcpcs> ListShortDeep {
			get {
				return _HcpcsCache.ListShortDeep;
			}
		}

		///<summary>A list of all Hcpcss. Returns a shallow copy.</summary>
		public static List<Hcpcs> ListShallow {
			get {
				return _HcpcsCache.ListShallow;
			}
		}

		///<summary>A list of all visible Hcpcss. Returns a shallow copy.</summary>
		public static List<Hcpcs> ListShort {
			get {
				return _HcpcsCache.ListShallowShort;
			}
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_HcpcsCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _HcpcsCache.GetTableFromCache(doRefreshCache);
		}

		#endregion
		*/

		///<summary></summary>
		public static long Insert(Hcpcs hcpcs){
			
			return Crud.HcpcsCrud.Insert(hcpcs);
		}

		///<summary></summary>
		public static void Update(Hcpcs hcpcs) {
			
			Crud.HcpcsCrud.Update(hcpcs);
		}

		public static List<Hcpcs> GetAll() {
			
			string command="SELECT * FROM hcpcs";
			return Crud.HcpcsCrud.SelectMany(command);
		}

		///<summary>Returns a list of just the codes for use in update or insert logic.</summary>
		public static List<string> GetAllCodes() {
			
			List<string> retVal=new List<string>();
			string command="SELECT HcpcsCode FROM hcpcs";
			DataTable table=Database.ExecuteDataTable(command);
			for(int i=0;i<table.Rows.Count;i++){
				retVal.Add(table.Rows[i][0].ToString());
			}
			return retVal;
		}

		///<summary>Returns the total count of HCPCS codes.  HCPCS codes cannot be hidden.</summary>
		public static long GetCodeCount() {
			
			string command="SELECT COUNT(*) FROM hcpcs";
			return PIn.Long(Database.ExecuteString(command));
		}

		///<summary>Returns the Hcpcs of the code passed in by looking in cache.  If code does not exist, returns null.</summary>
		public static Hcpcs GetByCode(string hcpcsCode) {
			
			string command="SELECT * FROM hcpcs WHERE HcpcsCode='"+POut.String(hcpcsCode)+"'";
			return Crud.HcpcsCrud.SelectOne(command);
		}

		///<summary>Directly from db.</summary>
		public static bool CodeExists(string hcpcsCode) {
			
			string command="SELECT COUNT(*) FROM hcpcs WHERE HcpcsCode='"+POut.String(hcpcsCode)+"'";
			string count=Database.ExecuteString(command);
			if(count=="0") {
				return false;
			}
			return true;
		}

		public static List<Hcpcs> GetBySearchText(string searchText) {
			
			string[] searchTokens=searchText.Split(' ');
			string command=@"SELECT * FROM hcpcs ";
			for(int i=0;i<searchTokens.Length;i++) {
				command+=(i==0?"WHERE ":"AND ")+"(HcpcsCode LIKE '%"+POut.String(searchTokens[i])+"%' OR DescriptionShort LIKE '%"+POut.String(searchTokens[i])+"%') ";
			}
			return Crud.HcpcsCrud.SelectMany(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<Hcpcs> Refresh(long patNum){
			
			string command="SELECT * FROM hcpcs WHERE PatNum = "+POut.Long(patNum);
			return Crud.HcpcsCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void Delete(long hcpcsNum) {
			
			string command= "DELETE FROM hcpcs WHERE HcpcsNum = "+POut.Long(hcpcsNum);
			Db.ExecuteNonQuery(command);
		}
		*/



	}
}