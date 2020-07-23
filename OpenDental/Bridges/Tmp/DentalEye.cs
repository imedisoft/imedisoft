using CodeBase;
using NDde;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace OpenDental.Bridges
{
    public static class DentalEye
	{
		/// <summary>
		/// Launches the program if necessary.
		/// Then passes patient.Cur data using DDE.
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

			//The path is available in the registry, but we'll just make the user enter it.
			if (!File.Exists(path))
			{
				MessageBox.Show("Could not find " + path);
				return;
			}

			//Make sure the program is running
			if (Process.GetProcessesByName("DentalEye").Length == 0)
			{
				ODFileUtils.ProcessStart(path);
				Thread.Sleep(TimeSpan.FromSeconds(4));
			}

			//command="[Add][PatNum][Fname][Lname][Address|Address2|City, ST Zip][phone1][phone2][mobile phone][email][sex(M/F)][birthdate (YYYY-MM-DD)]"
			ProgramProperty PPCur = ProgramProperties.GetCur(ForProgram, "Enter 0 to use PatientNum, or 1 to use ChartNum"); ;
			
			string patID;
			if (PPCur.Value == "0")
			{
				patID = pat.PatNum.ToString();
			}
			else
			{
				if (pat.ChartNumber == "")
				{
					MessageBox.Show("ChartNumber for this patient is blank.");
					return;
				}
				patID = pat.ChartNumber;
			}

			string command = "[Add][" + patID + "]"
				+ "[" + pat.FName + "]"
				+ "[" + pat.LName + "]"
				+ "[" + pat.Address + "|";
			if (pat.Address2 != "")
			{
				command += pat.Address2 + "|";
			}
			command += pat.City + ", " + pat.State + " " + pat.Zip + "]"
				+ "[" + pat.HmPhone + "]"
				+ "[" + pat.WkPhone + "]"
				+ "[" + pat.WirelessPhone + "]"
				+ "[" + pat.Email + "]";
			if (pat.Gender == PatientGender.Female)
				command += "[F]";
			else
				command += "[M]";
			command += "[" + pat.Birthdate.ToString("yyyy-MM-dd") + "]";

			try
			{
				//Create a context that uses a dedicated thread for DDE message pumping.
				using (DdeContext context = new DdeContext())
				{
					//Create a client.
					using (DdeClient client = new DdeClient("DENTEYE", "Patient", context))
					{
						//Establish the conversation.
						client.Connect();
						//Add patient or modify if already exists
						client.Execute(command, 2000);//timeout 2 secs
													  //Then, select patient
						command = "[Search][" + patID + "]";
						client.Execute(command, 2000);
					}
				}
			}
			catch
			{
			}
		}
	}
}