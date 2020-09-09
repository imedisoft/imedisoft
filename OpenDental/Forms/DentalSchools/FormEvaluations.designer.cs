namespace Imedisoft.Forms
{
	partial class FormEvaluations
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
            this.cancelButton = new OpenDental.UI.Button();
            this.evaluationsGrid = new OpenDental.UI.ODGrid();
            this.instructorComboBox = new System.Windows.Forms.ComboBox();
            this.instructorLabel = new System.Windows.Forms.Label();
            this.studentsGroupBox = new System.Windows.Forms.GroupBox();
            this.dateLabel = new System.Windows.Forms.Label();
            this.schoolCourseLabel = new System.Windows.Forms.Label();
            this.dateToLabel = new System.Windows.Forms.Label();
            this.schoolCourseComboBox = new System.Windows.Forms.ComboBox();
            this.dateStartTextBox = new ODR.ValidDate();
            this.dateEndTextBox = new ODR.ValidDate();
            this.providerIdLabel = new System.Windows.Forms.Label();
            this.providerIdTextBox = new System.Windows.Forms.TextBox();
            this.refreshButton = new OpenDental.UI.Button();
            this.firstNameLabel = new System.Windows.Forms.Label();
            this.firstNameTextBox = new System.Windows.Forms.TextBox();
            this.lastNameLabel = new System.Windows.Forms.Label();
            this.lastNameTextBox = new System.Windows.Forms.TextBox();
            this.addButton = new OpenDental.UI.Button();
            this.reportsButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.editButton = new OpenDental.UI.Button();
            this.studentsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(812, 484);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Close";
            // 
            // evaluationsGrid
            // 
            this.evaluationsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.evaluationsGrid.HScrollVisible = true;
            this.evaluationsGrid.Location = new System.Drawing.Point(12, 12);
            this.evaluationsGrid.Name = "evaluationsGrid";
            this.evaluationsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.evaluationsGrid.Size = new System.Drawing.Size(604, 466);
            this.evaluationsGrid.TabIndex = 2;
            this.evaluationsGrid.Title = "Evaluations";
            this.evaluationsGrid.TranslationName = "TableEvaluationSetup";
            this.evaluationsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.EvaluationsGrid_CellDoubleClick);
            this.evaluationsGrid.SelectionCommitted += new System.EventHandler(this.EvaluationsGrid_SelectionCommitted);
            // 
            // instructorComboBox
            // 
            this.instructorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.instructorComboBox.FormattingEnabled = true;
            this.instructorComboBox.ItemHeight = 13;
            this.instructorComboBox.Location = new System.Drawing.Point(98, 174);
            this.instructorComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.instructorComboBox.Name = "instructorComboBox";
            this.instructorComboBox.Size = new System.Drawing.Size(166, 21);
            this.instructorComboBox.TabIndex = 13;
            // 
            // instructorLabel
            // 
            this.instructorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.instructorLabel.AutoSize = true;
            this.instructorLabel.Location = new System.Drawing.Point(37, 177);
            this.instructorLabel.Name = "instructorLabel";
            this.instructorLabel.Size = new System.Drawing.Size(55, 13);
            this.instructorLabel.TabIndex = 12;
            this.instructorLabel.Text = "Instructor";
            // 
            // studentsGroupBox
            // 
            this.studentsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.studentsGroupBox.Controls.Add(this.instructorLabel);
            this.studentsGroupBox.Controls.Add(this.dateLabel);
            this.studentsGroupBox.Controls.Add(this.schoolCourseLabel);
            this.studentsGroupBox.Controls.Add(this.dateToLabel);
            this.studentsGroupBox.Controls.Add(this.schoolCourseComboBox);
            this.studentsGroupBox.Controls.Add(this.dateStartTextBox);
            this.studentsGroupBox.Controls.Add(this.dateEndTextBox);
            this.studentsGroupBox.Controls.Add(this.instructorComboBox);
            this.studentsGroupBox.Controls.Add(this.providerIdLabel);
            this.studentsGroupBox.Controls.Add(this.providerIdTextBox);
            this.studentsGroupBox.Controls.Add(this.refreshButton);
            this.studentsGroupBox.Controls.Add(this.firstNameLabel);
            this.studentsGroupBox.Controls.Add(this.firstNameTextBox);
            this.studentsGroupBox.Controls.Add(this.lastNameLabel);
            this.studentsGroupBox.Controls.Add(this.lastNameTextBox);
            this.studentsGroupBox.Location = new System.Drawing.Point(622, 12);
            this.studentsGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.studentsGroupBox.Name = "studentsGroupBox";
            this.studentsGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.studentsGroupBox.Size = new System.Drawing.Size(270, 250);
            this.studentsGroupBox.TabIndex = 0;
            this.studentsGroupBox.TabStop = false;
            this.studentsGroupBox.Text = "Criteria";
            // 
            // dateLabel
            // 
            this.dateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(62, 29);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(30, 13);
            this.dateLabel.TabIndex = 0;
            this.dateLabel.Text = "Date";
            // 
            // schoolCourseLabel
            // 
            this.schoolCourseLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.schoolCourseLabel.AutoSize = true;
            this.schoolCourseLabel.Location = new System.Drawing.Point(51, 55);
            this.schoolCourseLabel.Name = "schoolCourseLabel";
            this.schoolCourseLabel.Size = new System.Drawing.Size(41, 13);
            this.schoolCourseLabel.TabIndex = 4;
            this.schoolCourseLabel.Text = "Course";
            // 
            // dateToLabel
            // 
            this.dateToLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateToLabel.AutoSize = true;
            this.dateToLabel.Location = new System.Drawing.Point(174, 29);
            this.dateToLabel.Name = "dateToLabel";
            this.dateToLabel.Size = new System.Drawing.Size(17, 13);
            this.dateToLabel.TabIndex = 2;
            this.dateToLabel.Text = "to";
            // 
            // schoolCourseComboBox
            // 
            this.schoolCourseComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.schoolCourseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.schoolCourseComboBox.FormattingEnabled = true;
            this.schoolCourseComboBox.ItemHeight = 13;
            this.schoolCourseComboBox.Location = new System.Drawing.Point(98, 52);
            this.schoolCourseComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.schoolCourseComboBox.Name = "schoolCourseComboBox";
            this.schoolCourseComboBox.Size = new System.Drawing.Size(166, 21);
            this.schoolCourseComboBox.TabIndex = 5;
            this.schoolCourseComboBox.SelectionChangeCommitted += new System.EventHandler(this.SchoolCourseComboBox_SelectionChangeCommitted);
            // 
            // dateStartTextBox
            // 
            this.dateStartTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateStartTextBox.Location = new System.Drawing.Point(98, 26);
            this.dateStartTextBox.Name = "dateStartTextBox";
            this.dateStartTextBox.Size = new System.Drawing.Size(70, 20);
            this.dateStartTextBox.TabIndex = 1;
            this.dateStartTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.DateTextBox_Validating);
            // 
            // dateEndTextBox
            // 
            this.dateEndTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateEndTextBox.Location = new System.Drawing.Point(197, 26);
            this.dateEndTextBox.Name = "dateEndTextBox";
            this.dateEndTextBox.Size = new System.Drawing.Size(70, 20);
            this.dateEndTextBox.TabIndex = 3;
            this.dateEndTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.DateTextBox_Validating);
            // 
            // providerIdLabel
            // 
            this.providerIdLabel.AutoSize = true;
            this.providerIdLabel.Location = new System.Drawing.Point(34, 151);
            this.providerIdLabel.Name = "providerIdLabel";
            this.providerIdLabel.Size = new System.Drawing.Size(58, 13);
            this.providerIdLabel.TabIndex = 10;
            this.providerIdLabel.Text = "Provider #";
            // 
            // providerIdTextBox
            // 
            this.providerIdTextBox.Location = new System.Drawing.Point(98, 148);
            this.providerIdTextBox.MaxLength = 15;
            this.providerIdTextBox.Name = "providerIdTextBox";
            this.providerIdTextBox.Size = new System.Drawing.Size(60, 20);
            this.providerIdTextBox.TabIndex = 11;
            this.providerIdTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ProviderIdTextBox_KeyPress);
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.Image = global::Imedisoft.Properties.Resources.IconSyncAlt;
            this.refreshButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.refreshButton.Location = new System.Drawing.Point(184, 208);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(80, 25);
            this.refreshButton.TabIndex = 14;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.Location = new System.Drawing.Point(34, 125);
            this.firstNameLabel.Name = "firstNameLabel";
            this.firstNameLabel.Size = new System.Drawing.Size(58, 13);
            this.firstNameLabel.TabIndex = 8;
            this.firstNameLabel.Text = "First Name";
            // 
            // firstNameTextBox
            // 
            this.firstNameTextBox.Location = new System.Drawing.Point(98, 122);
            this.firstNameTextBox.MaxLength = 15;
            this.firstNameTextBox.Name = "firstNameTextBox";
            this.firstNameTextBox.Size = new System.Drawing.Size(166, 20);
            this.firstNameTextBox.TabIndex = 9;
            // 
            // lastNameLabel
            // 
            this.lastNameLabel.AutoSize = true;
            this.lastNameLabel.Location = new System.Drawing.Point(35, 99);
            this.lastNameLabel.Name = "lastNameLabel";
            this.lastNameLabel.Size = new System.Drawing.Size(57, 13);
            this.lastNameLabel.TabIndex = 6;
            this.lastNameLabel.Text = "Last Name";
            // 
            // lastNameTextBox
            // 
            this.lastNameTextBox.Location = new System.Drawing.Point(98, 96);
            this.lastNameTextBox.MaxLength = 15;
            this.lastNameTextBox.Name = "lastNameTextBox";
            this.lastNameTextBox.Size = new System.Drawing.Size(166, 20);
            this.lastNameTextBox.TabIndex = 7;
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(12, 484);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 3;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // reportsButton
            // 
            this.reportsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reportsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.reportsButton.Location = new System.Drawing.Point(147, 484);
            this.reportsButton.Name = "reportsButton";
            this.reportsButton.Size = new System.Drawing.Size(80, 25);
            this.reportsButton.TabIndex = 6;
            this.reportsButton.Text = "Reports";
            this.reportsButton.Click += new System.EventHandler(this.ReportsButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(84, 484);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPenBlack;
            this.editButton.Location = new System.Drawing.Point(48, 484);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(30, 25);
            this.editButton.TabIndex = 4;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // FormEvaluations
            // 
            this.AcceptButton = this.refreshButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(904, 521);
            this.Controls.Add(this.reportsButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.studentsGroupBox);
            this.Controls.Add(this.evaluationsGrid);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 520);
            this.Name = "FormEvaluations";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Evaluations";
            this.Load += new System.EventHandler(this.FormEvaluations_Load);
            this.studentsGroupBox.ResumeLayout(false);
            this.studentsGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.ODGrid evaluationsGrid;
		private System.Windows.Forms.ComboBox instructorComboBox;
		private System.Windows.Forms.Label instructorLabel;
		private System.Windows.Forms.GroupBox studentsGroupBox;
		private System.Windows.Forms.Label providerIdLabel;
		private System.Windows.Forms.TextBox providerIdTextBox;
		private System.Windows.Forms.Label firstNameLabel;
		private System.Windows.Forms.TextBox firstNameTextBox;
		private System.Windows.Forms.Label lastNameLabel;
		private System.Windows.Forms.TextBox lastNameTextBox;
		private OpenDental.UI.Button addButton;
		private System.Windows.Forms.Label dateToLabel;
		private System.Windows.Forms.Label dateLabel;
		private OpenDental.UI.Button refreshButton;
		private System.Windows.Forms.ComboBox schoolCourseComboBox;
		private System.Windows.Forms.Label schoolCourseLabel;
		private OpenDental.UI.Button reportsButton;
        private ODR.ValidDate dateEndTextBox;
        private ODR.ValidDate dateStartTextBox;
        private OpenDental.UI.Button deleteButton;
        private OpenDental.UI.Button editButton;
    }
}
