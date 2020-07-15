using System;
using System.Windows.Forms;

namespace OpenDentHL7
{
    public partial class FormDebug : Form
	{
        private readonly string serviceName;

		public FormDebug(string serviceName)
		{
			this.serviceName = serviceName;

			InitializeComponent();
		}

		private void FormDebug_Load(object sender, EventArgs e)
		{
			var service = new ServiceHL7();

			try
			{
				service.ServiceName = serviceName;
				service.StartManually();
			}
			catch (Exception exception)
			{
				MessageBox.Show(this, 
					exception.Message, "HL7", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
			}
		}
	}
}
