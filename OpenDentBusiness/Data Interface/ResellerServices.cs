using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	///<summary></summary>
	public class ResellerServices {

		#region Get Methods

		///<summary></summary>
		public static List<ResellerService> GetAll() {
			
			return Crud.ResellerServiceCrud.SelectMany("SELECT * FROM resellerservice");
		}

		///<summary></summary>
		public static List<ResellerService> GetServicesForReseller(long resellerNum) {
			
			string command="SELECT * FROM resellerservice WHERE ResellerNum = "+POut.Long(resellerNum);
			return Crud.ResellerServiceCrud.SelectMany(command);
		}

		///<summary>Gets one ResellerService from the db.</summary>
		public static ResellerService GetOne(long resellerServiceNum){
			
			return Crud.ResellerServiceCrud.SelectOne(resellerServiceNum);
		}

		#endregion

		#region Modification Methods
		
		#region Insert
		///<summary></summary>
		public static long Insert(ResellerService resellerService) {
			
			return Crud.ResellerServiceCrud.Insert(resellerService);
		}

		#endregion

		#region Update

		///<summary></summary>
		public static void Update(ResellerService resellerService) {
			
			Crud.ResellerServiceCrud.Update(resellerService);
		}

		#endregion

		#region Delete

		///<summary></summary>
		public static void Delete(long resellerServiceNum) {
			
			string command= "DELETE FROM resellerservice WHERE ResellerServiceNum = "+POut.Long(resellerServiceNum);
			Database.ExecuteNonQuery(command);
		}

		#endregion

		#endregion

		#region Misc Methods
		#endregion

	}
}