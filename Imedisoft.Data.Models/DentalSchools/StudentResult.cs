using Imedisoft.Data.Annotations;
using System;

namespace Imedisoft.Data.Models
{
    /// <summary>
    ///		<para>
    ///			The purpose of this table changed significantly in version 4.5. This now only stores completed requirements. 
    ///			There can be multiple completed requirements of each ReqNeededNum. 
    ///			No need to synchronize any longer.
    ///		</para>
    ///		<para>
    ///			<b>For Dental Schools.</b>
    ///		</para>
    /// </summary>
    [Table("student_results")]
	public class StudentResult
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(SchoolCourse), nameof(SchoolCourse.Id))]
		public long SchoolCourseId;

		[ForeignKey(typeof(SchoolCourseRequirement), nameof(SchoolCourseRequirement.Id))]
		public long SchoolCourseRequirementId;

		public string Description;

		/// <summary>
		/// The ID of the provider that represents the student.
		/// </summary>
		public long ProviderId;

		public long? ApptId;

		public long? PatientId;

		public long? InstructorId;

		/// <summary>
		/// The date on which the requirement was completed.
		/// </summary>
		public DateTime? CompletionDate;
	}
}
