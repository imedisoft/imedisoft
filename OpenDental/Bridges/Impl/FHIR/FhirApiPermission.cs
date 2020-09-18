namespace Imedisoft.Bridges.Fhir
{
    /// <summary>
    /// Identifies permissions for the FHIR API. Can be granted to FHIR API keys.
    /// </summary>
    public enum FhirApiPermission
	{
		AppointmentCreate,
		AppointmentRead,
		AppointmentUpdate,
		AppointmentDelete,
		PatientCreate,
		PatientRead,
		PatientUpdate,
		Subscriptions,
		LocationRead,
		OrganizationRead,
		PractitionerRead,
		ScheduleRead,
		CapabilityStatement,
		AllergyIntoleranceRead,
		MedicationRead,
		ConditionRead,
		ServiceRequestRead,
		ServiceRequestCreate,
		ProcedureRead,
		ProcedureCreate,
		ProcedureUpdate,
		PaymentRead,
		PaymentCreate,
		CommunicationRead,
		CommunicationCreate,
	}
}
