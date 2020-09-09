using Imedisoft.Data.Models;
using System.Collections.Generic;

namespace Imedisoft.Data
{
	public partial class GradingScaleItems
	{
		public static IEnumerable<GradingScaleItem> GetByGradingScale(long gradingScaleId) 
			=> SelectMany(
				"SELECT * FROM `grading_scale_items` " +
				"WHERE `grading_scale_id` = " + gradingScaleId + " " +
				"ORDER BY `value` DESC");

		public static void Save(GradingScaleItem gradingScaleItem)
        {
			if (gradingScaleItem.Id == 0) ExecuteInsert(gradingScaleItem);
            else
            {
				ExecuteUpdate(gradingScaleItem);
            }
        }

		public static void Delete(GradingScaleItem gradingScaleItem) 
			=> ExecuteDelete(gradingScaleItem);
	}
}
