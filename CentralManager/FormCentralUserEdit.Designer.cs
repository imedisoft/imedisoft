﻿namespace Imedisoft.CEMT.Forms
{
	partial class FormCentralUserEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralUserEdit));
            this.checkIsHidden = new System.Windows.Forms.CheckBox();
            this.passwordButton = new OpenDental.UI.Button();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.userGroupsListBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabUser = new System.Windows.Forms.TabPage();
            this.securityTreeUser = new OpenDental.UserControlSecurityTree();
            this.tabAlertSubs = new System.Windows.Forms.TabPage();
            this.listAlertSubMulti = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.unlockButton = new OpenDental.UI.Button();
            this.tabControl.SuspendLayout();
            this.tabUser.SuspendLayout();
            this.tabAlertSubs.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkIsHidden
            // 
            this.checkIsHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkIsHidden.AutoSize = true;
            this.checkIsHidden.Location = new System.Drawing.Point(12, 444);
            this.checkIsHidden.Name = "checkIsHidden";
            this.checkIsHidden.Size = new System.Drawing.Size(65, 19);
            this.checkIsHidden.TabIndex = 1;
            this.checkIsHidden.Text = "Hidden";
            this.checkIsHidden.UseVisualStyleBackColor = true;
            this.checkIsHidden.CheckedChanged += new System.EventHandler(this.checkIsHidden_CheckedChanged);
            // 
            // passwordButton
            // 
            this.passwordButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.passwordButton.Location = new System.Drawing.Point(12, 484);
            this.passwordButton.Name = "passwordButton";
            this.passwordButton.Size = new System.Drawing.Size(110, 25);
            this.passwordButton.TabIndex = 2;
            this.passwordButton.Text = "Change Password";
            this.passwordButton.Click += new System.EventHandler(this.PasswordButton_Click);
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(6, 6);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(200, 23);
            this.usernameTextBox.TabIndex = 0;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(446, 484);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(532, 484);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "&Cancel";
            // 
            // userGroupsListBox
            // 
            this.userGroupsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.userGroupsListBox.IntegralHeight = false;
            this.userGroupsListBox.ItemHeight = 15;
            this.userGroupsListBox.Location = new System.Drawing.Point(6, 35);
            this.userGroupsListBox.Name = "userGroupsListBox";
            this.userGroupsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.userGroupsListBox.Size = new System.Drawing.Size(200, 361);
            this.userGroupsListBox.TabIndex = 1;
            this.userGroupsListBox.SelectedIndexChanged += new System.EventHandler(this.UserGroupsListBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 60);
            this.label3.TabIndex = 7;
            this.label3.Text = "User Group";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabUser);
            this.tabControl.Controls.Add(this.tabAlertSubs);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(600, 430);
            this.tabControl.TabIndex = 0;
            // 
            // tabUser
            // 
            this.tabUser.BackColor = System.Drawing.SystemColors.Control;
            this.tabUser.Controls.Add(this.securityTreeUser);
            this.tabUser.Controls.Add(this.userGroupsListBox);
            this.tabUser.Controls.Add(this.usernameTextBox);
            this.tabUser.Location = new System.Drawing.Point(4, 24);
            this.tabUser.Name = "tabUser";
            this.tabUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabUser.Size = new System.Drawing.Size(592, 402);
            this.tabUser.TabIndex = 0;
            this.tabUser.Text = "User";
            // 
            // securityTreeUser
            // 
            this.securityTreeUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.securityTreeUser.BackColor = System.Drawing.Color.Transparent;
            this.securityTreeUser.Location = new System.Drawing.Point(212, 35);
            this.securityTreeUser.Name = "securityTreeUser";
            this.securityTreeUser.ReadOnly = true;
            this.securityTreeUser.Size = new System.Drawing.Size(374, 361);
            this.securityTreeUser.TabIndex = 2;
            // 
            // tabAlertSubs
            // 
            this.tabAlertSubs.Controls.Add(this.listAlertSubMulti);
            this.tabAlertSubs.Controls.Add(this.label7);
            this.tabAlertSubs.Location = new System.Drawing.Point(4, 24);
            this.tabAlertSubs.Name = "tabAlertSubs";
            this.tabAlertSubs.Padding = new System.Windows.Forms.Padding(3);
            this.tabAlertSubs.Size = new System.Drawing.Size(592, 402);
            this.tabAlertSubs.TabIndex = 1;
            this.tabAlertSubs.Text = "Alert Subs";
            // 
            // listAlertSubMulti
            // 
            this.listAlertSubMulti.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listAlertSubMulti.ItemHeight = 15;
            this.listAlertSubMulti.Location = new System.Drawing.Point(162, 24);
            this.listAlertSubMulti.Name = "listAlertSubMulti";
            this.listAlertSubMulti.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listAlertSubMulti.Size = new System.Drawing.Size(250, 349);
            this.listAlertSubMulti.TabIndex = 170;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(162, 1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(167, 20);
            this.label7.TabIndex = 169;
            this.label7.Text = "User Alert Subscriptions";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // unlockButton
            // 
            this.unlockButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.unlockButton.Location = new System.Drawing.Point(128, 484);
            this.unlockButton.Name = "unlockButton";
            this.unlockButton.Size = new System.Drawing.Size(110, 25);
            this.unlockButton.TabIndex = 3;
            this.unlockButton.Text = "Unlock Account";
            this.unlockButton.Click += new System.EventHandler(this.UnlockButton_Click);
            // 
            // FormCentralUserEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(624, 521);
            this.Controls.Add(this.unlockButton);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.checkIsHidden);
            this.Controls.Add(this.passwordButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(346, 355);
            this.Name = "FormCentralUserEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Edit";
            this.Load += new System.EventHandler(this.FormCentralUserEdit_Load);
            this.tabControl.ResumeLayout(false);
            this.tabUser.ResumeLayout(false);
            this.tabUser.PerformLayout();
            this.tabAlertSubs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox checkIsHidden;
		private OpenDental.UI.Button passwordButton;
		private System.Windows.Forms.TextBox usernameTextBox;
		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.ListBox userGroupsListBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabUser;
		private System.Windows.Forms.TabPage tabAlertSubs;
		private System.Windows.Forms.ListBox listAlertSubMulti;
		private System.Windows.Forms.Label label7;
		private OpenDental.UI.Button unlockButton;
		private OpenDental.UserControlSecurityTree securityTreeUser;
	}
}
