using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ScheduleOps {
		#region Get Methods
		///<summary></summary>
		public static List<ScheduleOp> GetForSched(long scheduleNum) {
			
			string command="SELECT * FROM scheduleop ";
			command+="WHERE schedulenum = "+scheduleNum;
			return Crud.ScheduleOpCrud.SelectMany(command);
		}

		///<summary>Gets all the ScheduleOps for the list of schedules.</summary>
		public static List<ScheduleOp> GetForSchedList(List<Schedule> schedules) {
			
			if(schedules==null || schedules.Count==0) {
				return new List<ScheduleOp>();
			}
			string command="SELECT * FROM scheduleop WHERE ScheduleNum IN ("+string.Join(",",schedules.Select(x=>x.ScheduleNum))+")";
			return Crud.ScheduleOpCrud.SelectMany(command);
		}

		///<summary>Gets all the ScheduleOps for the list of schedules.  Only returns ScheduleOps for the list of operatories passed in.
		///Necessary in the situation that a provider has two operatories but only one schedule that is assigned to both operatories.</summary>
		public static List<ScheduleOp> GetForSchedList(List<Schedule> schedules,List<long> listOpNums) {
			
			if(schedules==null || schedules.Count==0 || listOpNums==null || listOpNums.Count==0) {
				return new List<ScheduleOp>();
			}
			string command="SELECT * FROM scheduleop "
				+"WHERE ScheduleNum IN ("+string.Join(",",schedules.Select(x => POut.Long(x.ScheduleNum)))+") "
				+"AND OperatoryNum IN ("+string.Join(",",listOpNums.Select(x => POut.Long(x)))+")";
			return Crud.ScheduleOpCrud.SelectMany(command);
		}
		#endregion

		#region Modification Methods
		
		#region Insert
		///<summary></summary>
		public static long Insert(ScheduleOp op) {
			
			return Crud.ScheduleOpCrud.Insert(op);
		}
		#endregion

		#region Update
		#endregion

		#region Delete
		public static void DeleteBatch(List<long> listSchedOpNums) {
			
			if(listSchedOpNums==null || listSchedOpNums.Count==0) {
				return;
			}
			string command = "DELETE FROM scheduleop WHERE ScheduleOpNum IN ("+string.Join(",",listSchedOpNums)+")";
			Db.NonQ(command);
		}
		#endregion

		#endregion

		#region Misc Methods
		#endregion



	}

}













