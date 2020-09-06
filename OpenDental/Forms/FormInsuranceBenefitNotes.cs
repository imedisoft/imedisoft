namespace Imedisoft.Forms
{
    public partial class FormInsuranceBenefitNotes : FormBase
	{
		/// <summary>
		/// Gets or sets the benefit notes.
		/// </summary>
		public string BenefitNotes
        {
			get => benefitNotesTextBox.Text;
			set => benefitNotesTextBox.Text = value;
        }

		public FormInsuranceBenefitNotes() => InitializeComponent();
	}
}
