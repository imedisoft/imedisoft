using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using Imedisoft.UI;
using Imedisoft.Data.Models;
using Imedisoft.Data;
using Imedisoft.Forms;

namespace OpenDental{
	public partial class FormModuleSetup:ODForm {
		#region Fields - Private
		private List<Definition> _listDefsPosAdjTypes;
		private List<Definition> _listDefsNegAdjTypes;
		private bool _changed;
		private ColorDialog _colorDialog;
		///<summary>Used to determine a specific tab to have opened upon load.  Only set via the constructor and only used during load.</summary>
		private int _selectedTab;
		private List<BrokenApptProcedure> _listBrokenApptProcedures=new List<BrokenApptProcedure>();
		private bool _ynPrePayAllowedForTpProcs;
		///<summary>Helps store and set the Prefs for desease, medications, and alergies in the Chart Module when clicking OK on those forms.</summary>
		private long _diseaseDefNum;//DiseaseDef
		private long _medicationNum;
		private long _alergyDefNum;//AllergyDef
		#endregion Fields - Private

		#region Constructors
		///<summary>Default constructor.  Opens the form with the Appts tab selected.</summary>
		public FormModuleSetup():this(0) {
		}

		///<summary>Opens the form with a specific tab selected.  Currently 0-6 are the only valid values.  Defaults to Appts tab if invalid value passed in. 0=Appts, 1=Family, 2=Account, 3=Treat' Plan, 4=Chart, 5=Images, 6=Manage</summary>
		public FormModuleSetup(int selectedTab) {
			InitializeComponent();
			
			if(selectedTab<0 || selectedTab>6) {
				selectedTab=0;//Default to Appts tab.
			}
			_selectedTab=selectedTab;
		}
		#endregion Constructors
		
