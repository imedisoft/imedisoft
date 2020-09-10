using System.Collections.Generic;

namespace Imedisoft.Data.Models
{
    /// <summary>
    ///		<para>
    ///			This describes the type of product and intolerance suffered by the patient.
    ///		</para>
    /// </summary>
    /// <seealso href="https://phinvads.cdc.gov/vads/ViewValueSet.action?id=7AFDBFB5-A277-DE11-9B52-0015173D1785"/>
    public static class SnomedAllergyCode
	{
		public const string AllergyToSubstance = "419199007";
		public const string DrugAllergy = "416098002";
		public const string DrugIntolerance = "59037007";
		public const string FoodAllergy = "414285001";
		public const string FoodIntolerance = "235719002";
		public const string AdverseReactions = "420134006";
		public const string AdverseReactionsToDrug = "419511003";
		public const string AdverseReactionsToFood = "418471000";
		public const string AdverseReactionsToSubstance = "418038007";

		/// <summary>
		/// Determines whether the specified SNOMED-CT code represents a allergy.
		/// </summary>
		/// <param name="snomedCode">The SNOMED-CT code.</param>
		/// <returns>True if the code represents an allergy; otherwise, false.</returns>
		public static bool IsAllergyCode(string snomedCode)
		{
			return
				snomedCode == AllergyToSubstance ||
				snomedCode == DrugAllergy ||
				snomedCode == DrugIntolerance ||
				snomedCode == FoodAllergy ||
				snomedCode == FoodIntolerance ||
				snomedCode == AdverseReactions ||
				snomedCode == AdverseReactionsToDrug ||
				snomedCode == AdverseReactionsToFood ||
				snomedCode == AdverseReactionsToSubstance;
		}

		/// <summary>
		/// Enumerates all codes in this collection as data items.
		/// </summary>
		public static IEnumerable<DataItem<string>> EnumerateDataItems()
        {
			yield return new DataItem<string>(AllergyToSubstance, AllergyToSubstance + " - allergy to substance");
			yield return new DataItem<string>(DrugAllergy, DrugAllergy + " - drug allergy");
			yield return new DataItem<string>(DrugIntolerance, DrugIntolerance + " - drug intolerance");
			yield return new DataItem<string>(FoodAllergy, FoodAllergy + " - food allergy");
			yield return new DataItem<string>(FoodIntolerance, FoodIntolerance + " - food intolerance");
			yield return new DataItem<string>(AdverseReactions, AdverseReactions + " - propensity to adverse reactions");
			yield return new DataItem<string>(AdverseReactionsToDrug, AdverseReactionsToDrug + " - propensity to adverse reactions to drug");
			yield return new DataItem<string>(AdverseReactionsToFood, AdverseReactionsToFood + " - propensity to adverse reactions to food");
			yield return new DataItem<string>(AdverseReactionsToSubstance, AdverseReactionsToSubstance + " - propensity to adverse reactions to substance");
		}

		/// <summary>
		///		<para>
		///			Returns the preferred concept name of the given SNOMED-CT code.
		///		</para>
		///		<para>
		///			Returns 'none' if the given code is not a allergy code.
		///		</para>
		/// </summary>
		/// <param name="snomedCode">The SNOMED-CT code.</param>
		/// <returns>The preferred concept name of the specified code.</returns>
		public static string GetPreferredConceptName(string snomedCode)
		{
            return snomedCode switch
            {
                AllergyToSubstance => "allergy to substance",
                DrugAllergy => "drug allergy",
                DrugIntolerance => "drug intolerance",
                FoodAllergy => "food allergy",
                FoodIntolerance => "food intolerance",
                AdverseReactions => "propensity to adverse reactions",
                AdverseReactionsToDrug => "propensity to adverse reactions to drug",
                AdverseReactionsToFood => "propensity to adverse reactions to food",
                AdverseReactionsToSubstance => "propensity to adverse reactions to substance",
                _ => "none",
            };
        }

		/// <summary>
		///		<para>
		///			Gets a description of the given SNOMED-CT code. The description is a 
		///			combination of the code + its preferred concept name.
		///		</para>
		///		<para>
		///			Returns 'Error' if the specified code is not a valid allergy code.
		///		</para>
		/// </summary>
		/// <param name="snomedCode">The SNOMED-CT code.</param>
		/// <returns>A description of the specified SNOMED-CT code.</returns>
		public static string GetDescription(string snomedCode)
		{
			if (string.IsNullOrEmpty(snomedCode)) return "";

			if (IsAllergyCode(snomedCode))
			{
				return snomedCode + " - " + GetPreferredConceptName(snomedCode);
			}

			return "Error";
		}
	}
}
