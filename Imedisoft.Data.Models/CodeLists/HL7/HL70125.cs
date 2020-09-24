using System.Collections.Generic;

namespace Imedisoft.Data.Models.CodeLists.HL7
{
    /// <summary>
    ///		<para>
    ///			Value Type (IIS)
    ///		</para>
    ///		<para>
    ///			Corresponds to HL7 table <b>0125</b>.
    ///		</para>
    /// </summary>
    public static class HL70125
	{
		/// <summary>
		/// Coded Entry
		/// </summary>
		public const string Coded = "CE";

		/// <summary>
		/// Date
		/// </summary>
		public const string Dated = "DT";

		public const string Numeric = "NM";

		/// <summary>
		/// String Data
		/// </summary>
		public const string Text = "ST";

		/// <summary>
		/// Time Stamp (Date & Time)
		/// </summary>
		public const string DateAndTime = "TS";

		public static IEnumerable<DataItem<string>> GetDataItems()
		{
			yield return new DataItem<string>(Coded, "Coded Entry");
			yield return new DataItem<string>(Dated, "Date");
			yield return new DataItem<string>(Numeric, "Numeric");
			yield return new DataItem<string>(Text, "String Data");
			yield return new DataItem<string>(DateAndTime, "Time Stamp (Date & Time)");
		}
	}
}
