using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EvaluationDefs{
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

		///<summary>Gets all EvaluationDefs from the DB.</summary>
		public static List<EvaluationDef> Refresh(){
			
			string command="SELECT * FROM evaluationdef";
			return Crud.EvaluationDefCrud.SelectMany(command);
		}

		///<summary>Gets all EvaluationDefs from the DB that are attached to the specified course. If course is blank then it will get all of the defs.</summary>
		public static DataTable GetAllByCourse(long schoolCourseNum) {
			
			string command="SELECT evaluationdef.EvaluationDefNum, evaluationdef.EvalTitle, schoolcourse.CourseID FROM evaluationdef "
				+"INNER JOIN schoolcourse ON schoolcourse.SchoolCourseNum=evaluationdef.SchoolCourseNum "
				+"WHERE TRUE";
			if(schoolCourseNum!=0) {
				command+=" AND schoolcourse.SchoolCourseNum = '"+POut.Long(schoolCourseNum)+"'";
			}
			command+=" ORDER BY CourseID,EvalTitle";
			return Database.ExecuteDataTable(command);
		}

		///<summary>Gets one EvaluationDef from the db.</summary>
		public static EvaluationDef GetOne(long evaluationDefNum){
			
			return Crud.EvaluationDefCrud.SelectOne(evaluationDefNum);
		}

		///<summary></summary>
		public static long Insert(EvaluationDef evaluationDef){
			
			return Crud.EvaluationDefCrud.Insert(evaluationDef);
		}

		///<summary></summary>
		public static void Update(EvaluationDef evaluationDef){
			
			Crud.EvaluationDefCrud.Update(evaluationDef);
		}

		///<summary>Deletes an EvaluationDef and all EvaluationCriterionDefs attached to it.</summary>
		public static void Delete(long evaluationDefNum) {
			
			string command= "DELETE FROM evaluationdef WHERE EvaluationDefNum = "+POut.Long(evaluationDefNum);
			Database.ExecuteNonQuery(command);
			command= "DELETE FROM evaluationcriteriondef WHERE EvaluationDefNum = "+POut.Long(evaluationDefNum);
			Database.ExecuteNonQuery(command);
		}



	}
}