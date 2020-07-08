﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using WebServiceSerializer;
using CodeBase;
using System.IO;
using System.Windows.Forms;
using OpenDentBusiness.WebTypes;

namespace OpenDentBusiness {
	public static class WebServiceMainHQProxy {

		public static IWebServiceMainHQ MockWebServiceMainHQ {
			private get;//Use GetWebServiceMainHQInstance()
			set;
		}
		
		///<summary>Get an instance of the WebServicesHQ web service which includes the URL (pulled from PrefC). 
		///Optionally, you can provide the URL. This option should only be used by web apps which don't want to cause a call to PrefC.
		///Currently, OpenDentalREST and WebHostSynch hard-code the URL.</summary>
		public static IWebServiceMainHQ GetWebServiceMainHQInstance(string webServiceHqUrl="") {
			if(MockWebServiceMainHQ!=null) {
				return MockWebServiceMainHQ;
			}
			WebServiceMainHQReal service=new WebServiceMainHQReal();
#if ALPHA
			//Most beta applications should still set PrefName.WebServiceHQServerURL to the alpha address. This just makes it more fool proof for Alpha compiled applications.
			service.Url="https://10.10.1.184:49997/alpha/opendentalwebservicehq/webservicemainhq.asmx";
			return service;
#endif
			if(string.IsNullOrEmpty(webServiceHqUrl)) { //Default to the production URL.				
				service.Url=PrefC.GetString(PrefName.WebServiceHQServerURL);
			}
			else { //URL was provided so use that.
				service.Url=webServiceHqUrl;
			}
#if DEBUG
			//Change arguments for debug only.
			//service.Url="http://localhost/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx";//localhost
			service.Url="http://localhost/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx";
			//service.Url="http://10.10.2.18:55018/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx";//Sam's Computer
			//service.Url="https://server184:49997/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx";//The actual server hosting WebServiceMainHQ.
			//service.Url="http://sam/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx"; //Sams computer
			service.Timeout=(int)TimeSpan.FromMinutes(60).TotalMilliseconds;
#endif
			return service;
		}

		///<summary>Prior to 17.4 the user had the choice to install the eConnector but leave it disabled. 
		///As of 17.4 installing gives implied conset to also enable the servcie for communicating.</summary>
		public static ListenerServiceType SetEConnectorOn() {
			return WebSerializer.DeserializePrimitiveOrThrow<ListenerServiceType>(
					GetWebServiceMainHQInstance().SetEConnectorType(WebSerializer.SerializePrimitive<string>(PrefC.GetString(PrefName.RegistrationKey)),true));
		}

		///<summary>Throws exceptions.</summary>
		public static void BuildWebSchedNewPatApptURLs(List<long> listClinicNums,Action<XmlNode> aNodeParsed) {
			//Make a web call to HQ to get the URLs for all the clinics.
			string response=GetWebServiceMainHQInstance()
					.BuildWebSchedNewPatApptURLs(PrefC.GetString(PrefName.RegistrationKey),String.Join("|",listClinicNums));
			//Parse Response
			XmlDocument doc=new XmlDocument();
			XmlNode nodeError=null;
			XmlNode nodeResponse=null;
			XmlNodeList nodeURLs=null;
			//Invalid web service response passed in.  Node will be null and will throw correctly.
			ODException.SwallowAnyException(() => {
				doc.LoadXml(response);
				nodeError=doc.SelectSingleNode("//Error");
				nodeResponse=doc.SelectSingleNode("//GetWebSchedURLsResponse");
			});
			#region Error Handling
			if(nodeError!=null||nodeResponse==null) {
				string error=Lans.g("WebSched","There was an error with the web request.  Please try again or give us a call.");
				//Either something went wrong or someone tried to get cute and use our Web Sched service when they weren't supposed to.
				if(nodeError!=null) {
					error+="\r\n"+Lans.g("WebSched","Error Details")+":\r\n"+nodeError.InnerText;
				}
				throw new Exception(error);
			}
			nodeURLs=doc.GetElementsByTagName("URL");
			if(nodeURLs==null) {
				throw new Exception("Invalid response from server recieved.");
			}
			#endregion
			//At this point we know we got a valid response from our web service.
			//Loop through all the URL nodes that were returned.
			foreach(XmlNode node in nodeURLs) {
				aNodeParsed(node);
			}
		}
		
		///<summary>Test the connection to WebServiceMainHQ</summary>
		public static bool CanReachWebService() {
			try {
				GetWebServiceMainHQInstance().TestConnection("");
				//Communication succeeded so that is considered a pass.
				return true;
			}
			catch (Exception e) {
				e.DoNothing();
				return false;
			}
		}

