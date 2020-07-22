using OpenDentBusiness;
using System.Collections.Generic;
using System.IO;

namespace OpenDental.Bridges
{
    public static class VixWinOld
	{
		/// <summary>
		/// Sends data for Patient.Cur to the QuikLink directory. No further action is required.
		/// </summary>
		public static void SendData(Program ProgramCur, Patient pat)
		{
			List<ProgramProperty> ForProgram = ProgramProperties.GetForProgram(ProgramCur.Id);
			ProgramProperty PPCur = ProgramProperties.GetCur(ForProgram, "QuikLink directory.");
			string quikLinkDir = PPCur.Value;
			if (pat == null)
			{
				return;
			}
			if (!Directory.Exists(quikLinkDir))
			{
				MessageBox.Show(quikLinkDir + " is not a valid folder.");
				return;
			}
			try
			{
				string patID;
				PPCur = ProgramProperties.GetCur(ForProgram, "Enter 0 to use PatientNum, or 1 to use ChartNum"); ;
				if (PPCur.Value == "0")
				{
					patID = pat.PatNum.ToString().PadLeft(6, '0');
				}
				else
				{
					patID = pat.ChartNumber.PadLeft(6, '0');
				}
				if (patID.Length > 6)
				{
					MessageBox.Show("Patient ID is longer than six digits, so link failed.");
					return;
				}
				string fileName = quikLinkDir + patID + ".DDE";
				//MessageBox.Show(fileName);
				using (StreamWriter sw = new StreamWriter(fileName, false))
				{
					sw.WriteLine("\"" + pat.FName + "\","
						+ "\"" + pat.LName + "\","
						+ "\"" + patID + "\"");
				}
			}
			catch
			{
				MessageBox.Show("Error creating file.");
			}
		}
	}
}
