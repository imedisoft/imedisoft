using System.Collections.Generic;
using System.Data;

namespace OpenDentBusiness
{
    /// <summary>
    /// The purpose of this class is to provide a shared read lock and an exclusive write lock on a cache.
    /// </summary>
    public abstract class CacheAbs<T> where T : TableBase
	{
		/// <summary>
		/// Called at the end of FillCache which tells all implementors to refresh their caches with the new items within listItemsAll.
		/// </summary>
		protected abstract void OnNewCacheReceived(List<T> listItemsAll);
		
		/// <summary>
		/// This method queries the database for the list of objects and return it.
		/// </summary>
		protected abstract List<T> GetCacheFromDb();
		
		/// <summary>
		/// This method takes in a DataTable and turns it into a list of objects.
		/// </summary>
		protected abstract List<T> TableToList(DataTable table);

		protected abstract T Copy(T tableBase);

		/// <summary>
		/// Returns the main cache entity as a DataTable.
		/// </summary>
		protected abstract DataTable CacheToTable();

		/// <summary>
		/// After this method has run, both the client's and the server's cache should be initialized.
		/// </summary>
		protected abstract void FillCacheIfNeeded();

		/// <summary>
		/// Return true if the cache entity is null.
		/// </summary>
		protected abstract bool IsCacheNull();

		/// <summary>
		/// Fills the cache using the specified source. 
		/// If source is Database, then table can be null.
		/// </summary>
		private void FillCache(FillCacheSource source, DataTable table)
		{
			List<T> listItemsAll = new List<T>();

			if (source == FillCacheSource.Database)
			{
				listItemsAll = GetCacheFromDb();
			}
			else if (source == FillCacheSource.DataTable)
			{
				listItemsAll = TableToList(table);
			}

			OnNewCacheReceived(listItemsAll);
		}

		/// <summary>
		/// Fills the cache using the provided DataTable. Thread safe.
		/// </summary>
		public void FillCacheFromTable(DataTable table)
		{
			FillCache(FillCacheSource.DataTable, table);
		}

		/// <summary>
		/// Gets the table from the cache.
		/// </summary>
		public DataTable GetTableFromCache(bool doRefreshCache)
		{
			if (IsCacheNull() || doRefreshCache)
			{
				FillCache(FillCacheSource.Database, null);
			}

			return CacheToTable();
		}

		/// <summary>
		/// The source that the cache will be filled from.
		/// </summary>
		private enum FillCacheSource
		{
			/// <summary>
			/// Cache is to be filled from the database.
			/// </summary>
			Database,

			/// <summary>
			/// Cache is to be filled using the provided DataTable.
			/// </summary>
			DataTable
		}
	}
}
