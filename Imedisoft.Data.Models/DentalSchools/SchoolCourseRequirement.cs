using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    /// <summary>
    ///		<para>
    ///			Represents a requirement needed in order to complete a course.
    ///		</para>
    ///		<para>
    ///			<b>For Dental Schools.</b>
    ///		</para>
    /// </summary>
    [Table("school_course_requirements")]
	public class SchoolCourseRequirement
	{
		[PrimaryKey]
		public long Id;

		public string Description;

		[ForeignKey(typeof(SchoolCourse), nameof(SchoolCourse.Id))]
		public long SchoolCourseId;

		[ForeignKey(typeof(SchoolClass), nameof(SchoolClass.Id))]
		public long SchoolClassId;
	}
}
