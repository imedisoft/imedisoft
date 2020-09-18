using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenDentBusiness
{
    public partial class ProgramProperties
	{
		[CacheGroup(nameof(InvalidType.Programs))]
		private class ProgramPropertyCache : ListCache<ProgramProperty>
		{
            protected override IEnumerable<ProgramProperty> Load()
				=> SelectMany("SELECT * FROM `program_properties`");
		}

		private static readonly ProgramPropertyCache cache = new ProgramPropertyCache();

		public static ProgramProperty GetFirstOrDefault(Predicate<ProgramProperty> predicate) 
			=> cache.FirstOrDefault(predicate);

		public static List<ProgramProperty> GetWhere(Predicate<ProgramProperty> predicate) 
			=> cache.Find(predicate);

		public static void RefreshCache()
			=> cache.Refresh();

		public static bool UpdateProgramPropertyWithValue(ProgramProperty programProperty, string newValue)
		{
			if (programProperty.Value == newValue)
			{
				return false;
			}

			programProperty.Value = newValue;

			ExecuteUpdate(programProperty);

			return true;
		}

		public static void Save(ProgramProperty programProperty)
        {
			if (programProperty.Id == 0) ExecuteInsert(programProperty);
            else
            {
				ExecuteUpdate(programProperty);
            }
        }

		/// <summary>
		/// Copies rows for a given programNum for each clinic in listClinicNums.
		/// Returns true if changes were made to the db.
		/// </summary>
		public static bool InsertForClinic(long programId, List<long> clinicIds)
		{
			if (clinicIds == null || clinicIds.Count == 0)
			{
				return false;
			}

			var command = "INSERT INTO `program_properties` (`program_id`, `description`, `value`, `machine_name`, `clinic_id`) " + 
				string.Join(" UNION ", clinicIds.Select(clinicId =>
					"SELECT `id`, `description`, `value`, `machine_name`, " + clinicId + " " +
					"FROM `program_properties` WHERE `id` = " + programId + " AND `clinic_id` IS NULL"));

            return Database.ExecuteInsert(command) > 0;
		}

		/// <summary>
		/// Safe to call on any program. Only returns true if the program is not enabled 
		/// AND the program has a property of "Disable Advertising" = 1 OR "Disable Advertising HQ" = 1.
		/// This means that either the office has disabled the ad or HQ has disabled the ad.
		/// </summary>
		public static bool IsAdvertisingDisabled(ProgramName programName) 
			=> IsAdvertisingDisabled(Programs.GetCur(programName));

		/// <summary>
		/// Safe to call on any program. Only returns true if the program is not enabled 
		/// AND the program has a property of "Disable Advertising" = 1 OR "Disable Advertising HQ" = 1.
		/// This means that either the office has disabled the ad or HQ has disabled the ad.
		/// </summary>
		public static bool IsAdvertisingDisabled(Program program)
		{
			if (program == null || program.Enabled)
			{
				return false; // do not block advertising
			}

			return GetForProgram(program.Id).Any(x => 
				(x.Description == "Disable Advertising" && x.Value == "1") || //Office has decided to hide the advertising
				(x.Description == "Disable Advertising HQ" && x.Value == "1"));//HQ has decided to hide the advertising
		}

		/// <summary>
		/// Gets the value of the program property with the specified name.
		/// </summary>
		/// <param name="programId">The ID of the program.</param>
		/// <param name="programPropertyName">The name of the property.</param>
		/// <param name="clinicId">The ID of the clinic.</param>
		/// <param name="machineName">The name of the machine.</param>
		/// <returns>The value of the property or a empty string if the property is not set.</returns>
		public static string Get(long programId, string programPropertyName, long? clinicId, string machineName)
        {
			var programProperties = cache.Find(prop => prop.ProgramId == programId && prop.Description == programPropertyName.ToLower());
			var programProperty = programProperties.FirstOrDefault(prop =>
				prop.ClinicId == 0 && prop.MachineName == "");

			if (clinicId.HasValue)
			{
				// Is there a clinic specific property with a machine override?
				programProperty = programProperties.FirstOrDefault(prop =>
					prop.ClinicId == clinicId &&
					prop.MachineName.Equals(machineName, StringComparison.InvariantCultureIgnoreCase));

				// Is there is a clinic specific property?
				programProperty ??= programProperties.FirstOrDefault(prop =>
					prop.ClinicId == clinicId);
			}

			// Is there a global machine override?
			programProperty ??= programProperties.FirstOrDefault(prop => 
				prop.ClinicId == 0 &&
				prop.MachineName.Equals(machineName, StringComparison.InvariantCultureIgnoreCase));

			// Return the value of the highest priority property or a blank value if the property hasn't been configured.
			return programProperty?.Value ?? "";
        }

		/// <summary>
		/// Gets the value of the program property with the specified name.
		/// </summary>
		/// <param name="programId"></param>
		/// <param name="programPropertyName"></param>
		/// <param name="clinicId"></param>
		/// <returns></returns>
		public static string Get(long programId, string programPropertyName, long? clinicId = null)
			=> Get(programId, programPropertyName, clinicId, Environment.MachineName);







		/// <summary>
		/// True if this is a program that we advertise.
		/// </summary>
		public static bool IsAdvertisingBridge(long programId) 
			=> GetForProgram(programId).Any(x => x.Description.In("Disable Advertising", "Disable Advertising HQ"));

		/// <summary>
		/// Returns a list of ProgramProperties with the specified programNum and the specified clinicNum from the cache.
		/// To get properties when clinics are not enabled or properties for 'Headquarters' use clinicNum 0.
		/// Does not include path overrides.
		/// </summary>
		public static List<ProgramProperty> GetListForProgramAndClinic(long programId, long clinicId) 
			=> GetWhere(x => x.ProgramId == programId && x.ClinicId == clinicId && x.Description != "");

		/// <summary>
		/// Returns a List of ProgramProperties attached to the specified programNum with the given clinicnum.  
		/// Includes the default program properties as well (ClinicNum==0).
		/// </summary>
		public static List<ProgramProperty> GetListForProgramAndClinicWithDefault(long programId, long clinicId)
		{
			var clinicProperties = GetWhere(x => x.ProgramId == programId && x.ClinicId == clinicId);
			if (clinicId == 0)
			{
				return clinicProperties; // return the defaults cause ClinicNum of 0 is default.
			}

			// Get all the defaults and return a list of defaults mixed with overrides.
			List<ProgramProperty> listClinicAndDefaultProperties = GetWhere(x => x.ProgramId == programId && x.ClinicId == 0
				  && !clinicProperties.Any(y => y.Description == x.Description));
			listClinicAndDefaultProperties.AddRange(clinicProperties);
			return listClinicAndDefaultProperties;//Clinic users need to have all properties, defaults with the clinic overrides.
		}

		/// <summary>
		/// Returns the property value of the clinic override or default program property if no clinic override is found.
		/// </summary>
		public static string GetPropValForClinicOrDefault(long programNum, string desc, long clinicNum) 
			=> GetListForProgramAndClinicWithDefault(programNum, clinicNum).FirstOrDefault(x => x.Description == desc).Value;

		/// <summary>
		/// Returns a list of ProgramProperties attached to the specified programNum.
		/// Does not include path overrides.
		/// Uses thread-safe caching pattern. 
		/// Each call to this method creates an copy of the entire ProgramProperty cache.
		/// </summary>
		public static List<ProgramProperty> GetForProgram(long programNum) 
			=> GetWhere(x => x.ProgramId == programNum && x.Description != "")
				.OrderBy(x => x.ClinicId)
				.ThenBy(x => x.Id)
				.ToList();

		/// <summary>
		/// Sets the program property for all clinics. Returns the number of rows changed.
		/// </summary>
		public static long SetProperty(long programId, string desc, string propval) 
			=> Database.ExecuteNonQuery(
				$"UPDATE `program_properties` SET `value` = '{POut.String(propval)}' " +
				$"WHERE `program_id` = {programId} AND `description` = '{POut.String(desc)}'");

		/// <summary>
		/// After GetForProgram has been run, this gets one of those properties. DO NOT MODIFY the returned property. Read only.
		/// </summary>
		public static ProgramProperty GetCur(List<ProgramProperty> programProperties, string propertyDescription) 
			=> programProperties.FirstOrDefault(x => x.Description == propertyDescription);

		/// <summary>
		/// Throws exception if program property is not found.
		/// </summary>
		public static string GetPropVal(long programNum, string desc) 
			=> GetFirstOrDefault(x => x.ProgramId == programNum && x.Description == desc)?.Value 
				?? throw new ApplicationException("Property not found: " + desc);

		public static string GetPropVal(ProgramName programName, string desc) 
			=> GetPropVal(Programs.GetProgramNum(programName), desc);

		/// <summary>
		/// Returns the PropertyVal for programNum and clinicNum specified with the description specified. 
		/// If the property doesn't exist, returns an empty string. 
		/// For the PropertyVal for 'Headquarters' or clincs not enabled, use clinicNum 0.
		/// </summary>
		public static string GetPropVal(long programId, string propertyDesc, long clinicId) 
			=> GetPropValFromList(GetWhere(x => x.ProgramId == programId), propertyDesc, clinicId);

		/// <summary>
		/// Returns the PropertyVal from the list by PropertyDesc and ClinicNum.
		/// For the 'Headquarters' or for clinics not enabled, omit clinicNum or send clinicNum 0.  If not found returns an empty string.
		/// Primarily used when a local list has been copied from the cache and may differ from what's in the database. 
		/// Also possibly useful if dealing with a filtered list
		/// </summary>
		public static string GetPropValFromList(List<ProgramProperty> properties, string propertyDesc, long clinicId = 0) 
			=> properties
				.Where(x => x.ClinicId == clinicId && x.Description == propertyDesc)
				.FirstOrDefault()?.Value ?? "";

		/// <summary>
		/// Returns the property with the matching description from the provided list. 
		/// Null if the property cannot be found by the description.
		/// </summary>
		public static ProgramProperty GetPropByDesc(string propertyDesc, List<ProgramProperty> properties)
		{
			for (int i = 0; i < properties.Count; i++)
			{
				if (properties[i].Description == propertyDesc)
				{
					return properties[i];
				}
			}

			return null;
		}

		/// <summary>
		/// Returns the property with the matching description from the provided list.
		/// Null if the property cannot be found by the description.
		/// </summary>
		public static ProgramProperty GetPropForProgByDesc(long programNum, string propertyDesc) 
			=> GetForProgram(programNum).FirstOrDefault(x => x.Description == propertyDesc);

		/// <summary>
		/// Returns the property with the matching description from the provided list.
		/// Null if the property cannot be found by the description.
		/// </summary>
		public static ProgramProperty GetPropForProgByDesc(long programNum, string propertyDesc, long clinicNum = 0) 
			=> GetForProgram(programNum).FirstOrDefault(x => x.Description == propertyDesc && x.ClinicId == clinicNum);

		/// <summary>
		/// Used in FormUAppoint to get frequent and current data.
		/// </summary>
		public static string GetValFromDb(long programNum, string desc) 
			=> Database.ExecuteString(
				"SELECT `value` FROM `program_properties` " +
				"WHERE `program_id` = " + programNum + " AND `description` ='" + POut.String(desc) + "'");

		/// <summary>
		/// Returns the path override for the current computer and the specified programNum. 
		/// Returns empty string if no override found.
		/// </summary>
		public static string GetLocalPathOverrideForProgram(long programNum) 
			=> GetFirstOrDefault(x => 
				x.ProgramId == programNum && 
				x.Description == "" && 
				x.MachineName.ToUpper() == Environment.MachineName.ToUpper())?.Value ?? "";
		

		/// <summary>
		/// This will insert or update a local path override property for the specified programNum.
		/// </summary>
		public static void InsertOrUpdateLocalOverridePath(long programId, string newPath)
		{
			var programProperty = 
				GetFirstOrDefault(x => 
					x.ProgramId == programId && 
					x.Description == "" && 
					x.MachineName.ToUpper() == Environment.MachineName.ToUpper());

			if (programProperty != null)
			{
				programProperty.Value = newPath;

				ExecuteUpdate(programProperty);

				return; // Will only be one override per computer per program.
			}

            ExecuteInsert(new ProgramProperty
			{
				ProgramId = programId,
				Value = newPath,

				MachineName = Environment.MachineName.ToUpper()
			});
		}

		/// <summary>
		/// Syncs list against cache copy of program properties. listProgPropsNew should never include local path overrides (PropertyDesc=="").
		/// This sync uses the cache copy of program properties rather than a stale list because we want to make sure we never have
		/// duplicate properties and concurrency isn't really an issue.
		/// </summary>
		public static bool Sync(List<ProgramProperty> newProperties, long programId)
		{
			Database.ExecuteNonQuery("DELETE FROM `program_properties` WHERE `program_id` = " + programId);

			foreach (var property in newProperties)
            {
				property.ProgramId = programId;

				ExecuteInsert(property);
            }

			cache.Refresh();

			return true;
		}

		/// <summary>
		/// Syncs list against cache copy of program properties. 
		/// listProgPropsNew should never include local path overrides (PropertyDesc=="").
		/// This sync uses the cache copy of program properties rather than a stale list because we want to make sure we never have duplicate properties and concurrency isn't really an issue. 
		/// This WILL delete program properties from the database if missing from listProgPropsNew for the specified clinics.
		/// Only include clinics to which the current user is allowed access.
		/// </summary>
		public static void Sync(List<ProgramProperty> newProperties, long programId, List<long> clinicsIds)
		{
			var existingProperties = GetWhere(x => 
				x.ProgramId == programId && 
				x.Description != "" && 
				clinicsIds.Contains(x.ClinicId));

			Database.ExecuteNonQuery("DELETE FROM `program_properties` WHERE `program_id` = " + programId + " AND `clinic_id` IN (" + string.Join(", ", clinicsIds) + ")");

			foreach (var property in newProperties)
			{
				property.ProgramId = programId;

				ExecuteInsert(property);
			}

			cache.Refresh();
		}

		/// <summary>
		/// Exception means failed. Return means success. 
		/// paymentsAllowed should be check after return. 
		/// If false then assume payments cannot be made for this clinic.
		/// </summary>
		public static void GetXWebCreds(long clinicId, out WebTypes.Shared.XWeb.WebPaymentProperties xwebProperties)
		{
			string xWebID;
			string authKey;
			string terminalID;

            var program = Programs.GetCur(ProgramName.Xcharge);
			if (program == null)
			{ //XCharge not setup.
				throw new ODException("X-Charge program link not found.", 
					ODException.ErrorCodes.XWebProgramProperties);
			}

			if (!program.Enabled)
			{ //XCharge not turned on.
				throw new ODException("X-Charge program link is disabled.", 
					ODException.ErrorCodes.XWebProgramProperties);
			}

			// Validate ALL XWebID, AuthKey, and TerminalID. Each is required for X-Web to work.
			var properties = GetListForProgramAndClinic(program.Id, clinicId);
			xWebID = GetPropValFromList(properties, "XWebID", clinicId);
			authKey = GetPropValFromList(properties, "AuthKey", clinicId);
			terminalID = GetPropValFromList(properties, "TerminalID", clinicId);
			string paymentTypeDefString = GetPropValFromList(properties, "PaymentType", clinicId);
			if (string.IsNullOrEmpty(xWebID) || string.IsNullOrEmpty(authKey) || string.IsNullOrEmpty(terminalID) || !long.TryParse(paymentTypeDefString, out long paymentTypeDefNum))
			{
				throw new ODException("X-Web program properties not found.", 
					ODException.ErrorCodes.XWebProgramProperties);
			}

			// XWeb ID must be 12 digits, Auth Key 32 alphanumeric characters, and Terminal ID 8 digits.
			if (!Regex.IsMatch(xWebID, "^[0-9]{12}$") || !Regex.IsMatch(authKey, "^[A-Za-z0-9]{32}$") || !Regex.IsMatch(terminalID, "^[0-9]{8}$"))
			{
				throw new ODException("X-Web program properties not valid.",
					ODException.ErrorCodes.XWebProgramProperties);
			}

			string isPaymentsAllowed = GetPropValFromList(properties, "IsOnlinePaymentsEnabled", clinicId);

            xwebProperties = new WebTypes.Shared.XWeb.WebPaymentProperties
            {
                XWebID = xWebID,
                TerminalID = terminalID,
                AuthKey = authKey,
                PaymentTypeDefNum = paymentTypeDefNum,
                IsPaymentsAllowed = PIn.Bool(isPaymentsAllowed)
            };
        }

		/// <summary>
		/// Exception means failed. Return means success.
		/// paymentsAllowed should be check after return.
		/// If false then assume payments cannot be made for this clinic.
		/// </summary>
		public static void GetPayConnectPatPortalCreds(long clinicNum, out PayConnect.WebPaymentProperties payConnectProps)
		{
			var program = Programs.GetCur(ProgramName.PayConnect);
			if (program == null)
			{
				throw new ODException("PayConnect program link not found.",
					ODException.ErrorCodes.PayConnectProgramProperties);
			}

			if (!program.Enabled)
			{
				throw new ODException("PayConnect program link is disabled.", 
					ODException.ErrorCodes.PayConnectProgramProperties);
			}

			var properties = GetListForProgramAndClinic(program.Id, clinicNum);
			var token = GetPropValFromList(properties, PayConnect.ProgramProperties.PatientPortalPaymentsToken, clinicNum);
			if (string.IsNullOrEmpty(token))
			{
				throw new ODException("PayConnect online token not found.", 
					ODException.ErrorCodes.PayConnectProgramProperties);
			}

			string isPaymentsAllowed = GetPropValFromList(properties, PayConnect.ProgramProperties.PatientPortalPaymentsEnabled, clinicNum);

            payConnectProps = new PayConnect.WebPaymentProperties
            {
                Token = token,
                IsPaymentsAllowed = PIn.Bool(isPaymentsAllowed)
            };
        }

		/// <summary>
		/// Deletes a given programproperty from the table based upon its programPropertyNum.
		/// Must have a property description in the GetDeletablePropertyDescriptions() list to delete
		/// </summary>
		public static void Delete(ProgramProperty programProperty)
		{
			if (!GetDeletablePropertyDescriptions().Contains(programProperty.Description))
			{
				throw new Exception("Not allowed to delete the program property with description '" + programProperty.Description + "'.");
			}

			ExecuteDelete(programProperty);
		}

		/// <summary>
		/// Deleting from the ProgramProperty table should be considered dangerous and extremely deliberate, anyone looking to do so must
		/// explicitly add their condition to this method in the future.
		/// </summary>
		private static List<string> GetDeletablePropertyDescriptions()
		{
			return new List<string>() {
				PropertyDescs.ClinicHideButton,
			};
		}

		public static class PropertyDescs
		{
			public const string ImageFolder = "Image Folder";
			public const string PatOrChartNum = "Enter 0 to use PatientNum, or 1 to use ChartNum";
			public const string Username = "Username";
			public const string Password = "Password";
			public const string ClinicHideButton = "ClinicHideButton";

			public static class TransWorld
			{
				public const string SyncExcludePosAdjType = "SyncExcludePosAdjType";
				public const string SyncExcludeNegAdjType = "SyncExcludeNegAdjType";
			}

			public static class XCharge
			{
				public const string XChargeForceRecurringCharge = "XChargeForceRecurringCharge";
				public const string XChargePreventSavingNewCC = "XChargePreventSavingNewCC";
			}
		}
	}

	/// <summary>
	/// Container for the names of frequently used program properties.
	/// </summary>
	public static class ProgramPropertyName
    {
		public const string DisableAdvertising = "Disable Advertising";
		public const string DisableAdvertisingHq = "Disable Advertising HQ";

		public const string ProgramPathOverride = "program_path_override";

		public const string PatientIdentificationType = "identification_type";

	}
}
