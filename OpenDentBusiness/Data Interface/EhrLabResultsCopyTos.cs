using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EhrLabResultsCopyTos {
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
		public static List<EhrLabResultsCopyTo> GetForLab(long ehrLabNum) {
			
			string command="SELECT * FROM ehrlabresultscopyto WHERE EhrLabNum = "+POut.Long(ehrLabNum);
			return Crud.EhrLabResultsCopyToCrud.SelectMany(command);
		}

		///<summary>Deletes notes for lab results too.</summary>
		public static void DeleteForLab(long ehrLabNum) {
			
			string command="DELETE FROM ehrlabresultscopyto WHERE EhrLabNum = "+POut.Long(ehrLabNum);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static long Insert(EhrLabResultsCopyTo ehrLabResultsCopyTo) {
			
			return Crud.EhrLabResultsCopyToCrud.Insert(ehrLabResultsCopyTo);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<EhrLabResultsCopyTo> Refresh(long patNum){
			
			string command="SELECT * FROM ehrlabresultscopyto WHERE PatNum = "+POut.Long(patNum);
			return Crud.EhrLabResultsCopyToCrud.SelectMany(command);
		}

		///<summary>Gets one EhrLabResultsCopyTo from the db.</summary>
		public static EhrLabResultsCopyTo GetOne(long ehrLabResultsCopyToNum){
			
			return Crud.EhrLabResultsCopyToCrud.SelectOne(ehrLabResultsCopyToNum);
		}

		///<summary></summary>
		public static void Update(EhrLabResultsCopyTo ehrLabResultsCopyTo){
			
			Crud.EhrLabResultsCopyToCrud.Update(ehrLabResultsCopyTo);
		}

		///<summary></summary>
		public static void Delete(long ehrLabResultsCopyToNum) {
			
			string command= "DELETE FROM ehrlabresultscopyto WHERE EhrLabResultsCopyToNum = "+POut.Long(ehrLabResultsCopyToNum);
			Db.NonQ(command);
		}
		*/



	}
}