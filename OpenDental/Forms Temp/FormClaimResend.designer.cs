namespace Imedisoft.Forms
{
	partial class FormClaimResend
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
            this.claimOriginalRadioButton = new System.Windows.Forms.RadioButton();
            this.claimReplacementRadioButton = new System.Windows.Forms.RadioButton();
            this.acceptButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(332, 84);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // claimOriginalRadioButton
            // 
            this.claimOriginalRadioButton.AutoSize = true;
            this.claimOriginalRadioButton.Checked = true;
            this.claimOriginalRadioButton.Location = new System.Drawing.Point(12, 12);
            this.claimOriginalRadioButton.Name = "claimOriginalRadioButton";
            this.claimOriginalRadioButton.Size = new System.Drawing.Size(318, 17);
            this.claimOriginalRadioButton.TabIndex = 1;
            this.claimOriginalRadioButton.TabStop = true;
            this.claimOriginalRadioButton.Text = "The claim has not been accepted yet and I need to resend it.";
            this.claimOriginalRadioButton.UseVisualStyleBackColor = true;
            // 
            // claimReplacementRadioButton
            // 
            this.claimReplacementRadioButton.AutoSize = true;
            this.claimReplacementRadioButton.Location = new System.Drawing.Point(12, 37);
            this.claimReplacementRadioButton.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.claimReplacementRadioButton.Name = "claimReplacementRadioButton";
            this.claimReplacementRadioButton.Size = new System.Drawing.Size(380, 17);
            this.claimReplacementRadioButton.TabIndex = 2;
            this.claimReplacementRadioButton.Text = "The claim was accepted and I need to replace it with updated information.";
            this.claimReplacementRadioButton.UseVisualStyleBackColor = true;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.acceptButton.Image = global::Imedisoft.Properties.Resources.IconUpload;
            this.acceptButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.acceptButton.Location = new System.Drawing.Point(246, 84);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&Send";
            // 
            // FormClaimResend
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(424, 121);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.claimReplacementRadioButton);
            this.Controls.Add(this.claimOriginalRadioButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormClaimResend";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Resend Claim";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.RadioButton claimOriginalRadioButton;
		private System.Windows.Forms.RadioButton claimReplacementRadioButton;
		private OpenDental.UI.Button acceptButton;
	}
}
