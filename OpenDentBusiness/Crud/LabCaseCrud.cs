//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using Imedisoft.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class LabCaseCrud {
		///<summary>Gets one LabCase object from the database using the primary key.  Returns null if not found.</summary>
		public static LabCase SelectOne(long labCaseNum) {
			string command="SELECT * FROM labcase "
				+"WHERE LabCaseNum = "+POut.Long(labCaseNum);
			List<LabCase> list=TableToList(Database.ExecuteDataTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one LabCase object from the database using a query.</summary>
		public static LabCase SelectOne(string command) {

			List<LabCase> list=TableToList(Database.ExecuteDataTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of LabCase objects from the database using a query.</summary>
		public static List<LabCase> SelectMany(string command) {

			List<LabCase> list=TableToList(Database.ExecuteDataTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<LabCase> TableToList(DataTable table) {
			List<LabCase> retVal=new List<LabCase>();
			LabCase labCase;
			foreach(DataRow row in table.Rows) {
				labCase=new LabCase();
				labCase.LabCaseNum     = PIn.Long  (row["LabCaseNum"].ToString());
				labCase.PatNum         = PIn.Long  (row["PatNum"].ToString());
				labCase.LaboratoryNum  = PIn.Long  (row["LaboratoryNum"].ToString());
				labCase.AptNum         = PIn.Long  (row["AptNum"].ToString());
				labCase.PlannedAptNum  = PIn.Long  (row["PlannedAptNum"].ToString());
				labCase.DateTimeDue    = PIn.Date (row["DateTimeDue"].ToString());
				labCase.DateTimeCreated= PIn.Date (row["DateTimeCreated"].ToString());
				labCase.DateTimeSent   = PIn.Date (row["DateTimeSent"].ToString());
				labCase.DateTimeRecd   = PIn.Date (row["DateTimeRecd"].ToString());
				labCase.DateTimeChecked= PIn.Date (row["DateTimeChecked"].ToString());
				labCase.ProvNum        = PIn.Long  (row["ProvNum"].ToString());
				labCase.Instructions   = PIn.String(row["Instructions"].ToString());
				labCase.LabFee         = PIn.Double(row["LabFee"].ToString());
				labCase.DateTStamp     = PIn.Date (row["DateTStamp"].ToString());
				labCase.InvoiceNum     = PIn.String(row["InvoiceNum"].ToString());
				retVal.Add(labCase);
			}
			return retVal;
		}

		///<summary>Converts a list of LabCase into a DataTable.</summary>
		public static DataTable ListToTable(List<LabCase> listLabCases,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="LabCase";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("LabCaseNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("LaboratoryNum");
			table.Columns.Add("AptNum");
			table.Columns.Add("PlannedAptNum");
			table.Columns.Add("DateTimeDue");
			table.Columns.Add("DateTimeCreated");
			table.Columns.Add("DateTimeSent");
			table.Columns.Add("DateTimeRecd");
			table.Columns.Add("DateTimeChecked");
			table.Columns.Add("ProvNum");
			table.Columns.Add("Instructions");
			table.Columns.Add("LabFee");
			table.Columns.Add("DateTStamp");
			table.Columns.Add("InvoiceNum");
			foreach(LabCase labCase in listLabCases) {
				table.Rows.Add(new object[] {
					POut.Long  (labCase.LabCaseNum),
					POut.Long  (labCase.PatNum),
					POut.Long  (labCase.LaboratoryNum),
					POut.Long  (labCase.AptNum),
					POut.Long  (labCase.PlannedAptNum),
					POut.DateT (labCase.DateTimeDue,false),
					POut.DateT (labCase.DateTimeCreated,false),
					POut.DateT (labCase.DateTimeSent,false),
					POut.DateT (labCase.DateTimeRecd,false),
					POut.DateT (labCase.DateTimeChecked,false),
					POut.Long  (labCase.ProvNum),
					            labCase.Instructions,
					POut.Double(labCase.LabFee),
					POut.DateT (labCase.DateTStamp,false),
					            labCase.InvoiceNum,
				});
			}
			return table;
		}

		///<summary>Inserts one LabCase into the database.  Returns the new priKey.</summary>
		public static long Insert(LabCase labCase) {
			return Insert(labCase,false);
		}

		///<summary>Inserts one LabCase into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(LabCase labCase,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				labCase.LabCaseNum=ReplicationServers.GetKey("labcase","LabCaseNum");
			}
			string command="INSERT INTO labcase (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="LabCaseNum,";
			}
			command+="PatNum,LaboratoryNum,AptNum,PlannedAptNum,DateTimeDue,DateTimeCreated,DateTimeSent,DateTimeRecd,DateTimeChecked,ProvNum,Instructions,LabFee,InvoiceNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(labCase.LabCaseNum)+",";
			}
			command+=
				     POut.Long  (labCase.PatNum)+","
				+    POut.Long  (labCase.LaboratoryNum)+","
				+    POut.Long  (labCase.AptNum)+","
				+    POut.Long  (labCase.PlannedAptNum)+","
				+    POut.DateT (labCase.DateTimeDue)+","
				+    POut.DateT (labCase.DateTimeCreated)+","
				+    POut.DateT (labCase.DateTimeSent)+","
				+    POut.DateT (labCase.DateTimeRecd)+","
				+    POut.DateT (labCase.DateTimeChecked)+","
				+    POut.Long  (labCase.ProvNum)+","
				+    DbHelper.ParamChar+"paramInstructions,"
				+"'"+POut.Double(labCase.LabFee)+"',"
				//DateTStamp can only be set by MySQL
				+"'"+POut.String(labCase.InvoiceNum)+"')";
			if(labCase.Instructions==null) {
				labCase.Instructions="";
			}
			var paramInstructions = new MySqlParameter("paramInstructions", POut.StringParam(labCase.Instructions));
			if(useExistingPK || PrefC.RandomKeys) {
				Database.ExecuteNonQuery(command,paramInstructions);
			}
			else {
				labCase.LabCaseNum=Database.ExecuteInsert(command,paramInstructions);
			}
			return labCase.LabCaseNum;
		}

		///<summary>Inserts one LabCase into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(LabCase labCase) {
			return InsertNoCache(labCase,false);
		}

		///<summary>Inserts one LabCase into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(LabCase labCase,bool useExistingPK) {
			
			string command="INSERT INTO labcase (";
			if(!useExistingPK) {
				labCase.LabCaseNum=ReplicationServers.GetKeyNoCache("labcase","LabCaseNum");
			}
			if(useExistingPK) {
				command+="LabCaseNum,";
			}
			command+="PatNum,LaboratoryNum,AptNum,PlannedAptNum,DateTimeDue,DateTimeCreated,DateTimeSent,DateTimeRecd,DateTimeChecked,ProvNum,Instructions,LabFee,InvoiceNum) VALUES(";
			if(useExistingPK) {
				command+=POut.Long(labCase.LabCaseNum)+",";
			}
			command+=
				     POut.Long  (labCase.PatNum)+","
				+    POut.Long  (labCase.LaboratoryNum)+","
				+    POut.Long  (labCase.AptNum)+","
				+    POut.Long  (labCase.PlannedAptNum)+","
				+    POut.DateT (labCase.DateTimeDue)+","
				+    POut.DateT (labCase.DateTimeCreated)+","
				+    POut.DateT (labCase.DateTimeSent)+","
				+    POut.DateT (labCase.DateTimeRecd)+","
				+    POut.DateT (labCase.DateTimeChecked)+","
				+    POut.Long  (labCase.ProvNum)+","
				+    DbHelper.ParamChar+"paramInstructions,"
				+"'"+POut.Double(labCase.LabFee)+"',"
				//DateTStamp can only be set by MySQL
				+"'"+POut.String(labCase.InvoiceNum)+"')";
			if(labCase.Instructions==null) {
				labCase.Instructions="";
			}
			var paramInstructions = new MySqlParameter("paramInstructions", POut.StringParam(labCase.Instructions));
			if(useExistingPK) {
				Database.ExecuteNonQuery(command,paramInstructions);
			}
			else {
				labCase.LabCaseNum=Database.ExecuteInsert(command,paramInstructions);
			}
			return labCase.LabCaseNum;
		}

		///<summary>Updates one LabCase in the database.</summary>
		public static void Update(LabCase labCase) {
			string command="UPDATE labcase SET "
				+"PatNum         =  "+POut.Long  (labCase.PatNum)+", "
				+"LaboratoryNum  =  "+POut.Long  (labCase.LaboratoryNum)+", "
				+"AptNum         =  "+POut.Long  (labCase.AptNum)+", "
				+"PlannedAptNum  =  "+POut.Long  (labCase.PlannedAptNum)+", "
				+"DateTimeDue    =  "+POut.DateT (labCase.DateTimeDue)+", "
				+"DateTimeCreated=  "+POut.DateT (labCase.DateTimeCreated)+", "
				+"DateTimeSent   =  "+POut.DateT (labCase.DateTimeSent)+", "
				+"DateTimeRecd   =  "+POut.DateT (labCase.DateTimeRecd)+", "
				+"DateTimeChecked=  "+POut.DateT (labCase.DateTimeChecked)+", "
				+"ProvNum        =  "+POut.Long  (labCase.ProvNum)+", "
				+"Instructions   =  "+DbHelper.ParamChar+"paramInstructions, "
				+"LabFee         = '"+POut.Double(labCase.LabFee)+"', "
				//DateTStamp can only be set by MySQL
				+"InvoiceNum     = '"+POut.String(labCase.InvoiceNum)+"' "
				+"WHERE LabCaseNum = "+POut.Long(labCase.LabCaseNum);
			if(labCase.Instructions==null) {
				labCase.Instructions="";
			}
			var paramInstructions = new MySqlParameter("paramInstructions", POut.StringParam(labCase.Instructions));
			Database.ExecuteNonQuery(command,paramInstructions);
		}

		///<summary>Updates one LabCase in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(LabCase labCase,LabCase oldLabCase) {
			string command="";
			if(labCase.PatNum != oldLabCase.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(labCase.PatNum)+"";
			}
			if(labCase.LaboratoryNum != oldLabCase.LaboratoryNum) {
				if(command!="") { command+=",";}
				command+="LaboratoryNum = "+POut.Long(labCase.LaboratoryNum)+"";
			}
			if(labCase.AptNum != oldLabCase.AptNum) {
				if(command!="") { command+=",";}
				command+="AptNum = "+POut.Long(labCase.AptNum)+"";
			}
			if(labCase.PlannedAptNum != oldLabCase.PlannedAptNum) {
				if(command!="") { command+=",";}
				command+="PlannedAptNum = "+POut.Long(labCase.PlannedAptNum)+"";
			}
			if(labCase.DateTimeDue != oldLabCase.DateTimeDue) {
				if(command!="") { command+=",";}
				command+="DateTimeDue = "+POut.DateT(labCase.DateTimeDue)+"";
			}
			if(labCase.DateTimeCreated != oldLabCase.DateTimeCreated) {
				if(command!="") { command+=",";}
				command+="DateTimeCreated = "+POut.DateT(labCase.DateTimeCreated)+"";
			}
			if(labCase.DateTimeSent != oldLabCase.DateTimeSent) {
				if(command!="") { command+=",";}
				command+="DateTimeSent = "+POut.DateT(labCase.DateTimeSent)+"";
			}
			if(labCase.DateTimeRecd != oldLabCase.DateTimeRecd) {
				if(command!="") { command+=",";}
				command+="DateTimeRecd = "+POut.DateT(labCase.DateTimeRecd)+"";
			}
			if(labCase.DateTimeChecked != oldLabCase.DateTimeChecked) {
				if(command!="") { command+=",";}
				command+="DateTimeChecked = "+POut.DateT(labCase.DateTimeChecked)+"";
			}
			if(labCase.ProvNum != oldLabCase.ProvNum) {
				if(command!="") { command+=",";}
				command+="ProvNum = "+POut.Long(labCase.ProvNum)+"";
			}
			if(labCase.Instructions != oldLabCase.Instructions) {
				if(command!="") { command+=",";}
				command+="Instructions = "+DbHelper.ParamChar+"paramInstructions";
			}
			if(labCase.LabFee != oldLabCase.LabFee) {
				if(command!="") { command+=",";}
				command+="LabFee = '"+POut.Double(labCase.LabFee)+"'";
			}
			//DateTStamp can only be set by MySQL
			if(labCase.InvoiceNum != oldLabCase.InvoiceNum) {
				if(command!="") { command+=",";}
				command+="InvoiceNum = '"+POut.String(labCase.InvoiceNum)+"'";
			}
			if(command=="") {
				return false;
			}
			if(labCase.Instructions==null) {
				labCase.Instructions="";
			}
			var paramInstructions = new MySqlParameter("paramInstructions", POut.StringParam(labCase.Instructions));
			command="UPDATE labcase SET "+command
				+" WHERE LabCaseNum = "+POut.Long(labCase.LabCaseNum);
			Database.ExecuteNonQuery(command,paramInstructions);
			return true;
		}

		///<summary>Returns true if Update(LabCase,LabCase) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(LabCase labCase,LabCase oldLabCase) {
			if(labCase.PatNum != oldLabCase.PatNum) {
				return true;
			}
			if(labCase.LaboratoryNum != oldLabCase.LaboratoryNum) {
				return true;
			}
			if(labCase.AptNum != oldLabCase.AptNum) {
				return true;
			}
			if(labCase.PlannedAptNum != oldLabCase.PlannedAptNum) {
				return true;
			}
			if(labCase.DateTimeDue != oldLabCase.DateTimeDue) {
				return true;
			}
			if(labCase.DateTimeCreated != oldLabCase.DateTimeCreated) {
				return true;
			}
			if(labCase.DateTimeSent != oldLabCase.DateTimeSent) {
				return true;
			}
			if(labCase.DateTimeRecd != oldLabCase.DateTimeRecd) {
				return true;
			}
			if(labCase.DateTimeChecked != oldLabCase.DateTimeChecked) {
				return true;
			}
			if(labCase.ProvNum != oldLabCase.ProvNum) {
				return true;
			}
			if(labCase.Instructions != oldLabCase.Instructions) {
				return true;
			}
			if(labCase.LabFee != oldLabCase.LabFee) {
				return true;
			}
			//DateTStamp can only be set by MySQL
			if(labCase.InvoiceNum != oldLabCase.InvoiceNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one LabCase from the database.</summary>
		public static void Delete(long labCaseNum) {
			string command="DELETE FROM labcase "
				+"WHERE LabCaseNum = "+POut.Long(labCaseNum);
			Database.ExecuteNonQuery(command);
		}

	}
}