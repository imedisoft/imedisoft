namespace Imedisoft.Forms
{
	partial class FormTaskPreferences
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
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.sortApptDateTimeCheckBox = new System.Windows.Forms.CheckBox();
            this.showOpenTicketsCheckBox = new System.Windows.Forms.CheckBox();
            this.localGroupBox = new System.Windows.Forms.GroupBox();
            this.dockRightRadioButton = new System.Windows.Forms.RadioButton();
            this.dockBottomRadioButton = new System.Windows.Forms.RadioButton();
            this.yDefaultTextBox = new System.Windows.Forms.TextBox();
            this.yDefaultLabel = new System.Windows.Forms.Label();
            this.xDefaultTextBox = new System.Windows.Forms.TextBox();
            this.keepTaskListHiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.xDefaultLabel = new System.Windows.Forms.Label();
            this.alwaysShowTaskListCheckBox = new System.Windows.Forms.CheckBox();
            this.globalGroupBox = new System.Windows.Forms.GroupBox();
            this.inboxSetupButton = new OpenDental.UI.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.localGroupBox.SuspendLayout();
            this.globalGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(206, 324);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(292, 324);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // sortApptDateTimeCheckBox
            // 
            this.sortApptDateTimeCheckBox.AutoSize = true;
            this.sortApptDateTimeCheckBox.Location = new System.Drawing.Point(13, 72);
            this.sortApptDateTimeCheckBox.Name = "sortApptDateTimeCheckBox";
            this.sortApptDateTimeCheckBox.Size = new System.Drawing.Size(322, 17);
            this.sortApptDateTimeCheckBox.TabIndex = 4;
            this.sortApptDateTimeCheckBox.Text = "Default to sorting appointment type task lists by AptDateTime";
            // 
            // showOpenTicketsCheckBox
            // 
            this.showOpenTicketsCheckBox.AutoSize = true;
            this.showOpenTicketsCheckBox.Location = new System.Drawing.Point(13, 49);
            this.showOpenTicketsCheckBox.Name = "showOpenTicketsCheckBox";
            this.showOpenTicketsCheckBox.Size = new System.Drawing.Size(148, 17);
            this.showOpenTicketsCheckBox.TabIndex = 3;
            this.showOpenTicketsCheckBox.Text = "Show open tasks for user";
            // 
            // localGroupBox
            // 
            this.localGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.localGroupBox.Controls.Add(this.dockRightRadioButton);
            this.localGroupBox.Controls.Add(this.dockBottomRadioButton);
            this.localGroupBox.Controls.Add(this.yDefaultTextBox);
            this.localGroupBox.Controls.Add(this.yDefaultLabel);
            this.localGroupBox.Controls.Add(this.xDefaultTextBox);
            this.localGroupBox.Controls.Add(this.keepTaskListHiddenCheckBox);
            this.localGroupBox.Controls.Add(this.xDefaultLabel);
            this.localGroupBox.Enabled = false;
            this.localGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.localGroupBox.Location = new System.Drawing.Point(12, 166);
            this.localGroupBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.localGroupBox.Name = "localGroupBox";
            this.localGroupBox.Padding = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.localGroupBox.Size = new System.Drawing.Size(360, 130);
            this.localGroupBox.TabIndex = 2;
            this.localGroupBox.TabStop = false;
            this.localGroupBox.Text = "Local Computer Settings";
            // 
            // dockRightRadioButton
            // 
            this.dockRightRadioButton.AutoSize = true;
            this.dockRightRadioButton.Location = new System.Drawing.Point(13, 26);
            this.dockRightRadioButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.dockRightRadioButton.Name = "dockRightRadioButton";
            this.dockRightRadioButton.Size = new System.Drawing.Size(76, 17);
            this.dockRightRadioButton.TabIndex = 0;
            this.dockRightRadioButton.TabStop = true;
            this.dockRightRadioButton.Text = "Dock Right";
            this.dockRightRadioButton.UseVisualStyleBackColor = true;
            // 
            // dockBottomRadioButton
            // 
            this.dockBottomRadioButton.AutoSize = true;
            this.dockBottomRadioButton.Location = new System.Drawing.Point(95, 26);
            this.dockBottomRadioButton.Name = "dockBottomRadioButton";
            this.dockBottomRadioButton.Size = new System.Drawing.Size(85, 17);
            this.dockBottomRadioButton.TabIndex = 1;
            this.dockBottomRadioButton.TabStop = true;
            this.dockBottomRadioButton.Text = "Dock Bottom";
            this.dockBottomRadioButton.UseVisualStyleBackColor = true;
            // 
            // yDefaultTextBox
            // 
            this.yDefaultTextBox.Location = new System.Drawing.Point(210, 60);
            this.yDefaultTextBox.MaxLength = 4;
            this.yDefaultTextBox.Name = "yDefaultTextBox";
            this.yDefaultTextBox.Size = new System.Drawing.Size(47, 20);
            this.yDefaultTextBox.TabIndex = 5;
            this.yDefaultTextBox.Text = "542";
            this.yDefaultTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.yDefaultTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DefaultTextBox_KeyPress);
            this.yDefaultTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.DefaultTextBox_Validating);
            // 
            // yDefaultLabel
            // 
            this.yDefaultLabel.AutoSize = true;
            this.yDefaultLabel.Location = new System.Drawing.Point(153, 63);
            this.yDefaultLabel.Name = "yDefaultLabel";
            this.yDefaultLabel.Size = new System.Drawing.Size(51, 13);
            this.yDefaultLabel.TabIndex = 4;
            this.yDefaultLabel.Text = "Y Default";
            // 
            // xDefaultTextBox
            // 
            this.xDefaultTextBox.Location = new System.Drawing.Point(90, 60);
            this.xDefaultTextBox.MaxLength = 4;
            this.xDefaultTextBox.Name = "xDefaultTextBox";
            this.xDefaultTextBox.Size = new System.Drawing.Size(47, 20);
            this.xDefaultTextBox.TabIndex = 3;
            this.xDefaultTextBox.Text = "542";
            this.xDefaultTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.xDefaultTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DefaultTextBox_KeyPress);
            this.xDefaultTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.DefaultTextBox_Validating);
            // 
            // keepTaskListHiddenCheckBox
            // 
            this.keepTaskListHiddenCheckBox.AutoSize = true;
            this.keepTaskListHiddenCheckBox.Location = new System.Drawing.Point(13, 93);
            this.keepTaskListHiddenCheckBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.keepTaskListHiddenCheckBox.Name = "keepTaskListHiddenCheckBox";
            this.keepTaskListHiddenCheckBox.Size = new System.Drawing.Size(162, 17);
            this.keepTaskListHiddenCheckBox.TabIndex = 6;
            this.keepTaskListHiddenCheckBox.Text = "Don\'t show on this computer";
            this.keepTaskListHiddenCheckBox.UseVisualStyleBackColor = true;
            this.keepTaskListHiddenCheckBox.CheckedChanged += new System.EventHandler(this.KeepTaskListHiddenCheckBox_CheckedChanged);
            // 
            // xDefaultLabel
            // 
            this.xDefaultLabel.AutoSize = true;
            this.xDefaultLabel.Location = new System.Drawing.Point(33, 63);
            this.xDefaultLabel.Name = "xDefaultLabel";
            this.xDefaultLabel.Size = new System.Drawing.Size(51, 13);
            this.xDefaultLabel.TabIndex = 2;
            this.xDefaultLabel.Text = "X Default";
            // 
            // alwaysShowTaskListCheckBox
            // 
            this.alwaysShowTaskListCheckBox.AutoSize = true;
            this.alwaysShowTaskListCheckBox.Location = new System.Drawing.Point(13, 26);
            this.alwaysShowTaskListCheckBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.alwaysShowTaskListCheckBox.Name = "alwaysShowTaskListCheckBox";
            this.alwaysShowTaskListCheckBox.Size = new System.Drawing.Size(127, 17);
            this.alwaysShowTaskListCheckBox.TabIndex = 1;
            this.alwaysShowTaskListCheckBox.Text = "Always show task list";
            this.alwaysShowTaskListCheckBox.CheckedChanged += new System.EventHandler(this.AlwaysShowTaskListCheckBox_CheckedChanged);
            // 
            // globalGroupBox
            // 
            this.globalGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.globalGroupBox.Controls.Add(this.sortApptDateTimeCheckBox);
            this.globalGroupBox.Controls.Add(this.alwaysShowTaskListCheckBox);
            this.globalGroupBox.Controls.Add(this.showOpenTicketsCheckBox);
            this.globalGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.globalGroupBox.Location = new System.Drawing.Point(12, 43);
            this.globalGroupBox.Name = "globalGroupBox";
            this.globalGroupBox.Padding = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.globalGroupBox.Size = new System.Drawing.Size(360, 110);
            this.globalGroupBox.TabIndex = 1;
            this.globalGroupBox.TabStop = false;
            this.globalGroupBox.Text = "Global Settings";
            // 
            // inboxSetupButton
            // 
            this.inboxSetupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.inboxSetupButton.Location = new System.Drawing.Point(292, 12);
            this.inboxSetupButton.Name = "inboxSetupButton";
            this.inboxSetupButton.Size = new System.Drawing.Size(80, 25);
            this.inboxSetupButton.TabIndex = 0;
            this.inboxSetupButton.Text = "Inbox Setup";
            this.inboxSetupButton.UseVisualStyleBackColor = true;
            this.inboxSetupButton.Click += new System.EventHandler(this.InboxSetupButton_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // FormTaskPreferences
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.localGroupBox);
            this.Controls.Add(this.inboxSetupButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.globalGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTaskPreferences";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tasks Preferences";
            this.Load += new System.EventHandler(this.FormTaskPreferences_Load);
            this.localGroupBox.ResumeLayout(false);
            this.localGroupBox.PerformLayout();
            this.globalGroupBox.ResumeLayout(false);
            this.globalGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.CheckBox sortApptDateTimeCheckBox;
		private System.Windows.Forms.CheckBox showOpenTicketsCheckBox;
		private System.Windows.Forms.GroupBox localGroupBox;
		private System.Windows.Forms.RadioButton dockRightRadioButton;
		private System.Windows.Forms.RadioButton dockBottomRadioButton;
		private System.Windows.Forms.TextBox yDefaultTextBox;
		private System.Windows.Forms.Label yDefaultLabel;
		private System.Windows.Forms.TextBox xDefaultTextBox;
		private System.Windows.Forms.Label xDefaultLabel;
		private System.Windows.Forms.CheckBox keepTaskListHiddenCheckBox;
		private System.Windows.Forms.CheckBox alwaysShowTaskListCheckBox;
		private System.Windows.Forms.GroupBox globalGroupBox;
		private OpenDental.UI.Button inboxSetupButton;
		private System.Windows.Forms.ErrorProvider errorProvider1;
	}
}
