using Imedisoft.Data;
using Imedisoft.Data.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace OpenDentBusiness
{
    public class AutoCodeItems
	{
		[CacheGroup(nameof(InvalidType.AutoCodes))]
		private class AutoCodeItemCache : DictionaryCache<long, AutoCodeItem>
        {
            protected override IEnumerable<AutoCodeItem> Load() 
				=> Crud.AutoCodeItemCrud.SelectMany("SELECT * FROM autocodeitem");

			protected override long GetKey(AutoCodeItem item) 
				=> item.AutoCodeItemNum;
        }

		/// <summary>
		/// The object that accesses the cache in a thread-safe manner.
		/// </summary>
		private static readonly AutoCodeItemCache cache = new AutoCodeItemCache();

		public static AutoCodeItem GetOne(long codeNum) 
			=> cache.Find(codeNum);

		public static bool GetContainsKey(long codeNum) 
			=> cache.Contains(codeNum);

		public static void RefreshCache() => cache.Refresh();

		public static long Insert(AutoCodeItem Cur)
		{
			return Crud.AutoCodeItemCrud.Insert(Cur);
		}

		public static void Update(AutoCodeItem Cur)
		{
			Crud.AutoCodeItemCrud.Update(Cur);
		}

		public static void Delete(AutoCodeItem Cur)
		{

			string command = "DELETE FROM autocodeitem WHERE AutoCodeItemNum = '" + POut.Long(Cur.AutoCodeItemNum) + "'";
			Database.ExecuteNonQuery(command);
		}

		public static void Delete(long autoCodeNum)
		{

			string command = "DELETE FROM autocodeitem WHERE AutoCodeNum = '"
				+ POut.Long(autoCodeNum) + "'";
			Database.ExecuteNonQuery(command);
		}

		///<summary>Gets from cache.  No call to db.</summary>
		public static List<AutoCodeItem> GetListForCode(long autoCodeNum)
		{
			return cache.Find(x => x.AutoCodeNum == autoCodeNum).ToList();
		}

		//-----

		///<summary>Only called from ContrChart.listProcButtons_Click.  Called once for each tooth selected and for each autocode item attached to the button.</summary>
		public static long GetCodeNum(long autoCodeNum, string toothNum, string surf, bool isAdditional, long patNum, int age, bool willBeMissing)
		{
			//No need to check RemotingRole; no call to db.
			bool allCondsMet;
			List<AutoCodeItem> listForCode = AutoCodeItems.GetListForCode(autoCodeNum);
			if (listForCode.Count == 0)
			{
				return 0;
			}
			//bool willBeMissing=Procedures.WillBeMissing(toothNum,patNum);//moved this out so that this method has no db call
			List<AutoCodeCond> condList;
			for (int i = 0; i < listForCode.Count; i++)
			{
				condList = AutoCodeConds.GetListForItem(listForCode[i].AutoCodeItemNum);
				allCondsMet = true;
				for (int j = 0; j < condList.Count; j++)
				{
					if (!AutoCodeConds.ConditionIsMet(condList[j].Cond, toothNum, surf, isAdditional, willBeMissing, age))
					{
						allCondsMet = false;
					}
				}
				if (allCondsMet)
				{
					return listForCode[i].CodeNum;
				}
			}
			return listForCode[0].CodeNum;//if couldn't find a better match
		}

		///<summary>Only called when closing the procedure edit window. Usually returns the supplied CodeNum, unless a better match is found.</summary>
		public static long VerifyCode(long codeNum, string toothNum, string surf, bool isAdditional, long patNum, int age,
			out AutoCode AutoCodeCur)
		{
			//No need to check RemotingRole; no call to db.
			bool allCondsMet;
			AutoCodeCur = null;
			if (!GetContainsKey(codeNum))
			{
				return codeNum;
			}
			if (!AutoCodes.GetContainsKey(GetOne(codeNum).AutoCodeNum))
			{
				return codeNum;//just in case.
			}
			AutoCodeCur = AutoCodes.GetOne(GetOne(codeNum).AutoCodeNum);
			if (AutoCodeCur.LessIntrusive)
			{
				return codeNum;
			}
			bool willBeMissing = Procedures.WillBeMissing(toothNum, patNum);
			List<AutoCodeItem> listForCode = AutoCodeItems.GetListForCode(GetOne(codeNum).AutoCodeNum);
			List<AutoCodeCond> condList;
			for (int i = 0; i < listForCode.Count; i++)
			{
				condList = AutoCodeConds.GetListForItem(listForCode[i].AutoCodeItemNum);
				allCondsMet = true;
				for (int j = 0; j < condList.Count; j++)
				{
					if (!AutoCodeConds.ConditionIsMet(condList[j].Cond, toothNum, surf, isAdditional, willBeMissing, age))
					{
						allCondsMet = false;
					}
				}
				if (allCondsMet)
				{
					return listForCode[i].CodeNum;
				}
			}
			return codeNum;//if couldn't find a better match
		}

		///<summary>Checks inputs and determines if user should be prompted to pick a more applicable procedure code.</summary>
		///<param name="verifyCode">This is the recommended code based on input. If it matches procCode return value will be false.</param>
		public static bool ShouldPromptForCodeChange(Procedure proc, ProcedureCode procCode, Patient pat, bool isMandibular,
			List<ClaimProc> claimProcsForProc, out long verifyCode)
		{
			//No remoting role check; no call to db and method utilizes an out parameter.
			verifyCode = proc.CodeNum;
			//these areas have no autocodes
			if (procCode.TreatArea == TreatmentArea.Mouth
				|| procCode.TreatArea == TreatmentArea.Quad
				|| procCode.TreatArea == TreatmentArea.Sextant
				|| Procedures.IsAttachedToClaim(proc, claimProcsForProc))
			{
				return false;
			}
			//this represents the suggested code based on the autocodes set up.
			AutoCode AutoCodeCur = null;
			if (procCode.TreatArea == TreatmentArea.Arch)
			{
				if (string.IsNullOrEmpty(proc.Surf))
				{
					return false;
				}
				if (proc.Surf == "U")
				{
					verifyCode = AutoCodeItems.VerifyCode(procCode.CodeNum, "1", "", proc.IsAdditional, pat.PatNum, pat.Age, out AutoCodeCur);//max
				}
				else
				{
					verifyCode = AutoCodeItems.VerifyCode(procCode.CodeNum, "32", "", proc.IsAdditional, pat.PatNum, pat.Age, out AutoCodeCur);//mand
				}
			}
			else if (procCode.TreatArea == TreatmentArea.ToothRange)
			{
				//test for max or mand.
				verifyCode = AutoCodeItems.VerifyCode(procCode.CodeNum, (isMandibular) ? "32" : "1", "", proc.IsAdditional, pat.PatNum, pat.Age, out AutoCodeCur);
			}
			else
			{//surf or tooth
				string claimSurf = Tooth.SurfTidyForClaims(proc.Surf, proc.ToothNum);
				verifyCode = AutoCodeItems.VerifyCode(procCode.CodeNum, proc.ToothNum, claimSurf, proc.IsAdditional, pat.PatNum, pat.Age, out AutoCodeCur);
			}
			return procCode.CodeNum != verifyCode;
		}
	}
}
