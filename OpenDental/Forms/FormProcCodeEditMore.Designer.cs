namespace Imedisoft.Forms
{
	partial class FormProcCodeEditMore
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.closeButton = new OpenDental.UI.Button();
            this.feesGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(372, 484);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // feesGrid
            // 
            this.feesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.feesGrid.Location = new System.Drawing.Point(12, 12);
            this.feesGrid.Name = "feesGrid";
            this.feesGrid.Size = new System.Drawing.Size(440, 466);
            this.feesGrid.TabIndex = 1;
            this.feesGrid.Title = "Fees";
            this.feesGrid.TranslationName = "TableFees";
            this.feesGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.FeesGrid_CellDoubleClick);
            // 
            // FormProcCodeEditMore
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(464, 521);
            this.Controls.Add(this.feesGrid);
            this.Controls.Add(this.closeButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProcCodeEditMore";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "More Fees";
            this.Load += new System.EventHandler(this.FormProcCodeEditMore_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button closeButton;
		private OpenDental.UI.ODGrid feesGrid;
	}
}