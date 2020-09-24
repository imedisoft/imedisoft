using System.Collections.Generic;

namespace Imedisoft.Data.Models.CodeLists.HL7
{
    /// <summary>
    ///		<para>
    ///			Route Of Administration (IIS)
    ///		</para>
    ///		<para>
    ///			Corresponds to HL7 table <b>0162</b>.
    ///		</para>
    ///	</summary>
    public static class RouteOfAdministration
	{
		public const string Intradermal = "ID";
		public const string Intramuscular = "IM";
		public const string Nasal = "NS";
		public const string Intravenous = "IV";
		public const string Oral = "PO";
		public const string Other = "OTH";
		public const string Subcutaneous = "SC";
		public const string Transdermal = "TD";

		public static IEnumerable<DataItem<string>> GetDataItems()
        {
            yield return new DataItem<string>(Intradermal, "Intradermal");
            yield return new DataItem<string>(Intramuscular, "Intramuscular");
            yield return new DataItem<string>(Nasal, "Nasal");
            yield return new DataItem<string>(Intravenous, "Intravenous");
            yield return new DataItem<string>(Oral, "Oral");
            yield return new DataItem<string>(Other, "Other");
            yield return new DataItem<string>(Subcutaneous, "Subcutaneous");
            yield return new DataItem<string>(Transdermal, "Transdermal");
        }
	}
}
