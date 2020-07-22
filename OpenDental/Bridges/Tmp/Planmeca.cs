using CodeBase;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental.Bridges
{
    /// <summary>
    /// This bridge is now known as "Dimaxis" in the program links list.
    /// </summary>
    public static class Planmeca
	{
		/// <summary>
		/// Launches the program using the patient.Cur data.
		/// </summary>
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);

			//DxStart.exe ”PatientID” ”FamilyName” ”FirstName” ”BirthDate”
			List<ProgramProperty> ForProgram = ProgramProperties.GetForProgram(ProgramCur.Id);
			if (pat == null)
			{
				MessageBox.Show("Please select a patient first");
				return;
			}
			string info = "";

			//Patient id can be any string format
			ProgramProperty PPCur = ProgramProperties.GetCur(ForProgram, "Enter 0 to use PatientNum, or 1 to use ChartNum");
			string bDayFormat = ProgramProperties.GetCur(ForProgram, "Birthdate format (usually dd/MM/yyyy or MM/dd/yyyy)").Value;
			if (PPCur.Value == "0")
			{
				info += "\"" + pat.PatNum.ToString() + "\" ";
			}
			else
			{
				info += "\"" + pat.ChartNumber.Replace("\"", "") + "\" ";
			}
			info += "\"" + pat.LName.Replace("\"", "") + "\" "
				+ "\"" + pat.FName.Replace("\"", "") + "\" "
				+ "\"" + pat.Birthdate.ToString(bDayFormat) + "\"";//Format is dd/MM/yyyy by default. Planmeca also told a customer that the format should be MM/dd/yyyy (04/08/2016 cm).
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
