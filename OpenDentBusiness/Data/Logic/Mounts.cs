using Imedisoft.Data.Models;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class Mounts
	{
		public static Mount GetById(long mountId)
			=> SelectOne(mountId) ?? new Mount();

		public static long Insert(Mount mount) 
			=> ExecuteInsert(mount);

		public static void Update(Mount mount) 
			=> ExecuteUpdate(mount);

		public static void Delete(Mount mount) 
			=> ExecuteDelete(mount);
		
		public static Mount CreateFromDefinition(MountDef mountDefinition, long patientId, long categoryId)
		{
            var mount = new Mount
            {
                PatientId = patientId,
                Category = categoryId,
                Description = mountDefinition.Description,
                Note = "",
                Width = mountDefinition.Width,
                Height = mountDefinition.Height,
                BackColor = mountDefinition.BackColor,
                ListMountItems = new List<MountItem>()
            };

            ExecuteInsert(mount);

			foreach (var mountItemDefinition in MountItemDefs.GetByMountDefinition(mountDefinition.Id))
			{
                var mountItem = new MountItem
                {
                    MountId = mount.Id,
                    X = mountItemDefinition.X,
                    Y = mountItemDefinition.Y,
                    Width = mountItemDefinition.Width,
					Height = mountItemDefinition.Height,
					SortOrder = mountItemDefinition.SortOrder
				};

				mountItem.Id = MountItems.Insert(mountItem);
				mount.ListMountItems.Add(mountItem);
			}

			return mount;
		}
	}
}
