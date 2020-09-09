using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormGradingScaleEdit : FormBase
	{
		private readonly GradingScale gradingScale;
		private readonly List<GradingScaleItem> gradingScaleItems = new List<GradingScaleItem>();
		private readonly List<GradingScaleItem> gradingScaleItemsDeleted = new List<GradingScaleItem>();
		private bool gradingScaleItemsChanged;
		private bool isReadOnly;

		/// <summary>
		/// Gets the selected grading scale type.
		/// </summary>
		private GradingScaleType SelectedGradingScaleType 
			=> (GradingScaleType)typeComboBox.SelectedIndex;

		public FormGradingScaleEdit(GradingScale gradingScale)
		{
			InitializeComponent();

			this.gradingScale = gradingScale;
		}

		private void FormGradingScaleEdit_Load(object sender, EventArgs e)
		{
			gradingScaleItems.AddRange(GradingScaleItems.GetByGradingScale(gradingScale.Id));

			if (gradingScale.Type == GradingScaleType.Percentage)
			{
				percentLabel.Visible = true;
			}

			descriptionTextBox.Text = gradingScale.Description;

			typeComboBox.Items.Clear();
			typeComboBox.Items.Add(new DataItem<GradingScaleType>(GradingScaleType.PickList, Translation.Common.PickList));
			typeComboBox.Items.Add(new DataItem<GradingScaleType>(GradingScaleType.Percentage, Translation.Common.Percentage));
			typeComboBox.Items.Add(new DataItem<GradingScaleType>(GradingScaleType.Weighted, Translation.Common.Weighted));
			typeComboBox.SelectedIndex = (int)gradingScale.Type;

			TypeComboBox_SelectionChangeCommitted(this, EventArgs.Empty); // TODO: Do we need this?
			
			if (gradingScale.Id == 0)
			{
				return;
			}

			isReadOnly = GradingScales.IsInUseByEvaluation(gradingScale);
			if (isReadOnly)
			{
				warningLabel.Text = Translation.DentalSchools.GradingScaleCannotBeModifiedInUseByEvaluation;
				warningLabel.Visible = true;
				addButton.Enabled = false;
				editButton.Enabled = false;
				deleteButton.Enabled = false;
				descriptionTextBox.ReadOnly = true;
				typeComboBox.Enabled = false;
			}

			FillGrid();
		}

		private void FillGrid()
		{
			gradingScaleItemsGrid.BeginUpdate();
			gradingScaleItemsGrid.Columns.Clear();
			gradingScaleItemsGrid.Columns.Add(new GridColumn(Translation.Common.Shown, 60));
			gradingScaleItemsGrid.Columns.Add(new GridColumn(Translation.Common.Number, 60));
			gradingScaleItemsGrid.Columns.Add(new GridColumn(Translation.Common.Description, 160));
			gradingScaleItemsGrid.Rows.Clear();

			if (SelectedGradingScaleType == GradingScaleType.PickList)
			{
				foreach (var gradingScaleItem in gradingScaleItems)
				{
					var gridRow = new GridRow();
					gridRow.Cells.Add(gradingScaleItem.Text);
					gridRow.Cells.Add(gradingScaleItem.Value.ToString());
					gridRow.Cells.Add(gradingScaleItem.Description);
					gridRow.Tag = gradingScaleItem;

					gradingScaleItemsGrid.Rows.Add(gridRow);
				}
			}

			gradingScaleItemsGrid.EndUpdate();
		}

		private void GradingScaleItemsGrid_DoubleClick(object sender, EventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void GradingScaleItemsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled 
				= !isReadOnly && gradingScaleItemsGrid.SelectedTag<GradingScaleItem>() != null;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
            var gradingScaleItem = new GradingScaleItem
            {
                GradingScaleId = gradingScale.Id
            };

            using var formGradingScaleItemEdit = new FormGradingScaleItemEdit(gradingScaleItem);
			if (formGradingScaleItemEdit.ShowDialog()!= DialogResult.OK)
            {
				return;
            }

			gradingScaleItems.Add(gradingScaleItem);
			gradingScaleItemsChanged = true;

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			if (isReadOnly) return;

			var gradingScaleItem = gradingScaleItemsGrid.SelectedTag<GradingScaleItem>();
			if (gradingScaleItem == null)
			{
				return;
			}

			using var formGradingScaleItemEdit = new FormGradingScaleItemEdit(gradingScaleItem);
			if (formGradingScaleItemEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			gradingScaleItemsChanged = true;

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (isReadOnly) return;

			var gradingScaleItem = gradingScaleItemsGrid.SelectedTag<GradingScaleItem>();
			if (gradingScaleItem == null)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDelete))
			{
				return;
			}

			gradingScaleItems.Remove(gradingScaleItem);
			gradingScaleItemsDeleted.Add(gradingScaleItem);

			FillGrid();
		}

		private void TypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			var gradingScaleType = SelectedGradingScaleType;

			if (gradingScaleType == GradingScaleType.PickList)
			{
				addButton.Enabled = true;
				warningLabel.Visible = false;
				percentLabel.Visible = false;
			}
			else if (gradingScaleType == GradingScaleType.Percentage)
			{
				addButton.Enabled = false;
				warningLabel.Visible = true;
				percentLabel.Visible = true;
			}
			else
			{
				addButton.Enabled = false;
				warningLabel.Visible = true;
				percentLabel.Visible = false;
			}

			FillGrid();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (isReadOnly)
            {
				DialogResult = DialogResult.OK;

				return;
            }

			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterDescription);

				return;
			}

			if (GradingScales.IsDupicateDescription(description, gradingScale.Id))
			{
				ShowError(Translation.DentalSchools.PleaseEnterUniqueGradingScaleDescription);

				return;
			}

			gradingScale.Description = description;
			gradingScale.Type = SelectedGradingScaleType;

			GradingScales.Save(gradingScale);

			if (gradingScaleItemsChanged)
            {
				foreach (var gradingScaleItem in gradingScaleItems)
                {
					gradingScaleItem.GradingScaleId = gradingScale.Id;

					GradingScaleItems.Save(gradingScaleItem);
				}
            }

			foreach (var gradingScaleItem in gradingScaleItemsDeleted)
            {
				GradingScaleItems.Delete(gradingScaleItem);
            }

			DialogResult = DialogResult.OK;
		}
    }
}
