using System;
using System.Data;

namespace OpenDentBusiness.AutoComm
{
	/// <summary>
	/// Lite version of the action item that AutoComm will be handling. 
	/// Used to keep database I/O as small as possible for AutoComm.
	/// Will most likely be extended into more concrete class for different flavors of AutoCommAbs.
	/// </summary>
	public class AutoCommObj
	{
		/// <summary>
		/// Could be AptNum, RecallNum, etc.
		/// </summary>
		public long PrimaryKey { get; set; }

		/// <summary>
		/// The clinic where the action item is to take place.
		/// </summary>
		public long ClinicNum { get; set; }

		/// <summary>
		/// The PatNum for the action item.
		/// </summary>
		public long PatNum { get; set; }

		/// <summary>
		/// The provider for the action item.
		/// </summary>
		public long ProvNum { get; set; }

		/// <summary>
		/// When the action is supposed to take place.
		/// </summary>
		public DateTime DateTimeEvent { get; set; }

		/// <summary>
		/// Language the patient speaks. Could be blank if the patient doesn't have a language set.
		/// </summary>
		public string Language { get; set; }

		/// <summary>
		/// The recipient SMS phone number. If non-blank then assume this number can be texted.
		/// </summary>
		public string PhoneContact { get; set; }

		/// <summary>
		/// The recipient email. If non-blank then assume this email can be sent.
		/// </summary>
		public string EmailContact { get; set; }

		/// <summary>
		/// Patient first name. This is the only identifier allowed in order to avoid HIPPA violations.
		/// </summary>
		public string NameF { get; set; }

		public void SetPatientContact(PatComm patComm)
		{
			if (patComm is null)
			{
				return;
			}
			NameF = patComm.FName;
			PhoneContact = patComm.IsSmsAnOption ? patComm.SmsPhone : "";
			EmailContact = patComm.IsEmailAnOption ? patComm.Email : "";
			Language = patComm.Language;
		}
	}
}
