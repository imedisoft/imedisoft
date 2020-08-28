using Imedisoft.Data.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

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
	public class Clinic : TableBase
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

		///<summary>Second line of address.</summary>
		public string AddressLine2;

		public string City;

		/// <summary>2 char in the US.</summary>
		public string State;

		public string Zip;

		///<summary>Overrides Address on claims if not blank.</summary>
		public string BillingAddressLine1;

		///<summary>Second line of billing address.</summary>
		public string BillingAddressLine2;

		///<summary>Overrides City on claims if BillingAddress is not blank.</summary>
		public string BillingCity;

		///<summary>Overrides State on claims if BillingAddress is not blank.</summary>
		public string BillingState;

		///<summary>Overrides Zip on claims if BillingAddress is not blank.</summary>
		public string BillingZip;

		/// <summary>
		/// A value indicating whether the use the clinics billing address on outgoing claims.
		/// </summary>
		public bool BillingAddressOnClaims;

		///<summary>Overrides practice PayTo address if not blank.</summary>
		public string PayToAddressLine1;

		///<summary>Second line of PayTo address.</summary>
		public string PayToAddressLine2;

		///<summary>Overrides practice PayToCity if PayToAddress is not blank.</summary>
		public string PayToCity;

		///<summary>Overrides practice PayToState if PayToAddress is not blank.</summary>
		public string PayToState;

		///<summary>Overrides practice PayToZip if PayToAddress is not blank.</summary>
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

		///<summary>Enum:PlaceOfService Usually 0 unless a mobile clinic for instance.</summary>
		public PlaceOfService DefaultPlaceService;

		/// <summary>0=Default practice provider, -1=Treating provider.</summary>
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
		public long DefaultProviderId;

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
		[ForeignKey(typeof(Def), nameof(Def.DefNum))]
		public long? Region;

		/// <summary>
		///		<para>
		///			The sort order of the clinic.
		///		</para>
		///		<para>
		///			Only applies when the <b>ClinicListIsAlphabetical</b> is false.
		///		</para>
		/// </summary>
		public int ItemOrder;



		/// <summary>
		/// Used to filter MedLab results by the MedLab Account Number assigned to each clinic.
		/// </summary>
		[ForeignKey(typeof(MedLab), nameof(MedLab.PatAccountNum))]
		public string MedlabAccountId;

		///<summary>Clinic level preference. (Better Name is "IsAutomationEnabled" but that conflicts with other definitions of what Automation means. 
		///Determines if automated reminder
		///s and confirmations should be sent for/from this clinic.</summary>
		public bool IsConfirmEnabled;

		/// <summary>Clinic level preference. If true then this clinic is using the default automated reminder/confirmation settings as defined by the user.</summary>
		public bool IsConfirmDefault;

		/// <summary>
		/// A value indicating whether the clinic is hidden.
		/// </summary>
		public bool IsHidden;

		/// <summary>
		/// Indicates if the clinic should only be scheduled in a certain way (e.g. ortho only, etc)
		/// </summary>
		public string SchedulingNote;

		///<summary>Defaults to false.  If true, will require procedure be attached to controlled prescriptions written from this clinic.</summary>
		public bool HasProcedureOnRx;

		///<summary>List of specialty DefLinks for the clinic.  Not a database column.  Filled when the clinic cache is filled.</summary>
		[Ignore]
		private List<DefLink> _listClinicSpecialtyDefLinks;

		///<summary>List of specialty DefLinks for the clinic.  Not a database column.  Filled when the clinic cache is filled.</summary>
		[XmlIgnore, JsonIgnore, Obsolete]
		public List<DefLink> ListClinicSpecialtyDefLinks
		{
			get
			{
				if (_listClinicSpecialtyDefLinks == null)
				{
					_listClinicSpecialtyDefLinks = new List<DefLink>();
					if (Id > 0)
					{
						_listClinicSpecialtyDefLinks = DefLinks.GetListByFKey(Id, DefLinkType.Clinic);
					}
				}
				return _listClinicSpecialtyDefLinks;
			}
			set
			{
				_listClinicSpecialtyDefLinks = value;
			}
		}

		///<summary>Returns a copy of this Clinic and the associated list of specialty DefLinks.</summary>
		public Clinic Copy()
		{
			Clinic retval = (Clinic)MemberwiseClone();
			retval.ListClinicSpecialtyDefLinks = new List<DefLink>(ListClinicSpecialtyDefLinks);
			return retval;
		}
	}
}
