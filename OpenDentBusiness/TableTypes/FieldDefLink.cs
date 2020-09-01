using Imedisoft.Data.Annotations;
using System;

namespace OpenDentBusiness
{
	///<summary>A linker table that holds generic "field def" links (e.g. patient fields, appointment fields, etc).
	///Each row will have a corresponding FieldLocation, where this def type needs to be hidden from the user (All field defs are shown by default).
	///The presence of an entry in this table will cause field defs of that particular field type not to show up in the specified location.</summary>
	public class FieldDefLink : TableBase
	{
		[PrimaryKey]
		public long FieldDefLinkNum;

		/// <summary>A generic FieldDefNum FK to any particular field def item that will be defined by the FieldDefType column.</summary>
		public long FieldDefNum;

		/// <summary>Enum:FieldDefTypes Defines what FieldDefNum represents.</summary>
		public FieldDefTypes FieldDefType;

		/// <summary>Enum:FieldLocations Defines where this particular field def needs to be hidden.</summary>
		public FieldLocations FieldLocation;
	}

	/// <summary>
	/// Enum representing different types of field defs.
	/// </summary>
	public enum FieldDefTypes
	{
		Appointment,
		Patient
	}

	/// <summary>
	/// Enum representing where the field def should be hidden.
	/// </summary>
	public enum FieldLocations
	{
		Account,
		AppointmentEdit,
		Chart,
		Family,
		OrthoChart,
		GroupNote
	}
}
