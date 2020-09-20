using Imedisoft.Data.Models;

namespace Imedisoft.Models.Dtos
{
    class AutoNoteDto
	{
		public string Name { get; set; }

		public string Content { get; set; }

		public AutoNoteDto(AutoNote autoNote)
		{
			Name = autoNote.Name;
			Content = autoNote.Content;
		}
	}
}
