using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Commlogs
	{
		#region Get Methods

		///<summary>Returns the list of CommItemTypeAutos. Filters out the IsODHQ-only CommItemTypeAuto when the user is not in HQ.</summary>
		public static List<CommItemTypeAuto> GetCommItemTypes()
		{
			List<CommItemTypeAuto> listRet = Enum.GetValues(typeof(CommItemTypeAuto)).Cast<CommItemTypeAuto>().ToList();
			return listRet;
		}

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
		///<summary>Returns true if there are any rows that have a Note with char length greater than 65,535</summary>
		public static bool HasAnyLongNotes()
		{
			string command = "SELECT COUNT(*) FROM commlog WHERE CHAR_LENGTH(commlog.Note)>65535";
			return (Database.ExecuteString(command) != "0");
		}

		///<summary>Tailored for OD HQ.  Gets the most recent commlog.CommDateTime for a given patNum of type "support call or chat".
		///Returns DateTime.MinValue if no entry found.</summary>
		public static DateTime GetDateTimeOfLastEntryForPat(long patNum)
		{
			//no need for Oracle compatibility
			string command = "SELECT CommDateTime "
				+ "FROM commlog "
				+ "WHERE PatNum=" + POut.Long(patNum) + " "
				+ "AND (CommType=292 OR CommType=441) "//support call or chat, DefNums
				+ "AND CommSource=" + POut.Int((int)CommItemSource.User) + " "
				+ "ORDER BY CommDateTime DESC "
				+ "LIMIT 1";
			return PIn.Date(Database.ExecuteString(command));
		}
		#endregion


		///<summary>Gets all items for the current patient ordered by date.</summary>
		public static List<Commlog> Refresh(long patNum)
		{
			string command =
				"SELECT * FROM commlog"
				+ " WHERE PatNum = '" + patNum + "'"
				+ " ORDER BY CommDateTime";
			return Crud.CommlogCrud.SelectMany(command);
		}

		///<summary>Gets one commlog item from database.</summary>
		public static Commlog GetOne(long commlogNum)
		{
			return Crud.CommlogCrud.SelectOne(commlogNum);
		}

		///<summary>If a commlog exists with today's date for the current user and has no stop time, then that commlog is returned so it can be reopened.  Otherwise, return null.</summary>
		public static Commlog GetIncompleteEntry(long userNum, long patNum)
		{
			//no need for Oracle compatibility
			string command = "SELECT * FROM commlog WHERE DATE(CommDateTime)=CURDATE() "
				+ "AND UserNum=" + POut.Long(userNum) + " "
				+ "AND PatNum=" + POut.Long(patNum) + " "
				+ "AND (CommType=292 OR CommType=441) "//support call or chat, DefNums
				+ "AND Mode_=" + POut.Int((int)CommItemMode.Phone) + " "//mode=phone
				+ "AND DateTimeEnd < '1880-01-01' LIMIT 1";
			return Crud.CommlogCrud.SelectOne(command);
		}

		///<summary></summary>
		public static long Insert(Commlog comm)
		{
			return Crud.CommlogCrud.Insert(comm);
		}

		///<summary></summary>
		public static void Update(Commlog comm)
		{
			Crud.CommlogCrud.Update(comm);
		}

		///<summary>Updates only the changed fields (if any).</summary>
		public static bool Update(Commlog comm, Commlog oldCommlog)
		{
			return Crud.CommlogCrud.Update(comm, oldCommlog);
		}

		///<summary></summary>
		public static void Delete(Commlog comm)
		{
			string command = "SELECT COUNT(*) FROM smsfrommobile WHERE CommlogNum=" + POut.Long(comm.CommlogNum);
			if (Database.ExecuteString(command) != "0")
			{
				throw new Exception(Lans.g("CommLogs", "Not allowed to delete a commlog attached to a text message."));
			}
			Crud.CommlogCrud.Delete(comm.CommlogNum);
		}

		///<summary>Used when printing or emailing recall to make a commlog entry without any display.</summary>
		public static void InsertForRecallOrReactivation(long patNum, CommItemMode _mode, int numberOfReminders, long defNumNewStatus, CommItemTypeAuto type = CommItemTypeAuto.RECALL)
		{
			//No need to check RemotingRole; no call to db.
			InsertForRecallOrReactivation(patNum, _mode, numberOfReminders, defNumNewStatus, CommItemSource.User, Security.CurrentUser.Id//Recall commlog not associated to the Web Sched app.
				, DateTime.Now, type);
		}

		///<summary>Used when printing or emailing recall to make a commlog entry without any display.  
		///Set commSource to the corresponding entity that is making this recall.  E.g. Web Sched.
		///If the commSource is a 3rd party, set it to ProgramLink and make an overload that accepts the ProgramNum.</summary>
		public static Commlog InsertForRecallOrReactivation(long patNum, CommItemMode _mode, int numberOfReminders, long defNumNewStatus, CommItemSource commSource, long userNum, DateTime dateTimeNow, CommItemTypeAuto type = CommItemTypeAuto.RECALL)
		{

			long commType = Commlogs.GetTypeAuto(type);
			string commTypeStr = type == CommItemTypeAuto.RECALL ? "Recall" : "Reactivation";
			Commlog com = GetTodayCommlog(patNum, _mode, type);
			if (com != null)
			{
				return com;
			}
			com = new Commlog();
			com.PatNum = patNum;
			com.CommDateTime = dateTimeNow;
			com.CommType = commType;
			com.Mode_ = _mode;
			com.SentOrReceived = CommSentOrReceived.Sent;
			com.Note = "";
			if (numberOfReminders == 0)
			{
				com.Note = Lans.g("FormRecallList", $"{commTypeStr} reminder.");
			}
			else if (numberOfReminders == 1)
			{
				com.Note = Lans.g("FormRecallList", $"Second {commTypeStr} reminder.");
			}
			else if (numberOfReminders == 2)
			{
				com.Note = Lans.g("FormRecallList", $"Third {commTypeStr} reminder.");
			}
			else
			{
				com.Note = Lans.g("FormRecallList", $"{commTypeStr} reminder:") + " " + (numberOfReminders + 1).ToString();
			}
			if (defNumNewStatus == 0)
			{
				com.Note += "  " + Lans.g("Commlogs", "Status None");
			}
			else
			{
				com.Note += "  " + Defs.GetName(DefCat.RecallUnschedStatus, defNumNewStatus);
			}
			com.UserNum = userNum;
			com.CommSource = commSource;
			com.CommlogNum = Insert(com);
			EhrMeasureEvent newMeasureEvent = new EhrMeasureEvent();
			newMeasureEvent.DateTEvent = com.CommDateTime;
			newMeasureEvent.EventType = EhrMeasureEventType.ReminderSent;
			newMeasureEvent.PatNum = com.PatNum;
			newMeasureEvent.MoreInfo = com.Note;
			EhrMeasureEvents.Insert(newMeasureEvent);
			return com;
		}

		///<summary>Gets an existing Commlog sent today for patNum, _mode, and type.  Returns null if not found or no defs are setup for type.</summary>
		public static Commlog GetTodayCommlog(long patNum, CommItemMode _mode, CommItemTypeAuto type)
		{
			long commType = Commlogs.GetTypeAuto(type);
			string command;
			string datesql = "CURDATE()";
			if (commType == 0)
			{
				return null;
			}
			command = "SELECT * FROM commlog WHERE ";
			command += DbHelper.DtimeToDate("CommDateTime") + " = " + datesql;
			command += " AND PatNum=" + POut.Long(patNum) + " AND CommType=" + POut.Long(commType)
				+ " AND Mode_=" + POut.Long((int)_mode)
				+ " AND SentOrReceived=1";
			List<Commlog> listComms = Crud.CommlogCrud.SelectMany(command).OrderByDescending(x => x.CommDateTime).ToList();
			return listComms.FirstOrDefault();
		}

		///<Summary>Returns a defnum.  If no match, then it returns the first one in the list in that category.
		///If there are no defs in the category, 0 is returned.</Summary>
		public static long GetTypeAuto(CommItemTypeAuto typeauto)
		{
			//No need to check RemotingRole; no call to db.
			List<Def> listDefs = Defs.GetDefsForCategory(DefCat.CommLogTypes);
			Def def = listDefs.FirstOrDefault(x => x.ItemValue == typeauto.ToString());
			if (def != null)
			{
				return def.DefNum;
			}
			if (listDefs.Count > 0)
			{
				return listDefs[0].DefNum;
			}
			return 0;
		}

		public static int GetRecallUndoCount(DateTime date)
		{
			string command = "SELECT COUNT(*) FROM commlog "
				+ "WHERE " + DbHelper.DtimeToDate("CommDateTime") + " = " + POut.Date(date) + " "
				+ "AND (SELECT ItemValue FROM definition WHERE definition.DefNum=commlog.CommType) ='" + CommItemTypeAuto.RECALL.ToString() + "'";
			return Database.ExecuteInt(command);
		}

		public static void RecallUndo(DateTime date)
		{
			string command = "DELETE FROM commlog "
				+ "WHERE " + DbHelper.DtimeToDate("CommDateTime") + " = " + POut.Date(date) + " "
				+ "AND (SELECT ItemValue FROM definition WHERE definition.DefNum=commlog.CommType) ='" + CommItemTypeAuto.RECALL.ToString() + "'";
			Database.ExecuteNonQuery(command);
		}

		///<summary>Returns the message used to ask if the user would like to save the appointment/patient note as a commlog when deleting an appointment/patient note.  Only returns up to the first 30 characters of the note.</summary>
		public static string GetDeleteApptCommlogMessage(string noteText, ApptStatus apptStatus)
		{
			//No need to check RemotingRole; no call to db.
			string commlogMsgText = "";
			if (noteText != "")
			{
				if (apptStatus == ApptStatus.PtNote || apptStatus == ApptStatus.PtNoteCompleted)
				{
					commlogMsgText = Lans.g("Commlogs", "Save patient note in CommLog?") + "\r\n" + "\r\n";
				}
				else
				{
					commlogMsgText = Lans.g("Commlogs", "Save appointment note in CommLog?") + "\r\n" + "\r\n";
				}
				//Show up to 30 characters of the note because they can get rather large thus pushing the buttons off the screen.
				commlogMsgText += noteText.Substring(0, Math.Min(noteText.Length, 30));
				commlogMsgText += (noteText.Length > 30) ? "..." : "";//Append ... to the end of the message so that they know there is more to the note than what is displayed.
			}
			return commlogMsgText;
		}

		///<summary>Gets all commlogs for family that contain a DateTimeEnd entry.  Used internally to keep track of how long calls lasted.</summary>
		public static List<Commlog> GetTimedCommlogsForPat(long guarantor)
		{
			string command = "SELECT commlog.* FROM commlog "
				+ "INNER JOIN patient ON commlog.PatNum=patient.PatNum AND patient.Guarantor=" + POut.Long(guarantor) + " "
				+ "WHERE " + DbHelper.Year("commlog.DateTimeEnd") + ">1";
			return Crud.CommlogCrud.SelectMany(command);
		}
	}

	public enum CommItemTypeAuto
	{
		[ShortDescription("APPT"), Description("Appointent")]
		APPT,

		[ShortDescription("FIN"), Description("Financial")]
		FIN,

		[ShortDescription("RECALL"), Description("Recall")]
		RECALL,

		[ShortDescription("MISC"), Description("Miscellaneous")]
		MISC,

		[ShortDescription("TEXT"), Description("Text Communication (E-mail, Sms, etc.)")]
		TEXT,

		[ShortDescription("REACT"), Description("Reactivation")]
		REACT,

		[ShortDescription("FHIR"), Description("FHIR API")]
		FHIR,
	}
}