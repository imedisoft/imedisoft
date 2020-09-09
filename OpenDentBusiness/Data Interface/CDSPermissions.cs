using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class CDSPermissions {
		//TODO: implement caching;

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

		public static CDSPermission GetForUser(long usernum) {
			
			string command="SELECT * FROM cdspermission WHERE UserNum="+POut.Long(usernum);
			CDSPermission retval=Crud.CDSPermissionCrud.SelectOne(command);
			if(retval!=null) {
				return retval;
			}
			return new CDSPermission();//return new CDS permission that has no permissions granted.
		}

		///<summary></summary>
		public static List<CDSPermission> GetAll() {
			
			InsertMissingValues();
			string command="SELECT * FROM cdspermission";
			return Crud.CDSPermissionCrud.SelectMany(command);
		}

        /// <summary>
        /// Inserts one row per UserOD if they do not have one already.
        /// </summary>
        private static void InsertMissingValues()
        {
            const string command = "SELECT * FROM users WHERE is_hidden = 0 AND id NOT IN (SELECT UserNum FROM cdspermission)";

            foreach (var user in Userods.SelectMany(command))
            {
                Insert(new CDSPermission
                {
                    UserNum = user.Id
                });
			}
        }

        ///<summary></summary>
		public static long Insert(CDSPermission cDSPermission) {
			
			return Crud.CDSPermissionCrud.Insert(cDSPermission);
		}

		///<summary></summary>
		public static void Update(CDSPermission cDSPermission) {
			
			Crud.CDSPermissionCrud.Update(cDSPermission);
		}
		
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<CDSPermission> Refresh(long patNum){
			
			string command="SELECT * FROM cdspermission WHERE PatNum = "+POut.Long(patNum);
			return Crud.CDSPermissionCrud.SelectMany(command);
		}

		///<summary>Gets one CDSPermission from the db.</summary>
		public static CDSPermission GetOne(long cDSPermissionNum){
			
			return Crud.CDSPermissionCrud.SelectOne(cDSPermissionNum);
		}

		///<summary></summary>
		public static void Delete(long cDSPermissionNum) {
			
			string command= "DELETE FROM cdspermission WHERE CDSPermissionNum = "+POut.Long(cDSPermissionNum);
			Db.ExecuteNonQuery(command);
		}
		*/



	}
}