using Imedisoft.Data.Annotations;
using System;
using System.Collections;

namespace OpenDentBusiness
{
    /// <summary>
    /// Like a rolodex for businesses that the office interacts with. Used to store pharmacies, etc.
    /// </summary>
    [Table("contacts")]
	public class Contact : TableBase
	{
		[PrimaryKey]
		public long ContactNum;

		///<summary>Last name or, frequently, the entire name.</summary>
		public string LName;

		///<summary>First name is optional.</summary>
		public string FName;

		///<summary>Work phone.</summary>
		public string WkPhone;

		///<summary>Fax number.</summary>
		public string Fax;

		///<summary>FK to definition.DefNum</summary>
		public long Category;

		///<summary>Note for this contact.</summary>
		public string Notes;
	}
}
