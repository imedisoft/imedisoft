using CodeBase;
using Newtonsoft.Json;
using OpenDentBusiness;
using System;
using System.Net;

namespace OpenDental.Bridges
{
    public static class Oryx
	{
		/// <summary>
		/// Makes an API call to get an Oryx URL to launch that is specific to the current user and patient.
		/// </summary>
		public static void SendData(Program progOryx, Patient pat)
		{
			string clientUrl = "";
			try
			{
				clientUrl = OpenDentBusiness.ProgramProperties.GetPropVal(progOryx.Id, ProgramProperties.ClientUrl);

				if (clientUrl == "")
				{//Office has not signed up with Oryx yet, launch a promotional page.
					string promoUrl = "http://www.opendental.com/resources/redirects/redirectoryx.html";
#if DEBUG
					promoUrl = "http://www.opendental.com/resources/redirects/redirectoryxdebug.html";
#endif
					ODFileUtils.ProcessStart(promoUrl);
					return;
				}

				if (!progOryx.Enabled)
				{
					MsgBox.Show("Oryx must be enabled in Program Links.");
					return;
				}
				if (!clientUrl.ToLower().StartsWith("http"))
				{
					clientUrl = "https://" + clientUrl;
				}

				var username = UserPreference.GetString(UserPreferenceName.ProgramUserName, progOryx.Id);
				var password = UserPreference.GetString(UserPreferenceName.ProgramPassword, progOryx.Id);

				if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
				{
					//User hasn't entered credentials yet. Launch the office's Oryx page where the user can then log in.
					ODFileUtils.ProcessStart(clientUrl);
					return;
				}
				string apiUrl = clientUrl.TrimEnd('/') + "/api/auth/opendental/v1/login";

				var content = new
				{
                    username,
                    password,
					patientId = (pat != null ? pat.PatNum.ToString() : ""),
				};
				string contentJson = JsonConvert.SerializeObject(content);
				string responseStr;
				using (WebClient client = new WebClient())
				{
					client.Headers[HttpRequestHeader.ContentType] = "application/json";
					responseStr = client.UploadString(apiUrl, "POST", contentJson);
				}
				var response = new
				{
					success = false,
					redirectUrl = "",
					errorMessage = "",
				};
				response = JsonConvert.DeserializeAnonymousType(responseStr, response);
				if (!response.success)
				{
					MessageBox.Show("Error message from Oryx: " + response.errorMessage);
					return;
				}
				ODFileUtils.ProcessStart(response.redirectUrl);
			}
			catch (Exception ex)
			{
				string errorMessage = "Unable to launch Oryx.";
				if (ex is NotSupportedException && ex.Message == "The given path's format is not supported.")
				{
					//Oryx has asked us to give a more helpful error message when this happens.
					errorMessage += " This is likely because the Client URL is invalid.\r\nClient URL: " + clientUrl;
				}

				FriendlyException.Show(errorMessage, ex);
			}
		}

		/// <summary>
		/// Shows a form where the user can enter their username and password.
		/// </summary>
		public static void menuItemUserSettingsClick(object sender, EventArgs e)
		{
			FormOryxUserSettings FormOUS = new FormOryxUserSettings();
			FormOUS.Show();
		}

		public class ProgramProperties
		{
			public static string ClientUrl = "Client URL";
			public static string DisableAdvertising = "Disable Advertising";
		}
	}
}
