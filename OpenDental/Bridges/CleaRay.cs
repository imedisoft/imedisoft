using CodeBase;
using OpenDentBusiness;
using System;

namespace OpenDental.Bridges
{
    public static class CleaRay
	{
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);
			if (pat == null)
			{
				try
				{
					ODFileUtils.ProcessStart(path);
				}
				catch
				{
					MessageBox.Show(path + " is not available.");
				}
				return;
			}

			string str = "";
			if (ProgramProperties.GetPropVal(ProgramCur.ProgramNum, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
			{
				str += "/ID:" + TidyAndEncapsulate(pat.PatNum.ToString()) + " ";
			}
			else
			{
				str += "/ID:" + TidyAndEncapsulate(pat.ChartNumber) + " ";
			}

			str += "/LN:" + TidyAndEncapsulate(pat.LName) + " ";
			str += "/N:" + TidyAndEncapsulate(pat.FName) + " ";
			try
			{
				ODFileUtils.ProcessStart(path, str);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Removes semicolons, forward slashes, and spaces.
		/// </summary>
		private static string TidyAndEncapsulate(string input)
		{
			string retVal = input.Replace(";", "");
			retVal = retVal.Replace("/", "");
			return "\"" + retVal + "\"";
		}
	}
}
