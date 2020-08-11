using Imedisoft.Data;
using OpenDentBusiness;
using System.Collections.Generic;

namespace UnitTestsCore
{
    public class SignalodT
	{
		/// <summary>
		/// Deletes everything from the Signalod table.
		/// Does not truncate the table so that PKs are not reused on accident.
		/// </summary>
		public static void ClearSignalodTable()
		{
			try
			{
				Database.ExecuteNonQuery("DELETE FROM signalod");
			}
			catch
			{
			}
		}

		/// <summary>
		/// Gets all entries from the Signalod table.
		/// </summary>
		public static List<Signalod> GetAllSignalods()
		{
            //List<Signalod> listSignals;

			// TODO: Fix me...

   //         try
			//{
			//	listSignals = OpenDentBusiness.Crud.SignalodCrud.TableToList(
			//		Database.ExecuteDataTable("SELECT * FROM signalod"));
			//}
			//catch
			//{
			//	listSignals = new List<Signalod>();
			//}

			return null;
		}
	}
}
