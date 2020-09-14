using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public class TableTimeBar : ContrTable
	{
		private System.ComponentModel.IContainer components = null;

		public TableTimeBar()
		{
			InitializeComponent();

			MaxRows = 40;
			MaxCols = 1;
			ShowScroll = false;
			FieldsArePresent = false;
			HeadingIsPresent = false;
			InstantClassesPar();
			SetRowHeight(0, 39, 14);
			ColWidth[0] = 13;
			ColAlign[0] = HorizontalAlignment.Center;
			SetGridColor(Color.LightGray);
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
			this.SuspendLayout();
			// 
			// TableTimeBar
			// 
			this.Name = "TableTimeBar";
			this.Load += new System.EventHandler(this.TableTimeBar_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void TableTimeBar_Load(object sender, System.EventArgs e)
		{
			LayoutTables();
		}
	}
}
