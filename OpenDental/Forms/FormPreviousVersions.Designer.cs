namespace Imedisoft.Forms
{
	partial class FormPreviousVersions
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
            this.cancelButton = new OpenDental.UI.Button();
            this.updateHistoriesGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(232, 444);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Close";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // updateHistoriesGrid
            // 
            this.updateHistoriesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.updateHistoriesGrid.Location = new System.Drawing.Point(12, 12);
            this.updateHistoriesGrid.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.updateHistoriesGrid.Name = "updateHistoriesGrid";
            this.updateHistoriesGrid.Size = new System.Drawing.Size(300, 419);
            this.updateHistoriesGrid.TabIndex = 4;
            this.updateHistoriesGrid.Title = null;
            this.updateHistoriesGrid.TranslationName = "TablePreviousVersions";
            // 
            // FormPreviousVersions
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(324, 481);
            this.Controls.Add(this.updateHistoriesGrid);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(100, 100);
            this.Name = "FormPreviousVersions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Version History";
            this.Load += new System.EventHandler(this.FormPreviousVersions_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.ODGrid updateHistoriesGrid;
	}
}
