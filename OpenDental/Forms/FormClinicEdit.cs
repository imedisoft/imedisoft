using Imedisoft.Data;
using Imedisoft.Data.Models;
using Imedisoft.X12.Codes;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormClinicEdit : FormBase
	{
		private readonly Clinic clinic;
		private readonly List<DefLink> specialtyLinks = new List<DefLink>();
		private readonly List<DefLink> deletedSpecialtyLinks = new List<DefLink>();
		private readonly List<(string code, string description)> placesOfService;
		private readonly List<Provider> providers;
		private List<Definition> regions;
		private bool isMedLabHL7DefEnabled;
		private long? emailAddressId;

		public FormClinicEdit(Clinic clinic)
		{
			InitializeComponent();

			this.clinic = clinic;

			if (clinic.Id > 0)
            {
				specialtyLinks.AddRange(DefLinks.GetListByFKey(clinic.Id, DefLinkType.Clinic));
            }

			placesOfService = PlaceOfService.Codes.ToList();
			providers = Providers.GetProvsForClinic(clinic.Id);
			emailAddressId = clinic.EmailAddressId;
		}

		private void FormClinicEdit_Load(object sender, EventArgs e)
		{
			isMedicalCheckBox.Checked = clinic.IsMedicalOnly;
			descriptionTextBox.Text = clinic.Description;
			abbrTextBox.Text = clinic.Abbr;
			phoneTextBox.Text = TelephoneNumbers.ReFormat(clinic.Phone);
			faxTextBox.Text = TelephoneNumbers.ReFormat(clinic.Fax);
			useBillingAddressOnClaimsCheckBox.Checked = clinic.BillingAddressOnClaims;
			excludeFromInsVerifyListCheckBox.Checked = clinic.InsVerifyExcluded;
			isHiddenCheckBox.Checked = clinic.IsHidden;
			addressLine1TextBox.Text = clinic.AddressLine1;
			addressLine2TextBox.Text = clinic.AddressLine2;
			cityTextBox.Text = clinic.City;
			stateTextBox.Text = clinic.State;
			zipTextBox.Text = clinic.Zip;
			billingAddressLine1TextBox.Text = clinic.BillingAddressLine1;
			billingAddressLine2TextBox.Text = clinic.BillingAddressLine2;
			billingCityTextBox.Text = clinic.BillingCity;
			billingStateTextBox.Text = clinic.BillingState;
			billingZipTextBox.Text = clinic.BillingZip;
			payToAddressLine1TextBox.Text = clinic.PayToAddressLine1;
			payToAddressLine2TextBox.Text = clinic.PayToAddressLine2;
			payToCityTextBox.Text = clinic.PayToCity;
			payToStateTextBox.Text = clinic.PayToState;
			payToZipTextBox.Text = clinic.PayToZip;
			bankNumberTextBox.Text = clinic.BankNumber;
			schedulingNotesTextBox.Text = clinic.SchedulingNote;
			procCodeRequiredCheckBox.Checked = clinic.HasProcedureOnRx;

			if (Prefs.GetBool(PrefName.RxHasProc))
			{
				procCodeRequiredCheckBox.Enabled = true;
			}

			FillRegion();
			FillEmailAddress();
			FillDefaultInsBillingProvider();
			FillDefaultProvider();
			FillDefaultPlaceOfService();
			FillMedlab();
			FillSpecialty();
		}

		private void FillRegion()
        {
			regions = Definitions.GetByCategory(DefinitionCategory.Regions);

			regionComboBox.Items.Clear();
			regionComboBox.Items.Add(Translation.Common.None);
			regionComboBox.SelectedIndex = 0;

			foreach (var region in regions)
			{
				regionComboBox.Items.Add(region);

				if (region.Id == clinic.Region)
				{
					regionComboBox.SelectedItem = region;
				}
			}
		}

		private void FillEmailAddress()
		{
			if (emailAddressId.HasValue)
			{
				var emailAddress = EmailAddresses.GetOne(clinic.EmailAddressId.Value);

				if (emailAddress != null)
				{
					emailTextBox.Text = emailAddress.GetFrom();
					emailNoneButton.Enabled = true;

					return;
				}
			}

			emailAddressId = null;
			emailTextBox.Text = "";
			emailNoneButton.Enabled = false;
		}

		private void FillDefaultInsBillingProvider()
        {
			insBillingProviderComboBox.Items.Clear();

			foreach (var provider in providers)
            {
				insBillingProviderComboBox.Items.Add(provider);
				if (clinic.InsBillingProviderId == provider.Id)
                {
					insBillingProviderComboBox.SelectedItem = provider;
                }
            }

			switch (clinic.InsBillingProviderType)
            {
				case 'D':
					insBillingProviderDefaultRadioButton.Checked = true;
					break;

				case 'T':
					insBillingProviderTreatingRadioButton.Checked = true;
					break;

				case 'S':
					insBillingProviderSpecificRadioButton.Checked = true;
					break;
			}
		}

		private void FillDefaultProvider()
		{
			defaultProviderComboBox.Items.Clear();
			defaultProviderComboBox.Items.Add(Translation.Common.None);

			foreach (var provider in providers)
            {
				defaultProviderComboBox.Items.Add(provider);
				if (clinic.DefaultProviderId == provider.Id)
                {
					defaultProviderComboBox.SelectedItem = provider;
                }
            }

			if (defaultProviderComboBox.SelectedItem == null &&
				defaultProviderComboBox.Items.Count > 0)
				defaultProviderComboBox.SelectedIndex = 0;
		}

		private void FillDefaultPlaceOfService()
		{
			placeOfServiceComboBox.Items.Clear();

			foreach (var (code, description) in placesOfService)
			{
				placeOfServiceComboBox.Items.Add(description);
				if (clinic.DefaultPlaceOfService == code)
				{
					placeOfServiceComboBox.SelectedItem = description;
				}
			}
		}

		private void FillMedlab()
        {
			isMedLabHL7DefEnabled = HL7Defs.IsExistingHL7Enabled(0, true);

			if (isMedLabHL7DefEnabled)
			{
				medlabAccountTextBox.Visible = true;
				medlabAccountLabel.Visible = true;
				medlabAccountTextBox.Text = clinic.MedlabAccountId;
			}
		}

		private void FillSpecialty()
		{
			var clinicSpecialtyDefs = Definitions.GetDefsForCategory(DefinitionCategory.ClinicSpecialty).ToDictionary(x => x.Id);

			specialtiesGrid.BeginUpdate();
			specialtiesGrid.ListGridColumns.Clear();
			specialtiesGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Specialty, 100));
			specialtiesGrid.ListGridRows.Clear();

			foreach (var specialtyLink in specialtyLinks)
			{
				var specialtyDescript = "";
				if (clinicSpecialtyDefs.TryGetValue(specialtyLink.DefinitionId, out var def))
				{
					specialtyDescript = def.Name + (def.IsHidden ? " (" + Translation.Common.TagHidden + ")" : "");
				}

				var row = new GridRow();
				row.Cells.Add(specialtyDescript);
				row.Tag = specialtyLink;

				specialtiesGrid.ListGridRows.Add(row);
			}

			specialtiesGrid.EndUpdate();
		}

		private void InsBillingProviderSpecificRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			insBillingProviderComboBox.Enabled = insBillingProviderPickButton.Enabled
				= insBillingProviderSpecificRadioButton.Checked;
		}

		private void InsBillingProviderPickButton_Click(object sender, EventArgs e)
		{
			using var formProviderPick = new FormProviderPick(providers);

            if (insBillingProviderComboBox.SelectedItem is Provider selected)
            {
                formProviderPick.SelectedProviderId = selected.Id;
            }

            if (formProviderPick.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			foreach (var provider in providers)
            {
				if (provider.Id == formProviderPick.SelectedProviderId)
                {
					insBillingProviderComboBox.SelectedItem = provider;

					break;
                }
            }
		}

		private void DefaultProviderPickButton_Click(object sender, EventArgs e)
		{
			using var formProviderPick = new FormProviderPick(providers);

            if (defaultProviderComboBox.SelectedItem is Provider selected)
            {
                formProviderPick.SelectedProviderId = selected.Id;
            }

            if (formProviderPick.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			foreach (var provider in providers)
			{
				if (provider.Id == formProviderPick.SelectedProviderId)
				{
					defaultProviderComboBox.SelectedItem = provider;

					break;
				}
			}
		}

		private void EmailPickButton_Click(object sender, EventArgs e)
		{
            using var formEmailAddresses = new FormEmailAddresses
            {
                IsSelectionMode = true
            };

            if (formEmailAddresses.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			emailAddressId = formEmailAddresses.EmailAddressNum;

			FillEmailAddress();
		}

		private void EmailNoneButton_Click(object sender, EventArgs e)
		{
			emailAddressId = null;

			emailTextBox.Text = "";
			emailNoneButton.Enabled = false;
		}

		private void SpecialtiesGrid_SelectionCommitted(object sender, EventArgs e)
		{
			removeSpecialtyButton.Enabled = specialtiesGrid.SelectedGridRows.Count > 0;
		}

		private void AddSpecialtyButton_Click(object sender, EventArgs e)
		{
			using var formDefinitionPicker = new FormDefinitionPicker(DefinitionCategory.ClinicSpecialty)
			{
				AllowShowHidden = false,
				AllowMultiSelect = true
			};

			if (formDefinitionPicker.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			foreach (var definition in formDefinitionPicker.SelectedDefinitions)
			{
				if (specialtyLinks.Any(x => x.DefinitionId == definition.Id))
				{
					continue;
				}

				specialtyLinks.Add(new DefLink
				{
					DefinitionId = definition.Id,
					FKey = clinic.Id,
					LinkType = DefLinkType.Clinic
				});
			}

			FillSpecialty();
		}

		private void RemoveSpecialtyButton_Click(object sender, EventArgs e)
		{
			var definitionLink = specialtiesGrid.SelectedTag<DefLink>();
			if (definitionLink == null)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			if (specialtyLinks.Remove(definitionLink))
			{
				if (definitionLink.Id > 0)
				{
					deletedSpecialtyLinks.Add(definitionLink);
				}
			}

			FillSpecialty();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterDescription);

				return;
			}

			var abbr = abbrTextBox.Text.Trim();
			if (abbr.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterAbbreviation);
				return;
			}

			string phone = phoneTextBox.Text.Trim();
			if (Application.CurrentCulture.Name == "en-US")
			{
				phone = phone.Replace("(", "");
				phone = phone.Replace(")", "");
				phone = phone.Replace(" ", "");
				phone = phone.Replace("-", "");
				if (phone.Length != 0 && phone.Length != 10)
				{
					ShowError("Invalid phone.");

					return;
				}
			}

			string fax = faxTextBox.Text.Trim();
			if (Application.CurrentCulture.Name == "en-US")
			{
				fax = fax.Replace("(", "");
				fax = fax.Replace(")", "");
				fax = fax.Replace(" ", "");
				fax = fax.Replace("-", "");
				if (fax.Length != 0 && fax.Length != 10)
				{
					ShowError("Invalid fax.");

					return;
				}
			}

			var medlabAccountId = medlabAccountTextBox.Text.Trim();
			if (isMedLabHL7DefEnabled && !string.IsNullOrWhiteSpace(medlabAccountId))
			{
				var medlabAccountInUse = Clinics.Where(x => x.Id != clinic.Id).Any(x => x.MedlabAccountId.Equals(medlabAccountId));
				if (medlabAccountInUse)
				{
					ShowError(Translation.Common.MedLabAccountInUseByAnotherClinic);

					return;
				}
			}

			var region = regionComboBox.SelectedItem as Definition;
			var regionId = region?.Id;

			char insBillingProviderType =
				insBillingProviderSpecificRadioButton.Checked ? 'S' :
					(insBillingProviderTreatingRadioButton.Checked ? 'T' : 'D');

			var insBillingProvider = insBillingProviderComboBox.SelectedItem as Provider;
			var insBillingProviderId = insBillingProvider?.Id;

			if (insBillingProviderType == 'S' && insBillingProvider == null)
            {
				ShowError(Translation.Common.PleaseSelectProviderForInsuranceBilling);

				return;
            }

			var defaultProvider = defaultProviderComboBox.SelectedItem as Provider;
			var defaultProviderId = defaultProvider?.Id;

			// Make sure there are no users that are restricted to this clinic. Hiding the clinic in that case
			// would render the restricted users unable to login.
			if (isHiddenCheckBox.Checked)
			{
				var usersRestrictedToClinic = Userods.GetUsersOnlyThisClinic(clinic.Id);
				if (usersRestrictedToClinic.Count > 0)
				{
					ShowError(
						Translation.Common.UnableToHideClinicWithClinicRestrictedUsers + " " +
						string.Join(", ", usersRestrictedToClinic.Select(
							user => user.UserName)));

					return;
				}
			}

			clinic.IsMedicalOnly = isMedicalCheckBox.Checked;
			clinic.IsHidden = isHiddenCheckBox.Checked;
			clinic.InsVerifyExcluded = excludeFromInsVerifyListCheckBox.Checked;
			clinic.Abbr = abbr;
			clinic.Description = description;
			clinic.Phone = phone;
			clinic.Fax = fax;
			clinic.AddressLine1 = addressLine1TextBox.Text;
			clinic.AddressLine2 = addressLine2TextBox.Text;
			clinic.City = cityTextBox.Text;
			clinic.State = stateTextBox.Text;
			clinic.Zip = zipTextBox.Text;
			clinic.BillingAddressLine1 = billingAddressLine1TextBox.Text;
			clinic.BillingAddressLine2 = billingAddressLine2TextBox.Text;
			clinic.BillingCity = billingCityTextBox.Text;
			clinic.BillingState = billingStateTextBox.Text;
			clinic.BillingZip = billingZipTextBox.Text;
			clinic.PayToAddressLine1 = payToAddressLine1TextBox.Text;
			clinic.PayToAddressLine2 = payToAddressLine2TextBox.Text;
			clinic.PayToCity = payToCityTextBox.Text;
			clinic.PayToState = payToStateTextBox.Text;
			clinic.PayToZip = payToZipTextBox.Text;
			clinic.BankNumber = bankNumberTextBox.Text;
			clinic.DefaultPlaceOfService = placesOfService[placeOfServiceComboBox.SelectedIndex].code;
			clinic.BillingAddressOnClaims = useBillingAddressOnClaimsCheckBox.Checked;
			clinic.Region = regionId;
			clinic.InsBillingProviderType = insBillingProviderType;
			clinic.InsBillingProviderId = insBillingProviderId;
			clinic.DefaultProviderId = defaultProviderId;
			clinic.SchedulingNote = schedulingNotesTextBox.Text;
			clinic.EmailAddressId = emailAddressId;

			if (Prefs.GetBool(PrefName.RxHasProc))
			{
				clinic.HasProcedureOnRx = procCodeRequiredCheckBox.Checked;
			}

			if (isMedLabHL7DefEnabled)
			{
				clinic.MedlabAccountId = medlabAccountId;
			}

			DialogResult = DialogResult.OK;
		}
    }
}
