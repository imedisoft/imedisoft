using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;

namespace OpenDental{
	/// <summary></summary>
	public partial class FormProcSelect : ODForm {
		#region Private Variables
		private long _patNumCur;
		///<summary>A list of completed procedures that are associated to this patient or their payment plans.</summary>
		private List<Procedure> _listProcedures;
		private List<PaySplit> _listPaySplits;
		private List<Adjustment> _listAdjustments;
		private List<PayPlanCharge> _listPayPlanCharges;
		private List<ClaimProc> _listInsPayAsTotal;
		private List<ClaimProc> _listClaimProcs;
		private List<AccountEntry> _listAccountCharges;
		///<summary>Does not perform FIFO logic.</summary>
		private bool _isSimpleView;
		///<summary>Set to true to enable multiple procedure selection mode.</summary>
		private bool _isMultiSelect;
		private Label labelUnallocated;
		private bool _doShowUnallocatedLabel;
		#endregion

		#region Public Variables
		///<summary>If form closes with OK, this contains selected proc num.</summary>
		public List<Procedure> ListSelectedProcs=new List<Procedure>();
		///<summary>List of paysplits for the current payment.</summary>
		public List<PaySplit> ListSplitsCur=new List<PaySplit>();
		public bool ShowTpProcs= Prefs.GetBool(PrefName.PrePayAllowedForTpProcs);
		#endregion

		///<summary>Displays completed procedures for the passed-in pat. 
		///Pass in true for isSimpleView to show all completed procedures, 
		///otherwise the user will be able to pick between credit allocation strategies (FIFO, Explicit, All).</summary>
		public FormProcSelect(long patNum,bool isSimpleView,bool isMultiSelect=false,bool doShowUnallocatedLabel=false) {
			InitializeComponent();
			
			_patNumCur=patNum;
			_isSimpleView=isSimpleView;
			_isMultiSelect=isMultiSelect;
			_doShowUnallocatedLabel=doShowUnallocatedLabel;
		}

		private void FormProcSelect_Load(object sender,System.EventArgs e) {
			if(_isMultiSelect) {
				gridMain.SelectionMode=OpenDental.UI.GridSelectionMode.MultiExtended;
			}
			_listProcedures=Procedures.GetCompleteForPats(new List<long> { _patNumCur });
			if(ShowTpProcs) {
				_listProcedures.AddRange(Procedures.GetTpForPats(new List<long> {_patNumCur}));
			}
			_listAdjustments=Adjustments.GetAdjustForPats(new List<long> { _patNumCur });
			_listPayPlanCharges=PayPlanCharges.GetDueForPayPlans(PayPlans.GetForPats(null,_patNumCur),_patNumCur).ToList();//Does not get charges for the future.
			_listPaySplits=PaySplits.GetForPats(new List<long> { _patNumCur });//Might contain payplan payments.
			foreach(PaySplit split in ListSplitsCur) {
				//If this is a new payment, its paysplits will not be in the database yet, so we need to add them manually. We might also need to set the
				//ProcNum on the pay split if it has changed and has not been saved to the database.
				PaySplit splitDb=_listPaySplits.FirstOrDefault(x => x.IsSame(split));
				if(splitDb==null) {
					_listPaySplits.Add(split);
				}
				else {
					splitDb.ProcNum=split.ProcNum;
				}
			}
			_listInsPayAsTotal=ClaimProcs.GetByTotForPats(new List<long> { _patNumCur });
			_listClaimProcs=ClaimProcs.GetForProcs(_listProcedures.Select(x => x.ProcNum).ToList());
			labelUnallocated.Visible=_doShowUnallocatedLabel;
			if(PrefC.GetInt(PrefName.RigorousAdjustments)==(int)RigorousAdjustments.DontEnforce) {
				radioIncludeAllCredits.Checked=true;
			}
			else {
				radioOnlyAllocatedCredits.Checked=true;
			}
			FillGrid();
		}

