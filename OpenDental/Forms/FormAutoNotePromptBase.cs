using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public class FormAutoNotePromptBase : FormBase
	{
        /// <summary>
        /// Gets the result of the prompt.
        /// </summary>
        public FormAutoNotePromptResult PromptResult
        {
            get
            {
                return DialogResult switch
                {
                    DialogResult.OK => FormAutoNotePromptResult.MoveNext,
                    DialogResult.Retry => FormAutoNotePromptResult.MoveBack,
                    DialogResult.Ignore => FormAutoNotePromptResult.Skip,
                    _ => FormAutoNotePromptResult.Abort
                };
            }
        }

        public string Value { get; protected set; }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel)
            {
                if (!Confirm("Abort auto note entry?"))
                {
                    e.Cancel = true;
                }
            }
        }
    }

    /// <summary>
    /// Identifies the result of a auto note prompt form.
    /// </summary>
	public enum FormAutoNotePromptResult
    {
        MoveBack,
        MoveNext,
        Skip,
        Abort
    }
}
