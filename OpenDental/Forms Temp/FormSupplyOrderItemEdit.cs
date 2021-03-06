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

namespace OpenDental {
	public partial class FormSupplyOrderItemEdit:ODForm {
		private Supply _supply;
		public SupplyOrderItem SupplyOrderItemCur;
		public List<Supplier> ListSuppliersAll;

		public FormSupplyOrderItemEdit() {
			InitializeComponent();
			
		}

		private void FormSupplyOrderItemEdit_Load(object sender,EventArgs e) {
			_supply=Supplies.GetSupply(SupplyOrderItemCur.SupplyNum);
			textSupplier.Text=Suppliers.GetName(ListSuppliersAll,_supply.SupplierNum);
			textCategory.Text=Definitions.GetName(DefinitionCategory.SupplyCats,_supply.Category);
			textCatalogNumber.Text=_supply.CatalogNumber;
			textDescript.Text=_supply.Descript;
			textQty.Text=SupplyOrderItemCur.Qty.ToString();
			textPrice.Text=SupplyOrderItemCur.Price.ToString("n");
			textQty.Select();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			//don't ask
			SupplyOrderItems.DeleteObject(SupplyOrderItemCur);
			DialogResult=DialogResult.OK;
		}

		private void textPrice_TextChanged(object sender,EventArgs e) {
			FillSubtotal();
		}

		private void textQty_TextChanged(object sender,EventArgs e) {
			FillSubtotal();
		}

		private void FillSubtotal() {
			ValidateChildren();//allows errorProvider1 to populate error message text.
			if(textQty.errorProvider1.GetError(textQty)!=""
				|| textPrice.errorProvider1.GetError(textPrice)!="") 
			{	
				return;
			}
			if(textQty.Text=="" || textPrice.Text==""){
				return;
			}
			int qty=PIn.Int(textQty.Text);
			double price=PIn.Double(textPrice.Text);
			double subtotal=qty*price;
			textSubtotal.Text=subtotal.ToString("n");
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textQty.errorProvider1.GetError(textQty)!=""
				|| textPrice.errorProvider1.GetError(textPrice)!="")
			{
				MessageBox.Show("Please fix data entry errors first.");
				return;
			}
			SupplyOrderItemCur.Qty=PIn.Int(textQty.Text);
			SupplyOrderItemCur.Price=PIn.Double(textPrice.Text);
			SupplyOrderItems.Update(SupplyOrderItemCur);//never new
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		
	}
}