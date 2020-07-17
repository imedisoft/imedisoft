using CodeBase;
using Imedisoft.Data;
using OpenDentBusiness;
using System.Diagnostics;

namespace CentralManager
{
    public class CentralConnectionHelper
	{
		/// <summary>
		/// Returns the command line string to pass to the main program.
		/// </summary>
		private static string GetCommandLineArgs(CentralConnection connection, long? patientId)
		{
			string args = "";

			if (connection.DatabaseName != "")
			{
				args +=
					"ServerName=\"" + connection.ServerName + "\" " +
					"DatabaseName=\"" + connection.DatabaseName + "\" " +
					"MySqlUser=\"" + connection.MySqlUser + "\" ";

				if (connection.MySqlPassword != "")
				{
					args += "MySqlPassword=\"" + connection.MySqlPassword + "\" ";
				}
			}

			if (patientId.HasValue) args += "PatNum=" + patientId.Value;

			return args;
		}

		/// <summary>
		/// Launches the main program using the specified <paramref name="connection"/>.
		/// </summary>
		/// <param name="patientId">The (optional) ID of the patient to select.</param>
		/// <returns>The main program process.</returns>
		public static Process LaunchProgram(CentralConnection connection, long? patientId)
		{
			string args = GetCommandLineArgs(connection, patientId);

			try
			{
				return Process.Start("OpenDental.exe", args);
			}
			catch
			{
				ODMessageBox.Show("Unable to start the process OpenDental.exe.", "CEMT");
			}

			return null;
		}

		/// <summary>
		/// Sets the current data connection settings of the central manager to the connection settings passed in.
		/// Setting refreshCache to true will cause the entire local cache to get updated with the cache from the connection passed in if the new connection settings are successful.
		/// </summary>
		public static bool SetCentralConnection(CentralConnection centralConnection, bool refreshCache = false)
		{
			try
			{
				var dataConnection = new DataConnection();

				dataConnection.SetDbLocal(
					centralConnection.ServerName,
					centralConnection.MySqlUser,
					centralConnection.MySqlPassword,
					centralConnection.DatabaseName);

				if (refreshCache)
				{
					Cache.Refresh(InvalidType.AllLocal);
				}
			}
			catch
			{
				return false;
			}

			return true;
		}
	}
}
