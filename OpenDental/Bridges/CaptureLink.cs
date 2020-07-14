using CodeBase;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public static class CaptureLink
	{
		/// <summary>
		/// This bridge has not yet been added to the database.
		/// CaptureLink reads the bridge parameters from the clipboard.
		/// </summary>
		/// Command format: LName FName PatID
		public static void SendData(Program ProgramCur, Patient pat)
		{
			if (pat == null)
			{
				MessageBox.Show("No patient selected.");
				return;
			}

			string path = Programs.GetProgramPath(ProgramCur);
			string info = Tidy(pat.LName) + " ";
			info += Tidy(pat.FName) + " ";
			if (ProgramProperties.GetPropVal(ProgramCur.ProgramNum, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
			{
				info += pat.PatNum.ToString();
			}
			else
			{
				if (pat.ChartNumber == null || pat.ChartNumber == "")
				{
					MsgBox.Show("This patient does not have a chart number.");
					return;
				}
				info += Tidy(pat.ChartNumber);
			}

			Clipboard.Clear();
			ODClipboard.Text = info;

			try
			{
				ODFileUtils.ProcessStart(path);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Removes double-quotes and spaces.
		/// </summary>
		private static string Tidy(string str)
		{
			str = str.Replace("\"", "");
			return str.Replace(" ", "");
		}
	}
}
