using CodeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness {
	public class AccountApiMock : IAccountApi {

		public AppendAttachmentDataResponse AppendAttachmentData(AppendAttachmentDataRequest request) {
			throw new NotImplementedException();
		}

		public CreateDomainIdentityResponse CreateDomainIdentity(CreateDomainIdentityRequest request) {
			//Not used yet.
			throw new NotImplementedException();
		}

		public CreateTemplateResponse CreateTemplate(CreateTemplateRequest request) {
			return new CreateTemplateResponse { 
				TemplateNum=0
			};
		}

		public DeleteIdentityResponse DeleteIdentity(DeleteIdentityRequest request) {
			//Not used yet.
			throw new NotImplementedException();
		}

		public DeleteTemplateResponse DeleteTemplate(DeleteTemplateRequest request) {
			return new DeleteTemplateResponse();
		}

		public GetChainEmailsResponse GetChainEmails(GetChainEmailsRequest request) {
			//Not used yet.
			throw new NotImplementedException();
		}

		public GetDomainDKIMTokensResponse GetDomainDKIMTokens(GetDomainDKIMTokensRequest request) {
			//Not used yet.
			throw new NotImplementedException();
		}

		public GetEmailResponse GetEmail(GetEmailRequest request) {
			//Not used yet.
			throw new NotImplementedException();
		}

		public GetIdentityResponse GetIdentity(GetIdentityRequest request) {
			//Not used yet.
			throw new NotImplementedException();
		}

		public GetTemplateResponse GetTemplate(GetTemplateRequest request) {
			//Not used yet.
			throw new NotImplementedException();
		}

		public SendMassEmailResponse SendMassEmail(SendMassEmailRequest request) {
			return new SendMassEmailResponse {
				DictionaryUniqueIDToHostingID=request.ListDestinations.ToDictionary(x => x.UniqueID,x => (long)0),
			};
		}

		public SendNewEmailResponse SendNewEmail(SendNewEmailRequest request) {
			//Not used yet.
			throw new NotImplementedException();
		}

		public SendReplyResponse SendReply(SendReplyRequest request) {
			//Not used yet.
			throw new NotImplementedException();
		}

		public UpdateTemplateResponse UpdateTemplate(UpdateTemplateRequest request) {
			return new UpdateTemplateResponse();
		}
	}
}
