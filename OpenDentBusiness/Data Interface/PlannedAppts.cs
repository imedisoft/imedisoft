using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class PlannedAppts{
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


		///<summary>Gets all planned appt objects for a patient.</summary>
		public static List<PlannedAppt> Refresh(long patNum){
			
			string command="SELECT * FROM plannedappt WHERE PatNum="+POut.Long(patNum);
			return Crud.PlannedApptCrud.SelectMany(command);
		}

		///<Summary>Gets one plannedAppt from the database.</Summary>
		public static PlannedAppt GetOne(long plannedApptNum){
			
			return Crud.PlannedApptCrud.SelectOne(plannedApptNum);
		}

		///<summary>Gets one plannedAppt by patient, ordered by ItemOrder</summary>
		public static PlannedAppt GetOneOrderedByItemOrder(long patNum) {
			
			string command="SELECT * FROM plannedappt WHERE PatNum="+POut.Long(patNum)
				+" ORDER BY ItemOrder";
			command=DbHelper.LimitOrderBy(command,1);
			return Crud.PlannedApptCrud.SelectOne(command);
		}

		///<summary></summary>
		public static long Insert(PlannedAppt plannedAppt) {
			
			return Crud.PlannedApptCrud.Insert(plannedAppt);
		}

		///<summary></summary>
		public static void Update(PlannedAppt plannedAppt) {
			
			Crud.PlannedApptCrud.Update(plannedAppt);
		}

		///<summary></summary>
		public static void Update(PlannedAppt plannedAppt,PlannedAppt oldPlannedAppt) {
			
			Crud.PlannedApptCrud.Update(plannedAppt,oldPlannedAppt);
		}

	}
}