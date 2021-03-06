using Imedisoft.Data;
using System.Collections.Generic;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EhrLabNotes {
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
		public static List<EhrLabNote> GetForLab(long ehrLabNum) {
			
			string command="SELECT * FROM ehrlabnote WHERE EhrLabNum = "+POut.Long(ehrLabNum)+" AND EhrLabResultNum=0";
			return Crud.EhrLabNoteCrud.SelectMany(command);
		}

		///<summary></summary>
		public static List<EhrLabNote> GetForLabResult(long ehrLabResultNum) {
			
			string command="SELECT * FROM ehrlabnote WHERE EhrLabResultNum="+POut.Long(ehrLabResultNum);
			return Crud.EhrLabNoteCrud.SelectMany(command);
		}

		///<summary>Deletes notes for lab results too.</summary>
		public static void DeleteForLab(long ehrLabNum) {
			
			string command="DELETE FROM ehrlabnote WHERE EhrLabNum = "+POut.Long(ehrLabNum);
			Database.ExecuteNonQuery(command);
		}

		///<summary></summary>
		public static long Insert(EhrLabNote ehrLabNote) {
			
			return Crud.EhrLabNoteCrud.Insert(ehrLabNote);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<EhrLabNote> Refresh(long patNum){
			
			string command="SELECT * FROM ehrlabnote WHERE PatNum = "+POut.Long(patNum);
			return Crud.EhrLabNoteCrud.SelectMany(command);
		}

		///<summary>Gets one EhrLabNote from the db.</summary>
		public static EhrLabNote GetOne(long ehrLabNoteNum){
			
			return Crud.EhrLabNoteCrud.SelectOne(ehrLabNoteNum);
		}

		///<summary></summary>
		public static void Update(EhrLabNote ehrLabNote){
			
			Crud.EhrLabNoteCrud.Update(ehrLabNote);
		}

		///<summary></summary>
		public static void Delete(long ehrLabNoteNum) {
			
			string command= "DELETE FROM ehrlabnote WHERE EhrLabNoteNum = "+POut.Long(ehrLabNoteNum);
			Db.ExecuteNonQuery(command);
		}
		*/



	}
}