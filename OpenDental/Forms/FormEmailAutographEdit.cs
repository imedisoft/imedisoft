using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEmailAutographEdit : FormBase
	{
		private readonly EmailAutograph emailAutograph;

		public FormEmailAutographEdit(EmailAutograph emailAutograph)
		{
			InitializeComponent();

			this.emailAutograph = emailAutograph;
		}

		private void FormEmailAutographEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = emailAutograph.Description;
			autographTextBox.Text = emailAutograph.Autograph;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var autograph = autographTextBox.Text.Trim();
			if (autograph.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterAutograph);

				return;
			}

			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterDescription);

				return;
			}

			emailAutograph.Description = description;
			emailAutograph.Autograph = autograph;

			EmailAutographs.Save(emailAutograph);

			DialogResult = DialogResult.OK;
		}
	}
}
