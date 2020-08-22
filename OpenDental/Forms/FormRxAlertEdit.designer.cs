namespace Imedisoft.Forms
{
	partial class FormRxAlertEdit
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
            this.nameLabel = new System.Windows.Forms.Label();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.messageLabel = new System.Windows.Forms.Label();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.drugTextBox = new System.Windows.Forms.TextBox();
            this.drugLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.highSignificanceCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(12, 11);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(302, 20);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(320, 151);
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(300, 91);
            this.messageTextBox.TabIndex = 5;
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(36, 154);
            this.messageLabel.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(278, 13);
            this.messageLabel.TabIndex = 4;
            this.messageLabel.Text = "Or this alternate custom message may be shown instead";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(506, 304);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 7;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(592, 304);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "&Cancel";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(320, 12);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ReadOnly = true;
            this.nameTextBox.Size = new System.Drawing.Size(300, 20);
            this.nameTextBox.TabIndex = 1;
            // 
            // drugTextBox
            // 
            this.drugTextBox.Location = new System.Drawing.Point(308, 20);
            this.drugTextBox.Name = "drugTextBox";
            this.drugTextBox.ReadOnly = true;
            this.drugTextBox.Size = new System.Drawing.Size(300, 20);
            this.drugTextBox.TabIndex = 1;
            // 
            // drugLabel
            // 
            this.drugLabel.AutoSize = true;
            this.drugLabel.Location = new System.Drawing.Point(209, 23);
            this.drugLabel.Name = "drugLabel";
            this.drugLabel.Size = new System.Drawing.Size(93, 13);
            this.drugLabel.TabIndex = 0;
            this.drugLabel.Text = "This Rx is entered";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(83, 121);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Then the user will see a default alert message.";
            // 
            // groupBox
            // 
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox.Controls.Add(this.drugTextBox);
            this.groupBox.Controls.Add(this.drugLabel);
            this.groupBox.Location = new System.Drawing.Point(12, 38);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(660, 60);
            this.groupBox.TabIndex = 2;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "And then,";
            // 
            // highSignificanceCheckBox
            // 
            this.highSignificanceCheckBox.AutoSize = true;
            this.highSignificanceCheckBox.Location = new System.Drawing.Point(320, 248);
            this.highSignificanceCheckBox.Name = "highSignificanceCheckBox";
            this.highSignificanceCheckBox.Size = new System.Drawing.Size(118, 17);
            this.highSignificanceCheckBox.TabIndex = 6;
            this.highSignificanceCheckBox.Text = "Is High Significance";
            this.highSignificanceCheckBox.UseVisualStyleBackColor = true;
            // 
            // FormRxAlertEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(684, 341);
            this.Controls.Add(this.highSignificanceCheckBox);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRxAlertEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rx Alert";
            this.Load += new System.EventHandler(this.FormRxAlertEdit_Load);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.TextBox messageTextBox;
		private System.Windows.Forms.Label messageLabel;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.TextBox drugTextBox;
		private System.Windows.Forms.Label drugLabel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.CheckBox highSignificanceCheckBox;
	}
}
