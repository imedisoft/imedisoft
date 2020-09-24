using System.Collections.Generic;

namespace Imedisoft.Data.Models.CodeLists.HL7
{
    public static class SexualOrientation
	{
		public const string Homosexual = "38628009";
		public const string Heterosexual = "20430005";
		public const string Bisexual = "42035005";
		public const string Other = "OTH";
		public const string DontKnow = "UNK";
		public const string NonDisclosure = "ASKU";

		public static IEnumerable<DataItem<string>> GetDataItems()
		{
			yield return new DataItem<string>(Homosexual, "Homosexual");
			yield return new DataItem<string>(Heterosexual, "Heterosexual");
			yield return new DataItem<string>(Bisexual, "Bisexual");
			yield return new DataItem<string>(Other, "Other");
			yield return new DataItem<string>(DontKnow, "Doesn't know");
			yield return new DataItem<string>(NonDisclosure, "Does not wish to disclose");
		}
	}
}
