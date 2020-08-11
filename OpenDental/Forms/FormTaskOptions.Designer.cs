namespace Imedisoft.Forms
{
	partial class FormTaskOptions
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
            this.acceptButton = new OpenDental.UI.Button();
            this.showFinishedCheckBox = new System.Windows.Forms.CheckBox();
            this.startDateTextBox = new OpenDental.ValidDate();
            this.startDateLabel = new System.Windows.Forms.Label();
            this.sortApptDateTimeCheckBox = new System.Windows.Forms.CheckBox();
            this.defaultCollapsedCheckBox = new System.Windows.Forms.CheckBox();
            this.showArchivedTaskListsCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(276, 120);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // showFinishedCheckBox
            // 
            this.showFinishedCheckBox.AutoSize = true;
            this.showFinishedCheckBox.Location = new System.Drawing.Point(12, 12);
            this.showFinishedCheckBox.Name = "showFinishedCheckBox";
            this.showFinishedCheckBox.Size = new System.Drawing.Size(124, 17);
            this.showFinishedCheckBox.TabIndex = 11;
            this.showFinishedCheckBox.Text = "Show Finished Tasks";
            this.showFinishedCheckBox.UseVisualStyleBackColor = true;
            this.showFinishedCheckBox.Click += new System.EventHandler(this.checkShowFinished_Click);
            // 
            // startDateTextBox
            // 
            this.startDateTextBox.Location = new System.Drawing.Point(200, 35);
            this.startDateTextBox.Name = "startDateTextBox";
            this.startDateTextBox.Size = new System.Drawing.Size(80, 20);
            this.startDateTextBox.TabIndex = 13;
            // 
            // startDateLabel
            // 
            this.startDateLabel.AutoSize = true;
            this.startDateLabel.Location = new System.Drawing.Point(70, 38);
            this.startDateLabel.Name = "startDateLabel";
            this.startDateLabel.Size = new System.Drawing.Size(124, 13);
            this.startDateLabel.TabIndex = 14;
            this.startDateLabel.Text = "Finished Task Start Date";
            // 
            // sortApptDateTimeCheckBox
            // 
            this.sortApptDateTimeCheckBox.AutoSize = true;
            this.sortApptDateTimeCheckBox.Location = new System.Drawing.Point(12, 61);
            this.sortApptDateTimeCheckBox.Name = "sortApptDateTimeCheckBox";
            this.sortApptDateTimeCheckBox.Size = new System.Drawing.Size(258, 17);
            this.sortApptDateTimeCheckBox.TabIndex = 15;
            this.sortApptDateTimeCheckBox.Text = "Sort appointment type task lists by AptDateTime";
            this.sortApptDateTimeCheckBox.UseVisualStyleBackColor = true;
            // 
            // defaultCollapsedCheckBox
            // 
            this.defaultCollapsedCheckBox.AutoSize = true;
            this.defaultCollapsedCheckBox.Location = new System.Drawing.Point(12, 84);
            this.defaultCollapsedCheckBox.Name = "defaultCollapsedCheckBox";
            this.defaultCollapsedCheckBox.Size = new System.Drawing.Size(177, 17);
            this.defaultCollapsedCheckBox.TabIndex = 16;
            this.defaultCollapsedCheckBox.Text = "Default tasks to collapsed state";
            this.defaultCollapsedCheckBox.UseVisualStyleBackColor = true;
            // 
            // showArchivedTaskListsCheckBox
            // 
            this.showArchivedTaskListsCheckBox.AutoSize = true;
            this.showArchivedTaskListsCheckBox.Location = new System.Drawing.Point(12, 107);
            this.showArchivedTaskListsCheckBox.Name = "showArchivedTaskListsCheckBox";
            this.showArchivedTaskListsCheckBox.Size = new System.Drawing.Size(146, 17);
            this.showArchivedTaskListsCheckBox.TabIndex = 17;
            this.showArchivedTaskListsCheckBox.Text = "Show Archived Task Lists";
            this.showArchivedTaskListsCheckBox.UseVisualStyleBackColor = true;
            // 
            // FormTaskOptions
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(368, 157);
            this.ControlBox = false;
            this.Controls.Add(this.showArchivedTaskListsCheckBox);
            this.Controls.Add(this.defaultCollapsedCheckBox);
            this.Controls.Add(this.sortApptDateTimeCheckBox);
            this.Controls.Add(this.startDateTextBox);
            this.Controls.Add(this.startDateLabel);
            this.Controls.Add(this.showFinishedCheckBox);
            this.Controls.Add(this.acceptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormTaskOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Task Options";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.CheckBox showFinishedCheckBox;
		private OpenDental.ValidDate startDateTextBox;
		private System.Windows.Forms.Label startDateLabel;
		private System.Windows.Forms.CheckBox sortApptDateTimeCheckBox;
		private System.Windows.Forms.CheckBox defaultCollapsedCheckBox;
		private System.Windows.Forms.CheckBox showArchivedTaskListsCheckBox;
	}
}
