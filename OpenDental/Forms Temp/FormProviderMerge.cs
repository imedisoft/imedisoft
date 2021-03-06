using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using Imedisoft.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormProviderMerge:ODForm {
		private List<Provider> _listActiveProvs=new List<Provider>();

		public FormProviderMerge() {
			InitializeComponent();
			
			_listActiveProvs=Providers.FindAll(x => x.Status != ProviderStatus.Deleted,true);
		}

		private void butChangeProvInto_Click(object sender,EventArgs e) {
			FormProviderPick FormPP=new FormProviderPick(_listActiveProvs);
			FormPP.ShowDialog();
			if(FormPP.DialogResult==DialogResult.OK) {
				Provider selectedProv=Providers.GetById(FormPP.SelectedProviderId);
				textAbbrInto.Text=selectedProv.Abbr;
				textProvNumInto.Text=POut.Long(selectedProv.Id);
				textNpiInto.Text=selectedProv.NationalProviderID;
				textFullNameInto.Text=selectedProv.FirstName+" "+selectedProv.LastName;
				CheckUIState();
			}
		}

		private void butChangeProvFrom_Click(object sender,EventArgs e) {
			FormProviderPick FormPP=new FormProviderPick(checkDeletedProvs.Checked ? Providers.GetAll() : _listActiveProvs);
			FormPP.ShowDialog();
			if(FormPP.DialogResult==DialogResult.OK) {
				Provider selectedProv=Providers.GetById(FormPP.SelectedProviderId);
				textAbbrFrom.Text=selectedProv.Abbr;
				textProvNumFrom.Text=POut.Long(selectedProv.Id);
				textNpiFrom.Text=selectedProv.NationalProviderID;
				textFullNameFrom.Text=selectedProv.FirstName+" "+selectedProv.LastName;
				CheckUIState();
			}
		}

		private void CheckUIState() {
			butMerge.Enabled=(textProvNumInto.Text!="" && textProvNumFrom.Text!="");
		}

		private void butMerge_Click(object sender,EventArgs e) {
			string differentFields="";
			if(textProvNumFrom.Text==textProvNumInto.Text) { 
				//do not attempt a merge if the same provider was selected twice, or if one of the fields is blank.
				MessageBox.Show("You must select two different providers to merge.");
				return;
			}
			if(textNpiFrom.Text!=textNpiInto.Text) {
				differentFields+="\r\nNPI";
			}
			if(textFullNameFrom.Text!=textFullNameInto.Text) {
				differentFields+="\r\nFull Name";
			}
			long numPats=Providers.CountPats(PIn.Long(textProvNumFrom.Text));
			long numClaims=Providers.CountClaims(PIn.Long(textProvNumFrom.Text));
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Are you sure?  The results are permanent and cannot be undone.")) {
				return;
			}
			string msgText="";
			if(differentFields!="") {
				msgText="The following provider fields do not match"+": "+differentFields+"\r\n";
			}
			msgText+="This change is irreversible"+".  "+"This provider is the primary or secondary provider for"+" "+numPats+" "+"active patients"
				+", "+"and the billing or treating provider for"+" "+numClaims+" "+"claims"+".  "
				+"Continue anyways?";
			if(MessageBox.Show(msgText,"",MessageBoxButtons.OKCancel)!=DialogResult.OK)	{
				return;
			}
			long rowsChanged=Providers.Merge(PIn.Long(textProvNumFrom.Text),PIn.Long(textProvNumInto.Text));
			string logText="Providers merged"+": "+textAbbrFrom.Text+" "+"merged into"+" "+textAbbrInto.Text+".\r\n"
			+"Rows changed"+": "+POut.Long(rowsChanged);
			SecurityLogs.MakeLogEntry(Permissions.ProviderMerge,0,logText);
			textAbbrFrom.Clear();
			textProvNumFrom.Clear();
			textNpiFrom.Clear();
			textFullNameFrom.Clear();
			CheckUIState();
			MessageBox.Show("Done.");
			DataValid.SetInvalid(InvalidType.Providers);
			_listActiveProvs=Providers.FindAll(x => x.Status != ProviderStatus.Deleted,true);
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}
}