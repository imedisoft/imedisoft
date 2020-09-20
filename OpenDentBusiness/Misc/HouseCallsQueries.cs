using DataConnectionBase;
using Imedisoft.Data;
using System;
using System.Data;

namespace OpenDentBusiness
{
    public class HouseCallsQueries
	{
		public static DataTable GetHouseCalls(DateTime FromDate, DateTime ToDate) 
			=> Database.ExecuteDataTable(
				"SELECT patient.LName,patient.FName,patient.Preferred,patient.PatNum,patient.ChartNumber," +
				"patient.HmPhone,patient.WkPhone,patient.Email,patient.Address,patient.Address2," +
				"patient.City,patient.State,patient.Zip,appointment.AptDateTime,appointment.ProcDescript,patient.PriProv," +
				"appointment.IsNewPatient,patient.WirelessPhone " +
				"FROM patient,appointment " +
				"WHERE patient.PatNum=appointment.PatNum " +
				"AND (appointment.AptStatus=1 OR appointment.AptStatus=4) " + // sched or ASAP
				"AND appointment.AptDateTime > " + SOut.Date(FromDate) + " " + // > midnight
				"AND appointment.AptDateTime < " + SOut.Date(ToDate.AddDays(1))); // < midnight;
	}
}
