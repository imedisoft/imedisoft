using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;

namespace OpenDentBusiness
{
	/// <summary>
	/// These are custom fields added to appointments and managed by the user.
	/// </summary>
	[Table]
	public class ApptField : TableBase
	{
		[PrimaryKey]
		public long ApptFieldNum;

		[ForeignKey(typeof(Appointment), nameof(Appointment.AptNum))]
		public long AptNum;

		/// <summary>
		/// The full name is shown here for ease of use when running queries. 
		/// But the user is only allowed to change fieldNames in the patFieldDef setup window.
		/// </summary>
		[ForeignKey(typeof(AppointmentFieldDefinition), nameof(AppointmentFieldDefinition.Name))]
		public string FieldName;

		/// <summary>
		/// Any text that the user types in. Will later allow some automation.
		/// </summary>
		public string FieldValue;
	}
}

