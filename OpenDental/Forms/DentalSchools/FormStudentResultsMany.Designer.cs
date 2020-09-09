namespace Imedisoft.Forms
{
    partial class FormStudentResultsMany
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
            this.studentsGrid = new OpenDental.UI.ODGrid();
            this.cancelButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // courseLabel
            // 
            this.courseLabel.AutoSize = true;
            this.courseLabel.Location = new System.Drawing.Point(255, 9);
            this.courseLabel.Name = "courseLabel";
            this.courseLabel.Size = new System.Drawing.Size(41, 13);
            this.courseLabel.TabIndex = 3;
            this.courseLabel.Text = "Course";
            // 
            // courseComboBox
            // 
            this.courseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.courseComboBox.FormattingEnabled = true;
            this.courseComboBox.Location = new System.Drawing.Point(258, 25);
            this.courseComboBox.Name = "courseComboBox";
            this.courseComboBox.Size = new System.Drawing.Size(240, 21);
            this.courseComboBox.TabIndex = 4;
            this.courseComboBox.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommitted);
            // 
            // classLabel
            // 
            this.classLabel.AutoSize = true;
            this.classLabel.Location = new System.Drawing.Point(12, 9);
            this.classLabel.Name = "classLabel";
            this.classLabel.Size = new System.Drawing.Size(32, 13);
            this.classLabel.TabIndex = 1;
            this.classLabel.Text = "Class";
            // 
            // classComboBox
            // 
            this.classComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classComboBox.FormattingEnabled = true;
            this.classComboBox.Location = new System.Drawing.Point(12, 25);
            this.classComboBox.Name = "classComboBox";
            this.classComboBox.Size = new System.Drawing.Size(240, 21);
            this.classComboBox.TabIndex = 2;
            this.classComboBox.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommitted);
            // 
            // studentsGrid
            // 
            this.studentsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.studentsGrid.Location = new System.Drawing.Point(12, 52);
            this.studentsGrid.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.studentsGrid.Name = "studentsGrid";
            this.studentsGrid.Size = new System.Drawing.Size(660, 439);
            this.studentsGrid.TabIndex = 5;
            this.studentsGrid.Title = "Student Requirements";
            this.studentsGrid.TranslationName = "TableReqStudentMany";
            this.studentsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.StudentsGrid_CellDoubleClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(592, 504);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Close";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // FormReqStudentsMany
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(684, 541);
            this.Controls.Add(this.courseLabel);
            this.Controls.Add(this.courseComboBox);
            this.Controls.Add(this.classLabel);
            this.Controls.Add(this.classComboBox);
            this.Controls.Add(this.studentsGrid);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "FormReqStudentsMany";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Student Results";
            this.Load += new System.EventHandler(this.FormStudentResultsMany_Load);
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
	}
}
