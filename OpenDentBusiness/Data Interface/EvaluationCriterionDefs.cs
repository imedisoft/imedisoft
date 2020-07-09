using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EvaluationCriterionDefs{
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

		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern

		private class EvaluationCriterionDefCache : CacheListAbs<EvaluationCriterionDef> {
			protected override List<EvaluationCriterionDef> GetCacheFromDb() {
				string command="SELECT * FROM EvaluationCriterionDef ORDER BY ItemOrder";
				return Crud.EvaluationCriterionDefCrud.SelectMany(command);
			}
			protected override List<EvaluationCriterionDef> TableToList(DataTable table) {
				return Crud.EvaluationCriterionDefCrud.TableToList(table);
			}
			protected override EvaluationCriterionDef Copy(EvaluationCriterionDef EvaluationCriterionDef) {
				return EvaluationCriterionDef.Clone();
			}
			protected override DataTable ListToTable(List<EvaluationCriterionDef> listEvaluationCriterionDefs) {
				return Crud.EvaluationCriterionDefCrud.ListToTable(listEvaluationCriterionDefs,"EvaluationCriterionDef");
			}
			protected override void FillCacheIfNeeded() {
				EvaluationCriterionDefs.GetTableFromCache(false);
			}
			protected override bool IsInListShort(EvaluationCriterionDef EvaluationCriterionDef) {
				return !EvaluationCriterionDef.IsHidden;
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static EvaluationCriterionDefCache _EvaluationCriterionDefCache=new EvaluationCriterionDefCache();

		///<summary>A list of all EvaluationCriterionDefs. Returns a deep copy.</summary>
		public static List<EvaluationCriterionDef> ListDeep {
			get {
				return _EvaluationCriterionDefCache.ListDeep;
			}
		}

		///<summary>A list of all visible EvaluationCriterionDefs. Returns a deep copy.</summary>
		public static List<EvaluationCriterionDef> ListShortDeep {
			get {
				return _EvaluationCriterionDefCache.ListShortDeep;
			}
		}

		///<summary>A list of all EvaluationCriterionDefs. Returns a shallow copy.</summary>
		public static List<EvaluationCriterionDef> ListShallow {
			get {
				return _EvaluationCriterionDefCache.ListShallow;
			}
		}

		///<summary>A list of all visible EvaluationCriterionDefs. Returns a shallow copy.</summary>
		public static List<EvaluationCriterionDef> ListShort {
			get {
				return _EvaluationCriterionDefCache.ListShallowShort;
			}
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_EvaluationCriterionDefCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _EvaluationCriterionDefCache.GetTableFromCache(doRefreshCache);
		}

		#endregion
		*/


		///<summary>Gets a list of all possible EvaluationCriterionDefs.  Defs attached to an EvaluationDef are copies and will not be shown.</summary>
		public static List<EvaluationCriterionDef> GetAvailableCriterionDefs() {
			
			string command="SELECT * FROM evaluationcriteriondef where EvaluationDefNum=0";
			return Crud.EvaluationCriterionDefCrud.SelectMany(command);
		}

		///<summary>Gets a list of all EvaluationCriterion attached to an EvaluationDef.  Ordered by ItemOrder.</summary>
		public static List<EvaluationCriterionDef> GetAllForEvaluationDef(long evaluationDefNum){
			
			string command="SELECT * FROM evaluationcriteriondef WHERE EvaluationDefNum = "+POut.Long(evaluationDefNum)+" "
				+"ORDER BY ItemOrder";
			return Crud.EvaluationCriterionDefCrud.SelectMany(command);
		}

		///<summary>Gets one EvaluationCriterionDef from the db.</summary>
		public static EvaluationCriterionDef GetOne(long evaluationCriterionDefNum){
			
			return Crud.EvaluationCriterionDefCrud.SelectOne(evaluationCriterionDefNum);
		}

		///<summary></summary>
		public static long Insert(EvaluationCriterionDef evaluationCriterionDef){
			
			return Crud.EvaluationCriterionDefCrud.Insert(evaluationCriterionDef);
		}

		///<summary></summary>
		public static void Update(EvaluationCriterionDef evaluationCriterionDef){
			
			Crud.EvaluationCriterionDefCrud.Update(evaluationCriterionDef);
		}

		///<summary></summary>
		public static void Delete(long evaluationCriterionDefNum) {
			
			string command= "DELETE FROM evaluationcriteriondef WHERE EvaluationCriterionDefNum = "+POut.Long(evaluationCriterionDefNum);
			Db.NonQ(command);
		}



	}
}