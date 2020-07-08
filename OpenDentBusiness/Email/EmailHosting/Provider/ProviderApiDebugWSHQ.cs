using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using CodeBase;

//This file is auto-generated. Do not change.
namespace OpenDentBusiness {
	public class ProviderApiDebugWSHQ:IProviderApi {
		public ProviderApiDebugWSHQ() { }

		public CreateAccountResponse CreateAccount(CreateAccountRequest request) {
			return new CreateAccountResponse() {
				AccountGuid="DEBUGGUID-"+Guid.NewGuid().ToString(),
				AccountSecret="DEBUGSECRET-"+Guid.NewGuid().ToString(),
			};
		}

		public CreateAccountGuarantorResponse CreateAccountGuarantor(CreateAccountGuarantorRequest request) {
			return new CreateAccountGuarantorResponse() {
				AccountGuarantorGuid="DEBUGGUID-"+Guid.NewGuid().ToString(),
				AccountGuarantorNum=new Random().Next(10000,100000),
			};
		}

		public GetAccountResponse GetAccount(GetAccountRequest request) {
			throw new NotImplementedException();
		}

		public GetAccountGuarantorResponse GetAccountGuarantor(GetAccountGuarantorRequest request) {
			throw new NotImplementedException();
		}

		public GetGuarantorUsageResponse GetGuarantorUsage(GetGuarantorUsageRequest request) {
			throw new NotImplementedException();
		}

		public UpdateAccountGuarantorStatusResponse UpdateAccountGuarantorStatus(UpdateAccountGuarantorStatusRequest request) {
			throw new NotImplementedException();
		}

		public UpdateAccountStatusResponse UpdateAccountStatus(UpdateAccountStatusRequest request) {
			return new UpdateAccountStatusResponse() {
			};
		}
	}
}
