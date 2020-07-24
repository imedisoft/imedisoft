using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness
{
    public class DTP271
	{
		private static Dictionary<string, string> DTP01 = new Dictionary<string, string>
			{
				{ "102", "Issue" },
				{ "152", "Effective Date of Change" },
				{ "193", "Period Start" },
				{ "194", "Period End" },
				{ "198", "Completion" },
				{ "290", "Coordination of Benefits" },
				{ "291", "Plan" },
				{ "292", "Benefit" },
				{ "295", "Primary Care Provider" },
				{ "304", "Latest Visit or Consultation" },
				{ "307", "Eligibility" },
				{ "318", "Added" },
				{ "340", "Consolidated Omnibus Budget Reconciliation Act (COBRA) Begin" },
				{ "341", "Consolidated Omnibus Budget Reconciliation Act (COBRA) End" },
				{ "342", "Premium Paid to Date Begin" },
				{ "343", "Premium Paid to Date End" },
				{ "346", "Plan Begin" },
				{ "347", "Plan End" },
				{ "348", "Benefit Begin" },
				{ "349", "Benefit End" },
				{ "356", "Eligibility Begin" },
				{ "357", "Eligibility End" },
				{ "382", "Enrollment" },
				{ "435", "Admission" },
				{ "442", "Date of Death" },
				{ "458", "Certification" },
				{ "472", "Service" },
				{ "539", "Policy Effective" },
				{ "540", "Policy Expiration" },
				{ "636", "Date of Last Update" },
				{ "771", "Status" }
			};

		public X12Segment Segment { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DTP271"/> class.
		/// </summary>
		/// <param name="segment"></param>
		public DTP271(X12Segment segment)
		{
			Segment = segment;
		}

		public static string GetDateStr(string qualifier, string date)
		{
			if (qualifier == "D8") // Single Date
			{
				return X12Parse.ToDate(date).ToShortDateString();
			}

			var i = date.IndexOf('-');

			// TODO: What if there is no delimeter?

			return
				X12Parse.ToDate(date.Substring(0, i)).ToShortDateString() + "-" + 
				X12Parse.ToDate(date.Substring(i + 1)).ToShortDateString();
		}

		public static string GetQualifierDescription(string code)
		{
			if (DTP01.TryGetValue(code, out var description))
            {
				return description;
            }

			return "";
		}
	}
}
