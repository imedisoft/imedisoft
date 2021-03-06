using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class WikiListHeaderWidths{
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

		///<summary>Used temporarily.</summary>
		public static string DummyColName="Xrxzes";

		#region CachePattern

		private class WikiListHeaderWidthCache : CacheListAbs<WikiListHeaderWidth> {
			protected override List<WikiListHeaderWidth> GetCacheFromDb() {
				string command="SELECT * FROM wikilistheaderwidth";
				return Crud.WikiListHeaderWidthCrud.SelectMany(command);
			}
			protected override List<WikiListHeaderWidth> TableToList(DataTable table) {
				return Crud.WikiListHeaderWidthCrud.TableToList(table);
			}
			protected override WikiListHeaderWidth Copy(WikiListHeaderWidth wikiListHeaderWidth) {
				return wikiListHeaderWidth.Copy();
			}
			protected override DataTable ListToTable(List<WikiListHeaderWidth> listWikiListHeaderWidths) {
				return Crud.WikiListHeaderWidthCrud.ListToTable(listWikiListHeaderWidths,"WikiListHeaderWidth");
			}
			protected override void FillCacheIfNeeded() {
				WikiListHeaderWidths.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static WikiListHeaderWidthCache _wikiListHeaderWidthCache=new WikiListHeaderWidthCache();

		public static List<WikiListHeaderWidth> GetWhere(Predicate<WikiListHeaderWidth> match,bool isShort=false) {
			return _wikiListHeaderWidthCache.GetWhere(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_wikiListHeaderWidthCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			return _wikiListHeaderWidthCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		///<summary>Returns header widths for list sorted in the same order as the columns appear in the DB.  Uses cache.</summary>
		public static List<WikiListHeaderWidth> GetForList(string listName) {
			List<WikiListHeaderWidth> retVal = new List<WikiListHeaderWidth>();
			string command = "DESCRIBE wikilist_"+POut.String(listName);//TODO: Oracle compatible?
			DataTable tableDescripts = Database.ExecuteDataTable(command);
			List<WikiListHeaderWidth> listHeaderWidths=GetWhere(x => x.ListName==listName);
			for(int i = 0;i<tableDescripts.Rows.Count;i++) {
				WikiListHeaderWidth addWidth = listHeaderWidths.Where(x => x.ColName == tableDescripts.Rows[i][0].ToString()).FirstOrDefault();
				if(addWidth!=null) {
					retVal.Add(addWidth);
				}
			}
			return retVal;
		}

		///<summary>Also alters the db table for the list itself.  Throws exception if number of columns does not match.</summary>
		public static void UpdateNamesAndWidths(string listName,List<WikiListHeaderWidth> columnDefs) {
			string command="DESCRIBE wikilist_"+POut.String(listName);
			DataTable tableListDescription=Database.ExecuteDataTable(command);
			if(tableListDescription.Rows.Count!=columnDefs.Count) {
				throw new ApplicationException("List schema has been altered. Unable to save changes to list.");
			}
			List<string> listChanges=new List<string>();
			//rename Columns with dummy names in case user is renaming a new column with an old name.---------------------------------------------
			for(int i=1;i<tableListDescription.Rows.Count;i++) {//start with index 1 to skip PK col
				DataRow row=tableListDescription.Rows[i];
				listChanges.Add($"CHANGE {POut.String(row[0].ToString())} {POut.String(DummyColName+i)} TEXT NOT NULL");
				command=$@"UPDATE wikilistheaderwidth SET ColName='{POut.String(DummyColName+i)}'
					WHERE ListName='{POut.String(listName)}'
					AND ColName='{POut.String(row[0].ToString())}'";
				Database.ExecuteNonQuery(command);
			}
			Database.ExecuteNonQuery($"ALTER TABLE wikilist_{POut.String(listName)} {string.Join(",",listChanges)}");
			listChanges=new List<string>();
			//rename columns names and widths-------------------------------------------------------------------------------------------------------
			for(int i=1;i<tableListDescription.Rows.Count;i++) {//start with index 1 to skip PK col
				WikiListHeaderWidth wCur=columnDefs[i];
				listChanges.Add($"CHANGE {POut.String(DummyColName+i)} {POut.String(wCur.ColName)} TEXT NOT NULL");
				command=$@"UPDATE wikilistheaderwidth
					SET ColName='{POut.String(wCur.ColName)}',ColWidth='{POut.Int(wCur.ColWidth)}',PickList='{POut.String(wCur.PickList)}'
					WHERE ListName='{POut.String(listName)}'
					AND ColName='{POut.String(DummyColName+i)}'";
				Database.ExecuteNonQuery(command);
			}
			Database.ExecuteNonQuery($"ALTER TABLE wikilist_{POut.String(listName)} {string.Join(",",listChanges)}");
			//handle width of PK seperately because we do not rename the PK column, ever.
			command=$@"UPDATE wikilistheaderwidth
				SET ColWidth='{POut.Int(columnDefs[0].ColWidth)}',
				PickList='{POut.String(columnDefs[0].PickList)}'
				WHERE ListName='{POut.String(listName)}'
				AND ColName='{POut.String(columnDefs[0].ColName)}'";
			Database.ExecuteNonQuery(command);
			RefreshCache();
		}

		///<summary>No error checking. Only called from WikiLists.</summary>
		public static void InsertNew(WikiListHeaderWidth newWidth) {
			Crud.WikiListHeaderWidthCrud.Insert(newWidth);
			RefreshCache();
		}

		public static void InsertMany(List<WikiListHeaderWidth> listNewWidths) {
			Crud.WikiListHeaderWidthCrud.InsertMany(listNewWidths);
			RefreshCache();
		}

		///<summary>No error checking. Only called from WikiLists after the corresponding column has been dropped from its respective table.</summary>
		public static void Delete(string listName,string colName) {
			string command = "DELETE FROM wikilistheaderwidth WHERE ListName='"+POut.String(listName)+"' AND ColName='"+POut.String(colName)+"'";
			Database.ExecuteNonQuery(command);
			RefreshCache();
		}

		public static void DeleteForList(string listName) {
			string command = "DELETE FROM wikilistheaderwidth WHERE ListName='"+POut.String(listName)+"'";
			Database.ExecuteNonQuery(command);
			RefreshCache();
		}

		public static List<WikiListHeaderWidth> GetFromListHist(WikiListHist wikiListHist) {
			List<WikiListHeaderWidth> retVal=new List<WikiListHeaderWidth>();
			string[] arrayHeaders=wikiListHist.ListHeaders.Split(';');
			for(int i=0;i<arrayHeaders.Length;i++) {
				WikiListHeaderWidth hw=new WikiListHeaderWidth();
				hw.ListName=wikiListHist.ListName;
				hw.ColName=arrayHeaders[i].Split(',')[0];
				hw.ColWidth=PIn.Int(arrayHeaders[i].Split(',')[1]);
				retVal.Add(hw);
			}
			return retVal;
		}

	}
}