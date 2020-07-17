using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class VaccinePats{
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
		public static List<VaccinePat> Refresh(long patNum){
			
			string command="SELECT * FROM vaccinepat WHERE PatNum = "+POut.Long(patNum)+" ORDER BY DateTimeStart";
			return Crud.VaccinePatCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(VaccinePat vaccinePat){
			
			return Crud.VaccinePatCrud.Insert(vaccinePat);
		}

		///<summary></summary>
		public static void Update(VaccinePat vaccinePat){
			
			Crud.VaccinePatCrud.Update(vaccinePat);
		}

		///<summary></summary>
		public static void Delete(long vaccinePatNum) {
			
			string command= "DELETE FROM vaccinepat WHERE VaccinePatNum = "+POut.Long(vaccinePatNum);
			Database.ExecuteNonQuery(command);
			//Delete any attached observations.
			VaccineObses.DeleteForVaccinePat(vaccinePatNum);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		
		///<summary>Gets one VaccinePat from the db.</summary>
		public static VaccinePat GetOne(long vaccinePatNum){
			
			return Crud.VaccinePatCrud.SelectOne(vaccinePatNum);
		}
		*/



	}
}