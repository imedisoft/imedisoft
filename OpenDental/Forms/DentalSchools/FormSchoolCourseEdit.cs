using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormSchoolCourseEdit : FormBase
	{
		private readonly SchoolCourse schoolCourse;

		public FormSchoolCourseEdit(SchoolCourse schoolCourse)
		{
			InitializeComponent();

			this.schoolCourse = schoolCourse;
		}

		private void FormSchoolCourseEdit_Load(object sender, EventArgs e)
		{
			courseIdTextBox.Text = schoolCourse.CourseID;
			descriptionTextBox.Text = schoolCourse.Description;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
            {
				ShowError(Translation.Common.PleaseEnterDescription);

				return;
            }

			schoolCourse.CourseID = courseIdTextBox.Text;
			schoolCourse.Description = description;

			SchoolCourses.Save(schoolCourse);

			DialogResult = DialogResult.OK;
		}
	}
}
