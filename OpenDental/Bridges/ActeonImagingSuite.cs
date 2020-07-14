using CodeBase;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental.Bridges
{
    public class ActeonImagingSuite
	{
		/// <summary>
		/// Launches the program using command line.
		/// </summary>
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);
			List<ProgramProperty> ForProgram = ProgramProperties.GetForProgram(ProgramCur.ProgramNum);
			if (pat != null)
			{
                string propertyId = ProgramProperties.GetCur(ForProgram, "Enter 0 to use PatientNum, or 1 to use ChartNum").PropertyValue;
				string dobFormat = ProgramProperties.GetCur(ForProgram, "Birthdate format (default yyyyMMdd)").PropertyValue;
                string info = propertyId == "0" ? pat.PatNum.ToString() : pat.ChartNumber;
                info += " \"" + pat.LName.Replace("\"", "") + "\" \"" + pat.FName.Replace("\"", "") + "\" \"" + pat.Birthdate.ToString(dobFormat) + "\"";
				try
				{
					ODFileUtils.ProcessStart(path, ProgramCur.CommandLine + info);
				}
				catch
				{
					MessageBox.Show(path + " is not available, or there is an error in the command line options.");
				}
			} // if patient is loaded
			else
			{
				try
				{
					ODFileUtils.ProcessStart(path);
				}
				catch
				{
					MessageBox.Show(path + " is not available.");
				}
			}
		}
	}
}
