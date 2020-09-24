using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrDrugUnitEdit : FormBase
	{
		private readonly EhrDrugUnit drugUnit;

		public FormEhrDrugUnitEdit(EhrDrugUnit drugUnit)
		{
			InitializeComponent();

			this.drugUnit = drugUnit;
		}

		private void FormEhrDrugUnitEdit_Load(object sender, EventArgs e)
		{
			codeTextBox.Text = drugUnit.Code;
			descriptionTextBox.Text = drugUnit.Description;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var code = codeTextBox.Text.Trim();
			if (code.Length == 0)
            {
				ShowError(Translation.Common.PleaseEnterCode);

				return;
            }

			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
            {
				ShowError(Translation.Common.PleaseEnterDescription);

				return;
            }

			drugUnit.Code = code;
			drugUnit.Description = description;

			EhrDrugUnits.Save(drugUnit);

			DialogResult = DialogResult.OK;
		}
	}
}
