using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;

namespace OpenDentBusiness
{
	/// <summary>
	/// Keeps track of the last appointment view used on a per user basis. 
	/// Users can have multiple rows in this table when using clinics.
	/// </summary>
	[Table]
	public class UserodApptView
	{
		[PrimaryKey]
		public long UserodApptViewNum;

		[ForeignKey(typeof(User), nameof(User.Id))]
		public long UserNum;

		/// <summary>
		/// 0 if clinics is not being used or if the user has not been assigned a clinic.
		/// </summary>
		[ForeignKey(typeof(Clinic), nameof(Clinic.Id))]
		public long ClinicNum;

		[ForeignKey(typeof(ApptView), nameof(ApptView.ApptViewNum))]
		public long ApptViewNum;
	}
}
