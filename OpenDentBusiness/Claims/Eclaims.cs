using CodeBase;
using Imedisoft.Claims;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace OpenDentBusiness.Eclaims
{
	/// <summary>
	/// Summary description for Eclaims.
	/// </summary>
	public class Eclaims
	{
		/// <summary>
		/// Supply a list of ClaimSendQueueItems. 
		/// Called from FormClaimSend. 
		/// Can only send to one clearinghouse at a time.
		/// The queueItems must contain at least one item. 
		/// Each item in queueItems must have the same ClinicNum. 
		/// Cannot include Canadian.
		/// </summary>
		public static void SendBatch(Clearinghouse clearingHouse, List<ClaimSendQueueItem> queueItems, EnumClaimMedType medType, IFormClaimFormItemEdit formClaimFormItemEdit, Renaissance.FillRenaissanceDelegate fillRenaissance, ITerminalConnector terminalConnector)
		{
			var claimBridge = ClaimBridgeManager.GetBridgeByType(clearingHouse.TypeName);
			if (null == claimBridge)
			{
				return;
			}

			int batchNumber = Clearinghouses.GetNextBatchNumber(clearingHouse);

			if (!claimBridge.Send(clearingHouse, batchNumber, queueItems, medType))
			{
				if (string.IsNullOrEmpty(claimBridge.ErrorMessage))
				{
					MessageBox.Show(
						claimBridge.ErrorMessage, "Imedisoft",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
				}
				else
				{
					MessageBox.Show(
						"Failed to send.", "Imedisoft",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
				}
			}

			////---------------------------------------------------------------------------------------
			////Create the claim file for this clearinghouse
			//if (clearinghouseClin.TypeName == ElectronicClaimFormat.x837D_4010
			//	|| clearinghouseClin.TypeName == ElectronicClaimFormat.x837D_5010_dental
			//	|| clearinghouseClin.TypeName == ElectronicClaimFormat.x837_5010_med_inst)
			//{
			//	messageText = x837Controller.SendBatch(clearinghouseClin, queueItems, batchNumber, medType, false);
			//}
			//else if (clearinghouseClin.TypeName == ElectronicClaimFormat.Renaissance)
			//{
			//	messageText = Renaissance.SendBatch(clearinghouseClin, queueItems, batchNumber, formClaimFormItemEdit, fillRenaissance);
			//}
			//else if (clearinghouseClin.TypeName == ElectronicClaimFormat.Dutch)
			//{
			//	messageText = Dutch.SendBatch(clearinghouseClin, queueItems, batchNumber);
			//}
			//else if (clearinghouseClin.TypeName == ElectronicClaimFormat.Ramq)
			//{
			//	messageText = Ramq.SendBatch(clearinghouseClin, queueItems, batchNumber);
			//}
			//else
			//{
			//	messageText = "";//(ElectronicClaimFormat.None does not get sent)
			//}


			//if (messageText == "")
			//{//if failed to create claim file properly,
			//	return;//don't launch program or change claim status
			//}
			////----------------------------------------------------------------------------------------
			////Launch Client Program for this clearinghouse if applicable
			//if (clearinghouseClin.CommBridge == EclaimsCommBridge.None)
			//{
			//	AttemptLaunch(clearinghouseClin, batchNumber);
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.WebMD)
			//{
			//	if (!WebMD.Launch(clearinghouseClin, batchNumber))
			//	{
			//		MessageBox.Show("Error sending." + "\r\n" + WebMD.ErrorMessage);
			//		return;
			//	}
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.BCBSGA)
			//{
			//	if (!BCBSGA.Launch(clearinghouseClin, batchNumber, terminalConnector))
			//	{
			//		MessageBox.Show("Error sending." + "\r\n" + BCBSGA.ErrorMessage);
			//		return;
			//	}
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.Renaissance)
			//{
			//	AttemptLaunch(clearinghouseClin, batchNumber);
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.ClaimConnect)
			//{
			//	if (ClaimConnect.Launch(clearinghouseClin, batchNumber))
			//	{
			//		MessageBox.Show("Upload successful.");
			//	}
			//	else
			//	{
			//		MessageBox.Show("Error sending." + "\r\n" + ClaimConnect.ErrorMessage);
			//		return;
			//	}
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.RECS)
			//{
			//	if (!RECS.Launch(clearinghouseClin, batchNumber))
			//	{
			//		MessageBox.Show("Claim file created, but could not launch RECS client." + "\r\n" + RECS.ErrorMessage);
			//		//continue;
			//	}
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.Inmediata)
			//{
			//	if (!Inmediata.Launch(clearinghouseClin, batchNumber))
			//	{
			//		MessageBox.Show("Claim file created, but could not launch Inmediata client." + "\r\n" + Inmediata.ErrorMessage);
			//		//continue;
			//	}
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.AOS)
			//{ // added by SPK 7/13/05
			//	if (!AOS.Launch(clearinghouseClin, batchNumber))
			//	{
			//		MessageBox.Show("Claim file created, but could not launch AOS Communicator." + "\r\n" + AOS.ErrorMessage);
			//		//continue;
			//	}
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.PostnTrack)
			//{
			//	AttemptLaunch(clearinghouseClin, batchNumber);
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.MercuryDE)
			//{
			//	if (!MercuryDE.Launch(clearinghouseClin, batchNumber))
			//	{
			//		MessageBox.Show("Error sending." + "\r\n" + MercuryDE.ErrorMessage);
			//		return;
			//	}
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.ClaimX)
			//{
			//	if (!ClaimX.Launch(clearinghouseClin, batchNumber))
			//	{
			//		MessageBox.Show("Claim file created, but encountered an error while launching ClaimX Client." + ":\r\n" + ClaimX.ErrorMessage);
			//	}
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.EmdeonMedical)
			//{
			//	if (!EmdeonMedical.Launch(clearinghouseClin, batchNumber, medType))
			//	{
			//		MessageBox.Show("Error sending." + "\r\n" + EmdeonMedical.ErrorMessage);
			//		return;
			//	}
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.DentiCal)
			//{
			//	if (!DentiCal.Launch(clearinghouseClin, batchNumber))
			//	{
			//		MessageBox.Show("Error sending." + DentiCal.ErrorMessage);
			//		return;
			//	}
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.NHS)
			//{
			//	if (!NHS.Launch(clearinghouseClin, batchNumber))
			//	{
			//		MessageBox.Show("Error sending." + "\r\n" + NHS.ErrorMessage);
			//		return;
			//	}
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.EDS)
			//{
			//	if (!EDS.Launch(clearinghouseClin, messageText))
			//	{
			//		MessageBox.Show("Error sending." + "\r\n" + EDS.ErrorMessage);
			//		return;
			//	}
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.EdsMedical)
			//{
			//	if (!EdsMedical.Launch(clearinghouseClin, messageText))
			//	{
			//		MessageBox.Show("Error sending." + "\r\n" + EdsMedical.ErrorMessage);
			//		return;
			//	}
			//}
			//else if (clearinghouseClin.CommBridge == EclaimsCommBridge.Ramq)
			//{
			//	if (!Ramq.Launch(clearinghouseClin, batchNumber))
			//	{
			//		MessageBox.Show("Error sending." + Ramq.ErrorMessage);
			//		return;
			//	}
			//}
			////----------------------------------------------------------------------------------------
			////finally, mark the claims sent. (only if not Canadian)
			//EtransType etype = EtransType.ClaimSent;
			//if (clearinghouseClin.TypeName == ElectronicClaimFormat.Renaissance)
			//{
			//	etype = EtransType.Claim_Ren;
			//}
			////Canadians cannot send in batches (see above).  RAMQ is performing a similar algorithm but the steps are in a different order in Ramq.cs.
			//if (clearinghouseClin.TypeName != ElectronicClaimFormat.Canadian && clearinghouseClin.TypeName != ElectronicClaimFormat.Ramq)
			//{
			//	//Create the etransmessagetext that all claims in the batch will point to.
			//	EtransMessageText etransMsgText = new EtransMessageText();
			//	etransMsgText.MessageText = messageText;
			//	EtransMessageTexts.Insert(etransMsgText);
			//	for (int j = 0; j < queueItems.Count; j++)
			//	{
			//		Etrans etrans = Etranss.SetClaimSentOrPrinted(queueItems[j].ClaimNum, queueItems[j].PatNum,
			//			clearinghouseClin.HqClearinghouseNum, etype, batchNumber, Security.CurrentUser.Id);
			//		etrans.EtransMessageTextNum = etransMsgText.EtransMessageTextNum;
			//		Etranss.Update(etrans);
			//		//Now we need to update our cache of claims to reflect the change that took place in the database above in Etranss.SetClaimSentOrPrinted()
			//		queueItems[j].ClaimStatus = "S";
			//	}
			//}
		}

		/// <summary>
		/// If no comm bridge is selected for a clearinghouse, this launches any client program the user has entered. 
		/// We do not want to cause a rollback, so no return value.
		/// </summary>
		private static void AttemptLaunch(Clearinghouse clearinghouseClin, int batchNum)
		{
			if (clearinghouseClin.ClientProgram == "")
			{
				return;
			}

			if (!File.Exists(clearinghouseClin.ClientProgram))
			{
				MessageBox.Show(clearinghouseClin.ClientProgram + " does not exist.");

				return;
			}

			try
			{
				Process.Start(clearinghouseClin.ClientProgram);
			}
			catch (ODException odEx)
			{
				MessageBox.Show(odEx.Message);
			}
			catch
			{
				MessageBox.Show(
					"Client program could not be started. It may already be running. " +
					"You must open your client program to finish sending claims.");
			}
		}

		/// <summary>
		/// Fills the missing data field on the queueItem that was passed in. 
		/// This contains all missing data on this claim. 
		/// Claim will not be allowed to be sent electronically unless this string comes back empty.
		/// </summary>
		public static ClaimSendQueueItem GetMissingData(Clearinghouse clearinghouseClin, ClaimSendQueueItem queueItem)
		{
			//if (queueItem == null)
			//{
			//	return new ClaimSendQueueItem() { MissingData = "Unable to fill claim data. Please recreate claim." };
			//}
			//queueItem.Warnings = "";
			//queueItem.MissingData = "";
			//queueItem.ErrorsPreventingSave = "";
			////this is usually just the default clearinghouse or the clearinghouse for the PayorID.
			//if (clearinghouseClin == null)
			//{
			//	if (queueItem.MedType == EnumClaimMedType.Dental)
			//	{
			//		queueItem.MissingData += "No default dental clearinghouse set.";
			//	}
			//	else
			//	{
			//		queueItem.MissingData += "No default medical/institutional clearinghouse set.";
			//	}
			//	return queueItem;
			//}

			//#region Data Sanity Checking (for Replication)
			////Example: We had one replication customer who was able to delete an insurance plan for which was attached to a claim.
			////Imagine two replication servers, server A and server B.  An insplan is created which is not associated to any claims.
			////Both databases have a copy of the insplan.  The internet connection is lost.  On server A, a user deletes the insurance
			////plan (which is allowed because no claims are attached).  On server B, a user creates a claim with the insurance plan.
			////When the internet connection returns, the delete insplan statement is run on server B, which then creates a claim with
			////an invalid InsPlanNum on server B.  Without the checking below, the send claims window would crash for this one scenario.
			//Claim claim = Claims.GetClaim(queueItem.ClaimNum);//This should always exist, because we just did a select to get the queue item.
			//InsPlan insPlan = InsPlans.RefreshOne(claim.PlanNum);
			//if (insPlan == null)
			//{//Check for missing PlanNums
			//	queueItem.ErrorsPreventingSave = "Claim insurance plan record missing.  Please recreate claim.";
			//	queueItem.MissingData = "Claim insurance plan record missing.  Please recreate claim.";
			//	return queueItem;
			//}
			//if (claim.InsSubNum2 != 0)
			//{
			//	InsPlan insPlan2 = InsPlans.RefreshOne(claim.PlanNum2);
			//	if (insPlan2 == null)
			//	{//Check for missing PlanNums
			//		queueItem.MissingData = "Claim other insurance plan record missing.  Please recreate claim.";
			//		return queueItem;//This will let the office send other claims that passed validation without throwing an exception.
			//	}
			//}
			//#endregion Data Sanity Checking (for Replication)

			//if (claim.ProvTreat == 0)
			//{
			//	queueItem.MissingData = "No treating provider set.";

			//	return queueItem;
			//}

			//try
			//{
			//	if (clearinghouseClin.TypeName == ElectronicClaimFormat.x837D_4010)
			//	{
			//		X837_4010.Validate(clearinghouseClin, queueItem);//,out warnings);
			//														 //return;
			//	}
			//	else if (clearinghouseClin.TypeName == ElectronicClaimFormat.x837D_5010_dental
			//		|| clearinghouseClin.TypeName == ElectronicClaimFormat.x837_5010_med_inst)
			//	{
			//		X837_5010.Validate(clearinghouseClin, queueItem);//,out warnings);
			//														 //return;
			//	}
			//	else if (clearinghouseClin.TypeName == ElectronicClaimFormat.Renaissance)
			//	{
			//		queueItem.MissingData = Renaissance.GetMissingData(queueItem);
			//		//return;
			//	}
			//	else if (clearinghouseClin.TypeName == ElectronicClaimFormat.Canadian)
			//	{
			//		queueItem.MissingData = Canadian.GetMissingData(queueItem);
			//		//return;
			//	}
			//	else if (clearinghouseClin.TypeName == ElectronicClaimFormat.Dutch)
			//	{
			//		Dutch.GetMissingData(queueItem);//,out warnings);
			//										//return;
			//	}
			//	else if (clearinghouseClin.TypeName == ElectronicClaimFormat.Ramq)
			//	{
			//		Ramq.GetMissingData(clearinghouseClin, queueItem);
			//	}
			//}
			//catch (Exception e)
			//{
			//	queueItem.MissingData = "Unable to validate claim data: " + e.Message + " Please recreate claim.";
			//}
			return queueItem;
		}

	}
}
