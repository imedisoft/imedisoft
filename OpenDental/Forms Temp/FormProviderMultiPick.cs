using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using CodeBase;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental {
	///<summary>Only used in one place, when searching for an appt slot.  Could be rolled into FormProviderPick if done carefully.</summary>
	public partial class FormProvidersMultiPick:ODForm {
		public List<Provider> SelectedProviders;
		private List<Provider> _listProviders;

		public FormProvidersMultiPick(List<Provider> listProviders=null) {
			InitializeComponent();
			
			_listProviders=listProviders;
		}

		private void FormProvidersMultiPick_Load(object sender,EventArgs e) {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col=new GridColumn("Abbrev",90);
			gridMain.Columns.Add(col);
			col=new GridColumn("Last Name",90);
			gridMain.Columns.Add(col);
			col=new GridColumn("First Name",90);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			if(_listProviders==null) {
				_listProviders=Providers.GetAll(true);
			}
			for(int i=0;i<_listProviders.Count;i++){
				row=new GridRow();
				row.Cells.Add(_listProviders[i].Abbr);
				row.Cells.Add(_listProviders[i].LastName);
				row.Cells.Add(_listProviders[i].FirstName);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			List<long> listSelectedProvNums=SelectedProviders.Select(x => x.Id).ToList();
			for(int i=0;i<_listProviders.Count;i++) {
				if(_listProviders[i].Id.In(listSelectedProvNums)) {
					gridMain.SetSelected(i,true);
				}
			}
		}


		private void butProvDentist_Click(object sender,EventArgs e) {
			SelectedProviders=new List<Provider>();
			for(int i=0;i<_listProviders.Count;i++) {
				if(!_listProviders[i].IsSecondary) {
					SelectedProviders.Add(_listProviders[i]);
					gridMain.SetSelected(i,true);
				}
				else {
					gridMain.SetSelected(i,false);
				}
			}
		}

		private void butProvHygenist_Click(object sender,EventArgs e) {
			SelectedProviders=new List<Provider>();
			for(int i=0;i<_listProviders.Count;i++) {
				if(_listProviders[i].IsSecondary) {
					SelectedProviders.Add(_listProviders[i]);
					gridMain.SetSelected(i,true);
				}
				else {
					gridMain.SetSelected(i,false);
				}
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			SelectedProviders=new List<Provider>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				SelectedProviders.Add(_listProviders[gridMain.SelectedIndices[i]]);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		
	}
}