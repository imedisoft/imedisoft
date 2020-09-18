using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Imedisoft.Bridges.Fhir
{
    public static class FhirClient
    {
        private static HttpClient Client =>
            new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001") // TODO: Should be updated to fetch server Uri from prefs...
            };

        public static async Task<List<FhirApiKey>> GetApiKeysAsync()
        {
            var apiKeysJson = await Client.GetStringAsync("/api/v1/fhir/keys");

            return JsonSerializer.Deserialize<List<FhirApiKey>>(apiKeysJson);
        }

        public static async Task Assign(string key)
        {
            var args = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("key", key),
                });

            await Client.PostAsync("/api/v1/fhir/keys", args);
        }

        public static async Task<FhirApiKey> Toggle(string key)
        {
            var args = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("key", key),
                });

            var apiKeyResponse = await Client.PostAsync("/api/v1/fhir/keys/toggle", args);
            var apiKeyJson = await apiKeyResponse.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<FhirApiKey>(apiKeyJson);
        }

        /// <summary>
        /// Gets a description of the specified <paramref name="status"/>.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>A description of the status.</returns>
        public static string GetDescription(FhirApiKeyStatus status)
        {
            return status switch
            {
                FhirApiKeyStatus.Enabled => Translation.Common.Enabled,
                FhirApiKeyStatus.EnabledReadOnly => Translation.Enums.FhirApiKeyStatusEnabledReadOnly,
                FhirApiKeyStatus.DisabledByCustomer => Translation.Common.Disabled,
                FhirApiKeyStatus.DisabledByDeveloper => Translation.Enums.FhirApiKeyStatusDisabledByDeveloper,
                FhirApiKeyStatus.DisabledByHQ => Translation.Enums.FhirApiKeyStatusDisabledByHQ,
                _ => status.ToString()
            };
        }

        /// <summary>
        /// Gets a description of the specified <paramref name="permission"/>.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns>A description of the permission.</returns>
        public static string GetDescription(FhirApiPermission permission)
        {
            return permission switch
            {
                FhirApiPermission.AppointmentCreate => Translation.Enums.FhirApiPermissionAppointmentCreate,
                FhirApiPermission.AppointmentRead => Translation.Enums.FhirApiPermissionAppointmentRead,
                FhirApiPermission.AppointmentUpdate => Translation.Enums.FhirApiPermissionAppointmentUpdate,
                FhirApiPermission.AppointmentDelete => Translation.Enums.FhirApiPermissionAppointmentDelete,
                FhirApiPermission.PatientCreate => Translation.Enums.FhirApiPermissionPatientCreate,
                FhirApiPermission.PatientRead => Translation.Enums.FhirApiPermissionPatientRead,
                FhirApiPermission.PatientUpdate => Translation.Enums.FhirApiPermissionPatientUpdate,
                FhirApiPermission.Subscriptions => Translation.Enums.FhirApiPermissionSubscriptions,
                FhirApiPermission.LocationRead => Translation.Enums.FhirApiPermissionLocationRead,
                FhirApiPermission.OrganizationRead => Translation.Enums.FhirApiPermissionOrganizationRead,
                FhirApiPermission.PractitionerRead => Translation.Enums.FhirApiPermissionPractitionerRead,
                FhirApiPermission.ScheduleRead => Translation.Enums.FhirApiPermissionScheduleRead,
                FhirApiPermission.CapabilityStatement => Translation.Enums.FhirApiPermissionCapabilityStatement,
                FhirApiPermission.AllergyIntoleranceRead => Translation.Enums.FhirApiPermissionAllergyIntoleranceRead,
                FhirApiPermission.MedicationRead => Translation.Enums.FhirApiPermissionMedicationRead,
                FhirApiPermission.ConditionRead => Translation.Enums.FhirApiPermissionConditionRead,
                FhirApiPermission.ServiceRequestRead => Translation.Enums.FhirApiPermissionServiceRequestRead,
                FhirApiPermission.ServiceRequestCreate => Translation.Enums.FhirApiPermissionServiceRequestCreate,
                FhirApiPermission.ProcedureRead => Translation.Enums.FhirApiPermissionProcedureRead,
                FhirApiPermission.ProcedureCreate => Translation.Enums.FhirApiPermissionProcedureCreate,
                FhirApiPermission.ProcedureUpdate => Translation.Enums.FhirApiPermissionProcedureUpdate,
                FhirApiPermission.PaymentRead => Translation.Enums.FhirApiPermissionPaymentRead,
                FhirApiPermission.PaymentCreate => Translation.Enums.FhirApiPermissionPaymentCreate,
                FhirApiPermission.CommunicationRead => Translation.Enums.FhirApiPermissionCommunicationRead,
                FhirApiPermission.CommunicationCreate => Translation.Enums.FhirApiPermissionCommunicationCreate,
                _ => permission.ToString(),
            };
        }
    }
}
