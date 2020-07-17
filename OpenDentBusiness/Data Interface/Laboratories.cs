using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Laboratories {
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


		///<summary>Refresh all Laboratories</summary>
		public static List<Laboratory> Refresh() {
			
			string command="SELECT * FROM laboratory ORDER BY Description";
			return Crud.LaboratoryCrud.SelectMany(command);
		}

		///<summary>Gets one laboratory from database</summary>
		public static Laboratory GetOne(long laboratoryNum) {
			
			string command="SELECT * FROM laboratory WHERE LaboratoryNum="+POut.Long(laboratoryNum);
			return Crud.LaboratoryCrud.SelectOne(command);
		}

		///<summary></summary>
		public static long Insert(Laboratory laboratory) {
			
			return Crud.LaboratoryCrud.Insert(laboratory);
		}

		///<summary></summary>
		public static void Update(Laboratory laboratory) {
			
			Crud.LaboratoryCrud.Update(laboratory);
		}

		///<summary>Checks dependencies first.  Throws exception if can't delete.</summary>
		public static void Delete(long laboratoryNum) {
			
			string command;
			//check lab cases for dependencies
			command="SELECT LName,FName FROM patient,labcase "
				+"WHERE patient.PatNum=labcase.PatNum "
				+"AND LaboratoryNum ="+POut.Long(laboratoryNum)+" "
				+DbHelper.LimitAnd(30);
			DataTable table=Database.ExecuteDataTable(command);
			if(table.Rows.Count>0){
				string pats="";
				for(int i=0;i<table.Rows.Count;i++){
					pats+="\r";
					pats+=table.Rows[i][0].ToString()+", "+table.Rows[i][1].ToString();
				}
				throw new Exception(Lans.g("Laboratories","Cannot delete Laboratory because cases exist for")+pats);
			}
			//delete
			command= "DELETE FROM laboratory WHERE LaboratoryNum = "+POut.Long(laboratoryNum);
 			Database.ExecuteNonQuery(command);
		}

		
	
		

	}
	


}













