using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase;
using Newtonsoft.Json;

namespace OpenDentBusiness.AutoComm {
	public class Arrivals {
		private IEnumerable<ApptReminderSent> ListArrivalsSent=new List<ApptReminderSent>();
		private IEnumerable<ApptReminderRule> ListApptReminderRules=new List<ApptReminderRule>();
		private TagReplacer _tagReplacer;

		private Arrivals() {
			_tagReplacer=new ArrivalsTagReplacer();
		}

		public static Arrivals LoadArrivals() {
			return new Arrivals();
		}

		///<summary>If necessary, gets ApptReminderSents and corresponding ApptReminderRules for given appointments from database.</summary>
		public static Arrivals LoadArrivals(List<long> listClinicNums,List<long> listApptNums) {
			Arrivals arrivals=LoadArrivals();
			if(!PrefC.HasClinicsEnabled) {
				listClinicNums=new List<long> { 0 };
			}
			List<long> listSignedUpClinics=listClinicNums.Where(x => ClinicPrefs.GetBool(PrefName.ApptConfirmAutoSignedUp,x)).ToList();
			//Only do the work of looking up ApptReminderSents and ApptReminderRules if the appropriate eServices are enabled and in use.
			if(!listApptNums.IsNullOrEmpty() && listSignedUpClinics.Any() && PrefC.GetBool(PrefName.ApptArrivalAutoEnabled)) {
				arrivals.ListApptReminderRules=GetApptReminderRules(listSignedUpClinics);
				if(arrivals.ListApptReminderRules.Any()) {
					//This clinic has at least one Reminder Rule with an arrival/come-in template defined.  We now know we need ApptReminderSent data.
					arrivals.ListArrivalsSent=ApptReminderSents.GetForApt(listApptNums.ToArray());
				}
			}
			return arrivals;
		}

		///<summary>Gets Arrival ApptReminderRules, including rules for clinics that are using default rules.</summary>
		private static List<ApptReminderRule> GetApptReminderRules(List<long> listClinicNums) {
			List<ApptReminderRule> listRules=ApptReminderRules.GetForTypes(ApptReminderType.Arrival);
			List<ApptReminderRule> listRulesDefault=listRules.Where(x => x.ClinicNum==0).ToList();
			//Make sure a rule is included for clinics using defaults.
			foreach(long clinicNum in listClinicNums) {
				Clinic clinic=Clinics.GetClinic(clinicNum);
				if(clinic?.IsConfirmDefault??false) {//Clinic found
					listRules.AddRange(listRulesDefault.Select(x => x.CopyWithClinicNum(clinicNum)));
				}
			}
			return listRules.Where(x => x.IsEnabled && x.AreArrivalResponsesSetup && x.ClinicNum.In(listClinicNums))//Filtered to enabled/setup/signedup
					.ToList();
		}

		///<summary>Processes an inboud SMS and determines if it is an "I have arrived" message and office is setup to send an automatic Arrival 
		///Response.</summary>
		public static void ProcessArrival(SmsFromMobile sms) {
			if(sms.MsgTotal!=1 || sms.MsgText.ToLower().Trim()!=ArrivalsTagReplacer.ARRIVED_CODE.ToLower().Trim()) {//Not an "Arrived" sms.
				return;
			}
			//Get all patients in the family from here.  It's possible an appointment for a dependent triggered an arrival message but the sms was sent to 
			//the guarantor.  The response will be sent to the number that texted 'A', but will apply to any appointments in the family (at the same clinic)
			//for today.  All these appointments will be marked 'Arrived' as well.
			List<long> listFam=Patients.GetAllFamilyPatNums(sms.PatNum.SingleItemToList());
			ProcessArrival(sms.PatNum,listFam,sms.ClinicNum,sms.MobilePhoneNumber);
		}

		///<summary>Determines if office is setup to send an automatic Arrival Response for the given patient.</summary>
		public static void ProcessArrival(long patNumForResponse,List<long> listPatNums,long clinicNum,string mobilePhoneNumber=null
			,List<Appointment> listAppts=null) {
			if(patNumForResponse<=0) {//Invalid patNum.
				return;
			}
			//Run Arrival Processing on a thread, because we do not want this action, which includes a web call, to slow down the UI, as everything is
			//happening behind the scenes anyway.
			ODThread arrivalThread=new ODThread(o => {
				List<Appointment> listTodayAppts=(listAppts??Appointments.GetAppointmentsForPat(listPatNums.ToArray()))
					.Where(x => x.ClinicNum==clinicNum && x.AptDateTime.Date==DateTime.Today).ToList();
				Arrivals arrival=LoadArrivals(clinicNum.SingleItemToList(),listTodayAppts.Select(x => x.AptNum).ToList());
				arrival.TryProcessArrival(patNumForResponse,clinicNum,mobilePhoneNumber,listTodayAppts);
			});
			arrivalThread.AddExceptionHandler((ex) => Logger.LogError(MiscUtils.GetExceptionText(ex)));
			arrivalThread.Name=nameof(ProcessArrival)+$"_PatNum{patNumForResponse}";
			arrivalThread.GroupName=nameof(ProcessArrival);
			arrivalThread.Start();
		}

