using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;

namespace OpenDentBusiness {
	///<summary></summary>
	public class MountDefs {
		#region CachePattern

		private class MountDefCache : CacheListAbs<MountDef> {
			protected override List<MountDef> GetCacheFromDb() {
				string command="SELECT * FROM mountdef ORDER BY ItemOrder";
				return Crud.MountDefCrud.SelectMany(command);
			}
			protected override List<MountDef> TableToList(DataTable table) {
				return Crud.MountDefCrud.TableToList(table);
			}
			protected override MountDef Copy(MountDef mountDef) {
				return mountDef.Copy();
			}
			protected override DataTable ListToTable(List<MountDef> listMountDefs) {
				return Crud.MountDefCrud.ListToTable(listMountDefs,"MountDef");
			}
			protected override void FillCacheIfNeeded() {
				MountDefs.GetTableFromCache(false);
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static MountDefCache _mountDefCache=new MountDefCache();

		public static List<MountDef> GetDeepCopy(bool isShort=false) {
			return _mountDefCache.GetDeepCopy(isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			//No need to check RemotingRole; no call to db.
			_mountDefCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _mountDefCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		///<summary></summary>
		public static void Update(MountDef def) {
			
			Crud.MountDefCrud.Update(def);
		}

		///<summary></summary>
		public static long Insert(MountDef def) {
			
			return Crud.MountDefCrud.Insert(def);
		}

		///<summary>No need to surround with try/catch, because all deletions are allowed.</summary>
		public static void Delete(long mountDefNum) {
			
			string command="DELETE FROM mountdef WHERE MountDefNum="+POut.Long(mountDefNum);
			Database.ExecuteNonQuery(command);
			command="DELETE FROM mountitemdef WHERE MountDefNum ="+POut.Long(mountDefNum);
			Database.ExecuteNonQuery(command);
		}
	}

		



		
	

	

	


}










