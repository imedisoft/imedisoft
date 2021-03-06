using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using Imedisoft.Data;

namespace OpenDental {
	public partial class FormTreatmentPlanDiscount:ODForm {
		private List<Procedure> _oldListProcs;
		private List<Procedure> _listProcs;

		public FormTreatmentPlanDiscount(List<Procedure> listProcs) {
			InitializeComponent();
			
			_oldListProcs=new List<Procedure>();
			for(int i=0;i<listProcs.Count;i++) {
				_oldListProcs.Add(listProcs[i].Copy());
			}
			_listProcs=listProcs;
		}

		private void FormTreatmentPlanDiscount_Load(object sender,EventArgs e) {
			textPercentage.Text=Preferences.GetString(PreferenceName.TreatPlanDiscountPercent);
		}

		private void butOK_Click(object sender,EventArgs e) {
			int countProcsLinkedToOrthoCase=0;
			float percent=0;//Placeholder
			if(!float.TryParse(textPercentage.Text,out percent)) {
				MessageBox.Show("Percent is invalid. Please enter a valid number to continue.");
				return;
			}
			bool hasDiscount=false;
			for(int i=0;i<_listProcs.Count;i++) {
				if(_listProcs[i].Discount!=0) {
					hasDiscount=true;
					break;
				}
			}
			if(hasDiscount //A discount exists for a procedure
				&& !MsgBox.Show(MsgBoxButtons.YesNo,"One or more of the selected procedures has a discount value already set.  This will overwrite current discount values with a new value.  Continue?")) 
			{
				return;
			}
			List<long> listProcNumsLinkedToOrthoCases=OrthoProcLinks.GetManyForProcs(_listProcs.Select(x => x.ProcNum).ToList()).Select(y => y.ProcNum).ToList();
			for(int j=0;j<_listProcs.Count;j++) {
				if(listProcNumsLinkedToOrthoCases.Contains(_listProcs[j].ProcNum)) {
					countProcsLinkedToOrthoCase++;
				}
				else if(percent==0) {
					_listProcs[j].Discount=0;//Potentially clears out old discount.
				}
				else {
					_listProcs[j].Discount=_listProcs[j].ProcFee*(percent/100);
				}
				if(_listProcs[j].Discount!=_oldListProcs[j].Discount) {//Discount was changed
					string message="Discount created or changed from Treat Plan module for procedure"
						+": "+ProcedureCodes.GetById(_listProcs[j].CodeNum).Code+"  "+"Dated"
						+": "+_listProcs[j].ProcDate.ToShortDateString()+"  "+"With a Fee of"+": "+_listProcs[j].ProcFee.ToString("c")+".  "+"Attributed a"+" "+percent
					+" "+"percent discount, changing the discount value from"+" "+_oldListProcs[j].Discount.ToString("c")+" "+"to"+" "+_listProcs[j].Discount.ToString("c");
					SecurityLogs.MakeLogEntry(Permissions.TreatPlanDiscountEdit,_listProcs[j].PatNum,message);
				}
				Procedures.Update(_listProcs[j],_oldListProcs[j]);
			}
			if(countProcsLinkedToOrthoCase>0) {
				string countProcsSkipped=countProcsLinkedToOrthoCase.ToString();
				MessageBox.Show(this,"Procedures attached to ortho cases cannot have discounts. Procedures skipped:"+" "+countProcsSkipped);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}