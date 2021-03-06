using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class PayConnectResponseWebs{
		#region Get Methods
		
		///<summary>Gets one PayConnectResponseWeb from the db.</summary>
		public static PayConnectResponseWeb GetOne(long payConnectResponseWebNum){
			
			return Crud.PayConnectResponseWebCrud.SelectOne(payConnectResponseWebNum);
		}

		public static PayConnectResponseWeb GetOneByPayNum(long payNum) {
			
			string command=$"SELECT * FROM payconnectresponseweb WHERE PayNum={POut.Long(payNum)}";
			return Crud.PayConnectResponseWebCrud.SelectOne(command);
		}

		///<summary>Gets all PayConnectResponseWebs that have a status of Pending from the db.</summary>
		public static List<PayConnectResponseWeb> GetAllPending() {
			
			return Crud.PayConnectResponseWebCrud.SelectMany("SELECT * FROM payconnectresponseweb WHERE ProcessingStatus='"+PayConnectWebStatus.Pending.ToString()+"'");
		}
		#endregion Get Methods
		#region Modification Methods
		#region Insert
		///<summary></summary>
		public static long Insert(PayConnectResponseWeb payConnectResponseWeb){
			
			return Crud.PayConnectResponseWebCrud.Insert(payConnectResponseWeb);
		}
		#endregion Insert
		#region Update
		///<summary></summary>
		public static void Update(PayConnectResponseWeb payConnectResponseWeb){
			
			Crud.PayConnectResponseWebCrud.Update(payConnectResponseWeb);
		}
		#endregion Update
		#endregion Modification Methods
		#region Misc Methods


		public static void HandleResponseError(PayConnectResponseWeb responseWeb,string resStr) {
			responseWeb.LastResponseStr=resStr;
			if(responseWeb.ProcessingStatus==PayConnectWebStatus.Created) {
				responseWeb.ProcessingStatus=PayConnectWebStatus.CreatedError;
			}
			else if(responseWeb.ProcessingStatus==PayConnectWebStatus.Pending) {
				responseWeb.ProcessingStatus=PayConnectWebStatus.PendingError;
			}
			else {
				responseWeb.ProcessingStatus=PayConnectWebStatus.UnknownError;
			}
			responseWeb.DateTimeLastError=MiscData.GetNowDateTime();
		}


		#endregion Misc Methods




	}
}