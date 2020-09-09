using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using Imedisoft.Data;

namespace OpenDentBusiness{
	///<summary></summary>
	public class UserGroupAttaches{
		//If this table type will exist as cached data, uncomment the Cache Pattern region below and edit.

		#region Cache Pattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add GetTableFromCache and FillCacheFromTable to the Cache.cs file with all the other Cache types.
		//Also, consider making an invalid type for this class in Cache.GetAllCachedInvalidTypes() if needed.

		private class UserGroupAttachCache : CacheListAbs<UserGroupAttach> {
			protected override List<UserGroupAttach> GetCacheFromDb() {
				string command="SELECT * FROM usergroupattach";
				return Crud.UserGroupAttachCrud.SelectMany(command);
			}
			protected override List<UserGroupAttach> TableToList(DataTable table) {
				return Crud.UserGroupAttachCrud.TableToList(table);
			}
			protected override UserGroupAttach Copy(UserGroupAttach userGroupAttach) {
				return userGroupAttach.Copy();
			}
			protected override DataTable ListToTable(List<UserGroupAttach> listUserGroupAttaches) {
				return Crud.UserGroupAttachCrud.ListToTable(listUserGroupAttaches,"UserGroupAttach");
			}
			protected override void FillCacheIfNeeded() {
				UserGroupAttaches.GetTableFromCache(false);
			}
			//protected override bool IsInListShort(UserGroupAttach userGroupAttach) {
			//	return true;//Either change this method or delete it.
			//}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static UserGroupAttachCache _userGroupAttachCache=new UserGroupAttachCache();

		public static List<UserGroupAttach> GetWhere(Predicate<UserGroupAttach> match,bool isShort=false) {
			return _userGroupAttachCache.GetWhere(match,isShort);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			//No need to check RemotingRole; no call to db.
			_userGroupAttachCache.FillCacheFromTable(table);
		}

		///<summary>Returns the cache in the form of a DataTable. Always refreshes the ClientWeb's cache.</summary>
		///<param name="doRefreshCache">If true, will refresh the cache if RemotingRole is ClientDirect or ServerWeb.</param> 
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			return _userGroupAttachCache.GetTableFromCache(doRefreshCache);
		}

		public static void RefreshCache() {
			GetTableFromCache(true);
		}
		#endregion Cache Pattern
		
		#region Get Methods
		///<summary>Returns all usergroupattaches for a single user from the cache.</summary>
		public static List<UserGroupAttach> GetForUser(long userNum) {
			//No need to check RemotingRole; no call to db.
			return GetWhere(x => x.UserId == userNum);
		}

		public static List<UserGroupAttach> GetForUserGroup(long usergroupNum) {
			//No need to check RemotingRole; no call to db.
			return GetWhere(x => x.UserGroupId== usergroupNum);
		}

		///<summary>Gets all UserGroupAttaches from the database where the associated users or usergroups' CEMTNums are not 0.</summary>
		public static List<UserGroupAttach> GetForCEMTUsersAndUserGroups() {
			
			string command = @"
				SELECT usergroupattach.* 
				FROM usergroupattach
				INNER JOIN userod ON userod.UserNum = usergroupattach.UserNum
					AND userod.UserNumCEMT != 0";
			return Crud.UserGroupAttachCrud.SelectMany(command);
		}

		///<summary>Pass in a list of CEMT usergroupattaches, and this will return a list of corresponding local usergroupattaches.</summary>
		public static List<UserGroupAttach> TranslateCEMTToLocal(List<UserGroupAttach> listUserGroupAttachCEMT) {
			List<UserGroupAttach> retVal = new List<UserGroupAttach>();
			List<Userod> listRemoteUsers = Userods.GetUsersNoCache();
			List<UserGroup> listRemoteGroups = UserGroups.GetCEMTGroupsNoCache().ToList();
			foreach(UserGroupAttach attachCur in listUserGroupAttachCEMT) {
				Userod userCur = listRemoteUsers.FirstOrDefault(x => attachCur.UserId == x.UserIdCEMT);
				UserGroup userGroupCur = listRemoteGroups.FirstOrDefault(x => attachCur.UserGroupId == x.CentralUserGroupId);
				if(userCur == null || userGroupCur == null) {
					continue;
				}
				UserGroupAttach userGroupAttachNew = new UserGroupAttach() {
					UserId = userCur.Id,
					UserGroupId = userGroupCur.Id
				};
				retVal.Add(userGroupAttachNew);
			}
			return retVal;
		}

