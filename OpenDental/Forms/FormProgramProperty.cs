using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormProgramProperty : FormBase
	{
		private readonly ProgramProperty programProperty;

		/// <summary>
		/// A value indicating whether the property value is encrypted in the database. 
		/// Typically true for properties that represent passwords.
		/// </summary>
		private readonly bool encrypted;

		public FormProgramProperty(ProgramProperty programProperty, bool encrypted = false)
		{
			InitializeComponent();

			this.programProperty = programProperty;
			this.encrypted = encrypted;
		}

		private void FormProgramProperty_Load(object sender, EventArgs e)
		{
			propertyTextBox.Text = programProperty.Name;

			if (encrypted && programProperty.Value != "")
			{
                CDT.Class1.Decrypt(programProperty.Value, out string decryptedText);

                valueTextBox.Text = decryptedText;
			}
			else
			{
				valueTextBox.Text = programProperty.Value;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			programProperty.Value = valueTextBox.Text;

			if (encrypted)
			{
                CDT.Class1.Encrypt(programProperty.Value, out string encryptedText);

                programProperty.Value = encryptedText;
			}

			ProgramProperties.Update(programProperty);

			DialogResult = DialogResult.OK;
		}
	}
}
