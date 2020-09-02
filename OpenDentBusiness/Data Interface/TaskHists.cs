using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public class TaskHists
	{
		public static string GetChangesDescription(TaskHistory taskCur, TaskHistory taskNext)
		{
			if (taskCur.Description.StartsWith("This task was cut from task list ") || 
				taskCur.Description.StartsWith("This task was copied from task "))
			{
				return taskCur.Description;
			}

			if (!taskCur.DateStart.HasValue) return "New task.";

			var stringBuilder = new StringBuilder();

			if (taskNext.TaskListId != taskCur.TaskListId)
			{
				string descOne = "(DELETED)";
				string descTwo = "(DELETED)";

				var taskList = TaskLists.GetOne(taskCur.TaskListId);
				if (taskList != null)
				{
					descOne = taskList.Description;
				}

				taskList = TaskLists.GetOne(taskNext.TaskListId);
				if (taskList != null)
				{
					descTwo = taskList.Description;
				}

				stringBuilder.AppendLine(
					$"Task list changed from {descOne} to {descTwo}.");
			}

			if (taskNext.PatientId != taskCur.PatientId)
			{
				stringBuilder.AppendLine(
					"Task patient attachment changed.");
			}

			if (taskNext.AppointmentId != taskCur.AppointmentId)
			{
				stringBuilder.AppendLine(
					"Task appointment attachment changed.");
			}

			if (taskNext.Description != taskCur.Description && 
				!taskNext.Description.StartsWith("This task was cut from task list ") && 
				!taskNext.Description.StartsWith("This task was copied from task "))
			{
				stringBuilder.AppendLine("Task description changed.");
			}

			if (taskNext.Status != taskCur.Status)
			{
				stringBuilder.AppendLine(
					$"Task status changed from {taskCur.Status} to {taskNext.Status}.");
			}

			if (taskNext.DateStart != taskCur.DateStart)
			{
				stringBuilder.AppendLine(
					$"Task date added changed from {taskCur.DateStart} to {taskNext.DateStart}.");
			}

			if (taskNext.UserId != taskCur.UserId)
			{
				stringBuilder.AppendLine(
					$"Task author changed from {Userods.GetUser(taskCur.UserId).UserName} to {Userods.GetUser(taskNext.UserId).UserName}.");
			}

			if (taskNext.DateCompleted != taskCur.DateCompleted)
			{
				stringBuilder.AppendLine(
					$"Task date finished changed from {taskCur.DateCompleted} to {taskNext.DateCompleted}.");
			}

			if (taskNext.PriorityId != taskCur.PriorityId)
			{
				stringBuilder.AppendLine(
					"Task priority changed from " + 
					Definitions.GetDef(DefinitionCategory.TaskPriorities, taskCur.PriorityId).Name + " to " + 
					Definitions.GetDef(DefinitionCategory.TaskPriorities, taskNext.PriorityId).Name + ".");
			}


			return stringBuilder.ToString();
		}

		public static long Insert(TaskHistory taskHistory) 
			=> Crud.TaskHistCrud.Insert(taskHistory);

		public static List<TaskHistory> GetArchivesForTask(long taskId) 
			=> Crud.TaskHistCrud.SelectMany("SELECT * FROM `task_history` WHERE `task_id` =" + taskId + " ORDER BY `date_added`");
	}
}