		#region Events - Standard
		private void FormModuleSetup_Load(object sender, System.EventArgs e) {
			_changed=false;
			try {//try/catch used to prevent setup form from partially loading and filling controls.  Causes UEs, Example: TimeCardOvertimeFirstDayOfWeek set to -1 because UI control not filled properly.
				FillAppts();
				FillFamily();
				FillAccount();
				FillTreatPlan();
				FillChart();
				FillImages();
				FillManage();
			}
			catch(Exception ex) {
				FriendlyException.Show("An error has occurred while attempting to load preferences.  Run database maintenance and try again.",ex);
				DialogResult=DialogResult.Abort;
				return;
			}
			//Now that all the tabs are filled, use _selectedTab to open a specific tab that the user is trying to view.
			tabControlMain.SelectedTab=tabControlMain.TabPages[_selectedTab];//Guaranteed to be a valid tab.  Validated in constructor.
			if(PrefC.RandomKeys) {
				groupTreatPlanSort.Visible=false;
			}
			Plugins.HookAddCode(this,"FormModuleSetup.FormModuleSetup_Load_end");
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			//validation is done within each save.
			//One save to db might succeed, and then a subsequent save can fail to validate.  That's ok.
			if(!SaveAppts()
				|| !SaveFamily()
				|| !SaveAccount()
				|| !SaveTreatPlan()
				|| !SaveChart()
				|| !SaveImages()
				|| !SaveManage()				
			){
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormModuleSetup_FormClosing(object sender,FormClosingEventArgs e) {
			if(_changed){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}
		#endregion Events - Standard

		#region Methods - Event Handlers Appts
		private void butApptLineColor_Click(object sender,EventArgs e) {
			_colorDialog.Color=butColor.BackColor;//Pre-select current pref color
			if(_colorDialog.ShowDialog()==DialogResult.OK) {
				butApptLineColor.BackColor=_colorDialog.Color;
			}
		}

		private void butColor_Click(object sender,EventArgs e) {
			_colorDialog.Color=butColor.BackColor;//Pre-select current pref color
			if(_colorDialog.ShowDialog()==DialogResult.OK) {
				butColor.BackColor=_colorDialog.Color;
			}
		}

		private void checkAppointmentTimeIsLocked_MouseUp(object sender,MouseEventArgs e) {
			if(checkAppointmentTimeIsLocked.Checked) {
				if(MsgBox.Show(MsgBoxButtons.YesNo,"Would you like to lock appointment times for all existing appointments?")){
					Appointments.SetAptTimeLocked();
				}
			}
		}

		private void checkApptsRequireProcs_CheckedChanged(object sender,EventArgs e) {
			textApptWithoutProcsDefaultLength.Enabled=(!checkApptsRequireProcs.Checked);
		}
		#endregion Methods - Event Handlers Appts

		#region Methods - Event Handlers Family
		private void checkAllowedFeeSchedsAutomate_Click(object sender,EventArgs e) {
			if(!checkAllowedFeeSchedsAutomate.Checked){
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Allowed fee schedules will now be set up for all insurance plans that do not already have one.\r\nThe name of each fee schedule will exactly match the name of the carrier.\r\nOnce created, allowed fee schedules can be easily managed from the fee schedules window.\r\nContinue?")){
				checkAllowedFeeSchedsAutomate.Checked=false;
				return;
			}
			Cursor=Cursors.WaitCursor;
			long schedsAdded=InsPlans.GenerateAllowedFeeSchedules();
			Cursor=Cursors.Default;
			MessageBox.Show("Done.  Allowed fee schedules added: "+schedsAdded.ToString());
			DataValid.SetInvalid(InvalidType.FeeScheds);
		}

		private void checkInsDefaultAssignmentOfBenefits_Click(object sender,EventArgs e) {
			//Users with Setup permission are always allowed to change the Checked property of this check box.
			//However, there is a second step when changing the value that can only be performed by users with the InsPlanChangeAssign permission.
			if(!Security.IsAuthorized(Permissions.InsPlanChangeAssign,true)) {
				return;
			}
			string promptMsg="Would you like to immediately change all plans to use assignment of benefits?\r\n"
					+$"Warning: This will update all existing plans to render payment to the provider on all future claims.";
			if(!checkInsDefaultAssignmentOfBenefits.Checked) {
				promptMsg="Would you like to immediately change all plans to use assignment of benefits?\r\n"
					+$"Warning: This will update all existing plans to render payment to the patient on all future claims.";
			}
			if(MessageBox.Show(promptMsg,"Change all plans?",MessageBoxButtons.YesNo)==DialogResult.No) {
				return;
			}
			long subsAffected=InsSubs.SetAllSubsAssignBen(checkInsDefaultAssignmentOfBenefits.Checked);
			SecurityLogs.MakeLogEntry(Permissions.InsPlanChangeAssign,0
				,"The following count of plan(s) had their assignment of benefits updated in the Family tab in Module Preferences:"+" "+POut.Long(subsAffected)
			);
			MessageBox.Show("Plans affected:"+" "+POut.Long(subsAffected));
		}

		private void checkInsDefaultShowUCRonClaims_Click(object sender,EventArgs e) {
			if(!checkInsDefaultShowUCRonClaims.Checked) {
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Would you like to immediately change all plans to show office UCR fees on claims?")) {
				return;
			}
			long plansAffected=InsPlans.SetAllPlansToShowUCR();
			MessageBox.Show("Plans affected: "+plansAffected.ToString());
		}

		private void comboCobRule_SelectionChangeCommitted(object sender,EventArgs e) {
			if(MsgBox.Show(MsgBoxButtons.YesNo,"Would you like to change the COB rule for all existing insurance plans?")) {
				InsPlans.UpdateCobRuleForAll((EnumCobRule)comboCobRule.SelectedIndex);
			}
		}
		#endregion Methods - Event Handlers Family

		#region Methods - Event Handlers Account
		private void butBadDebt_Click(object sender, EventArgs e)
		{
			string[] arrayDefNums = Preferences.GetString(PreferenceName.BadDebtAdjustmentTypes).Split(new char[] { ',' }); //comma-delimited list.
			List<long> listBadAdjDefNums = new List<long>();
			foreach (string strDefNum in arrayDefNums)
			{
				listBadAdjDefNums.Add(PIn.Long(strDefNum));
			}
			List<Definition> listBadAdjDefs = Definitions.GetDefs(DefinitionCategory.AdjTypes, listBadAdjDefNums);

            using var formDefinitionPicker = new FormDefinitionPicker(DefinitionCategory.AdjTypes, listBadAdjDefs)
            {
                AllowShowHidden = true,
                AllowMultiSelect = true
            };

            if (formDefinitionPicker.ShowDialog(this) == DialogResult.OK)
			{
				FillListboxBadDebt(formDefinitionPicker.SelectedDefinitions);
			}
		}

		private void butReplacements_Click(object sender,EventArgs e) {
			FormMessageReplacements form=new FormMessageReplacements(MessageReplaceType.Patient);
			form.IsSelectionMode=true;
			form.ShowDialog();
			if(form.DialogResult!=DialogResult.OK) {
				return;
			}
			textClaimIdentifier.Focus();
			int cursorIndex=textClaimIdentifier.SelectionStart;
			textClaimIdentifier.Text=textClaimIdentifier.Text.Insert(cursorIndex,form.Replacement);
			textClaimIdentifier.SelectionStart=cursorIndex+form.Replacement.Length;
		}
		
		private void checkAllowPrePayToTpProcs_Click(object sender,EventArgs e) {
			if(checkAllowPrePayToTpProcs.Checked) {
				checkIsRefundable.Visible=true;
				checkIsRefundable.Checked=Preferences.GetBool(PreferenceName.TpPrePayIsNonRefundable);
				_ynPrePayAllowedForTpProcs=true;
			}
			else {
				checkIsRefundable.Visible=false;
				checkIsRefundable.Checked=false;
				_ynPrePayAllowedForTpProcs=false;
			}
		}

		private void checkRecurringChargesAutomated_CheckedChanged(object sender,EventArgs e) {
			labelRecurringChargesAutomatedTime.Enabled=checkRecurringChargesAutomated.Checked;
			textRecurringChargesTime.Enabled=checkRecurringChargesAutomated.Checked;
		}

		private void checkRepeatingChargesAutomated_CheckedChanged(object sender,EventArgs e) {
			labelRepeatingChargesAutomatedTime.Enabled=checkRepeatingChargesAutomated.Checked;
			textRepeatingChargesAutomatedTime.Enabled=checkRepeatingChargesAutomated.Checked;
		}

		private void checkShowFamilyCommByDefault_Click(object sender,EventArgs e) {
			MessageBox.Show("You will need to restart the program for the change to take effect.");
		}

		private void comboPayPlansVersion_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboPayPlansVersion.SelectedIndex==(int)PayPlanVersions.AgeCreditsAndDebits-1) {//Minus 1 because the enum starts at 1.
				checkHideDueNow.Visible=true;
				checkHideDueNow.Checked=Preferences.GetBool(PreferenceName.PayPlanHideDueNow);
			}
			else {
				checkHideDueNow.Visible=false;
				checkHideDueNow.Checked=false;
			}
		}

		///<summary>Turning on automated repeating charges, but recurring charges are also enabled and set to run before auto repeating charges.  Prompt user that this is unadvisable.</summary>
		private void PromptRecurringRepeatingChargesTimes(object sender,EventArgs e) {
			if(checkRepeatingChargesAutomated.Checked && checkRecurringChargesAutomated.Checked
				&& PIn.Date(textRepeatingChargesAutomatedTime.Text).TimeOfDay>=PIn.Date(textRecurringChargesTime.Text).TimeOfDay)
			{
				MessageBox.Show("Recurring charges run time is currently set before Repeating charges run time.\r\nConsider setting repeating charges to "
					+"automatically run before recurring charges.");
			}
		}
		#endregion Methods - Event Handlers Account

		#region Methods - Event Handlers Treat' Plan
		private void checkFrequency_Click(object sender,EventArgs e) {
			textInsBW.Enabled=checkFrequency.Checked;
			textInsPano.Enabled=checkFrequency.Checked;
			textInsExam.Enabled=checkFrequency.Checked;
			textInsCancerScreen.Enabled=checkFrequency.Checked;
			textInsProphy.Enabled=checkFrequency.Checked;
			textInsFlouride.Enabled=checkFrequency.Checked;
			textInsSealant.Enabled=checkFrequency.Checked;
			textInsCrown.Enabled=checkFrequency.Checked;
			textInsSRP.Enabled=checkFrequency.Checked;
			textInsDebridement.Enabled=checkFrequency.Checked;
			textInsPerioMaint.Enabled=checkFrequency.Checked;
			textInsDentures.Enabled=checkFrequency.Checked;
			textInsImplant.Enabled=checkFrequency.Checked;
		}
		
		private void radioTreatPlanSortOrder_Click(object sender,EventArgs e) {
			//Sort by order is a false 
			if(Preferences.GetBool(PreferenceName.TreatPlanSortByTooth)==radioTreatPlanSortOrder.Checked) {
				MessageBox.Show("You will need to restart the program for the change to take effect.");
			}
		}

		private void radioTreatPlanSortTooth_Click(object sender,EventArgs e) {
			if(Preferences.GetBool(PreferenceName.TreatPlanSortByTooth)!=radioTreatPlanSortTooth.Checked) {
				MessageBox.Show("You will need to restart the program for the change to take effect.");
			}
		}
		#endregion Methods - Event Handlers Treat' Plan

		#region Methods - Event Handlers Chart
		private void butAllergiesIndicateNone_Click(object sender,EventArgs e) {
			FormAllergyDefs formA=new FormAllergyDefs();
			formA.IsSelectionMode=true;
			formA.ShowDialog();
			if(formA.DialogResult!=DialogResult.OK) {
				return;
			}
			_alergyDefNum=formA.SelectedAllergyDef.Id;
			textAllergiesIndicateNone.Text=AllergyDefs.GetOne(_alergyDefNum).Description;
		}

		private void butDiagnosisCode_Click(object sender,EventArgs e) {
			if(checkDxIcdVersion.Checked) {//ICD-10
				FormIcd10s formI=new FormIcd10s();
				formI.IsSelectionMode=true;
				if(formI.ShowDialog()==DialogResult.OK) {
					textICD9DefaultForNewProcs.Text=formI.SelectedIcd10.Code;
				}
			}
			else {//ICD-9
				FormIcd9s formI=new FormIcd9s();
				formI.IsSelectionMode=true;
				if(formI.ShowDialog()==DialogResult.OK) {
					textICD9DefaultForNewProcs.Text=formI.SelectedIcd9.Code;
				}
			}
		}
		
		private void butMedicationsIndicateNone_Click(object sender,EventArgs e) {
			FormMedications formM=new FormMedications();
			formM.IsSelectionMode=true;
			formM.ShowDialog();
			if(formM.DialogResult!=DialogResult.OK) {
				return;
			}
			_medicationNum=formM.SelectedMedicationNum;
			textMedicationsIndicateNone.Text=Medications.GetDescription(formM.SelectedMedicationNum);
		}

		private void butProblemsIndicateNone_Click(object sender,EventArgs e) {
			FormProblemDefinitions formD=new FormProblemDefinitions();
			formD.IsSelectionMode=true;
			formD.ShowDialog();
			if(formD.DialogResult!=DialogResult.OK) {
				return;
			}
			//the list should only ever contain one item.
			_diseaseDefNum=formD.SelectedProblemDefinitions[0].Id;
			textProblemsIndicateNone.Text=formD.SelectedProblemDefinitions[0].Description;
		}

		private void checkClaimProcsAllowEstimatesOnCompl_CheckedChanged(object sender,EventArgs e) {
			if(checkClaimProcsAllowEstimatesOnCompl.Checked) {//user is attempting to Allow Estimates to be created for backdated complete procedures
				InputBox inputBox=new InputBox("Please enter password");
				inputBox.ShowDialog();
				if(inputBox.DialogResult!=DialogResult.OK) {
					checkClaimProcsAllowEstimatesOnCompl.Checked=false;
					return;
				}
				if(inputBox.textResult.Text!="abracadabra") {//To prevent unaware users from clicking this box
					checkClaimProcsAllowEstimatesOnCompl.Checked=false;
					MessageBox.Show("Wrong password");
					return;
				}
			}
		}

		private void checkDxIcdVersion_Click(object sender,EventArgs e) {
			SetIcdLabels();
		}

		private void checkProcLockingIsAllowed_Click(object sender,EventArgs e) {
			if(checkProcLockingIsAllowed.Checked) {//if user is checking box			
				if(!MsgBox.Show(MsgBoxButtons.OKCancel,"This option is not normally used, because all notes are already locked internally, and all changes to notes are viewable in the audit mode of the Chart module.  This option is only for offices that insist on locking each procedure and only allowing notes to be appended.  Using this option, there really is no way to unlock a procedure, regardless of security permission.  So locked procedures can instead be marked as invalid in the case of mistakes.  But it's a hassle to mark procedures invalid, and they also cause clutter.  This option can be turned off later, but locked procedures will remain locked.\r\n\r\nContinue anyway?")) {
					checkProcLockingIsAllowed.Checked=false;
				}
			}
			else {//unchecking box
				MessageBox.Show("Turning off this option will not affect any procedures that are already locked or invalidated.");
			}
		}
		#endregion Methods - Event Handlers Chart

		#region Methods - Event Handlers Images

		#endregion Methods - Event Handlers Images

		#region Methods - Event Handlers Manage

		#endregion Methods - Event Handlers Manage
		
		#region Methods - Appts
		private void FillAppts(){
			BrokenApptProcedure brokenApptCodeDB=(BrokenApptProcedure)PrefC.GetInt(PreferenceName.BrokenApptProcedure);
			foreach(BrokenApptProcedure option in Enum.GetValues(typeof(BrokenApptProcedure))) {
				if(option==BrokenApptProcedure.Missed && !ProcedureCodes.HasMissedCode()) {
					continue;
				}
				if(option==BrokenApptProcedure.Cancelled && !ProcedureCodes.HasCancelledCode()) {
					continue;
				}
				if(option==BrokenApptProcedure.Both && (!ProcedureCodes.HasMissedCode() || !ProcedureCodes.HasCancelledCode())) {
					continue;
				}
				_listBrokenApptProcedures.Add(option);
				int index=comboBrokenApptProc.Items.Add(option.ToString());
				if(option==brokenApptCodeDB) {
					comboBrokenApptProc.SelectedIndex=index;
				}
			}
			if(comboBrokenApptProc.Items.Count==1) {//None
				comboBrokenApptProc.SelectedIndex=0;
				comboBrokenApptProc.Enabled=false;
			}
			checkSolidBlockouts.Checked=Preferences.GetBool(PreferenceName.SolidBlockouts);
			checkBrokenApptAdjustment.Checked=Preferences.GetBool(PreferenceName.BrokenApptAdjustment);
			checkBrokenApptCommLog.Checked=Preferences.GetBool(PreferenceName.BrokenApptCommLog);
			//checkBrokenApptNote.Checked=Prefs.GetBool(PrefName.BrokenApptCommLogNotAdjustment);
			checkApptBubbleDelay.Checked = Preferences.GetBool(PreferenceName.ApptBubbleDelay);
			checkAppointmentBubblesDisabled.Checked=Preferences.GetBool(PreferenceName.AppointmentBubblesDisabled);
			_listDefsPosAdjTypes=Definitions.GetPositiveAdjTypes();
			_listDefsNegAdjTypes=Definitions.GetNegativeAdjTypes();
			comboBrokenApptAdjType.Items.AddDefs(_listDefsPosAdjTypes);
			comboBrokenApptAdjType.SetSelectedDefNum(Preferences.GetLong(PreferenceName.BrokenAppointmentAdjustmentType));
			comboProcDiscountType.Items.AddDefs(_listDefsNegAdjTypes);
			comboProcDiscountType.SetSelectedDefNum(Preferences.GetLong(PreferenceName.TreatPlanDiscountAdjustmentType));
			textDiscountPercentage.Text=Preferences.GetDouble(PreferenceName.TreatPlanDiscountPercent).ToString();
			checkApptExclamation.Checked=Preferences.GetBool(PreferenceName.ApptExclamationShowForUnsentIns);
			comboTimeArrived.Items.AddDefNone();
			comboTimeArrived.SelectedIndex=0;
			comboTimeArrived.Items.AddDefs(Definitions.GetDefsForCategory(DefinitionCategory.ApptConfirmed,true));
			comboTimeArrived.SetSelectedDefNum(Preferences.GetLong(PreferenceName.AppointmentTimeArrivedTrigger));
			comboTimeSeated.Items.AddDefNone();
			comboTimeSeated.SelectedIndex=0;
			comboTimeSeated.Items.AddDefs(Definitions.GetDefsForCategory(DefinitionCategory.ApptConfirmed,true));
			comboTimeSeated.SetSelectedDefNum(Preferences.GetLong(PreferenceName.AppointmentTimeSeatedTrigger));
			comboTimeDismissed.Items.AddDefNone();
			comboTimeDismissed.SelectedIndex=0;
			comboTimeDismissed.Items.AddDefs(Definitions.GetDefsForCategory(DefinitionCategory.ApptConfirmed,true));
			comboTimeDismissed.SetSelectedDefNum(Preferences.GetLong(PreferenceName.AppointmentTimeDismissedTrigger));
			checkApptRefreshEveryMinute.Checked=Preferences.GetBool(PreferenceName.ApptModuleRefreshesEveryMinute);
			foreach(SearchBehaviorCriteria searchBehavior in Enum.GetValues(typeof(SearchBehaviorCriteria))) {
				comboSearchBehavior.Items.Add(searchBehavior.GetDescription());
			}
			ODBoxItem<double> comboItem;
			for(int i=0;i<11;i++) {
				double seconds=(double)i/10;
				if(i==0) {
					comboItem = new ODBoxItem<double>("No delay",seconds);
				}
				else {
					comboItem = new ODBoxItem<double>(seconds.ToString("f1") + " "+"seconds",seconds);
				}
				comboDelay.Items.Add(comboItem);
				if(Preferences.GetDouble(PreferenceName.FormClickDelay)==seconds) {
					comboDelay.SelectedIndex = i;
				}
			}
			comboSearchBehavior.SelectedIndex=PrefC.GetInt(PreferenceName.AppointmentSearchBehavior);
			checkAppointmentTimeIsLocked.Checked=Preferences.GetBool(PreferenceName.AppointmentTimeIsLocked);
			textApptBubNoteLength.Text=PrefC.GetInt(PreferenceName.AppointmentBubblesNoteLength).ToString();
			checkWaitingRoomFilterByView.Checked=Preferences.GetBool(PreferenceName.WaitingRoomFilterByView);
			textWaitRoomWarn.Text=PrefC.GetInt(PreferenceName.WaitingRoomAlertTime).ToString();
			butColor.BackColor=PrefC.GetColor(PreferenceName.WaitingRoomAlertColor);
			butApptLineColor.BackColor=PrefC.GetColor(PreferenceName.AppointmentTimeLineColor);
			checkApptModuleDefaultToWeek.Checked=Preferences.GetBool(PreferenceName.ApptModuleDefaultToWeek);
			checkApptTimeReset.Checked=Preferences.GetBool(PreferenceName.AppointmentClinicTimeReset);
			if(!PrefC.HasClinicsEnabled) {
				checkApptTimeReset.Visible=false;
			}
			checkApptModuleAdjInProd.Checked=Preferences.GetBool(PreferenceName.ApptModuleAdjustmentsInProd);
			checkUseOpHygProv.Checked=Preferences.GetBool(PreferenceName.ApptSecondaryProviderConsiderOpOnly);
			checkApptModuleProductionUsesOps.Checked=Preferences.GetBool(PreferenceName.ApptModuleProductionUsesOps);
			checkApptsRequireProcs.Checked=Preferences.GetBool(PreferenceName.ApptsRequireProc);
			checkApptAllowFutureComplete.Checked=Preferences.GetBool(PreferenceName.ApptAllowFutureComplete);
			checkApptAllowEmptyComplete.Checked=Preferences.GetBool(PreferenceName.ApptAllowEmptyComplete);
			comboApptSchedEnforceSpecialty.Items.AddRange(Enum.GetValues(typeof(ApptSchedEnforceSpecialty)).OfType<ApptSchedEnforceSpecialty>()
				.Select(x => x.GetDescription()).ToArray());
			comboApptSchedEnforceSpecialty.SelectedIndex=PrefC.GetInt(PreferenceName.ApptSchedEnforceSpecialty);
			if(!PrefC.HasClinicsEnabled) {
				comboApptSchedEnforceSpecialty.Visible=false;
				labelApptSchedEnforceSpecialty.Visible=false;
			}
			textApptWithoutProcsDefaultLength.Text=Preferences.GetString(PreferenceName.AppointmentWithoutProcsDefaultLength);
			checkReplaceBlockouts.Checked=Preferences.GetBool(PreferenceName.ReplaceExistingBlockout);
			checkUnscheduledListNoRecalls.Checked=Preferences.GetBool(PreferenceName.UnscheduledListNoRecalls);
			textApptAutoRefreshRange.Text= Preferences.GetString(PreferenceName.ApptAutoRefreshRange);
			checkPreventChangesToComplAppts.Checked= Preferences.GetBool(PreferenceName.ApptPreventChangesToCompleted);
			checkApptsAllowOverlap.Checked=Preferences.GetBool(PreferenceName.ApptsAllowOverlap, true);
			textApptFontSize.Text= Preferences.GetString(PreferenceName.ApptFontSize);
			textApptProvbarWidth.Text= Preferences.GetString(PreferenceName.ApptProvbarWidth);
			checkBrokenApptRequiredOnMove.Checked= Preferences.GetBool(PreferenceName.BrokenApptRequiredOnMove);
		}

		private static YN CheckStateToYN(CheckState checkState)
        {
			switch (checkState)
            {
				case CheckState.Checked:
					return YN.Yes;

				case CheckState.Unchecked:
					return YN.No;
            }

			return YN.Unknown;
        }

		///<summary>Returns false if validation fails.</summary>
		private bool SaveAppts(){
			int noteLength=0;
			if(!int.TryParse(textApptBubNoteLength.Text,out noteLength)) {
				MessageBox.Show("Max appointment note length is invalid. Please enter a valid number to continue.");
				return false;
			}
			if(noteLength<0) {
				MessageBox.Show("Max appointment note length cannot be a negative number.");
				return false;
			}
			int waitingRoomAlertTime=0;
			try {
				waitingRoomAlertTime=PIn.Int(textWaitRoomWarn.Text);
				if(waitingRoomAlertTime<0) {
					throw new ApplicationException("Waiting room time cannot be negative");//User never sees this message.
				}
			}
			catch {
				MessageBox.Show("Waiting room alert time is invalid.");
				return false;
			}
			if(textApptWithoutProcsDefaultLength.errorProvider1.GetError(textApptWithoutProcsDefaultLength)!=""
				| textApptAutoRefreshRange.errorProvider1.GetError(textApptAutoRefreshRange)!="")
			{
				MessageBox.Show("Please fix data entry errors first.");
				return false;
			}
			float apptFontSize=0;
			if(!float.TryParse(textApptFontSize.Text,out apptFontSize)){
				MessageBox.Show("Appt Font Size invalid.");
				return false;
			}
			if(apptFontSize<1 || apptFontSize>40){
				MessageBox.Show("Appt Font Size must be between 1 and 40.");
				return false;
			}
			if(!textApptProvbarWidth.IsValid) {
				MessageBox.Show("Please fix data errors first.");
				return false;
			}
			_changed|=Preferences.Set(PreferenceName.AppointmentBubblesDisabled,checkAppointmentBubblesDisabled.Checked);
			_changed|=Preferences.Set(PreferenceName.ApptBubbleDelay,checkApptBubbleDelay.Checked);
			_changed|=Preferences.Set(PreferenceName.SolidBlockouts,checkSolidBlockouts.Checked);
			//_changed|=Prefs.UpdateBool(PrefName.BrokenApptCommLogNotAdjustment,checkBrokenApptNote.Checked) //Deprecated
			_changed|=Preferences.Set(PreferenceName.BrokenApptAdjustment,checkBrokenApptAdjustment.Checked);
			_changed|=Preferences.Set(PreferenceName.BrokenApptCommLog,checkBrokenApptCommLog.Checked);
			_changed|=Preferences.Set(PreferenceName.BrokenApptProcedure,(int)_listBrokenApptProcedures[comboBrokenApptProc.SelectedIndex]);
			_changed|=Preferences.Set(PreferenceName.ApptExclamationShowForUnsentIns,checkApptExclamation.Checked);
			_changed|=Preferences.Set(PreferenceName.ApptModuleRefreshesEveryMinute,checkApptRefreshEveryMinute.Checked);
			_changed|=Preferences.Set(PreferenceName.AppointmentSearchBehavior,comboSearchBehavior.SelectedIndex);
			_changed|=Preferences.Set(PreferenceName.AppointmentTimeIsLocked,checkAppointmentTimeIsLocked.Checked);
			_changed|=Preferences.Set(PreferenceName.AppointmentBubblesNoteLength,noteLength);
			_changed|=Preferences.Set(PreferenceName.WaitingRoomFilterByView,checkWaitingRoomFilterByView.Checked);
			_changed|=Preferences.Set(PreferenceName.WaitingRoomAlertTime,waitingRoomAlertTime);
			_changed|=Preferences.Set(PreferenceName.WaitingRoomAlertColor,butColor.BackColor.ToArgb());
			_changed|=Preferences.Set(PreferenceName.AppointmentTimeLineColor,butApptLineColor.BackColor.ToArgb());
			_changed|=Preferences.Set(PreferenceName.ApptModuleDefaultToWeek,checkApptModuleDefaultToWeek.Checked);
			_changed|=Preferences.Set(PreferenceName.AppointmentClinicTimeReset,checkApptTimeReset.Checked);
			_changed|=Preferences.Set(PreferenceName.ApptModuleAdjustmentsInProd,checkApptModuleAdjInProd.Checked);
			_changed|=Preferences.Set(PreferenceName.ApptSecondaryProviderConsiderOpOnly,checkUseOpHygProv.Checked);
			_changed|=Preferences.Set(PreferenceName.ApptModuleProductionUsesOps,checkApptModuleProductionUsesOps.Checked);
			_changed|=Preferences.Set(PreferenceName.ApptsRequireProc,checkApptsRequireProcs.Checked);
			_changed|=Preferences.Set(PreferenceName.ApptAllowFutureComplete,checkApptAllowFutureComplete.Checked);
			_changed|=Preferences.Set(PreferenceName.ApptAllowEmptyComplete,checkApptAllowEmptyComplete.Checked);
			_changed|=Preferences.Set(PreferenceName.ApptSchedEnforceSpecialty,comboApptSchedEnforceSpecialty.SelectedIndex);
			_changed|=Preferences.Set(PreferenceName.FormClickDelay,((ODBoxItem<double>)comboDelay.Items[comboDelay.SelectedIndex]).Tag);
			_changed|=Preferences.Set(PreferenceName.AppointmentWithoutProcsDefaultLength,textApptWithoutProcsDefaultLength.Text);
			_changed|=Preferences.Set(PreferenceName.ReplaceExistingBlockout,checkReplaceBlockouts.Checked);
			_changed|=Preferences.Set(PreferenceName.UnscheduledListNoRecalls,checkUnscheduledListNoRecalls.Checked);
			_changed|=Preferences.Set(PreferenceName.ApptAutoRefreshRange,textApptAutoRefreshRange.Text);
			_changed|=Preferences.Set(PreferenceName.ApptPreventChangesToCompleted,checkPreventChangesToComplAppts.Checked);
			_changed|=Preferences.Set(PreferenceName.ApptsAllowOverlap, checkApptsAllowOverlap.Checked);
			_changed|=Preferences.Set(nameof(PreferenceName.ApptFontSize),apptFontSize.ToString());
			_changed|=Preferences.Set(nameof(PreferenceName.ApptProvbarWidth),PIn.Int(textApptProvbarWidth.Text));
			_changed|=Preferences.Set(PreferenceName.BrokenApptRequiredOnMove,checkBrokenApptRequiredOnMove.Checked);
			if(comboBrokenApptAdjType.SelectedIndex!=-1) {
				_changed|=Preferences.Set(PreferenceName.BrokenAppointmentAdjustmentType,comboBrokenApptAdjType.GetSelectedDefNum());
			}
			if(comboProcDiscountType.SelectedIndex!=-1) {
				_changed|=Preferences.Set(PreferenceName.TreatPlanDiscountAdjustmentType,comboProcDiscountType.GetSelectedDefNum());
			}
			long timeArrivedTrigger=0;
			if(comboTimeArrived.SelectedIndex>0){
				timeArrivedTrigger=comboTimeArrived.GetSelectedDefNum();
			}
			List<string> listTriggerNewNums=new List<string>();
			if(Preferences.Set(PreferenceName.AppointmentTimeArrivedTrigger,timeArrivedTrigger)){
				listTriggerNewNums.Add(POut.Long(timeArrivedTrigger));
				_changed=true;
			}
			long timeSeatedTrigger=0;
			if(comboTimeSeated.SelectedIndex>0){
				timeSeatedTrigger=comboTimeSeated.GetSelectedDefNum();
			}
			if(Preferences.Set(PreferenceName.AppointmentTimeSeatedTrigger,timeSeatedTrigger)){
				listTriggerNewNums.Add(POut.Long(timeSeatedTrigger));
				_changed=true;
			}
			long timeDismissedTrigger=0;
			if(comboTimeDismissed.SelectedIndex>0){
				timeDismissedTrigger=comboTimeDismissed.GetSelectedDefNum();
			}
			if(Preferences.Set(PreferenceName.AppointmentTimeDismissedTrigger,timeDismissedTrigger)){
				listTriggerNewNums.Add(POut.Long(timeDismissedTrigger));
				_changed=true;
			}
			if(listTriggerNewNums.Count>0) {
				//Adds the appointment triggers to the list of confirmation statuses excluded from sending eConfirms,eReminders, and eThankYous.
				List<string> listEConfirm= Preferences.GetString(PreferenceName.ApptConfirmExcludeEConfirm).Split(',')
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Union(listTriggerNewNums).ToList();
				List<string> listESend= Preferences.GetString(PreferenceName.ApptConfirmExcludeESend).Split(',')
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Union(listTriggerNewNums).ToList();
				List<string> listERemind= Preferences.GetString(PreferenceName.ApptConfirmExcludeERemind).Split(',')
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Union(listTriggerNewNums).ToList();
				List<string> listEThanks=Preferences.GetString(PreferenceName.ApptConfirmExcludeEThankYou).Split(',')
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Union(listTriggerNewNums).ToList();
				//Update new Value strings in database.  We don't remove the old ones.
				Preferences.Set(PreferenceName.ApptConfirmExcludeEConfirm,string.Join(",",listEConfirm));
				Preferences.Set(PreferenceName.ApptConfirmExcludeESend,string.Join(",",listESend));
				Preferences.Set(PreferenceName.ApptConfirmExcludeERemind,string.Join(",",listERemind));
				Preferences.Set(PreferenceName.ApptConfirmExcludeEThankYou,string.Join(",",listEThanks));
			}
			return true;
		}
		#endregion Methods - Appts

		#region Methods - Family
		private void FillFamily(){
			checkInsurancePlansShared.Checked=Preferences.GetBool(PreferenceName.InsurancePlansShared);
			checkPPOpercentage.Checked=Preferences.GetBool(PreferenceName.InsDefaultPPOpercent);
			checkAllowedFeeSchedsAutomate.Checked=Preferences.GetBool(PreferenceName.AllowedFeeSchedsAutomate);
			checkCoPayFeeScheduleBlankLikeZero.Checked=Preferences.GetBool(PreferenceName.CoPay_FeeSchedule_BlankLikeZero);
			checkFixedBenefitBlankLikeZero.Checked=Preferences.GetBool(PreferenceName.FixedBenefitBlankLikeZero);
			checkInsDefaultShowUCRonClaims.Checked=Preferences.GetBool(PreferenceName.InsDefaultShowUCRonClaims);
			checkInsDefaultAssignmentOfBenefits.Checked=Preferences.GetBool(PreferenceName.InsDefaultAssignBen);
			checkInsPPOsecWriteoffs.Checked=Preferences.GetBool(PreferenceName.InsPPOsecWriteoffs);
			for(int i=0;i<Enum.GetNames(typeof(EnumCobRule)).Length;i++) {
				comboCobRule.Items.Add(Enum.GetNames(typeof(EnumCobRule))[i]);
			}
			comboCobRule.SelectedIndex=PrefC.GetInt(PreferenceName.InsDefaultCobRule);
			checkTextMsgOkStatusTreatAsNo.Checked=Preferences.GetBool(PreferenceName.TextMsgOkStatusTreatAsNo);
			checkFamPhiAccess.Checked=Preferences.GetBool(PreferenceName.FamPhiAccess);
			checkGoogleAddress.Checked=Preferences.GetBool(PreferenceName.ShowFeatureGoogleMaps);
			checkSelectProv.Checked=Preferences.GetBool(PreferenceName.PriProvDefaultToSelectProv);
			if(!Preferences.GetBool(PreferenceName.ShowFeatureSuperfamilies)) {
				groupBoxSuperFamily.Visible=false;
			}
			else {
				foreach(SortStrategy option in Enum.GetValues(typeof(SortStrategy))) {
					comboSuperFamSort.Items.Add(option.GetDescription());
				}
				comboSuperFamSort.SelectedIndex=PrefC.GetInt(PreferenceName.SuperFamSortStrategy);
				checkSuperFamSync.Checked=Preferences.GetBool(PreferenceName.PatientAllSuperFamilySync);
				checkSuperFamAddIns.Checked=Preferences.GetBool(PreferenceName.SuperFamNewPatAddIns);
				checkSuperFamCloneCreate.Checked=Preferences.GetBool(PreferenceName.CloneCreateSuperFamily);
			}
			//users should only see the claimsnapshot tab page if they have it set to something other than ClaimCreate.
			//if a user wants to be able to change claimsnapshot settings, the following MySQL statement should be run:
			//UPDATE preference SET ValueString = 'Service'	 WHERE PrefName = 'ClaimSnapshotTriggerType'
			if(PIn.Enum<ClaimSnapshotTrigger>(Preferences.GetString(PreferenceName.ClaimSnapshotTriggerType),true) == ClaimSnapshotTrigger.ClaimCreate) {
				groupBoxClaimSnapshot.Visible=false;
			}
			foreach(ClaimSnapshotTrigger trigger in Enum.GetValues(typeof(ClaimSnapshotTrigger))) {
				comboClaimSnapshotTrigger.Items.Add(trigger.GetDescription());
			}
			comboClaimSnapshotTrigger.SelectedIndex=(int)PIn.Enum<ClaimSnapshotTrigger>(Preferences.GetString(PreferenceName.ClaimSnapshotTriggerType),true);
			textClaimSnapshotRunTime.Text=PrefC.GetDate(PreferenceName.ClaimSnapshotRunTime).ToShortTimeString();
			checkClaimUseOverrideProcDescript.Checked=Preferences.GetBool(PreferenceName.ClaimPrintProcChartedDesc);
			checkClaimTrackingRequireError.Checked=Preferences.GetBool(PreferenceName.ClaimTrackingRequiresError);
			checkPatInitBillingTypeFromPriInsPlan.Checked=Preferences.GetBool(PreferenceName.PatInitBillingTypeFromPriInsPlan);
			checkPreferredReferrals.Checked=Preferences.GetBool(PreferenceName.ShowPreferedReferrals);
			checkAutoFillPatEmail.Checked=Preferences.GetBool(PreferenceName.AddFamilyInheritsEmail);
			if(!PrefC.HasClinicsEnabled) {
				checkAllowPatsAtHQ.Visible=false;
			}
			checkAllowPatsAtHQ.Checked=Preferences.GetBool(PreferenceName.ClinicAllowPatientsAtHeadquarters);
			checkInsPlanExclusionsUseUCR.Checked=Preferences.GetBool(PreferenceName.InsPlanUseUcrFeeForExclusions);
			checkInsPlanExclusionsMarkDoNotBill.Checked=Preferences.GetBool(PreferenceName.InsPlanExclusionsMarkDoNotBillIns);
			checkPatientSSNMasked.Checked=Preferences.GetBool(PreferenceName.PatientSSNMasked);
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				checkPatientSSNMasked.Text="Mask patient Social Insurance Numbers";
			}
			checkPatientDOBMasked.Checked=Preferences.GetBool(PreferenceName.PatientDOBMasked);
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveFamily(){
			DateTime claimSnapshotRunTime=DateTime.MinValue;
			if(!DateTime.TryParse(textClaimSnapshotRunTime.Text,out claimSnapshotRunTime)) {
				MessageBox.Show("Service Snapshot Run Time must be a valid time value.");
				return false;
			}
			claimSnapshotRunTime=new DateTime(1881,01,01,claimSnapshotRunTime.Hour,claimSnapshotRunTime.Minute,claimSnapshotRunTime.Second);
			_changed|=Preferences.Set(PreferenceName.InsurancePlansShared,checkInsurancePlansShared.Checked);
			_changed|=Preferences.Set(PreferenceName.InsDefaultPPOpercent,checkPPOpercentage.Checked);
			_changed|=Preferences.Set(PreferenceName.AllowedFeeSchedsAutomate,checkAllowedFeeSchedsAutomate.Checked);
			_changed|=Preferences.Set(PreferenceName.CoPay_FeeSchedule_BlankLikeZero,checkCoPayFeeScheduleBlankLikeZero.Checked);
			_changed|=Preferences.Set(PreferenceName.FixedBenefitBlankLikeZero,checkFixedBenefitBlankLikeZero.Checked);
			_changed|=Preferences.Set(PreferenceName.InsDefaultShowUCRonClaims,checkInsDefaultShowUCRonClaims.Checked);
			_changed|=Preferences.Set(PreferenceName.InsDefaultAssignBen,checkInsDefaultAssignmentOfBenefits.Checked);
			_changed|=Preferences.Set(PreferenceName.InsDefaultCobRule,comboCobRule.SelectedIndex);
			_changed|=Preferences.Set(PreferenceName.TextMsgOkStatusTreatAsNo,checkTextMsgOkStatusTreatAsNo.Checked);
			_changed|=Preferences.Set(PreferenceName.FamPhiAccess,checkFamPhiAccess.Checked);
			_changed|=Preferences.Set(PreferenceName.InsPPOsecWriteoffs,checkInsPPOsecWriteoffs.Checked);
			_changed|=Preferences.Set(PreferenceName.ShowFeatureGoogleMaps,checkGoogleAddress.Checked);
			_changed|=Preferences.Set(PreferenceName.PriProvDefaultToSelectProv,checkSelectProv.Checked);
			_changed|=Preferences.Set(PreferenceName.SuperFamSortStrategy,comboSuperFamSort.SelectedIndex);
			_changed|=Preferences.Set(PreferenceName.PatientAllSuperFamilySync,checkSuperFamSync.Checked);
			_changed|=Preferences.Set(PreferenceName.SuperFamNewPatAddIns,checkSuperFamAddIns.Checked);
			_changed|=Preferences.Set(PreferenceName.CloneCreateSuperFamily,checkSuperFamCloneCreate.Checked);
			_changed|=Preferences.Set(PreferenceName.ClaimSnapshotRunTime,claimSnapshotRunTime);
			_changed|=Preferences.Set(PreferenceName.ClaimPrintProcChartedDesc,checkClaimUseOverrideProcDescript.Checked);
			_changed|=Preferences.Set(PreferenceName.ClaimTrackingRequiresError,checkClaimTrackingRequireError.Checked);
			_changed|=Preferences.Set(PreferenceName.PatInitBillingTypeFromPriInsPlan,checkPatInitBillingTypeFromPriInsPlan.Checked);
			_changed|=Preferences.Set(PreferenceName.ShowPreferedReferrals,checkPreferredReferrals.Checked);
			_changed|=Preferences.Set(PreferenceName.AddFamilyInheritsEmail,checkAutoFillPatEmail.Checked);
			_changed|=Preferences.Set(PreferenceName.ClinicAllowPatientsAtHeadquarters,checkAllowPatsAtHQ.Checked);
			_changed|=Preferences.Set(PreferenceName.InsPlanExclusionsMarkDoNotBillIns,checkInsPlanExclusionsMarkDoNotBill.Checked);
			_changed|=Preferences.Set(PreferenceName.InsPlanUseUcrFeeForExclusions,checkInsPlanExclusionsUseUCR.Checked);
			_changed|=Preferences.Set(PreferenceName.PatientSSNMasked,checkPatientSSNMasked.Checked);
			_changed|=Preferences.Set(PreferenceName.PatientDOBMasked,checkPatientDOBMasked.Checked);
			foreach(ClaimSnapshotTrigger trigger in Enum.GetValues(typeof(ClaimSnapshotTrigger))) {
				if(trigger.GetDescription()==comboClaimSnapshotTrigger.Text) {
					if(Preferences.Set(PreferenceName.ClaimSnapshotTriggerType,trigger.ToString())) {
						_changed=true;
					}
					break;
				}
			}
			return true;
		}
		#endregion Methods - Family

		#region Methods - Account
		private void FillAccount(){
			#region Pay/Adj Tab
			checkStoreCCTokens.Checked=Preferences.GetBool(PreferenceName.StoreCCtokens);
			foreach(PayClinicSetting prompt in Enum.GetValues(typeof(PayClinicSetting))) {
				comboPaymentClinicSetting.Items.Add(prompt.GetDescription());
			}
			comboPaymentClinicSetting.SelectedIndex=PrefC.GetInt(PreferenceName.PaymentClinicSetting);
			checkPaymentsPromptForPayType.Checked=Preferences.GetBool(PreferenceName.PaymentsPromptForPayType);
			comboUnallocatedSplits.Items.AddDefs(Definitions.GetDefsForCategory(DefinitionCategory.PaySplitUnearnedType,true));
			comboUnallocatedSplits.SetSelectedDefNum(Preferences.GetLong(PreferenceName.PrepaymentUnearnedType));
			comboPayPlanAdj.Items.AddDefs(_listDefsNegAdjTypes);
			comboPayPlanAdj.SetSelectedDefNum(Preferences.GetLong(PreferenceName.PayPlanAdjType));
			comboFinanceChargeAdjType.Items.AddDefs(_listDefsPosAdjTypes);
			comboFinanceChargeAdjType.SetSelectedDefNum(Preferences.GetLong(PreferenceName.FinanceChargeAdjustmentType));
			comboBillingChargeAdjType.Items.AddDefs(_listDefsPosAdjTypes);
			comboBillingChargeAdjType.SetSelectedDefNum(Preferences.GetLong(PreferenceName.BillingChargeAdjustmentType));
			comboSalesTaxAdjType.Items.AddDefs(_listDefsPosAdjTypes);
			comboSalesTaxAdjType.SetSelectedDefNum(Preferences.GetLong(PreferenceName.SalesTaxAdjustmentType));
			textTaxPercent.Text=Preferences.GetDouble(PreferenceName.SalesTaxPercentage).ToString();
			string[] arrayDefNums=Preferences.GetString(PreferenceName.BadDebtAdjustmentTypes).Split(new char[] {','}); //comma-delimited list.
			List<long> listBadAdjDefNums = new List<long>();
			foreach(string strDefNum in arrayDefNums) {
				listBadAdjDefNums.Add(PIn.Long(strDefNum));
			}
			FillListboxBadDebt(Definitions.GetDefs(DefinitionCategory.AdjTypes,listBadAdjDefNums));
			checkAllowFutureDebits.Checked=Preferences.GetBool(PreferenceName.AccountAllowFutureDebits);
			checkAllowEmailCCReceipt.Checked=Preferences.GetBool(PreferenceName.AllowEmailCCReceipt);
			List<RigorousAccounting> listEnums=Enum.GetValues(typeof(RigorousAccounting)).OfType<RigorousAccounting>().ToList();
			for(int i=0;i<listEnums.Count;i++) {
				comboRigorousAccounting.Items.Add(listEnums[i].GetDescription());
			}
			comboRigorousAccounting.SelectedIndex=PrefC.GetInt(PreferenceName.RigorousAccounting);
			List<RigorousAdjustments> listAdjEnums=Enum.GetValues(typeof(RigorousAdjustments)).OfType<RigorousAdjustments>().ToList();
			for(int i=0;i<listAdjEnums.Count;i++) {
				comboRigorousAdjustments.Items.Add(listAdjEnums[i].GetDescription());
			}
			comboRigorousAdjustments.SelectedIndex=PrefC.GetInt(PreferenceName.RigorousAdjustments);
			checkHidePaysplits.Checked=Preferences.GetBool(PreferenceName.PaymentWindowDefaultHideSplits);
			checkPaymentsTransferPatientIncomeOnly.Checked=Preferences.GetBool(PreferenceName.PaymentsTransferPatientIncomeOnly);
			checkAllowPrepayProvider.Checked=Preferences.GetBool(PreferenceName.AllowPrepayProvider);
			comboRecurringChargePayType.Items.AddDefNone("("+"default"+")");
			comboRecurringChargePayType.Items.AddDefs(Definitions.GetDefsForCategory(DefinitionCategory.PaymentTypes,true));
			comboRecurringChargePayType.SetSelectedDefNum(Preferences.GetLong(PreferenceName.RecurringChargesPayTypeCC)); 
			_ynPrePayAllowedForTpProcs=Preferences.GetBool(PreferenceName.PrePayAllowedForTpProcs);
			checkAllowPrePayToTpProcs.Checked= Preferences.GetBool(PreferenceName.PrePayAllowedForTpProcs);
			checkIsRefundable.Checked=Preferences.GetBool(PreferenceName.TpPrePayIsNonRefundable);
			checkIsRefundable.Visible=checkAllowPrePayToTpProcs.Checked;//pref will be unchecked if parent gets turned off.
			comboTpUnearnedType.Items.AddDefs(Definitions.GetDefsForCategory(DefinitionCategory.PaySplitUnearnedType,true));
			comboTpUnearnedType.SetSelectedDefNum(Preferences.GetLong(PreferenceName.TpUnearnedType));
			#endregion Pay/Adj Tab
			#region Insurance Tab
			checkProviderIncomeShows.Checked=Preferences.GetBool(PreferenceName.ProviderIncomeTransferShows);
			checkClaimMedTypeIsInstWhenInsPlanIsMedical.Checked=Preferences.GetBool(PreferenceName.ClaimMedTypeIsInstWhenInsPlanIsMedical);
			checkClaimFormTreatDentSaysSigOnFile.Checked=Preferences.GetBool(PreferenceName.ClaimFormTreatDentSaysSigOnFile);
			textInsWriteoffDescript.Text=Preferences.GetString(PreferenceName.InsWriteoffDescript);
			textClaimAttachPath.Text=Preferences.GetString(PreferenceName.ClaimAttachExportPath);
			checkEclaimsMedicalProvTreatmentAsOrdering.Checked=Preferences.GetBool(PreferenceName.ClaimMedProvTreatmentAsOrdering);
			checkEclaimsSeparateTreatProv.Checked=Preferences.GetBool(PreferenceName.EclaimsSeparateTreatProv);
			checkClaimsValidateACN.Checked=Preferences.GetBool(PreferenceName.ClaimsValidateACN);
			checkAllowProcAdjFromClaim.Checked=Preferences.GetBool(PreferenceName.AllowProcAdjFromClaim);
			comboClaimCredit.Items.AddRange(Enum.GetNames(typeof(ClaimProcCreditsGreaterThanProcFee)));
			comboClaimCredit.SelectedIndex=PrefC.GetInt(PreferenceName.ClaimProcAllowCreditsGreaterThanProcFee);
			checkAllowFuturePayments.Checked=Preferences.GetBool(PreferenceName.AllowFutureInsPayments);
			textClaimIdentifier.Text=Preferences.GetString(PreferenceName.ClaimIdPrefix);
			foreach(ClaimZeroDollarProcBehavior procBehavior in Enum.GetValues(typeof(ClaimZeroDollarProcBehavior))) {
				comboZeroDollarProcClaimBehavior.Items.Add(procBehavior.ToString());
			}
			comboZeroDollarProcClaimBehavior.SelectedIndex=PrefC.GetInt(PreferenceName.ClaimZeroDollarProcBehavior);
			checkClaimTrackingExcludeNone.Checked=Preferences.GetBool(PreferenceName.ClaimTrackingStatusExcludesNone);
			checkInsPayNoWriteoffMoreThanProc.Checked=Preferences.GetBool(PreferenceName.InsPayNoWriteoffMoreThanProc);
			checkPromptForSecondaryClaim.Checked=Preferences.GetBool(PreferenceName.PromptForSecondaryClaim);
			checkInsEstRecalcReceived.Checked=Preferences.GetBool(PreferenceName.InsEstRecalcReceived);
			checkCanadianPpoLabEst.Checked=Preferences.GetBool(PreferenceName.CanadaCreatePpoLabEst);
			#endregion Insurance Tab
			#region Misc Account Tab
			checkBalancesDontSubtractIns.Checked=Preferences.GetBool(PreferenceName.BalancesDontSubtractIns);
			checkAgingMonthly.Checked=Preferences.GetBool(PreferenceName.AgingCalculatedMonthlyInsteadOfDaily);
			if(!Preferences.GetBool(PreferenceName.AgingIsEnterprise)) {//AgingIsEnterprise requires aging to be daily
				checkAgingMonthly.Text="Aging calculated monthly instead of daily";
				checkAgingMonthly.Enabled=true;
			}
			checkAccountShowPaymentNums.Checked=Preferences.GetBool(PreferenceName.AccountShowPaymentNums);
			checkShowFamilyCommByDefault.Checked=Preferences.GetBool(PreferenceName.ShowAccountFamilyCommEntries);
			checkRecurChargPriProv.Checked=Preferences.GetBool(PreferenceName.RecurringChargesUsePriProv);
			checkPpoUseUcr.Checked=Preferences.GetBool(PreferenceName.InsPpoAlwaysUseUcrFee);
			checkRecurringChargesUseTransDate.Checked=Preferences.GetBool(PreferenceName.RecurringChargesUseTransDate);
			checkStatementInvoiceGridShowWriteoffs.Checked=Preferences.GetBool(PreferenceName.InvoicePaymentsGridShowNetProd);
			checkShowAllocateUnearnedPaymentPrompt.Checked=Preferences.GetBool(PreferenceName.ShowAllocateUnearnedPaymentPrompt);
			checkPayPlansExcludePastActivity.Checked=Preferences.GetBool(PreferenceName.PayPlansExcludePastActivity);
			checkPayPlansUseSheets.Checked=Preferences.GetBool(PreferenceName.PayPlansUseSheets);
			checkAllowFutureTrans.Checked=Preferences.GetBool(PreferenceName.FutureTransDatesAllowed);
			checkAgingProcLifo.CheckState=PrefC.GetYNCheckState(PreferenceName.AgingProcLifo);
			foreach(PayPlanVersions version in Enum.GetValues(typeof(PayPlanVersions))) {
				comboPayPlansVersion.Items.Add(version.GetDescription());
			}
			comboPayPlansVersion.SelectedIndex=PrefC.GetInt(PreferenceName.PayPlansVersion) - 1;
			if(comboPayPlansVersion.SelectedIndex==(int)PayPlanVersions.AgeCreditsAndDebits-1) {//Minus 1 because the enum starts at 1.
				checkHideDueNow.Visible=true;
				checkHideDueNow.Checked=Preferences.GetBool(PreferenceName.PayPlanHideDueNow);
			}
			else {
				checkHideDueNow.Visible=false;
				checkHideDueNow.Checked=false;
			}
			checkRecurringChargesAutomated.Checked=Preferences.GetBool(PreferenceName.RecurringChargesAutomatedEnabled);
			textRecurringChargesTime.Text=PrefC.GetDate(PreferenceName.RecurringChargesAutomatedTime).TimeOfDay.ToShortTimeString();
			checkRecurPatBal0.Checked=Preferences.GetBool(PreferenceName.RecurringChargesAllowedWhenNoPatBal);
			checkRepeatingChargesRunAging.Checked=Preferences.GetBool(PreferenceName.RepeatingChargesRunAging);
			checkRepeatingChargesAutomated.Checked=Preferences.GetBool(PreferenceName.RepeatingChargesAutomated);
			textRepeatingChargesAutomatedTime.Text=PrefC.GetDate(PreferenceName.RepeatingChargesAutomatedTime).TimeOfDay.ToShortTimeString();
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {
				checkCanadianPpoLabEst.Visible=false;
			}
			textDynamicPayPlan.Text=PrefC.GetDate(PreferenceName.DynamicPayPlanRunTime).TimeOfDay.ToShortTimeString();
			#endregion Misc Account Tab
		}

		private void FillListboxBadDebt(List<Definition> listSelectedDefs) {
			listboxBadDebtAdjs.Items.Clear();
			foreach(Definition defCur in listSelectedDefs) {
				listboxBadDebtAdjs.Items.Add(new ODBoxItem<Definition>(defCur.Name,defCur));
			}
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveAccount(){
			double taxPercent=0;
			if(!double.TryParse(textTaxPercent.Text,out taxPercent)) {
				MessageBox.Show("Sales Tax percent is invalid.  Please enter a valid number to continue.");
				return false;
			}
			if(taxPercent<0) {
				MessageBox.Show("Sales Tax percent cannot be a negative number.");
				return false;
			}
			if(checkRecurringChargesAutomated.Checked 
				&& (string.IsNullOrWhiteSpace(textRecurringChargesTime.Text) || !textRecurringChargesTime.IsValid)) 
			{
				MessageBox.Show("Recurring charge time must be a valid time.");
				return false;
			}
			if(checkRepeatingChargesAutomated.Checked 
				&& (string.IsNullOrWhiteSpace(textRepeatingChargesAutomatedTime.Text) || !textRepeatingChargesAutomatedTime.IsValid)) 
			{
				MessageBox.Show("Repeating charge time must be a valid time.");
				return false;
			}
			if(string.IsNullOrWhiteSpace(textDynamicPayPlan.Text) || !textDynamicPayPlan.IsValid) {
				MessageBox.Show("Dynamic payment plan time must be a valid time.");
				return false;
			}
			string strListBadDebtAdjTypes=string.Join(",",listboxBadDebtAdjs.Items.Cast<ODBoxItem<Definition>>().Select(x => x.Tag.Id));
			_changed|=Preferences.Set(PreferenceName.BalancesDontSubtractIns,checkBalancesDontSubtractIns.Checked);
			_changed|=Preferences.Set(PreferenceName.AgingCalculatedMonthlyInsteadOfDaily,checkAgingMonthly.Checked);
			_changed|=Preferences.Set(PreferenceName.StoreCCtokens,checkStoreCCTokens.Checked);
			_changed|=Preferences.Set(PreferenceName.ProviderIncomeTransferShows,checkProviderIncomeShows.Checked);
			_changed|=Preferences.Set(PreferenceName.ShowAccountFamilyCommEntries,checkShowFamilyCommByDefault.Checked);
			_changed|=Preferences.Set(PreferenceName.ClaimFormTreatDentSaysSigOnFile,checkClaimFormTreatDentSaysSigOnFile.Checked);
			_changed|=Preferences.Set(PreferenceName.ClaimAttachExportPath,textClaimAttachPath.Text);
			_changed|=Preferences.Set(PreferenceName.EclaimsSeparateTreatProv,checkEclaimsSeparateTreatProv.Checked);
			_changed|=Preferences.Set(PreferenceName.ClaimsValidateACN,checkClaimsValidateACN.Checked);
			_changed|=Preferences.Set(PreferenceName.ClaimMedTypeIsInstWhenInsPlanIsMedical,checkClaimMedTypeIsInstWhenInsPlanIsMedical.Checked);
			_changed|=Preferences.Set(PreferenceName.AccountShowPaymentNums,checkAccountShowPaymentNums.Checked);
			_changed|=Preferences.Set(PreferenceName.PayPlansUseSheets,checkPayPlansUseSheets.Checked);
			_changed|=Preferences.Set(PreferenceName.RecurringChargesUsePriProv,checkRecurChargPriProv.Checked);
			_changed|=Preferences.Set(PreferenceName.InsWriteoffDescript,textInsWriteoffDescript.Text);
			_changed|=Preferences.Set(PreferenceName.PaymentClinicSetting,comboPaymentClinicSetting.SelectedIndex);
			_changed|=Preferences.Set(PreferenceName.PaymentsPromptForPayType,checkPaymentsPromptForPayType.Checked);
			_changed|=Preferences.Set(PreferenceName.PayPlansVersion,comboPayPlansVersion.SelectedIndex+1);
			_changed|=Preferences.Set(PreferenceName.ClaimMedProvTreatmentAsOrdering,checkEclaimsMedicalProvTreatmentAsOrdering.Checked);
			_changed|=Preferences.Set(PreferenceName.InsPpoAlwaysUseUcrFee,checkPpoUseUcr.Checked);
			_changed|=Preferences.Set(PreferenceName.ProcPromptForAutoNote,checkProcsPromptForAutoNote.Checked);
			_changed|=Preferences.Set(PreferenceName.SalesTaxPercentage,taxPercent,false);//Do not round this double for Hawaii
			_changed|=Preferences.Set(PreferenceName.PayPlansExcludePastActivity,checkPayPlansExcludePastActivity.Checked);
			_changed|=Preferences.Set(PreferenceName.RecurringChargesUseTransDate,checkRecurringChargesUseTransDate.Checked);
			_changed|=Preferences.Set(PreferenceName.InvoicePaymentsGridShowNetProd,checkStatementInvoiceGridShowWriteoffs.Checked);
			_changed|=Preferences.Set(PreferenceName.AccountAllowFutureDebits,checkAllowFutureDebits.Checked);
			_changed|=Preferences.Set(PreferenceName.PayPlanHideDueNow,checkHideDueNow.Checked);
			_changed|=Preferences.Set(PreferenceName.AllowProcAdjFromClaim,checkAllowProcAdjFromClaim.Checked);
			_changed|=Preferences.Set(PreferenceName.ClaimProcAllowCreditsGreaterThanProcFee,comboClaimCredit.SelectedIndex);
			_changed|=Preferences.Set(PreferenceName.AllowEmailCCReceipt,checkAllowEmailCCReceipt.Checked);
			_changed|=Preferences.Set(PreferenceName.ClaimIdPrefix,textClaimIdentifier.Text);
			int prefRigorousAccounting=PrefC.GetInt(PreferenceName.RigorousAccounting);
			if(Preferences.Set(PreferenceName.RigorousAccounting,comboRigorousAccounting.SelectedIndex)) {
				_changed=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rigorous accounting changed from "+
					((RigorousAccounting)prefRigorousAccounting).GetDescription()+" to "
					+((RigorousAccounting)comboRigorousAccounting.SelectedIndex).GetDescription()+".");
			}
			int prefRigorousAdjustments=PrefC.GetInt(PreferenceName.RigorousAdjustments);
			if(Preferences.Set(PreferenceName.RigorousAdjustments,comboRigorousAdjustments.SelectedIndex)) {
				_changed=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rigorous adjustments changed from "+
					((RigorousAdjustments)prefRigorousAdjustments).GetDescription()+" to "
					+((RigorousAdjustments)comboRigorousAdjustments.SelectedIndex).GetDescription()+".");
			}
			//_changed|=Prefs.UpdateBool(PrefName.PayPlanHideDebitsFromAccountModule,checkHidePayPlanDebits.Checked)
			_changed|=Preferences.Set(PreferenceName.BadDebtAdjustmentTypes,strListBadDebtAdjTypes);
			_changed|=Preferences.Set(PreferenceName.AllowFutureInsPayments,checkAllowFuturePayments.Checked);
			_changed|=Preferences.Set(PreferenceName.PaymentWindowDefaultHideSplits,checkHidePaysplits.Checked);
			_changed|=Preferences.Set(PreferenceName.PaymentsTransferPatientIncomeOnly,checkPaymentsTransferPatientIncomeOnly.Checked);
			_changed|=Preferences.Set(PreferenceName.ShowAllocateUnearnedPaymentPrompt,checkShowAllocateUnearnedPaymentPrompt.Checked);
			_changed|=Preferences.Set(PreferenceName.FutureTransDatesAllowed,checkAllowFutureTrans.Checked);
			_changed|=Preferences.Set(PreferenceName.AgingProcLifo,checkAgingProcLifo.Checked);
			_changed|=Preferences.Set(PreferenceName.AllowPrepayProvider,checkAllowPrepayProvider.Checked);
			_changed|=Preferences.Set(PreferenceName.RecurringChargesAutomatedEnabled,checkRecurringChargesAutomated.Checked);
			_changed|=Preferences.Set(PreferenceName.RecurringChargesAutomatedTime,PIn.Date(textRecurringChargesTime.Text));
			_changed|=Preferences.Set(PreferenceName.RepeatingChargesAutomated,checkRepeatingChargesAutomated.Checked);
			_changed|=Preferences.Set(PreferenceName.RepeatingChargesAutomatedTime,PIn.Date(textRepeatingChargesAutomatedTime.Text));
			_changed|=Preferences.Set(PreferenceName.RepeatingChargesRunAging,checkRepeatingChargesRunAging.Checked);
			_changed|=Preferences.Set(PreferenceName.ClaimZeroDollarProcBehavior,comboZeroDollarProcClaimBehavior.SelectedIndex);
			_changed|=Preferences.Set(PreferenceName.ClaimTrackingStatusExcludesNone,checkClaimTrackingExcludeNone.Checked);
			_changed|=Preferences.Set(PreferenceName.InsPayNoWriteoffMoreThanProc,checkInsPayNoWriteoffMoreThanProc.Checked);
			_changed|=Preferences.Set(PreferenceName.PromptForSecondaryClaim,checkPromptForSecondaryClaim.Checked);
			_changed|=Preferences.Set(PreferenceName.InsEstRecalcReceived,checkInsEstRecalcReceived.Checked);
			_changed|=Preferences.Set(PreferenceName.CanadaCreatePpoLabEst,checkCanadianPpoLabEst.Checked);
			_changed|=Preferences.Set(PreferenceName.RecurringChargesPayTypeCC,comboRecurringChargePayType.GetSelectedDefNum());
			_changed|=Preferences.Set(PreferenceName.RecurringChargesAllowedWhenNoPatBal,checkRecurPatBal0.Checked);
			_changed|=Preferences.Set(PreferenceName.PrePayAllowedForTpProcs,_ynPrePayAllowedForTpProcs);
			_changed|=Preferences.Set(PreferenceName.TpUnearnedType,comboTpUnearnedType.GetSelectedDefNum());
			_changed|=Preferences.Set(PreferenceName.TpPrePayIsNonRefundable,checkIsRefundable.Checked);
			_changed|=Preferences.Set(PreferenceName.DynamicPayPlanRunTime,PIn.Date(textDynamicPayPlan.Text));
			if(comboFinanceChargeAdjType.SelectedIndex!=-1) {
				_changed|=Preferences.Set(PreferenceName.FinanceChargeAdjustmentType,comboFinanceChargeAdjType.GetSelectedDefNum());
			}
			if(comboBillingChargeAdjType.SelectedIndex!=-1) {
				_changed|=Preferences.Set(PreferenceName.BillingChargeAdjustmentType,comboBillingChargeAdjType.GetSelectedDefNum());
			}
			if(comboSalesTaxAdjType.SelectedIndex!=-1) {
				_changed|=Preferences.Set(PreferenceName.SalesTaxAdjustmentType,comboSalesTaxAdjType.GetSelectedDefNum());
			}
			if(comboPayPlanAdj.SelectedIndex!=-1) {
				_changed|=Preferences.Set(PreferenceName.PayPlanAdjType,comboPayPlanAdj.GetSelectedDefNum());
			}
			if(comboUnallocatedSplits.SelectedIndex!=-1) {
				_changed|=Preferences.Set(PreferenceName.PrepaymentUnearnedType,comboUnallocatedSplits.GetSelectedDefNum());
			}
			return true;
		}
		#endregion Methods - Account

