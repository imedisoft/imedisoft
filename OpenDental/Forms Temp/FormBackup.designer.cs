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
            this.backupTabControl = new System.Windows.Forms.TabControl();
            this.backupTabPage = new System.Windows.Forms.TabPage();
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
            this.archiveTabPage = new System.Windows.Forms.TabPage();
            this.archiveMakeBackupCheckBox = new System.Windows.Forms.CheckBox();
            this.archiveInfoLabel = new System.Windows.Forms.Label();
            this.archiveButton = new OpenDental.UI.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.archiveDateTime = new System.Windows.Forms.DateTimePicker();
            this.saveButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.folderBrowserSupplementalCopyNetworkPath = new System.Windows.Forms.FolderBrowserDialog();
            this.backupTabControl.SuspendLayout();
            this.backupTabPage.SuspendLayout();
            this.restoreGroupBox.SuspendLayout();
            this.backupGroupBox.SuspendLayout();
            this.archiveTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // backupTabControl
            // 
            this.backupTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.backupTabControl.Controls.Add(this.backupTabPage);
            this.backupTabControl.Controls.Add(this.archiveTabPage);
            this.backupTabControl.Location = new System.Drawing.Point(12, 12);
            this.backupTabControl.Name = "backupTabControl";
            this.backupTabControl.SelectedIndex = 0;
            this.backupTabControl.Size = new System.Drawing.Size(560, 465);
            this.backupTabControl.TabIndex = 0;
            // 
            // backupTabPage
            // 
            this.backupTabPage.BackColor = System.Drawing.Color.Transparent;
            this.backupTabPage.Controls.Add(this.restoreGroupBox);
            this.backupTabPage.Controls.Add(this.backupGroupBox);
            this.backupTabPage.Controls.Add(this.backupInfoLabel);
            this.backupTabPage.Location = new System.Drawing.Point(4, 22);
            this.backupTabPage.Name = "backupTabPage";
            this.backupTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.backupTabPage.Size = new System.Drawing.Size(552, 439);
            this.backupTabPage.TabIndex = 0;
            this.backupTabPage.Text = "Backup";
            this.backupTabPage.UseVisualStyleBackColor = true;
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
            this.restoreGroupBox.Location = new System.Drawing.Point(6, 229);
            this.restoreGroupBox.Name = "restoreGroupBox";
            this.restoreGroupBox.Size = new System.Drawing.Size(540, 180);
            this.restoreGroupBox.TabIndex = 3;
            this.restoreGroupBox.TabStop = false;
            this.restoreGroupBox.Text = "Restore";
            // 
            // createDatabaseCheckBox
            // 
            this.createDatabaseCheckBox.AutoSize = true;
            this.createDatabaseCheckBox.Checked = true;
            this.createDatabaseCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.createDatabaseCheckBox.Location = new System.Drawing.Point(6, 127);
            this.createDatabaseCheckBox.Name = "createDatabaseCheckBox";
            this.createDatabaseCheckBox.Size = new System.Drawing.Size(215, 17);
            this.createDatabaseCheckBox.TabIndex = 6;
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
            this.restoreTargetTextBox.Size = new System.Drawing.Size(497, 20);
            this.restoreTargetTextBox.TabIndex = 4;
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
            this.backupSourceButton.Location = new System.Drawing.Point(509, 47);
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
            this.backupSourceTextBox.Size = new System.Drawing.Size(497, 20);
            this.backupSourceTextBox.TabIndex = 1;
            // 
            // restoreButton
            // 
            this.restoreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.restoreButton.Location = new System.Drawing.Point(454, 149);
            this.restoreButton.Name = "restoreButton";
            this.restoreButton.Size = new System.Drawing.Size(80, 25);
            this.restoreButton.TabIndex = 7;
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
            this.backupGroupBox.Location = new System.Drawing.Point(6, 93);
            this.backupGroupBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.backupGroupBox.Name = "backupGroupBox";
            this.backupGroupBox.Size = new System.Drawing.Size(540, 130);
            this.backupGroupBox.TabIndex = 2;
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
            this.backupButton.Location = new System.Drawing.Point(454, 99);
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
            this.backupDestTextBox.Size = new System.Drawing.Size(497, 20);
            this.backupDestTextBox.TabIndex = 1;
            // 
            // backupDestButton
            // 
            this.backupDestButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.backupDestButton.Location = new System.Drawing.Point(509, 47);
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
            this.backupInfoLabel.Location = new System.Drawing.Point(6, 3);
            this.backupInfoLabel.Name = "backupInfoLabel";
            this.backupInfoLabel.Size = new System.Drawing.Size(540, 80);
            this.backupInfoLabel.TabIndex = 0;
            this.backupInfoLabel.Text = "BACKUPS ARE USELESS UNLESS YOU REGULARLY VERIFY THEIR QUALITY BY TAKING A BACKUP " +
    "HOME AND RESTORING IT TO YOUR HOME COMPUTER.  \r\n\r\nWe suggest an encrypted USB fl" +
    "ash drive for this purpose.";
            // 
            // archiveTabPage
            // 
            this.archiveTabPage.BackColor = System.Drawing.Color.Transparent;
            this.archiveTabPage.Controls.Add(this.archiveMakeBackupCheckBox);
            this.archiveTabPage.Controls.Add(this.archiveInfoLabel);
            this.archiveTabPage.Controls.Add(this.archiveButton);
            this.archiveTabPage.Controls.Add(this.label2);
            this.archiveTabPage.Controls.Add(this.archiveDateTime);
            this.archiveTabPage.Location = new System.Drawing.Point(4, 22);
            this.archiveTabPage.Name = "archiveTabPage";
            this.archiveTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.archiveTabPage.Size = new System.Drawing.Size(552, 439);
            this.archiveTabPage.TabIndex = 1;
            this.archiveTabPage.Text = "Remove Old Data";
            this.archiveTabPage.UseVisualStyleBackColor = true;
            // 
            // archiveMakeBackupCheckBox
            // 
            this.archiveMakeBackupCheckBox.AutoSize = true;
            this.archiveMakeBackupCheckBox.Location = new System.Drawing.Point(163, 231);
            this.archiveMakeBackupCheckBox.Name = "archiveMakeBackupCheckBox";
            this.archiveMakeBackupCheckBox.Size = new System.Drawing.Size(167, 17);
            this.archiveMakeBackupCheckBox.TabIndex = 14;
            this.archiveMakeBackupCheckBox.Text = "Backup before removing data";
            this.archiveMakeBackupCheckBox.UseVisualStyleBackColor = true;
            // 
            // archiveInfoLabel
            // 
            this.archiveInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.archiveInfoLabel.Location = new System.Drawing.Point(3, 3);
            this.archiveInfoLabel.Name = "archiveInfoLabel";
            this.archiveInfoLabel.Size = new System.Drawing.Size(543, 80);
            this.archiveInfoLabel.TabIndex = 12;
            this.archiveInfoLabel.Text = resources.GetString("archiveInfoLabel.Text");
            // 
            // archiveButton
            // 
            this.archiveButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.archiveButton.Location = new System.Drawing.Point(159, 254);
            this.archiveButton.Name = "archiveButton";
            this.archiveButton.Size = new System.Drawing.Size(100, 25);
            this.archiveButton.TabIndex = 2;
            this.archiveButton.Text = "Remove Old Data";
            this.archiveButton.UseVisualStyleBackColor = true;
            this.archiveButton.Click += new System.EventHandler(this.ArchiveButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(160, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(191, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Remove old data entries on or before:";
            // 
            // archiveDateTime
            // 
            this.archiveDateTime.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.archiveDateTime.Location = new System.Drawing.Point(159, 205);
            this.archiveDateTime.Name = "archiveDateTime";
            this.archiveDateTime.Size = new System.Drawing.Size(237, 20);
            this.archiveDateTime.TabIndex = 1;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Location = new System.Drawing.Point(12, 483);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(86, 26);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save Defaults";
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(486, 483);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(86, 26);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormBackup
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(584, 521);
            this.Controls.Add(this.backupTabControl);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBackup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Backup";
            this.Load += new System.EventHandler(this.FormBackup_Load);
            this.backupTabControl.ResumeLayout(false);
            this.backupTabPage.ResumeLayout(false);
            this.restoreGroupBox.ResumeLayout(false);
            this.restoreGroupBox.PerformLayout();
            this.backupGroupBox.ResumeLayout(false);
            this.backupGroupBox.PerformLayout();
            this.archiveTabPage.ResumeLayout(false);
            this.archiveTabPage.PerformLayout();
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
		private System.Windows.Forms.TabControl backupTabControl;
		private System.Windows.Forms.TabPage archiveTabPage;
		private System.Windows.Forms.TabPage backupTabPage;
		private System.Windows.Forms.DateTimePicker archiveDateTime;
		private System.Windows.Forms.Label label2;
		private OpenDental.UI.Button archiveButton;
		private System.Windows.Forms.Label archiveInfoLabel;
		private System.Windows.Forms.CheckBox archiveMakeBackupCheckBox;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserSupplementalCopyNetworkPath;
        private System.Windows.Forms.Label backupDestExamplesLabel;
        private System.Windows.Forms.CheckBox createDatabaseCheckBox;
    }
}
