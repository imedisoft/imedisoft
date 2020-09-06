using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using Imedisoft.Data;
using Imedisoft.Data.Models;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EduResources{
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
		public static List<EduResource> GenerateForPatient(long patNum){
			
			List<Problem> diseaseList = Problems.GetByPatient(patNum).ToList();
			List<MedicationPat> medicationPatList = MedicationPats.Refresh(patNum,false);
			List<LabResult> labResultList = LabResults.GetAllForPatient(patNum);
			List<EhrLabResult> listEhrLabResults= EhrLabResults.GetAllForPatient(patNum);
			List<EduResource> eduResourceListAll = Crud.EduResourceCrud.SelectMany("SELECT * FROM eduresource");
			List<EhrMeasureEvent> listTobaccoEvents=EhrMeasureEvents.RefreshByType(patNum,EhrMeasureEventType.TobaccoUseAssessed)
				.FindAll(x => x.CodeSystemResult=="SNOMEDCT");
			List<EduResource> retVal = new List<EduResource>();
			eduResourceListAll.FindAll(x => x.DiseaseDefNum!=0 && diseaseList.Any(y => y.ProblemDefId==x.DiseaseDefNum)).ForEach(x => retVal.Add(x));
			eduResourceListAll.FindAll(x => x.MedicationNum!=0
					&& medicationPatList.Any(y => y.MedicationNum==x.MedicationNum 
						|| (y.MedicationNum==0 && Medications.GetMedication(x.MedicationNum).RxCui==y.RxCui)))
				.ForEach(x => retVal.Add(x));
			eduResourceListAll.FindAll(x => x.SmokingSnoMed!="" && listTobaccoEvents.Any(y => y.CodeValueResult==x.SmokingSnoMed)).ForEach(x => retVal.Add(x));
			foreach(EduResource resourceCur in eduResourceListAll.Where(x => x.LabResultID!="")) {
				foreach(LabResult labResult in labResultList.Where(x => x.TestID==resourceCur.LabResultID)) {
					if(resourceCur.LabResultCompare.StartsWith("<")){
						//PIn.Int not used because blank not allowed.
						try{
							if(int.Parse(labResult.ObsValue) < int.Parse(resourceCur.LabResultCompare.Substring(1))){
								retVal.Add(resourceCur);
							}
						}
						catch{
							//This could only happen if the validation in either input didn't work.
						}
					}
					else if(resourceCur.LabResultCompare.StartsWith(">")){
						try {
							if(int.Parse(labResult.ObsValue) > int.Parse(resourceCur.LabResultCompare.Substring(1))) {
								retVal.Add(resourceCur);
							}
						}
						catch {
							//This could only happen if the validation in either input didn't work.
						}
					}
				}//end LabResultList
				foreach(EhrLabResult ehrLabResult in listEhrLabResults.Where(x => x.ObservationIdentifierID==resourceCur.LabResultID)) {//matches loinc only.
					if(retVal.Contains(resourceCur)){
						continue;//already added from loop above.
					}
					retVal.Add(resourceCur);
				}//end EhrLabResults
			}
			return retVal;
		}

		///<summary></summary>
		public static List<EduResource> SelectAll(){
			
			string command="SELECT * FROM eduresource";
			return Crud.EduResourceCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void Delete(long eduResourceNum) {
			
			string command= "DELETE FROM eduresource WHERE EduResourceNum = "+POut.Long(eduResourceNum);
			Database.ExecuteNonQuery(command);
		}

		///<summary></summary>
		public static long Insert(EduResource eduResource) {
			
			return Crud.EduResourceCrud.Insert(eduResource);
		}

		///<summary></summary>
		public static void Update(EduResource eduResource) {
			
			Crud.EduResourceCrud.Update(eduResource);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<EduResource> Refresh(long patNum){
			
			string command="SELECT * FROM eduresource WHERE PatNum = "+POut.Long(patNum);
			return Crud.EduResourceCrud.SelectMany(command);
		}

		///<summary>Gets one EduResource from the db.</summary>
		public static EduResource GetOne(long eduResourceNum){
			
			return Crud.EduResourceCrud.SelectOne(eduResourceNum);
		}



		*/



	}
}