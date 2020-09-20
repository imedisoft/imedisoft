using Imedisoft.Data.Annotations;
using System;

namespace Imedisoft.Data.Models
{
    [Table("email_addresses")]
	public class EmailAddress
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(EmailAutograph), nameof(EmailAutograph.Id))]
		public long? EmailAutographId;

		public string SmtpServer;

		public string SmtpUsername;

		/// <summary>
		/// Password associated with this email address. Encrypted when stored in the database and decrypted before using.
		/// </summary>
		public string SmtpPassword;

		/// <summary>
		/// Usually 587, sometimes 25 or 465.
		/// </summary>
		public int SmtpPort;

		[Column("use_ssl")]
		public bool UseSSL;

		/// <summary>
		/// The email address of the sender as it should appear to the recipient.
		/// </summary>
		public string SenderAddress;

		public string Pop3Server;

		public int Pop3Port;

		/// <summary>
		/// FK to userod.UserNum. 
		/// Associates a user with this email address. 
		/// A user may only have one email address associated with them.
		/// Can be 0 if no user is associated with this email address.
		/// </summary>
		[ForeignKey(typeof(User), nameof(User.Id))]
		public long? UserId;

		/// <summary>
		/// Just makes it easier to know what email address the user picked in the inbox. Not a DB column.
		/// </summary>
		[Ignore, Obsolete]
		public long WebmailProviderId;

		/// <summary>
		/// Needed for OAuth.
		/// </summary>
		public string AccessToken;

		/// <summary>
		/// Needed for OAuth.
		/// </summary>
		public string RefreshToken;

		/// <summary>
		/// We assume the email settings are implicit if the server port is 465.
		/// </summary>
		public bool IsImplicitSsl => SmtpPort == 465;

		/// <summary>
		/// Returns the SenderAddress if it is not blank, otherwise returns the EmailUsername.
		/// </summary>
		public string GetFrom() => string.IsNullOrEmpty(SenderAddress) ? SmtpUsername : SenderAddress;
	}
}
