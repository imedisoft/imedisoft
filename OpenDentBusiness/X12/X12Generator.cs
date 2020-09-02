using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenDentBusiness
{
    public class X12Generator
	{
		/// <summary>
		/// If clearinghouseClin.SenderTIN is blank, then 810624427 will be used to indicate Open Dental.
		/// </summary>
		public static string GetISA06(Clearinghouse clearinghouseClinic)
		{
			if (string.IsNullOrEmpty(clearinghouseClinic.SenderTIN))
			{
				return TidyString("810624427", 15, 15); // TIN of OD.
			}

			return TidyString(clearinghouseClinic.SenderTIN, 15, 15);
		}

		/// <summary>
		/// Sometimes SenderTIN, sometimes OD's TIN.
		/// </summary>
		public static string GetGS02(Clearinghouse clearinghouseClinic)
		{
			if (string.IsNullOrEmpty(clearinghouseClinic.SenderTIN))
			{
				return TidyString("810624427", 15, 2);
			}

			return TidyString(clearinghouseClinic.SenderTIN, 15, 2);
		}

		/// <summary>
		/// Returns the Provider Taxonomy code for the given specialty. Always 10 characters, validated.
		/// </summary>
		public static string GetTaxonomy(Provider provider)
		{
			if (provider.TaxonomyCodeOverride != "")
			{
				return provider.TaxonomyCodeOverride;
			}

			var providerSpecialization = Definitions.GetDef(DefinitionCategory.ProviderSpecialties, provider.Specialty);
			if (providerSpecialization == null || string.IsNullOrEmpty(providerSpecialization.Taxonomy))
            {
				return "1223G0001X"; // General
			}

			return providerSpecialization.Taxonomy;
		}

		/// <summary>
		/// Converts any string to an acceptable format for X12.
		/// Converts to all caps and strips off all invalid characters.
		/// Optionally shortens the string to the specified length and/or makes sure the string is long enough by padding with spaces.
		/// </summary>
		public static string TidyString(string str, int maxLength = -1, int minLength = 0)
		{
			string result = str.ToUpper();

			result = Regex.Replace(result, //replaces characters in this input string
										   //Allowed: !"&'()+,-./;?=(space)#   # is actually part of extended character set
				"[^\\w!\"&'\\(\\)\\+,-\\./;\\?= #]",//[](any single char)^(that is not)\w(A-Z or 0-9) or one of the above chars.
				"");

			result = Regex.Replace(result, "[_]", ""); // replaces _
			if (maxLength != -1)
			{
				if (result.Length > maxLength)
				{
					result = result.Substring(0, maxLength);
				}
			}

			if (minLength > 0)
			{
				if (result.Length < minLength)
				{
					result = result.PadRight(minLength, ' ');
				}
			}

			return result;
		}
	}
}
