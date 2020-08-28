using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	public partial class FormModuleSetup:ODForm {
		#region Fields - Private
		private List<Def> _listDefsPosAdjTypes;
		private List<Def> _listDefsNegAdjTypes;
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
		private void butBadDebt_Click(object sender,EventArgs e) {
			string[] arrayDefNums=Prefs.GetString(PrefName.BadDebtAdjustmentTypes).Split(new char[] {','}); //comma-delimited list.
			List<long> listBadAdjDefNums = new List<long>();
			foreach(string strDefNum in arrayDefNums) {
				listBadAdjDefNums.Add(PIn.Long(strDefNum));
			}
			List<Def> listBadAdjDefs=Defs.GetDefs(DefCat.AdjTypes,listBadAdjDefNums);
			FormDefinitionPicker FormDP = new FormDefinitionPicker(DefCat.AdjTypes,listBadAdjDefs);
			FormDP.HasShowHiddenOption=true;
			FormDP.IsMultiSelectionMode=true;
			FormDP.ShowDialog();
			if(FormDP.DialogResult==DialogResult.OK) {
				FillListboxBadDebt(FormDP.ListSelectedDefs);
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
				checkIsRefundable.Checked=Prefs.GetBool(PrefName.TpPrePayIsNonRefundable);
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
				checkHideDueNow.Checked=Prefs.GetBool(PrefName.PayPlanHideDueNow);
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
			if(Prefs.GetBool(PrefName.TreatPlanSortByTooth)==radioTreatPlanSortOrder.Checked) {
				MessageBox.Show("You will need to restart the program for the change to take effect.");
			}
		}

		private void radioTreatPlanSortTooth_Click(object sender,EventArgs e) {
			if(Prefs.GetBool(PrefName.TreatPlanSortByTooth)!=radioTreatPlanSortTooth.Checked) {
				MessageBox.Show("You will need to restart the program for the change to take effect.");
			}
		}
		#endregion Methods - Event Handlers Treat' Plan

		#region Methods - Event Handlers Chart
		private void butAllergiesIndicateNone_Click(object sender,EventArgs e) {
			FormAllergySetup formA=new FormAllergySetup();
			formA.IsSelectionMode=true;
			formA.ShowDialog();
			if(formA.DialogResult!=DialogResult.OK) {
				return;
			}
			_alergyDefNum=formA.SelectedAllergyDefNum;
			textAllergiesIndicateNone.Text=AllergyDefs.GetOne(formA.SelectedAllergyDefNum).Description;
		}

		private void butDiagnosisCode_Click(object sender,EventArgs e) {
			if(checkDxIcdVersion.Checked) {//ICD-10
				FormIcd10s formI=new FormIcd10s();
				formI.IsSelectionMode=true;
				if(formI.ShowDialog()==DialogResult.OK) {
					textICD9DefaultForNewProcs.Text=formI.SelectedIcd10.Icd10Code;
				}
			}
			else {//ICD-9
				FormIcd9s formI=new FormIcd9s();
				formI.IsSelectionMode=true;
				if(formI.ShowDialog()==DialogResult.OK) {
					textICD9DefaultForNewProcs.Text=formI.SelectedIcd9.ICD9Code;
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
			FormDiseaseDefs formD=new FormDiseaseDefs();
			formD.IsSelectionMode=true;
			formD.ShowDialog();
			if(formD.DialogResult!=DialogResult.OK) {
				return;
			}
			//the list should only ever contain one item.
			_diseaseDefNum=formD.ListSelectedDiseaseDefs[0].DiseaseDefNum;
			textProblemsIndicateNone.Text=formD.ListSelectedDiseaseDefs[0].DiseaseName;
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
			BrokenApptProcedure brokenApptCodeDB=(BrokenApptProcedure)PrefC.GetInt(PrefName.BrokenApptProcedure);
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
			checkSolidBlockouts.Checked=Prefs.GetBool(PrefName.SolidBlockouts);
			checkBrokenApptAdjustment.Checked=Prefs.GetBool(PrefName.BrokenApptAdjustment);
			checkBrokenApptCommLog.Checked=Prefs.GetBool(PrefName.BrokenApptCommLog);
			//checkBrokenApptNote.Checked=Prefs.GetBool(PrefName.BrokenApptCommLogNotAdjustment);
			checkApptBubbleDelay.Checked = Prefs.GetBool(PrefName.ApptBubbleDelay);
			checkAppointmentBubblesDisabled.Checked=Prefs.GetBool(PrefName.AppointmentBubblesDisabled);
			_listDefsPosAdjTypes=Defs.GetPositiveAdjTypes();
			_listDefsNegAdjTypes=Defs.GetNegativeAdjTypes();
			comboBrokenApptAdjType.Items.AddDefs(_listDefsPosAdjTypes);
			comboBrokenApptAdjType.SetSelectedDefNum(Prefs.GetLong(PrefName.BrokenAppointmentAdjustmentType));
			comboProcDiscountType.Items.AddDefs(_listDefsNegAdjTypes);
			comboProcDiscountType.SetSelectedDefNum(Prefs.GetLong(PrefName.TreatPlanDiscountAdjustmentType));
			textDiscountPercentage.Text=Prefs.GetDouble(PrefName.TreatPlanDiscountPercent).ToString();
			checkApptExclamation.Checked=Prefs.GetBool(PrefName.ApptExclamationShowForUnsentIns);
			comboTimeArrived.Items.AddDefNone();
			comboTimeArrived.SelectedIndex=0;
			comboTimeArrived.Items.AddDefs(Defs.GetDefsForCategory(DefCat.ApptConfirmed,true));
			comboTimeArrived.SetSelectedDefNum(Prefs.GetLong(PrefName.AppointmentTimeArrivedTrigger));
			comboTimeSeated.Items.AddDefNone();
			comboTimeSeated.SelectedIndex=0;
			comboTimeSeated.Items.AddDefs(Defs.GetDefsForCategory(DefCat.ApptConfirmed,true));
			comboTimeSeated.SetSelectedDefNum(Prefs.GetLong(PrefName.AppointmentTimeSeatedTrigger));
			comboTimeDismissed.Items.AddDefNone();
			comboTimeDismissed.SelectedIndex=0;
			comboTimeDismissed.Items.AddDefs(Defs.GetDefsForCategory(DefCat.ApptConfirmed,true));
			comboTimeDismissed.SetSelectedDefNum(Prefs.GetLong(PrefName.AppointmentTimeDismissedTrigger));
			checkApptRefreshEveryMinute.Checked=Prefs.GetBool(PrefName.ApptModuleRefreshesEveryMinute);
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
				if(Prefs.GetDouble(PrefName.FormClickDelay)==seconds) {
					comboDelay.SelectedIndex = i;
				}
			}
			comboSearchBehavior.SelectedIndex=PrefC.GetInt(PrefName.AppointmentSearchBehavior);
			checkAppointmentTimeIsLocked.Checked=Prefs.GetBool(PrefName.AppointmentTimeIsLocked);
			textApptBubNoteLength.Text=PrefC.GetInt(PrefName.AppointmentBubblesNoteLength).ToString();
			checkWaitingRoomFilterByView.Checked=Prefs.GetBool(PrefName.WaitingRoomFilterByView);
			textWaitRoomWarn.Text=PrefC.GetInt(PrefName.WaitingRoomAlertTime).ToString();
			butColor.BackColor=PrefC.GetColor(PrefName.WaitingRoomAlertColor);
			butApptLineColor.BackColor=PrefC.GetColor(PrefName.AppointmentTimeLineColor);
			checkApptModuleDefaultToWeek.Checked=Prefs.GetBool(PrefName.ApptModuleDefaultToWeek);
			checkApptTimeReset.Checked=Prefs.GetBool(PrefName.AppointmentClinicTimeReset);
			if(!PrefC.HasClinicsEnabled) {
				checkApptTimeReset.Visible=false;
			}
			checkApptModuleAdjInProd.Checked=Prefs.GetBool(PrefName.ApptModuleAdjustmentsInProd);
			checkUseOpHygProv.Checked=Prefs.GetBool(PrefName.ApptSecondaryProviderConsiderOpOnly);
			checkApptModuleProductionUsesOps.Checked=Prefs.GetBool(PrefName.ApptModuleProductionUsesOps);
			checkApptsRequireProcs.Checked=Prefs.GetBool(PrefName.ApptsRequireProc);
			checkApptAllowFutureComplete.Checked=Prefs.GetBool(PrefName.ApptAllowFutureComplete);
			checkApptAllowEmptyComplete.Checked=Prefs.GetBool(PrefName.ApptAllowEmptyComplete);
			comboApptSchedEnforceSpecialty.Items.AddRange(Enum.GetValues(typeof(ApptSchedEnforceSpecialty)).OfType<ApptSchedEnforceSpecialty>()
				.Select(x => x.GetDescription()).ToArray());
			comboApptSchedEnforceSpecialty.SelectedIndex=PrefC.GetInt(PrefName.ApptSchedEnforceSpecialty);
			if(!PrefC.HasClinicsEnabled) {
				comboApptSchedEnforceSpecialty.Visible=false;
				labelApptSchedEnforceSpecialty.Visible=false;
			}
			textApptWithoutProcsDefaultLength.Text=Prefs.GetString(PrefName.AppointmentWithoutProcsDefaultLength);
			checkReplaceBlockouts.Checked=Prefs.GetBool(PrefName.ReplaceExistingBlockout);
			checkUnscheduledListNoRecalls.Checked=Prefs.GetBool(PrefName.UnscheduledListNoRecalls);
			textApptAutoRefreshRange.Text= Prefs.GetString(PrefName.ApptAutoRefreshRange);
			checkPreventChangesToComplAppts.Checked= Prefs.GetBool(PrefName.ApptPreventChangesToCompleted);
			checkApptsAllowOverlap.Checked=Prefs.GetBool(PrefName.ApptsAllowOverlap, true);
			textApptFontSize.Text= Prefs.GetString(PrefName.ApptFontSize);
			textApptProvbarWidth.Text= Prefs.GetString(PrefName.ApptProvbarWidth);
			checkBrokenApptRequiredOnMove.Checked= Prefs.GetBool(PrefName.BrokenApptRequiredOnMove);
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
			_changed|=Prefs.Set(PrefName.AppointmentBubblesDisabled,checkAppointmentBubblesDisabled.Checked);
			_changed|=Prefs.Set(PrefName.ApptBubbleDelay,checkApptBubbleDelay.Checked);
			_changed|=Prefs.Set(PrefName.SolidBlockouts,checkSolidBlockouts.Checked);
			//_changed|=Prefs.UpdateBool(PrefName.BrokenApptCommLogNotAdjustment,checkBrokenApptNote.Checked) //Deprecated
			_changed|=Prefs.Set(PrefName.BrokenApptAdjustment,checkBrokenApptAdjustment.Checked);
			_changed|=Prefs.Set(PrefName.BrokenApptCommLog,checkBrokenApptCommLog.Checked);
			_changed|=Prefs.Set(PrefName.BrokenApptProcedure,(int)_listBrokenApptProcedures[comboBrokenApptProc.SelectedIndex]);
			_changed|=Prefs.Set(PrefName.ApptExclamationShowForUnsentIns,checkApptExclamation.Checked);
			_changed|=Prefs.Set(PrefName.ApptModuleRefreshesEveryMinute,checkApptRefreshEveryMinute.Checked);
			_changed|=Prefs.Set(PrefName.AppointmentSearchBehavior,comboSearchBehavior.SelectedIndex);
			_changed|=Prefs.Set(PrefName.AppointmentTimeIsLocked,checkAppointmentTimeIsLocked.Checked);
			_changed|=Prefs.Set(PrefName.AppointmentBubblesNoteLength,noteLength);
			_changed|=Prefs.Set(PrefName.WaitingRoomFilterByView,checkWaitingRoomFilterByView.Checked);
			_changed|=Prefs.Set(PrefName.WaitingRoomAlertTime,waitingRoomAlertTime);
			_changed|=Prefs.Set(PrefName.WaitingRoomAlertColor,butColor.BackColor.ToArgb());
			_changed|=Prefs.Set(PrefName.AppointmentTimeLineColor,butApptLineColor.BackColor.ToArgb());
			_changed|=Prefs.Set(PrefName.ApptModuleDefaultToWeek,checkApptModuleDefaultToWeek.Checked);
			_changed|=Prefs.Set(PrefName.AppointmentClinicTimeReset,checkApptTimeReset.Checked);
			_changed|=Prefs.Set(PrefName.ApptModuleAdjustmentsInProd,checkApptModuleAdjInProd.Checked);
			_changed|=Prefs.Set(PrefName.ApptSecondaryProviderConsiderOpOnly,checkUseOpHygProv.Checked);
			_changed|=Prefs.Set(PrefName.ApptModuleProductionUsesOps,checkApptModuleProductionUsesOps.Checked);
			_changed|=Prefs.Set(PrefName.ApptsRequireProc,checkApptsRequireProcs.Checked);
			_changed|=Prefs.Set(PrefName.ApptAllowFutureComplete,checkApptAllowFutureComplete.Checked);
			_changed|=Prefs.Set(PrefName.ApptAllowEmptyComplete,checkApptAllowEmptyComplete.Checked);
			_changed|=Prefs.Set(PrefName.ApptSchedEnforceSpecialty,comboApptSchedEnforceSpecialty.SelectedIndex);
			_changed|=Prefs.Set(PrefName.FormClickDelay,((ODBoxItem<double>)comboDelay.Items[comboDelay.SelectedIndex]).Tag);
			_changed|=Prefs.Set(PrefName.AppointmentWithoutProcsDefaultLength,textApptWithoutProcsDefaultLength.Text);
			_changed|=Prefs.Set(PrefName.ReplaceExistingBlockout,checkReplaceBlockouts.Checked);
			_changed|=Prefs.Set(PrefName.UnscheduledListNoRecalls,checkUnscheduledListNoRecalls.Checked);
			_changed|=Prefs.Set(PrefName.ApptAutoRefreshRange,textApptAutoRefreshRange.Text);
			_changed|=Prefs.Set(PrefName.ApptPreventChangesToCompleted,checkPreventChangesToComplAppts.Checked);
			_changed|=Prefs.Set(PrefName.ApptsAllowOverlap, checkApptsAllowOverlap.Checked);
			_changed|=Prefs.Set(nameof(PrefName.ApptFontSize),apptFontSize.ToString());
			_changed|=Prefs.Set(nameof(PrefName.ApptProvbarWidth),PIn.Int(textApptProvbarWidth.Text));
			_changed|=Prefs.Set(PrefName.BrokenApptRequiredOnMove,checkBrokenApptRequiredOnMove.Checked);
			if(comboBrokenApptAdjType.SelectedIndex!=-1) {
				_changed|=Prefs.Set(PrefName.BrokenAppointmentAdjustmentType,comboBrokenApptAdjType.GetSelectedDefNum());
			}
			if(comboProcDiscountType.SelectedIndex!=-1) {
				_changed|=Prefs.Set(PrefName.TreatPlanDiscountAdjustmentType,comboProcDiscountType.GetSelectedDefNum());
			}
			long timeArrivedTrigger=0;
			if(comboTimeArrived.SelectedIndex>0){
				timeArrivedTrigger=comboTimeArrived.GetSelectedDefNum();
			}
			List<string> listTriggerNewNums=new List<string>();
			if(Prefs.Set(PrefName.AppointmentTimeArrivedTrigger,timeArrivedTrigger)){
				listTriggerNewNums.Add(POut.Long(timeArrivedTrigger));
				_changed=true;
			}
			long timeSeatedTrigger=0;
			if(comboTimeSeated.SelectedIndex>0){
				timeSeatedTrigger=comboTimeSeated.GetSelectedDefNum();
			}
			if(Prefs.Set(PrefName.AppointmentTimeSeatedTrigger,timeSeatedTrigger)){
				listTriggerNewNums.Add(POut.Long(timeSeatedTrigger));
				_changed=true;
			}
			long timeDismissedTrigger=0;
			if(comboTimeDismissed.SelectedIndex>0){
				timeDismissedTrigger=comboTimeDismissed.GetSelectedDefNum();
			}
			if(Prefs.Set(PrefName.AppointmentTimeDismissedTrigger,timeDismissedTrigger)){
				listTriggerNewNums.Add(POut.Long(timeDismissedTrigger));
				_changed=true;
			}
			if(listTriggerNewNums.Count>0) {
				//Adds the appointment triggers to the list of confirmation statuses excluded from sending eConfirms,eReminders, and eThankYous.
				List<string> listEConfirm= Prefs.GetString(PrefName.ApptConfirmExcludeEConfirm).Split(',')
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Union(listTriggerNewNums).ToList();
				List<string> listESend= Prefs.GetString(PrefName.ApptConfirmExcludeESend).Split(',')
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Union(listTriggerNewNums).ToList();
				List<string> listERemind= Prefs.GetString(PrefName.ApptConfirmExcludeERemind).Split(',')
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Union(listTriggerNewNums).ToList();
				List<string> listEThanks=Prefs.GetString(PrefName.ApptConfirmExcludeEThankYou).Split(',')
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Union(listTriggerNewNums).ToList();
				//Update new Value strings in database.  We don't remove the old ones.
				Prefs.Set(PrefName.ApptConfirmExcludeEConfirm,string.Join(",",listEConfirm));
				Prefs.Set(PrefName.ApptConfirmExcludeESend,string.Join(",",listESend));
				Prefs.Set(PrefName.ApptConfirmExcludeERemind,string.Join(",",listERemind));
				Prefs.Set(PrefName.ApptConfirmExcludeEThankYou,string.Join(",",listEThanks));
			}
			return true;
		}
		#endregion Methods - Appts

		#region Methods - Family
		private void FillFamily(){
			checkInsurancePlansShared.Checked=Prefs.GetBool(PrefName.InsurancePlansShared);
			checkPPOpercentage.Checked=Prefs.GetBool(PrefName.InsDefaultPPOpercent);
			checkAllowedFeeSchedsAutomate.Checked=Prefs.GetBool(PrefName.AllowedFeeSchedsAutomate);
			checkCoPayFeeScheduleBlankLikeZero.Checked=Prefs.GetBool(PrefName.CoPay_FeeSchedule_BlankLikeZero);
			checkFixedBenefitBlankLikeZero.Checked=Prefs.GetBool(PrefName.FixedBenefitBlankLikeZero);
			checkInsDefaultShowUCRonClaims.Checked=Prefs.GetBool(PrefName.InsDefaultShowUCRonClaims);
			checkInsDefaultAssignmentOfBenefits.Checked=Prefs.GetBool(PrefName.InsDefaultAssignBen);
			checkInsPPOsecWriteoffs.Checked=Prefs.GetBool(PrefName.InsPPOsecWriteoffs);
			for(int i=0;i<Enum.GetNames(typeof(EnumCobRule)).Length;i++) {
				comboCobRule.Items.Add(Enum.GetNames(typeof(EnumCobRule))[i]);
			}
			comboCobRule.SelectedIndex=PrefC.GetInt(PrefName.InsDefaultCobRule);
			checkTextMsgOkStatusTreatAsNo.Checked=Prefs.GetBool(PrefName.TextMsgOkStatusTreatAsNo);
			checkFamPhiAccess.Checked=Prefs.GetBool(PrefName.FamPhiAccess);
			checkGoogleAddress.Checked=Prefs.GetBool(PrefName.ShowFeatureGoogleMaps);
			checkSelectProv.Checked=Prefs.GetBool(PrefName.PriProvDefaultToSelectProv);
			if(!Prefs.GetBool(PrefName.ShowFeatureSuperfamilies)) {
				groupBoxSuperFamily.Visible=false;
			}
			else {
				foreach(SortStrategy option in Enum.GetValues(typeof(SortStrategy))) {
					comboSuperFamSort.Items.Add(option.GetDescription());
				}
				comboSuperFamSort.SelectedIndex=PrefC.GetInt(PrefName.SuperFamSortStrategy);
				checkSuperFamSync.Checked=Prefs.GetBool(PrefName.PatientAllSuperFamilySync);
				checkSuperFamAddIns.Checked=Prefs.GetBool(PrefName.SuperFamNewPatAddIns);
				checkSuperFamCloneCreate.Checked=Prefs.GetBool(PrefName.CloneCreateSuperFamily);
			}
			//users should only see the claimsnapshot tab page if they have it set to something other than ClaimCreate.
			//if a user wants to be able to change claimsnapshot settings, the following MySQL statement should be run:
			//UPDATE preference SET ValueString = 'Service'	 WHERE PrefName = 'ClaimSnapshotTriggerType'
			if(PIn.Enum<ClaimSnapshotTrigger>(Prefs.GetString(PrefName.ClaimSnapshotTriggerType),true) == ClaimSnapshotTrigger.ClaimCreate) {
				groupBoxClaimSnapshot.Visible=false;
			}
			foreach(ClaimSnapshotTrigger trigger in Enum.GetValues(typeof(ClaimSnapshotTrigger))) {
				comboClaimSnapshotTrigger.Items.Add(trigger.GetDescription());
			}
			comboClaimSnapshotTrigger.SelectedIndex=(int)PIn.Enum<ClaimSnapshotTrigger>(Prefs.GetString(PrefName.ClaimSnapshotTriggerType),true);
			textClaimSnapshotRunTime.Text=PrefC.GetDate(PrefName.ClaimSnapshotRunTime).ToShortTimeString();
			checkClaimUseOverrideProcDescript.Checked=Prefs.GetBool(PrefName.ClaimPrintProcChartedDesc);
			checkClaimTrackingRequireError.Checked=Prefs.GetBool(PrefName.ClaimTrackingRequiresError);
			checkPatInitBillingTypeFromPriInsPlan.Checked=Prefs.GetBool(PrefName.PatInitBillingTypeFromPriInsPlan);
			checkPreferredReferrals.Checked=Prefs.GetBool(PrefName.ShowPreferedReferrals);
			checkAutoFillPatEmail.Checked=Prefs.GetBool(PrefName.AddFamilyInheritsEmail);
			if(!PrefC.HasClinicsEnabled) {
				checkAllowPatsAtHQ.Visible=false;
			}
			checkAllowPatsAtHQ.Checked=Prefs.GetBool(PrefName.ClinicAllowPatientsAtHeadquarters);
			checkInsPlanExclusionsUseUCR.Checked=Prefs.GetBool(PrefName.InsPlanUseUcrFeeForExclusions);
			checkInsPlanExclusionsMarkDoNotBill.Checked=Prefs.GetBool(PrefName.InsPlanExclusionsMarkDoNotBillIns);
			checkPatientSSNMasked.Checked=Prefs.GetBool(PrefName.PatientSSNMasked);
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				checkPatientSSNMasked.Text="Mask patient Social Insurance Numbers";
			}
			checkPatientDOBMasked.Checked=Prefs.GetBool(PrefName.PatientDOBMasked);
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveFamily(){
			DateTime claimSnapshotRunTime=DateTime.MinValue;
			if(!DateTime.TryParse(textClaimSnapshotRunTime.Text,out claimSnapshotRunTime)) {
				MessageBox.Show("Service Snapshot Run Time must be a valid time value.");
				return false;
			}
			claimSnapshotRunTime=new DateTime(1881,01,01,claimSnapshotRunTime.Hour,claimSnapshotRunTime.Minute,claimSnapshotRunTime.Second);
			_changed|=Prefs.Set(PrefName.InsurancePlansShared,checkInsurancePlansShared.Checked);
			_changed|=Prefs.Set(PrefName.InsDefaultPPOpercent,checkPPOpercentage.Checked);
			_changed|=Prefs.Set(PrefName.AllowedFeeSchedsAutomate,checkAllowedFeeSchedsAutomate.Checked);
			_changed|=Prefs.Set(PrefName.CoPay_FeeSchedule_BlankLikeZero,checkCoPayFeeScheduleBlankLikeZero.Checked);
			_changed|=Prefs.Set(PrefName.FixedBenefitBlankLikeZero,checkFixedBenefitBlankLikeZero.Checked);
			_changed|=Prefs.Set(PrefName.InsDefaultShowUCRonClaims,checkInsDefaultShowUCRonClaims.Checked);
			_changed|=Prefs.Set(PrefName.InsDefaultAssignBen,checkInsDefaultAssignmentOfBenefits.Checked);
			_changed|=Prefs.Set(PrefName.InsDefaultCobRule,comboCobRule.SelectedIndex);
			_changed|=Prefs.Set(PrefName.TextMsgOkStatusTreatAsNo,checkTextMsgOkStatusTreatAsNo.Checked);
			_changed|=Prefs.Set(PrefName.FamPhiAccess,checkFamPhiAccess.Checked);
			_changed|=Prefs.Set(PrefName.InsPPOsecWriteoffs,checkInsPPOsecWriteoffs.Checked);
			_changed|=Prefs.Set(PrefName.ShowFeatureGoogleMaps,checkGoogleAddress.Checked);
			_changed|=Prefs.Set(PrefName.PriProvDefaultToSelectProv,checkSelectProv.Checked);
			_changed|=Prefs.Set(PrefName.SuperFamSortStrategy,comboSuperFamSort.SelectedIndex);
			_changed|=Prefs.Set(PrefName.PatientAllSuperFamilySync,checkSuperFamSync.Checked);
			_changed|=Prefs.Set(PrefName.SuperFamNewPatAddIns,checkSuperFamAddIns.Checked);
			_changed|=Prefs.Set(PrefName.CloneCreateSuperFamily,checkSuperFamCloneCreate.Checked);
			_changed|=Prefs.Set(PrefName.ClaimSnapshotRunTime,claimSnapshotRunTime);
			_changed|=Prefs.Set(PrefName.ClaimPrintProcChartedDesc,checkClaimUseOverrideProcDescript.Checked);
			_changed|=Prefs.Set(PrefName.ClaimTrackingRequiresError,checkClaimTrackingRequireError.Checked);
			_changed|=Prefs.Set(PrefName.PatInitBillingTypeFromPriInsPlan,checkPatInitBillingTypeFromPriInsPlan.Checked);
			_changed|=Prefs.Set(PrefName.ShowPreferedReferrals,checkPreferredReferrals.Checked);
			_changed|=Prefs.Set(PrefName.AddFamilyInheritsEmail,checkAutoFillPatEmail.Checked);
			_changed|=Prefs.Set(PrefName.ClinicAllowPatientsAtHeadquarters,checkAllowPatsAtHQ.Checked);
			_changed|=Prefs.Set(PrefName.InsPlanExclusionsMarkDoNotBillIns,checkInsPlanExclusionsMarkDoNotBill.Checked);
			_changed|=Prefs.Set(PrefName.InsPlanUseUcrFeeForExclusions,checkInsPlanExclusionsUseUCR.Checked);
			_changed|=Prefs.Set(PrefName.PatientSSNMasked,checkPatientSSNMasked.Checked);
			_changed|=Prefs.Set(PrefName.PatientDOBMasked,checkPatientDOBMasked.Checked);
			foreach(ClaimSnapshotTrigger trigger in Enum.GetValues(typeof(ClaimSnapshotTrigger))) {
				if(trigger.GetDescription()==comboClaimSnapshotTrigger.Text) {
					if(Prefs.Set(PrefName.ClaimSnapshotTriggerType,trigger.ToString())) {
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
			checkStoreCCTokens.Checked=Prefs.GetBool(PrefName.StoreCCtokens);
			foreach(PayClinicSetting prompt in Enum.GetValues(typeof(PayClinicSetting))) {
				comboPaymentClinicSetting.Items.Add(prompt.GetDescription());
			}
			comboPaymentClinicSetting.SelectedIndex=PrefC.GetInt(PrefName.PaymentClinicSetting);
			checkPaymentsPromptForPayType.Checked=Prefs.GetBool(PrefName.PaymentsPromptForPayType);
			comboUnallocatedSplits.Items.AddDefs(Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType,true));
			comboUnallocatedSplits.SetSelectedDefNum(Prefs.GetLong(PrefName.PrepaymentUnearnedType));
			comboPayPlanAdj.Items.AddDefs(_listDefsNegAdjTypes);
			comboPayPlanAdj.SetSelectedDefNum(Prefs.GetLong(PrefName.PayPlanAdjType));
			comboFinanceChargeAdjType.Items.AddDefs(_listDefsPosAdjTypes);
			comboFinanceChargeAdjType.SetSelectedDefNum(Prefs.GetLong(PrefName.FinanceChargeAdjustmentType));
			comboBillingChargeAdjType.Items.AddDefs(_listDefsPosAdjTypes);
			comboBillingChargeAdjType.SetSelectedDefNum(Prefs.GetLong(PrefName.BillingChargeAdjustmentType));
			comboSalesTaxAdjType.Items.AddDefs(_listDefsPosAdjTypes);
			comboSalesTaxAdjType.SetSelectedDefNum(Prefs.GetLong(PrefName.SalesTaxAdjustmentType));
			textTaxPercent.Text=Prefs.GetDouble(PrefName.SalesTaxPercentage).ToString();
			string[] arrayDefNums=Prefs.GetString(PrefName.BadDebtAdjustmentTypes).Split(new char[] {','}); //comma-delimited list.
			List<long> listBadAdjDefNums = new List<long>();
			foreach(string strDefNum in arrayDefNums) {
				listBadAdjDefNums.Add(PIn.Long(strDefNum));
			}
			FillListboxBadDebt(Defs.GetDefs(DefCat.AdjTypes,listBadAdjDefNums));
			checkAllowFutureDebits.Checked=Prefs.GetBool(PrefName.AccountAllowFutureDebits);
			checkAllowEmailCCReceipt.Checked=Prefs.GetBool(PrefName.AllowEmailCCReceipt);
			List<RigorousAccounting> listEnums=Enum.GetValues(typeof(RigorousAccounting)).OfType<RigorousAccounting>().ToList();
			for(int i=0;i<listEnums.Count;i++) {
				comboRigorousAccounting.Items.Add(listEnums[i].GetDescription());
			}
			comboRigorousAccounting.SelectedIndex=PrefC.GetInt(PrefName.RigorousAccounting);
			List<RigorousAdjustments> listAdjEnums=Enum.GetValues(typeof(RigorousAdjustments)).OfType<RigorousAdjustments>().ToList();
			for(int i=0;i<listAdjEnums.Count;i++) {
				comboRigorousAdjustments.Items.Add(listAdjEnums[i].GetDescription());
			}
			comboRigorousAdjustments.SelectedIndex=PrefC.GetInt(PrefName.RigorousAdjustments);
			checkHidePaysplits.Checked=Prefs.GetBool(PrefName.PaymentWindowDefaultHideSplits);
			checkPaymentsTransferPatientIncomeOnly.Checked=Prefs.GetBool(PrefName.PaymentsTransferPatientIncomeOnly);
			checkAllowPrepayProvider.Checked=Prefs.GetBool(PrefName.AllowPrepayProvider);
			comboRecurringChargePayType.Items.AddDefNone("("+"default"+")");
			comboRecurringChargePayType.Items.AddDefs(Defs.GetDefsForCategory(DefCat.PaymentTypes,true));
			comboRecurringChargePayType.SetSelectedDefNum(Prefs.GetLong(PrefName.RecurringChargesPayTypeCC)); 
			_ynPrePayAllowedForTpProcs=Prefs.GetBool(PrefName.PrePayAllowedForTpProcs);
			checkAllowPrePayToTpProcs.Checked= Prefs.GetBool(PrefName.PrePayAllowedForTpProcs);
			checkIsRefundable.Checked=Prefs.GetBool(PrefName.TpPrePayIsNonRefundable);
			checkIsRefundable.Visible=checkAllowPrePayToTpProcs.Checked;//pref will be unchecked if parent gets turned off.
			comboTpUnearnedType.Items.AddDefs(Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType,true));
			comboTpUnearnedType.SetSelectedDefNum(Prefs.GetLong(PrefName.TpUnearnedType));
			#endregion Pay/Adj Tab
			#region Insurance Tab
			checkProviderIncomeShows.Checked=Prefs.GetBool(PrefName.ProviderIncomeTransferShows);
			checkClaimMedTypeIsInstWhenInsPlanIsMedical.Checked=Prefs.GetBool(PrefName.ClaimMedTypeIsInstWhenInsPlanIsMedical);
			checkClaimFormTreatDentSaysSigOnFile.Checked=Prefs.GetBool(PrefName.ClaimFormTreatDentSaysSigOnFile);
			textInsWriteoffDescript.Text=Prefs.GetString(PrefName.InsWriteoffDescript);
			textClaimAttachPath.Text=Prefs.GetString(PrefName.ClaimAttachExportPath);
			checkEclaimsMedicalProvTreatmentAsOrdering.Checked=Prefs.GetBool(PrefName.ClaimMedProvTreatmentAsOrdering);
			checkEclaimsSeparateTreatProv.Checked=Prefs.GetBool(PrefName.EclaimsSeparateTreatProv);
			checkClaimsValidateACN.Checked=Prefs.GetBool(PrefName.ClaimsValidateACN);
			checkAllowProcAdjFromClaim.Checked=Prefs.GetBool(PrefName.AllowProcAdjFromClaim);
			comboClaimCredit.Items.AddRange(Enum.GetNames(typeof(ClaimProcCreditsGreaterThanProcFee)));
			comboClaimCredit.SelectedIndex=PrefC.GetInt(PrefName.ClaimProcAllowCreditsGreaterThanProcFee);
			checkAllowFuturePayments.Checked=Prefs.GetBool(PrefName.AllowFutureInsPayments);
			textClaimIdentifier.Text=Prefs.GetString(PrefName.ClaimIdPrefix);
			foreach(ClaimZeroDollarProcBehavior procBehavior in Enum.GetValues(typeof(ClaimZeroDollarProcBehavior))) {
				comboZeroDollarProcClaimBehavior.Items.Add(procBehavior.ToString());
			}
			comboZeroDollarProcClaimBehavior.SelectedIndex=PrefC.GetInt(PrefName.ClaimZeroDollarProcBehavior);
			checkClaimTrackingExcludeNone.Checked=Prefs.GetBool(PrefName.ClaimTrackingStatusExcludesNone);
			checkInsPayNoWriteoffMoreThanProc.Checked=Prefs.GetBool(PrefName.InsPayNoWriteoffMoreThanProc);
			checkPromptForSecondaryClaim.Checked=Prefs.GetBool(PrefName.PromptForSecondaryClaim);
			checkInsEstRecalcReceived.Checked=Prefs.GetBool(PrefName.InsEstRecalcReceived);
			checkCanadianPpoLabEst.Checked=Prefs.GetBool(PrefName.CanadaCreatePpoLabEst);
			#endregion Insurance Tab
			#region Misc Account Tab
			checkBalancesDontSubtractIns.Checked=Prefs.GetBool(PrefName.BalancesDontSubtractIns);
			checkAgingMonthly.Checked=Prefs.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily);
			if(!Prefs.GetBool(PrefName.AgingIsEnterprise)) {//AgingIsEnterprise requires aging to be daily
				checkAgingMonthly.Text="Aging calculated monthly instead of daily";
				checkAgingMonthly.Enabled=true;
			}
			checkAccountShowPaymentNums.Checked=Prefs.GetBool(PrefName.AccountShowPaymentNums);
			checkShowFamilyCommByDefault.Checked=Prefs.GetBool(PrefName.ShowAccountFamilyCommEntries);
			checkRecurChargPriProv.Checked=Prefs.GetBool(PrefName.RecurringChargesUsePriProv);
			checkPpoUseUcr.Checked=Prefs.GetBool(PrefName.InsPpoAlwaysUseUcrFee);
			checkRecurringChargesUseTransDate.Checked=Prefs.GetBool(PrefName.RecurringChargesUseTransDate);
			checkStatementInvoiceGridShowWriteoffs.Checked=Prefs.GetBool(PrefName.InvoicePaymentsGridShowNetProd);
			checkShowAllocateUnearnedPaymentPrompt.Checked=Prefs.GetBool(PrefName.ShowAllocateUnearnedPaymentPrompt);
			checkPayPlansExcludePastActivity.Checked=Prefs.GetBool(PrefName.PayPlansExcludePastActivity);
			checkPayPlansUseSheets.Checked=Prefs.GetBool(PrefName.PayPlansUseSheets);
			checkAllowFutureTrans.Checked=Prefs.GetBool(PrefName.FutureTransDatesAllowed);
			checkAgingProcLifo.CheckState=PrefC.GetYNCheckState(PrefName.AgingProcLifo);
			foreach(PayPlanVersions version in Enum.GetValues(typeof(PayPlanVersions))) {
				comboPayPlansVersion.Items.Add(version.GetDescription());
			}
			comboPayPlansVersion.SelectedIndex=PrefC.GetInt(PrefName.PayPlansVersion) - 1;
			if(comboPayPlansVersion.SelectedIndex==(int)PayPlanVersions.AgeCreditsAndDebits-1) {//Minus 1 because the enum starts at 1.
				checkHideDueNow.Visible=true;
				checkHideDueNow.Checked=Prefs.GetBool(PrefName.PayPlanHideDueNow);
			}
			else {
				checkHideDueNow.Visible=false;
				checkHideDueNow.Checked=false;
			}
			checkRecurringChargesAutomated.Checked=Prefs.GetBool(PrefName.RecurringChargesAutomatedEnabled);
			textRecurringChargesTime.Text=PrefC.GetDate(PrefName.RecurringChargesAutomatedTime).TimeOfDay.ToShortTimeString();
			checkRecurPatBal0.Checked=Prefs.GetBool(PrefName.RecurringChargesAllowedWhenNoPatBal);
			checkRepeatingChargesRunAging.Checked=Prefs.GetBool(PrefName.RepeatingChargesRunAging);
			checkRepeatingChargesAutomated.Checked=Prefs.GetBool(PrefName.RepeatingChargesAutomated);
			textRepeatingChargesAutomatedTime.Text=PrefC.GetDate(PrefName.RepeatingChargesAutomatedTime).TimeOfDay.ToShortTimeString();
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {
				checkCanadianPpoLabEst.Visible=false;
			}
			textDynamicPayPlan.Text=PrefC.GetDate(PrefName.DynamicPayPlanRunTime).TimeOfDay.ToShortTimeString();
			#endregion Misc Account Tab
		}

		private void FillListboxBadDebt(List<Def> listSelectedDefs) {
			listboxBadDebtAdjs.Items.Clear();
			foreach(Def defCur in listSelectedDefs) {
				listboxBadDebtAdjs.Items.Add(new ODBoxItem<Def>(defCur.ItemName,defCur));
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
			string strListBadDebtAdjTypes=string.Join(",",listboxBadDebtAdjs.Items.Cast<ODBoxItem<Def>>().Select(x => x.Tag.DefNum));
			_changed|=Prefs.Set(PrefName.BalancesDontSubtractIns,checkBalancesDontSubtractIns.Checked);
			_changed|=Prefs.Set(PrefName.AgingCalculatedMonthlyInsteadOfDaily,checkAgingMonthly.Checked);
			_changed|=Prefs.Set(PrefName.StoreCCtokens,checkStoreCCTokens.Checked);
			_changed|=Prefs.Set(PrefName.ProviderIncomeTransferShows,checkProviderIncomeShows.Checked);
			_changed|=Prefs.Set(PrefName.ShowAccountFamilyCommEntries,checkShowFamilyCommByDefault.Checked);
			_changed|=Prefs.Set(PrefName.ClaimFormTreatDentSaysSigOnFile,checkClaimFormTreatDentSaysSigOnFile.Checked);
			_changed|=Prefs.Set(PrefName.ClaimAttachExportPath,textClaimAttachPath.Text);
			_changed|=Prefs.Set(PrefName.EclaimsSeparateTreatProv,checkEclaimsSeparateTreatProv.Checked);
			_changed|=Prefs.Set(PrefName.ClaimsValidateACN,checkClaimsValidateACN.Checked);
			_changed|=Prefs.Set(PrefName.ClaimMedTypeIsInstWhenInsPlanIsMedical,checkClaimMedTypeIsInstWhenInsPlanIsMedical.Checked);
			_changed|=Prefs.Set(PrefName.AccountShowPaymentNums,checkAccountShowPaymentNums.Checked);
			_changed|=Prefs.Set(PrefName.PayPlansUseSheets,checkPayPlansUseSheets.Checked);
			_changed|=Prefs.Set(PrefName.RecurringChargesUsePriProv,checkRecurChargPriProv.Checked);
			_changed|=Prefs.Set(PrefName.InsWriteoffDescript,textInsWriteoffDescript.Text);
			_changed|=Prefs.Set(PrefName.PaymentClinicSetting,comboPaymentClinicSetting.SelectedIndex);
			_changed|=Prefs.Set(PrefName.PaymentsPromptForPayType,checkPaymentsPromptForPayType.Checked);
			_changed|=Prefs.Set(PrefName.PayPlansVersion,comboPayPlansVersion.SelectedIndex+1);
			_changed|=Prefs.Set(PrefName.ClaimMedProvTreatmentAsOrdering,checkEclaimsMedicalProvTreatmentAsOrdering.Checked);
			_changed|=Prefs.Set(PrefName.InsPpoAlwaysUseUcrFee,checkPpoUseUcr.Checked);
			_changed|=Prefs.Set(PrefName.ProcPromptForAutoNote,checkProcsPromptForAutoNote.Checked);
			_changed|=Prefs.Set(PrefName.SalesTaxPercentage,taxPercent,false);//Do not round this double for Hawaii
			_changed|=Prefs.Set(PrefName.PayPlansExcludePastActivity,checkPayPlansExcludePastActivity.Checked);
			_changed|=Prefs.Set(PrefName.RecurringChargesUseTransDate,checkRecurringChargesUseTransDate.Checked);
			_changed|=Prefs.Set(PrefName.InvoicePaymentsGridShowNetProd,checkStatementInvoiceGridShowWriteoffs.Checked);
			_changed|=Prefs.Set(PrefName.AccountAllowFutureDebits,checkAllowFutureDebits.Checked);
			_changed|=Prefs.Set(PrefName.PayPlanHideDueNow,checkHideDueNow.Checked);
			_changed|=Prefs.Set(PrefName.AllowProcAdjFromClaim,checkAllowProcAdjFromClaim.Checked);
			_changed|=Prefs.Set(PrefName.ClaimProcAllowCreditsGreaterThanProcFee,comboClaimCredit.SelectedIndex);
			_changed|=Prefs.Set(PrefName.AllowEmailCCReceipt,checkAllowEmailCCReceipt.Checked);
			_changed|=Prefs.Set(PrefName.ClaimIdPrefix,textClaimIdentifier.Text);
			int prefRigorousAccounting=PrefC.GetInt(PrefName.RigorousAccounting);
			if(Prefs.Set(PrefName.RigorousAccounting,comboRigorousAccounting.SelectedIndex)) {
				_changed=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rigorous accounting changed from "+
					((RigorousAccounting)prefRigorousAccounting).GetDescription()+" to "
					+((RigorousAccounting)comboRigorousAccounting.SelectedIndex).GetDescription()+".");
			}
			int prefRigorousAdjustments=PrefC.GetInt(PrefName.RigorousAdjustments);
			if(Prefs.Set(PrefName.RigorousAdjustments,comboRigorousAdjustments.SelectedIndex)) {
				_changed=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rigorous adjustments changed from "+
					((RigorousAdjustments)prefRigorousAdjustments).GetDescription()+" to "
					+((RigorousAdjustments)comboRigorousAdjustments.SelectedIndex).GetDescription()+".");
			}
			//_changed|=Prefs.UpdateBool(PrefName.PayPlanHideDebitsFromAccountModule,checkHidePayPlanDebits.Checked)
			_changed|=Prefs.Set(PrefName.BadDebtAdjustmentTypes,strListBadDebtAdjTypes);
			_changed|=Prefs.Set(PrefName.AllowFutureInsPayments,checkAllowFuturePayments.Checked);
			_changed|=Prefs.Set(PrefName.PaymentWindowDefaultHideSplits,checkHidePaysplits.Checked);
			_changed|=Prefs.Set(PrefName.PaymentsTransferPatientIncomeOnly,checkPaymentsTransferPatientIncomeOnly.Checked);
			_changed|=Prefs.Set(PrefName.ShowAllocateUnearnedPaymentPrompt,checkShowAllocateUnearnedPaymentPrompt.Checked);
			_changed|=Prefs.Set(PrefName.FutureTransDatesAllowed,checkAllowFutureTrans.Checked);
			_changed|=Prefs.Set(PrefName.AgingProcLifo,checkAgingProcLifo.Checked);
			_changed|=Prefs.Set(PrefName.AllowPrepayProvider,checkAllowPrepayProvider.Checked);
			_changed|=Prefs.Set(PrefName.RecurringChargesAutomatedEnabled,checkRecurringChargesAutomated.Checked);
			_changed|=Prefs.Set(PrefName.RecurringChargesAutomatedTime,PIn.Date(textRecurringChargesTime.Text));
			_changed|=Prefs.Set(PrefName.RepeatingChargesAutomated,checkRepeatingChargesAutomated.Checked);
			_changed|=Prefs.Set(PrefName.RepeatingChargesAutomatedTime,PIn.Date(textRepeatingChargesAutomatedTime.Text));
			_changed|=Prefs.Set(PrefName.RepeatingChargesRunAging,checkRepeatingChargesRunAging.Checked);
			_changed|=Prefs.Set(PrefName.ClaimZeroDollarProcBehavior,comboZeroDollarProcClaimBehavior.SelectedIndex);
			_changed|=Prefs.Set(PrefName.ClaimTrackingStatusExcludesNone,checkClaimTrackingExcludeNone.Checked);
			_changed|=Prefs.Set(PrefName.InsPayNoWriteoffMoreThanProc,checkInsPayNoWriteoffMoreThanProc.Checked);
			_changed|=Prefs.Set(PrefName.PromptForSecondaryClaim,checkPromptForSecondaryClaim.Checked);
			_changed|=Prefs.Set(PrefName.InsEstRecalcReceived,checkInsEstRecalcReceived.Checked);
			_changed|=Prefs.Set(PrefName.CanadaCreatePpoLabEst,checkCanadianPpoLabEst.Checked);
			_changed|=Prefs.Set(PrefName.RecurringChargesPayTypeCC,comboRecurringChargePayType.GetSelectedDefNum());
			_changed|=Prefs.Set(PrefName.RecurringChargesAllowedWhenNoPatBal,checkRecurPatBal0.Checked);
			_changed|=Prefs.Set(PrefName.PrePayAllowedForTpProcs,_ynPrePayAllowedForTpProcs);
			_changed|=Prefs.Set(PrefName.TpUnearnedType,comboTpUnearnedType.GetSelectedDefNum());
			_changed|=Prefs.Set(PrefName.TpPrePayIsNonRefundable,checkIsRefundable.Checked);
			_changed|=Prefs.Set(PrefName.DynamicPayPlanRunTime,PIn.Date(textDynamicPayPlan.Text));
			if(comboFinanceChargeAdjType.SelectedIndex!=-1) {
				_changed|=Prefs.Set(PrefName.FinanceChargeAdjustmentType,comboFinanceChargeAdjType.GetSelectedDefNum());
			}
			if(comboBillingChargeAdjType.SelectedIndex!=-1) {
				_changed|=Prefs.Set(PrefName.BillingChargeAdjustmentType,comboBillingChargeAdjType.GetSelectedDefNum());
			}
			if(comboSalesTaxAdjType.SelectedIndex!=-1) {
				_changed|=Prefs.Set(PrefName.SalesTaxAdjustmentType,comboSalesTaxAdjType.GetSelectedDefNum());
			}
			if(comboPayPlanAdj.SelectedIndex!=-1) {
				_changed|=Prefs.Set(PrefName.PayPlanAdjType,comboPayPlanAdj.GetSelectedDefNum());
			}
			if(comboUnallocatedSplits.SelectedIndex!=-1) {
				_changed|=Prefs.Set(PrefName.PrepaymentUnearnedType,comboUnallocatedSplits.GetSelectedDefNum());
			}
			return true;
		}
		#endregion Methods - Account

		#region Methods - Treat' Plan
		private void FillTreatPlan(){
			textTreatNote.Text=Prefs.GetString(PrefName.TreatmentPlanNote);
			checkTreatPlanShowCompleted.Checked=Prefs.GetBool(PrefName.TreatPlanShowCompleted);
			if(Clinics.IsMedicalClinic(Clinics.ClinicId)) {
				checkTreatPlanShowCompleted.Visible=false;
			}
			else {
				checkTreatPlanShowCompleted.Checked=Prefs.GetBool(PrefName.TreatPlanShowCompleted);
			}
			checkTreatPlanItemized.Checked=Prefs.GetBool(PrefName.TreatPlanItemized);
			checkTPSaveSigned.Checked=Prefs.GetBool(PrefName.TreatPlanSaveSignedToPdf);
			checkFrequency.Checked=Prefs.GetBool(PrefName.InsChecksFrequency);
			textInsBW.Text=Prefs.GetString(PrefName.InsBenBWCodes);
			textInsPano.Text=Prefs.GetString(PrefName.InsBenPanoCodes);
			textInsExam.Text=Prefs.GetString(PrefName.InsBenExamCodes);
			textInsCancerScreen.Text=Prefs.GetString(PrefName.InsBenCancerScreeningCodes);
			textInsProphy.Text=Prefs.GetString(PrefName.InsBenProphyCodes);
			textInsFlouride.Text=Prefs.GetString(PrefName.InsBenFlourideCodes);
			textInsSealant.Text=Prefs.GetString(PrefName.InsBenSealantCodes);
			textInsCrown.Text=Prefs.GetString(PrefName.InsBenCrownCodes);
			textInsSRP.Text=Prefs.GetString(PrefName.InsBenSRPCodes);
			textInsDebridement.Text=Prefs.GetString(PrefName.InsBenFullDebridementCodes);
			textInsPerioMaint.Text=Prefs.GetString(PrefName.InsBenPerioMaintCodes);
			textInsDentures.Text=Prefs.GetString(PrefName.InsBenDenturesCodes);
			textInsImplant.Text=Prefs.GetString(PrefName.InsBenImplantCodes);
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
			radioTreatPlanSortTooth.Checked=Prefs.GetBool(PrefName.TreatPlanSortByTooth) || PrefC.RandomKeys;
			//Currently, the TreatPlanSortByTooth preference gets overridden by 
			//the RandomPrimaryKeys preferece due to "Order Entered" being based on the ProcNum
			groupTreatPlanSort.Enabled=!PrefC.RandomKeys;
			textInsHistBW.Text=Prefs.GetString(PrefName.InsHistBWCodes);
			textInsHistDebridement.Text=Prefs.GetString(PrefName.InsHistDebridementCodes);
			textInsHistExam.Text=Prefs.GetString(PrefName.InsHistExamCodes);
			textInsHistFMX.Text=Prefs.GetString(PrefName.InsHistPanoCodes);
			textInsHistPerioMaint.Text=Prefs.GetString(PrefName.InsHistPerioMaintCodes);
			textInsHistPerioLL.Text=Prefs.GetString(PrefName.InsHistPerioLLCodes);
			textInsHistPerioLR.Text=Prefs.GetString(PrefName.InsHistPerioLRCodes);
			textInsHistPerioUL.Text=Prefs.GetString(PrefName.InsHistPerioULCodes);
			textInsHistPerioUR.Text=Prefs.GetString(PrefName.InsHistPerioURCodes);
			textInsHistProphy.Text=Prefs.GetString(PrefName.InsHistProphyCodes);
			checkPromptSaveTP.Checked=Prefs.GetBool(PrefName.TreatPlanPromptSave);
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveTreatPlan(){
			float percent=0;
			if(!float.TryParse(textDiscountPercentage.Text,out percent)) {
				MessageBox.Show("Procedure discount percent is invalid. Please enter a valid number to continue.");
				return false;
			}
			if(Prefs.GetString(PrefName.TreatmentPlanNote)!=textTreatNote.Text) {
				List<long> listTreatPlanNums=TreatPlans.GetNumsByNote(Prefs.GetString(PrefName.TreatmentPlanNote));//Find active/inactive TP's that match exactly.
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
			_changed|=Prefs.Set(PrefName.TreatmentPlanNote,textTreatNote.Text);
			_changed|=Prefs.Set(PrefName.TreatPlanShowCompleted,checkTreatPlanShowCompleted.Checked);
			_changed|=Prefs.Set(PrefName.TreatPlanDiscountPercent,percent);
			_changed|=Prefs.Set(PrefName.TreatPlanItemized,checkTreatPlanItemized.Checked);
			_changed|=Prefs.Set(PrefName.TreatPlanSaveSignedToPdf,checkTPSaveSigned.Checked);
			_changed|=Prefs.Set(PrefName.InsChecksFrequency,checkFrequency.Checked);
			_changed|=Prefs.Set(PrefName.InsBenBWCodes,textInsBW.Text);
			_changed|=Prefs.Set(PrefName.InsBenPanoCodes,textInsPano.Text);
			_changed|=Prefs.Set(PrefName.InsBenExamCodes,textInsExam.Text);
			_changed|=Prefs.Set(PrefName.InsBenCancerScreeningCodes,textInsCancerScreen.Text);
			_changed|=Prefs.Set(PrefName.InsBenProphyCodes,textInsProphy.Text);
			_changed|=Prefs.Set(PrefName.InsBenFlourideCodes,textInsFlouride.Text);
			_changed|=Prefs.Set(PrefName.InsBenSealantCodes,textInsSealant.Text);
			_changed|=Prefs.Set(PrefName.InsBenCrownCodes,textInsCrown.Text);
			_changed|=Prefs.Set(PrefName.InsBenSRPCodes,textInsSRP.Text);
			_changed|=Prefs.Set(PrefName.InsBenFullDebridementCodes,textInsDebridement.Text);
			_changed|=Prefs.Set(PrefName.InsBenPerioMaintCodes,textInsPerioMaint.Text);
			_changed|=Prefs.Set(PrefName.InsBenDenturesCodes,textInsDentures.Text);
			_changed|=Prefs.Set(PrefName.InsBenImplantCodes,textInsImplant.Text);
			_changed|=Prefs.Set(PrefName.TreatPlanSortByTooth,radioTreatPlanSortTooth.Checked || PrefC.RandomKeys);
			_changed|=Prefs.Set(PrefName.InsHistBWCodes,textInsHistBW.Text);
			_changed|=Prefs.Set(PrefName.InsHistDebridementCodes,textInsHistDebridement.Text);
			_changed|=Prefs.Set(PrefName.InsHistExamCodes,textInsHistExam.Text);
			_changed|=Prefs.Set(PrefName.InsHistPanoCodes,textInsHistFMX.Text);
			_changed|=Prefs.Set(PrefName.InsHistPerioMaintCodes,textInsHistPerioMaint.Text);
			_changed|=Prefs.Set(PrefName.InsHistPerioLLCodes,textInsHistPerioLL.Text);
			_changed|=Prefs.Set(PrefName.InsHistPerioLRCodes,textInsHistPerioLR.Text);
			_changed|=Prefs.Set(PrefName.InsHistPerioULCodes,textInsHistPerioUL.Text);
			_changed|=Prefs.Set(PrefName.InsHistPerioURCodes,textInsHistPerioUR.Text);
			_changed|=Prefs.Set(PrefName.InsHistProphyCodes,textInsHistProphy.Text);
			_changed|=Prefs.Set(PrefName.TreatPlanPromptSave, checkPromptSaveTP.Checked);
			return true;
		}
		#endregion Methods - Treat' Plan

		#region Methods - Chart
		private void FillChart(){
			comboToothNomenclature.Items.Add("Universal (Common in the US, 1-32)");
			comboToothNomenclature.Items.Add("FDI Notation (International, 11-48)");
			comboToothNomenclature.Items.Add("Haderup (Danish)");
			comboToothNomenclature.Items.Add("Palmer (Ortho)");
			comboToothNomenclature.SelectedIndex = PrefC.GetInt(PrefName.UseInternationalToothNumbers);
			if(Clinics.IsMedicalClinic(Clinics.ClinicId)) {
				labelToothNomenclature.Visible=false;
				comboToothNomenclature.Visible=false;
			}
			checkAutoClearEntryStatus.Checked=Prefs.GetBool(PrefName.AutoResetTPEntryStatus);
			checkAllowSettingProcsComplete.Checked=Prefs.GetBool(PrefName.AllowSettingProcsComplete);
			//checkChartQuickAddHideAmalgam.Checked=Prefs.GetBool(PrefName.ChartQuickAddHideAmalgam); //Deprecated.
			//checkToothChartMoveMenuToRight.Checked=Prefs.GetBool(PrefName.ToothChartMoveMenuToRight);
			textProblemsIndicateNone.Text		=DiseaseDefs.GetName(Prefs.GetLong(PrefName.ProblemsIndicateNone)); //DB maint to fix corruption
			_diseaseDefNum=Prefs.GetLong(PrefName.ProblemsIndicateNone);
			textMedicationsIndicateNone.Text=Medications.GetDescription(Prefs.GetLong(PrefName.MedicationsIndicateNone)); //DB maint to fix corruption
			_medicationNum=Prefs.GetLong(PrefName.MedicationsIndicateNone);
			textAllergiesIndicateNone.Text	=AllergyDefs.GetDescription(Prefs.GetLong(PrefName.AllergiesIndicateNone)); //DB maint to fix corruption
			_alergyDefNum=Prefs.GetLong(PrefName.AllergiesIndicateNone);
			checkProcGroupNoteDoesAggregate.Checked=Prefs.GetBool(PrefName.ProcGroupNoteDoesAggregate);
			checkChartNonPatientWarn.Checked=Prefs.GetBool(PrefName.ChartNonPatientWarn);
			//checkChartAddProcNoRefreshGrid.Checked=Prefs.GetBool(PrefName.ChartAddProcNoRefreshGrid);//Not implemented.  May revisit some day.
			checkMedicalFeeUsedForNewProcs.Checked=Prefs.GetBool(PrefName.MedicalFeeUsedForNewProcs);
			checkProvColorChart.Checked=Prefs.GetBool(PrefName.UseProviderColorsInChart);
			checkPerioSkipMissingTeeth.Checked=Prefs.GetBool(PrefName.PerioSkipMissingTeeth);
			checkPerioTreatImplantsAsNotMissing.Checked=Prefs.GetBool(PrefName.PerioTreatImplantsAsNotMissing);
			if(Prefs.GetByte(PrefName.DxIcdVersion)==9) {
				checkDxIcdVersion.Checked=false;
			}
			else {//ICD-10
				checkDxIcdVersion.Checked=true;
			}
			SetIcdLabels();
			textICD9DefaultForNewProcs.Text=Prefs.GetString(PrefName.ICD9DefaultForNewProcs);
			checkProcLockingIsAllowed.Checked=Prefs.GetBool(PrefName.ProcLockingIsAllowed);
			textMedDefaultStopDays.Text=Prefs.GetString(PrefName.MedDefaultStopDays);
			checkScreeningsUseSheets.Checked=Prefs.GetBool(PrefName.ScreeningsUseSheets);
			checkProcsPromptForAutoNote.Checked=Prefs.GetBool(PrefName.ProcPromptForAutoNote);
			for(int i=0;i<Enum.GetNames(typeof(ProcCodeListSort)).Length;i++) {
				comboProcCodeListSort.Items.Add(Enum.GetNames(typeof(ProcCodeListSort))[i]);
			}
			comboProcCodeListSort.SelectedIndex=PrefC.GetInt(PrefName.ProcCodeListSortOrder);
			checkProcEditRequireAutoCode.Checked=Prefs.GetBool(PrefName.ProcEditRequireAutoCodes);
			checkClaimProcsAllowEstimatesOnCompl.Checked=Prefs.GetBool(PrefName.ClaimProcsAllowedToBackdate);
			checkSignatureAllowDigital.Checked=Prefs.GetBool(PrefName.SignatureAllowDigital);
			checkCommLogAutoSave.Checked=Prefs.GetBool(PrefName.CommLogAutoSave);
			comboProcFeeUpdatePrompt.Items.Add("No prompt, don't change fee");
			comboProcFeeUpdatePrompt.Items.Add("No prompt, always change fee");
			comboProcFeeUpdatePrompt.Items.Add("Prompt, when patient portion changes");
			comboProcFeeUpdatePrompt.Items.Add("Prompt, always");
			comboProcFeeUpdatePrompt.SelectedIndex=PrefC.GetInt(PrefName.ProcFeeUpdatePrompt);
			checkProcProvChangesCp.Checked=Prefs.GetBool(PrefName.ProcProvChangesClaimProcWithClaim);
			checkBoxRxClinicUseSelected.Checked=Prefs.GetBool(PrefName.ElectronicRxClinicUseSelected);
			if(!PrefC.HasClinicsEnabled) {
				checkBoxRxClinicUseSelected.Visible=false;
			}
			checkProcNoteConcurrencyMerge.Checked=Prefs.GetBool(PrefName.ProcNoteConcurrencyMerge);
			checkIsAlertRadiologyProcsEnabled.Checked=Prefs.GetBool(PrefName.IsAlertRadiologyProcsEnabled);
			checkShowPlannedApptPrompt.Checked=Prefs.GetBool(PrefName.ShowPlannedAppointmentPrompt);
			checkNotesProviderSigOnly.Checked=Prefs.GetBool(PrefName.NotesProviderSignatureOnly);
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
			_changed|=Prefs.Set(PrefName.AllowSettingProcsComplete,checkAllowSettingProcsComplete.Checked);
			_changed|=Prefs.Set(PrefName.AutoResetTPEntryStatus,checkAutoClearEntryStatus.Checked);
			_changed|=Prefs.Set(PrefName.ProblemsIndicateNone,_diseaseDefNum);
			_changed|=Prefs.Set(PrefName.MedicationsIndicateNone,_medicationNum);
			_changed|=Prefs.Set(PrefName.AllergiesIndicateNone,_alergyDefNum);
			_changed|=Prefs.Set(PrefName.UseInternationalToothNumbers,comboToothNomenclature.SelectedIndex);
			_changed|=Prefs.Set(PrefName.ProcGroupNoteDoesAggregate,checkProcGroupNoteDoesAggregate.Checked);
			_changed|=Prefs.Set(PrefName.MedicalFeeUsedForNewProcs,checkMedicalFeeUsedForNewProcs.Checked);
			_changed|=Prefs.Set(PrefName.DxIcdVersion,(byte)(checkDxIcdVersion.Checked?10:9));
			_changed|=Prefs.Set(PrefName.ICD9DefaultForNewProcs,textICD9DefaultForNewProcs.Text);
			_changed|=Prefs.Set(PrefName.ProcLockingIsAllowed,checkProcLockingIsAllowed.Checked);
			_changed|=Prefs.Set(PrefName.ChartNonPatientWarn,checkChartNonPatientWarn.Checked);
			_changed|=Prefs.Set(PrefName.MedDefaultStopDays,daysStop);
			_changed|=Prefs.Set(PrefName.UseProviderColorsInChart,checkProvColorChart.Checked);
			_changed|=Prefs.Set(PrefName.PerioSkipMissingTeeth,checkPerioSkipMissingTeeth.Checked);
			_changed|=Prefs.Set(PrefName.PerioTreatImplantsAsNotMissing,checkPerioTreatImplantsAsNotMissing.Checked);
			_changed|=Prefs.Set(PrefName.ScreeningsUseSheets,checkScreeningsUseSheets.Checked);
			_changed|=Prefs.Set(PrefName.ProcCodeListSortOrder,comboProcCodeListSort.SelectedIndex);
			_changed|=Prefs.Set(PrefName.ProcEditRequireAutoCodes,checkProcEditRequireAutoCode.Checked);
			_changed|=Prefs.Set(PrefName.ClaimProcsAllowedToBackdate,checkClaimProcsAllowEstimatesOnCompl.Checked);
			_changed|=Prefs.Set(PrefName.SignatureAllowDigital,checkSignatureAllowDigital.Checked);
			_changed|=Prefs.Set(PrefName.CommLogAutoSave,checkCommLogAutoSave.Checked);
			_changed|=Prefs.Set(PrefName.ProcFeeUpdatePrompt,comboProcFeeUpdatePrompt.SelectedIndex);
			//_changed|=Prefs.UpdateBool(PrefName.ToothChartMoveMenuToRight,checkToothChartMoveMenuToRight.Checked);
			//_changed|=Prefs.UpdateBool(PrefName.ChartQuickAddHideAmalgam, checkChartQuickAddHideAmalgam.Checked); //Deprecated.
			//_changed|=Prefs.UpdateBool(PrefName.ChartAddProcNoRefreshGrid,checkChartAddProcNoRefreshGrid.Checked);//Not implemented.  May revisit someday.
			_changed|=Prefs.Set(PrefName.ProcProvChangesClaimProcWithClaim,checkProcProvChangesCp.Checked);
			_changed|=Prefs.Set(PrefName.ElectronicRxClinicUseSelected,checkBoxRxClinicUseSelected.Checked);
			_changed|=Prefs.Set(PrefName.ProcNoteConcurrencyMerge,checkProcNoteConcurrencyMerge.Checked);
			_changed|=Prefs.Set(PrefName.IsAlertRadiologyProcsEnabled,checkIsAlertRadiologyProcsEnabled.Checked);
			_changed|=Prefs.Set(PrefName.ShowPlannedAppointmentPrompt,checkShowPlannedApptPrompt.Checked);
			_changed|=Prefs.Set(PrefName.NotesProviderSignatureOnly,checkNotesProviderSigOnly.Checked);
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
			switch(PrefC.GetInt(PrefName.ImagesModuleTreeIsCollapsed)) {
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
			_changed|= Prefs.Set(PrefName.ImagesModuleTreeIsCollapsed,imageModuleIsCollapsedVal);
			return true;
		}
		#endregion Methods - Images

		#region Methods - Manage
		private void FillManage(){
			checkRxSendNewToQueue.Checked=Prefs.GetBool(PrefName.RxSendNewToQueue);
			int claimZeroPayRollingDays=PrefC.GetInt(PrefName.ClaimPaymentNoShowZeroDate);
			if(claimZeroPayRollingDays>=0) {
				textClaimsReceivedDays.Text=(claimZeroPayRollingDays+1).ToString();//The minimum value is now 1 ("today"), to match other areas of OD.
			}
			for(int i=0;i<7;i++) {
				comboTimeCardOvertimeFirstDayOfWeek.Items.Add(Enum.GetNames(typeof(DayOfWeek))[i]);
			}
			comboTimeCardOvertimeFirstDayOfWeek.SelectedIndex=PrefC.GetInt(PrefName.TimeCardOvertimeFirstDayOfWeek);
			checkTimeCardADP.Checked=Prefs.GetBool(PrefName.TimeCardADPExportIncludesName);
			checkClaimsSendWindowValidateOnLoad.Checked=Prefs.GetBool(PrefName.ClaimsSendWindowValidatesOnLoad);
			checkScheduleProvEmpSelectAll.Checked=Prefs.GetBool(PrefName.ScheduleProvEmpSelectAll);
			checkClockEventAllowBreak.Checked=Prefs.GetBool(PrefName.ClockEventAllowBreak);
			checkEraAllowTotalPayment.Checked=Prefs.GetBool(PrefName.EraAllowTotalPayments);
			//Statements
			checkStatementShowReturnAddress.Checked=Prefs.GetBool(PrefName.StatementShowReturnAddress);
			checkStatementShowNotes.Checked=Prefs.GetBool(PrefName.StatementShowNotes);
			checkStatementShowAdjNotes.Checked=Prefs.GetBool(PrefName.StatementShowAdjNotes);
			checkStatementShowProcBreakdown.Checked=Prefs.GetBool(PrefName.StatementShowProcBreakdown);
			comboUseChartNum.Items.Add("PatNum");
			comboUseChartNum.Items.Add("ChartNumber");
			if(Prefs.GetBool(PrefName.StatementAccountsUseChartNumber)) {
				comboUseChartNum.SelectedIndex=1;
			}
			else {
				comboUseChartNum.SelectedIndex=0;
			}
			checkStatementsAlphabetically.Checked=Prefs.GetBool(PrefName.PrintStatementsAlphabetically);
			checkStatementsAlphabetically.Visible=PrefC.HasClinicsEnabled;
			if(Prefs.GetLong(PrefName.StatementsCalcDueDate)!=-1) {
				textStatementsCalcDueDate.Text=Prefs.GetLong(PrefName.StatementsCalcDueDate).ToString();
			}
			textPayPlansBillInAdvanceDays.Text=Prefs.GetLong(PrefName.PayPlansBillInAdvanceDays).ToString();
			textBillingElectBatchMax.Text=PrefC.GetInt(PrefName.BillingElectBatchMax).ToString();
			checkIntermingleDefault.Checked=Prefs.GetBool(PrefName.IntermingleFamilyDefault);
			checkBillingShowProgress.Checked=Prefs.GetBool(PrefName.BillingShowSendProgress);
			checkClaimPaymentBatchOnly.Checked=Prefs.GetBool(PrefName.ClaimPaymentBatchOnly);
			checkEraOneClaimPerPage.Checked=Prefs.GetBool(PrefName.EraPrintOneClaimPerPage);
			checkIncludeEraWOPercCoPay.Checked=Prefs.GetBool(PrefName.EraIncludeWOPercCoPay);
			checkShowAutoDeposit.Checked=Prefs.GetBool(PrefName.ShowAutoDeposit);
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
			_changed|=Prefs.Set(PrefName.RxSendNewToQueue,checkRxSendNewToQueue.Checked);
			_changed|=Prefs.Set(PrefName.TimeCardOvertimeFirstDayOfWeek,comboTimeCardOvertimeFirstDayOfWeek.SelectedIndex);
			_changed|=Prefs.Set(PrefName.TimeCardADPExportIncludesName,checkTimeCardADP.Checked);
			_changed|=Prefs.Set(PrefName.ClaimsSendWindowValidatesOnLoad,checkClaimsSendWindowValidateOnLoad.Checked);
			_changed|=Prefs.Set(PrefName.StatementShowReturnAddress,checkStatementShowReturnAddress.Checked);
			_changed|=Prefs.Set(PrefName.ScheduleProvEmpSelectAll,checkScheduleProvEmpSelectAll.Checked);
			_changed|=Prefs.Set(PrefName.ClockEventAllowBreak,checkClockEventAllowBreak.Checked);
			_changed|=Prefs.Set(PrefName.EraAllowTotalPayments,checkEraAllowTotalPayment.Checked);
			_changed|=Prefs.Set(PrefName.StatementShowNotes,checkStatementShowNotes.Checked);
			_changed|=Prefs.Set(PrefName.StatementShowAdjNotes,checkStatementShowAdjNotes.Checked);
			_changed|=Prefs.Set(PrefName.StatementShowProcBreakdown,checkStatementShowProcBreakdown.Checked);
			_changed|=Prefs.Set(PrefName.StatementAccountsUseChartNumber,comboUseChartNum.SelectedIndex==1);
			_changed|=Prefs.Set(PrefName.PayPlansBillInAdvanceDays,PIn.Long(textPayPlansBillInAdvanceDays.Text));
			_changed|=Prefs.Set(PrefName.IntermingleFamilyDefault,checkIntermingleDefault.Checked);
			_changed|=Prefs.Set(PrefName.BillingElectBatchMax,PIn.Int(textBillingElectBatchMax.Text));
			_changed|=Prefs.Set(PrefName.BillingShowSendProgress,checkBillingShowProgress.Checked);
			_changed|=Prefs.Set(PrefName.ClaimPaymentNoShowZeroDate,(textClaimsReceivedDays.Text=="")?-1:(PIn.Int(textClaimsReceivedDays.Text)-1));
			_changed|=Prefs.Set(PrefName.ClaimPaymentBatchOnly,checkClaimPaymentBatchOnly.Checked);
			_changed|=Prefs.Set(PrefName.EraPrintOneClaimPerPage,checkEraOneClaimPerPage.Checked);
			_changed|=Prefs.Set(PrefName.EraIncludeWOPercCoPay,checkIncludeEraWOPercCoPay.Checked);
			_changed|=Prefs.Set(PrefName.ShowAutoDeposit,checkShowAutoDeposit.Checked);
			_changed|=Prefs.Set(PrefName.PrintStatementsAlphabetically,checkStatementsAlphabetically.Checked);
			if(textStatementsCalcDueDate.Text==""){
				if(Prefs.Set(PrefName.StatementsCalcDueDate,-1)){
					_changed=true;
				}
			}
			else{
				if(Prefs.Set(PrefName.StatementsCalcDueDate,PIn.Long(textStatementsCalcDueDate.Text))){
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






