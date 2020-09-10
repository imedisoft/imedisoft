using Imedisoft.Data;
using WebServiceSerializer;

namespace OpenDentBusiness.WebTypes.WebForms
{
	public class WebUtils
	{
		public static long GetDentalOfficeID(string registrationKey = null)
		{
			if (string.IsNullOrEmpty(registrationKey))
			{
				registrationKey = Preferences.GetString(PreferenceName.RegistrationKey);
			}

			try
			{
				string payload = PayloadHelper.CreatePayloadWebHostSynch(registrationKey, new PayloadItem(registrationKey, "RegKey"));

				return WebSerializer.DeserializeTag<long>(SheetsSynchProxy.GetWebServiceInstance().GetDentalOfficeID(payload), "Success");
			}
			catch
			{
			}

			return 0;
		}

		public static string GetSheetDefAddress(string registrationKey = null)
		{
			if (string.IsNullOrEmpty(registrationKey))
			{
				registrationKey = Preferences.GetString(PreferenceName.RegistrationKey);
			}

			try
			{
				string payload = PayloadHelper.CreatePayloadWebHostSynch(registrationKey, new PayloadItem(registrationKey, "RegKey"));

				return WebSerializer.DeserializeTag<string>(SheetsSynchProxy.GetWebServiceInstance().GetSheetDefAddress(payload), "Success");
			}
			catch
			{
			}

			return "";
		}
	}
}
