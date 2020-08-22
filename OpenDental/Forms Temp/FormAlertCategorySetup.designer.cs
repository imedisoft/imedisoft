namespace OpenDental
{
	partial class FormAlertCategorySetup
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
            this.duplicateButton = new OpenDental.UI.Button();
            this.copyButton = new OpenDental.UI.Button();
            this.internalGrid = new OpenDental.UI.ODGrid();
            this.customGrid = new OpenDental.UI.ODGrid();
            this.closeButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // duplicateButton
            // 
            this.duplicateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.duplicateButton.Image = global::Imedisoft.Properties.Resources.IconPlus;
            this.duplicateButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.duplicateButton.Location = new System.Drawing.Point(703, 564);
            this.duplicateButton.Name = "duplicateButton";
            this.duplicateButton.Size = new System.Drawing.Size(80, 25);
            this.duplicateButton.TabIndex = 3;
            this.duplicateButton.Text = "Duplicate";
            this.duplicateButton.Click += new System.EventHandler(this.DuplicateButton_Click);
            // 
            // copyButton
            // 
            this.copyButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.copyButton.Image = global::Imedisoft.Properties.Resources.IconArrowRight;
            this.copyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.copyButton.Location = new System.Drawing.Point(400, 230);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(80, 25);
            this.copyButton.TabIndex = 1;
            this.copyButton.Text = "Copy";
            this.copyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // internalGrid
            // 
            this.internalGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.internalGrid.Location = new System.Drawing.Point(12, 12);
            this.internalGrid.Name = "internalGrid";
            this.internalGrid.Size = new System.Drawing.Size(370, 546);
            this.internalGrid.TabIndex = 0;
            this.internalGrid.Title = "Internal";
            this.internalGrid.TranslationName = "TableInternal";
            this.internalGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.InternalGrid_CellDoubleClick);
            // 
            // customGrid
            // 
            this.customGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customGrid.Location = new System.Drawing.Point(499, 12);
            this.customGrid.Name = "customGrid";
            this.customGrid.Size = new System.Drawing.Size(370, 546);
            this.customGrid.TabIndex = 2;
            this.customGrid.Title = "Custom";
            this.customGrid.TranslationName = "TableCustom";
            this.customGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.CustomGrid_CellDoubleClick);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(789, 564);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "&Close";
            // 
            // FormAlertCategorySetup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(881, 601);
            this.Controls.Add(this.duplicateButton);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.internalGrid);
            this.Controls.Add(this.customGrid);
            this.Controls.Add(this.closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAlertCategorySetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Alert Category Setup";
            this.Load += new System.EventHandler(this.FormAlertCategorySetup_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button duplicateButton;
		private OpenDental.UI.Button copyButton;
		private OpenDental.UI.ODGrid internalGrid;
		private OpenDental.UI.ODGrid customGrid;
		private OpenDental.UI.Button closeButton;
	}
}