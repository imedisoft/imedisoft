namespace Imedisoft.CEMT.Forms
{
    partial class FormCentralSecurity
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralSecurity));
            this.imageListPerm = new System.Windows.Forms.ImageList(this.components);
            this.lockGroupBox = new System.Windows.Forms.GroupBox();
            this.lockInfoLabel = new System.Windows.Forms.Label();
            this.prefInfoLabel = new System.Windows.Forms.Label();
            this.adminCheckBox = new System.Windows.Forms.CheckBox();
            this.securityLockCheckBox = new System.Windows.Forms.CheckBox();
            this.daysLabel = new System.Windows.Forms.Label();
            this.dateLabel = new System.Windows.Forms.Label();
            this.daysTextBox = new System.Windows.Forms.TextBox();
            this.dateTextBox = new System.Windows.Forms.TextBox();
            this.daysInfoLabel = new System.Windows.Forms.Label();
            this.syncCodeTextBox = new System.Windows.Forms.TextBox();
            this.syncCodeLabel = new System.Windows.Forms.Label();
            this.syncGroupBox = new System.Windows.Forms.GroupBox();
            this.pushBoth = new OpenDental.UI.Button();
            this.pushInfoLabel = new System.Windows.Forms.Label();
            this.syncInfoLabel = new System.Windows.Forms.Label();
            this.pushLocksButton = new OpenDental.UI.Button();
            this.pushUsersButton = new OpenDental.UI.Button();
            this.securityEditor = new OpenDental.UserControlSecurityUserGroup();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.lockGroupBox.SuspendLayout();
            this.syncGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListPerm
            // 
            this.imageListPerm.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListPerm.ImageStream")));
            this.imageListPerm.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListPerm.Images.SetKeyName(0, "grayBox.gif");
            this.imageListPerm.Images.SetKeyName(1, "checkBoxUnchecked.gif");
            this.imageListPerm.Images.SetKeyName(2, "checkBoxChecked.gif");
            this.imageListPerm.Images.SetKeyName(3, "checkBoxGreen.gif");
            // 
            // lockGroupBox
            // 
            this.lockGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lockGroupBox.Controls.Add(this.lockInfoLabel);
            this.lockGroupBox.Controls.Add(this.prefInfoLabel);
            this.lockGroupBox.Controls.Add(this.adminCheckBox);
            this.lockGroupBox.Controls.Add(this.securityLockCheckBox);
            this.lockGroupBox.Controls.Add(this.daysLabel);
            this.lockGroupBox.Controls.Add(this.dateLabel);
            this.lockGroupBox.Controls.Add(this.daysTextBox);
            this.lockGroupBox.Controls.Add(this.dateTextBox);
            this.lockGroupBox.Controls.Add(this.daysInfoLabel);
            this.lockGroupBox.Location = new System.Drawing.Point(13, 463);
            this.lockGroupBox.Name = "lockGroupBox";
            this.lockGroupBox.Size = new System.Drawing.Size(420, 200);
            this.lockGroupBox.TabIndex = 1;
            this.lockGroupBox.TabStop = false;
            this.lockGroupBox.Text = "Lock Date";
            // 
            // lockInfoLabel
            // 
            this.lockInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lockInfoLabel.Location = new System.Drawing.Point(6, 16);
            this.lockInfoLabel.Name = "lockInfoLabel";
            this.lockInfoLabel.Size = new System.Drawing.Size(408, 66);
            this.lockInfoLabel.TabIndex = 0;
            this.lockInfoLabel.Text = resources.GetString("lockInfoLabel.Text");
            // 
            // prefInfoLabel
            // 
            this.prefInfoLabel.Location = new System.Drawing.Point(184, 151);
            this.prefInfoLabel.Name = "prefInfoLabel";
            this.prefInfoLabel.Size = new System.Drawing.Size(180, 40);
            this.prefInfoLabel.TabIndex = 8;
            this.prefInfoLabel.Text = "(these settings are only editable from Central Manager)";
            this.prefInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // adminCheckBox
            // 
            this.adminCheckBox.AutoSize = true;
            this.adminCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.adminCheckBox.Location = new System.Drawing.Point(51, 151);
            this.adminCheckBox.Name = "adminCheckBox";
            this.adminCheckBox.Size = new System.Drawing.Size(127, 17);
            this.adminCheckBox.TabIndex = 6;
            this.adminCheckBox.Text = "Lock Includes Admins";
            this.adminCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.adminCheckBox.UseVisualStyleBackColor = true;
            // 
            // securityLockCheckBox
            // 
            this.securityLockCheckBox.AutoSize = true;
            this.securityLockCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.securityLockCheckBox.Location = new System.Drawing.Point(6, 174);
            this.securityLockCheckBox.Name = "securityLockCheckBox";
            this.securityLockCheckBox.Size = new System.Drawing.Size(172, 17);
            this.securityLockCheckBox.TabIndex = 7;
            this.securityLockCheckBox.Text = "Central Manager Security Lock";
            this.securityLockCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.securityLockCheckBox.UseVisualStyleBackColor = true;
            // 
            // daysLabel
            // 
            this.daysLabel.AutoSize = true;
            this.daysLabel.Location = new System.Drawing.Point(58, 114);
            this.daysLabel.Name = "daysLabel";
            this.daysLabel.Size = new System.Drawing.Size(31, 13);
            this.daysLabel.TabIndex = 3;
            this.daysLabel.Text = "Days";
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(59, 88);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(30, 13);
            this.dateLabel.TabIndex = 1;
            this.dateLabel.Text = "Date";
            // 
            // daysTextBox
            // 
            this.daysTextBox.Location = new System.Drawing.Point(95, 111);
            this.daysTextBox.Name = "daysTextBox";
            this.daysTextBox.Size = new System.Drawing.Size(50, 20);
            this.daysTextBox.TabIndex = 4;
            this.daysTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DaysTextBox_KeyDown);
            this.daysTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DaysTextBox_KeyPress);
            this.daysTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.DaysTextBox_Validating);
            // 
            // dateTextBox
            // 
            this.dateTextBox.Location = new System.Drawing.Point(95, 85);
            this.dateTextBox.Name = "dateTextBox";
            this.dateTextBox.Size = new System.Drawing.Size(100, 20);
            this.dateTextBox.TabIndex = 2;
            this.dateTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DateTextBox_KeyDown);
            this.dateTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.DateTextBox_Validating);
            // 
            // daysInfoLabel
            // 
            this.daysInfoLabel.AutoSize = true;
            this.daysInfoLabel.Location = new System.Drawing.Point(151, 114);
            this.daysInfoLabel.Name = "daysInfoLabel";
            this.daysInfoLabel.Size = new System.Drawing.Size(101, 13);
            this.daysInfoLabel.TabIndex = 5;
            this.daysInfoLabel.Text = "1 means only today";
            // 
            // syncCodeTextBox
            // 
            this.syncCodeTextBox.Location = new System.Drawing.Point(6, 42);
            this.syncCodeTextBox.Name = "syncCodeTextBox";
            this.syncCodeTextBox.ReadOnly = true;
            this.syncCodeTextBox.Size = new System.Drawing.Size(140, 20);
            this.syncCodeTextBox.TabIndex = 1;
            // 
            // syncCodeLabel
            // 
            this.syncCodeLabel.AutoSize = true;
            this.syncCodeLabel.Location = new System.Drawing.Point(6, 26);
            this.syncCodeLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.syncCodeLabel.Name = "syncCodeLabel";
            this.syncCodeLabel.Size = new System.Drawing.Size(58, 13);
            this.syncCodeLabel.TabIndex = 0;
            this.syncCodeLabel.Text = "Sync Code";
            // 
            // syncGroupBox
            // 
            this.syncGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.syncGroupBox.Controls.Add(this.pushBoth);
            this.syncGroupBox.Controls.Add(this.pushInfoLabel);
            this.syncGroupBox.Controls.Add(this.syncInfoLabel);
            this.syncGroupBox.Controls.Add(this.syncCodeLabel);
            this.syncGroupBox.Controls.Add(this.pushLocksButton);
            this.syncGroupBox.Controls.Add(this.syncCodeTextBox);
            this.syncGroupBox.Controls.Add(this.pushUsersButton);
            this.syncGroupBox.Location = new System.Drawing.Point(439, 463);
            this.syncGroupBox.Name = "syncGroupBox";
            this.syncGroupBox.Size = new System.Drawing.Size(240, 200);
            this.syncGroupBox.TabIndex = 2;
            this.syncGroupBox.TabStop = false;
            this.syncGroupBox.Text = "Sync Options";
            // 
            // pushBoth
            // 
            this.pushBoth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pushBoth.Location = new System.Drawing.Point(6, 169);
            this.pushBoth.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.pushBoth.Name = "pushBoth";
            this.pushBoth.Size = new System.Drawing.Size(80, 25);
            this.pushBoth.TabIndex = 5;
            this.pushBoth.Text = "Push Both";
            this.pushBoth.Click += new System.EventHandler(this.PushBothButton_Click);
            // 
            // pushInfoLabel
            // 
            this.pushInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pushInfoLabel.Location = new System.Drawing.Point(92, 114);
            this.pushInfoLabel.Name = "pushInfoLabel";
            this.pushInfoLabel.Size = new System.Drawing.Size(142, 80);
            this.pushInfoLabel.TabIndex = 6;
            this.pushInfoLabel.Text = "Window will come up to allow selecting databases to push to";
            this.pushInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // syncInfoLabel
            // 
            this.syncInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.syncInfoLabel.Location = new System.Drawing.Point(6, 65);
            this.syncInfoLabel.Name = "syncInfoLabel";
            this.syncInfoLabel.Size = new System.Drawing.Size(228, 45);
            this.syncInfoLabel.TabIndex = 2;
            this.syncInfoLabel.Text = "All databases that can sync with this one must have this code in their Misc Setup" +
    " window.";
            // 
            // pushLocksButton
            // 
            this.pushLocksButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pushLocksButton.Location = new System.Drawing.Point(6, 141);
            this.pushLocksButton.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.pushLocksButton.Name = "pushLocksButton";
            this.pushLocksButton.Size = new System.Drawing.Size(80, 25);
            this.pushLocksButton.TabIndex = 4;
            this.pushLocksButton.Text = "Push Locks";
            this.pushLocksButton.Click += new System.EventHandler(this.PushLocksButton_Click);
            // 
            // pushUsersButton
            // 
            this.pushUsersButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pushUsersButton.Location = new System.Drawing.Point(6, 113);
            this.pushUsersButton.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.pushUsersButton.Name = "pushUsersButton";
            this.pushUsersButton.Size = new System.Drawing.Size(80, 25);
            this.pushUsersButton.TabIndex = 3;
            this.pushUsersButton.Text = "Push Users";
            this.pushUsersButton.Click += new System.EventHandler(this.PushUsersButton_Click);
            // 
            // securityEditor
            // 
            this.securityEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.securityEditor.IsForCEMT = true;
            this.securityEditor.Location = new System.Drawing.Point(13, 13);
            this.securityEditor.MinimumSize = new System.Drawing.Size(914, 217);
            this.securityEditor.Name = "securityEditor";
            this.securityEditor.SelectedUser = null;
            this.securityEditor.SelectedUserGroup = null;
            this.securityEditor.Size = new System.Drawing.Size(958, 444);
            this.securityEditor.TabIndex = 0;
            this.securityEditor.AddUserClick += new OpenDental.UserControlSecurityUserGroup.SecurityTabsEventHandler(this.SecurityEditor_AddUserClick);
            this.securityEditor.EditUserClick += new OpenDental.UserControlSecurityUserGroup.SecurityTabsEventHandler(this.SecurityEditor_EditUserClick);
            this.securityEditor.AddUserGroupClick += new OpenDental.UserControlSecurityUserGroup.SecurityTabsEventHandler(this.SecurityEditor_AddUserGroupClick);
            this.securityEditor.EditUserGroupClick += new OpenDental.UserControlSecurityUserGroup.SecurityTabsEventHandler(this.SecurityEditor_EditUserGroupClick);
            this.securityEditor.ReportPermissionChecked += new OpenDental.UserControlSecurityUserGroup.SecurityTreeEventHandler(this.SecurityEditor_ReportPermissionChecked);
            this.securityEditor.GroupPermissionChecked += new OpenDental.UserControlSecurityUserGroup.SecurityTreeEventHandler(this.SecurityEditor_GroupPermissionChecked);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(891, 607);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(891, 638);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Close";
            // 
            // FormCentralSecurity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 676);
            this.Controls.Add(this.securityEditor);
            this.Controls.Add(this.syncGroupBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.lockGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1000, 714);
            this.Name = "FormCentralSecurity";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Central Manager User Setup";
            this.Load += new System.EventHandler(this.FormCentralSecurity_Load);
            this.lockGroupBox.ResumeLayout(false);
            this.lockGroupBox.PerformLayout();
            this.syncGroupBox.ResumeLayout(false);
            this.syncGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenDental.UI.Button cancelButton;
        private System.Windows.Forms.ImageList imageListPerm;
        private System.Windows.Forms.GroupBox lockGroupBox;
        private System.Windows.Forms.Label lockInfoLabel;
        private System.Windows.Forms.CheckBox adminCheckBox;
        private System.Windows.Forms.Label daysLabel;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.TextBox daysTextBox;
        private System.Windows.Forms.TextBox dateTextBox;
        private System.Windows.Forms.Label daysInfoLabel;
        private System.Windows.Forms.Label prefInfoLabel;
        private System.Windows.Forms.CheckBox securityLockCheckBox;
        private OpenDental.UI.Button acceptButton;
        private OpenDental.UI.Button pushUsersButton;
        private OpenDental.UI.Button pushLocksButton;
        private System.Windows.Forms.TextBox syncCodeTextBox;
        private System.Windows.Forms.Label syncCodeLabel;
        private System.Windows.Forms.GroupBox syncGroupBox;
        private System.Windows.Forms.Label syncInfoLabel;
        private OpenDental.UserControlSecurityUserGroup securityEditor;
        private System.Windows.Forms.Label pushInfoLabel;
        private OpenDental.UI.Button pushBoth;
    }
}
