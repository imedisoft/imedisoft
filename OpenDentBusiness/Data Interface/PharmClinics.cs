using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class PharmClinics{
		
		#region Get Methods

		///<summary>Gets one PharmClinic from the db.</summary>
		///<param name="pharmClinicNum">The primary key for the object that will be retrieved.</param>
		public static PharmClinic GetOne(long pharmClinicNum) {
			
			return Crud.PharmClinicCrud.SelectOne(pharmClinicNum);
		}

		///<summary>Gets a list of PharmClinics for a given pharmacy</summary>
		///<param name="pharmacyNum">The primary key of the pharmacy.</param>
		public static List<PharmClinic> GetPharmClinicsForPharmacy(long pharmacyNum) {
			
			string command="SELECT * FROM pharmclinic WHERE PharmacyNum = "+POut.Long(pharmacyNum);
			return Crud.PharmClinicCrud.SelectMany(command);
		}

		///<summary>Gets a pharmclinic for a specific clinic and pharmacy pair. Can return null.</summary>
		///<param name="pharmacyNum">The primary key of the pharmacy.</param>
		///<param name="clinicNum">The primary key of the clinic.</param>
		public static PharmClinic GetOneForPharmacyAndClinic(long pharmacyNum,long clinicNum) {
			
			string command = "SELECT * FROM pharmclinic WHERE PharmacyNum = "+POut.Long(pharmacyNum)+" AND ClinicNum = "+POut.Long(clinicNum);
			return Crud.PharmClinicCrud.SelectOne(command);
		}

		#endregion
		#region Modification Methods
		#region Insert

		///<summary></summary>
		///<param name="pharmClinic">The PharmClinic to insert.</param>
		public static long Insert(PharmClinic pharmClinic){
			
			return Crud.PharmClinicCrud.Insert(pharmClinic);
		}

			#endregion
		#region Update

		///<summary>Updates a single pharmclinic object.</summary>
		///<param name="pharmClinic">The PharmClinic to update.</param>
		public static void Update(PharmClinic pharmClinic){
			
			Crud.PharmClinicCrud.Update(pharmClinic);
		}

		///<summary>Takes two lists of makes the appropriate database changes.</summary>
		///<param name="listPharmClinicsNew">The new list of PharmClinic objects.</param>
		///<param name="listPharmClinicsOld">The old list of PharmClinic objects.</param>
		public static void Sync(List<PharmClinic> listPharmClinicsNew,List<PharmClinic> listPharmClinicsOld) {
			if(listPharmClinicsOld.Count==0 && listPharmClinicsNew.Count==0) {//No need to send to middle tier.
				return;
			}
			
			Crud.PharmClinicCrud.Sync(listPharmClinicsNew,listPharmClinicsOld);
		}

			#endregion
		#region Delete
		///<summary></summary>
		public static void Delete(long pharmClinicNum) {
			
			Crud.PharmClinicCrud.Delete(pharmClinicNum);
		}
			#endregion
		#endregion
		#region Misc Methods
		

		
		#endregion
		



	}
}