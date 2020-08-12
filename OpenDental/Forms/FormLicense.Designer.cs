namespace Imedisoft.Forms
{
    partial class FormLicense
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
            this.licenseListBox = new System.Windows.Forms.ListBox();
            this.licenseTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(792, 544);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // licenseListBox
            // 
            this.licenseListBox.IntegralHeight = false;
            this.licenseListBox.Location = new System.Drawing.Point(12, 12);
            this.licenseListBox.Name = "licenseListBox";
            this.licenseListBox.Size = new System.Drawing.Size(180, 350);
            this.licenseListBox.TabIndex = 1;
            this.licenseListBox.SelectedIndexChanged += new System.EventHandler(this.LicenseListBox_SelectedIndexChanged);
            // 
            // licenseTextBox
            // 
            this.licenseTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.licenseTextBox.Location = new System.Drawing.Point(198, 12);
            this.licenseTextBox.Multiline = true;
            this.licenseTextBox.Name = "licenseTextBox";
            this.licenseTextBox.ReadOnly = true;
            this.licenseTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.licenseTextBox.Size = new System.Drawing.Size(674, 526);
            this.licenseTextBox.TabIndex = 2;
            // 
            // FormLicense
            // 
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(884, 581);
            this.Controls.Add(this.licenseTextBox);
            this.Controls.Add(this.licenseListBox);
            this.Controls.Add(this.closeButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(820, 480);
            this.Name = "FormLicense";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Licenses";
            this.Load += new System.EventHandler(this.FormLicense_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		private OpenDental.UI.Button closeButton;
        private System.Windows.Forms.ListBox licenseListBox;
        private System.Windows.Forms.TextBox licenseTextBox;
    }
}
