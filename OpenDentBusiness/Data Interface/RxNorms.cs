using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenDentBusiness
{
    public class RxNorms
	{
		/// <summary>
		/// RxNorm table is considered to be too small if less than 50 RxNorms in table, because our default medication list contains 50 items, implying that the user has not imported RxNorms.
		/// </summary>
		public static bool IsRxNormTableSmall() => Database.ExecuteLong("SELECT COUNT(*) FROM rxnorm") < 50;

		public static RxNorm GetByRxCUI(string rxCui) 
			=> Crud.RxNormCrud.SelectOne(
				"SELECT * FROM rxnorm WHERE RxCui='" + POut.String(rxCui) + "' AND MmslCode=''");

		/// <summary>
		/// Never returns multums, only used for displaying after a search.
		/// </summary>
		public static List<RxNorm> GetListByCodeOrDesc(string codeOrDesc, bool isExact, bool ignoreNumbers)
		{
			string command = "SELECT * FROM rxnorm WHERE MmslCode='' ";
			if (ignoreNumbers)
			{
				command += "AND Description NOT REGEXP '.*[0-9]+.*' ";
			}

			if (isExact)
			{
				command += "AND (RxCui LIKE '" + POut.String(codeOrDesc) + "' OR Description LIKE '" + POut.String(codeOrDesc) + "')";
			}
			else
			{
				// Similar matches
				string[] searchWords = codeOrDesc.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

				if (searchWords.Length > 0)
				{
					command += "AND ("
						+ "RxCui LIKE '%" + POut.String(codeOrDesc) + "%' "
						+ " OR "
						+ "(" + string.Join(" AND ", searchWords.Select(x => "Description LIKE '%" + POut.String(x) + "%'")) + ") "
						+ ")";
				}
			}

			command += " ORDER BY Description";

			return Crud.RxNormCrud.SelectMany(command);
		}

		/// <summary>
		/// Used to return the multum code based on RxCui. 
		/// If blank, use the Description instead.
		/// </summary>
		public static string GetMmslCodeByRxCui(string rxCui) 
			=> Database.ExecuteString(
				"SELECT MmslCode FROM rxnorm WHERE MmslCode!='' AND RxCui='" + rxCui + "'");

		///<summary></summary>
		public static string GetDescByRxCui(string rxCui) 
			=> Database.ExecuteString(
				"SELECT Description FROM rxnorm WHERE MmslCode='' AND RxCui='" + rxCui + "'");

		public static RxNorm GetOne(long rxNormNum) 
			=> Crud.RxNormCrud.SelectOne(rxNormNum);

		public static long Insert(RxNorm rxNorm) 
			=>  Crud.RxNormCrud.Insert(rxNorm);

		public static void Update(RxNorm rxNorm) 
			=> Crud.RxNormCrud.Update(rxNorm);

		public static List<RxNorm> GetAll() 
			=> Crud.RxNormCrud.SelectMany("SELECT * FROM rxnorm");

		/// <summary>
		/// Returns a list of just the codes for use in the codesystem import tool.
		/// </summary>
		public static List<string> GetAllCodes() 
			=> Database.SelectMany("SELECT RxCui FROM rxnorm", Database.ToScalar<string>).ToList();

		/// <summary>
		/// Returns the count of all RxNorm codes in the database.
		/// RxNorms cannot be hidden.
		/// </summary>
		public static long GetCodeCount() 
			=> Database.ExecuteLong("SELECT COUNT(*) FROM rxnorm");
	}
}
