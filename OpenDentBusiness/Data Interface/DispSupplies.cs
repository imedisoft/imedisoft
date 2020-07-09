using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class DispSupplies{
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
		public static DataTable RefreshDispensary(long provNum) {
			
			string command="SELECT supply.Descript,dispsupply.DateDispensed,dispsupply.DispQuantity,dispsupply.Note "
				+"FROM dispsupply LEFT JOIN supply ON dispsupply.SupplyNum=supply.SupplyNum "
					+"WHERE dispsupply.ProvNum="+POut.Long(provNum)+" "
					+"ORDER BY DateDispensed,Descript";
			return Db.GetTable(command);
		}

		///<summary></summary>
		public static long Insert(DispSupply dispSupply){
			
			return Crud.DispSupplyCrud.Insert(dispSupply);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<DispSupply> Refresh(long patNum){
			
			string command="SELECT * FROM dispsupply WHERE PatNum = "+POut.Long(patNum);
			return Crud.DispSupplyCrud.SelectMany(command);
		}

		///<summary>Gets one DispSupply from the db.</summary>
		public static DispSupply GetOne(long dispSupplyNum){
			
			return Crud.DispSupplyCrud.SelectOne(dispSupplyNum);
		}

		

		///<summary></summary>
		public static void Update(DispSupply dispSupply){
			
			Crud.DispSupplyCrud.Update(dispSupply);
		}

		///<summary></summary>
		public static void Delete(long dispSupplyNum) {
			
			string command= "DELETE FROM dispsupply WHERE DispSupplyNum = "+POut.Long(dispSupplyNum);
			Db.NonQ(command);
		}
		*/



	}
}