using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class LoginAttempts{

		///<summary>Returns the login attempts for the given user in the last X minutes.</summary>
		public static int CountForUser(string userName,UserWebFKeyType type,int lastXMinutes) {
			
			string command=$@"SELECT COUNT(*) FROM loginattempt WHERE UserName='{POut.String(userName)}' AND LoginType={POut.Int((int)type)}
				AND DateTFail >= {DbHelper.DateAddMinute("NOW()",POut.Int(-lastXMinutes))}";
			return PIn.Int(Db.GetCount(command));
		}

		///<summary></summary>
		public static long InsertFailed(string userName,UserWebFKeyType type) {
			
			return Crud.LoginAttemptCrud.Insert(new LoginAttempt { UserName=userName,LoginType=type });
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		#region Get Methods
		///<summary></summary>
		public static List<LoginAttempt> Refresh(long patNum){
			
			string command="SELECT * FROM loginattempt WHERE PatNum = "+POut.Long(patNum);
			return Crud.LoginAttemptCrud.SelectMany(command);
		}
		
		///<summary>Gets one LoginAttempt from the db.</summary>
		public static LoginAttempt GetOne(long loginAttemptNum){
			
			return Crud.LoginAttemptCrud.SelectOne(loginAttemptNum);
		}
		#endregion Get Methods
		#region Modification Methods
		#region Insert
		#endregion Insert
		#region Update
		///<summary></summary>
		public static void Update(LoginAttempt loginAttempt){
			
			Crud.LoginAttemptCrud.Update(loginAttempt);
		}
		#endregion Update
		#region Delete
		///<summary></summary>
		public static void Delete(long loginAttemptNum) {
			
			Crud.LoginAttemptCrud.Delete(loginAttemptNum);
		}
		#endregion Delete
		#endregion Modification Methods
		#region Misc Methods
		

		
		#endregion Misc Methods
		*/



	}
}