using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CodeBase;

namespace DataConnectionBase
{
	public class DbAdminMysql
	{
		///<summary>The host names for which we will create users.</summary>
		private static string[] _arrayHostNames = new string[] { "%", "::1", "127.0.0.1", "localhost" };

		///<summary>Throws exceptions.
		///Tests the connection to the "mysql" database with the given adminUserName/password.
		///Returns an open connection if successful, throws upon failure.</summary>
		public static DataConnection ConnectAndTest(string adminUserName, string password, string server = "localhost")
		{
			//The connection string might only break with the ';' character, but we need usernames to be consistent accross all functions in this class.
			if (SOut.HasInjectionChars(adminUserName))
			{
				throw new ODException("The specified admin user name contains invalid characters.");
			}
			//The connection string might only break with the ';' character, but we need passwords to be consistent accross all functions in this class.
			if (SOut.HasInjectionChars(password))
			{
				throw new ODException("The specified password contains invalid characters.");
			}
			DataConnection con = null;
			try
			{
				con = new DataConnection(server, "mysql", adminUserName, password);
			}
			catch (Exception ex)
			{
				throw new ODException("Failed to connect with user '" + adminUserName + "' and password '" + password + "': " + ex.Message);
			}
			try
			{
				GetHostNamesForUser(con, adminUserName);
			}
			catch (Exception ex)
			{
				if (con != null)
				{
					con.Dispose();
					con = null;
				}
				throw new ODException("Admin permission test failed for user '" + adminUserName + "' and password '" + password + "': " + ex.Message);
			}
			return con;
		}

		///<summary>Throws exceptions.
		///If the user associated to the connection "conAdmin" does not have permission to the mysql.user table, then this function will throw.
		///Upon success, will return a list of all host names registered for the specified userName.  Otherwise, if failed, throws error.</summary>
		public static List<string> GetHostNamesForUser(DataConnection conAdmin, string userName)
		{
			if (SOut.HasInjectionChars(userName))
			{
				throw new ODException("The specified user name contains invalid characters.");
			}
			string command = "SELECT user,host FROM mysql.user WHERE user='" + userName + "'";
			DataTable table = conAdmin.GetTable(command);
			List<string> listHostNames = new List<string>();
			foreach (DataRow row in table.Rows)
			{
				listHostNames.Add(row["host"].ToString());
			}
			return listHostNames;
		}

		///<summary>Throws exceptions.
		///If the user associated to the connection "conAdmin" does not have permission to the mysql.user table, then this function will throw.
		///Will also throw if the user assocated to connection "conAdmin" does not have DROP permission.
		///Finally, will throw if the MySQL commands were successful, but the user was not fully dropped.</summary>
		public static void DropUser(DataConnection conAdmin, string userName)
		{
			if (SOut.HasInjectionChars(userName))
			{
				throw new ODException("The specified user name contains invalid characters.");
			}
			//Get the list of host names currently registered for the given userName, so we can drop them one by one.
			List<string> listHostNames = GetHostNamesForUser(conAdmin, userName);
			foreach (string hostName in listHostNames)
			{
				string command = "DROP USER '" + userName + "'@'" + hostName + "'";
				conAdmin.NonQ(command);
			}
			listHostNames = GetHostNamesForUser(conAdmin, userName);
			if (listHostNames != null && listHostNames.Count > 0)
			{
				StringBuilder sb = new StringBuilder();
				foreach (string hostName in listHostNames)
				{
					sb.AppendLine("Failed to drop user '" + userName + "' at host '" + hostName + "'");
				}
				throw new ODException(sb.ToString());
			}
		}

		///<summary>Sets the fully privilaged user to the specified userName/password and drops the oldUserName.
		///If the oldUserName is empty or null or the same as userName, then will not drop old user.
		///Uses the conAdmin connection to perform the operation.  The conAdmin is expected to be a connection created using admin credentials.
		///Returns null on success, or an error string on failure.</summary>
		public static string ModifyUser(DataConnection conAdmin, string userName, string password, string oldUserName = "")
		{
			if (!String.IsNullOrEmpty(oldUserName) && oldUserName != userName && SOut.HasInjectionChars(oldUserName))
			{
				return ("The specified old user name contains invalid characters.");
			}
			if (SOut.HasInjectionChars(userName))
			{
				return ("The specified user name contains invalid characters.");
			}
			if (SOut.HasInjectionChars(password))
			{
				return ("The specified password contains invalid characters.");
			}
			string errMsg = "";
			foreach (string hostName in _arrayHostNames)
			{
				errMsg = GrantToUser(conAdmin, hostName, userName, password, doSetPassword: true, hasFullPermission: true);
				if (errMsg != null)
				{
					return errMsg;
				}
			}
			DataConnection con = null;
			try
			{
				con = ConnectAndTest(userName, password);//Ensure new credentials work.
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
			if (con != null)
			{
				errMsg = null;
				if (!String.IsNullOrEmpty(oldUserName) && oldUserName != userName)
				{
					try
					{
						DropUser(con, oldUserName);//Drop the root user from a connection based on the new user credentials.
					}
					catch (Exception ex)
					{
						errMsg = ex.Message;
					}
				}
				con.Dispose();
				return errMsg;
			}
			return null;
		}

		/// <summary>
		/// Uses the given connection to grant permissions to the given userName/password.
		/// 
		/// Returns null on success, or an error string on failure.
		/// </summary>
		/// <param name="hasFullPermission">
		/// If true, the user will only be given the SELECT permission. Otherwise, they will get all permissions.
		/// </param>
		private static string GrantToUser(DataConnection conAdmin, string hostName, string userName, string password, bool doSetPassword, bool hasFullPermission)
		{
			if (SOut.HasInjectionChars(hostName))
			{
				return ("The specified host name contains invalid characters.");
			}
			if (SOut.HasInjectionChars(userName))
			{
				return ("The specified user name contains invalid characters.");
			}
			if (doSetPassword && SOut.HasInjectionChars(password))
			{
				return ("The specified password contains invalid characters.");
			}

			string command;
			if (!hasFullPermission)
			{
				// Granting permissions only gives permissions; it doesn't take any away. That's why we need to revoke them first.
				command = "REVOKE ALL, GRANT OPTION FROM '" + userName + "'@'" + hostName + "'";
				try
				{
					conAdmin.NonQ(command);
				}
				catch (Exception ex)
				{
					return "Failed to revoke permission to user '" + userName + "' for host '" + hostName + "': " + ex.Message;
				}
			}

			// https://dev.mysql.com/doc/refman/5.5/en/grant.html
			command = "GRANT " + (hasFullPermission ? "ALL PRIVILEGES" : "SELECT") + " ON *.* TO '" + userName + "'@'" + hostName + "'";
			if (doSetPassword)
			{
				command += " IDENTIFIED BY '" + password + "'";
			}
			if (hasFullPermission)
			{
				command += " WITH GRANT OPTION";
			}
			try
			{
				conAdmin.NonQ(command);
			}
			catch (Exception ex)
			{
				return ("Failed to grant permission to user '" + userName + "' for host '" + hostName + "': " + ex.Message);
			}
			return null;
		}

		/// <summary>
		/// Returns the MySQL version.
		/// </summary>
		public static Version GetMySQLVersion(DataConnection con)
		{
			string version = con.GetScalar("SELECT @@Version");

			return new Version(MiscUtils.GetVersionFromString(version));
		}
	}
}
