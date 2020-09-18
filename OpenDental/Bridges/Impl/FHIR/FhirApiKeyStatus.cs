namespace Imedisoft.Bridges.Fhir
{
    /// <summary>
    /// Identifies the status of an FHIR API key.
    /// </summary>
    /// <seealso cref="FhirApiKey"/>
    public enum FhirApiKeyStatus
	{
		Enabled,
		EnabledReadOnly,

		DisabledByCustomer,
		DisabledByDeveloper,
		DisabledByHQ
	}
}
