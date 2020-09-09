using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormDiscountPlans : FormBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether the form is in selection mode.
		/// </summary>
		public bool IsSelectionMode { get; set; }

		/// <summary>
		/// Gets the selected plan.
		/// </summary>
		public DiscountPlan SelectedPlan { get; private set; }

		public FormDiscountPlans()
		{
			InitializeComponent();
		}

		private void FormDiscountPlans_Load(object sender, EventArgs e)
		{
			FillGrid();

			mergeButton.Visible = showHiddenCheckBox.Visible = !IsSelectionMode;
		}

		private void FillGrid()
		{
			var discountPlans = DiscountPlans.GetAll(showHiddenCheckBox.Checked);
			var discountPlanPatientCounts = DiscountPlans.GetPatCountsForPlans(discountPlans.Select(x => x.DiscountPlanNum).ToList());

			discountPlans.Sort(DiscountPlanComparer);

			discountPlansGrid.BeginUpdate();
			discountPlansGrid.Columns.Clear();
			discountPlansGrid.Columns.Add(new GridColumn(Translation.Common.Description, 200));
			discountPlansGrid.Columns.Add(new GridColumn(Translation.Common.FeeSchedule, 170));
			discountPlansGrid.Columns.Add(new GridColumn(Translation.Common.AdjustmentType, showHiddenCheckBox.Checked ? 150 : 170));
			discountPlansGrid.Columns.Add(new GridColumn(Translation.Common.Patients, 40));
			if (showHiddenCheckBox.Checked)
			{
				discountPlansGrid.Columns.Add(new GridColumn(Translation.Common.Hidden, 20, HorizontalAlignment.Center));
			}

			discountPlansGrid.Rows.Clear();

			int selectedIdx = -1;
			foreach (var discountPlan in discountPlans)
			{
				var adjustmentType = Definitions.GetDef(DefinitionCategory.AdjTypes, discountPlan.DefNum);

				var gridRow = new GridRow();
				gridRow.Cells.Add(discountPlan.Description);
				gridRow.Cells.Add(FeeScheds.GetDescription(discountPlan.FeeSchedNum));
				gridRow.Cells.Add((adjustmentType == null) ? "" : adjustmentType.Name);
				gridRow.Cells.Add(discountPlanPatientCounts.ContainsKey(discountPlan.DiscountPlanNum) ? 
					discountPlanPatientCounts[discountPlan.DiscountPlanNum].ToString() : "0");
				
				if (showHiddenCheckBox.Checked)
				{
					gridRow.Cells.Add(discountPlan.IsHidden ? "X" : "");
				}

				gridRow.Tag = discountPlan;

				discountPlansGrid.Rows.Add(gridRow);
				if (SelectedPlan != null && discountPlan.DiscountPlanNum == SelectedPlan.DiscountPlanNum)
				{
					selectedIdx = discountPlansGrid.Rows.Count - 1;
				}
			}

			discountPlansGrid.EndUpdate();
			discountPlansGrid.SetSelected(selectedIdx, true);
		}

		private void DiscountPlansGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (IsSelectionMode)
			{
				AcceptButton_Click(this, EventArgs.Empty);

				return;
			}

			var discountPlan = discountPlansGrid.SelectedTag<DiscountPlan>();
			if (discountPlan == null)
            {
				return;
            }

            using var formDiscountPlanEdit = new FormDiscountPlanEdit
            {
                DiscountPlanCur = discountPlan
			};

            if (formDiscountPlanEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.InsPlanEdit))
			{
				return;
			}

            var discountPlan = new DiscountPlan
            {
                IsNew = true
            };

            using var formDiscountPlanEdit = new FormDiscountPlanEdit
            {
                DiscountPlanCur = discountPlan
            };

            if (formDiscountPlanEdit.ShowDialog() == DialogResult.OK)
			{
				return;
			}

			SelectedPlan = discountPlan;

			FillGrid();
		}

		private void MergeButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.InsPlanEdit))
            {
                return;
            }

            using var formDiscountPlanMerge = new FormDiscountPlanMerge();

            formDiscountPlanMerge.ShowDialog(this);

            FillGrid();
        }

        private void ShowHiddenCheckBox_Click(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (IsSelectionMode)
			{
				if (discountPlansGrid.GetSelectedIndex() == -1)
				{
					ShowError(Translation.Common.PleaseSelectItem);

					return;
				}

				SelectedPlan = (DiscountPlan)discountPlansGrid.Rows[discountPlansGrid.GetSelectedIndex()].Tag;
			}

			DialogResult = DialogResult.OK;
		}

		private static int DiscountPlanComparer(DiscountPlan x, DiscountPlan y)
		{
			if (x.IsHidden != y.IsHidden)
			{
				return x.IsHidden.CompareTo(y.IsHidden);
			}

			return x.Description.CompareTo(y.Description);
		}
	}
}
