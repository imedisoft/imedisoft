using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
	public partial class EvaluationCriterionDefs
	{
		public static IEnumerable<EvaluationCriterionDef> GetAvailableCriterionDefs() 
			=> SelectMany("SELECT * FROM `evaluation_criterion_defs` WHERE `evaluation_def_id` IS NULL");

		public static IEnumerable<EvaluationCriterionDef> GetAllForEvaluationDef(long evaluationDefId) 
			=> SelectMany("SELECT * FROM `evaluation_criterion_defs` WHERE `evaluation_def_id` = " + evaluationDefId + " ORDER BY `sort_order`");

		public static void Save(EvaluationCriterionDef evaluationCriterionDef)
        {
			if (evaluationCriterionDef.Id == 0) ExecuteInsert(evaluationCriterionDef);
            else
            {
				ExecuteUpdate(evaluationCriterionDef);
            }
        }

		public static void Delete(EvaluationCriterionDef evaluationCriterionDef) 
			=> ExecuteDelete(evaluationCriterionDef);
	}
}
