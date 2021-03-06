using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using CodeBase;
using Imedisoft.Data;

namespace OpenDentBusiness{
	///<summary></summary>
	public class TaskUnreads{
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion


		///<summary></summary>
		public static long Insert(TaskUnread taskUnread){
			
			return Crud.TaskUnreadCrud.Insert(taskUnread);
		}

		///<summary>Batch inserts one TaskUnread for every entry in listTasks.
		///Does not validate if the tasks were previously unread or not.  Do not use this method if caller has not already validated that inserting many
		///TaskUnreads will not create duplicates.  All values in listTask will have IsUnread set true.</summary>
		public static void InsertManyForTasks(List<Task> listTasks,long currUserNum) {
			if(listTasks.IsNullOrEmpty() || currUserNum==0) {
				//Do not insert any TaskUnreads if none given or invalid usernum.
				return;
			}
			
			List<TaskUnread> listUnreads=new List<TaskUnread>();
			foreach(Task task in listTasks) {
				listUnreads.Add(new TaskUnread(){ 
					TaskId=task.Id,
					UserId=currUserNum
				});
				task.IsUnread=true;
			}
			Crud.TaskUnreadCrud.InsertMany(listUnreads);
		}

		///<summary>Sets a task read by a user by deleting all the matching taskunreads.  Quick and efficient to run any time.</summary>
		public static void SetRead(long userNum,params Task[] arrayTasks) {
			if(arrayTasks==null || arrayTasks.Length==0) {
				return;
			}
			foreach(Task task in arrayTasks) {
				task.IsUnread=false;
			}
			
			string command="DELETE FROM taskunread WHERE UserNum = "+POut.Long(userNum)+" "
				+"AND TaskNum IN ("+string.Join(",",arrayTasks.Select(x => POut.Long(x.Id)))+")";
			Database.ExecuteNonQuery(command);
		}

		public static bool AddUnreads(Task task,long curUserNum) {
			
			//if the task is done, don't add unreads
			string command = "SELECT TaskStatus,UserNum,ReminderGroupId,DateTimeEntry,NOW() DbTime "
				+"FROM task WHERE TaskNum = "+POut.Long(task.Id);
			DataTable table=Database.ExecuteDataTable(command);
			if(table.Rows.Count==0) {
				return task.IsUnread;//only happens when a task was deleted by one user but left open on another user's computer.
			}
			TaskStatus taskStatus=(TaskStatus)PIn.Int(table.Rows[0]["TaskStatus"].ToString());
			long userNumOwner=PIn.Long(table.Rows[0]["UserNum"].ToString());
			if(taskStatus==TaskStatus.Done) {
				return task.IsUnread;
			}
			//Set it unread for the original owner of the task.
			if(userNumOwner!=curUserNum) {//but only if it's some other user
				SetUnread(userNumOwner,task);
			}
			//Set it for this user if a future repeating task, so it will be new when "due".  Doing this here so we don't check every row below.
			//Only for future dates because we don't want to mark as new if it was already "due" and you added a note or something.
			if((PIn.String(table.Rows[0]["ReminderGroupId"].ToString())!="")//Is a reminder
				&& (PIn.Date(table.Rows[0]["DateTimeEntry"].ToString())>PIn.Date(table.Rows[0]["DbTime"].ToString())))//Is "due" in the future by DbTime 
			{
				SetUnread(curUserNum,task);//Set unread for current user only, other users dealt with below.
			}
			//Then, for anyone subscribed
			long userNum;
			bool isUnread;
			//task subscriptions are not cached yet, so we use a query.
			//Get a list of all subscribers to this task
			command=@"SELECT 
									tasksubscription.UserNum,
									(CASE WHEN taskunread.UserNum IS NULL THEN 0 ELSE 1 END) IsUnread
								FROM tasksubscription
								INNER JOIN tasklist ON tasksubscription.TaskListNum = tasklist.TaskListNum 
								INNER JOIN taskancestor ON taskancestor.TaskListNum = tasklist.TaskListNum 
									AND taskancestor.TaskNum = "+POut.Long(task.Id)+" ";
			command+="LEFT JOIN taskunread ON taskunread.UserNum = tasksubscription.UserNum AND taskunread.TaskNum=taskancestor.TaskNum";
			table=Database.ExecuteDataTable(command);
			List<long> listUserNums=new List<long>();
			for(int i=0;i<table.Rows.Count;i++) {
				userNum=PIn.Long(table.Rows[i]["UserNum"].ToString());
				isUnread=PIn.Bool(table.Rows[i]["IsUnread"].ToString());
				if(userNum==userNumOwner//already set
					|| userNum==curUserNum//If the current user is subscribed to this task. User has obviously already read it.
					|| listUserNums.Contains(userNum)
					|| isUnread) //Unread currently exists
				{
					continue;
				}
				listUserNums.Add(userNum);
			}
			SetUnreadMany(listUserNums,task);//This no longer results in duplicates like it used to
			return task.IsUnread;
		}

		public static bool IsUnread(long userNum,Task task) {
			
			task.IsUnread=true;
			string command="SELECT COUNT(*) FROM taskunread WHERE UserNum = "+POut.Long(userNum)+" "
				+"AND TaskNum = "+POut.Long(task.Id);
			if(Database.ExecuteString(command)=="0") {
				task.IsUnread=false;
			}
			return task.IsUnread;
		}

		///<summary>Sets unread for a single user.  Works well without duplicates, whether it's already set to Unread(new) or not.</summary>
		public static void SetUnread(long userNum,Task task) {
			if(IsUnread(userNum,task)) {
				return;//Already set to unread, so nothing else to do
			}
			TaskUnread taskUnread=new TaskUnread();
			taskUnread.TaskId=task.Id;
			taskUnread.UserId=userNum;
			task.IsUnread=true;
			Insert(taskUnread);
		}
		
		///<summary>Sets unread for a list of users.  This assumes that the list passed in has already checked for duplicate task unreads.</summary>
		public static bool SetUnreadMany(List<long> listUserNums,Task task) {
			
			List<TaskUnread> listUnreadsToInsert=new List<TaskUnread>();
			foreach(long userNum in listUserNums) {
				TaskUnread taskUnread=new TaskUnread();
				taskUnread.TaskId=task.Id;
				taskUnread.UserId=userNum;
				listUnreadsToInsert.Add(taskUnread);
			}
			Crud.TaskUnreadCrud.InsertMany(listUnreadsToInsert);
			if(listUserNums.Contains(Security.CurrentUser.Id)) {//The IsUnread flag is only used for local refreshes.
				task.IsUnread=true;
			}
			return task.IsUnread;
		}

		public static void DeleteForTask(Task task) {
			
			string command="DELETE FROM taskunread WHERE TaskNum = "+POut.Long(task.Id);
			Database.ExecuteNonQuery(command);
		}



	}
}