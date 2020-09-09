using CodeBase;
using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public class Cache
	{
		/// <summary>
		/// Called directly from UI in one spot.
		/// Called from above repeatedly.
		/// The end result is that both server and client have been properly refreshed.
		/// </summary>
		public static void Refresh(params InvalidType[] invalidTypes)
		{
			GetCacheDs(invalidTypes);
		}

		private static void GetCacheDs(params InvalidType[] invalidTypes)
		{
			string suffix = "Refreshing Caches: ";

            // TODO: Logger.LogToPath("", LogPath.Signals, LogPhase.Start, "InvalidType(s): " + string.Join(" - ", arrayITypes.OrderBy(x => x.ToString())));

			bool refreshAll = false;
			if (invalidTypes.Contains(InvalidType.AllLocal))
			{
				refreshAll = true;
			}


			if (invalidTypes.Contains(InvalidType.AccountingAutoPays) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.AccountingAutoPays.ToString());
				AccountingAutoPays.RefreshCache();
			}

			if (invalidTypes.Contains(InvalidType.AlertCategories) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.AlertCategories.ToString());
				AlertCategories.RefreshCache();
			}

			if (invalidTypes.Contains(InvalidType.AlertCategoryLinks) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.AlertCategoryLinks.ToString());
				AlertCategoryLinks.RefreshCache();
			}

			if (invalidTypes.Contains(InvalidType.AppointmentTypes) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.AppointmentTypes.ToString());
				AppointmentTypes.GetTableFromCache(true);
			}

			if (invalidTypes.Contains(InvalidType.AutoCodes) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.AutoCodes.ToString());
				AutoCodes.RefreshCache();
				AutoCodeItems.RefreshCache();
				AutoCodeConds.RefreshCache();
			}

			if (refreshAll || invalidTypes.Contains(InvalidType.Automation))
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Automation.ToString());
				Automations.RefreshCache();
			}

			if (invalidTypes.Contains(InvalidType.AutoNotes) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.AutoNotes.ToString());
				AutoNotes.GetTableFromCache(true);
				AutoNoteControls.GetTableFromCache(true);
			}

			if (invalidTypes.Contains(InvalidType.Carriers) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Carriers.ToString());
				Carriers.GetTableFromCache(true); // run on startup, after telephone reformat, after list edit.
			}

			if (invalidTypes.Contains(InvalidType.ClaimForms) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.ClaimForms.ToString());
				ClaimFormItems.GetTableFromCache(true);
				ClaimForms.GetTableFromCache(true);
			}

			if (invalidTypes.Contains(InvalidType.ClearHouses) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.ClearHouses.ToString());
				Clearinghouses.GetTableFromCache(true);
			}

			if (invalidTypes.Contains(InvalidType.ClinicErxs) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.ClinicErxs.ToString());
				ClinicErxs.GetTableFromCache(true);
			}

			if (invalidTypes.Contains(InvalidType.ClinicPrefs) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.ClinicPrefs.ToString());
				ClinicPrefs.RefreshCache();
			}

			//InvalidType.Clinics see InvalidType.Providers
			if (invalidTypes.Contains(InvalidType.Computers) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Computers.ToString());
				Computers.RefreshCache();
				Printers.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Defs) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Defs.ToString());
				Definitions.RefreshCache();
			}
			if (invalidTypes.Contains(InvalidType.DentalSchools) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.DentalSchools.ToString());
				SchoolClasses.RefreshCache();
				SchoolCourses.RefreshCache();
			}
			if (invalidTypes.Contains(InvalidType.DictCustoms) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.DictCustoms.ToString());
				DictCustoms.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Diseases) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Diseases.ToString());
				ProblemDefinitions.RefreshCache();
				Icd9s.RefreshCache();
			}
			if (invalidTypes.Contains(InvalidType.DisplayFields) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.DisplayFields.ToString());
				DisplayFields.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.DisplayReports) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.DisplayReports.ToString());
				DisplayReports.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Ebills) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Ebills.ToString());
				Ebills.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.EhrCodes))
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.EhrCodes.ToString());
				EhrCodes.UpdateList();//Unusual pattern for an unusual "table".  Not really a table, but a mishmash of hard coded partial code systems that are needed for CQMs.
			}
			if (invalidTypes.Contains(InvalidType.ElectIDs) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.ElectIDs.ToString());
				ElectIDs.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Email) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Email.ToString());
				EmailAddresses.GetTableFromCache(true);
				EmailTemplates.GetTableFromCache(true);
				EmailAutographs.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Employees) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Employees.ToString());
				Employees.RefreshCache();
				PayPeriods.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Employers) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Employers.ToString());
				Employers.RefreshCache();
			}
			if (invalidTypes.Contains(InvalidType.FeeScheds) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.FeeScheds.ToString());
				FeeScheds.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.HL7Defs) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.HL7Defs.ToString());
				HL7Defs.GetTableFromCache(true);
				HL7DefMessages.GetTableFromCache(true);
				HL7DefSegments.GetTableFromCache(true);
				HL7DefFields.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.InsCats) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.InsCats.ToString());
				CovCats.GetTableFromCache(true);
				CovSpans.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.InsFilingCodes) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.InsFilingCodes.ToString());
				InsFilingCodes.GetTableFromCache(true);
				InsFilingCodeSubtypes.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Letters) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Letters.ToString());
				Letters.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.LetterMerge) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.LetterMerge.ToString());
				LetterMergeFields.GetTableFromCache(true);
				LetterMerges.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Medications) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Medications.ToString());
				Medications.RefreshCache();
			}
			if (invalidTypes.Contains(InvalidType.Operatories) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Operatories.ToString());
				Operatories.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.OrthoChartTabs) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.OrthoChartTabs.ToString());
				OrthoChartTabs.GetTableFromCache(true);
				OrthoChartTabLinks.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.CustomFields) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.CustomFields.ToString());
				PatFieldDefs.GetTableFromCache(true);
				AppointmentFieldDefinitions.RefreshCache();
			}
			if (invalidTypes.Contains(InvalidType.Pharmacies) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Pharmacies.ToString());
				Pharmacies.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Prefs) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Prefs.ToString());
				Prefs.RefreshCache();
			}
			if (invalidTypes.Contains(InvalidType.ProcButtons) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.ProcButtons.ToString());
				ProcButtons.GetTableFromCache(true);
				ProcButtonItems.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.ProcMultiVisits) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.ProcMultiVisits.ToString());
				ProcMultiVisits.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.ProcCodes) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.ProcCodes.ToString());
				ProcedureCodes.GetTableFromCache(true);
				ProcCodeNotes.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Programs) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Programs.ToString());
				Programs.RefreshCache();
				ProgramProperties.RefreshCache();
			}
			if (invalidTypes.Contains(InvalidType.ProviderErxs) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.ProviderErxs.ToString());
				ProviderErxs.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.ProviderClinicLink) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.ProviderClinicLink.ToString());
				ProviderClinicLinks.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.ProviderIdents) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.ProviderIdents.ToString());
				ProviderIdents.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Providers) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Providers.ToString());
				Providers.RefreshCache(); ;
				//Refresh the clinics as well because InvalidType.Providers has a comment that says "also includes clinics".  Also, there currently isn't an itype for Clinics.
				Clinics.RefreshCache();
			}
			if (invalidTypes.Contains(InvalidType.QuickPaste) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.QuickPaste.ToString());
				QuickPasteNotes.GetTableFromCache(true);
				QuickPasteCats.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.RecallTypes) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.RecallTypes.ToString());
				RecallTypes.GetTableFromCache(true);
				RecallTriggers.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Referral) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Referral.ToString());
				Referrals.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.ReplicationServers) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.ReplicationServers.ToString());
				ReplicationServers.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.RequiredFields) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.RequiredFields.ToString());
				RequiredFields.GetTableFromCache(true);
				RequiredFieldConditions.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Security) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Security.ToString());
				//There is a chance that some future engineer will introduce a signal that tells another workstation to refresh the users when it shouldn't.
				//It is completely safe to skip over getting the user cache when IsCacheAllowed is false because the setter for that boolean nulls the cache.
				//This means that the cache will refill itself automatically the next time it is accessed as soon as the boolean flips back to true.
                Userods.RefreshCache();
				UserGroups.RefreshCache();

				GroupPermissions.RefreshCache();
				UserGroupAttaches.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Sheets) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Sheets.ToString());
				SheetDefs.GetTableFromCache(true);
				SheetFieldDefs.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.SigMessages) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.SigMessages.ToString());
				SigElementDefs.GetTableFromCache(true);
				SigButDefs.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Sites) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Sites.ToString());
				Sites.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.SmsBlockPhones) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.SmsBlockPhones.ToString());
				SmsBlockPhones.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.SmsPhones) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.SmsPhones.ToString());
				SmsPhones.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Sops) || refreshAll)
			{  //InvalidType.Sops is currently never used 11/14/2014
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Sops.ToString());
				Sops.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.StateAbbrs) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.StateAbbrs.ToString());
				StateAbbrs.GetTableFromCache(true);
			}
			//InvalidTypes.Tasks not handled here.
			if (invalidTypes.Contains(InvalidType.TimeCardRules) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.TimeCardRules.ToString());
				TimeCardRules.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.ToolButsAndMounts) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.ToolButsAndMounts.ToString());
				ToolButItems.GetTableFromCache(true);
				MountDefs.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.UserClinics) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.UserClinics.ToString());
				UserClinics.RefreshCache();
			}
			if (invalidTypes.Contains(InvalidType.UserQueries) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.UserQueries.ToString());
				UserQueries.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Vaccines) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Vaccines.ToString());
				VaccineDefs.GetTableFromCache(true);
				DrugManufacturers.GetTableFromCache(true);
				DrugUnits.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Views) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Views.ToString());
				ApptViews.GetTableFromCache(true);
				ApptViewItems.GetTableFromCache(true);
				AppointmentRules.GetTableFromCache(true);
				ProcApptColors.GetTableFromCache(true);
			}
			if (invalidTypes.Contains(InvalidType.Wiki) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.Wiki.ToString());
				WikiListHeaderWidths.GetTableFromCache(true);
				WikiPages.RefreshCache();
			}
			if (invalidTypes.Contains(InvalidType.ZipCodes) || refreshAll)
			{
				ODEvent.Fire(EventCategory.Cache, suffix + InvalidType.ZipCodes.ToString());
				ZipCodes.GetTableFromCache(true);
			}

			// TODO: Logger.LogToPath("", LogPath.Signals, LogPhase.End);
		}

		/// <summary>
		/// Returns a list of all invalid types that are used for the cache.
		/// </summary>
		public static List<InvalidType> GetAllCachedInvalidTypes()
		{
            return new List<InvalidType>
            {
                InvalidType.ProcCodes,
                InvalidType.Prefs,
                InvalidType.Views,
                InvalidType.AutoCodes,
                InvalidType.Carriers,
                InvalidType.ClearHouses,
                InvalidType.Computers,
                InvalidType.InsCats,
                InvalidType.Employees,
                InvalidType.Defs,
                InvalidType.Email,
                InvalidType.Letters,
                InvalidType.QuickPaste,
                InvalidType.Security,
                InvalidType.Programs,
                InvalidType.ToolButsAndMounts,
                InvalidType.Providers,
                InvalidType.ClaimForms,
                InvalidType.ZipCodes,
                InvalidType.LetterMerge,
                InvalidType.DentalSchools,
                InvalidType.Operatories,
                InvalidType.Sites,
                InvalidType.Pharmacies,
                InvalidType.Sheets,
                InvalidType.RecallTypes,
                InvalidType.FeeScheds,
                InvalidType.DisplayFields,
                InvalidType.CustomFields,
                InvalidType.AccountingAutoPays,
                InvalidType.ProcButtons,
                InvalidType.Diseases,
                InvalidType.Languages,
                InvalidType.AutoNotes,
                InvalidType.ElectIDs,
                InvalidType.Employers,
                InvalidType.ProviderIdents,
                InvalidType.InsFilingCodes,
                InvalidType.ReplicationServers,
                InvalidType.Automation,
                InvalidType.TimeCardRules,
                InvalidType.Vaccines,
                InvalidType.HL7Defs,
                InvalidType.DictCustoms,
                InvalidType.Wiki,
                InvalidType.Sops,
                InvalidType.EhrCodes,
                InvalidType.AppointmentTypes,
                InvalidType.Medications,
                InvalidType.ProviderErxs,
                InvalidType.StateAbbrs,
                InvalidType.RequiredFields,
                InvalidType.Ebills,
                InvalidType.UserClinics,
                InvalidType.OrthoChartTabs,
                InvalidType.SigMessages,
                InvalidType.ClinicPrefs,
                InvalidType.SmsBlockPhones,
                InvalidType.ClinicErxs,
                InvalidType.DisplayReports,
                InvalidType.UserQueries,
                InvalidType.SmsPhones,
                InvalidType.Referral,
                InvalidType.ProcMultiVisits,
                InvalidType.ProviderClinicLink
            };
		}
	}
}
