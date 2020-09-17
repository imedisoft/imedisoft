using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class AllergyDefs
	{
		public static IEnumerable<AllergyDef> GetAll(bool includeHidden = false) 
			=> includeHidden ?
				SelectMany("SELECT * FROM `allergy_defs` ORDER BY `description`") :
				SelectMany("SELECT * FROM `allergy_defs` WHERE `is_hidden` = 0 ORDER BY `description`");

		public static AllergyDef GetById(long allergyDefinitionId) 
			=> SelectOne(allergyDefinitionId);
		
		public static AllergyDef GetById(long allergyDefinitionId, IEnumerable<AllergyDef> allergyDefinitions)
		{
			var allergyDefinition = allergyDefinitions.FirstOrDefault(x => x.Id == allergyDefinitionId);

			return allergyDefinition ?? GetById(allergyDefinitionId);
		}

		public static IEnumerable<AllergyDef> GetByIds(IEnumerable<long> allergyDefinitionIds)
		{
			var allergyDefinitionIdsList = allergyDefinitionIds.ToList();
			if (allergyDefinitionIdsList.Count > 0)
			{
				return SelectMany(
					"SELECT * FROM `allergy_defs` WHERE `id` IN (" + string.Join(", ", allergyDefinitionIdsList) + ")");
			}

			return new List<AllergyDef>();
		}

		public static AllergyDef GetByDescription(string allergyDescription) 
			=> SelectOne("SELECT * FROM `allergy_defs` WHERE `description` = @description", 
				new MySqlParameter("description", allergyDescription ?? ""));

		public static IEnumerable<AllergyDef> GetByPatient(long patientId, bool includeInactive)
		{
			var command =
				"SELECT ad.* FROM `allergy_defs` ad " +
				"INNER JOIN `allergies` a ON a.`allergy_def_id` = ad.`id` " +
				"WHERE a.`patient_id` = " + patientId;

			if (!includeInactive)
			{
				command += " AND a.`status_is_active` != 0";
			}

			return SelectMany(command);
		}

		public static AllergyDef GetByRxNorm(long rxnorm) // TODO: rxnorm -> string
		{
			if (rxnorm == 0) return null;

			return SelectOne(
				"SELECT ad.* FROM `alergy_defs` ad " +
				"INNER JOIN `medications` m ON ad.`medication_id` = m.`id` " +
				"AND m.`rx_cui` = " + rxnorm + " " +
				"WHERE ad.`snomed_code` IN ('" + SnomedAllergyCode.DrugAllergy + "', '" + SnomedAllergyCode.DrugIntolerance + "')");
		}

		public static AllergyDef GetByMedication(long medicationId) 
			=> SelectOne("SELECT * FROM `allergy_defs` WHERE `medication_id` = " + medicationId);
		
		public static long Insert(AllergyDef allergyDefinition) 
			=> ExecuteInsert(allergyDefinition);
		
		public static void Update(AllergyDef allergyDefinition) 
			=> ExecuteUpdate(allergyDefinition);

		public static void Save(AllergyDef allergyDefinition)
        {
			if (allergyDefinition.Id == 0) ExecuteInsert(allergyDefinition);
            else
            {
				ExecuteUpdate(allergyDefinition);
            }
		}

		public static void Delete(long allergyDefinitionId) 
			=> ExecuteDelete(allergyDefinitionId);

		public static bool IsInUse(long allergyDefinitionId)
		{
			var command = "SELECT COUNT(*) FROM `allergies` WHERE `allergy_def_id` = " + allergyDefinitionId;
			if (Database.ExecuteLong(command) > 0)
			{
				return true;
			}

			command = "SELECT COUNT(*) FROM rxalert WHERE AllergyDefNum=" + allergyDefinitionId;
			if (Database.ExecuteLong(command) > 0)
			{
				return true;
			}

			if (allergyDefinitionId == Preferences.GetLong(PreferenceName.AllergiesIndicateNone))
			{
				return true;
			}

			return false;
		}

		public static string GetDescription(long allergyDefinitionId)
		{
			if (allergyDefinitionId == 0)
			{
				return "";
			}

			return SelectOne(allergyDefinitionId)?.Description ?? "";
		}
	}
}
