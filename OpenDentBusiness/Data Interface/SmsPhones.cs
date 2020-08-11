using CodeBase;
using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace OpenDentBusiness
{
    public class SmsPhones
	{
		/// <summary>
		/// Used to display "SHORTCODE" on the customer side as the phone number for SMS sent via Short Code.
		/// End user does not need to see the specific short code number we use at HQ.
		/// This ensures we do not recored this communication on a different valid SmsPhone/VLN that it didn't truly take place on.
		/// However, on the HQ side, we want records of this communication to be listed as having taken place on the actual Short Code number.
		/// </summary>
		public const string SHORTCODE = "SHORTCODE";

		private class SmsPhoneCache : CacheListAbs<SmsPhone>
		{
			protected override List<SmsPhone> GetCacheFromDb()
			{
				return Crud.SmsPhoneCrud.SelectMany("SELECT * FROM smsphone");
			}

			protected override List<SmsPhone> TableToList(DataTable table)
			{
				return Crud.SmsPhoneCrud.TableToList(table);
			}

			protected override SmsPhone Copy(SmsPhone smsPhone)
			{
				return smsPhone.Copy();
			}

			protected override DataTable ListToTable(List<SmsPhone> listSmsPhones)
			{
				return Crud.SmsPhoneCrud.ListToTable(listSmsPhones, "SmsPhone");
			}

			protected override void FillCacheIfNeeded()
			{
				SmsPhones.GetTableFromCache(false);
			}
		}

		private static readonly SmsPhoneCache cache = new SmsPhoneCache();

		public static SmsPhone GetFirstOrDefault(Func<SmsPhone, bool> match, bool isShort = false)
		{
			return cache.GetFirstOrDefault(match, isShort);
		}

		public static DataTable GetTableFromCache(bool doRefreshCache)
		{
			return cache.GetTableFromCache(doRefreshCache);
		}

		public static void RefreshCache()
		{
			GetTableFromCache(true);
		}

		public static SmsPhone GetByPhone(string phoneNumber)
		{
			return Crud.SmsPhoneCrud.SelectOne("SELECT * FROM smsphone WHERE PhoneNumber='" + POut.String(phoneNumber) + "'");
		}

		public static long Insert(SmsPhone smsPhone)
		{
			return Crud.SmsPhoneCrud.Insert(smsPhone);
		}

		public static void Update(SmsPhone smsPhone)
		{
			Crud.SmsPhoneCrud.Update(smsPhone);
		}

		public static List<SmsPhone> GetAll()
		{
			return Crud.SmsPhoneCrud.SelectMany("SELECT * FROM smsphone");
		}

		public static DataTable GetSmsUsageLocal(List<long> listClinicNums, DateTime dateMonth, List<SmsPhone> listPhones)
		{
			#region Initialize retVal DataTable
			string strNoActivePhones = "No Active Phones";
			List<SmsPhone> listSmsPhones = listPhones.Where(x => x.ClinicNum.In(listClinicNums)).ToList();
			DateTime dateStart = dateMonth.Date.AddDays(1 - dateMonth.Day);//remove time portion and day of month portion. Remainder should be midnight of the first of the month
			DateTime dateEnd = dateStart.AddMonths(1);//This should be midnight of the first of the following month.
													  //This query builds the data table that will be filled from several other queries, instead of writing one large complex query.
													  //It is written this way so that the queries are simple to write and understand, and makes Oracle compatibility easier to maintain.
			string command = @"SELECT 
							  0 ClinicNum,
							  ' ' PhoneNumber,
							  ' ' CountryCode,
							  0 SentMonth,
							  0.0 SentCharge,
								0.0 SentDiscount,
								0.0 SentPreDiscount,
							  0 ReceivedMonth,
							  0.0 ReceivedCharge 
							FROM
							  DUAL";//this is a cute way to get a data table with the correct layout without having to query any real data.
			DataTable retVal = Database.ExecuteDataTable(command).Clone();//use .Clone() to get schema only, with no rows.
			retVal.TableName = "SmsUsageLocal";
			for (int i = 0; i < listClinicNums.Count; i++)
			{
				DataRow row = retVal.NewRow();
				row["ClinicNum"] = listClinicNums[i];
				row["PhoneNumber"] = strNoActivePhones;
				SmsPhone firstActivePhone = listSmsPhones
					.Where(x => x.ClinicNum == listClinicNums[i])//phones for this clinic
					.Where(x => x.DateTimeInactive.Year < 1880)//that are active
					.OrderByDescending(x => x.IsPrimary)
					.ThenBy(x => x.DateTimeActive)
					.FirstOrDefault();
				if (firstActivePhone != null)
				{
					row["PhoneNumber"] = firstActivePhone.PhoneNumber;
					row["CountryCode"] = firstActivePhone.CountryCode;
				}
				row["SentMonth"] = 0;
				row["SentCharge"] = 0.0;
				row["SentDiscount"] = 0.0;
				row["SentPreDiscount"] = 0.0;
				row["ReceivedMonth"] = 0;
				row["ReceivedCharge"] = 0.0;
				retVal.Rows.Add(row);
			}
			#endregion
			#region Fill retVal DataTable
			//Sent Last Month
			command = "SELECT ClinicNum, COUNT(*), ROUND(SUM(MsgChargeUSD),2),ROUND(SUM(MsgDiscountUSD),2)"
				+ ",SUM(CASE SmsPhoneNumber WHEN '" + POut.String(SmsPhones.SHORTCODE) + "' THEN 1 ELSE 0 END) FROM smstomobile "
				+ "WHERE DateTimeSent >=" + POut.Date(dateStart) + " "
				+ "AND DateTimeSent<" + POut.Date(dateEnd) + " "
				+ "AND MsgChargeUSD>0 GROUP BY ClinicNum";
			DataTable table = Database.ExecuteDataTable(command);
			for (int i = 0; i < table.Rows.Count; i++)
			{
				for (int j = 0; j < retVal.Rows.Count; j++)
				{
					if (retVal.Rows[j]["ClinicNum"].ToString() != table.Rows[i]["ClinicNum"].ToString())
					{
						continue;
					}
					retVal.Rows[j]["SentMonth"] = table.Rows[i][1];//.ToString();
					retVal.Rows[j]["SentCharge"] = table.Rows[i][2];//.ToString();
					retVal.Rows[j]["SentDiscount"] = table.Rows[i][3];
					retVal.Rows[j]["SentPreDiscount"] = PIn.Double(retVal.Rows[j]["SentCharge"].ToString()) + PIn.Double(retVal.Rows[j]["SentDiscount"].ToString());
					//No active phone but at least one of these messages sent from Short Code
					if (retVal.Rows[j]["PhoneNumber"].ToString() == strNoActivePhones && PIn.Long(table.Rows[i][4].ToString()) > 0)
					{
						retVal.Rows[j]["PhoneNumber"] = POut.String(SmsPhones.SHORTCODE);//display "SHORTCODE" as primary number.
					}
					break;
				}
			}
			//Received Month
			command = "SELECT ClinicNum, COUNT(*),SUM(CASE SmsPhoneNumber WHEN '" + POut.String(SmsPhones.SHORTCODE) + "' THEN 1 ELSE 0 END) FROM smsfrommobile "
				+ "WHERE DateTimeReceived >=" + POut.Date(dateStart) + " "
				+ "AND DateTimeReceived<" + POut.Date(dateEnd) + " "
				+ "GROUP BY ClinicNum";
			table = Database.ExecuteDataTable(command);
			for (int i = 0; i < table.Rows.Count; i++)
			{
				for (int j = 0; j < retVal.Rows.Count; j++)
				{
					if (retVal.Rows[j]["ClinicNum"].ToString() != table.Rows[i]["ClinicNum"].ToString())
					{
						continue;
					}
					retVal.Rows[j]["ReceivedMonth"] = table.Rows[i][1].ToString();
					retVal.Rows[j]["ReceivedCharge"] = "0";
					//No active phone but at least one of these messages sent from Short Code
					if (retVal.Rows[j]["PhoneNumber"].ToString() == strNoActivePhones && PIn.Long(table.Rows[i][2].ToString()) > 0)
					{
						retVal.Rows[j]["PhoneNumber"] = POut.String(SmsPhones.SHORTCODE);//display "SHORTCODE" as primary number.
					}
					break;
				}
			}
			#endregion
			return retVal;
		}

		///<summary>Find all phones in the db (by PhoneNumber) and sync with listPhonesSync. If a given PhoneNumber does not already exist then insert the SmsPhone.
		///If a given PhoneNumber exists in the local db but does not exist in the HQ-provided listPhoneSync, then deacitvate that phone locallly.
		///Return true if a change has been made to the database.</summary>
		public static bool UpdateOrInsertFromList(List<SmsPhone> listPhonesSync)
		{

			//Get all phones so we can filter as needed below.
			string command = "SELECT * FROM smsphone";
			List<SmsPhone> listPhonesDb = Crud.SmsPhoneCrud.SelectMany(command);
			bool isChanged = false;
			//Deal with phones that occur in the HQ-supplied list.
			foreach (SmsPhone phoneSync in listPhonesSync)
			{
				SmsPhone phoneOld = listPhonesDb.FirstOrDefault(x => x.PhoneNumber == phoneSync.PhoneNumber);
				//Upsert.
				if (phoneOld != null)
				{ //This phone already exists. Update it to look like the phone we are trying to insert.
					phoneOld.ClinicNum = phoneSync.ClinicNum; //The clinic may have changed so set it to the new clinic.
					phoneOld.CountryCode = phoneSync.CountryCode;
					phoneOld.DateTimeActive = phoneSync.DateTimeActive;
					phoneOld.DateTimeInactive = phoneSync.DateTimeInactive;
					phoneOld.InactiveCode = phoneSync.InactiveCode;
					Update(phoneOld);
				}
				else
				{ //This phone is new so insert it.
					Insert(phoneSync);
				}
				isChanged = true;
			}
			//Deal with phones which are in the local db but that do not occur in the HQ-supplied list.
			foreach (SmsPhone phoneNotFound in listPhonesDb.FindAll(x => !listPhonesSync.Any(y => y.PhoneNumber == x.PhoneNumber)))
			{
				//This phone not found at HQ so deactivate it.
				phoneNotFound.DateTimeInactive = DateTime.Now;
				phoneNotFound.InactiveCode = "Phone not found at HQ";
				Update(phoneNotFound);
				isChanged = true;
			}
			return isChanged;
		}

		///<summary>Returns current clinic limit minus message usage for current calendar month.</summary>
		public static double GetClinicBalance(long clinicNum)
		{

			double limit = 0;
			if (!PrefC.HasClinicsEnabled)
			{
				if (PrefC.GetDate(PrefName.SmsContractDate).Year > 1880)
				{
					limit = Prefs.GetDouble(PrefName.SmsMonthlyLimit);
				}
			}
			else
			{
				if (clinicNum == 0 && Clinics.GetCount(true) > 0)
				{//Sending text for "Unassigned" patient.  Use the first non-hidden clinic. (for now)
					clinicNum = Clinics.GetFirst(true).ClinicNum;
				}
				Clinic clinicCur = Clinics.GetClinic(clinicNum);
				if (clinicCur != null && clinicCur.SmsContractDate.Year > 1880)
				{
					limit = clinicCur.SmsMonthlyLimit;
				}
			}
			DateTime dtStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
			DateTime dtEnd = dtStart.AddMonths(1);
			string command = "SELECT SUM(MsgChargeUSD) FROM smstomobile WHERE ClinicNum=" + POut.Long(clinicNum) + " "
				+ "AND DateTimeSent>=" + POut.Date(dtStart) + " AND DateTimeSent<" + POut.Date(dtEnd);
			limit -= Database.ExecuteDouble(command);
			return limit;
		}

		/// <summary>
		/// Returns true if texting is enabled for any clinics (including hidden), or if not using clinics, if it is enabled for the practice.
		/// </summary>
		public static bool IsIntegratedTextingEnabled()
		{
			if (Plugins.HookMethod(null, "SmsPhones.IsIntegratedTextingEnabled_start"))
			{
				return true;
			}

			if (!PrefC.HasClinicsEnabled)
			{
				return PrefC.GetDateT(PrefName.SmsContractDate).Year > 1880;
			}

			return (Clinics.GetFirstOrDefault(x => x.SmsContractDate.Year > 1880) != null);
		}

		/// <summary>
		/// Returns 0 if clinics not in use, or patient.ClinicNum if assigned to a clinic, or ClinicNum of the default texting clinic.
		/// </summary>
		public static long GetClinicNumForTexting(long patNum)
		{
			if (!PrefC.HasClinicsEnabled || Clinics.GetCount() == 0)
			{
				return 0;
			}

			Clinic clinic = Clinics.GetClinic(Patients.GetPat(patNum).ClinicNum);//if patnum invalid will throw unhandled exception.
			if (clinic != null)
			{
				return clinic.ClinicNum;
			}

			return Prefs.GetLong(PrefName.TextingDefaultClinicNum);
		}
	}
}
