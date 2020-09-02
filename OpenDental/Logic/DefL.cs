using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public class DefL
	{
		#region GetMethods
		public static List<DefCatOptions> GetOptionsForDefCats(IEnumerable<string> definitionCategories)
		{
			var defCatOptions = new List<DefCatOptions>();
			foreach (string defCatCur in definitionCategories)
			{
				var options = new DefCatOptions(defCatCur);
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

				defCatOptions.Add(options);
			}

			return defCatOptions;
		}

		private static string GetItemDescForImages(string itemValue)
		{
			var results = new List<string>();

			if (itemValue.Contains("X")) results.Add("ChartModule");
			if (itemValue.Contains("F")) results.Add("PatientForm");
			if (itemValue.Contains("P")) results.Add("PatientPic");
			if (itemValue.Contains("S")) results.Add("Statement");
			if (itemValue.Contains("T")) results.Add("ToothChart");
			if (itemValue.Contains("R")) results.Add("TreatPlans");
			if (itemValue.Contains("L")) results.Add("PatientPortal");
			if (itemValue.Contains("A")) results.Add("PayPlans");
			if (itemValue.Contains("C")) results.Add("ClaimAttachments");
			if (itemValue.Contains("B")) results.Add("LabCases");
			if (itemValue.Contains("U")) results.Add("AutoSaveForms");

			return string.Join(", ", results);
		}
		#endregion

		/// <summary>
		/// Fills the passed in grid with the definitions in the passed in list.
		/// </summary>
		public static void FillGridDefs(ODGrid definitionsGrid, DefCatOptions selectedDefCatOpt, List<Definition> definitions)
		{
			Definition definition = null;
			if (definitionsGrid.GetSelectedIndex() > -1)
			{
				definition = (Definition)definitionsGrid.ListGridRows[definitionsGrid.GetSelectedIndex()].Tag;
			}

			int scroll = definitionsGrid.ScrollValue;

			definitionsGrid.BeginUpdate();
			definitionsGrid.ListGridColumns.Clear();
			definitionsGrid.ListGridColumns.Add(new GridColumn("Name", 190));
			definitionsGrid.ListGridColumns.Add(new GridColumn(selectedDefCatOpt.ValueText, 190));
			definitionsGrid.ListGridColumns.Add(new GridColumn(selectedDefCatOpt.EnableColor ? "Color" : "", 40));
			definitionsGrid.ListGridColumns.Add(new GridColumn(selectedDefCatOpt.CanHide ? "Hide" : "", 30, HorizontalAlignment.Center));
			definitionsGrid.ListGridRows.Clear();

			foreach (var def in definitions)
			{
				if (Definitions.IsDefDeprecated(def))
				{
					def.IsHidden = true;
				}

				var row = new GridRow();
				row.Cells.Add(def.Name);

				if (selectedDefCatOpt.DefCat == DefinitionCategory.ImageCats)
				{
					row.Cells.Add(GetItemDescForImages(def.Value));
				}
				else if (selectedDefCatOpt.DefCat == DefinitionCategory.AutoNoteCats)
				{
					Dictionary<string, string> dictAutoNoteDefs = new Dictionary<string, string>();
					dictAutoNoteDefs = definitions.ToDictionary(x => x.Id.ToString(), x => x.Name);
                    row.Cells.Add(dictAutoNoteDefs.TryGetValue(def.Value, out string nameCur) ? nameCur : def.Value);
                }
				else if (selectedDefCatOpt.DefCat == DefinitionCategory.WebSchedNewPatApptTypes)
				{
					AppointmentType appointmentType = AppointmentTypes.GetWebSchedNewPatApptTypeByDef(def.Id);
					row.Cells.Add(appointmentType == null ? "" : appointmentType.Name);
				}
				else if (selectedDefCatOpt.DoShowItemOrderInValue)
				{
					row.Cells.Add(def.SortOrder.ToString());
				}
				else
				{
					row.Cells.Add(def.Value);
				}

				row.Cells.Add("");
				if (selectedDefCatOpt.EnableColor)
				{
					row.Cells[row.Cells.Count - 1].BackColor = def.Color;
				}

				row.Cells.Add(def.IsHidden ? "X" : "");
				row.Tag = def;

				definitionsGrid.ListGridRows.Add(row);
			}

			definitionsGrid.EndUpdate();
			if (definition != null)
			{
				for (int i = 0; i < definitionsGrid.ListGridRows.Count; i++)
				{
					if (((Definition)definitionsGrid.ListGridRows[i].Tag).Id == definition.Id)
					{
						definitionsGrid.SetSelected(i, true);
						break;
					}
				}
			}

			definitionsGrid.ScrollValue = scroll;
		}

		public static bool GridDefsDoubleClick(Definition selectedDef, DefCatOptions selectedDefCatOpt, List<Definition> listDefsCur, List<Definition> listDefsAll, bool isDefChanged)
		{
			switch (selectedDefCatOpt.DefCat)
			{
				case DefinitionCategory.BlockoutTypes:
					using (var formDefEditBlockout = new FormDefEditBlockout(selectedDef))
					{
						if (formDefEditBlockout.ShowDialog() == DialogResult.OK)
						{
							isDefChanged = true;
						}
					}
					break;

				case DefinitionCategory.ImageCats:
					using (var formDefEditImages = new FormDefEditImages(selectedDef))
					{
						formDefEditImages.IsNew = false;

						if (formDefEditImages.ShowDialog() == DialogResult.OK)
						{
							isDefChanged = true;
						}
					}
					break;

				case DefinitionCategory.WebSchedNewPatApptTypes:
					using (var formDefEditWSNPApptTypes = new FormDefEditWSNPApptTypes(selectedDef))
					{
						if (formDefEditWSNPApptTypes.ShowDialog() == DialogResult.OK)
						{
							if (formDefEditWSNPApptTypes.IsDeleted)
							{
								listDefsAll.Remove(selectedDef);
							}
							isDefChanged = true;
						}
					}
					break;

				default:
					using (var formDefEdit = new FormDefEdit(selectedDef, listDefsCur, selectedDefCatOpt))
					{
						formDefEdit.IsNew = false;
						if (formDefEdit.ShowDialog() == DialogResult.OK)
						{
							if (formDefEdit.IsDeleted)
							{
								listDefsAll.Remove(selectedDef);
							}

							isDefChanged = true;
						}
					}
					break;
			}

			return isDefChanged;
		}

		public static bool AddDef(ODGrid gridDefs, DefCatOptions selectedDefCatOpt)
		{
			int itemOrder = 0;
			if (Definitions.GetDefsForCategory(selectedDefCatOpt.DefCat).Count > 0)
			{
				itemOrder = Definitions.GetDefsForCategory(selectedDefCatOpt.DefCat).Max(x => x.SortOrder) + 1;
			}

            var newDef = new Definition
            {
                SortOrder = itemOrder,
                Category = selectedDefCatOpt.DefCat,
                Name = "",
                Value = ""
            };

            if (selectedDefCatOpt.DefCat == DefinitionCategory.InsurancePaymentType)
			{
				newDef.Value = "N";
			}

			switch (selectedDefCatOpt.DefCat)
			{
				case DefinitionCategory.BlockoutTypes:
					using (var formDefEditBlockout = new FormDefEditBlockout(newDef))
					{
						formDefEditBlockout.IsNew = true;
						if (formDefEditBlockout.ShowDialog() != DialogResult.OK)
						{
							return false;
						}
					}
					break;

				case DefinitionCategory.ImageCats:
					using (var formDefEditImages = new FormDefEditImages(newDef))
					{
						formDefEditImages.IsNew = true;
						if (formDefEditImages.ShowDialog() != DialogResult.OK)
						{
							return false;
						}
					}
					break;

				case DefinitionCategory.WebSchedNewPatApptTypes:
					using (var formDefEditWSNPAppt = new FormDefEditWSNPApptTypes(newDef))
					{
						if (formDefEditWSNPAppt.ShowDialog() != DialogResult.OK)
						{
							return false;
						}
					}
					break;

				default:
					var currentDefs = new List<Definition>();
					foreach (var gridRow in gridDefs.ListGridRows)
					{
						currentDefs.Add((Definition)gridRow.Tag);
					}

					using (var formDefEdit = new FormDefEdit(newDef, currentDefs, selectedDefCatOpt))
					{
						formDefEdit.IsNew = true;
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
		/// Will attempt to hide the currently selected definition of the ODGrid that is passed in.
		/// </summary>
		public static bool TryHideDefSelectedInGrid(ODGrid gridDefs, DefCatOptions selectedDefCatOpt)
		{
			if (gridDefs.GetSelectedIndex() == -1)
			{
				ODMessageBox.Show("Please select item first,");
				return false;
			}

			Definition defSelected = (Definition)gridDefs.ListGridRows[gridDefs.GetSelectedIndex()].Tag;
			if (!CanHideDef(defSelected, selectedDefCatOpt))
			{
				return false;
			}

			Definitions.HideDef(defSelected);
			return true;
		}

		///<summary>Returns true if definition can be hidden or is already hidden. Displays error message and returns false if not.</summary>
		public static bool CanHideDef(Definition def, DefCatOptions defCatOpt)
		{
			if (def.IsHidden)
			{
				// Return true if Def is already hidden.
				return true;
			}

			if (!defCatOpt.CanHide || !defCatOpt.CanEditName)
			{
				ODMessageBox.Show("Definitions of this category cannot be hidden.");
				return false; // We should never get here, but if we do, something went wrong because the definition shouldn't have been hideable
			}

			// Stop users from hiding the last definition in categories that must have at least one def in them.
			List<Definition> listDefsCurNotHidden = Definitions.GetDefsForCategory(defCatOpt.DefCat, true);
			if (Definitions.NeedOneUnhidden(def.Category) && listDefsCurNotHidden.Count == 1)
			{
				ODMessageBox.Show("You cannot hide the last definition in this category.");
				return false;
			}

			if (def.Category == DefinitionCategory.ProviderSpecialties && (Providers.IsSpecialtyInUse(def.Id) || Referrals.IsSpecialtyInUse(def.Id)))
			{
				ODMessageBox.Show("You cannot hide a specialty if it is in use by a provider or a referral source.");
				return false;
			}

			if (Definitions.IsDefinitionInUse(def))
			{
				// DefNum will be zero if it is being created but hasn't been saved to DB yet, thus it can't be in use.
				if (def.Id.In(
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
				else if (def.Id.In(
					Prefs.GetLong(PrefName.RecallStatusMailed),
					Prefs.GetLong(PrefName.RecallStatusTexted),
					Prefs.GetLong(PrefName.RecallStatusEmailed),
					Prefs.GetLong(PrefName.RecallStatusEmailedTexted)))
				{
					ODMessageBox.Show("You cannot hide a definition that is used as a status in the Setup Recall window.");
					return false;
				}
				else if (def.Id == Prefs.GetLong(PrefName.WebSchedNewPatConfirmStatus))
				{
					ODMessageBox.Show("You cannot hide a definition that is used as an appointment confirmation status in Web Sched New Pat Appt.");
					return false;
				}
				else if (def.Id == Prefs.GetLong(PrefName.WebSchedRecallConfirmStatus))
				{
					ODMessageBox.Show("You cannot hide a definition that is used as an appointment confirmation status in Web Sched Recall Appt.");
					return false;
				}
				else if (def.Id == Prefs.GetLong(PrefName.PracticeDefaultBillType))
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

			if (def.Category == DefinitionCategory.PaySplitUnearnedType)
			{
				if (listDefsCurNotHidden.FindAll(x => string.IsNullOrEmpty(x.Value)).Count == 1 && def.Value == "")
				{
					ODMessageBox.Show("Must have at least one definition that shows in Account");
					return false;
				}
				if (listDefsCurNotHidden.FindAll(x => !string.IsNullOrEmpty(x.Value)).Count == 1 && def.Value != "")
				{
					ODMessageBox.Show("Must have at least one definition that does not show in Account");
					return false;
				}
			}

			// Warn the user if they are about to hide a billing type currently in use.
			if (defCatOpt.DefCat == DefinitionCategory.BillingTypes && Patients.IsBillingTypeInUse(def.Id))
			{
				if (!MsgBox.Show(MsgBoxButtons.OKCancel, "Warning: Billing type is currently in use by patients, insurance plans, or preferences."))
				{
					return false;
				}
			}
			return true;
		}

		public static bool UpClick(ODGrid gridDefs)
		{
			if (gridDefs.GetSelectedIndex() == -1)
			{
				ODMessageBox.Show("Please select an item first.");
				return false;
			}

			if (gridDefs.GetSelectedIndex() == 0)
			{
				return false;
			}

			Definition defSelected = (Definition)gridDefs.ListGridRows[gridDefs.GetSelectedIndex()].Tag;
			Definition defAbove = (Definition)gridDefs.ListGridRows[gridDefs.GetSelectedIndex() - 1].Tag;
			int indexDefSelectedItemOrder = defSelected.SortOrder;
			defSelected.SortOrder = defAbove.SortOrder;
			defAbove.SortOrder = indexDefSelectedItemOrder;
			Definitions.Update(defSelected);
			Definitions.Update(defAbove);
			return true;
		}

		public static bool DownClick(ODGrid gridDefs)
		{
			if (gridDefs.GetSelectedIndex() == -1)
			{
				ODMessageBox.Show("Please select an item first.");
				return false;
			}

			if (gridDefs.GetSelectedIndex() == gridDefs.ListGridRows.Count - 1)
			{
				return false;
			}

			Definition defSelected = (Definition)gridDefs.ListGridRows[gridDefs.GetSelectedIndex()].Tag;
			Definition defBelow = (Definition)gridDefs.ListGridRows[gridDefs.GetSelectedIndex() + 1].Tag;
			int indexDefSelectedItemOrder = defSelected.SortOrder;
			defSelected.SortOrder = defBelow.SortOrder;
			defBelow.SortOrder = indexDefSelectedItemOrder;
			Definitions.Update(defSelected);
			Definitions.Update(defBelow);
			return true;
		}
	}
}
