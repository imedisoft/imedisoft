using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
    /// <summary>
	/// The patient does not want to recieve messages for a particular type of communication.
	/// </summary>
    [Table]
	public class CommOptOut : TableBase
	{
		[PrimaryKey]
		public long CommOptOutNum;

		/// <summary>FK to patient.PatNum. The patient who is opting out of this form of communication.</summary>
		public long PatNum;

		/// <summary>Enum:CommOptOutType The type of communication for which this patient does not want to receive messages.</summary>
		public CommOptOutType CommType;

		/// <summary>Enum:CommOptOutMode The manner of message that the patient does not want to receive for this type of communication.</summary>
		public CommOptOutMode CommMode;
	}

	public enum CommOptOutType
	{
		/// <summary>
		/// Should not be in the database.
		/// </summary>
		None,

		eConfirm,
		eReminder,
		eThankYou,
	}

	public enum CommOptOutMode
	{
		Text,
		Email,
	}
}
