using CodeBase;
using OpenDentBusiness;
using System;

namespace OpenDental.Bridges
{
    public static class SmartDent
	{
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);
			if (pat == null)
			{
				try
				{
					ODFileUtils.ProcessStart(path);//should start SMARTDent without bringing up a pt.
				}
				catch
				{
					MessageBox.Show(path + " is not available.");
				}
				return;
			}

			string info = "";
			if (ProgramProperties.GetPropVal(ProgramCur.ProgramNum, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
			{
				info += "\"" + pat.PatNum.ToString() + "\" ";
			}
			else
			{
				info += "\"" + Tidy(pat.ChartNumber) + "\" ";
			}

			if (pat.FName != "")
			{
				info += "\"" + Tidy(pat.FName) + " ";
			}
			else
			{
				info += "\"";
			}

			info += Tidy(pat.LName) + "\"";
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
			retVal = retVal.Replace("\"", "");
			return retVal;
		}
	}
}
