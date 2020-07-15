using DataConnectionBase;
using OpenDentBusiness;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace CentralManager
{
	public class CentralConnectionHelper
	{
		/// <summary>
		/// Returns command-line arguments for launching Open Dental based off of the settings for the connection passed in.
		/// </summary>
		private static string GetArgsFromConnection(CentralConnection centralConnection, bool useDynamicMode)
		{
			string args = "";

			if (centralConnection.DatabaseName != "")
			{
				args +=
					"ServerName=\"" + centralConnection.ServerName + "\" " +
					"DatabaseName=\"" + centralConnection.DatabaseName + "\" " +
					"MySqlUser=\"" + centralConnection.MySqlUser + "\" ";

				if (centralConnection.MySqlPassword != "")
				{
					args += "MySqlPassword=\"" + centralConnection.MySqlPassword + "\" ";
				}
			}

			args += "DynamicMode=\"" + useDynamicMode.ToString() + "\" ";

			return args;
		}

		/// <summary>
		/// Launches OD.
		/// Sets hWnd and ProcessID.
		/// If this fails to launch, a textbox will appear.
		/// </summary>
		public static void LaunchOpenDental(CentralConnection centralConnection, bool useDynamicMode, bool isAutoLogin, long patNum, ref WindowInfo windowInfo)
		{
			string args = GetArgsFromConnection(centralConnection, useDynamicMode);

			if (isAutoLogin)
			{
				args += "UserName=\"" + Security.CurUser.UserName + "\" ";
				args += "OdPassword=\"" + Security.PasswordTyped + "\" ";
			}

			if (patNum != 0)
			{
				args += "PatNum=" + patNum.ToString();
			}

			try
			{
				Process process = Process.Start("OpenDental.exe", args);
				windowInfo.HWnd = IntPtr.Zero;//process.MainWindowHandle;//but this hWnd seems to be wrong
				windowInfo.ProcessId = process.Id;
			}
			catch
			{
				CodeBase.ODMessageBox.Show("Unable to start the process OpenDental.exe.");
			}
		}

		/// <summary>
		/// Sets the current data connection settings of the central manager to the connection settings passed in.
		/// Automatically refreshes the local cache to reflect the cache of the connection passed in.
		/// There is an overload for this function if you dont want to refresh the entire cache.
		/// </summary>
		public static bool SetCentralConnection(CentralConnection centralConnection)
		{
			return SetCentralConnection(centralConnection, true);
		}

		/// <summary>
		/// Sets the current data connection settings of the central manager to the connection settings passed in.
		/// Setting refreshCache to true will cause the entire local cache to get updated with the cache from the connection passed in if the new connection settings are successful.
		/// </summary>
		public static bool SetCentralConnection(CentralConnection centralConnection, bool refreshCache)
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
