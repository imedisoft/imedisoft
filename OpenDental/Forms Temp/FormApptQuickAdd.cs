using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental {
	public partial class FormApptQuickAdd:ODForm {
		///<summary>If form closes with OK, this contains selected code nums.  All codeNums will already have been checked for validity.</summary>
		public List<long> SelectedCodeNums;
		public Point ParentFormLocation;
		private List<Definition> _listApptProcsQuickAddDefs;

		///<summary>Security handled in calling form.</summary>
		public FormApptQuickAdd() {
			InitializeComponent();
			
		}

		private void FormApptQuickAdd_Load(object sender,EventArgs e) {
			_listApptProcsQuickAddDefs=Definitions.GetByCategory(DefinitionCategory.ApptProcsQuickAdd);
			for(int i=0;i<_listApptProcsQuickAddDefs.Count;i++) {
				listQuickAdd.Items.Add(_listApptProcsQuickAddDefs[i].Name);
			}
			this.Location=new Point(this.ParentFormLocation.X+75,this.ParentFormLocation.Y+25);
		}

		private void FormApptQuickAdd_Shown(object sender,EventArgs e) {
			
		}

		private void listQuickAdd_DoubleClick(object sender,EventArgs e) {
			if(listQuickAdd.SelectedIndices.Count==0) {
				return;
			}
			SelectedCodeNums=new List<long>();
			string[] procCodeStrArray=_listApptProcsQuickAddDefs[listQuickAdd.SelectedIndices[0]].Value.Split(',');
			long codeNum;
			for(int i=0;i<procCodeStrArray.Length;i++) {
				codeNum=ProcedureCodes.GetCodeNum(procCodeStrArray[i]);
				if(codeNum==0){
					MessageBox.Show("Definition contains invalid code: "+procCodeStrArray[i]);
					return;
				}
				SelectedCodeNums.Add(codeNum);
			}
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(listQuickAdd.SelectedIndices.Count==0) {
				MessageBox.Show("Please select items first.");
				return;
			}
			SelectedCodeNums=new List<long>();
			string[] procCodeStrArray;
			long codeNum;
			for(int s=0;s<listQuickAdd.SelectedIndices.Count;s++) {
				procCodeStrArray=_listApptProcsQuickAddDefs[listQuickAdd.SelectedIndices[s]].Value.Split(',');
				for(int i=0;i<procCodeStrArray.Length;i++) {
					codeNum=ProcedureCodes.GetCodeNum(procCodeStrArray[i]);
					if(codeNum==0) {
						MessageBox.Show("Definition contains invalid code: "+procCodeStrArray[i]);
						return;
					}
					SelectedCodeNums.Add(codeNum);
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		

		
	}
}