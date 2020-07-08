//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EServiceSignalCrud {
		///<summary>Gets one EServiceSignal object from the database using the primary key.  Returns null if not found.</summary>
		public static EServiceSignal SelectOne(long eServiceSignalNum) {
			string command="SELECT * FROM eservicesignal "
				+"WHERE EServiceSignalNum = "+POut.Long(eServiceSignalNum);
			List<EServiceSignal> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EServiceSignal object from the database using a query.</summary>
		public static EServiceSignal SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EServiceSignal> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EServiceSignal objects from the database using a query.</summary>
		public static List<EServiceSignal> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EServiceSignal> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EServiceSignal> TableToList(DataTable table) {
			List<EServiceSignal> retVal=new List<EServiceSignal>();
			EServiceSignal eServiceSignal;
			foreach(DataRow row in table.Rows) {
				eServiceSignal=new EServiceSignal();
				eServiceSignal.EServiceSignalNum= PIn.Long  (row["EServiceSignalNum"].ToString());
				eServiceSignal.ServiceCode      = PIn.Int   (row["ServiceCode"].ToString());
				eServiceSignal.ReasonCategory   = PIn.Int   (row["ReasonCategory"].ToString());
				eServiceSignal.ReasonCode       = PIn.Int   (row["ReasonCode"].ToString());
				eServiceSignal.Severity         = (OpenDentBusiness.eServiceSignalSeverity)PIn.Int(row["Severity"].ToString());
				eServiceSignal.Description      = PIn.String(row["Description"].ToString());
				eServiceSignal.SigDateTime      = PIn.DateT (row["SigDateTime"].ToString());
				eServiceSignal.Tag              = PIn.String(row["Tag"].ToString());
				eServiceSignal.IsProcessed      = PIn.Bool  (row["IsProcessed"].ToString());
				retVal.Add(eServiceSignal);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceSignal into a DataTable.</summary>
		public static DataTable ListToTable(List<EServiceSignal> listEServiceSignals,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EServiceSignal";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EServiceSignalNum");
			table.Columns.Add("ServiceCode");
			table.Columns.Add("ReasonCategory");
			table.Columns.Add("ReasonCode");
			table.Columns.Add("Severity");
			table.Columns.Add("Description");
			table.Columns.Add("SigDateTime");
			table.Columns.Add("Tag");
			table.Columns.Add("IsProcessed");
			foreach(EServiceSignal eServiceSignal in listEServiceSignals) {
				table.Rows.Add(new object[] {
					POut.Long  (eServiceSignal.EServiceSignalNum),
					POut.Int   (eServiceSignal.ServiceCode),
					POut.Int   (eServiceSignal.ReasonCategory),
					POut.Int   (eServiceSignal.ReasonCode),
					POut.Int   ((int)eServiceSignal.Severity),
					            eServiceSignal.Description,
					POut.DateT (eServiceSignal.SigDateTime,false),
					            eServiceSignal.Tag,
					POut.Bool  (eServiceSignal.IsProcessed),
				});
			}
			return table;
		}

		///<summary>Inserts one EServiceSignal into the database.  Returns the new priKey.</summary>
		public static long Insert(EServiceSignal eServiceSignal) {
			return Insert(eServiceSignal,false);
		}

		///<summary>Inserts one EServiceSignal into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EServiceSignal eServiceSignal,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				eServiceSignal.EServiceSignalNum=ReplicationServers.GetKey("eservicesignal","EServiceSignalNum");
			}
			string command="INSERT INTO eservicesignal (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EServiceSignalNum,";
			}
			command+="ServiceCode,ReasonCategory,ReasonCode,Severity,Description,SigDateTime,Tag,IsProcessed) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(eServiceSignal.EServiceSignalNum)+",";
			}
			command+=
				     POut.Int   (eServiceSignal.ServiceCode)+","
				+    POut.Int   (eServiceSignal.ReasonCategory)+","
				+    POut.Int   (eServiceSignal.ReasonCode)+","
				+    POut.Int   ((int)eServiceSignal.Severity)+","
				+    DbHelper.ParamChar+"paramDescription,"
				+    POut.DateT (eServiceSignal.SigDateTime)+","
				+    DbHelper.ParamChar+"paramTag,"
				+    POut.Bool  (eServiceSignal.IsProcessed)+")";
			if(eServiceSignal.Description==null) {
				eServiceSignal.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(eServiceSignal.Description));
			if(eServiceSignal.Tag==null) {
				eServiceSignal.Tag="";
			}
			OdSqlParameter paramTag=new OdSqlParameter("paramTag",OdDbType.Text,POut.StringParam(eServiceSignal.Tag));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramDescription,paramTag);
			}
			else {
				eServiceSignal.EServiceSignalNum=Db.NonQ(command,true,"EServiceSignalNum","eServiceSignal",paramDescription,paramTag);
			}
			return eServiceSignal.EServiceSignalNum;
		}

		///<summary>Inserts one EServiceSignal into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EServiceSignal eServiceSignal) {
			return InsertNoCache(eServiceSignal,false);
		}

		///<summary>Inserts one EServiceSignal into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EServiceSignal eServiceSignal,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO eservicesignal (";
			if(!useExistingPK && isRandomKeys) {
				eServiceSignal.EServiceSignalNum=ReplicationServers.GetKeyNoCache("eservicesignal","EServiceSignalNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EServiceSignalNum,";
			}
			command+="ServiceCode,ReasonCategory,ReasonCode,Severity,Description,SigDateTime,Tag,IsProcessed) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(eServiceSignal.EServiceSignalNum)+",";
			}
			command+=
				     POut.Int   (eServiceSignal.ServiceCode)+","
				+    POut.Int   (eServiceSignal.ReasonCategory)+","
				+    POut.Int   (eServiceSignal.ReasonCode)+","
				+    POut.Int   ((int)eServiceSignal.Severity)+","
				+    DbHelper.ParamChar+"paramDescription,"
				+    POut.DateT (eServiceSignal.SigDateTime)+","
				+    DbHelper.ParamChar+"paramTag,"
				+    POut.Bool  (eServiceSignal.IsProcessed)+")";
			if(eServiceSignal.Description==null) {
				eServiceSignal.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(eServiceSignal.Description));
			if(eServiceSignal.Tag==null) {
				eServiceSignal.Tag="";
			}
			OdSqlParameter paramTag=new OdSqlParameter("paramTag",OdDbType.Text,POut.StringParam(eServiceSignal.Tag));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramDescription,paramTag);
			}
			else {
				eServiceSignal.EServiceSignalNum=Db.NonQ(command,true,"EServiceSignalNum","eServiceSignal",paramDescription,paramTag);
			}
			return eServiceSignal.EServiceSignalNum;
		}

		///<summary>Updates one EServiceSignal in the database.</summary>
		public static void Update(EServiceSignal eServiceSignal) {
			string command="UPDATE eservicesignal SET "
				+"ServiceCode      =  "+POut.Int   (eServiceSignal.ServiceCode)+", "
				+"ReasonCategory   =  "+POut.Int   (eServiceSignal.ReasonCategory)+", "
				+"ReasonCode       =  "+POut.Int   (eServiceSignal.ReasonCode)+", "
				+"Severity         =  "+POut.Int   ((int)eServiceSignal.Severity)+", "
				+"Description      =  "+DbHelper.ParamChar+"paramDescription, "
				+"SigDateTime      =  "+POut.DateT (eServiceSignal.SigDateTime)+", "
				+"Tag              =  "+DbHelper.ParamChar+"paramTag, "
				+"IsProcessed      =  "+POut.Bool  (eServiceSignal.IsProcessed)+" "
				+"WHERE EServiceSignalNum = "+POut.Long(eServiceSignal.EServiceSignalNum);
			if(eServiceSignal.Description==null) {
				eServiceSignal.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(eServiceSignal.Description));
			if(eServiceSignal.Tag==null) {
				eServiceSignal.Tag="";
			}
			OdSqlParameter paramTag=new OdSqlParameter("paramTag",OdDbType.Text,POut.StringParam(eServiceSignal.Tag));
			Db.NonQ(command,paramDescription,paramTag);
		}

		///<summary>Updates one EServiceSignal in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EServiceSignal eServiceSignal,EServiceSignal oldEServiceSignal) {
			string command="";
			if(eServiceSignal.ServiceCode != oldEServiceSignal.ServiceCode) {
				if(command!="") { command+=",";}
				command+="ServiceCode = "+POut.Int(eServiceSignal.ServiceCode)+"";
			}
			if(eServiceSignal.ReasonCategory != oldEServiceSignal.ReasonCategory) {
				if(command!="") { command+=",";}
				command+="ReasonCategory = "+POut.Int(eServiceSignal.ReasonCategory)+"";
			}
			if(eServiceSignal.ReasonCode != oldEServiceSignal.ReasonCode) {
				if(command!="") { command+=",";}
				command+="ReasonCode = "+POut.Int(eServiceSignal.ReasonCode)+"";
			}
			if(eServiceSignal.Severity != oldEServiceSignal.Severity) {
				if(command!="") { command+=",";}
				command+="Severity = "+POut.Int   ((int)eServiceSignal.Severity)+"";
			}
			if(eServiceSignal.Description != oldEServiceSignal.Description) {
				if(command!="") { command+=",";}
				command+="Description = "+DbHelper.ParamChar+"paramDescription";
			}
			if(eServiceSignal.SigDateTime != oldEServiceSignal.SigDateTime) {
				if(command!="") { command+=",";}
				command+="SigDateTime = "+POut.DateT(eServiceSignal.SigDateTime)+"";
			}
			if(eServiceSignal.Tag != oldEServiceSignal.Tag) {
				if(command!="") { command+=",";}
				command+="Tag = "+DbHelper.ParamChar+"paramTag";
			}
			if(eServiceSignal.IsProcessed != oldEServiceSignal.IsProcessed) {
				if(command!="") { command+=",";}
				command+="IsProcessed = "+POut.Bool(eServiceSignal.IsProcessed)+"";
			}
			if(command=="") {
				return false;
			}
			if(eServiceSignal.Description==null) {
				eServiceSignal.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(eServiceSignal.Description));
			if(eServiceSignal.Tag==null) {
				eServiceSignal.Tag="";
			}
			OdSqlParameter paramTag=new OdSqlParameter("paramTag",OdDbType.Text,POut.StringParam(eServiceSignal.Tag));
			command="UPDATE eservicesignal SET "+command
				+" WHERE EServiceSignalNum = "+POut.Long(eServiceSignal.EServiceSignalNum);
			Db.NonQ(command,paramDescription,paramTag);
			return true;
		}

		///<summary>Returns true if Update(EServiceSignal,EServiceSignal) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EServiceSignal eServiceSignal,EServiceSignal oldEServiceSignal) {
			if(eServiceSignal.ServiceCode != oldEServiceSignal.ServiceCode) {
				return true;
			}
			if(eServiceSignal.ReasonCategory != oldEServiceSignal.ReasonCategory) {
				return true;
			}
			if(eServiceSignal.ReasonCode != oldEServiceSignal.ReasonCode) {
				return true;
			}
			if(eServiceSignal.Severity != oldEServiceSignal.Severity) {
				return true;
			}
			if(eServiceSignal.Description != oldEServiceSignal.Description) {
				return true;
			}
			if(eServiceSignal.SigDateTime != oldEServiceSignal.SigDateTime) {
				return true;
			}
			if(eServiceSignal.Tag != oldEServiceSignal.Tag) {
				return true;
			}
			if(eServiceSignal.IsProcessed != oldEServiceSignal.IsProcessed) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EServiceSignal from the database.</summary>
		public static void Delete(long eServiceSignalNum) {
			string command="DELETE FROM eservicesignal "
				+"WHERE EServiceSignalNum = "+POut.Long(eServiceSignalNum);
			Db.NonQ(command);
		}

	}
}