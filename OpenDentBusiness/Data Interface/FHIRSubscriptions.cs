using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class FHIRSubscriptions{
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

		///<summary>Gets one FHIRSubscription from the db.</summary>
		public static FHIRSubscription GetOne(long fHIRSubscriptionNum) {
			
			return Crud.FHIRSubscriptionCrud.SelectOne(fHIRSubscriptionNum);
		}

		///<summary></summary>
		public static long Insert(FHIRSubscription fHIRSubscription) {
			
			long fHIRSubscriptionNum=Crud.FHIRSubscriptionCrud.Insert(fHIRSubscription);
			foreach(FHIRContactPoint fHIRContactPoint in fHIRSubscription.ListContactPoints) {
				fHIRContactPoint.FHIRSubscriptionNum=fHIRSubscriptionNum;
				FHIRContactPoints.Insert(fHIRContactPoint);
			}
			return fHIRSubscriptionNum;
		}

		///<summary></summary>
		public static void Update(FHIRSubscription fHIRSubscription) {
			
			string command="SELECT * FROM fhircontactpoint WHERE FHIRSubscriptionNum="+POut.Long(fHIRSubscription.FHIRSubscriptionNum);
			List<FHIRContactPoint> listDbOld=Crud.FHIRContactPointCrud.SelectMany(command);
			foreach(FHIRContactPoint contactPoint in fHIRSubscription.ListContactPoints) {
				contactPoint.FHIRSubscriptionNum=fHIRSubscription.FHIRSubscriptionNum;
				if(listDbOld.Any(x => x.FHIRContactPointNum==contactPoint.FHIRContactPointNum)) {
					//Update any FHIRContactPoint that already exists in the db
					FHIRContactPoints.Update(contactPoint);
				}
				else {
					//Insert any FHIRContactPoint that does not exist in the db
					FHIRContactPoints.Insert(contactPoint);
				}
			}			
			//Delete any FHIRContactPoint that exists in the db but not in the new list
			listDbOld.FindAll(x => !fHIRSubscription.ListContactPoints.Any(y => y.FHIRContactPointNum==x.FHIRContactPointNum))
				.ForEach(x => FHIRContactPoints.Delete(x.FHIRContactPointNum));
			Crud.FHIRSubscriptionCrud.Update(fHIRSubscription);
		}

		///<summary></summary>
		public static void Update(FHIRSubscription fHIRSubscription,FHIRSubscription fHIRSubscriptionOld) {
			
			Crud.FHIRSubscriptionCrud.Update(fHIRSubscription,fHIRSubscriptionOld);
		}

			///<summary></summary>
		public static void Delete(long fHIRSubscriptionNum) {
			
			Crud.FHIRSubscriptionCrud.Delete(fHIRSubscriptionNum);
			string command="DELETE FROM fhircontactpoint WHERE FHIRSubscriptionNum="+POut.Long(fHIRSubscriptionNum);
			Db.NonQ(command);
		}
	}
}