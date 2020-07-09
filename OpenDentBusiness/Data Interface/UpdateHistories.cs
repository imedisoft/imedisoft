using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using CodeBase;

namespace OpenDentBusiness {
	///<summary></summary>
	public class UpdateHistories{
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

		///<summary>Gets one UpdateHistory from the db.</summary>
		public static UpdateHistory GetOne(long updateNum){
			
			return Crud.UpdateHistoryCrud.SelectOne(updateNum);
		}

		///<summary></summary>
		public static long Insert(UpdateHistory updateHistory){
			return Crud.UpdateHistoryCrud.Insert(updateHistory);
		}

		///<summary></summary>
		public static void Update(UpdateHistory updateHistory){
			
			Crud.UpdateHistoryCrud.Update(updateHistory);
		}

		///<summary></summary>
		public static void Delete(long updateNum) {
			
			Crud.UpdateHistoryCrud.Delete(updateNum);
		}

		///<summary>All updatehistory entries ordered by DateTimeUpdated.</summary>
		public static List<UpdateHistory> GetAll() {
			
			string command="SELECT * FROM updatehistory ORDER BY DateTimeUpdated";
			return Crud.UpdateHistoryCrud.SelectMany(command);
		}

		///<summary>Get the most recently inserted updatehistory entry. Ordered by DateTimeUpdated.</summary>
		public static UpdateHistory GetLastUpdateHistory() {
			
			string command=@"SELECT * 
				FROM updatehistory
				ORDER BY DateTimeUpdated DESC
				LIMIT 1";
			return Crud.UpdateHistoryCrud.SelectOne(command);
		}

		///<summary>Gets the most recently inserted updatehistory entries. Ordered by DateTimeUpdated.</summary>
		public static List<UpdateHistory> GetPreviousUpdateHistories(int count) {
			
			string command=@"SELECT * 
				FROM updatehistory
				ORDER BY DateTimeUpdated DESC
				LIMIT "+POut.Int(count);
			return Crud.UpdateHistoryCrud.SelectMany(command);
		}

		///<summary>Returns the latest version information.</summary>
		public static UpdateHistory GetForVersion(string version) {
			
			string command="SELECT * FROM updatehistory WHERE ProgramVersion='"+POut.String(version.ToString())+"'";
			return Crud.UpdateHistoryCrud.SelectOne(command);
		}

		///<summary>Returns the earliest datetime that a version was reached. If that version has not been reached, returns the MinDate.</summary>
		public static DateTime GetDateForVersion(Version version) {
			List<UpdateHistory> listUpdates=UpdateHistories.GetAll();
			foreach(UpdateHistory update in listUpdates) {
				Version compareVersion=new Version();
				ODException.SwallowAnyException(() => { compareVersion=new Version(update.ProgramVersion); });//Just in case.
				if(compareVersion>=version) {
					return update.DateTimeUpdated;
				}
			}
			//The earliest version was later than the version passed in.
			return new DateTime(1,1,1);
		}



	}
}