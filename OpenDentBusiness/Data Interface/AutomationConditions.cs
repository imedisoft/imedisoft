using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class AutomationConditions{
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
		private class AutomationConditionCache : CacheListAbs<AutomationCondition> {
			protected override List<AutomationCondition> GetCacheFromDb() {
				string command="SELECT * FROM automationcondition";
				return Crud.AutomationConditionCrud.SelectMany(command);
			}
			protected override List<AutomationCondition> TableToList(DataTable table) {
				return Crud.AutomationConditionCrud.TableToList(table);
			}
			protected override AutomationCondition Copy(AutomationCondition automationCondition) {
				return automationCondition.Clone();
			}
			protected override DataTable ListToTable(List<AutomationCondition> listAutomationConditions) {
				return Crud.AutomationConditionCrud.ListToTable(listAutomationConditions,"AutomationCondition");
			}
			protected override void FillCacheIfNeeded() {
				AutomationConditions.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static AutomationConditionCache _automationConditionCache=new AutomationConditionCache();

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_automationConditionCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _automationConditionCache.GetTableFromCache(doRefreshCache);
		}
		#endregion

		///<summary>Gets one AutomationCondition from the db.</summary>
		public static AutomationCondition GetOne(long automationConditionNum) {
			
			return Crud.AutomationConditionCrud.SelectOne(automationConditionNum);
		}

		///<summary>Gets a list of AutomationConditions from the db by AutomationNum.</summary>
		public static List<AutomationCondition> GetListByAutomationNum(long automationNum) {
			
			string command="SELECT * FROM automationcondition WHERE AutomationNum = "+POut.Long(automationNum);
			return Crud.AutomationConditionCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(AutomationCondition automationCondition) {
			
			return Crud.AutomationConditionCrud.Insert(automationCondition);
		}

		///<summary></summary>
		public static void Update(AutomationCondition automationCondition) {
			
			Crud.AutomationConditionCrud.Update(automationCondition);
		}

		///<summary></summary>
		public static void Delete(long automationConditionNum) {
			
			string command= "DELETE FROM automationcondition WHERE AutomationConditionNum = "+POut.Long(automationConditionNum);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void DeleteByAutomationNum(long automationNum) {
			
			string command= "DELETE FROM automationcondition WHERE AutomationNum = "+POut.Long(automationNum);
			Db.NonQ(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<AutomationCondition> Refresh(long patNum){
			
			string command="SELECT * FROM automationcondition WHERE PatNum = "+POut.Long(patNum);
			return Crud.AutomationConditionCrud.SelectMany(command);
		}

		
		*/



	}
}