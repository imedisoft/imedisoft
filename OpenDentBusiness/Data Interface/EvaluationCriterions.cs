using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EvaluationCriterions{
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

		///<summary>Get all Criterion attached to an Evaluation.</summary>
		public static List<EvaluationCriterion> Refresh(long evaluationNum){
			
			string command="SELECT * FROM evaluationcriterion WHERE EvaluationNum = "+POut.Long(evaluationNum);
			return Crud.EvaluationCriterionCrud.SelectMany(command);
		}

		///<summary>Gets one EvaluationCriterion from the db.</summary>
		public static EvaluationCriterion GetOne(long evaluationCriterionNum){
			
			return Crud.EvaluationCriterionCrud.SelectOne(evaluationCriterionNum);
		}

		///<summary></summary>
		public static long Insert(EvaluationCriterion evaluationCriterion){
			
			return Crud.EvaluationCriterionCrud.Insert(evaluationCriterion);
		}

		///<summary></summary>
		public static void Update(EvaluationCriterion evaluationCriterion){
			
			Crud.EvaluationCriterionCrud.Update(evaluationCriterion);
		}

		///<summary></summary>
		public static void Delete(long evaluationCriterionNum) {
			
			string command= "DELETE FROM evaluationcriterion WHERE EvaluationCriterionNum = "+POut.Long(evaluationCriterionNum);
			Database.ExecuteNonQuery(command);
		}



	}
}