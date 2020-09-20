namespace Imedisoft.Forms
{
	partial class FormEhrAmendmentEdit
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
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.scanLabel = new System.Windows.Forms.Label();
            this.sourceComboBox = new System.Windows.Forms.ComboBox();
            this.sourceLabel = new System.Windows.Forms.Label();
            this.sourceNameTextBox = new System.Windows.Forms.TextBox();
            this.sourceNameLabel = new System.Windows.Forms.Label();
            this.dateRequestedLabel = new System.Windows.Forms.Label();
            this.dateAcceptedLabel = new System.Windows.Forms.Label();
            this.dateAppendedLabel = new System.Windows.Forms.Label();
            this.dateRequestedButton = new OpenDental.UI.Button();
            this.dateAcceptedButton = new OpenDental.UI.Button();
            this.dateAppendedButton = new OpenDental.UI.Button();
            this.scanButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.dateAcceptedTextBox = new System.Windows.Forms.TextBox();
            this.dateRequestedTextBox = new System.Windows.Forms.TextBox();
            this.dateAppendedTextBox = new System.Windows.Forms.TextBox();
            this.statusGroupBox = new System.Windows.Forms.GroupBox();
            this.statusDeniedRadioButton = new System.Windows.Forms.RadioButton();
            this.statusAcceptedRadioButton = new System.Windows.Forms.RadioButton();
            this.statusRequestedRadioButton = new System.Windows.Forms.RadioButton();
            this.statusGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(30, 206);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(104, 13);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Description/Location";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.AcceptsReturn = true;
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Location = new System.Drawing.Point(140, 203);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.descriptionTextBox.Size = new System.Drawing.Size(452, 100);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // scanLabel
            // 
            this.scanLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scanLabel.AutoSize = true;
            this.scanLabel.Location = new System.Drawing.Point(93, 351);
            this.scanLabel.Name = "scanLabel";
            this.scanLabel.Size = new System.Drawing.Size(169, 13);
            this.scanLabel.TabIndex = 3;
            this.scanLabel.Text = "An amendment has been scanned";
            this.scanLabel.Visible = false;
            // 
            // sourceComboBox
            // 
            this.sourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceComboBox.FormattingEnabled = true;
            this.sourceComboBox.Location = new System.Drawing.Point(140, 149);
            this.sourceComboBox.Name = "sourceComboBox";
            this.sourceComboBox.Size = new System.Drawing.Size(180, 21);
            this.sourceComboBox.TabIndex = 17;
            // 
            // sourceLabel
            // 
            this.sourceLabel.AutoSize = true;
            this.sourceLabel.Location = new System.Drawing.Point(94, 152);
            this.sourceLabel.Name = "sourceLabel";
            this.sourceLabel.Size = new System.Drawing.Size(40, 13);
            this.sourceLabel.TabIndex = 16;
            this.sourceLabel.Text = "Source";
            // 
            // sourceNameTextBox
            // 
            this.sourceNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceNameTextBox.Location = new System.Drawing.Point(140, 176);
            this.sourceNameTextBox.Multiline = true;
            this.sourceNameTextBox.Name = "sourceNameTextBox";
            this.sourceNameTextBox.Size = new System.Drawing.Size(452, 21);
            this.sourceNameTextBox.TabIndex = 19;
            // 
            // sourceNameLabel
            // 
            this.sourceNameLabel.AutoSize = true;
            this.sourceNameLabel.Location = new System.Drawing.Point(64, 179);
            this.sourceNameLabel.Name = "sourceNameLabel";
            this.sourceNameLabel.Size = new System.Drawing.Size(70, 13);
            this.sourceNameLabel.TabIndex = 18;
            this.sourceNameLabel.Text = "Source Name";
            // 
            // dateRequestedLabel
            // 
            this.dateRequestedLabel.AutoSize = true;
            this.dateRequestedLabel.Location = new System.Drawing.Point(49, 15);
            this.dateRequestedLabel.Name = "dateRequestedLabel";
            this.dateRequestedLabel.Size = new System.Drawing.Size(85, 13);
            this.dateRequestedLabel.TabIndex = 6;
            this.dateRequestedLabel.Text = "Date Requested";
            // 
            // dateAcceptedLabel
            // 
            this.dateAcceptedLabel.AutoSize = true;
            this.dateAcceptedLabel.Location = new System.Drawing.Point(19, 43);
            this.dateAcceptedLabel.Name = "dateAcceptedLabel";
            this.dateAcceptedLabel.Size = new System.Drawing.Size(115, 13);
            this.dateAcceptedLabel.TabIndex = 9;
            this.dateAcceptedLabel.Text = "Date Accepted/Denied";
            // 
            // dateAppendedLabel
            // 
            this.dateAppendedLabel.AutoSize = true;
            this.dateAppendedLabel.Location = new System.Drawing.Point(52, 69);
            this.dateAppendedLabel.Name = "dateAppendedLabel";
            this.dateAppendedLabel.Size = new System.Drawing.Size(82, 13);
            this.dateAppendedLabel.TabIndex = 12;
            this.dateAppendedLabel.Text = "Date Appended";
            // 
            // dateRequestedButton
            // 
            this.dateRequestedButton.Location = new System.Drawing.Point(326, 12);
            this.dateRequestedButton.Name = "dateRequestedButton";
            this.dateRequestedButton.Size = new System.Drawing.Size(60, 20);
            this.dateRequestedButton.TabIndex = 8;
            this.dateRequestedButton.Text = "Now";
            this.dateRequestedButton.Click += new System.EventHandler(this.DateRequestedButton_Click);
            // 
            // dateAcceptedButton
            // 
            this.dateAcceptedButton.Location = new System.Drawing.Point(326, 39);
            this.dateAcceptedButton.Name = "dateAcceptedButton";
            this.dateAcceptedButton.Size = new System.Drawing.Size(60, 20);
            this.dateAcceptedButton.TabIndex = 11;
            this.dateAcceptedButton.Text = "Now";
            this.dateAcceptedButton.Click += new System.EventHandler(this.DateAcceptedButton_Click);
            // 
            // dateAppendedButton
            // 
            this.dateAppendedButton.Location = new System.Drawing.Point(326, 66);
            this.dateAppendedButton.Name = "dateAppendedButton";
            this.dateAppendedButton.Size = new System.Drawing.Size(60, 20);
            this.dateAppendedButton.TabIndex = 14;
            this.dateAppendedButton.Text = "Now";
            this.dateAppendedButton.Click += new System.EventHandler(this.DateAppendedButton_Click);
            // 
            // scanButton
            // 
            this.scanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scanButton.Location = new System.Drawing.Point(12, 345);
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(75, 24);
            this.scanButton.TabIndex = 2;
            this.scanButton.Text = "Scan";
            this.scanButton.Click += new System.EventHandler(this.ScanButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(512, 344);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(426, 344);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // dateAcceptedTextBox
            // 
            this.dateAcceptedTextBox.Location = new System.Drawing.Point(140, 39);
            this.dateAcceptedTextBox.Multiline = true;
            this.dateAcceptedTextBox.Name = "dateAcceptedTextBox";
            this.dateAcceptedTextBox.Size = new System.Drawing.Size(180, 21);
            this.dateAcceptedTextBox.TabIndex = 10;
            // 
            // dateRequestedTextBox
            // 
            this.dateRequestedTextBox.Location = new System.Drawing.Point(140, 12);
            this.dateRequestedTextBox.Multiline = true;
            this.dateRequestedTextBox.Name = "dateRequestedTextBox";
            this.dateRequestedTextBox.Size = new System.Drawing.Size(180, 21);
            this.dateRequestedTextBox.TabIndex = 7;
            // 
            // dateAppendedTextBox
            // 
            this.dateAppendedTextBox.Location = new System.Drawing.Point(140, 66);
            this.dateAppendedTextBox.Multiline = true;
            this.dateAppendedTextBox.Name = "dateAppendedTextBox";
            this.dateAppendedTextBox.Size = new System.Drawing.Size(180, 21);
            this.dateAppendedTextBox.TabIndex = 13;
            // 
            // statusGroupBox
            // 
            this.statusGroupBox.Controls.Add(this.statusDeniedRadioButton);
            this.statusGroupBox.Controls.Add(this.statusAcceptedRadioButton);
            this.statusGroupBox.Controls.Add(this.statusRequestedRadioButton);
            this.statusGroupBox.Location = new System.Drawing.Point(140, 93);
            this.statusGroupBox.Name = "statusGroupBox";
            this.statusGroupBox.Size = new System.Drawing.Size(280, 50);
            this.statusGroupBox.TabIndex = 15;
            this.statusGroupBox.TabStop = false;
            this.statusGroupBox.Text = "Status";
            // 
            // statusDeniedRadioButton
            // 
            this.statusDeniedRadioButton.AutoSize = true;
            this.statusDeniedRadioButton.Location = new System.Drawing.Point(191, 21);
            this.statusDeniedRadioButton.Name = "statusDeniedRadioButton";
            this.statusDeniedRadioButton.Size = new System.Drawing.Size(58, 17);
            this.statusDeniedRadioButton.TabIndex = 2;
            this.statusDeniedRadioButton.TabStop = true;
            this.statusDeniedRadioButton.Text = "Denied";
            this.statusDeniedRadioButton.UseVisualStyleBackColor = true;
            // 
            // statusAcceptedRadioButton
            // 
            this.statusAcceptedRadioButton.AutoSize = true;
            this.statusAcceptedRadioButton.Location = new System.Drawing.Point(115, 21);
            this.statusAcceptedRadioButton.Name = "statusAcceptedRadioButton";
            this.statusAcceptedRadioButton.Size = new System.Drawing.Size(70, 17);
            this.statusAcceptedRadioButton.TabIndex = 1;
            this.statusAcceptedRadioButton.TabStop = true;
            this.statusAcceptedRadioButton.Text = "Accepted";
            this.statusAcceptedRadioButton.UseVisualStyleBackColor = true;
            // 
            // statusRequestedRadioButton
            // 
            this.statusRequestedRadioButton.AutoSize = true;
            this.statusRequestedRadioButton.Checked = true;
            this.statusRequestedRadioButton.Location = new System.Drawing.Point(32, 21);
            this.statusRequestedRadioButton.Name = "statusRequestedRadioButton";
            this.statusRequestedRadioButton.Size = new System.Drawing.Size(77, 17);
            this.statusRequestedRadioButton.TabIndex = 0;
            this.statusRequestedRadioButton.TabStop = true;
            this.statusRequestedRadioButton.Text = "Requested";
            this.statusRequestedRadioButton.UseVisualStyleBackColor = true;
            // 
            // FormEhrAmendmentEdit
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(604, 381);
            this.Controls.Add(this.statusGroupBox);
            this.Controls.Add(this.dateAppendedTextBox);
            this.Controls.Add(this.dateRequestedTextBox);
            this.Controls.Add(this.dateAcceptedTextBox);
            this.Controls.Add(this.dateRequestedButton);
            this.Controls.Add(this.dateAcceptedButton);
            this.Controls.Add(this.dateAppendedButton);
            this.Controls.Add(this.dateAppendedLabel);
            this.Controls.Add(this.dateAcceptedLabel);
            this.Controls.Add(this.dateRequestedLabel);
            this.Controls.Add(this.scanButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.sourceNameLabel);
            this.Controls.Add(this.sourceNameTextBox);
            this.Controls.Add(this.sourceComboBox);
            this.Controls.Add(this.sourceLabel);
            this.Controls.Add(this.scanLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEhrAmendmentEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Amendment";
            this.Load += new System.EventHandler(this.FormEhrAmendmentEdit_Load);
            this.statusGroupBox.ResumeLayout(false);
            this.statusGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.Label scanLabel;
		private System.Windows.Forms.ComboBox sourceComboBox;
		private System.Windows.Forms.Label sourceLabel;
		private System.Windows.Forms.TextBox sourceNameTextBox;
		private System.Windows.Forms.Label sourceNameLabel;
		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button scanButton;
		private System.Windows.Forms.Label dateRequestedLabel;
		private System.Windows.Forms.Label dateAcceptedLabel;
		private System.Windows.Forms.Label dateAppendedLabel;
		private OpenDental.UI.Button dateAppendedButton;
		private OpenDental.UI.Button dateAcceptedButton;
		private OpenDental.UI.Button dateRequestedButton;
		private System.Windows.Forms.TextBox dateAcceptedTextBox;
		private System.Windows.Forms.TextBox dateRequestedTextBox;
		private System.Windows.Forms.TextBox dateAppendedTextBox;
        private System.Windows.Forms.GroupBox statusGroupBox;
        private System.Windows.Forms.RadioButton statusRequestedRadioButton;
        private System.Windows.Forms.RadioButton statusDeniedRadioButton;
        private System.Windows.Forms.RadioButton statusAcceptedRadioButton;
    }
}
