namespace OpenDental
{
    partial class FormChooseDatabase
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
            this.connectionGroupBox = new System.Windows.Forms.GroupBox();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.databaseComboBox = new System.Windows.Forms.ComboBox();
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.serverLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.userLabel = new System.Windows.Forms.Label();
            this.databaseLabel = new System.Windows.Forms.Label();
            this.connectionLabel = new System.Windows.Forms.Label();
            this.autoConnectComboBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.connectionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectionGroupBox
            // 
            this.connectionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionGroupBox.Controls.Add(this.userTextBox);
            this.connectionGroupBox.Controls.Add(this.databaseComboBox);
            this.connectionGroupBox.Controls.Add(this.serverTextBox);
            this.connectionGroupBox.Controls.Add(this.serverLabel);
            this.connectionGroupBox.Controls.Add(this.passwordTextBox);
            this.connectionGroupBox.Controls.Add(this.passwordLabel);
            this.connectionGroupBox.Controls.Add(this.userLabel);
            this.connectionGroupBox.Controls.Add(this.databaseLabel);
            this.connectionGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.connectionGroupBox.Location = new System.Drawing.Point(12, 52);
            this.connectionGroupBox.Name = "connectionGroupBox";
            this.connectionGroupBox.Size = new System.Drawing.Size(320, 145);
            this.connectionGroupBox.TabIndex = 1;
            this.connectionGroupBox.TabStop = false;
            this.connectionGroupBox.Text = "Database Connection Settings";
            // 
            // userTextBox
            // 
            this.userTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userTextBox.Location = new System.Drawing.Point(110, 52);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(204, 20);
            this.userTextBox.TabIndex = 3;
            // 
            // databaseComboBox
            // 
            this.databaseComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.databaseComboBox.DropDownHeight = 390;
            this.databaseComboBox.IntegralHeight = false;
            this.databaseComboBox.Location = new System.Drawing.Point(110, 104);
            this.databaseComboBox.MaxDropDownItems = 100;
            this.databaseComboBox.Name = "databaseComboBox";
            this.databaseComboBox.Size = new System.Drawing.Size(204, 21);
            this.databaseComboBox.TabIndex = 7;
            this.databaseComboBox.DropDown += new System.EventHandler(this.DatabaseComboBox_DropDown);
            // 
            // serverTextBox
            // 
            this.serverTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverTextBox.Location = new System.Drawing.Point(110, 26);
            this.serverTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(204, 20);
            this.serverTextBox.TabIndex = 1;
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(35, 29);
            this.serverLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(69, 13);
            this.serverLabel.TabIndex = 0;
            this.serverLabel.Text = "Server Name";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Location = new System.Drawing.Point(110, 78);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(204, 20);
            this.passwordTextBox.TabIndex = 5;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(15, 81);
            this.passwordLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(89, 13);
            this.passwordLabel.TabIndex = 4;
            this.passwordLabel.Text = "MySQL Password";
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(13, 55);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(91, 13);
            this.userLabel.TabIndex = 2;
            this.userLabel.Text = "MySQL Username";
            // 
            // databaseLabel
            // 
            this.databaseLabel.AutoSize = true;
            this.databaseLabel.Location = new System.Drawing.Point(51, 107);
            this.databaseLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.databaseLabel.Name = "databaseLabel";
            this.databaseLabel.Size = new System.Drawing.Size(53, 13);
            this.databaseLabel.TabIndex = 6;
            this.databaseLabel.Text = "Database";
            // 
            // connectionLabel
            // 
            this.connectionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionLabel.Location = new System.Drawing.Point(12, 9);
            this.connectionLabel.Name = "connectionLabel";
            this.connectionLabel.Size = new System.Drawing.Size(320, 40);
            this.connectionLabel.TabIndex = 0;
            this.connectionLabel.Text = "These values will only be used on this computer. They have to be set on each comp" +
    "uter";
            // 
            // autoConnectComboBox
            // 
            this.autoConnectComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.autoConnectComboBox.AutoSize = true;
            this.autoConnectComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.autoConnectComboBox.Location = new System.Drawing.Point(12, 203);
            this.autoConnectComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.autoConnectComboBox.Name = "autoConnectComboBox";
            this.autoConnectComboBox.Size = new System.Drawing.Size(303, 18);
            this.autoConnectComboBox.TabIndex = 2;
            this.autoConnectComboBox.Text = "Do not show this window on startup (this computer only)";
            this.autoConnectComboBox.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(252, 244);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(166, 244);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // FormChooseDatabase
            // 
            this.AcceptButton = this.acceptButton;
            this.ClientSize = new System.Drawing.Size(344, 281);
            this.Controls.Add(this.connectionLabel);
            this.Controls.Add(this.connectionGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.autoConnectComboBox);
            this.Controls.Add(this.acceptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChooseDatabase";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Database";
            this.Load += new System.EventHandler(this.FormChooseDatabase_Load);
            this.connectionGroupBox.ResumeLayout(false);
            this.connectionGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox connectionGroupBox;
        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.ComboBox databaseComboBox;
        private System.Windows.Forms.CheckBox autoConnectComboBox;
        private System.Windows.Forms.TextBox serverTextBox;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.Label databaseLabel;
        private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.Button acceptButton;
        private System.Windows.Forms.Label connectionLabel;
    }
}
