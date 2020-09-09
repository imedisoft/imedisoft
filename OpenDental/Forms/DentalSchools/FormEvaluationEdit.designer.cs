namespace Imedisoft.Forms
{
	partial class FormEvaluationEdit
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
            this.schoolCourseLabel = new System.Windows.Forms.Label();
            this.schoolCourseTextBox = new System.Windows.Forms.TextBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.gradingScaleLabel = new System.Windows.Forms.Label();
            this.gradingScaleTextBox = new System.Windows.Forms.TextBox();
            this.dateLabel = new System.Windows.Forms.Label();
            this.instructorLabel = new System.Windows.Forms.Label();
            this.instructorTextBox = new System.Windows.Forms.TextBox();
            this.studentLabel = new System.Windows.Forms.Label();
            this.studentTextBox = new System.Windows.Forms.TextBox();
            this.gradeShowingLabel = new System.Windows.Forms.Label();
            this.gradeShowingOverrideTextBox = new System.Windows.Forms.TextBox();
            this.gradeNumberLabel = new System.Windows.Forms.Label();
            this.gradeNumberOverrideTextBox = new System.Windows.Forms.TextBox();
            this.gradeNumberTextBox = new System.Windows.Forms.TextBox();
            this.gradeShowingTextBox = new System.Windows.Forms.TextBox();
            this.gradingScalesGrid = new OpenDental.UI.ODGrid();
            this.dateTextBox = new System.Windows.Forms.TextBox();
            this.studentButton = new OpenDental.UI.Button();
            this.evaluationCriteriaGrid = new OpenDental.UI.ODGrid();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // schoolCourseLabel
            // 
            this.schoolCourseLabel.AutoSize = true;
            this.schoolCourseLabel.Location = new System.Drawing.Point(113, 68);
            this.schoolCourseLabel.Name = "schoolCourseLabel";
            this.schoolCourseLabel.Size = new System.Drawing.Size(41, 13);
            this.schoolCourseLabel.TabIndex = 4;
            this.schoolCourseLabel.Text = "Course";
            // 
            // schoolCourseTextBox
            // 
            this.schoolCourseTextBox.Location = new System.Drawing.Point(160, 64);
            this.schoolCourseTextBox.MaxLength = 255;
            this.schoolCourseTextBox.Name = "schoolCourseTextBox";
            this.schoolCourseTextBox.ReadOnly = true;
            this.schoolCourseTextBox.Size = new System.Drawing.Size(160, 20);
            this.schoolCourseTextBox.TabIndex = 5;
            this.schoolCourseTextBox.TabStop = false;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(447, 67);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(27, 13);
            this.titleLabel.TabIndex = 11;
            this.titleLabel.Text = "Title";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new System.Drawing.Point(480, 64);
            this.titleTextBox.MaxLength = 255;
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.ReadOnly = true;
            this.titleTextBox.Size = new System.Drawing.Size(160, 20);
            this.titleTextBox.TabIndex = 12;
            this.titleTextBox.TabStop = false;
            // 
            // gradingScaleLabel
            // 
            this.gradingScaleLabel.AutoSize = true;
            this.gradingScaleLabel.Location = new System.Drawing.Point(82, 42);
            this.gradingScaleLabel.Name = "gradingScaleLabel";
            this.gradingScaleLabel.Size = new System.Drawing.Size(72, 13);
            this.gradingScaleLabel.TabIndex = 2;
            this.gradingScaleLabel.Text = "Grading Scale";
            // 
            // gradingScaleTextBox
            // 
            this.gradingScaleTextBox.Location = new System.Drawing.Point(160, 38);
            this.gradingScaleTextBox.MaxLength = 255;
            this.gradingScaleTextBox.Name = "gradingScaleTextBox";
            this.gradingScaleTextBox.ReadOnly = true;
            this.gradingScaleTextBox.Size = new System.Drawing.Size(160, 20);
            this.gradingScaleTextBox.TabIndex = 3;
            this.gradingScaleTextBox.TabStop = false;
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(124, 16);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(30, 13);
            this.dateLabel.TabIndex = 0;
            this.dateLabel.Text = "Date";
            // 
            // instructorLabel
            // 
            this.instructorLabel.AutoSize = true;
            this.instructorLabel.Location = new System.Drawing.Point(419, 41);
            this.instructorLabel.Name = "instructorLabel";
            this.instructorLabel.Size = new System.Drawing.Size(55, 13);
            this.instructorLabel.TabIndex = 9;
            this.instructorLabel.Text = "Instructor";
            // 
            // instructorTextBox
            // 
            this.instructorTextBox.Location = new System.Drawing.Point(480, 38);
            this.instructorTextBox.MaxLength = 255;
            this.instructorTextBox.Name = "instructorTextBox";
            this.instructorTextBox.ReadOnly = true;
            this.instructorTextBox.Size = new System.Drawing.Size(160, 20);
            this.instructorTextBox.TabIndex = 10;
            this.instructorTextBox.TabStop = false;
            // 
            // studentLabel
            // 
            this.studentLabel.AutoSize = true;
            this.studentLabel.Location = new System.Drawing.Point(429, 15);
            this.studentLabel.Name = "studentLabel";
            this.studentLabel.Size = new System.Drawing.Size(45, 13);
            this.studentLabel.TabIndex = 6;
            this.studentLabel.Text = "Student";
            // 
            // studentTextBox
            // 
            this.studentTextBox.Location = new System.Drawing.Point(480, 12);
            this.studentTextBox.MaxLength = 255;
            this.studentTextBox.Name = "studentTextBox";
            this.studentTextBox.ReadOnly = true;
            this.studentTextBox.Size = new System.Drawing.Size(160, 20);
            this.studentTextBox.TabIndex = 7;
            this.studentTextBox.TabStop = false;
            // 
            // gradeShowingLabel
            // 
            this.gradeShowingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gradeShowingLabel.AutoSize = true;
            this.gradeShowingLabel.Location = new System.Drawing.Point(401, 411);
            this.gradeShowingLabel.Name = "gradeShowingLabel";
            this.gradeShowingLabel.Size = new System.Drawing.Size(73, 13);
            this.gradeShowingLabel.TabIndex = 18;
            this.gradeShowingLabel.Text = "Overall Grade";
            // 
            // gradeShowingOverrideTextBox
            // 
            this.gradeShowingOverrideTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gradeShowingOverrideTextBox.Location = new System.Drawing.Point(541, 408);
            this.gradeShowingOverrideTextBox.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.gradeShowingOverrideTextBox.MaxLength = 255;
            this.gradeShowingOverrideTextBox.Name = "gradeShowingOverrideTextBox";
            this.gradeShowingOverrideTextBox.Size = new System.Drawing.Size(60, 20);
            this.gradeShowingOverrideTextBox.TabIndex = 20;
            // 
            // gradeNumberLabel
            // 
            this.gradeNumberLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gradeNumberLabel.AutoSize = true;
            this.gradeNumberLabel.Location = new System.Drawing.Point(361, 385);
            this.gradeNumberLabel.Name = "gradeNumberLabel";
            this.gradeNumberLabel.Size = new System.Drawing.Size(113, 13);
            this.gradeNumberLabel.TabIndex = 15;
            this.gradeNumberLabel.Text = "Overall Grade Number";
            // 
            // gradeNumberOverrideTextBox
            // 
            this.gradeNumberOverrideTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gradeNumberOverrideTextBox.Location = new System.Drawing.Point(541, 382);
            this.gradeNumberOverrideTextBox.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.gradeNumberOverrideTextBox.MaxLength = 255;
            this.gradeNumberOverrideTextBox.Name = "gradeNumberOverrideTextBox";
            this.gradeNumberOverrideTextBox.Size = new System.Drawing.Size(60, 20);
            this.gradeNumberOverrideTextBox.TabIndex = 17;
            // 
            // gradeNumberTextBox
            // 
            this.gradeNumberTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gradeNumberTextBox.Location = new System.Drawing.Point(480, 382);
            this.gradeNumberTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.gradeNumberTextBox.MaxLength = 255;
            this.gradeNumberTextBox.Name = "gradeNumberTextBox";
            this.gradeNumberTextBox.ReadOnly = true;
            this.gradeNumberTextBox.Size = new System.Drawing.Size(60, 20);
            this.gradeNumberTextBox.TabIndex = 16;
            this.gradeNumberTextBox.TabStop = false;
            // 
            // gradeShowingTextBox
            // 
            this.gradeShowingTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gradeShowingTextBox.Location = new System.Drawing.Point(480, 408);
            this.gradeShowingTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.gradeShowingTextBox.MaxLength = 255;
            this.gradeShowingTextBox.Name = "gradeShowingTextBox";
            this.gradeShowingTextBox.ReadOnly = true;
            this.gradeShowingTextBox.Size = new System.Drawing.Size(60, 20);
            this.gradeShowingTextBox.TabIndex = 19;
            this.gradeShowingTextBox.TabStop = false;
            // 
            // gradingScalesGrid
            // 
            this.gradingScalesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gradingScalesGrid.Location = new System.Drawing.Point(632, 90);
            this.gradingScalesGrid.Name = "gradingScalesGrid";
            this.gradingScalesGrid.Size = new System.Drawing.Size(200, 286);
            this.gradingScalesGrid.TabIndex = 14;
            this.gradingScalesGrid.Title = "Grading Scale";
            this.gradingScalesGrid.TranslationName = "FormEvaluationDefEdit";
            this.gradingScalesGrid.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.GradingScalesGrid_CellClick);
            // 
            // dateTextBox
            // 
            this.dateTextBox.Location = new System.Drawing.Point(160, 12);
            this.dateTextBox.Name = "dateTextBox";
            this.dateTextBox.Size = new System.Drawing.Size(100, 20);
            this.dateTextBox.TabIndex = 1;
            // 
            // studentButton
            // 
            this.studentButton.Image = global::Imedisoft.Properties.Resources.IconEllipsisSmall;
            this.studentButton.Location = new System.Drawing.Point(646, 12);
            this.studentButton.Name = "studentButton";
            this.studentButton.Size = new System.Drawing.Size(25, 20);
            this.studentButton.TabIndex = 8;
            this.studentButton.Click += new System.EventHandler(this.StudentButton_Click);
            // 
            // evaluationCriteriaGrid
            // 
            this.evaluationCriteriaGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.evaluationCriteriaGrid.Location = new System.Drawing.Point(12, 90);
            this.evaluationCriteriaGrid.Name = "evaluationCriteriaGrid";
            this.evaluationCriteriaGrid.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
            this.evaluationCriteriaGrid.Size = new System.Drawing.Size(614, 286);
            this.evaluationCriteriaGrid.TabIndex = 13;
            this.evaluationCriteriaGrid.Title = "Evaluation Criteria";
            this.evaluationCriteriaGrid.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.EvaluationCriteriaGrid_CellClick);
            this.evaluationCriteriaGrid.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.EvaluationCriteriaGrid_CellLeave);
            this.evaluationCriteriaGrid.CellEnter += new OpenDental.UI.ODGridClickEventHandler(this.EvaluationCriteriaGrid_CellEnter);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(666, 444);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 21;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(752, 444);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 22;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormEvaluationEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(844, 481);
            this.Controls.Add(this.gradingScalesGrid);
            this.Controls.Add(this.gradeNumberTextBox);
            this.Controls.Add(this.gradeShowingTextBox);
            this.Controls.Add(this.gradeNumberLabel);
            this.Controls.Add(this.gradeNumberOverrideTextBox);
            this.Controls.Add(this.dateTextBox);
            this.Controls.Add(this.studentButton);
            this.Controls.Add(this.evaluationCriteriaGrid);
            this.Controls.Add(this.gradeShowingLabel);
            this.Controls.Add(this.gradeShowingOverrideTextBox);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.instructorLabel);
            this.Controls.Add(this.instructorTextBox);
            this.Controls.Add(this.studentLabel);
            this.Controls.Add(this.studentTextBox);
            this.Controls.Add(this.schoolCourseLabel);
            this.Controls.Add(this.schoolCourseTextBox);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.gradingScaleLabel);
            this.Controls.Add(this.gradingScaleTextBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 340);
            this.Name = "FormEvaluationEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Evaluation";
            this.Load += new System.EventHandler(this.FormEvaluationEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label schoolCourseLabel;
		private System.Windows.Forms.TextBox schoolCourseTextBox;
		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.TextBox titleTextBox;
		private System.Windows.Forms.Label gradingScaleLabel;
		private System.Windows.Forms.TextBox gradingScaleTextBox;
		private System.Windows.Forms.Label dateLabel;
		private System.Windows.Forms.Label instructorLabel;
		private System.Windows.Forms.TextBox instructorTextBox;
		private System.Windows.Forms.Label studentLabel;
		private System.Windows.Forms.TextBox studentTextBox;
		private System.Windows.Forms.Label gradeShowingLabel;
		private System.Windows.Forms.TextBox gradeShowingOverrideTextBox;
		private OpenDental.UI.ODGrid evaluationCriteriaGrid;
		private OpenDental.UI.Button studentButton;
		private System.Windows.Forms.TextBox dateTextBox;
		private System.Windows.Forms.Label gradeNumberLabel;
		private System.Windows.Forms.TextBox gradeNumberOverrideTextBox;
		private System.Windows.Forms.TextBox gradeNumberTextBox;
		private System.Windows.Forms.TextBox gradeShowingTextBox;
		private OpenDental.UI.ODGrid gradingScalesGrid;
	}
}
