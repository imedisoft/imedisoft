using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebServiceSerializer;

namespace OpenDentBusiness
{
	public class WebServiceMainHQMockDemo : WebServiceMainHQ.WebServiceMainHQ, IWebServiceMainHQ
	{
		public List<long> GetEServiceClinicsAllowed(List<long> listClinicNums, eServiceCode eService)
		{
			throw new NotImplementedException();
		}

		public new string EServiceSetup(string officeData)
		{
			try
			{
				WebServiceMainHQProxy.EServiceSetup.SignupOut signupOut = new WebServiceMainHQProxy.EServiceSetup.SignupOut()
				{
					EServices = GetEServicesForAll(),
					HasClinics = PrefC.HasClinicsEnabled,
					ListenerTypeInt = (int)ListenerServiceType.ListenerServiceProxy,
					MethodNameInt = (int)WebServiceMainHQProxy.EServiceSetup.SetupMethod.GetSignupOutFull,
					Phones = new List<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutPhone>(),
					Prompts = new List<string>(),
					SignupPortalPermissionInt = (int)SignupPortalPermission.FullPermission,
					SignupPortalUrl = GetHostedUrlForCode(eServiceCode.SignupPortal),
				};

				//Write the response out as a plain string. We will deserialize it on the other side.
				return WebSerializer.SerializePrimitive<string>(WebSerializer.WriteXml(signupOut));
			}
			catch (Exception ex)
			{
				StringBuilder strbuild = new StringBuilder();
				using (XmlWriter writer = XmlWriter.Create(strbuild, WebSerializer.CreateXmlWriterSettings(true)))
				{
					writer.WriteStartElement("Response");
					writer.WriteStartElement("Error");
					writer.WriteString(ex.Message);
					writer.WriteEndElement();
					writer.WriteEndElement();
				}
				return strbuild.ToString();
			}
		}

		///<summary>Returns all possible eServices for every clinic in the database.</summary>
		private List<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService> GetEServicesForAll()
		{
			if (PrefC.HasClinicsEnabled)
			{
				var services = new List<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>();
				foreach (var clinic in Clinics.GetDeepCopy(true))
				{
					services.AddRange(GetEServicesForClinic(clinic.ClinicNum));
				}

				return services;
			}

			return GetEServicesForClinic(0);
		}

		/// <summary>
		/// Returns all possible eServices for the clinic passed in.
		/// </summary>
		private List<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService> GetEServicesForClinic(long clinicId = 0) 
			=> new List<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>() {
				GetEServiceForCode(eServiceCode.Bundle, clinicId),
				GetEServiceForCode(eServiceCode.ConfirmationRequest, clinicId),
				GetEServiceForCode(eServiceCode.MobileWeb, clinicId),
				GetEServiceForCode(eServiceCode.PatientPortal, clinicId),
				GetEServiceForCode(eServiceCode.PatientPortalMakePayment, clinicId),
				GetEServiceForCode(eServiceCode.PatientPortalViewStatement, clinicId),
				GetEServiceForCode(eServiceCode.SignupPortal, clinicId),
				GetEServiceForCode(eServiceCode.SoftwareUpdate, clinicId),
				GetEServiceForCode(eServiceCode.WebForms, clinicId),
				GetEServiceForCode(eServiceCode.EClipboard, clinicId),
				GetEServiceForCode(eServiceCode.WebSched, clinicId),
				GetEServiceForCode(eServiceCode.WebSchedASAP, clinicId),
				GetEServiceForCode(eServiceCode.WebSchedNewPatAppt, clinicId),
				GetEServiceForCode(eServiceCode.EmailMassUsage, clinicId),
			};

		private WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService GetEServiceForCode(eServiceCode code, long clinicNum = 0)
		{
			if (code == eServiceCode.IntegratedTexting)
			{
				return new WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutSms()
				{
					ClinicNum = clinicNum,
					EServiceCodeInt = (int)code,
					HostedUrl = GetHostedUrlForCode(code),
					HostedUrlPayment = "http://debug.hosted.url.payment",//TODO: no idea what to do here.
					IsEnabled = true,
					CountryCode = "US",
					MonthlySmsLimit = 20,
					SmsContractDate = DateTime.Today.AddYears(-1),
				};
			}

			return new WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService()
			{
				ClinicNum = clinicNum,
				EServiceCodeInt = (int)code,
				HostedUrl = GetHostedUrlForCode(code),
				HostedUrlPayment = "http://debug.hosted.url.payment",//TODO: no idea what to do here.
				IsEnabled = true,
			};
		}

		private string GetHostedUrlForCode(eServiceCode code)
		{
			switch (code)
			{
				case eServiceCode.MobileWeb:
					return "http://127.0.0.1:5000/MobileWeb.html";

				case eServiceCode.PatientPortal:
				case eServiceCode.PatientPortalMakePayment:
				case eServiceCode.PatientPortalViewStatement:
					return "http://127.0.0.1:4000/PatientPortal.html";

				case eServiceCode.SignupPortal:
					return "http://127.0.0.1:8888/SignupPortal";

				case eServiceCode.WebForms:
					return "http://127.0.0.1:3000/WebForms.html";

				case eServiceCode.WebSched:
				case eServiceCode.WebSchedASAP:
				case eServiceCode.WebSchedNewPatAppt:
					return "http://127.0.0.1:8000/WebSched.html";

				case eServiceCode.EmailMassUsage:
					return "https://www.opendental.com/site/massemail.html";

				default:
					return "";
			}
		}

		public new string EmailHostingSignup(string officeData) 
			=> PayloadHelper.CreateSuccessResponse(new List<PayloadItem> {
				new PayloadItem("guid","AccountGUID"),
			});

		public new string EmailHostingChangeClinicStatus(string officeData) 
			=> PayloadHelper.CreateSuccessResponse("Success", "ChangeClinicStatusResponse");
	}
}
