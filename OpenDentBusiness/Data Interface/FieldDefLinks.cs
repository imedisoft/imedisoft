using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class FieldDefLinks{
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion


		///<summary></summary>
		public static List<FieldDefLink> GetAll() {
			
			string command="SELECT * FROM fielddeflink";
			return Crud.FieldDefLinkCrud.SelectMany(command);
		}

		///<summary>Gets a list of FieldDefLinks for a specified location.</summary>
		public static List<FieldDefLink> GetForLocation(FieldLocations fieldLocation) {
			
			string command="SELECT * FROM fielddeflink WHERE FieldLocation="+POut.Int((int)fieldLocation);
			return Crud.FieldDefLinkCrud.SelectMany(command);
		}

		public static bool Sync(List<FieldDefLink> listNew) {
			
			string command="SELECT * FROM fielddeflink";
			List<FieldDefLink> listDB=Crud.FieldDefLinkCrud.SelectMany(command);
			return Crud.FieldDefLinkCrud.Sync(listNew,listDB);
		}

		///<summary>Deletes all fieldDefLink rows that are associated to the given fieldDefNum and fieldDefType.</summary>
		public static void DeleteForFieldDefNum(long fieldDefNum,FieldDefTypes fieldDefType) {
			
			if(fieldDefNum==0) {
					return;
			}
			//Only delete records of the correct fieldDefType (Pat vs Appt)
			Database.ExecuteNonQuery("DELETE FROM fieldDefLink WHERE FieldDefNum="+POut.Long(fieldDefNum)+" AND FieldDefType="+POut.Int((int)fieldDefType));
		}

	}
}