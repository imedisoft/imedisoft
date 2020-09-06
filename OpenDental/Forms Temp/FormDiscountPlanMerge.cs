using System;
using System.IO;
using System.Windows.Forms;
using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using Imedisoft.Forms;
using OpenDentBusiness;

namespace OpenDental
{
	public partial class FormDiscountPlanMerge : ODForm
	{
		private DiscountPlan _planInto;
		private DiscountPlan _planFrom;

		public FormDiscountPlanMerge()
		{
			InitializeComponent();
		}

		private void FormDiscountPlanMerge_Load(object sender, EventArgs e)
		{
		}

		private void butChangePlanInto_Click(object sender, EventArgs e)
		{
			FormDiscountPlans FormDPs = new FormDiscountPlans();
			FormDPs.IsSelectionMode = true;
			if (FormDPs.ShowDialog() == DialogResult.OK)
			{
				_planInto = FormDPs.SelectedPlan;
				textDescriptionInto.Text = _planInto.Description;
				textFeeSchedInto.Text = FeeScheds.GetDescription(_planInto.FeeSchedNum);
				textAdjTypeInto.Text = Definitions.GetName(DefinitionCategory.AdjTypes, _planInto.DefNum);
			}
			CheckUIState();
		}

		private void butChangePlanFrom_Click(object sender, EventArgs e)
		{
			FormDiscountPlans FormDPs = new FormDiscountPlans();
			FormDPs.IsSelectionMode = true;
			if (FormDPs.ShowDialog() == DialogResult.OK)
			{
				_planFrom = FormDPs.SelectedPlan;
				textDescriptionFrom.Text = _planFrom.Description;
				textFeeSchedFrom.Text = FeeScheds.GetDescription(_planFrom.FeeSchedNum);
				textAdjTypeFrom.Text = Definitions.GetName(DefinitionCategory.AdjTypes, _planFrom.DefNum);
			}
			CheckUIState();
		}

		private void CheckUIState()
		{
			butMerge.Enabled = (_planInto != null && _planFrom != null);
		}

		private void butMerge_Click(object sender, EventArgs e)
		{
			if (_planFrom.DiscountPlanNum == _planInto.DiscountPlanNum)
			{
				MessageBox.Show("You must select two different Discount Plans to merge.");
				return;
			}

			if (!MsgBox.Show(MsgBoxButtons.YesNo, "Merge the Discount Plan at the bottom into the Discount Plan shown at the top?"))
			{
				return;//The user chose not to merge.
			}

			Cursor = Cursors.WaitCursor;
			DiscountPlans.MergeTwoPlans(_planInto, _planFrom);
			Cursor = Cursors.Default;
			SecurityLogs.MakeLogEntry(Permissions.DiscountPlanMerge, 0, $"{_planFrom.Description} merged into {_planInto.Description}");
			MessageBox.Show("Plans merged successfully.");
			_planFrom = null;
			textDescriptionFrom.Text = "";
			textFeeSchedFrom.Text = "";
			textAdjTypeFrom.Text = "";
			CheckUIState();
		}

		private void butClose_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
