using CodeBase;
using OpenDentBusiness;
using System;
using System.Diagnostics;
using System.IO;

namespace OpenDentBusiness.Eclaims
{
    public static class Inmediata
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
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;

				return false;
			}
			return true;
		}
	}
}
