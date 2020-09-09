using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormGradingScales : FormBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether the form is in selection mode.
		/// </summary>
		public bool IsSelectionMode { get; set; }

		/// <summary>
		/// Gets the selected grading scale.
		/// </summary>
		public GradingScale SelectedGradingScale => gradingScalesGrid.SelectedTag<GradingScale>();

		public FormGradingScales()
		{
			InitializeComponent();
		}

		private void FormGradingScales_Load(object sender, EventArgs e)
		{
			if (IsSelectionMode)
			{
				acceptButton.Visible = true;
				cancelButton.Text = Translation.Common.CancelWithMnemonic;
			}

			FillGrid();
		}

		private void FillGrid()
		{
			gradingScalesGrid.BeginUpdate();
			gradingScalesGrid.Columns.Clear();
			gradingScalesGrid.Columns.Add(new GridColumn(Translation.Common.Description, 160));
			gradingScalesGrid.Rows.Clear();

			foreach (var gradingScale in GradingScales.GetAll())
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(gradingScale.Description);
				gridRow.Tag = gradingScale;

				gradingScalesGrid.Rows.Add(gridRow);
			}

			gradingScalesGrid.EndUpdate();
		}

		private void GradingScalesGrid_DoubleClick(object sender, EventArgs e)
		{
			if (IsSelectionMode)
			{
				AcceptButton_Click(this, EventArgs.Empty);

				return;
			}

			EditButton_Click(this, EventArgs.Empty);
		}

		private void GradingScalesGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled = SelectedGradingScale != null;
		}

		private void AddButtin_Click(object sender, EventArgs e)
		{
			var gradingScale = new GradingScale();

			using var formGradingScaleEdit = new FormGradingScaleEdit(gradingScale);
			if (formGradingScaleEdit.ShowDialog()!= DialogResult.OK)
            {
				return;
            }

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var gradingScale = SelectedGradingScale;
			if (gradingScale == null)
			{
				return;
			}

			using var formGradingScaleEdit = new FormGradingScaleEdit(gradingScale);
			if (formGradingScaleEdit.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var gradingScale = SelectedGradingScale;
			if (gradingScale == null)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDelete))
            {
				return;
            }

			try
			{
				GradingScales.Delete(gradingScale.Id);

				FillGrid();
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (SelectedGradingScale == null)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			DialogResult = DialogResult.OK;
		}
    }
}
