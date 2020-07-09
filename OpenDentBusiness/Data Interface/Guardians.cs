using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OpenDentBusiness
{
	///<summary></summary>
	public class Guardians
	{
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


		///<summary>Get all guardians for a one dependant/child.</summary>
		public static List<Guardian> Refresh(long patNumChild)
		{

			string command = "SELECT * FROM guardian WHERE PatNumChild = " + POut.Long(patNumChild) + " ORDER BY Relationship";
			return Crud.GuardianCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(Guardian guardian)
		{

			return Crud.GuardianCrud.Insert(guardian);
		}

		///<summary></summary>
		public static void Update(Guardian guardian)
		{

			Crud.GuardianCrud.Update(guardian);
		}

		///<summary></summary>
		public static void Delete(long guardianNum)
		{

			Crud.GuardianCrud.Delete(guardianNum);
		}

		///<summary></summary>
		public static void DeleteForFamily(long patNumGuar)
		{

			string command = "DELETE FROM guardian "
				+ "WHERE PatNumChild IN (SELECT p.PatNum FROM patient p WHERE p.Guarantor=" + POut.Long(patNumGuar) + ")";
			Db.NonQ(command);
		}

		///<summary></summary>
		public static bool ExistForFamily(long patNumGuar)
		{

			string command = "SELECT COUNT(*) FROM guardian "
				+ "WHERE PatNumChild IN (SELECT p.PatNum FROM patient p WHERE p.Guarantor=" + POut.Long(patNumGuar) + ")";
			if (Db.GetCount(command) == "0")
			{
				return false;
			}
			return true;
		}

		/// <summary>Short abbreviation of relationship within parentheses.</summary>
		public static string GetGuardianRelationshipStr(GuardianRelationship relat)
		{
			//No need to check RemotingRole; no call to db.
			switch (relat)
			{
				case GuardianRelationship.Brother: return "(br)";
				case GuardianRelationship.CareGiver: return "(cg)";
				case GuardianRelationship.Child: return "(c)";
				case GuardianRelationship.Father: return "(d)";
				case GuardianRelationship.FosterChild: return "(fc)";
				case GuardianRelationship.Friend: return "(f)";
				case GuardianRelationship.Grandchild: return "(gc)";
				case GuardianRelationship.Grandfather: return "(gf)";
				case GuardianRelationship.Grandmother: return "(gm)";
				case GuardianRelationship.Grandparent: return "(gp)";
				case GuardianRelationship.Guardian: return "(g)";
				case GuardianRelationship.LifePartner: return "(lp)";
				case GuardianRelationship.Mother: return "(m)";
				case GuardianRelationship.Other: return "(o)";
				case GuardianRelationship.Parent: return "(p)";
				case GuardianRelationship.Self: return "(se)";
				case GuardianRelationship.Sibling: return "(sb)";
				case GuardianRelationship.Sister: return "(ss)";
				case GuardianRelationship.Sitter: return "(s)";
				case GuardianRelationship.Spouse: return "(sp)";
				case GuardianRelationship.Stepchild: return "(sc)";
				case GuardianRelationship.Stepfather: return "(sf)";
				case GuardianRelationship.Stepmother: return "(sm)";
			}
			return "";
		}

		///<summary>Inserts, updates, or deletes database rows from the provided list of family PatNums back to the state of listNew.
		///Must always pass in the list of family PatNums.</summary>
		public static void RevertChanges(List<Guardian> listNew, List<long> listFamPatNums)
		{
			List<Guardian> listDB = listFamPatNums.SelectMany(x => Guardians.Refresh(x)).ToList();
			//Usually we don't like using a DB list for sync because of potential deletions of newer entries.  However I am leaving this function alone 
			//because it would be a lot of work to rewrite FormPatientEdit to only undo the changes that this instance of the window specifically made.
			Crud.GuardianCrud.Sync(listNew, listDB);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.</summary>
		public static void Sync(List<Guardian> listNew, List<Guardian> listOld)
		{
			Crud.GuardianCrud.Sync(listNew, listOld);
		}
	}
}