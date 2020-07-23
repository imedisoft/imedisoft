using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestsCore
{
    public class UpdateHistoryT {
		///<summary>Inserts an UpdateHistory for the version passed in if there is no entry for that version. Returns the primary key of the row inserted
		///or 0 if an UpdateHistory of that version already exists.</summary>
		public static long CreateUpdateHistory(string version,DateTime dateTimeUpdate=default(DateTime)) {
			List<UpdateHistory> listUpdates=UpdateHistories.GetAll().ToList();
			if(listUpdates.Any(x=>x.Version==version)) {
				return 0;
			}
			UpdateHistory update=new UpdateHistory();
			update.Version="16.1.1.0";
			long updateHistoryNum=UpdateHistories.Insert(update);
			if(dateTimeUpdate!=default(DateTime)) {
				//DateTimeUpdated is a DateTEntry column, so we have to forcefully update it.
				string command="UPDATE updatehistory SET DateTimeUpdated="+POut.DateT(dateTimeUpdate)+" WHERE UpdateHistoryNum="+POut.Long(updateHistoryNum);
				Database.ExecuteNonQuery(command);
			}
			return updateHistoryNum;
		}

		public static void ClearUpdateHistoryTable() {
			string command="DELETE FROM updatehistory";
			Database.ExecuteNonQuery(command);
		}
	}
}
