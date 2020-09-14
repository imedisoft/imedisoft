using System.Drawing;

namespace OpenDental
{
    public class TableAutoItem : ContrTable
	{
		private System.ComponentModel.IContainer components = null;

		public TableAutoItem()
		{
			InitializeComponent();
			MaxRows = 20;
			MaxCols = 3;
			ShowScroll = true;
			FieldsArePresent = true;
			HeadingIsPresent = false;
			InstantClassesPar();
			SetRowHeight(0, 19, 14);
			Fields[0] = "Code";
			Fields[1] = "Description";
			Fields[2] = "Conditions";
			ColWidth[0] = 100;
			ColWidth[1] = 200;
			ColWidth[2] = 400;
			DefaultGridColor = Color.LightGray;
			LayoutTables();
		}

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

		private void InitializeComponent()
		{
			// 
			// TableAutoItem
			// 
			this.Name = "TableAutoItem";
			this.Load += new System.EventHandler(this.TableAutoItem_Load);

		}
		#endregion

		private void TableAutoItem_Load(object sender, System.EventArgs e)
		{
			LayoutTables();
		}
	}
}
