using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    /// <summary>
	/// In the program, this is now called an autonote prompt.
	/// </summary>
    [Table("auto_note_prompts")]
	public class AutoNotePrompt
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The description of the prompt as it will be referred to from other windows.
		/// </summary>
		public string Description;

		public string Type;

		/// <summary>
		/// The prompt text.
		/// </summary>
		public string Label;

		/// <summary>
		/// For TextBox, this is the default text.  For a ComboBox, this is the list of possible responses, one per line.
		/// </summary>
		public string Options;

		public string PromptTag => "[Prompt:\"" + Description + "\"]";
	}
}
