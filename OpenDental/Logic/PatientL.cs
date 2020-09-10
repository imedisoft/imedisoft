using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Imedisoft.Data;
using OpenDentBusiness;

namespace OpenDental
{
	public class PatientL
	{
		/// <summary>
		/// Collection of Patient Names and Patnums for the last five patients. 
		/// Displayed when clicking the dropdown button.
		/// </summary>
		private static readonly List<PatientButton> lastFivePatients = new List<PatientButton>();

		/// <summary>
		/// Static variable to store the currently selected patient for updating the main title bar for the Update Time countdown.
		/// Stored so we don't have to make a database call every second.
		/// </summary>
		private static Patient selectedPatient;

		/// <summary>
		/// Gets limited patient information for the patients that are in the patient select history drop down.
		/// </summary>
		public static List<Patient> GetPatientsLimFromMenu() => Patients.GetLimForPats(lastFivePatients.Select(x => x.PatientId).ToList(), true);

		/// <summary>
		/// Removes a patient from the dropdown menu.
		/// Only used when Delete Patient is called.
		/// </summary>
		public static void RemoveFromMenu(long patientId)
		{
			lock (lastFivePatients)
			{
				lastFivePatients.RemoveAll(x => x.PatientId == patientId);
			}
		}

		/// <summary>
		/// Removes all patients from the dropdown menu and from the history.
		/// Only used when a user logs off and a different user logs on.
		/// Important for enterprise customers with clinic restrictions for users.
		/// </summary>
		public static void RemoveAllFromMenu(ContextMenu menu)
		{
			menu.MenuItems.Clear();

			lock (lastFivePatients)
			{
				lastFivePatients.Clear();
			}
		}

		/// <summary>
		/// The current patient will already be on the button.
		/// This adds the family members when user clicks dropdown arrow. Can handle null values for pat and fam.
		/// Need to supply the menu to fill as well as the EventHandler to set for each item (all the same).
		/// </summary>
		public static void AddFamilyToMenu(ContextMenu menu, EventHandler onClick, long patientId, Family family)
		{
			menu.MenuItems.Clear();
			if (lastFivePatients.Count == 0 && patientId == 0)
			{
				return; // Without this the Select Patient dropdown would only have a bar and FAMILY.
			}

			for (int i = 0; i < lastFivePatients.Count; i++)
			{
				menu.MenuItems.Add(lastFivePatients[i].Name, onClick);
			}

			menu.MenuItems.Add("-");
			menu.MenuItems.Add("FAMILY");

			if (patientId != 0 && family != null)
			{
				for (int i = 0; i < family.ListPats.Length; i++)
				{
					menu.MenuItems.Add(family.ListPats[i].GetNameLF(), onClick);
				}
			}
		}

		/// <summary>
		/// Does not handle null values. Use zero.
		/// Does not handle adding family members.
		/// Returns true if patient has changed.
		/// </summary>
		public static bool AddPatientToMenu(string patientName, long patientId)
		{
			if (patientId == 0) return false;

			if (lastFivePatients.Count > 0 && patientId == lastFivePatients[0].PatientId)
			{
				return false;
			}

			lock (lastFivePatients)
			{
				int index = lastFivePatients.FindIndex(x => x.PatientId == patientId);
				if (index > -1)
				{
					lastFivePatients.RemoveAt(index);
				}

				lastFivePatients.Insert(0, new PatientButton(patientId, patientName ?? ""));
				if (lastFivePatients.Count > 5)
				{
					lastFivePatients.RemoveAt(5);
				}
			}

			return true;
		}

		/// <summary>
		/// Determines which menu Item was selected from the Patient dropdown list and returns the patNum for that patient.
		/// This will not be activated when click on 'FAMILY' or on separator, because they do not have events attached.
		/// Calling class then does a ModuleSelected.
		/// </summary>
		public static long ButtonSelect(ContextMenu menu, object sender, Family fam)
		{
			int index = menu.MenuItems.IndexOf((MenuItem)sender);

			if (index < lastFivePatients.Count)
			{
				return lastFivePatients[index].PatientId;
			}

			if (fam == null) return 0; // Will never happen

			return fam.ListPats[index - lastFivePatients.Count - 2].PatNum;
		}

		/// <summary>
		/// Returns a string representation of the current state of the application designed for display in the main title.
		/// Accepts null for pat and 0 for clinicNum.
		/// </summary>
		public static string GetMainTitle(Patient patient, long clinicNum)
		{
			string result = Preferences.GetString(PreferenceName.MainWindowTitle);

			object[] parameters = { result };
			Plugins.HookAddCode(null, "PatientL.GetMainTitle_beginning", parameters);
			result = (string)parameters[0];
			selectedPatient = patient;

			if (PrefC.HasClinicsEnabled && clinicNum > 0)
			{
				if (result != "")
				{
					result += " - Clinic: ";
				}

				if (Preferences.GetBool(PreferenceName.TitleBarClinicUseAbbr))
				{
					result += Clinics.GetAbbr(clinicNum);
				}
				else
				{
					result += Clinics.GetDescription(clinicNum);
				}
			}

			if (Security.CurrentUser != null)
			{
				result += " {" + Security.CurrentUser.UserName + "}";
			}

			if (patient == null || patient.PatNum == 0 || patient.PatNum == -1)
			{
				return result;
			}

			result += " - " + patient.GetNameLF();
			if (Preferences.GetBool(PreferenceName.TitleBarShowSpecialty))
			{
				result += string.IsNullOrWhiteSpace(patient.Specialty) ? "" : " (" + patient.Specialty + ")";
			}

			if (Preferences.GetLong(PreferenceName.ShowIDinTitleBar) == 1)
			{
				result += " - " + patient.PatNum.ToString();
			}
			else if (Preferences.GetLong(PreferenceName.ShowIDinTitleBar) == 2)
			{
				result += " - " + patient.ChartNumber;
			}
			else if (Preferences.GetLong(PreferenceName.ShowIDinTitleBar) == 3)
			{
				if (patient.Birthdate.Year > 1880)
				{
					result += " - " + patient.Birthdate.ToShortDateString();
				}
			}

			if (patient.SiteNum != 0)
			{
				result += " - " + Sites.GetDescription(patient.SiteNum);
			}

			return result;
		}

		/// <summary>
		/// Sets the cached patient specialty to null so that the main title will refresh the specialty from the database if there is a valid patient selected.
		/// </summary>
		public static void InvalidateSelectedPatSpecialty()
		{
			if (selectedPatient == null)
			{
				return;
			}

			selectedPatient.Specialty = null;
		}

		/// <summary>
		/// Internal class of Patient.PatNum and Patient First & Last Names (nameFL) used for presentation on Patient Select button.
		/// </summary>
		private class PatientButton
		{
			public long PatientId { get; }

			public string Name { get; }

			public PatientButton(long patientId, string patientName)
			{
				PatientId = patientId;
				Name = patientName;
			}
		}
	}
}
