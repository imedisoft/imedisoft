namespace Imedisoft.Forms
{
    public partial class FormAutoNotePromptPreview : FormBase
	{
		public string Preview => previewTextBox.Text.Trim();

		public FormAutoNotePromptPreview(string preview)
		{
			InitializeComponent();

			previewTextBox.Text = preview;
		}
	}
}
