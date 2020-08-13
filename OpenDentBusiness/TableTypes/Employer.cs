using Imedisoft.Data.Annotations;
using System;
using System.Collections;

namespace OpenDentBusiness
{
    /// <summary>
    /// Most insurance plans are organized by employer. 
    /// This table keeps track of the list of employers. 
    /// The address fields were added at one point, but I don't know why they don't show in the program in order to edit. 
    /// Nobody has noticed their absence even though it's been a few years, so for now we are just using the EmpName and not the address.
    /// </summary>
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
