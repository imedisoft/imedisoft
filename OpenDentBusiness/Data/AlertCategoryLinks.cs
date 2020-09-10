using Imedisoft.Data;
using Imedisoft.Data.Cache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenDentBusiness
{
    public partial class AlertCategoryLinks
	{
		[CacheGroup(nameof(InvalidType.AlertCategoryLinks))]
		private class AlertCategoryLinkCache : ListCache<AlertCategoryLink>
		{
			protected override IEnumerable<AlertCategoryLink> Load()
				=> SelectMany("SELECT * FROM alertcategorylink");
		}

		private static readonly AlertCategoryLinkCache cache = new AlertCategoryLinkCache();

		public static List<AlertCategoryLink> GetWhere(Predicate<AlertCategoryLink> match) 
			=> cache.Find(match);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static AlertCategoryLink GetOne(long alertCategoryLinkNum) 
			=> SelectOne(alertCategoryLinkNum);

		public static List<AlertCategoryLink> GetForCategory(long alertCategoryNum)
		{
			if (alertCategoryNum == 0)
			{
				return new List<AlertCategoryLink>();
			}

			return SelectMany(
				"SELECT * FROM alertcategorylink WHERE AlertCategoryNum = " + alertCategoryNum).ToList();
		}

		public static void DeleteForCategory(long alertCategoryNum)
			=> Database.ExecuteNonQuery("DELETE FROM alertcategorylink WHERE AlertCategoryNum = " + alertCategoryNum);

		/// <summary>
		/// Inserts, updates, or deletes db rows to match listNew.
		/// No need to pass in userNum, it's set before remoting role check and passed to the server if necessary.
		/// Doesn't create ApptComm items, but will delete them.  If you use Sync, you must create new AlertCategoryLink items.
		/// </summary>
		public static bool Sync(List<AlertCategoryLink> listNew, List<AlertCategoryLink> listDB)
		{
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<AlertCategoryLink> listIns = new List<AlertCategoryLink>();
			List<AlertCategoryLink> listUpdNew = new List<AlertCategoryLink>();
			List<AlertCategoryLink> listUpdDB = new List<AlertCategoryLink>();
			List<AlertCategoryLink> listDel = new List<AlertCategoryLink>();
			listNew.Sort((AlertCategoryLink x, AlertCategoryLink y) => { return x.Id.CompareTo(y.Id); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((AlertCategoryLink x, AlertCategoryLink y) => { return x.Id.CompareTo(y.Id); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew = 0;
			int idxDB = 0;
			int rowsUpdatedCount = 0;
			AlertCategoryLink fieldNew;
			AlertCategoryLink fieldDB;
			//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.
			//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.
			while (idxNew < listNew.Count || idxDB < listDB.Count)
			{
				fieldNew = null;
				if (idxNew < listNew.Count)
				{
					fieldNew = listNew[idxNew];
				}
				fieldDB = null;
				if (idxDB < listDB.Count)
				{
					fieldDB = listDB[idxDB];
				}
				//begin compare
				if (fieldNew != null && fieldDB == null)
				{//listNew has more items, listDB does not.
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if (fieldNew == null && fieldDB != null)
				{//listDB has more items, listNew does not.
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				else if (fieldNew.Id < fieldDB.Id)
				{//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if (fieldNew.Id > fieldDB.Id)
				{//dbPK less than newPK, dbItem is 'next'
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				//Both lists contain the 'next' item, update required
				listUpdNew.Add(fieldNew);
				listUpdDB.Add(fieldDB);
				idxNew++;
				idxDB++;
			}
			//Commit changes to DB
			for (int i = 0; i < listIns.Count; i++)
			{
				Insert(listIns[i]);
			}

			for (int i = 0; i < listUpdNew.Count; i++)
			{
				if (Update(listUpdNew[i], listUpdDB[i]))
				{
					rowsUpdatedCount++;
				}
			}

			for (int i = 0; i < listDel.Count; i++)
			{
				Delete(listDel[i].Id);
			}

			if (rowsUpdatedCount > 0 || listIns.Count > 0 || listDel.Count > 0)
			{
				return true;
			}
			return false;
		}
	}
}
