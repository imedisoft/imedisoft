using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EmailAutographs{
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

		#region CachePattern

		private class EmailAutographCache : CacheListAbs<EmailAutograph> {
			protected override List<EmailAutograph> GetCacheFromDb() {
				string command="SELECT * FROM emailautograph ORDER BY Description";
				return Crud.EmailAutographCrud.SelectMany(command);
			}
			protected override List<EmailAutograph> TableToList(DataTable table) {
				return Crud.EmailAutographCrud.TableToList(table);
			}
			protected override EmailAutograph Copy(EmailAutograph emailAutograph) {
				return emailAutograph.Copy();
			}
			protected override DataTable ListToTable(List<EmailAutograph> listEmailAutographs) {
				return Crud.EmailAutographCrud.ListToTable(listEmailAutographs,"EmailAutograph");
			}
			protected override void FillCacheIfNeeded() {
				EmailAutographs.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static EmailAutographCache _emailAutographCache=new EmailAutographCache();

		public static List<EmailAutograph> GetDeepCopy(bool isShort=false) {
			return _emailAutographCache.GetDeepCopy(isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_emailAutographCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _emailAutographCache.GetTableFromCache(doRefreshCache);
		}

		#endregion
	
		/////<summary>Gets one EmailAutograph from the db.</summary>
		//public static EmailAutograph GetOne(long emailAutographNum){
		//	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
		//		return Meth.GetObject<EmailAutograph>(MethodBase.GetCurrentMethod(),emailAutographNum);
		//	}
		//	return Crud.EmailAutographCrud.SelectOne(emailAutographNum);
		//}
		
		///<summary>Insert one EmailAutograph in the database.</summary>
		public static long Insert(EmailAutograph emailAutograph){
			
			return Crud.EmailAutographCrud.Insert(emailAutograph);
		}
		
		///<summary>Updates an existing EmailAutograph in the database.</summary>
		public static void Update(EmailAutograph emailAutograph){
			
			Crud.EmailAutographCrud.Update(emailAutograph);
		}

		///<summary>Delete on EmailAutograph from the database.</summary>
		public static void Delete(long emailAutographNum) {
			
			Crud.EmailAutographCrud.Delete(emailAutographNum);
		}
	}
}