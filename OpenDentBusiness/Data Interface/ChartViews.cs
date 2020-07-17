using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ChartViews{
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
		
		private class ChartViewCache : CacheListAbs<ChartView> {
			protected override List<ChartView> GetCacheFromDb() {
				string command="SELECT * FROM chartview ORDER BY ItemOrder";
				return Crud.ChartViewCrud.SelectMany(command);
			}
			protected override List<ChartView> TableToList(DataTable table) {
				return Crud.ChartViewCrud.TableToList(table);
			}
			protected override ChartView Copy(ChartView chartView) {
				return chartView.Copy();
			}
			protected override DataTable ListToTable(List<ChartView> listChartViews) {
				return Crud.ChartViewCrud.ListToTable(listChartViews,"ChartView");
			}
			protected override void FillCacheIfNeeded() {
				ChartViews.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static ChartViewCache _chartViewCache=new ChartViewCache();

		public static List<ChartView> GetDeepCopy(bool isShort=false) {
			return _chartViewCache.GetDeepCopy(isShort);
		}

		public static ChartView GetFirst(bool isShort=false) {
			return _chartViewCache.GetFirst(isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_chartViewCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _chartViewCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		///<summary></summary>
		public static long Insert(ChartView chartView) {
			
			return Crud.ChartViewCrud.Insert(chartView);
		}

		///<summary></summary>
		public static void Update(ChartView chartView) {
			
			Crud.ChartViewCrud.Update(chartView);
		}

		///<summary></summary>
		public static void Delete(long chartViewNum) {
			
			string command= "DELETE FROM chartview WHERE ChartViewNum = "+POut.Long(chartViewNum);
			Database.ExecuteNonQuery(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<ChartView> Refresh(long patNum){
			
			string command="SELECT * FROM chartview WHERE PatNum = "+POut.Long(patNum);
			return Crud.ChartViewCrud.SelectMany(command);
		}

		///<summary>Gets one ChartView from the db.</summary>
		public static ChartView GetOne(long chartViewNum){
			
			return Crud.ChartViewCrud.SelectOne(chartViewNum);
		}

		*/



	}
}