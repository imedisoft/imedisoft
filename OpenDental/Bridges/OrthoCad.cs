using CodeBase;
using OpenDentBusiness;
using System;

namespace OpenDental.Bridges
{
    public static class OrthoCad
	{
		public static void SendData(Program ProgramCur, Patient pat)
		{
			if (pat == null)
			{
				MsgBox.Show("Please select a patient first.");
				return;
			}

			string path = Programs.GetProgramPath(ProgramCur);
			string cmd = "";
			if (ProgramProperties.GetPropVal(ProgramCur.ProgramNum, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
			{
				cmd += "-patient_id=" + POut.Long(pat.PatNum);
			}
			else
			{
				cmd += "-chart_number=" + pat.ChartNumber;
			}

			try
			{
				ODFileUtils.ProcessStart(path, cmd);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
