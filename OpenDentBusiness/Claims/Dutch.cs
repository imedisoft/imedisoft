using Imedisoft.Claims;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenDentBusiness.Eclaims
{
	public sealed class Dutch : ClaimBridge
	{
		public Dutch() : base("Dutch")
		{
		}

		protected override bool OnSend(Clearinghouse clearingHouse, long batchNumber, List<ClaimSendQueueItem> queueItems, EnumClaimMedType medType)
		{
			for (int i = 0; i < queueItems.Count; i++)
			{
				if (!CreateClaim(clearingHouse, queueItems[i]))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Called once for each claim to be created. 
		/// For claims with a lot of procedures, this may actually create multiple claims.  
		/// Normally returns empty string unless something went wrong.
		/// </summary>
		public static bool CreateClaim(Clearinghouse clearingHouse, ClaimSendQueueItem queueItem)
		{
			var stringBuilder = new StringBuilder();


			Claim claim = Claims.GetClaim(queueItem.ClaimNum);
			Provider provBill = Providers.GetById(claim.ProvBill);
			Patient pat = Patients.GetPat(claim.PatNum);
			InsurancePlan insplan = InsPlans.GetPlan(claim.PlanNum, new List<InsurancePlan>());
			InsSub insSub = InsSubs.GetSub(claim.InsSubNum, new List<InsSub>());
			Carrier carrier = Carriers.GetCarrier(insplan.CarrierId);
			List<ClaimProc> claimProcList = ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcsForClaim = ClaimProcs.GetForSendClaim(claimProcList, claim.ClaimNum);
			List<Procedure> procList = Procedures.Refresh(claim.PatNum);
			Procedure proc;
			ProcedureCode procCode;

			string t = "\t";

			stringBuilder.Append("110\t111\t112\t118\t203/403\tF108/204/404\t205/405\t206\t207\t208\t209\t210\t211\t212\t215\t217\t219\t406\t408\t409\t410\t411\t413\t414\t415\t416\t418\t419\t420\t421\t422\t423\t424\t425\t426\t428\t429\t430\t432\t433\r\n");

			for (int i = 0; i < claimProcsForClaim.Count; i++)
			{
				//claimProcsForClaim already excludes any claimprocs with ProcNum=0, so no payments etc.
				proc = Procedures.GetProcFromList(procList, claimProcsForClaim[i].ProcNum);
				//procCode=Pro
				stringBuilder.Append(provBill.SSN + t);//110
				stringBuilder.Append(provBill.MedicaidID + t);//111
				stringBuilder.Append(t);//112
				stringBuilder.Append(t);//118
				stringBuilder.Append(pat.SSN + t);//203/403
				stringBuilder.Append(carrier.Name + t);//carrier name?
				stringBuilder.Append(insSub.SubscriberID + t);
				stringBuilder.Append(pat.PatNum.ToString() + t);
				stringBuilder.Append(pat.Birthdate.ToString("dd-MM-yyyy") + t);
				if (pat.Gender == PatientGender.Female)
				{
					stringBuilder.Append("2" + t);
				}
				else
				{
					stringBuilder.Append("1" + t);
				}
				stringBuilder.Append("1" + t);
				stringBuilder.Append(DutchLName(pat.LName) + t);//last name without prefix
				stringBuilder.Append(DutchLNamePrefix(pat.LName) + t);//prefix
				stringBuilder.Append("2" + t);
				stringBuilder.Append(DutchInitials(pat) + t);//215. initials
				stringBuilder.Append(pat.Zip + t);
				stringBuilder.Append(DutchAddressNumber(pat.Address) + t);//219 house number.  Already validated.
				stringBuilder.Append(t);
				stringBuilder.Append(proc.ProcDate.ToString("dd-MM-yyyy") + t);//procDate
				procCode = ProcedureCodes.GetById(proc.CodeNum);
				string strProcCode = procCode.Code;
				if (strProcCode.EndsWith("00"))
				{//ending with 00 indicates it's a lab code.
					stringBuilder.Append("02" + t);
				}
				else
				{
					stringBuilder.Append("01" + t);//409. Procedure code (01) or lab costs (02)
				}
				stringBuilder.Append(t);
				stringBuilder.Append(t);
				stringBuilder.Append(strProcCode + t);
				stringBuilder.Append(GetUL(proc, procCode) + t);//414. U/L.
				stringBuilder.Append(Tooth.ToInternat(proc.ToothNum) + t);
				stringBuilder.Append(Tooth.SurfTidyForClaims(proc.Surf, proc.ToothNum) + t);//needs validation
				stringBuilder.Append(t);
				if (claim.AccidentRelated == "")
				{//not accident
					stringBuilder.Append("N" + t);
				}
				else
				{
					stringBuilder.Append("J" + t);
				}
				stringBuilder.Append(pat.SSN + t);
				stringBuilder.Append(t);
				stringBuilder.Append(t);
				stringBuilder.Append(t);
				stringBuilder.Append(proc.ProcFee.ToString("F") + t);
				stringBuilder.Append("1" + t);
				stringBuilder.Append(proc.ProcFee.ToString("F") + t);
				stringBuilder.Append(t);
				stringBuilder.Append(t);
				stringBuilder.Append(proc.ProcFee.ToString("F") + t);
				stringBuilder.Append(t);
				stringBuilder.Append(t);
				stringBuilder.Append("\r\n");
			}

			string exportPath = clearingHouse.ExportPath;

			if (!Directory.Exists(exportPath))
			{
				try
				{
					Directory.CreateDirectory(exportPath);
				}
				catch (Exception exception)
				{
					MessageBox.Show("Failed to create directory '" + exportPath + "'. " + exception.Message);

					return false;
				}
			}

			string saveFile = Path.Combine(exportPath, "claims" + claim.ClaimNum.ToString() + ".txt");

			File.WriteAllText(saveFile, stringBuilder.ToString());

			return true;
		}

		/// <summary>
		/// Returns only the portion of the LName not including the prefix
		/// </summary>
		public static string DutchLName(string lastName)
		{
			// eg. Berg, van der

			var p = lastName.IndexOf(',');

			return p == -1 ?
				lastName :
				lastName.Substring(0, p);
		}

		/// <summary>
		/// Returns only the prefix of the LName
		/// </summary>
		public static string DutchLNamePrefix(string fullLName)
		{
			//eg. Berg, van der
			if (!fullLName.Contains(","))
			{
				return "";
			}

			if (fullLName.EndsWith(","))
			{
				return "";
			}

			string retVal = fullLName.Substring(fullLName.IndexOf(",") + 1);// van der
			retVal.TrimStart(' ');
			return retVal;
		}

		public static string DutchInitials(Patient pat)
		{
			string[] arrayFirstNames = pat.FName.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
			string retVal = "";

			for (int i = 0; i < arrayFirstNames.Length; i++)
			{
				retVal += arrayFirstNames[i].Substring(0, 1).ToUpper() + ".";
			}

			if (pat.MiddleI != "")
			{
				retVal += pat.MiddleI.Substring(0, 1).ToUpper() + ".";
			}

			return retVal;
		}

		/// <summary>
		/// Returns only the house number portion of the address. 
		/// Expects a number. 
		/// Already validated that some text came before the number.
		/// </summary>
		public static string DutchAddressNumber(string address)
		{
			Match match = Regex.Match(address, "[0-9]+");//find the first group of numbers
			return match.Value;
		}

		/// <summary>
		/// Returns either 0,1,or 2
		/// </summary>
		public static string GetUL(Procedure procedure, ProcedureCode procedureCode)
		{
			if (procedureCode.TreatmentArea == ProcedureTreatmentArea.Arch)
			{
				if (procedure.Surf == "U")
					return "1";

				if (procedure.Surf == "L")
					return "2";


				return "0"; // should never happen
			}
			else
			{
				return "0";
			}
		}

		/// <summary>
		/// Returns a string describing all missing data on this claim. 
		/// Claim will not be allowed to be sent electronically unless this string comes back empty. 
		/// There is also an out parameter containing any warnings. 
		/// Warnings will not block sending.
		/// </summary>
		public static void GetMissingData(ClaimSendQueueItem queueItem)
		{
			var claim = Claims.GetClaim(queueItem.ClaimNum);
			var patient = Patients.GetPat(claim.PatNum);

			if (!Regex.IsMatch(patient.Address, @"^[a-zA-Z ]+[0-9]+")) // Format must be streetname, then some numbers, then anything else.
			{
				queueItem.MissingData = "Patient address format";
			}

			queueItem.Warnings = "";
		}
	}
}
