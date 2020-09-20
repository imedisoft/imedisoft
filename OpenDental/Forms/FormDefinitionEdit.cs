using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
	public partial class FormDefinitionEdit : FormBase
	{
		private readonly Definition definition;
		private readonly DefinitionCategoryOptions categoryOptions;
		private readonly List<Definition> definitions;
		private List<long> excludeSendIds;
		private List<long> excludeConfirmIds;
		private List<long> excludeReminderIds;
		private List<long> excludeThankYouIds;
		private List<long> excludeArrivalSendIds;
		private List<long> excludeArrivelResponseIds;

		/// <summary>
		/// Gets a value indicating whether the definition was deleted.
		/// </summary>
		public bool IsDeleted { get; private set; } = false;

		/// <summary>
		/// defCur should be the currently selected def from FormDefinitions.
		/// defList is going to be the in-memory list of definitions currently displaying to the user. 
		/// defList typically is out of synch with the cache which is why we need to pass it in.
		/// </summary>
		public FormDefinitionEdit(Definition definition, List<Definition> definitions, DefinitionCategoryOptions categoryOptions)
		{
			InitializeComponent();

			this.definition = definition;
			this.categoryOptions = categoryOptions;
			this.definitions = definitions;
		}

		private static bool IsDefinitionInUse(long definitionId, params string[] preferenceNames)
        {
			foreach (var preferenceName in preferenceNames)
            {
				if (Preferences.GetLong(preferenceName) == definitionId)
                {
					return true;
                }
            }

			return false;
        }

		private void FormDefEdit_Load(object sender, EventArgs e)
		{
			if (definition.Category == DefinitionCategory.ApptConfirmed)
			{
				excludeSendIds = Preferences.GetString(PreferenceName.ApptConfirmExcludeESend).Split(',').ToList().Select(x => PIn.Long(x)).ToList();
				excludeConfirmIds = Preferences.GetString(PreferenceName.ApptConfirmExcludeEConfirm).Split(',').ToList().Select(x => PIn.Long(x)).ToList();
				excludeReminderIds = Preferences.GetString(PreferenceName.ApptConfirmExcludeERemind).Split(',').ToList().Select(x => PIn.Long(x)).ToList();
				excludeThankYouIds = Preferences.GetString(PreferenceName.ApptConfirmExcludeEThankYou).Split(',').ToList().Select(x => PIn.Long(x)).ToList();
				excludeArrivalSendIds = Preferences.GetString(PreferenceName.ApptConfirmExcludeArrivalSend).Split(',').ToList().Select(x => PIn.Long(x)).ToList();
				excludeArrivelResponseIds = Preferences.GetString(PreferenceName.ApptConfirmExcludeArrivalResponse).Split(',').ToList().Select(x => PIn.Long(x)).ToList();

				// 0 will get automatically added to the list when this is the first of its kind.  We never want 0 inserted.
				excludeSendIds.Remove(0);
				excludeConfirmIds.Remove(0);
				excludeReminderIds.Remove(0);
				excludeThankYouIds.Remove(0);
				excludeArrivalSendIds.Remove(0);
				excludeArrivalSendIds.Remove(0);

				checkExcludeSend.Checked = excludeSendIds.Contains(definition.Id);
				checkExcludeConfirm.Checked = excludeConfirmIds.Contains(definition.Id);
				checkExcludeRemind.Checked = excludeReminderIds.Contains(definition.Id);
				checkExcludeThanks.Checked = excludeThankYouIds.Contains(definition.Id);
				checkExcludeArrivalSend.Checked = excludeArrivalSendIds.Contains(definition.Id);
				checkExcludeArrivalResponse.Checked = excludeArrivelResponseIds.Contains(definition.Id);
			}
			else
			{
				groupEConfirm.Visible = false;
				groupBoxEReminders.Visible = false;
				groupBoxEThanks.Visible = false;
				groupBoxArrivals.Visible = false;
			}

			// We never want to send confirmation or reminders to an appointment when it is in a triggered confirm status.
			if (IsDefinitionInUse(definition.Id, 
				PreferenceName.AppointmentTimeArrivedTrigger,
				PreferenceName.AppointmentTimeDismissedTrigger,
				PreferenceName.AppointmentTimeSeatedTrigger))
			{
				checkExcludeConfirm.Enabled = false;
				checkExcludeRemind.Enabled = false;
				checkExcludeSend.Enabled = false;
				checkExcludeThanks.Enabled = false;
				checkExcludeArrivalSend.Enabled = false;
				checkExcludeArrivalResponse.Enabled = false;
				checkExcludeConfirm.Checked = true;
				checkExcludeRemind.Checked = true;
				checkExcludeSend.Checked = true;
				checkExcludeThanks.Checked = true;
				checkExcludeArrivalSend.Checked = true;
				checkExcludeArrivalResponse.Checked = true;
			}

			string itemName = definition.Name;

			if (!categoryOptions.CanEditName)
			{
				// Allow foreign users to translate definitions that they do not have access to translate.
				// Use FormDefinitions instead of 'this' because the users will have already translated the item names in that form and no need to duplicate.
				itemName = definition.Name;

				nameTextBox.ReadOnly = true;
				if (!definition.IsHidden || Definitions.IsDefDeprecated(definition))
				{
					hiddenCheckBox.Enabled = false; // prevent hiding defs that are hard-coded into OD. Prevent unhiding defs that are deprecated.
				}
			}

			valueLabel.Text = categoryOptions.ValueText;
			if (definition.Category == DefinitionCategory.AdjTypes && definition.Id > 0)
			{
				valueLabel.Text = "Not allowed to change type after an adjustment is created.";
				valueTextBox.Visible = false;
			}

			if (definition.Category == DefinitionCategory.BillingTypes)
			{
				valueLabel.Text = "E=Email bill, C=Collection, CE=Collection Excluded";
			}

			if (definition.Category == DefinitionCategory.PaySplitUnearnedType)
			{
				valueLabel.Text = "X=Do Not Show in Account or on Reports";
			}

			if (!categoryOptions.EnableValue)
			{
				valueLabel.Visible = false;
				valueTextBox.Visible = false;
			}

			if (!categoryOptions.EnableColor)
			{
				labelColor.Visible = false;
				colorButton.Visible = false;
			}

			hiddenCheckBox.Checked = definition.IsHidden;
			hiddenCheckBox.Visible = categoryOptions.CanHide;

			deleteButton.Visible = categoryOptions.CanDelete;
			
			if (categoryOptions.IsValueDefNum)
			{
				valueTextBox.ReadOnly = true;
				valueTextBox.BackColor = SystemColors.Control;
				valueLabel.Text = "Use the select button to choose a definition from the list.";

				selectButton.Visible = true;
				clearButton.Visible = true;
			}
			else if (categoryOptions.DoShowItemOrderInValue)
			{
				valueLabel.Text = "Internal Priority";
				valueTextBox.Text = definition.SortOrder.ToString();
				valueTextBox.ReadOnly = true;
				selectButton.Visible = false;
				clearButton.Visible = false;
			}
			else
			{
				selectButton.Visible = false;
				clearButton.Visible = false;
			}

			nameTextBox.Text = itemName;
			valueTextBox.Text = definition.Value;
			colorButton.BackColor = definition.Color;

			if (categoryOptions.DoShowNoColor)
			{
				noColorCheckBox.Visible = true;

				noColorCheckBox.Checked = definition.Color.ToArgb() == Color.Empty.ToArgb();
			}
		}

		private void ColorButton_Click(object sender, EventArgs e)
		{
			using var colorDialog = new ColorDialog
			{
				Color = colorButton.BackColor
			};

			if (colorDialog.ShowDialog(this) != DialogResult.OK)
            {
				return;
            }

			colorButton.BackColor = colorDialog.Color;
			noColorCheckBox.Checked = colorDialog.Color == Color.Empty;
		}

		private void SelectButton_Click(object sender, EventArgs e)
		{
			if (!long.TryParse(this.definition.Value, out var parentId)) return;

            using var formDefinitionPicker = new FormDefinitionPicker(this.definition.Category, definitions.ToList().FindAll(x => x.Id == parentId), this.definition.Id)
            {
                AllowMultiSelect = false,
                AllowShowHidden = false
            };

			if (formDefinitionPicker.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			valueTextBox.Text = formDefinitionPicker.SelectedDefinition.Id.ToString();
		}

		private void ClearButton_Click(object sender, EventArgs e) 
			=> valueTextBox.Clear();

		private void NoColorCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (noColorCheckBox.Checked)
			{
				colorButton.BackColor = Color.Empty;
			}
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (definition.Id == 0)
			{
				DialogResult = DialogResult.Cancel;

				return;
			}

			if (definition.Category == DefinitionCategory.ClaimCustomTracking && definitions.Count(x => x.Category == DefinitionCategory.ClaimCustomTracking) == 1 ||
				definition.Category == DefinitionCategory.InsurancePaymentType && definitions.Count(x => x.Category == DefinitionCategory.InsurancePaymentType) == 1 ||
				definition.Category == DefinitionCategory.SupplyCats && definitions.Count(x => x.Category == DefinitionCategory.SupplyCats) == 1)
			{
				ShowError("Cannot delete the last definition from this category.");

				return;
			}

			bool isAutoNoteRefresh = false;
			if (definition.Category == DefinitionCategory.AutoNoteCats && AutoNotes.Exists(x => x.AutoNoteCategoryId == definition.Id))
			{
				if (!Confirm("Deleting this Auto Note Category will uncategorize some auto notes. Delete anyway?"))
				{
					return;
				}

				isAutoNoteRefresh = true;
			}

			try
			{
				Definitions.Delete(definition);

				IsDeleted = true;
				if (isAutoNoteRefresh)
				{
					CacheManager.RefreshGlobal(nameof(InvalidType.AutoNotes));
				}

				DialogResult = DialogResult.OK;
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (hiddenCheckBox.Checked && definition.Id > 0)
			{
				if (!DefL.CanHideDefinition(definition, categoryOptions))
				{
					return;
				}
			}

			var name = nameTextBox.Text.Trim();
			if (name.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterName);

				return;
			}

			switch (definition.Category)
			{
				case DefinitionCategory.AccountQuickCharge:
				case DefinitionCategory.ApptProcsQuickAdd:
					var procedureCodes = valueTextBox.Text.Split(',');
					var procedureCodesFound = new List<string>();

					foreach (var code in procedureCodes)
					{
						var procedureCode = ProcedureCodes.GetProcCode(code);
						if (procedureCode.Id == 0)
						{
							// Now check to see if the trimmed version of the code does not exist either.
							procedureCode = ProcedureCodes.GetProcCode(code.Trim());
							if (procedureCode.Id == 0)
							{
								ShowError("Invalid procedure code entered: " + code);

								return;
							}
						}

						procedureCodesFound.Add(procedureCode.Code);
					}

					valueTextBox.Text = string.Join(",", procedureCodesFound);
					break;

				case DefinitionCategory.AdjTypes:
					if (valueTextBox.Text != "+" && valueTextBox.Text != "-" && valueTextBox.Text != "dp")
					{
						ShowError("Valid values are +, -, or dp.");
						return;
					}
					break;

				case DefinitionCategory.BillingTypes:
					if (!valueTextBox.Text.ToLower().In("", "e", "c", "ce"))
					{
						ShowError("Valid values are blank, E, C, or CE.");
						return;
					}
					break;

				case DefinitionCategory.ClaimCustomTracking:
					int value = 0;
					if (!int.TryParse(valueTextBox.Text, out value) || value < 0)
					{
						ShowError("Days Suppressed must be a valid non-negative number.");
						return;
					}
					break;

				case DefinitionCategory.CommLogTypes:
					List<string> listCommItemTypes = Commlogs.GetCommItemTypes().Select(x => x.GetDescription(useShortVersionIfAvailable: true)).ToList();
					if (valueTextBox.Text != "" && !listCommItemTypes.Any(x => x == valueTextBox.Text))
					{
						ShowError("Valid values are:" + " " + string.Join(", ", listCommItemTypes));
						return;
					}
					break;

				case DefinitionCategory.DiscountTypes:
					int discVal;
					if (valueTextBox.Text == "") break;
					try
					{
						discVal = System.Convert.ToInt32(valueTextBox.Text);
					}
					catch
					{
						ShowError("Not a valid number");
						return;
					}
					if (discVal < 0 || discVal > 100)
					{
						ShowError("Valid values are between 0 and 100");
						return;
					}
					valueTextBox.Text = discVal.ToString();
					break;

				case DefinitionCategory.ImageCats:
					valueTextBox.Text = valueTextBox.Text.ToUpper().Replace(",", "");
					if (!Regex.IsMatch(valueTextBox.Text, @"^[XPS]*$"))
					{
						valueTextBox.Text = "";
					}
					break;

				case DefinitionCategory.InsurancePaymentType:
					if (valueTextBox.Text != "" && valueTextBox.Text != "N")
					{
						ShowError("Valid values are blank or N.");
						return;
					}
					break;

				case DefinitionCategory.PaySplitUnearnedType:
					if (!valueTextBox.Text.ToLower().In("", "x"))
					{
						ShowError("Valid values are blank or 'X'");
						return;
					}

					var unearnedTypes = definitions.FindAll(x => x.Category == DefinitionCategory.PaySplitUnearnedType);
					if (unearnedTypes.FindAll(x => string.IsNullOrEmpty(x.Value)).Count == 1 && definition.Value == "" && valueTextBox.Text != "" && definition.Id > 0)
					{
						ShowError("Must have at least one definition that shows in Account.");

						return;
					}
					else if (unearnedTypes.FindAll(x => !string.IsNullOrEmpty(x.Value)).Count == 1 && definition.Value != "" && valueTextBox.Text == "")
					{
						ShowError("Must have at least one definition that does not show in Account.");

						return;
					}
					break;

				case DefinitionCategory.RecallUnschedStatus:
					if (valueTextBox.Text.Length > 7)
					{
						ShowError("Maximum length is 7.");
						return;
					}
					break;

				case DefinitionCategory.TxPriorities:
					if (valueTextBox.Text.Length > 7)
					{
						ShowError("Maximum length of abbreviation is 7.");
						return;
					}
					break;

				default:
					break;
			}

			definition.Name = name;
			definition.Value = valueTextBox.Text;

			if (categoryOptions.EnableValue && !categoryOptions.IsValueDefNum)
			{
				definition.Value = valueTextBox.Text;
			}

			if (categoryOptions.EnableColor)
			{
				//If checkNoColor is checked, insert empty into the database. Otherwise, use the color they picked.
				definition.Color = noColorCheckBox.Checked ? Color.Empty : colorButton.BackColor;
			}

			definition.IsHidden = hiddenCheckBox.Checked;
			
			Definitions.Save(definition);

			//Must be after the upsert so that we have access to the DefNum for new Defs.
			if (definition.Category == DefinitionCategory.ApptConfirmed)
			{
				//==================== EXCLUDE SEND ====================
				UpdateExlusionsList(checkExcludeSend, excludeSendIds, definition, PreferenceName.ApptConfirmExcludeESend);
				//==================== EXCLUDE CONFIRM ====================
				UpdateExlusionsList(checkExcludeConfirm, excludeConfirmIds, definition, PreferenceName.ApptConfirmExcludeEConfirm);
				//==================== EXCLUDE REMIND ====================				
				UpdateExlusionsList(checkExcludeRemind, excludeReminderIds, definition, PreferenceName.ApptConfirmExcludeERemind);
				//==================== EXCLUDE THANKYOU ====================
				UpdateExlusionsList(checkExcludeThanks, excludeThankYouIds, definition, PreferenceName.ApptConfirmExcludeEThankYou);
				//==================== EXCLUDE ARRIVAL SEND ====================
				UpdateExlusionsList(checkExcludeArrivalSend, excludeArrivalSendIds, definition, PreferenceName.ApptConfirmExcludeArrivalSend);
				//==================== EXCLUDE ARRIVAL RESPONSE ====================
				UpdateExlusionsList(checkExcludeArrivalResponse, excludeArrivelResponseIds, definition, PreferenceName.ApptConfirmExcludeArrivalResponse);
				Signalods.SetInvalid(InvalidType.Prefs);
			}

			DialogResult = DialogResult.OK;
		}

		private static void UpdateExlusionsList(CheckBox checkBox, List<long> excludedIds, Definition definition, string preferenceName)
		{
			if (checkBox.Checked)
			{
				excludedIds.Add(definition.Id);
			}
			else
			{
				excludedIds.RemoveAll(x => x == definition.Id);
			}

			Preferences.Set(preferenceName, string.Join(",", excludedIds.Distinct().OrderBy(x => x)));
		}
	}
}
