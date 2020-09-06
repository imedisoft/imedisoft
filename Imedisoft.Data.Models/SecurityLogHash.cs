using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    /// <summary>
    /// Stores hashes of audit logs for detecting alteration.
    /// </summary>
    [Table("security_log_hashes")]
	public class SecurityLogHash
	{
		[PrimaryKey(AutoIncrement = false), ForeignKey(typeof(SecurityLog), nameof(SecurityLog.Id))]
		public long SecurityLogId;

		/// <summary>
		/// The SHA-256 hash of PermType, UserNum, LogDateTime, LogText, and PatNum, all concatenated together. 
		/// This hash has length of 32 bytes encoded as base64. 
		/// Used to detect if the entry has been altered outside of Open Dental.
		/// </summary>
		public string Hash;
	}
}
