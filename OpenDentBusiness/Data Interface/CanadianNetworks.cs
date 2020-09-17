using Imedisoft.Data.Cache;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public class CanadianNetworks
	{
		private class CanadianNetworkCache : ListCache<CanadianNetwork>
		{
            protected override IEnumerable<CanadianNetwork> Load() 
				=> Crud.CanadianNetworkCrud.SelectMany("SELECT * FROM `canadian_networks` ORDER BY `description`");
        }

		private static readonly CanadianNetworkCache cache = new CanadianNetworkCache();

		public static List<CanadianNetwork> GetAll() 
			=> cache.GetAll();

		public static CanadianNetwork FirstOrDefault(Predicate<CanadianNetwork> predicate) 
			=> cache.FirstOrDefault(predicate);

		public static CanadianNetwork GetNetwork(long canadianNetworkId, Clearinghouse clearinghouse)
		{
			var network = FirstOrDefault(x => x.Id == canadianNetworkId);

			// CSI is the previous name for the network now known as INSTREAM.
			// For ClaimStream, we use a "bidirect" such that any communication going to INSTREAM/CSI will be redirected to the TELUS B network instead.
			// This works because INSTREAM was bought out by TELUS and communications to both networks and handled by the same organization now.
			// Sending directly to INSTREAM fails with an error because TELUS expects us to use the "bidirect".
			if (clearinghouse.CommBridge == EclaimsCommBridge.Claimstream && network.Abbr == "CSI")
			{
				network = FirstOrDefault(x => x.Abbr == "TELUS B");
			}

			return network;
		}
	}
}
