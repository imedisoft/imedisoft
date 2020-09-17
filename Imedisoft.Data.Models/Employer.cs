using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    [Table]
	public class Employer
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The name of the employer.
		/// </summary>
		public string Name;

		/// <summary>
		/// The phone number of the employer (including any puncutation).
		/// </summary>
		public string Phone;

		public override string ToString() => Name;

        public Employer Copy() => (Employer)MemberwiseClone();
	}
}
