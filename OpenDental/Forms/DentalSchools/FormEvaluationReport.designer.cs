namespace Imedisoft.Forms
{
	partial class FormEvaluationReport
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
            this.dateLabel = new System.Windows.Forms.Label();
            this.dateToLabel = new System.Windows.Forms.Label();
            this.allCoursesCheckBox = new System.Windows.Forms.CheckBox();
            this.allInstructorsCheckBox = new System.Windows.Forms.CheckBox();
            this.dateStartTextBox = new System.Windows.Forms.TextBox();
            this.dateEndTextBox = new System.Windows.Forms.TextBox();
            this.schoolCoursesGrid = new OpenDental.UI.ODGrid();
            this.instructorsGrid = new OpenDental.UI.ODGrid();
            this.studentsGrid = new OpenDental.UI.ODGrid();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.allStudentsCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(127, 15);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(57, 13);
            this.dateLabel.TabIndex = 1;
            this.dateLabel.Text = "Date Start";
            // 
            // dateToLabel
            // 
            this.dateToLabel.AutoSize = true;
            this.dateToLabel.Location = new System.Drawing.Point(266, 15);
            this.dateToLabel.Name = "dateToLabel";
            this.dateToLabel.Size = new System.Drawing.Size(17, 13);
            this.dateToLabel.TabIndex = 3;
            this.dateToLabel.Text = "to";
            // 
            // allCoursesCheckBox
            // 
            this.allCoursesCheckBox.AutoSize = true;
            this.allCoursesCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.allCoursesCheckBox.Location = new System.Drawing.Point(12, 44);
            this.allCoursesCheckBox.Name = "allCoursesCheckBox";
            this.allCoursesCheckBox.Size = new System.Drawing.Size(85, 18);
            this.allCoursesCheckBox.TabIndex = 5;
            this.allCoursesCheckBox.Text = "All Courses";
            this.allCoursesCheckBox.CheckedChanged += new System.EventHandler(this.AllCoursesCheckBox_CheckedChanged);
            // 
            // allInstructorsCheckBox
            // 
            this.allInstructorsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.allInstructorsCheckBox.AutoSize = true;
            this.allInstructorsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.allInstructorsCheckBox.Location = new System.Drawing.Point(446, 44);
            this.allInstructorsCheckBox.Name = "allInstructorsCheckBox";
            this.allInstructorsCheckBox.Size = new System.Drawing.Size(99, 18);
            this.allInstructorsCheckBox.TabIndex = 7;
            this.allInstructorsCheckBox.Text = "All Instructors";
            this.allInstructorsCheckBox.CheckedChanged += new System.EventHandler(this.AllInstructorsCheckBox_CheckedChanged);
            // 
            // dateStartTextBox
            // 
            this.dateStartTextBox.Location = new System.Drawing.Point(190, 12);
            this.dateStartTextBox.Name = "dateStartTextBox";
            this.dateStartTextBox.Size = new System.Drawing.Size(70, 20);
            this.dateStartTextBox.TabIndex = 2;
            // 
            // dateEndTextBox
            // 
            this.dateEndTextBox.Location = new System.Drawing.Point(289, 12);
            this.dateEndTextBox.Name = "dateEndTextBox";
            this.dateEndTextBox.Size = new System.Drawing.Size(70, 20);
            this.dateEndTextBox.TabIndex = 4;
            // 
            // schoolCoursesGrid
            // 
            this.schoolCoursesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schoolCoursesGrid.Location = new System.Drawing.Point(12, 68);
            this.schoolCoursesGrid.Name = "schoolCoursesGrid";
            this.schoolCoursesGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.schoolCoursesGrid.Size = new System.Drawing.Size(428, 443);
            this.schoolCoursesGrid.TabIndex = 6;
            this.schoolCoursesGrid.Title = "Courses";
            this.schoolCoursesGrid.TranslationName = "TableCourses";
            this.schoolCoursesGrid.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.SchoolCoursesGrid_CellClick);
            // 
            // instructorsGrid
            // 
            this.instructorsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.instructorsGrid.Location = new System.Drawing.Point(446, 68);
            this.instructorsGrid.Name = "instructorsGrid";
            this.instructorsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.instructorsGrid.Size = new System.Drawing.Size(250, 443);
            this.instructorsGrid.TabIndex = 8;
            this.instructorsGrid.Title = "Instructors";
            this.instructorsGrid.TranslationName = "TableInstructors";
            this.instructorsGrid.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.InstructorsGrid_CellClick);
            // 
            // studentsGrid
            // 
            this.studentsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.studentsGrid.Location = new System.Drawing.Point(702, 68);
            this.studentsGrid.Name = "studentsGrid";
            this.studentsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.studentsGrid.Size = new System.Drawing.Size(250, 443);
            this.studentsGrid.TabIndex = 10;
            this.studentsGrid.Title = "Students";
            this.studentsGrid.TranslationName = "TableStudents";
            this.studentsGrid.SelectionCommitted += new System.EventHandler(this.StudentsGrid_SelectionCommitted);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(786, 544);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 11;
            this.acceptButton.Text = "OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(872, 544);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Close";
            // 
            // allStudentsCheckBox
            // 
            this.allStudentsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.allStudentsCheckBox.AutoSize = true;
            this.allStudentsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.allStudentsCheckBox.Location = new System.Drawing.Point(702, 44);
            this.allStudentsCheckBox.Name = "allStudentsCheckBox";
            this.allStudentsCheckBox.Size = new System.Drawing.Size(89, 18);
            this.allStudentsCheckBox.TabIndex = 7;
            this.allStudentsCheckBox.Text = "All Students";
            this.allStudentsCheckBox.CheckedChanged += new System.EventHandler(this.AllStudentsCheckBox_CheckedChanged);
            // 
            // FormEvaluationReport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(964, 581);
            this.Controls.Add(this.allStudentsCheckBox);
            this.Controls.Add(this.allInstructorsCheckBox);
            this.Controls.Add(this.allCoursesCheckBox);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.dateToLabel);
            this.Controls.Add(this.dateStartTextBox);
            this.Controls.Add(this.dateEndTextBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.schoolCoursesGrid);
            this.Controls.Add(this.instructorsGrid);
            this.Controls.Add(this.studentsGrid);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(870, 340);
            this.Name = "FormEvaluationReport";
            this.Text = "Evaluation Report";
            this.Load += new System.EventHandler(this.FormEvaluationReport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.ODGrid studentsGrid;
		private OpenDental.UI.ODGrid instructorsGrid;
		private OpenDental.UI.ODGrid schoolCoursesGrid;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.Label dateLabel;
		private System.Windows.Forms.TextBox dateStartTextBox;
		private System.Windows.Forms.TextBox dateEndTextBox;
		private System.Windows.Forms.Label dateToLabel;
		private System.Windows.Forms.CheckBox allCoursesCheckBox;
		private System.Windows.Forms.CheckBox allInstructorsCheckBox;
        private System.Windows.Forms.CheckBox allStudentsCheckBox;
    }
}
