using System;
using System.Diagnostics;
using System.IO;
using CodeBase;

namespace OpenDentBusiness.Eclaims
{
	/// <summary>
	/// ClaimX. added by RSM 7/27/11
	/// </summary>
	public class ClaimX
	{
		public static string ErrorMessage = "";

		/// <summary>
		/// Returns true if the communications were successful, and false if they failed.
		/// </summary>
		public static bool Launch(Clearinghouse clearinghouseClin, int batchNum)
		{
			try
			{
				ODFileUtils.ProcessStart(clearinghouseClin.ClientProgram);
			}
			catch (Exception exception)
			{
				ErrorMessage = exception.Message;

				return false;
			}

			return true;
		}
	}
}
