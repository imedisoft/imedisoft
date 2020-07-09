using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ConnectionGroups{
		///<summary></summary>
		public static List<ConnectionGroup> GetAll(){
			
			string command="SELECT * FROM connectiongroup ORDER BY Description";
			return Crud.ConnectionGroupCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(ConnectionGroup connectionGroup){
			
			return Crud.ConnectionGroupCrud.Insert(connectionGroup);
		}

		///<summary></summary>
		public static void Update(ConnectionGroup connectionGroup){
			
			Crud.ConnectionGroupCrud.Update(connectionGroup);
		}

		///<summary></summary>
		public static void Delete(long connectionGroupNum) {
			
			string command= "DELETE FROM connectiongroup WHERE ConnectionGroupNum = "+POut.Long(connectionGroupNum);
			Db.NonQ(command);
		}
	}
}