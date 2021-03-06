using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EmailMessageUids{
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


		///<summary>Gets all unique email ids for the given recipient email address.  The result is used to determine which emails to download for a particular inbox address.</summary>
		public static List<EmailMessageUid> GetForRecipientAddress(string strRecipientAddress) {
			
			string command="SELECT * FROM emailmessageuid WHERE RecipientAddress='"+POut.String(strRecipientAddress)+"'";
			return Crud.EmailMessageUidCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(EmailMessageUid emailMessageUid) {
			
			return Crud.EmailMessageUidCrud.Insert(emailMessageUid);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary>Gets one EmailMessageUid from the db.</summary>
		public static EmailMessageUid GetOne(long emailMessageUidNum){
			
			return Crud.EmailMessageUidCrud.SelectOne(emailMessageUidNum);
		}

		///<summary></summary>
		public static void Update(EmailMessageUid emailMessageUid){
			
			Crud.EmailMessageUidCrud.Update(emailMessageUid);
		}

		///<summary></summary>
		public static void Delete(long emailMessageUidNum) {
			
			string command= "DELETE FROM emailmessageuid WHERE EmailMessageUidNum = "+POut.Long(emailMessageUidNum);
			Db.ExecuteNonQuery(command);
		}
		*/



	}
}