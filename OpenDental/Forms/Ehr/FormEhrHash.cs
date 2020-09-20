using OpenDentBusiness;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrHash : FormBase
	{
		public FormEhrHash() 
			=> InitializeComponent();

		private void TransmitButton_Click(object sender, EventArgs e)
		{
			if (hashTextBox.Text.Trim() == "" || messageTextBox.Text.Trim() == "")
			{
				ShowError("Data or hash should not be blank.");

				return;
			}

			string attachContents = 
				"Original message:\r\n" + messageTextBox.Text + "\r\n\r\n\r\n" +
				"Hash:\r\n" + hashTextBox.Text;

			Cursor = Cursors.WaitCursor;

			try
			{
				EmailMessages.SendTestUnsecure("Hash", "hash.txt", attachContents);
			}
			catch (Exception exception)
			{
				Cursor = Cursors.Default;

				ShowError(exception.Message);
				return;
			}

			Cursor = Cursors.Default;

			ShowInfo("Sent");
		}

		private void GenerateButton_Click(object sender, EventArgs e)
		{
			using var algorithm = SHA1.Create();

			var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(messageTextBox.Text));

			hashTextBox.Text = string.Concat(hash.Select(b => b.ToString("x2")));
		}

		private void CloseButton_Click(object sender, EventArgs e) 
			=> Close();
	}
}
