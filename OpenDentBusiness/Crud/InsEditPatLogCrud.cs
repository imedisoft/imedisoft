//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

namespace OpenDentBusiness.Crud{
	public class InsEditPatLogCrud {
		///<summary>Gets one InsEditPatLog object from the database using the primary key.  Returns null if not found.</summary>
		public static InsEditPatLog SelectOne(long insEditPatLogNum) {
			string command="SELECT * FROM inseditpatlog "
				+"WHERE InsEditPatLogNum = "+POut.Long(insEditPatLogNum);
			List<InsEditPatLog> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one InsEditPatLog object from the database using a query.</summary>
		public static InsEditPatLog SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<InsEditPatLog> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of InsEditPatLog objects from the database using a query.</summary>
		public static List<InsEditPatLog> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<InsEditPatLog> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<InsEditPatLog> TableToList(DataTable table) {
			List<InsEditPatLog> retVal=new List<InsEditPatLog>();
			InsEditPatLog insEditPatLog;
			foreach(DataRow row in table.Rows) {
				insEditPatLog=new InsEditPatLog();
				insEditPatLog.InsEditPatLogNum= PIn.Long  (row["InsEditPatLogNum"].ToString());
				insEditPatLog.FKey            = PIn.Long  (row["FKey"].ToString());
				insEditPatLog.LogType         = (OpenDentBusiness.InsEditPatLogType)PIn.Int(row["LogType"].ToString());
				insEditPatLog.FieldName       = PIn.String(row["FieldName"].ToString());
				insEditPatLog.OldValue        = PIn.String(row["OldValue"].ToString());
				insEditPatLog.NewValue        = PIn.String(row["NewValue"].ToString());
				insEditPatLog.UserNum         = PIn.Long  (row["UserNum"].ToString());
				insEditPatLog.DateTStamp      = PIn.DateT (row["DateTStamp"].ToString());
				insEditPatLog.ParentKey       = PIn.Long  (row["ParentKey"].ToString());
				insEditPatLog.Description     = PIn.String(row["Description"].ToString());
				retVal.Add(insEditPatLog);
			}
			return retVal;
		}

		///<summary>Converts a list of InsEditPatLog into a DataTable.</summary>
		public static DataTable ListToTable(List<InsEditPatLog> listInsEditPatLogs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="InsEditPatLog";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("InsEditPatLogNum");
			table.Columns.Add("FKey");
			table.Columns.Add("LogType");
			table.Columns.Add("FieldName");
			table.Columns.Add("OldValue");
			table.Columns.Add("NewValue");
			table.Columns.Add("UserNum");
			table.Columns.Add("DateTStamp");
			table.Columns.Add("ParentKey");
			table.Columns.Add("Description");
			foreach(InsEditPatLog insEditPatLog in listInsEditPatLogs) {
				table.Rows.Add(new object[] {
					POut.Long  (insEditPatLog.InsEditPatLogNum),
					POut.Long  (insEditPatLog.FKey),
					POut.Int   ((int)insEditPatLog.LogType),
					            insEditPatLog.FieldName,
					            insEditPatLog.OldValue,
					            insEditPatLog.NewValue,
					POut.Long  (insEditPatLog.UserNum),
					POut.DateT (insEditPatLog.DateTStamp,false),
					POut.Long  (insEditPatLog.ParentKey),
					            insEditPatLog.Description,
				});
			}
			return table;
		}

		///<summary>Inserts one InsEditPatLog into the database.  Returns the new priKey.</summary>
		public static long Insert(InsEditPatLog insEditPatLog) {
			return Insert(insEditPatLog,false);
		}

		///<summary>Inserts one InsEditPatLog into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(InsEditPatLog insEditPatLog,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				insEditPatLog.InsEditPatLogNum=ReplicationServers.GetKey("inseditpatlog","InsEditPatLogNum");
			}
			string command="INSERT INTO inseditpatlog (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="InsEditPatLogNum,";
			}
			command+="FKey,LogType,FieldName,OldValue,NewValue,UserNum,ParentKey,Description) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(insEditPatLog.InsEditPatLogNum)+",";
			}
			command+=
				     POut.Long  (insEditPatLog.FKey)+","
				+    POut.Int   ((int)insEditPatLog.LogType)+","
				+"'"+POut.String(insEditPatLog.FieldName)+"',"
				+"'"+POut.String(insEditPatLog.OldValue)+"',"
				+"'"+POut.String(insEditPatLog.NewValue)+"',"
				+    POut.Long  (insEditPatLog.UserNum)+","
				//DateTStamp can only be set by MySQL
				+    POut.Long  (insEditPatLog.ParentKey)+","
				+"'"+POut.String(insEditPatLog.Description)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				insEditPatLog.InsEditPatLogNum=Db.NonQ(command,true,"InsEditPatLogNum","insEditPatLog");
			}
			return insEditPatLog.InsEditPatLogNum;
		}

		///<summary>Inserts many InsEditPatLogs into the database.</summary>
		public static void InsertMany(List<InsEditPatLog> listInsEditPatLogs) {
			InsertMany(listInsEditPatLogs,false);
		}

		///<summary>Inserts many InsEditPatLogs into the database.  Provides option to use the existing priKey.</summary>
		public static void InsertMany(List<InsEditPatLog> listInsEditPatLogs,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				foreach(InsEditPatLog insEditPatLog in listInsEditPatLogs) {
					Insert(insEditPatLog);
				}
			}
			else {
				StringBuilder sbCommands=null;
				int index=0;
				int countRows=0;
				while(index < listInsEditPatLogs.Count) {
					InsEditPatLog insEditPatLog=listInsEditPatLogs[index];
					StringBuilder sbRow=new StringBuilder("(");
					bool hasComma=false;
					if(sbCommands==null) {
						sbCommands=new StringBuilder();
						sbCommands.Append("INSERT INTO inseditpatlog (");
						if(useExistingPK) {
							sbCommands.Append("InsEditPatLogNum,");
						}
						sbCommands.Append("FKey,LogType,FieldName,OldValue,NewValue,UserNum,ParentKey,Description) VALUES ");
						countRows=0;
					}
					else {
						hasComma=true;
					}
					if(useExistingPK) {
						sbRow.Append(POut.Long(insEditPatLog.InsEditPatLogNum)); sbRow.Append(",");
					}
					sbRow.Append(POut.Long(insEditPatLog.FKey)); sbRow.Append(",");
					sbRow.Append(POut.Int((int)insEditPatLog.LogType)); sbRow.Append(",");
					sbRow.Append("'"+POut.String(insEditPatLog.FieldName)+"'"); sbRow.Append(",");
					sbRow.Append("'"+POut.String(insEditPatLog.OldValue)+"'"); sbRow.Append(",");
					sbRow.Append("'"+POut.String(insEditPatLog.NewValue)+"'"); sbRow.Append(",");
					sbRow.Append(POut.Long(insEditPatLog.UserNum)); sbRow.Append(",");
					//DateTStamp can only be set by MySQL
					sbRow.Append(POut.Long(insEditPatLog.ParentKey)); sbRow.Append(",");
					sbRow.Append("'"+POut.String(insEditPatLog.Description)+"'"); sbRow.Append(")");
					if(sbCommands.Length+sbRow.Length+1 > TableBase.MaxAllowedPacketCount && countRows > 0) {
						Db.NonQ(sbCommands.ToString());
						sbCommands=null;
					}
					else {
						if(hasComma) {
							sbCommands.Append(",");
						}
						sbCommands.Append(sbRow.ToString());
						countRows++;
						if(index==listInsEditPatLogs.Count-1) {
							Db.NonQ(sbCommands.ToString());
						}
						index++;
					}
				}
			}
		}

		///<summary>Inserts one InsEditPatLog into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(InsEditPatLog insEditPatLog) {
			return InsertNoCache(insEditPatLog,false);
		}

		///<summary>Inserts one InsEditPatLog into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(InsEditPatLog insEditPatLog,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO inseditpatlog (";
			if(!useExistingPK && isRandomKeys) {
				insEditPatLog.InsEditPatLogNum=ReplicationServers.GetKeyNoCache("inseditpatlog","InsEditPatLogNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="InsEditPatLogNum,";
			}
			command+="FKey,LogType,FieldName,OldValue,NewValue,UserNum,ParentKey,Description) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(insEditPatLog.InsEditPatLogNum)+",";
			}
			command+=
				     POut.Long  (insEditPatLog.FKey)+","
				+    POut.Int   ((int)insEditPatLog.LogType)+","
				+"'"+POut.String(insEditPatLog.FieldName)+"',"
				+"'"+POut.String(insEditPatLog.OldValue)+"',"
				+"'"+POut.String(insEditPatLog.NewValue)+"',"
				+    POut.Long  (insEditPatLog.UserNum)+","
				//DateTStamp can only be set by MySQL
				+    POut.Long  (insEditPatLog.ParentKey)+","
				+"'"+POut.String(insEditPatLog.Description)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				insEditPatLog.InsEditPatLogNum=Db.NonQ(command,true,"InsEditPatLogNum","insEditPatLog");
			}
			return insEditPatLog.InsEditPatLogNum;
		}

		///<summary>Updates one InsEditPatLog in the database.</summary>
		public static void Update(InsEditPatLog insEditPatLog) {
			string command="UPDATE inseditpatlog SET "
				+"FKey            =  "+POut.Long  (insEditPatLog.FKey)+", "
				+"LogType         =  "+POut.Int   ((int)insEditPatLog.LogType)+", "
				+"FieldName       = '"+POut.String(insEditPatLog.FieldName)+"', "
				+"OldValue        = '"+POut.String(insEditPatLog.OldValue)+"', "
				+"NewValue        = '"+POut.String(insEditPatLog.NewValue)+"', "
				+"UserNum         =  "+POut.Long  (insEditPatLog.UserNum)+", "
				//DateTStamp can only be set by MySQL
				+"ParentKey       =  "+POut.Long  (insEditPatLog.ParentKey)+", "
				+"Description     = '"+POut.String(insEditPatLog.Description)+"' "
				+"WHERE InsEditPatLogNum = "+POut.Long(insEditPatLog.InsEditPatLogNum);
			Db.NonQ(command);
		}

		///<summary>Updates one InsEditPatLog in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(InsEditPatLog insEditPatLog,InsEditPatLog oldInsEditPatLog) {
			string command="";
			if(insEditPatLog.FKey != oldInsEditPatLog.FKey) {
				if(command!="") { command+=",";}
				command+="FKey = "+POut.Long(insEditPatLog.FKey)+"";
			}
			if(insEditPatLog.LogType != oldInsEditPatLog.LogType) {
				if(command!="") { command+=",";}
				command+="LogType = "+POut.Int   ((int)insEditPatLog.LogType)+"";
			}
			if(insEditPatLog.FieldName != oldInsEditPatLog.FieldName) {
				if(command!="") { command+=",";}
				command+="FieldName = '"+POut.String(insEditPatLog.FieldName)+"'";
			}
			if(insEditPatLog.OldValue != oldInsEditPatLog.OldValue) {
				if(command!="") { command+=",";}
				command+="OldValue = '"+POut.String(insEditPatLog.OldValue)+"'";
			}
			if(insEditPatLog.NewValue != oldInsEditPatLog.NewValue) {
				if(command!="") { command+=",";}
				command+="NewValue = '"+POut.String(insEditPatLog.NewValue)+"'";
			}
			if(insEditPatLog.UserNum != oldInsEditPatLog.UserNum) {
				if(command!="") { command+=",";}
				command+="UserNum = "+POut.Long(insEditPatLog.UserNum)+"";
			}
			//DateTStamp can only be set by MySQL
			if(insEditPatLog.ParentKey != oldInsEditPatLog.ParentKey) {
				if(command!="") { command+=",";}
				command+="ParentKey = "+POut.Long(insEditPatLog.ParentKey)+"";
			}
			if(insEditPatLog.Description != oldInsEditPatLog.Description) {
				if(command!="") { command+=",";}
				command+="Description = '"+POut.String(insEditPatLog.Description)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE inseditpatlog SET "+command
				+" WHERE InsEditPatLogNum = "+POut.Long(insEditPatLog.InsEditPatLogNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(InsEditPatLog,InsEditPatLog) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(InsEditPatLog insEditPatLog,InsEditPatLog oldInsEditPatLog) {
			if(insEditPatLog.FKey != oldInsEditPatLog.FKey) {
				return true;
			}
			if(insEditPatLog.LogType != oldInsEditPatLog.LogType) {
				return true;
			}
			if(insEditPatLog.FieldName != oldInsEditPatLog.FieldName) {
				return true;
			}
			if(insEditPatLog.OldValue != oldInsEditPatLog.OldValue) {
				return true;
			}
			if(insEditPatLog.NewValue != oldInsEditPatLog.NewValue) {
				return true;
			}
			if(insEditPatLog.UserNum != oldInsEditPatLog.UserNum) {
				return true;
			}
			//DateTStamp can only be set by MySQL
			if(insEditPatLog.ParentKey != oldInsEditPatLog.ParentKey) {
				return true;
			}
			if(insEditPatLog.Description != oldInsEditPatLog.Description) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one InsEditPatLog from the database.</summary>
		public static void Delete(long insEditPatLogNum) {
			string command="DELETE FROM inseditpatlog "
				+"WHERE InsEditPatLogNum = "+POut.Long(insEditPatLogNum);
			Db.NonQ(command);
		}

	}
}