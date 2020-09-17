using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Imedisoft.Data
{
    public partial class EmailAddresses
	{
		[CacheGroup(nameof(InvalidType.Email))]
        private class EmailAddressCache : ListCache<EmailAddress>
        {
			protected override IEnumerable<EmailAddress> Load() 
				=> SelectMany("SELECT * FROM `email_addresses` WHERE `user_id` IS NULL ORDER BY `smtp_username`");
        }

        private static readonly EmailAddressCache cache = new EmailAddressCache();

		public static List<EmailAddress> GetDeepCopy(bool isShort = false)
		{
			return cache.GetAll();
		}

		public static EmailAddress GetFirstOrDefault(Predicate<EmailAddress> match, bool isShort = false)
		{
			return cache.FirstOrDefault(match);
		}

		public static List<EmailAddress> GetWhere(Predicate<EmailAddress> match, bool isShort = false)
		{
			return cache.Find(match);
		}

		public static void RefreshCache()
		{
			cache.Refresh();
		}

		///<summary>Gets the default email address for the clinic/practice. Takes a clinic num. 
		///If clinic num is 0 or there is no default for that clinic, it will get practice default. 
		///May return a new blank object.</summary>
		public static EmailAddress GetByClinic(long clinicId, bool doAllowNullReturn = false)
		{
			EmailAddress emailAddress = null;
			Clinic clinic = Clinics.GetById(clinicId);
			if (!PrefC.HasClinicsEnabled || clinic == null)
			{//No clinic, get practice default
				emailAddress = GetOne(Preferences.GetLong(PreferenceName.EmailDefaultAddressNum));
			}
			else
			{
				emailAddress = GetOne(clinic.EmailAddressId ?? 0);
				if (emailAddress == null)
				{//clinic.EmailAddressNum 0. Use default.
					emailAddress = GetOne(Preferences.GetLong(PreferenceName.EmailDefaultAddressNum));
				}
			}
			if (emailAddress == null)
			{
				emailAddress = GetFirstOrDefault(x => true);//user didn't set a default, so just pick the first email in the list.
				if (emailAddress == null && !doAllowNullReturn)
				{//If there are no email addresses AT ALL!!
					emailAddress = new EmailAddress();//To avoid null checks.
					emailAddress.SmtpPassword = "";
					emailAddress.SmtpUsername = "";
					emailAddress.Pop3Server = "";
					emailAddress.SenderAddress = "";
					emailAddress.SmtpServer = "";
				}
			}
			return emailAddress;
		}

		public static EmailAddress GetForUser(long userId) 
			=> SelectOne("SELECT * FROM `email_addresses` WHERE `user_id` = " + userId);
		

		///<summary>Gets the default email address for new outgoing emails.
		///Will attempt to get the current user's email address first. 
		///If it can't find one, will return the clinic/practice default.
		///Can return a new blank email address if no email addresses are defined for the clinic/practice.</summary>
		public static EmailAddress GetNewEmailDefault(long userId, long clinicId)
		{
			return GetForUser(userId) ?? GetByClinic(clinicId);
		}

		public static EmailAddress GetOne(long emailAddressId) 
			=> cache.FirstOrDefault(x => x.Id == emailAddressId);

		/// <summary>Returns true if the passed-in email username already exists in the cached list of non-user email addresses.</summary>
		public static bool AddressExists(string emailUserName, long skipEmailAddressNum = 0)
		{
			List<EmailAddress> listEmailAddresses = GetWhere(x => x.Id != skipEmailAddressNum
				  && x.SmtpUsername.Trim().ToLower() == emailUserName.Trim().ToLower());
			return (listEmailAddresses.Count > 0);
		}

		public static IEnumerable<EmailAddress> GetAll()
		{
			return SelectMany("SELECT * FROM `email_addresses`");
		}

		public static bool ExistsValidEmail()
		{
			return cache.FirstOrDefault(x => x.SmtpServer != "") != null;
		}

		public static bool IsValidEmail(string email)
		{
			return Regex.IsMatch(email ?? "", @".+@.+\..+");
		}

		public static long Insert(EmailAddress emailAddress)
		{
			return ExecuteInsert(emailAddress);
		}

		public static void Update(EmailAddress emailAddress)
		{
			ExecuteUpdate(emailAddress);
		}

		public static void Delete(long emailAddressNum)
		{
			ExecuteDelete(emailAddressNum);
		}
	}
}
