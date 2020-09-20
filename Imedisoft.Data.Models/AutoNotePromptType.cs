using System.Collections.Generic;

namespace Imedisoft.Data.Models
{
    public static class AutoNotePromptType
	{
		public const string Text = "Text";
		public const string OneResponse = "OneResponse";
		public const string MultiResponse = "MultiResponse";

		public static IEnumerable<DataItem<string>> GetValues()
        {
			yield return new DataItem<string>(Text, Translation.Enums.AutoNotePromptTypeText);
			yield return new DataItem<string>(OneResponse, Translation.Enums.AutoNotePromptTypeOneResponse);
			yield return new DataItem<string>(MultiResponse, Translation.Enums.AutoNotePromptTypeMultiResponse);
		}
	}
}
