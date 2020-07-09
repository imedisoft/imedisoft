using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class PatientPortalInvites{

		///<summary></summary>
		public static List<PatientPortalInvite> Refresh(long patNum) {
			
			string command="SELECT * FROM patientportalinvite WHERE PatNum = "+POut.Long(patNum);
			return Crud.PatientPortalInviteCrud.SelectMany(command);
		}

		///<summary>Gets a list of all PatientPortalInvites matching the passed in parameters. To get all PatientPortalInvites, pass in no parameters.
		///</summary>
		public static List<PatientPortalInvite> GetMany(params SQLWhere[] whereClause) {
			List<SQLWhere> listWheres=new List<SQLWhere>();
			foreach(SQLWhere where in whereClause) {
				listWheres.Add(where);
			}
			return GetMany(listWheres);
		}

		///<summary>Gets a list of all PatientPortalInvites matching the passed in parameters.</summary>
		public static List<PatientPortalInvite> GetMany(List<SQLWhere> listWheres) {
			
			string command="SELECT * FROM patientportalinvite ";
			if(listWheres!=null && listWheres.Count > 0) {
				command+="WHERE "+string.Join(" AND ",listWheres);
			}
			return Crud.PatientPortalInviteCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void InsertMany(List<PatientPortalInvite> listPatientPortalInvites) {
			
			Crud.PatientPortalInviteCrud.InsertMany(listPatientPortalInvites);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		#region Get Methods

		
		///<summary>Gets one PatientPortalInvite from the db.</summary>
		public static PatientPortalInvite GetOne(long patientPortalInviteNum){
			
			return Crud.PatientPortalInviteCrud.SelectOne(patientPortalInviteNum);
		}
		#endregion
		#region Modification Methods
			#region Insert
		///<summary></summary>
		public static long Insert(PatientPortalInvite patientPortalInvite){
			
			return Crud.PatientPortalInviteCrud.Insert(patientPortalInvite);
		}
			#endregion
			#region Update
		///<summary></summary>
		public static void Update(PatientPortalInvite patientPortalInvite){
			
			Crud.PatientPortalInviteCrud.Update(patientPortalInvite);
		}
			#endregion
			#region Delete
		///<summary></summary>
		public static void Delete(long patientPortalInviteNum) {
			
			Crud.PatientPortalInviteCrud.Delete(patientPortalInviteNum);
		}
			#endregion
		#endregion
		#region Misc Methods
		

		
		#endregion
		*/



	}
}