using Imedisoft.Data.Annotations;
using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Some insurance companies require special provider ID #s, and this table holds them.
    /// </summary>
    [Table]
	public class ProviderIdent : TableBase
	{
		[PrimaryKey]
		public long ProviderIdentNum;

		[ForeignKey(typeof(Provider), nameof(Provider.Id))]
		public long ProviderId;

		/// <summary>
		/// FK to carrier.ElectID aka Electronic ID. An ID only applies to one insurance carrier.
		/// </summary>
		public string PayorID;

		public ProviderSupplementalID SuppIDType;

		/// <summary>
		/// The number assigned by the ins carrier.
		/// </summary>
		public string IDNumber;

		public ProviderIdent Copy()
		{
			return (ProviderIdent)MemberwiseClone();
		}
	}
}
