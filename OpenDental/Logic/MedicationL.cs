using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenDental
{
    public class MedicationL
	{
		/// <summary>
		/// Inserts any new medications in listNewMeds, as well as updating any existing medications in listExistingMeds in conflict with the corresponding new medication.
		/// </summary>
		public static int ImportMedications(List<(Medication, string)> importedMedications, List<Medication> existingMedications)
		{
			int importedMedication = 0;

			foreach ((Medication, string) newMedication in importedMedications)
			{
				if (IsDuplicateMedication(newMedication, existingMedications))
				{
					continue;
				}

				InsertNewMedication(newMedication, existingMedications);

				importedMedication++;
			}

			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Imported " + SOut.Int(importedMedication) + " medications.");

			return importedMedication;
		}

		/// <summary>
		/// Determines if med is a duplicate of another Medication in listMedsExisting.
		/// Given medGenNamePair is a medication that we are checking and the given generic name if set.
		/// A duplicate is defined as MedName is equal, GenericName is equal, RxCui is equal and either Notes is equal or not defined.
		/// A new medication with all properties being equal to an existing medication except with a blank Notes property is considered to be a duplicate, as it is likely the existing Medication is simply a user edited version of the same Medication.
		/// </summary>
		private static bool IsDuplicateMedication((Medication, string) medGenNamePair, List<Medication> existingMedications)
		{
			Medication med = medGenNamePair.Item1;
			string genericName = medGenNamePair.Item2;
			bool isNoteChecked = true;

			//If everything is identical, except med.Notes is blank while x.Notes is not blank, we consider this to be a duplicate.
			if (string.IsNullOrEmpty(med.Notes))
			{
				isNoteChecked = false;
			}

			return existingMedications.Any(
				x => x.Name.Trim().ToLower() == med.Name.Trim().ToLower() && 
				Medications.GetGenericName(x.GenericId??0).Trim().ToLower() == genericName.Trim().ToLower() && 
				x.RxCui == med.RxCui && 
				(!isNoteChecked || (x.Notes.Trim().ToLower() == med.Notes.Trim().ToLower())));
		}

		/// <summary>
		/// Inserts the given medNew.
		/// Given medGennamePair is a medication that we are checking and the given generic name if set.
		/// ListMedsExisting is used to identify the GenericNum for medNew.
		/// </summary>
		private static void InsertNewMedication((Medication Medication, string GenericName) medicationInfo, List<Medication> existingMedications)
		{
			var newMedication = medicationInfo.Medication;

			// Try to find a match for the generic medication.
			long genericMedicationNum = existingMedications.FirstOrDefault(x => x.Name == medicationInfo.GenericName)?.Id ?? 0;
			if (genericMedicationNum != 0)
			{
				newMedication.GenericId = genericMedicationNum;
			}

			Medications.Insert(newMedication);

			// Found no match initially, assume given medication is the generic.
			if (genericMedicationNum == 0)
			{
				newMedication.GenericId = newMedication.Id;

				Medications.Update(newMedication);
			}

			existingMedications.Add(newMedication);
		}

		/// <summary>
		/// Exports all medications to the passed in filename.
		/// </summary>
		public static int ExportMedications(string path, List<Medication> medications)
		{
			var stringBuilder = new StringBuilder();

            static string Quote(string text) => "\"" + text.Replace("\"", "\\\"") + "\"";

			foreach (var medication in medications)
			{
				stringBuilder.AppendLine(
					Quote(medication.Name) + '\t' + 
					Quote(Medications.GetGenericName(medication.GenericId ?? 0)) + '\t' +
					Quote(medication.Notes) + '\t' + 
					Quote(medication.RxCui));
			}

			File.WriteAllText(path, stringBuilder.ToString());

			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Exported" + " " + SOut.Int(medications.Count) + " medications to: " + path);

			return medications.Count;
		}

		/// <summary>
		/// Throws exception.
		/// Reads tab delimited medication information from given filename.
		/// Returns the list of new medications with all generic medications before brand medications.
		/// File required to be formatted such that each row contain: MedName\tGenericName\tNotes\tRxCui
		/// </summary>
		public static List<(Medication, string)> GetMedicationsFromFile(string fileName, bool isTemp = false)
		{
			var newMedications = new List<(Medication, string)>();
			if (string.IsNullOrEmpty(fileName))
			{
				return newMedications;
			}

			string medicationData = File.ReadAllText(fileName);
			if (isTemp)
			{
				File.Delete(fileName);
			}

			var medicationLines = SplitLines(medicationData);
			foreach (string[] line in medicationLines)
			{
				if (line.Length != 4)
				{
					throw new ODException("Invalid formatting detected in file.");
				}

                var medication = new Medication
                {
                    Name = SIn.String(line[0]).Trim(),
                    Notes = SIn.String(line[2]).Trim(),
                    RxCui = line[3]
                };

                string genericName = SIn.String(line[1]).Trim();

				newMedications.Add((medication, genericName));
			}

			return SortGenericMedicationsFirst(newMedications);
		}

		/// <summary>
		/// Returns a list of string arrays for the provided data.
		/// Lines are determined by new line characters and tabs between fields.
		/// </summary>
		private static List<string[]> SplitLines(string data)
		{
			var results = new List<string[]>();
			if (string.IsNullOrWhiteSpace(data))
			{
				return results;
			}

			var fieldStarted = false;
			var field = "";

			var lines = data.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();

			foreach (string line in lines)
			{
				var fields = new List<string>();

				for (int i = 0; i < line.Length; i++)
				{
					char c = line[i];
					if (!fieldStarted)
					{
						if (c == '"') // Start of a new field.
						{
							fieldStarted = true;

							continue;
						}
						else if (c == '\t')
						{
							continue;
						}
						else throw new Exception("Invalid formatting in Medication file.");
					}

					if (c == '"' && (field.Length == 0 || field[field.Length - 1] != '\\'))
					{
						fieldStarted = false;

						fields.Add(field.Replace("\\\"", "\"")); //Unescape any " in the field.
						field = "";

						continue;
					}

					// Normal character inside a field.
					field += c;
				}

				results.Add(fields.ToArray());
			}

			return results;
		}

		/// <summary>
		/// Custom sorting so that generic medications are above branded medications.
		/// </summary>
		private static List<(Medication, string)> SortGenericMedicationsFirst(List<(Medication, string)> medications)
		{
			var genericMedications = new List<(Medication, string)>();
			var brandedMedications = new List<(Medication, string)>();

			foreach ((Medication Medication, string GenericName) medicationInfo in medications)
			{
				if (medicationInfo.Medication.Name.ToLower().In(medicationInfo.GenericName.ToLower(), ""))
				{
					genericMedications.Add(medicationInfo);
				}
				else
				{
					brandedMedications.Add(medicationInfo);
				}
			}

			genericMedications.AddRange(brandedMedications);

			return genericMedications;
		}
	}
}
