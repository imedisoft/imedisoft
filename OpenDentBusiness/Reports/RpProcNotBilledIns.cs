using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpProcNotBilledIns {
		///<summary>If not using clinics then supply an empty list of clinicNums.  listClinicNums must have at least one item if using clinics.
		///The table returned has the following columns in this order: 
		///PatientName, ProcDate, Descript, ProcFee, ProcNum, ClinicNum, PatNum, IsInProcess</summary>
		public static DataTable GetProcsNotBilled(List<long> listClinicNums,bool includeMedProcs,DateTime dateStart,DateTime dateEnd,
			bool showProcsBeforeIns,bool hasMultiVisitProcs)
		{
			string query="SELECT ";
			if(Preferences.GetBool(PreferenceName.ReportsShowPatNum)) {
				query+=DbHelper.Concat("CAST(patient.PatNum AS CHAR)","'-'","patient.LName","', '","patient.FName","' '","patient.MiddleI");
			}
			else {
				query+=DbHelper.Concat("patient.LName","', '","patient.FName","' '","patient.MiddleI");
			}
			query+=$@" PatientName,
				CASE WHEN procmultivisit.ProcMultiVisitNum IS NULL THEN '{ProcStat.C}'
					ELSE '{ProcStatExt.InProcess}' END Stat,
				procedurelog.ProcDate,'' Descript,
				procedurelog.ProcFee*(procedurelog.UnitQty+procedurelog.BaseUnits) procFee,
				procedurelog.ProcNum,procedurelog.ClinicNum,patient.PatNum,procedurelog.CodeNum
				FROM patient
				INNER JOIN procedurelog ON procedurelog.PatNum=patient.PatNum
					AND procedurelog.ProcFee>0
					AND procedurelog.procstatus={(int)ProcStat.C}
					AND procedurelog.ProcDate	BETWEEN {POut.Date(dateStart)} AND {POut.Date(dateEnd)}
				LEFT JOIN claimproc ON claimproc.ProcNum=procedurelog.ProcNum
				LEFT JOIN insplan ON insplan.PlanNum=claimproc.PlanNum
				LEFT JOIN procmultivisit ON procmultivisit.ProcNum=procedurelog.ProcNum
					AND procmultivisit.IsInProcess=1
				WHERE EXISTS(SELECT 1 FROM patplan WHERE patplan.PatNum=patient.PatNum)
				AND (
					(claimproc.NoBillIns=0 AND claimproc.Status={(int)ClaimProcStatus.Estimate}){(!showProcsBeforeIns?"":$@"
					OR claimproc.ClaimProcNum IS NULL")}
				){(hasMultiVisitProcs?"":$@"
				AND procmultivisit.ProcMultiVisitNum IS NULL")}{(listClinicNums.Count<1?"":$@"
				AND procedurelog.ClinicNum IN ({string.Join(",",listClinicNums)})")}
				GROUP BY procedurelog.ProcNum
				HAVING !MIN(insplan.IsMedical){(includeMedProcs?" OR MAX(insplan.IsMedical)":"")}{(showProcsBeforeIns?" OR ISNULL(MIN(insplan.PlanNum))":"")}
				ORDER BY patient.LName,patient.FName,patient.PatNum,procedurelog.ProcDate";
			DataTable table=Database.ExecuteDataTable(query);
			Dictionary<long,ProcedureCode> dictProcCodes=ProcedureCodes.GetListDeep().ToDictionary(x => x.Id);
			foreach(DataRow rawRow in table.Select()) {
				if(!dictProcCodes.TryGetValue(PIn.Long(rawRow["CodeNum"].ToString()),out ProcedureCode procCode)) {
					table.Rows.Remove(rawRow);
					continue;
				}
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && procCode.IsCanadianLab) {//ignore Canadian labs
					table.Rows.Remove(rawRow);
					continue;
				}
				rawRow["Descript"]=procCode.Description??"";
			}
			return table;
		}

	}
}