		#endregion
		#region Modification Methods
		#region Insert
		///<summary></summary>
		public static long Insert(UserGroupAttach userGroupAttach) {
			return Crud.UserGroupAttachCrud.Insert(userGroupAttach);
		}
		#endregion
		#region Update
		///<summary>Manually sync the database on the lists passed in. This does not check the PKs of the items in either list.
		///Instead, it only cares about info in the UserGroupNum and UserNum columns.
		///Returns the number of rows that were changed. Currently only used in the CEMT tool.</summary>
		public static long SyncCEMT(List<UserGroupAttach> listNew,List<UserGroupAttach> listOld) {
			//This remoting role check isn't necessary but will save on network traffic
			
			//the users and usergroups in listNew correspond to UserNumCEMTs and UserGroupNumCEMTs.

			// - If a row with the same UserGroupNum and UserNum exists in ListNew that does not exist in list Old, add it to listAdd.
			// - If a row with the same UserGroupNum and UserNum exists in ListOld that does not exist in ListNew, add it to listDel.
			List<UserGroupAttach> listAdd = new List<UserGroupAttach>();
			List<UserGroupAttach> listDel = new List<UserGroupAttach>();
			long rowsChanged = 0;
			foreach(UserGroupAttach userGroupAtt in listNew) {
				if(!listOld.Exists(x => x.UserGroupId == userGroupAtt.UserGroupId && x.UserId == userGroupAtt.UserId)) {
					listAdd.Add(userGroupAtt);
				}
			}
			foreach(UserGroupAttach userGroupAtt in listOld) {
				if(!listNew.Exists(x => x.UserGroupId == userGroupAtt.UserGroupId && x.UserId == userGroupAtt.UserId)) {
					listDel.Add(userGroupAtt);
				}
			}
			//make sure that there is only one unique (UserGroup, UserGroupNum) row in the add list. (this is precautionary)
			listAdd = listAdd.GroupBy(x => new { x.UserId,x.UserGroupId }).Select(x => x.First()).ToList();
			//Get users and user groups from remote db to compare against for log entrys
			List<Userod> listRemoteUsers = Userods.GetUsersNoCache();
			List<UserGroup> listRemoteGroups = UserGroups.GetCEMTGroupsNoCache().ToList();
			foreach(UserGroupAttach userGroupAdd in listAdd) {
				rowsChanged++;
				UserGroupAttaches.Insert(userGroupAdd);
				Userod user=listRemoteUsers.FirstOrDefault(x => x.Id==userGroupAdd.UserId);
				UserGroup userGroup=listRemoteGroups.FirstOrDefault(x => x.Id==userGroupAdd.UserGroupId);
				SecurityLogs.MakeLogEntryNoCache(Permissions.SecurityAdmin,0,"User: "+user.UserName+" added to user group: "
					+userGroup.Description+" by CEMT user: "+Security.CurrentUser.UserName);
			}
			foreach(UserGroupAttach userGroupDel in listDel) {
				rowsChanged++;
				UserGroupAttaches.Delete(userGroupDel);
				Userod user=listRemoteUsers.FirstOrDefault(x => x.Id==userGroupDel.UserId);
				UserGroup userGroup=listRemoteGroups.FirstOrDefault(x => x.Id==userGroupDel.UserGroupId);
				SecurityLogs.MakeLogEntryNoCache(Permissions.SecurityAdmin,0,"User: "+user.UserName+" removed from user group: "
					+userGroup.Description+" by CEMT user: "+Security.CurrentUser.UserName);
			}
			return rowsChanged;
		}

		#endregion
		#region Delete
		public static void Delete(UserGroupAttach userGroupAttach) {
			
			Crud.UserGroupAttachCrud.Delete(userGroupAttach.UserGroupAttachNum);
		}

		///<summary>Does not add a new usergroupattach if the passed-in userCur is already attached to userGroup.</summary>
		public static void AddForUser(Userod userCur,long userGroupNum) {
			
			if(!Userods.IsInUserGroup(userCur.Id, userGroupNum)) {
				UserGroupAttach userGroupAttach = new UserGroupAttach();
				userGroupAttach.UserGroupId = userGroupNum;
				userGroupAttach.UserId = userCur.Id;
				Crud.UserGroupAttachCrud.Insert(userGroupAttach);
			}
		}

		///<summary>Pass in the user and all of the userGroups that the user should be attached to.
		///Detaches the userCur from any usergroups that are not in the given list.
		///Returns a count of how many user group attaches were affected.</summary>
		public static long SyncForUser(Userod userCur,List<long> listUserGroupNums) {
			
			long rowsChanged=0;
			foreach(long userGroupNum in listUserGroupNums) {
				if(!Userods.IsInUserGroup(userCur.Id, userGroupNum)) {
					UserGroupAttach userGroupAttach = new UserGroupAttach();
					userGroupAttach.UserGroupId = userGroupNum;
					userGroupAttach.UserId = userCur.Id;
					Crud.UserGroupAttachCrud.Insert(userGroupAttach);
					rowsChanged++;
				}
			}
			foreach(UserGroupAttach userGroupAttach in UserGroupAttaches.GetForUser(userCur.Id)) {
				if(!listUserGroupNums.Contains(userGroupAttach.UserGroupId)) {
					Crud.UserGroupAttachCrud.Delete(userGroupAttach.UserGroupAttachNum);
					rowsChanged++;
				}
			}
			return rowsChanged;
		}

		#endregion
		#endregion
		#region Misc Methods

		#endregion



	}
}