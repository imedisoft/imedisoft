using CodeBase;
using OpenDentBusiness;
using System;
using System.IO;

namespace OpenDental.Bridges
{
    public static class VixWin
	{
		/// <summary>
		/// Sends data for Patient.Cur by command line interface.
		/// </summary>
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);
			if (pat == null)
			{
				return;
			}

			//Example: c:\vixwin\vixwin -I 123ABC -N Bill^Smith -P X:\VXImages\
			string info = "-I ";
			bool isChartNum = PIn.Bool(ProgramProperties.GetPropVal(ProgramCur.Id, "Enter 0 to use PatientNum, or 1 to use ChartNum"));
			string ppImagePath = ProgramProperties.GetPropVal(ProgramCur.Id, "Optional Image Path");
			if (isChartNum)
			{
				info += pat.ChartNumber;//max 64 char
			}
			else
			{
				info += pat.PatNum.ToString();
			}
			info += " -N " + pat.FName.Replace(" ", "") + "^" + pat.LName.Replace(" ", "");//no spaces allowed

			if (ppImagePath != "")
			{//optional image path
				if (!Directory.Exists(ppImagePath))
				{
					MessageBox.Show("Unable to find image path " + ppImagePath);
					return;
				}
				info += " -P " + ppImagePath;
			}
			try
			{
				ODFileUtils.ProcessStart(path, info, createDirIfNeeded: ppImagePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\r\nFile and command line:\r\n" + path + " " + info);
			}
		}
	}
}
