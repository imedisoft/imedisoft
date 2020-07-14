using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public class Shared
	{
		/// <summary>
		/// Converts numbers to ordinals. For example, 120 to 120th, 73 to 73rd.
		/// Probably doesn't work too well with foreign language translations.
		/// Used in the Birthday postcards.
		/// </summary>
		public static string NumberToOrdinal(int number)
		{
			if (number == 11)
			{
				return "11th";
			}
			if (number == 12)
			{
				return "12th";
			}
			if (number == 13)
			{
				return "13th";
			}
			string str = number.ToString();
			string last = str.Substring(str.Length - 1);
			switch (last)
			{
				case "0":
				case "4":
				case "5":
				case "6":
				case "7":
				case "8":
				case "9":
					return str + "th";
				case "1":
					return str + "st";
				case "2":
					return str + "nd";
				case "3":
					return str + "rd";
			}
			return ""; // Will never happen
		}

		/// <summary>
		/// Returns false if the backup, repair, or the optimze failed.
		/// Set isSilent to true to suppress the failure message boxes.
		/// However, progress windows will always be shown.
		/// </summary>
		public static bool BackupRepairAndOptimize(bool isSilent, BackupLocation backupLocation, bool isSecurityLogged = true)
		{
			if (!MakeABackup(isSilent, backupLocation, isSecurityLogged))
			{
				return false;
			}
			try
			{
				ODProgress.ShowAction(() => DatabaseMaintenances.RepairAndOptimize(),
					eventType: typeof(MiscDataEvent),
					odEventType: ODEventType.MiscData);
			}
			catch (Exception ex)
			{//MiscData.MakeABackup() could have thrown an exception.
			 //Show the user that something what went wrong when not in silent mode.
				if (!isSilent)
				{
					if (ex.Message != "")
					{
						MessageBox.Show(ex.Message);
					}
					MsgBox.Show("Optimize and Repair failed.");
				}
				return false;
			}
			return true;
		}

		///<summary>This is a wrapper method for MiscData.MakeABackup() that will show a progress window so that the user can see progress.
		///Returns false if making a backup failed.</summary>
		public static bool MakeABackup(BackupLocation backupLocation)
		{
			return MakeABackup(false, backupLocation);
		}

		///<summary>This is a wrapper method for MiscData.MakeABackup() that will show a progress window so that the user can see progress.
		///Set isSilent to true to suppress the failure message boxes.  However, the progress window will always be shown.
		///Returns false if making a backup failed.</summary>
		public static bool MakeABackup(bool isSilent, BackupLocation backupLocation, bool isSecurityLogged = true)
		{
#if DEBUG
			switch (MessageBox.Show("Would you like to make a backup of the DB?", "DEBUG ONLY", MessageBoxButtons.YesNoCancel))
			{
				case DialogResult.Cancel:
					return false;
				case DialogResult.No:
					return true;
				case DialogResult.Yes:
				default:
					//do nothing, make backup like usual.
					break;
			}
#endif
			//Create a thread that will show a window and then stay open until the closing action is called.
			try
			{
				ODProgress.ShowAction(() => MiscData.MakeABackup(),
					eventType: typeof(MiscDataEvent),
					odEventType: ODEventType.MiscData);
			}
			catch (Exception ex)
			{//MiscData.MakeABackup() could have thrown an exception.
			 //Show the user that something what went wrong when not in silent mode.
				if (!isSilent)
				{
					if (ex.Message != "")
					{
						MessageBox.Show(ex.Message);
					}
					//Reusing translation in ClassConvertDatabase, since it is most likely the only place a translation would have been performed previously.
					MsgBox.Show("Backup failed. Your database has not been altered.");
				}
				return false;
			}
			if (isSecurityLogged && PrefC.GetStringNoCache(PrefName.UpdateStreamLinePassword) != "abracadabra")
			{
				SecurityLogs.MakeLogEntryNoCache(Permissions.Backup, 0, Lan.G("Backups", "A backup was created when running the") + " " + backupLocation.ToString());
			}
			return true;
		}
	}

	/// <summary>
	/// Handles a global event to keep local data synchronized.
	/// </summary>
	public class DataValid
	{
		/// <summary>
		/// Triggers an event that causes a signal to be sent to all other computers telling them what kind of locally stored data needs to be updated.
		/// Either supply a set of flags for the types, or supply a date if the appointment screen needs to be refreshed.
		/// Yes, this does immediately refresh the local data, too.
		/// The AllLocal override does all types except appointment date for the local computer only, such as when starting up.
		/// </summary>
		public static void SetInvalid(params InvalidType[] invalidTypes)
			=> FormOpenDental.S_DataValid_BecomeInvalid(
				new ValidEventArgs(DateTime.MinValue, invalidTypes, false, 0));

		/// <summary>
		/// Triggers an event that causes a signal to be sent to all other computers telling them what kind of locally stored data needs to be updated.
		/// Either supply a set of flags for the types, or supply a date if the appointment screen needs to be refreshed.
		/// Yes, this does immediately refresh the local data, too.
		/// The AllLocal override does all types except appointment date for the local computer only, such as when starting up.
		/// </summary>
		public static void SetInvalid()
			=> FormOpenDental.S_DataValid_BecomeInvalid(
				new ValidEventArgs(DateTime.MinValue, new[] { InvalidType.AllLocal }, true, 0));
	}

	public delegate void ValidEventHandler(ValidEventArgs e);

	public class ValidEventArgs : EventArgs
	{
		public ValidEventArgs(DateTime dateViewing, InvalidType[] itypes, bool onlyLocal, long taskNum)
		{
			DateViewing = dateViewing;
			ITypes = itypes;
			OnlyLocal = onlyLocal;
			TaskNum = taskNum;
		}

		public DateTime DateViewing { get; }

		public InvalidType[] ITypes { get; }

		public bool OnlyLocal { get; }

		public long TaskNum { get; }
	}

	/// <summary>
	/// Used to trigger a global event to jump between modules and perform actions in other modules.
	/// PatNum is optional. If 0, then no effect.
	/// </summary>
	public class GotoModule
	{
		/// <summary>
		/// Goes directly to an existing appointment.
		/// </summary>
		public static void GotoAppointment(DateTime dateSelected, long selectedAptNum)
			=> OnModuleSelected(new ModuleEventArgs(dateSelected, new List<long>(), selectedAptNum, 0, 0, 0, 0));

		/// <summary>
		/// Goes directly to a claim in someone's Account.
		/// </summary>
		public static void GotoClaim(long claimNum)
			=> OnModuleSelected(new ModuleEventArgs(DateTime.MinValue, new List<long>(), 0, EnumModuleType.Account, claimNum, 0, 0));

		/// <summary>
		/// Goes directly to an Account. 
		/// Sometimes, patient is selected some other way instead of being passed in here, so OK to pass in a patNum of zero.
		/// </summary>
		public static void GotoAccount(long patNum)
			=> OnModuleSelected(new ModuleEventArgs(DateTime.MinValue, new List<long>(), 0, EnumModuleType.Account, 0, patNum, 0));

		/// <summary>
		/// Goes directly to Family module.
		/// Sometimes, patient is selected some other way instead of being passed in here, so OK to pass in a patNum of zero.
		/// </summary>
		public static void GotoFamily(long patNum)
			=> OnModuleSelected(new ModuleEventArgs(DateTime.MinValue, new List<long>(), 0, EnumModuleType.Family, 0, patNum, 0));

		/// <summary>
		/// Goes directly to TP module. 
		/// Sometimes, patient is selected some other way instead of being passed in here, so OK to pass in a patNum of zero.
		/// </summary>
		public static void GotoTreatmentPlan(long patNum)
			=> OnModuleSelected(new ModuleEventArgs(DateTime.MinValue, new List<long>(), 0, EnumModuleType.TreatPlan, 0, patNum, 0));

		public static void GotoChart(long patNum)
			=> OnModuleSelected(new ModuleEventArgs(DateTime.MinValue, new List<long>(), 0, EnumModuleType.Chart, 0, patNum, 0));

		public static void GotoManage(long patNum)
			=> OnModuleSelected(new ModuleEventArgs(DateTime.MinValue, new List<long>(), 0, EnumModuleType.Manage, 0, patNum, 0));

		/// <summary>
		/// Puts appointments on pinboard, then jumps to Appointments module with today's date.
		/// Sometimes, patient is selected some other way instead of being passed in here, so OK to pass in a patNum of zero.
		/// </summary>
		public static void PinToAppt(List<long> pinAptNums, long patNum)
			=> OnModuleSelected(new ModuleEventArgs(DateTime.Today, pinAptNums, 0, EnumModuleType.Appointments, 0, patNum, 0));

		/// <summary>
		/// Jumps to Images module and pulls up the specified image.
		/// </summary>
		public static void GotoImage(long patNum, long docNum)
			=> OnModuleSelected(new ModuleEventArgs(DateTime.MinValue, new List<long>(), 0, EnumModuleType.Images, 0, patNum, docNum));

		protected static void OnModuleSelected(ModuleEventArgs e)
			=> FormOpenDental.S_GotoModule_ModuleSelected(e);
	}

	public class ModuleEventArgs : EventArgs
	{
        public ModuleEventArgs(DateTime dateSelected, List<long> listPinApptNums, long selectedAptNum, EnumModuleType moduleType, long claimNum, long patNum, long docNum)
		{
			DateSelected = dateSelected;
			ListPinApptNums = listPinApptNums;
			SelectedAptNum = selectedAptNum;
			ModuleType = moduleType;
			ClaimNum = claimNum;
			PatNum = patNum;
			DocNum = docNum;
		}

        /// <summary>
		/// If going to the ApptModule, this lets you pick a date.
		/// </summary>
        public DateTime DateSelected { get; }

        /// <summary>
		/// The aptNums of the appointments that we want to put on the pinboard of the Apt Module.
		/// </summary>
        public List<long> ListPinApptNums { get; }

        public long SelectedAptNum { get; }

        public EnumModuleType ModuleType { get; }

        /// <summary>
		/// If going to Account module, this lets you pick a claim.
		/// </summary>
        public long ClaimNum { get; }

        public long PatNum { get; }

        /// <summary>
		/// If going to Images module, this lets you pick which image.
		/// </summary>
        public long DocNum { get; }
    }

	///<summary>Used to log where a backup was initiated from.
	///These enum values are named in a way so that they sound good at the end of this sentence:
	///"A backup was created when running the [enumValHere]"
	///</summary>
	public enum BackupLocation
	{
		ConvertScript,
		DatabaseMaintenanceTool,
		OptimizeTool,
		InnoDbTool
	}

	/// <summary>
	/// Displays any error messages in a MessageBox.
	/// </summary>
	public class ShowErrors : Logger.IWriteLine
	{
		private readonly Control parent;

		public ShowErrors()
		{
		}

		/// <summary>
		/// Use this constructor to make sure that the Cursor is always Default when the MessageBox is shown.
		/// </summary>
		public ShowErrors(Control parent)
		{
			this.parent = parent;
		}

		/// <summary>
		/// Shows all Errors in a message box. BeginInvokes over to the main thread if necessary.
		/// </summary>
		public void WriteLine(string data, LogLevel logLevel, string subDirectory = "")
		{
			if (logLevel != LogLevel.Error) return;

			if (parent != null && parent.InvokeRequired)
			{
				parent.BeginInvoke(() => WriteLine(data, logLevel, subDirectory));
			}
            else
            {
				ODMessageBox.Show(data);
			}
		}
	}
}
