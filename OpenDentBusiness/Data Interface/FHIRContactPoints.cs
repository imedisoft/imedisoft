using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using OpenDentBusiness.Crud;

namespace OpenDentBusiness{
	///<summary></summary>
	public class FHIRContactPoints {
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

		public static List<FHIRContactPoint> GetContactPoints(long fHIRSubscriptionNum) {
			
			string command="SELECT * FROM fhircontactpoint WHERE FHIRSubscriptionNum="+POut.Long(fHIRSubscriptionNum);
			return FHIRContactPointCrud.SelectMany(command);
		}


		///<summary></summary>
		public static long Insert(FHIRContactPoint fHIRContactPoint) {
			
			return Crud.FHIRContactPointCrud.Insert(fHIRContactPoint);
		}

		///<summary></summary>
		public static void Update(FHIRContactPoint fHIRContactPoint) {
			
			Crud.FHIRContactPointCrud.Update(fHIRContactPoint);
		}

		///<summary></summary>
		public static void Delete(long fHIRContactPointNum) {
			
			Crud.FHIRContactPointCrud.Delete(fHIRContactPointNum);
		}

	}
}