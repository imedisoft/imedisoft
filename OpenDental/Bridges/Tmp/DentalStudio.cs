using CodeBase;
using OpenDentBusiness;
using System;

namespace OpenDental.Bridges
{
    public static class DentalStudio
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
			//The parameters must be in a specific order.

			//Param1: UserName
			string userName = ProgramProperties.GetPropVal(ProgramCur.Id, "UserName (clear to use OD username)");
			if (userName == "")
			{//Give the customer the option to use OD usernames.
				userName = Security.CurrentUser.UserName;
			}
			str += Tidy(userName) + " ";

			//Param2: UserPassword
			string userPassword = ProgramProperties.GetPropVal(ProgramCur.Id, "UserPassword");
			if (userName == "")
			{//Give the customer the option to use OD usernames.
			 //TODO: Dental Studio might need to be contacted or our bridge might need to be enhanced for sending / updating password hashes.
				userPassword = Security.CurrentUser.PasswordHash;
			}
			str += Tidy(userPassword) + " ";

			//Param3: PatientLName
			str += Tidy(pat.LName) + " ";

			//Param4: PatientFName
			str += Tidy(pat.FName) + " ";

			//Param5: PatientSex
			if (pat.Gender == PatientGender.Female)
			{
				str += Tidy("F") + " ";
			}
			else if (pat.Gender == PatientGender.Male)
			{
				str += Tidy("M") + " ";
			}
			else
			{//unknown
				str += Tidy("O") + " ";//O=Other
			}

			//Param6: PatientID
			if (ProgramProperties.GetPropVal(ProgramCur.Id, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
			{
				str += Tidy(pat.PatNum.ToString()) + " ";
			}
			else
			{
				str += Tidy(pat.ChartNumber) + " ";
			}

			//Param7: Action
			str += Tidy("O") + " ";//For future use.  Must always be O for now (not zero).  The O stands for "Open".
								   //Param8: PatientBDate
			str += Tidy(pat.Birthdate.ToString("MM/dd/yyyy"));//DentalStudio patient matching does not depend on birthdate, so we cand send 01/01/0001. DentalStudio will update the birthdate after updated in OD.
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
		/// Removes double-quotes, then surrounds with double-quotes.
		/// </summary>
		private static string Tidy(string input)
		{
			string retVal = input.Replace("\"", "");
			retVal = "\"" + retVal + "\"";
			return retVal;
		}
	}
}
