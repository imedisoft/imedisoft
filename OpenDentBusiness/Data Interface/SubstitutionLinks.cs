using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SubstitutionLinks{
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary>Gets one SubstitutionLink from the db.</summary>
		public static SubstitutionLink GetOne(long substitutionLinkNum){
			
			return Crud.SubstitutionLinkCrud.SelectOne(substitutionLinkNum);
		}

		///<summary></summary>
		public static void Update(SubstitutionLink substitutionLink){
			
			Crud.SubstitutionLinkCrud.Update(substitutionLink);
		}

		///<summary></summary>
		public static void Delete(long substitutionLinkNum) {
			
			Crud.SubstitutionLinkCrud.Delete(substitutionLinkNum);
		}

		

		
		*/

		///<summary></summary>
		public static List<SubstitutionLink> GetAllForPlans(List<InsPlan> listInsPlans) {
			//No need to check RemotingRole; no call to db.
			return GetAllForPlans(listInsPlans.Select(x => x.PlanNum).ToArray());
		}

		///<summary></summary>
		public static List<SubstitutionLink> GetAllForPlans(params long[] arrayPlanNums){
			
			if(arrayPlanNums.Length==0) {
				return new List<SubstitutionLink>();
			}
			List <long> listPlanNums=new List<long>(arrayPlanNums);
			string command="SELECT * FROM substitutionlink WHERE PlanNum IN("+String.Join(",",listPlanNums.Select(x => POut.Long(x)))+")";
			return Crud.SubstitutionLinkCrud.SelectMany(command);
		}

		///<summary>Inserts, updates, or deletes the passed in list against the stale list listOld.  Returns true if db changes were made.</summary>
		public static bool Sync(List<SubstitutionLink> listNew,List<SubstitutionLink> listOld) {
			
			return Crud.SubstitutionLinkCrud.Sync(listNew,listOld);
		}

		public static bool HasSubstCodeForPlan(InsPlan insPlan,long codeNum,List<SubstitutionLink> listSubLinks) {
			//No need to check RemotingRole; no call to db.
			if(insPlan.CodeSubstNone) {
				return false;
			}
			return !listSubLinks.Exists(x => x.PlanNum==insPlan.PlanNum && x.CodeNum==codeNum && x.SubstOnlyIf==SubstitutionCondition.Never);
		}

		///<summary>Returns true if the procedure has a substitution code for the give tooth and InsPlans.</summary>
		public static bool HasSubstCodeForProcCode(ProcedureCode procCode,string toothNum,List<SubstitutionLink> listSubLinks,
			List<InsPlan> listPatInsPlans) 
		{
			//No need to check RemotingRole; no call to db.
			foreach(InsPlan plan in listPatInsPlans) {
				//Check to see if any allow substitutions.
				if(HasSubstCodeForPlan(plan,procCode.CodeNum,listSubLinks)) {
					long subCodeNum=ProcedureCodes.GetSubstituteCodeNum(procCode.ProcCode,toothNum,plan.PlanNum,listSubLinks);//for posterior composites
					if(procCode.CodeNum!=subCodeNum && subCodeNum>0) {
						return true;
					}
				}
			}
			return false;
		}

		///<summary>Inserts a copy of all of the planNumOld SubstitutionLinks with the planNumNew. This should be done every time a new insplan gets created
		///and you want to maintain the SubstitutionLink of the old insplan.</summary>
		public static void CopyLinksToNewPlan(long planNumNew,long planNumOld) {
			//No need to check RemotingRole; no call to db.
			//Get a list of the sub links of the old insplan. After the foreach loop below, this list will no longer contain the sub links for the old insplan.
			List<SubstitutionLink> listSubstLinksOfOldPlan=SubstitutionLinks.GetAllForPlans(planNumOld);
			foreach(SubstitutionLink subLink in listSubstLinksOfOldPlan) {
				//Only change the old planNum with the new planNum. Insert will "create" a new SubstitutionLink with a new primary key. 
				subLink.PlanNum=planNumNew;
			}
			InsertMany(listSubstLinksOfOldPlan);
		}

		///<summary></summary>
		public static long Insert(SubstitutionLink substitutionLink) {
			
			return Crud.SubstitutionLinkCrud.Insert(substitutionLink);
		}

		public static void InsertMany(List<SubstitutionLink> listSubstitutionLinks) {
			
			Crud.SubstitutionLinkCrud.InsertMany(listSubstitutionLinks);
		}
	}
}