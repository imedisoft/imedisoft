using System.Collections.Generic;

namespace Imedisoft.X12.Codes
{
    /// <summary>
    ///		<para>
    ///			Place of Service Codes are two-digit codes placed on health care professional 
    ///			claims to indicate the setting in which a service was provided.
    ///		</para>
    /// </summary>
    /// <seealso href="https://www.cms.gov/Medicare/Coding/place-of-service-codes"/>
    /// <seealso href="https://www.cms.gov/Medicare/Coding/place-of-service-codes/Place_of_Service_Code_Set"/>
    /// <seealso href="https://www.cms.gov/Medicare/Medicare-Fee-for-Service-Payment/PhysicianFeeSched/Downloads/Website-POS-database.pdf"/>
    public static class PlaceOfService
	{
		/// <summary>
		/// The location where health services and health related services are provided or 
		/// received, through a telecommunication system. 
		/// </summary>
		public const string Telehealth = "02";

		/// <summary>
		/// A facility whose primary purpose is education.
		/// </summary>
		public const string School = "03";

		/// <summary>
		/// Location, other than a hospital, skilled nursing facility (SNF), military treatment 
		/// facility, community health center, State or local public health clinic, or intermediate
		/// care facility (ICF), where the health professional routinely provides health 
		/// examinations, diagnosis, and treatment of illness or injury on an ambulatory basis.
		/// </summary>
		public const string Office = "11";

		/// <summary>
		/// Location, other than a hospital or other facility, where the patient receives care in a
		/// private residence.
		/// </summary>
		public const string Home = "12";

		/// <summary>
		/// A facility/unit that moves from place-to-place equipped to provide preventive, 
		/// screening, diagnostic, and/or treatment services.
		/// </summary>
		public const string MobileUnit = "15";

		/// <summary>
		/// A facility, other than psychiatric, which primarily provides diagnostic, therapeutic 
		/// (both surgical and nonsurgical), and rehabilitation services by, or under, the 
		/// supervision of physicians to patients admitted for a variety of medical conditions.
		/// </summary>
		public const string InpatientHospital = "21";

		/// <summary>
		/// A portion of a hospital’s main campus which provides diagnostic, therapeutic (both 
		/// surgical and nonsurgical), and rehabilitation services to sick or injured persons who 
		/// do not require hospitalization or institutionalization.
		/// </summary>
		public const string OnCampusOutpatientHospital = "22";

		/// <summary>
		/// A portion of a hospital where emergency diagnosis and treatment of illness or injury is
		/// provided.
		/// </summary>
		public const string EmergencyRoomHospital = "23";

		/// <summary>
		/// A freestanding facility, other than a physician's office, where surgical and diagnostic
		/// services are provided on an ambulatory basis.
		/// </summary>
		public const string AmbulatorySurgicalCenter = "24";

		/// <summary>
		/// A medical facility operated by one or more of the Uniformed Services. Military 
		/// Treatment Facility (MTF) also refers to certain former U.S. Public Health 
		/// Service (USPHS) facilities now designated as Uniformed Service Treatment Facilities (USTF).
		/// </summary>
		public const string MilitaryTreatmentFacility = "26";

		/// <summary>
		/// A facility which primarily provides inpatient skilled nursing care and related services
		/// to patients who require medical, nursing, or rehabilitative services but does not 
		/// provide the level of care or treatment available in a hospital.
		/// </summary>
		public const string SkilledNursingFacility = "31"; // SkilledNursingFacility

		/// <summary>
		/// A facility which provides room, board and other personal assistance services, generally
		/// on a long-term basis, and which does not include a medical component.
		/// </summary>
		public const string CustodialCareFacility = "33";

		/// <summary>
		/// A facility located in a medically underserved area that provides Medicare beneficiaries
		/// preventive primary medical care under the general direction of a physician.
		/// </summary>
		public const string FederallyQualifiedHealthCenter = "50";

		/// <summary>
		/// A facility maintained by either State or local health departments that provides
		/// ambulatory primary medical care under the general direction of a physician
		/// </summary>
		public const string PublicHealthClinic = "71";

		/// <summary>
		/// Other place of service not identified above.
		/// </summary>
		public const string OtherPlaceOfService = "99";

		/// <summary>
		/// Enumerates all available codes in this set.
		/// </summary>
		public static IEnumerable<(string code, string description)> Codes
        {
            get
            {
				yield return (Telehealth, Translation.X12.PlaceOfServiceTelehealth);
				yield return (School, Translation.X12.PlaceOfServiceSchool);
				yield return (Office, Translation.X12.PlaceOfServiceOffice);
				yield return (Home, Translation.X12.PlaceOfServiceHome);
				yield return (MobileUnit, Translation.X12.PlaceOfServiceMobileUnit);
				yield return (InpatientHospital, Translation.X12.PlaceOfServiceInpatientHospital);
				yield return (OnCampusOutpatientHospital, Translation.X12.PlaceOfServiceOnCampusOutpatientHospital);
				yield return (EmergencyRoomHospital, Translation.X12.PlaceOfServiceEmergencyRoomHospital);
				yield return (AmbulatorySurgicalCenter, Translation.X12.PlaceOfServiceAmbulatorySurgicalCenter);
				yield return (MilitaryTreatmentFacility, Translation.X12.PlaceOfServiceMilitaryTreatmentFacility);
				yield return (SkilledNursingFacility, Translation.X12.PlaceOfServiceSkilledNursingFacility);
				yield return (CustodialCareFacility, Translation.X12.PlaceOfServiceCustodialCareFacility);
				yield return (FederallyQualifiedHealthCenter, Translation.X12.PlaceOfServiceFederallyQualifiedHealthCenter);
				yield return (PublicHealthClinic, Translation.X12.PlaceOfServicePublicHealthClinic);
				yield return (OtherPlaceOfService, Translation.X12.PlaceOfServiceOtherPlaceOfService);
			}
        }

		/// <summary>
		/// Gets the description of the specified <paramref name="code"/>.
		/// </summary>
		/// <param name="code">The place of service code.</param>
		/// <returns>A description of the code.</returns>
		public static string GetDescription(string code)
        {
			return code switch
			{
				Telehealth => Translation.X12.PlaceOfServiceTelehealth,
				School => Translation.X12.PlaceOfServiceSchool,
				Office => Translation.X12.PlaceOfServiceOffice,
				Home => Translation.X12.PlaceOfServiceHome,
				MobileUnit => Translation.X12.PlaceOfServiceMobileUnit,
				InpatientHospital => Translation.X12.PlaceOfServiceInpatientHospital,
				OnCampusOutpatientHospital => Translation.X12.PlaceOfServiceOnCampusOutpatientHospital,
				EmergencyRoomHospital => Translation.X12.PlaceOfServiceEmergencyRoomHospital,
				AmbulatorySurgicalCenter => Translation.X12.PlaceOfServiceAmbulatorySurgicalCenter,
				MilitaryTreatmentFacility => Translation.X12.PlaceOfServiceMilitaryTreatmentFacility,
				SkilledNursingFacility => Translation.X12.PlaceOfServiceSkilledNursingFacility,
				CustodialCareFacility => Translation.X12.PlaceOfServiceCustodialCareFacility,
				FederallyQualifiedHealthCenter => Translation.X12.PlaceOfServiceFederallyQualifiedHealthCenter,
				PublicHealthClinic => Translation.X12.PlaceOfServicePublicHealthClinic,
				OtherPlaceOfService => Translation.X12.PlaceOfServiceOtherPlaceOfService,
				_ => Translation.Common.Unknown
			};
        }
	}
}
