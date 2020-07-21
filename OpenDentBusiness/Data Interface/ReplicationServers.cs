using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Net;
using System.Reflection;

namespace OpenDentBusiness
{
    public class ReplicationServers
	{
		/// <summary>
		/// This value is only retrieved once upon startup. 
		/// This variable is a long because Google's cloud services have server id's that are of a higher value than a signed int can contained.
		/// Additionally, 0 is a valid server id based on MySQL so we need to use -1 and can't use a uint data type.
		/// </summary>
		private static long serverId = -1;

		/// <summary>
		/// The first time this is accessed, the value is obtained using a query.
		/// Will be 0 unless a server id was set in my.ini.
		/// </summary>
		public static long ServerId => serverId == -1 ?
			serverId = GetServerId() :
			serverId;

		private class ReplicationServerCache : CacheListAbs<ReplicationServer>
		{
			protected override List<ReplicationServer> GetCacheFromDb() 
				=> Crud.ReplicationServerCrud.SelectMany("SELECT * FROM replicationserver ORDER BY ServerId");

			protected override List<ReplicationServer> TableToList(DataTable table) 
				=> Crud.ReplicationServerCrud.TableToList(table);

			protected override ReplicationServer Copy(ReplicationServer replicationServer) 
				=> replicationServer.Copy();

			protected override DataTable ListToTable(List<ReplicationServer> listReplicationServers) 
				=> Crud.ReplicationServerCrud.ListToTable(listReplicationServers, "ReplicationServer");

			protected override void FillCacheIfNeeded() 
				=> ReplicationServers.GetTableFromCache(false);
		}

		private static readonly ReplicationServerCache cache = new ReplicationServerCache();

		public static ReplicationServer GetFirstOrDefault(Func<ReplicationServer, bool> match, bool isShort = false)
		{
			return cache.GetFirstOrDefault(match, isShort);
		}

		public static DataTable GetTableFromCache(bool doRefreshCache)
		{
			return cache.GetTableFromCache(doRefreshCache);
		}

		/// <summary>
		/// Gets the MySQL server_id variable for the current connection.
		/// </summary>
		private static long GetServerId()
		{
			DataTable table = Database.ExecuteDataTable("SHOW VARIABLES LIKE 'server_id'");

			return SIn.Long(table.Rows[0][1].ToString());
		}

		/// <summary>
		/// Generates a random primary key. 
		/// Tests to see if that key already exists before returning it for use. 
		/// The range of returned values is greater than 0, and less than or equal to 9223372036854775807.
		/// </summary>
		public static long GetKey(string tablename, string field)
		{
			//establish the range for this server
			long rangeStart = 10000;
			long rangeEnd = long.MaxValue;

			// the following line triggers a separate call to db if server_id=-1.  Must be cap.
			if (ServerId != 0)
			{//if it IS 0, then there is no server_id set.
				var self = GetFirstOrDefault(x => x.ServerId == ServerId);
				if (self != null)
				{//a ReplicationServer row was found for this server_id
					if (self.RangeEnd - self.RangeStart >= 999999)
					{//and a valid range was entered that was at least 1,000,000
						rangeStart = self.RangeStart;
						rangeEnd = self.RangeEnd;
					}
				}
			}

			long rndLong;
			long span = rangeEnd - rangeStart;
			do
			{
				rndLong = (long)(ODRandom.NextDouble() * span) + rangeStart;
			}
			while (rndLong == 0
				|| rndLong < rangeStart
				|| rndLong > rangeEnd
				|| KeyInUse(tablename, field, rndLong));
			return rndLong;
		}

		/// <summary>
		/// Generates a random primary key without using the cache.
		/// </summary>
		public static long GetKeyNoCache(string tablename, string field)
		{
			long rangeStart = 10000;
			long rangeEnd = long.MaxValue;
			long server_id = GetServerId();

			if (server_id != 0)
			{
				var self = GetServer(server_id);
				if (self != null && self.RangeEnd - self.RangeStart >= 999999)
				{
					rangeStart = self.RangeStart;
					rangeEnd = self.RangeEnd;
				}
			}

			long span = rangeEnd - rangeStart;
			long rndLong = (long)(ODRandom.NextDouble() * span) + rangeStart;
			while (rndLong == 0
				|| rndLong < rangeStart
				|| rndLong > rangeEnd
				|| KeyInUse(tablename, field, rndLong))
			{
				rndLong = (long)(ODRandom.NextDouble() * span) + rangeStart;
			}
			return rndLong;
		}

		private static ReplicationServer GetServer(long serverId) 
			=> Crud.ReplicationServerCrud.SelectOne(
				"SELECT * FROM replicationserver WHERE ServerId=" + serverId);

		private static bool KeyInUse(string tablename, string field, long keynum) 
			=> Database.ExecuteLong(
				"SELECT COUNT(*) FROM " + tablename + " WHERE " + field + "=" + keynum.ToString()) > 0;
	}
}
