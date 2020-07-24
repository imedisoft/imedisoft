using System;
using System.Collections.Generic;
using CodeBase;
using System.Linq;

namespace OpenDentBusiness
{
	/// <summary>
	/// A 271 is the eligibility response to a 270.
	/// </summary>
	public class X271 : X12object
	{
		public X271(string messageText) : base(messageText)
		{
		}

		/// <summary>
		/// In realtime mode, X12 limits the request to one patient. 
		/// We will always use the subscriber. 
		/// So all EB segments are for the subscriber.
		/// </summary>
		public List<EB271> GetListEB(bool isInNetwork, bool isCoinsuranceInverted)
		{
			var results = new List<EB271>();

			EB271 eb = null;
			for (int i = 0; i < Segments.Count; i++)
			{
				// loop until we encounter the first EB
				if (Segments[i].SegmentID != "EB" && eb == null)
				{
					continue;
				}

				if (Segments[i].SegmentID == "EB")
				{
					// add the previous eb
					if (eb != null)
					{
						results.Add(eb);
					}

					X12Segment hsdSeg = null;
					if (Segments[i + 1].SegmentID == "HSD")
					{
						hsdSeg = Segments[i + 1];
					}

					// then, start the next one
					eb = new EB271(Segments[i], isInNetwork, isCoinsuranceInverted, hsdSeg);
					continue;
				}
				else if (Segments[i].SegmentID == "SE")
				{
					// end of benefits
					results.Add(eb);
					break;
				}
				else
				{
					// add to existing eb
					eb.SupplementalSegments.Add(Segments[i]);
					continue;
				}
			}

			return results;
		}

		public string GetGroupNum()
		{
			foreach (var segment in Segments)
			{
				if (segment.SegmentID == "REF" && segment.Elements[1] == "6P")
				{
					// Line beginning "REF*6P* indicates group num
					return segment.Elements[2]; // The group num is the third element in the line
				}
			}

			return "";
		}

		/// <summary>
		/// Only the DTP segments that come before the EB segments.
		/// X12 loop 2100C.
		/// </summary>
		public List<DTP271> GetListDtpSubscriber()
		{
			var results = new List<DTP271>();
	
			foreach (var segment in Segments)
			{
				if (segment.SegmentID == "EB")
				{
					break;
				}

				if (segment.SegmentID != "DTP")
				{
					continue;
				}

				results.Add(new DTP271(segment));
			}

			return results;
		}

		/// <summary>
		/// If there was no processing error (2100A, 2100B, 2100C, 2110C AAA segment), then this will return empty string.
		/// </summary>
		public string GetProcessingError()
		{
			string result = "";

			foreach (var segment in Segments)
			{
				if (segment.SegmentID != "AAA")
				{
					continue;
				}

				if (result != "")
				{//if multiple errors
					result += ", ";
				}

				result += 
					GetRejectReason(segment.Get(3)) + ", " + 
					GetFollowupAction(segment.Get(4));
			}

			return result;
		}

		/// <summary>
		/// Some of these codes are only found in certain loops.
		/// </summary>
		private string GetRejectReason(string code) 
			=> code switch
            {
                "04" => "Authorized Quantity Exceeded (too many patients in request)",
                "15" => "Required application data missing",
                "41" => "Authorization Access Restriction (not allowed to submit requests)",
                "42" => "Unable to Respond at Current Time",
                "43" => "Invalid/Missing Provider Identification",
                "44" => "Invalid/Missing Provider Name",
                "45" => "Invalid/Missing Provider Specialty",
                "46" => "Invalid/Missing Provider Phone Number",
                "47" => "Invalid/Missing Provider State",
                "48" => "Invalid/Missing Referring Provider Identification Number",
                "49" => "Provider is Not Primary Care Physician",
                "50" => "Provider Ineligible for Inquiries",
                "51" => "Provider Not on File",
                "52" => "Service Dates Not Within Provider Plan Enrollment",
                "53" => "Inquired Benefit Inconsistent with Provider Type",
                "54" => "Inappropriate Product/Service ID Qualifier",
                "55" => "Inappropriate Product/Service ID",
                "56" => "Inappropriate Date",
                "57" => "Invalid/Missing Date(s) of Service",
                "58" => "Invalid/Missing Date-of-Birth",
                "60" => "Date of Birth Follows Date(s) of Service",
                "61" => "Date of Death Precedes Date(s) of Service",
                "62" => "Date of Service Not Within Allowable Inquiry Period",
                "63" => "Date of Service in Future",
                "64" => "Invalid/Missing Patient ID",
                "65" => "Invalid/Missing Patient Name",
                "66" => "Invalid/Missing Patient Gender Code",
                "67" => "Patient Not Found",
                "68" => "Duplicate Patient ID Number",
                "69" => "Inconsistent with Patient’s Age",
                "70" => "Inconsistent with Patient’s Gender",
                "71" => "Patient Birth Date Does Not Match That for the Patient on the Database",
                "72" => "Invalid/Missing Subscriber/Insured ID",
                "73" => "Invalid/Missing Subscriber/Insured Name",
                "74" => "Invalid/Missing Subscriber/Insured Gender Code",
                "75" => "Subscriber/Insured Not Found",
                "76" => "Duplicate Subscriber/Insured ID Number",
                "77" => "Subscriber Found, Patient Not Found",
                "78" => "Subscriber/Insured Not in Group/Plan Identified",
                "79" => "Invalid Participant Identification (this payer does not provide e-benefits)",
                "80" => "No Response received - Transaction Terminated",
                "97" => "Invalid or Missing Provider Address",
                "T4" => "Payer Name or Identifier Missing",
                _ => "Error code '" + code + "' not valid.",
            };

