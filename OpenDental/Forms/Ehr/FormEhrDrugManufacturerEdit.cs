using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrDrugManufacturerEdit : FormBase
	{
		private readonly EhrDrugManufacturer drugManufacturer;
		private readonly string originalCode;

		public FormEhrDrugManufacturerEdit(EhrDrugManufacturer drugManufacturer)
		{
			InitializeComponent();

			this.drugManufacturer = drugManufacturer;

			originalCode = drugManufacturer.Code;
		}

		private void FormEhrDrugManufacturerEdit_Load(object sender, EventArgs e)
		{
			nameTextBox.Text = drugManufacturer.Name;
			codeTextBox.Text = drugManufacturer.Code;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var name = nameTextBox.Text.Trim();
			if (name.Length == 0)
            {
				ShowError(Translation.Common.PleaseEnterName);

				return;
            }

			var code = codeTextBox.Text.Trim();
			if (code.Length == 0)
            {
				ShowError(Translation.Common.PleaseEnterCode);

				return;
            }

			if (originalCode != code)
            {
				if (EhrDrugManufacturers.Any(x => x.Code.Equals(code)))
				{
					ShowError(Translation.Ehr.ManufacturerWithThisCodeAlreadyExists);

					return;
				}
			}

			drugManufacturer.Name = name;
			drugManufacturer.Code = code;

			EhrDrugManufacturers.Save(drugManufacturer);

			DialogResult = DialogResult.OK;
		}
	}
}