		#region EService setup
		///<summary>Called by local practice db to query HQ for EService setup info. Must remain very lite and versionless. Will be used by signup portal.
		///If HasClinics==true then any SignupOut.EServices entries where ClinicNum==0 are invalid and should be ignored.
		///If HasClinics==false then SignupOut.EServices should only pay attention items where ClinicNum==0.
		///This list is kept completely unfiltered by ClinicNum for forward compatibility reasons. 
		///The ClinicNum 0 items are always used by the Signup portal to determine default signup preferences.
		///However, these items are only used for validation and billing in the case where HasClinics==true.</summary>
		public static EServiceSetup.SignupOut GetEServiceSetupFull(SignupPortalPermission permission,bool isSwitchClinicPref=false,EServiceSetup.SignupOut oldSignupOut=null) {
			//Clinics will be stored in this order at HQ to allow signup portal to display them in proper order.
			List<Clinic> clinics=Clinics.GetDeepCopy().OrderBy(x => x.ItemOrder).ToList();
			if(PrefC.GetBool(PrefName.ClinicListIsAlphabetical)) {
				clinics=clinics.OrderBy(x => x.Abbr).ToList();
			}
			string shortCodePracticeTitle=string.IsNullOrWhiteSpace(PrefC.GetString(PrefName.ShortCodeOptInClinicTitle))
				? PrefC.GetString(PrefName.PracticeTitle) : PrefC.GetString(PrefName.ShortCodeOptInClinicTitle);
			bool isMockChanged=false;
			if(ODBuild.IsDebug() && WebServiceMainHQProxy.MockWebServiceMainHQ==null && !Environment.MachineName.In("CHRISM","ANDREWD","SAM","LINDSAYS")) {
				WebServiceMainHQProxy.MockWebServiceMainHQ=new WebServiceMainHQMockDemo();
				isMockChanged=true;
			}
			EServiceSetup.SignupOut signupOut=WebSerializer.ReadXml<EServiceSetup.SignupOut>
				(
					WebSerializer.DeserializePrimitiveOrThrow<string>
					(
						GetWebServiceMainHQInstance().EServiceSetup
						(
							PayloadHelper.CreatePayload
							(
								WebSerializer.WriteXml(new EServiceSetup.SignupIn() {
									MethodNameInt=(int)EServiceSetup.SetupMethod.GetSignupOutFull,
									HasClinics=PrefC.HasClinicsEnabled,
									//ClinicNum is not currently used as input.
									ClinicNum=0,
									ProgramVersionStr=PrefC.GetString(PrefName.ProgramVersion),
									SignupPortalPermissionInt=(int)permission,
									Clinics=clinics
										.Select(x => new EServiceSetup.SignupIn.ClinicLiteIn() {
											ClinicNum=x.ClinicNum,
											ClinicTitle=x.Abbr,
											IsHidden=x.IsHidden,
											//Use the ClinicPref as an override, otherwise, Clinic.Abbr is used, if blank, finally use practice title.
											ShortCodeOptInYourDentist=ClinicPrefs.GetPref(PrefName.ShortCodeOptInClinicTitle,x.ClinicNum)?.ValueString
												?? (string.IsNullOrWhiteSpace(x.Description) ? shortCodePracticeTitle : x.Description),
										}).ToList(),
									IsSwitchClinicPref=isSwitchClinicPref,
									//Use the ClinicNum=0 ClinicPref as an override, otherwise, PracticeTitle.
									ShortCodeOptInYourDentist=shortCodePracticeTitle,
								}),eServiceCode.Undefined
							)
						)
					)
				);
			if(ODBuild.IsDebug() && isMockChanged) {
				WebServiceMainHQProxy.MockWebServiceMainHQ=null;
			}
			//We just got the latest sync info from HQ so update the local db to reflect what HQ says is true.
			#region Reconcile Phones
			List<SmsPhone> listPhonesHQ=EServiceSetup.SignupOut.SignupOutPhone.ToSmsPhones(signupOut.Phones);
			bool isCacheInvalid=false;
			isCacheInvalid|=SmsPhones.UpdateOrInsertFromList(listPhonesHQ);
			#endregion
			#region Reconcile practice and clinics
			List<EServiceSetup.SignupOut.SignupOutSms> smsSignups=GetSignups<EServiceSetup.SignupOut.SignupOutSms>(signupOut,eServiceCode.IntegratedTexting);
			bool isSmsEnabled=false;
			if(PrefC.HasClinicsEnabled) { //Clinics are ON so loop through all clinics and reconcile with HQ.
				List<Clinic> listClinicsAll=Clinics.GetDeepCopy();
				foreach(Clinic clinicDb in listClinicsAll) {
					WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutSms clinicSignup=
						smsSignups.FirstOrDefault(x => x.ClinicNum==clinicDb.ClinicNum)??new WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutSms() {
							//Not found so turn it off.
							SmsContractDate=DateTime.MinValue,
							MonthlySmsLimit=0,
							IsEnabled=false,
						};
					Clinic clinicNew=clinicDb.Copy();
					clinicNew.SmsContractDate=clinicSignup.SmsContractDate;
					clinicNew.SmsMonthlyLimit=clinicSignup.MonthlySmsLimit;
					isCacheInvalid|=Clinics.Update(clinicNew,clinicDb);
					isSmsEnabled|=clinicSignup.IsEnabled;
					isCacheInvalid|=ImportHQShortCodeSettings(clinicSignup);
				}	
				WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutSms practiceSignup=
					smsSignups.FirstOrDefault(x => x.ClinicNum==0)??new EServiceSetup.SignupOut.SignupOutSms();
				//Must also import Short Code settings for the default clinic.
				isCacheInvalid|=ImportHQShortCodeSettings(practiceSignup);
			}
			else { //Clinics are off so ClinicNum 0 is the practice clinic.
				WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutSms practiceSignup=
					smsSignups.FirstOrDefault(x => x.ClinicNum==0)??new WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutSms() {
						//Not found so turn it off.
						SmsContractDate=DateTime.MinValue,
						MonthlySmsLimit=0,
						IsEnabled=false,
					};
				isCacheInvalid
					|=Prefs.UpdateDateT(PrefName.SmsContractDate,practiceSignup.SmsContractDate)
					|Prefs.UpdateLong(PrefName.TextingDefaultClinicNum,0)
					|Prefs.UpdateDouble(PrefName.SmsMonthlyLimit,practiceSignup.MonthlySmsLimit);
				isSmsEnabled|=practiceSignup.IsEnabled;
				isCacheInvalid|=ImportHQShortCodeSettings(practiceSignup);
			}
			#endregion
			#region Reconcile CallFire
			//Turn off CallFire if SMS has been activated.
			//This only happens the first time SMS is turned on and CallFire is still activated.
			if(isSmsEnabled && Programs.IsEnabled(ProgramName.CallFire)) {
				Program callfire=Programs.GetCur(ProgramName.CallFire);
				if(callfire!=null) {
					callfire.Enabled=false;
					Programs.Update(callfire);
					Signalods.Insert(new Signalod() { IType=InvalidType.Providers });
					signupOut.Prompts.Add("Call Fire has been disabled. Cancel Integrated Texting and access program properties to retain Call Fire.");
				}
			}
			#endregion
			#region eConfirmations
			if(Prefs.UpdateBool(PrefName.ApptConfirmAutoSignedUp,IsEServiceActive(signupOut,eServiceCode.ConfirmationRequest))) {
				//HQ does not match the local pref. Make it match with HQ.
				isCacheInvalid=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Automated appointment eConfirmations automatically changed by HQ.  Local pref set to "
					+IsEServiceActive(signupOut,eServiceCode.ConfirmationRequest).ToString()+".");
			}
			#endregion
			#region eClipboard
			List<long> listEClipboardClinicNums=signupOut.EServices.Where(x => x.EService==eServiceCode.EClipboard && x.IsEnabled)
				.Select(x => x.ClinicNum).ToList();
			string clinicNumsEClipboard=string.Join(",",listEClipboardClinicNums);
			if(Prefs.UpdateString(PrefName.EClipboardClinicsSignedUp,clinicNumsEClipboard)) {
				//HQ didn't match the local pref.
				isCacheInvalid=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,$"eClipboard clinics signed up changed. Local pref set to {clinicNumsEClipboard}");
				MobileAppDevices.UpdateIsAllowed(listEClipboardClinicNums);
			}
			#endregion
			#region MobileWeb/ODMobile
			//Capture list of subscribers before sync.
			List<long> listMobileWebClinicNumsBefore=MobileAppDevices.GetClinicSignedUpForMobileWeb();
			//Capture list of subscribers after sync.
			List<long> listMobileWebClinicNumsAfter=signupOut.EServices.Where(x => x.EService==eServiceCode.MobileWeb && x.IsEnabled)
				.Select(x => x.ClinicNum).ToList();
			//Shut down any existing ODMobile session for clinics which were just unsubscribed.
			//NOTE: It is very important that this run BEFORE we update the pref below. 
			//The push handler will check the pref we are about the change below and block unsubscribed clinics.
			listMobileWebClinicNumsBefore
				//All ClinicNums which were included before the sync but not included after the sync (no longer subscribed to ODMobile).
				.Where(x => !listMobileWebClinicNumsAfter.Contains(x)).Distinct()
				//Logout all these clinics' users.
				.ForEach(x => { PushNotificationUtils.ODM_LogoutClinic(x); });
			string clinicNumsMobileWeb=string.Join(",",listMobileWebClinicNumsAfter);
			if(Prefs.UpdateString(PrefName.MobileWebClinicsSignedUp,clinicNumsMobileWeb)) {
				//HQ didn't match the local pref.
				isCacheInvalid=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,$"MobileWeb clinics signed up changed. Local pref set to {clinicNumsMobileWeb}");
			}
			#endregion
			#region MassEmail
			//Sync mass email prefs by clinic.
			List<EServiceSetup.SignupOut.SignupOutEService> massEmailSignups=signupOut.EServices.Where(x => x.EService==eServiceCode.EmailMassUsage).ToList();
			List<ClinicPref> massEmailPrefs=ClinicPrefs.GetPrefAllClinics(PrefName.MassEmailStatus).ToList();
			foreach(EServiceSetup.SignupOut.SignupOutEService massEmailSignup in massEmailSignups){
				ClinicPref massEmailPref=massEmailPrefs.FirstOrDefault(x => x.ClinicNum==massEmailSignup.ClinicNum);
				int fromPref=0;
				if(massEmailPref!=null){
					fromPref=PIn.Int(massEmailPref.ValueString,false);
				}
				MassEmailStatus existingFlags=(MassEmailStatus)fromPref;
				if(massEmailSignup.IsEnabled) { //Add flag.
					existingFlags=existingFlags.AddFlag(MassEmailStatus.Activated);
				}
				else{ //Remove flag.
					existingFlags=existingFlags.RemoveFlag(MassEmailStatus.Activated);
				}
				//If we made it this far, we have activated that account successfully.
				ClinicPrefs.Upsert(PrefName.MassEmailStatus,massEmailSignup.ClinicNum,((int)existingFlags).ToString());
			}
			#endregion
			#region WebSchedASAP
			if(Prefs.UpdateBool(PrefName.WebSchedAsapEnabled,IsEServiceActive(signupOut,eServiceCode.WebSchedASAP))) {
				isCacheInvalid=true;
				SecurityLogs.MakeLogEntry(Permissions.EServicesSetup,0,"Web Sched ASAP automatically changed by HQ. Local pref set to "
					+IsEServiceActive(signupOut,eServiceCode.WebSchedASAP).ToString()+".");
			}
			#endregion WebSchedASAP
			#region Preferences
			isCacheInvalid|=UpdatePreferences(signupOut.PushablePrefs);
			#endregion
			if(isCacheInvalid) { //Something changed in the db. Alert other workstations and change this workstation immediately.
				Signalods.Insert(new Signalod() { IType=InvalidType.Prefs });
				Prefs.RefreshCache();
				Signalods.Insert(new Signalod() { IType=InvalidType.Providers });
				Providers.RefreshCache();
				Clinics.RefreshCache();
				Signalods.Insert(new Signalod() { IType=InvalidType.SmsPhones });
				SmsPhones.RefreshCache();
				Signalods.Insert(new Signalod() { IType=InvalidType.ClinicPrefs });
				ClinicPrefs.RefreshCache();
			}
			LogEServiceSetup(signupOut,oldSignupOut);
			return signupOut;
		}

		///<summary>Takes a list of preferences from HQ and, if a corresponding OpenDentBusiness.Pref exists, updates Pref.ValueString.</summary>
		private static bool UpdatePreferences(List<EServiceSetup.SignupOut.PushablePref> listPrefs) {
			bool isCacheInvalid=false;
			foreach(EServiceSetup.SignupOut.PushablePref pushablePref in listPrefs) {
				if(!Prefs.GetContainsKey(pushablePref.PrefName)) {//This eConnector is on a version behind HQ.
					continue;
				}
				Pref preference=Prefs.GetPref(pushablePref.PrefName);
				if(preference.ValueString==pushablePref.ValueString) {//No change.
					continue;
				}
				preference.ValueString=pushablePref.ValueString;
				Prefs.Update(preference);
				isCacheInvalid=true;
				//If this preference does not exist at the dental office yet, disregard.
			}
			return isCacheInvalid;
		}

		private static void LogEServiceSetup(EServiceSetup.SignupOut signupNew, EServiceSetup.SignupOut signupOld=null) {
			bool includeSecurityLogEntry=false;
			string logText=$"GetEServicesSetupFull called by user {Security.CurUser.UserName} ({Security.CurUser.UserNum})";
			Lookup<long,EServiceSetup.SignupOut.SignupOutEService> signupsLookupNew=
				(Lookup<long,EServiceSetup.SignupOut.SignupOutEService>)signupNew.EServices.ToLookup(x => x.ClinicNum);
			#region comparative logging
			if(signupOld!=null) {
				logText+="Summary of changes made to EServiceSetup: ";
				string logChangesSummary="";
				#region EService Signups
				Lookup<long,EServiceSetup.SignupOut.SignupOutEService> signupsLookupOld=
					(Lookup<long,EServiceSetup.SignupOut.SignupOutEService>)signupOld.EServices.ToLookup(x => x.ClinicNum);
				//New iterate through each other clinic to see if there were changes
				foreach(long clinicNum in signupsLookupNew.Where(x => signupsLookupOld.Any(y => y.Key==x.Key)).Select(x => x.Key)) {
					string logChangesClinicSummary="";
					List<EServiceSetup.SignupOut.SignupOutEService> listEServicesOld=signupsLookupOld[clinicNum].ToList();
					List<EServiceSetup.SignupOut.SignupOutEService> listEServicesNew=signupsLookupNew[clinicNum].ToList();
					//Then handle changes where the signup already existed
					foreach(EServiceSetup.SignupOut.SignupOutEService eServiceNew in listEServicesNew.Where(x => listEServicesOld.Any(y => y.EService==x.EService))) {
						EServiceSetup.SignupOut.SignupOutEService eServiceOld=listEServicesOld.First(x => x.EService==eServiceNew.EService);
						//The eService is being enabled
						if(eServiceNew.DateTimeStart!=eServiceOld.DateTimeStart) {
							logChangesClinicSummary+=$"{Environment.NewLine}   " +
								$"{eServiceNew.EService} was set to start on {eServiceNew.DateTimeStart}.";
						}
						//The eService is being disabled or possible re-enabled
						if(eServiceNew.DateTimeStop!=eServiceOld.DateTimeStop) {
							if(eServiceNew.DateTimeStop==DateTime.MinValue) {
								logChangesClinicSummary+=$"{Environment.NewLine}   " +
									$"{eServiceNew.EService} was re-enabled.";
							}
							else {
								logChangesClinicSummary+=$"{Environment.NewLine}   " +
									$"{eServiceNew.EService} was set to stop on {eServiceNew.DateTimeStop}.";
							}
						}
						if(eServiceNew.GetType()==typeof(EServiceSetup.SignupOut.SignupOutSms)) {
							EServiceSetup.SignupOut.SignupOutSms smsOld=(EServiceSetup.SignupOut.SignupOutSms)eServiceOld;
							EServiceSetup.SignupOut.SignupOutSms smsNew=(EServiceSetup.SignupOut.SignupOutSms)eServiceNew;
							if(smsOld.MonthlySmsLimit!=smsNew.MonthlySmsLimit) {
								logChangesClinicSummary+=$"{Environment.NewLine}   {smsNew.EService} " +
									$"MonthlySmsLimit change from {smsOld.MonthlySmsLimit} to {smsNew.MonthlySmsLimit}";
							}
							if(smsOld.SmsContractDate!=smsNew.SmsContractDate) {
								logChangesClinicSummary+=$"{Environment.NewLine}   {smsNew.EService} " +
									$"SmsContractDate change from {smsOld.SmsContractDate} to {smsNew.SmsContractDate}";
							}
							if(smsOld.ShortCodeTypeFlags!=smsNew.ShortCodeTypeFlags) {
								string shortCodesOld=string.Join("|",smsOld.ShortCodeTypeFlags.GetFlags().Select(x => x.GetDescription()));
								string shortCodesNew=string.Join("|",smsNew.ShortCodeTypeFlags.GetFlags().Select(x => x.GetDescription()));
								logChangesClinicSummary+=$"{Environment.NewLine}   {smsNew.EService} " +
									$"ShortCodeApptReminderTypes changed from {shortCodesOld} to {shortCodesNew}";
							}
						}
					}
					#endregion EService Signups
					#region Phones
					//Finally handle any changes made to phones
					List<EServiceSetup.SignupOut.SignupOutPhone> listPhonesOld=signupOld.Phones.FindAll(x => x.ClinicNum==clinicNum);
					List<EServiceSetup.SignupOut.SignupOutPhone> listPhonesNew=signupNew.Phones.FindAll(x => x.ClinicNum==clinicNum);
					//The case where there are phones in the old list that aren't in the new one
					foreach(EServiceSetup.SignupOut.SignupOutPhone phoneOld in listPhonesOld.Where(x => !listPhonesNew.Any(y => y.PhoneNumber==x.PhoneNumber))) {
						logChangesClinicSummary+=$"{Environment.NewLine}   " +
							$"Removed Phone Number: {phoneOld.PhoneNumber}";
					}
					//The case where there are phones in the new list that weren't in the old one
					foreach(EServiceSetup.SignupOut.SignupOutPhone phoneNew in listPhonesNew.Where(x => !listPhonesOld.Any(y => y.PhoneNumber==x.PhoneNumber))) {
						logChangesClinicSummary+=$"{Environment.NewLine}   " +
							$"Added Phone Number: {phoneNew.PhoneNumber}";
					}
					//Then handle changes where the phone already existed
					foreach(EServiceSetup.SignupOut.SignupOutPhone phoneNew in listPhonesNew.Where(x => listPhonesOld.Any(y => y.PhoneNumber==x.PhoneNumber))) {
						EServiceSetup.SignupOut.SignupOutPhone phoneOld=listPhonesOld.First(x => x.PhoneNumber==phoneNew.PhoneNumber);
						if(phoneOld.IsActivated!=phoneNew.IsActivated) {
							logChangesClinicSummary+=$"{Environment.NewLine}   " +
								$"PhoneNumber {phoneNew.PhoneNumber} was {(phoneNew.IsActivated?"Activated":"Deactivated")}";
						}
						if(phoneOld.IsPrimary!=phoneNew.IsPrimary) {
							logChangesClinicSummary+=$"{Environment.NewLine}   " +
								$"PhoneNumber {phoneNew.PhoneNumber} was {(phoneNew.IsPrimary?"set to":"removed as")} Primary phone.";
						}
						if(phoneOld.InactiveCode!=phoneNew.InactiveCode) {
							logChangesClinicSummary+=$"{Environment.NewLine}   PhoneNumber {phoneNew.PhoneNumber} " +
								$"InactiveCode changed from {phoneOld.InactiveCode} to {phoneNew.InactiveCode}";
						}
					}
					#endregion Phones
					//Add this clinic to the log if needed
					if(!string.IsNullOrWhiteSpace(logChangesClinicSummary)) {
						logChangesSummary+=$"{(Environment.NewLine)}Changes made to Clinic {Clinics.GetAbbr(clinicNum)} ({clinicNum})";
						logChangesSummary+=logChangesClinicSummary;
					}
				}
				//If we haven't recorded any changes, indicate this in the log, otherwise add the changes we did record here to the log
				logText+=string.IsNullOrWhiteSpace(logChangesSummary)?"No changes were made.":logChangesSummary;
				//We want to create a securitylog entry only if changes were made
				includeSecurityLogEntry=!string.IsNullOrWhiteSpace(logChangesSummary); 
			}
			#endregion comparative logging
			#region signupNew
			foreach(long clinicNum in signupsLookupNew.Select(x => x.Key)) {
				logText+=$"{(string.IsNullOrWhiteSpace(logText) ? "" : Environment.NewLine)}Clinic: {Clinics.GetAbbr(clinicNum)} ({clinicNum})";
				foreach(EServiceSetup.SignupOut.SignupOutEService signupOutEService in signupsLookupNew[clinicNum]) {
					string statusText="Disabled"; //if signupOutEService.IsEnabled is disabled that's the only state it can have so this is "default"
					if(signupOutEService.IsEnabled) {
						if(signupOutEService.DateTimeStop.Year>1880) {
							statusText="Pending Stop";
						}
						else {
							statusText="Enabled";
						}
					}
					logText+=$@"{Environment.NewLine}   {signupOutEService.EService.ToString()} ({statusText})";
					//If this is an SMS signup log additional data
					if(signupOutEService.GetType()==typeof(EServiceSetup.SignupOut.SignupOutSms)) {
						EServiceSetup.SignupOut.SignupOutSms signupOutSms=(EServiceSetup.SignupOut.SignupOutSms)signupOutEService;
						logText+=$@"
      MonthlySmsLimit: {signupOutSms.MonthlySmsLimit}
      SmsContractDate: {signupOutSms.SmsContractDate}
      CountryCode: {signupOutSms.CountryCode}";
					}
				}
				//Add phones for the clinic to the 
				if(signupNew.Phones.Any(x => x.ClinicNum==clinicNum)) {
					logText+=$"{Environment.NewLine}   Phones";
					foreach(EServiceSetup.SignupOut.SignupOutPhone phone in signupNew.Phones.Where(x => x.ClinicNum==clinicNum)) {
						logText+=$@"
      PhoneNumber: {phone.PhoneNumber}
         IsActivated: {(phone.IsActivated?"Yes":"No")}
         IsPrimay: {(phone.IsPrimary?"Yes":"No")}
         DateTimeActive: {phone.DateTimeActive}
         DateTimeInactive: {phone.DateTimeInactive}
         CountryCode: {phone.CountryCode}
         InactiveCode: {phone.InactiveCode}";
					}
				}
			}
			#endregion signupNew
			//Insert the log as an EServiceSignal
			long eServiceSignalNum=OpenDentBusiness.EServiceSignals.Insert(new EServiceSignal() { 
				ServiceCode=(int)eServiceCode.SignupPortal,
				Severity=eServiceSignalSeverity.Info,
				Description=logText,
				SigDateTime=DateTime.Now,
			});
			if(includeSecurityLogEntry) {
				SecurityLogs.MakeLogEntry(Permissions.Setup,patNum:0,logText:
					$"Changes were made to EServiceSetup. See EServiceSignalNum {eServiceSignalNum} for more details.");
			}
		}

		///<summary>Sets PrefName.ShortCodeApptReminderTypes for the clinic based on settings at HQ.</summary>
		private static bool ImportHQShortCodeSettings(WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutSms clinicSignup) {
			if(clinicSignup.ShortCodeTypeFlags==ShortCodeTypeFlag.None) {
				//No need to insert or update ClinicPrefs for ShortCodeTypeFlag.None.  Just delete it if it exists.
				return (ClinicPrefs.DeletePrefs(clinicSignup.ClinicNum,new List<PrefName>() { PrefName.ShortCodeApptReminderTypes })>0);
			}
			//Upsert clinicpref to match HQ.
			return ClinicPrefs.Upsert(PrefName.ShortCodeApptReminderTypes,clinicSignup.ClinicNum,POut.Int((int)clinicSignup.ShortCodeTypeFlags));
		}

		///<summary>Called by local practice db to query HQ for EService setup info. Must remain very lite and versionless. Will be used by signup portal.
		///If any args are null or not provided then they will be retrieved from Prefs.</summary>
		public static EServiceSetup.SignupOut GetEServiceSetupLite(
			SignupPortalPermission permission,string registrationKey=null,string practiceTitle=null,string practicePhone=null,string programVersion=null) 
		{
			return WebSerializer.ReadXml<EServiceSetup.SignupOut>
				(
					WebSerializer.DeserializePrimitiveOrThrow<string>
					(
						GetWebServiceMainHQInstance().EServiceSetup
						(
							PayloadHelper.CreatePayload
							(
								WebSerializer.WriteXml(new EServiceSetup.SignupIn() {
									MethodNameInt=(int)EServiceSetup.SetupMethod.GetEServiceSetupLite,
									SignupPortalPermissionInt=(int)permission,
								}),
								eServiceCode.Undefined,
								//null is allowed for the rest. Will get converted to RegKey Pref.
								registrationKey, 
								practiceTitle,
								practicePhone,
								programVersion
							)
						)
					)
				);
		}

		///<summary>Called by local practice db to query HQ for EService setup info. Must remain very lite and versionless. 
		///This method will determine if clinics are enabled and HQ will include ClinicNum 0 in the output without it first being included in the input.
		///If clinics not enabled and an emptly list is passed in, then this method will automatically validate against the practice clinic (ClinicNum 0).</summary>
		public static List<long> GetEServiceClinicsAllowed(List<long> listClinicNums,eServiceCode eService) {
			if(MockWebServiceMainHQ!=null) {
				//Using the mock implementation because calling the real guts of this method would be difficult in testing because you would have to have a
				//database setup with signups for eServices.
				return MockWebServiceMainHQ.GetEServiceClinicsAllowed(listClinicNums,eService);
			}
			EServiceSetup.SignupOut signupOut=WebSerializer.ReadXml<EServiceSetup.SignupOut>
			(
				WebSerializer.DeserializePrimitiveOrThrow<string>
				(
					GetWebServiceMainHQInstance().EServiceSetup
					(
						PayloadHelper.CreatePayload
						(
							WebSerializer.WriteXml(new EServiceSetup.SignupIn() {
								MethodNameInt=(int)EServiceSetup.SetupMethod.ValidateClinics,
								HasClinics=PrefC.HasClinicsEnabled,
								SignupPortalPermissionInt=(int)SignupPortalPermission.ReadOnly,
								Clinics=listClinicNums.Select(x => new EServiceSetup.SignupIn.ClinicLiteIn() { ClinicNum=x }).ToList(),
							}),
							eService
						)
					)
				)
			);
			//Include the "Practice" clinic if the calling method didn't specifically provide ClinicNum 0.
			if(!signupOut.HasClinics && !listClinicNums.Contains(0)) {
				listClinicNums.Add(0);
			}
			return signupOut.EServices
				//We only care about the input eService.
				.FindAll(x => x.EService==eService)
				//Must be included in the HQ output list in order to be considered valid.
				.FindAll(x => listClinicNums.Any(y => y == x.ClinicNum))
				.Select(x => x.ClinicNum).ToList();
		}

		///<summary>Makes web service call to HQ and validates if the given eService is allowed for the given clinic.
		///Set clinicNum==0 to check for a non-clinics practice. Returns true if allowed; otherwise returns false.
		///This is just a wrapper for GetEServiceClinicsAllowed().</summary>
		public static bool IsEServiceClinicAllowed(long clinicNum,eServiceCode eService) {
			return GetEServiceClinicsAllowed(new List<long> { clinicNum },eService).Any(x => x==clinicNum);
		}

		///<summary>Makes web service call to HQ and validates if the given eService is allowed for the practice.
		///True indicates that at least 1 clinic and/or the practice iteself is signed up.
		///Use IsEServiceClinicAllowed() or GetEServiceClinicsAllowed() if you care about specific clinics.
		///This is just a wrapper for GetEServiceClinicsAllowed().</summary>
		public static bool IsEServiceAllowed(eServiceCode eService) {
			return GetEServiceClinicsAllowed(Clinics.GetDeepCopy().Select(x => x.ClinicNum).ToList(),eService).Count>0;
		}

		///<summary>Helper to interpret output from GetEServiceSetupFull().
		///Gets all signups found for the given eService.</summary>
		public static List<T> GetSignups<T>(EServiceSetup.SignupOut signupOut,eServiceCode eService) where T : WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService {
			return signupOut.EServices
				.FindAll(x => x.EService==eService)
				.Cast<T>().ToList();
		}

		///<summary>Helper to interpret output from GetEServiceSetupFull().
		///Indicates if HQ says this account is registered for the given eService.</summary>
		public static bool IsEServiceActive(EServiceSetup.SignupOut signupOut,eServiceCode eService) {
			return GetSignups<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>(signupOut,eService)
				.Any(x => x.IsEnabled);
		}

		///<summary>Used to send EServiceClinic(s) to and from HQ. Must remain very lite and versionless. Will be used by signup portal.</summary>
		public class EServiceSetup {
			///<summary>Input of WebServiceHQ.EServiceSetup() web method.</summary>
			public class SignupIn {
				///<summary>All clinics belonging to local db. Required for lite version? NO.</summary>
				public List<ClinicLiteIn> Clinics;
				///<summary>Try to convert this to SignupPortalPermission enum in OD. If not found then omit this list item. Required for all versions? YES.</summary>
				public int SignupPortalPermissionInt;
				///<summary>Local DB ClinicNum. Can be 0. Required for lite version? NO.</summary>
				public long ClinicNum;
				///<summary>Should be convertible to Version. Required for lite version? NO.</summary>
				public string ProgramVersionStr;
				///<summary>Indicates if this practice has the clinics feature turned on.</summary>
				public bool HasClinics=false;
				///<summary>Try to convert this to SetupMethod enum in OD. This flag will determine which method is called by WebServiceHQ. Required for all versions? YES.</summary>
				public int MethodNameInt;
				///<summary>Request is coming from customer in FormShowFeatures. Customer has elected to turn 'Clinics' feature on or off. 
				///Back-end will detect this flag and change foreign keys to match new setup for this customer.</summary>
				public bool IsSwitchClinicPref=false;
				///<summary>ClinicPref.  Individual clinics can specify a Clinic Title to use in the Short Code Opt In Reply, substitutes [YourDentist]
				///"You'll now receive appointment messages from [YourDentist] Reply HELP for Help, Reply STOP to cancel. Msg&data rates may apply."
				///</summary>
				public string ShortCodeOptInYourDentist;

				[XmlIgnore]
				public SetupMethod MethodName {
					get {
						if(Enum.IsDefined(typeof(SetupMethod),MethodNameInt)) {
							return (SetupMethod)MethodNameInt;
						}
						return SetupMethod.Undefined;
					}
				}

				[XmlIgnore]
				public SignupPortalPermission PortalPermission {
					get {
						if(Enum.IsDefined(typeof(SignupPortalPermission),SignupPortalPermissionInt)) {
							return (SignupPortalPermission)SignupPortalPermissionInt;
						}
						return SignupPortalPermission.Denied;
					}
				}
				
				[XmlIgnore]
				public Version ProgramVersion {
					get {
						Version ret;
						if(!Version.TryParse(ProgramVersionStr,out ret)) {
							ret=new Version(0,0);
						}
						return ret;
					}
				}

				///<summary>Lite version of HQ EServiceClinic table. Versionless so keep simple and do not remove or rename fields.</summary>
				public class ClinicLiteIn {
					public long ClinicNum;
					public string ClinicTitle;
					public bool IsHidden;
					public string ShortCodeOptInYourDentist;
				}
			}

			///<summary>Output of WebServiceHQ.EServiceSetup() web method. Set SignupIn.IsLiteVersion=true for lite version.</summary>
			public class SignupOut {
				///<summary>What type of listener is being used by this office. Included in lite version? YES.</summary>
				public int ListenerTypeInt;
				///<summary>Clinic based description of EService signup status. 
				///Full version will included SignupOutSms for IntegratedTexting entries. 
				///Lite version includes only SignupOutEService objects, even for IntegratedTexting. Included in lite version? YES.</summary>
				public List<SignupOutEService> EServices;
				///<summary>Any phones owned by this reg key. Included in lite version? NO.</summary>
				public List<SignupOutPhone> Phones;
				///<summary>Navigates to signup portal using given SignupIn inputs. Included in lite version? NO.</summary>
				public string SignupPortalUrl;
				///<summary>Indicates if this practice has the clinics feature turned on.</summary>
				public bool HasClinics=false;
				///<summary>Try to convert this to SignupPortalPermission enum in OD. If not found then omit this list item. Required for lite version? NO.</summary>
				public int SignupPortalPermissionInt;
				///<summary>Try to convert this to SetupMethod enum in OD. This flag will determine which method is called by WebServiceHQ. Required for all versions? YES.</summary>
				public int MethodNameInt;
				///<summary>List of preferences that will be updated at the dental office to the value set by HQ.</summary>
				public List<PushablePref> PushablePrefs;
				///<summary>This list will be filled by GetEServiceSetupFull(). 
				///If any Prompts are added then they should typically be shown to the user. They will usually indicate that a local pref of some sort has been changed.</summary>
				[XmlIgnore]
				public List<string> Prompts=new List<string>();
				
				[XmlIgnore]
				public SetupMethod MethodName {
					get {
						if(Enum.IsDefined(typeof(SetupMethod),MethodNameInt)) {
							return (SetupMethod)MethodNameInt;
						}
						return SetupMethod.Undefined;
					}
				}

				[XmlIgnore]
				public SignupPortalPermission PortalPermission {
					get {
						if(Enum.IsDefined(typeof(SignupPortalPermission),SignupPortalPermissionInt)) {
							return (SignupPortalPermission)SignupPortalPermissionInt;
						}
						return SignupPortalPermission.Denied;
					}
				}

				[XmlIgnore]
				public ListenerServiceType ListenerType {
					get {
						if(Enum.IsDefined(typeof(ListenerServiceType),ListenerTypeInt)) {
							return (ListenerServiceType)ListenerTypeInt;
						}
						return ListenerServiceType.DisabledByHQ;
					}
				}

				public class SignupOutSms:SignupOutEService {
					///<summary>Monthly amount spent on texting which this clinic does not want to exceed.</summary>
					public double MonthlySmsLimit;
					///<summary>The start date of the texting service.</summary>
					public DateTime SmsContractDate;
					///<summary>Country code linked to this clinic for the purpose of Integrated Texting. String version of ISO31661.</summary>
					public string CountryCode;
					///<summary>Indicates this clinic is setup to use Short Codes for certain sms services.</summary>
					public int ShortCodeTypeFlagsInt;

					[XmlIgnore]
					public ShortCodeTypeFlag ShortCodeTypeFlags {
						get {
							return (ShortCodeTypeFlag)((ShortCodeTypeFlag)ShortCodeTypeFlagsInt).GetFlags().Sum(x => (int)x);
						}
					}
				}

				///<summary>Used to push values to OpenDentBusiness.Pref entries at the dental office.</summary>
				public class PushablePref {
					public string PrefName;
					public string ValueString;
				}

				///<summary>Lite version of HQ EServiceSignup table. Versionless so keep simple and do not remove or rename fields.</summary>
				[XmlInclude(typeof(SignupOutSms))] //Allows sub-class to be serialized without custom serializer.
				public class SignupOutEService {
					public long ClinicNum;
					///<summary>Try to convert this to eServiceCode enum in OD. If not found then omit this list item.</summary>
					public int EServiceCodeInt;
					///<summary>URL used for this EService for this clinic. Only applies in some scenarios, otherwise empty.</summary>
					public string HostedUrl;
					///<summary>URL used for the Patient Portal EService for this clinic to make payments quickly. Only applies in some scenarios, otherwise empty.</summary>
					public string HostedUrlPayment;
					///<summary>From HQ RepeatCharge and HQ EServiceSignup.</summary>
					public bool IsEnabled;
					///<summary>DateTime at which this signup becomes active.</summary>
					public DateTime DateTimeStart;
					///<summary>DateTime at which this signup becomes inactive, or if account is active this is DateTime.MinimumValue</summary>
					public DateTime DateTimeStop;

					[XmlIgnore]
					public eServiceCode EService {
						get {
							if(Enum.IsDefined(typeof(eServiceCode),EServiceCodeInt)) {
								return (eServiceCode)EServiceCodeInt;
							}
							return eServiceCode.Undefined;
						}
					}
				}

				///<summary>Lite version of SmsPhone table. Versionless so keep simple and do not remove or rename fields.</summary>
				public class SignupOutPhone {
					public long ClinicNum;
					public string PhoneNumber;
					public string CountryCode;
					public DateTime DateTimeActive;
					public DateTime DateTimeInactive;
					public string InactiveCode;
					public bool IsActivated;
					public bool IsPrimary;

					public static List<SmsPhone> ToSmsPhones(List<SignupOutPhone> listSignupPhones) {
						return listSignupPhones.Select(x => new SmsPhone() {
							ClinicNum=x.ClinicNum,
							CountryCode=x.CountryCode,
							DateTimeActive=x.DateTimeActive,
							DateTimeInactive=x.DateTimeInactive,
							InactiveCode=x.InactiveCode,
							PhoneNumber=x.PhoneNumber,
							IsPrimary=x.IsPrimary,
						}).ToList();
					}
				}				
			}

			///<summary>Order matters for serialization. Do not change order.</summary>
			public enum SetupMethod {
				Undefined,
				GetSignupOutFull,
				GetEServiceSetupLite,
				ValidateClinics,
			}
		}
		#endregion

		///<summary>Gets the specified number of short GUIDs</summary>
		///<returns>First item in the Tuple is the short GUID. Second item is the URL for the short GUID if there is one for this eService.</returns>
		public static List<ShortGuidResult> GetShortGUIDs(int numberToGet,int numberForSms,long clinicNum,eServiceCode eService) {
			List<PayloadItem> listPayloadItems=new List<PayloadItem> {
				new PayloadItem(clinicNum,"ClinicNum"),
				new PayloadItem(numberToGet,"NumberShortGUIDsToGet"),
				new PayloadItem(numberForSms,"NumberShortGUIDsForSMS"),
			};
			string officeData=PayloadHelper.CreatePayload(PayloadHelper.CreatePayloadContent(listPayloadItems),eService);
			string result=WebServiceMainHQProxy.GetWebServiceMainHQInstance().GenerateShortGUIDs(officeData);
			return WebSerializer.DeserializeTag<List<ShortGuidResult>>(result,"ListShortGuidResults");
		}

		///<summary>Sends payload to HQ with version update information to be added to bugs database in CustomerVersion table.</summary>
		public static void UpdateCustomerVersionHistory() {
			string updateServerName=PrefC.GetString(PrefName.WebServiceServerName).ToLower();
			if(updateServerName!="" && !ODEnvironment.IdIsThisComputer(updateServerName)){//When set, only send data from update server
				return;
			}
			List<UpdateHistory> listUpdates=UpdateHistories.GetPreviousUpdateHistories(2);//Sorted by UpdateHistory.DateTimeUpdated DESC
			string versionFrom;
			string versionTo;
			if(listUpdates.Count>0 && listUpdates[0].DateTimeUpdated.Date!=DateTime.Today) {//Only send out version info to HQ on same day as update.)
				return;
			}
			else if(listUpdates.Count==2) {
				versionFrom=listUpdates[1].ProgramVersion;
				versionTo=listUpdates[0].ProgramVersion;
			}
			else {//Most likely a new customer with 1 or 0 updates.
				versionFrom=(listUpdates.Count==1?listUpdates[0].ProgramVersion:"0.0.0.0");
				versionTo=PrefC.GetString(PrefName.ProgramVersion);
			}
			List<PayloadItem> listPayloadItems=new List<PayloadItem>(){
				new PayloadItem(versionFrom,"VersionFrom"),
				new PayloadItem(versionTo,"VersionTo")
			};
			GetWebServiceMainHQInstance().CustomerUpdateCommitted(PayloadHelper.CreatePayload(listPayloadItems,eServiceCode.CustomerVersion));
		}

		///<summary>WebServiceMainHQ.GenerateShortGUIDs returns a list of these.</summary>
		[Serializable]
		public class ShortGuidResult {
			public string ShortGuid;
			public string ShortURL;
			public string MediumURL;
			public bool IsForSms;
		}
	}



}
