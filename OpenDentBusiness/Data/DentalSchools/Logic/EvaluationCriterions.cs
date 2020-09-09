using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
	public partial class EvaluationCriterions
	{
		public static IEnumerable<EvaluationCriterion> GetByEvaluation(long evaluationId) 
			=> SelectMany("SELECT * FROM `evaluation_criterions` WHERE `evaluation_id` = " + evaluationId);

		public static void Save(EvaluationCriterion evaluationCriterion)
        {
			if (evaluationCriterion.Id == 0) ExecuteInsert(evaluationCriterion);
            else
            {
				ExecuteUpdate(evaluationCriterion);
            }
        }
	}
}
