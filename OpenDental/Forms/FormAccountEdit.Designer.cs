namespace Imedisoft.Forms
{
    partial class FormAccountEdit
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
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.typeLabel = new System.Windows.Forms.Label();
            this.bankNumberLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.bankNumberTextBox = new System.Windows.Forms.TextBox();
            this.typeListBox = new System.Windows.Forms.ListBox();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.inactiveCheckBox = new System.Windows.Forms.CheckBox();
            this.colorButton = new System.Windows.Forms.Button();
            this.colorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(134, 15);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Description";
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(163, 38);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(31, 13);
            this.typeLabel.TabIndex = 2;
            this.typeLabel.Text = "Type";
            // 
            // bankNumberLabel
            // 
            this.bankNumberLabel.AutoSize = true;
            this.bankNumberLabel.Location = new System.Drawing.Point(38, 127);
            this.bankNumberLabel.Name = "bankNumberLabel";
            this.bankNumberLabel.Size = new System.Drawing.Size(156, 13);
            this.bankNumberLabel.TabIndex = 4;
            this.bankNumberLabel.Text = "Bank Number (for deposit slips)";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Location = new System.Drawing.Point(200, 12);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(352, 20);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // bankNumberTextBox
            // 
            this.bankNumberTextBox.Location = new System.Drawing.Point(200, 124);
            this.bankNumberTextBox.Name = "bankNumberTextBox";
            this.bankNumberTextBox.Size = new System.Drawing.Size(280, 20);
            this.bankNumberTextBox.TabIndex = 5;
            // 
            // typeListBox
            // 
            this.typeListBox.FormattingEnabled = true;
            this.typeListBox.IntegralHeight = false;
            this.typeListBox.Location = new System.Drawing.Point(200, 38);
            this.typeListBox.Name = "typeListBox";
            this.typeListBox.Size = new System.Drawing.Size(150, 80);
            this.typeListBox.TabIndex = 3;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(386, 234);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 9;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(472, 234);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "&Cancel";
            // 
            // inactiveCheckBox
            // 
            this.inactiveCheckBox.AutoSize = true;
            this.inactiveCheckBox.Location = new System.Drawing.Point(200, 150);
            this.inactiveCheckBox.Name = "inactiveCheckBox";
            this.inactiveCheckBox.Size = new System.Drawing.Size(65, 17);
            this.inactiveCheckBox.TabIndex = 6;
            this.inactiveCheckBox.Text = "Inactive";
            this.inactiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // colorButton
            // 
            this.colorButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colorButton.Location = new System.Drawing.Point(200, 173);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(30, 20);
            this.colorButton.TabIndex = 8;
            this.colorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // colorLabel
            // 
            this.colorLabel.AutoSize = true;
            this.colorLabel.Location = new System.Drawing.Point(138, 177);
            this.colorLabel.Name = "colorLabel";
            this.colorLabel.Size = new System.Drawing.Size(56, 13);
            this.colorLabel.TabIndex = 7;
            this.colorLabel.Text = "Row Color";
            // 
            // FormAccountEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(564, 271);
            this.Controls.Add(this.colorLabel);
            this.Controls.Add(this.colorButton);
            this.Controls.Add(this.inactiveCheckBox);
            this.Controls.Add(this.typeListBox);
            this.Controls.Add(this.bankNumberTextBox);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.bankNumberLabel);
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAccountEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Account";
            this.Load += new System.EventHandler(this.FormAccountEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.Label typeLabel;
		private System.Windows.Forms.Label bankNumberLabel;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.TextBox bankNumberTextBox;
		private System.Windows.Forms.ListBox typeListBox;
		private System.Windows.Forms.CheckBox inactiveCheckBox;
		private System.Windows.Forms.Button colorButton;
		private System.Windows.Forms.Label colorLabel;
	}
}
