using CodeBase;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental.Bridges
{
    public static class Lightyear
	{
		/// <summary>
		/// Launches the program using the patient.Cur data.
		/// </summary>
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);

			List<ProgramProperty> ForProgram = ProgramProperties.GetForProgram(ProgramCur.Id); ;
			if (pat == null)
			{
				MessageBox.Show("Please select a patient first");
				return;
			}
			string info = "";
			
			//Patient id can be any string format
			ProgramProperty PPCur = ProgramProperties.GetCur(ForProgram, "Enter 0 to use PatientNum, or 1 to use ChartNum"); ;
			if (PPCur.Value == "0")
			{
				info += "-i \"" + pat.PatNum.ToString() + "\" ";
			}
			else
			{
				info += "-i \"" + pat.ChartNumber.Replace("\"", "") + "\" ";
			}
			info += "-n \"" + pat.LName.Replace("\"", "") + ", " + pat.FName.Replace("\"", "") + "\"";

			try
			{
				ODFileUtils.ProcessStart(path, info);
			}
			catch
			{
				MessageBox.Show(path + " is not available.");
			}
		}
	}
}
