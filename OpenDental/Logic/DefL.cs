using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using Imedisoft.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public class DefL
	{
		public static List<DefinitionCategoryOptions> GetOptionsForDefCats(IEnumerable<string> definitionCategories)
		{
			var optionsList = new List<DefinitionCategoryOptions>();
			foreach (string defCatCur in definitionCategories)
			{
				var options = new DefinitionCategoryOptions(defCatCur);
				switch (defCatCur)
				{
					case DefinitionCategory.AccountColors:
						options.CanEditName = false;
						options.EnableColor = true;
						options.HelpText = "Changes the color of text for different types of entries in Account Module";
						break;

					case DefinitionCategory.AccountQuickCharge:
						options.CanDelete = true;
						options.EnableValue = true;
						options.ValueText = "Procedure Codes";
						options.HelpText = "Account Proc Quick Add items.  Each entry can be a series of procedure codes separated by commas (e.g. D0180,D1101,D8220).  Used in the account module to quickly charge patients for items.";
						break;

					case DefinitionCategory.AdjTypes:
						options.EnableValue = true;
						options.ValueText =  "+, -, or dp";
						options.HelpText =  "Plus increases the patient balance.  Minus decreases it.  Dp means discount plan.  Not allowed to change value after creating new type since changes affect all patient accounts.";
						break;

					case DefinitionCategory.AppointmentColors:
						options.CanEditName = false;
						options.EnableColor = true;
						options.HelpText = "Changes colors of background in Appointments Module, and colors for completed appointments.";
						break;

					case DefinitionCategory.ApptConfirmed:
						options.EnableColor = true;
						options.EnableValue = true;
						options.ValueText = "Abbrev";
						options.HelpText = "Color shows on each appointment if Appointment View is set to show ConfirmedColor.";
						break;

					case DefinitionCategory.ApptProcsQuickAdd:
						options.EnableValue = true;
						options.ValueText = "ADA Code(s)";
						if (Clinics.IsMedicalClinic(Clinics.ClinicId))
						{
							options.HelpText = "These are the procedures that you can quickly add to the treatment plan from within the appointment editing window.  Multiple procedures may be separated by commas with no spaces. These definitions may be freely edited without affecting any patient records.";
						}
						else
						{
							options.HelpText = "These are the procedures that you can quickly add to the treatment plan from within the appointment editing window.  They must not require a tooth number. Multiple procedures may be separated by commas with no spaces. These definitions may be freely edited without affecting any patient records.";
						}
						break;

					case DefinitionCategory.AutoDeposit:
						options.CanDelete = true;
						options.CanHide = true;
						options.EnableValue = true;
						options.ValueText = "Account Number";
						break;

					case DefinitionCategory.AutoNoteCats:
						options.CanDelete = true;
						options.CanHide = false;
						options.EnableValue = true;
						options.IsValueDefNum = true;
						options.ValueText =  "Parent Category";
						options.HelpText =  "Leave the Parent Category blank for categories at the root level. Assign a Parent Category to move a category within another. The order set here will only affect the order within the assigned Parent Category in the Auto Note list. For example, a category may be moved above its parent in this list, but it will still be within its Parent Category in the Auto Note list.";
						break;

					case DefinitionCategory.BillingTypes:
						options.EnableValue = true;
						options.ValueText = "E, C, or CE";
						options.HelpText = "E=Email bill, C=Collection, CE=Collection Excluded.  It is recommended to use as few billing types as possible.  They can be useful when running reports to separate delinquent accounts, but can cause 'forgotten accounts' if used without good office procedures. Changes affect all patients.";
						break;

					case DefinitionCategory.BlockoutTypes:
						options.EnableColor = true;
						options.HelpText = "Blockout types are used in the appointments module.";
						options.EnableValue = true;
						options.ValueText = "Flags";
						break;

					case DefinitionCategory.ChartGraphicColors:
						options.CanEditName = false;
						options.EnableColor = true;
						if (Clinics.IsMedicalClinic(Clinics.ClinicId))
						{
							options.HelpText = "These colors will be used to graphically display treatments.";
						}
						else
						{
							options.HelpText = "These colors will be used on the graphical tooth chart to draw restorations.";
						}
						break;

					case DefinitionCategory.ClaimCustomTracking:
						options.CanDelete = true;
						options.CanHide = false;
						options.EnableValue = true;
						options.ValueText =  "Days Suppressed";
						options.HelpText = "Some offices may set up claim tracking statuses such as 'review', 'hold', 'riskmanage', etc." + "\r\n" +
							"Set the value of 'Days Suppressed' to the number of days the claim will be suppressed from the Outstanding Claims Report " +
							"when the status is changed to the selected status.";
						break;

					case DefinitionCategory.ClaimErrorCode:
						options.CanDelete = true;
						options.CanHide = false;
						options.EnableValue = true;
						options.ValueText = "Description";
						options.HelpText =  "Used to track error codes when entering claim custom statuses.";
						break;

					case DefinitionCategory.ClaimPaymentTracking:
						options.ValueText = "Value";
						options.HelpText =  "EOB adjudication method codes to be used for insurance payments.  Last entry cannot be hidden.";
						break;

					case DefinitionCategory.ClaimPaymentGroups:
						options.ValueText =  "Value";
						options.HelpText =  "Used to group claim payments in the daily payments report.";
						break;

					case DefinitionCategory.ClinicSpecialty:
						options.CanHide = true;
						options.CanDelete = false;
						options.HelpText = "You can add as many specialties as you want.  Changes affect all current records.";
						break;

					case DefinitionCategory.CommLogTypes:
						options.EnableValue = true;
						options.EnableColor = true;
						options.DoShowNoColor = true;
						string commItemTypes = string.Join(", ", Commlogs.GetCommItemTypes().Select(x => x.GetDescription(useShortVersionIfAvailable: true)));
						options.ValueText = "Usage";
						options.HelpText = "Changes affect all current commlog entries.  Optionally set Usage to one of the following: " + commItemTypes + 
							". Only one of each. This helps automate new entries.";
						break;

					case DefinitionCategory.ContactCategories:
						options.HelpText = "You can add as many categories as you want.  Changes affect all current contact records.";
						break;

					case DefinitionCategory.Diagnosis:
						options.EnableValue = true;
						options.ValueText = "1 or 2 letter abbreviation";
						options.HelpText = "The diagnosis list is shown when entering a procedure.  Ones that are less used should go lower on the list.  The abbreviation is shown in the progress notes.  BE VERY CAREFUL.  Changes affect all patients.";
						break;

					case DefinitionCategory.FeeColors:
						options.CanEditName = false;
						options.CanHide = false;
						options.EnableColor = true;
						options.HelpText = "These are the colors associated to fee types.";
						break;

					case DefinitionCategory.ImageCats:
						options.ValueText = "Usage";
						options.HelpText = "These are the categories that will be available in the image and chart modules.  If you hide a category, images in that category will be hidden, so only hide a category if you are certain it has never been used.  Multiple categories can be set to show in the Chart module, but only one category should be set for patient pictures, statements, and tooth charts. Selecting multiple categories for treatment plans will save the treatment plan in each category. Affects all patient records.";
						break;

					case DefinitionCategory.InsurancePaymentType:
						options.CanDelete = true;
						options.CanHide = false;
						options.EnableValue = true;
						options.ValueText = "N=Not selected for deposit";
						options.HelpText = "These are claim payment types for insurance payments attached to claims.";
						break;

					case DefinitionCategory.InsuranceVerificationStatus:
						options.ValueText = "Usage";
						options.HelpText = "These are statuses for the insurance verification list.";
						break;

					case DefinitionCategory.LetterMergeCats:
						options.HelpText = "Categories for Letter Merge.  You can safely make any changes you want.";
						break;

					case DefinitionCategory.MiscColors:
						options.CanEditName = false;
						options.EnableColor = true;
						options.HelpText = "";
						break;

					case DefinitionCategory.PaymentTypes:
						options.EnableValue = true;
						options.ValueText = "N=Not selected for deposit";
						options.HelpText = "Types of payments that patients might make. Any changes will affect all patients.";
						break;

					case DefinitionCategory.PayPlanCategories:
						options.HelpText = "Assign payment plans to different categories";
						break;

					case DefinitionCategory.PaySplitUnearnedType:
						options.ValueText = "Do Not Show on Account";
						options.HelpText = "Usually only used by offices that use accrual basis accounting instead of cash basis accounting. Any changes will affect all patients.";
						options.EnableValue = true;
						break;

					case DefinitionCategory.ProcButtonCats:
						options.HelpText = "These are similar to the procedure code categories, but are only used for organizing and grouping the procedure buttons in the Chart module.";
						break;

					case DefinitionCategory.ProcCodeCats:
						options.HelpText = "These are the categories for organizing procedure codes. They do not have to follow ADA categories.  There is no relationship to insurance categories which are setup in the Ins Categories section.  Does not affect any patient records.";
						break;

					case DefinitionCategory.ProgNoteColors:
						options.CanEditName = false;
						options.EnableColor = true;
						options.HelpText = "Changes color of text for different types of entries in the Chart Module Progress Notes.";
						break;

					case DefinitionCategory.Prognosis:
						break;

					case DefinitionCategory.ProviderSpecialties:
						options.HelpText = "Provider specialties cannot be deleted.  Changes to provider specialties could affect e-claims.";
						break;

					case DefinitionCategory.RecallUnschedStatus:
						options.EnableValue = true;
						options.ValueText = "Abbreviation";
						options.HelpText = "Recall/Unsched Status.  Abbreviation must be 7 characters or less.  Changes affect all patients.";
						break;

					case DefinitionCategory.Regions:
						options.CanHide = false;
						options.HelpText = "The region identifying the clinic it is assigned to.";
						break;

					case DefinitionCategory.SupplyCats:
						options.CanDelete = true;
						options.CanHide = false;
						options.HelpText = "The categories for inventory supplies.";
						break;

					case DefinitionCategory.TaskPriorities:
						options.EnableColor = true;
						options.EnableValue = true;
						options.ValueText = "D = Default, R = Reminder";
						options.HelpText = "Priorities available for selection within the task edit window.  Task lists are sorted using the order of these priorities.  They can have any description and color.  At least one priority should be Default (D).  If more than one priority is flagged as the default, the last default in the list will be used.  If no default is set, the last priority will be used.  Use (R) to indicate the initial reminder task priority to use when creating reminder tasks.  Changes affect all tasks where the definition is used.";
						break;

					case DefinitionCategory.TxPriorities:
						options.EnableColor = true;
						options.EnableValue = true;
						options.DoShowItemOrderInValue = true;
						options.ValueText = "Internal Priority";
						options.HelpText = 
							"Displayed order should match order of priority of treatment.  They are used in Treatment Plan and Chart " +
							"modules. They can be simple numbers or descriptive abbreviations 7 letters or less.  Changes affect all procedures where the " +
							"definition is used.  'Internal Priority' does not show, but is used for list order and for automated selection of which procedures " +
							"are next in a planned appointment.";
						break;

					case DefinitionCategory.WebSchedNewPatApptTypes:
						options.CanDelete = true;
						options.CanHide = false;
						options.ValueText = "Appointment Type";
						options.HelpText = "Appointment types to be displayed in the Web Sched New Pat Appt web application.  These are selectable for the new patients and will be saved to the appointment note.";
						break;

					case DefinitionCategory.CarrierGroupNames:
						options.CanHide = true;
						options.HelpText = "These are group names for Carriers.";
						break;

					case DefinitionCategory.TimeCardAdjTypes:
						options.CanEditName = true;
						options.CanHide = true;
						options.HelpText = "These are PTO Adjustments Types used for tracking on employee time cards and ADP export.";
						break;
				}

				optionsList.Add(options);
			}

			return optionsList;
		}

		private static string GetDescriptionForImageCategory(string itemValue)
		{
			var results = new List<string>();

			if (itemValue.Contains("X")) results.Add("Chart Module");
			if (itemValue.Contains("F")) results.Add("Patient Form");
			if (itemValue.Contains("P")) results.Add("Patient Pictures");
			if (itemValue.Contains("S")) results.Add("Statements");
			if (itemValue.Contains("T")) results.Add("Tooth Chart");
			if (itemValue.Contains("R")) results.Add("Treatment Plans");
			if (itemValue.Contains("L")) results.Add("Patient Portal");
			if (itemValue.Contains("A")) results.Add("Payment Plans");
			if (itemValue.Contains("C")) results.Add("Claim Attachments");
			if (itemValue.Contains("B")) results.Add("Lab Cases");
			if (itemValue.Contains("U")) results.Add("AutoSave Forms");

			return string.Join(", ", results);
		}

		public static void FillGrid(ODGrid grid, DefinitionCategoryOptions selectedCategoryOptions, List<Definition> definitions)
		{
			var selectedDefinition = grid.SelectedTag<Definition>();
			int? selectedDefinitionIndex = null;

			int scroll = grid.ScrollValue;

			grid.BeginUpdate();
			grid.Columns.Clear();
			grid.Columns.Add(new GridColumn("Name", 190));
			grid.Columns.Add(new GridColumn(selectedCategoryOptions.ValueText, 190));
			grid.Columns.Add(new GridColumn(selectedCategoryOptions.EnableColor ? "Color" : "", 40));
			grid.Columns.Add(new GridColumn(selectedCategoryOptions.CanHide ? "Hide" : "", 30, HorizontalAlignment.Center));
			grid.Rows.Clear();

			foreach (var definition in definitions)
			{
				if (Definitions.IsDefDeprecated(definition))
				{
					definition.IsHidden = true;
				}

				var gridRow = new GridRow();
				gridRow.Cells.Add(definition.Name);

				if (selectedCategoryOptions.Category == DefinitionCategory.ImageCats)
				{
					gridRow.Cells.Add(GetDescriptionForImageCategory(definition.Value));
				}
				else if (selectedCategoryOptions.Category == DefinitionCategory.AutoNoteCats)
				{
					var parentCat = definitions.FirstOrDefault(x => x.Id.ToString() == definition.Value);

					gridRow.Cells.Add(parentCat?.Name ?? definition.Value);
                }
				else if (selectedCategoryOptions.Category == DefinitionCategory.WebSchedNewPatApptTypes)
				{
					AppointmentType appointmentType = AppointmentTypes.GetWebSchedNewPatApptTypeByDef(definition.Id);
					gridRow.Cells.Add(appointmentType == null ? "" : appointmentType.Name);
				}
				else if (selectedCategoryOptions.DoShowItemOrderInValue)
				{
					gridRow.Cells.Add(definition.SortOrder.ToString());
				}
				else
				{
					gridRow.Cells.Add(definition.Value);
				}

				gridRow.Cells.Add("");
				if (selectedCategoryOptions.EnableColor)
				{
					gridRow.Cells[gridRow.Cells.Count - 1].BackColor = definition.Color;
				}

				gridRow.Cells.Add(definition.IsHidden ? "X" : "");
				gridRow.Tag = definition;

				grid.Rows.Add(gridRow);

				if (definition.Id == selectedDefinition.Id)
                {
					selectedDefinitionIndex = grid.Rows.Count - 1;
				}
			}

			grid.EndUpdate();

			if (selectedDefinitionIndex.HasValue)
            {
				grid.SetSelected(selectedDefinitionIndex.Value, true);
            }

			grid.ScrollValue = scroll;
		}

		public static bool TryEditDefinition(Definition definition, DefinitionCategoryOptions categoryOptions, List<Definition> definitions)
		{
			switch (categoryOptions.Category)
			{
				case DefinitionCategory.BlockoutTypes:
					using (var formDefEditBlockout = new FormDefEditBlockout(definition))
					{
						if (formDefEditBlockout.ShowDialog() == DialogResult.OK)
						{
							return true;
						}
					}
					break;

				case DefinitionCategory.ImageCats:
					using (var formDefEditImages = new FormDefEditImages(definition))
					{
						formDefEditImages.IsNew = false;

						if (formDefEditImages.ShowDialog() == DialogResult.OK)
						{
							return true;
						}
					}
					break;

				case DefinitionCategory.WebSchedNewPatApptTypes:
					using (var formDefEditWSNPApptTypes = new FormDefEditWSNPApptTypes(definition))
					{
						if (formDefEditWSNPApptTypes.ShowDialog() == DialogResult.OK)
						{
							return true;
						}
					}
					break;

				default:
					using (var formDefEdit = new FormDefinitionEdit(definition, definitions, categoryOptions))
					{
						if (formDefEdit.ShowDialog() == DialogResult.OK)
						{
							return true;
						}
					}
					break;
			}

			return false;
		}

		public static bool CreateDefinition(ODGrid grid, DefinitionCategoryOptions categoryOptions)
		{
			int itemOrder = 0;
			if (Definitions.GetDefsForCategory(categoryOptions.Category).Count > 0)
			{
				itemOrder = Definitions.GetDefsForCategory(categoryOptions.Category).Max(x => x.SortOrder) + 1;
			}

            var definition = new Definition
            {
                SortOrder = itemOrder,
                Category = categoryOptions.Category,
                Name = "",
                Value = ""
            };

            if (categoryOptions.Category == DefinitionCategory.InsurancePaymentType)
			{
				definition.Value = "N";
			}

			switch (categoryOptions.Category)
			{
				case DefinitionCategory.BlockoutTypes:
					using (var formDefEditBlockout = new FormDefEditBlockout(definition))
					{
						formDefEditBlockout.IsNew = true;
						if (formDefEditBlockout.ShowDialog() != DialogResult.OK)
						{
							return false;
						}
					}
					break;

				case DefinitionCategory.ImageCats:
					using (var formDefEditImages = new FormDefEditImages(definition))
					{
						formDefEditImages.IsNew = true;
						if (formDefEditImages.ShowDialog() != DialogResult.OK)
						{
							return false;
						}
					}
					break;

				case DefinitionCategory.WebSchedNewPatApptTypes:
					using (var formDefEditWSNPAppt = new FormDefEditWSNPApptTypes(definition))
					{
						if (formDefEditWSNPAppt.ShowDialog() != DialogResult.OK)
						{
							return false;
						}
					}
					break;

				default:
					var currentDefs = new List<Definition>();
					foreach (var gridRow in grid.Rows)
					{
						currentDefs.Add((Definition)gridRow.Tag);
					}

					using (var formDefEdit = new FormDefinitionEdit(definition, currentDefs, categoryOptions))
					{
						if (formDefEdit.ShowDialog() != DialogResult.OK)
						{
							return false;
						}
					}
					break;
			}

			return true;
		}

		/// <summary>
		/// Attempts to hide the definition that is selected in the specified <paramref name="grid"/>.
		/// </summary>
		/// <param name="grid">The grid.</param>
		/// <param name="categoryOptions">The options of the category.</param>
		public static bool TryHideSelectedDefinition(ODGrid grid, DefinitionCategoryOptions categoryOptions)
		{
			var definition = grid.SelectedTag<Definition>();
			if (definition == null)
			{
				ODMessageBox.Show(Imedisoft.Translation.Common.PleaseSelectItemFirst);

				return false;
			}

			if (!CanHideDefinition(definition, categoryOptions))
			{
				return false;
			}

			Definitions.Hide(definition);

			return true;
		}

		/// <summary>
		///		<para>
		///			Determines whether the specified <paramref name="definition"/> can be hidden.
		///		</para>
		///		<para>
		///			Displays a alert if the definition cannot be hidden.
		///		</para>
		/// </summary>
		/// <param name="definition">The definition.</param>
		/// <param name="categoryOptions"></param>
		/// <returns>True if the definition can be hidden; otherwise, false.</returns>
		public static bool CanHideDefinition(Definition definition, DefinitionCategoryOptions categoryOptions)
		{
			if (definition.IsHidden) return true;

			if (!categoryOptions.CanHide || !categoryOptions.CanEditName)
			{
				ODMessageBox.Show("Definitions of this category cannot be hidden.");

				return false; // We should never get here, but if we do, something went wrong because the definition shouldn't have been hideable
			}

			// Stop users from hiding the last definition in categories that must have at least one def in them.
			var visibleDefinitions = Definitions.GetByCategory(categoryOptions.Category);
			if (Definitions.NeedOneUnhidden(definition.Category) && visibleDefinitions.Count == 1)
			{
				ODMessageBox.Show("You cannot hide the last definition in this category.");

				return false;
			}

			if (definition.Category == DefinitionCategory.ProviderSpecialties && (Providers.IsSpecialtyInUse(definition.Id) || Referrals.IsSpecialtyInUse(definition.Id)))
			{
				ODMessageBox.Show("You cannot hide a specialty if it is in use by a provider or a referral source.");

				return false;
			}

			if (Definitions.IsDefinitionInUse(definition))
			{
				if (definition.Id.In(
					Prefs.GetLong(PrefName.BrokenAppointmentAdjustmentType),
					Prefs.GetLong(PrefName.AppointmentTimeArrivedTrigger),
					Prefs.GetLong(PrefName.AppointmentTimeSeatedTrigger),
					Prefs.GetLong(PrefName.AppointmentTimeDismissedTrigger),
					Prefs.GetLong(PrefName.TreatPlanDiscountAdjustmentType),
					Prefs.GetLong(PrefName.BillingChargeAdjustmentType),
					Prefs.GetLong(PrefName.FinanceChargeAdjustmentType),
					Prefs.GetLong(PrefName.PrepaymentUnearnedType),
					Prefs.GetLong(PrefName.SalesTaxAdjustmentType),
					Prefs.GetLong(PrefName.RecurringChargesPayTypeCC),
					Prefs.GetLong(PrefName.TpUnearnedType)))
				{
					ODMessageBox.Show("You cannot hide a definition if it is in use within Module Preferences.");
					return false;
				}
				else if (definition.Id.In(
					Prefs.GetLong(PrefName.RecallStatusMailed),
					Prefs.GetLong(PrefName.RecallStatusTexted),
					Prefs.GetLong(PrefName.RecallStatusEmailed),
					Prefs.GetLong(PrefName.RecallStatusEmailedTexted)))
				{
					ODMessageBox.Show("You cannot hide a definition that is used as a status in the Setup Recall window.");
					return false;
				}
				else if (definition.Id == Prefs.GetLong(PrefName.WebSchedNewPatConfirmStatus))
				{
					ODMessageBox.Show("You cannot hide a definition that is used as an appointment confirmation status in Web Sched New Pat Appt.");
					return false;
				}
				else if (definition.Id == Prefs.GetLong(PrefName.WebSchedRecallConfirmStatus))
				{
					ODMessageBox.Show("You cannot hide a definition that is used as an appointment confirmation status in Web Sched Recall Appt.");
					return false;
				}
				else if (definition.Id == Prefs.GetLong(PrefName.PracticeDefaultBillType))
				{
					ODMessageBox.Show("You cannot hide a billing type when it is selected as the practice default billing type.");
					return false;
				}
				else
				{
					if (!MsgBox.Show(MsgBoxButtons.OKCancel, "Warning: This definition is currently in use within the program."))
					{
						return false;
					}
				}
			}

			if (definition.Category == DefinitionCategory.PaySplitUnearnedType)
			{
				if (visibleDefinitions.FindAll(x => string.IsNullOrEmpty(x.Value)).Count == 1 && definition.Value == "")
				{
					ODMessageBox.Show("Must have at least one definition that shows in Account");

					return false;
				}

				if (visibleDefinitions.FindAll(x => !string.IsNullOrEmpty(x.Value)).Count == 1 && definition.Value != "")
				{
					ODMessageBox.Show("Must have at least one definition that does not show in Account");

					return false;
				}
			}

			// Warn the user if they are about to hide a billing type currently in use.
			if (categoryOptions.Category == DefinitionCategory.BillingTypes && Patients.IsBillingTypeInUse(definition.Id))
			{
				if (!MsgBox.Show(MsgBoxButtons.OKCancel, "Warning: Billing type is currently in use by patients, insurance plans, or preferences."))
				{
					return false;
				}
			}

			return true;
		}




		/// <summary>
		/// Moves the selected definition up in the specified <paramref name="grid"/>.
		/// </summary>
		/// <param name="grid">The grid.</param>
		/// <returns>True if the selected definition was moved; otherwise, false.</returns>
		public static bool MoveSelectedUp(ODGrid grid)
		{
			var index = grid.GetSelectedIndex();
			if (index == -1)
			{
				ODMessageBox.Show(Imedisoft.Translation.Common.PleaseSelectItemFirst);

				return false;
			}

			if (index == 0)
			{
				return false;
			}

			var item1 = (Definition)grid.Rows[index].Tag;
			var item2 = (Definition)grid.Rows[index - 1].Tag;

			(item1.SortOrder, item2.SortOrder) = (item2.SortOrder, item1.SortOrder);

			Definitions.Update(item1);
			Definitions.Update(item2);

			return true;
		}

		/// <summary>
		/// Moves the selected definition down in the specified <paramref name="grid"/>.
		/// </summary>
		/// <param name="grid">The grid.</param>
		/// <returns>True if the selected definition was moved; otherwise, false.</returns>
		public static bool MoveSelectedDown(ODGrid grid)
		{
			var index = grid.GetSelectedIndex();
			if (index == -1)
			{
				ODMessageBox.Show(Imedisoft.Translation.Common.PleaseSelectItemFirst);

				return false;
			}

			if (index == grid.Rows.Count - 1)
			{
				return false;
			}

			var item1 = (Definition)grid.Rows[index].Tag;
			var item2 = (Definition)grid.Rows[index + 1].Tag;

			(item1.SortOrder, item2.SortOrder) = (item2.SortOrder, item1.SortOrder);

			Definitions.Update(item1);
			Definitions.Update(item2);

			return true;
		}
	}
}
