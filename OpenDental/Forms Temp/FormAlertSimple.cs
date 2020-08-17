namespace OpenDental
{
    public partial class FormAlertSimple : ODForm
	{
		/// <summary>
		/// Always use Show() instead of ShowDialog() and make sure to programmatically call Close(), because the user will not be able to close (no buttons are visible).
		/// </summary>
		public FormAlertSimple(string strMsgText)
		{
			InitializeComponent();

			labelMsg.Text = strMsgText;
		}
	}
}