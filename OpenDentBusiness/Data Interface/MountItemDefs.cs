using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using Imedisoft.Data;
using OpenDentBusiness;

namespace OpenDentBusiness {
	///<summary></summary>
	public class MountItemDefs {
		public static List<MountItemDef> GetForMountDef(long mountDefNum){
			
			string command="SELECT * FROM mountitemdef WHERE MountDefNum='"+POut.Long(mountDefNum)+"' ORDER BY ItemOrder";
			return Crud.MountItemDefCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void Update(MountItemDef mountItemDef) {
			
			Crud.MountItemDefCrud.Update(mountItemDef);
		}

		///<summary></summary>
		public static long Insert(MountItemDef mountItemDef) {
			
			return Crud.MountItemDefCrud.Insert(mountItemDef);
		}

		///<summary>No need to surround with try/catch, because all deletions are allowed.</summary>
		public static void Delete(long mountItemDefNum) {
			
			string command="DELETE FROM mountitemdef WHERE MountItemDefNum="+POut.Long(mountItemDefNum);
			Database.ExecuteNonQuery(command);
		}

		///<summary></summary>
		public static void DeleteForMount(long mountDefNum) {
			
			string command="DELETE FROM mountitemdef WHERE MountDefNum="+POut.Long(mountDefNum);
			Database.ExecuteNonQuery(command);
		}
		
		
	}

		



		
	

	

	


}










