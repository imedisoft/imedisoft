using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace OpenDentBusiness
{
    public partial class Programs
	{
		[CacheGroup(nameof(InvalidType.Programs))]
		private class ProgramCache : ListCache<Program>
		{
            protected override IEnumerable<Program> Load()
				=> SelectMany("SELECT * FROM program ORDER BY ProgDesc");
		}

		private static readonly ProgramCache cache = new ProgramCache();

		public static List<Program> GetListDeep() 
			=> cache.GetAll();

		public static Program GetFirstOrDefault(Predicate<Program> match)
			=> cache.FirstOrDefault(match);

		public static List<Program> GetWhere(Predicate<Program> match) 
			=> cache.Find(match);

		public static void RefreshCache() 
			=> cache.Refresh();

		/// <summary>
		/// Returns true if a Program link with the given name or number exists and is enabled. Handles null.
		/// </summary>
		public static bool IsEnabled(ProgramName programName) 
			=> GetFirstOrDefault(x => x.Name == programName.ToString())?.Enabled ?? false;

		public static bool IsEnabled(long programId) 
			=> GetFirstOrDefault(x => x.Id == programId)?.Enabled ?? false;

		/// <summary>
		/// Returns the Program of the passed in ProgramNum.
		/// Will be null if a Program is not found.
		/// </summary>
		public static Program GetProgram(long programNum)
			=> GetFirstOrDefault(x => x.Id == programNum);

		/// <summary>
		/// Supply a valid program Name, and this will set Cur to be the corresponding Program object.
		/// </summary>
		public static Program GetCur(ProgramName progName) 
			=> GetFirstOrDefault(x => x.Name == progName.ToString());

		/// <summary>
		/// Supply a valid program Name. Will return 0 if not found.
		/// </summary>
		public static long GetProgramNum(ProgramName progName) 
			=> GetCur(progName)?.Id ?? 0;

		/// <summary>
		/// Using eClinicalWorks tight integration.
		/// </summary>
		[Obsolete]
		public static bool UsingEcwTightMode()
			=> IsEnabled(ProgramName.eClinicalWorks) && ProgramProperties.GetPropVal(ProgramName.eClinicalWorks, "eClinicalWorksMode") == "0";

		/// <summary>
		/// Using eClinicalWorks full mode.
		/// </summary>
		[Obsolete]
		public static bool UsingEcwFullMode()
			=> IsEnabled(ProgramName.eClinicalWorks) && ProgramProperties.GetPropVal(ProgramName.eClinicalWorks, "eClinicalWorksMode") == "2";

		/// <summary>
		/// Returns true if using eCW in tight or full mode.
		/// In these modes, appointments ARE allowed to overlap because we block users from seeing them.
		/// </summary>
		[Obsolete] 
		public static bool UsingEcwTightOrFullMode() 
			=> UsingEcwTightMode() || UsingEcwFullMode();

		public static bool UsingOrion 
			=> IsEnabled(ProgramName.Orion);

		/// <summary>
		/// Returns the local override path if available or returns original program path. Always returns a valid path.
		/// </summary>
		public static string GetProgramPath(Program program)
		{
			string overridePath = ProgramProperties.GetLocalPathOverrideForProgram(program.Id);
			if (overridePath != "")
			{
				return overridePath;
			}

			return program.Path;
		}

		/// <summary>
		/// Returns true if input program is a static program. 
		/// Static programs are ones we do not want the user to be able to modify in some way.
		/// </summary>
		public static bool IsStatic(Program prog)
		{
			if (prog.Name == ProgramName.RapidCall.ToString())
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// For each enabled bridge, if the bridge uses a file to transmit patient data to the 
		/// other software, then we need to remove the files or clear the files when OD is exiting.
		/// Required for EHR 2014 module d.7 (as stated by proctor).
		/// </summary>
		public static void ScrubExportedPatientData()
		{
            ScrubFileForProperty(ProgramName.Apixia, "System path to Apixia Digital Imaging ini file"); // C:\Program Files\Digirex\Switch.ini
            ScrubFileForProperty(ProgramName.Carestream, "Patient.ini path"); // C:\Carestream\Patient.ini
			ScrubFileForProperty(ProgramName.DBSWin, "Text file path"); // C:\patdata.txt
			ScrubFileForProperty(ProgramName.Dexis, "InfoFile path"); // InfoFile.txt
			ScrubFileForProperty(ProgramName.Dolphin, "Filename"); // C:\Dolphin\Import\Import.txt

			var program = GetCur(ProgramName.EwooEZDent);
			if (program.Enabled)
            {
                var path = GetProgramPath(program);
                if (File.Exists(path))
                {
                    string dir = Path.GetDirectoryName(path);
                    string linkage = Path.Combine(dir, "linkage.xml");
                    if (File.Exists(linkage))
                    {
                        try
                        {
                            File.Delete(linkage);
                        }
                        catch
                        {
                            // Another instance of OD might be closing at the same time, in which case the delete will fail.
							// Could also be a permission issue or a concurrency issue. Ignore.
                        }
                    }
                }
            }

            ScrubFileForProperty(ProgramName.HouseCalls, "Export Path", "Appt.txt"); // C:\HouseCalls\Appt.txt
			ScrubFileForProperty(ProgramName.iCat, "XML output file path"); // C:\iCat\Out\pm.xml
			ScrubFileForProperty(ProgramName.MediaDent, "Text file path"); // C:\MediadentInfo.txt
			ScrubFileForProperty(ProgramName.Patterson, "System path to Patterson Imaging ini"); // C:\Program Files\PDI\Shared files\Imaging.ini

			program = GetCur(ProgramName.Sirona);
			if (program.Enabled)
			{
				var path = GetProgramPath(program); // Read file C:\sidexis\sifiledb.ini

				path = Path.Combine(Path.GetDirectoryName(path), "sifiledb.ini");
				if (File.Exists(path))
				{
					string sendBox = ReadValueFromIni("FromStation0", "File", path);
					if (File.Exists(sendBox))
					{
						File.WriteAllText(sendBox, ""); // Clear the sendbox instead of deleting.
					}
				}
			}

			ScrubFileForProperty(ProgramName.TigerView, "Tiger1.ini path", null, false); // C:\Program Files\PDI\Shared files\Imaging.ini.  TigerView complains if the file is not present.
			ScrubFileForProperty(ProgramName.XDR, "InfoFile path"); // C:\XDRClient\Bin\infofile.txt
		}

		/// <summary>
		/// Needed for Sirona bridge data scrub in ScrubExportedPatientData().
		/// </summary>
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileStringFromIni(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		/// <summary>
		/// Needed for Sirona bridge data scrub in ScrubExportedPatientData().
		/// </summary>
		private static string ReadValueFromIni(string section, string key, string path)
		{
			var stringBuilder = new StringBuilder(255);
            GetPrivateProfileStringFromIni(section, key, "", stringBuilder, 255, path);

            return stringBuilder.ToString();
		}

		/// <summary>
		/// If isRemovable is false, then the file referenced in the program property will be cleared.
		/// If isRemovable is true, then the file referenced in the program property will be deleted.
		/// </summary>
		private static void ScrubFileForProperty(ProgramName programName, string propertyName, string propertyFileName = null, bool isRemovable = true)
		{
			var program = GetCur(programName);
			if (!program.Enabled)
			{
				return;
			}

			string fileName = Path.Combine(ProgramProperties.GetPropVal(program.Id, propertyName));
			if (!string.IsNullOrEmpty(propertyFileName))
            {
				fileName = Path.Combine(fileName, propertyFileName);
            }

			if (!File.Exists(fileName)) return;

			try
			{
				File.WriteAllText(fileName, ""); // Always clear the file contents, in case deleting fails below.
			}
			catch
			{
				// Another instance of OD might be closing at the same time, in which case the delete will fail.
				// Could also be a permission issue or a concurrency issue. Ignore.
			}

			if (!isRemovable) return;

			try
			{
				File.Delete(fileName);
			}
			catch
			{
				// Another instance of OD might be closing at the same time, in which case the delete will fail.
				// Could also be a permission issue or a concurrency issue. Ignore.
			}
		}

		/// <summary>
		/// Returns true if more than 1 credit card processing program is enabled.
		/// </summary>
		public static bool HasMultipleCreditCardProgramsEnabled()
		{
			return new List<bool> {
				IsEnabled(ProgramName.Xcharge),
				IsEnabled(ProgramName.PayConnect),
				IsEnabled(ProgramName.PaySimple)
			}.Count(x => x == true) >= 2;
		}
	}
}
