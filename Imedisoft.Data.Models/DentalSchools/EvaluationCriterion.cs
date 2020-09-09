using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    [Table("evaluation_criterions")]
	public class EvaluationCriterion
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(Evaluation), nameof(Evaluation.Id))]
		public long EvaluationId;

		public string Description;

		/// <summary>
		/// This row will show in bold and will not have a grade attached to it.
		/// </summary>
		public bool IsCategory;

		/// <summary>
		/// The grading scale used for this criterion. 
		/// Having this here allows the instructor to edit saved grades and also allows the evaluation overall grade to consider whether to include this criterion in the calculation.
		/// </summary>
		[ForeignKey(typeof(GradingScale), nameof(GradingScale.Id))]
		public long GradingScaleId;

		/// <summary>
		/// Copied from gradingscaleitem.GradeShowing. 
		/// Required. For example A, B, C, D, F, or 1-10, pass, fail, 89, etc. 
		/// Except for percentages, must come from pick list.
		/// </summary>
		public string GradeShowing;

		/// <summary>
		/// Copied from gradingscaleitem.GradeNumber. 
		/// Required. For example A=4, A-=3.8, pass=1, percentages stored as 89, etc. 
		/// Except for percentages, must come from pick list.
		/// </summary>
		public float GradeNumber;

		/// <summary>
		/// A note about why this student received this particular grade on this criterion.
		/// </summary>
		public string Notes;

		/// <summary>
		/// Copied from item order of def. 
		/// Defines the order that all the criteria show on the evaluation. 
		/// User not allowed to change here, only in the def.
		/// </summary>
		public int SortOrder;

		/// <summary>
		/// For ScaleType=Points, sets the maximum value of points for this criterion.
		/// </summary>
		public float MaxPointsAllowed;
	}
}
