using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAutoCodeLessIntrusive : FormBase
	{
		private readonly Patient patient;
        private readonly ProcedureCode procedureCode;
		private readonly long newProcedureCodeId;
		private readonly List<PatPlan> patientPlans;
		private readonly List<InsSub> insuranceSubscribers;
		private readonly List<InsurancePlan> insurancePlans;
		private readonly List<Benefit> benefits;
		private readonly List<ClaimProc> claimProcedures;
		private readonly string teeth;

		public Procedure Procedure { get; }

        public FormAutoCodeLessIntrusive(Patient patient, Procedure procedure, ProcedureCode procedureCode, long newProcedureCodeId, List<PatPlan> patientPlans, List<InsSub> insuranceSubscribers, List<InsurancePlan> insurancePlans, List<Benefit> benefits, List<ClaimProc> claimProcedures, string teeth = null)
		{
			Procedure = procedure;

			this.patient = patient;
			this.procedureCode = procedureCode;
			this.newProcedureCodeId = newProcedureCodeId;
			this.patientPlans = patientPlans;
			this.insuranceSubscribers = insuranceSubscribers;
			this.insurancePlans = insurancePlans;
			this.benefits = benefits;
			this.claimProcedures = claimProcedures;
			this.teeth = teeth;

			InitializeComponent();
		}

		private void FormAutoCodeLessIntrusive_Load(object sender, EventArgs e)
		{
			var newProcedureCode = ProcedureCodes.GetById(newProcedureCodeId);

			procedureLabel.Text =
				newProcedureCode.Code + " (" + newProcedureCode.Description + ") " +
				"is the recommended procedure code for this procedure. Change procedure code and fee?";

			if (Preferences.GetBool(PreferenceName.ProcEditRequireAutoCodes))
			{
				cancelButton.Text = "Edit Proc";
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (OrthoProcLinks.IsProcLinked(Procedure.ProcNum))
			{
				ShowError("This procedure is attached to an ortho case and its code cannot be changed.");

				return;
			}

			// Moved from FormProcEdit.SaveAndClose() in version 16.3+
			Procedure procOld = Procedure.Copy();
			Procedure.CodeNum = newProcedureCodeId;

			if (new[] { ProcStat.TP, ProcStat.C, ProcStat.TPi, ProcStat.Cn }.Contains(Procedure.ProcStatus))
			{
				// Only change the fee if Complete, TP, TPi, or Cn.
				InsSub prisub;
				InsurancePlan priplan = null;
				if (patientPlans.Count > 0)
				{
					prisub = InsSubs.GetSub(patientPlans[0].InsSubNum, insuranceSubscribers);
					priplan = InsPlans.GetPlan(prisub.PlanNum, insurancePlans);
				}

				Procedure.ProcFee = Fees.GetAmount0(Procedure.CodeNum, FeeScheds.GetFeeSched(patient, insurancePlans, patientPlans, insuranceSubscribers, Procedure.ProvNum),
					Procedure.ClinicNum, Procedure.ProvNum);

				if (priplan != null && priplan.PlanType == "p")
				{
					// PPO
					double standardfee = Fees.GetAmount0(Procedure.CodeNum, Providers.GetById(Patients.GetProvNum(patient)).FeeScheduleId, Procedure.ClinicNum,
						Procedure.ProvNum);
					Procedure.ProcFee = Math.Max(Procedure.ProcFee, standardfee);
				}
			}

			Procedures.Update(Procedure, procOld);

			// Compute estimates required, otherwise if adding through quick add, it could have incorrect WO or InsEst if code changed.
			Procedures.ComputeEstimates(Procedure, patient.PatNum, claimProcedures, true, insurancePlans, patientPlans, benefits, patient.Age, insuranceSubscribers);
			
			Recalls.Synch(Procedure.PatNum);

			if (Procedure.ProcStatus.In(ProcStat.C, ProcStat.EO, ProcStat.EC))
			{
				string logText = procedureCode.Code + " (" + Procedure.ProcStatus + "), ";
				if (teeth != null && teeth.Trim() != "")
				{
					logText += "Teeth: " + teeth + ", ";
				}
				logText += "Fee" + ": " + Procedure.ProcFee.ToString("F") + ", " + procedureCode.Description;

				Permissions perm = Permissions.ProcCompleteEdit;
				if (Procedure.ProcStatus.In(ProcStat.EO, ProcStat.EC))
				{
					perm = Permissions.ProcExistingEdit;
				}

				SecurityLogs.MakeLogEntry(perm, patient.PatNum, logText);
			}

			DialogResult = DialogResult.OK;
		}
	}
}
