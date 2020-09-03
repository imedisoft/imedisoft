namespace Imedisoft.Forms
{
    partial class FormZipCodes
    {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
            this.addButton = new OpenDental.UI.Button();
            this.closeButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.zipCodesGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(462, 12);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 2;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(380, 563);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(76, 26);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(462, 43);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 3;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // zipCodesGrid
            // 
            this.zipCodesGrid.AllowSortingByColumn = true;
            this.zipCodesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zipCodesGrid.Location = new System.Drawing.Point(12, 12);
            this.zipCodesGrid.Name = "zipCodesGrid";
            this.zipCodesGrid.Size = new System.Drawing.Size(444, 545);
            this.zipCodesGrid.TabIndex = 1;
            this.zipCodesGrid.Title = "Zip Codes";
            this.zipCodesGrid.TranslationName = "TableZipCodes";
            this.zipCodesGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.ZipCodesGrid_CellDoubleClick);
            this.zipCodesGrid.SelectionCommitted += new System.EventHandler(this.ZipCodesGrid_SelectionCommitted);
            // 
            // FormZipCodes
            // 
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(504, 601);
            this.Controls.Add(this.zipCodesGrid);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormZipCodes";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Zip Codes";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormZipCodes_Closing);
            this.Load += new System.EventHandler(this.FormZipCodes_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private OpenDental.UI.Button addButton;
        private OpenDental.UI.Button deleteButton;
        private OpenDental.UI.Button closeButton;
        private OpenDental.UI.ODGrid zipCodesGrid;
    }
}
