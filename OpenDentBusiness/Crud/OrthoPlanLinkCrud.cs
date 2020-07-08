//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class OrthoPlanLinkCrud {
		///<summary>Gets one OrthoPlanLink object from the database using the primary key.  Returns null if not found.</summary>
		public static OrthoPlanLink SelectOne(long orthoPlanLinkNum) {
			string command="SELECT * FROM orthoplanlink "
				+"WHERE OrthoPlanLinkNum = "+POut.Long(orthoPlanLinkNum);
			List<OrthoPlanLink> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one OrthoPlanLink object from the database using a query.</summary>
		public static OrthoPlanLink SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<OrthoPlanLink> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of OrthoPlanLink objects from the database using a query.</summary>
		public static List<OrthoPlanLink> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<OrthoPlanLink> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<OrthoPlanLink> TableToList(DataTable table) {
			List<OrthoPlanLink> retVal=new List<OrthoPlanLink>();
			OrthoPlanLink orthoPlanLink;
			foreach(DataRow row in table.Rows) {
				orthoPlanLink=new OrthoPlanLink();
				orthoPlanLink.OrthoPlanLinkNum= PIn.Long  (row["OrthoPlanLinkNum"].ToString());
				orthoPlanLink.OrthoCaseNum    = PIn.Long  (row["OrthoCaseNum"].ToString());
				orthoPlanLink.LinkType        = (OpenDentBusiness.OrthoPlanLinkType)PIn.Int(row["LinkType"].ToString());
				orthoPlanLink.FKey            = PIn.Long  (row["FKey"].ToString());
				orthoPlanLink.IsActive        = PIn.Bool  (row["IsActive"].ToString());
				orthoPlanLink.SecDateTEntry   = PIn.DateT (row["SecDateTEntry"].ToString());
				orthoPlanLink.SecUserNumEntry = PIn.Long  (row["SecUserNumEntry"].ToString());
				retVal.Add(orthoPlanLink);
			}
			return retVal;
		}

		///<summary>Converts a list of OrthoPlanLink into a DataTable.</summary>
		public static DataTable ListToTable(List<OrthoPlanLink> listOrthoPlanLinks,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="OrthoPlanLink";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("OrthoPlanLinkNum");
			table.Columns.Add("OrthoCaseNum");
			table.Columns.Add("LinkType");
			table.Columns.Add("FKey");
			table.Columns.Add("IsActive");
			table.Columns.Add("SecDateTEntry");
			table.Columns.Add("SecUserNumEntry");
			foreach(OrthoPlanLink orthoPlanLink in listOrthoPlanLinks) {
				table.Rows.Add(new object[] {
					POut.Long  (orthoPlanLink.OrthoPlanLinkNum),
					POut.Long  (orthoPlanLink.OrthoCaseNum),
					POut.Int   ((int)orthoPlanLink.LinkType),
					POut.Long  (orthoPlanLink.FKey),
					POut.Bool  (orthoPlanLink.IsActive),
					POut.DateT (orthoPlanLink.SecDateTEntry,false),
					POut.Long  (orthoPlanLink.SecUserNumEntry),
				});
			}
			return table;
		}

		///<summary>Inserts one OrthoPlanLink into the database.  Returns the new priKey.</summary>
		public static long Insert(OrthoPlanLink orthoPlanLink) {
			return Insert(orthoPlanLink,false);
		}

		///<summary>Inserts one OrthoPlanLink into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(OrthoPlanLink orthoPlanLink,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				orthoPlanLink.OrthoPlanLinkNum=ReplicationServers.GetKey("orthoplanlink","OrthoPlanLinkNum");
			}
			string command="INSERT INTO orthoplanlink (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="OrthoPlanLinkNum,";
			}
			command+="OrthoCaseNum,LinkType,FKey,IsActive,SecDateTEntry,SecUserNumEntry) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(orthoPlanLink.OrthoPlanLinkNum)+",";
			}
			command+=
				     POut.Long  (orthoPlanLink.OrthoCaseNum)+","
				+    POut.Int   ((int)orthoPlanLink.LinkType)+","
				+    POut.Long  (orthoPlanLink.FKey)+","
				+    POut.Bool  (orthoPlanLink.IsActive)+","
				+    DbHelper.Now()+","
				+    POut.Long  (orthoPlanLink.SecUserNumEntry)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				orthoPlanLink.OrthoPlanLinkNum=Db.NonQ(command,true,"OrthoPlanLinkNum","orthoPlanLink");
			}
			return orthoPlanLink.OrthoPlanLinkNum;
		}

		///<summary>Inserts one OrthoPlanLink into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(OrthoPlanLink orthoPlanLink) {
			return InsertNoCache(orthoPlanLink,false);
		}

		///<summary>Inserts one OrthoPlanLink into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(OrthoPlanLink orthoPlanLink,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO orthoplanlink (";
			if(!useExistingPK && isRandomKeys) {
				orthoPlanLink.OrthoPlanLinkNum=ReplicationServers.GetKeyNoCache("orthoplanlink","OrthoPlanLinkNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="OrthoPlanLinkNum,";
			}
			command+="OrthoCaseNum,LinkType,FKey,IsActive,SecDateTEntry,SecUserNumEntry) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(orthoPlanLink.OrthoPlanLinkNum)+",";
			}
			command+=
				     POut.Long  (orthoPlanLink.OrthoCaseNum)+","
				+    POut.Int   ((int)orthoPlanLink.LinkType)+","
				+    POut.Long  (orthoPlanLink.FKey)+","
				+    POut.Bool  (orthoPlanLink.IsActive)+","
				+    DbHelper.Now()+","
				+    POut.Long  (orthoPlanLink.SecUserNumEntry)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				orthoPlanLink.OrthoPlanLinkNum=Db.NonQ(command,true,"OrthoPlanLinkNum","orthoPlanLink");
			}
			return orthoPlanLink.OrthoPlanLinkNum;
		}

		///<summary>Updates one OrthoPlanLink in the database.</summary>
		public static void Update(OrthoPlanLink orthoPlanLink) {
			string command="UPDATE orthoplanlink SET "
				+"OrthoCaseNum    =  "+POut.Long  (orthoPlanLink.OrthoCaseNum)+", "
				+"LinkType        =  "+POut.Int   ((int)orthoPlanLink.LinkType)+", "
				+"FKey            =  "+POut.Long  (orthoPlanLink.FKey)+", "
				+"IsActive        =  "+POut.Bool  (orthoPlanLink.IsActive)+", "
				//SecDateTEntry not allowed to change
				+"SecUserNumEntry =  "+POut.Long  (orthoPlanLink.SecUserNumEntry)+" "
				+"WHERE OrthoPlanLinkNum = "+POut.Long(orthoPlanLink.OrthoPlanLinkNum);
			Db.NonQ(command);
		}

		///<summary>Updates one OrthoPlanLink in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(OrthoPlanLink orthoPlanLink,OrthoPlanLink oldOrthoPlanLink) {
			string command="";
			if(orthoPlanLink.OrthoCaseNum != oldOrthoPlanLink.OrthoCaseNum) {
				if(command!="") { command+=",";}
				command+="OrthoCaseNum = "+POut.Long(orthoPlanLink.OrthoCaseNum)+"";
			}
			if(orthoPlanLink.LinkType != oldOrthoPlanLink.LinkType) {
				if(command!="") { command+=",";}
				command+="LinkType = "+POut.Int   ((int)orthoPlanLink.LinkType)+"";
			}
			if(orthoPlanLink.FKey != oldOrthoPlanLink.FKey) {
				if(command!="") { command+=",";}
				command+="FKey = "+POut.Long(orthoPlanLink.FKey)+"";
			}
			if(orthoPlanLink.IsActive != oldOrthoPlanLink.IsActive) {
				if(command!="") { command+=",";}
				command+="IsActive = "+POut.Bool(orthoPlanLink.IsActive)+"";
			}
			//SecDateTEntry not allowed to change
			if(orthoPlanLink.SecUserNumEntry != oldOrthoPlanLink.SecUserNumEntry) {
				if(command!="") { command+=",";}
				command+="SecUserNumEntry = "+POut.Long(orthoPlanLink.SecUserNumEntry)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE orthoplanlink SET "+command
				+" WHERE OrthoPlanLinkNum = "+POut.Long(orthoPlanLink.OrthoPlanLinkNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(OrthoPlanLink,OrthoPlanLink) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(OrthoPlanLink orthoPlanLink,OrthoPlanLink oldOrthoPlanLink) {
			if(orthoPlanLink.OrthoCaseNum != oldOrthoPlanLink.OrthoCaseNum) {
				return true;
			}
			if(orthoPlanLink.LinkType != oldOrthoPlanLink.LinkType) {
				return true;
			}
			if(orthoPlanLink.FKey != oldOrthoPlanLink.FKey) {
				return true;
			}
			if(orthoPlanLink.IsActive != oldOrthoPlanLink.IsActive) {
				return true;
			}
			//SecDateTEntry not allowed to change
			if(orthoPlanLink.SecUserNumEntry != oldOrthoPlanLink.SecUserNumEntry) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one OrthoPlanLink from the database.</summary>
		public static void Delete(long orthoPlanLinkNum) {
			string command="DELETE FROM orthoplanlink "
				+"WHERE OrthoPlanLinkNum = "+POut.Long(orthoPlanLinkNum);
			Db.NonQ(command);
		}

	}
}