using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormCountyEdit : FormBase
	{
		private readonly County county;

		public FormCountyEdit(County county)
		{
			InitializeComponent();

			this.county = county;
		}

		private void FormCountyEdit_Load(object sender, EventArgs e)
		{
			nameTextBox.Text = county.Name;
			codeTextBox.Text = county.Code;
		}

		private void CountyNameTextBox_TextChanged(object sender, EventArgs e)
		{
			if (nameTextBox.Text.Length == 1)
			{
				nameTextBox.Text = nameTextBox.Text.ToUpper();
				nameTextBox.SelectionStart = 1;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var name = nameTextBox.Text.Trim();
			if (name.Length == 0)
            {
				ShowError(Translation.Common.PleaseEnterName);
				return;
            }

			if (Counties.Exists(name, county.Id))
			{
				ShowError(Translation.Common.CountyNameAlreadyExists);

				return;
			}

			county.Name = name;
			county.Code = codeTextBox.Text;

			Counties.Save(county);
			
			DialogResult = DialogResult.OK;
		}
	}
}
