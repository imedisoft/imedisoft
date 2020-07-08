//This file is auto-generated. Do not change.
namespace OpenDentBusiness {
	public interface IAccountApi {

		///<summary>Appends data to the specified tmp file</summary>
		AppendAttachmentDataResponse AppendAttachmentData(AppendAttachmentDataRequest request);

		///<summary>Returns all the emails associated to the given chain id.The primary key of the chain.</summary>
		GetChainEmailsResponse GetChainEmails(GetChainEmailsRequest request);

		///<summary>Returns a secure email for the given primary key.The primary key of the secure email to get.</summary>
		GetEmailResponse GetEmail(GetEmailRequest request);

		///<summary>Queues all of the given template destinations for the given template to be sent.</summary>
		SendMassEmailResponse SendMassEmail(SendMassEmailRequest request);

		///<summary>Creates a new email and email chain for the given request. Will send a "You've got mail" unsecure email to the recipients email address.</summary>
		SendNewEmailResponse SendNewEmail(SendNewEmailRequest request);

		///<summary>Sends a new email, attaches it to the current chain, and links it to the given email id.</summary>
		SendReplyResponse SendReply(SendReplyRequest request);

		///<summary>Creates a new domain identity for the given domain. If the domain already exists as an identity, will return a BadRequest.</summary>
		CreateDomainIdentityResponse CreateDomainIdentity(CreateDomainIdentityRequest request);

		///<summary>Removes the identity at the given primary key.The primary key of the identity to delete.</summary>
		DeleteIdentityResponse DeleteIdentity(DeleteIdentityRequest request);

		///<summary>For the given identity num, returns the domain tokens needed for the CNAME records of the DNS.The primary key of the identity.</summary>
		GetDomainDKIMTokensResponse GetDomainDKIMTokens(GetDomainDKIMTokensRequest request);

		///<summary>Gets an email identity (emails/domains that can be sent from) with the given ID.The primary key of the identity.</summary>
		GetIdentityResponse GetIdentity(GetIdentityRequest request);

		///<summary>Creates a new template with the given information.</summary>
		CreateTemplateResponse CreateTemplate(CreateTemplateRequest request);

		///<summary>Removes a template for the given primary key. The template must belong to the account calling this method. If the template has already been removed, this will still return no content.The primary key of the template to delete.</summary>
		DeleteTemplateResponse DeleteTemplate(DeleteTemplateRequest request);

		///<summary>Gets a template with the given ID.The primary key of the template.</summary>
		GetTemplateResponse GetTemplate(GetTemplateRequest request);

		///<summary>Updates a template with the given primary key to the given information. All fields are required.</summary>
		UpdateTemplateResponse UpdateTemplate(UpdateTemplateRequest request);
	}
}
