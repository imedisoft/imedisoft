using CodeBase;
using OpenDentBusiness;
using System;
using System.Diagnostics;

namespace OpenDentBusiness.Eclaims
{
    /// <summary>
    /// Summary description for AOS. added by SPK 7/13/05
    /// </summary>
    public static class AOS
	{
		public static string ErrorMessage = "";

		/// <summary>
		/// Returns true if the communications were successful, and false if they failed.
		/// </summary>
		public static bool Launch(Clearinghouse clearinghouseClin, int batchNum)
		{
			try
			{
				Process.Start(clearinghouseClin.ClientProgram);
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
