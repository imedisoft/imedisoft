using CodeBase;
using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OpenDentBusiness
{
    public class Adjustments
	{
		#region Get Methods

		public static List<Adjustment> GetMany(List<long> listAdjNums)
		{
			if (listAdjNums.IsNullOrEmpty())
			{
				return new List<Adjustment>();
			}
			string command = $"SELECT * FROM adjustment WHERE adjustment.AdjNum IN ({string.Join(",", listAdjNums.Select(x => POut.Long(x)))})";
			return Crud.AdjustmentCrud.SelectMany(command).ToList();
		}

		///<summary>Gets all adjustments for a single patient.</summary>
		public static Adjustment[] Refresh(long patNum)
		{
			string command =
				"SELECT * FROM adjustment"
				+ " WHERE PatNum = " + POut.Long(patNum) + " ORDER BY AdjDate";
			return Crud.AdjustmentCrud.SelectMany(command).ToArray();
		}

		///<summary>Gets one adjustment from the db.</summary>
		public static Adjustment GetOne(long adjNum)
		{
			string command =
				"SELECT * FROM adjustment"
				+ " WHERE AdjNum = " + POut.Long(adjNum);
			return Crud.AdjustmentCrud.SelectOne(adjNum);
		}

		///<summary>Gets the amount used for the specified adjustment (Sums paysplits that have AdjNum passed in).  Pass in PayNum to exclude splits on that payment.</summary>
		public static double GetAmtAllocated(long adjNum, long excludedPayNum, List<PaySplit> listSplits = null)
		{
			if (listSplits != null)
			{
				return listSplits.FindAll(x => x.PayNum != excludedPayNum).Sum(x => x.SplitAmt);
			}
			string command = "SELECT SUM(SplitAmt) FROM paysplit WHERE AdjNum=" + POut.Long(adjNum);
			if (excludedPayNum != 0)
			{
				command += " AND PayNum!=" + POut.Long(excludedPayNum);
			}
			return Database.ExecuteDouble(command);
		}

		///<summary>Gets all adjustments for the patients passed in.</summary>
		public static List<Adjustment> GetAdjustForPats(List<long> listPatNums)
		{
			string command = "SELECT * FROM adjustment "
				+ "WHERE PatNum IN(" + String.Join(", ", listPatNums) + ") ";
			return Crud.AdjustmentCrud.SelectMany(command).ToList();
		}

		///<summary>Loops through the supplied list of adjustments and returns an ArrayList of adjustments for the given proc.</summary>
		public static ArrayList GetForProc(long procNum, Adjustment[] List)
		{
			//No need to check RemotingRole; no call to db.
			ArrayList retVal = new ArrayList();
			for (int i = 0; i < List.Length; i++)
			{
				if (List[i].ProcedureId == procNum)
				{
					retVal.Add(List[i]);
				}
			}
			return retVal;
		}

		public static List<Adjustment> GetForProcs(List<long> listProcNums)
		{
			List<Adjustment> listAdjustments = new List<Adjustment>();
			if (listProcNums == null || listProcNums.Count < 1)
			{
				return listAdjustments;
			}
			string command = "SELECT * FROM adjustment WHERE ProcNum IN(" + string.Join(",", listProcNums) + ")";
			return Crud.AdjustmentCrud.SelectMany(command).ToList();
		}

		/// <summary>Returns a DataTable of adjustments of a given adjustment type and for a given pat</summary>
		public static List<Adjustment> GetAdjustForPatByType(long patNum, long adjType)
		{
			string queryBrokenApts = "SELECT * FROM adjustment WHERE PatNum=" + POut.Long(patNum)
				+ " AND AdjType=" + POut.Long(adjType);
			return Crud.AdjustmentCrud.SelectMany(queryBrokenApts).ToList();
		}

		/// <summary>Returns a dictionary of adjustments of a given adjustment type and for the given pats such that the key is the patNum.
		/// Every patNum given will exist as key with a list in the returned dictonary.
		/// Only considers adjs where AdjDate is strictly less than the given maxAdjDate.</summary>
		public static SerializableDictionary<long, List<Adjustment>> GetAdjustForPatsByType(List<long> listPatNums, long adjType, DateTime maxAdjDate)
		{
			if (listPatNums == null || listPatNums.Count == 0)
			{
				return new SerializableDictionary<long, List<Adjustment>>();
			}
			string queryBrokenApts = "SELECT * FROM adjustment "
				+ "WHERE PatNum IN (" + string.Join(",", listPatNums) + ") "
				+ "AND AdjType=" + POut.Long(adjType) + " "
				+ "AND " + DbHelper.DateTConditionColumn("AdjDate", ConditionOperator.LessThan, maxAdjDate);
			List<Adjustment> listAdjs = Crud.AdjustmentCrud.SelectMany(queryBrokenApts).ToList();
			SerializableDictionary<long, List<Adjustment>> retVal = new SerializableDictionary<long, List<Adjustment>>();
			foreach (long patNum in listPatNums)
			{
				retVal[patNum] = listAdjs.FindAll(x => x.PatientId == patNum);
			}
			return retVal;
		}

		///<summary>Used from ContrAccount and ProcEdit to display and calculate adjustments attached to procs.</summary>
		public static double GetTotForProc(long procNum, Adjustment[] List, long excludedNum = 0)
		{
			//No need to check RemotingRole; no call to db.
			double retVal = 0;
			for (int i = 0; i < List.Length; i++)
			{
				if ((List[i].Id == excludedNum))
				{
					continue;
				}
				if (List[i].ProcedureId == procNum)
				{
					retVal += List[i].AdjustAmount;
				}
			}
			return retVal;
		}

		///<summary>Returns the sum of adjustments for the date range for the passed in operatories or providers.</summary>
		public static decimal GetAdjustAmtForAptView(DateTime dateStart, DateTime dateEnd, long clinicNum, List<long> listOpNums, List<long> listProvNums)
		{
			string command = GetQueryAdjustmentsForAppointments(dateStart, dateEnd, listOpNums, doGetSum: true);
			if (!listProvNums.IsNullOrEmpty())
			{
				command += "AND adjustment.ProvNum IN(" + string.Join(",", listProvNums.Select(x => POut.Long(x))) + ") ";
			}
			if (clinicNum > 0 && PrefC.HasClinicsEnabled)
			{
				command += "AND adjustment.ClinicNum=" + POut.Long(clinicNum);
			}
			return PIn.Decimal(Database.ExecuteString(command));
		}
		#endregion

		#region Modification Methods

		#region Insert
		///<summary></summary>
		public static long Insert(Adjustment adj)
		{
			//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
			adj.AddedByUserId = Security.CurrentUser.Id;
			long adjNum = Crud.AdjustmentCrud.Insert(adj);
			return adjNum;
		}

		///<summary>Creates a new discount adjustment for the given procedure.</summary>
		public static void CreateAdjustmentForDiscount(Procedure procedure)
		{
			//No need to check RemotingRole; no call to db.
			Adjustment AdjustmentCur = new Adjustment();
			AdjustmentCur.AddedDate = DateTime.Today;
			AdjustmentCur.AdjustDate = DateTime.Today;
			AdjustmentCur.ProcedureDate = procedure.ProcDate;
			AdjustmentCur.ProviderId = procedure.ProvNum;
			AdjustmentCur.PatientId = procedure.PatNum;
			AdjustmentCur.Type = Preferences.GetLong(PreferenceName.TreatPlanDiscountAdjustmentType);
			AdjustmentCur.ClinicId = procedure.ClinicNum;
			AdjustmentCur.AdjustAmount = -procedure.Discount;//Discount must be negative here.
			AdjustmentCur.ProcedureId = procedure.ProcNum;
			Insert(AdjustmentCur);
			TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(AdjustmentCur);
		}

		///<summary>Creates a new discount adjustment for the given procedure using the discount plan fee.</summary>
		public static void CreateAdjustmentForDiscountPlan(Procedure procedure)
		{
			//No need to check RemotingRole; no call to db.
			DiscountPlan discountPlan = DiscountPlans.GetPlan(Patients.GetPat(procedure.PatNum).DiscountPlanNum);
			if (discountPlan == null)
			{
				return;//No discount plan.
			}
			//Figure out how much the patient saved and make an adjustment for the difference so that the office find how much money they wrote off.
			double discountAmt = Fees.GetAmount(procedure.CodeNum, discountPlan.FeeSchedNum, procedure.ClinicNum, procedure.ProvNum);
			if (discountAmt == -1)
			{
				return;//No fee entered, don't make adjustment.
			}
			double adjAmt = procedure.ProcFee - discountAmt;
			if (adjAmt <= 0)
			{
				return;//We do not need to create adjustments for 0 dollars.
			}
			Adjustment adjustmentCur = new Adjustment();
			adjustmentCur.AddedDate = DateTime.Today;
			adjustmentCur.AdjustDate = DateTime.Today;
			adjustmentCur.ProcedureDate = procedure.ProcDate;
			adjustmentCur.ProviderId = procedure.ProvNum;
			adjustmentCur.PatientId = procedure.PatNum;
			adjustmentCur.Type = discountPlan.DefNum;
			adjustmentCur.ClinicId = procedure.ClinicNum;
			adjustmentCur.AdjustAmount = (-adjAmt);
			adjustmentCur.ProcedureId = procedure.ProcNum;
			Insert(adjustmentCur);
			TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(adjustmentCur);
			SecurityLogs.MakeLogEntry(Permissions.AdjustmentCreate, procedure.PatNum, "Adjustment made for discount plan: " + adjustmentCur.AdjustAmount.ToString("f"));
		}
		#endregion

		#region Update
		///<summary></summary>
		public static void Update(Adjustment adj)
		{
			Crud.AdjustmentCrud.Update(adj);
		}

		public static void DetachFromInvoice(long statementNum)
		{
			string command = "UPDATE adjustment SET StatementNum=0 WHERE StatementNum=" + POut.Long(statementNum) + "";
			Database.ExecuteNonQuery(command);
		}

		public static void DetachAllFromInvoices(List<long> listStatementNums)
		{
			if (listStatementNums == null || listStatementNums.Count == 0)
			{
				return;
			}
			string command = "UPDATE adjustment SET StatementNum=0 WHERE StatementNum IN (" + string.Join(",", listStatementNums.Select(x => POut.Long(x))) + ")";
			Database.ExecuteNonQuery(command);
		}
		#endregion

		#region Delete
		///<summary>This will soon be eliminated or changed to only allow deleting on same day as EntryDate.</summary>
		public static void Delete(Adjustment adj)
		{
			Crud.AdjustmentCrud.Delete(adj.Id);
			PaySplits.UnlinkForAdjust(adj);
		}

		///<summary>Deletes all adjustments for a procedure</summary>
		public static void DeleteForProcedure(long procNum)
		{
			//Create log for each adjustment that is going to be deleted.
			string command = "SELECT * FROM adjustment WHERE ProcNum = " + POut.Long(procNum); //query for all adjustments of a procedure 
			List<Adjustment> listAdjustments = Crud.AdjustmentCrud.SelectMany(command).ToList();
			for (int i = 0; i < listAdjustments.Count; i++)
			{ //loops through the rows
				SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit, listAdjustments[i].PatientId, //and creates audit trail entry for every row to be deleted
				"Delete adjustment for patient: "
				+ Patients.GetLim(listAdjustments[i].PatientId).GetNameLF() + ", "
				+ (listAdjustments[i].AdjustAmount).ToString("c"), 0, listAdjustments[i].LastModifiedDate);
			}
			//Delete each adjustment for the procedure.
			command = "DELETE FROM adjustment WHERE ProcNum = " + POut.Long(procNum);
			Database.ExecuteNonQuery(command);
		}
		#endregion
		#endregion

		#region Misc Methods
		///<summary>Returns the number of finance or billing charges deleted.</summary>
		public static long UndoFinanceOrBillingCharges(DateTime dateUndo, bool isBillingCharges)
		{
			string adjTypeStr = "Finance";
			long adjTypeDefNum = Preferences.GetLong(PreferenceName.FinanceChargeAdjustmentType);
			if (isBillingCharges)
			{
				adjTypeStr = "Billing";
				adjTypeDefNum = Preferences.GetLong(PreferenceName.BillingChargeAdjustmentType);
			}
			string command = "SELECT adjustment.AdjAmt,patient.PatNum,patient.Guarantor,patient.LName,patient.FName,patient.Preferred,patient.MiddleI,"
				+ "adjustment.SecDateTEdit "
				+ "FROM adjustment "
				+ "INNER JOIN patient ON patient.PatNum=adjustment.PatNum "
				+ "WHERE AdjDate=" + POut.Date(dateUndo) + " "
				+ "AND AdjType=" + POut.Long(adjTypeDefNum);
			DataTable table = Database.ExecuteDataTable(command);
			List<Action> listActions = new List<Action>();
			int loopCount = 0;
			foreach (DataRow row in table.Rows)
			{//loops through the rows and creates audit trail entry for every row to be deleted
				listActions.Add(new Action(() =>
				{
					SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit, PIn.Long(row["PatNum"].ToString()),
						"Delete adjustment for patient, undo " + adjTypeStr.ToLower() + " charges: "
						+ Patients.GetNameLF(row["LName"].ToString(), row["FName"].ToString(), row["Preferred"].ToString(), row["MiddleI"].ToString())
						+ ", " + PIn.Double(row["AdjAmt"].ToString()).ToString("c"), 0, PIn.Date(row["SecDateTEdit"].ToString()));
					if (++loopCount % 5 == 0)
					{
						BillingEvent.Fire(EventCategory.Billing, "Creating log entries for " + adjTypeStr.ToLower() + " charges"
							+ ": " + loopCount + " out of " + table.Rows.Count);
					}
				}));
			}
			ODThread.RunParallel(listActions, TimeSpan.FromMinutes(2));
			command = "DELETE FROM adjustment WHERE AdjDate=" + POut.Date(dateUndo) + " AND AdjType=" + POut.Long(adjTypeDefNum);
			BillingEvent.Fire(EventCategory.Billing, "Deleting" + " " + table.Rows.Count + " "
				+  adjTypeStr.ToLower() + " charge adjustments" + "...");
			return Database.ExecuteNonQuery(command);
		}

		///<summary>Returns a query string used to get adjustments for all patients who have an appointment in the date range and in one of the operatories
		///passed in.</summary>
		public static string GetQueryAdjustmentsForAppointments(DateTime dateStart, DateTime dateEnd, List<long> listOpNums, bool doGetSum)
		{
			//No need to check RemotingRole; no call to db.
			if (listOpNums.IsNullOrEmpty())
			{
				return "SELECT " + (doGetSum ? "SUM(adjustment.AdjAmt)" : "*")
					+ " FROM adjustment WHERE AdjDate BETWEEN " + POut.Date(dateStart) + " AND " + POut.Date(dateEnd) + " ";
			}
			string command = "SELECT "
				+ (doGetSum ? "SUM(adjustment.AdjAmt)" : "*")
				+ " FROM adjustment WHERE AdjDate BETWEEN " + POut.Date(dateStart) + " AND " + POut.Date(dateEnd)
					+ " AND PatNum IN("
						+ "SELECT PatNum FROM appointment "
							+ "WHERE AptDateTime BETWEEN " + POut.Date(dateStart) + " AND " + POut.Date(dateEnd.AddDays(1))
							+ "AND AptStatus IN (" + POut.Int((int)ApptStatus.Scheduled)
							+ ", " + POut.Int((int)ApptStatus.Complete)
							+ ", " + POut.Int((int)ApptStatus.Broken)
							+ ", " + POut.Int((int)ApptStatus.PtNote)
							+ ", " + POut.Int((int)ApptStatus.PtNoteCompleted) + ")"
							+ " AND Op IN(" + string.Join(",", listOpNums) + ")) ";
			return command;
		}

		#endregion
	}
}
