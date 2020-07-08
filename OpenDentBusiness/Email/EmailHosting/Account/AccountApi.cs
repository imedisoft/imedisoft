using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using CodeBase;

//This file is auto-generated. Do not change.
namespace OpenDentBusiness {
	public class AccountApi : IAccountApi {
		
		private string _endpoint {
			get {
				if(ODBuild.IsDebug()) {
					return "https://localhost:44330/";
				}
				return "https://emailhosting.opendental.cloud:8200/";
			}
		}

		private HttpClient _client { get; }=new HttpClient();

		public AccountApi(string guid,string secret) {
			_client.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Basic",Convert.ToBase64String(Encoding.UTF8.GetBytes(guid+":"+secret)));
#if DEBUG
			ServicePointManager.ServerCertificateValidationCallback += (sender,cert,chain,sslPolicyErrors) => true;
#endif
		}

		///<summary>Appends data to the specified tmp file</summary>
		public AppendAttachmentDataResponse AppendAttachmentData(AppendAttachmentDataRequest request) {
			return RunApiCall<AppendAttachmentDataResponse>(request,"PUT",$"v1/attachment/append");
		}

		///<summary>Returns all the emails associated to the given chain id.The primary key of the chain.</summary>
		public GetChainEmailsResponse GetChainEmails(GetChainEmailsRequest request) {
			return RunApiCall<GetChainEmailsResponse>(request,"GET",$"v1/emailchain/{request.EmailChainNum.ToString()}/emails");
		}

		///<summary>Returns a secure email for the given primary key.The primary key of the secure email to get.</summary>
		public GetEmailResponse GetEmail(GetEmailRequest request) {
			return RunApiCall<GetEmailResponse>(request,"GET",$"v1/email/secure/{request.EmailNum.ToString()}");
		}

		///<summary>Queues all of the given template destinations for the given template to be sent.</summary>
		public SendMassEmailResponse SendMassEmail(SendMassEmailRequest request) {
			return RunApiCall<SendMassEmailResponse>(request,"POST",$"v1/email/mass");
		}

		///<summary>Creates a new email and email chain for the given request. Will send a "You've got mail" unsecure email to the recipients email address.</summary>
		public SendNewEmailResponse SendNewEmail(SendNewEmailRequest request) {
			return RunApiCall<SendNewEmailResponse>(request,"POST",$"v1/email/secure");
		}

		///<summary>Sends a new email, attaches it to the current chain, and links it to the given email id.</summary>
		public SendReplyResponse SendReply(SendReplyRequest request) {
			return RunApiCall<SendReplyResponse>(request,"POST",$"v1/email/secure/reply");
		}

		///<summary>Creates a new domain identity for the given domain. If the domain already exists as an identity, will return a BadRequest.</summary>
		public CreateDomainIdentityResponse CreateDomainIdentity(CreateDomainIdentityRequest request) {
			return RunApiCall<CreateDomainIdentityResponse>(request,"POST",$"v1/identity/");
		}

		///<summary>Removes the identity at the given primary key.The primary key of the identity to delete.</summary>
		public DeleteIdentityResponse DeleteIdentity(DeleteIdentityRequest request) {
			return RunApiCall<DeleteIdentityResponse>(request,"DELETE",$"v1/identity/{request.IdentityNum.ToString()}");
		}

		///<summary>For the given identity num, returns the domain tokens needed for the CNAME records of the DNS.The primary key of the identity.</summary>
		public GetDomainDKIMTokensResponse GetDomainDKIMTokens(GetDomainDKIMTokensRequest request) {
			return RunApiCall<GetDomainDKIMTokensResponse>(request,"GET",$"v1/identity/dkim/{request.IdentityNum.ToString()}");
		}

		///<summary>Gets an email identity (emails/domains that can be sent from) with the given ID.The primary key of the identity.</summary>
		public GetIdentityResponse GetIdentity(GetIdentityRequest request) {
			return RunApiCall<GetIdentityResponse>(request,"GET",$"v1/identity/{request.TemplateNum.ToString()}");
		}

		///<summary>Creates a new template with the given information.</summary>
		public CreateTemplateResponse CreateTemplate(CreateTemplateRequest request) {
			return RunApiCall<CreateTemplateResponse>(request,"POST",$"v1/template/");
		}

		///<summary>Removes a template for the given primary key. The template must belong to the account calling this method. If the template has already been removed, this will still return no content.The primary key of the template to delete.</summary>
		public DeleteTemplateResponse DeleteTemplate(DeleteTemplateRequest request) {
			return RunApiCall<DeleteTemplateResponse>(request,"DELETE",$"v1/template/{request.TemplateNum.ToString()}");
		}

		///<summary>Gets a template with the given ID.The primary key of the template.</summary>
		public GetTemplateResponse GetTemplate(GetTemplateRequest request) {
			return RunApiCall<GetTemplateResponse>(request,"GET",$"v1/template/{request.TemplateNum.ToString()}");
		}

		///<summary>Updates a template with the given primary key to the given information. All fields are required.</summary>
		public UpdateTemplateResponse UpdateTemplate(UpdateTemplateRequest request) {
			return RunApiCall<UpdateTemplateResponse>(request,"PUT",$"v1/template/");
		}

		private T RunApiCall<T>(object requestObj,string httpType,string path) where T : class,new() {
			T retVal=null;
			Exception ex=null;
			System.Threading.Tasks.Task.Run(async () => {
				try {
					string payload=JsonConvert.SerializeObject(requestObj);
					HttpResponseMessage message=null;
					if(httpType=="GET") {
						//Don't use the request object for GET.
						message=await _client.GetAsync(_endpoint+path);
					}
					else if(httpType=="POST") {
						message=await _client.PostAsync(_endpoint+path,new StringContent(payload,Encoding.UTF8,"application/json"));
					}
					else if(httpType=="PUT") {
						message=await _client.PutAsync(_endpoint+path,new StringContent(payload,Encoding.UTF8,"application/json"));
					}
					else if(httpType=="DELETE") {
						//Don't use the request object for DELETE.
						message=await _client.DeleteAsync(_endpoint+path);
					}
					else {
						throw new ApplicationException($@"Unknown {nameof(httpType)}: {httpType}");
					}
					string body=await message.Content.ReadAsStringAsync();
					var result=JsonConvert.DeserializeAnonymousType(body,new {
						Information="",
						Content=default(T),
					});
					if(!message.IsSuccessStatusCode) {
						throw new ApplicationException($"HttpStatusCode returned: {(int)message.StatusCode} - {message.StatusCode.ToString()}\nInformation: {result?.Information??"NULL RESULT"}");
					}
					//If we made it here and Content is null, they had nothing to return. We will return a new instance so that no one gets an unexpected null result.
					retVal=result?.Content??new T();
				}
				catch(Exception e) {
					ex=e;
				}
			}).GetAwaiter().GetResult();
			if(ex!=null) {
				throw ex;
			}
			return retVal;
		}
	}
}
