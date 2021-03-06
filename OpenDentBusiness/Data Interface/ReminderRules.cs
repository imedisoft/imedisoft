using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ReminderRules{
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
		public static long Insert(ReminderRule reminderRule) {
			
			return Crud.ReminderRuleCrud.Insert(reminderRule);
		}

		///<summary></summary>
		public static void Update(ReminderRule reminderRule) {
			
			Crud.ReminderRuleCrud.Update(reminderRule);
		}

		///<summary></summary>
		public static void Delete(long reminderRuleNum) {
			
			string command= "DELETE FROM reminderrule WHERE ReminderRuleNum = "+POut.Long(reminderRuleNum);
			Database.ExecuteNonQuery(command);
		}

		///<summary></summary>
		public static List<ReminderRule> SelectAll() {
			
			string command="SELECT * FROM reminderrule";
			return Crud.ReminderRuleCrud.SelectMany(command);
		}

		public static List<ReminderRule> GetRemindersForPatient(Patient PatCur) {
			
			//Problem,Medication,Allergy,Age,Gender,LabResult
			List<ReminderRule> fullListReminders = Crud.ReminderRuleCrud.SelectMany("SELECT * FROM reminderrule");
			List<ReminderRule> retVal = new List<ReminderRule>();
			List<Problem> listProblems = Problems.GetByPatient(PatCur.PatNum).ToList();
			List<Medication> listMedications = Medications.GetByPatientNoCache(PatCur.PatNum).ToList();
			List<Allergy> listAllergies = Allergies.GetByPatient(PatCur.PatNum).ToList();
			List<LabResult> listLabResults = LabResults.GetAllForPatient(PatCur.PatNum);
			for(int i=0;i<fullListReminders.Count;i++) {
				switch(fullListReminders[i].ReminderCriterion) {
					case EhrCriterion.Problem:
						for(int j=0;j<listProblems.Count;j++) {
							if(fullListReminders[i].CriterionFK==listProblems[j].ProblemDefId) {
								retVal.Add(fullListReminders[i]);
								break;
							}
						}
						break;
					case EhrCriterion.Medication:
						for(int j=0;j<listMedications.Count;j++) {
							if(fullListReminders[i].CriterionFK==listMedications[j].Id) {
								retVal.Add(fullListReminders[i]);
								break;
							}
						}
						break;
					case EhrCriterion.Allergy:
						for(int j=0;j<listAllergies.Count;j++) {
							if(fullListReminders[i].CriterionFK==listAllergies[j].AllergyDefId) {
								retVal.Add(fullListReminders[i]);
								break;
							}
						}
						break;
					case EhrCriterion.Age:
						if(fullListReminders[i].CriterionValue[0]=='<') {
							if(PatCur.Age<int.Parse(fullListReminders[i].CriterionValue.Substring(1,fullListReminders[i].CriterionValue.Length-1))){
								retVal.Add(fullListReminders[i]);
							}
						}
						else if(fullListReminders[i].CriterionValue[0]=='>'){
							if(PatCur.Age>int.Parse(fullListReminders[i].CriterionValue.Substring(1,fullListReminders[i].CriterionValue.Length-1))){
								retVal.Add(fullListReminders[i]);
							}
						}
						else{
							//This section should never be reached
						}
						break;
					case EhrCriterion.Gender:
						if(PatCur.Gender.ToString().ToLower()==fullListReminders[i].CriterionValue.ToLower()){
							retVal.Add(fullListReminders[i]);
						}
						break;
					case EhrCriterion.LabResult:
						for(int j=0;j<listLabResults.Count;j++) {
							if(listLabResults[j].TestName.ToLower().Contains(fullListReminders[i].CriterionValue.ToLower())) {				
								retVal.Add(fullListReminders[i]);
								break;
							}
						}
						break;
					//case EhrCriterion.ICD9:
					//  for(int j=0;j<listProblems.Count;j++) {
					//    if(fullListReminders[i].CriterionFK==listProblems[j].DiseaseDefNum) {
					//      retVal.Add(fullListReminders[i]);
					//      break;
					//    }
					//  }
					//  break;
				}
			}
			return retVal;
		}


		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<ReminderRule> Refresh(long patNum){
			
			string command="SELECT * FROM reminderrule WHERE PatNum = "+POut.Long(patNum);
			return Crud.ReminderRuleCrud.SelectMany(command);
		}

		///<summary>Gets one ReminderRule from the db.</summary>
		public static ReminderRule GetOne(long reminderRuleNum){
			
			return Crud.ReminderRuleCrud.SelectOne(reminderRuleNum);
		}


		*/



	}
}