using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class MountDefs
	{
		[CacheGroup(nameof(InvalidType.ToolButsAndMounts))]
		private class MountDefCache : ListCache<MountDef>
		{
			protected override IEnumerable<MountDef> Load() 
				=> SelectMany("SELECT * FROM `mount_defs` ORDER BY `sort_order`");
        }

		private static readonly MountDefCache cache = new MountDefCache();

		public static List<MountDef> GetAll() 
			=> cache.GetAll();

		public static void RefreshCache() 
			=> cache.Refresh();

		public static void Update(MountDef mountDefinition) 
			=> ExecuteUpdate(mountDefinition);

		public static long Insert(MountDef mountDefinition) 
			=> ExecuteInsert(mountDefinition);
		
		public static void Delete(long mountDefinitionId) 
			=> ExecuteDelete(mountDefinitionId);
	}
}
