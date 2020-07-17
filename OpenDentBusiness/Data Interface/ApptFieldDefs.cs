using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ApptFieldDefs{
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
		private class ApptFieldDefCache : CacheListAbs<ApptFieldDef> {
			protected override List<ApptFieldDef> GetCacheFromDb() {
				string command="SELECT * FROM apptfielddef ORDER BY FieldName";
				return Crud.ApptFieldDefCrud.SelectMany(command);
			}
			protected override List<ApptFieldDef> TableToList(DataTable table) {
				return Crud.ApptFieldDefCrud.TableToList(table);
			}
			protected override ApptFieldDef Copy(ApptFieldDef apptFieldDef) {
				return apptFieldDef.Clone();
			}
			protected override DataTable ListToTable(List<ApptFieldDef> listApptFieldDefs) {
				return Crud.ApptFieldDefCrud.ListToTable(listApptFieldDefs,"ApptFieldDef");
			}
			protected override void FillCacheIfNeeded() {
				ApptFieldDefs.GetTableFromCache(false);
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static ApptFieldDefCache _apptFieldDefCache=new ApptFieldDefCache();

		public static bool GetExists(Predicate<ApptFieldDef> match,bool isShort=false) {
			return _apptFieldDefCache.GetExists(match,isShort);
		}

		public static List<ApptFieldDef> GetDeepCopy(bool isShort=false) {
			return _apptFieldDefCache.GetDeepCopy(isShort);
		}

		public static ApptFieldDef GetFirstOrDefault(Func<ApptFieldDef,bool> match,bool isShort=false) {
			return _apptFieldDefCache.GetFirstOrDefault(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_apptFieldDefCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _apptFieldDefCache.GetTableFromCache(doRefreshCache);
		}
		#endregion

		///<summary>Must supply the old field name so that the apptFields attached to appointments can be updated.  Will throw exception if new FieldName is already in use.</summary>
		public static void Update(ApptFieldDef apptFieldDef,string oldFieldName) {
			
			string command="SELECT COUNT(*) FROM apptfielddef WHERE FieldName='"+POut.String(apptFieldDef.FieldName)+"' "
				+"AND ApptFieldDefNum != "+POut.Long(apptFieldDef.ApptFieldDefNum);
			if(Database.ExecuteString(command)!="0"){
				throw new ApplicationException(Lans.g("FormApptFieldDefEdit","Field name already in use."));
			}
			Crud.ApptFieldDefCrud.Update(apptFieldDef);
			command="UPDATE apptfield SET FieldName='"+POut.String(apptFieldDef.FieldName)+"' "
				+"WHERE FieldName='"+POut.String(oldFieldName)+"'";
			Database.ExecuteNonQuery(command);
		}

		///<summary>Surround with try/catch in case field name already in use.</summary>
		public static long Insert(ApptFieldDef apptFieldDef) {
			
			string command="SELECT COUNT(*) FROM apptfielddef WHERE FieldName='"+POut.String(apptFieldDef.FieldName)+"'";
			if(Database.ExecuteString(command)!="0") {
				throw new ApplicationException(Lans.g("FormApptFieldDefEdit","Field name already in use."));
			}
			return Crud.ApptFieldDefCrud.Insert(apptFieldDef);
		}

		///<summary>Surround with try/catch, because it will throw an exception if any appointment is using this def.</summary>
		public static void Delete(ApptFieldDef apptFieldDef) {
			
			string command="SELECT LName,FName,AptDateTime "
				+"FROM patient,apptfield,appointment WHERE "
				+"patient.PatNum=appointment.PatNum "
				+"AND appointment.AptNum=apptfield.AptNum "
				+"AND FieldName='"+POut.String(apptFieldDef.FieldName)+"'";
			DataTable table=Database.ExecuteDataTable(command);
			DateTime aptDateTime;
			if(table.Rows.Count>0) {
				string s=Lans.g("FormApptFieldDefEdit","Not allowed to delete. Already in use by ")+table.Rows.Count.ToString()
					+" "+Lans.g("FormApptFieldDefEdit","appointments, including")+" \r\n";
				for(int i=0;i<table.Rows.Count;i++) {
					if(i>5) {
						break;
					}
					aptDateTime=PIn.Date(table.Rows[i]["AptDateTime"].ToString());
					s+=table.Rows[i]["LName"].ToString()+", "+table.Rows[i]["FName"].ToString()+POut.DateT(aptDateTime,false)+"\r\n";
				}
				throw new ApplicationException(s);
			}
			command="DELETE FROM apptfielddef WHERE ApptFieldDefNum ="+POut.Long(apptFieldDef.ApptFieldDefNum);
			Database.ExecuteNonQuery(command);
		}
		
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<ApptFieldDef> Refresh(long patNum){
			
			string command="SELECT * FROM apptfielddef WHERE PatNum = "+POut.Long(patNum);
			return Crud.ApptFieldDefCrud.SelectMany(command);
		}

		///<summary>Gets one ApptFieldDef from the db.</summary>
		public static ApptFieldDef GetOne(long apptFieldDefNum){
			
			return Crud.ApptFieldDefCrud.SelectOne(apptFieldDefNum);
		}
		*/

		public static string GetFieldName(long apptFieldDefNum) {
			//No need to check RemotingRole; no call to db.
			ApptFieldDef apptFieldDef=GetFirstOrDefault(x => x.ApptFieldDefNum==apptFieldDefNum);
			return (apptFieldDef==null ? "" : apptFieldDef.FieldName);
		}

		/// <summary>GetPickListByFieldName returns the pick list identified by the field name passed as a parameter.</summary>
		public static string GetPickListByFieldName(string FieldName) {
			//No need to check RemotingRole; no call to db.
			ApptFieldDef apptFieldDef =GetFirstOrDefault(x => x.FieldName==FieldName);
			return (apptFieldDef==null ? "" : apptFieldDef.PickList);
		}

		///<summary>Returns true if there are any duplicate field names in the entire apptfielddef table.</summary>
		public static bool HasDuplicateFieldNames() {
			
			string command="SELECT COUNT(*) FROM apptfielddef GROUP BY FieldName HAVING COUNT(FieldName) > 1";
			return (Database.ExecuteScalar(command)!="");
		}

		///<summary>Returns the ApptFieldDef for the specified field name. Returns null if an ApptFieldDef does not exist for that field name.</summary>
		public static ApptFieldDef GetFieldDefByFieldName(string fieldName) {
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => x.FieldName==fieldName);
		}
	}
}