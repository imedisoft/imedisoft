using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OpenDentBusiness;
using DataConnectionBase;

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
				DataCore.NonQ("DELETE FROM signalod");
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
            List<Signalod> listSignals;

            try
			{
				listSignals = OpenDentBusiness.Crud.SignalodCrud.TableToList(
					DataCore.GetTable("SELECT * FROM signalod"));
			}
			catch
			{
				listSignals = new List<Signalod>();
			}

			return listSignals;
		}
	}
}
