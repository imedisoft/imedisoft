using Imedisoft.Data.Annotations;
using System;
using System.Collections;
using System.Drawing;
using System.Xml.Serialization;

namespace OpenDentBusiness
{
    /// <summary>
    /// Appointment type is used to override appointment color. 
    /// </summary>
    [Table("appointment_types")]
	public class AppointmentType : TableBase
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The display name of the appointment type.
		/// </summary>
		public string Name;

		public Color Color;

		/// <summary>
		///		<para>
		///			Time pattern, X for doctor, / for assist time. Stored in 5 minute increments.
		///		</para>
		///		<para>
		///			Convert as needed to 10 or 15 minute representations for display.
		///		</para>
		///		<para>
		///			Will be blank if the pattern should be dynamically calculated via the procedures found in <see cref="ProcedureCodes"/>.
		///		</para>
		/// </summary>
		public string Pattern;

		/// <summary>
		/// Comma-delimited list of procedure codes.
		/// </summary>
		public string ProcedureCodes;

		/// <summary>
		/// The display order of the appointment type.
		/// </summary>
		public int ItemOrder;

		/// <summary>
		/// A value indicating whether the appointment type has been hidden.
		/// </summary>
		public bool Hidden;

		public AppointmentType Copy() 
			=> (AppointmentType)MemberwiseClone();
	}
}
