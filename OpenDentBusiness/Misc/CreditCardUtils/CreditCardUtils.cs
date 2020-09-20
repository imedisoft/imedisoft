using System;
using System.Globalization;
using System.Text;

namespace OpenDentBusiness
{
    public class CreditCardUtils
	{
		public static string GetCardType(string creditCardNumber)
		{
			if (string.IsNullOrEmpty(creditCardNumber)) return "";

			creditCardNumber = StripNonDigits(creditCardNumber);
			if (creditCardNumber.StartsWith("4"))
			{
				return "VISA";
			}

			if (creditCardNumber.StartsWith("5"))
			{
				return "MASTERCARD";
			}

			if (creditCardNumber.StartsWith("34") || creditCardNumber.StartsWith("37"))
			{
				return "AMEX";
			}

			if (creditCardNumber.StartsWith("30") || creditCardNumber.StartsWith("36") || creditCardNumber.StartsWith("38"))
			{
				return "DINERS";
			}

			if (creditCardNumber.StartsWith("6011"))
			{
				return "DISCOVER";
			}

			return "";
		}

		/// <summary>
		/// Strips non-digit characters from a string. Returns the modified string, or null if 's' is null.
		/// </summary>
		public static string StripNonDigits(string s)
		{
			return StripNonDigits(s, new char[] { });
		}

		/// <summary>
		/// Strips non-digit characters from a string. The variable s is the string to strip.
		/// The allowed array must contain characters that should not be stripped. Returns the modified string, or null if 's' is null.
		/// </summary>
		public static string StripNonDigits(string s, char[] allowed)
		{
			if (s == null) return null;

			StringBuilder buff = new StringBuilder(s);
			StripNonDigits(buff, allowed);

			return buff.ToString();
		}

		/// <summary>
		/// Strips non-digit characters from a string. The variable s is the string to strip.
		/// </summary>
		public static void StripNonDigits(StringBuilder s)
		{
			StripNonDigits(s, new char[] { });
		}

		/// <summary>
		/// Strips non-digit characters from a string. The variable s is the string to strip.
		/// The allowed array must contain the characters that should not be stripped.
		/// </summary>
		public static void StripNonDigits(StringBuilder s, char[] allowed)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsDigit(s[i]) && !ContainsCharacter(s[i], allowed))
				{
					s.Remove(i, 1);
					i--;
				}
			}
		}

		/// <summary>
		/// Searches a character array for the presence of the given character.
		/// Variable c is the character to search for. The search array is the array to search in.
		/// Returns true if the character is present in the array. false otherwise.
		/// </summary>
		public static bool ContainsCharacter(char c, char[] search)
		{
			foreach (char x in search)
			{
				if (c == x)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns the clinic information for the passed in clinicNum.
		/// </summary>
		public static string AddClinicToReceipt(long clinicNum)
		{
			string result = "";

			// clinicNum will be 0 if clinics are not enabled or if the payment.ClinicNum=0, which will happen if the patient.ClinicNum=0 and the user
			// does not change the clinic on the payment before sending to PayConnect or if the user decides to process the payment for 'Headquarters'
			// and manually changes the clinic on the payment from the patient's clinic to 'none'
			Clinic clinicCur;
			if (clinicNum == 0)
			{
				clinicCur = Clinics.GetPracticeAsClinicZero();
			}
			else
			{
				clinicCur = Clinics.GetById(clinicNum);
			}

			if (clinicCur != null)
			{
				if (clinicCur.Description.Length > 0)
				{
					result += clinicCur.Description + Environment.NewLine;
				}

				if (clinicCur.AddressLine1.Length > 0)
				{
					result += clinicCur.AddressLine1 + Environment.NewLine;
				}

				if (clinicCur.AddressLine2.Length > 0)
				{
					result += clinicCur.AddressLine2 + Environment.NewLine;
				}

				if (clinicCur.City.Length > 0 || clinicCur.State.Length > 0 || clinicCur.Zip.Length > 0)
				{
					result += clinicCur.City + ", " + clinicCur.State + " " + clinicCur.Zip + Environment.NewLine;
				}

				if (clinicCur.Phone.Length == 10 && (CultureInfo.CurrentCulture.Name == "en-US" || CultureInfo.CurrentCulture.Name.EndsWith("CA"))) //Canadian. en-CA or fr-CA
				{
					result += "(" + clinicCur.Phone.Substring(0, 3) + ")" + clinicCur.Phone.Substring(3, 3) + "-" + clinicCur.Phone.Substring(6) + Environment.NewLine;
				}
				else if (clinicCur.Phone.Length > 0)
				{
					result += clinicCur.Phone + Environment.NewLine;
				}
			}
			result += Environment.NewLine;
			return result;
		}
	}
}
