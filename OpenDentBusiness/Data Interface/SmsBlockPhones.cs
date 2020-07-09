using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SmsBlockPhones{

		#region Cache Pattern

		private class SmsBlockPhoneCache : CacheListAbs<SmsBlockPhone> {
			protected override List<SmsBlockPhone> GetCacheFromDb() {
				string command="SELECT * FROM smsblockphone";
				return Crud.SmsBlockPhoneCrud.SelectMany(command);
			}
			protected override List<SmsBlockPhone> TableToList(DataTable table) {
				return Crud.SmsBlockPhoneCrud.TableToList(table);
			}
			protected override SmsBlockPhone Copy(SmsBlockPhone smsBlockPhone) {
				return smsBlockPhone.Copy();
			}
			protected override DataTable ListToTable(List<SmsBlockPhone> listSmsBlockPhones) {
				return Crud.SmsBlockPhoneCrud.ListToTable(listSmsBlockPhones,"SmsBlockPhone");
			}
			protected override void FillCacheIfNeeded() {
				SmsBlockPhones.GetTableFromCache(false);
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static SmsBlockPhoneCache _smsBlockPhoneCache=new SmsBlockPhoneCache();

		public static List<SmsBlockPhone> GetDeepCopy(bool isShort=false) {
			return _smsBlockPhoneCache.GetDeepCopy(isShort);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_smsBlockPhoneCache.FillCacheFromTable(table);
		}

		///<summary>Returns the cache in the form of a DataTable. Always refreshes the ClientWeb's cache.</summary>
		///<param name="doRefreshCache">If true, will refresh the cache if RemotingRole is ClientDirect or ServerWeb.</param> 
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			return _smsBlockPhoneCache.GetTableFromCache(doRefreshCache);
		}
		#endregion Cache Pattern

		#region Modification Methods
		#region Insert
		///<summary></summary>
		public static long Insert(SmsBlockPhone smsBlockPhone) {
			
			return Crud.SmsBlockPhoneCrud.Insert(smsBlockPhone);
		}
		#endregion
		#endregion
	}
}