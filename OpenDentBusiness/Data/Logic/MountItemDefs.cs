using Imedisoft.Data.Models;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class MountItemDefs
	{
		public static IEnumerable<MountItemDef> GetByMountDefinition(long mountDefinitionId) 
			=> SelectMany("SELECT * FROM `mount_item_defs` WHERE `mount_def_id` = " + mountDefinitionId + " ORDER BY `sort_order`");
		
		public static void Update(MountItemDef mountItemDefinition) 
			=> ExecuteUpdate(mountItemDefinition);
		
		public static long Insert(MountItemDef mountItemDefinition) 
			=> ExecuteInsert(mountItemDefinition);
		
		public static void Delete(long mountItemDefinitionId) 
			=> ExecuteDelete(mountItemDefinitionId);
		
		public static void DeleteForMount(long mountDefinitionId) 
			=> Database.ExecuteNonQuery("DELETE FROM mount_item_defs WHERE mount_def_id = " + mountDefinitionId);
	}
}
