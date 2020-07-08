//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class CvxCrud {
		///<summary>Gets one Cvx object from the database using the primary key.  Returns null if not found.</summary>
		public static Cvx SelectOne(long cvxNum) {
			string command="SELECT * FROM cvx "
				+"WHERE CvxNum = "+POut.Long(cvxNum);
			List<Cvx> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Cvx object from the database using a query.</summary>
		public static Cvx SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Cvx> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Cvx objects from the database using a query.</summary>
		public static List<Cvx> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Cvx> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Cvx> TableToList(DataTable table) {
			List<Cvx> retVal=new List<Cvx>();
			Cvx cvx;
			foreach(DataRow row in table.Rows) {
				cvx=new Cvx();
				cvx.CvxNum     = PIn.Long  (row["CvxNum"].ToString());
				cvx.CvxCode    = PIn.String(row["CvxCode"].ToString());
				cvx.Description= PIn.String(row["Description"].ToString());
				cvx.IsActive   = PIn.String(row["IsActive"].ToString());
				retVal.Add(cvx);
			}
			return retVal;
		}

		///<summary>Converts a list of Cvx into a DataTable.</summary>
		public static DataTable ListToTable(List<Cvx> listCvxs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Cvx";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("CvxNum");
			table.Columns.Add("CvxCode");
			table.Columns.Add("Description");
			table.Columns.Add("IsActive");
			foreach(Cvx cvx in listCvxs) {
				table.Rows.Add(new object[] {
					POut.Long  (cvx.CvxNum),
					            cvx.CvxCode,
					            cvx.Description,
					            cvx.IsActive,
				});
			}
			return table;
		}

		///<summary>Inserts one Cvx into the database.  Returns the new priKey.</summary>
		public static long Insert(Cvx cvx) {
			return Insert(cvx,false);
		}

		///<summary>Inserts one Cvx into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Cvx cvx,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				cvx.CvxNum=ReplicationServers.GetKey("cvx","CvxNum");
			}
			string command="INSERT INTO cvx (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="CvxNum,";
			}
			command+="CvxCode,Description,IsActive) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(cvx.CvxNum)+",";
			}
			command+=
				 "'"+POut.String(cvx.CvxCode)+"',"
				+"'"+POut.String(cvx.Description)+"',"
				+"'"+POut.String(cvx.IsActive)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				cvx.CvxNum=Db.NonQ(command,true,"CvxNum","cvx");
			}
			return cvx.CvxNum;
		}

		///<summary>Inserts one Cvx into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Cvx cvx) {
			return InsertNoCache(cvx,false);
		}

		///<summary>Inserts one Cvx into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Cvx cvx,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO cvx (";
			if(!useExistingPK && isRandomKeys) {
				cvx.CvxNum=ReplicationServers.GetKeyNoCache("cvx","CvxNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="CvxNum,";
			}
			command+="CvxCode,Description,IsActive) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(cvx.CvxNum)+",";
			}
			command+=
				 "'"+POut.String(cvx.CvxCode)+"',"
				+"'"+POut.String(cvx.Description)+"',"
				+"'"+POut.String(cvx.IsActive)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				cvx.CvxNum=Db.NonQ(command,true,"CvxNum","cvx");
			}
			return cvx.CvxNum;
		}

		///<summary>Updates one Cvx in the database.</summary>
		public static void Update(Cvx cvx) {
			string command="UPDATE cvx SET "
				+"CvxCode    = '"+POut.String(cvx.CvxCode)+"', "
				+"Description= '"+POut.String(cvx.Description)+"', "
				+"IsActive   = '"+POut.String(cvx.IsActive)+"' "
				+"WHERE CvxNum = "+POut.Long(cvx.CvxNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Cvx in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Cvx cvx,Cvx oldCvx) {
			string command="";
			if(cvx.CvxCode != oldCvx.CvxCode) {
				if(command!="") { command+=",";}
				command+="CvxCode = '"+POut.String(cvx.CvxCode)+"'";
			}
			if(cvx.Description != oldCvx.Description) {
				if(command!="") { command+=",";}
				command+="Description = '"+POut.String(cvx.Description)+"'";
			}
			if(cvx.IsActive != oldCvx.IsActive) {
				if(command!="") { command+=",";}
				command+="IsActive = '"+POut.String(cvx.IsActive)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE cvx SET "+command
				+" WHERE CvxNum = "+POut.Long(cvx.CvxNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(Cvx,Cvx) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Cvx cvx,Cvx oldCvx) {
			if(cvx.CvxCode != oldCvx.CvxCode) {
				return true;
			}
			if(cvx.Description != oldCvx.Description) {
				return true;
			}
			if(cvx.IsActive != oldCvx.IsActive) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Cvx from the database.</summary>
		public static void Delete(long cvxNum) {
			string command="DELETE FROM cvx "
				+"WHERE CvxNum = "+POut.Long(cvxNum);
			Db.NonQ(command);
		}

	}
}