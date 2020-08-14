using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormMountDefGenerate:ODForm {
		public MountDef MountDefCur;

		public FormMountDefGenerate() {
			InitializeComponent();
			
		}

		private void FormMountDefGenerate_Load(object sender, EventArgs e){
			listType.SelectedIndex=0;
			textWidth.Text="1700";
			textHeight.Text="1300";
			//FMX width=1700x4 + 1300x3 = 10,700
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textWidth.errorProvider1.GetError(textWidth)!=""
				|| textHeight.errorProvider1.GetError(textHeight)!="")
			{
				MessageBox.Show("Please fix data entry errors first.");
				return;
			}
			int w=PIn.Int(textWidth.Text);
			int h=PIn.Int(textHeight.Text);
			if(h>w){
				if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Width should normally be greater than height.  Continue anyway?")){
					return;
				}
			}
			MountItemDefs.DeleteForMount(MountDefCur.MountDefNum);
			if(listType.SelectedIndex==0){//FMX
				if(MountDefCur.IsNew){
					MountDefCur.Description="FMX";
				}
				MountDefCur.Height=3*h;
				MountDefCur.Width=w*4+h*3;
				MountDefCur.ColorBack=Color.Black;
				//BWs first:
				AddItem(1,0,h,w,h);
				AddItem(2,w,h,w,h);
				AddItem(3,3*w+3*h,h,w,h);
				AddItem(4,2*w+3*h,h,w,h);
				//PAs
				AddItem(5,0,0,w,h);//UR
				AddItem(6,w,0,w,h);
				AddItem(7,3*w+3*h,2*h,w,h);//LL
				AddItem(8,2*w+3*h,2*h,w,h);
				AddItem(9,0,2*h,w,h);//LR
				AddItem(10,w,2*h,w,h);
				AddItem(11,3*w+3*h,0,w,h);//UL
				AddItem(12,2*w+3*h,0,w,h);
				//Anterior
				AddItem(13,2*w,0,h,w);//max
				AddItem(14,2*w+h,0,h,w);
				AddItem(15,2*w+2*h,0,h,w);
				AddItem(16,2*w,3*h-w,h,w);//mand
				AddItem(17,2*w+h,3*h-w,h,w);
				AddItem(18,2*w+2*h,3*h-w,h,w);
			}
			if(listType.SelectedIndex==1){//4BW
				if(MountDefCur.IsNew){
					MountDefCur.Description="4BW";
				}
				MountDefCur.Height=h;
				MountDefCur.Width=w*4;
				MountDefCur.ColorBack=Color.Black;
				AddItem(1,0,0,w,h);
				AddItem(2,w,0,w,h);
				AddItem(3,3*w,0,w,h);
				AddItem(4,2*w,0,w,h);
			}
			if(listType.SelectedIndex==2){//Photos 3x2
				if(MountDefCur.IsNew){
					MountDefCur.Description="Photos";
				}
				MountDefCur.Height=h*2;
				MountDefCur.Width=w*3;
				MountDefCur.ColorBack=Color.White;
				AddItem(1,0,0,w,h);
				AddItem(2,w,0,w,h);
				AddItem(3,2*w,0,w,h);
				AddItem(4,0,h,w,h);
				AddItem(5,w,h,w,h);
				AddItem(6,2*w,h,w,h);
			}
			DialogResult=DialogResult.OK;
		}

		private void AddItem(int itemOrder,int x,int y,int w,int h){
			MountItemDef mountItemDef=new MountItemDef();
			mountItemDef.MountDefNum=MountDefCur.MountDefNum;
			mountItemDef.ItemOrder=itemOrder;
			mountItemDef.Xpos=x;
			mountItemDef.Ypos=y;
			mountItemDef.Width=w;
			mountItemDef.Height=h;
			MountItemDefs.Insert(mountItemDef);
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		
	}
}