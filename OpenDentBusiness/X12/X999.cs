using DataConnectionBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness
{
	public class X999 : X12object
	{
		public X999(string messageText) : base(messageText)
		{
		}

		/// <summary>
		/// In X12 lingo, the batchNumber is known as the functional group.
		/// </summary>
		public int GetBatchNumber()
		{
			if (FunctGroups[0].Transactions.Count == 1)
			{
				var segment = FunctGroups[0].Transactions[0].GetSegmentByID("AK1");
				if (segment == null)
				{
					return 0;
				}

				if (int.TryParse(segment.Get(2), out var batchNumber))
				{
					return batchNumber;
				}
			}

			return 0;
		}

		/// <summary>
		/// Do this first to get a list of all trans nums that are contained within this 999.
		/// Then, for each trans num, we can later retrieve the AckCode for that single trans num.
		/// </summary>
		public List<int> GetTransactionNumbers()
		{
			var transactionNumbers = new List<int>();

            foreach (var segment in FunctGroups[0].Transactions[0].Segments)
            {
                if (segment.SegmentID == "AK2")
                {
					if (int.TryParse(segment.Get(2), out var result) && result != 0)
                    {
						transactionNumbers.Add(result);
                    }
                }
            }

            return transactionNumbers;
		}

		/// <summary>
		/// Use after GetTransactionNumbers. Will return A=Accepted, R=Rejected, or "" if can't determine.
		/// </summary>
		public string GetAckForTransaction(int transactionNumber)
		{
			bool foundTransactionNumber = false;

            foreach (var segment in FunctGroups[0].Transactions[0].Segments)
			{
				if (foundTransactionNumber)
				{
					if (segment.SegmentID != "IK5")
					{
						continue;
					}

					return segment.Get(1) switch
					{
						// Accepted (A) or Accepted with errors (E)
						string code when code == "A" || code == "E" => "A",

						// Everything else (M, R, W or X)
						_ => "R"
					};
				}

				if (segment.SegmentID == "AK2")
				{
					if (int.TryParse(segment.Get(2), out var result) && result == transactionNumber)
                    {
						foundTransactionNumber = true;
                    }
				}
			}

			return "";
		}

		/// <summary>
		/// Will return "" if unable to determine.
		/// But would normally return A=Accepted or R=Rejected or P=Partially accepted if only some of the transactions were accepted.
		/// </summary>
		public string GetBatchAckCode()
		{
			if (FunctGroups[0].Transactions.Count != 1) return "";

			var segment = FunctGroups[0].Transactions[0].GetSegmentByID("AK9");
			if (segment == null)
			{
				return "";
			}

			return segment.Get(1) switch
			{
				// Accepted (A) or Accepted With Errors (E)
				string code when code == "A" || code == "E" => "A",

				// Partially Accepted (P)
				string code when code == "P" => "P",

				// Everything else (M, R, W or X)
				_ => "R"
			};
		}

		public string GetHumanReadable()
		{
			var stringBuilder = new StringBuilder();

			foreach (var segment in Segments)
            {
				if (segment.SegmentID != "IK3" && segment.SegmentID != "IK4")
                {
					continue;
                }

				switch (segment.SegmentID)
                {
					case "IK3":
						stringBuilder.AppendLine($"Segment {segment.Get(1)}: {GetSegmentSyntaxError(segment.Get(4))}");
						break;

					case "IK4":
						stringBuilder.AppendLine($"Segment {segment.Get(1)}: {GetElementSyntaxError(segment.Get(3))}");
						break;
                }
            }

			return stringBuilder.ToString().Trim();
		}

		private string GetSegmentSyntaxError(string code) 
			=> code switch
            {
                "1" => "Unrecognized segment ID",
                "2" => "Unexpected segment",
                "3" => "Required segment missing",
                "4" => "Loop occurs over maximum times",
                "5" => "Segment exceeds maximum use",
                "6" => "Segment not in defined transaction set",
                "7" => "Segment not in proper sequence",
                "8" => "Segment has data element errors",
                "I4" => "Implementation \"not used\" segment present",
                "I6" => "Implementation dependent segment missing",
                "I7" => "Implementation loop occurs under minimum times",
                "I8" => "Implementation segment below minimum use",
                "I9" => "Implementation dependent \"not used\" segment present",
                _ => code,
            };

		private string GetElementSyntaxError(string code) 
			=> code switch
            {
                "1" => "Required data element missing",
                "2" => "Conditional required data element missing",
                "3" => "Too many data elements",
                "4" => "Data element too short",
                "5" => "Data element too long",
                "6" => "Invalid character in data element",
                "7" => "Invalid code value",
                "8" => "Invalid date",
                "9" => "Invalid time",
                "10" => "Exclusion condition violated",
                "12" => "Too many repetitions",
                "13" => "Too many components",
                "I10" => "Implementation \"not used\" data element present",
                "I11" => "Implementation too few repetitions",
                "I12" => "Implementation pattern match failure",
                "I13" => "Implementation dependent \"not used\" data element present",
                "I6" => "Code value not used in implementation",
                "I9" => "Implementation dependent data element missing",
                _ => code, // will never happen
            };
	}
}
