using System.Collections.Generic;

namespace Imedisoft.Data.Models.CodeLists.HL7
{
    /// <summary>
    ///		<para>
    ///			Substance Refusal Reason (IIS)
    ///		</para>
    ///		<para>
    ///			Corresponds to CDC code set <b>NIP001</b>.
    ///		</para>
    /// </summary>
    public static class SubstanceRefusalReason
	{
		public const string ParentalDecision = "00";
		public const string ReligiousExemption = "01";
		public const string Other = "02";
		public const string PatientDecision = "03";

        public static IEnumerable<DataItem<string>> GetDataItems()
        {
            yield return new DataItem<string>(ParentalDecision, "Parental Decision");
            yield return new DataItem<string>(ReligiousExemption, "Religious Exemption");
            yield return new DataItem<string>(Other, "Other");
            yield return new DataItem<string>(PatientDecision, "Patient Decision");
        }
	}
}
