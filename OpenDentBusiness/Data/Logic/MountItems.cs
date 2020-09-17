using Imedisoft.Data.Models;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class MountItems
	{
		public static IEnumerable<MountItem> GetByMount(long mountId) 
			=> SelectMany("SELECT * FROM `mount_items` WHERE `mount_id` = " + mountId + " ORDER BY `sort_order`");

		public static long Insert(MountItem mountItem) 
			=> ExecuteInsert(mountItem);

		public static void Delete(MountItem mountItem) 
			=> ExecuteDelete(mountItem);
	}
}
