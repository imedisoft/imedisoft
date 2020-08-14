using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental{
///<summary></summary>
	public class TableCommLog : OpenDental.ContrTable{
		private System.ComponentModel.IContainer components = null;

		///<summary></summary>
		public TableCommLog(){
			InitializeComponent();
			MaxRows=50;
			MaxCols=2;
			ShowScroll=true;
			FieldsArePresent=true;
			HeadingIsPresent=true;
			Heading="Communications Log - Appointment Scheduling";
			InstantClassesPar();
			SetRowHeight(0,49,14);
			Fields[0]="Date";
			Fields[1]="Note";
			//Fields[2]="-Insurance Est";
			//Fields[3]="=Amount Due";
			ColWidth[0]=70;
			ColWidth[1]=530;
			//ColWidth[2]=100;
			//ColWidth[3]=100;
			//ColAlign[1]=HorizontalAlignment.Right;
			DefaultGridColor=Color.LightGray;
			LayoutTables();
		}

		///<summary></summary>
		protected override void Dispose(bool disposing){
			if(disposing){
				if (components != null){
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Designer generated code

		private void InitializeComponent(){
			// 
			// TableAutoBilling
			// 
			this.Name = "TableAutoBilling";
			this.Load += new System.EventHandler(this.TableAutoBilling_Load);

		}
		#endregion

		
		private void TableAutoBilling_Load(object sender, System.EventArgs e) {
		  LayoutTables();
		}

	}
}

