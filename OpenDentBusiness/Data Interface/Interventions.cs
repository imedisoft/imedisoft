using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Interventions{
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
		private class InterventionCache : CacheListAbs<Intervention> {
			protected override List<Intervention> GetCacheFromDb() {
				string command="SELECT * FROM Intervention ORDER BY ItemOrder";
				return Crud.InterventionCrud.SelectMany(command);
			}
			protected override List<Intervention> TableToList(DataTable table) {
				return Crud.InterventionCrud.TableToList(table);
			}
			protected override Intervention Copy(Intervention Intervention) {
				return Intervention.Clone();
			}
			protected override DataTable ListToTable(List<Intervention> listInterventions) {
				return Crud.InterventionCrud.ListToTable(listInterventions,"Intervention");
			}
			protected override void FillCacheIfNeeded() {
				Interventions.GetTableFromCache(false);
			}
			protected override bool IsInListShort(Intervention Intervention) {
				return !Intervention.IsHidden;
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static InterventionCache _InterventionCache=new InterventionCache();

		///<summary>A list of all Interventions. Returns a deep copy.</summary>
		public static List<Intervention> ListDeep {
			get {
				return _InterventionCache.ListDeep;
			}
		}

		///<summary>A list of all visible Interventions. Returns a deep copy.</summary>
		public static List<Intervention> ListShortDeep {
			get {
				return _InterventionCache.ListShortDeep;
			}
		}

		///<summary>A list of all Interventions. Returns a shallow copy.</summary>
		public static List<Intervention> ListShallow {
			get {
				return _InterventionCache.ListShallow;
			}
		}

		///<summary>A list of all visible Interventions. Returns a shallow copy.</summary>
		public static List<Intervention> ListShort {
			get {
				return _InterventionCache.ListShallowShort;
			}
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_InterventionCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _InterventionCache.GetTableFromCache(doRefreshCache);
		}

		#endregion
		*/
		
		///<summary></summary>
		public static long Insert(Intervention intervention) {
			
			return Crud.InterventionCrud.Insert(intervention);
		}

		///<summary></summary>
		public static void Update(Intervention intervention) {
			
			Crud.InterventionCrud.Update(intervention);
		}

		///<summary></summary>
		public static void Delete(long interventionNum) {
			
			string command= "DELETE FROM intervention WHERE InterventionNum = "+POut.Long(interventionNum);
			Database.ExecuteNonQuery(command);
		}

		///<summary></summary>
		public static List<Intervention> Refresh(long patNum) {
			
			string command="SELECT * FROM intervention WHERE PatNum = "+POut.Long(patNum);
			return Crud.InterventionCrud.SelectMany(command);
		}

		public static List<Intervention> Refresh(long patNum,InterventionCodeSet codeSet) {
			
			string command="SELECT * FROM intervention WHERE PatNum = "+POut.Long(patNum)+" AND CodeSet = "+POut.Int((int)codeSet);
			return Crud.InterventionCrud.SelectMany(command);
		}

		///<summary>Gets list of CodeValue strings from interventions with DateEntry in the last year and CodeSet equal to the supplied codeSet.
		///Result list is grouped by CodeValue, CodeSystem even though we only return the list of CodeValues.  However, there are no codes in the
		///EHR intervention code list that conflict between code systems, so we should never have a duplicate code in the returned list.</summary>
		public static List<string> GetAllForCodeSet(InterventionCodeSet codeSet) {
			
			string command="SELECT CodeValue FROM intervention WHERE CodeSet="+POut.Int((int)codeSet)+" "
				+"AND "+DbHelper.DtimeToDate("DateEntry")+">="+POut.Date(MiscData.GetNowDateTime().AddYears(-1))+" "
				+"GROUP BY CodeValue,CodeSystem";
			return Database.GetListString(command);
		}

		///<summary>Gets one Intervention from the db.</summary>
		public static Intervention GetOne(long interventionNum) {
			
			return Crud.InterventionCrud.SelectOne(interventionNum);
		}

	}
}