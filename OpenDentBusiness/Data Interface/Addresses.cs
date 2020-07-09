using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Addresses{
		#region Get Methods
		///<summary>Returns null if none.</summary>
		public static Address GetOneByPatNum(long patNum) {
			
			string command="SELECT * FROM address WHERE PatNumTaxPhysical = "+POut.Long(patNum);
			return Crud.AddressCrud.SelectOne(command);
		}
		#endregion Get Methods
		
		#region Modification Methods
		///<summary></summary>
		public static long Insert(Address address){
			
			return Crud.AddressCrud.Insert(address);
		}

		///<summary></summary>
		public static void Update(Address address){
			
			Crud.AddressCrud.Update(address);
		}

		///<summary></summary>
		public static void Delete(long addressNum) {
			
			Crud.AddressCrud.Delete(addressNum);
		}
		#endregion Modification Methods

	



	}
}