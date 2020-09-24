using System.Collections.Generic;

namespace Imedisoft.Data.Models.CodeLists.HL7
{
	/// <summary>
	///		<para>
	///			Immunization Information Source.
	///		</para>
	///		<para>
	///			Corresponds to HL7 value set <b>NIP001</b>.
	///		</para>
	///		<para>
	///			Exported in <b>HL7 RXA-9</b>.
	///		</para>
	/// </summary>
	public static class NIP001
    {
		public const string NewRecord = "00";
		public const string HistoricalSourceUnspecified = "01";
		public const string HistoricalFromOtherProvider = "02";
		public const string HistoricalFromParentsWrittenRecord = "03";
		public const string HistoricalFromParentsRecall = "04";
		public const string HistoricalFromOtherRegistry = "05";
		public const string HistoricalFromBirthCertificate = "06";
		public const string HistoricalFromSchoolRecord = "07";
		public const string HistoricalFromPublicAgency = "08";

		public static IEnumerable<DataItem<string>> GetDataItems()
        {
			yield return new DataItem<string>(NewRecord, "New immunization record");
			yield return new DataItem<string>(HistoricalSourceUnspecified, "Historical information - source unspecified");
			yield return new DataItem<string>(HistoricalFromOtherProvider, "Historical information - from other provider");
			yield return new DataItem<string>(HistoricalFromParentsWrittenRecord, "Historical information - from parent's written record");
			yield return new DataItem<string>(HistoricalFromParentsRecall, "Historical information - from parent's recall");
			yield return new DataItem<string>(HistoricalFromOtherRegistry, "Historical information - from other registry");
			yield return new DataItem<string>(HistoricalFromBirthCertificate, "Historical information - from birth certificate");
			yield return new DataItem<string>(HistoricalFromSchoolRecord, "Historical information - from school record");
			yield return new DataItem<string>(HistoricalFromPublicAgency, "Historical information - from public agency");
		}
	}
}