		///<summary>Determines if the patient corresponding to the SmsFromMobile can be sent an Arrival Response and marked as Arrived.</summary>
		private bool TryProcessArrival(long patNum,long clinicNum,string mobilePhoneNumber,List<Appointment> listAppts) {
			bool retVal=false;
			List<Appointment> listApptsToday=listAppts.Where(x => x.ClinicNum==clinicNum && x.AptDateTime.Date==DateTime.Today).OrderBy(x => x.AptDateTime).ToList();
			string logSubDir=ODFileUtils.CombinePaths(nameof(Arrivals),nameof(TryProcessArrival),clinicNum.ToString());
			if(listApptsToday.Count==0) {
				Logger.LogError($"PatNum: {patNum} does not have any appointments at ClinicNum {clinicNum} today.");
				return false;
			}
			List<Appointment> listApptsAutomationEnabled=listApptsToday
				//Check if (given clinic exists and has automation enabled) or (HQ "clinic" and Arrivals are enabled)
				.Where(x => Clinics.GetClinic(x.ClinicNum)?.IsConfirmEnabled??(x.ClinicNum==0 && PrefC.GetBool(PrefName.ApptArrivalAutoEnabled)))
				.ToList();
			if(listApptsAutomationEnabled.Count==0) {
				Logger.LogError($"PatNum: {patNum} has appointments at ClinicNum {clinicNum} today, but automation is not enabled for this clinic.");
				return false;
			}
			List<ApptResponse> listApptResponses=GetApptResponses(listApptsAutomationEnabled);
			if(!listApptResponses.IsNullOrEmpty()) {
				//There is a configured Arrival Response for this patient, then mark as arrived and send sms.
				MarkArrived(listApptResponses.Select(x => x.Appointment));
				string message=AppendEClipboardTokens(listApptResponses);
				retVal=TrySendArrivalResponseSms(patNum,mobilePhoneNumber,clinicNum,message,logSubDir);
			}
			return retVal;
		}

		private string AppendEClipboardTokens(List<ApptResponse> listApptResponses) {				
			//Here is where we will also look at eClipboard prefs to see if we should include the eClipboard token.  We need to aggregate all tokens into
			//one response message.
			return listApptResponses.First().Response;
		}

		///<summary>Sets appointments.Confirmed to the ArrivedTimeTrigger.</summary>
		private void MarkArrived(IEnumerable<Appointment> listAppts) {
			long arrivedTrigger=PrefC.GetLong(PrefName.AppointmentTimeArrivedTrigger);
			foreach(Appointment appt in listAppts.Where(x => x.Confirmed!=arrivedTrigger)) {
				//This update will trigger eClipboard to generate the appropriate check-in sheets if the appointment is not already marked as arrived.
				//If the clinic is setup for eClipboard checking, the appropriate token needs to be included in the Arrival Response sms.
				Appointments.SetConfirmed(appt,arrivedTrigger);
			}
		}

		///<summary>Attempts to send the Arrival Response sms.  Logs on failure.</summary>
		private bool TrySendArrivalResponseSms(long patNum,string wirelessPhone,long clinicNum,string message,string logDir) {
			bool retVal=false;
			try {
				if(wirelessPhone is null) {
					//try to find a usable phone number for the patient.
					foreach(PatComm pat in Patients.GetPatComms(patNum.SingleItemToList(),Clinics.GetClinic(clinicNum))
						.OrderByDescending(x => x.PatNum==patNum)) 
					{
						if(pat.IsSmsAnOption) {
							wirelessPhone=pat.SmsPhone;//Stripped of formatting.
							break;
						}
					}
				}
				if(wirelessPhone is null) {
					Logger.LogError($"Unable to find a WirelessPhone for PatNum: {patNum}.");
					return false;
				}
				SmsToMobile sent=SmsToMobiles.SendSmsSingle(patNum,wirelessPhone,message,clinicNum,SmsMessageSource.Arrival);
				Logger.LogInfo($"Sent {JsonConvert.SerializeObject(sent)}");
				retVal=true;
			}
			catch(Exception ex) {
				string err=$"Failed to send Arrival Response '{message}' to PatNum: {patNum}, WirelessPhone: {wirelessPhone}. "
					+MiscUtils.GetExceptionText(ex);
				Logger.LogError(err);
			}
			return retVal;
		}

