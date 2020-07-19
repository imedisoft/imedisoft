using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using Imedisoft.Data;

namespace OpenDentBusiness
{
	public class AlertSubs
	{

		public static void DeleteAndInsertForSuperUsers(List<Userod> listUsers, List<AlertSub> listAlertSubs)
		{
			if (listUsers == null || listUsers.Count < 1)
			{
				return;
			}
			string command = "DELETE FROM alertsub WHERE UserNum IN(" + string.Join(",", listUsers.Select(x => x.Id).ToList()) + ")";
			Database.ExecuteNonQuery(command);
			foreach (AlertSub alertSub in listAlertSubs)
			{
				command = "INSERT INTO alertsub (UserNum,ClinicNum,Type) VALUES(" + alertSub.UserNum.ToString() + "," + alertSub.ClinicNum.ToString() + "," + ((int)alertSub.Type).ToString() + ")";
				Database.ExecuteNonQuery(command);
			}
		}

		#region Get Methods
		public static List<AlertSub> GetAll()
		{
			string command = "SELECT * FROM alertsub";
			return Crud.AlertSubCrud.SelectMany(command).ToList();
		}

		///<summary>Returns list of all AlertSubs for given userNum. Can also specify a clinicNum as well.</summary>
		public static List<AlertSub> GetAllForUser(long userNum, long clinicNum = -1)
		{
			string command = "SELECT * FROM alertsub WHERE UserNum=" + POut.Long(userNum);
			if (clinicNum != -1)
			{
				command += " AND ClinicNum=" + POut.Long(clinicNum);
			}
			return Crud.AlertSubCrud.SelectMany(command).ToList();
		}
		#endregion

		///<summary>Gets one AlertSub from the db.</summary>
		public static AlertSub GetOne(long alertSubNum)
		{
			return Crud.AlertSubCrud.SelectOne(alertSubNum);
		}

		///<summary></summary>
		public static long Insert(AlertSub alertSub)
		{
			return Crud.AlertSubCrud.Insert(alertSub);
		}

		public static void Update(AlertSub alertSub)
		{
			Crud.AlertSubCrud.Update(alertSub);
		}

		public static void Delete(long alertSubNum)
		{
			Crud.AlertSubCrud.Delete(alertSubNum);
		}

		///<summary>Inserts, updates, or deletes db rows to match listNew.  No need to pass in userNum, it's set before remoting role check and passed to
		///the server if necessary.  Doesn't create ApptComm items, but will delete them.  If you use Sync, you must create new Apptcomm items.</summary>
		public static bool Sync(List<AlertSub> listNew, List<AlertSub> listDB)
		{
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<AlertSub> listIns = new List<AlertSub>();
			List<AlertSub> listUpdNew = new List<AlertSub>();
			List<AlertSub> listUpdDB = new List<AlertSub>();
			List<AlertSub> listDel = new List<AlertSub>();
			listNew.Sort((AlertSub x, AlertSub y) => { return x.AlertSubNum.CompareTo(y.AlertSubNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((AlertSub x, AlertSub y) => { return x.AlertSubNum.CompareTo(y.AlertSubNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew = 0;
			int idxDB = 0;
			int rowsUpdatedCount = 0;
			AlertSub fieldNew;
			AlertSub fieldDB;
			//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.
			//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.
			while (idxNew < listNew.Count || idxDB < listDB.Count)
			{
				fieldNew = null;
				if (idxNew < listNew.Count)
				{
					fieldNew = listNew[idxNew];
				}
				fieldDB = null;
				if (idxDB < listDB.Count)
				{
					fieldDB = listDB[idxDB];
				}
				//begin compare
				if (fieldNew != null && fieldDB == null)
				{//listNew has more items, listDB does not.
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if (fieldNew == null && fieldDB != null)
				{//listDB has more items, listNew does not.
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				else if (fieldNew.AlertSubNum < fieldDB.AlertSubNum)
				{//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if (fieldNew.AlertSubNum > fieldDB.AlertSubNum)
				{//dbPK less than newPK, dbItem is 'next'
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				//Both lists contain the 'next' item, update required
				listUpdNew.Add(fieldNew);
				listUpdDB.Add(fieldDB);
				idxNew++;
				idxDB++;
			}
			//Commit changes to DB
			for (int i = 0; i < listIns.Count; i++)
			{
				Insert(listIns[i]);
			}
			for (int i = 0; i < listUpdNew.Count; i++)
			{
				if (Crud.AlertSubCrud.Update(listUpdNew[i], listUpdDB[i]))
				{
					rowsUpdatedCount++;
				}
			}
			for (int i = 0; i < listDel.Count; i++)
			{
				Delete(listDel[i].AlertSubNum);
			}
			if (rowsUpdatedCount > 0 || listIns.Count > 0 || listDel.Count > 0)
			{
				return true;
			}
			return false;
		}
	}
}