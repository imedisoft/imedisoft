using Imedisoft.Data;
using OpenDentBusiness;

namespace UnitTestsCore
{
	public class TaskSubscriptionT
	{
		///<summary>Creates a TaskSubscription.</summary>
		public static void CreateTaskSubscription(long userId = 0, long taskListId = 0)
		{
			TaskSubscriptions.Subscribe(taskListId, userId);
		}

		///<summary>Deletes everything from the TaskSubscription table.  Does not truncate the table so that PKs are not reused on accident.</summary>
		public static void ClearTaskSubscriptionTable()
		{
			string command = "DELETE FROM tasksubscription";
			Database.ExecuteNonQuery(command);
		}
	}
}