		#region Methods - Treat' Plan
		private void FillTreatPlan(){
			textTreatNote.Text=Preferences.GetString(PreferenceName.TreatmentPlanNote);
			checkTreatPlanShowCompleted.Checked=Preferences.GetBool(PreferenceName.TreatPlanShowCompleted);
			if(Clinics.IsMedicalClinic(Clinics.ClinicId)) {
				checkTreatPlanShowCompleted.Visible=false;
			}
			else {
				checkTreatPlanShowCompleted.Checked=Preferences.GetBool(PreferenceName.TreatPlanShowCompleted);
			}
			checkTreatPlanItemized.Checked=Preferences.GetBool(PreferenceName.TreatPlanItemized);
			checkTPSaveSigned.Checked=Preferences.GetBool(PreferenceName.TreatPlanSaveSignedToPdf);
			checkFrequency.Checked=Preferences.GetBool(PreferenceName.InsChecksFrequency);
			textInsBW.Text=Preferences.GetString(PreferenceName.InsBenBWCodes);
			textInsPano.Text=Preferences.GetString(PreferenceName.InsBenPanoCodes);
			textInsExam.Text=Preferences.GetString(PreferenceName.InsBenExamCodes);
			textInsCancerScreen.Text=Preferences.GetString(PreferenceName.InsBenCancerScreeningCodes);
			textInsProphy.Text=Preferences.GetString(PreferenceName.InsBenProphyCodes);
			textInsFlouride.Text=Preferences.GetString(PreferenceName.InsBenFlourideCodes);
			textInsSealant.Text=Preferences.GetString(PreferenceName.InsBenSealantCodes);
			textInsCrown.Text=Preferences.GetString(PreferenceName.InsBenCrownCodes);
			textInsSRP.Text=Preferences.GetString(PreferenceName.InsBenSRPCodes);
			textInsDebridement.Text=Preferences.GetString(PreferenceName.InsBenFullDebridementCodes);
			textInsPerioMaint.Text=Preferences.GetString(PreferenceName.InsBenPerioMaintCodes);
			textInsDentures.Text=Preferences.GetString(PreferenceName.InsBenDenturesCodes);
			textInsImplant.Text=Preferences.GetString(PreferenceName.InsBenImplantCodes);
			if(!checkFrequency.Checked) {
				textInsBW.Enabled=false;
				textInsPano.Enabled=false;
				textInsExam.Enabled=false;
				textInsCancerScreen.Enabled=false;
				textInsProphy.Enabled=false;
				textInsFlouride.Enabled=false;
				textInsSealant.Enabled=false;
				textInsCrown.Enabled=false;
				textInsSRP.Enabled=false;
				textInsDebridement.Enabled=false;
				textInsPerioMaint.Enabled=false;
				textInsDentures.Enabled=false;
				textInsImplant.Enabled=false;
			}
			radioTreatPlanSortTooth.Checked=Preferences.GetBool(PreferenceName.TreatPlanSortByTooth) || PrefC.RandomKeys;
			//Currently, the TreatPlanSortByTooth preference gets overridden by 
			//the RandomPrimaryKeys preferece due to "Order Entered" being based on the ProcNum
			groupTreatPlanSort.Enabled=!PrefC.RandomKeys;
			textInsHistBW.Text=Preferences.GetString(PreferenceName.InsHistBWCodes);
			textInsHistDebridement.Text=Preferences.GetString(PreferenceName.InsHistDebridementCodes);
			textInsHistExam.Text=Preferences.GetString(PreferenceName.InsHistExamCodes);
			textInsHistFMX.Text=Preferences.GetString(PreferenceName.InsHistPanoCodes);
			textInsHistPerioMaint.Text=Preferences.GetString(PreferenceName.InsHistPerioMaintCodes);
			textInsHistPerioLL.Text=Preferences.GetString(PreferenceName.InsHistPerioLLCodes);
			textInsHistPerioLR.Text=Preferences.GetString(PreferenceName.InsHistPerioLRCodes);
			textInsHistPerioUL.Text=Preferences.GetString(PreferenceName.InsHistPerioULCodes);
			textInsHistPerioUR.Text=Preferences.GetString(PreferenceName.InsHistPerioURCodes);
			textInsHistProphy.Text=Preferences.GetString(PreferenceName.InsHistProphyCodes);
			checkPromptSaveTP.Checked=Preferences.GetBool(PreferenceName.TreatPlanPromptSave);
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveTreatPlan(){
			float percent=0;
			if(!float.TryParse(textDiscountPercentage.Text,out percent)) {
				MessageBox.Show("Procedure discount percent is invalid. Please enter a valid number to continue.");
				return false;
			}
			if(Preferences.GetString(PreferenceName.TreatmentPlanNote)!=textTreatNote.Text) {
				List<long> listTreatPlanNums=TreatPlans.GetNumsByNote(Preferences.GetString(PreferenceName.TreatmentPlanNote));//Find active/inactive TP's that match exactly.
				if(listTreatPlanNums.Count>0) {
					DialogResult dr=MessageBox.Show("Unsaved treatment plans found with default notes"+": "+listTreatPlanNums.Count+"\r\n"
						+"Would you like to change them now?","",MessageBoxButtons.YesNoCancel);
					switch(dr) {
						case DialogResult.Cancel:
							return false;
						case DialogResult.Yes:
						case DialogResult.OK:
							TreatPlans.UpdateNotes(textTreatNote.Text,listTreatPlanNums);//change tp notes
							break;
						default://includes "No"
							//do nothing
							break;
					}
				}
			}
			_changed|=Preferences.Set(PreferenceName.TreatmentPlanNote,textTreatNote.Text);
			_changed|=Preferences.Set(PreferenceName.TreatPlanShowCompleted,checkTreatPlanShowCompleted.Checked);
			_changed|=Preferences.Set(PreferenceName.TreatPlanDiscountPercent,percent);
			_changed|=Preferences.Set(PreferenceName.TreatPlanItemized,checkTreatPlanItemized.Checked);
			_changed|=Preferences.Set(PreferenceName.TreatPlanSaveSignedToPdf,checkTPSaveSigned.Checked);
			_changed|=Preferences.Set(PreferenceName.InsChecksFrequency,checkFrequency.Checked);
			_changed|=Preferences.Set(PreferenceName.InsBenBWCodes,textInsBW.Text);
			_changed|=Preferences.Set(PreferenceName.InsBenPanoCodes,textInsPano.Text);
			_changed|=Preferences.Set(PreferenceName.InsBenExamCodes,textInsExam.Text);
			_changed|=Preferences.Set(PreferenceName.InsBenCancerScreeningCodes,textInsCancerScreen.Text);
			_changed|=Preferences.Set(PreferenceName.InsBenProphyCodes,textInsProphy.Text);
			_changed|=Preferences.Set(PreferenceName.InsBenFlourideCodes,textInsFlouride.Text);
			_changed|=Preferences.Set(PreferenceName.InsBenSealantCodes,textInsSealant.Text);
			_changed|=Preferences.Set(PreferenceName.InsBenCrownCodes,textInsCrown.Text);
			_changed|=Preferences.Set(PreferenceName.InsBenSRPCodes,textInsSRP.Text);
			_changed|=Preferences.Set(PreferenceName.InsBenFullDebridementCodes,textInsDebridement.Text);
			_changed|=Preferences.Set(PreferenceName.InsBenPerioMaintCodes,textInsPerioMaint.Text);
			_changed|=Preferences.Set(PreferenceName.InsBenDenturesCodes,textInsDentures.Text);
			_changed|=Preferences.Set(PreferenceName.InsBenImplantCodes,textInsImplant.Text);
			_changed|=Preferences.Set(PreferenceName.TreatPlanSortByTooth,radioTreatPlanSortTooth.Checked || PrefC.RandomKeys);
			_changed|=Preferences.Set(PreferenceName.InsHistBWCodes,textInsHistBW.Text);
			_changed|=Preferences.Set(PreferenceName.InsHistDebridementCodes,textInsHistDebridement.Text);
			_changed|=Preferences.Set(PreferenceName.InsHistExamCodes,textInsHistExam.Text);
			_changed|=Preferences.Set(PreferenceName.InsHistPanoCodes,textInsHistFMX.Text);
			_changed|=Preferences.Set(PreferenceName.InsHistPerioMaintCodes,textInsHistPerioMaint.Text);
			_changed|=Preferences.Set(PreferenceName.InsHistPerioLLCodes,textInsHistPerioLL.Text);
			_changed|=Preferences.Set(PreferenceName.InsHistPerioLRCodes,textInsHistPerioLR.Text);
			_changed|=Preferences.Set(PreferenceName.InsHistPerioULCodes,textInsHistPerioUL.Text);
			_changed|=Preferences.Set(PreferenceName.InsHistPerioURCodes,textInsHistPerioUR.Text);
			_changed|=Preferences.Set(PreferenceName.InsHistProphyCodes,textInsHistProphy.Text);
			_changed|=Preferences.Set(PreferenceName.TreatPlanPromptSave, checkPromptSaveTP.Checked);
			return true;
		}
		#endregion Methods - Treat' Plan

		#region Methods - Chart
		private void FillChart(){
			comboToothNomenclature.Items.Add("Universal (Common in the US, 1-32)");
			comboToothNomenclature.Items.Add("FDI Notation (International, 11-48)");
			comboToothNomenclature.Items.Add("Haderup (Danish)");
			comboToothNomenclature.Items.Add("Palmer (Ortho)");
			comboToothNomenclature.SelectedIndex = PrefC.GetInt(PreferenceName.UseInternationalToothNumbers);
			if(Clinics.IsMedicalClinic(Clinics.ClinicId)) {
				labelToothNomenclature.Visible=false;
				comboToothNomenclature.Visible=false;
			}
			checkAutoClearEntryStatus.Checked=Preferences.GetBool(PreferenceName.AutoResetTPEntryStatus);
			checkAllowSettingProcsComplete.Checked=Preferences.GetBool(PreferenceName.AllowSettingProcsComplete);
			//checkChartQuickAddHideAmalgam.Checked=Prefs.GetBool(PrefName.ChartQuickAddHideAmalgam); //Deprecated.
			//checkToothChartMoveMenuToRight.Checked=Prefs.GetBool(PrefName.ToothChartMoveMenuToRight);
			textProblemsIndicateNone.Text		=ProblemDefinitions.GetName(Preferences.GetLong(PreferenceName.ProblemsIndicateNone)); //DB maint to fix corruption
			_diseaseDefNum=Preferences.GetLong(PreferenceName.ProblemsIndicateNone);
			textMedicationsIndicateNone.Text=Medications.GetDescription(Preferences.GetLong(PreferenceName.MedicationsIndicateNone)); //DB maint to fix corruption
			_medicationNum=Preferences.GetLong(PreferenceName.MedicationsIndicateNone);
			textAllergiesIndicateNone.Text	=AllergyDefs.GetDescription(Preferences.GetLong(PreferenceName.AllergiesIndicateNone)); //DB maint to fix corruption
			_alergyDefNum=Preferences.GetLong(PreferenceName.AllergiesIndicateNone);
			checkProcGroupNoteDoesAggregate.Checked=Preferences.GetBool(PreferenceName.ProcGroupNoteDoesAggregate);
			checkChartNonPatientWarn.Checked=Preferences.GetBool(PreferenceName.ChartNonPatientWarn);
			//checkChartAddProcNoRefreshGrid.Checked=Prefs.GetBool(PrefName.ChartAddProcNoRefreshGrid);//Not implemented.  May revisit some day.
			checkMedicalFeeUsedForNewProcs.Checked=Preferences.GetBool(PreferenceName.MedicalFeeUsedForNewProcs);
			checkProvColorChart.Checked=Preferences.GetBool(PreferenceName.UseProviderColorsInChart);
			checkPerioSkipMissingTeeth.Checked=Preferences.GetBool(PreferenceName.PerioSkipMissingTeeth);
			checkPerioTreatImplantsAsNotMissing.Checked=Preferences.GetBool(PreferenceName.PerioTreatImplantsAsNotMissing);
			if(Preferences.GetByte(PreferenceName.DxIcdVersion)==9) {
				checkDxIcdVersion.Checked=false;
			}
			else {//ICD-10
				checkDxIcdVersion.Checked=true;
			}
			SetIcdLabels();
			textICD9DefaultForNewProcs.Text=Preferences.GetString(PreferenceName.ICD9DefaultForNewProcs);
			checkProcLockingIsAllowed.Checked=Preferences.GetBool(PreferenceName.ProcLockingIsAllowed);
			textMedDefaultStopDays.Text=Preferences.GetString(PreferenceName.MedDefaultStopDays);
			checkScreeningsUseSheets.Checked=Preferences.GetBool(PreferenceName.ScreeningsUseSheets);
			checkProcsPromptForAutoNote.Checked=Preferences.GetBool(PreferenceName.ProcPromptForAutoNote);
			for(int i=0;i<Enum.GetNames(typeof(ProcCodeListSort)).Length;i++) {
				comboProcCodeListSort.Items.Add(Enum.GetNames(typeof(ProcCodeListSort))[i]);
			}
			comboProcCodeListSort.SelectedIndex=PrefC.GetInt(PreferenceName.ProcCodeListSortOrder);
			checkProcEditRequireAutoCode.Checked=Preferences.GetBool(PreferenceName.ProcEditRequireAutoCodes);
			checkClaimProcsAllowEstimatesOnCompl.Checked=Preferences.GetBool(PreferenceName.ClaimProcsAllowedToBackdate);
			checkSignatureAllowDigital.Checked=Preferences.GetBool(PreferenceName.SignatureAllowDigital);
			checkCommLogAutoSave.Checked=Preferences.GetBool(PreferenceName.CommLogAutoSave);
			comboProcFeeUpdatePrompt.Items.Add("No prompt, don't change fee");
			comboProcFeeUpdatePrompt.Items.Add("No prompt, always change fee");
			comboProcFeeUpdatePrompt.Items.Add("Prompt, when patient portion changes");
			comboProcFeeUpdatePrompt.Items.Add("Prompt, always");
			comboProcFeeUpdatePrompt.SelectedIndex=PrefC.GetInt(PreferenceName.ProcFeeUpdatePrompt);
			checkProcProvChangesCp.Checked=Preferences.GetBool(PreferenceName.ProcProvChangesClaimProcWithClaim);
			checkBoxRxClinicUseSelected.Checked=Preferences.GetBool(PreferenceName.ElectronicRxClinicUseSelected);
			if(!PrefC.HasClinicsEnabled) {
				checkBoxRxClinicUseSelected.Visible=false;
			}
			checkProcNoteConcurrencyMerge.Checked=Preferences.GetBool(PreferenceName.ProcNoteConcurrencyMerge);
			checkIsAlertRadiologyProcsEnabled.Checked=Preferences.GetBool(PreferenceName.IsAlertRadiologyProcsEnabled);
			checkShowPlannedApptPrompt.Checked=Preferences.GetBool(PreferenceName.ShowPlannedAppointmentPrompt);
			checkNotesProviderSigOnly.Checked=Preferences.GetBool(PreferenceName.NotesProviderSignatureOnly);
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveChart(){
			int daysStop=0;
			if(!int.TryParse(textMedDefaultStopDays.Text,out daysStop)) {
				MessageBox.Show("Days until medication order stop date entered was is invalid. Please enter a valid number to continue.");
				return false;
			}
			if(daysStop<0) {
				MessageBox.Show("Days until medication order stop date cannot be a negative number.");
				return false;
			}
			_changed|=Preferences.Set(PreferenceName.AllowSettingProcsComplete,checkAllowSettingProcsComplete.Checked);
			_changed|=Preferences.Set(PreferenceName.AutoResetTPEntryStatus,checkAutoClearEntryStatus.Checked);
			_changed|=Preferences.Set(PreferenceName.ProblemsIndicateNone,_diseaseDefNum);
			_changed|=Preferences.Set(PreferenceName.MedicationsIndicateNone,_medicationNum);
			_changed|=Preferences.Set(PreferenceName.AllergiesIndicateNone,_alergyDefNum);
			_changed|=Preferences.Set(PreferenceName.UseInternationalToothNumbers,comboToothNomenclature.SelectedIndex);
			_changed|=Preferences.Set(PreferenceName.ProcGroupNoteDoesAggregate,checkProcGroupNoteDoesAggregate.Checked);
			_changed|=Preferences.Set(PreferenceName.MedicalFeeUsedForNewProcs,checkMedicalFeeUsedForNewProcs.Checked);
			_changed|=Preferences.Set(PreferenceName.DxIcdVersion,(byte)(checkDxIcdVersion.Checked?10:9));
			_changed|=Preferences.Set(PreferenceName.ICD9DefaultForNewProcs,textICD9DefaultForNewProcs.Text);
			_changed|=Preferences.Set(PreferenceName.ProcLockingIsAllowed,checkProcLockingIsAllowed.Checked);
			_changed|=Preferences.Set(PreferenceName.ChartNonPatientWarn,checkChartNonPatientWarn.Checked);
			_changed|=Preferences.Set(PreferenceName.MedDefaultStopDays,daysStop);
			_changed|=Preferences.Set(PreferenceName.UseProviderColorsInChart,checkProvColorChart.Checked);
			_changed|=Preferences.Set(PreferenceName.PerioSkipMissingTeeth,checkPerioSkipMissingTeeth.Checked);
			_changed|=Preferences.Set(PreferenceName.PerioTreatImplantsAsNotMissing,checkPerioTreatImplantsAsNotMissing.Checked);
			_changed|=Preferences.Set(PreferenceName.ScreeningsUseSheets,checkScreeningsUseSheets.Checked);
			_changed|=Preferences.Set(PreferenceName.ProcCodeListSortOrder,comboProcCodeListSort.SelectedIndex);
			_changed|=Preferences.Set(PreferenceName.ProcEditRequireAutoCodes,checkProcEditRequireAutoCode.Checked);
			_changed|=Preferences.Set(PreferenceName.ClaimProcsAllowedToBackdate,checkClaimProcsAllowEstimatesOnCompl.Checked);
			_changed|=Preferences.Set(PreferenceName.SignatureAllowDigital,checkSignatureAllowDigital.Checked);
			_changed|=Preferences.Set(PreferenceName.CommLogAutoSave,checkCommLogAutoSave.Checked);
			_changed|=Preferences.Set(PreferenceName.ProcFeeUpdatePrompt,comboProcFeeUpdatePrompt.SelectedIndex);
			//_changed|=Prefs.UpdateBool(PrefName.ToothChartMoveMenuToRight,checkToothChartMoveMenuToRight.Checked);
			//_changed|=Prefs.UpdateBool(PrefName.ChartQuickAddHideAmalgam, checkChartQuickAddHideAmalgam.Checked); //Deprecated.
			//_changed|=Prefs.UpdateBool(PrefName.ChartAddProcNoRefreshGrid,checkChartAddProcNoRefreshGrid.Checked);//Not implemented.  May revisit someday.
			_changed|=Preferences.Set(PreferenceName.ProcProvChangesClaimProcWithClaim,checkProcProvChangesCp.Checked);
			_changed|=Preferences.Set(PreferenceName.ElectronicRxClinicUseSelected,checkBoxRxClinicUseSelected.Checked);
			_changed|=Preferences.Set(PreferenceName.ProcNoteConcurrencyMerge,checkProcNoteConcurrencyMerge.Checked);
			_changed|=Preferences.Set(PreferenceName.IsAlertRadiologyProcsEnabled,checkIsAlertRadiologyProcsEnabled.Checked);
			_changed|=Preferences.Set(PreferenceName.ShowPlannedAppointmentPrompt,checkShowPlannedApptPrompt.Checked);
			_changed|=Preferences.Set(PreferenceName.NotesProviderSignatureOnly,checkNotesProviderSigOnly.Checked);
			return true;
		}

		private void SetIcdLabels() {
			byte icdVersion=9;
			if(checkDxIcdVersion.Checked) {
				icdVersion=10;
			}
			labelIcdCodeDefault.Text="Default ICD"+"-"+icdVersion+" "+"code for new procedures and when set complete";
		}
		#endregion Methods - Chart

		#region Methods - Images
		private void FillImages(){
			switch(PrefC.GetInt(PreferenceName.ImagesModuleTreeIsCollapsed)) {
				case 0:
					radioImagesModuleTreeIsExpanded.Checked=true;
					break;
				case 1:
					radioImagesModuleTreeIsCollapsed.Checked=true;
					break;
				case 2:
					radioImagesModuleTreeIsPersistentPerUser.Checked=true;
					break;
			}
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveImages(){
			int imageModuleIsCollapsedVal=0;
			if(radioImagesModuleTreeIsExpanded.Checked) {
				imageModuleIsCollapsedVal=0;
			}
			else if(radioImagesModuleTreeIsCollapsed.Checked) {
				imageModuleIsCollapsedVal=1;
			}
			else if(radioImagesModuleTreeIsPersistentPerUser.Checked) {
				imageModuleIsCollapsedVal=2;
			}
			_changed|= Preferences.Set(PreferenceName.ImagesModuleTreeIsCollapsed,imageModuleIsCollapsedVal);
			return true;
		}
		#endregion Methods - Images

		#region Methods - Manage
		private void FillManage(){
			checkRxSendNewToQueue.Checked=Preferences.GetBool(PreferenceName.RxSendNewToQueue);
			int claimZeroPayRollingDays=PrefC.GetInt(PreferenceName.ClaimPaymentNoShowZeroDate);
			if(claimZeroPayRollingDays>=0) {
				textClaimsReceivedDays.Text=(claimZeroPayRollingDays+1).ToString();//The minimum value is now 1 ("today"), to match other areas of OD.
			}
			for(int i=0;i<7;i++) {
				comboTimeCardOvertimeFirstDayOfWeek.Items.Add(Enum.GetNames(typeof(DayOfWeek))[i]);
			}
			comboTimeCardOvertimeFirstDayOfWeek.SelectedIndex=PrefC.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek);
			checkTimeCardADP.Checked=Preferences.GetBool(PreferenceName.TimeCardADPExportIncludesName);
			checkClaimsSendWindowValidateOnLoad.Checked=Preferences.GetBool(PreferenceName.ClaimsSendWindowValidatesOnLoad);
			checkScheduleProvEmpSelectAll.Checked=Preferences.GetBool(PreferenceName.ScheduleProvEmpSelectAll);
			checkClockEventAllowBreak.Checked=Preferences.GetBool(PreferenceName.ClockEventAllowBreak);
			checkEraAllowTotalPayment.Checked=Preferences.GetBool(PreferenceName.EraAllowTotalPayments);
			//Statements
			checkStatementShowReturnAddress.Checked=Preferences.GetBool(PreferenceName.StatementShowReturnAddress);
			checkStatementShowNotes.Checked=Preferences.GetBool(PreferenceName.StatementShowNotes);
			checkStatementShowAdjNotes.Checked=Preferences.GetBool(PreferenceName.StatementShowAdjNotes);
			checkStatementShowProcBreakdown.Checked=Preferences.GetBool(PreferenceName.StatementShowProcBreakdown);
			comboUseChartNum.Items.Add("PatNum");
			comboUseChartNum.Items.Add("ChartNumber");
			if(Preferences.GetBool(PreferenceName.StatementAccountsUseChartNumber)) {
				comboUseChartNum.SelectedIndex=1;
			}
			else {
				comboUseChartNum.SelectedIndex=0;
			}
			checkStatementsAlphabetically.Checked=Preferences.GetBool(PreferenceName.PrintStatementsAlphabetically);
			checkStatementsAlphabetically.Visible=PrefC.HasClinicsEnabled;
			if(Preferences.GetLong(PreferenceName.StatementsCalcDueDate)!=-1) {
				textStatementsCalcDueDate.Text=Preferences.GetLong(PreferenceName.StatementsCalcDueDate).ToString();
			}
			textPayPlansBillInAdvanceDays.Text=Preferences.GetLong(PreferenceName.PayPlansBillInAdvanceDays).ToString();
			textBillingElectBatchMax.Text=PrefC.GetInt(PreferenceName.BillingElectBatchMax).ToString();
			checkIntermingleDefault.Checked=Preferences.GetBool(PreferenceName.IntermingleFamilyDefault);
			checkBillingShowProgress.Checked=Preferences.GetBool(PreferenceName.BillingShowSendProgress);
			checkClaimPaymentBatchOnly.Checked=Preferences.GetBool(PreferenceName.ClaimPaymentBatchOnly);
			checkEraOneClaimPerPage.Checked=Preferences.GetBool(PreferenceName.EraPrintOneClaimPerPage);
			checkIncludeEraWOPercCoPay.Checked=Preferences.GetBool(PreferenceName.EraIncludeWOPercCoPay);
			checkShowAutoDeposit.Checked=Preferences.GetBool(PreferenceName.ShowAutoDeposit);
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveManage(){
			if(textStatementsCalcDueDate.errorProvider1.GetError(textStatementsCalcDueDate)!=""
				| textPayPlansBillInAdvanceDays.errorProvider1.GetError(textPayPlansBillInAdvanceDays)!=""
				| textBillingElectBatchMax.errorProvider1.GetError(textBillingElectBatchMax)!="")
			{
				MessageBox.Show("Please fix data entry errors first.");
				return false;
			}
			if(textClaimsReceivedDays.errorProvider1.GetError(textClaimsReceivedDays)!="") {
				MessageBox.Show("Show claims received after days must be a positive integer or blank.");
				return false;
			}
			_changed|=Preferences.Set(PreferenceName.RxSendNewToQueue,checkRxSendNewToQueue.Checked);
			_changed|=Preferences.Set(PreferenceName.TimeCardOvertimeFirstDayOfWeek,comboTimeCardOvertimeFirstDayOfWeek.SelectedIndex);
			_changed|=Preferences.Set(PreferenceName.TimeCardADPExportIncludesName,checkTimeCardADP.Checked);
			_changed|=Preferences.Set(PreferenceName.ClaimsSendWindowValidatesOnLoad,checkClaimsSendWindowValidateOnLoad.Checked);
			_changed|=Preferences.Set(PreferenceName.StatementShowReturnAddress,checkStatementShowReturnAddress.Checked);
			_changed|=Preferences.Set(PreferenceName.ScheduleProvEmpSelectAll,checkScheduleProvEmpSelectAll.Checked);
			_changed|=Preferences.Set(PreferenceName.ClockEventAllowBreak,checkClockEventAllowBreak.Checked);
			_changed|=Preferences.Set(PreferenceName.EraAllowTotalPayments,checkEraAllowTotalPayment.Checked);
			_changed|=Preferences.Set(PreferenceName.StatementShowNotes,checkStatementShowNotes.Checked);
			_changed|=Preferences.Set(PreferenceName.StatementShowAdjNotes,checkStatementShowAdjNotes.Checked);
			_changed|=Preferences.Set(PreferenceName.StatementShowProcBreakdown,checkStatementShowProcBreakdown.Checked);
			_changed|=Preferences.Set(PreferenceName.StatementAccountsUseChartNumber,comboUseChartNum.SelectedIndex==1);
			_changed|=Preferences.Set(PreferenceName.PayPlansBillInAdvanceDays,PIn.Long(textPayPlansBillInAdvanceDays.Text));
			_changed|=Preferences.Set(PreferenceName.IntermingleFamilyDefault,checkIntermingleDefault.Checked);
			_changed|=Preferences.Set(PreferenceName.BillingElectBatchMax,PIn.Int(textBillingElectBatchMax.Text));
			_changed|=Preferences.Set(PreferenceName.BillingShowSendProgress,checkBillingShowProgress.Checked);
			_changed|=Preferences.Set(PreferenceName.ClaimPaymentNoShowZeroDate,(textClaimsReceivedDays.Text=="")?-1:(PIn.Int(textClaimsReceivedDays.Text)-1));
			_changed|=Preferences.Set(PreferenceName.ClaimPaymentBatchOnly,checkClaimPaymentBatchOnly.Checked);
			_changed|=Preferences.Set(PreferenceName.EraPrintOneClaimPerPage,checkEraOneClaimPerPage.Checked);
			_changed|=Preferences.Set(PreferenceName.EraIncludeWOPercCoPay,checkIncludeEraWOPercCoPay.Checked);
			_changed|=Preferences.Set(PreferenceName.ShowAutoDeposit,checkShowAutoDeposit.Checked);
			_changed|=Preferences.Set(PreferenceName.PrintStatementsAlphabetically,checkStatementsAlphabetically.Checked);
			if(textStatementsCalcDueDate.Text==""){
				if(Preferences.Set(PreferenceName.StatementsCalcDueDate,-1)){
					_changed=true;
				}
			}
			else{
				if(Preferences.Set(PreferenceName.StatementsCalcDueDate,PIn.Long(textStatementsCalcDueDate.Text))){
					_changed=true;
				}
			}
			return true;
		}
		#endregion Methods - Manage

		#region Methods - Other

		public override string HelpSubject => tabControlMain.SelectedTab.Name;

		#endregion
	}
}






