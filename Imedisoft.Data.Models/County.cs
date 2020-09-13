using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
	[Table("counties")]
	public class County
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// Frequently used as the primary key of this table. But it's allowed to change. Change is programmatically synchronized.
		/// </summary>
		public string Name;

		/// <summary>
		/// Optional. Usage varies.
		/// </summary>
		public string Code;

		/// <summary>
		/// Returns a string representation of the county.
		/// </summary>
        public override string ToString() 
			=> string.IsNullOrEmpty(Code) ? Name : Name + ", " + Code;
    }
}
