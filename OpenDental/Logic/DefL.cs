using CodeBase;
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
		public static List<DefCatOptions> GetOptionsForDefCats(Array defCats)
		{
			var defCatOptions = new List<DefCatOptions>();
			foreach (DefCat defCatCur in defCats)
			{
				if (defCatCur.GetDescription() == "NotUsed")
				{
					continue;
				}

				var options = new DefCatOptions(defCatCur);
				switch (defCatCur)
				{
					case DefCat.AccountColors:
						options.CanEditName = false;
						options.EnableColor = true;
						options.HelpText = "Changes the color of text for different types of entries in Account Module";
						break;

					case DefCat.AccountQuickCharge:
						options.CanDelete = true;
						options.EnableValue = true;
						options.ValueText = "Procedure Codes";
						options.HelpText = "Account Proc Quick Add items.  Each entry can be a series of procedure codes separated by commas (e.g. D0180,D1101,D8220).  Used in the account module to quickly charge patients for items.";
						break;

					case DefCat.AdjTypes:
						options.EnableValue = true;
						options.ValueText =  "+, -, or dp";
						options.HelpText =  "Plus increases the patient balance.  Minus decreases it.  Dp means discount plan.  Not allowed to change value after creating new type since changes affect all patient accounts.";
						break;

					case DefCat.AppointmentColors:
						options.CanEditName = false;
						options.EnableColor = true;
						options.HelpText = "Changes colors of background in Appointments Module, and colors for completed appointments.";
						break;

					case DefCat.ApptConfirmed:
						options.EnableColor = true;
						options.EnableValue = true;
						options.ValueText = "Abbrev";
						options.HelpText = "Color shows on each appointment if Appointment View is set to show ConfirmedColor.";
						break;

					case DefCat.ApptProcsQuickAdd:
						options.EnableValue = true;
						options.ValueText = "ADA Code(s)";
						if (Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum))
						{
							options.HelpText = "These are the procedures that you can quickly add to the treatment plan from within the appointment editing window.  Multiple procedures may be separated by commas with no spaces. These definitions may be freely edited without affecting any patient records.";
						}
						else
						{
							options.HelpText = "These are the procedures that you can quickly add to the treatment plan from within the appointment editing window.  They must not require a tooth number. Multiple procedures may be separated by commas with no spaces. These definitions may be freely edited without affecting any patient records.";
						}
						break;
					case DefCat.AutoDeposit:
						options.CanDelete = true;
						options.CanHide = true;
						options.EnableValue = true;
						options.ValueText = "Account Number";
						break;

					case DefCat.AutoNoteCats:
						options.CanDelete = true;
						options.CanHide = false;
						options.EnableValue = true;
						options.IsValueDefNum = true;
						options.ValueText =  "Parent Category";
						options.HelpText =  "Leave the Parent Category blank for categories at the root level. Assign a Parent Category to move a category within another. The order set here will only affect the order within the assigned Parent Category in the Auto Note list. For example, a category may be moved above its parent in this list, but it will still be within its Parent Category in the Auto Note list.";
						break;

					case DefCat.BillingTypes:
						options.EnableValue = true;
						options.ValueText = "E, C, or CE";
						options.HelpText = "E=Email bill, C=Collection, CE=Collection Excluded.  It is recommended to use as few billing types as possible.  They can be useful when running reports to separate delinquent accounts, but can cause 'forgotten accounts' if used without good office procedures. Changes affect all patients.";
						break;

					case DefCat.BlockoutTypes:
						options.EnableColor = true;
						options.HelpText = "Blockout types are used in the appointments module.";
						options.EnableValue = true;
						options.ValueText = "Flags";
						break;

					case DefCat.ChartGraphicColors:
						options.CanEditName = false;
						options.EnableColor = true;
						if (Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum))
						{
							options.HelpText = "These colors will be used to graphically display treatments.";
						}
						else
						{
							options.HelpText = "These colors will be used on the graphical tooth chart to draw restorations.";
						}
						break;

					case DefCat.ClaimCustomTracking:
						options.CanDelete = true;
						options.CanHide = false;
						options.EnableValue = true;
						options.ValueText =  "Days Suppressed";
						options.HelpText = "Some offices may set up claim tracking statuses such as 'review', 'hold', 'riskmanage', etc." + "\r\n" +
							"Set the value of 'Days Suppressed' to the number of days the claim will be suppressed from the Outstanding Claims Report " +
							"when the status is changed to the selected status.";
						break;

					case DefCat.ClaimErrorCode:
						options.CanDelete = true;
						options.CanHide = false;
						options.EnableValue = true;
						options.ValueText = "Description";
						options.HelpText =  "Used to track error codes when entering claim custom statuses.";
						break;

					case DefCat.ClaimPaymentTracking:
						options.ValueText = "Value";
						options.HelpText =  "EOB adjudication method codes to be used for insurance payments.  Last entry cannot be hidden.";
						break;

					case DefCat.ClaimPaymentGroups:
						options.ValueText =  "Value";
						options.HelpText =  "Used to group claim payments in the daily payments report.";
						break;

					case DefCat.ClinicSpecialty:
						options.CanHide = true;
						options.CanDelete = false;
						options.HelpText = "You can add as many specialties as you want.  Changes affect all current records.";
						break;

					case DefCat.CommLogTypes:
						options.EnableValue = true;
						options.EnableColor = true;
						options.DoShowNoColor = true;
						string commItemTypes = string.Join(", ", Commlogs.GetCommItemTypes().Select(x => x.GetDescription(useShortVersionIfAvailable: true)));
						options.ValueText = "Usage";
						options.HelpText = "Changes affect all current commlog entries.  Optionally set Usage to one of the following: " + commItemTypes + 
							". Only one of each. This helps automate new entries.";
						break;

					case DefCat.ContactCategories:
						options.HelpText = "You can add as many categories as you want.  Changes affect all current contact records.";
						break;

					case DefCat.Diagnosis:
						options.EnableValue = true;
						options.ValueText = "1 or 2 letter abbreviation";
						options.HelpText = "The diagnosis list is shown when entering a procedure.  Ones that are less used should go lower on the list.  The abbreviation is shown in the progress notes.  BE VERY CAREFUL.  Changes affect all patients.";
						break;

					case DefCat.FeeColors:
						options.CanEditName = false;
						options.CanHide = false;
						options.EnableColor = true;
						options.HelpText = "These are the colors associated to fee types.";
						break;

					case DefCat.ImageCats:
						options.ValueText = "Usage";
						options.HelpText = "These are the categories that will be available in the image and chart modules.  If you hide a category, images in that category will be hidden, so only hide a category if you are certain it has never been used.  Multiple categories can be set to show in the Chart module, but only one category should be set for patient pictures, statements, and tooth charts. Selecting multiple categories for treatment plans will save the treatment plan in each category. Affects all patient records.";
						break;

					case DefCat.InsurancePaymentType:
						options.CanDelete = true;
						options.CanHide = false;
						options.EnableValue = true;
						options.ValueText = "N=Not selected for deposit";
						options.HelpText = "These are claim payment types for insurance payments attached to claims.";
						break;

					case DefCat.InsuranceVerificationStatus:
						options.ValueText = "Usage";
						options.HelpText = "These are statuses for the insurance verification list.";
						break;

					case DefCat.JobPriorities:
						options.CanDelete = false;
						options.CanHide = true;
						options.EnableValue = true;
						options.EnableColor = true;
						options.ValueText = "Comma-delimited keywords";
						options.HelpText = "These are job priorities that determine how jobs are sorted in the Job Manager System.  Required values are: OnHold, Low, Normal, MediumHigh, High, Urgent, BugDefault, JobDefault, DocumentationDefault.";
						break;

					case DefCat.LetterMergeCats:
						options.HelpText = "Categories for Letter Merge.  You can safely make any changes you want.";
						break;

					case DefCat.MiscColors:
						options.CanEditName = false;
						options.EnableColor = true;
						options.HelpText = "";
						break;

					case DefCat.PaymentTypes:
						options.EnableValue = true;
						options.ValueText = "N=Not selected for deposit";
						options.HelpText = "Types of payments that patients might make. Any changes will affect all patients.";
						break;

					case DefCat.PayPlanCategories:
						options.HelpText = "Assign payment plans to different categories";
						break;

					case DefCat.PaySplitUnearnedType:
						options.ValueText = "Do Not Show on Account";
						options.HelpText = "Usually only used by offices that use accrual basis accounting instead of cash basis accounting. Any changes will affect all patients.";
						options.EnableValue = true;
						break;

					case DefCat.ProcButtonCats:
						options.HelpText = "These are similar to the procedure code categories, but are only used for organizing and grouping the procedure buttons in the Chart module.";
						break;

					case DefCat.ProcCodeCats:
						options.HelpText = "These are the categories for organizing procedure codes. They do not have to follow ADA categories.  There is no relationship to insurance categories which are setup in the Ins Categories section.  Does not affect any patient records.";
						break;

					case DefCat.ProgNoteColors:
						options.CanEditName = false;
						options.EnableColor = true;
						options.HelpText = "Changes color of text for different types of entries in the Chart Module Progress Notes.";
						break;

					case DefCat.Prognosis:
						break;

					case DefCat.ProviderSpecialties:
						options.HelpText = "Provider specialties cannot be deleted.  Changes to provider specialties could affect e-claims.";
						break;

					case DefCat.RecallUnschedStatus:
						options.EnableValue = true;
						options.ValueText = "Abbreviation";
						options.HelpText = "Recall/Unsched Status.  Abbreviation must be 7 characters or less.  Changes affect all patients.";
						break;

					case DefCat.Regions:
						options.CanHide = false;
						options.HelpText = "The region identifying the clinic it is assigned to.";
						break;

					case DefCat.SupplyCats:
						options.CanDelete = true;
						options.CanHide = false;
						options.HelpText = "The categories for inventory supplies.";
						break;

					case DefCat.TaskPriorities:
						options.EnableColor = true;
						options.EnableValue = true;
						options.ValueText = "D = Default, R = Reminder";
						options.HelpText = "Priorities available for selection within the task edit window.  Task lists are sorted using the order of these priorities.  They can have any description and color.  At least one priority should be Default (D).  If more than one priority is flagged as the default, the last default in the list will be used.  If no default is set, the last priority will be used.  Use (R) to indicate the initial reminder task priority to use when creating reminder tasks.  Changes affect all tasks where the definition is used.";
						break;

					case DefCat.TxPriorities:
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

					case DefCat.WebSchedNewPatApptTypes:
						options.CanDelete = true;
						options.CanHide = false;
						options.ValueText = "Appointment Type";
						options.HelpText = "Appointment types to be displayed in the Web Sched New Pat Appt web application.  These are selectable for the new patients and will be saved to the appointment note.";
						break;

					case DefCat.CarrierGroupNames:
						options.CanHide = true;
						options.HelpText = "These are group names for Carriers.";
						break;

					case DefCat.TimeCardAdjTypes:
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
		public static void FillGridDefs(ODGrid definitionsGrid, DefCatOptions selectedDefCatOpt, List<Def> definitions)
		{
			Def definition = null;
			if (definitionsGrid.GetSelectedIndex() > -1)
			{
				definition = (Def)definitionsGrid.ListGridRows[definitionsGrid.GetSelectedIndex()].Tag;
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
				if (Defs.IsDefDeprecated(def))
				{
					def.IsHidden = true;
				}

				var row = new GridRow();
				row.Cells.Add(def.ItemName);

				if (selectedDefCatOpt.DefCat == DefCat.ImageCats)
				{
					row.Cells.Add(GetItemDescForImages(def.ItemValue));
				}
				else if (selectedDefCatOpt.DefCat == DefCat.AutoNoteCats)
				{
					Dictionary<string, string> dictAutoNoteDefs = new Dictionary<string, string>();
					dictAutoNoteDefs = definitions.ToDictionary(x => x.DefNum.ToString(), x => x.ItemName);
                    row.Cells.Add(dictAutoNoteDefs.TryGetValue(def.ItemValue, out string nameCur) ? nameCur : def.ItemValue);
                }
				else if (selectedDefCatOpt.DefCat == DefCat.WebSchedNewPatApptTypes)
				{
					AppointmentType appointmentType = AppointmentTypes.GetWebSchedNewPatApptTypeByDef(def.DefNum);
					row.Cells.Add(appointmentType == null ? "" : appointmentType.Name);
				}
				else if (selectedDefCatOpt.DoShowItemOrderInValue)
				{
					row.Cells.Add(def.ItemOrder.ToString());
				}
				else
				{
					row.Cells.Add(def.ItemValue);
				}

				row.Cells.Add("");
				if (selectedDefCatOpt.EnableColor)
				{
					row.Cells[row.Cells.Count - 1].BackColor = def.ItemColor;
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
					if (((Def)definitionsGrid.ListGridRows[i].Tag).DefNum == definition.DefNum)
					{
						definitionsGrid.SetSelected(i, true);
						break;
					}
				}
			}

			definitionsGrid.ScrollValue = scroll;
		}

		public static bool GridDefsDoubleClick(Def selectedDef, DefCatOptions selectedDefCatOpt, List<Def> listDefsCur, List<Def> listDefsAll, bool isDefChanged)
		{
			switch (selectedDefCatOpt.DefCat)
			{
				case DefCat.BlockoutTypes:
					using (var formDefEditBlockout = new FormDefEditBlockout(selectedDef))
					{
						if (formDefEditBlockout.ShowDialog() == DialogResult.OK)
						{
							isDefChanged = true;
						}
					}
					break;

				case DefCat.ImageCats:
					using (var formDefEditImages = new FormDefEditImages(selectedDef))
					{
						formDefEditImages.IsNew = false;

						if (formDefEditImages.ShowDialog() == DialogResult.OK)
						{
							isDefChanged = true;
						}
					}
					break;

				case DefCat.WebSchedNewPatApptTypes:
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
            Def newDef = new Def
            {
                IsNew = true
            };

            int itemOrder = 0;
			if (Defs.GetDefsForCategory(selectedDefCatOpt.DefCat).Count > 0)
			{
				itemOrder = Defs.GetDefsForCategory(selectedDefCatOpt.DefCat).Max(x => x.ItemOrder) + 1;
			}

			newDef.ItemOrder = itemOrder;
			newDef.Category = selectedDefCatOpt.DefCat;
			newDef.ItemName = "";
			newDef.ItemValue = "";

			if (selectedDefCatOpt.DefCat == DefCat.InsurancePaymentType)
			{
				newDef.ItemValue = "N";
			}

			switch (selectedDefCatOpt.DefCat)
			{
				case DefCat.BlockoutTypes:
					using (var formDefEditBlockout = new FormDefEditBlockout(newDef))
					{
						formDefEditBlockout.IsNew = true;
						if (formDefEditBlockout.ShowDialog() != DialogResult.OK)
						{
							return false;
						}
					}
					break;

				case DefCat.ImageCats:
					using (var formDefEditImages = new FormDefEditImages(newDef))
					{
						formDefEditImages.IsNew = true;
						if (formDefEditImages.ShowDialog() != DialogResult.OK)
						{
							return false;
						}
					}
					break;

				case DefCat.WebSchedNewPatApptTypes:
					using (var formDefEditWSNPAppt = new FormDefEditWSNPApptTypes(newDef))
					{
						if (formDefEditWSNPAppt.ShowDialog() != DialogResult.OK)
						{
							return false;
						}
					}
					break;

				default:
					var currentDefs = new List<Def>();
					foreach (var gridRow in gridDefs.ListGridRows)
					{
						currentDefs.Add((Def)gridRow.Tag);
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

			Def defSelected = (Def)gridDefs.ListGridRows[gridDefs.GetSelectedIndex()].Tag;
			if (!CanHideDef(defSelected, selectedDefCatOpt))
			{
				return false;
			}

			Defs.HideDef(defSelected);
			return true;
		}

		///<summary>Returns true if definition can be hidden or is already hidden. Displays error message and returns false if not.</summary>
		public static bool CanHideDef(Def def, DefCatOptions defCatOpt)
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
			List<Def> listDefsCurNotHidden = Defs.GetDefsForCategory(defCatOpt.DefCat, true);
			if (Defs.NeedOneUnhidden(def.Category) && listDefsCurNotHidden.Count == 1)
			{
				ODMessageBox.Show("You cannot hide the last definition in this category.");
				return false;
			}

			if (def.Category == DefCat.ProviderSpecialties && (Providers.IsSpecialtyInUse(def.DefNum) || Referrals.IsSpecialtyInUse(def.DefNum)))
			{
				ODMessageBox.Show("You cannot hide a specialty if it is in use by a provider or a referral source.");
				return false;
			}

			if (Defs.IsDefinitionInUse(def))
			{
				// DefNum will be zero if it is being created but hasn't been saved to DB yet, thus it can't be in use.
				if (def.DefNum.In(
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
				else if (def.DefNum.In(
					Prefs.GetLong(PrefName.RecallStatusMailed),
					Prefs.GetLong(PrefName.RecallStatusTexted),
					Prefs.GetLong(PrefName.RecallStatusEmailed),
					Prefs.GetLong(PrefName.RecallStatusEmailedTexted)))
				{
					ODMessageBox.Show("You cannot hide a definition that is used as a status in the Setup Recall window.");
					return false;
				}
				else if (def.DefNum == Prefs.GetLong(PrefName.WebSchedNewPatConfirmStatus))
				{
					ODMessageBox.Show("You cannot hide a definition that is used as an appointment confirmation status in Web Sched New Pat Appt.");
					return false;
				}
				else if (def.DefNum == Prefs.GetLong(PrefName.WebSchedRecallConfirmStatus))
				{
					ODMessageBox.Show("You cannot hide a definition that is used as an appointment confirmation status in Web Sched Recall Appt.");
					return false;
				}
				else if (def.DefNum == Prefs.GetLong(PrefName.PracticeDefaultBillType))
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

			if (def.Category == DefCat.PaySplitUnearnedType)
			{
				if (listDefsCurNotHidden.FindAll(x => string.IsNullOrEmpty(x.ItemValue)).Count == 1 && def.ItemValue == "")
				{
					ODMessageBox.Show("Must have at least one definition that shows in Account");
					return false;
				}
				if (listDefsCurNotHidden.FindAll(x => !string.IsNullOrEmpty(x.ItemValue)).Count == 1 && def.ItemValue != "")
				{
					ODMessageBox.Show("Must have at least one definition that does not show in Account");
					return false;
				}
			}

			// Warn the user if they are about to hide a billing type currently in use.
			if (defCatOpt.DefCat == DefCat.BillingTypes && Patients.IsBillingTypeInUse(def.DefNum))
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

			Def defSelected = (Def)gridDefs.ListGridRows[gridDefs.GetSelectedIndex()].Tag;
			Def defAbove = (Def)gridDefs.ListGridRows[gridDefs.GetSelectedIndex() - 1].Tag;
			int indexDefSelectedItemOrder = defSelected.ItemOrder;
			defSelected.ItemOrder = defAbove.ItemOrder;
			defAbove.ItemOrder = indexDefSelectedItemOrder;
			Defs.Update(defSelected);
			Defs.Update(defAbove);
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

			Def defSelected = (Def)gridDefs.ListGridRows[gridDefs.GetSelectedIndex()].Tag;
			Def defBelow = (Def)gridDefs.ListGridRows[gridDefs.GetSelectedIndex() + 1].Tag;
			int indexDefSelectedItemOrder = defSelected.ItemOrder;
			defSelected.ItemOrder = defBelow.ItemOrder;
			defBelow.ItemOrder = indexDefSelectedItemOrder;
			Defs.Update(defSelected);
			Defs.Update(defBelow);
			return true;
		}
	}
}
