using CodeBase;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental.Bridges
{
    public static class PerioPal
	{
		/// <summary>
		/// Launches the program using command line.
		/// </summary>
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);

			//Usage: [Application Path]PerioPal "PtChart; PtName ; PtBday; PtMedAlert;"
			List<ProgramProperty> ForProgram = ProgramProperties.GetForProgram(ProgramCur.Id); ;
			if (pat == null)
			{
				return;
			}
			string info = "\"";

			ProgramProperty PPCur = ProgramProperties.GetCur(ForProgram, "Enter 0 to use PatientNum, or 1 to use ChartNum"); ;
			if (PPCur.Value == "0")
			{
				info += pat.PatNum.ToString();
			}
			else
			{
				info += Cleanup(pat.ChartNumber);
			}

			info += ";"
				+ Cleanup(pat.LName) + ";"
				+ Cleanup(pat.FName) + ";"
				+ pat.Birthdate.ToShortDateString() + ";";

			bool hasMedicalAlert = false;
			if (pat.MedUrgNote != "")
			{
				hasMedicalAlert = true;
			}
			if (pat.Premed)
			{
				hasMedicalAlert = true;
			}
			if (hasMedicalAlert)
			{
				info += "Y;";
			}
			else
			{
				info += "N;";
			}

			try
			{
				ODFileUtils.ProcessStart(path, info);
			}
			catch
			{
				MessageBox.Show(path + " " + info + " is not available.");
			}
		}

		/// <summary>
		/// Makes sure invalid characters don't slip through.
		/// </summary>
		private static string Cleanup(string input)
		{
			string retVal = input;
			retVal = retVal.Replace(";", "");
			retVal = retVal.Replace(" ", "");
			return retVal;
		}
	}
}
