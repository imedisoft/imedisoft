using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormSchoolCourseRequirementEdit : FormBase
	{
		private readonly SchoolCourseRequirement schoolCourseRequirement;

		public FormSchoolCourseRequirementEdit(SchoolCourseRequirement schoolCourseRequirement)
		{
			InitializeComponent();

			this.schoolCourseRequirement = schoolCourseRequirement;
		}

		private void FormSchoolCourseRequirementEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = schoolCourseRequirement.Description;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
			{
				MessageBox.Show(Translation.Common.PleaseEnterDescription);

				return;
			}

			schoolCourseRequirement.Description = description;

			DialogResult = DialogResult.OK;
		}
	}
}