		///<summary>Determines if the given aptNum corresponds to an ApptReminderRule that has a corresponding ComeInMessageTemplate.  Does not make
		///database calls.</summary>
		public bool HasComeInMsg(long aptNum) {
			ApptReminderRule rule=GetArrivalRule(aptNum);
			return !string.IsNullOrWhiteSpace(rule?.TemplateComeInMessage??"");
		}

		///<summary>Gets an ApptReminderRule with a ComeInMessageTemplate that corresponds to the given aptNum. Does not make database calls.</summary>
		private ApptReminderRule GetArrivalRule(long aptNum) {
			IEnumerable<long> listReminderRuleNums=ListArrivalsSent.Where(x => x.ApptNum==aptNum).Select(x => x.ApptReminderRuleNum);
			ApptReminderRule rule=ListApptReminderRules.FirstOrDefault(x => x.ApptReminderRuleNum.In(listReminderRuleNums) && x.AreArrivalResponsesSetup);
			return rule;
		}

		///<summary>Returns true if an appropriate 'Come In' message template was found for the given appointment. Makes database calls.</summary>
		public bool TryGetComeInMsg(long aptNum,out string message) {
			return TryGetComeInMsg(Appointments.GetOneApt(aptNum),out message);
		}
			
		///<summary>Returns true if an appropriate 'Come In' message template was found for the given appointment.</summary>
		public bool TryGetComeInMsg(Appointment appt,out string message) {
			return TryGetMsgFromTemplate(appt,(rule) => rule?.TemplateComeInMessage??"",out message);
		}

		///<summary>Returns a list of Appointment/Arrival Responses if an appropriate Arrival Response message template was found for at least one of 
		///the given appointments.</summary>
		private List<ApptResponse> GetApptResponses(List<Appointment> listAppts) {
			List<ApptResponse> listResponses=new List<ApptResponse>();
			List<long> listConfirmStatusToSkip=PrefC.GetString(PrefName.ApptConfirmExcludeArrivalResponse)
				.Split(",",StringSplitOptions.RemoveEmptyEntries)
				.Select(x => PIn.Long(x))
				.ToList();
			foreach(Appointment appt in listAppts) {
				if(!appt.Confirmed.In(listConfirmStatusToSkip) && TryGetMsgFromTemplate(appt,(rule) => rule?.TemplateAutoReply??"",out string message)) {
					listResponses.Add(new ApptResponse(appt,message));
				}
			}
			return listResponses;
		}

		private bool TryGetMsgFromTemplate(Appointment appt,Func<ApptReminderRule,string> getTemplate,out string message) {
			ApptReminderRule rule=null;
			string msg="";
			string logDir=ODFileUtils.CombinePaths(nameof(Arrivals),nameof(TryGetMsgFromTemplate),appt.ClinicNum.ToString());
			try {
				rule=GetArrivalRule(appt.AptNum);
				string template=getTemplate(rule);
				if(string.IsNullOrWhiteSpace(template)) {
					string info=$"Unable to find template for Appointment.AptNum: {appt.AptNum}";
					Logger.LogInfo(info);
				}
				else {
					Clinic clinic=(appt.ClinicNum==0) ? Clinics.GetPracticeAsClinicZero() : Clinics.GetClinic(appt.ClinicNum);
					//PatComm is only used in this context for FName, so matching exactly to appt.PatNum (rather than Guarantor) is appropriate.
					PatComm patComm=Patients.GetPatComms(appt.PatNum.SingleItemToList(),clinic).FirstOrDefault(x => x.PatNum==appt.PatNum);
					msg=_tagReplacer.ReplaceTags(template,new ApptLite(appt,patComm),clinic,false);
				}
			}
			catch(Exception ex) {
				Logger.LogError(MiscUtils.GetExceptionText(ex));
			}
			message=msg;
			return !string.IsNullOrWhiteSpace(message);
		}

		private class ApptResponse {
			public Appointment Appointment;
			public string Response;
				
			public ApptResponse(Appointment appt,string response) {
				Appointment=appt;
				Response=response;
			}
		}
	}

}
