using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using CodeBase;
using Imedisoft.Data;

namespace OpenDentBusiness{
	///<summary></summary>
	public class PerioExams{
		///<summary>Bad pattern. This is public static because it would be hard to pass it into ContrPerio.  Only used by UI.</summary>
		public static List<PerioExam> ListExams;

		///<summary>Most recent date last.  All exams loaded, even if not displayed.</summary>
		public static void Refresh(long patNum) {
			//No need to check RemotingRole; no call to db.
			DataTable table=GetExamsTable(patNum);
			ListExams=new List<PerioExam>();
			PerioExam exam;
			for(int i=0;i<table.Rows.Count;i++){
				exam=new PerioExam();
				exam.PerioExamNum= PIn.Long   (table.Rows[i][0].ToString());
				exam.PatNum      = PIn.Long(table.Rows[i][1].ToString());
				exam.ExamDate    = PIn.Date(table.Rows[i][2].ToString());
				exam.ProvNum     = PIn.Long(table.Rows[i][3].ToString());
				exam.DateTMeasureEdit     = PIn.Date(table.Rows[i][4].ToString());
				ListExams.Add(exam);
			}
			//return list;
			//PerioMeasures.Refresh(patNum);
		}

		public static DataTable GetExamsTable(long patNum) {
			
			string command=
				"SELECT * from perioexam"
				+" WHERE PatNum = '"+patNum.ToString()+"'"
				+" ORDER BY perioexam.ExamDate";
			DataTable table=Database.ExecuteDataTable(command);
			return table;
		}

		public static List<PerioExam> GetExamsList(long patNum) {
			//No need to check RemotingRole; no call to db.
			return Crud.PerioExamCrud.TableToList(GetExamsTable(patNum));
		}

		///<summary></summary>
		public static void Update(PerioExam Cur){
			
			Crud.PerioExamCrud.Update(Cur);
		}

		///<summary></summary>
		public static long Insert(PerioExam Cur) {
			
			return Crud.PerioExamCrud.Insert(Cur);
		}


		///<summary>Creates a new perio exam for the given patient. Returns that perio exam. Handles setting default skipped teeth/implants. Does not create a security log entry.</summary>
		public static PerioExam CreateNewExam(Patient pat) {
			
			PerioExam newExam=new PerioExam {
				PatNum=pat.PatNum,
				ExamDate=DateTimeOD.Today,
				ProvNum=pat.PriProv,
				DateTMeasureEdit=MiscData.GetNowDateTime()
			};
			Insert(newExam);
			PerioMeasures.SetSkipped(newExam.PerioExamNum,GetSkippedTeethForExam(pat,newExam));
			return newExam;
		}

		///<summary>Returns the toothNums from 1-32 to skip for the given patient.</summary>
		private static List<int> GetSkippedTeethForExam(Patient pat,PerioExam examCur) {
			List<int> listSkippedTeeth=new List<int>();
			List<PerioExam> listOtherExamsForPat=GetExamsList(pat.PatNum)
				.Where(x => x.PerioExamNum!=examCur.PerioExamNum)
				.OrderBy(x => x.ExamDate)
				.ToList();
			//If any other perio exams exist, we'll use the latest exam for the skipped tooth.
			if(!listOtherExamsForPat.IsNullOrEmpty()) {
				listSkippedTeeth=PerioMeasures.GetSkipped(listOtherExamsForPat.Last().PerioExamNum);
			}
			//For patient's first perio chart, any teeth marked missing are automatically marked skipped.
			else if(Prefs.GetBool(PrefName.PerioSkipMissingTeeth)) {
				//Procedures will only be queried for as needed.
				List<Procedure> listProcs=null;
				foreach(string missingTooth in ToothInitials.GetMissingOrHiddenTeeth(ToothInitials.Refresh(pat.PatNum))) {
					if(missingTooth.CompareTo("A")>=0 && missingTooth.CompareTo("Z")<=0) {
						//If is a letter (not a number)
						//Skipped teeth are only recorded by tooth number within the perio exam.
						continue;
					}
					int toothNum=PIn.Int(missingTooth);
					//Check if this tooth has had an implant done AND the office has the preference to SHOW implants
					if(Prefs.GetBool(PrefName.PerioTreatImplantsAsNotMissing)) {
						if(listProcs==null) {
							listProcs=Procedures.Refresh(pat.PatNum);
						}
						if(IsToothImplant(toothNum,listProcs)) {
							//Remove the tooth from the list of skipped teeth if it exists.
							listSkippedTeeth.RemoveAll(x => x==toothNum);
							//We do not want to add it back to the list below.
							continue;
						}
					}
					//This tooth is missing and we know it is not an implant OR the office has the preference to ignore implants.
					//Simply add it to our list of skipped teeth.
					listSkippedTeeth.Add(toothNum);
				}
			}
			return listSkippedTeeth;
		}

		///<summary>Returns true if the toothNum passed in has ever had an implant before. Based on the given patient procedures.</summary>
		private static bool IsToothImplant(int toothNum,List<Procedure> listProcsForPatient) {
			return listProcsForPatient
				.FindAll(x => x.ToothNum==toothNum.ToString() && x.ProcStatus.In(ProcStat.C,ProcStat.EC,ProcStat.EO))
				//ProcedureCodes are cached.
				.Any(x => ProcedureCodes.GetProcCode(x.CodeNum).PaintType==ToothPaintingType.Implant);
		}

		///<summary></summary>
		public static void Delete(PerioExam Cur){
			
			string command= "DELETE from perioexam WHERE PerioExamNum = '"+Cur.PerioExamNum.ToString()+"'";
			Database.ExecuteNonQuery(command);
			command= "DELETE from periomeasure WHERE PerioExamNum = '"+Cur.PerioExamNum.ToString()+"'";
			Database.ExecuteNonQuery(command);
		}

		///<summary>Used by PerioMeasures when refreshing to organize array.</summary>
		public static int GetExamIndex(List<PerioExam> list,long perioExamNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<list.Count;i++) {
				if(list[i].PerioExamNum==perioExamNum) {
					return i;
				}
			}
			//MessageBox.Show("Error. PerioExamNum not in list: "+perioExamNum.ToString());
			return 0;
		}

		///<summary>Used by ContrPerio to get a perio exam.</summary>
		public static PerioExam GetOnePerioExam(long perioExamNum) {
			
			return Crud.PerioExamCrud.SelectOne(perioExamNum);
		}

	}
}