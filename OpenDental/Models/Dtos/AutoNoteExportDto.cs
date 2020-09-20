using System.Collections.Generic;

namespace Imedisoft.Models.Dtos
{
    class AutoNoteExportDto
	{
		public List<AutoNoteDto> AutoNotes { get; set; }

		public List<AutoNotePromptDto> AutoNotePrompts { get; set; }
	}
}
