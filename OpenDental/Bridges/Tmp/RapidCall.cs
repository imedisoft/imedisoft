using CodeBase;
using OpenDentBusiness;
using System;

namespace OpenDental.Bridges
{
    public static class RapidCall
	{
		public static void SendData(Program ProgramCur)
		{
			string path = Programs.GetProgramPath(ProgramCur);
			try
			{
				ODFileUtils.ProcessStart(path, ProgramCur.CommandLine);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public static void ShowPage()
		{
			try
			{
				if (Programs.IsEnabled(ProgramName.RapidCall))
				{
					SendData(Programs.GetCur(ProgramName.RapidCall));
				}
				else
				{
					ODFileUtils.ProcessStart("http://www.opendental.com/resources/redirects/redirectdentaltekrapidcall.html");
				}
			}
			catch
			{
				throw new Exception("Failed to open web browser. Please make sure you have a default browser set and are connected to the internet and then try again.");
			}
		}
	}
}
