using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EServiceSignals {
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

		
		///<summary>returns all eServiceSignals for a given service within the date range, inclusive.</summary>
		public static List<EServiceSignal> GetServiceHistory(eServiceCode serviceCode,DateTime dateStart,DateTime dateStop,int limit=0) {
			
			string command="SELECT * FROM eservicesignal "
				+"WHERE ServiceCode="+POut.Int((int)serviceCode)+" "
				+"AND SigDateTime BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateStop.Date.AddDays(1))+" "
				+"ORDER BY SigDateTime DESC, Severity DESC";
			if(limit > 0) {
				command=DbHelper.LimitOrderBy(command,limit);
			}
			return Crud.EServiceSignalCrud.SelectMany(command);
		}

		///<summary>Returns the last known status for the given eService.</summary>
		public static eServiceSignalSeverity GetServiceStatus(eServiceCode serviceCode) {
			
			//The only statuses within the eServiceSignalSeverity enum are NotEnabled, Working, and Critical.
			//All other statuses are used for logging purposes and should not be considered within this method.
			string command="SELECT * FROM eservicesignal WHERE ServiceCode="+POut.Int((int)serviceCode)+" "
				+"ORDER BY SigDateTime DESC, Severity DESC ";
			command=DbHelper.LimitOrderBy(command,1);
			List<EServiceSignal> listSignal=Crud.EServiceSignalCrud.SelectMany(command);
			if(listSignal.Count==0) {
				//NoSignals exist for this service.
				return eServiceSignalSeverity.None;
			}
			return listSignal[0].Severity;
		}

		///<summary>Returns the last known status for the Listener Service.  
		///Returns Critical if a signal has not been entered in the last 5 minutes.
		///Returns Error if there are ANY error signals that have not been processed.</summary>
		public static eServiceSignalSeverity GetListenerServiceStatus() {
			
			//Additionally, this query will run a subselect to get the count of all unprocessed errors.
			//Running that query as a subselect here simply saves an extra call to the database.
			//This subselect should be fine to run here since the query is limited to one result and the count of unprocessed errors should be small.
			string command="SELECT eservicesignal.*," //eservicesignal.* is required because we will manually call TableToList() later.
					+"(SELECT COUNT(*) FROM eservicesignal WHERE Severity="+POut.Int((int)eServiceSignalSeverity.Error)+" AND IsProcessed=0) PendingErrors, "
					+DbHelper.Now()+" ServerTime "
				+"FROM eservicesignal WHERE ServiceCode="+POut.Int((int)eServiceCode.ListenerService)+" "
				+"AND Severity IN("+POut.Int((int)eServiceSignalSeverity.NotEnabled)+","
					+POut.Int((int)eServiceSignalSeverity.Working)+","
					+POut.Int((int)eServiceSignalSeverity.Error)+","
					+POut.Int((int)eServiceSignalSeverity.Critical)+") "
				+"ORDER BY SigDateTime DESC, Severity DESC ";
			command=DbHelper.LimitOrderBy(command,1);
			DataTable table=Database.ExecuteDataTable(command);
			List<EServiceSignal> listSignal=Crud.EServiceSignalCrud.TableToList(table);
			if(listSignal.Count==0) { //No signals means the eConnector has never run. Nothing to report.				
				return eServiceSignalSeverity.None;
			}
			if(listSignal[0].Severity==eServiceSignalSeverity.NotEnabled) { //NotEnabled means they don't care what the status is. Nothing to report.
				return eServiceSignalSeverity.NotEnabled;
			}
			DateTime dtNow=PIn.Date(table.Rows[0]["ServerTime"].ToString());
			if(
				//eConnector exited gracefully and inserted its own critical signal.
				listSignal[0].Severity==eServiceSignalSeverity.Critical ||
				//eConnector did not exit gracefully but has not inserted a heartbeat in at least 6 minutes. It is considered critical.
				//Listener is dropping a heartbeat every 5 minutes, so give 1 minute grace period to squelch race condition.
				listSignal[0].SigDateTime<dtNow.AddMinutes(-6))
			{
				return eServiceSignalSeverity.Critical;
			}
			//We need to flag the service monitor as Error if there are ANY pending errors.
			if(table.Rows[0]["PendingErrors"].ToString()!="0") {
				return eServiceSignalSeverity.Error;
			}
			return listSignal[0].Severity;
		}

		///<summary>Also inserts an EConnectorError Alert where applicable.</summary>
		public static long Insert(EServiceSignal eServiceSignal) {
			
			//If this is an error and the EConnectorError alert is not already present, create it now.
			if(eServiceSignal.Severity==eServiceSignalSeverity.Error && AlertItems.RefreshForType(AlertType.EConnectorError).Count()==0) {
				//Create an alert.
				AlertItems.Insert(new AlertItem {
					//Do not allow delete. The only way for this alert to be deleted is to open the eConnector form and ACK the error(s).
					Actions=AlertAction.MarkAsRead | AlertAction.OpenForm,
					Description="eConnector has posted an error.",
					Severity=AlertSeverityType.Low,
					Type=AlertType.EConnectorError,
					FormToOpen=FormType.FormEServicesEConnector,
				});
			}
			return Crud.EServiceSignalCrud.Insert(eServiceSignal);
		}

		///<summary>Inserts a healthy heartbeat.</summary>
		public static void InsertHeartbeatForService(eServiceCode serviceCode) {
			
			AlertItems.DeleteFor(AlertType.EConnectorDown);
			string command="SELECT * FROM eservicesignal WHERE ServiceCode="+POut.Int((int)serviceCode)
				+" AND Severity IN ("
				+POut.Int((int)eServiceSignalSeverity.NotEnabled)+","
				+POut.Int((int)eServiceSignalSeverity.Working)+","
				+POut.Int((int)eServiceSignalSeverity.Critical)
				+") ORDER BY SigDateTime DESC LIMIT 1";//only select not enabled, working, and critical statuses.
			EServiceSignal eServiceSignalLast=Crud.EServiceSignalCrud.SelectOne(command);
			DateTime dtNow=MiscData.GetNowDateTime();
			//If initializing or changing state to working from not working, insert two signals; An anchor and a rolling timestamp.
			if(eServiceSignalLast==null || eServiceSignalLast.Severity!=eServiceSignalSeverity.Working) { //First ever heartbeat or critical which was not previously critical.
				if(eServiceSignalLast!=null && eServiceSignalLast.Severity==eServiceSignalSeverity.Critical
					//Do not create a signal if the eConnector was stopped because of an update
					&& (eServiceSignalLast.SigDateTime > UpdateHistories.GetLastUpdateHistory().InstalledOn
						|| UpdateHistories.GetLastUpdateHistory().InstalledOn.AddMinutes(10) < dtNow))
				{ 
					//Changing from critical to working so alert user that this change took place and tell them how long we were in critical state.
					//Insert() will also insert Alert.
					Insert(new EServiceSignal() { ServiceCode=(int)serviceCode,Severity=eServiceSignalSeverity.Error,SigDateTime=dtNow,IsProcessed=false,Description="Listener was critical for "+DateTime.Now.Subtract(eServiceSignalLast.SigDateTime).ToStringHmm() });
				}
				Insert(new EServiceSignal() { ServiceCode=(int)serviceCode,Severity=eServiceSignalSeverity.Working,SigDateTime=dtNow,IsProcessed=true,Description="Heartbeat Anchor" });//anchor heartbeat
				Insert(new EServiceSignal() { ServiceCode=(int)serviceCode,Severity=eServiceSignalSeverity.Working,SigDateTime=dtNow.AddSeconds(1),IsProcessed=true,Description="Heartbeat" });//rolling heartbeat
				return;
			}
			eServiceSignalLast.SigDateTime=dtNow;
			Update(eServiceSignalLast);
		}

		///<summary></summary>
		public static void Update(EServiceSignal eServiceSignal) {
			
			Crud.EServiceSignalCrud.Update(eServiceSignal);
		}

		///<summary>Sets IsProcessed to true on all eService signals of the passed in severity.</summary>
		public static void ProcessSignalsForSeverity(eServiceSignalSeverity severity) {
			
			string command="UPDATE eservicesignal SET IsProcessed=1 WHERE Severity="+POut.Int((int)severity);
			Database.ExecuteNonQuery(command);
			if(severity==eServiceSignalSeverity.Error) { //Delete corresponding alert.
				AlertItems.DeleteFor(AlertType.EConnectorError);
			}
		}

		///<summary>Sets IsProcessed to true on eService signals of Error severity that are within 15 minutes of the passed in DateTime.</summary>
		public static void ProcessErrorSignalsAroundTime(DateTime dateTime) {
			
			if(dateTime.Year<1880) {
				return;//Nothing to do.
			}
			string command="UPDATE eservicesignal SET IsProcessed=1 "
				+"WHERE Severity="+POut.Int((int)eServiceSignalSeverity.Error)+" "
				+"AND SigDateTime BETWEEN "+POut.DateT(dateTime.AddMinutes(-15))+" AND "+POut.DateT(dateTime.AddMinutes(15));
			Database.ExecuteNonQuery(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<EServiceSignal> Refresh(long patNum){
			
			string command="SELECT * FROM eservicesignal WHERE PatNum = "+POut.Long(patNum);
			return Crud.EServiceSignalCrud.SelectMany(command);
		}

		///<summary>Gets one EServiceSignal from the db.</summary>
		public static EServiceSignal GetOne(long eServiceSignalNum){
			
			return Crud.EServiceSignalCrud.SelectOne(eServiceSignalNum);
		}

		///<summary></summary>
		public static long Insert(EServiceSignal eServiceSignal){
			
			return Crud.EServiceSignalCrud.Insert(eServiceSignal);
		}

		///<summary></summary>
		public static void Update(EServiceSignal eServiceSignal){
			
			Crud.EServiceSignalCrud.Update(eServiceSignal);
		}

		///<summary></summary>
		public static void Delete(long eServiceSignalNum) {
			
			string command= "DELETE FROM eservicesignal WHERE EServiceSignalNum = "+POut.Long(eServiceSignalNum);
			Db.ExecuteNonQuery(command);
		}
		*/



	}
}