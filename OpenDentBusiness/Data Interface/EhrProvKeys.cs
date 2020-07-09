using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EhrProvKeys{
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
		public static List<EhrProvKey> RefreshForFam(long guarantor){
			
			string command="SELECT ehrprovkey.* FROM ehrprovkey,patient "
				+"WHERE ehrprovkey.PatNum=patient.PatNum "
				+"AND patient.Guarantor="+POut.Long(guarantor)+" "
				+"GROUP BY ehrprovkey.EhrProvKeyNum "
				+"ORDER BY ehrprovkey.LName,ehrprovkey.FName";
			return Crud.EhrProvKeyCrud.SelectMany(command);
		}

		///<summary>Get a list of all EhrProvKeys. Ordered by LName and then YearValue.</summary>
		public static List<EhrProvKey> GetAllKeys() {
			
			string command="SELECT ehrprovkey.* FROM ehrprovkey "
			+"ORDER BY LName,YearValue";
			return Crud.EhrProvKeyCrud.SelectMany(command);
		}

		///<summary>Get a list of all EhrProvKeys for a provider matching the given first and last name.  Ordered by year value.  Returns empty list if lName or fName is empty.</summary>
		public static List<EhrProvKey> GetKeysByFLName(string lName, string fName) {
			
			if(lName==null || fName==null || 
				lName.Trim()=="" || fName.Trim()=="") {
				return new List<EhrProvKey>();
			}
			string command="SELECT ehrprovkey.* FROM ehrprovkey"
			+" WHERE ehrprovkey.LName='"+POut.String(lName)
			+"' AND ehrprovkey.FName='"+POut.String(fName)
			+"' ORDER BY ehrprovkey.YearValue DESC";
			return Crud.EhrProvKeyCrud.SelectMany(command);
		}

		///<summary>Returns true if a provider with the same last and first name passed in has ever had an EHR prov key.</summary>
		public static bool HasProvHadKey(string lName,string fName) {
			
			string command="SELECT COUNT(*) FROM ehrprovkey WHERE ehrprovkey.LName='"+POut.String(lName)+"' AND ehrprovkey.FName='"+POut.String(fName)+"'";
			return Db.GetCount(command)!="0";
		}

		///<summary>True if the ehrprovkey table has any rows, otherwise false.</summary>
		public static bool HasEhrKeys() {
			
			string command="SELECT COUNT(*) FROM ehrprovkey";
			return PIn.Bool(Db.GetScalar(command));
		}

		///<summary></summary>
		public static long Insert(EhrProvKey ehrProvKey){
			
			return Crud.EhrProvKeyCrud.Insert(ehrProvKey);
		}

		///<summary></summary>
		public static void Update(EhrProvKey ehrProvKey){
			
			Crud.EhrProvKeyCrud.Update(ehrProvKey);
		}

		///<summary></summary>
		public static void Delete(long ehrProvKeyNum) {
			
			string command= "DELETE FROM ehrprovkey WHERE EhrProvKeyNum = "+POut.Long(ehrProvKeyNum);
			Db.NonQ(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		

		///<summary>Gets one EhrProvKey from the db.</summary>
		public static EhrProvKey GetOne(long ehrProvKeyNum){
			
			return Crud.EhrProvKeyCrud.SelectOne(ehrProvKeyNum);
		}

		
		*/



	}
}