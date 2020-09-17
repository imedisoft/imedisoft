using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormOrthoSetup : ODForm
	{
		private bool _hasChanges;
		///<summary>Set to the OrthoAutoProcCodeNum Pref value on load.  Can be changed by the user via this form.</summary>
		private long _orthoAutoProcCodeNum;
		///<summary>Filled upon load.</summary>
		private List<long> _listOrthoPlacementCodeNums = new List<long>();

		public FormOrthoSetup()
		{
			InitializeComponent();

		}

		private void FormOrthoSetup_Load(object sender, EventArgs e)
		{
			checkPatClone.Checked = Preferences.GetBool(PreferenceName.ShowFeaturePatientClone);
			checkApptModuleShowOrthoChartItem.Checked = Preferences.GetBool(PreferenceName.ApptModuleShowOrthoChartItem);
			checkOrthoEnabled.Checked = Preferences.GetBool(PreferenceName.OrthoEnabled);
			checkOrthoFinancialInfoInChart.Checked = Preferences.GetBool(PreferenceName.OrthoCaseInfoInOrthoChart);
			checkOrthoClaimMarkAsOrtho.Checked = Preferences.GetBool(PreferenceName.OrthoClaimMarkAsOrtho);
			checkOrthoClaimUseDatePlacement.Checked = Preferences.GetBool(PreferenceName.OrthoClaimUseDatePlacement);
			textOrthoMonthsTreat.Text = Preferences.GetByte(PreferenceName.OrthoDefaultMonthsTreat).ToString();
			_orthoAutoProcCodeNum = Preferences.GetLong(PreferenceName.OrthoAutoProcCodeNum);
			textOrthoAutoProc.Text = ProcedureCodes.GetStringProcCode(_orthoAutoProcCodeNum);
			checkConsolidateInsPayment.Checked = Preferences.GetBool(PreferenceName.OrthoInsPayConsolidated);
			string strListOrthoNums = Preferences.GetString(PreferenceName.OrthoPlacementProcsList);
			if (strListOrthoNums != "")
			{
				_listOrthoPlacementCodeNums.AddRange(strListOrthoNums.Split(new char[] { ',' }).ToList().Select(x => PIn.Long(x)));
			}
			textBandingCodes.Text = Preferences.GetString(PreferenceName.OrthoBandingCodes);
			textVisitCodes.Text = Preferences.GetString(PreferenceName.OrthoVisitCodes);
			textDebondCodes.Text = Preferences.GetString(PreferenceName.OrthoDebondCodes);
			RefreshListBoxProcs();
		}

		private void RefreshListBoxProcs()
		{
			listboxOrthoPlacementProcs.Items.Clear();
			foreach (long orthoProcCodeNum in _listOrthoPlacementCodeNums)
			{
				ProcedureCode procCodeCur = ProcedureCodes.GetProcCode(orthoProcCodeNum);
				ODBoxItem<ProcedureCode> listBoxItem = new ODBoxItem<ProcedureCode>(procCodeCur.Code, procCodeCur);
				listboxOrthoPlacementProcs.Items.Add(listBoxItem);
			}
		}

		private void butOrthoDisplayFields_Click(object sender, EventArgs e)
		{
			FormDisplayFieldsOrthoChart FormDFOC = new FormDisplayFieldsOrthoChart();
			FormDFOC.ShowDialog();
		}

		private void butPickOrthoProc_Click(object sender, EventArgs e)
		{
			FormProcCodes FormPC = new FormProcCodes();
			FormPC.IsSelectionMode = true;
			FormPC.ShowDialog();
			if (FormPC.DialogResult == DialogResult.OK)
			{
				_orthoAutoProcCodeNum = FormPC.SelectedCodeNum;
				textOrthoAutoProc.Text = ProcedureCodes.GetStringProcCode(_orthoAutoProcCodeNum);
			}
		}

		private void butPlacementProcsEdit_Click(object sender, EventArgs e)
		{
			FormProcCodes FormPC = new FormProcCodes();
			FormPC.IsSelectionMode = true;
			FormPC.ShowDialog();
			if (FormPC.DialogResult == DialogResult.OK)
			{
				_listOrthoPlacementCodeNums.Add(FormPC.SelectedCodeNum);
			}
			RefreshListBoxProcs();
		}

		private void butDelete_Click(object sender, EventArgs e)
		{
			if (listboxOrthoPlacementProcs.SelectedIndices.Count == 0)
			{
				MessageBox.Show("Select an item to delete.");
				return;
			}
			if (!MsgBox.Show(MsgBoxButtons.YesNo, "Are you sure you want to delete the selected items?"))
			{
				return;
			}
			foreach (ODBoxItem<ProcedureCode> boxItem in listboxOrthoPlacementProcs.SelectedItems)
			{
				_listOrthoPlacementCodeNums.Remove(boxItem.Tag.Id);
			}
			RefreshListBoxProcs();
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			if (textOrthoMonthsTreat.errorProvider1.GetError(textOrthoMonthsTreat) != "")
			{
				MessageBox.Show("Default months treatment must be between 0 and 255 months.");
				return;
			}
			if (Preferences.GetBool(PreferenceName.ShowFeaturePatientClone) != checkPatClone.Checked)
			{
				MessageBox.Show("You will need to restart OpenDental for this change to take effect.");
			}
			if (Preferences.Set(PreferenceName.ShowFeaturePatientClone, checkPatClone.Checked)
			| Preferences.Set(PreferenceName.ApptModuleShowOrthoChartItem, checkApptModuleShowOrthoChartItem.Checked)
			| Preferences.Set(PreferenceName.OrthoEnabled, checkOrthoEnabled.Checked)
			| Preferences.Set(PreferenceName.OrthoCaseInfoInOrthoChart, checkOrthoFinancialInfoInChart.Checked)
			| Preferences.Set(PreferenceName.OrthoClaimMarkAsOrtho, checkOrthoClaimMarkAsOrtho.Checked)
			| Preferences.Set(PreferenceName.OrthoClaimUseDatePlacement, checkOrthoClaimUseDatePlacement.Checked)
			| Preferences.Set(PreferenceName.OrthoDefaultMonthsTreat, PIn.Byte(textOrthoMonthsTreat.Text))
			| Preferences.Set(PreferenceName.OrthoInsPayConsolidated, checkConsolidateInsPayment.Checked)
			| Preferences.Set(PreferenceName.OrthoAutoProcCodeNum, _orthoAutoProcCodeNum)
			| Preferences.Set(PreferenceName.OrthoPlacementProcsList, string.Join(",", _listOrthoPlacementCodeNums))
			| Preferences.Set(PreferenceName.OrthoBandingCodes, PIn.String(textBandingCodes.Text))
			| Preferences.Set(PreferenceName.OrthoVisitCodes, PIn.String(textVisitCodes.Text))
			| Preferences.Set(PreferenceName.OrthoDebondCodes, PIn.String(textDebondCodes.Text))
			)
			{
				_hasChanges = true;
			}
			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			_hasChanges = false;
			DialogResult = DialogResult.Cancel;
		}

		private void FormOrthoSetup_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (_hasChanges)
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}
	}
}
