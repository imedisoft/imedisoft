using Imedisoft.Data;

namespace UnitTestsCore
{
    public class ClaimSnapshotT
	{
		public static void ClearClaimSnapshotTable()
		{
			string command = "DELETE FROM claimsnapshot WHERE ClaimSnapshotNum > 0";
			Database.ExecuteNonQuery(command);
		}
	}
}
