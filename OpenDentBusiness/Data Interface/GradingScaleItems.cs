using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class GradingScaleItems{
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

		///<summary>Gets all grading scale items ordered by GradeNumber descending.</summary>
		public static List<GradingScaleItem> Refresh(long gradingScaleNum){
			
			string command="SELECT * FROM gradingscaleitem WHERE GradingScaleNum = "+POut.Long(gradingScaleNum)
				+" ORDER BY GradeNumber DESC";
			return Crud.GradingScaleItemCrud.SelectMany(command);
		}

		///<summary>Gets one GradingScaleItem from the db.</summary>
		public static GradingScaleItem GetOne(long gradingScaleItemNum){
			
			return Crud.GradingScaleItemCrud.SelectOne(gradingScaleItemNum);
		}

		///<summary></summary>
		public static long Insert(GradingScaleItem gradingScaleItem){
			
			return Crud.GradingScaleItemCrud.Insert(gradingScaleItem);
		}

		///<summary></summary>
		public static void Update(GradingScaleItem gradingScaleItem){
			
			Crud.GradingScaleItemCrud.Update(gradingScaleItem);
		}

		///<summary></summary>
		public static void DeleteAllByGradingScale(long gradingScaleNum) {
			
			string command= "DELETE FROM gradingscaleitem WHERE GradingScaleNum = "+POut.Long(gradingScaleNum);
			Database.ExecuteNonQuery(command);
		}

		///<summary></summary>
		public static void Delete(long gradingScaleItemNum) {
			
			string command= "DELETE FROM gradingscaleitem WHERE GradingScaleItemNum = "+POut.Long(gradingScaleItemNum);
			Database.ExecuteNonQuery(command);
		}



	}
}