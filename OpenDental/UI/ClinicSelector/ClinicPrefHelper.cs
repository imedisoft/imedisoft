using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;

namespace OpenDental
{
    public class ClinicPrefHelper
	{
		/// <summary>
		/// This list includes all clinics as well the "default" pref with clinicnum=0.
		/// It includes only those prefnames that are used on the form where this helper is present.
		/// All items are mixed, and we filter.
		/// </summary>
		private readonly List<ClinicPref> clinicPrefs = new List<ClinicPref>();
		
		/// <summary>
		/// List of prefs that this helper will manage.
		/// </summary>
		private readonly List<PrefName> prefNames = new List<PrefName>();

		public ClinicPrefHelper(params PrefName[] prefNames)
		{
			var clinics = Clinics.GetForUserod(Security.CurrentUser, doIncludeHQ: true);

			foreach (var prefName in prefNames.Distinct())
			{
				// Explicity load the entire list because some forms using the clinicPrefHelper might have a "use defaults" pref
				// that this ClinicPrefHelper doesn't and shouldn't know about. We will clean up unneccessary clinic prefs when 
				// we call SyncPrefs.
				foreach (var clinic in clinics)
				{
					clinicPrefs.Add(new ClinicPref()
					{
						PrefName = prefName,
						ClinicNum = clinic.ClinicNum,
						ValueString = ClinicPrefs.GetPrefValue(prefName, clinic.ClinicNum)
					});
				}

				this.prefNames.Add(prefName);
			}
		}

		/// <summary>
		/// For all types, so pass in string val.
		/// This changes the value in the clinicpref that we have in memory.
		/// It will be synched later.
		/// </summary>
		public void ValChangedByUser(PrefName prefName, long clinicNum, string newVal)
		{
			if (clinicNum < 0) return;
			
			// Update the current value for the pref that we are storing in the list
			ClinicPref clinicPref = clinicPrefs.FirstOrDefault(x => x.ClinicNum == clinicNum && x.PrefName == prefName);
			if (clinicPref == null)
			{ 
				// Doesn't exist so create one
				clinicPref = new ClinicPref()
				{
					PrefName = prefName,
					ClinicNum = clinicNum
				};
				clinicPrefs.Add(clinicPref);
			}

			clinicPref.ValueString = newVal;
		}

		/// <summary>
		/// If there is no val for this clinic, then it uses the default pref, which is also in the available list.
		/// </summary>
		public bool GetBoolVal(PrefName prefName, long clinicNum)
		{
			if (clinicNum < 0) return false;
			
			if (clinicPrefs.Any(x => x.ClinicNum == clinicNum && x.PrefName == prefName))
			{ 
				return PIn.Bool(clinicPrefs.FirstOrDefault(x => x.ClinicNum == clinicNum && x.PrefName == prefName).ValueString);
			}

			return PIn.Bool(clinicPrefs.FirstOrDefault(x => x.ClinicNum == 0 && x.PrefName == prefName).ValueString);
		}

		public string GetStringVal(PrefName prefName, long clinicNum)
		{
			if (clinicNum < 0) return "";

			if (clinicPrefs.Any(x => x.ClinicNum == clinicNum && x.PrefName == prefName))
			{
				return clinicPrefs.FirstOrDefault(x => x.ClinicNum == clinicNum && x.PrefName == prefName).ValueString;
			}

			return clinicPrefs.FirstOrDefault(x => x.ClinicNum == 0 && x.PrefName == prefName).ValueString;
		}

		public bool GetDefaultBoolVal(PrefName prefName)
		{
			return PIn.Bool(clinicPrefs.First(x => x.ClinicNum == 0 && x.PrefName == prefName).ValueString);
		}

		public string GetDefaultStringVal(PrefName prefName)
		{
			return clinicPrefs.First(x => x.ClinicNum == 0 && x.PrefName == prefName).ValueString;
		}

		public List<ClinicPref> GetWhere(PrefName prefName, string valueString)
		{
			return clinicPrefs.FindAll(x => x.PrefName == prefName && x.ValueString == valueString);
		}

		/// <summary>
		/// Save all pref changes relating to prefs that were added in Init().
		/// </summary>>
		public bool SyncAllPrefs()
		{
			bool result = false;

			foreach (var prefName in prefNames)
			{
				if (SyncPref(prefName))
				{
					result = true;
				}
			}

			return result;
		}

		/// <summary>
		/// Save all pref changes relating to the given pref. PrefName must have been included in Init().
		/// It is suggested that you use SyncAllPrefs(), it is safer.
		/// </summary>>
		public bool SyncPref(PrefName prefName)
		{
			// We ensured that our list had default (ClinicNum 0) prefs when we included defaults in Init(). Should always be available.
			string hqValue = clinicPrefs.First(x => x.ClinicNum == 0 && x.PrefName == prefName).ValueString;
			
			// Save the default (HQ) pref first.
			bool didSave = prefName.Update(hqValue);

			// Our list will likely have clinic-specific entries which are identical to HQ defaults. 
			// In this case, remove those duplicates so we don't save them to the db.
			clinicPrefs.RemoveAll(x => x.ClinicNum != 0 && x.PrefName == prefName && x.ValueString.Equals(hqValue));

			var listNonDefaultClinicPrefs = clinicPrefs.FindAll(x => x.ClinicNum > 0 && x.PrefName == prefName);
			if (ClinicPrefs.Sync(listNonDefaultClinicPrefs, ClinicPrefs.GetPrefAllClinics(prefName)))
			{
				didSave = true;
			}

			if (didSave)
			{
				Signalods.SetInvalid(InvalidType.ClinicPrefs);
				ClinicPrefs.RefreshCache();
			}

			return didSave;
		}

		public List<long> GetClinicsWithChanges()
		{
			var result = new List<long>();

			foreach (var prefName in prefNames)
			{
				result.AddRange(GetClinicsWithChanges(prefName));
			}

			return result.Distinct().ToList();
		}

		/// <summary>
		/// Essentially a "sync" method although it doesn't save to the db. 
		/// Takes the list of new clinic preference values and compares it to the database. 
		/// Returns a list of clinics that have had their preference values changed.
		/// </summary>
		public List<long> GetClinicsWithChanges(PrefName prefName)
		{
            List<ClinicPref> clinicPrefs = this.clinicPrefs.FindAll(x => x.PrefName == prefName);
			List<ClinicPref> clinicPrefsInDb = ClinicPrefs.GetPrefAllClinics(prefName, includeDefault: true);

			List<long> clinicsIds = new List<long>();
			
			clinicsIds.AddRange(clinicPrefs.Where(x =>
				//Get the items from the new list that aren't in the old list 
				!clinicPrefsInDb.Select(y => y.ClinicNum).Contains(x.ClinicNum)
				//AND that aren't using the default preference value
				&& x.ValueString != PrefC.GetString(prefName))
				//Add the clinic nums
				.Select(x => x.ClinicNum));

			// Add any items that have been deleted or updated.
			foreach (ClinicPref oldClinicPref in clinicPrefsInDb)
			{
				ClinicPref newClinicPref = clinicPrefs.FirstOrDefault(x => x.ClinicNum == oldClinicPref.ClinicNum);
				if (newClinicPref == null)
				{ 
					// Item was in db and now is not.
					clinicsIds.Add(oldClinicPref.ClinicNum);
					continue;
				}

				if (newClinicPref.ValueString != oldClinicPref.ValueString)
				{ 
					// Item has changed.
					clinicsIds.Add(oldClinicPref.ClinicNum);
				}
			}

			return clinicsIds.Distinct().ToList();
		}
	}
}
