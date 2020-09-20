namespace Imedisoft.Forms
{
	partial class FormEhrHash
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
            this.hashTextBox = new System.Windows.Forms.TextBox();
            this.generateButton = new OpenDental.UI.Button();
            this.messageLabel = new System.Windows.Forms.Label();
            this.generateLabel = new System.Windows.Forms.Label();
            this.hashLabel = new System.Windows.Forms.Label();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.transmitButton = new OpenDental.UI.Button();
            this.closeButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // hashTextBox
            // 
            this.hashTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hashTextBox.Location = new System.Drawing.Point(190, 211);
            this.hashTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.hashTextBox.Name = "hashTextBox";
            this.hashTextBox.Size = new System.Drawing.Size(382, 20);
            this.hashTextBox.TabIndex = 5;
            // 
            // generateButton
            // 
            this.generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.generateButton.Location = new System.Drawing.Point(12, 211);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(120, 25);
            this.generateButton.TabIndex = 2;
            this.generateButton.Text = "Generate Hash";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(12, 9);
            this.messageLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(89, 13);
            this.messageLabel.TabIndex = 0;
            this.messageLabel.Text = "Enter data below";
            // 
            // generateLabel
            // 
            this.generateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.generateLabel.Location = new System.Drawing.Point(12, 239);
            this.generateLabel.Name = "generateLabel";
            this.generateLabel.Size = new System.Drawing.Size(120, 15);
            this.generateLabel.TabIndex = 3;
            this.generateLabel.Text = "( uses SHA-1 )";
            this.generateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hashLabel
            // 
            this.hashLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hashLabel.AutoSize = true;
            this.hashLabel.Location = new System.Drawing.Point(155, 214);
            this.hashLabel.Name = "hashLabel";
            this.hashLabel.Size = new System.Drawing.Size(31, 13);
            this.hashLabel.TabIndex = 4;
            this.hashLabel.Text = "Hash";
            // 
            // messageTextBox
            // 
            this.messageTextBox.AcceptsReturn = true;
            this.messageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageTextBox.Location = new System.Drawing.Point(12, 30);
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(560, 175);
            this.messageTextBox.TabIndex = 1;
            // 
            // transmitButton
            // 
            this.transmitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.transmitButton.Location = new System.Drawing.Point(406, 244);
            this.transmitButton.Name = "transmitButton";
            this.transmitButton.Size = new System.Drawing.Size(80, 25);
            this.transmitButton.TabIndex = 6;
            this.transmitButton.Text = "Transmit";
            this.transmitButton.UseVisualStyleBackColor = true;
            this.transmitButton.Click += new System.EventHandler(this.TransmitButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(492, 244);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 7;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // FormEhrHash
            // 
            this.AcceptButton = this.generateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(584, 281);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.transmitButton);
            this.Controls.Add(this.hashLabel);
            this.Controls.Add(this.generateLabel);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.hashTextBox);
            this.Controls.Add(this.messageTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEhrHash";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hash";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox hashTextBox;
		private OpenDental.UI.Button generateButton;
		private System.Windows.Forms.Label messageLabel;
		private System.Windows.Forms.Label generateLabel;
		private System.Windows.Forms.Label hashLabel;
		private System.Windows.Forms.TextBox messageTextBox;
		private OpenDental.UI.Button transmitButton;
		private OpenDental.UI.Button closeButton;
	}
}
