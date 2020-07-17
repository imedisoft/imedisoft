using Imedisoft.Data;

namespace UnitTestsCore
{
    public class EServiceSignalT {
		public static void ClearEServiceSignalTable() {
			string command="DELETE FROM eservicesignal";
			Database.ExecuteNonQuery(command);
		}
	}
}
