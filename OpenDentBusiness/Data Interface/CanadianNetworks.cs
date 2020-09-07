using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenDentBusiness{
	///<summary></summary>
	public class CanadianNetworks{

		#region Get Methods
		#endregion

		#region Modification Methods

		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		#region Cache Pattern

		private class CanadianNetworkCache : CacheListAbs<CanadianNetwork> {
			protected override List<CanadianNetwork> GetCacheFromDb() {
				string command="SELECT * FROM canadiannetwork ORDER BY Descript";
				return Crud.CanadianNetworkCrud.SelectMany(command);
			}
			protected override List<CanadianNetwork> TableToList(DataTable table) {
				return Crud.CanadianNetworkCrud.TableToList(table);
			}
			protected override CanadianNetwork Copy(CanadianNetwork canadianNetwork) {
				return canadianNetwork.Copy();
			}
			protected override DataTable ListToTable(List<CanadianNetwork> listCanadianNetworks) {
				return Crud.CanadianNetworkCrud.ListToTable(listCanadianNetworks,"CanadianNetwork");
			}
			protected override void FillCacheIfNeeded() {
				CanadianNetworks.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static CanadianNetworkCache _canadianNetworkCache=new CanadianNetworkCache();

		public static List<CanadianNetwork> GetDeepCopy(bool isShort=false) {
			return _canadianNetworkCache.GetDeepCopy(isShort);
		}

		public static CanadianNetwork GetFirstOrDefault(Func<CanadianNetwork,bool> match,bool isShort=false) {
			return _canadianNetworkCache.GetFirstOrDefault(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_canadianNetworkCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _canadianNetworkCache.GetTableFromCache(doRefreshCache);
		}

		#endregion Cache Pattern

		///<summary></summary>
		public static long Insert(CanadianNetwork canadianNetwork) {
			
			return Crud.CanadianNetworkCrud.Insert(canadianNetwork);
		}

		///<summary></summary>
		public static void Update(CanadianNetwork canadianNetwork){
			
			Crud.CanadianNetworkCrud.Update(canadianNetwork);
		}

		///<summary>Uses cache.</summary>
		public static CanadianNetwork GetNetwork(long networkNum,Clearinghouse clearinghouseClin) {
			//No need to check RemotingRole; no call to db.
			CanadianNetwork network=GetFirstOrDefault(x => x.Id==networkNum);
			//CSI is the previous name for the network now known as INSTREAM.
			//For ClaimStream, we use a "bidirect" such that any communication going to INSTREAM/CSI will be redirected to the TELUS B network instead.
			//This works because INSTREAM was bought out by TELUS and communications to both networks and handled by the same organization now.
			//Sending directly to INSTREAM fails with an error because TELUS expects us to use the "bidirect".
			if(clearinghouseClin.CommBridge==EclaimsCommBridge.Claimstream && network.Abbr=="CSI") {
				network=GetFirstOrDefault(x => x.Abbr=="TELUS B");
			}
			return network;
		}
	}
}













