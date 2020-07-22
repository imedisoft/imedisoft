using CodeBase;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace OpenDental.Bridges
{
    /// <summary>
    /// Also used by the XDR bridge until 19.2 when XDR was broken out into its own bridge.
    /// </summary>
    public static class Dexis
	{
		/// <summary>
		/// Sends data for Patient.Cur to the InfoFile and launches the program.
		/// </summary>
		public static void SendData(Program program, Patient patient)
		{
			string path = Programs.GetProgramPath(program);
			var programProperties = ProgramProperties.GetForProgram(program.Id);
			var programProperty = ProgramProperties.GetCur(programProperties, "InfoFile path");

			string infoFile = programProperty.Value;
			if (infoFile.Trim() == "")
			{
				infoFile = Path.Combine(Path.GetTempPath(), "infofile.txt");
			}

			if (patient != null)
			{
				try
				{
					// patientID can be any string format, max 8 char.
					// There is no validation to ensure that length is 8 char or less.
					programProperty = ProgramProperties.GetCur(programProperties, "Enter 0 to use PatientNum, or 1 to use ChartNum");

					string id = "";
					if (programProperty.Value == "0")
					{
						id = patient.PatNum.ToString();
					}
					else
					{
						id = patient.ChartNumber;
					}

					// Encoding 1252 was specifically requested by the XDR development team to help with accented characters (ex Canadian customers).
					// On 05/19/2015, a reseller noticed UTF8 encoding in the Dexis bridge caused a similar issue.
					// We decided it was safe to switch Dexis from using UTF8 to code page 1252 because the bridge depends entirely on the bridge ID,
					// not the patient names.  Thus there is no chance of breaking the Dexis bridge by using code page 1252 instead.
					// 06/01/2015 A customer tested and confirmed that using the XDR bridge and thus coding page 1252, solved the special characters issue.
					Encoding encoding = Encoding.GetEncoding(1252);

					using var memoryStream = new MemoryStream();

					using (var streamWriter = new StreamWriter(memoryStream, encoding))
					{
						streamWriter.WriteLine(
							patient.LName + ", " + patient.FName + "  " +
							patient.Birthdate.ToShortDateString() + "  (" + id + ")");

						streamWriter.WriteLine("PN=" + id);
						streamWriter.WriteLine("LN=" + patient.LName);
						streamWriter.WriteLine("FN=" + patient.FName);
						streamWriter.WriteLine("BD=" + patient.Birthdate.ToShortDateString());

						if (patient.Gender == PatientGender.Female)
							streamWriter.WriteLine("SX=F");
						else
							streamWriter.WriteLine("SX=M");
					}

					ODFileUtils.WriteAllBytesThenStart(infoFile, memoryStream.ToArray(), path, "\"@" + infoFile + "\"");
				}
				catch
				{
					MessageBox.Show(path + " is not available.");
				}
			}
			else
			{
				try
				{
					Process.Start(path);
				}
				catch
				{
					MessageBox.Show(path + " is not available.");
				}
			}
		}

	}
}
