using Imedisoft.Data;
using Imedisoft.Data.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
	public partial class UserClinics
	{
		[CacheGroup(nameof(InvalidType.UserClinics))]
		private class UserClinicCache : ListCache<UserClinic>
		{
			protected override IEnumerable<UserClinic> Load() 
				=> SelectMany("SELECT * FROM `user_clinics`");
		}

		private static readonly UserClinicCache cache = new UserClinicCache();

		/// <summary>
		/// Gets a deep copy of all matching items from the cache via ListLong. Set isShort true to search through ListShort instead.
		/// </summary>
		public static List<UserClinic> GetWhere(Predicate<UserClinic> predicate) 
			=> cache.Find(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		/// <summary>
		/// Gets all User to Clinic associations for the user. Can return an empty list if there are none.
		/// </summary>
		public static List<UserClinic> GetForUser(long userId) 
			=> GetWhere(x => x.UserId == userId);

		/// <summary>
		/// Gets all User to Clinic associations for a clinic. Can return an empty list if there are none.
		/// </summary>
		public static List<UserClinic> GetForClinic(long clinicId) 
			=> GetWhere(x => x.ClinicId == clinicId);

		public static bool Sync(List<UserClinic> listNew, long userId)
		{
			// List<UserClinic> listOld = GetForUser(userId);
			// TODO: Fix me.. return Crud.UserClinicCrud.Sync(listNew, listOld);
			return false;
		}
	}
}
