using System.Drawing;

namespace OpenDental
{
    public class TableProvIdent : ContrTable
	{
		private System.ComponentModel.IContainer components = null;

		public TableProvIdent()
		{
			InitializeComponent();

			MaxRows = 20;
			MaxCols = 3;
			ShowScroll = true;
			FieldsArePresent = true;
			HeadingIsPresent = false;
			InstantClassesPar();
			SetRowHeight(0, 19, 14);
			Fields[0] = "Payor ID";
			Fields[1] = "Type";
			Fields[2] = "ID Number";
			ColWidth[0] = 90;
			ColWidth[1] = 110;
			ColWidth[2] = 100;
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
			// TableProvIdent
			// 
			this.Name = "TableProvIdent";
			this.Load += new System.EventHandler(this.TableProvIdent_Load);

		}
		#endregion

		private void TableProvIdent_Load(object sender, System.EventArgs e)
		{
			LayoutTables();
		}
	}
}
