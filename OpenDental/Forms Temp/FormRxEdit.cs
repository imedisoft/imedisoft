using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormRxEdit : FormBase
	{
		private readonly Patient patient;
		private readonly RxPat rxPat;



		public FormRpPrintPreview pView = new FormRpPrintPreview();
		

		

		///<summary>If the Rx has already been printed, this will contain the archived sheet. The print button will be not visible, and the view button will be visible.</summary>
		private Sheet sheet;
		private List<Procedure> _listInUseProcs;







		public FormRxEdit(Patient patient, RxPat rxPat)
		{
			InitializeComponent();

			this.rxPat = rxPat;
			this.patient = patient;
		}

		private void FormRxEdit_Load(object sender, EventArgs e)
		{
			if (rxPat.Id == 0)
			{
				auditButton.Visible = false;
				viewButton.Visible = false;
				viewLabel.Visible = false;
				sheet = null;

				if (Preferences.GetBool(PreferenceName.ShowFeatureEhr) && Security.CurrentUser.ProviderId.HasValue) // Is CPOE
				{
					cpoeLabel.Visible = true;
					providerComboBox.Enabled = false;
					providerButton.Enabled = false;

					rxPat.ProvNum = Security.CurrentUser.ProviderId.Value;
				}
				else
				{
					var provider = Providers.GetById(Security.CurrentUser.ProviderId);
					if (provider != null && !provider.IsSecondary)
					{
						// Only set the provider on the Rx if the provider is not a hygienist.
						rxPat.ProvNum = provider.Id;
					}
				}
			}
			else
			{
				sheet = Sheets.GetRx(rxPat.PatNum, rxPat.Id);
				if (sheet == null)
				{
					viewButton.Visible = false;
					viewLabel.Visible = false;
				}
				else
				{
					printButton.Visible = false;
				}

				if (!Security.IsAuthorized(Permissions.RxEdit))
				{
					dateTextBox.Enabled = false;
					checkControlled.Enabled = false;
					procRequiredCheckBox.Enabled = false;
					procedureComboBox.Enabled = false;
					daysOfSupplyTextBox.Enabled = false;
					drugTextBox.Enabled = false;
					sigTextBox.Enabled = false;
					dispTextBox.Enabled = false;
					refillsTextBox.Enabled = false;
					patInstructionsTextBox.Enabled = false;
					providerComboBox.Enabled = false;
					providerButton.Enabled = false;
					dosageCodeTextBox.Enabled = false;
					notesTextBox.Enabled = false;
					pharmacyButton.Enabled = false;
					sendStatusComboBox.Enabled = false;
					deleteButton.Enabled = false;
					comboClinic.Enabled = false;
					acceptButton.Enabled = false;
				}
			}

			// Security is handled on the Rx button click in the Chart module
			dateTextBox.Text = rxPat.RxDate.ToString("d");
			checkControlled.Checked = rxPat.IsControlled;
			procedureComboBox.Items.Clear();
			if (Preferences.GetBool(PreferenceName.RxHasProc))
			{
				procRequiredCheckBox.Checked = rxPat.IsProcRequired;
				procedureComboBox.Items.Add("none");
				procedureComboBox.SelectedIndex = 0;

				List<ProcedureCode> listProcCodes = ProcedureCodes.GetListDeep();
				DateTime rxDate = PIn.Date(dateTextBox.Text);
				if (rxDate.Year < 1880)
				{
					rxDate = DateTime.Today;
				}

				_listInUseProcs = Procedures.Refresh(rxPat.PatNum)
					.FindAll(x => x.ProcNum == rxPat.ProcNum
						|| (x.ProcStatus == ProcStat.C && x.DateComplete <= rxDate && x.DateComplete >= rxDate.AddYears(-1))
						|| x.ProcStatus == ProcStat.TP)
					.OrderBy(x => x.ProcStatus.ToString())
					.ThenBy(x => (x.ProcStatus == ProcStat.C) ? x.DateComplete : x.DateTP)
					.ToList();

				foreach (Procedure proc in _listInUseProcs)
				{
					ProcedureCode procCode = ProcedureCodes.GetById(proc.CodeNum, listProcCodes);
					string itemText = proc.ProcStatus.ToString();
					if (ProcMultiVisits.IsProcInProcess(proc.ProcNum))
					{
						itemText = ProcStatExt.InProcess;
					}
					if (proc.ProcStatus == ProcStat.C)
					{
						itemText += " " + proc.DateComplete.ToShortDateString();
					}
					else
					{
						itemText += " " + proc.DateTP.ToShortDateString();
					}
					itemText += " " + Procedures.GetDescription(proc);
					procedureComboBox.Items.Add(itemText);
					if (proc.ProcNum == rxPat.ProcNum)
					{
						procedureComboBox.SelectedIndex = procedureComboBox.Items.Count - 1;
					}
				}
				if (rxPat.DaysOfSupply != 0)
				{
					daysOfSupplyTextBox.Text = POut.Double(rxPat.DaysOfSupply);
				}
			}
			else
			{
				procRequiredCheckBox.Enabled = false;
				procedureLabel.Enabled = false;
				procedureComboBox.Enabled = false;
				daysOfSupplyLabel.Enabled = false;
				daysOfSupplyTextBox.Enabled = false;
			}

			for (int i = 0; i < Enum.GetNames(typeof(RxSendStatus)).Length; i++)
			{
				sendStatusComboBox.Items.Add(Enum.GetNames(typeof(RxSendStatus))[i]);
			}

			sendStatusComboBox.SelectedIndex = (int)rxPat.SendStatus;
			drugTextBox.Text = rxPat.Drug;
			sigTextBox.Text = rxPat.Sig;
			dispTextBox.Text = rxPat.Disp;
			refillsTextBox.Text = rxPat.Refills;
			patInstructionsTextBox.Text = rxPat.PatientInstruction;

			if (Preferences.GetBool(PreferenceName.ShowFeatureEhr))
			{
				dosageCodeTextBox.Text = rxPat.DosageCode;
			}
			else
			{
				dosageCodeLabel.Visible = false;
				dosageCodeTextBox.Visible = false;
			}

			notesTextBox.Text = rxPat.Notes;
			pharmacyInfoTextBox.Text = rxPat.ErxPharmacyInfo;
			pharmacyTextBox.Text = Pharmacies.GetDescription(rxPat.PharmacyNum);
			comboClinic.SelectedClinicNum = rxPat.ClinicNum;

			FillComboProvNum();
		}

		/// <summary>
		/// Fill the provider combobox with items depending on the clinic selected
		/// </summary>
		private void FillComboProvNum()
		{
			providerComboBox.Items.Clear();
			providerComboBox.Items.AddProvsAbbr(Providers.GetProvsForClinic(comboClinic.SelectedClinicNum));

			if (rxPat.ProvNum == 0) // If new Rx
			{
				if (providerComboBox.Items.Count == 0) // No items in dropdown
				{
					// Use clinic default prov. We need some default selection.
					providerComboBox.SetSelectedProvNum(Providers.GetDefaultProvider(comboClinic.SelectedClinicNum).Id);
				}
				else
				{
					providerComboBox.SetSelected(0); // First in list
				}
			}
			else
			{
				providerComboBox.SetSelectedProvNum(rxPat.ProvNum); // Use prov from Rx
			}
		}

		private void ProviderButton_Click(object sender, EventArgs e)
		{
            using var formProviderPick = new FormProviderPick(providerComboBox.Items.GetAll<Provider>())
            {
                SelectedProviderId = providerComboBox.GetSelectedProvNum()
            };

            if (formProviderPick.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			providerComboBox.SetSelectedProvNum(formProviderPick.SelectedProviderId);
		}

		private void PharmacyButton_Click(object sender, EventArgs e)
		{
            using var formPharmacies = new FormPharmacies
            {
                IsSelectionMode = true,
                SelectedPharmacyNum = rxPat.PharmacyNum
            };

            if (formPharmacies.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			rxPat.PharmacyNum = formPharmacies.SelectedPharmacyNum;

			pharmacyTextBox.Text = Pharmacies.GetDescription(rxPat.PharmacyNum);
		}

		private void AuditButton_Click(object sender, EventArgs e)
		{
            var permissions = new List<Permissions>
            {
                Permissions.RxCreate,
                Permissions.RxEdit
            };

            using var formAuditOneType = new FormAuditOneType(rxPat.PatNum, permissions, "Audit Trail for Rx", rxPat.Id);

			formAuditOneType.ShowDialog(this);
		}

		/// <summary>
		/// Attempts to save, returning true if successful.
		/// </summary>
		private bool SaveRx()
		{
			if (dateTextBox.errorProvider1.GetError(dateTextBox) != "" || (daysOfSupplyTextBox.Text != "" && daysOfSupplyTextBox.errorProvider1.GetError(daysOfSupplyTextBox) != ""))
			{
				ShowError("Please fix data entry errors first.");

				return false;
			}

			long newProviderId = providerComboBox.GetSelectedProvNum();
			if (newProviderId == 0)
			{
				ShowError("Invalid provider.");

				return false;
			}

			// Prevents prescriptions from being added that have a provider selected that is past their term date.
			var invalidProviderIds = Providers.GetInvalidProvsByTermDate(new List<long> { newProviderId }, DateTime.Now);
			if (invalidProviderIds.Count > 0)
			{
				ShowError("The provider selected has a Term Date prior to today. Please select another provider.");

				return false;
			}

			rxPat.ProvNum = newProviderId;
			rxPat.RxDate = PIn.Date(dateTextBox.Text);
			rxPat.Drug = drugTextBox.Text;
			rxPat.IsControlled = checkControlled.Checked;
			if (Preferences.GetBool(PreferenceName.RxHasProc))
			{
				rxPat.IsProcRequired = procRequiredCheckBox.Checked;
				if (procedureComboBox.SelectedIndex == 0)
				{//none
					rxPat.ProcNum = 0;
				}
				else
				{
					rxPat.ProcNum = _listInUseProcs[procedureComboBox.SelectedIndex - 1].ProcNum;
				}
				rxPat.DaysOfSupply = PIn.Double(daysOfSupplyTextBox.Text);
			}
			rxPat.Sig = sigTextBox.Text;
			rxPat.Disp = dispTextBox.Text;
			rxPat.Refills = refillsTextBox.Text;
			rxPat.DosageCode = dosageCodeTextBox.Text;
			rxPat.Notes = notesTextBox.Text;
			rxPat.SendStatus = (RxSendStatus)sendStatusComboBox.SelectedIndex;
			rxPat.PatientInstruction = patInstructionsTextBox.Text;
			rxPat.ClinicNum = (comboClinic.SelectedClinicNum == -1 ? rxPat.ClinicNum : comboClinic.SelectedClinicNum);//If no selection, don't change the ClinicNum
			
			// TODO:
			// hook for additional authorization before prescription is saved
			//bool[] authorized = new bool[1] { false };
			//if (Plugins.HookMethod(this, "FormRxEdit.SaveRx_Authorize", authorized, Providers.GetProv(selectedProvNum), rxPat, _rxPatOld))
			//{
			//	if (!authorized[0])
			//	{
			//		return false;
			//	}
			//}

			// Pharmacy is set when using pick button.
			if (rxPat.Id == 0)
			{
				rxPat.Id = RxPats.Insert(rxPat);

				SecurityLogs.MakeLogEntry(
					Permissions.RxCreate, rxPat.PatNum, 
					"CREATED(" + rxPat.RxDate.ToShortDateString() + "," + rxPat.Drug + "," + rxPat.ProvNum + "," + rxPat.Disp + "," + rxPat.Refills + ")", 
					rxPat.Id, DateTime.MinValue);//No date previous needed, new Rx Pat

				if (FormProcGroup.IsOpen)
				{
					FormProcGroup.RxNum = rxPat.Id;
				}
			}
			else
			{
				// TODO:
				//if (RxPats.Update(rxPat, _rxPatOld))
				//{
				//	//The rx has changed, make an edit entry.
				//	SecurityLogs.MakeLogEntry(Permissions.RxEdit, rxPat.PatNum, "FROM(" + _rxPatOld.RxDate.ToShortDateString() + "," + _rxPatOld.Drug + ","
				//		+ _rxPatOld.ProvNum + "," + _rxPatOld.Disp + "," + _rxPatOld.Refills + ")" + "\r\nTO(" + rxPat.RxDate.ToShortDateString() + "," + rxPat.Drug + ","
				//		+ rxPat.ProvNum + "," + rxPat.Disp + "," + rxPat.Refills + ")", rxPat.RxNum, _rxPatOld.DateTStamp);
				//}
			}

			// If there is not a link for the current PharmClinic combo, make one.
			if (Pharmacies.GetOne(rxPat.PharmacyNum) != null && PharmClinics.GetOneForPharmacyAndClinic(rxPat.PharmacyNum, rxPat.ClinicNum) == null)
			{
				PharmClinics.Insert(new PharmClinic(rxPat.PharmacyNum, rxPat.ClinicNum));
			}

			return true;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (rxPat.Id == 0)
			{
				DialogResult = DialogResult.Cancel;

				return;
			}

			if (Confirm(Translation.Common.ConfirmDelete))
			{
				return;
			}

			SecurityLogs.MakeLogEntry(Permissions.RxEdit, rxPat.PatNum, 
				"FROM(" + rxPat.RxDate.ToShortDateString() + "," + rxPat.Drug + "," + rxPat.ProvNum + "," + rxPat.Disp + "," + rxPat.Refills + ")\r\n" +
				"TO('deleted')", rxPat.Id, rxPat.DateTStamp);
			
			RxPats.Delete(rxPat.Id);

			DialogResult = DialogResult.OK;
		}

		private void PrintPatInstructionsButton_Click(object sender, EventArgs e)
		{
			PrintRx(true);
		}

		private void PrintButton_Click(object sender, EventArgs e)
		{
			if (!PrintRx(false))
			{
				return;
			}

			if (rxPat.IsNew)
			{
				AutomationL.Trigger(AutomationTrigger.RxCreate, new List<string>(), patient.PatNum, 0, new List<RxPat>() { rxPat });
			}

			DialogResult = DialogResult.OK;
		}

		/// <summary>
		/// Prints the prescription. 
		/// Returns true if printing was successful.
		/// Otherwise; displays an error message and returns false.
		/// </summary>
		private bool PrintRx(bool isInstructions)
		{
			if (PrinterSettings.InstalledPrinters.Count == 0)
			{
				ShowError(
					"Error: No Printers Installed\r\n" +
					"If you do have a printer installed, restarting the workstation may solve the problem.");

				return false;
			}

			if (!isInstructions)
			{
				//only visible if sheet==null.
				if (sendStatusComboBox.SelectedIndex == (int)RxSendStatus.InElectQueue
					|| sendStatusComboBox.SelectedIndex == (int)RxSendStatus.SentElect)
				{
					//do not change status
				}
				else
				{
					sendStatusComboBox.SelectedIndex = (int)RxSendStatus.Printed;
				}
			}

			if (!SaveRx())
			{
				return false;
			}

			//This logic is an exact copy of FormRxManage.butPrintSelect_Click()'s logic when 1 Rx is selected.  
			//If this is updated, that method needs to be updated as well.
			SheetDef sheetDef;
			if (isInstructions)
			{
				sheetDef = SheetDefs.GetInternalOrCustom(SheetInternalType.RxInstruction);
			}
			else
			{
				sheetDef = SheetDefs.GetSheetsDefault(SheetTypeEnum.Rx, Clinics.Active.Id);
			}

			sheet = SheetUtil.CreateSheet(sheetDef, patient.PatNum);
			SheetParameter.SetParameter(sheet, "RxNum", rxPat.Id);
			SheetFiller.FillFields(sheet);
			SheetUtil.CalculateHeights(sheet);

			if (!OpenDental.SheetPrinting.PrintRx(sheet, rxPat))
			{
				return false;
			}

			return true;
		}

		private void ViewButton_Click(object sender, EventArgs e)
		{
			if (!SaveRx())
			{
				return;
			}

			SheetFields.GetFieldsAndParameters(sheet);
			FormSheetFillEdit.ShowForm(sheet, FormSheetFillEdit_FormClosing);
		}

		private void ClinicComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			FillComboProvNum();
		}

		/// <summary>
		/// Event handler for closing FormSheetFillEdit when it is non-modal.
		/// </summary>
		private void FormSheetFillEdit_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (((FormSheetFillEdit)sender).DialogResult == DialogResult.OK)
			{ 
				// If user clicked cancel, then we can just stay in this form.
				DialogResult = DialogResult.OK;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!SaveRx())
			{
				return;
			}

			if (rxPat.IsNew)
			{
				AutomationL.Trigger(AutomationTrigger.RxCreate, new List<string>(), patient.PatNum, 0, new List<RxPat>() { rxPat });
			}

			DialogResult = DialogResult.OK;
		}
    }
}
