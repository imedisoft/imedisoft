using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using OpenDentBusiness;
using CodeBase;
using Imedisoft.Data;

namespace OpenDentBusiness {
	public class MountItems {
		public static long Insert(MountItem mountItem) {
			
			return Crud.MountItemCrud.Insert(mountItem);
		}
		/*
		public static void Update(MountItem mountItem) {
			
			Crud.MountItemCrud.Update(mountItem);
		}*/

		public static void Delete(MountItem mountItem) {
			
			string command="DELETE FROM mountitem WHERE MountItemNum='"+POut.Long(mountItem.MountItemNum)+"'";
			Database.ExecuteNonQuery(command);
		}

		///<summary>Returns the list of mount items associated with the given mount key. In order by ItemOrder, which is 1-indexed.</summary>
		public static List<MountItem> GetItemsForMount(long mountNum) {
			
			string command="SELECT * FROM mountitem WHERE MountNum='"+POut.Long(mountNum)+"' ORDER BY ItemOrder";
			return Crud.MountItemCrud.SelectMany(command);
		}

	}
}