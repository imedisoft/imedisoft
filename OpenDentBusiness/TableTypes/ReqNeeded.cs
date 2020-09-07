using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;

namespace OpenDentBusiness
{
	/// <summary>
	/// For Dental Schools. 
	/// Requirements needed in order to complete a course.
	/// </summary>
	[Table("school_course_requirements")]
	public class ReqNeeded : TableBase
	{
		[PrimaryKey]
		public long ReqNeededNum;

		public string Descript;

		[ForeignKey(typeof(SchoolCourse), nameof(SchoolCourse.Id))]
		public long SchoolCourseNum;

		[ForeignKey(typeof(SchoolClass), nameof(SchoolClass.Id))]
		public long SchoolClassNum;

		public ReqNeeded Copy()
		{
            return new ReqNeeded
            {
                ReqNeededNum = ReqNeededNum,
                Descript = Descript,
                SchoolCourseNum = SchoolCourseNum,
                SchoolClassNum = SchoolClassNum
            };
		}
	}
}
