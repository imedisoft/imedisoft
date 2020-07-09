using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ScheduledProcesses{
		
		//Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		#region Get Methods
		///<summary></summary>
		public static List<ScheduledProcess> Refresh(){
			
			string command="SELECT * FROM scheduledprocess";
			return Crud.ScheduledProcessCrud.SelectMany(command);
		}

		#endregion Get Methods
		
		#region Modification Methods
		#region Insert
		///<summary></summary>
		public static long Insert(ScheduledProcess scheduledProcess){
			
			return Crud.ScheduledProcessCrud.Insert(scheduledProcess);
		}
		#endregion Insert
		
		#region Update
		///<summary></summary>
		public static void Update(ScheduledProcess scheduledProcess,ScheduledProcess oldScheduledProcess){
			
			Crud.ScheduledProcessCrud.Update(scheduledProcess,oldScheduledProcess);
		}
		#endregion Update
		
		#region Delete
		///<summary></summary>
		public static void Delete(long scheduledProcessNum) {
			
			Crud.ScheduledProcessCrud.Delete(scheduledProcessNum);
		}
		#endregion Delete
	
		#endregion Modification Methods
		#region Misc Methods
		/// <summary>Returns a list of all scheduled actions with a matching Action type, Frequency to run, and TimeToRun as those passed in.</summary>
		public static List<ScheduledProcess> CheckAlreadyScheduled(ScheduledActionEnum schedActionEnum, FrequencyToRunEnum freqToRunEnum, 
			DateTime timeToRun) 
		{
			string command=$@"SELECT * FROM scheduledprocess 
				WHERE ScheduledAction='{POut.String(schedActionEnum.ToString())}' AND 
				FrequencyToRun='{POut.String(freqToRunEnum.ToString())}' AND 
				TIME(TimeToRun)=TIME({POut.DateT(timeToRun)}) ";
			return Crud.ScheduledProcessCrud.SelectMany(command);
		}

		#endregion Misc Methods
	}
}