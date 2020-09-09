using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using OpenDentBusiness.Eclaims;

namespace OpenDental {
	public partial class FormCanadaPaymentReconciliation:ODForm {

		List<Carrier> carriers=new List<Carrier>();
		private List<Provider> _listProviders;

		public FormCanadaPaymentReconciliation() {
			InitializeComponent();
			
		}

		private void FormCanadaPaymentReconciliation_Load(object sender,EventArgs e) {
			carriers=Carriers.GetWhere(x => x.CDAnetVersion!="02" &&//This transaction does not exist in version 02.
				(x.CanadianSupportedTypes & CanSupTransTypes.RequestForPaymentReconciliation_06)==CanSupTransTypes.RequestForPaymentReconciliation_06);
			foreach(Carrier carrier in carriers) {
				listCarriers.Items.Add(carrier.CarrierName);
			}
			long defaultProvNum=Prefs.GetLong(PrefName.PracticeDefaultProv);
			_listProviders=Providers.GetDeepCopy(true);
			for(int i=0;i<_listProviders.Count;i++) {
				if(_listProviders[i].IsCDAnet) {
					listBillingProvider.Items.Add(_listProviders[i].Abbr);
					listTreatingProvider.Items.Add(_listProviders[i].Abbr);
					if(_listProviders[i].Id==defaultProvNum) {
						listBillingProvider.SelectedIndex=i;
						textBillingOfficeNumber.Text=_listProviders[i].CanadianOfficeNumber;
						listTreatingProvider.SelectedIndex=i;
						textTreatingOfficeNumber.Text=_listProviders[i].CanadianOfficeNumber;
					}
				}
			}
			textDateReconciliation.Text=DateTime.Today.ToShortDateString();
		}

		private void listBillingProvider_Click(object sender,EventArgs e) {
			textBillingOfficeNumber.Text=_listProviders[listBillingProvider.SelectedIndex].CanadianOfficeNumber;
		}

		private void listTreatingProvider_Click(object sender,EventArgs e) {
			textTreatingOfficeNumber.Text=_listProviders[listTreatingProvider.SelectedIndex].CanadianOfficeNumber;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(listCarriers.SelectedIndex<0) {
				MessageBox.Show("You must first choose a carrier.");
				return;
			}
			if(listBillingProvider.SelectedIndex<0) {
				MessageBox.Show("You must first choose a billing provider.");
				return;
			}
			if(listTreatingProvider.SelectedIndex<0) {
				MessageBox.Show("You must first choose a treating provider.");
				return;
			}
			DateTime reconciliationDate;
			try {
				reconciliationDate=DateTime.Parse(textDateReconciliation.Text).Date;
			}
			catch {
				MessageBox.Show("Reconciliation date invalid.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			try {
				Carrier carrier=carriers[listCarriers.SelectedIndex];
				Clearinghouse clearinghouseHq=Canadian.GetCanadianClearinghouseHq(carrier);
				Clearinghouse clearinghouseClin=Clearinghouses.OverrideFields(clearinghouseHq,Clinics.Active.Id); 
				CanadianOutput.GetPaymentReconciliations(clearinghouseClin,carrier,_listProviders[listTreatingProvider.SelectedIndex],
					_listProviders[listBillingProvider.SelectedIndex],reconciliationDate,Clinics.Active.Id, false,FormCCDPrint.PrintCCD);
				Cursor=Cursors.Default;
				MessageBox.Show("Done.");
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show("Request failed: "+ex.Message);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}