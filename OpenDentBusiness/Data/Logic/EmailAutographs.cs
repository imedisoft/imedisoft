using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class EmailAutographs
	{
		[CacheGroup(nameof(InvalidType.Email))]
        private class EmailAutographCache : ListCache<EmailAutograph>
        {
			protected override IEnumerable<EmailAutograph> Load() 
				=> SelectMany("SELECT * FROM `email_autographs` ORDER BY `description`");
        }

        private static readonly EmailAutographCache cache = new EmailAutographCache();

		public static List<EmailAutograph> GetAll() 
			=> cache.GetAll();

		public static void RefreshCache() 
			=> cache.Refresh();

		public static void Save(EmailAutograph emailAutograph)
        {
			if (emailAutograph.Id == 0) ExecuteInsert(emailAutograph);
            else
            {
				ExecuteUpdate(emailAutograph);
            }
        }

		public static void Delete(long emailAutographId) 
			=> ExecuteDelete(emailAutographId);
	}
}
