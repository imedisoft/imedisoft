using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness
{
    /// <summary>
    /// This is a class for handling US postal codes for states, districts and territories. 
    /// Used primarily for validating user input.
    /// </summary>
    public class USlocales
	{
		/// <summary>
		/// A list of all US locals, including name and postal abbreviation.
		/// </summary>
		public static List<(string State, string Abbr)> ListAll = new List<(string State, string Abbr)>()
		{
			// 50 States.
			("Alaska", "AK"),
			("Alabama", "AL"),
			("Arkansas", "AR"),
			("Arizona", "AZ"),
			("California", "CA"),
			("Colorado", "CO"),
			("Connecticut", "CT"),
			("Delaware", "DE"),
			("Florida", "FL"),
			("Georgia", "GA"),
			("Hawaii", "HI"),
			("Iowa", "IA"),
			("Idaho", "ID"),
			("Illinois", "IL"),
			("Indiana", "IN"),
			("Kansas", "KS"),
			("Kentucky", "KY"),
			("Louisiana", "LA"),
			("Massachussetts", "MA"),
			("Maryland", "MD"),
			("Maine", "ME"),
			("Michigan", "MI"),
			("Minnesota", "MN"),
			("Missouri", "MO"),
			("Mississippi", "MS"),
			("Montana", "MT"),
			("North Carolina", "NC"),
			("North Dakota", "ND"),
			("Nebraska", "NE"),
			("New Hampshire", "NH"),
			("New Jersey", "NJ"),
			("New Mexico", "NM"),
			("Nevada", "NV"),
			("New York", "NY"),
			("Ohio", "OH"),
			("Oklahoma", "OK"),
			("Oregon", "OR"),
			("Pennsylvania", "PA"),
			("Rhode Island", "RI"),
			("South Carolina", "SC"),
			("South Dakota", "SD"),
			("Tennessee", "TN"),
			("Texas", "TX"),
			("Utah", "UT"),
			("Virginia", "VA"),
			("Vermont", "VT"),
			("Washington", "WA"),
			("Wisconsin", "WI"),
			("West Virginia", "WV"),
			("Wyoming", "WY"),

			// US Districts
			("District of Columbia", "DC"),

			// US territories. (see https://simple.wikipedia.org/wiki/U.S._postal_abbreviations)
			("American Samoa", "AS"),
			("Federated States of Micronesia", "FM"),
			("Guam", "GU"),
			("Marshall Islands", "MH"),
			("Northern Mariana Islands", "MP"),
			("Palau", "PW"),
			("Puerto Rico", "PR"),
			("United States Minor Outlying Islands", "UM"),
			("U.S. Virgin Islands", "VI")
		};

		/// <summary>
		/// Validates the provided postal code is in our list of locales (case insensitive).
		/// </summary>
		public static bool IsValidAbbr(string stateAbbr)
			=> ListAll.Any(x => x.State.Equals(stateAbbr, StringComparison.CurrentCultureIgnoreCase));
	}
}
