using CodeBase;
using OpenDentBusiness;
using System.Diagnostics;

namespace OpenDental.Bridges
{
    public static class Dxis
	{
		/// <summary>
		/// Launches the program using a combination of command line characters and the patient.Cur data.
		/// </summary>
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);

			// usage: C:\Dxis\Dxis.exe /i /t:UniqueID - Practice Name
			// The UniqueID can be a combo of patient name and id.  I think we'll combine Lname,Fname,PatNum
			if (pat == null)
			{
				MsgBox.Show("Please select a patient first.");
				return;
			}

            string info = "/i /t:" + pat.LName + " " + pat.FName + " " + pat.PatNum.ToString() + " - " + PrefC.GetString(PrefName.PracticeTitle);
			try
			{
				Process process = ODFileUtils.ProcessStart(path, info);

				process.WaitForExit(); // puts OD in sleep mode because the pano is so resource intensive.
			}
			catch
			{
				MessageBox.Show(path + " is not available.");
			}
		}
	}
}
