using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;

namespace OpenDentBusiness
{
    [Table("contacts")]
	public class Contact : TableBase
	{
		[PrimaryKey]
		public long Id;

		public string LastName;

		public string FirstName;

		public string WkPhone;

		public string Fax;

		[ForeignKey(typeof(Definition), nameof(Definition.Id))]
		public long Category;

		public string Notes;
	}
}
