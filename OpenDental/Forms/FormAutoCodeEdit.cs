using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAutoCodeEdit : FormBase
	{
		private readonly AutoCode autoCode;
		private readonly List<AutoCodeItemState> autoCodeItemStates = new List<AutoCodeItemState>();
		private readonly List<AutoCodeItem> autoCodeItemsDeleted = new List<AutoCodeItem>();

		class AutoCodeItemState
        {
			public AutoCodeItem AutoCodeItem;

			public ProcedureCode ProcedureCode;

			public readonly List<AutoCodeConditionType> Conditions = new List<AutoCodeConditionType>();

			public AutoCodeItemState(AutoCodeItem autoCodeItem, IEnumerable<AutoCodeConditionType> conditions)
            {
				AutoCodeItem = autoCodeItem;

				Conditions.AddRange(conditions);

				ProcedureCode = ProcedureCodes.GetById(autoCodeItem.ProcedureCodeId);
			}
        }

		public FormAutoCodeEdit(AutoCode autoCode)
		{
			InitializeComponent();

			this.autoCode = autoCode;
		}

		private void FormAutoCodeEdit_Load(object sender, EventArgs e)
		{
			AutoCodeItems.RefreshCache();
			AutoCodeConditions.RefreshCache();

			var autoCodeConditions = AutoCodeConditions.GetAll();
			var autoCodeItemsAll = AutoCodeItems.GetByAutoCode(autoCode.Id);
			var autoCodeItemStates = autoCodeItemsAll.Select(autoCodeItem 
				=> new AutoCodeItemState(autoCodeItem, 
					autoCodeConditions
						.Where(autoCodeCondition => autoCodeCondition.AutoCodeItemId == autoCodeItem.Id)
						.Select(autoCodeCondition => autoCodeCondition.Type)));

			this.autoCodeItemStates.AddRange(autoCodeItemStates);

			descriptionTextBox.Text = autoCode.Description;
			hiddenCheckBox.Checked = autoCode.IsHidden;
			lessIntrusiveCheckBox.Checked = autoCode.LessIntrusive;

			FillGrid();
		}

		private void FillGrid()
		{
			autoCodeItemsGrid.BeginUpdate();
			autoCodeItemsGrid.Columns.Clear();
			autoCodeItemsGrid.Columns.Add(new GridColumn(Translation.Common.Code, 70));
			autoCodeItemsGrid.Columns.Add(new GridColumn(Translation.Common.Description, 250));
			autoCodeItemsGrid.Columns.Add(new GridColumn(Translation.Common.Conditions, 100));
			autoCodeItemsGrid.Rows.Clear();

			foreach (var autoCodeItemState in autoCodeItemStates)
            {
				var gridRow = new GridRow();
				gridRow.Cells.Add(autoCodeItemState.ProcedureCode.Code);
				gridRow.Cells.Add(autoCodeItemState.ProcedureCode.Description);
				gridRow.Cells.Add(string.Join(", ", autoCodeItemState.Conditions.Select(x => AutoCodeConditions.GetDescription(x))));
				gridRow.Tag = autoCodeItemState;

				autoCodeItemsGrid.Rows.Add(gridRow);
			}

			autoCodeItemsGrid.EndUpdate();
		}

		private void AutoCodeItemsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void AutoCodeItemsGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled = autoCodeItemsGrid.SelectedRows.Count > 0;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			using var formAutoItemEdit = new FormAutoCodeItemEdit();
			if (formAutoItemEdit.ShowDialog(this) != DialogResult.OK)
            {
				return;
            }

            var autoCodeItem = new AutoCodeItem
            {
                ProcedureCodeId = formAutoItemEdit.ProcedureCodeId
            };

			autoCodeItemStates.Add(new AutoCodeItemState(autoCodeItem, formAutoItemEdit.Conditions));

            FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var autoCodeItemState = autoCodeItemsGrid.SelectedTag<AutoCodeItemState>();
			if (autoCodeItemState == null)
			{
				return;
			}

			using var formAutoItemEdit = new FormAutoCodeItemEdit(autoCodeItemState.AutoCodeItem.ProcedureCodeId, autoCodeItemState.Conditions);
			if (formAutoItemEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			autoCodeItemState.AutoCodeItem.ProcedureCodeId = formAutoItemEdit.ProcedureCodeId;
			autoCodeItemState.Conditions.Clear();
			autoCodeItemState.Conditions.AddRange(formAutoItemEdit.Conditions);

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var autoCodeItemState = autoCodeItemsGrid.SelectedTag<AutoCodeItemState>();
			if (autoCodeItemState == null)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
            {
				return;
            }

			autoCodeItemStates.Remove(autoCodeItemState);
			if (autoCodeItemState.AutoCodeItem.Id > 0)
            {
				autoCodeItemsDeleted.Add(autoCodeItemState.AutoCodeItem);
            }

			FillGrid();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterDescription);

				return;
			}

			if (autoCodeItemStates.Count == 0)
			{
				ShowError(Translation.Common.MustHaveAtLeastOneItemInList);

				return;
			}

			if (HasDuplicates(autoCodeItemStates))
            {
				ShowError(Translation.Common.CannotHaveAutoCodeItemsWithDuplicateConditions);

				return;
            }

			autoCode.Description = description;
			autoCode.IsHidden = hiddenCheckBox.Checked;
			autoCode.LessIntrusive = lessIntrusiveCheckBox.Checked;

			AutoCodes.Save(autoCode);

			foreach (var autoCodeItemState in autoCodeItemStates)
            {
				var autoCodeItem = autoCodeItemState.AutoCodeItem;

				autoCodeItem.AutoCodeId = autoCode.Id;

				AutoCodeItems.Save(autoCodeItem);

				AutoCodeConditions.Set(autoCodeItem.Id, autoCodeItemState.Conditions);
            }

			foreach (var autoCodeItem in autoCodeItemsDeleted)
            {
				AutoCodeItems.Delete(autoCodeItem);
            }

			DialogResult = DialogResult.OK;
		}

		private static bool HasDuplicates(List<AutoCodeItemState> autoCodeItemStates)
		{
			if (autoCodeItemStates.Count < 2) return false;

			for (int i = 0; i < autoCodeItemStates.Count; i++)
			{
				for (int j = 0; j < autoCodeItemStates.Count; j++)
				{
					if (i == j) continue;

					var item1 = autoCodeItemStates[i];
					var item2 = autoCodeItemStates[j];

					if (item1.AutoCodeItem.ProcedureCodeId != item2.AutoCodeItem.ProcedureCodeId) continue;

					int matches = item1.Conditions.Count(type => item2.Conditions.Contains(type));
					if (matches == item2.Conditions.Count)
					{
						return true;
					}
				}
			}

			return false;
		}
    }
}
