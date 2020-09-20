using DataConnectionBase;
using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace OpenDentBusiness
{
    public class TrojanQueries
	{
		public static DateTime GetMaxProcedureDate(long PatNum) 
			=> Database.ExecuteDateTime(
				$"SELECT MAX(ProcDate) FROM procedurelog,patient " +
				$"WHERE patient.PatNum=procedurelog.PatNum " +
				$"AND procedurelog.ProcStatus={(int)ProcStat.C} " +
				$"AND patient.Guarantor={PatNum}") ?? DateTime.MinValue;

		public static DateTime GetMaxPaymentDate(long PatNum) 
			=> Database.ExecuteDateTime(
				$"SELECT MAX(DatePay) FROM paysplit,patient " +
				$"WHERE patient.PatNum=paysplit.PatNum " +
				$"AND patient.Guarantor={PatNum}") ?? DateTime.MinValue;
		

		/// <summary>
		/// Increments the PreviousFileNumber program property to the next available int and returns that new file number.
		/// </summary>
		public static int GetUniqueFileNum()
		{
			long progNum = Programs.GetProgramNum(ProgramName.TrojanExpressCollect);

			int fileNum = SIn.Int(ProgramProperties.GetValFromDb(progNum, "PreviousFileNumber"), false) + 1;
			while (ProgramProperties.SetProperty(progNum, "PreviousFileNumber", fileNum.ToString()) < 1)
			{
				fileNum++;
			}

			return fileNum;
		}

		/// <summary>
		/// Get the list of records for the pending plan deletion report for plans that need to be brought to the patient's attention.
		/// </summary>
		public static DataTable GetPendingDeletionTable(Collection<string[]> deletePatientRecords)
		{
			string whereTrojanID = "";
			for (int i = 0; i < deletePatientRecords.Count; i++)
			{
				if (i > 0)
				{
					whereTrojanID += "OR ";
				}

				whereTrojanID += "i.TrojanID='" + deletePatientRecords[i][0] + "' ";
			}

			string command = "SELECT DISTINCT " +
					"p.FName," +
					"p.LName," +
					"p.FName," +
					"p.LName," +
					"p.SSN," +
					"p.Birthdate," +
					"i.GroupNum," +
					"s.SubscriberID," +
					"i.TrojanID," +
					"CASE i.EmployerNum WHEN 0 THEN '' ELSE e.EmpName END," +
					"CASE i.EmployerNum WHEN 0 THEN '' ELSE e.Phone END," +
					"c.CarrierName," +
					"c.Phone " +
					"FROM patient p,insplan i,employer e,carrier c,inssub s " +
					"WHERE p.PatNum=s.Subscriber AND " +
					"(" + whereTrojanID + ") AND " +
					"i.CarrierNum=c.CarrierNum AND " +
					"s.PlanNum=i.PlanNum AND " +
					"(i.EmployerNum=e.EmployerNum OR i.EmployerNum=0) AND " +
					"(SELECT COUNT(*) FROM patplan a WHERE a.PatNum=p.PatNum AND a.InsSubNum=s.InsSubNum) > 0 " +
					"ORDER BY i.TrojanID,p.LName,p.FName";

			return Database.ExecuteDataTable(command);
		}

		/// <summary>
		/// Get the list of records for the pending plan deletion report for plans which need to be bought to Trojan's attention.
		/// </summary>
		public static DataTable GetPendingDeletionTableTrojan(Collection<string[]> deleteTrojanRecords)
		{
			string whereTrojanID = "";
			for (int i = 0; i < deleteTrojanRecords.Count; i++)
			{
				if (i > 0)
				{
					whereTrojanID += "OR ";
				}
				whereTrojanID += "i.TrojanID='" + deleteTrojanRecords[i][0] + "' ";
			}
			string command = "SELECT DISTINCT " +
					"p.FName," +
					"p.LName," +
					"p.FName," +
					"p.LName," +
					"p.SSN," +
					"p.Birthdate," +
					"i.GroupNum," +
					"s.SubscriberID," +
					"i.TrojanID," +
					"CASE i.EmployerNum WHEN 0 THEN '' ELSE e.EmpName END," +
					"CASE i.EmployerNum WHEN 0 THEN '' ELSE e.Phone END," +
					"c.CarrierName," +
					"c.Phone " +
					"FROM patient p,insplan i,employer e,carrier c,inssub s " +
					"WHERE p.PatNum=s.Subscriber AND " +
					"(" + whereTrojanID + ") AND " +
					"i.CarrierNum=c.CarrierNum AND " +
					"s.PlanNum=i.PlanNum AND " +
					"(i.EmployerNum=e.EmployerNum OR i.EmployerNum=0) AND " +
					"(SELECT COUNT(*) FROM patplan a WHERE a.PatNum=p.PatNum AND a.InsSubNum=s.InsSubNum) > 0 " +
					"ORDER BY i.TrojanID,p.LName,p.FName";
			return Database.ExecuteDataTable(command);
		}

		public static InsurancePlan GetPlanWithTrojanID(string trojanID) 
			=> Crud.InsPlanCrud.SelectOne("SELECT * FROM insplan WHERE TrojanID = '" + SOut.String(trojanID) + "'");

		public static void UpdatePlan(TrojanObject troj, long planNum, bool updateBenefits)
		{
			long employerNum = Employers.GetEmployerNum(troj.ENAME);

			Database.ExecuteNonQuery(
				"UPDATE insplan SET " +
					"EmployerNum=" + SOut.Long(employerNum) + ", " +
					"GroupName='" + SOut.String(troj.PLANDESC) + "', " +
					"GroupNum='" + SOut.String(troj.POLICYNO) + "', " +
					"CarrierNum= " + SOut.Long(troj.CarrierNum) + " " +
				"WHERE PlanNum=" + SOut.Long(planNum));

			Database.ExecuteNonQuery("UPDATE inssub SET BenefitNotes='" + SOut.String(troj.BenefitNotes) + "' HERE PlanNum=" + SOut.Long(planNum));

			if (updateBenefits)
			{
				Database.ExecuteNonQuery("DELETE FROM benefit WHERE PlanNum=" + SOut.Long(planNum));

				for (int j = 0; j < troj.BenefitList.Count; j++)
				{
					troj.BenefitList[j].PlanNum = planNum;

					Benefits.Insert(troj.BenefitList[j]);
				}

				InsPlans.ComputeEstimatesForTrojanPlan(planNum);
			}
		}
	}

	/// <summary>
	/// This is used as a container for plan and benefit info coming in from Trojan.
	/// </summary>
	[Serializable]
	public class TrojanObject
	{
		///<summary>TrojanID</summary>
		public string TROJANID;
		///<summary>Employer name</summary>
		public string ENAME;
		///<summary>GroupName</summary>
		public string PLANDESC;
		///<summary>Carrier phone</summary>
		public string ELIGPHONE;
		///<summary>GroupNum</summary>
		public string POLICYNO;
		///<summary>Accepts eclaims</summary>
		public bool ECLAIMS;
		///<summary>ElectID</summary>
		public string PAYERID;
		///<summary>CarrierName</summary>
		public string MAILTO;
		///<summary>Address</summary>
		public string MAILTOST;
		///<summary>City</summary>
		public string MAILCITYONLY;
		///<summary>State</summary>
		public string MAILSTATEONLY;
		///<summary>Zip</summary>
		public string MAILZIPONLY;
		///<summary>The only thing that will be missing from these benefits is the PlanNum.</summary>
		public List<Benefit> BenefitList;
		///<summary>This can be filled at some point based on the carrier fields.</summary>
		public long CarrierNum;

		public string BenefitNotes;
		public string PlanNote;
		public int MonthRenewal;
	}
}
