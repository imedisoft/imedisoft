﻿using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestsCore
{
    public class ApptReminderRuleT {
		public static ApptReminderRule CreateApptReminderRule(long clinicNum,ApptReminderType type,TimeSpan tsPrior,
			bool isSendAll = true,CommType priority1 = CommType.Preferred,CommType priority2 = CommType.Text,CommType priority3 = CommType.Email,
			TimeSpan doNotSendWithin=default(TimeSpan),bool isAutoReplyEnabled=true,string language="") 
		{
			ApptReminderRule clinicRule=ApptReminderRules.CreateDefaultReminderRule(type,clinicNum);
			clinicRule.TSPrior=tsPrior;
			clinicRule.IsSendAll=isSendAll;
			clinicRule.SendOrder=string.Join(",",new List<CommType>() { priority1,priority2,priority3 }.Select(x => ((int)x).ToString()).ToArray());
			clinicRule.DoNotSendWithin=doNotSendWithin;
			clinicRule.Language=language;
			if(type==ApptReminderType.PatientPortalInvite && clinicNum > 0) {
				clinicRule.SendOrder="2";//Email only
				clinicRule.IsSendAll=false;
				if(ClinicPrefs.Set(clinicNum,PrefName.PatientPortalInviteEnabled,"1") | ClinicPrefs.Set(clinicNum,PrefName.PatientPortalInviteUseDefaults,"0")) {
					ClinicPrefs.RefreshCache();
				}
			}
			clinicRule.IsAutoReplyEnabled=isAutoReplyEnabled;
			ApptReminderRules.Insert(clinicRule);
			return clinicRule;
		}

		///<summary>Deletes everything from the apptreminderrule table.  Does not truncate the table so that PKs are not reused on accident.</summary>
		public static void ClearApptReminderRuleTable() {
			string command="DELETE FROM apptreminderrule WHERE ApptReminderRuleNum > 0";
			Database.ExecuteNonQuery(command);
		}
	}
}
