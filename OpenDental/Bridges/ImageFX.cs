using CodeBase;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental.Bridges
{
    public static class ImageFX
	{
		/// <summary>
		/// Launches the program using a combination of command line characters and the patient.Cur data.
		/// They also have an available file based method which passes more information, but we don't use it yet.
		/// </summary>
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);
			List<ProgramProperty> ForProgram = ProgramProperties.GetForProgram(ProgramCur.ProgramNum); ;
			if (pat != null)
			{
				string info = "-";
				ProgramProperty PPCur = ProgramProperties.GetCur(ForProgram, "Enter 0 to use PatientNum, or 1 to use ChartNum"); ;
				if (PPCur.PropertyValue == "0")
				{
					info += ClipTo(pat.PatNum.ToString(), 10) + ";";
				}
				else
				{
					info += ClipTo(pat.ChartNumber, 10) + ";";
				}
				info += ClipTo(pat.FName, 25) + ";"
					+ ClipTo(pat.LName, 25) + ";"
					+ ClipTo(pat.SSN, 15) + ";"
					+ pat.Birthdate.ToString("MM/dd/yyyy") + ";";
				try
				{
					ODFileUtils.ProcessStart(path, info);
				}
				catch
				{
					MessageBox.Show(path + " is not available.");
				}
			}
			else
			{
				try
				{
					ODFileUtils.ProcessStart(path);//should start ImageFX without bringing up a pt.
				}
				catch
				{
					MessageBox.Show(path + " is not available.");
				}
			}
		}

		/// <summary>
		/// Clips the length of the string as well as making sure invalid characters don't slip through.
		/// </summary>
		private static string ClipTo(string input, int length)
		{
			string retVal = input.Replace(";", "");
			if (retVal.Length > length)
			{
				retVal = retVal.Substring(0, length);
			}
			return retVal;
		}
	}
}
