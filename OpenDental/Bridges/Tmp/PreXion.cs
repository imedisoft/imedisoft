using CodeBase;
using OpenDentBusiness;
using System;

namespace OpenDental.Bridges
{
    public static class PreXion
	{
		/// <summary>
		/// Launches the program using command line and chartnumber.
		/// </summary>
		public static void SendDataViewer(Program programCur, Patient pat)
		{
			string path = Programs.GetProgramPath(programCur);
			string cmdline = "-l " + ProgramProperties.GetPropVal(programCur.Id, "Username");
			cmdline += " -p " + ProgramProperties.GetPropVal(programCur.Id, "Password");
			if (pat != null)
			{
				cmdline += " -pid ";
				if (ProgramProperties.GetPropVal(programCur.Id, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
				{
					cmdline += pat.PatNum.ToString();
				}
				else
				{
					cmdline += Tidy(pat.ChartNumber);
				}
			}
			cmdline += " " + ProgramProperties.GetPropVal(programCur.Id, "Server Name");
			cmdline += " " + ProgramProperties.GetPropVal(programCur.Id, "Port");
			try
			{
				ODFileUtils.ProcessStart(path, cmdline);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Launches the program using command line and chartnumber.
		/// </summary>
		public static void SendDataAquire(Program programCur, Patient pat)
		{
			string path = Programs.GetProgramPath(programCur);
			if (pat == null)
			{
				MsgBox.Show("Please select a patient first.");
				return;
			}
			string cmdline = "/BRIDGE -pid \"";
			if (ProgramProperties.GetPropVal(programCur.Id, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
			{
				cmdline += pat.PatNum.ToString();
			}
			else
			{
				cmdline += Tidy(pat.ChartNumber);
			}
			cmdline += "\" -first \"";
			if (pat.FName != "")
			{
				cmdline += Tidy(pat.FName);
			}
			else
			{
				cmdline += "NA";//not sure if necessary or not
			}
			cmdline += "\" -last \"";
			if (pat.LName != "")
			{
				cmdline += Tidy(pat.LName);
			}
			else
			{
				cmdline += "NA";
			}
			cmdline += "\" -s \"";
			if (pat.Gender == PatientGender.Female)
			{
				cmdline += "F";
			}
			else if (pat.Gender == PatientGender.Male)
			{
				cmdline += "M";
			}
			else
			{
				cmdline += "U";
			}
			cmdline += "\" -dob \"";
			string birthdateFormat = ProgramProperties.GetPropVal(programCur.Id, "Birthdate format");
			cmdline += pat.Birthdate.ToString(birthdateFormat) + "\"";
			try
			{
				ODFileUtils.ProcessStart(path, cmdline);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Removes semicolons and replaces spaces with underscores.
		/// </summary>
		private static string Tidy(string input)
		{
			string retVal = input.Replace(";", "");
			retVal = retVal.Replace(" ", "_");
			retVal = retVal.Replace("\"", "");
			return retVal;
		}
	}
}
