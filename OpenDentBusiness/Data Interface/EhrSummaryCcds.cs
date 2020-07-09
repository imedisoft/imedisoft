using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EhrSummaryCcds{
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

		private class EhrSummaryCcdCache : CacheListAbs<EhrSummaryCcd> {
			protected override List<EhrSummaryCcd> GetCacheFromDb() {
				string command="SELECT * FROM ehrsummaryccd";
				return Crud.EhrSummaryCcdCrud.SelectMany(command);
			}
			protected override List<EhrSummaryCcd> TableToList(DataTable table) {
				return Crud.EhrSummaryCcdCrud.TableToList(table);
			}
			protected override EhrSummaryCcd Copy(EhrSummaryCcd ehrSummaryCcd) {
				return ehrSummaryCcd.Copy();
			}
			protected override DataTable ListToTable(List<EhrSummaryCcd> listEhrSummaryCcds) {
				return Crud.EhrSummaryCcdCrud.ListToTable(listEhrSummaryCcds,"EhrSummaryCcd");
			}
			protected override void FillCacheIfNeeded() {
				EhrSummaryCcds.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static EhrSummaryCcdCache _ehrSummaryCcdCache=new EhrSummaryCcdCache();

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_ehrSummaryCcdCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _ehrSummaryCcdCache.GetTableFromCache(doRefreshCache);
		}

		#endregion
		
		///<summary></summary>
		public static List<EhrSummaryCcd> Refresh(long patNum){
			
			string command="SELECT * FROM ehrsummaryccd WHERE PatNum = "+POut.Long(patNum)+" ORDER BY DateSummary";
			return Crud.EhrSummaryCcdCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(EhrSummaryCcd ehrSummaryCcd){
			
			return Crud.EhrSummaryCcdCrud.Insert(ehrSummaryCcd);
		}

		///<summary>Returns null if no record is found.</summary>
		public static EhrSummaryCcd GetOneForEmailAttach(long emailAttachNum) {
			
			string command="SELECT * FROM ehrsummaryccd WHERE EmailAttachNum="+POut.Long(emailAttachNum)+" LIMIT 1";
			return Crud.EhrSummaryCcdCrud.SelectOne(command);
		}

		///<summary></summary>
		public static void Update(EhrSummaryCcd ehrSummaryCcd) {
			
			Crud.EhrSummaryCcdCrud.Update(ehrSummaryCcd);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.


		///<summary>Gets one EhrSummaryCcd from the db.</summary>
		public static EhrSummaryCcd GetOne(long ehrSummaryCcdNum){
			
			return Crud.EhrSummaryCcdCrud.SelectOne(ehrSummaryCcdNum);
		}

		///<summary></summary>
		public static void Delete(long ehrSummaryCcdNum) {
			
			string command= "DELETE FROM ehrsummaryccd WHERE EhrSummaryCcdNum = "+POut.Long(ehrSummaryCcdNum);
			Db.NonQ(command);
		}
		*/



	}
}