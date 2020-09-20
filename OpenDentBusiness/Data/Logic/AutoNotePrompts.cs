using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Imedisoft.Data
{
    public partial class AutoNotePrompts
	{
		[CacheGroup(nameof(InvalidType.AutoNotes))]
        private class AutoNotePromptCache : ListCache<AutoNotePrompt>
        {
			protected override IEnumerable<AutoNotePrompt> Load() 
				=> SelectMany("SELECT * FROM `auto_note_prompts` ORDER BY `description`");
        }

        private static readonly AutoNotePromptCache cache = new AutoNotePromptCache();

		public static void RefreshCache() 
			=> cache.Refresh();

		public static List<AutoNotePrompt> GetAll()
			=> cache.GetAll();

		public static AutoNotePrompt GetByDescription(string description)
			=> cache.FirstOrDefault(autoNotePrompt => autoNotePrompt.Description == description);

		public static List<AutoNotePrompt> GetByAutoNoteContent(IEnumerable<AutoNote> autoNotes)
		{
			var autoNotePrompts = new List<AutoNotePrompt>();
			var autoNotePromptTags = new List<Match>();

			foreach (var autoNote in autoNotes)
			{
				autoNotePromptTags.AddRange(GetPromptRegexMatches(autoNote.Content));
			}

			foreach (var tag in autoNotePromptTags)
			{
				var description = tag.ToString().Replace("[Prompt:\"", "").Replace("\"]", "");
				if (autoNotePrompts.Any(autoNotePrompt => autoNotePrompt.Description == description))
                {
					continue;
                }

				var autoNotePrompt = GetByDescription(description);
				if (autoNotePrompt != null)
				{
					autoNotePrompts.Add(autoNotePrompt);
				}
			}

			return autoNotePrompts;
		}

		public static void Save(AutoNotePrompt autoNotePrompt)
		{
			if (autoNotePrompt.Id == 0) ExecuteInsert(autoNotePrompt);
			else
			{
				ExecuteUpdate(autoNotePrompt);
			}
		}

		public static void Delete(long autoNotePromptId) 
			=> ExecuteDelete(autoNotePromptId);

		public static List<Match> GetPromptRegexMatches(string note)
			=> Regex.Matches(note, @"\[Prompt:""[a-zA-Z_0-9 ]+""\]").OfType<Match>().ToList();

		public static bool IsDescriptionValid(string description) 
			=> Regex.IsMatch(description, "^[a-zA-Z_0-9 ]*$");
	}
}
