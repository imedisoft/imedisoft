using Imedisoft.Data.Annotations;
using System;

namespace Imedisoft.Data.Models
{
    [Table("evaluations")]
	public class Evaluation
	{
		[PrimaryKey]
		public long Id;

		public long InstructorId;

		public long StudentId;

		/// <summary>
		/// For example to PEDO 732.
		/// </summary>
		[ForeignKey(typeof(SchoolCourse), nameof(SchoolCourse.Id))]
		public long SchoolCourseId;

		/// <summary>
		/// The overall grading scale for this evaluation. 
		/// Criteria will not all necessarily have the same scale.
		/// </summary>
		[ForeignKey(typeof(GradingScale), nameof(GradingScale.Id))]
		public long GradingScaleId;

		/// <summary>
		/// Copied from evaluation def. Not editable.
		/// </summary>
		public string Title;

		/// <summary>
		/// Date of the evaluation.
		/// </summary>
		public DateTime EvaluationDate;

		/// <summary>
		/// OverallGradeNumber is calculated as described below. 
		/// Once the nearest number on the scale is found, the corresponding gradescaleitem.GradeShowing is used here.
		/// </summary>
		public string OverallGradeShowing;

		/// <summary>
		/// Always recalculated as each individual criterion is changed, so no risk of getting out of synch. 
		/// Only considers criteria on the evaluation that use the same grading scale as the evaluation itself. 
		/// It's an average of all those criteria. 
		/// When averaging, the result will almost never exactly equal one of the numbers in the scale, so the nearest one must be found and used here. 
		/// For example, if the average is 3.6 on a 4 point scale, this will show 4. 
		/// Percentages will be rounded to the nearest whole number. 
		/// This is the value that will be returned in reports and also used in calculations of the student's grade for the term.
		/// </summary>
		public float OverallGradeNumber;

		/// <summary>
		/// Any note that the instructor wishes to place at the bottom of this evaluation.</summary>
		public string Notes;
	}
}
