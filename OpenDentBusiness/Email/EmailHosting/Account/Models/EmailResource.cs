using System;
using System.Collections.Generic;

//This file is auto-generated. Do not change.
namespace OpenDentBusiness {
	///<summary>Represents a secure email stored or in transit to/from at the Email Hosting endpoint.</summary>
	public class EmailResource {

		///<summary>The address this email was sent from.</summary>
		public string FromAddress { get; set; }

		///<summary>The address this email was sent to.</summary>
		public string ToAddress { get; set; }

		///<summary>The subject of this email.</summary>
		public string Subject { get; set; }

		///<summary>The body of this email.</summary>
		public string BodyHtml { get; set; }

	}
}
