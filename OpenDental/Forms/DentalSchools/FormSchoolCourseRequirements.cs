using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormSchoolCourseRequirements : FormBase
	{
		private readonly List<SchoolCourseRequirement> schoolCourseRequirements = new List<SchoolCourseRequirement>();
		private readonly List<SchoolCourseRequirement> schoolCourseRequirementsDeleted = new List<SchoolCourseRequirement>();
		private bool hasChanges;

		public FormSchoolCourseRequirements()
		{
			InitializeComponent();
		}

		private void FormSchoolCourseRequirements_Load(object sender, EventArgs e)
		{
			foreach (var schoolClass in SchoolClasses.GetAll())
            {
				classComboBox.Items.Add(schoolClass);
				copyToClassComboBox.Items.Add(schoolClass);
            }

			foreach (var schoolCourse in SchoolCourses.GetAll())
			{
				courseComboBox.Items.Add(schoolCourse);
				copyToCourseComboBox.Items.Add(schoolCourse);
			}

			if (classComboBox.Items.Count > 0)
			{
				classComboBox.SelectedIndex = 0;
				copyToClassComboBox.SelectedIndex = 0;
			}

			if (courseComboBox.Items.Count > 0)
			{
				courseComboBox.SelectedIndex = 0;
				copyToCourseComboBox.SelectedIndex = 0;
			}

			schoolCourseRequirements.AddRange(SchoolCourseRequirements.GetAll());

			FillGrid();
		}

		private void FillGrid()
		{
            if (!(classComboBox.SelectedItem is SchoolClass schoolClass) || 
				!(courseComboBox.SelectedItem is SchoolCourse schoolCourse))
            {
                return;
            }

            long selectedRequirement = 0;
			if (requirementsGrid.GetSelectedIndex() != -1)
			{
				selectedRequirement = requirementsGrid.SelectedTag<SchoolCourseRequirement>().Id;
			}

			int scroll = requirementsGrid.ScrollValue;

			requirementsGrid.BeginUpdate();
			requirementsGrid.ListGridColumns.Clear();
			requirementsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Description, 200));
			requirementsGrid.ListGridRows.Clear();

			var requirements = schoolCourseRequirements
				.Where(schoolCourseRequirement => 
					schoolCourseRequirement.SchoolClassId == schoolClass.Id && 
					schoolCourseRequirement.SchoolCourseId == schoolCourse.Id)
				.OrderBy(schoolCourseRequirement =>
					schoolCourseRequirement.Description);

			foreach (var schoolCourseRequirement in requirements)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(schoolCourseRequirement.Description);
				gridRow.Tag = schoolCourseRequirement;

				requirementsGrid.ListGridRows.Add(gridRow);

				if (schoolCourseRequirement.Id == selectedRequirement)
				{
					requirementsGrid.SetSelected(requirementsGrid.ListGridRows.Count - 1, true);
				}
			}

			requirementsGrid.EndUpdate();
			requirementsGrid.ScrollValue = scroll;

			deleteAllButton.Enabled = requirementsGrid.ListGridRows.Count > 0;
		}

		private void ClassComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void CourseComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
            if (!(classComboBox.SelectedItem is SchoolClass schoolClass) || 
				!(courseComboBox.SelectedItem is SchoolCourse schoolCourse))
            {
                ShowError(Translation.DentalSchools.PleaseSelectClassAndCourseFirst);

                return;
            }

            var schoolCourseRequirement = new SchoolCourseRequirement
            {
                SchoolClassId = schoolClass.Id,
                SchoolCourseId = schoolCourse.Id
            };

            using var formSchoolCourseRequirementEdit = new FormSchoolCourseRequirementEdit(schoolCourseRequirement);
			if (formSchoolCourseRequirementEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			var exists = requirementsGrid.GetTags<SchoolCourseRequirement>()
				.Any(x =>
					x.Description == schoolCourseRequirement.Description &&
					x.SchoolClassId == schoolCourseRequirement.SchoolClassId &&
					x.SchoolCourseId == schoolCourseRequirement.SchoolCourseId);

			if (exists)
			{
				ShowError(Translation.DentalSchools.RequirementAlreadyExists);

				return;
			}
			
			schoolCourseRequirements.Add(schoolCourseRequirement);

			hasChanges = true;

			FillGrid();
		}

		private void RequirementsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void RequirementsGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled =
				requirementsGrid.SelectedTag<SchoolCourseRequirement>() != null;
		}

		private void CopyToButton_Click(object sender, EventArgs e)
		{
            if (!(classComboBox.SelectedItem is SchoolClass copyFromClass) || 
				!(copyToClassComboBox.SelectedItem is SchoolClass copyToClass) || 
				!(courseComboBox.SelectedItem is SchoolCourse copyFromCourse) || 
				!(copyToCourseComboBox.SelectedItem is SchoolCourse copyToCourse))
            {
                ShowError(Translation.DentalSchools.PleaseSelectClassAndCourseFirst);

                return;
            }

            if (!Confirm(Translation.DentalSchools.ConfirmCopyRequirements))
			{
				return;
			}

			if (copyFromClass.Id == copyToClass.Id && copyFromCourse.Id == copyToCourse.Id)
			{
				return;
			}

			foreach (var schoolCourseRequirement in requirementsGrid.GetTags<SchoolCourseRequirement>())
			{
                var newSchoolCourseRequirement = new SchoolCourseRequirement
                {
                    Description = schoolCourseRequirement.Description,
                    SchoolClassId = copyToClass.Id,
                    SchoolCourseId = copyToCourse.Id
                };

				var exists = schoolCourseRequirements
					.Any(x =>
						x.Description == schoolCourseRequirement.Description &&
						x.SchoolClassId == schoolCourseRequirement.SchoolClassId &&
						x.SchoolCourseId == schoolCourseRequirement.SchoolCourseId);

				if (exists)
				{
					continue;
				}

				schoolCourseRequirements.Add(newSchoolCourseRequirement);
			}

			classComboBox.SelectedIndex = copyToClassComboBox.SelectedIndex;
			courseComboBox.SelectedIndex = copyToCourseComboBox.SelectedIndex;

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var schoolCourseRequirement = requirementsGrid.SelectedTag<SchoolCourseRequirement>();
			if (schoolCourseRequirement == null)
			{
				return;
			}

			using var formSchoolCourseRequirementEdit = new FormSchoolCourseRequirementEdit(schoolCourseRequirement);
			if (formSchoolCourseRequirementEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			hasChanges = true;

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var schoolCourseRequirement = requirementsGrid.SelectedTag<SchoolCourseRequirement>();
			if (schoolCourseRequirement == null)
			{
				return;
			}

			if (StudentResults.IsInUseBy(schoolCourseRequirement.Id, out var studentNames))
			{
				var message =
					Translation.DentalSchools.RequirementAlreadyInUseConfirmDelete + 
					"\r\n" + studentNames;

				if (!Confirm(message))
				{
					return;
				}
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
			{
				return;
			}

			schoolCourseRequirements.Remove(schoolCourseRequirement);
			schoolCourseRequirementsDeleted.Add(schoolCourseRequirement);

			FillGrid();
		}

		private void DeleteAllButton_Click(object sender, EventArgs e)
		{
			if (!Confirm(Translation.DentalSchools.ConfirmDeleteRequirements))
			{
				return;
			}

			foreach (var schoolCourseRequirement in requirementsGrid.GetTags<SchoolCourseRequirement>())
			{
				schoolCourseRequirements.Remove(schoolCourseRequirement);
				schoolCourseRequirementsDeleted.Add(schoolCourseRequirement);
			}

			FillGrid();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			foreach (var schoolCourseRequirement in schoolCourseRequirementsDeleted)
            {
				SchoolCourseRequirements.Delete(schoolCourseRequirement.Id);
            }

			if (hasChanges)
            {
				foreach (var schoolCourseRequirement in schoolCourseRequirements)
                {
					SchoolCourseRequirements.Save(schoolCourseRequirement);
                }
            }

			DialogResult = DialogResult.OK;
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}
    }
}
