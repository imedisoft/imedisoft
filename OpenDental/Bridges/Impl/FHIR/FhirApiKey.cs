using System;
using System.Text.Json.Serialization;

namespace Imedisoft.Bridges.Fhir
{
    public class FhirApiKey
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("status")]
        public FhirApiKeyStatus Status { get; set; }

        [JsonPropertyName("permissions")]
        public FhirApiPermission[] Permissions { get; set; }

        [JsonPropertyName("developerName")]
        public string DeveloperName { get; set; }

        [JsonPropertyName("developerEmail")]
        public string DeveloperEmail { get; set; }

        [JsonPropertyName("developerPhone")]
        public string DeveloperPhone { get; set; }

        [JsonPropertyName("disabledOn")]
        public DateTime? DisabledOn { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets a value indicating whether the key was disabled by the customer.
        /// </summary>
        public bool IsDisabledByCustomer
            => Status == FhirApiKeyStatus.DisabledByCustomer;
    }
}
