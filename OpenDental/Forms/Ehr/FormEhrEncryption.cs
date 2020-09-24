using OpenDentBusiness;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrEncryption : FormBase
	{
		private const string Key = "AKQjlLUjlcABVbqp";

		private static readonly byte[] KeyBytes = Encoding.UTF8.GetBytes(Key);

		public FormEhrEncryption()
		{
			InitializeComponent();
		}

		private void EncryptButton_Click(object sender, EventArgs e)
		{
			var text = inputTextBox.Text.Trim();
			if (text.Length == 0)
			{
				ShowError(Translation.Ehr.NothingToEncrypt);

				return;
			}

			outputTextBox.Text = Encryption(text);
		}

		private void DecryptButton_Click(object sender, EventArgs e)
		{
			var text = inputTextBox.Text.Trim();
			if (text.Length == 0)
			{
				ShowError(Translation.Ehr.NothingToDecrypt);

				return;
			}

			outputTextBox.Text = Decryption(text);
		}

		public string Encryption(string data)
		{
			var bytes = Encoding.UTF8.GetBytes(data);
			
			using var aes = Aes.Create();

            aes.Key = KeyBytes;
            aes.IV = new byte[16];

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
            }

            var encrypted = memoryStream.ToArray();

            return Convert.ToBase64String(encrypted);
        }

		public string Decryption(string data)
		{
			var bytes = Encoding.UTF8.GetBytes(data);

			try
            {
                using var aes = Aes.Create();

                aes.Key = KeyBytes;
                aes.IV = new byte[16];

                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using var memoryStream = new MemoryStream(bytes);
                using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                using var streamReader = new StreamReader(cryptoStream);

                return streamReader.ReadToEnd();
            }
            catch
			{
				ShowError(Translation.Ehr.EnteredTextIsNotValidEncryptedText);

				return "";
			}
		}

        private string GenerateHash(string message)
		{
			return string.Concat(Encoding.ASCII.GetBytes(message).Select(x => x.ToString("x2")));
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var text = inputTextBox.Text.Trim();
			if (text.Length == 0)
			{
				ShowError(Translation.Ehr.NothingToSend);

				return;
			}

			string message = Encryption(text);
			string hash = GenerateHash(text);

			Cursor = Cursors.WaitCursor;

			try
			{
				EmailMessages.SendTestUnsecure(Translation.Ehr.Encryption, "encryption.txt",
					string.Format(Translation.Ehr.EncryptedMessageWithHash, message, hash));
			}
			catch (Exception exception)
			{
				Cursor = Cursors.Default;

				ShowError(exception.Message);

				return;
			}

			Cursor = Cursors.Default;
			
			ShowInfo(Translation.Common.Sent);
		}
	}
}
