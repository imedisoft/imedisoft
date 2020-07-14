using CodeBase;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental.Bridges
{
    public static class Trophy
	{
		/// <summary>
		/// Launches the program using a combination of command line characters and the patient.Cur data.
		/// </summary>
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);
			List<ProgramProperty> ForProgram = ProgramProperties.GetForProgram(ProgramCur.ProgramNum); ;
			if (pat != null)
			{
				ProgramProperty PPCur = ProgramProperties.GetCur(ForProgram, "Storage Path");
				string comline = "-P" + PPCur.PropertyValue + @"\";
				//Patient id can be any string format
				PPCur = ProgramProperties.GetCur(ForProgram, "Enter 0 to use PatientNum, or 1 to use ChartNum"); ;
				if (PPCur.PropertyValue == "0")
				{
					comline += pat.PatNum.ToString();
				}
				else
				{
					comline += pat.ChartNumber;
				}
				comline += " -N" + pat.LName + ", " + pat.FName;
				comline = comline.Replace("\"", "");//gets rid of any quotes
				comline = comline.Replace("'", "");//gets rid of any single quotes
				try
				{
					ODFileUtils.ProcessStart(path, comline);
				}
				catch
				{
					MessageBox.Show(path + " is not available.");
				}
			}//if patient is loaded
			else
			{
				try
				{
					ODFileUtils.ProcessStart(path);//should start Trophy without bringing up a pt.
				}
				catch
				{
					MessageBox.Show(path + " is not available.");
				}
			}
		}
	}
}
