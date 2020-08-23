using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
	/// <summary>
	/// For Dental Schools. 
	/// Requirements needed in order to complete a course.
	/// </summary>
	[Table]
	public class ReqNeeded : TableBase
	{
		[PrimaryKey]
		public long ReqNeededNum;

		public string Descript;

		[ForeignKey(typeof(SchoolCourse), nameof(SchoolCourse.SchoolCourseNum))]
		public long SchoolCourseNum;

		[ForeignKey(typeof(SchoolClass), nameof(SchoolClass.SchoolClassNum))]
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
