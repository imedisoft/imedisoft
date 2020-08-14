using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental{
///<summary></summary>
	public class TableRefSelect : OpenDental.ContrTable{
		private System.ComponentModel.IContainer components = null;

		///<summary></summary>
		public TableRefSelect(){
			InitializeComponent();
			InstantClasses();
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if (components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code

		private void InitializeComponent(){
			// 
			// TableRefSelect
			// 
			this.Name = "TableRefSelect";
			this.Load += new System.EventHandler(this.TableRefSelect_Load);

		}
		#endregion

		///<summary></summary>
		public void InstantClasses(){
			MaxRows=20;
			MaxCols=7;
			ShowScroll=true;
			FieldsArePresent=true;
			HeadingIsPresent=true;
			InstantClassesPar();
			SetRowHeight(0,19,14);
			SetGridColor(Color.LightGray);
			SetBackGColor(Color.White);
			Heading="Select Referral";

			Fields[0]="Last Name";
      Fields[1]="FirstName"; 
			Fields[2]="MI";
			Fields[3]="Title";
			Fields[4]="Specialty";
			Fields[5]="Patient";
			Fields[6]="Note";

			ColWidth[0]=140;
			ColWidth[1]=80;
			ColWidth[2]=25;
			ColWidth[3]=65;
			ColWidth[4]=75;
			ColWidth[5]=50;
			ColWidth[6]=375;

			LayoutTables();
		}

		private void TableRefSelect_Load(object sender, System.EventArgs e) {
		  LayoutTables();		
		}

	}
}

