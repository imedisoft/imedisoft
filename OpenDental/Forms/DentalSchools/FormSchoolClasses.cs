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
    public partial class FormSchoolClasses : FormBase
	{
		private bool hasChanges;

		public FormSchoolClasses()
		{
			InitializeComponent();
		}

		private void FormSchoolClasses_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			SchoolClasses.RefreshCache();

			schoolClassesGrid.BeginUpdate();
			schoolClassesGrid.Columns.Clear();
			schoolClassesGrid.Columns.Add(new GridColumn(Translation.Common.Description, 80));
			schoolClassesGrid.Rows.Clear();

			foreach (var schoolClass in SchoolClasses.GetAll())
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(schoolClass.Description);
				gridRow.Tag = schoolClass;

				schoolClassesGrid.Rows.Add(gridRow);
			}

			schoolClassesGrid.EndUpdate();
		}

		private void SchoolClassesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void SchoolClassesGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled =
				schoolClassesGrid.SelectedTag<SchoolClass>() != null;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var schoolClass = new SchoolClass();

			using var formSchoolClassEdit = new FormSchoolClassEdit(schoolClass);
			if (formSchoolClassEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			hasChanges = true;

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var schoolClass = schoolClassesGrid.SelectedTag<SchoolClass>();
			if (schoolClass == null)
			{
				return;
			}

			using var formSchoolClassEdit = new FormSchoolClassEdit(schoolClass);
			if (formSchoolClassEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			hasChanges = true;

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var schoolClass = schoolClassesGrid.SelectedTag<SchoolClass>();
			if (schoolClass == null)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
			{
				return;
			}

			try
			{
				SchoolClasses.Delete(schoolClass);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			hasChanges = true;

			FillGrid();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void FormSchoolClasses_Closing(object sender, CancelEventArgs e)
		{
			if (hasChanges)
			{
				CacheManager.RefreshGlobal(nameof(InvalidType.DentalSchools));
			}
		}
    }
}
