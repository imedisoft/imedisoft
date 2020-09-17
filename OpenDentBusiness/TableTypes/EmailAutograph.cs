using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
    [Table("email_autographs")]
	public class EmailAutograph : TableBase
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// Description of the autograph. This is what the user sees when picking an autograph.
		/// </summary>
		public string Description;

		/// <summary>
		/// Email address(es) that this autograph is associated with. An autograph can be associated with multiple addresses.
		/// </summary>
		public string EmailAddress;

		/// <summary>
		/// The actual text of the autograph.
		/// </summary>
		public string AutographText;

		public EmailAutograph Copy() => (EmailAutograph)MemberwiseClone();
	}
}
