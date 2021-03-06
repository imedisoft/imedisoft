using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using System.Linq;
using Imedisoft.Data;
using Imedisoft.Data.Models;

namespace OpenDental {
	public partial class FormSupplyOrderEdit:ODForm {
		public SupplyOrder SupplyOrderCur;
		public List<Supplier> ListSuppliersAll;

		///<Summary>This form is only used to edit existing supplyOrders, not to add new ones.</Summary>
		public FormSupplyOrderEdit() {
			InitializeComponent();
			
		}

		private void FormSupplyOrderEdit_Load(object sender,EventArgs e) {
			textSupplier.Text=Suppliers.GetName(ListSuppliersAll,SupplyOrderCur.SupplierNum);
			if(SupplyOrderCur.DatePlaced.Year>2200){
				textDatePlaced.Text=DateTime.Today.ToShortDateString();
				SupplyOrderCur.UserNum=Security.CurrentUser.Id;
			}
			else{
				textDatePlaced.Text=SupplyOrderCur.DatePlaced.ToShortDateString();
			}
			textAmountTotal.Text=SupplyOrderCur.AmountTotal.ToString("n");
			textShippingCharge.Text=SupplyOrderCur.ShippingCharge.ToString("n");
			if(SupplyOrderCur.DateReceived.Year > 1880) {
				textDateReceived.Text=SupplyOrderCur.DateReceived.ToShortDateString();
			}
			textNote.Text=SupplyOrderCur.Note;
			comboUser.Items.AddNone<User>();
			comboUser.Items.AddList(Users.GetUsers().FindAll(x => !x.IsHidden),x=>x.UserName);//the abbr parameter is usually skipped. <T> is inferred.
			comboUser.SetSelectedKey<User>(SupplyOrderCur.UserNum,x=>x.Id,x=>Users.GetUserName(x)); 
		}
				
		private void butToday_Click(object sender,EventArgs e) {
			textDateReceived.Text=DateTime.Today.ToShortDateString();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Delete entire order?")){
				return;
			}
			SupplyOrders.DeleteObject(SupplyOrderCur);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textDatePlaced.errorProvider1.GetError(textDatePlaced)!=""
				|| textAmountTotal.errorProvider1.GetError(textAmountTotal)!=""
				|| textShippingCharge.errorProvider1.GetError(textShippingCharge)!=""
				|| !textDateReceived.IsValid)
			{
				MessageBox.Show("Please fix data entry errors first.");
				return;
			}
			if(textDatePlaced.Text==""){
				SupplyOrderCur.DatePlaced=new DateTime(2500,1,1);
				SupplyOrderCur.UserNum=0;//even if they had set a user, set it back because the order hasn't been placed. 
			}
			else{
				SupplyOrderCur.DatePlaced=PIn.Date(textDatePlaced.Text);
				SupplyOrderCur.UserNum=comboUser.GetSelectedKey<User>(x=>x.Id);
			}
			SupplyOrderCur.AmountTotal=PIn.Double(textAmountTotal.Text);
			SupplyOrderCur.Note=textNote.Text;
			SupplyOrderCur.ShippingCharge=PIn.Double(textShippingCharge.Text);
			SupplyOrderCur.DateReceived=PIn.Date(textDateReceived.Text);
			SupplyOrders.Update(SupplyOrderCur);//never new
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}