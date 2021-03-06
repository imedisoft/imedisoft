﻿using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public class AutomationL
	{
		///<summary>ProcCodes will be null unless trigger is CompleteProcedure or ScheduledProcedure.
		///This routine will generally fail silently.  Will return true if a trigger happened.</summary>

		public static bool Trigger(AutomationTrigger trigger, List<string> procCodes, long patNum, long aptNum = 0)
		{
			return Trigger<object>(trigger, procCodes, patNum, aptNum);
		}

		public static bool Trigger<T>(AutomationTrigger trigger, List<string> procCodes, long patNum, long aptNum = 0, T triggerObj = default)
		{
			if (patNum == 0)
			{//Could happen for OpenPatient trigger
				return false;
			}
			List<Automation> listAutomations = Automations.GetAll();
			bool automationHappened = false;
			for (int i = 0; i < listAutomations.Count; i++)
			{
				if (listAutomations[i].Trigger != trigger)
				{
					continue;
				}
				if (trigger == AutomationTrigger.CompleteProcedure || trigger == AutomationTrigger.ScheduleProcedure)
				{
					if (procCodes == null || procCodes.Count == 0)
					{
						continue;//fail silently
					}
					string[] arrayCodes = listAutomations[i].ProcedureCodes.Split(',');
					if (procCodes.All(x => !arrayCodes.Contains(x)))
					{
						continue;
					}
				}
				//matching automation item has been found
				//Get possible list of conditions that exist for this automation item
				List<AutomationCondition> autoConditionsList = AutomationConditions.GetListByAutomationNum(listAutomations[i].Id);
				if (autoConditionsList.Count > 0 && !CheckAutomationConditions(autoConditionsList, patNum, triggerObj))
				{
					continue;
				}
				SheetDef sheetDef;
				Sheet sheet;
				FormSheetFillEdit FormSF;
				Appointment aptNew;
				Appointment aptOld;
				switch (listAutomations[i].Action)
				{
					case AutomationAction.CreateCommlog:
						if (Plugins.HookMethod(null, "AutomationL.Trigger_CreateCommlog_start", patNum, aptNum, listAutomations[i].CommType,
										listAutomations[i].MessageContent, trigger))
						{
							automationHappened = true;
							continue;
						}
						Commlog commlogCur = new Commlog();
						commlogCur.PatNum = patNum;
						commlogCur.CommDateTime = DateTime.Now;
						commlogCur.CommType = listAutomations[i].CommType.Value;
						commlogCur.Note = listAutomations[i].MessageContent;
						commlogCur.Mode_ = CommItemMode.None;
						commlogCur.UserNum = Security.CurrentUser.Id;
						commlogCur.IsNew = true;
						FormCommItem commItemView = new FormCommItem(commlogCur);
						commItemView.ShowDialog();
						automationHappened = true;
						continue;
					case AutomationAction.PopUp:
						MessageBox.Show(listAutomations[i].MessageContent);
						automationHappened = true;
						continue;
					case AutomationAction.PopUpThenDisable10Min:
						Plugins.HookAddCode(null, "AutomationL.Trigger_PopUpThenDisable10Min_begin", listAutomations[i], procCodes, patNum);
						long automationNum = listAutomations[i].Id;
						bool hasAutomationBlock = FormOpenDental.DicBlockedAutomations.ContainsKey(automationNum);
						if (hasAutomationBlock && FormOpenDental.DicBlockedAutomations[automationNum].ContainsKey(patNum))
						{//Automation block exist for current patient.
							continue;
						}
						if (hasAutomationBlock)
						{
							FormOpenDental.DicBlockedAutomations[automationNum].Add(patNum, DateTime.Now.AddMinutes(10));//Disable for 10 minutes.
						}
						else
						{//Add automationNum to higher level dictionary .
							FormOpenDental.DicBlockedAutomations.Add(automationNum,
								new Dictionary<long, DateTime>()
								{
									{ patNum,DateTime.Now.AddMinutes(10) }//Disable for 10 minutes.
								});
						}
						MessageBox.Show(listAutomations[i].MessageContent);
						automationHappened = true;
						continue;
					case AutomationAction.PrintPatientLetter:
					case AutomationAction.ShowExamSheet:
					case AutomationAction.ShowConsentForm:
						sheetDef = SheetDefs.GetSheetDef(listAutomations[i].SheetDefinitionId.Value);
						sheet = SheetUtil.CreateSheet(sheetDef, patNum);
						SheetParameter.SetParameter(sheet, "PatNum", patNum);
						SheetFiller.FillFields(sheet);
						SheetUtil.CalculateHeights(sheet);
						FormSF = new FormSheetFillEdit(sheet);
						FormSF.ShowDialog();
						automationHappened = true;
						continue;
					case AutomationAction.PrintReferralLetter:
						long referralNum = RefAttaches.GetReferralNum(patNum);
						if (referralNum == 0)
						{
							MsgBox.Show("This patient has no referral source entered.");
							automationHappened = true;
							continue;
						}
						sheetDef = SheetDefs.GetSheetDef(listAutomations[i].SheetDefinitionId.Value);
						sheet = SheetUtil.CreateSheet(sheetDef, patNum);
						SheetParameter.SetParameter(sheet, "PatNum", patNum);
						SheetParameter.SetParameter(sheet, "ReferralNum", referralNum);
						//Don't fill these params if the sheet doesn't use them.
						if (sheetDef.SheetFieldDefs.Any(x =>
							 (x.FieldType == SheetFieldType.Grid && x.FieldName == "ReferralLetterProceduresCompleted")
							 || (x.FieldType == SheetFieldType.Special && x.FieldName == "toothChart")))
						{
							List<Procedure> listProcs = Procedures.GetCompletedForDateRange(DateTime.Today, DateTime.Today
								, listPatNums: new List<long>() { patNum }
								, includeNote: true
								, includeGroupNote: true
							);
							if (sheetDef.SheetFieldDefs.Any(x => x.FieldType == SheetFieldType.Grid && x.FieldName == "ReferralLetterProceduresCompleted"))
							{
								SheetParameter.SetParameter(sheet, "CompletedProcs", listProcs);
							}
							if (sheetDef.SheetFieldDefs.Any(x => x.FieldType == SheetFieldType.Special && x.FieldName == "toothChart"))
							{
								SheetParameter.SetParameter(sheet, "toothChartImg", SheetPrinting.GetToothChartHelper(patNum, false, listProceduresFilteredOverride: listProcs));
							}
						}
						SheetFiller.FillFields(sheet);
						SheetUtil.CalculateHeights(sheet);
						FormSF = new FormSheetFillEdit(sheet);
						FormSF.ShowDialog();
						automationHappened = true;
						continue;
					case AutomationAction.SetApptASAP:
						aptNew = Appointments.GetOneApt(aptNum);
						if (aptNew == null)
						{
							MsgBox.Show("Invalid appointment for automation.");
							automationHappened = true;
							continue;
						}
						aptOld = aptNew.Copy();
						aptNew.Priority = ApptPriority.ASAP;
						Appointments.Update(aptNew, aptOld);//Appointments S-Class handles Signalods
						continue;
					case AutomationAction.SetApptType:
						aptNew = Appointments.GetOneApt(aptNum);
						if (aptNew == null)
						{
							MsgBox.Show("Invalid appointment for automation.");
							automationHappened = true;
							continue;
						}
						aptOld = aptNew.Copy();
						aptNew.AppointmentTypeNum = listAutomations[i].AppointmentTypeId.Value;
						AppointmentType aptTypeCur = AppointmentTypes.GetFirstOrDefault(x => x.Id == aptNew.AppointmentTypeNum);
						if (aptTypeCur != null)
						{
							aptNew.ColorOverride = aptTypeCur.Color;
							aptNew.Pattern = AppointmentTypes.GetTimePatternForAppointmentType(aptTypeCur);
							List<Procedure> listProcs = Appointments.ApptTypeMissingProcHelper(aptNew, aptTypeCur, new List<Procedure>());
							Procedures.UpdateAptNums(listProcs.Select(x => x.ProcNum).ToList(), aptNew.AptNum, aptNew.AptStatus == ApptStatus.Planned);
						}
						Appointments.Update(aptNew, aptOld);//Appointments S-Class handles Signalods
						continue;
					case AutomationAction.PatRestrictApptSchedTrue:
						if (!Security.IsAuthorized(Permissions.PatientApptRestrict, true))
						{
							SecurityLogs.MakeLogEntry(Permissions.PatientApptRestrict, patNum, "Attempt to restrict patient scheduling was blocked due to lack of user permission.");
							continue;
						}
						PatRestrictions.Upsert(patNum, PatRestrict.ApptSchedule);
						automationHappened = true;
						continue;
					case AutomationAction.PatRestrictApptSchedFalse:
						if (!Security.IsAuthorized(Permissions.PatientApptRestrict, true))
						{
							SecurityLogs.MakeLogEntry(Permissions.PatientApptRestrict, patNum, "Attempt to allow patient scheduling was blocked due to lack of user permission.");
							continue;
						}
						PatRestrictions.RemovePatRestriction(patNum, PatRestrict.ApptSchedule);
						automationHappened = true;
						continue;
					case AutomationAction.PrintRxInstruction:
						List<RxPat> listRx = (List<RxPat>)(object)triggerObj;
						if (listRx == null)
						{
							//Got here via a pre-existing trigger that doesn't pass in triggerObj.  We now block creation of automation triggers that could get 
							//here via code that does not pass in triggerObj.
							continue;
						}
						//We go through each new Rx where the patient note isn't blank.
						//There should only usually be one new rx, but we'll loop just in case.
						foreach (RxPat rx in listRx.Where(x => !string.IsNullOrWhiteSpace(x.PatientInstruction)))
						{
							//This logic is an exact copy of FormRxManage.butPrintSelect_Click()'s logic when 1 Rx is selected.  
							//If this is updated, that method needs to be updated as well.
							sheetDef = SheetDefs.GetSheetDef(listAutomations[i].SheetDefinitionId.Value);
							sheet = SheetUtil.CreateSheet(sheetDef, patNum);
							SheetParameter.SetParameter(sheet, "RxNum", rx.Id);
							SheetFiller.FillFields(sheet);
							SheetUtil.CalculateHeights(sheet);
							FormSF = new FormSheetFillEdit(sheet);
							FormSF.ShowDialog();
							automationHappened = true;
						}
						continue;
					case AutomationAction.ChangePatStatus:
						Patient pat = Patients.GetPat(patNum);
						Patient patOld = pat.Copy();
						pat.PatStatus = listAutomations[i].PatientStatus;
						//Don't allow changing status from Archived if this is a merged patient.
						if (patOld.PatStatus != pat.PatStatus
							&& patOld.PatStatus == PatientStatus.Archived
							&& PatientLinks.WasPatientMerged(patOld.PatNum))
						{
							MsgBox.Show("Not allowed to change the status of a merged patient.");
							continue;
						}
						switch (pat.PatStatus)
						{
							case PatientStatus.Deceased:
								if (patOld.PatStatus != PatientStatus.Deceased)
								{
									List<Appointment> listFutureAppts = Appointments.GetFutureSchedApts(pat.PatNum);
									if (listFutureAppts.Count > 0)
									{
										string apptDates = string.Join("\r\n", listFutureAppts.Take(10).Select(x => x.AptDateTime.ToString()));
										if (listFutureAppts.Count > 10)
										{
											apptDates += "(...)";
										}
										if (MessageBox.Show(
											"This patient has scheduled appointments in the future" + ":\r\n" + apptDates + "\r\n"
												+ "Would you like to delete them and set the patient to Deceased?",
											"Delete future appointments?",
											MessageBoxButtons.YesNo) == DialogResult.Yes)
										{
											foreach (Appointment appt in listFutureAppts)
											{
												Appointments.Delete(appt.AptNum, true);
											}
										}
										else
										{
											continue;
										}
									}
								}
								break;
						}
						//Re-activate or disable recalls depending on the the status that the patient is changing to.
						Patients.UpdateRecalls(pat, patOld, "ChangePatStatus automation");
						if (Patients.Update(pat, patOld))
						{
							SecurityLogs.MakeLogEntry(Permissions.PatientEdit, patNum, "Patient status changed from " + patOld.PatStatus.GetDescription() +
								" to " + listAutomations[i].PatientStatus.GetDescription() + " through ChangePatStatus automation.");
						}
						automationHappened = true;
						continue;
				}
			}
			return automationHappened;
		}

		private static bool CheckAutomationConditions<T>(List<AutomationCondition> autoConditionsList, long patNum, T triggerObj = default)
		{
			//Make sure every condition returns true
			for (int i = 0; i < autoConditionsList.Count; i++)
			{
				switch (autoConditionsList[i].CompareField)
				{
					case AutoCondField.NeedsSheet:
						if (NeedsSheet(autoConditionsList[i], patNum))
						{
							return false;
						}
						break;
					case AutoCondField.Problem:
						if (!ProblemComparison(autoConditionsList[i], patNum))
						{
							return false;
						}
						break;
					case AutoCondField.Medication:
						if (!MedicationComparison(autoConditionsList[i], patNum))
						{
							return false;
						}
						break;
					case AutoCondField.Allergy:
						if (!AllergyComparison(autoConditionsList[i], patNum))
						{
							return false;
						}
						break;
					case AutoCondField.Age:
						if (!AgeComparison(autoConditionsList[i], patNum))
						{
							return false;
						}
						break;
					case AutoCondField.Gender:
						if (!GenderComparison(autoConditionsList[i], patNum))
						{
							return false;
						}
						break;
					case AutoCondField.Labresult:
						if (!LabresultComparison(autoConditionsList[i], patNum))
						{
							return false;
						}
						break;
					case AutoCondField.InsuranceNotEffective:
						if (!InsuranceNotEffectiveComparison(autoConditionsList[i], patNum))
						{
							return false;
						}
						break;
					case AutoCondField.BillingType:
						if (!BillingTypeComparison(autoConditionsList[i], patNum))
						{
							return false;
						}
						break;
					case AutoCondField.IsProcRequired:
						//ONLY TO BE USED FOR RxCreate AUTOMATION TRIGGER
						if (!IsProcRequiredComparison(autoConditionsList[i], patNum, triggerObj))
						{
							return false;
						}
						break;
					case AutoCondField.IsControlled:
						//ONLY TO BE USED FOR RxCreate AUTOMATION TRIGGER
						if (!IsControlledComparison(autoConditionsList[i], patNum, triggerObj))
						{
							return false;
						}
						break;
					case AutoCondField.IsPatientInstructionPresent:
						//ONLY TO BE USED FOR RxCreate AUTOMATION TRIGGER
						if (!IsPatientInstructionPresent(autoConditionsList[i], patNum, triggerObj))
						{
							return false;
						}
						break;
				}
			}
			return true;
		}
		#region Comparisons
		private static bool NeedsSheet(AutomationCondition autoCond, long patNum)
		{
			List<Sheet> sheetList = Sheets.GetForPatientForToday(patNum);
			switch (autoCond.Comparison)
			{//Find out what operand to use.
				case AutoCondComparison.Equals:
					//Loop through every sheet to find one that matches the condition.
					for (int i = 0; i < sheetList.Count; i++)
					{
						if (sheetList[i].Description == autoCond.CompareString)
						{//Operand based on AutoCondComparison.
							return true;
						}
					}
					break;
				case AutoCondComparison.Contains:
					for (int i = 0; i < sheetList.Count; i++)
					{
						if (sheetList[i].Description.ToLower().Contains(autoCond.CompareString.ToLower()))
						{
							return true;
						}
					}
					break;
			}
			return false;
		}

		private static bool ProblemComparison(AutomationCondition autoCond, long patNum)
		{
			List<Problem> problemList = Problems.GetByPatient(patNum, true).ToList();
			switch (autoCond.Comparison)
			{//Find out what operand to use.
				case AutoCondComparison.Equals:
					for (int i = 0; i < problemList.Count; i++)
					{//Includes hidden
						if (ProblemDefinitions.GetName(problemList[i].ProblemDefId) == autoCond.CompareString)
						{
							return true;
						}
					}
					break;
				case AutoCondComparison.Contains:
					for (int i = 0; i < problemList.Count; i++)
					{
						if (ProblemDefinitions.GetName(problemList[i].ProblemDefId).ToLower().Contains(autoCond.CompareString.ToLower()))
						{
							return true;
						}
					}
					break;
			}
			return false;
		}

		private static bool MedicationComparison(AutomationCondition autoCond, long patNum)
		{
			List<Medication> medList = Medications.GetByPatientNoCache(patNum).ToList();
			switch (autoCond.Comparison)
			{
				case AutoCondComparison.Equals:
					for (int i = 0; i < medList.Count; i++)
					{
						if (medList[i].Name == autoCond.CompareString)
						{
							return true;
						}
					}
					break;
				case AutoCondComparison.Contains:
					for (int i = 0; i < medList.Count; i++)
					{
						if (medList[i].Name.ToLower().Contains(autoCond.CompareString.ToLower()))
						{
							return true;
						}
					}
					break;
			}
			return false;
		}

		private static bool AllergyComparison(AutomationCondition autoCond, long patNum)
		{
			List<AllergyDef> listAllergyDefs = AllergyDefs.GetByPatient(patNum, false).ToList();

            return autoCond.Comparison switch
            {
                AutoCondComparison.Equals => listAllergyDefs.Any(x => x.Description == autoCond.CompareString),
                AutoCondComparison.Contains => listAllergyDefs.Any(x => x.Description.ToLower().Contains(autoCond.CompareString.ToLower())),
                _ => false,
            };
        }

		private static bool AgeComparison(AutomationCondition autoCond, long patNum)
		{
			Patient pat = Patients.GetPat(patNum);

			int age = pat.Age;
            if (!int.TryParse(autoCond.CompareString, out int ageTrigger))
            {
                return false; // This is only possible due to an old bug that was fixed.
            }

            return autoCond.Comparison switch
            {
                AutoCondComparison.Equals => age == ageTrigger,
                AutoCondComparison.Contains => age.ToString().Contains(autoCond.CompareString),
                AutoCondComparison.GreaterThan => age > ageTrigger,
                AutoCondComparison.LessThan => age < ageTrigger,
                _ => false,
            };
        }

		private static bool GenderComparison(AutomationCondition autoCond, long patNum)
		{
			Patient pat = Patients.GetPat(patNum);

            return autoCond.Comparison switch
            {
                AutoCondComparison.Equals => (pat.Gender.ToString().Substring(0, 1).ToLower() == autoCond.CompareString.ToLower()),
                AutoCondComparison.Contains => (pat.Gender.ToString().Substring(0, 1).ToLower().Contains(autoCond.CompareString.ToLower())),
                _ => false,
            };
        }

		private static bool LabresultComparison(AutomationCondition autoCond, long patNum)
		{
			List<LabResult> listResults = LabResults.GetAllForPatient(patNum);
			switch (autoCond.Comparison)
			{
				case AutoCondComparison.Equals:
					for (int i = 0; i < listResults.Count; i++)
					{
						if (listResults[i].TestName == autoCond.CompareString)
						{
							return true;
						}
					}
					break;
				case AutoCondComparison.Contains:
					for (int i = 0; i < listResults.Count; i++)
					{
						if (listResults[i].TestName.ToLower().Contains(autoCond.CompareString.ToLower()))
						{
							return true;
						}
					}
					break;
			}
			return false;
		}

		///<summary>Returns false if the insurance plan is effective.  True if today is outside of the insurance effective date range.</summary>
		private static bool InsuranceNotEffectiveComparison(AutomationCondition autoCond, long patNum)
		{
			PatPlan patPlanCur = PatPlans.GetPatPlan(patNum, 1);
			if (patPlanCur == null)
			{
				return false;
			}

			InsSub insSubCur = InsSubs.GetOne(patPlanCur.InsSubNum);
			if (insSubCur == null)
			{
				return false;
			}

			if (DateTime.Today >= insSubCur.DateEffective && DateTime.Today <= insSubCur.DateTerm)
			{
				return false;//Allen - Not not effective
			}

			return true;
		}

		///<summary>Returns true if the patient's billing type matches the autocondition billing type.</summary>
		private static bool BillingTypeComparison(AutomationCondition autoCond, long patNum)
		{
			Patient pat = Patients.GetPat(patNum);
			Definition patBillType = Definitions.GetDef(DefinitionCategory.BillingTypes, pat.BillingType);
			if (patBillType == null)
			{
				return false;
			}
			switch (autoCond.Comparison)
			{
				case AutoCondComparison.Equals:
					return patBillType.Name.ToLower() == autoCond.CompareString.ToLower();
				case AutoCondComparison.Contains:
					return patBillType.Name.ToLower().Contains(autoCond.CompareString.ToLower());
				default:
					return false;
			}
		}

		///<summary>Returns true if the patient is a RxPat and if IsProcRequired is true</summary>
		///ONLY TO BE USED FOR RxCreate AUTOMATION TRIGGER
		private static bool IsProcRequiredComparison<T>(AutomationCondition autoCond, long patNum, T triggerObj)
		{
			try
			{
				List<RxPat> listRx = (List<RxPat>)(object)triggerObj;
				return listRx.Any(x => x.IsProcRequired);
			}
			catch
			{
				return false;
			}
		}

		///<summary>Returns true if the patient is a RxPat and if IsControlled is true</summary>
		///ONLY TO BE USED FOR RxCreate AUTOMATION TRIGGER
		private static bool IsControlledComparison<T>(AutomationCondition autoCond, long patNum, T triggerObj)
		{
			try
			{
				List<RxPat> listRx = (List<RxPat>)(object)triggerObj;
				return listRx.Any(x => x.IsControlled);
			}
			catch
			{
				return false;
			}
		}

		///<summary>Returns true if at least one RxPat has a patient letter filled out.</summary>
		private static bool IsPatientInstructionPresent<T>(AutomationCondition autoCond, long patNum, T triggerObj)
		{
			try
			{
				List<RxPat> listRx = (List<RxPat>)(object)triggerObj;
				return listRx.Any(x => !string.IsNullOrWhiteSpace(x.PatientInstruction));
			}
			catch
			{
				return false;
			}
		}
		#endregion
	}
}
