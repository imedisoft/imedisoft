using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Sops{
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

		#region CachePattern

		private class SopCache : CacheListAbs<Sop> {
			protected override List<Sop> GetCacheFromDb() {
				string command="SELECT * FROM sop";
				return Crud.SopCrud.SelectMany(command);
			}
			protected override List<Sop> TableToList(DataTable table) {
				return Crud.SopCrud.TableToList(table);
			}
			protected override Sop Copy(Sop sop) {
				return sop.Copy();
			}
			protected override DataTable ListToTable(List<Sop> listSops) {
				return Crud.SopCrud.ListToTable(listSops,"Sop");
			}
			protected override void FillCacheIfNeeded() {
				Sops.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static SopCache _sopCache=new SopCache();

		public static List<Sop> GetDeepCopy(bool isShort=false) {
			return _sopCache.GetDeepCopy(isShort);
		}

		public static Sop GetFirstOrDefault(Func<Sop,bool> match,bool isShort=false) {
			return _sopCache.GetFirstOrDefault(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_sopCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			return _sopCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		///<summary></summary>
		public static long Insert(Sop sop){
			
			return Crud.SopCrud.Insert(sop);
		}

		///<summary></summary>
		public static void Update(Sop sop) {
			
			Crud.SopCrud.Update(sop);
		}

		///<summary>Returns the count of all SOP codes.  SOP codes cannot be hidden.</summary>
		public static long GetCodeCount() {
			
			string command="SELECT COUNT(*) FROM sop";
			return PIn.Long(Db.GetCount(command));
		}

		///<summary>Returns the description for the specified SopCode.  Returns an empty string if no code is found.</summary>
		public static string GetDescriptionFromCode(string sopCode) {
			Sop sop=GetFirstOrDefault(x => x.SopCode==sopCode);
			return (sop==null ? "" : sop.Description);
		}
	}
}