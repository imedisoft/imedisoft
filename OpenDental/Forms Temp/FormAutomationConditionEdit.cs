using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using CodeBase;
using Imedisoft.Data.Models;
using Imedisoft.Forms;

namespace OpenDental {
	public partial class FormAutomationConditionEdit:ODForm {
		public bool IsNew;
		public AutomationCondition ConditionCur;

		public FormAutomationConditionEdit() {
			InitializeComponent();
			

		}

		private void FormAutomationConditionEdit_Load(object sender,EventArgs e) {
			foreach(AutoCondField condField in Enum.GetValues(typeof(AutoCondField))) {
				listCompareField.Items.Add(condField.GetDescription());
				listCompareField.SelectedIndex=0;
			}
			for(int i=0;i<Enum.GetNames(typeof(AutoCondComparison)).Length;i++) {
				if(Enum.GetNames(typeof(AutoCondComparison))[i]=="None") {//Skip none as an option for the comparison field
					continue;
				}
				listComparison.Items.Add(Enum.GetNames(typeof(AutoCondComparison))[i]);
				listComparison.SelectedIndex=0;
			}
			if(!IsNew) {
				textCompareString.Text=ConditionCur.CompareString;
				listCompareField.SelectedIndex=(int)ConditionCur.CompareField;
				if(!ConditionCur.CompareField.In(AutoCondField.InsuranceNotEffective,AutoCondField.IsControlled,AutoCondField.IsProcRequired,
					AutoCondField.IsPatientInstructionPresent))
				{
					listComparison.SelectedIndex=(int)ConditionCur.Comparison;
				}
				ShowOrHideFields((AutoCondField)listCompareField.SelectedIndex);
			}
		}

		///<summary>Show or hides the form fileds based of needs.</summary>
		private void ShowOrHideFields(AutoCondField autoCondField) {
			if(autoCondField==AutoCondField.BillingType) {
				butBillingType.Visible=true;
				labelCompareString.Text="Text (Type or pick from list) ";
			}
			else {
				butBillingType.Visible=false;
				labelCompareString.Text="Text";
			}
			if(autoCondField.In(AutoCondField.InsuranceNotEffective,AutoCondField.IsControlled,AutoCondField.IsProcRequired
				,AutoCondField.IsPatientInstructionPresent)) 
			{//Hide fields
				labelWarning.Visible=true;
				if(autoCondField==AutoCondField.InsuranceNotEffective) {
					labelWarning.Text="At any point in time this rule is triggered, the current date and time will be compared to the "
						+"effective date range of the current patient's primary insurance.  If it does not fall within the range then the "
						+"action for this automation will be triggered.";
				}
				else {
					labelWarning.Text="This automation field will only work when using the RxCreate trigger.";
				}
				labelComparison.Visible=false;
				labelCompareString.Visible=false;
				listComparison.Visible=false;
				textCompareString.Visible=false;
			}
			else {//Show fields
				labelWarning.Visible=false;
				labelComparison.Visible=true;
				labelCompareString.Visible=true;
				listComparison.Visible=true;
				textCompareString.Visible=true;
			}
		}

		///<summary>Logic might get more complex as fields and comparisons are added so a seperate function was made.</summary>
		private bool ReasonableLogic() {
			AutoCondComparison comp=(AutoCondComparison)listComparison.SelectedIndex;
			AutoCondField cond=(AutoCondField)listCompareField.SelectedIndex;
			//So far Age is only thing that allows GreaterThan or LessThan.
			if(cond!=AutoCondField.Age) {
				if(comp==AutoCondComparison.GreaterThan || comp==AutoCondComparison.LessThan) {
					return false;
				}
			}
			else {
				int age;
				//Make sure that the user typed in an integer and not a word.
				if(!int.TryParse(textCompareString.Text,out age)) {
					return false;
				}
			}
			return true;
		}

		private void listCompareField_Click(object sender,EventArgs e) {
			ShowOrHideFields((AutoCondField)listCompareField.SelectedIndex);
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Delete this condition?")) {
				return;
			}
			try {
				AutomationConditions.Delete(ConditionCur.AutomationConditionNum);
				DialogResult=DialogResult.OK;
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void butBillingType_Click(object sender, EventArgs e)
		{
            using var formDefinitionPicker = new FormDefinitionPicker(DefinitionCategory.BillingTypes)
            {
                AllowShowHidden = false,
                AllowMultiSelect = false
            };

            if (formDefinitionPicker.ShowDialog(this) == DialogResult.OK)
			{
				textCompareString.Text = formDefinitionPicker.SelectedDefinition?.Name ?? "";
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if((AutoCondField)listCompareField.SelectedIndex==AutoCondField.InsuranceNotEffective
				|| (AutoCondField)listCompareField.SelectedIndex==AutoCondField.IsProcRequired
				|| (AutoCondField)listCompareField.SelectedIndex==AutoCondField.IsControlled
				|| (AutoCondField)listCompareField.SelectedIndex==AutoCondField.IsPatientInstructionPresent) 
			{
				ConditionCur.CompareString=""; 
				ConditionCur.Comparison=AutoCondComparison.None;
			}
			else {
				if(textCompareString.Text.Trim()=="") {
					MessageBox.Show("Text not allowed to be blank.");
					return;
				}
				if(!ReasonableLogic()) {
					MessageBox.Show("Comparison does not make sense with chosen field.");
					return;
				}
				if(((AutoCondField)listCompareField.SelectedIndex==AutoCondField.Gender
					&& !(textCompareString.Text.ToLower()=="m" || textCompareString.Text.ToLower()=="f"))) 
				{
					MessageBox.Show("Allowed gender values are M or F.");
					return;
				}
				ConditionCur.CompareString=textCompareString.Text;
				ConditionCur.Comparison=(AutoCondComparison)listComparison.SelectedIndex;
			}
			ConditionCur.CompareField=(AutoCondField)listCompareField.SelectedIndex;
			if(IsNew) {
				AutomationConditions.Insert(ConditionCur);
			}
			else {
				AutomationConditions.Update(ConditionCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}