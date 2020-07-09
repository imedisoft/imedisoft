using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class DocumentMiscs{
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


		public static DocumentMisc GetByTypeAndFileName(string fileName,DocumentMiscType docMiscType) {
			
			string command="SELECT * FROM documentmisc "
				+"WHERE DocMiscType="+POut.Int((int)docMiscType)+" "
				+"AND FileName='"+POut.String(fileName)+"'";
			return Crud.DocumentMiscCrud.SelectOne(command);
		}

		///<summary></summary>
		public static long Insert(DocumentMisc documentMisc) {
			
			return Crud.DocumentMiscCrud.Insert(documentMisc);
		}

		///<summary>Appends the passed in rawBase64 string to the RawBase64 column in the db for the UpdateFiles DocMiscType row.</summary>
		public static void AppendRawBase64ForUpdateFiles(string rawBase64) {
			
			string command="UPDATE documentmisc SET RawBase64=CONCAT("+DbHelper.IfNull("RawBase64","")+","+DbHelper.ParamChar+"paramRawBase64) "
				+"WHERE DocMiscType="+POut.Int((int)DocumentMiscType.UpdateFiles);
			OdSqlParameter paramRawBase64=new OdSqlParameter("paramRawBase64",OdDbType.Text,rawBase64);
			Db.NonQ(command,paramRawBase64);
		}

		///<summary></summary>
		public static void DeleteAllForType(DocumentMiscType docMiscType) {
			
			string command="DELETE FROM documentmisc WHERE DocMiscType="+POut.Int((int)docMiscType);
			Db.NonQ(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<DocumentMisc> Refresh(long patNum){
			
			string command="SELECT * FROM documentmisc WHERE PatNum = "+POut.Long(patNum);
			return Crud.DocumentMiscCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(DocumentMisc documentMisc){
			
			return Crud.DocumentMiscCrud.Insert(documentMisc);
		}

		///<summary></summary>
		public static void Update(DocumentMisc documentMisc){
			
			Crud.DocumentMiscCrud.Update(documentMisc);
		}

		///<summary></summary>
		public static void Delete(long docMiscNum) {
			
			string command= "DELETE FROM documentmisc WHERE DocMiscNum = "+POut.Long(docMiscNum);
			Db.NonQ(command);
		}
		*/



	}
}