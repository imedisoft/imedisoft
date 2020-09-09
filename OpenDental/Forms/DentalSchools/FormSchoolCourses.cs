using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormSchoolCourses : FormBase
	{
		private bool hasChanges;

		/// <summary>
		/// Gets or sets a value indicating whether the form is in selection mode.
		/// </summary>
		public bool IsSelectionMode { get; set; }

		/// <summary>
		/// Gets the selected course.
		/// </summary>
		public SchoolCourse SelectedSchoolCourse => schoolCoursesGrid.SelectedTag<SchoolCourse>();

		public FormSchoolCourses()
		{
			InitializeComponent();
		}

		private void FormSchoolCourses_Load(object sender, EventArgs e)
		{
			if (IsSelectionMode)
			{
				addButton.Visible = false;
				acceptButton.Visible = true;
				cancelButton.Text = Translation.Common.CancelWithMnemonic;
			}

			FillGrid();
		}

		private void FillGrid()
		{
			SchoolCourses.RefreshCache();

			schoolCoursesGrid.BeginUpdate();
			schoolCoursesGrid.ListGridColumns.Clear();
			schoolCoursesGrid.ListGridColumns.Add(new GridColumn(Translation.DentalSchools.CourseID, 100));
			schoolCoursesGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Description, 80));
			schoolCoursesGrid.ListGridRows.Clear();

			foreach (var schoolCourse in SchoolCourses.GetAll())
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(schoolCourse.CourseID);
				gridRow.Cells.Add(schoolCourse.Description);
				gridRow.Tag = schoolCourse;

				schoolCoursesGrid.ListGridRows.Add(gridRow);
			}

			schoolCoursesGrid.EndUpdate();
		}

		private void SchoolCoursesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void SchoolCoursesGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled = SelectedSchoolCourse != null;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var schoolCourse = new SchoolCourse();

			using var formSchoolCourseEdit = new FormSchoolCourseEdit(schoolCourse);
			if (formSchoolCourseEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			hasChanges = true;

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var schoolCourse = SelectedSchoolCourse;
			if (schoolCourse == null)
			{
				return;
			}

			if (IsSelectionMode)
			{
				AcceptButton_Click(this, EventArgs.Empty);

				return;
			}

			using var formSchoolCourseEdit = new FormSchoolCourseEdit(schoolCourse);
			if (formSchoolCourseEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			hasChanges = true;

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var schoolCourse = SelectedSchoolCourse;
			if (schoolCourse == null)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
            {
				return;
            }

			try
			{
				SchoolCourses.Delete(schoolCourse);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			hasChanges = true;

			FillGrid();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!IsSelectionMode) return;

			if (SelectedSchoolCourse == null)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			DialogResult = DialogResult.OK;
		}

		private void FormSchoolCourses_Closing(object sender, CancelEventArgs e)
		{
			if (hasChanges)
			{
				CacheManager.RefreshGlobal(nameof(InvalidType.DentalSchools));
			}
		}
    }
}
