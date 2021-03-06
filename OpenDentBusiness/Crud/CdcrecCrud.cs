//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class CdcrecCrud {
		///<summary>Gets one Cdcrec object from the database using the primary key.  Returns null if not found.</summary>
		public static Cdcrec SelectOne(long cdcrecNum) {
			string command="SELECT * FROM cdcrec "
				+"WHERE CdcrecNum = "+POut.Long(cdcrecNum);
			List<Cdcrec> list=TableToList(Database.ExecuteDataTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Cdcrec object from the database using a query.</summary>
		public static Cdcrec SelectOne(string command) {
			List<Cdcrec> list=TableToList(Database.ExecuteDataTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Cdcrec objects from the database using a query.</summary>
		public static List<Cdcrec> SelectMany(string command) {
			List<Cdcrec> list=TableToList(Database.ExecuteDataTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Cdcrec> TableToList(DataTable table) {
			List<Cdcrec> retVal=new List<Cdcrec>();
			Cdcrec cdcrec;
			foreach(DataRow row in table.Rows) {
				cdcrec=new Cdcrec();
				cdcrec.Id       = PIn.Long  (row["CdcrecNum"].ToString());
				cdcrec.Code      = PIn.String(row["CdcrecCode"].ToString());
				cdcrec.HierarchicalCode= PIn.String(row["HeirarchicalCode"].ToString());
				cdcrec.Description     = PIn.String(row["Description"].ToString());
				retVal.Add(cdcrec);
			}
			return retVal;
		}

		///<summary>Converts a list of Cdcrec into a DataTable.</summary>
		public static DataTable ListToTable(List<Cdcrec> listCdcrecs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Cdcrec";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("CdcrecNum");
			table.Columns.Add("CdcrecCode");
			table.Columns.Add("HeirarchicalCode");
			table.Columns.Add("Description");
			foreach(Cdcrec cdcrec in listCdcrecs) {
				table.Rows.Add(new object[] {
					POut.Long  (cdcrec.Id),
					            cdcrec.Code,
					            cdcrec.HierarchicalCode,
					            cdcrec.Description,
				});
			}
			return table;
		}

		///<summary>Inserts one Cdcrec into the database.  Returns the new priKey.</summary>
		public static long Insert(Cdcrec cdcrec) {
			return Insert(cdcrec,false);
		}

		///<summary>Inserts one Cdcrec into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Cdcrec cdcrec,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				cdcrec.Id=ReplicationServers.GetKey("cdcrec","CdcrecNum");
			}
			string command="INSERT INTO cdcrec (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="CdcrecNum,";
			}
			command+="CdcrecCode,HeirarchicalCode,Description) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(cdcrec.Id)+",";
			}
			command+=
				 "'"+POut.String(cdcrec.Code)+"',"
				+"'"+POut.String(cdcrec.HierarchicalCode)+"',"
				+"'"+POut.String(cdcrec.Description)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Database.ExecuteNonQuery(command);
			}
			else {
				cdcrec.Id=Database.ExecuteInsert(command);
			}
			return cdcrec.Id;
		}

		///<summary>Inserts one Cdcrec into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Cdcrec cdcrec) {
			return InsertNoCache(cdcrec,false);
		}

		///<summary>Inserts one Cdcrec into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Cdcrec cdcrec,bool useExistingPK) {
			
			string command="INSERT INTO cdcrec (";
			if(!useExistingPK) {
				cdcrec.Id=ReplicationServers.GetKeyNoCache("cdcrec","CdcrecNum");
			}
			if(useExistingPK) {
				command+="CdcrecNum,";
			}
			command+="CdcrecCode,HeirarchicalCode,Description) VALUES(";
			if(useExistingPK) {
				command+=POut.Long(cdcrec.Id)+",";
			}
			command+=
				 "'"+POut.String(cdcrec.Code)+"',"
				+"'"+POut.String(cdcrec.HierarchicalCode)+"',"
				+"'"+POut.String(cdcrec.Description)+"')";
			if(useExistingPK) {
				Database.ExecuteNonQuery(command);
			}
			else {
				cdcrec.Id=Database.ExecuteInsert(command);
			}
			return cdcrec.Id;
		}

		///<summary>Updates one Cdcrec in the database.</summary>
		public static void Update(Cdcrec cdcrec) {
			string command="UPDATE cdcrec SET "
				+"CdcrecCode      = '"+POut.String(cdcrec.Code)+"', "
				+"HeirarchicalCode= '"+POut.String(cdcrec.HierarchicalCode)+"', "
				+"Description     = '"+POut.String(cdcrec.Description)+"' "
				+"WHERE CdcrecNum = "+POut.Long(cdcrec.Id);
			Database.ExecuteNonQuery(command);
		}

		///<summary>Updates one Cdcrec in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Cdcrec cdcrec,Cdcrec oldCdcrec) {
			string command="";
			if(cdcrec.Code != oldCdcrec.Code) {
				if(command!="") { command+=",";}
				command+="CdcrecCode = '"+POut.String(cdcrec.Code)+"'";
			}
			if(cdcrec.HierarchicalCode != oldCdcrec.HierarchicalCode) {
				if(command!="") { command+=",";}
				command+="HeirarchicalCode = '"+POut.String(cdcrec.HierarchicalCode)+"'";
			}
			if(cdcrec.Description != oldCdcrec.Description) {
				if(command!="") { command+=",";}
				command+="Description = '"+POut.String(cdcrec.Description)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE cdcrec SET "+command
				+" WHERE CdcrecNum = "+POut.Long(cdcrec.Id);
			Database.ExecuteNonQuery(command);
			return true;
		}

		///<summary>Returns true if Update(Cdcrec,Cdcrec) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Cdcrec cdcrec,Cdcrec oldCdcrec) {
			if(cdcrec.Code != oldCdcrec.Code) {
				return true;
			}
			if(cdcrec.HierarchicalCode != oldCdcrec.HierarchicalCode) {
				return true;
			}
			if(cdcrec.Description != oldCdcrec.Description) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Cdcrec from the database.</summary>
		public static void Delete(long cdcrecNum) {
			string command="DELETE FROM cdcrec "
				+"WHERE CdcrecNum = "+POut.Long(cdcrecNum);
			Database.ExecuteNonQuery(command);
		}

	}
}