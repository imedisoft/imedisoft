using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormMountItemDefEdit:ODForm {
		public MountItemDef MountItemDefCur;

		public FormMountItemDefEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormMountItemDefEdit_Load(object sender, EventArgs e){
			textXpos.Text=MountItemDefCur.Xpos.ToString();
			textYpos.Text=MountItemDefCur.Ypos.ToString();
			textWidth.Text=MountItemDefCur.Width.ToString();
			textHeight.Text=MountItemDefCur.Height.ToString();
		}

		private void butDelete_Click(object sender, EventArgs e){
			if(MountItemDefCur.IsNew){//although not currenly used
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete?")){
				return;
			}
			MountItemDefs.Delete(MountItemDefCur.MountItemDefNum);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textXpos.errorProvider1.GetError(textXpos)!=""
				|| textYpos.errorProvider1.GetError(textYpos)!=""
				|| textWidth.errorProvider1.GetError(textWidth)!=""
				|| textHeight.errorProvider1.GetError(textHeight)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			MountItemDefCur.Xpos=PIn.Int(textXpos.Text);
			MountItemDefCur.Ypos=PIn.Int(textYpos.Text);
			MountItemDefCur.Width=PIn.Int(textWidth.Text);
			MountItemDefCur.Height=PIn.Int(textHeight.Text);
			if(MountItemDefCur.IsNew){//but not currenly used
				MountItemDefs.Insert(MountItemDefCur);
			}
			else{
				MountItemDefs.Update(MountItemDefCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		
	}
}