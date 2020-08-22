namespace Imedisoft.Forms
{
    partial class FormAuditOneType
    {
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.grid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.Location = new System.Drawing.Point(9, 9);
            this.grid.Margin = new System.Windows.Forms.Padding(0);
            this.grid.Name = "grid";
            this.grid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.grid.Size = new System.Drawing.Size(886, 603);
            this.grid.TabIndex = 2;
            this.grid.Title = "Audit Trail";
            this.grid.TranslationName = "TableAudit";
            // 
            // FormAuditOneType
            // 
            this.ClientSize = new System.Drawing.Size(904, 621);
            this.Controls.Add(this.grid);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAuditOneType";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Audit Trail";
            this.Load += new System.EventHandler(this.FormAuditOneType_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private OpenDental.UI.ODGrid grid;
    }
}
