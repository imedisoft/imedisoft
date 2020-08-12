using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;
using Renci.SshNet.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Computers {

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


		#region CachePattern

		private class ComputerCache : CacheListAbs<Computer> {
			protected override List<Computer> GetCacheFromDb() {
				string command="SELECT * FROM computer ORDER BY CompName";
				return Crud.ComputerCrud.SelectMany(command);
			}
			protected override List<Computer> TableToList(DataTable table) {
				return Crud.ComputerCrud.TableToList(table);
			}
			protected override Computer Copy(Computer computer) {
				return computer.Copy();
			}
			protected override DataTable ListToTable(List<Computer> listComputers) {
				return Crud.ComputerCrud.ListToTable(listComputers,"Computer");
			}
			protected override void FillCacheIfNeeded() {
				Computers.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static ComputerCache _computerCache=new ComputerCache();

		public static List<Computer> GetDeepCopy(bool isShort=false) {
			return _computerCache.GetDeepCopy(isShort);
		}

		public static Computer GetFirstOrDefault(Func<Computer,bool> match,bool isShort=false) {
			return _computerCache.GetFirstOrDefault(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_computerCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			//It is important to call EnsureComputerInDB prior to the remoting role check.
			EnsureComputerInDB(Environment.MachineName);
			
			return _computerCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		public static void EnsureComputerInDB(string computerName){
			
			string command=
				"SELECT * from computer "
				+"WHERE compname = '"+computerName+"'";
			DataTable table=Database.ExecuteDataTable(command);
			if(table.Rows.Count==0) {
				Computer Cur=new Computer();
				Cur.MachineName=computerName;
				Computers.Insert(Cur);
			}
		}

		///<summary>ONLY use this if compname is not already present</summary>
		public static long Insert(Computer comp) {
			
			return Crud.ComputerCrud.Insert(comp);
		}

		public static void Delete(Computer comp){
			
			string command= "DELETE FROM computer WHERE computernum = '"+comp.Id.ToString()+"'";
 			Database.ExecuteNonQuery(command);
		}

		///<summary>Only called from Printers.GetForSit</summary>
		public static Computer GetCur(){
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => x.MachineName.ToUpper()==Environment.MachineName.ToUpper());
		}

		///<summary>Returns all computers with an active heart beat.  A heart beat less than 4 minutes old is considered active.</summary>
		public static List<Computer> GetRunningComputers() {
			
			//heartbeat is every three minutes.  We'll allow four to be generous.
			string command="SELECT * FROM computer WHERE LastHeartBeat > SUBTIME(NOW(),'00:04:00')";
			return Crud.ComputerCrud.SelectMany(command);
		}

		/// <summary>When starting up, in an attempt to be fast, it will not add a new computer to the list.</summary>
		public static void UpdateHeartBeat(string computerName,bool isStartup) {
			
			string command;
			if(!isStartup) {
				if(_computerCache.ListIsNull()) {
					RefreshCache();//adds new computer to list
				}
				command="SELECT LastHeartBeat<"+DbHelper.DateAddMinute(DbHelper.Now(),"-3")+" FROM computer WHERE CompName='"+POut.String(computerName)+"'";
				if(!PIn.Bool(Database.ExecuteString(command))) {//no need to update if LastHeartBeat is already within the last 3 mins
					return;//remote app servers with multiple connections would fight over the lock on a single row to update the heartbeat unnecessarily
				}
			}
			command="UPDATE computer SET LastHeartBeat="+DbHelper.Now()+" WHERE CompName = '"+POut.String(computerName)+"'";
			Database.ExecuteNonQuery(command);
		}

		public static void ClearHeartBeat(string computerName) {
			
			string command= "UPDATE computer SET LastHeartBeat="+POut.Date(new DateTime(0001,1,1),true)+" WHERE CompName = '"+POut.String(computerName)+"'";
			Database.ExecuteNonQuery(command);
		}

		public static void ClearAllHeartBeats(string machineNameException) {
			
			string command= "UPDATE computer SET LastHeartBeat="+POut.Date(new DateTime(0001,1,1),true)+" "
				+"WHERE CompName != '"+POut.String(machineNameException)+"'";
			Database.ExecuteNonQuery(command);
		}

		/// <summary>
		/// Gets details of the database service.
		///</summary>
		public static (string name, string comment, string hostname, string version) GetServiceInfo() 
			=> (Database.ExecuteString("SELECT @@socket"),
				Database.ExecuteString("SELECT @@version_comment"),
				Database.ExecuteString("SELECT @@hostname"),
				Database.ExecuteString("SELECT @@version"));

	}
}