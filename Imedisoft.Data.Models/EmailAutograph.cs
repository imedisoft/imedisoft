using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    [Table("email_autographs")]
	public class EmailAutograph
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// Description of the autograph. This is what the user sees when picking an autograph.
		/// </summary>
		public string Description;

		/// <summary>
		/// The actual text of the autograph.
		/// </summary>
		public string Autograph;

		public EmailAutograph Copy() => (EmailAutograph)MemberwiseClone();
	}
}
