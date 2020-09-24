using Imedisoft.Data.Annotations;
using System;

namespace Imedisoft.Data.Models
{
    /// <summary>
    /// Can also be a CCR.
    /// Received CCDs/CCRs are stored both here and in emailattach.
    /// Sent CCDs are not saved here, but are only stored in emailattach.
    /// To display a saved Ccd, it is combined with an internal stylesheet.
    /// </summary>
    [Table("ehr_summary_ccds")]
	public class EhrSummaryCcd
	{
		[PrimaryKey]
		public long Id;

		public long PatientId;

		/// <summary>
		///	The ID of the e-mail attachment the CCD came from.
		/// </summary>
		public long EmailAttachmentId;

		/// <summary>
		/// The date on which the CCD was received.
		/// </summary>
		public DateTime? Date;

		/// <summary>
		/// The XML contents of the received CCD file.
		/// </summary>
		public string Content;

		public EhrSummaryCcd Copy()
		{
			return (EhrSummaryCcd)MemberwiseClone();
		}
	}
}
