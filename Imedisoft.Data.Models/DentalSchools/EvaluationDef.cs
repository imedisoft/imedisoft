using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    /// <summary>
    /// An evaluation def is the entire form that the instructor sets up ahead of time. 
    /// Actual evaluations for students are copied from these 'templates', so an evaluation def can be altered or deleted without damaging any student record. 
    /// Evaluation defs are usually not specific to instructors, but if different instructors want different evaluation forms, they can use the description column to differentiate.  For example, the description can include the instructor's name or even the year.  But most commonly, the same evaluation will be used from year to year. 
    /// There should be a duplicate function to make a copy an entire evaluation def and then allow user to alter the SchoolCourseNum.
    /// </summary>
    [Table("evaluation_defs")]
	public class EvaluationDef
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// For example to PEDO 732.
		/// </summary>
		[ForeignKey(typeof(SchoolCourse), nameof(SchoolCourse.Id))]
		public long SchoolCourseId;

		/// <summary>
		/// The default grading scale for this evaluation. Each criterion will typically use the same scale, but that is not required.
		/// </summary>
		[ForeignKey(typeof(GradingScale), nameof(GradingScale.Id))]
		public long GradingScaleId; // TODO: Should be nullable?

		public string Title;
	}
}
