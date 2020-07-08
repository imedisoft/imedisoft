//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class RxPatCrud {
		///<summary>Gets one RxPat object from the database using the primary key.  Returns null if not found.</summary>
		public static RxPat SelectOne(long rxNum) {
			string command="SELECT * FROM rxpat "
				+"WHERE RxNum = "+POut.Long(rxNum);
			List<RxPat> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one RxPat object from the database using a query.</summary>
		public static RxPat SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<RxPat> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of RxPat objects from the database using a query.</summary>
		public static List<RxPat> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<RxPat> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<RxPat> TableToList(DataTable table) {
			List<RxPat> retVal=new List<RxPat>();
			RxPat rxPat;
			foreach(DataRow row in table.Rows) {
				rxPat=new RxPat();
				rxPat.RxNum             = PIn.Long  (row["RxNum"].ToString());
				rxPat.PatNum            = PIn.Long  (row["PatNum"].ToString());
				rxPat.RxDate            = PIn.Date  (row["RxDate"].ToString());
				rxPat.Drug              = PIn.String(row["Drug"].ToString());
				rxPat.Sig               = PIn.String(row["Sig"].ToString());
				rxPat.Disp              = PIn.String(row["Disp"].ToString());
				rxPat.Refills           = PIn.String(row["Refills"].ToString());
				rxPat.ProvNum           = PIn.Long  (row["ProvNum"].ToString());
				rxPat.Notes             = PIn.String(row["Notes"].ToString());
				rxPat.PharmacyNum       = PIn.Long  (row["PharmacyNum"].ToString());
				rxPat.IsControlled      = PIn.Bool  (row["IsControlled"].ToString());
				rxPat.DateTStamp        = PIn.DateT (row["DateTStamp"].ToString());
				rxPat.SendStatus        = (OpenDentBusiness.RxSendStatus)PIn.Int(row["SendStatus"].ToString());
				rxPat.RxCui             = PIn.Long  (row["RxCui"].ToString());
				rxPat.DosageCode        = PIn.String(row["DosageCode"].ToString());
				rxPat.ErxGuid           = PIn.String(row["ErxGuid"].ToString());
				rxPat.IsErxOld          = PIn.Bool  (row["IsErxOld"].ToString());
				rxPat.ErxPharmacyInfo   = PIn.String(row["ErxPharmacyInfo"].ToString());
				rxPat.IsProcRequired    = PIn.Bool  (row["IsProcRequired"].ToString());
				rxPat.ProcNum           = PIn.Long  (row["ProcNum"].ToString());
				rxPat.DaysOfSupply      = PIn.Double(row["DaysOfSupply"].ToString());
				rxPat.PatientInstruction= PIn.String(row["PatientInstruction"].ToString());
				rxPat.ClinicNum         = PIn.Long  (row["ClinicNum"].ToString());
				retVal.Add(rxPat);
			}
			return retVal;
		}

		///<summary>Converts a list of RxPat into a DataTable.</summary>
		public static DataTable ListToTable(List<RxPat> listRxPats,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="RxPat";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("RxNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("RxDate");
			table.Columns.Add("Drug");
			table.Columns.Add("Sig");
			table.Columns.Add("Disp");
			table.Columns.Add("Refills");
			table.Columns.Add("ProvNum");
			table.Columns.Add("Notes");
			table.Columns.Add("PharmacyNum");
			table.Columns.Add("IsControlled");
			table.Columns.Add("DateTStamp");
			table.Columns.Add("SendStatus");
			table.Columns.Add("RxCui");
			table.Columns.Add("DosageCode");
			table.Columns.Add("ErxGuid");
			table.Columns.Add("IsErxOld");
			table.Columns.Add("ErxPharmacyInfo");
			table.Columns.Add("IsProcRequired");
			table.Columns.Add("ProcNum");
			table.Columns.Add("DaysOfSupply");
			table.Columns.Add("PatientInstruction");
			table.Columns.Add("ClinicNum");
			foreach(RxPat rxPat in listRxPats) {
				table.Rows.Add(new object[] {
					POut.Long  (rxPat.RxNum),
					POut.Long  (rxPat.PatNum),
					POut.DateT (rxPat.RxDate,false),
					            rxPat.Drug,
					            rxPat.Sig,
					            rxPat.Disp,
					            rxPat.Refills,
					POut.Long  (rxPat.ProvNum),
					            rxPat.Notes,
					POut.Long  (rxPat.PharmacyNum),
					POut.Bool  (rxPat.IsControlled),
					POut.DateT (rxPat.DateTStamp,false),
					POut.Int   ((int)rxPat.SendStatus),
					POut.Long  (rxPat.RxCui),
					            rxPat.DosageCode,
					            rxPat.ErxGuid,
					POut.Bool  (rxPat.IsErxOld),
					            rxPat.ErxPharmacyInfo,
					POut.Bool  (rxPat.IsProcRequired),
					POut.Long  (rxPat.ProcNum),
					POut.Double(rxPat.DaysOfSupply),
					            rxPat.PatientInstruction,
					POut.Long  (rxPat.ClinicNum),
				});
			}
			return table;
		}

		///<summary>Inserts one RxPat into the database.  Returns the new priKey.</summary>
		public static long Insert(RxPat rxPat) {
			return Insert(rxPat,false);
		}

		///<summary>Inserts one RxPat into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(RxPat rxPat,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				rxPat.RxNum=ReplicationServers.GetKey("rxpat","RxNum");
			}
			string command="INSERT INTO rxpat (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="RxNum,";
			}
			command+="PatNum,RxDate,Drug,Sig,Disp,Refills,ProvNum,Notes,PharmacyNum,IsControlled,SendStatus,RxCui,DosageCode,ErxGuid,IsErxOld,ErxPharmacyInfo,IsProcRequired,ProcNum,DaysOfSupply,PatientInstruction,ClinicNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(rxPat.RxNum)+",";
			}
			command+=
				     POut.Long  (rxPat.PatNum)+","
				+    POut.Date  (rxPat.RxDate)+","
				+"'"+POut.String(rxPat.Drug)+"',"
				+"'"+POut.String(rxPat.Sig)+"',"
				+"'"+POut.String(rxPat.Disp)+"',"
				+"'"+POut.String(rxPat.Refills)+"',"
				+    POut.Long  (rxPat.ProvNum)+","
				+"'"+POut.String(rxPat.Notes)+"',"
				+    POut.Long  (rxPat.PharmacyNum)+","
				+    POut.Bool  (rxPat.IsControlled)+","
				//DateTStamp can only be set by MySQL
				+    POut.Int   ((int)rxPat.SendStatus)+","
				+    POut.Long  (rxPat.RxCui)+","
				+"'"+POut.String(rxPat.DosageCode)+"',"
				+"'"+POut.String(rxPat.ErxGuid)+"',"
				+    POut.Bool  (rxPat.IsErxOld)+","
				+"'"+POut.String(rxPat.ErxPharmacyInfo)+"',"
				+    POut.Bool  (rxPat.IsProcRequired)+","
				+    POut.Long  (rxPat.ProcNum)+","
				+"'"+POut.Double(rxPat.DaysOfSupply)+"',"
				+    DbHelper.ParamChar+"paramPatientInstruction,"
				+    POut.Long  (rxPat.ClinicNum)+")";
			if(rxPat.PatientInstruction==null) {
				rxPat.PatientInstruction="";
			}
			OdSqlParameter paramPatientInstruction=new OdSqlParameter("paramPatientInstruction",OdDbType.Text,POut.StringParam(rxPat.PatientInstruction));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramPatientInstruction);
			}
			else {
				rxPat.RxNum=Db.NonQ(command,true,"RxNum","rxPat",paramPatientInstruction);
			}
			return rxPat.RxNum;
		}

		///<summary>Inserts one RxPat into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(RxPat rxPat) {
			return InsertNoCache(rxPat,false);
		}

		///<summary>Inserts one RxPat into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(RxPat rxPat,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO rxpat (";
			if(!useExistingPK && isRandomKeys) {
				rxPat.RxNum=ReplicationServers.GetKeyNoCache("rxpat","RxNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="RxNum,";
			}
			command+="PatNum,RxDate,Drug,Sig,Disp,Refills,ProvNum,Notes,PharmacyNum,IsControlled,SendStatus,RxCui,DosageCode,ErxGuid,IsErxOld,ErxPharmacyInfo,IsProcRequired,ProcNum,DaysOfSupply,PatientInstruction,ClinicNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(rxPat.RxNum)+",";
			}
			command+=
				     POut.Long  (rxPat.PatNum)+","
				+    POut.Date  (rxPat.RxDate)+","
				+"'"+POut.String(rxPat.Drug)+"',"
				+"'"+POut.String(rxPat.Sig)+"',"
				+"'"+POut.String(rxPat.Disp)+"',"
				+"'"+POut.String(rxPat.Refills)+"',"
				+    POut.Long  (rxPat.ProvNum)+","
				+"'"+POut.String(rxPat.Notes)+"',"
				+    POut.Long  (rxPat.PharmacyNum)+","
				+    POut.Bool  (rxPat.IsControlled)+","
				//DateTStamp can only be set by MySQL
				+    POut.Int   ((int)rxPat.SendStatus)+","
				+    POut.Long  (rxPat.RxCui)+","
				+"'"+POut.String(rxPat.DosageCode)+"',"
				+"'"+POut.String(rxPat.ErxGuid)+"',"
				+    POut.Bool  (rxPat.IsErxOld)+","
				+"'"+POut.String(rxPat.ErxPharmacyInfo)+"',"
				+    POut.Bool  (rxPat.IsProcRequired)+","
				+    POut.Long  (rxPat.ProcNum)+","
				+"'"+POut.Double(rxPat.DaysOfSupply)+"',"
				+    DbHelper.ParamChar+"paramPatientInstruction,"
				+    POut.Long  (rxPat.ClinicNum)+")";
			if(rxPat.PatientInstruction==null) {
				rxPat.PatientInstruction="";
			}
			OdSqlParameter paramPatientInstruction=new OdSqlParameter("paramPatientInstruction",OdDbType.Text,POut.StringParam(rxPat.PatientInstruction));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramPatientInstruction);
			}
			else {
				rxPat.RxNum=Db.NonQ(command,true,"RxNum","rxPat",paramPatientInstruction);
			}
			return rxPat.RxNum;
		}

		///<summary>Updates one RxPat in the database.</summary>
		public static void Update(RxPat rxPat) {
			string command="UPDATE rxpat SET "
				+"PatNum            =  "+POut.Long  (rxPat.PatNum)+", "
				+"RxDate            =  "+POut.Date  (rxPat.RxDate)+", "
				+"Drug              = '"+POut.String(rxPat.Drug)+"', "
				+"Sig               = '"+POut.String(rxPat.Sig)+"', "
				+"Disp              = '"+POut.String(rxPat.Disp)+"', "
				+"Refills           = '"+POut.String(rxPat.Refills)+"', "
				+"ProvNum           =  "+POut.Long  (rxPat.ProvNum)+", "
				+"Notes             = '"+POut.String(rxPat.Notes)+"', "
				+"PharmacyNum       =  "+POut.Long  (rxPat.PharmacyNum)+", "
				+"IsControlled      =  "+POut.Bool  (rxPat.IsControlled)+", "
				//DateTStamp can only be set by MySQL
				+"SendStatus        =  "+POut.Int   ((int)rxPat.SendStatus)+", "
				+"RxCui             =  "+POut.Long  (rxPat.RxCui)+", "
				+"DosageCode        = '"+POut.String(rxPat.DosageCode)+"', "
				+"ErxGuid           = '"+POut.String(rxPat.ErxGuid)+"', "
				+"IsErxOld          =  "+POut.Bool  (rxPat.IsErxOld)+", "
				+"ErxPharmacyInfo   = '"+POut.String(rxPat.ErxPharmacyInfo)+"', "
				+"IsProcRequired    =  "+POut.Bool  (rxPat.IsProcRequired)+", "
				+"ProcNum           =  "+POut.Long  (rxPat.ProcNum)+", "
				+"DaysOfSupply      = '"+POut.Double(rxPat.DaysOfSupply)+"', "
				+"PatientInstruction=  "+DbHelper.ParamChar+"paramPatientInstruction, "
				+"ClinicNum         =  "+POut.Long  (rxPat.ClinicNum)+" "
				+"WHERE RxNum = "+POut.Long(rxPat.RxNum);
			if(rxPat.PatientInstruction==null) {
				rxPat.PatientInstruction="";
			}
			OdSqlParameter paramPatientInstruction=new OdSqlParameter("paramPatientInstruction",OdDbType.Text,POut.StringParam(rxPat.PatientInstruction));
			Db.NonQ(command,paramPatientInstruction);
		}

		///<summary>Updates one RxPat in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(RxPat rxPat,RxPat oldRxPat) {
			string command="";
			if(rxPat.PatNum != oldRxPat.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(rxPat.PatNum)+"";
			}
			if(rxPat.RxDate.Date != oldRxPat.RxDate.Date) {
				if(command!="") { command+=",";}
				command+="RxDate = "+POut.Date(rxPat.RxDate)+"";
			}
			if(rxPat.Drug != oldRxPat.Drug) {
				if(command!="") { command+=",";}
				command+="Drug = '"+POut.String(rxPat.Drug)+"'";
			}
			if(rxPat.Sig != oldRxPat.Sig) {
				if(command!="") { command+=",";}
				command+="Sig = '"+POut.String(rxPat.Sig)+"'";
			}
			if(rxPat.Disp != oldRxPat.Disp) {
				if(command!="") { command+=",";}
				command+="Disp = '"+POut.String(rxPat.Disp)+"'";
			}
			if(rxPat.Refills != oldRxPat.Refills) {
				if(command!="") { command+=",";}
				command+="Refills = '"+POut.String(rxPat.Refills)+"'";
			}
			if(rxPat.ProvNum != oldRxPat.ProvNum) {
				if(command!="") { command+=",";}
				command+="ProvNum = "+POut.Long(rxPat.ProvNum)+"";
			}
			if(rxPat.Notes != oldRxPat.Notes) {
				if(command!="") { command+=",";}
				command+="Notes = '"+POut.String(rxPat.Notes)+"'";
			}
			if(rxPat.PharmacyNum != oldRxPat.PharmacyNum) {
				if(command!="") { command+=",";}
				command+="PharmacyNum = "+POut.Long(rxPat.PharmacyNum)+"";
			}
			if(rxPat.IsControlled != oldRxPat.IsControlled) {
				if(command!="") { command+=",";}
				command+="IsControlled = "+POut.Bool(rxPat.IsControlled)+"";
			}
			//DateTStamp can only be set by MySQL
			if(rxPat.SendStatus != oldRxPat.SendStatus) {
				if(command!="") { command+=",";}
				command+="SendStatus = "+POut.Int   ((int)rxPat.SendStatus)+"";
			}
			if(rxPat.RxCui != oldRxPat.RxCui) {
				if(command!="") { command+=",";}
				command+="RxCui = "+POut.Long(rxPat.RxCui)+"";
			}
			if(rxPat.DosageCode != oldRxPat.DosageCode) {
				if(command!="") { command+=",";}
				command+="DosageCode = '"+POut.String(rxPat.DosageCode)+"'";
			}
			if(rxPat.ErxGuid != oldRxPat.ErxGuid) {
				if(command!="") { command+=",";}
				command+="ErxGuid = '"+POut.String(rxPat.ErxGuid)+"'";
			}
			if(rxPat.IsErxOld != oldRxPat.IsErxOld) {
				if(command!="") { command+=",";}
				command+="IsErxOld = "+POut.Bool(rxPat.IsErxOld)+"";
			}
			if(rxPat.ErxPharmacyInfo != oldRxPat.ErxPharmacyInfo) {
				if(command!="") { command+=",";}
				command+="ErxPharmacyInfo = '"+POut.String(rxPat.ErxPharmacyInfo)+"'";
			}
			if(rxPat.IsProcRequired != oldRxPat.IsProcRequired) {
				if(command!="") { command+=",";}
				command+="IsProcRequired = "+POut.Bool(rxPat.IsProcRequired)+"";
			}
			if(rxPat.ProcNum != oldRxPat.ProcNum) {
				if(command!="") { command+=",";}
				command+="ProcNum = "+POut.Long(rxPat.ProcNum)+"";
			}
			if(rxPat.DaysOfSupply != oldRxPat.DaysOfSupply) {
				if(command!="") { command+=",";}
				command+="DaysOfSupply = '"+POut.Double(rxPat.DaysOfSupply)+"'";
			}
			if(rxPat.PatientInstruction != oldRxPat.PatientInstruction) {
				if(command!="") { command+=",";}
				command+="PatientInstruction = "+DbHelper.ParamChar+"paramPatientInstruction";
			}
			if(rxPat.ClinicNum != oldRxPat.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(rxPat.ClinicNum)+"";
			}
			if(command=="") {
				return false;
			}
			if(rxPat.PatientInstruction==null) {
				rxPat.PatientInstruction="";
			}
			OdSqlParameter paramPatientInstruction=new OdSqlParameter("paramPatientInstruction",OdDbType.Text,POut.StringParam(rxPat.PatientInstruction));
			command="UPDATE rxpat SET "+command
				+" WHERE RxNum = "+POut.Long(rxPat.RxNum);
			Db.NonQ(command,paramPatientInstruction);
			return true;
		}

		///<summary>Returns true if Update(RxPat,RxPat) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(RxPat rxPat,RxPat oldRxPat) {
			if(rxPat.PatNum != oldRxPat.PatNum) {
				return true;
			}
			if(rxPat.RxDate.Date != oldRxPat.RxDate.Date) {
				return true;
			}
			if(rxPat.Drug != oldRxPat.Drug) {
				return true;
			}
			if(rxPat.Sig != oldRxPat.Sig) {
				return true;
			}
			if(rxPat.Disp != oldRxPat.Disp) {
				return true;
			}
			if(rxPat.Refills != oldRxPat.Refills) {
				return true;
			}
			if(rxPat.ProvNum != oldRxPat.ProvNum) {
				return true;
			}
			if(rxPat.Notes != oldRxPat.Notes) {
				return true;
			}
			if(rxPat.PharmacyNum != oldRxPat.PharmacyNum) {
				return true;
			}
			if(rxPat.IsControlled != oldRxPat.IsControlled) {
				return true;
			}
			//DateTStamp can only be set by MySQL
			if(rxPat.SendStatus != oldRxPat.SendStatus) {
				return true;
			}
			if(rxPat.RxCui != oldRxPat.RxCui) {
				return true;
			}
			if(rxPat.DosageCode != oldRxPat.DosageCode) {
				return true;
			}
			if(rxPat.ErxGuid != oldRxPat.ErxGuid) {
				return true;
			}
			if(rxPat.IsErxOld != oldRxPat.IsErxOld) {
				return true;
			}
			if(rxPat.ErxPharmacyInfo != oldRxPat.ErxPharmacyInfo) {
				return true;
			}
			if(rxPat.IsProcRequired != oldRxPat.IsProcRequired) {
				return true;
			}
			if(rxPat.ProcNum != oldRxPat.ProcNum) {
				return true;
			}
			if(rxPat.DaysOfSupply != oldRxPat.DaysOfSupply) {
				return true;
			}
			if(rxPat.PatientInstruction != oldRxPat.PatientInstruction) {
				return true;
			}
			if(rxPat.ClinicNum != oldRxPat.ClinicNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one RxPat from the database.</summary>
		public static void Delete(long rxNum) {
			ClearFkey(rxNum);
			string command="DELETE FROM rxpat "
				+"WHERE RxNum = "+POut.Long(rxNum);
			Db.NonQ(command);
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching rxNum as FKey and are related to RxPat.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the RxPat table type.</summary>
		public static void ClearFkey(long rxNum) {
			if(rxNum==0) {
				return;
			}
			string command="UPDATE securitylog SET FKey=0 WHERE FKey="+POut.Long(rxNum)+" AND PermType IN (9,76)";
			Db.NonQ(command);
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching rxNums as FKey and are related to RxPat.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the RxPat table type.</summary>
		public static void ClearFkey(List<long> listRxNums) {
			if(listRxNums==null || listRxNums.FindAll(x => x != 0).Count==0) {
				return;
			}
			string command="UPDATE securitylog SET FKey=0 WHERE FKey IN("+String.Join(",",listRxNums.FindAll(x => x != 0))+") AND PermType IN (9,76)";
			Db.NonQ(command);
		}

	}
}