		private void FillGrid(){
			CreditCalcType credCalc;
			if(_isSimpleView) {
				credCalc = CreditCalcType.ExcludeAll;
				groupBreakdown.Visible=false;
				groupCreditLogic.Visible=false;
			}
			else if(radioIncludeAllCredits.Checked) {
				credCalc = CreditCalcType.IncludeAll;
			}
			else if(radioOnlyAllocatedCredits.Checked) {
				credCalc = CreditCalcType.AllocatedOnly;
			}
			else {
				credCalc= CreditCalcType.ExcludeAll;
			}
			_listAccountCharges=AccountModules.GetListUnpaidAccountCharges(_listProcedures, _listAdjustments,
				_listPaySplits, _listClaimProcs, _listPayPlanCharges, _listInsPayAsTotal, credCalc, ListSplitsCur);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col=new GridColumn("Date",70);
			gridMain.Columns.Add(col);
			col=new GridColumn("Prov",55);
			gridMain.Columns.Add(col);
			col=new GridColumn("Code",55);
			gridMain.Columns.Add(col);
			if(Clinics.IsMedicalClinic(Clinics.ClinicId)) {
				col=new GridColumn("Description",290);
				gridMain.Columns.Add(col);
			}
			else {
				col=new GridColumn("Tooth",40);
				gridMain.Columns.Add(col);
				col=new GridColumn("Description",250);
				gridMain.Columns.Add(col);
			}
			if(credCalc == CreditCalcType.ExcludeAll) {
				col=new GridColumn("Amt",40,HorizontalAlignment.Right){ IsWidthDynamic=true };
				gridMain.Columns.Add(col);
			}
			else {
				col=new GridColumn("Amt Orig",60,HorizontalAlignment.Right);
				gridMain.Columns.Add(col);
				col=new GridColumn("Amt Avail",60,HorizontalAlignment.Right);
				gridMain.Columns.Add(col);
				col=new GridColumn("Amt End",60,HorizontalAlignment.Right);
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			GridRow row;
			foreach(AccountEntry entry in _listAccountCharges) {
				if((entry.GetType()!=typeof(ProcExtended) || Math.Round(entry.AmountEnd,3) == 0) && credCalc!=CreditCalcType.ExcludeAll) {
					continue;
				}
				Procedure procCur = ((ProcExtended)entry.Tag).Proc;
				ProcedureCode procCodeCur = ProcedureCodes.GetProcCode(procCur.CodeNum);
				row=new GridRow();
				row.Cells.Add(procCur.ProcDate.ToShortDateString());
				row.Cells.Add(Providers.GetAbbr(entry.ProvNum));
				row.Cells.Add(procCodeCur.ProcCode);
				if(!Clinics.IsMedicalClinic(Clinics.ClinicId)) {
					row.Cells.Add(procCur.ToothNum=="" ? Tooth.SurfTidyFromDbToDisplay(procCur.Surf,procCur.ToothNum) : Tooth.ToInternat(procCur.ToothNum));
				}
				string descriptionText="";
				if(procCur.ProcStatus==ProcStat.TP) {
					descriptionText="(TP) ";
				}
				descriptionText+=procCodeCur.Descript;
				row.Cells.Add(descriptionText);
				row.Cells.Add(entry.AmountOriginal.ToString("f"));
				if(credCalc != CreditCalcType.ExcludeAll) {
					row.Cells.Add(entry.AmountAvailable.ToString("f"));
					row.Cells.Add(entry.AmountEnd.ToString("f"));
				}
				row.Tag=entry;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			if(!_isSimpleView) {
				RefreshBreakdown();
			}
		}

		private void RefreshBreakdown() {
			if(gridMain.GetSelectedIndex()==-1) {
				labelAmtOriginal.Text=(0).ToString("c");
				labelPositiveAdjs.Text=(0).ToString("c");
				labelNegativeAdjs.Text=(0).ToString("c");
				labelPayPlanCredits.Text=(0).ToString("c");
				labelPaySplits.Text=(0).ToString("c");
				labelInsEst.Text=(0).ToString("c");
				labelInsPay.Text=(0).ToString("c");
				labelOther.Text=(0).ToString("c");
				labelAmtStart.Text=(0).ToString("c");
				labelWriteOff.Text=(0).ToString("c");
				labelWriteOffEst.Text=(0).ToString("c");
				labelCurrentSplits.Text=(0).ToString("c");
				labelAmtEnd.Text=(0).ToString("c");
				return;
			}
			//there could be more than one proc selected if IsMultiSelect = true.
			List<AccountEntry> listSelectedEntries=gridMain.SelectedTags<AccountEntry>();
			List<ProcExtended> listSelectedProcExts=listSelectedEntries.Select(x => (ProcExtended)x.Tag).ToList();
			labelAmtOriginal.Text=    listSelectedProcExts.Sum(x => x.AmountOriginal).ToString("c");
			labelPositiveAdjs.Text=   listSelectedProcExts.Sum(x => x.PositiveAdjTotal).ToString("c");
			labelNegativeAdjs.Text=   listSelectedProcExts.Sum(x => x.NegativeAdjTotals).ToString("c");
			labelPayPlanCredits.Text= (-listSelectedProcExts.Sum(x => x.PayPlanCreditTotal)).ToString("c");
			labelPaySplits.Text=      (-listSelectedProcExts.Sum(x => x.PaySplitTotal)).ToString("c");
			labelInsEst.Text=         (-listSelectedProcExts.Sum(x => x.InsEstTotal)).ToString("c");
			labelInsPay.Text=         (-listSelectedProcExts.Sum(x => x.InsPayTotal)).ToString("c");
			labelWriteOff.Text=       (-listSelectedProcExts.Sum(x => x.WriteOffTotal)).ToString("c");
			labelWriteOffEst.Text=    (-listSelectedProcExts.Sum(x => x.WriteOffEstTotal)).ToString("c");
			//other credits apply when calculating using FIFO.
			labelOther.Text=          (-(listSelectedProcExts.Sum(x =>(decimal)x.AmountStart) - listSelectedEntries.Sum(y => y.AmountAvailable))).ToString("c");
			labelAmtStart.Text=       listSelectedEntries.Sum(y => y.AmountAvailable).ToString("c");
			labelCurrentSplits.Text=  (-listSelectedProcExts.Sum(x => x.SplitsCurTotal)).ToString("c");
			labelAmtEnd.Text=         listSelectedEntries.Sum(y => y.AmountEnd).ToString("c");
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			RefreshBreakdown();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			ListSelectedProcs.Add(((ProcExtended)gridMain.SelectedTag<AccountEntry>().Tag).Proc);
			DialogResult=DialogResult.OK;
		}

		private void radioCreditCalc_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				MessageBox.Show("Please select an item first.");
				return;
			}
			ListSelectedProcs.AddRange(gridMain.SelectedTags<AccountEntry>().Select(x => ((ProcExtended)x.Tag).Proc));
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
    }
	}
}





















