using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormLogoffWarning : FormBase
	{
		public FormLogoffWarning() => InitializeComponent();

		private void Timer_Tick(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
