namespace OpenDental{
	partial class FormUserSetting {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.supressMessageCheckBox = new System.Windows.Forms.CheckBox();
            this.autoLogoffTextBox = new System.Windows.Forms.TextBox();
            this.autoLogoffLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(186, 204);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(272, 204);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "&Cancel";
            // 
            // supressMessageCheckBox
            // 
            this.supressMessageCheckBox.AutoSize = true;
            this.supressMessageCheckBox.Location = new System.Drawing.Point(50, 74);
            this.supressMessageCheckBox.Margin = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this.supressMessageCheckBox.Name = "supressMessageCheckBox";
            this.supressMessageCheckBox.Size = new System.Drawing.Size(203, 17);
            this.supressMessageCheckBox.TabIndex = 4;
            this.supressMessageCheckBox.Text = "Close/Log off message is suppressed";
            // 
            // autoLogoffTextBox
            // 
            this.autoLogoffTextBox.Location = new System.Drawing.Point(12, 12);
            this.autoLogoffTextBox.Name = "autoLogoffTextBox";
            this.autoLogoffTextBox.ReadOnly = true;
            this.autoLogoffTextBox.Size = new System.Drawing.Size(29, 20);
            this.autoLogoffTextBox.TabIndex = 2;
            this.autoLogoffTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // autoLogoffLabel
            // 
            this.autoLogoffLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoLogoffLabel.Location = new System.Drawing.Point(47, 12);
            this.autoLogoffLabel.Name = "autoLogoffLabel";
            this.autoLogoffLabel.Size = new System.Drawing.Size(305, 32);
            this.autoLogoffLabel.TabIndex = 3;
            this.autoLogoffLabel.Text = "Automatic logoff time in minutes\r\n(Note: Edit in User Edit window inside Security" +
    " Settings)";
            // 
            // FormUserSetting
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(364, 241);
            this.Controls.Add(this.autoLogoffTextBox);
            this.Controls.Add(this.autoLogoffLabel);
            this.Controls.Add(this.supressMessageCheckBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUserSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Settings";
            this.Load += new System.EventHandler(this.FormUserSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.CheckBox supressMessageCheckBox;
		private System.Windows.Forms.TextBox autoLogoffTextBox;
		private System.Windows.Forms.Label autoLogoffLabel;
	}
}