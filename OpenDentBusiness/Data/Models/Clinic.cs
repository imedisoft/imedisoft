using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;
using Imedisoft.X12.Codes;

namespace OpenDentBusiness
{
    /// <summary>
    ///		<para>
    ///			A clinic is usually a separate physical office location. 
    ///		</para>
    ///		<para>
    ///			Patients, operatories, claims, and many other types of objects can be assigned to a clinic.
    ///		</para>
    /// </summary>
    [Table("clinics")]
	public class Clinic
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		///	A abbreviation for the clinic's description.
		/// </summary>
		public string Abbr;

		/// <summary>
		/// A description of the clinic.
		/// </summary>
		public string Description;

		public string AddressLine1;
		public string AddressLine2;
		public string City;
		public string State;
		public string Zip;

		public string BillingAddressLine1;
		public string BillingAddressLine2;
		public string BillingCity;
		public string BillingState;
		public string BillingZip;

		/// <summary>
		/// A value indicating whether the use the clinics billing address on outgoing claims.
		/// </summary>
		public bool BillingAddressOnClaims;

		public string PayToAddressLine1;
		public string PayToAddressLine2;
		public string PayToCity;
		public string PayToState;
		public string PayToZip;

		/// <summary>
		///		<para>
		///			The phone number of the clinic. Does not include any punctuation.
		///		</para>
		///		<para>
		///			Exactly 10 digits or blank in USA and Canada.
		///		</para>
		/// </summary>
		public string Phone;

		/// <summary>
		/// The account number for deposits.
		/// </summary>
		public string BankNumber;

		/// <summary>
		/// The two-digit place of service code that indicates the clinic setting.
		/// </summary>
		/// <seealso cref="PlaceOfService"/>
		[Column(MaxLength = 2)]
		public string DefaultPlaceOfService = PlaceOfService.Office;

		/// <summary>
		///		<para>
		///			A code indicating the which provider to use for insurance billing.
		///		</para>
		///		<list type="table">
		///			<item>
		///				<term>D</term> Default
		///			</item>
		///			<item>
		///				<term>T</term> Treating Provider
		///			</item>
		///			<item>
		///				<term>S</term> Specific Provider (see <see cref="InsBillingProviderId"/>).
		///			</item>
		///		</list>
		/// </summary>
		public char InsBillingProviderType = 'D';

		/// <summary>
		/// The ID of the provider for insurance billing. Only applies if the value of <see cref="InsBillingProviderType"/> is 'S'.
		/// </summary>
		/// <seealso cref="InsBillingProviderType"/>
		[ForeignKey(typeof(Provider), nameof(Provider.ProvNum))]
		public long? InsBillingProviderId;

		/// <summary>
		/// A value indicating whether the clinic should be excluded from showing up in the Insurance Verification List.
		/// </summary>
		public bool InsVerifyExcluded;

		/// <summary>
		///		<para>
		///			The fax number of the clinic. Does not include any punctuation.
		///		</para>
		///		<para>
		///			Exactly 10 digits or blank in USA and Canada.
		///		</para>
		/// </summary>
		public string Fax;

		[ForeignKey(typeof(EmailAddress), nameof(EmailAddress.EmailAddressNum))]
		public long? EmailAddressId;

		/// <summary>
		/// Used in place of the default practice provider when making new patients.
		/// </summary>
		[ForeignKey(typeof(Provider), nameof(Provider.ProvNum))]
		public long? DefaultProviderId;

		/// <summary>
		///		<para>
		///			A value indicating whether the clinic is a medical clinic.
		///		</para>
		///		<para>
		///			Used to hide/change certain areas of Open Dental, like hiding the tooth chart and changing 'dentist' to 'provider'.
		///		</para>
		/// </summary>
		public bool IsMedicalOnly;

		/// <summary>
		/// The ID of the definition of the region of the clinic.
		/// </summary>
		[ForeignKey(typeof(Definition), nameof(Definition.Id))]
		public long? Region;

		/// <summary>
		/// Used to filter MedLab results by the MedLab Account Number assigned to each clinic.
		/// </summary>
		[ForeignKey(typeof(MedLab), nameof(MedLab.PatAccountNum))]
		public string MedlabAccountId;

		/// <summary>
		/// A value indicating whether to send automated reminders/confirmations for this clinic.
		/// </summary>
		public bool IsConfirmEnabled;

		/// <summary>
		/// A value indicating whether to the use the default automated reminder/confirmation settings as defined by the user.
		/// </summary>
		public bool IsConfirmDefault;

		/// <summary>
		/// A value indicating whether the clinic is hidden.
		/// </summary>
		public bool IsHidden;

		/// <summary>
		/// Indicates if the clinic should only be scheduled in a certain way (e.g. ortho only, etc)
		/// </summary>
		public string SchedulingNote;

		/// <summary>
		/// A value indicating whether the clinic requires procedures to be attached to controlled prescriptions.
		/// </summary>
		public bool HasProcedureOnRx;

		public Clinic Copy() => (Clinic)MemberwiseClone();
	}
}
