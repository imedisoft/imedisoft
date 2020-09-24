using System.Collections.Generic;

namespace Imedisoft.Data.Models.CodeLists.HL7
{
    public static class GenderIdentity
	{
		public const string Male = "446151000124109";
		public const string Female = "446141000124107";
		public const string TransgenderMale = "407377005";
		public const string TransgenderFemale = "407376001";
		public const string NonBinary = "446131000124102";
		public const string Other = "OTH";
		public const string NonDisclosure = "ASKU";

		public static IEnumerable<DataItem<string>> GetDataItems()
		{
			yield return new DataItem<string>(Male, "Male");
			yield return new DataItem<string>(Female, "Female");
			yield return new DataItem<string>(TransgenderMale, "Transgender male");
			yield return new DataItem<string>(TransgenderFemale, "Transgender woman");
			yield return new DataItem<string>(NonBinary, "Genderqueer");
			yield return new DataItem<string>(Other, "Other");
			yield return new DataItem<string>(NonDisclosure, "Does not wish to disclose");
		}
	}
}
