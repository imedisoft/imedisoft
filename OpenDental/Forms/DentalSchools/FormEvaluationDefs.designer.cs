namespace Imedisoft.Forms
{
	partial class FormEvaluationDefs
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
            this.duplicateButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.evaluationDefsGrid = new OpenDental.UI.ODGrid();
            this.schoolCourseComboBox = new System.Windows.Forms.ComboBox();
            this.schoolCourseLabel = new System.Windows.Forms.Label();
            this.editButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(432, 424);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Close";
            // 
            // duplicateButton
            // 
            this.duplicateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.duplicateButton.Location = new System.Drawing.Point(147, 424);
            this.duplicateButton.Name = "duplicateButton";
            this.duplicateButton.Size = new System.Drawing.Size(80, 25);
            this.duplicateButton.TabIndex = 7;
            this.duplicateButton.Text = "Duplicate";
            this.duplicateButton.Click += new System.EventHandler(this.DuplicateButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(12, 424);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 4;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // evaluationDefsGrid
            // 
            this.evaluationDefsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.evaluationDefsGrid.Location = new System.Drawing.Point(12, 39);
            this.evaluationDefsGrid.Name = "evaluationDefsGrid";
            this.evaluationDefsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.evaluationDefsGrid.Size = new System.Drawing.Size(500, 379);
            this.evaluationDefsGrid.TabIndex = 3;
            this.evaluationDefsGrid.Title = "Evaluation Definitions";
            this.evaluationDefsGrid.TranslationName = "TableEvaluationSetup";
            this.evaluationDefsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.EvaluationDefsGrid_CellDoubleClick);
            this.evaluationDefsGrid.SelectionCommitted += new System.EventHandler(this.EvaluationDefsGrid_SelectionCommitted);
            // 
            // schoolCourseComboBox
            // 
            this.schoolCourseComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.schoolCourseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.schoolCourseComboBox.FormattingEnabled = true;
            this.schoolCourseComboBox.ItemHeight = 13;
            this.schoolCourseComboBox.Location = new System.Drawing.Point(100, 12);
            this.schoolCourseComboBox.Name = "schoolCourseComboBox";
            this.schoolCourseComboBox.Size = new System.Drawing.Size(160, 21);
            this.schoolCourseComboBox.TabIndex = 2;
            this.schoolCourseComboBox.SelectionChangeCommitted += new System.EventHandler(this.SchoolCourseComboBox_SelectionChangeCommitted);
            // 
            // schoolCourseLabel
            // 
            this.schoolCourseLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.schoolCourseLabel.AutoSize = true;
            this.schoolCourseLabel.Location = new System.Drawing.Point(53, 15);
            this.schoolCourseLabel.Name = "schoolCourseLabel";
            this.schoolCourseLabel.Size = new System.Drawing.Size(41, 13);
            this.schoolCourseLabel.TabIndex = 1;
            this.schoolCourseLabel.Text = "Course";
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPenBlack;
            this.editButton.Location = new System.Drawing.Point(48, 424);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(30, 25);
            this.editButton.TabIndex = 5;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(84, 424);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 6;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormEvaluationDefs
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(524, 461);
            this.Controls.Add(this.schoolCourseComboBox);
            this.Controls.Add(this.schoolCourseLabel);
            this.Controls.Add(this.evaluationDefsGrid);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.duplicateButton);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(420, 420);
            this.Name = "FormEvaluationDefs";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Evaluation Definitions";
            this.Load += new System.EventHandler(this.FormEvaluationDefs_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button duplicateButton;
		private OpenDental.UI.Button addButton;
		private OpenDental.UI.ODGrid evaluationDefsGrid;
		private System.Windows.Forms.ComboBox schoolCourseComboBox;
		private System.Windows.Forms.Label schoolCourseLabel;
        private OpenDental.UI.Button editButton;
        private OpenDental.UI.Button deleteButton;
    }
}
