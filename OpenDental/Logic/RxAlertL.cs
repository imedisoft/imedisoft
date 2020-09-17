using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public class RxAlertL
	{
		/// <summary>
		/// Returns false if user does not wish to continue after seeing alert.
		/// </summary>
		public static bool DisplayAlerts(long patNum, long rxDefNum)
		{
            List<RxAlert> alertList = RxAlerts.Refresh(rxDefNum);
            
            List<Problem> diseases = Problems.GetByPatient(patNum).ToList();
			List<Allergy> allergies = Allergies.GetByPatient(patNum).ToList();
			List<MedicationPat> medicationPats = MedicationPats.Refresh(patNum, false);//Exclude discontinued, only active meds.
			List<string> diseaseMatches = new List<string>();
			List<string> allergiesMatches = new List<string>();
			List<string> medicationsMatches = new List<string>();
			List<string> customMessages = new List<string>();
			bool showHighSigOnly = Preferences.GetBool(PreferenceName.EhrRxAlertHighSeverity);

			for (int i = 0; i < alertList.Count; i++)
			{
				for (int j = 0; j < diseases.Count; j++)
				{
					//This does not look for matches with icd9s.
					if (alertList[i].DiseaseDefId == diseases[j].ProblemDefId && diseases[j].Status == 0)
					{//ProbStatus is active.
						if (alertList[i].NotificationMsg == "")
						{
							diseaseMatches.Add(ProblemDefinitions.GetName(diseases[j].ProblemDefId));
						}
						else
						{
							customMessages.Add(alertList[i].NotificationMsg);
						}
					}
				}

				for (int j = 0; j < allergies.Count; j++)
				{
					if (alertList[i].AllergyDefId == allergies[j].AllergyDefId && allergies[j].IsActive)
					{
						if (alertList[i].NotificationMsg == "")
						{
							allergiesMatches.Add(AllergyDefs.GetById(alertList[i].AllergyDefId).Description);
						}
						else
						{
							customMessages.Add(alertList[i].NotificationMsg);
						}
					}
				}

				for (int j = 0; j < medicationPats.Count; j++)
				{
					bool isMedInteraction = false;
					Medication medForAlert = Medications.GetById(alertList[i].MedicationId);
					if (medForAlert == null)
					{
						continue;//MedicationNum will be 0 for all other alerts that are not medication alerts.
					}
					if (medicationPats[j].MedicationNum != 0 && alertList[i].MedicationId == medicationPats[j].MedicationNum)
					{//Medication from medication list.
						isMedInteraction = true;
					}
					else if (medicationPats[j].MedicationNum == 0 && medForAlert.RxCui != "" && medicationPats[j].RxCui.ToString() == medForAlert.RxCui)
					{//Medication from NewCrop. Unfortunately, neither of these RxCuis are required.
						isMedInteraction = true;
					}
					if (!isMedInteraction)
					{
						continue;//No known interaction.
					}
					//Medication interaction.
					if (showHighSigOnly && !alertList[i].IsHighSignificance)
					{//if set to only show high significance alerts and this is not a high significance interaction, do not show alert
						continue;//Low significance alert.
					}
					if (alertList[i].NotificationMsg == "")
					{
						Medications.RefreshCache();
						medicationsMatches.Add(Medications.GetById(alertList[i].MedicationId).Name);
					}
					else
					{
						customMessages.Add(alertList[i].NotificationMsg);
					}
				}
			}

			//these matches do not include ones that have custom messages.
			if (diseaseMatches.Count > 0 || allergiesMatches.Count > 0 || medicationsMatches.Count > 0)
			{
				string alert = "";
				for (int i = 0; i < diseaseMatches.Count; i++)
				{
					if (i == 0)
					{
						alert += "This patient has the following medical problems: ";
					}
					alert += diseaseMatches[i];
					if ((i + 1) == diseaseMatches.Count)
					{
						alert += ".\r\n";
					}
					else
					{
						alert += ", ";
					}
				}
				for (int i = 0; i < allergiesMatches.Count; i++)
				{
					if (i == 0 && diseaseMatches.Count > 0)
					{
						alert += "and the following allergies: ";
					}
					else if (i == 0)
					{
						alert = "This patient has the following allergies: ";
					}
					alert += allergiesMatches[i];
					if ((i + 1) == allergiesMatches.Count)
					{
						alert += ".\r\n";
					}
					else
					{
						alert += ", ";
					}
				}
				for (int i = 0; i < medicationsMatches.Count; i++)
				{
					if (i == 0 && (diseaseMatches.Count > 0 || allergiesMatches.Count > 0))
					{
						alert += "and is taking the following medications: ";
					}
					else if (i == 0)
					{
						alert = "This patient is taking the following medications: ";
					}
					alert += medicationsMatches[i];
					if ((i + 1) == medicationsMatches.Count)
					{
						alert += ".\r\n";
					}
					else
					{
						alert += ", ";
					}
				}
				alert += "\r\n" + "Continue anyway?";
				if (MessageBox.Show(alert, "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
				{
					return false;
				}
			}
			for (int i = 0; i < customMessages.Count; i++)
			{
				if (MessageBox.Show(customMessages[i] + "\r\n" + "Continue anyway?", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
				{
					return false;
				}
			}
			return true;
		}
	}
}
