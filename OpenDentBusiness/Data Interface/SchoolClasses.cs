using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SchoolClasses {

		#region CachePattern

		private class SchoolClassCache : CacheListAbs<SchoolClass> {
			protected override List<SchoolClass> GetCacheFromDb() {
				string command=
					"SELECT * FROM schoolclass "
					+"ORDER BY GradYear,Descript";
				return Crud.SchoolClassCrud.SelectMany(command);
			}
			protected override List<SchoolClass> TableToList(DataTable table) {
				return Crud.SchoolClassCrud.TableToList(table);
			}
			protected override SchoolClass Copy(SchoolClass schoolClass) {
				return schoolClass.Copy();
			}
			protected override DataTable ListToTable(List<SchoolClass> listSchoolClasss) {
				return Crud.SchoolClassCrud.ListToTable(listSchoolClasss,"SchoolClass");
			}
			protected override void FillCacheIfNeeded() {
				SchoolClasses.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static SchoolClassCache _schoolClassCache=new SchoolClassCache();

		public static List<SchoolClass> GetDeepCopy(bool isShort=false) {
			return _schoolClassCache.GetDeepCopy(isShort);
		}

		public static SchoolClass GetFirstOrDefault(Func<SchoolClass,bool> match,bool isShort=false) {
			return _schoolClassCache.GetFirstOrDefault(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_schoolClassCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			return _schoolClassCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

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
		public static void Update(SchoolClass sc){
			
			Crud.SchoolClassCrud.Update(sc);
		}

		///<summary></summary>
		public static long Insert(SchoolClass sc) {
			
			return Crud.SchoolClassCrud.Insert(sc);
		}

		///<summary></summary>
		public static void InsertOrUpdate(SchoolClass sc, bool isNew){
			//No need to check RemotingRole; no call to db.
			//if(IsRepeating && DateTask.Year>1880){
			//	throw new Exception(Lans.g(this,"Task cannot be tagged repeating and also have a date."));
			//}
			if(isNew){
				Insert(sc);
			}
			else{
				Update(sc);
			}
		}

		///<summary>Surround by a try/catch in case there are dependencies.</summary>
		public static void Delete(long classNum) {
			
			//check for attached providers
			string  command="SELECT COUNT(*) FROM provider WHERE SchoolClassNum = '"
				+POut.Long(classNum)+"'";
			DataTable table=Database.ExecuteDataTable(command);
			if(PIn.String(table.Rows[0][0].ToString())!="0"){
				throw new Exception(Lans.g("SchoolClasses","Class already in use by providers."));
			}
			//check for attached reqneededs.
			command="SELECT COUNT(*) FROM reqneeded WHERE SchoolClassNum = '"
				+POut.Long(classNum)+"'";
			table=Database.ExecuteDataTable(command);
			if(PIn.String(table.Rows[0][0].ToString())!="0") {
				throw new Exception(Lans.g("SchoolClasses","Class already in use by 'requirements needed' table."));
			}
			command= "DELETE from schoolclass WHERE SchoolClassNum = '"
				+POut.Long(classNum)+"'";
 			Database.ExecuteNonQuery(command);
		}

		public static string GetDescript(long SchoolClassNum) {
			//No need to check RemotingRole; no call to db.
			SchoolClass schoolClass=GetFirstOrDefault(x => x.SchoolClassNum==SchoolClassNum);
			return (schoolClass==null ? "" : GetDescript(schoolClass));
		}

		public static string GetDescript(SchoolClass schoolClass) {
			//No need to check RemotingRole; no call to db.
			return schoolClass.GradYear+"-"+schoolClass.Descript;
		}
	}

	

	


}




















