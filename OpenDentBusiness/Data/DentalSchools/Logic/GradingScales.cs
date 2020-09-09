using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
	public partial class GradingScales
	{
		public static IEnumerable<GradingScale> GetAll() 
			=> SelectMany("SELECT * FROM `grading_scales`");

		public static GradingScale GetById(long gradingScaleId) 
			=> SelectOne(gradingScaleId);

		public static bool IsDupicateDescription(string description, long excludeGradingScaleId)
		{
			var count = Database.ExecuteLong(
				"SELECT COUNT(*) FROM `grading_scales` " +
				"WHERE `description` = @description " +
				"AND `id` != " + excludeGradingScaleId,
					new MySqlParameter("description", description ?? ""));

			return count > 0;
		}

		public static bool IsInUseByEvaluation(GradingScale gradingScale)
		{
			return Database.ExecuteLong(
				"SELECT COUNT(*) " +
				"FROM `evaluations` e, `evaluation_criterions` ec " +
				"WHERE e.`grading_scale_id` = " + gradingScale.Id + " " +
				"OR ec.`grading_scale_id` = " + gradingScale.Id) > 0;
		}

		public static void Save(GradingScale gradingScale)
        {
			if (gradingScale.Id == 0) ExecuteInsert(gradingScale);
            else
            {
				ExecuteUpdate(gradingScale);
            }
        }

		public static void Delete(long gradingScaleId)
		{
			var commands = new string[]
			{
				"SELECT COUNT(*) FROM `evaluation_defs` WHERE `grading_scale_id` = " + gradingScaleId,
				"SELECT COUNT(*) FROM `evaluation_criterion_defs` WHERE `grading_scale_id` = " + gradingScaleId,
				"SELECT COUNT(*) FROM `evaluations` WHERE `grading_scale_id` = " + gradingScaleId,
				"SELECT COUNT(*) FROM `evaluation_criterions` WHERE `grading_scale_id` = " + gradingScaleId
			};

			foreach (var command in commands)
            {
				if (Database.ExecuteLong(command) > 0)
                {
					throw new Exception(Translation.DentalSchools.GradingScaleInUse);
                }
            }

			ExecuteDelete(gradingScaleId);
		}
	}
}
