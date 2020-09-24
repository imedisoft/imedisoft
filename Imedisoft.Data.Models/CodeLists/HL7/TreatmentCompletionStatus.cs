using System.Collections.Generic;

namespace Imedisoft.Data.Models.CodeLists.HL7
{
    /// <summary>
    ///		<para>
    ///			Treatment Completion Status (HL7)
    ///		</para>
    ///		<para>
    ///			Corresponds to HL7 table 0322.
    ///		</para>
    /// </summary>
    public static class TreatmentCompletionStatus
	{
		public const string Complete = "CP";
		public const string Refused = "RE";
		public const string NotAdministered = "NA";
		public const string PartiallyAdministered = "PA";

        public static IEnumerable<DataItem<string>> GetDataItems()
        {
            yield return new DataItem<string>(Complete, "Complete");
            yield return new DataItem<string>(Refused, "Refused");
            yield return new DataItem<string>(NotAdministered, "Not Administered");
            yield return new DataItem<string>(PartiallyAdministered, "Partially Administered");
        }
	}
}
