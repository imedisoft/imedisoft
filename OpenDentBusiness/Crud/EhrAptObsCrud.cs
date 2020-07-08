//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EhrAptObsCrud {
		///<summary>Gets one EhrAptObs object from the database using the primary key.  Returns null if not found.</summary>
		public static EhrAptObs SelectOne(long ehrAptObsNum) {
			string command="SELECT * FROM ehraptobs "
				+"WHERE EhrAptObsNum = "+POut.Long(ehrAptObsNum);
			List<EhrAptObs> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EhrAptObs object from the database using a query.</summary>
		public static EhrAptObs SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrAptObs> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EhrAptObs objects from the database using a query.</summary>
		public static List<EhrAptObs> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrAptObs> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EhrAptObs> TableToList(DataTable table) {
			List<EhrAptObs> retVal=new List<EhrAptObs>();
			EhrAptObs ehrAptObs;
			foreach(DataRow row in table.Rows) {
				ehrAptObs=new EhrAptObs();
				ehrAptObs.EhrAptObsNum   = PIn.Long  (row["EhrAptObsNum"].ToString());
				ehrAptObs.AptNum         = PIn.Long  (row["AptNum"].ToString());
				ehrAptObs.IdentifyingCode= (OpenDentBusiness.EhrAptObsIdentifier)PIn.Int(row["IdentifyingCode"].ToString());
				ehrAptObs.ValType        = (OpenDentBusiness.EhrAptObsType)PIn.Int(row["ValType"].ToString());
				ehrAptObs.ValReported    = PIn.String(row["ValReported"].ToString());
				ehrAptObs.UcumCode       = PIn.String(row["UcumCode"].ToString());
				ehrAptObs.ValCodeSystem  = PIn.String(row["ValCodeSystem"].ToString());
				retVal.Add(ehrAptObs);
			}
			return retVal;
		}

		///<summary>Converts a list of EhrAptObs into a DataTable.</summary>
		public static DataTable ListToTable(List<EhrAptObs> listEhrAptObss,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EhrAptObs";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EhrAptObsNum");
			table.Columns.Add("AptNum");
			table.Columns.Add("IdentifyingCode");
			table.Columns.Add("ValType");
			table.Columns.Add("ValReported");
			table.Columns.Add("UcumCode");
			table.Columns.Add("ValCodeSystem");
			foreach(EhrAptObs ehrAptObs in listEhrAptObss) {
				table.Rows.Add(new object[] {
					POut.Long  (ehrAptObs.EhrAptObsNum),
					POut.Long  (ehrAptObs.AptNum),
					POut.Int   ((int)ehrAptObs.IdentifyingCode),
					POut.Int   ((int)ehrAptObs.ValType),
					            ehrAptObs.ValReported,
					            ehrAptObs.UcumCode,
					            ehrAptObs.ValCodeSystem,
				});
			}
			return table;
		}

		///<summary>Inserts one EhrAptObs into the database.  Returns the new priKey.</summary>
		public static long Insert(EhrAptObs ehrAptObs) {
			return Insert(ehrAptObs,false);
		}

		///<summary>Inserts one EhrAptObs into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EhrAptObs ehrAptObs,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				ehrAptObs.EhrAptObsNum=ReplicationServers.GetKey("ehraptobs","EhrAptObsNum");
			}
			string command="INSERT INTO ehraptobs (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EhrAptObsNum,";
			}
			command+="AptNum,IdentifyingCode,ValType,ValReported,UcumCode,ValCodeSystem) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(ehrAptObs.EhrAptObsNum)+",";
			}
			command+=
				     POut.Long  (ehrAptObs.AptNum)+","
				+    POut.Int   ((int)ehrAptObs.IdentifyingCode)+","
				+    POut.Int   ((int)ehrAptObs.ValType)+","
				+"'"+POut.String(ehrAptObs.ValReported)+"',"
				+"'"+POut.String(ehrAptObs.UcumCode)+"',"
				+"'"+POut.String(ehrAptObs.ValCodeSystem)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				ehrAptObs.EhrAptObsNum=Db.NonQ(command,true,"EhrAptObsNum","ehrAptObs");
			}
			return ehrAptObs.EhrAptObsNum;
		}

		///<summary>Inserts one EhrAptObs into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrAptObs ehrAptObs) {
			return InsertNoCache(ehrAptObs,false);
		}

		///<summary>Inserts one EhrAptObs into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrAptObs ehrAptObs,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO ehraptobs (";
			if(!useExistingPK && isRandomKeys) {
				ehrAptObs.EhrAptObsNum=ReplicationServers.GetKeyNoCache("ehraptobs","EhrAptObsNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EhrAptObsNum,";
			}
			command+="AptNum,IdentifyingCode,ValType,ValReported,UcumCode,ValCodeSystem) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(ehrAptObs.EhrAptObsNum)+",";
			}
			command+=
				     POut.Long  (ehrAptObs.AptNum)+","
				+    POut.Int   ((int)ehrAptObs.IdentifyingCode)+","
				+    POut.Int   ((int)ehrAptObs.ValType)+","
				+"'"+POut.String(ehrAptObs.ValReported)+"',"
				+"'"+POut.String(ehrAptObs.UcumCode)+"',"
				+"'"+POut.String(ehrAptObs.ValCodeSystem)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				ehrAptObs.EhrAptObsNum=Db.NonQ(command,true,"EhrAptObsNum","ehrAptObs");
			}
			return ehrAptObs.EhrAptObsNum;
		}

		///<summary>Updates one EhrAptObs in the database.</summary>
		public static void Update(EhrAptObs ehrAptObs) {
			string command="UPDATE ehraptobs SET "
				+"AptNum         =  "+POut.Long  (ehrAptObs.AptNum)+", "
				+"IdentifyingCode=  "+POut.Int   ((int)ehrAptObs.IdentifyingCode)+", "
				+"ValType        =  "+POut.Int   ((int)ehrAptObs.ValType)+", "
				+"ValReported    = '"+POut.String(ehrAptObs.ValReported)+"', "
				+"UcumCode       = '"+POut.String(ehrAptObs.UcumCode)+"', "
				+"ValCodeSystem  = '"+POut.String(ehrAptObs.ValCodeSystem)+"' "
				+"WHERE EhrAptObsNum = "+POut.Long(ehrAptObs.EhrAptObsNum);
			Db.NonQ(command);
		}

		///<summary>Updates one EhrAptObs in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EhrAptObs ehrAptObs,EhrAptObs oldEhrAptObs) {
			string command="";
			if(ehrAptObs.AptNum != oldEhrAptObs.AptNum) {
				if(command!="") { command+=",";}
				command+="AptNum = "+POut.Long(ehrAptObs.AptNum)+"";
			}
			if(ehrAptObs.IdentifyingCode != oldEhrAptObs.IdentifyingCode) {
				if(command!="") { command+=",";}
				command+="IdentifyingCode = "+POut.Int   ((int)ehrAptObs.IdentifyingCode)+"";
			}
			if(ehrAptObs.ValType != oldEhrAptObs.ValType) {
				if(command!="") { command+=",";}
				command+="ValType = "+POut.Int   ((int)ehrAptObs.ValType)+"";
			}
			if(ehrAptObs.ValReported != oldEhrAptObs.ValReported) {
				if(command!="") { command+=",";}
				command+="ValReported = '"+POut.String(ehrAptObs.ValReported)+"'";
			}
			if(ehrAptObs.UcumCode != oldEhrAptObs.UcumCode) {
				if(command!="") { command+=",";}
				command+="UcumCode = '"+POut.String(ehrAptObs.UcumCode)+"'";
			}
			if(ehrAptObs.ValCodeSystem != oldEhrAptObs.ValCodeSystem) {
				if(command!="") { command+=",";}
				command+="ValCodeSystem = '"+POut.String(ehrAptObs.ValCodeSystem)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE ehraptobs SET "+command
				+" WHERE EhrAptObsNum = "+POut.Long(ehrAptObs.EhrAptObsNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(EhrAptObs,EhrAptObs) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EhrAptObs ehrAptObs,EhrAptObs oldEhrAptObs) {
			if(ehrAptObs.AptNum != oldEhrAptObs.AptNum) {
				return true;
			}
			if(ehrAptObs.IdentifyingCode != oldEhrAptObs.IdentifyingCode) {
				return true;
			}
			if(ehrAptObs.ValType != oldEhrAptObs.ValType) {
				return true;
			}
			if(ehrAptObs.ValReported != oldEhrAptObs.ValReported) {
				return true;
			}
			if(ehrAptObs.UcumCode != oldEhrAptObs.UcumCode) {
				return true;
			}
			if(ehrAptObs.ValCodeSystem != oldEhrAptObs.ValCodeSystem) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EhrAptObs from the database.</summary>
		public static void Delete(long ehrAptObsNum) {
			string command="DELETE FROM ehraptobs "
				+"WHERE EhrAptObsNum = "+POut.Long(ehrAptObsNum);
			Db.NonQ(command);
		}

	}
}