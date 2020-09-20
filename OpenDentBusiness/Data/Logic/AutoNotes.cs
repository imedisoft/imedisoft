using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class AutoNotes
	{
		[CacheGroup(nameof(InvalidType.AutoNotes))]
        private class AutoNoteCache : ListCache<AutoNote>
        {
			protected override IEnumerable<AutoNote> Load() 
				=> SelectMany("SELECT * FROM `auto_notes` ORDER BY `name`");
        }

        private static readonly AutoNoteCache cache = new AutoNoteCache();

		public static List<AutoNote> GetAll() 
			=> cache.GetAll();

		public static List<AutoNote> Find(Predicate<AutoNote> predicate) 
			=> cache.Find(predicate);

		public static bool Exists(Predicate<AutoNote> predicate) 
			=> cache.Any(predicate);

		public static bool Exists(string name)
			=> cache.Any(autoNote => autoNote.Name.Equals(name));

		public static void RefreshCache() 
			=> cache.Refresh();

		public static void Save(AutoNote autoNote)
		{
			if (autoNote.Id == 0) ExecuteInsert(autoNote);
            else
            {
				ExecuteUpdate(autoNote);
            }
		}

		public static void Delete(long autoNoteId) 
			=> ExecuteDelete(autoNoteId);

		public static string GetContentByTitle(string name) 
			=> cache.FirstOrDefault(autoNote => autoNote.Name.Equals(name))?.Content ?? "";

		public static void RemoveFromCategory(long autoNoteCategoryId) 
			=> Database.ExecuteNonQuery(
				"UPDATE `auto_notes` SET `auto_note_category_id` = NULL WHERE `category` = " + autoNoteCategoryId);
	}
}
