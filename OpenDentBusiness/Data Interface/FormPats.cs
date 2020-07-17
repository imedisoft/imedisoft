using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenDentBusiness{
	///<summary></summary>
	public class FormPats{
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
		public static long Insert(FormPat Cur) {
			
			return Crud.FormPatCrud.Insert(Cur);
		}

		public static FormPat GetOne(long formPatNum) {
			
			string command= "SELECT * FROM formpat WHERE FormPatNum="+POut.Long(formPatNum);
			FormPat formpat=Crud.FormPatCrud.SelectOne(formPatNum);
			if(formpat==null){
				return null;//should never happen.
			}
			command="SELECT * FROM question WHERE FormPatNum="+POut.Long(formPatNum);
			formpat.QuestionList=Crud.QuestionCrud.SelectMany(command);
			return formpat;
		}

		///<summary></summary>
		public static void Delete(long formPatNum) {
			
			string command="DELETE FROM formpat WHERE FormPatNum="+POut.Long(formPatNum);
			Database.ExecuteNonQuery(command);
			command="DELETE FROM question WHERE FormPatNum="+POut.Long(formPatNum);
			Database.ExecuteNonQuery(command);
		}


	}

	
	

}













