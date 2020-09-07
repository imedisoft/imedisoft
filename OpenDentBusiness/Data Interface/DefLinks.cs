using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
	public class DefLinks
	{
		/// <summary>
		/// Gets list of all DefLinks by defLinkType.
		/// </summary>
		public static List<DefLink> GetDefLinksByType(DefLinkType defType)
		{
			return Crud.DefLinkCrud.SelectMany("SELECT * FROM deflink WHERE LinkType=" + (int)defType);
		}

		/// <summary>
		/// Gets list of all DefLinks for the definition and defLinkType passed in.
		/// </summary>
		public static List<DefLink> GetDefLinksByType(DefLinkType defType, long defNum)
		{
			return Crud.DefLinkCrud.SelectMany("SELECT * FROM deflink WHERE LinkType=" + (int)defType + " AND DefNum=" + defNum);
		}

		/// <summary>
		/// Gets list of all DefLinks for the definitions and defLinkType passed in.
		/// </summary>
		public static List<DefLink> GetDefLinksByTypeAndDefs(DefLinkType defType, List<long> listDefNums)
		{
			if (listDefNums == null || listDefNums.Count < 1)
			{
				return new List<DefLink>();
			}

			return Crud.DefLinkCrud.SelectMany(
				"SELECT * FROM deflink WHERE LinkType=" + (int)defType + " AND DefNum IN(" + string.Join(",", listDefNums) + ")");
		}

		/// <summary>
		/// Gets list of all operatory specific DefLinks associated to the WebSchedNewPatApptTypes definition category.
		/// </summary>
		public static List<DefLink> GetDefLinksForWebSchedNewPatApptOperatories()
		{
			//Get all definitions that are associated to the WebSchedNewPatApptTypes category that are linked to an operatory.
			List<Definition> listWSNPAATDefs = Definitions.GetDefsForCategory(DefinitionCategory.WebSchedNewPatApptTypes);//Cannot hide defs of this category at this time.
																								//Return all of the deflinks that are of type Operatory in order to get the operatory specific deflinks.
			return GetDefLinksByTypeAndDefs(DefLinkType.Operatory, listWSNPAATDefs.Select(x => x.Id).ToList());
		}

		/// <summary>
		/// Gets list of all appointment type specific DefLinks associated to the WebSchedNewPatApptTypes definition category.
		/// </summary>
		public static List<DefLink> GetDefLinksForWebSchedNewPatApptApptTypes()
		{
			//Get all definitions that are associated to the WebSchedNewPatApptTypes category that are linked to an operatory.
			List<Definition> listWSNPAATDefs = Definitions.GetDefsForCategory(DefinitionCategory.WebSchedNewPatApptTypes);//Cannot hide defs of this category at this time.
																								//Return all of the deflinks that are of type Operatory in order to get the operatory specific deflinks.
			
			return GetDefLinksByTypeAndDefs(DefLinkType.AppointmentType, listWSNPAATDefs.Select(x => x.Id).ToList());
		}

		/// <summary>
		/// Gets one DefLinks by FKey. Must provide DefLinkType. 
		/// Returns null if not found.
		/// </summary>
		public static DefLink GetOneByFKey(long fKey, DefLinkType defType)
		{
			return GetListByFKeys(new List<long>() { fKey }, defType).FirstOrDefault();
		}

		/// <summary>
		/// Gets list of DefLinks by FKey. Must provide DefLinkType.
		/// </summary>
		public static List<DefLink> GetListByFKey(long fKey, DefLinkType defType)
		{
			return GetListByFKeys(new List<long>() { fKey }, defType);
		}

		/// <summary>
		/// Gets list of DefLinks by FKeys. Must provide DefLinkType.
		/// </summary>
		public static List<DefLink> GetListByFKeys(List<long> listFKeys, DefLinkType defType)
		{
			if (listFKeys.Count == 0)
			{
				return new List<DefLink>();
			}

			return Crud.DefLinkCrud.SelectMany(
				"SELECT * FROM deflink WHERE FKey IN (" + string.Join(",", listFKeys) + ") " +
				"AND LinkType =" + (int)defType);
		}

		public static DefLink GetOne(long defLinkNum)
		{
			return Crud.DefLinkCrud.SelectOne(defLinkNum);
		}

		public static long Insert(DefLink defLink)
		{
			return Crud.DefLinkCrud.Insert(defLink);
		}

		/// <summary>
		/// Inserts or updates the FKey entry for the corresponding definition passed in.
		/// This is a helper method that should only be used when there can only be a one to one relationship between DefNum and FKey.
		/// </summary>
		public static void SetFKeyForDef(long defNum, long fKey, DefLinkType linkType)
		{
			//No need to check RemotingRole; no call to db.
			//Look for the def link first to decide if we need to run an update or an insert statement.
			List<DefLink> listDefLinks = GetDefLinksByType(linkType, defNum);
			if (listDefLinks.Count > 0)
			{
				UpdateDefWithFKey(defNum, fKey, linkType);
			}
			else
			{
				Insert(new DefLink()
				{
					DefinitionId = defNum,
					FKey = fKey,
					LinkType = linkType,
				});
			}
		}

		/// <summary>
		/// Sync method for inserting all necessary DefNums associated to the operatory passed in.
		/// Does nothing if operatory.ListWSNPAOperatoryDefNums is null. 
		/// Will delete all deflinks if the list is empty.
		/// Optionally pass in the list of deflinks to consider in order to save database calls.
		/// </summary>
		public static void SyncWebSchedNewPatApptOpLinks(Operatory operatory, List<DefLink> listOpDefLinks = null)
		{
			if (operatory.ListWSNPAOperatoryDefNums == null)
			{
				return;//null means that this column was never even considered.  Save time by simply returning.
			}
			
			// Get all operatory deflinks from the database if a specific list was not passed in.
			listOpDefLinks ??= GetDefLinksForWebSchedNewPatApptOperatories();
			
			// Filter the deflinks down in order to get the current DefNums that are linked to the operatory passed in.
			listOpDefLinks = listOpDefLinks.Where(x => x.FKey == operatory.Id).ToList();
			
			// Delete all def links that are associated to DefNums that are not in listDefNums.
			List<DefLink> listDefLinksToDelete = listOpDefLinks.Where(x => !operatory.ListWSNPAOperatoryDefNums.Contains(x.DefinitionId)).ToList();
			DeleteDefLinks(listDefLinksToDelete.Select(x => x.Id).ToList());
			
			// Insert new DefLinks for all DefNums that were passed in that are not in listOpDefLinks.
			List<long> listDefNumsToInsert = operatory.ListWSNPAOperatoryDefNums.Where(x => !listOpDefLinks.Select(y => y.DefinitionId).Contains(x)).ToList();
			InsertDefLinksForDefs(listDefNumsToInsert, operatory.Id, DefLinkType.Operatory);
			
			// There is no reason to "update" deflinks so there is nothing else to do.
		}

		public static void InsertDefLinksForDefs(List<long> listDefNums, long fKey, DefLinkType linkType)
		{
			if (listDefNums == null || listDefNums.Count < 1)
			{
				return;
			}
			foreach (long defNum in listDefNums)
			{
				Insert(new DefLink()
				{
					DefinitionId = defNum,
					FKey = fKey,
					LinkType = linkType,
				});
			}
		}

		/// <summary>
		/// Creates multiple rows from a list of foreign keys using a single defnum and link type.
		/// </summary>
		public static void InsertDefLinksForFKeys(long defNum, List<long> listFKeys, DefLinkType linkType)
		{
			if (listFKeys.IsNullOrEmpty())
			{
				return;
			}

			Crud.DefLinkCrud.InsertMany(listFKeys.Select(x => new DefLink() { DefinitionId = defNum, FKey = x, LinkType = linkType }).ToList());
		}

		public static void Update(DefLink defLink)
		{
			Crud.DefLinkCrud.Update(defLink);
		}

		/// <summary>
		/// Updates the FKey column on all deflink rows for the corresponding definition and type.
		/// </summary>
		public static void UpdateDefWithFKey(long defNum, long fKey, DefLinkType defType)
		{
			Database.ExecuteNonQuery(
				"UPDATE deflink SET FKey=" + fKey + " WHERE LinkType=" + (int)defType + " AND DefNum=" + defNum);
		}

		/// <summary>Syncs two supplied lists of DefLink.</summary>
		public static bool Sync(List<DefLink> listNew, List<DefLink> listOld)
		{
			return Crud.DefLinkCrud.Sync(listNew, listOld);
		}

		public static void Delete(long defLinkNum)
		{
			Crud.DefLinkCrud.Delete(defLinkNum);
		}

		///<summary>Deletes all links for the specified FKey and link type.</summary>
		public static void DeleteAllForFKeys(List<long> listFKeys, DefLinkType defType)
		{
			if (listFKeys == null || listFKeys.Count < 1)
			{
				return;
			}

			string command = "DELETE FROM deflink "
				+ "WHERE LinkType=" + POut.Int((int)defType) + " "
				+ "AND FKey IN(" + string.Join(",", listFKeys.Select(x => POut.Long(x))) + ")";
			Database.ExecuteNonQuery(command);
		}

		/// <summary>Deletes all links for the specified definition and link type.</summary>
		public static void DeleteAllForDef(long defNum, DefLinkType defType)
		{

			string command = "DELETE FROM deflink WHERE LinkType=" + POut.Int((int)defType) + " AND DefNum=" + POut.Long(defNum);
			Database.ExecuteNonQuery(command);
		}

		public static void DeleteDefLinks(List<long> listDefLinkNums)
		{
			if (listDefLinkNums == null || listDefLinkNums.Count < 1)
			{
				return;
			}

			Database.ExecuteNonQuery(
				"DELETE FROM deflink WHERE DefLinkNum IN (" + string.Join(",", listDefLinkNums) + ")");
		}
	}
}
