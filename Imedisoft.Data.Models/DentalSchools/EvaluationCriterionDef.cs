using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    /// <summary>
    /// Rows on an evaluation def. The individual items that will be graded. Criterion Defs
    /// </summary>
    [Table("evaluation_criterion_defs")]
	public class EvaluationCriterionDef 
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(EvaluationDef), nameof(EvaluationDef.Id))]
		public long EvaluationDefId;

		/// <summary>
		/// The grading scale used for this criterion. 
		/// As a user builds an evaluationDef, each criterion should default to the GradingScaleNum of the EvaluationDef, and then the user can change if needed. 
		/// The individual criteria do not have to be the same scale as the evaluation.
		/// </summary>
		[ForeignKey(typeof(GradingScale), nameof(GradingScale.Id))]
		public long GradingScaleId;

		public string Description;

		/// <summary>
		/// This row will show in bold and will not have a grade attached to it.
		/// </summary>
		public bool IsCategory;

		/// <summary>
		/// Defines the order that all the criteria show on the evaluation.
		/// </summary>
		public int SortOrder;

		/// <summary>
		/// For ScaleType=Points, sets the maximum value of points for this criterion.
		/// </summary>
		public float MaxPointsAllowed;
	}
}
