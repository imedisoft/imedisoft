using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using CodeBase;

namespace OpenDental {
	public partial class FormOrthoSetup:ODForm {
		private bool _hasChanges;
		///<summary>Set to the OrthoAutoProcCodeNum Pref value on load.  Can be changed by the user via this form.</summary>
		private long _orthoAutoProcCodeNum;
		///<summary>Filled upon load.</summary>
		private List<long> _listOrthoPlacementCodeNums= new List<long>();

		public FormOrthoSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormOrthoSetup_Load(object sender,EventArgs e) {
			checkPatClone.Checked=Prefs.GetBool(PrefName.ShowFeaturePatientClone);
			checkApptModuleShowOrthoChartItem.Checked=Prefs.GetBool(PrefName.ApptModuleShowOrthoChartItem);
			checkOrthoEnabled.Checked=Prefs.GetBool(PrefName.OrthoEnabled);
			checkOrthoFinancialInfoInChart.Checked=Prefs.GetBool(PrefName.OrthoCaseInfoInOrthoChart);
			checkOrthoClaimMarkAsOrtho.Checked=Prefs.GetBool(PrefName.OrthoClaimMarkAsOrtho);
			checkOrthoClaimUseDatePlacement.Checked=Prefs.GetBool(PrefName.OrthoClaimUseDatePlacement);
			textOrthoMonthsTreat.Text=Prefs.GetByte(PrefName.OrthoDefaultMonthsTreat).ToString();
			_orthoAutoProcCodeNum=Prefs.GetLong(PrefName.OrthoAutoProcCodeNum);
			textOrthoAutoProc.Text=ProcedureCodes.GetStringProcCode(_orthoAutoProcCodeNum);
			checkConsolidateInsPayment.Checked=Prefs.GetBool(PrefName.OrthoInsPayConsolidated);
			string strListOrthoNums = Prefs.GetString(PrefName.OrthoPlacementProcsList);
			if(strListOrthoNums!="") {
				_listOrthoPlacementCodeNums.AddRange(strListOrthoNums.Split(new char[] { ',' }).ToList().Select(x => PIn.Long(x)));
			}
			textBandingCodes.Text=Prefs.GetString(PrefName.OrthoBandingCodes);
			textVisitCodes.Text=Prefs.GetString(PrefName.OrthoVisitCodes);
			textDebondCodes.Text=Prefs.GetString(PrefName.OrthoDebondCodes);
			RefreshListBoxProcs();
		}

		private void RefreshListBoxProcs() {
			listboxOrthoPlacementProcs.Items.Clear();
			foreach(long orthoProcCodeNum in _listOrthoPlacementCodeNums) {
				ProcedureCode procCodeCur = ProcedureCodes.GetProcCode(orthoProcCodeNum);
				ODBoxItem<ProcedureCode> listBoxItem = new ODBoxItem<ProcedureCode>(procCodeCur.ProcCode,procCodeCur);
				listboxOrthoPlacementProcs.Items.Add(listBoxItem);
			}
		}

		private void butOrthoDisplayFields_Click(object sender,EventArgs e) {
			FormDisplayFieldsOrthoChart FormDFOC = new FormDisplayFieldsOrthoChart();
			FormDFOC.ShowDialog();
		}

		private void butPickOrthoProc_Click(object sender,EventArgs e) {
			FormProcCodes FormPC = new FormProcCodes();
			FormPC.IsSelectionMode=true;
			FormPC.ShowDialog();
			if(FormPC.DialogResult == DialogResult.OK) {
				_orthoAutoProcCodeNum=FormPC.SelectedCodeNum;
				textOrthoAutoProc.Text=ProcedureCodes.GetStringProcCode(_orthoAutoProcCodeNum);
			}
		}

		private void butPlacementProcsEdit_Click(object sender,EventArgs e) {
			FormProcCodes FormPC = new FormProcCodes();
			FormPC.IsSelectionMode = true;
			FormPC.ShowDialog();
			if(FormPC.DialogResult == DialogResult.OK) {
				_listOrthoPlacementCodeNums.Add(FormPC.SelectedCodeNum);
			}
			RefreshListBoxProcs();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(listboxOrthoPlacementProcs.SelectedIndices.Count == 0) {
				MessageBox.Show("Select an item to delete.");
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Are you sure you want to delete the selected items?")) {
				return;
			}
			foreach(ODBoxItem<ProcedureCode> boxItem in listboxOrthoPlacementProcs.SelectedItems) {
				_listOrthoPlacementCodeNums.Remove(boxItem.Tag.CodeNum);
			}
			RefreshListBoxProcs();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textOrthoMonthsTreat.errorProvider1.GetError(textOrthoMonthsTreat)!="") {
				MessageBox.Show("Default months treatment must be between 0 and 255 months.");
				return;
			}
			if(Prefs.GetBool(PrefName.ShowFeaturePatientClone)!=checkPatClone.Checked) {
				MessageBox.Show("You will need to restart OpenDental for this change to take effect.");
			}
			if(Prefs.Set(PrefName.ShowFeaturePatientClone,checkPatClone.Checked)
			| Prefs.Set(PrefName.ApptModuleShowOrthoChartItem,checkApptModuleShowOrthoChartItem.Checked)
			| Prefs.Set(PrefName.OrthoEnabled,checkOrthoEnabled.Checked)
			| Prefs.Set(PrefName.OrthoCaseInfoInOrthoChart,checkOrthoFinancialInfoInChart.Checked)
			| Prefs.Set(PrefName.OrthoClaimMarkAsOrtho,checkOrthoClaimMarkAsOrtho.Checked)
			| Prefs.Set(PrefName.OrthoClaimUseDatePlacement,checkOrthoClaimUseDatePlacement.Checked)
			| Prefs.Set(PrefName.OrthoDefaultMonthsTreat,PIn.Byte(textOrthoMonthsTreat.Text))
			| Prefs.Set(PrefName.OrthoInsPayConsolidated,checkConsolidateInsPayment.Checked)
			| Prefs.Set(PrefName.OrthoAutoProcCodeNum,_orthoAutoProcCodeNum)
			| Prefs.Set(PrefName.OrthoPlacementProcsList,string.Join(",",_listOrthoPlacementCodeNums))
			| Prefs.Set(PrefName.OrthoBandingCodes,PIn.String(textBandingCodes.Text))
			| Prefs.Set(PrefName.OrthoVisitCodes,PIn.String(textVisitCodes.Text))
			| Prefs.Set(PrefName.OrthoDebondCodes,PIn.String(textDebondCodes.Text))
			) {
				_hasChanges=true;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			_hasChanges=false;
			DialogResult=DialogResult.Cancel;
		}

		private void FormOrthoSetup_FormClosing(object sender,FormClosingEventArgs e) {
			if(_hasChanges) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}
	}
}