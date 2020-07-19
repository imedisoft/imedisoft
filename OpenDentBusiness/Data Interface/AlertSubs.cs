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
			return Crud.AlertSubCrud.SelectMany(command);
		}

		///<summary>Returns list of all AlertSubs for given userNum. Can also specify a clinicNum as well.</summary>
		public static List<AlertSub> GetAllForUser(long userNum, long clinicNum = -1)
		{
			string command = "SELECT * FROM alertsub WHERE UserNum=" + POut.Long(userNum);
			if (clinicNum != -1)
			{
				command += " AND ClinicNum=" + POut.Long(clinicNum);
			}
			return Crud.AlertSubCrud.SelectMany(command);
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
		public static bool Sync(List<AlertSub> listNew, List<AlertSub> listOld)
		{
			return Crud.AlertSubCrud.Sync(listNew, listOld);
		}
	}
}