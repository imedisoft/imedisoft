using Imedisoft.Data.Models;

namespace Imedisoft.Models.Dtos
{
    class AutoNotePromptDto
	{
		public string Description { get; set; }

		public string Type { get; set; }

		public string Label { get; set; }

		public string Options { get; set; }

		public AutoNotePromptDto(AutoNotePrompt autoNotePrompt)
		{
			Description = autoNotePrompt.Description;
			Type = autoNotePrompt.Type;
			Label = autoNotePrompt.Label;
			Options = autoNotePrompt.Options;
		}
	}
}
