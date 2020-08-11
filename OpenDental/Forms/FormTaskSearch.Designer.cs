namespace Imedisoft.Forms
{
	partial class FormTaskSearch
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
            this.criteriaGroupBox = new System.Windows.Forms.GroupBox();
            this.includeCompletedCheckBox = new System.Windows.Forms.CheckBox();
            this.limitCheckBox = new System.Windows.Forms.CheckBox();
            this.priorityComboBox = new System.Windows.Forms.ComboBox();
            this.priorityLabel = new System.Windows.Forms.Label();
            this.dateCompletedGroupBox = new System.Windows.Forms.GroupBox();
            this.clearCompletedButton = new OpenDental.UI.Button();
            this.dateCompletedToDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.dateCompletedFromDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.dateCompletedToLabel = new System.Windows.Forms.Label();
            this.dateCompletedFromLabel = new System.Windows.Forms.Label();
            this.dateCreatedGroupBox = new System.Windows.Forms.GroupBox();
            this.clearCreatedButton = new OpenDental.UI.Button();
            this.dateCreatedToDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.dateCreatedFromDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.dateCreatedToLabel = new System.Windows.Forms.Label();
            this.dateCreatedFromLabel = new System.Windows.Forms.Label();
            this.pickUserButton = new OpenDental.UI.Button();
            this.taskListLabel = new System.Windows.Forms.Label();
            this.pickPatientButton = new OpenDental.UI.Button();
            this.userComboBox = new System.Windows.Forms.ComboBox();
            this.patientIdTextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.patientIdLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.taskIdTextBox = new System.Windows.Forms.TextBox();
            this.taskIdLabel = new System.Windows.Forms.Label();
            this.taskListTextBox = new System.Windows.Forms.TextBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.tasksGrid = new OpenDental.UI.ODGrid();
            this.refreshButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.newTaskButton = new OpenDental.UI.Button();
            this.criteriaGroupBox.SuspendLayout();
            this.dateCompletedGroupBox.SuspendLayout();
            this.dateCreatedGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // criteriaGroupBox
            // 
            this.criteriaGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.criteriaGroupBox.Controls.Add(this.includeCompletedCheckBox);
            this.criteriaGroupBox.Controls.Add(this.limitCheckBox);
            this.criteriaGroupBox.Controls.Add(this.priorityComboBox);
            this.criteriaGroupBox.Controls.Add(this.priorityLabel);
            this.criteriaGroupBox.Controls.Add(this.dateCompletedGroupBox);
            this.criteriaGroupBox.Controls.Add(this.dateCreatedGroupBox);
            this.criteriaGroupBox.Controls.Add(this.pickUserButton);
            this.criteriaGroupBox.Controls.Add(this.taskListLabel);
            this.criteriaGroupBox.Controls.Add(this.pickPatientButton);
            this.criteriaGroupBox.Controls.Add(this.userComboBox);
            this.criteriaGroupBox.Controls.Add(this.patientIdTextBox);
            this.criteriaGroupBox.Controls.Add(this.descriptionLabel);
            this.criteriaGroupBox.Controls.Add(this.patientIdLabel);
            this.criteriaGroupBox.Controls.Add(this.descriptionTextBox);
            this.criteriaGroupBox.Controls.Add(this.taskIdTextBox);
            this.criteriaGroupBox.Controls.Add(this.taskIdLabel);
            this.criteriaGroupBox.Controls.Add(this.taskListTextBox);
            this.criteriaGroupBox.Controls.Add(this.userLabel);
            this.criteriaGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.criteriaGroupBox.Location = new System.Drawing.Point(672, 12);
            this.criteriaGroupBox.Name = "criteriaGroupBox";
            this.criteriaGroupBox.Size = new System.Drawing.Size(250, 450);
            this.criteriaGroupBox.TabIndex = 2;
            this.criteriaGroupBox.TabStop = false;
            this.criteriaGroupBox.Text = "Search by:";
            // 
            // includeCompletedCheckBox
            // 
            this.includeCompletedCheckBox.AutoSize = true;
            this.includeCompletedCheckBox.Checked = true;
            this.includeCompletedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.includeCompletedCheckBox.Location = new System.Drawing.Point(6, 396);
            this.includeCompletedCheckBox.Name = "includeCompletedCheckBox";
            this.includeCompletedCheckBox.Size = new System.Drawing.Size(115, 17);
            this.includeCompletedCheckBox.TabIndex = 17;
            this.includeCompletedCheckBox.Text = "Include Completed";
            this.includeCompletedCheckBox.UseVisualStyleBackColor = true;
            // 
            // limitCheckBox
            // 
            this.limitCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.limitCheckBox.AutoSize = true;
            this.limitCheckBox.Checked = true;
            this.limitCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.limitCheckBox.Location = new System.Drawing.Point(6, 419);
            this.limitCheckBox.Name = "limitCheckBox";
            this.limitCheckBox.Size = new System.Drawing.Size(108, 17);
            this.limitCheckBox.TabIndex = 18;
            this.limitCheckBox.Text = "Limit Results (50)";
            this.limitCheckBox.UseVisualStyleBackColor = true;
            // 
            // priorityComboBox
            // 
            this.priorityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priorityComboBox.FormattingEnabled = true;
            this.priorityComboBox.Location = new System.Drawing.Point(90, 122);
            this.priorityComboBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.priorityComboBox.Name = "priorityComboBox";
            this.priorityComboBox.Size = new System.Drawing.Size(123, 21);
            this.priorityComboBox.TabIndex = 13;
            // 
            // priorityLabel
            // 
            this.priorityLabel.AutoSize = true;
            this.priorityLabel.Location = new System.Drawing.Point(43, 125);
            this.priorityLabel.Name = "priorityLabel";
            this.priorityLabel.Size = new System.Drawing.Size(41, 13);
            this.priorityLabel.TabIndex = 12;
            this.priorityLabel.Text = "Priority";
            // 
            // dateCompletedGroupBox
            // 
            this.dateCompletedGroupBox.Controls.Add(this.clearCompletedButton);
            this.dateCompletedGroupBox.Controls.Add(this.dateCompletedToDateTimePicker);
            this.dateCompletedGroupBox.Controls.Add(this.dateCompletedFromDateTimePicker);
            this.dateCompletedGroupBox.Controls.Add(this.dateCompletedToLabel);
            this.dateCompletedGroupBox.Controls.Add(this.dateCompletedFromLabel);
            this.dateCompletedGroupBox.Location = new System.Drawing.Point(6, 267);
            this.dateCompletedGroupBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.dateCompletedGroupBox.Name = "dateCompletedGroupBox";
            this.dateCompletedGroupBox.Size = new System.Drawing.Size(238, 100);
            this.dateCompletedGroupBox.TabIndex = 15;
            this.dateCompletedGroupBox.TabStop = false;
            this.dateCompletedGroupBox.Text = "Date Completed";
            // 
            // clearCompletedButton
            // 
            this.clearCompletedButton.Location = new System.Drawing.Point(157, 71);
            this.clearCompletedButton.Name = "clearCompletedButton";
            this.clearCompletedButton.Size = new System.Drawing.Size(50, 20);
            this.clearCompletedButton.TabIndex = 4;
            this.clearCompletedButton.Text = "Clear";
            this.clearCompletedButton.UseVisualStyleBackColor = true;
            this.clearCompletedButton.Click += new System.EventHandler(this.ClearCompletedButton_Click);
            // 
            // dateCompletedToDateTimePicker
            // 
            this.dateCompletedToDateTimePicker.CustomFormat = " ";
            this.dateCompletedToDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateCompletedToDateTimePicker.Location = new System.Drawing.Point(84, 45);
            this.dateCompletedToDateTimePicker.Name = "dateCompletedToDateTimePicker";
            this.dateCompletedToDateTimePicker.Size = new System.Drawing.Size(123, 20);
            this.dateCompletedToDateTimePicker.TabIndex = 3;
            this.dateCompletedToDateTimePicker.ValueChanged += new System.EventHandler(this.DateCompletedToDateTimePicker_ValueChanged);
            // 
            // dateCompletedFromDateTimePicker
            // 
            this.dateCompletedFromDateTimePicker.CustomFormat = " ";
            this.dateCompletedFromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateCompletedFromDateTimePicker.Location = new System.Drawing.Point(84, 19);
            this.dateCompletedFromDateTimePicker.Name = "dateCompletedFromDateTimePicker";
            this.dateCompletedFromDateTimePicker.Size = new System.Drawing.Size(123, 20);
            this.dateCompletedFromDateTimePicker.TabIndex = 1;
            this.dateCompletedFromDateTimePicker.ValueChanged += new System.EventHandler(this.DateCompletedFromDateTimePicker_ValueChanged);
            // 
            // dateCompletedToLabel
            // 
            this.dateCompletedToLabel.AutoSize = true;
            this.dateCompletedToLabel.Location = new System.Drawing.Point(59, 48);
            this.dateCompletedToLabel.Name = "dateCompletedToLabel";
            this.dateCompletedToLabel.Size = new System.Drawing.Size(19, 13);
            this.dateCompletedToLabel.TabIndex = 2;
            this.dateCompletedToLabel.Text = "To";
            // 
            // dateCompletedFromLabel
            // 
            this.dateCompletedFromLabel.AutoSize = true;
            this.dateCompletedFromLabel.Location = new System.Drawing.Point(47, 22);
            this.dateCompletedFromLabel.Name = "dateCompletedFromLabel";
            this.dateCompletedFromLabel.Size = new System.Drawing.Size(31, 13);
            this.dateCompletedFromLabel.TabIndex = 0;
            this.dateCompletedFromLabel.Text = "From";
            // 
            // dateCreatedGroupBox
            // 
            this.dateCreatedGroupBox.Controls.Add(this.clearCreatedButton);
            this.dateCreatedGroupBox.Controls.Add(this.dateCreatedToDateTimePicker);
            this.dateCreatedGroupBox.Controls.Add(this.dateCreatedFromDateTimePicker);
            this.dateCreatedGroupBox.Controls.Add(this.dateCreatedToLabel);
            this.dateCreatedGroupBox.Controls.Add(this.dateCreatedFromLabel);
            this.dateCreatedGroupBox.Location = new System.Drawing.Point(6, 154);
            this.dateCreatedGroupBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.dateCreatedGroupBox.Name = "dateCreatedGroupBox";
            this.dateCreatedGroupBox.Size = new System.Drawing.Size(238, 100);
            this.dateCreatedGroupBox.TabIndex = 14;
            this.dateCreatedGroupBox.TabStop = false;
            this.dateCreatedGroupBox.Text = "Date Created";
            // 
            // clearCreatedButton
            // 
            this.clearCreatedButton.Location = new System.Drawing.Point(157, 71);
            this.clearCreatedButton.Name = "clearCreatedButton";
            this.clearCreatedButton.Size = new System.Drawing.Size(50, 20);
            this.clearCreatedButton.TabIndex = 4;
            this.clearCreatedButton.Text = "Clear";
            this.clearCreatedButton.UseVisualStyleBackColor = true;
            this.clearCreatedButton.Click += new System.EventHandler(this.ClearCreatedButton_Click);
            // 
            // dateCreatedToDateTimePicker
            // 
            this.dateCreatedToDateTimePicker.CustomFormat = " ";
            this.dateCreatedToDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateCreatedToDateTimePicker.Location = new System.Drawing.Point(84, 45);
            this.dateCreatedToDateTimePicker.Name = "dateCreatedToDateTimePicker";
            this.dateCreatedToDateTimePicker.Size = new System.Drawing.Size(123, 20);
            this.dateCreatedToDateTimePicker.TabIndex = 3;
            this.dateCreatedToDateTimePicker.ValueChanged += new System.EventHandler(this.DateCreatedToDateTimePicker_ValueChanged);
            // 
            // dateCreatedFromDateTimePicker
            // 
            this.dateCreatedFromDateTimePicker.CustomFormat = " ";
            this.dateCreatedFromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateCreatedFromDateTimePicker.Location = new System.Drawing.Point(84, 19);
            this.dateCreatedFromDateTimePicker.Name = "dateCreatedFromDateTimePicker";
            this.dateCreatedFromDateTimePicker.Size = new System.Drawing.Size(123, 20);
            this.dateCreatedFromDateTimePicker.TabIndex = 1;
            this.dateCreatedFromDateTimePicker.ValueChanged += new System.EventHandler(this.DateCreatedFromDateTimePicker_ValueChanged);
            // 
            // dateCreatedToLabel
            // 
            this.dateCreatedToLabel.AutoSize = true;
            this.dateCreatedToLabel.Location = new System.Drawing.Point(59, 48);
            this.dateCreatedToLabel.Name = "dateCreatedToLabel";
            this.dateCreatedToLabel.Size = new System.Drawing.Size(19, 13);
            this.dateCreatedToLabel.TabIndex = 2;
            this.dateCreatedToLabel.Text = "To";
            // 
            // dateCreatedFromLabel
            // 
            this.dateCreatedFromLabel.AutoSize = true;
            this.dateCreatedFromLabel.Location = new System.Drawing.Point(47, 22);
            this.dateCreatedFromLabel.Name = "dateCreatedFromLabel";
            this.dateCreatedFromLabel.Size = new System.Drawing.Size(31, 13);
            this.dateCreatedFromLabel.TabIndex = 0;
            this.dateCreatedFromLabel.Text = "From";
            // 
            // pickUserButton
            // 
            this.pickUserButton.Location = new System.Drawing.Point(219, 16);
            this.pickUserButton.Name = "pickUserButton";
            this.pickUserButton.Size = new System.Drawing.Size(25, 20);
            this.pickUserButton.TabIndex = 2;
            this.pickUserButton.Text = "...";
            this.pickUserButton.UseVisualStyleBackColor = true;
            this.pickUserButton.Click += new System.EventHandler(this.PickUserButton_Click);
            // 
            // taskListLabel
            // 
            this.taskListLabel.AutoSize = true;
            this.taskListLabel.Location = new System.Drawing.Point(36, 41);
            this.taskListLabel.Name = "taskListLabel";
            this.taskListLabel.Size = new System.Drawing.Size(48, 13);
            this.taskListLabel.TabIndex = 3;
            this.taskListLabel.Text = "Task List";
            // 
            // pickPatientButton
            // 
            this.pickPatientButton.Location = new System.Drawing.Point(219, 101);
            this.pickPatientButton.Name = "pickPatientButton";
            this.pickPatientButton.Size = new System.Drawing.Size(25, 20);
            this.pickPatientButton.TabIndex = 11;
            this.pickPatientButton.Text = "...";
            this.pickPatientButton.UseVisualStyleBackColor = true;
            this.pickPatientButton.Click += new System.EventHandler(this.PickPatientButton_Click);
            // 
            // userComboBox
            // 
            this.userComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userComboBox.FormattingEnabled = true;
            this.userComboBox.Location = new System.Drawing.Point(90, 16);
            this.userComboBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.userComboBox.Name = "userComboBox";
            this.userComboBox.Size = new System.Drawing.Size(123, 21);
            this.userComboBox.TabIndex = 1;
            // 
            // patientIdTextBox
            // 
            this.patientIdTextBox.Location = new System.Drawing.Point(90, 101);
            this.patientIdTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.patientIdTextBox.Name = "patientIdTextBox";
            this.patientIdTextBox.Size = new System.Drawing.Size(123, 20);
            this.patientIdTextBox.TabIndex = 10;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(24, 83);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 7;
            this.descriptionLabel.Text = "Description";
            // 
            // patientIdLabel
            // 
            this.patientIdLabel.AutoSize = true;
            this.patientIdLabel.Location = new System.Drawing.Point(32, 104);
            this.patientIdLabel.Name = "patientIdLabel";
            this.patientIdLabel.Size = new System.Drawing.Size(52, 13);
            this.patientIdLabel.TabIndex = 9;
            this.patientIdLabel.Text = "Patient #";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(90, 80);
            this.descriptionTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(123, 20);
            this.descriptionTextBox.TabIndex = 8;
            // 
            // taskIdTextBox
            // 
            this.taskIdTextBox.Location = new System.Drawing.Point(90, 59);
            this.taskIdTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.taskIdTextBox.Name = "taskIdTextBox";
            this.taskIdTextBox.Size = new System.Drawing.Size(123, 20);
            this.taskIdTextBox.TabIndex = 6;
            // 
            // taskIdLabel
            // 
            this.taskIdLabel.AutoSize = true;
            this.taskIdLabel.Location = new System.Drawing.Point(44, 62);
            this.taskIdLabel.Name = "taskIdLabel";
            this.taskIdLabel.Size = new System.Drawing.Size(40, 13);
            this.taskIdLabel.TabIndex = 5;
            this.taskIdLabel.Text = "Task #";
            // 
            // taskListTextBox
            // 
            this.taskListTextBox.Location = new System.Drawing.Point(90, 38);
            this.taskListTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.taskListTextBox.Name = "taskListTextBox";
            this.taskListTextBox.Size = new System.Drawing.Size(123, 20);
            this.taskListTextBox.TabIndex = 4;
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(55, 19);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(29, 13);
            this.userLabel.TabIndex = 0;
            this.userLabel.Text = "User";
            // 
            // tasksGrid
            // 
            this.tasksGrid.AllowSortingByColumn = true;
            this.tasksGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tasksGrid.Location = new System.Drawing.Point(12, 12);
            this.tasksGrid.Name = "tasksGrid";
            this.tasksGrid.NoteSpanStart = 2;
            this.tasksGrid.NoteSpanStop = 2;
            this.tasksGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.tasksGrid.Size = new System.Drawing.Size(654, 672);
            this.tasksGrid.TabIndex = 1;
            this.tasksGrid.Title = "Task Results";
            this.tasksGrid.TranslationName = "TableProg";
            this.tasksGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.TasksGrid_CellDoubleClick);
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.Image = global::Imedisoft.Properties.Resources.IconSyncAlt;
            this.refreshButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.refreshButton.Location = new System.Drawing.Point(842, 468);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(80, 25);
            this.refreshButton.TabIndex = 4;
            this.refreshButton.Text = "&Refresh";
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(842, 659);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Close";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // newTaskButton
            // 
            this.newTaskButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newTaskButton.Location = new System.Drawing.Point(672, 468);
            this.newTaskButton.Name = "newTaskButton";
            this.newTaskButton.Size = new System.Drawing.Size(80, 25);
            this.newTaskButton.TabIndex = 3;
            this.newTaskButton.Text = "&New Task";
            this.newTaskButton.Click += new System.EventHandler(this.NewTaskButton_Click);
            // 
            // FormTaskSearch
            // 
            this.AcceptButton = this.refreshButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(934, 696);
            this.Controls.Add(this.newTaskButton);
            this.Controls.Add(this.criteriaGroupBox);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.tasksGrid);
            this.Controls.Add(this.cancelButton);
            this.Name = "FormTaskSearch";
            this.Text = "Task Search";
            this.Load += new System.EventHandler(this.FormTaskSearch_Load);
            this.criteriaGroupBox.ResumeLayout(false);
            this.criteriaGroupBox.PerformLayout();
            this.dateCompletedGroupBox.ResumeLayout(false);
            this.dateCompletedGroupBox.PerformLayout();
            this.dateCreatedGroupBox.ResumeLayout(false);
            this.dateCreatedGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.ODGrid tasksGrid;
		private OpenDental.UI.Button refreshButton;
		private System.Windows.Forms.GroupBox criteriaGroupBox;
		private System.Windows.Forms.TextBox patientIdTextBox;
		private System.Windows.Forms.Label patientIdLabel;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.TextBox taskIdTextBox;
		private System.Windows.Forms.Label taskIdLabel;
		private System.Windows.Forms.TextBox taskListTextBox;
		private System.Windows.Forms.Label taskListLabel;
		private System.Windows.Forms.Label userLabel;
		private System.Windows.Forms.CheckBox limitCheckBox;
		private OpenDental.UI.Button pickPatientButton;
		private System.Windows.Forms.ComboBox userComboBox;
		private OpenDental.UI.Button pickUserButton;
		private System.Windows.Forms.ComboBox priorityComboBox;
		private System.Windows.Forms.Label priorityLabel;
		private System.Windows.Forms.GroupBox dateCompletedGroupBox;
		private System.Windows.Forms.GroupBox dateCreatedGroupBox;
		private System.Windows.Forms.DateTimePicker dateCreatedFromDateTimePicker;
		private System.Windows.Forms.Label dateCreatedToLabel;
		private System.Windows.Forms.Label dateCreatedFromLabel;
		private System.Windows.Forms.DateTimePicker dateCompletedToDateTimePicker;
		private System.Windows.Forms.DateTimePicker dateCompletedFromDateTimePicker;
		private System.Windows.Forms.Label dateCompletedToLabel;
		private System.Windows.Forms.Label dateCompletedFromLabel;
		private System.Windows.Forms.DateTimePicker dateCreatedToDateTimePicker;
		private OpenDental.UI.Button clearCompletedButton;
		private OpenDental.UI.Button clearCreatedButton;
		private OpenDental.UI.Button newTaskButton;
		private System.Windows.Forms.CheckBox includeCompletedCheckBox;
	}
}
