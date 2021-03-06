using System;
using System.Drawing;

namespace OpenDental.Forms
{
    public class TableCarriers : ContrTable
	{
		private System.ComponentModel.IContainer components = null;

		public TableCarriers()
		{
			InitializeComponent();
			MaxRows = 50;
			MaxCols = 8;
			ShowScroll = true;
			FieldsArePresent = true;
			HeadingIsPresent = false;
			InstantClassesPar();
			SetRowHeight(0, 49, 14);
			Fields[0] = "Carrier Name";
			Fields[1] = "Phone";
			Fields[2] = "Address";
			Fields[3] = "Address2";
			Fields[4] = "City";
			Fields[5] = "ST";
			Fields[6] = "Zip";
			Fields[7] = "ElectID";
			ColWidth[0] = 160;
			ColWidth[1] = 90;
			ColWidth[2] = 130;
			ColWidth[3] = 120;
			ColWidth[4] = 110;
			ColWidth[5] = 60;
			ColWidth[6] = 90;
			ColWidth[7] = 60;
			DefaultGridColor = Color.LightGray;
			LayoutTables();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// TableCarriers
			// 
			this.Name = "TableCarriers";
			this.Load += new System.EventHandler(this.TableCarriers_Load);

		}
		#endregion

		private void TableCarriers_Load(object sender, EventArgs e)
		{
			LayoutTables();
		}
	}
}
