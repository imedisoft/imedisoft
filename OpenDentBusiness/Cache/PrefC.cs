using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public class PrefC
	{
        private static YN _isVerboseLoggingSession;

        /// <summary>
        /// This property is just a shortcut to this pref to make typing faster.
        /// This pref is used a lot.
        /// </summary>
        // TODO: [Obsolete]
        public static bool RandomKeys => GetBool(PrefName.RandomPrimaryKeys);

		/// <summary>
		/// Logical shortcut to the ClaimPaymentNoShowZeroDate pref.
		/// Returns 0001-01-01 if pref is disabled.
		/// </summary>
		public static DateTime DateClaimReceivedAfter
		{
			get
			{
				DateTime date = DateTime.MinValue;
				int days = GetInt(PrefName.ClaimPaymentNoShowZeroDate);
				if (days >= 0)
				{
					date = DateTime.Today.AddDays(-days);
				}
				return date;
			}
		}

		/// <summary>
		/// This property returns true if the preference for clinics is on and there is at least one non-hidden clinic.
		/// </summary>
		public static bool HasClinicsEnabled => !GetBool(PrefName.EasyNoClinics) && Clinics.GetCount(true) > 0;

		/// <summary>
		/// Returns a list of DefNums that represent WSNPA Generally Allowed blockout types.
		/// </summary>
		public static List<long> GetWebSchedNewPatAllowedBlockouts
		{
			get
			{
				string value = GetString(PrefName.WebSchedNewPatApptIgnoreBlockoutTypes);

				if (string.IsNullOrEmpty(value))
				{
					return new List<long>();
				}

				return value.Split(',').Select(long.Parse).ToList();
			}
		}

		/// <summary>
		/// Returns a list of DefNums that represent Web Sched Recall Generally Allowed blockout types.
		/// </summary>
		public static List<long> GetWebSchedRecallAllowedBlockouts
		{
			get
			{
				string value = GetString(PrefName.WebSchedRecallIgnoreBlockoutTypes);

				if (string.IsNullOrEmpty(value))
				{
					return new List<long>();
				}

				return value.Split(',').Select(long.Parse).ToList();
			}
		}

		/// <summary>
		/// True if the computer name of this session is included in the HasVerboseLogging PrefValue.
		/// </summary>
		public static bool IsVerboseLoggingSession()
		{
			try
			{
				if (Prefs.DictIsNull()) throw new Exception("Prefs cache is null");
				

				if (_isVerboseLoggingSession == YN.Unknown)
				{
					if (GetString(PrefName.HasVerboseLogging).ToLower().Split(',').ToList().Exists(x => x == Environment.MachineName.ToLower()))
					{
						_isVerboseLoggingSession = YN.Yes;
					}
					else
					{
						_isVerboseLoggingSession = YN.No;
					}
				}

				return _isVerboseLoggingSession == YN.Yes;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Call this when we have a new Pref cache in order to re-establish logging preference from this computer.
		/// </summary>
		public static void InvalidateVerboseLogging() => _isVerboseLoggingSession = YN.Unknown;

		/// <summary>
		/// True if the practice has set a window to restrict the times that automatic communications will be sent out.
		/// </summary>
		public static bool DoRestrictAutoSendWindow 
			=> GetDateT(PrefName.AutomaticCommunicationTimeStart).TimeOfDay != GetDateT(PrefName.AutomaticCommunicationTimeEnd).TimeOfDay;


		/// <summary>
		/// Returns a valid DateFormat for patient communications.
		/// If the current preference is invalid, returns "d" which is equivalent to .ToShortDateString()
		/// </summary>
		public static string PatientCommunicationDateFormat
		{
			get
			{
				string format = GetString(PrefName.PatientCommunicationDateFormat);

				try
				{
					DateTime.Today.ToString(format);
				}
				catch
				{
					format = "d"; // Default to "d" which is equivalent to .ToShortDateString()
				}

				return format;
			}
		}

		/// <summary>
		/// Gets a pref of type long.
		/// </summary>
		public static long GetLong(PrefName preferenceName) 
			=> SIn.Long(Prefs.GetOne(preferenceName).ValueString);

		/// <summary>
		/// For UI display when we store a zero/meaningless value as -1. Returns "0" when useZero is true, otherwise "".
		/// </summary>
		public static string GetLongHideNegOne(PrefName preferenceName, bool useZero = false)
		{
			long value = GetLong(preferenceName);

			if (value == -1)
			{
				return useZero ? "0" : "";
			}

			return SOut.Long(value);
		}

		/// <summary>
		/// Gets a pref of type int32.
		/// Used when the pref is an enumeration, itemorder, etc.
		/// Also used for historical queries in ConvertDatabase.
		/// </summary>
		public static int GetInt(PrefName preferenceName) 
			=> SIn.Int(Prefs.GetOne(preferenceName).ValueString);

		/// <summary>
		/// Gets a pref of type byte. Used when the pref is a very small integer (0-255).
		/// </summary>
		public static byte GetByte(PrefName preferenceName) 
			=> SIn.Byte(Prefs.GetOne(preferenceName).ValueString);
		
		/// <summary>
		/// Gets a pref of type double.
		/// </summary>
		public static double GetDouble(PrefName preferenceName) 
			=> SIn.Double(Prefs.GetOne(preferenceName).ValueString);

		/// <summary>
		/// Gets a pref of type double.
		/// </summary>
		public static double GetDouble(PrefName preferenceName, bool doUseEnUSFormat) 
			=> SIn.Double(Prefs.GetOne(preferenceName).ValueString, doUseEnUSFormat);

		/// <summary>
		/// Gets a pref of type bool.
		/// </summary>
		public static bool GetBool(PrefName preferenceName) 
			=> SIn.Bool(Prefs.GetOne(preferenceName).ValueString);

		/// <summary>
		/// Gets the bool value for a YN pref. If Unknown, then returns the default.
		/// If you want the 3 state version, then use PrefC.GetEnum&lt;YN&gt; or PrefC.GetCheckState.
		/// </summary>
		public static bool GetYN(PrefName preferenceName)
		{
			YN yn = (YN)SIn.Int(Prefs.GetOne(preferenceName).ValueString);
			if (yn == YN.Yes)
			{
				return true;
			}

			if (yn == YN.No)
			{
				return false;
			}

			// Unknown, so use the default
			PrefValueType prefValueType = preferenceName.GetValueType();
			if (prefValueType == PrefValueType.YN_DEFAULT_FALSE)
			{
				return false;
			}
			if (prefValueType == PrefValueType.YN_DEFAULT_TRUE)
			{
				return true;
			}

			throw new ArgumentException("Invalid type");
		}

		/// <summary>
		/// Gets YN value for use in pref setup windows with a 3 state checkbox.
		/// </summary>
		public static System.Windows.Forms.CheckState GetYNCheckState(PrefName preferenceName)
		{
			YN yn = (YN)SIn.Int(Prefs.GetOne(preferenceName).ValueString);
			if (yn == YN.Yes)
			{
				return System.Windows.Forms.CheckState.Checked;
			}

			if (yn == YN.No)
			{
				return System.Windows.Forms.CheckState.Unchecked;
			}
			return System.Windows.Forms.CheckState.Indeterminate;
		}

		/// <summary>
		/// Gets a pref of the specified enum type.
		/// </summary>
		public static T GetEnum<T>(PrefName preferenceName) where T : struct, IConvertible 
			=> SIn.Enum<T>(GetInt(preferenceName));

		/// <summary>
		/// Gets a pref of type bool, but will not throw an exception if null or not found.
		/// Indicate whether the silent default is true or false.
		/// </summary>
		public static bool GetBoolSilent(PrefName preferenceName, bool silentDefault)
		{
			if (Prefs.DictIsNull()) return silentDefault;

            try
            {
				var pref = Prefs.GetOne(preferenceName);

				if (pref != null)
                {
					return SIn.Bool(pref.ValueString);
                }
            }
            catch
            {
            }

			return silentDefault;
		}

		/// <summary>
		/// Gets a pref of type string.
		/// </summary>
		public static string GetString(PrefName preferenceName) 
			=> Prefs.GetOne(preferenceName).ValueString;

		/// <summary>
		/// Gets a pref of type string without using the cache.
		/// </summary>
		public static string GetStringNoCache(PrefName preferenceName) 
			=> Database.ExecuteString("SELECT ValueString FROM preference WHERE PrefName='" + SOut.String(preferenceName.ToString()) + "'");

		/// <summary>
		/// Gets a pref of type string.  Will not throw an exception if null or not found.
		/// </summary>
		public static string GetStringSilent(PrefName preferenceName)
		{
			if (Prefs.DictIsNull()) return "";

            try
            {
				return Prefs.GetOne(preferenceName)?.ValueString ?? "";
            }
            catch
            {
            }

			return "";
		}

		/// <summary>
		/// Gets a pref of type date.
		/// </summary>
		public static DateTime GetDate(PrefName preferenceName) 
			=> SIn.Date(Prefs.GetOne(preferenceName).ValueString);

		/// <summary>
		/// Gets a pref of type datetime.
		/// </summary>
		public static DateTime GetDateT(PrefName preferenceName) // TODO: Deprecate this in favor of GetDate()
			=> SIn.Date(Prefs.GetOne(preferenceName).ValueString);

		/// <summary>
		/// Gets a color from an int32 pref.
		/// </summary>
		public static Color GetColor(PrefName preferenceName) 
			=> Color.FromArgb(SIn.Int(Prefs.GetOne(preferenceName).ValueString));

		/// <summary>
		/// Used sometimes for prefs that are not part of the enum, especially for outside developers.
		/// </summary>
		public static string GetRaw(string preferenceName) 
			=> Prefs.GetOne(preferenceName).ValueString;

		/// <summary>
		/// Gets culture info from DB if possible, if not returns current culture.
		/// </summary>
		public static CultureInfo GetLanguageAndRegion()
		{
			CultureInfo cultureInfo = CultureInfo.CurrentCulture;

			ODException.SwallowAnyException(() =>
			{
				var preference = Prefs.GetOne("LanguageAndRegion");

				if (!string.IsNullOrEmpty(preference?.ValueString))
				{
					cultureInfo = CultureInfo.GetCultureInfo(preference.ValueString);
				}
			});

			return cultureInfo;
		}

		/// <summary>
		/// Returns true if either the XCharge program or PayConnect program is enabled and at least one clinic has online payments enabled.
		/// progEnabledForPayments will return the program that is enabled for online payments if it is allowed.
		/// Both programs cannot be enabled at the same time
		/// </summary>
		public static bool HasOnlinePaymentEnabled(out ProgramName progEnabledForPayments)
		{
			progEnabledForPayments = ProgramName.None;

			Program progXCharge = Programs.GetCur(ProgramName.Xcharge);
			Program progPayConnect = Programs.GetCur(ProgramName.PayConnect);

			if (progXCharge.Enabled)
			{
				var programProperties = ProgramProperties.GetForProgram(progXCharge.Id);
				if (programProperties.Any(x => x.Name == "IsOnlinePaymentsEnabled" && x.Value == "1"))
				{
					progEnabledForPayments = ProgramName.Xcharge;
					return true;
				}
			}

			if (progPayConnect.Enabled)
			{
				var programProperties = ProgramProperties.GetForProgram(progPayConnect.Id);
				if (programProperties.Any(x => x.Name == PayConnect.ProgramProperties.PatientPortalPaymentsEnabled && x.Value == "1"))
				{
					progEnabledForPayments = ProgramName.PayConnect;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Used by an outside developer.
		/// </summary>
		public static bool ContainsKey(string preferenceName) 
			=> Prefs.GetContainsKey(preferenceName);

		/// <summary>
		/// Used by an outside developer.
		/// </summary>
		public static bool HListIsNull() 
			=> Prefs.DictIsNull();

        /// <summary>
		/// Static variable used to always reflect FormOpenDental.IsTreatPlanSortByTooth.  
		/// This setter should only be called in FormOpenDental.IsTreatPlanSortByTooth.  
		/// This getter should only be called from the Client side when used with MiddleTier.
		/// </summary>
        public static bool IsTreatPlanSortByTooth { get; set; }

		/// <summary>
		/// Returns the path to the temporary opendental directory, temp/opendental. 
		/// Also performs one-time cleanup, if necessary. 
		/// In FormOpenDental_FormClosing, the contents of temp/opendental get cleaned up.
		/// </summary>
		[Obsolete("The fuck? Why is there file IO here...")]
		public static string GetTempFolderPath()
		{
			// Will clean up entire temp folder for a month after the enhancement of temp file cleanups as long as the temp\opendental folder doesn't already exist.
			string tempPath = ODFileUtils.CombinePaths(Path.GetTempPath(), "opendental");
			if (Directory.Exists(tempPath))
			{
				return tempPath;
			}

			Directory.CreateDirectory(tempPath);
			if (DateTime.Today > GetDate(PrefName.TempFolderDateFirstCleaned).AddMonths(1))
			{
				return tempPath;
			}

			// This might be used if this is the first time running this version on the computer that did the db update.
			// This might also be used if this is a computer that was turned off for a few weeks around the time of update conversion.
			// We need some sort of time limit just in case it's annoying and keeps happening.
			// So this will have a small risk of missing a computer, but the benefit of limiting outweighs the risk.
			// Empty entire temp folder.  Blank folders will be left behind because they do not matter.
			var fileNames = Directory.GetFiles(Path.GetTempPath());
			foreach (var fileName in fileNames)
            {
                try
                {
					var ext = Path.GetExtension(fileName).ToLower();
					if (ext == ".exe" || ext == ".cs")
					{
						continue;
					}

					File.Delete(fileName);
                }
                catch
                {
                }
            }

			return tempPath;
		}

		/// <summary>
		/// Creates a new randomly named file in the given directory path with the given extension and returns the full path to the new file.
		/// </summary>
		[Obsolete("The fuck? Why is there file IO here...")]
		public static string GetRandomTempFile(string ext) 
			=> ODFileUtils.CreateRandomFile(GetTempFolderPath(), ext);

		public static long GetDefaultSheetDefNum(SheetTypeEnum sheetType) 
			=> GetLong(Prefs.GetSheetDefPref(sheetType));

		/// <summary>
		/// Returns true if the office has a report server set up.
		/// </summary>
		public static bool HasReportServer => !string.IsNullOrEmpty(ReportingServer.Server);

		/// <summary>
		/// Returns the value (in minutes) of how long to wait prior to automatically logging the user off.
		/// Runs a query if SecurityLogOffAllowUserOverride is true in order to get the log off time override for the current user.
		/// Throws an exception if an invalid user override is found or an invalid global value is found (SecurityLogOffAfterMinutes).
		/// </summary>
		public static int LogOffTimer
		{
			get
			{
				if (GetBool(PrefName.SecurityLogOffAllowUserOverride))
				{
					UserOdPref userOverride = UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.Id, UserOdFkeyType.LogOffTimerOverride).FirstOrDefault();
					if (userOverride != null)
					{
						if (!int.TryParse(userOverride.ValueString, out int logOffMins))
						{
							throw new ODException($"Invalid LogOffTimerOverride set for user.\r\n"
								+ $"UserNum: {Security.CurUser.Id}\r\n"
								+ $"ValueString: {userOverride.ValueString}");
						}
						return logOffMins;
					}
				}
				return GetInt(PrefName.SecurityLogOffAfterMinutes);
			}
		}

		/// <summary>
		/// A helper class to get Reporting Server preferences.
		/// </summary>
		public static class ReportingServer
		{
			public static string DisplayStr
			{
				get
				{
					var server = Server;

					if (!string.IsNullOrEmpty(server))
					{
						return server + ": " + Database;
					}

					return "";
				}
			}

			public static string Server 
				=> GetString(PrefName.ReportingServerCompName);
				
			public static string Database 
				=> GetString(PrefName.ReportingServerDbName);
	
			public static string MySqlUser 
				=> GetString(PrefName.ReportingServerMySqlUser);

			public static string MySqlPass
			{
				get
				{
					string pass;
					CDT.Class1.Decrypt(GetString(PrefName.ReportingServerMySqlPassHash), out pass);
					return pass;
				}
			}
		}
	}
}
