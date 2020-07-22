using CodeBase;
using OpenDentBusiness;
using System;

namespace OpenDental.Bridges
{
    public static class HandyDentist
	{
		/// <summary>
		/// Launches the program using a combination of command line characters and the patient.Cur data.
		/// </summary>
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);
			if (pat == null)
			{
				try
				{
					ODFileUtils.ProcessStart(path);//should start HandyDentist without bringing up a pt.
				}
				catch
				{
					MessageBox.Show(path + " is not available.");
				}
				return;
			}
			string info = "-no:\"";

			if (ProgramProperties.GetPropVal(ProgramCur.Id, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
			{
				info += pat.PatNum.ToString();
			}
			else
			{
				if (pat.ChartNumber == null || pat.ChartNumber == "")
				{
					MsgBox.Show("This patient does not have a chart number.");
					return;
				}
				info += Tidy(pat.ChartNumber);
			}

			info += "\" -fname:\"" + Tidy(pat.FName) + "\" ";
			info += " -lname:\"" + Tidy(pat.LName) + "\" ";

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
			string retVal = input.Replace(";", "");
			retVal = retVal.Replace(" ", "");
			return retVal;
		}
	}
}
