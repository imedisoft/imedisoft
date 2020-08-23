namespace Imedisoft.Forms
{
	partial class FormBackup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBackup));
            this.restoreGroupBox = new System.Windows.Forms.GroupBox();
            this.createDatabaseCheckBox = new System.Windows.Forms.CheckBox();
            this.restoreTargetLabel = new System.Windows.Forms.Label();
            this.restoreTargetTextBox = new System.Windows.Forms.TextBox();
            this.backupSourceLabel = new System.Windows.Forms.Label();
            this.backupSourceButton = new OpenDental.UI.Button();
            this.backupSourceTextBox = new System.Windows.Forms.TextBox();
            this.restoreButton = new OpenDental.UI.Button();
            this.backupGroupBox = new System.Windows.Forms.GroupBox();
            this.backupDestExamplesLabel = new System.Windows.Forms.Label();
            this.backupDestLabel = new System.Windows.Forms.Label();
            this.backupButton = new OpenDental.UI.Button();
            this.backupDestTextBox = new System.Windows.Forms.TextBox();
            this.backupDestButton = new OpenDental.UI.Button();
            this.backupInfoLabel = new System.Windows.Forms.Label();
            this.saveButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.folderBrowserSupplementalCopyNetworkPath = new System.Windows.Forms.FolderBrowserDialog();
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.restoreGroupBox.SuspendLayout();
            this.backupGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // restoreGroupBox
            // 
            this.restoreGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.restoreGroupBox.Controls.Add(this.createDatabaseCheckBox);
            this.restoreGroupBox.Controls.Add(this.restoreTargetLabel);
            this.restoreGroupBox.Controls.Add(this.restoreTargetTextBox);
            this.restoreGroupBox.Controls.Add(this.backupSourceLabel);
            this.restoreGroupBox.Controls.Add(this.backupSourceButton);
            this.restoreGroupBox.Controls.Add(this.backupSourceTextBox);
            this.restoreGroupBox.Controls.Add(this.restoreButton);
            this.restoreGroupBox.Location = new System.Drawing.Point(12, 205);
            this.restoreGroupBox.Name = "restoreGroupBox";
            this.restoreGroupBox.Size = new System.Drawing.Size(440, 180);
            this.restoreGroupBox.TabIndex = 2;
            this.restoreGroupBox.TabStop = false;
            this.restoreGroupBox.Text = "Restore";
            // 
            // createDatabaseCheckBox
            // 
            this.createDatabaseCheckBox.AutoSize = true;
            this.createDatabaseCheckBox.Checked = true;
            this.createDatabaseCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.createDatabaseCheckBox.Enabled = false;
            this.createDatabaseCheckBox.Location = new System.Drawing.Point(6, 127);
            this.createDatabaseCheckBox.Name = "createDatabaseCheckBox";
            this.createDatabaseCheckBox.Size = new System.Drawing.Size(215, 17);
            this.createDatabaseCheckBox.TabIndex = 5;
            this.createDatabaseCheckBox.Text = "Create the database if it does not exist";
            this.createDatabaseCheckBox.UseVisualStyleBackColor = true;
            // 
            // restoreTargetLabel
            // 
            this.restoreTargetLabel.AutoSize = true;
            this.restoreTargetLabel.Location = new System.Drawing.Point(6, 80);
            this.restoreTargetLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 5);
            this.restoreTargetLabel.Name = "restoreTargetLabel";
            this.restoreTargetLabel.Size = new System.Drawing.Size(207, 13);
            this.restoreTargetLabel.TabIndex = 3;
            this.restoreTargetLabel.Text = "Restore backup to the following database";
            // 
            // restoreTargetTextBox
            // 
            this.restoreTargetTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.restoreTargetTextBox.Location = new System.Drawing.Point(6, 101);
            this.restoreTargetTextBox.Name = "restoreTargetTextBox";
            this.restoreTargetTextBox.ReadOnly = true;
            this.restoreTargetTextBox.Size = new System.Drawing.Size(397, 20);
            this.restoreTargetTextBox.TabIndex = 4;
            this.restoreTargetTextBox.TextChanged += new System.EventHandler(this.InputTextChanged_TextChanged);
            // 
            // backupSourceLabel
            // 
            this.backupSourceLabel.AutoSize = true;
            this.backupSourceLabel.Location = new System.Drawing.Point(6, 26);
            this.backupSourceLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 5);
            this.backupSourceLabel.Name = "backupSourceLabel";
            this.backupSourceLabel.Size = new System.Drawing.Size(60, 13);
            this.backupSourceLabel.TabIndex = 0;
            this.backupSourceLabel.Text = "Backup File";
            // 
            // backupSourceButton
            // 
            this.backupSourceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.backupSourceButton.Location = new System.Drawing.Point(409, 47);
            this.backupSourceButton.Name = "backupSourceButton";
            this.backupSourceButton.Size = new System.Drawing.Size(25, 20);
            this.backupSourceButton.TabIndex = 2;
            this.backupSourceButton.Text = "...";
            this.backupSourceButton.Click += new System.EventHandler(this.BackupSourceButton_Click);
            // 
            // backupSourceTextBox
            // 
            this.backupSourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.backupSourceTextBox.Location = new System.Drawing.Point(6, 47);
            this.backupSourceTextBox.Name = "backupSourceTextBox";
            this.backupSourceTextBox.Size = new System.Drawing.Size(397, 20);
            this.backupSourceTextBox.TabIndex = 1;
            this.backupSourceTextBox.TextChanged += new System.EventHandler(this.InputTextChanged_TextChanged);
            // 
            // restoreButton
            // 
            this.restoreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.restoreButton.Image = global::Imedisoft.Properties.Resources.IconUpload;
            this.restoreButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.restoreButton.Location = new System.Drawing.Point(354, 149);
            this.restoreButton.Name = "restoreButton";
            this.restoreButton.Size = new System.Drawing.Size(80, 25);
            this.restoreButton.TabIndex = 6;
            this.restoreButton.Text = "Restore";
            this.restoreButton.Click += new System.EventHandler(this.RestoreButton_Click);
            // 
            // backupGroupBox
            // 
            this.backupGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.backupGroupBox.Controls.Add(this.backupDestExamplesLabel);
            this.backupGroupBox.Controls.Add(this.backupDestLabel);
            this.backupGroupBox.Controls.Add(this.backupButton);
            this.backupGroupBox.Controls.Add(this.backupDestTextBox);
            this.backupGroupBox.Controls.Add(this.backupDestButton);
            this.backupGroupBox.Location = new System.Drawing.Point(12, 69);
            this.backupGroupBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.backupGroupBox.Name = "backupGroupBox";
            this.backupGroupBox.Size = new System.Drawing.Size(440, 130);
            this.backupGroupBox.TabIndex = 1;
            this.backupGroupBox.TabStop = false;
            this.backupGroupBox.Text = "Backup";
            // 
            // backupDestExamplesLabel
            // 
            this.backupDestExamplesLabel.AutoSize = true;
            this.backupDestExamplesLabel.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backupDestExamplesLabel.Location = new System.Drawing.Point(6, 70);
            this.backupDestExamplesLabel.Name = "backupDestExamplesLabel";
            this.backupDestExamplesLabel.Size = new System.Drawing.Size(190, 11);
            this.backupDestExamplesLabel.TabIndex = 3;
            this.backupDestExamplesLabel.Text = "e.g. D:\\, D:\\Backups\\ or \\\\frontdesk\\backups\\";
            // 
            // backupDestLabel
            // 
            this.backupDestLabel.AutoSize = true;
            this.backupDestLabel.Location = new System.Drawing.Point(6, 26);
            this.backupDestLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 5);
            this.backupDestLabel.Name = "backupDestLabel";
            this.backupDestLabel.Size = new System.Drawing.Size(84, 13);
            this.backupDestLabel.TabIndex = 0;
            this.backupDestLabel.Text = "Backup Location";
            // 
            // backupButton
            // 
            this.backupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.backupButton.Image = ((System.Drawing.Image)(resources.GetObject("backupButton.Image")));
            this.backupButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.backupButton.Location = new System.Drawing.Point(354, 99);
            this.backupButton.Name = "backupButton";
            this.backupButton.Size = new System.Drawing.Size(80, 25);
            this.backupButton.TabIndex = 4;
            this.backupButton.Text = "Backup";
            this.backupButton.Click += new System.EventHandler(this.BackupButton_Click);
            // 
            // backupDestTextBox
            // 
            this.backupDestTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.backupDestTextBox.Location = new System.Drawing.Point(6, 47);
            this.backupDestTextBox.Name = "backupDestTextBox";
            this.backupDestTextBox.Size = new System.Drawing.Size(397, 20);
            this.backupDestTextBox.TabIndex = 1;
            this.backupDestTextBox.TextChanged += new System.EventHandler(this.InputTextChanged_TextChanged);
            // 
            // backupDestButton
            // 
            this.backupDestButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.backupDestButton.Location = new System.Drawing.Point(409, 47);
            this.backupDestButton.Name = "backupDestButton";
            this.backupDestButton.Size = new System.Drawing.Size(25, 20);
            this.backupDestButton.TabIndex = 2;
            this.backupDestButton.Text = "...";
            this.backupDestButton.Click += new System.EventHandler(this.BackupDestButton_Click);
            // 
            // backupInfoLabel
            // 
            this.backupInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.backupInfoLabel.Location = new System.Drawing.Point(50, 9);
            this.backupInfoLabel.Name = "backupInfoLabel";
            this.backupInfoLabel.Size = new System.Drawing.Size(402, 50);
            this.backupInfoLabel.TabIndex = 0;
            this.backupInfoLabel.Text = "Backups are useless unless you regularly verify their quality by taking a backup " +
    "home and restoring it to your home computer. We suggest an encrypted USB flash d" +
    "rive for this purpose.";
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(12, 423);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(86, 26);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save Defaults";
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(366, 423);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(86, 26);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Image = global::Imedisoft.Properties.Resources.Icon32Save;
            this.iconPictureBox.Location = new System.Drawing.Point(12, 12);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(32, 32);
            this.iconPictureBox.TabIndex = 3;
            this.iconPictureBox.TabStop = false;
            // 
            // FormBackup
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(464, 461);
            this.Controls.Add(this.restoreGroupBox);
            this.Controls.Add(this.iconPictureBox);
            this.Controls.Add(this.backupGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.backupInfoLabel);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBackup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Backup";
            this.Load += new System.EventHandler(this.FormBackup_Load);
            this.restoreGroupBox.ResumeLayout(false);
            this.restoreGroupBox.PerformLayout();
            this.backupGroupBox.ResumeLayout(false);
            this.backupGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label backupInfoLabel;
		private OpenDental.UI.Button restoreButton;
		private System.Windows.Forms.GroupBox restoreGroupBox;
		private System.Windows.Forms.Label backupDestLabel;
		private System.Windows.Forms.Label backupSourceLabel;
		private OpenDental.UI.Button backupButton;
		private OpenDental.UI.Button backupDestButton;
		private OpenDental.UI.Button backupSourceButton;
		private System.Windows.Forms.Label restoreTargetLabel;
		private System.Windows.Forms.TextBox backupDestTextBox;
		private System.Windows.Forms.TextBox backupSourceTextBox;
		private System.Windows.Forms.TextBox restoreTargetTextBox;
		private OpenDental.UI.Button saveButton;
		private System.Windows.Forms.GroupBox backupGroupBox;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserSupplementalCopyNetworkPath;
        private System.Windows.Forms.Label backupDestExamplesLabel;
        private System.Windows.Forms.CheckBox createDatabaseCheckBox;
        private System.Windows.Forms.PictureBox iconPictureBox;
    }
}
