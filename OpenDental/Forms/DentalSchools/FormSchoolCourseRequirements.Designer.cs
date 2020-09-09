namespace Imedisoft.Forms
{
    partial class FormSchoolCourseRequirements
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
            this.classLabel = new System.Windows.Forms.Label();
            this.classComboBox = new System.Windows.Forms.ComboBox();
            this.courseLabel = new System.Windows.Forms.Label();
            this.courseComboBox = new System.Windows.Forms.ComboBox();
            this.copyToGroupBox = new System.Windows.Forms.GroupBox();
            this.copyButton = new OpenDental.UI.Button();
            this.copyToCourseLabel = new System.Windows.Forms.Label();
            this.copyToClassComboBox = new System.Windows.Forms.ComboBox();
            this.copyToCourseComboBox = new System.Windows.Forms.ComboBox();
            this.copyToClassLabel = new System.Windows.Forms.Label();
            this.selectedGroupBox = new System.Windows.Forms.GroupBox();
            this.addButton = new OpenDental.UI.Button();
            this.requirementsGrid = new OpenDental.UI.ODGrid();
            this.acceptButton = new OpenDental.UI.Button();
            this.deleteAllButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.editButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.copyToGroupBox.SuspendLayout();
            this.selectedGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // classLabel
            // 
            this.classLabel.AutoSize = true;
            this.classLabel.Location = new System.Drawing.Point(42, 29);
            this.classLabel.Name = "classLabel";
            this.classLabel.Size = new System.Drawing.Size(32, 13);
            this.classLabel.TabIndex = 0;
            this.classLabel.Text = "Class";
            // 
            // classComboBox
            // 
            this.classComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classComboBox.FormattingEnabled = true;
            this.classComboBox.Location = new System.Drawing.Point(80, 26);
            this.classComboBox.Name = "classComboBox";
            this.classComboBox.Size = new System.Drawing.Size(224, 21);
            this.classComboBox.TabIndex = 1;
            this.classComboBox.SelectionChangeCommitted += new System.EventHandler(this.ClassComboBox_SelectionChangeCommitted);
            // 
            // courseLabel
            // 
            this.courseLabel.AutoSize = true;
            this.courseLabel.Location = new System.Drawing.Point(33, 56);
            this.courseLabel.Name = "courseLabel";
            this.courseLabel.Size = new System.Drawing.Size(41, 13);
            this.courseLabel.TabIndex = 2;
            this.courseLabel.Text = "Course";
            // 
            // courseComboBox
            // 
            this.courseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.courseComboBox.FormattingEnabled = true;
            this.courseComboBox.Location = new System.Drawing.Point(80, 53);
            this.courseComboBox.Name = "courseComboBox";
            this.courseComboBox.Size = new System.Drawing.Size(224, 21);
            this.courseComboBox.TabIndex = 3;
            this.courseComboBox.SelectionChangeCommitted += new System.EventHandler(this.CourseComboBox_SelectionChangeCommitted);
            // 
            // copyToGroupBox
            // 
            this.copyToGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copyToGroupBox.Controls.Add(this.copyButton);
            this.copyToGroupBox.Controls.Add(this.copyToCourseLabel);
            this.copyToGroupBox.Controls.Add(this.copyToClassComboBox);
            this.copyToGroupBox.Controls.Add(this.copyToCourseComboBox);
            this.copyToGroupBox.Controls.Add(this.copyToClassLabel);
            this.copyToGroupBox.Location = new System.Drawing.Point(562, 138);
            this.copyToGroupBox.Name = "copyToGroupBox";
            this.copyToGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.copyToGroupBox.Size = new System.Drawing.Size(310, 120);
            this.copyToGroupBox.TabIndex = 6;
            this.copyToGroupBox.TabStop = false;
            this.copyToGroupBox.Text = "Copy Requirements To";
            // 
            // copyButton
            // 
            this.copyButton.Location = new System.Drawing.Point(224, 80);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(80, 25);
            this.copyButton.TabIndex = 4;
            this.copyButton.Text = "Copy";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.CopyToButton_Click);
            // 
            // copyToCourseLabel
            // 
            this.copyToCourseLabel.AutoSize = true;
            this.copyToCourseLabel.Location = new System.Drawing.Point(33, 56);
            this.copyToCourseLabel.Name = "copyToCourseLabel";
            this.copyToCourseLabel.Size = new System.Drawing.Size(41, 13);
            this.copyToCourseLabel.TabIndex = 2;
            this.copyToCourseLabel.Text = "Course";
            // 
            // copyToClassComboBox
            // 
            this.copyToClassComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.copyToClassComboBox.FormattingEnabled = true;
            this.copyToClassComboBox.Location = new System.Drawing.Point(80, 26);
            this.copyToClassComboBox.Name = "copyToClassComboBox";
            this.copyToClassComboBox.Size = new System.Drawing.Size(224, 21);
            this.copyToClassComboBox.TabIndex = 1;
            // 
            // copyToCourseComboBox
            // 
            this.copyToCourseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.copyToCourseComboBox.FormattingEnabled = true;
            this.copyToCourseComboBox.Location = new System.Drawing.Point(80, 53);
            this.copyToCourseComboBox.Name = "copyToCourseComboBox";
            this.copyToCourseComboBox.Size = new System.Drawing.Size(224, 21);
            this.copyToCourseComboBox.TabIndex = 3;
            // 
            // copyToClassLabel
            // 
            this.copyToClassLabel.AutoSize = true;
            this.copyToClassLabel.Location = new System.Drawing.Point(42, 29);
            this.copyToClassLabel.Name = "copyToClassLabel";
            this.copyToClassLabel.Size = new System.Drawing.Size(32, 13);
            this.copyToClassLabel.TabIndex = 0;
            this.copyToClassLabel.Text = "Class";
            // 
            // selectedGroupBox
            // 
            this.selectedGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedGroupBox.Controls.Add(this.classComboBox);
            this.selectedGroupBox.Controls.Add(this.classLabel);
            this.selectedGroupBox.Controls.Add(this.courseComboBox);
            this.selectedGroupBox.Controls.Add(this.addButton);
            this.selectedGroupBox.Controls.Add(this.courseLabel);
            this.selectedGroupBox.Location = new System.Drawing.Point(562, 12);
            this.selectedGroupBox.Name = "selectedGroupBox";
            this.selectedGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.selectedGroupBox.Size = new System.Drawing.Size(310, 120);
            this.selectedGroupBox.TabIndex = 5;
            this.selectedGroupBox.TabStop = false;
            this.selectedGroupBox.Text = "Selected Class/Course";
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(224, 80);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 25);
            this.addButton.TabIndex = 4;
            this.addButton.Text = "&Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // requirementsGrid
            // 
            this.requirementsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.requirementsGrid.Location = new System.Drawing.Point(12, 12);
            this.requirementsGrid.Name = "requirementsGrid";
            this.requirementsGrid.Size = new System.Drawing.Size(544, 526);
            this.requirementsGrid.TabIndex = 1;
            this.requirementsGrid.Title = "Requirements";
            this.requirementsGrid.TranslationName = "TableRequirementsNeeded";
            this.requirementsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.RequirementsGrid_CellDoubleClick);
            this.requirementsGrid.SelectionCommitted += new System.EventHandler(this.RequirementsGrid_SelectionCommitted);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(792, 513);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 7;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // deleteAllButton
            // 
            this.deleteAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteAllButton.Enabled = false;
            this.deleteAllButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteAllButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteAllButton.Location = new System.Drawing.Point(111, 544);
            this.deleteAllButton.Name = "deleteAllButton";
            this.deleteAllButton.Size = new System.Drawing.Size(100, 25);
            this.deleteAllButton.TabIndex = 4;
            this.deleteAllButton.Text = "Delete All";
            this.deleteAllButton.Click += new System.EventHandler(this.DeleteAllButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(792, 544);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPenBlack;
            this.editButton.Location = new System.Drawing.Point(12, 544);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(30, 25);
            this.editButton.TabIndex = 2;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(48, 544);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 3;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormReqNeededs
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(884, 581);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.deleteAllButton);
            this.Controls.Add(this.selectedGroupBox);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.copyToGroupBox);
            this.Controls.Add(this.requirementsGrid);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(900, 620);
            this.Name = "FormReqNeededs";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "School Course Requirements";
            this.Load += new System.EventHandler(this.FormSchoolCourseRequirements_Load);
            this.copyToGroupBox.ResumeLayout(false);
            this.copyToGroupBox.PerformLayout();
            this.selectedGroupBox.ResumeLayout(false);
            this.selectedGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button addButton;
		private OpenDental.UI.ODGrid requirementsGrid;
		private System.Windows.Forms.ComboBox classComboBox;
		private System.Windows.Forms.Label classLabel;
		private System.Windows.Forms.Label courseLabel;
		private System.Windows.Forms.ComboBox courseComboBox;
		private System.Windows.Forms.GroupBox copyToGroupBox;
		private System.Windows.Forms.Label copyToCourseLabel;
		private System.Windows.Forms.ComboBox copyToClassComboBox;
		private System.Windows.Forms.ComboBox copyToCourseComboBox;
		private System.Windows.Forms.Label copyToClassLabel;
		private OpenDental.UI.Button deleteAllButton;
		private System.Windows.Forms.GroupBox selectedGroupBox;
		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button copyButton;
        private OpenDental.UI.Button editButton;
        private OpenDental.UI.Button deleteButton;
    }
}
