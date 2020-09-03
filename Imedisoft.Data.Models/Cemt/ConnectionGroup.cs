using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models.Cemt
{
    [Table("cemt_connection_groups")]
	public class ConnectionGroup
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// Description of the connection group
		/// </summary>
		public string Description;

		/// <summary>
		/// Returns a string representation of the connection group.
		/// </summary>
		public override string ToString() => Description;
    }
}