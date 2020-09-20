using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class AutoNoteCategories
    {
        [CacheGroup(nameof(InvalidType.AutoNotes))]
        private class AutoNoteCategoryCache : ListCache<AutoNoteCategory>
        {
            protected override IEnumerable<AutoNoteCategory> Load() 
                => SelectMany("SELECT * FROM `auto_note_categories` ORDER BY `description`");
        }

        private static readonly AutoNoteCategoryCache cache = new AutoNoteCategoryCache();

        public static List<AutoNoteCategory> GetAll()
            => cache.GetAll();

        public static void RefreshCache()
            => cache.Refresh();

        public static void Save(AutoNoteCategory autoNoteCategory)
        {
            if (autoNoteCategory.Id == 0) ExecuteInsert(autoNoteCategory);
            else
            {
                ExecuteUpdate(autoNoteCategory);
            }
        }
    }
}
