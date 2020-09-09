using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAdjustSelect : FormBase
	{
		public Adjustment SelectedAdj;
		private List<PaySplit> _listSplitsCur;

		///<summary>The display the opposite amount of the amtPaySplitCur that was originally passed in.
		///This is because PaySplits are stored in the opposite of how they are applied.
		///E.g. A PaySplit stored as $30 will subtract $30 from something (and visa versa).</summary>
		private double _amtPaySplitCurDisplay;

		///<summary>Account entries made out of all adjustments that are not associated to a procedure for the patient.</summary>
		private List<AccountEntry> _listPatAdjEntries = new List<AccountEntry>();

		public FormAdjustSelect(double amtPaySplitCur, PaySplit paySplitCur, List<PaySplit> listSplitsCur, List<Adjustment> adjustments, List<PaySplit> listAdjPaySplits)
		{
			InitializeComponent();

			//Flip the sign on the pay split amount passed in so that.
			_amtPaySplitCurDisplay = amtPaySplitCur * -1;
			_listSplitsCur = listSplitsCur;

			//Convert all unattached adjustments to account entries.
			_listPatAdjEntries = adjustments
				.FindAll(x => x.ProcedureId == 0)
				.Select(x => new AccountEntry(x))
				.ToList();

			foreach (AccountEntry entry in _listPatAdjEntries)
			{
				//Figure out how much each adjustment has left, not counting this payment.
				entry.AmountAvailable -= (decimal)Adjustments.GetAmtAllocated(
					entry.PriKey, paySplitCur.PayNum, listAdjPaySplits.FindAll(x => x.AdjNum == entry.PriKey));

				//Reduce adjustments based on current payment's splits as well but not the current split (could be new, could be modified).
				entry.AmountAvailable -= (decimal)Adjustments.GetAmtAllocated(
					entry.PriKey, 0, _listSplitsCur.FindAll(x => x.AdjNum == entry.PriKey && x != paySplitCur));
			}
		}

		private void FormAdjustSelect_Load(object sender, EventArgs e)
		{
			amountCurrentSplitLabel.Text = _amtPaySplitCurDisplay.ToString("C");

			FillGrid();
		}

		private void FillGrid()
		{
			adjustmentsGrid.BeginUpdate();
			adjustmentsGrid.Rows.Clear();
			adjustmentsGrid.Columns.Clear();
			adjustmentsGrid.Columns.Add(new GridColumn(Translation.Common.Date, 70, HorizontalAlignment.Center));
			adjustmentsGrid.Columns.Add(new GridColumn("Provider", 60) { IsWidthDynamic = true });
			adjustmentsGrid.Columns.Add(new GridColumn("Clinic", 60) { IsWidthDynamic = true });
			adjustmentsGrid.Columns.Add(new GridColumn("Amt. Orig.", 60, HorizontalAlignment.Right));
			adjustmentsGrid.Columns.Add(new GridColumn("Amt. Avail.", 60, HorizontalAlignment.Right));

			foreach (var accountEntry in _listPatAdjEntries)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(((Adjustment)accountEntry.Tag).AdjustDate.ToShortDateString());
				gridRow.Cells.Add(Providers.GetAbbr(((Adjustment)accountEntry.Tag).ProviderId));
				gridRow.Cells.Add(Clinics.GetAbbr(((Adjustment)accountEntry.Tag).ClinicId));
				gridRow.Cells.Add(accountEntry.AmountOriginal.ToString("F"));
				gridRow.Cells.Add(accountEntry.AmountAvailable.ToString("F"));
				gridRow.Tag = accountEntry;

				adjustmentsGrid.Rows.Add(gridRow);
			}

			adjustmentsGrid.EndUpdate();
		}

		private void AdjustmentsGrid_CellClick(object sender, ODGridClickEventArgs e)
		{
			var entry = adjustmentsGrid.SelectedTag<AccountEntry>();
			if (entry == null)
            {
				return;
            }

			// Figure out the amount that has already been used (orig - avail) and then flip the sign for display purposes (like _amtPaySplitCurDisplay).
			decimal amtUsedDisplay = (entry.AmountOriginal - entry.AmountAvailable) * -1;

			amountOriginalLabel.Text = entry.AmountOriginal.ToString("C");
			amountUsedLabel.Text = amtUsedDisplay.ToString("C");
			amountAvailableLabel.Text = entry.AmountAvailable.ToString("C");
			amountEndLabel.Text = ((double)entry.AmountAvailable + _amtPaySplitCurDisplay).ToString("C");
		}

		private void AdjustmentsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			AcceptButton_Click(this, EventArgs.Empty);
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (adjustmentsGrid.SelectedIndices.Length < 1)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			SelectedAdj = (Adjustment)adjustmentsGrid.SelectedTag<AccountEntry>().Tag;
			DialogResult = DialogResult.OK;
		}
	}
}
