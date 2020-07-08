using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace OpenDentBusiness {
	///<summary>Stores both sent and received emails, as well as saved emails which are still in composition.</summary>
	[Serializable]
	public class EmailMessage:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long EmailMessageNum;
		///<summary>FK to patient.PatNum. The patient whom is sending this message. May be sent by a guarantor on behalf of a dependent.</summary>
		public long PatNum;
		///<summary>Either a single email address or a comma-delimited list of addresses.  
		///For web mail messages, this will not be an email address.  Instead, it will be the name of the corresponding patient or provider.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string ToAddress;
		///<summary>Valid email address.  For web mail messages, this will not be an email address.  Instead, it will be the name of the corresponding patient or provider.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string FromAddress;
		///<summary>Subject line.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Subject;
		///<summary>Body of the email</summary>
//TODO: This column may need to be changed to the TextIsClobNote attribute to remove more than 50 consecutive new line characters.
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string BodyText;
		///<summary>Date and time the message was sent. Automated at the UI level.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime MsgDateTime;
		///<summary>Enum:EmailSentOrReceived Neither, Received, Read, WebMailReceived, WebMailRecdRead, WebMailSent, WebMailSentRead, SentDirect, ReceivedEncrypted, ReceivedDirect, ReadDirect, AckDirectProcessed, AckDirectNotSent</summary>
		public EmailSentOrReceived SentOrReceived;
		///<summary>Copied from the EmailAddress.EmailUsername field when a message is received into the inbox.
		///Similar to the ToAddress, except the ToAddress could contain multiple recipient addresses
		///or group email address instead. The recipient address helps match the an email to a particular EmailAddress.</summary>
		public string RecipientAddress;
		///<summary>For incomming email only.  The raw email contents for encrypted email or email which we had trouble parsing.
		///For unencrypted (clear text) email, this will be similar to the raw email except the attachments will be dissolved to prevent db bloating. 
		///Can be used for debugging if there are any issues parsing the content.
		///This will bloat the database a little bit, but we need it for now to ensure our inbox is working in real world scenarios.
		///Might be blank for a few emails downloaded immediately after the email inbox feature was created.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string RawEmailIn;
		///<summary>Not a database column.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public List<EmailAttach> Attachments;
		///<summary>FK to provider.ProvNum.  The provider to whom this message was sent or from whom this message was sent.  Only used when EmailSentOrReceived is WebMailReceived, WebMailRecdRead, WebMailSent, or WebMailSentRead.  Will be 0 if not a web mail message.</summary>
		public long ProvNumWebMail;
		///<summary>FK to patient.PatNum. Represents the patient to whom this email message is addressed, or from whom it is being sent on behalf of. If guarantor is sending on behalf of self then this field will match PatNum field.</summary>
		public long PatNumSubj;
		///<summary>Single address or comma-delimited list of addresses.  User may enter multiple email addresses for visible carbon copies.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string CcAddress;
		///<summary>Single email address or comma-delimited list of addresses.  User may enter multiple email addresses for blind carbon copies.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string BccAddress;
		///<summary>Enum:HideInFlags None=0,EmailInbox=1,ApptEdit=2,ContrChartProgNotes=4,ContrAccountGridProg.  Indicates which places in the program that should not show this email message, bitwise.</summary>
		public HideInFlags HideIn;
		///<summary>FK to appointment.AptNum. Used to a attach an email to an appointment for eReminders and eConfirmations.</summary>
		public long AptNum;
		///<summary>FK to userod.UserNum.  Optional.  0 if unknown (ex recieved emails).</summary>
		public long UserNum;
		///<summary>Enum:EmailType </summary>
		public EmailType HtmlType;
		///<summary>Not a database column.  Only set when IsHtml is true.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public string HtmlText;
		///<summary>Not a database column.  When true, the img tags will contain paths to local temp files where the images are stored. If false, the images
		///need to be downloaded from the cloud.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public bool AreImagesDownloaded;
		///<summary>Timestamp automatically generated and user not allowed to change.  The actual date of entry.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateTEntry)]
		public DateTime SecDateTEntry;
		///<summary>Automatically updated by MySQL every time a row is added or changed. Could be changed due to user editing, custom queries or program
		///updates.  Not user editable with the UI.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TimeStamp)]
		public DateTime SecDateTEdit;

		///<summary>Constructor</summary>
		public EmailMessage(){
			Attachments=new List<EmailAttach>();
		}

		public EmailMessage Copy() {
			EmailMessage e=(EmailMessage)this.MemberwiseClone();
			e.Attachments=new List<EmailAttach>();
			for(int i=0;i<Attachments.Count;i++) {
				e.Attachments.Add(Attachments[i].Copy());
			}
			return e;
		}
	}

	///<summary>0=Neither, 1=Sent, 2=Received, 3=Read, 4=WebMailReceived, 5=WebMailRecdRead, 6=WebMailSent, 7=WebMailSentRead, 8=SentDirect, 9=ReceivedEncrypted, 10=ReceivedDirect, 11=ReadDirect, 12=AckDirectProcessed, 13=AckDirectNotSent</summary>
	public enum EmailSentOrReceived {
		///<summary>0 Unsent</summary>
		Neither,
		///<summary>1 For regular email only.</summary>
		Sent,
		///<summary>2 For regular email only.  Shows in Inbox.  Once it's attached to a patient it will also show in Chart module.</summary>
		Received,
		///<summary>3 For received regular email only.  Has been read.  Shows in Inbox.  Once it's attached to a patient it will also show in Chart module.</summary>
		Read,
		///<summary>4 WebMail received from patient portal.  Shows in OD Inbox and in pt Chart module.  Also shows in PP as a sent and unread WebMail msg.</summary>
		WebMailReceived,
		///<summary>5 WebMail received from patient portal that has been marked read.  Shows in the OD Inbox and in pt Chart module.  Also shows in PP as a sent and read WebMail.</summary>
		WebMailRecdRead,
		///<summary>6 Webmail sent from provider to patient.  Shows in Chart module and also shows in PP as a received and unread WebMail msg.</summary>
		WebMailSent,
		///<summary>7 Webmail sent from provider to patient and read by patient.  Shows in Chart module and also shows in PP as a received and read WebMail msg.</summary>
		WebMailSentRead,
		///<summary>8 Sent and encrypted using Direct. Required for counting messages in EHR modules g.1 and g.2, Automated Measure Calculation.</summary>
		SentDirect,
		///<summary>9 Received email matches application/pkcs7-mime mime type, but could not be decrypted.  Shows in Inbox.  The user can decrypt from FormEmailMessageEdit.  If the user has the correct private key, then the status will change to Read.</summary>
		ReceivedEncrypted,
		///<summary>10 Received email matches application/pkcs7-mime mime type and has been decrypted.  Shows in Inbox.  Once it's attached to a patient it will also show in Chart module.  When viewing inside of FormEmailMessageEdit, the XML body of the message shows as xhtml instead of raw.  Still need to work on supporting collapsing and expanding, as required for meaningful use in 2014.</summary>
		ReceivedDirect,
		///<summary>11 For received direct messages.  Has been read.  Shows in Inbox.  Once it's attached to a patient it will also show in Chart module.  When viewing inside of FormEmailMessageEdit, the XML body of the message shows as xhtml.</summary>
		ReadDirect,
		///<summary>12 Message Delivery Notification (MDN) processed.  Always outgoing.  Indicates to sender that a Direct message was received and decrypted, but not necessarily displayed for the user.  Does not show in patient Chart.  Attached to the same patient as the incoming email which caused the MDN to be sent.</summary>
		AckDirectProcessed,
		///<summary>13 Message Delivery Notification (MDN) created and saved to db, but not sent yet.  Does not show in patient Chart.  Attached to the same patient as the incoming email which caused the MDN to be created.
		///This status is used to try resending MDNs if they fail to send.  The MDN is saved to the db so the unset MDNs can be found easily, and also because MDNs are hard to rebuild again later.</summary>
		AckDirectNotSent,
	}
	
	///<summary></summary>
	[Flags]
	public enum HideInFlags {
		///<summary>0 - None</summary>
		None=0,
		///<summary>1 - Hide email from EmailInbox grids</summary>
		[Description("Email Inbox")]
		EmailInbox=1,
		///<summary>2 - Hide email from Appointment Edit grid</summary>
		[Description("Appointment Edit")]
		ApptEdit=2,
		///<summary>4 - Hide email from ContrChart ProgNotes grid</summary>
		[Description("Chart Progress Notes")]
		ChartProgNotes=4,
		///<summary>8 - Hide email from ContrAccount gridProg grid</summary>
		[Description("Account Progress Notes")]
		AccountProgNotes=8,
		///<summary>16 - Hide email from ContrAcount CommLog grid</summary>
		[Description("Account Comm Log")]
		AccountCommLog=16
  }

	///<summary></summary>
  public enum MailboxType {
    ///<summary>1</summary>
    Inbox,
    ///<summary>2</summary>
    Sent,
  }

	///<summary></summary>
	public enum EmailType {
		///<summary>0 - This is a regular email that may contain our special wiki markup.</summary>
		Regular,
		///<summary>1 - Html. Basic html email which uses the master template.</summary>
		Html,
		///<summary>2 - More advanced html that does not include the master template, user must provide everything.</summary>
		RawHtml
	}

}
