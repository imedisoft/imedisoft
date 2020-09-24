using System.Collections.Generic;

namespace Imedisoft.Data.Models.CodeLists.HL7
{
    /// <summary>
    ///		<para>
    ///			Observation Identifier (IIS)
    ///		</para>
    ///		<para>
    ///			Corresponds to HL7 value set <b>NIP003</b>. This code set is a subset of LOINC codes.
    ///		</para>
    ///		<para>
    ///			Used in HL7 OBX-3.
    ///		</para>
    /// </summary>
    public static class NIP003
	{
		// TODO: Move this table to the database...

		public const string DatePublished = "29768-9";
		public const string DatePresented = "29769-7";
		public const string DatePrecautionExpiration = "30944-3";
		public const string Precaution = "30945-0";
		public const string DatePrecautionEffective = "30946-8";
		public const string TypeOf = "30956-7";
		public const string FundsPurchasedWith = "30963-3";
		public const string DoseNumber = "30973-2";
		public const string NextDue = "30979-9";
		public const string DateDue = "30980-7";
		public const string DateEarliestAdminister = "30981-5";
		public const string ReasonForcast = "30982-3";
		public const string Reaction = "31044-1";
		public const string ComponentType = "38890-0";
		public const string TakeResponseType = "46249-9";
		public const string DateTakeResponse = "46250-7";
		public const string ScheduleUsed = " 59779-9";
		public const string Series = "59780-7";
		public const string DoseValidity = "59781-5";
		public const string NumDosesPrimary = "59782-3";
		public const string StatusInSeries = "59783-1";
		public const string DiseaseWithImmunity = "59784-9";
		public const string Indication = "59785-6";
		public const string FundPgmEligCat = "64994-7";
		public const string DocumentType = "69764-9";

		public static IEnumerable<DataItem<string>> GetDataItems()
		{
			yield return new DataItem<string>(DatePublished, "Date vaccine information statement published");
			yield return new DataItem<string>(DatePresented, "Date vaccine information statement presented");
			yield return new DataItem<string>(DatePrecautionExpiration, "Date of vaccination temporary contraindication and or precaution expiration");
			yield return new DataItem<string>(Precaution, "Vaccination contraindication and or precaution");
			yield return new DataItem<string>(DatePrecautionEffective, "Date vaccination contraindication and or precaution effective");
			yield return new DataItem<string>(TypeOf, "Type");
			yield return new DataItem<string>(FundsPurchasedWith, "Funds vaccine purchased with");
			yield return new DataItem<string>(DoseNumber, "Dose number");
			yield return new DataItem<string>(NextDue, "Vaccines due next");
			yield return new DataItem<string>(DateDue, "Date vaccine due");
			yield return new DataItem<string>(DateEarliestAdminister, "Earliest date to give");
			yield return new DataItem<string>(ReasonForcast, "Reason applied by forcast logic to project this vaccine");
			yield return new DataItem<string>(Reaction, "Reaction");
			yield return new DataItem<string>(ComponentType, "Vaccine component type");
			yield return new DataItem<string>(TakeResponseType, "Vaccination take-response type");
			yield return new DataItem<string>(DateTakeResponse, "Vaccination take-response date");
			yield return new DataItem<string>(ScheduleUsed, "Immunization schedule used");
			yield return new DataItem<string>(Series, "Immunization series");
			yield return new DataItem<string>(DoseValidity, "Dose validity");
			yield return new DataItem<string>(NumDosesPrimary, "Number of doses in primary immunization series");
			yield return new DataItem<string>(StatusInSeries, "Status in immunization series");
			yield return new DataItem<string>(DiseaseWithImmunity, "Disease with presumed immunity");
			yield return new DataItem<string>(Indication, "Indication for Immunization");
			yield return new DataItem<string>(FundPgmEligCat, "Vaccine fund pgm elig cat");
			yield return new DataItem<string>(DocumentType, "Document type");
		}
	}
}
