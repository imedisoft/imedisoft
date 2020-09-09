namespace Imedisoft.Forms
{
	partial class FormEvaluationDefEdit
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
            this.gradingScaleTextBox = new System.Windows.Forms.TextBox();
            this.gradingScaleLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.schoolCourseLabel = new System.Windows.Forms.Label();
            this.schoolCourseTextBox = new System.Windows.Forms.TextBox();
            this.schoolCourseButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.gradingScaleButton = new OpenDental.UI.Button();
            this.downButton = new OpenDental.UI.Button();
            this.upButton = new OpenDental.UI.Button();
            this.evaluationCriterionDefsGrid = new OpenDental.UI.ODGrid();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.totalPointsLabel = new System.Windows.Forms.Label();
            this.totalPointsTextBox = new System.Windows.Forms.TextBox();
            this.editButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // gradingScaleTextBox
            // 
            this.gradingScaleTextBox.Location = new System.Drawing.Point(120, 38);
            this.gradingScaleTextBox.MaxLength = 255;
            this.gradingScaleTextBox.Name = "gradingScaleTextBox";
            this.gradingScaleTextBox.ReadOnly = true;
            this.gradingScaleTextBox.Size = new System.Drawing.Size(160, 20);
            this.gradingScaleTextBox.TabIndex = 3;
            // 
            // gradingScaleLabel
            // 
            this.gradingScaleLabel.AutoSize = true;
            this.gradingScaleLabel.Location = new System.Drawing.Point(42, 41);
            this.gradingScaleLabel.Name = "gradingScaleLabel";
            this.gradingScaleLabel.Size = new System.Drawing.Size(72, 13);
            this.gradingScaleLabel.TabIndex = 2;
            this.gradingScaleLabel.Text = "Grading Scale";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(87, 15);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(27, 13);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Title";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new System.Drawing.Point(120, 12);
            this.titleTextBox.MaxLength = 255;
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(160, 20);
            this.titleTextBox.TabIndex = 1;
            // 
            // schoolCourseLabel
            // 
            this.schoolCourseLabel.AutoSize = true;
            this.schoolCourseLabel.Location = new System.Drawing.Point(73, 67);
            this.schoolCourseLabel.Name = "schoolCourseLabel";
            this.schoolCourseLabel.Size = new System.Drawing.Size(41, 13);
            this.schoolCourseLabel.TabIndex = 5;
            this.schoolCourseLabel.Text = "Course";
            // 
            // schoolCourseTextBox
            // 
            this.schoolCourseTextBox.Location = new System.Drawing.Point(120, 64);
            this.schoolCourseTextBox.MaxLength = 255;
            this.schoolCourseTextBox.Name = "schoolCourseTextBox";
            this.schoolCourseTextBox.ReadOnly = true;
            this.schoolCourseTextBox.Size = new System.Drawing.Size(160, 20);
            this.schoolCourseTextBox.TabIndex = 6;
            // 
            // schoolCourseButton
            // 
            this.schoolCourseButton.Image = global::Imedisoft.Properties.Resources.IconEllipsisSmall;
            this.schoolCourseButton.Location = new System.Drawing.Point(286, 64);
            this.schoolCourseButton.Name = "schoolCourseButton";
            this.schoolCourseButton.Size = new System.Drawing.Size(25, 20);
            this.schoolCourseButton.TabIndex = 7;
            this.schoolCourseButton.Click += new System.EventHandler(this.SchoolCourseButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(422, 93);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 9;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // gradingScaleButton
            // 
            this.gradingScaleButton.Image = global::Imedisoft.Properties.Resources.IconEllipsisSmall;
            this.gradingScaleButton.Location = new System.Drawing.Point(286, 38);
            this.gradingScaleButton.Name = "gradingScaleButton";
            this.gradingScaleButton.Size = new System.Drawing.Size(25, 20);
            this.gradingScaleButton.TabIndex = 4;
            this.gradingScaleButton.Click += new System.EventHandler(this.GradingScaleButton_Click);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Enabled = false;
            this.downButton.Image = global::Imedisoft.Properties.Resources.IconArrowDown;
            this.downButton.Location = new System.Drawing.Point(422, 244);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(30, 25);
            this.downButton.TabIndex = 13;
            this.downButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Enabled = false;
            this.upButton.Image = global::Imedisoft.Properties.Resources.IconArrowUp;
            this.upButton.Location = new System.Drawing.Point(422, 213);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(30, 25);
            this.upButton.TabIndex = 12;
            this.upButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // evaluationCriterionDefsGrid
            // 
            this.evaluationCriterionDefsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.evaluationCriterionDefsGrid.Location = new System.Drawing.Point(12, 93);
            this.evaluationCriterionDefsGrid.Name = "evaluationCriterionDefsGrid";
            this.evaluationCriterionDefsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.evaluationCriterionDefsGrid.Size = new System.Drawing.Size(404, 274);
            this.evaluationCriterionDefsGrid.TabIndex = 8;
            this.evaluationCriterionDefsGrid.Title = "Criteria Used";
            this.evaluationCriterionDefsGrid.TranslationName = "FormEvaluationDefEdit";
            this.evaluationCriterionDefsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.EvaluationCriterionDefsGrid_CellDoubleClick);
            this.evaluationCriterionDefsGrid.SelectionCommitted += new System.EventHandler(this.EvaluationCriterionDefsGrid_SelectionCommitted);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(286, 424);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 16;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(372, 424);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 17;
            this.cancelButton.Text = "&Cancel";
            // 
            // totalPointsLabel
            // 
            this.totalPointsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.totalPointsLabel.AutoSize = true;
            this.totalPointsLabel.Location = new System.Drawing.Point(287, 376);
            this.totalPointsLabel.Name = "totalPointsLabel";
            this.totalPointsLabel.Size = new System.Drawing.Size(63, 13);
            this.totalPointsLabel.TabIndex = 14;
            this.totalPointsLabel.Text = "Total Points";
            // 
            // totalPointsTextBox
            // 
            this.totalPointsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.totalPointsTextBox.Location = new System.Drawing.Point(356, 373);
            this.totalPointsTextBox.MaxLength = 255;
            this.totalPointsTextBox.Name = "totalPointsTextBox";
            this.totalPointsTextBox.ReadOnly = true;
            this.totalPointsTextBox.Size = new System.Drawing.Size(60, 20);
            this.totalPointsTextBox.TabIndex = 15;
            this.totalPointsTextBox.Text = "n/a";
            this.totalPointsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPenBlack;
            this.editButton.Location = new System.Drawing.Point(422, 124);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(30, 25);
            this.editButton.TabIndex = 10;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(422, 155);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 11;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormEvaluationDefEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(464, 461);
            this.Controls.Add(this.totalPointsLabel);
            this.Controls.Add(this.totalPointsTextBox);
            this.Controls.Add(this.schoolCourseLabel);
            this.Controls.Add(this.schoolCourseTextBox);
            this.Controls.Add(this.schoolCourseButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.gradingScaleLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.gradingScaleTextBox);
            this.Controls.Add(this.gradingScaleButton);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.evaluationCriterionDefsGrid);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(390, 372);
            this.Name = "FormEvaluationDefEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Evaluation Definition";
            this.Load += new System.EventHandler(this.FormEvaluationDefEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button downButton;
		private OpenDental.UI.Button upButton;
		private OpenDental.UI.ODGrid evaluationCriterionDefsGrid;
		private OpenDental.UI.Button gradingScaleButton;
		private System.Windows.Forms.TextBox gradingScaleTextBox;
		private OpenDental.UI.Button addButton;
		private System.Windows.Forms.Label gradingScaleLabel;
		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.TextBox titleTextBox;
		private System.Windows.Forms.Label schoolCourseLabel;
		private System.Windows.Forms.TextBox schoolCourseTextBox;
		private OpenDental.UI.Button schoolCourseButton;
		private System.Windows.Forms.Label totalPointsLabel;
		private System.Windows.Forms.TextBox totalPointsTextBox;
        private OpenDental.UI.Button editButton;
        private OpenDental.UI.Button deleteButton;
    }
}
