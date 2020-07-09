using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	///<summary></summary>
	public class FamilyHealths {
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
		public static void Delete(long familyHealthNum) {
			
			string command= "DELETE FROM familyhealth WHERE FamilyHealthNum = "+POut.Long(familyHealthNum);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static long Insert(FamilyHealth familyHealth) {
			
			return Crud.FamilyHealthCrud.Insert(familyHealth);
		}

		///<summary></summary>
		public static void Update(FamilyHealth familyHealth) {
			
			Crud.FamilyHealthCrud.Update(familyHealth);
		}

		///<summary></summary>
		public static List<FamilyHealth> GetFamilyHealthForPat(long patNum) {
			
			string command="SELECT * FROM familyhealth WHERE PatNum = "+POut.Long(patNum);
			return Crud.FamilyHealthCrud.SelectMany(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary>Gets one FamilyHealth from the db.</summary>
		public static FamilyHealth GetOne(long familyHealthNum){
			
			return Crud.FamilyHealthCrud.SelectOne(familyHealthNum);
		}
		*/



	}
}