//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class HL7DefMessageCrud {
		///<summary>Gets one HL7DefMessage object from the database using the primary key.  Returns null if not found.</summary>
		public static HL7DefMessage SelectOne(long hL7DefMessageNum) {
			string command="SELECT * FROM hl7defmessage "
				+"WHERE HL7DefMessageNum = "+POut.Long(hL7DefMessageNum);
			List<HL7DefMessage> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one HL7DefMessage object from the database using a query.</summary>
		public static HL7DefMessage SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<HL7DefMessage> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of HL7DefMessage objects from the database using a query.</summary>
		public static List<HL7DefMessage> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<HL7DefMessage> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<HL7DefMessage> TableToList(DataTable table) {
			List<HL7DefMessage> retVal=new List<HL7DefMessage>();
			HL7DefMessage hL7DefMessage;
			foreach(DataRow row in table.Rows) {
				hL7DefMessage=new HL7DefMessage();
				hL7DefMessage.HL7DefMessageNum= PIn.Long  (row["HL7DefMessageNum"].ToString());
				hL7DefMessage.HL7DefNum       = PIn.Long  (row["HL7DefNum"].ToString());
				string messageType=row["MessageType"].ToString();
				if(messageType=="") {
					hL7DefMessage.MessageType   =(OpenDentBusiness.MessageTypeHL7)0;
				}
				else try{
					hL7DefMessage.MessageType   =(OpenDentBusiness.MessageTypeHL7)Enum.Parse(typeof(OpenDentBusiness.MessageTypeHL7),messageType);
				}
				catch{
					hL7DefMessage.MessageType   =(OpenDentBusiness.MessageTypeHL7)0;
				}
				string eventType=row["EventType"].ToString();
				if(eventType=="") {
					hL7DefMessage.EventType     =(OpenDentBusiness.EventTypeHL7)0;
				}
				else try{
					hL7DefMessage.EventType     =(OpenDentBusiness.EventTypeHL7)Enum.Parse(typeof(OpenDentBusiness.EventTypeHL7),eventType);
				}
				catch{
					hL7DefMessage.EventType     =(OpenDentBusiness.EventTypeHL7)0;
				}
				hL7DefMessage.InOrOut         = (OpenDentBusiness.InOutHL7)PIn.Int(row["InOrOut"].ToString());
				hL7DefMessage.ItemOrder       = PIn.Int   (row["ItemOrder"].ToString());
				hL7DefMessage.Note            = PIn.String(row["Note"].ToString());
				string messageStructure=row["MessageStructure"].ToString();
				if(messageStructure=="") {
					hL7DefMessage.MessageStructure=(OpenDentBusiness.MessageStructureHL7)0;
				}
				else try{
					hL7DefMessage.MessageStructure=(OpenDentBusiness.MessageStructureHL7)Enum.Parse(typeof(OpenDentBusiness.MessageStructureHL7),messageStructure);
				}
				catch{
					hL7DefMessage.MessageStructure=(OpenDentBusiness.MessageStructureHL7)0;
				}
				retVal.Add(hL7DefMessage);
			}
			return retVal;
		}

		///<summary>Converts a list of HL7DefMessage into a DataTable.</summary>
		public static DataTable ListToTable(List<HL7DefMessage> listHL7DefMessages,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="HL7DefMessage";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("HL7DefMessageNum");
			table.Columns.Add("HL7DefNum");
			table.Columns.Add("MessageType");
			table.Columns.Add("EventType");
			table.Columns.Add("InOrOut");
			table.Columns.Add("ItemOrder");
			table.Columns.Add("Note");
			table.Columns.Add("MessageStructure");
			foreach(HL7DefMessage hL7DefMessage in listHL7DefMessages) {
				table.Rows.Add(new object[] {
					POut.Long  (hL7DefMessage.HL7DefMessageNum),
					POut.Long  (hL7DefMessage.HL7DefNum),
					POut.Int   ((int)hL7DefMessage.MessageType),
					POut.Int   ((int)hL7DefMessage.EventType),
					POut.Int   ((int)hL7DefMessage.InOrOut),
					POut.Int   (hL7DefMessage.ItemOrder),
					            hL7DefMessage.Note,
					POut.Int   ((int)hL7DefMessage.MessageStructure),
				});
			}
			return table;
		}

		///<summary>Inserts one HL7DefMessage into the database.  Returns the new priKey.</summary>
		public static long Insert(HL7DefMessage hL7DefMessage) {
			return Insert(hL7DefMessage,false);
		}

		///<summary>Inserts one HL7DefMessage into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(HL7DefMessage hL7DefMessage,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				hL7DefMessage.HL7DefMessageNum=ReplicationServers.GetKey("hl7defmessage","HL7DefMessageNum");
			}
			string command="INSERT INTO hl7defmessage (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="HL7DefMessageNum,";
			}
			command+="HL7DefNum,MessageType,EventType,InOrOut,ItemOrder,Note,MessageStructure) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(hL7DefMessage.HL7DefMessageNum)+",";
			}
			command+=
				     POut.Long  (hL7DefMessage.HL7DefNum)+","
				+"'"+POut.String(hL7DefMessage.MessageType.ToString())+"',"
				+"'"+POut.String(hL7DefMessage.EventType.ToString())+"',"
				+    POut.Int   ((int)hL7DefMessage.InOrOut)+","
				+    POut.Int   (hL7DefMessage.ItemOrder)+","
				+    DbHelper.ParamChar+"paramNote,"
				+"'"+POut.String(hL7DefMessage.MessageStructure.ToString())+"')";
			if(hL7DefMessage.Note==null) {
				hL7DefMessage.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(hL7DefMessage.Note));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				hL7DefMessage.HL7DefMessageNum=Db.NonQ(command,true,"HL7DefMessageNum","hL7DefMessage",paramNote);
			}
			return hL7DefMessage.HL7DefMessageNum;
		}

		///<summary>Inserts one HL7DefMessage into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(HL7DefMessage hL7DefMessage) {
			return InsertNoCache(hL7DefMessage,false);
		}

		///<summary>Inserts one HL7DefMessage into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(HL7DefMessage hL7DefMessage,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO hl7defmessage (";
			if(!useExistingPK && isRandomKeys) {
				hL7DefMessage.HL7DefMessageNum=ReplicationServers.GetKeyNoCache("hl7defmessage","HL7DefMessageNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="HL7DefMessageNum,";
			}
			command+="HL7DefNum,MessageType,EventType,InOrOut,ItemOrder,Note,MessageStructure) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(hL7DefMessage.HL7DefMessageNum)+",";
			}
			command+=
				     POut.Long  (hL7DefMessage.HL7DefNum)+","
				+"'"+POut.String(hL7DefMessage.MessageType.ToString())+"',"
				+"'"+POut.String(hL7DefMessage.EventType.ToString())+"',"
				+    POut.Int   ((int)hL7DefMessage.InOrOut)+","
				+    POut.Int   (hL7DefMessage.ItemOrder)+","
				+    DbHelper.ParamChar+"paramNote,"
				+"'"+POut.String(hL7DefMessage.MessageStructure.ToString())+"')";
			if(hL7DefMessage.Note==null) {
				hL7DefMessage.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(hL7DefMessage.Note));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				hL7DefMessage.HL7DefMessageNum=Db.NonQ(command,true,"HL7DefMessageNum","hL7DefMessage",paramNote);
			}
			return hL7DefMessage.HL7DefMessageNum;
		}

		///<summary>Updates one HL7DefMessage in the database.</summary>
		public static void Update(HL7DefMessage hL7DefMessage) {
			string command="UPDATE hl7defmessage SET "
				+"HL7DefNum       =  "+POut.Long  (hL7DefMessage.HL7DefNum)+", "
				+"MessageType     = '"+POut.String(hL7DefMessage.MessageType.ToString())+"', "
				+"EventType       = '"+POut.String(hL7DefMessage.EventType.ToString())+"', "
				+"InOrOut         =  "+POut.Int   ((int)hL7DefMessage.InOrOut)+", "
				+"ItemOrder       =  "+POut.Int   (hL7DefMessage.ItemOrder)+", "
				+"Note            =  "+DbHelper.ParamChar+"paramNote, "
				+"MessageStructure= '"+POut.String(hL7DefMessage.MessageStructure.ToString())+"' "
				+"WHERE HL7DefMessageNum = "+POut.Long(hL7DefMessage.HL7DefMessageNum);
			if(hL7DefMessage.Note==null) {
				hL7DefMessage.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(hL7DefMessage.Note));
			Db.NonQ(command,paramNote);
		}

		///<summary>Updates one HL7DefMessage in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(HL7DefMessage hL7DefMessage,HL7DefMessage oldHL7DefMessage) {
			string command="";
			if(hL7DefMessage.HL7DefNum != oldHL7DefMessage.HL7DefNum) {
				if(command!="") { command+=",";}
				command+="HL7DefNum = "+POut.Long(hL7DefMessage.HL7DefNum)+"";
			}
			if(hL7DefMessage.MessageType != oldHL7DefMessage.MessageType) {
				if(command!="") { command+=",";}
				command+="MessageType = '"+POut.String(hL7DefMessage.MessageType.ToString())+"'";
			}
			if(hL7DefMessage.EventType != oldHL7DefMessage.EventType) {
				if(command!="") { command+=",";}
				command+="EventType = '"+POut.String(hL7DefMessage.EventType.ToString())+"'";
			}
			if(hL7DefMessage.InOrOut != oldHL7DefMessage.InOrOut) {
				if(command!="") { command+=",";}
				command+="InOrOut = "+POut.Int   ((int)hL7DefMessage.InOrOut)+"";
			}
			if(hL7DefMessage.ItemOrder != oldHL7DefMessage.ItemOrder) {
				if(command!="") { command+=",";}
				command+="ItemOrder = "+POut.Int(hL7DefMessage.ItemOrder)+"";
			}
			if(hL7DefMessage.Note != oldHL7DefMessage.Note) {
				if(command!="") { command+=",";}
				command+="Note = "+DbHelper.ParamChar+"paramNote";
			}
			if(hL7DefMessage.MessageStructure != oldHL7DefMessage.MessageStructure) {
				if(command!="") { command+=",";}
				command+="MessageStructure = '"+POut.String(hL7DefMessage.MessageStructure.ToString())+"'";
			}
			if(command=="") {
				return false;
			}
			if(hL7DefMessage.Note==null) {
				hL7DefMessage.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(hL7DefMessage.Note));
			command="UPDATE hl7defmessage SET "+command
				+" WHERE HL7DefMessageNum = "+POut.Long(hL7DefMessage.HL7DefMessageNum);
			Db.NonQ(command,paramNote);
			return true;
		}

		///<summary>Returns true if Update(HL7DefMessage,HL7DefMessage) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(HL7DefMessage hL7DefMessage,HL7DefMessage oldHL7DefMessage) {
			if(hL7DefMessage.HL7DefNum != oldHL7DefMessage.HL7DefNum) {
				return true;
			}
			if(hL7DefMessage.MessageType != oldHL7DefMessage.MessageType) {
				return true;
			}
			if(hL7DefMessage.EventType != oldHL7DefMessage.EventType) {
				return true;
			}
			if(hL7DefMessage.InOrOut != oldHL7DefMessage.InOrOut) {
				return true;
			}
			if(hL7DefMessage.ItemOrder != oldHL7DefMessage.ItemOrder) {
				return true;
			}
			if(hL7DefMessage.Note != oldHL7DefMessage.Note) {
				return true;
			}
			if(hL7DefMessage.MessageStructure != oldHL7DefMessage.MessageStructure) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one HL7DefMessage from the database.</summary>
		public static void Delete(long hL7DefMessageNum) {
			string command="DELETE FROM hl7defmessage "
				+"WHERE HL7DefMessageNum = "+POut.Long(hL7DefMessageNum);
			Db.NonQ(command);
		}

	}
}