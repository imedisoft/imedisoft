using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EhrCarePlans{
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
		public static List<EhrCarePlan> Refresh(long patNum) {
			
			string command="SELECT * FROM ehrcareplan WHERE PatNum = "+POut.Long(patNum)+" ORDER BY DatePlanned";
			return Crud.EhrCarePlanCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(EhrCarePlan ehrCarePlan) {
			
			return Crud.EhrCarePlanCrud.Insert(ehrCarePlan);
		}

		///<summary></summary>
		public static void Update(EhrCarePlan ehrCarePlan) {
			
			Crud.EhrCarePlanCrud.Update(ehrCarePlan);
		}

		///<summary></summary>
		public static void Delete(long ehrCarePlanNum) {
			
			string command= "DELETE FROM ehrcareplan WHERE EhrCarePlanNum = "+POut.Long(ehrCarePlanNum);
			Database.ExecuteNonQuery(command);
		}
		
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary>Gets one EhrCarePlan from the db.</summary>
		public static EhrCarePlan GetOne(long ehrCarePlanNum){
			
			return Crud.EhrCarePlanCrud.SelectOne(ehrCarePlanNum);
		}

		*/



	}
}