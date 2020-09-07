using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    /// <summary>
    /// Used in dental schools. e.g. OP 732 Operative Dentistry Clinic II.
    /// </summary>
    [Table("school_courses")]
	public class SchoolCourse
	{
		[PrimaryKey]
		public long Id;

		[Column("course_id")]
		public string CourseID;

		/// <summary>
		/// eg: Pediatric Dentistry Clinic II
		/// </summary>
		public string Descript;
	}
}
