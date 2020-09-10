using Imedisoft.Data;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
	///<summary></summary>
	public class InsVerifyHists
	{
		#region Get Methods
		public static List<InsVerifyHist> GetForFKeyByType(long fKey, VerifyTypes verifyType)
		{

			string command = $"SELECT * FROM insverifyhist WHERE FKey={POut.Long(fKey)} AND VerifyType={POut.Int((int)verifyType)}";
			return Crud.InsVerifyHistCrud.SelectMany(command);
		}
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


		///<summary>Gets one InsVerifyHist from the db.</summary>
		public static InsVerifyHist GetOne(long insVerifyHistNum)
		{

			return Crud.InsVerifyHistCrud.SelectOne(insVerifyHistNum);
		}

		///<summary></summary>
		public static long Insert(InsVerifyHist insVerifyHist)
		{

			return Crud.InsVerifyHistCrud.Insert(insVerifyHist);
		}

		///<summary></summary>
		public static void Delete(long insVerifyHistNum)
		{

			Crud.InsVerifyHistCrud.Delete(insVerifyHistNum);
		}

		///<summary>If the passed in InsVerify is null, do nothing.  
		///Otherwise, insert the passed in InsVerify into InsVerifyHist and blank out InsVerify's UserNum, Status, and Note.</summary>
		public static void InsertFromInsVerify(InsVerify insVerify)
		{
			if (insVerify == null)
			{
				return;
			}
			Insert(new InsVerifyHist(insVerify));
			insVerify.UserNum = 0;
			insVerify.DefNum = 0;
			insVerify.Note = "";
			insVerify.DateLastAssigned = DateTime.MinValue;
			if (Preferences.GetBool(PreferenceName.InsVerifyFutureDateBenefitYear) && insVerify.AppointmentDateTime > DateTime.MinValue)
			{
				insVerify.DateLastVerified = insVerify.AppointmentDateTime;
			}
			InsVerifies.Update(insVerify);
		}
	}
}