		private string GetFollowupAction(string code) 
			=> code switch
            {
                "C" => "Please Correct and Resubmit",
                "N" => "Resubmission Not Allowed",
                "P" => "Please Resubmit Original Transaction",
                "R" => "Resubmission Allowed",
                "S" => "Do Not Resubmit; Inquiry Initiated to a Third Party",
                "W" => "Please Wait 30 Days and Resubmit",
                "X" => "Please Wait 10 Days and Resubmit",
                "Y" => "Do Not Resubmit; We Will Hold Your Request and Respond Again Shortly",
                _ => "Error code '" + code + "' not valid.",
            };

		///<summary>Returns a non-empty string if there would be a display issue due to invalid settings.
		///Use the result to block the display from the user when needed.</summary>
		public static string ValidateSettings()
		{
			string validationErrors = "";
			Array arrayEbenetitCats = Enum.GetValues(typeof(EbenefitCategory));
			for (int i = 0; i < arrayEbenetitCats.Length; i++)
			{
				EbenefitCategory ebenCat = (EbenefitCategory)arrayEbenetitCats.GetValue(i);
				if (ebenCat == EbenefitCategory.None)
				{
					continue;
				}
				CovCat covCat = CovCats.GetForEbenCat(ebenCat);
				if (covCat == null)
				{
					if (validationErrors != "")
					{
						validationErrors += ", ";
					}
					validationErrors += ebenCat.ToString();
				}
			}
			if (validationErrors != "")
			{
				validationErrors = "Missing or hidden insurance category for each of the following E-benefits:" + "\r\n"
					+ validationErrors + "\r\n"
					+ "Go to Setup then Insurance Categories to add or edit.";
			}
			return validationErrors;
		}

		internal bool IsValidForBatchVerification(List<EB271> listBenefits, bool isCoinsuranceInverted, out string errorMsg)
		{
			errorMsg = this.GetProcessingError();
			if (errorMsg != "")
			{
				return false;
			}
			if (listBenefits.Count == 0)
			{
				errorMsg = "No benefits reported.";
				return false;
			}
			else if (listBenefits.Count == 1)
			{
				EB271 eb271 = listBenefits[0];
				switch (eb271.Segment.Elements[1])
				{
					case "U"://Contact Following Entity for Information for Eligibility or Benefit Information
						errorMsg = "Contact carrier for more information.";//There will be an MSG segment following this.
						X12Segment msgSegment = eb271.SupplementalSegments.FirstOrDefault(x => x.SegmentID == "MSG");
						if (msgSegment != null)
						{
							errorMsg += "\r\n" + msgSegment.Get(1);
						}
						break;
					//The following codes have not been reported as of yet.
					case "6"://Inactive
					case "7"://Inactive - Pending Eligibility Update
					case "8"://Inactive - Pending Investigation
					case "T"://Card(s) Reported Lost/Stolen
					case "V"://Cannot Process
						errorMsg = eb271.GetEB01Description(eb271.Segment.Elements[1]);//Returns null if given code is not known.
						break;
					default:
						//Intentionally blank, most other EB01 codes are not easily identified as errors.
						break;
				}
			}
			return (errorMsg.IsNullOrEmpty());
		}
	}
}
