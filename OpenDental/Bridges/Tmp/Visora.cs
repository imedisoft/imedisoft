using CodeBase;
using OpenDentBusiness;

namespace OpenDental.Bridges
{
    public static class Visora
	{
		public static void SendData(Program ProgramCur, Patient pat)
		{
			string path = Programs.GetProgramPath(ProgramCur);
			//filepath.exe -patid 123 -fname John -lname Doe -dob 01/25/1962 -ssn 123456789 -gender M
			if (pat == null)
			{
				MessageBox.Show("Please select a patient first");
				return;
			}
			string info = "-patid ";
			if (ProgramProperties.GetPropVal(ProgramCur.Id, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
			{
				info += pat.PatNum.ToString() + " ";
			}
			else
			{
				info += pat.ChartNumber + " ";
			}
			info += "-fname " + Tidy(pat.FName) + " "
				+ "-lname " + Tidy(pat.LName) + " ";
			if (pat.Birthdate.Year > 1880)
			{
				info += "-dob " + pat.Birthdate.ToShortDateString() + " ";
			}
			else
			{
				info += "-dob  ";
			}
			if (pat.SSN.Replace("0", "").Trim() != "")
			{//An SSN which is all zeros will be treated as a blank SSN.  Needed for eCW, since eCW sets SSN to 000-00-0000 if the patient does not have an SSN.
				info += "-ssn " + pat.SSN + " ";
			}
			else
			{
				info += "-ssn  ";
			}
			if (pat.Gender == PatientGender.Female)
			{
				info += "-gender F";
			}
			else
			{
				info += "-gender M";
			}
			try
			{
				ODFileUtils.ProcessStart(path, info);
			}
			catch
			{
				MessageBox.Show(path + " is not available.");
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
