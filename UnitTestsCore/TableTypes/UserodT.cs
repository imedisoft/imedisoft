﻿using System.Collections.Generic;
using CodeBase;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class UserodT {

		///<summary>Inserts the new user, refreshes the cache and then returns UserNum</summary>
		public static Userod CreateUser(string userName="",string password="",List<long> userGroupNumbers=null,long clinicNum=0,bool isClinicIsRestricted=false) {
			if(userName=="") {
				userName="Username"+MiscUtils.CreateRandomAlphaNumericString(8);
			}
			if(password=="") {
				password="1234";
			}
			if(userGroupNumbers==null) {
				userGroupNumbers=new List<long> { 1 };
			}
			Userod newUser=new Userod();
			newUser.UserName=userName;
			newUser.PasswordHash=Password.Hash(password);
			newUser.ClinicId=clinicNum;
			newUser.ClinicIsRestricted=isClinicIsRestricted;
			do {
				//In case the username is already taken
				try {
					newUser.Id=Userods.Insert(newUser,userGroupNumbers);
				}
				catch {
					newUser.UserName="Username"+MiscUtils.CreateRandomAlphaNumericString(8);
				}
			}while(newUser.Id==0);
			Userods.RefreshCache();
			UserGroupAttaches.RefreshCache();
			return newUser;
		}

		public static void ClearPasswords() {
			var users=Userods.GetAll();
			users.ForEach((System.Action<Userod>)(x => { x.PasswordHash=""; Userods.Update(x); }));
		}
	}
}
