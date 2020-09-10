using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Encounters{
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
		public static List<Encounter> Refresh(long patNum){
			
			string command="SELECT * FROM encounter WHERE PatNum = "+POut.Long(patNum)+" ORDER BY DateEncounter";
			return Crud.EncounterCrud.SelectMany(command);
		}

		///<summary>Gets one Encounter from the db.</summary>
		public static Encounter GetOne(long encounterNum) {
			
			return Crud.EncounterCrud.SelectOne(encounterNum);
		}

		///<summary>Automatically generate and insert encounter as long as there is no other encounter with that date and provider for that patient.  Does not insert an encounter if one of the CQM default encounter prefs are invalid.</summary>
		public static void InsertDefaultEncounter(long patNum, long provNum, DateTime date) {
			
			//Validate prefs. If they are not set, we have nothing to insert so no reason to check.
			if(Preferences.GetString(PreferenceName.CQMDefaultEncounterCodeSystem)=="" || Preferences.GetString(PreferenceName.CQMDefaultEncounterCodeValue)=="none"){
				return;
			}
			//If no encounter for date for this patient
			string command="SELECT COUNT(*) NumEncounters FROM encounter WHERE encounter.PatNum="+POut.Long(patNum)+" "
				+"AND encounter.DateEncounter="+POut.Date(date)+" "
				+"AND encounter.ProvNum="+POut.Long(provNum);
			int count=PIn.Int(Database.ExecuteString(command));
			if(count > 0) { //Encounter already exists for date
				return;
			}
			//Insert encounter with default encounter code system and code value set in Setup>EHR>Settings
			Encounter encounter = new Encounter();
			encounter.PatNum=patNum;
			encounter.ProvNum=provNum;
			encounter.DateEncounter=date;
			encounter.CodeSystem=Preferences.GetString(PreferenceName.CQMDefaultEncounterCodeSystem);
			encounter.CodeValue=Preferences.GetString(PreferenceName.CQMDefaultEncounterCodeValue);
			Insert(encounter);
		}

		///<summary>Inserts encounters for a specified code for a specified date range if there is not already an encounter for that code, patient, 
		///provider, and procdate.</summary>
		public static long InsertEncsFromProcDates(DateTime startDate,DateTime endDate,string codeValue,string codeSystem) {
			
			string command="INSERT INTO encounter (PatNum,ProvNum,CodeValue,CodeSystem,Note,DateEncounter) "
        +"(SELECT PatNum,ProvNum,'"+POut.String(codeValue)+"','"+POut.String(codeSystem)+"',"
        +"'Encounter auto-generated by Open Dental based on completed procedure information.',ProcDate "
        +"FROM procedurelog "
        +"WHERE ProcStatus=2 "
        +"AND ProcDate BETWEEN "+POut.Date(startDate)+" AND "+POut.Date(endDate)+" "
        +"AND NOT EXISTS("//Don't insert if there's already an encounter with this code for this pat,prov,date
        +"SELECT * FROM encounter "
        +"WHERE encounter.PatNum=procedurelog.PatNum "
        +"AND encounter.ProvNum=procedurelog.ProvNum "
        +"AND encounter.DateEncounter=procedurelog.ProcDate "
        +"AND encounter.CodeValue='"+POut.String(codeValue)+"' "
        +"AND encounter.CodeSystem='"+POut.String(codeSystem)+"') "
        +"GROUP BY PatNum,ProvNum,ProcDate)";
			return Database.ExecuteInsert(command);
		}

		///<summary></summary>
		public static long Insert(Encounter encounter) {
			
			return Crud.EncounterCrud.Insert(encounter);
		}

		///<summary></summary>
		public static void Update(Encounter encounter) {
			
			Crud.EncounterCrud.Update(encounter);
		}

		///<summary></summary>
		public static void Delete(long encounterNum) {
			
			string command= "DELETE FROM encounter WHERE EncounterNum = "+POut.Long(encounterNum);
			Database.ExecuteNonQuery(command);
		}


	}
}