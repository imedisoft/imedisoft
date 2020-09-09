namespace Imedisoft.Forms
{
    partial class FormStudentResultsForAppointment
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
            this.courseLabel = new System.Windows.Forms.Label();
            this.courseComboBox = new System.Windows.Forms.ComboBox();
            this.classLabel = new System.Windows.Forms.Label();
            this.classComboBox = new System.Windows.Forms.ComboBox();
            this.instructorLabel = new System.Windows.Forms.Label();
            this.instructorComboBox = new System.Windows.Forms.ComboBox();
            this.requirementsGrid = new OpenDental.UI.ODGrid();
            this.addButton = new OpenDental.UI.Button();
            this.removeButton = new OpenDental.UI.Button();
            this.resultsGrid = new OpenDental.UI.ODGrid();
            this.studentsGrid = new OpenDental.UI.ODGrid();
            this.cancelButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // courseLabel
            // 
            this.courseLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.courseLabel.AutoSize = true;
            this.courseLabel.Location = new System.Drawing.Point(585, 42);
            this.courseLabel.Name = "courseLabel";
            this.courseLabel.Size = new System.Drawing.Size(41, 13);
            this.courseLabel.TabIndex = 5;
            this.courseLabel.Text = "Course";
            // 
            // courseComboBox
            // 
            this.courseComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.courseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.courseComboBox.FormattingEnabled = true;
            this.courseComboBox.Location = new System.Drawing.Point(632, 39);
            this.courseComboBox.Name = "courseComboBox";
            this.courseComboBox.Size = new System.Drawing.Size(240, 21);
            this.courseComboBox.TabIndex = 6;
            this.courseComboBox.SelectionChangeCommitted += new System.EventHandler(this.CourseComboBox_SelectionChangeCommitted);
            // 
            // classLabel
            // 
            this.classLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.classLabel.AutoSize = true;
            this.classLabel.Location = new System.Drawing.Point(594, 15);
            this.classLabel.Name = "classLabel";
            this.classLabel.Size = new System.Drawing.Size(32, 13);
            this.classLabel.TabIndex = 3;
            this.classLabel.Text = "Class";
            // 
            // classComboBox
            // 
            this.classComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.classComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classComboBox.FormattingEnabled = true;
            this.classComboBox.Location = new System.Drawing.Point(632, 12);
            this.classComboBox.Name = "classComboBox";
            this.classComboBox.Size = new System.Drawing.Size(240, 21);
            this.classComboBox.TabIndex = 4;
            this.classComboBox.SelectionChangeCommitted += new System.EventHandler(this.ClassComboBox_SelectionChangeCommitted);
            // 
            // instructorLabel
            // 
            this.instructorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.instructorLabel.AutoSize = true;
            this.instructorLabel.Location = new System.Drawing.Point(571, 69);
            this.instructorLabel.Name = "instructorLabel";
            this.instructorLabel.Size = new System.Drawing.Size(55, 13);
            this.instructorLabel.TabIndex = 7;
            this.instructorLabel.Text = "Instructor";
            // 
            // instructorComboBox
            // 
            this.instructorComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.instructorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.instructorComboBox.FormattingEnabled = true;
            this.instructorComboBox.Location = new System.Drawing.Point(632, 66);
            this.instructorComboBox.Name = "instructorComboBox";
            this.instructorComboBox.Size = new System.Drawing.Size(240, 21);
            this.instructorComboBox.TabIndex = 8;
            this.instructorComboBox.SelectionChangeCommitted += new System.EventHandler(this.InstructorComboBox_SelectionChangeCommitted);
            // 
            // requirementsGrid
            // 
            this.requirementsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.requirementsGrid.Location = new System.Drawing.Point(218, 12);
            this.requirementsGrid.Name = "requirementsGrid";
            this.requirementsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.requirementsGrid.Size = new System.Drawing.Size(300, 597);
            this.requirementsGrid.TabIndex = 2;
            this.requirementsGrid.Title = "Requirements";
            this.requirementsGrid.SelectionCommitted += new System.EventHandler(this.RequirementsGrid_SelectionCommitted);
            // 
            // addButton
            // 
            this.addButton.Enabled = false;
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconArrowDownGreen;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(524, 189);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 25);
            this.addButton.TabIndex = 9;
            this.addButton.Text = "Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Enabled = false;
            this.removeButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.removeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.removeButton.Location = new System.Drawing.Point(610, 189);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(80, 25);
            this.removeButton.TabIndex = 10;
            this.removeButton.Text = "Remove";
            this.removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // resultsGrid
            // 
            this.resultsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsGrid.Location = new System.Drawing.Point(524, 220);
            this.resultsGrid.Name = "resultsGrid";
            this.resultsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.resultsGrid.Size = new System.Drawing.Size(348, 260);
            this.resultsGrid.TabIndex = 11;
            this.resultsGrid.Title = "Currently Attached Results";
            this.resultsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.ResultsGrid_CellDoubleClick);
            this.resultsGrid.SelectionCommitted += new System.EventHandler(this.ResultsGrid_SelectionCommitted);
            // 
            // studentsGrid
            // 
            this.studentsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.studentsGrid.Location = new System.Drawing.Point(12, 12);
            this.studentsGrid.Name = "studentsGrid";
            this.studentsGrid.Size = new System.Drawing.Size(200, 597);
            this.studentsGrid.TabIndex = 1;
            this.studentsGrid.Title = "Students";
            this.studentsGrid.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.StudentsGrid_CellClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(792, 584);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Close";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // FormStudentResultsForAppointment
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(884, 621);
            this.Controls.Add(this.instructorLabel);
            this.Controls.Add(this.instructorComboBox);
            this.Controls.Add(this.requirementsGrid);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.resultsGrid);
            this.Controls.Add(this.courseLabel);
            this.Controls.Add(this.courseComboBox);
            this.Controls.Add(this.classLabel);
            this.Controls.Add(this.classComboBox);
            this.Controls.Add(this.studentsGrid);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(860, 580);
            this.Name = "FormStudentResultsForAppointment";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Student Results for Appointment";
            this.Load += new System.EventHandler(this.FormStudentResultsForAppointment_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.ODGrid studentsGrid;
		private System.Windows.Forms.Label courseLabel;
		private System.Windows.Forms.ComboBox courseComboBox;
		private System.Windows.Forms.Label classLabel;
		private System.Windows.Forms.ComboBox classComboBox;
		private OpenDental.UI.ODGrid resultsGrid;
		private OpenDental.UI.Button removeButton;
		private OpenDental.UI.Button addButton;
		private OpenDental.UI.ODGrid requirementsGrid;
		private System.Windows.Forms.Label instructorLabel;
		private System.Windows.Forms.ComboBox instructorComboBox;
	}
}
