using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Vitalsigns{
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
		public static List<Vitalsign> Refresh(long patNum) {
			
			string command="SELECT * FROM vitalsign WHERE PatNum = "+POut.Long(patNum)+" ORDER BY DateTaken";
			return Crud.VitalsignCrud.SelectMany(command);
		}

		///<summary>Gets one Vitalsign from the db.</summary>
		public static Vitalsign GetOne(long vitalsignNum){
			
			return Crud.VitalsignCrud.SelectOne(vitalsignNum);
		}

		///<summary>Get vitalsign that this EhrNotPerformed object is linked to. Returns null if not found.</summary>
		public static Vitalsign GetFromEhrNotPerformedNum(long ehrNotPerfNum) {
			
			string command="SELECT * FROM vitalsign WHERE EhrNotPerformedNum="+POut.Long(ehrNotPerfNum);
			return Crud.VitalsignCrud.SelectOne(command);
		}

		///<summary>Gets one Vitalsign with the given DiseaseNum as the PregDiseaseNum.</summary>
		public static List<Vitalsign> GetListFromPregDiseaseNum(long pregDiseaseNum) {
			string command="SELECT * FROM vitalsign WHERE vitalsign.PregDiseaseNum="+POut.Long(pregDiseaseNum);
			return Crud.VitalsignCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(Vitalsign vitalsign){
			
			return Crud.VitalsignCrud.Insert(vitalsign);
		}

		///<summary></summary>
		public static void Update(Vitalsign vitalsign){
			
			Crud.VitalsignCrud.Update(vitalsign);
		}

		///<summary></summary>
		public static void Delete(long vitalsignNum) {
			
			string command= "DELETE FROM vitalsign WHERE VitalsignNum = "+POut.Long(vitalsignNum);
			Database.ExecuteNonQuery(command);
		}

		public static float CalcBMI(float weight,float height) {
			//No need to check RemotingRole; no call to db.
			//BMI = (lbs*703)/(in^2)
			if(weight==0 || height==0) {
				return 0;
			}
			float bmi = (float)((weight*703f)/(height*height));
			return bmi;
		}


	}
}