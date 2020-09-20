using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
	[Table("canadian_networks")]
	public class CanadianNetwork
	{
		[PrimaryKey]
		public long Id;

		public string Abbr;

		public string Description;

		public string TransactionPrefix;

		/// <summary>
		/// Set to true if this network is in charge of handling all Request for Payment 
		/// Reconciliation (RPR) transactions for all carriers within this network, as opposed to 
		/// the individual carriers wihtin the network processing the RPR transactions themselves.
		/// </summary>
		public bool IsRprHandler;
	}
}
