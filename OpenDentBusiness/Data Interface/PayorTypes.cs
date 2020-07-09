using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class PayorTypes{
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
		public static List<PayorType> Refresh(long patNum){
			
			string command="SELECT * FROM payortype WHERE PatNum = "+POut.Long(patNum)+" ORDER BY DateStart";
			return Crud.PayorTypeCrud.SelectMany(command);
		}

		///<summary>This will return "SopCode - Description" or empty string if the patient does not have a payor type entered.</summary>
		public static string GetCurrentDescription(long patNum) {
			
			PayorType payorType=GetCurrentType(patNum);
			if(payorType==null) {
				return "";
			}
			return payorType.SopCode+" - "+Sops.GetDescriptionFromCode(payorType.SopCode);
		}

		///<summary>Gets most recent PayorType for a patient.</summary>
		public static PayorType GetCurrentType(long patNum) {
			
			string command=DbHelper.LimitOrderBy("SELECT * FROM payortype WHERE PatNum="+POut.Long(patNum)+" ORDER BY DateStart DESC",1);
			return Crud.PayorTypeCrud.SelectOne(command);
		}

		///<summary>Gets one PayorType from the db.</summary>
		public static PayorType GetOne(long payorTypeNum){
			
			return Crud.PayorTypeCrud.SelectOne(payorTypeNum);
		}

		///<summary></summary>
		public static long Insert(PayorType payorType){
			
			return Crud.PayorTypeCrud.Insert(payorType);
		}

		///<summary></summary>
		public static void Update(PayorType payorType){
			
			Crud.PayorTypeCrud.Update(payorType);
		}

		///<summary></summary>
		public static void Delete(long payorTypeNum) {
			
			string command= "DELETE FROM payortype WHERE PayorTypeNum = "+POut.Long(payorTypeNum);
			Db.NonQ(command);
		}



	}
}