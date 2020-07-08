//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EbillCrud {
		///<summary>Gets one Ebill object from the database using the primary key.  Returns null if not found.</summary>
		public static Ebill SelectOne(long ebillNum) {
			string command="SELECT * FROM ebill "
				+"WHERE EbillNum = "+POut.Long(ebillNum);
			List<Ebill> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Ebill object from the database using a query.</summary>
		public static Ebill SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Ebill> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Ebill objects from the database using a query.</summary>
		public static List<Ebill> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Ebill> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Ebill> TableToList(DataTable table) {
			List<Ebill> retVal=new List<Ebill>();
			Ebill ebill;
			foreach(DataRow row in table.Rows) {
				ebill=new Ebill();
				ebill.EbillNum        = PIn.Long  (row["EbillNum"].ToString());
				ebill.ClinicNum       = PIn.Long  (row["ClinicNum"].ToString());
				ebill.ClientAcctNumber= PIn.String(row["ClientAcctNumber"].ToString());
				ebill.ElectUserName   = PIn.String(row["ElectUserName"].ToString());
				ebill.ElectPassword   = PIn.String(row["ElectPassword"].ToString());
				ebill.PracticeAddress = (OpenDentBusiness.EbillAddress)PIn.Int(row["PracticeAddress"].ToString());
				ebill.RemitAddress    = (OpenDentBusiness.EbillAddress)PIn.Int(row["RemitAddress"].ToString());
				retVal.Add(ebill);
			}
			return retVal;
		}

		///<summary>Converts a list of Ebill into a DataTable.</summary>
		public static DataTable ListToTable(List<Ebill> listEbills,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Ebill";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EbillNum");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("ClientAcctNumber");
			table.Columns.Add("ElectUserName");
			table.Columns.Add("ElectPassword");
			table.Columns.Add("PracticeAddress");
			table.Columns.Add("RemitAddress");
			foreach(Ebill ebill in listEbills) {
				table.Rows.Add(new object[] {
					POut.Long  (ebill.EbillNum),
					POut.Long  (ebill.ClinicNum),
					            ebill.ClientAcctNumber,
					            ebill.ElectUserName,
					            ebill.ElectPassword,
					POut.Int   ((int)ebill.PracticeAddress),
					POut.Int   ((int)ebill.RemitAddress),
				});
			}
			return table;
		}

		///<summary>Inserts one Ebill into the database.  Returns the new priKey.</summary>
		public static long Insert(Ebill ebill) {
			return Insert(ebill,false);
		}

		///<summary>Inserts one Ebill into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Ebill ebill,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				ebill.EbillNum=ReplicationServers.GetKey("ebill","EbillNum");
			}
			string command="INSERT INTO ebill (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EbillNum,";
			}
			command+="ClinicNum,ClientAcctNumber,ElectUserName,ElectPassword,PracticeAddress,RemitAddress) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(ebill.EbillNum)+",";
			}
			command+=
				     POut.Long  (ebill.ClinicNum)+","
				+"'"+POut.String(ebill.ClientAcctNumber)+"',"
				+"'"+POut.String(ebill.ElectUserName)+"',"
				+"'"+POut.String(ebill.ElectPassword)+"',"
				+    POut.Int   ((int)ebill.PracticeAddress)+","
				+    POut.Int   ((int)ebill.RemitAddress)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				ebill.EbillNum=Db.NonQ(command,true,"EbillNum","ebill");
			}
			return ebill.EbillNum;
		}

		///<summary>Inserts one Ebill into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Ebill ebill) {
			return InsertNoCache(ebill,false);
		}

		///<summary>Inserts one Ebill into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Ebill ebill,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO ebill (";
			if(!useExistingPK && isRandomKeys) {
				ebill.EbillNum=ReplicationServers.GetKeyNoCache("ebill","EbillNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EbillNum,";
			}
			command+="ClinicNum,ClientAcctNumber,ElectUserName,ElectPassword,PracticeAddress,RemitAddress) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(ebill.EbillNum)+",";
			}
			command+=
				     POut.Long  (ebill.ClinicNum)+","
				+"'"+POut.String(ebill.ClientAcctNumber)+"',"
				+"'"+POut.String(ebill.ElectUserName)+"',"
				+"'"+POut.String(ebill.ElectPassword)+"',"
				+    POut.Int   ((int)ebill.PracticeAddress)+","
				+    POut.Int   ((int)ebill.RemitAddress)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				ebill.EbillNum=Db.NonQ(command,true,"EbillNum","ebill");
			}
			return ebill.EbillNum;
		}

		///<summary>Updates one Ebill in the database.</summary>
		public static void Update(Ebill ebill) {
			string command="UPDATE ebill SET "
				+"ClinicNum       =  "+POut.Long  (ebill.ClinicNum)+", "
				+"ClientAcctNumber= '"+POut.String(ebill.ClientAcctNumber)+"', "
				+"ElectUserName   = '"+POut.String(ebill.ElectUserName)+"', "
				+"ElectPassword   = '"+POut.String(ebill.ElectPassword)+"', "
				+"PracticeAddress =  "+POut.Int   ((int)ebill.PracticeAddress)+", "
				+"RemitAddress    =  "+POut.Int   ((int)ebill.RemitAddress)+" "
				+"WHERE EbillNum = "+POut.Long(ebill.EbillNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Ebill in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Ebill ebill,Ebill oldEbill) {
			string command="";
			if(ebill.ClinicNum != oldEbill.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(ebill.ClinicNum)+"";
			}
			if(ebill.ClientAcctNumber != oldEbill.ClientAcctNumber) {
				if(command!="") { command+=",";}
				command+="ClientAcctNumber = '"+POut.String(ebill.ClientAcctNumber)+"'";
			}
			if(ebill.ElectUserName != oldEbill.ElectUserName) {
				if(command!="") { command+=",";}
				command+="ElectUserName = '"+POut.String(ebill.ElectUserName)+"'";
			}
			if(ebill.ElectPassword != oldEbill.ElectPassword) {
				if(command!="") { command+=",";}
				command+="ElectPassword = '"+POut.String(ebill.ElectPassword)+"'";
			}
			if(ebill.PracticeAddress != oldEbill.PracticeAddress) {
				if(command!="") { command+=",";}
				command+="PracticeAddress = "+POut.Int   ((int)ebill.PracticeAddress)+"";
			}
			if(ebill.RemitAddress != oldEbill.RemitAddress) {
				if(command!="") { command+=",";}
				command+="RemitAddress = "+POut.Int   ((int)ebill.RemitAddress)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE ebill SET "+command
				+" WHERE EbillNum = "+POut.Long(ebill.EbillNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(Ebill,Ebill) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Ebill ebill,Ebill oldEbill) {
			if(ebill.ClinicNum != oldEbill.ClinicNum) {
				return true;
			}
			if(ebill.ClientAcctNumber != oldEbill.ClientAcctNumber) {
				return true;
			}
			if(ebill.ElectUserName != oldEbill.ElectUserName) {
				return true;
			}
			if(ebill.ElectPassword != oldEbill.ElectPassword) {
				return true;
			}
			if(ebill.PracticeAddress != oldEbill.PracticeAddress) {
				return true;
			}
			if(ebill.RemitAddress != oldEbill.RemitAddress) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Ebill from the database.</summary>
		public static void Delete(long ebillNum) {
			string command="DELETE FROM ebill "
				+"WHERE EbillNum = "+POut.Long(ebillNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<Ebill> listNew,List<Ebill> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<Ebill> listIns    =new List<Ebill>();
			List<Ebill> listUpdNew =new List<Ebill>();
			List<Ebill> listUpdDB  =new List<Ebill>();
			List<Ebill> listDel    =new List<Ebill>();
			listNew.Sort((Ebill x,Ebill y) => { return x.EbillNum.CompareTo(y.EbillNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((Ebill x,Ebill y) => { return x.EbillNum.CompareTo(y.EbillNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			Ebill fieldNew;
			Ebill fieldDB;
			//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.
			//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.
			while(idxNew<listNew.Count || idxDB<listDB.Count) {
				fieldNew=null;
				if(idxNew<listNew.Count) {
					fieldNew=listNew[idxNew];
				}
				fieldDB=null;
				if(idxDB<listDB.Count) {
					fieldDB=listDB[idxDB];
				}
				//begin compare
				if(fieldNew!=null && fieldDB==null) {//listNew has more items, listDB does not.
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew==null && fieldDB!=null) {//listDB has more items, listNew does not.
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				else if(fieldNew.EbillNum<fieldDB.EbillNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.EbillNum>fieldDB.EbillNum) {//dbPK less than newPK, dbItem is 'next'
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				//Both lists contain the 'next' item, update required
				listUpdNew.Add(fieldNew);
				listUpdDB.Add(fieldDB);
				idxNew++;
				idxDB++;
			}
			//Commit changes to DB
			for(int i=0;i<listIns.Count;i++) {
				Insert(listIns[i]);
			}
			for(int i=0;i<listUpdNew.Count;i++) {
				if(Update(listUpdNew[i],listUpdDB[i])) {
					rowsUpdatedCount++;
				}
			}
			for(int i=0;i<listDel.Count;i++) {
				Delete(listDel[i].EbillNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}