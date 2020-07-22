using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenDental.Bridges
{
    public static class DBSWin
	{
		/// <summary>
		/// Sends data for Patient.Cur to an import file which DBSWin will automatically recognize.
		/// </summary>
		public static void SendData(Program ProgramCur, Patient pat)
		{
			if (pat == null)
			{
				MsgBox.Show("Please select a patient first.");
				return;
			}

			List<ProgramProperty> ForProgram = ProgramProperties.GetForProgram(ProgramCur.Id);
			ProgramProperty PPCur = ProgramProperties.GetCur(ForProgram, "Text file path");
			
			string infoFile = PPCur.Value;
			try
			{
				using (StreamWriter sw = new StreamWriter(infoFile, false))
				{
					//PATLASTNAME;PATFIRSTNAME;PATBIRTHDAY;PATCARDNUMBER;PATTOWN;PATSTREET;PATPHONENUMBER;PATTITLE;PATSEX;PATPOSTALCODE;
					//everything after birthday is optional
					sw.Write(Tidy(pat.LName) + ";");
					sw.Write(Tidy(pat.FName) + ";");
					sw.Write(pat.Birthdate.ToString("d.M.yyyy") + ";");
					PPCur = ProgramProperties.GetCur(ForProgram, "Enter 0 to use PatientNum, or 1 to use ChartNum"); ;
					if (PPCur.Value == "0")
					{
						sw.Write(pat.PatNum.ToString() + ";");
					}
					else
					{
						sw.Write(Tidy(pat.ChartNumber) + ";");
					}
					sw.Write(Tidy(pat.City) + ";");
					sw.Write(Tidy(pat.Address) + ";");
					sw.Write(Tidy(pat.HmPhone) + ";");
					sw.Write(";");//title
					if (pat.Gender == PatientGender.Female)
						sw.Write("f;");
					else
						sw.Write("m;");
					sw.Write(Tidy(pat.Zip) + ";");
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		/// <summary>
		/// Strips out the semicolons.
		/// </summary>
		private static string Tidy(string str)
		{
			return str.Replace(";", "");
		}
	}
}
