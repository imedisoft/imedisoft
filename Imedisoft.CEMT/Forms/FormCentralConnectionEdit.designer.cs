namespace Imedisoft.CEMT.Forms
{
    partial class FormCentralConnectionEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralConnectionEdit));
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.databaseServerTextBox = new System.Windows.Forms.TextBox();
            this.databaseServerLabel = new System.Windows.Forms.Label();
            this.databaseNameTextBox = new System.Windows.Forms.TextBox();
            this.databaseNameLabel = new System.Windows.Forms.Label();
            this.databaseUserTextBox = new System.Windows.Forms.TextBox();
            this.databaseUserLabel = new System.Windows.Forms.Label();
            this.databasePasswordTextBox = new System.Windows.Forms.TextBox();
            this.databasePasswordLabel = new System.Windows.Forms.Label();
            this.noteTextBox = new System.Windows.Forms.TextBox();
            this.noteLabel = new System.Windows.Forms.Label();
            this.showBreakdownCheckBox = new System.Windows.Forms.CheckBox();
            this.databasePortTextBox = new System.Windows.Forms.TextBox();
            this.databasePortLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(286, 314);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 14;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(372, 314);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 15;
            this.cancelButton.Text = "&Cancel";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(12, 314);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 13;
            this.deleteButton.Text = "Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // databaseServerTextBox
            // 
            this.databaseServerTextBox.Location = new System.Drawing.Point(140, 12);
            this.databaseServerTextBox.Name = "databaseServerTextBox";
            this.databaseServerTextBox.Size = new System.Drawing.Size(200, 23);
            this.databaseServerTextBox.TabIndex = 1;
            // 
            // databaseServerLabel
            // 
            this.databaseServerLabel.AutoSize = true;
            this.databaseServerLabel.Location = new System.Drawing.Point(81, 15);
            this.databaseServerLabel.Name = "databaseServerLabel";
            this.databaseServerLabel.Size = new System.Drawing.Size(53, 15);
            this.databaseServerLabel.TabIndex = 0;
            this.databaseServerLabel.Text = "Host / IP";
            // 
            // databaseNameTextBox
            // 
            this.databaseNameTextBox.Location = new System.Drawing.Point(140, 128);
            this.databaseNameTextBox.Name = "databaseNameTextBox";
            this.databaseNameTextBox.Size = new System.Drawing.Size(200, 23);
            this.databaseNameTextBox.TabIndex = 9;
            // 
            // databaseNameLabel
            // 
            this.databaseNameLabel.AutoSize = true;
            this.databaseNameLabel.Location = new System.Drawing.Point(79, 131);
            this.databaseNameLabel.Name = "databaseNameLabel";
            this.databaseNameLabel.Size = new System.Drawing.Size(55, 15);
            this.databaseNameLabel.TabIndex = 8;
            this.databaseNameLabel.Text = "Database";
            // 
            // databaseUserTextBox
            // 
            this.databaseUserTextBox.Location = new System.Drawing.Point(140, 70);
            this.databaseUserTextBox.Name = "databaseUserTextBox";
            this.databaseUserTextBox.Size = new System.Drawing.Size(200, 23);
            this.databaseUserTextBox.TabIndex = 5;
            // 
            // databaseUserLabel
            // 
            this.databaseUserLabel.AutoSize = true;
            this.databaseUserLabel.Location = new System.Drawing.Point(74, 73);
            this.databaseUserLabel.Name = "databaseUserLabel";
            this.databaseUserLabel.Size = new System.Drawing.Size(60, 15);
            this.databaseUserLabel.TabIndex = 4;
            this.databaseUserLabel.Text = "Username";
            // 
            // databasePasswordTextBox
            // 
            this.databasePasswordTextBox.Location = new System.Drawing.Point(140, 99);
            this.databasePasswordTextBox.Name = "databasePasswordTextBox";
            this.databasePasswordTextBox.Size = new System.Drawing.Size(200, 23);
            this.databasePasswordTextBox.TabIndex = 7;
            this.databasePasswordTextBox.UseSystemPasswordChar = true;
            // 
            // databasePasswordLabel
            // 
            this.databasePasswordLabel.AutoSize = true;
            this.databasePasswordLabel.Location = new System.Drawing.Point(77, 102);
            this.databasePasswordLabel.Name = "databasePasswordLabel";
            this.databasePasswordLabel.Size = new System.Drawing.Size(57, 15);
            this.databasePasswordLabel.TabIndex = 6;
            this.databasePasswordLabel.Text = "Password";
            // 
            // noteTextBox
            // 
            this.noteTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noteTextBox.Location = new System.Drawing.Point(140, 190);
            this.noteTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.noteTextBox.Multiline = true;
            this.noteTextBox.Name = "noteTextBox";
            this.noteTextBox.Size = new System.Drawing.Size(312, 100);
            this.noteTextBox.TabIndex = 12;
            // 
            // noteLabel
            // 
            this.noteLabel.AutoSize = true;
            this.noteLabel.Location = new System.Drawing.Point(101, 193);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.Size = new System.Drawing.Size(33, 15);
            this.noteLabel.TabIndex = 11;
            this.noteLabel.Text = "Note";
            // 
            // showBreakdownCheckBox
            // 
            this.showBreakdownCheckBox.AutoSize = true;
            this.showBreakdownCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.showBreakdownCheckBox.Location = new System.Drawing.Point(140, 157);
            this.showBreakdownCheckBox.Name = "showBreakdownCheckBox";
            this.showBreakdownCheckBox.Size = new System.Drawing.Size(211, 20);
            this.showBreakdownCheckBox.TabIndex = 10;
            this.showBreakdownCheckBox.Text = "Show clinic breakdown on reports";
            this.showBreakdownCheckBox.UseVisualStyleBackColor = true;
            // 
            // databasePortTextBox
            // 
            this.databasePortTextBox.Enabled = false;
            this.databasePortTextBox.Location = new System.Drawing.Point(140, 41);
            this.databasePortTextBox.Name = "databasePortTextBox";
            this.databasePortTextBox.Size = new System.Drawing.Size(60, 23);
            this.databasePortTextBox.TabIndex = 3;
            this.databasePortTextBox.Text = "5432";
            // 
            // databasePortLabel
            // 
            this.databasePortLabel.AutoSize = true;
            this.databasePortLabel.Enabled = false;
            this.databasePortLabel.Location = new System.Drawing.Point(105, 44);
            this.databasePortLabel.Name = "databasePortLabel";
            this.databasePortLabel.Size = new System.Drawing.Size(29, 15);
            this.databasePortLabel.TabIndex = 2;
            this.databasePortLabel.Text = "Port";
            // 
            // FormCentralConnectionEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(464, 351);
            this.Controls.Add(this.databasePortLabel);
            this.Controls.Add(this.databasePortTextBox);
            this.Controls.Add(this.databaseServerLabel);
            this.Controls.Add(this.showBreakdownCheckBox);
            this.Controls.Add(this.databasePasswordTextBox);
            this.Controls.Add(this.noteTextBox);
            this.Controls.Add(this.databaseServerTextBox);
            this.Controls.Add(this.databasePasswordLabel);
            this.Controls.Add(this.noteLabel);
            this.Controls.Add(this.databaseNameLabel);
            this.Controls.Add(this.databaseUserTextBox);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.databaseUserLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.databaseNameTextBox);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCentralConnectionEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Connection";
            this.Load += new System.EventHandler(this.FormCentralConnectionEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.Button acceptButton;
        private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.Button deleteButton;
        private System.Windows.Forms.TextBox databaseServerTextBox;
        private System.Windows.Forms.Label databaseServerLabel;
        private System.Windows.Forms.TextBox databaseNameTextBox;
        private System.Windows.Forms.Label databaseNameLabel;
        private System.Windows.Forms.TextBox databaseUserTextBox;
        private System.Windows.Forms.Label databaseUserLabel;
        private System.Windows.Forms.TextBox databasePasswordTextBox;
        private System.Windows.Forms.Label databasePasswordLabel;
        private System.Windows.Forms.TextBox noteTextBox;
        private System.Windows.Forms.Label noteLabel;
        private System.Windows.Forms.CheckBox showBreakdownCheckBox;
        private System.Windows.Forms.TextBox databasePortTextBox;
        private System.Windows.Forms.Label databasePortLabel;
    }
}