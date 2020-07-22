using CodeBase;
using OpenDentBusiness;

namespace OpenDental.Bridges
{
    /// <summary>
    /// A simple program link to launch Midway Dental's website.
    /// </summary>
    public static class Midway
	{
		/// <summary>
		/// Opens the homepage for Midway Dental's website
		/// </summary>
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);
			try
			{
				ODFileUtils.ProcessStart(path);
			}
			catch
			{
				MessageBox.Show(path + " is not available.");
			}
			return;
		}
	}
}
