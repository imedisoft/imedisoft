using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using Imedisoft.Data;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormRecurringChargesHistory:ODForm {
		///<summary>The list of charges that have most recently been fetched from the database.</summary>
		private List<RecurringCharge> _listRecurringCharges;
		///<summary>Dictionary of patient names. Key is the PatNum.</summary>
		private Dictionary<long,string> _dictPatNames=new Dictionary<long,string>();
		///<summary>The date range that was selected the last time the charges were fetched from the database.</summary>
		private DateRange _previousDateRange;
		///<summary>The clinic nums that were selected the last time the charges were fetched from the database.</summary>
		private List<long> _listPreviousClinicNums;

		public FormRecurringChargesHistory() {
			InitializeComponent();
			
			gridMain.ContextMenu=contextMenu;
		}

		private void FormRecurringChargesHistory_Load(object sender,EventArgs e) {
			datePicker.SetDateTimeFrom(DateTime.Today.AddMonths(-1));
			datePicker.SetDateTimeTo(DateTime.Today);
			comboStatuses.FillWithEnum<RecurringChargeStatus>();
			comboStatuses.SetSelected(true);
			comboAutomated.Items.AddRange(new[] {
				"Automated and Manual",
				"Automated Only",
				"Manual Only",
			});
			comboAutomated.SelectedIndex=0;
			RefreshRecurringCharges();
			FillGrid();
		}

		///<summary>Gets recurring charges from the database.</summary>
		private void RefreshRecurringCharges() {
			Cursor=Cursors.WaitCursor;
			List<SQLWhere> listWheres=new List<SQLWhere> {
				SQLWhere.CreateBetween(nameof(RecurringCharge.DateTimeCharge),datePicker.GetDateTimeFrom(),datePicker.GetDateTimeTo(),true)
			};
			if(PrefC.HasClinicsEnabled) {
				listWheres.Add(SQLWhere.CreateIn(nameof(RecurringCharge.ClinicNum),comboClinics.ListSelectedClinicNums));
			}
			_listRecurringCharges=RecurringCharges.GetMany(listWheres);
			if(_listRecurringCharges.Any(x => !_dictPatNames.ContainsKey(x.PatNum))) {
				Dictionary<long,string> dictPatNames=Patients.GetPatientNames(_listRecurringCharges.Select(x => x.PatNum).ToList());
				dictPatNames.ForEach(x => _dictPatNames[x.Key]=x.Value);
			}
			_previousDateRange=new DateRange(datePicker.GetDateTimeFrom(),datePicker.GetDateTimeTo());
			_listPreviousClinicNums=comboClinics.ListSelectedClinicNums;
			Cursor=Cursors.Default;
		}

		///<summary>Will refresh charges from the database if necessary.</summary>
		private void FillGrid() {
			if(!_previousDateRange.IsInRange(datePicker.GetDateTimeFrom()) || !_previousDateRange.IsInRange(datePicker.GetDateTimeTo())
				|| comboClinics.ListSelectedClinicNums.Any(x => !_listPreviousClinicNums.Contains(x))) 
			{
				RefreshRecurringCharges();
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new GridColumn("PatNum",55,GridSortingStrategy.AmountParse));
			gridMain.Columns.Add(new GridColumn("Name",185));
			if(PrefC.HasClinicsEnabled) {
				gridMain.Columns.Add(new GridColumn("Clinic",65));
			}
			gridMain.Columns.Add(new GridColumn("Date Charge",135,HorizontalAlignment.Center,
				GridSortingStrategy.DateParse));
			gridMain.Columns.Add(new GridColumn("Charge Status",90));
			gridMain.Columns.Add(new GridColumn("User",90));
			gridMain.Columns.Add(new GridColumn("Family Bal",PrefC.HasClinicsEnabled ? 70 : 85,HorizontalAlignment.Right,
				GridSortingStrategy.AmountParse));
			gridMain.Columns.Add(new GridColumn("PayPlan Due",PrefC.HasClinicsEnabled ? 80 : 90,HorizontalAlignment.Right,
				GridSortingStrategy.AmountParse));
			gridMain.Columns.Add(new GridColumn("Total Due",PrefC.HasClinicsEnabled ? 65 : 80,HorizontalAlignment.Right,
				GridSortingStrategy.AmountParse));
			gridMain.Columns.Add(new GridColumn("Repeat Amt",PrefC.HasClinicsEnabled ? 75 : 90,HorizontalAlignment.Right,
				GridSortingStrategy.AmountParse));
			gridMain.Columns.Add(new GridColumn("Charge Amt",PrefC.HasClinicsEnabled ? 85 : 95,HorizontalAlignment.Right,
				GridSortingStrategy.AmountParse));
			if(gridMain.WidthAllColumns > gridMain.Width) {
				gridMain.HScrollVisible=true;
			}
			gridMain.Rows.Clear();
			foreach(RecurringCharge charge in _listRecurringCharges.OrderBy(x => x.DateTimeCharge)) {
				bool isAutomated=(charge.UserNum==0);
				if(!datePicker.IsInDateRange(charge.DateTimeCharge) 
					|| (PrefC.HasClinicsEnabled && !charge.ClinicNum.In(comboClinics.ListSelectedClinicNums))
					|| !charge.ChargeStatus.In(comboStatuses.SelectedTags<RecurringChargeStatus>())
					|| (comboAutomated.SelectedIndex==1 && !isAutomated) || (comboAutomated.SelectedIndex==2 && isAutomated))
				{
					continue;
				}
				GridRow row=new GridRow();
				row.Cells.Add(charge.PatNum.ToString());
				string patName;
				if(!_dictPatNames.TryGetValue(charge.PatNum,out patName)) {
					patName="UNKNOWN";
				}
				row.Cells.Add(patName);
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(Clinics.FirstOrDefault(x => x.Id==charge.ClinicNum,true)?.Description??"");
				}
				row.Cells.Add(charge.DateTimeCharge.ToString());
				row.Cells.Add(charge.ChargeStatus.GetDescription());
				row.Cells.Add(Users.FirstOrDefault(x => x.Id==charge.UserNum)?.UserName??"");
				row.Cells.Add(charge.FamBal.ToString("c"));
				row.Cells.Add(charge.PayPlanDue.ToString("c"));
				row.Cells.Add(charge.TotalDue.ToString("c"));
				row.Cells.Add(charge.RepeatAmt.ToString("c"));
				row.Cells.Add(charge.ChargeAmt.ToString("c"));
				row.Tag=charge;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void FilterChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			RefreshRecurringCharges();
			FillGrid();
		}
		
		private void contextMenu_Popup(object sender,EventArgs e) {
			RecurringCharge charge=gridMain.SelectedTag<RecurringCharge>();
			if(charge==null) {
				return;
			}
			menuItemOpenPayment.Visible=(charge.PayNum!=0);
			menuItemViewError.Visible=(charge.ChargeStatus==RecurringChargeStatus.ChargeFailed);
			menuItemDeletePending.Visible=(charge.ChargeStatus==RecurringChargeStatus.NotYetCharged);
		}
		
		private void menuItemGoTo_Click(object sender,EventArgs e) {
			RecurringCharge charge=gridMain.SelectedTag<RecurringCharge>();
			if(charge==null || !Security.IsAuthorized(Permissions.AccountModule)) {
				return;
			}
			GotoModule.GotoAccount(charge.PatNum);
		}

		private void menuItemOpenPayment_Click(object sender,EventArgs e) {
			RecurringCharge charge=gridMain.SelectedTag<RecurringCharge>();
			if(charge==null || charge.PayNum==0) {
				return;
			}
			Payment pay=Payments.GetPayment(charge.PayNum);
			if(pay==null) {//The payment has been deleted
				MessageBox.Show("This payment no longer exists.");
				return;
			}
			Patient pat=Patients.GetPat(pay.PatNum);
			Family fam=Patients.GetFamily(pat.PatNum);
			FormPayment FormP=new FormPayment(pat,fam,pay,false);
			FormP.ShowDialog();
		}

		private void menuItemViewError_Click(object sender,EventArgs e) {
			RecurringCharge charge=gridMain.SelectedTag<RecurringCharge>();
			if(charge==null) {
				return;
			}
			new MsgBoxCopyPaste(charge.ErrorMsg).Show();
		}

		private void menuItemDeletePending_Click(object sender,EventArgs e) {
			RecurringCharge charge=gridMain.SelectedTag<RecurringCharge>();
			if(charge==null || !MsgBox.Show(MsgBoxButtons.OKCancel,"Delete this pending recurring charge?"
				+"\r\n\r\nAnother user or service may be processing this card right now.")) 
			{
				return;
			}
			RecurringCharge chargeDb=RecurringCharges.GetOne(charge.RecurringChargeNum);
			if(chargeDb==null || chargeDb.ChargeStatus!=RecurringChargeStatus.NotYetCharged) {
				MessageBox.Show("This recurring charge is no longer pending. Unable to delete.");
				return;
			}
			RecurringCharges.Delete(charge.RecurringChargeNum);
			RefreshRecurringCharges();
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}
}