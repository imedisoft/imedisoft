using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
	///<summary></summary>
	public class TableProcAdj : OpenDental.ContrTable
	{
		private System.ComponentModel.IContainer components = null;

		///<summary></summary>
		public TableProcAdj(){
			InitializeComponent();
			MaxRows=5;
			MaxCols=4;
			ShowScroll=true;
			FieldsArePresent=true;
			HeadingIsPresent=true;
			InstantClassesPar();
			SetRowHeight(0,4,14);
			Heading="Adjustments";
			Fields[0]="Entry Date";
			Fields[1]="Amount";
			Fields[2]="Type";
			Fields[3]="Note";
			ColAlign[0]=HorizontalAlignment.Center;
			ColAlign[1]=HorizontalAlignment.Right;
			ColWidth[0]=70;
			ColWidth[1]=55;
			ColWidth[2]=100;
			ColWidth[3]=250;
			DefaultGridColor=Color.LightGray;
			LayoutTables();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// TableProcAdj
			// 
			this.Name = "TableProcAdj";
			this.Load += new System.EventHandler(this.TableProcAdj_Load);

		}
		#endregion

		private void TableProcAdj_Load(object sender, System.EventArgs e) {
			LayoutTables();
		}
	}



}



