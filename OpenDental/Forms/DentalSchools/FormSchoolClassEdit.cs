using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormSchoolClassEdit : FormBase
	{
		private readonly SchoolClass schoolClass;

		public FormSchoolClassEdit(SchoolClass schoolClass)
		{
			InitializeComponent();

			this.schoolClass = schoolClass;
		}

		private void FormSchoolClassEdit_Load(object sender, EventArgs e)
		{
			if (schoolClass.Year != 0)
			{
				yearTextBox.Text = schoolClass.Year.ToString();
			}
            else
            {
				yearTextBox.Text = DateTime.Now.Year.ToString();
            }

			descriptionTextBox.Text = schoolClass.Description;
		}

		private void YearTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void YearTextBox_Validating(object sender, CancelEventArgs e)
		{
			if (int.TryParse(yearTextBox.Text, out var year))
			{
				if (year < 1990) year = 1990;
				else if (year >= 2099)
				{
					year = 2099;
				}

				yearTextBox.Text = year.ToString();
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!int.TryParse(yearTextBox.Text, out var year))
            {
				ShowError(Translation.DentalSchools.PleaseEnterGraduationYear);

				return;
            }

			schoolClass.Year = year;
			schoolClass.Description = descriptionTextBox.Text;

			SchoolClasses.Save(schoolClass);

			DialogResult = DialogResult.OK;
		}
    }
}
