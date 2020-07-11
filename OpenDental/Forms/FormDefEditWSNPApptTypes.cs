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
	public partial class FormDefEditWSNPApptTypes:ODForm {
		private Def _defCur;
		///<summary>Every WSNPA reason is required to be associated to one (and only one) appointment type.
		///This is where the length and procedures of the appointment are retrieved.</summary>
		private AppointmentType _apptTypeCur;
		///<summary>The blockout types that this WSNPA reason is restricted to.  Can be empty.</summary>
		private List<Def> _listRestrictToBlockoutTypes;

		public bool IsDeleted {
			get;
			private set;
		}

		public FormDefEditWSNPApptTypes(Def defCur) {
			InitializeComponent();
			Lan.F(this);
			_defCur=defCur;
			checkHidden.Checked=_defCur.IsHidden;
			textName.Text=_defCur.ItemName;
			//Look for an associated appointment type.
			List<DefLink> listApptTypeDefLinks=DefLinks.GetDefLinksByType(DefLinkType.AppointmentType);
			DefLink defLink=listApptTypeDefLinks.FirstOrDefault(x => x.DefNum==_defCur.DefNum);
			if(defLink!=null) {
				_apptTypeCur=AppointmentTypes.GetFirstOrDefault(x => x.AppointmentTypeNum==defLink.FKey);
			}
			List<DefLink> listRestrictToDefLinks=DefLinks.GetDefLinksByType(DefLinkType.BlockoutType,_defCur.DefNum);
			_listRestrictToBlockoutTypes=Defs.GetDefs(DefCat.BlockoutTypes,listRestrictToDefLinks.Select(x => x.FKey).ToList());
			FillApptTypeValue();
			FillBlockoutTypeValues();
		}

		private void FillApptTypeValue() {
			textApptType.Clear();
			if(_apptTypeCur!=null) {
				textApptType.Text=_apptTypeCur.AppointmentTypeName;
			}
		}

		private void FillBlockoutTypeValues() {
			textRestrictToBlockouts.Clear();
			textRestrictToBlockouts.Text=string.Join(",",_listRestrictToBlockoutTypes.Select(x => x.ItemName));
		}

		private void butSelect_Click(object sender,EventArgs e) {
			FormApptTypes FormAT=new FormApptTypes();
			FormAT.IsSelectionMode=true;
			FormAT.SelectedAptType=_apptTypeCur;
			if(FormAT.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_apptTypeCur=FormAT.SelectedAptType;
			FillApptTypeValue();
		}

		private void butColor_Click(object sender,EventArgs e) {
			colorDialog1.Color=butColor.BackColor;
			colorDialog1.ShowDialog();
			butColor.BackColor=colorDialog1.Color;
		}

		private void butSelectBlockouts_Click(object sender,EventArgs e) {
			FormDefinitionPicker formDP=new FormDefinitionPicker(DefCat.BlockoutTypes,_listRestrictToBlockoutTypes);
			formDP.IsMultiSelectionMode=true;
			formDP.ShowDialog();
			if(formDP.DialogResult!=DialogResult.OK) {
				return;
			}
			_listRestrictToBlockoutTypes=formDP.ListSelectedDefs.DeepCopy<List<Def>,List<Def>>();
			FillBlockoutTypeValues();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(string.IsNullOrEmpty(textName.Text.Trim())) {
				MessageBox.Show("Reason required.");
				return;
			}
			if(_apptTypeCur==null) {
				MessageBox.Show("Appointment Type required.");
				return;
			}
			_defCur.ItemName=PIn.String(textName.Text);
			if(_defCur.IsNew) {
				Defs.Insert(_defCur);
			}
			else {
				Defs.Update(_defCur);
			}
			DefLinks.SetFKeyForDef(_defCur.DefNum,_apptTypeCur.AppointmentTypeNum,DefLinkType.AppointmentType);
			DefLinks.DeleteAllForDef(_defCur.DefNum,DefLinkType.BlockoutType);//Remove all blockouts before inserting the new set
			DefLinks.InsertDefLinksForFKeys(_defCur.DefNum,_listRestrictToBlockoutTypes.Select(x => x.DefNum).ToList(),DefLinkType.BlockoutType);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			try {
				Defs.Delete(_defCur);
				//Web Sched New Pat Appt appointment type defs can be associated to multiple types of deflinks.  Clean them up.
				DefLinks.DeleteAllForDef(_defCur.DefNum,DefLinkType.AppointmentType);
				DefLinks.DeleteAllForDef(_defCur.DefNum,DefLinkType.Operatory);
				DefLinks.DeleteAllForDef(_defCur.DefNum,DefLinkType.BlockoutType);
				IsDeleted=true;
				DialogResult=DialogResult.OK;
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}
}