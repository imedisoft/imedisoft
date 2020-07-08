using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using CodeBase;
using Newtonsoft.Json;

namespace OpenDentBusiness{
	///<summary></summary>
	public class CareCreditWebResponses {
		#region Get Methods
		///<summary>Gets one PayConnectResponseWeb from the db.</summary>
		public static CareCreditWebResponse GetOne(long ccWebResponse) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<CareCreditWebResponse>(MethodBase.GetCurrentMethod(),ccWebResponse);
			}
			return Crud.CareCreditWebResponseCrud.SelectOne(ccWebResponse);
		}

		///<summary></summary>
		public static CareCreditWebResponse GetOneByPayNum(long payNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<CareCreditWebResponse>(MethodBase.GetCurrentMethod(),payNum);
			}
			string command=$"SELECT * FROM carecreditwebresponse WHERE PayNum={POut.Long(payNum)}";
			return Crud.CareCreditWebResponseCrud.SelectOne(command);
		}

		///<summary></summary>
		public static CareCreditWebResponse GetByRefId(string refNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<CareCreditWebResponse>(MethodBase.GetCurrentMethod(),refNum);
			}
			string command=$"SELECT * FROM carecreditwebresponse WHERE RefNumber='{POut.String(refNum)}'";
			return Crud.CareCreditWebResponseCrud.SelectOne(command);
		}

		///<summary>Gets all PayConnectResponseWebs that have a status of Pending from the db.</summary>
		public static List<CareCreditWebResponse> GetAllPending() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CareCreditWebResponse>>(MethodBase.GetCurrentMethod());
			}
			return Crud.CareCreditWebResponseCrud.SelectMany("SELECT * FROM carecreditwebresponse "
				+"WHERE ProcessingStatus='"+CareCreditWebStatus.Pending.ToString()+"'");
		}

		public static List<CareCreditWebResponse> GetApprovedTransactions(List<long> listClinicNums,DateTime dateFrom,DateTime dateTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CareCreditWebResponse>>(MethodBase.GetCurrentMethod(),listClinicNums,dateFrom,dateTo);
			}
			string clinicFilter="";
			if(!listClinicNums.IsNullOrEmpty()) {
				clinicFilter=$"AND ClinicNum IN({string.Join(",",listClinicNums.Select(x => POut.Long(x)))})";
			}
			string command="SELECT * FROM carecreditwebresponse "
				+$"WHERE {DbHelper.BetweenDates("DateTimeCompleted",dateFrom,dateTo)} "
				+"AND ProcessingStatus IN('"+CareCreditWebStatus.Completed.ToString()+"') "
				+$"AND ServiceType='{CareCreditServiceType.Prefill.ToString()}' "
				+$"{clinicFilter} ";
			return Crud.CareCreditWebResponseCrud.SelectMany(command);
		}

		///<summary>Returns a distinct list of PatNums that have a row in the date range passed in. Uses DateTimeEntry.</summary>
		public static List<long> GetPatNumsForDateRangeToExclude(DateTime dateFrom,DateTime dateTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),dateFrom,dateTo);
			}
			string command="SELECT PatNum FROM carecreditwebresponse "
				+$"WHERE ProcessingStatus IN({string.Join(",",GetExclusionStatuses().Select(x => $"'{POut.String(x.ToString())}'")).ToList()}) "
				+$"AND {DbHelper.BetweenDates("DateTimeEntry",dateFrom,dateTo)}";
			return Db.GetListLong(command).Distinct().ToList();
		}

		#endregion Get Methods
		#region Modification Methods
		#region Insert
		///<summary></summary>
		public static long Insert(CareCreditWebResponse ccWebResponse) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				ccWebResponse.CareCreditWebResponseNum=Meth.GetLong(MethodBase.GetCurrentMethod(),ccWebResponse);
				return ccWebResponse.CareCreditWebResponseNum;
			}
			return Crud.CareCreditWebResponseCrud.Insert(ccWebResponse);
		}
		#endregion Insert
		#region Update
		///<summary></summary>
		public static void Update(CareCreditWebResponse ccWebResponse) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),ccWebResponse);
				return;
			}
			Crud.CareCreditWebResponseCrud.Update(ccWebResponse);
		}
		#endregion Update
		#endregion Modification Methods
		#region Misc Methods
		public static List<CareCreditWebStatus> GetExclusionStatuses() {
			return new List<CareCreditWebStatus>() { CareCreditWebStatus.Pending,CareCreditWebStatus.CallForAuth,
				CareCreditWebStatus.Completed,CareCreditWebStatus.DupQS,CareCreditWebStatus.Declined,CareCreditWebStatus.AccountFound };
		}

		///<summary></summary>
		public static void HandleResponseError(CareCreditWebResponse ccWebResponse,string resStr) {
			//No remoting role check; no call to db
			ccWebResponse.LastResponseStr=resStr;
			if(ccWebResponse.ProcessingStatus==CareCreditWebStatus.Created) {
				ccWebResponse.ProcessingStatus=CareCreditWebStatus.CreatedError;
			}
			else if(ccWebResponse.ProcessingStatus==CareCreditWebStatus.Pending) {
				ccWebResponse.ProcessingStatus=CareCreditWebStatus.PendingError;
			}
			else {
				ccWebResponse.ProcessingStatus=CareCreditWebStatus.UnknownError;
			}
			ccWebResponse.DateTimeLastError=MiscData.GetNowDateTime();
		}
		#endregion Misc Methods



	}
}