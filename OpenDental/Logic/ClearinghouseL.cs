using OpenDentBusiness;

namespace OpenDental
{
    public class ClearinghouseL
	{
		/// <summary>
		/// Returns the clearinghouse specified by the given num. 
		/// Will only return an HQ-level clearinghouse. (Will it though?)
		/// Do not attempt to pass in a clinic-level clearinghouseNum.
		/// Can return null if no match found.
		/// </summary>
		public static Clearinghouse GetClearinghouseHq(long hqClearinghouseNum, bool suppressError = false)
		{
			var clearinghouse = Clearinghouses.GetClearinghouse(hqClearinghouseNum);

			if (clearinghouse == null && !suppressError)
			{
				MsgBox.Show("Error. Could not locate Clearinghouse.");
			}

			return clearinghouse;
		}

		public static string GetDescript(long clearinghouseNum) 
			=> GetClearinghouseHq(clearinghouseNum)?.Description ?? "";
	}
}
