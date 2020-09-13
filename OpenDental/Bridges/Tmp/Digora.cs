using CodeBase;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace OpenDental.Bridges
{
    public static class Digora
	{
		/// <summary>
		/// We will use the clipboard interface, although there is an ole automation interface available.
		/// </summary>
		/// 
		//Command format: $$DFWIN$$ <Command> <Options>
		//Return value: $$DFWOUT$$<Return value>[\nReturn string] (we will ignore this value for now)
		//$$DFWIN$$ OPEN -n"LName, FName" -c"PatNum" -r -a
		//option -r creates patient if not found, -a changes focus to Digora
		public static void SendData(Program ProgramCur, Patient pat)
		{
			if (pat == null)
			{
				MsgBox.Show("No patient selected.");
				return;
			}

			List<ProgramProperty> ForProgram = ProgramProperties.GetForProgram(ProgramCur.Id);
			string info = "$$DFWIN$$ OPEN -n\"" + Tidy(pat.LName) + ", " + Tidy(pat.FName) + "\" -c\"";

			ProgramProperty PPCur = ProgramProperties.GetCur(ForProgram, "Enter 0 to use PatientNum, or 1 to use ChartNum"); ;
			if (PPCur.Value == "0")
			{
				info += pat.PatNum.ToString();
			}
			else
			{
				info += pat.ChartNumber;
			}

			info += "\" -r -a";
			try
			{
				ODClipboard.Text = info;
			}
			catch (Exception)
			{
				// The clipboard will sometimes fail to SetText for many different reasons.  Often times another attempt will be successful.
				MsgBox.Show("Error accessing the clipboard, please try again.");
				return;
			}
		}

		private static string Tidy(string str)
		{
			return str.Replace("\"", "");
		}
	}
}
