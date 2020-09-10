using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormInsPayFix : ODForm
	{
		public FormInsPayFix()
		{
			InitializeComponent();
		}

		private void butRun_Click(object sender, EventArgs e)
		{
			List<ClaimPaySplit> splits = Claims.GetInsPayNotAttachedForFixTool();
			if (splits.Count == 0)
			{
				MessageBox.Show("There are currently no insurance payments that are not attached to an insurance check.");

				DialogResult = DialogResult.OK;

				return;
			}

			Cursor = Cursors.WaitCursor;
			string invalidClaimDate = "";
			DateTime curDate = MiscData.GetNowDateTime().Date;
			for (int i = 0; i < splits.Count; i++)
			{
				Claim claim = Claims.GetClaim(splits[i].ClaimNum);
				if (claim == null)
				{
					continue;
				}
				if (claim.DateReceived.Date > curDate && !Preferences.GetBool(PreferenceName.AllowFutureInsPayments) && !Preferences.GetBool(PreferenceName.FutureTransDatesAllowed))
				{
					invalidClaimDate += "\r\n" + "PatNum" + " " + claim.PatNum + ", " + claim.DateService.ToShortDateString();
					continue;
				}
				ClaimPayment cp = new ClaimPayment();
				cp.CheckDate = claim.DateReceived;
				cp.CheckAmt = splits[i].InsPayAmt;
				cp.ClinicNum = claim.ClinicNum;
				cp.CarrierName = splits[i].Carrier;
				cp.PayType = Definitions.GetFirstForCategory(DefinitionCategory.InsurancePaymentType, true).Id;
				ClaimPayments.Insert(cp);
				List<ClaimProc> claimP = ClaimProcs.RefreshForClaim(splits[i].ClaimNum);
				for (int j = 0; j < claimP.Count; j++)
				{
					if (claimP[j].ClaimPaymentNum != 0 || claimP[j].InsPayAmt == 0)
					{ //If claimpayment already attached to claimproc or ins didn't pay.
						continue; //Do not change
					}
					claimP[j].DateCP = claim.DateReceived;
					claimP[j].ClaimPaymentNum = cp.ClaimPaymentNum;
					ClaimProcs.Update(claimP[j]);
				}
			}

			Cursor = Cursors.Default;
			if (invalidClaimDate != "")
			{
				invalidClaimDate = "\r\n" + "Cannot make future-dated insurance payments for these claims:" + invalidClaimDate;
			}

			MsgBoxCopyPaste messageBox = new MsgBoxCopyPaste("Insurance checks created:" + " " + splits.Count + invalidClaimDate);
			messageBox.ShowDialog();

			DialogResult = DialogResult.OK;//Close the window because there is nothing else to do
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
