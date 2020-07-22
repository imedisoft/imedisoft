using OpenDentBusiness;
using System;
using System.Diagnostics;

namespace OpenDental.Bridges
{
    public static class Benco
	{
		public static void SendData(Program ProgramCur)
		{
			string path = ProgramCur.Path;
			try
			{
				Process.Start(path);
			}
			catch (Exception ex)
			{
				FriendlyException.Show("Unable to launch " + ProgramCur.Description + ".", ex);
			}
		}
	}
}
