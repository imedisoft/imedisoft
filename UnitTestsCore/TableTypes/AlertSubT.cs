using Imedisoft.Data;

namespace UnitTestsCore
{
    public class AlertSubT
	{
		public static void ClearAlertSubTable()
		{
			string command = "DELETE FROM alertsub WHERE AlertSubNum > 0";
			Database.ExecuteNonQuery(command);
		}
	}
}
