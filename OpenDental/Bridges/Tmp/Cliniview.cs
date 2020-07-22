using CodeBase;
using OpenDentBusiness;
using System;
using System.IO;

namespace OpenDental.Bridges
{
    public static class Cliniview
	{
		/// <summary>
		/// Launches the program using command line.
		/// </summary>
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);

			//usage: C:\Program Files\CliniView\CliniView.exe -123;John;Smith;123456789;01/01/1980
			//Critical not to have any spaces in argument string.
			//We got this info directly from the programmers at CliniView
			if (pat == null)
			{
				MsgBox.Show("Please select a patient first.");
				return;
			}

			if (!File.Exists(path))
			{
				MessageBox.Show(path + " not found.");
				return;
			}

			string info = "-";
			if (ProgramProperties.GetPropVal(ProgramCur.Id, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "1")
			{
				if (pat.ChartNumber == "")
				{
					MsgBox.Show("This patient has no ChartNumber entered.");
					return;
				}
				info += pat.ChartNumber;
			}
			else
			{
				info += pat.PatNum.ToString();
			}

			info += ";" + Tidy(pat.FName)
				+ ";" + Tidy(pat.LName)
				+ ";" + pat.SSN//dashes already missing
				+ ";" + pat.Birthdate.ToString("MM/dd/yyyy");

			try
			{
				ODFileUtils.ProcessStart(path, info);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Removes semicolons and spaces.
		/// </summary>
		private static string Tidy(string input)
		{
			string retVal = input.Replace(";", "");//get rid of any semicolons.
			retVal = retVal.Replace(" ", "");
			return retVal;
		}
	}
}
