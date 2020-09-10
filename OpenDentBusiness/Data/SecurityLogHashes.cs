using Imedisoft.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Imedisoft.Data
{
    public partial class SecurityLogHashes
	{
		public static SecurityLogHash GetOne(long securityLogHashNum)
		{
			return SelectOne(securityLogHashNum);
		}

		public static void DeleteWithMaxPriKey(long maxSecurityLogHashNum)
		{
			if (maxSecurityLogHashNum == 0)
			{
				return;
			}

			string command = "DELETE FROM securityloghash WHERE SecurityLogHashNum <= " + maxSecurityLogHashNum;
			Database.ExecuteNonQuery(command);
		}

		public static void DeleteForSecurityLogEntries(List<long> listSecurityLogNums)
		{
			if (listSecurityLogNums.Count < 1)
			{
				return;
			}

			string command = $"DELETE FROM securityloghash WHERE SecurityLogNum IN ({string.Join(",", listSecurityLogNums)})";
			Database.ExecuteNonQuery(command);
		}

		public static void Insert(SecurityLogHash securityLogHash)
			=> ExecuteInsert(securityLogHash);

		public static void InsertSecurityLogHash(long securityLogId)
		{
			var securityLog = SecurityLogs.GetOne(securityLogId);
			if (securityLog == null)
			{
				System.Threading.Thread.Sleep(100);

				securityLog = SecurityLogs.GetOne(securityLogId); //need a fresh copy because of time stamps, etc.
			}

			if (securityLog == null)
			{
				//We give up at this point. The end result will be the securitylog row shows up as RED in the audit trail.
				//We don't want other things to fail/practice flow to be interrupted just because of securitylog issues.

				return;
			}

			Insert(new SecurityLogHash
			{
				SecurityLogId = securityLog.Id,
				Hash = GetHashString(securityLog)
			});
		}

		public static void InsertSecurityLogHashNoCache(long securityLogId)
		{
			var securityLog = SecurityLogs.SelectOne(securityLogId);

			Insert(new SecurityLogHash
			{
				SecurityLogId = securityLog.Id,
				Hash = GetHashString(securityLog)
			});
		}

		/// <summary>
		/// Returns a SHA-256 hash of the specified security log.
		/// </summary>
		public static string GetHashString(SecurityLog securityLog)
		{
			using var algorithm = SHA256.Create();

			var securityLogDetails = 
				$"{(int)securityLog.Type}{securityLog.UserId}" +
				$"{securityLog.LogDate:yyyyMMddHHiiss}{securityLog.LogMessage}{securityLog.PatientId}" +
				$"{securityLog.ObjectId}{securityLog.ObjectDate:yyyyMMddHHiiss}";
			
			var hash = algorithm.ComputeHash(
				Encoding.UTF8.GetBytes(securityLogDetails));

			return string.Concat(hash.Select(b => b.ToString("x2")));
		}
	}
}
