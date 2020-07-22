using CodeBase;
using OpenDentBusiness;
using System;

namespace OpenDental.Bridges
{
    public static class Adstra
	{
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);
			if (pat == null)
			{
				MsgBox.Show("Please select a patient first.");
				return;
			}
			string str = "";
			str += Tidy(pat.LName) + ",";
			str += Tidy(pat.FName) + ",";
			if (ProgramProperties.GetPropVal(ProgramCur.Id, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
			{
				str += pat.PatNum.ToString() + ",,";
			}
			else
			{
				str += "," + Tidy(pat.ChartNumber) + ",";
			}
			//If birthdates are optional, only send them if they are valid.
			str += pat.Birthdate.ToString("yyyy/MM/dd"); //changed to match bridge format. Was MM/dd/yyy
			try
			{
				ODFileUtils.ProcessStart(path, str);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		///<summary>Removes semicolons and spaces.</summary>
		private static string Tidy(string input)
		{
			string retVal = input.Replace(";", "");
			retVal = retVal.Replace(" ", "");
			return retVal;
		}
	}
}

