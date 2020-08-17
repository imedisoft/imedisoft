namespace Imedisoft.Forms
{
    public partial class FormClaimResend : FormBase
	{
		public FormClaimResend() 
			=> InitializeComponent();

		public bool IsClaimReplacement 
			=> claimReplacementRadioButton.Checked;
	}
}