namespace Imedisoft.Forms
{
    partial class FormProblemDefinitions
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
            this.searchGroupBox = new System.Windows.Forms.GroupBox();
            this.showHiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.snomedTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.snomedLabel = new System.Windows.Forms.Label();
            this.icd10Label = new System.Windows.Forms.Label();
            this.icd10TextBox = new System.Windows.Forms.TextBox();
            this.icd9Label = new System.Windows.Forms.Label();
            this.icd9TextBox = new System.Windows.Forms.TextBox();
            this.problemsGrid = new OpenDental.UI.ODGrid();
            this.addButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.searchGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchGroupBox
            // 
            this.searchGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchGroupBox.Controls.Add(this.showHiddenCheckBox);
            this.searchGroupBox.Controls.Add(this.descriptionLabel);
            this.searchGroupBox.Controls.Add(this.snomedTextBox);
            this.searchGroupBox.Controls.Add(this.descriptionTextBox);
            this.searchGroupBox.Controls.Add(this.snomedLabel);
            this.searchGroupBox.Controls.Add(this.icd10Label);
            this.searchGroupBox.Controls.Add(this.icd10TextBox);
            this.searchGroupBox.Controls.Add(this.icd9Label);
            this.searchGroupBox.Controls.Add(this.icd9TextBox);
            this.searchGroupBox.Location = new System.Drawing.Point(12, 12);
            this.searchGroupBox.Name = "searchGroupBox";
            this.searchGroupBox.Size = new System.Drawing.Size(634, 120);
            this.searchGroupBox.TabIndex = 1;
            this.searchGroupBox.TabStop = false;
            this.searchGroupBox.Text = "Search";
            // 
            // showHiddenCheckBox
            // 
            this.showHiddenCheckBox.AutoSize = true;
            this.showHiddenCheckBox.Checked = true;
            this.showHiddenCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showHiddenCheckBox.Location = new System.Drawing.Point(100, 97);
            this.showHiddenCheckBox.Name = "showHiddenCheckBox";
            this.showHiddenCheckBox.Size = new System.Drawing.Size(88, 17);
            this.showHiddenCheckBox.TabIndex = 8;
            this.showHiddenCheckBox.Text = "Show Hidden";
            this.showHiddenCheckBox.CheckedChanged += new System.EventHandler(this.ShowHiddenCheckBox_CheckedChanged);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(34, 73);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 6;
            this.descriptionLabel.Text = "Description";
            // 
            // snomedTextBox
            // 
            this.snomedTextBox.AcceptsTab = true;
            this.snomedTextBox.Location = new System.Drawing.Point(100, 45);
            this.snomedTextBox.Name = "snomedTextBox";
            this.snomedTextBox.Size = new System.Drawing.Size(250, 20);
            this.snomedTextBox.TabIndex = 5;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.AcceptsTab = true;
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Location = new System.Drawing.Point(100, 71);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(528, 20);
            this.descriptionTextBox.TabIndex = 7;
            // 
            // snomedLabel
            // 
            this.snomedLabel.AutoSize = true;
            this.snomedLabel.Location = new System.Drawing.Point(45, 47);
            this.snomedLabel.Name = "snomedLabel";
            this.snomedLabel.Size = new System.Drawing.Size(49, 13);
            this.snomedLabel.TabIndex = 4;
            this.snomedLabel.Text = "SNOMED";
            // 
            // icd10Label
            // 
            this.icd10Label.AutoSize = true;
            this.icd10Label.Location = new System.Drawing.Point(257, 23);
            this.icd10Label.Name = "icd10Label";
            this.icd10Label.Size = new System.Drawing.Size(37, 13);
            this.icd10Label.TabIndex = 2;
            this.icd10Label.Text = "ICD10";
            // 
            // icd10TextBox
            // 
            this.icd10TextBox.AcceptsTab = true;
            this.icd10TextBox.Location = new System.Drawing.Point(300, 19);
            this.icd10TextBox.Name = "icd10TextBox";
            this.icd10TextBox.Size = new System.Drawing.Size(140, 20);
            this.icd10TextBox.TabIndex = 3;
            // 
            // icd9Label
            // 
            this.icd9Label.AutoSize = true;
            this.icd9Label.Location = new System.Drawing.Point(63, 23);
            this.icd9Label.Name = "icd9Label";
            this.icd9Label.Size = new System.Drawing.Size(31, 13);
            this.icd9Label.TabIndex = 0;
            this.icd9Label.Text = "ICD9";
            // 
            // icd9TextBox
            // 
            this.icd9TextBox.AcceptsTab = true;
            this.icd9TextBox.Location = new System.Drawing.Point(100, 19);
            this.icd9TextBox.Name = "icd9TextBox";
            this.icd9TextBox.Size = new System.Drawing.Size(140, 20);
            this.icd9TextBox.TabIndex = 1;
            // 
            // problemsGrid
            // 
            this.problemsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.problemsGrid.Location = new System.Drawing.Point(12, 138);
            this.problemsGrid.Name = "problemsGrid";
            this.problemsGrid.Size = new System.Drawing.Size(634, 461);
            this.problemsGrid.TabIndex = 2;
            this.problemsGrid.Title = null;
            this.problemsGrid.TranslationName = "TableProblems";
            this.problemsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.ProblemsGrid_CellDoubleClick);
            this.problemsGrid.SelectionCommitted += new System.EventHandler(this.ProblemsGrid_SelectionCommitted);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(652, 137);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 25);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "&Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(652, 543);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 7;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(652, 574);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Close";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(652, 168);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormDiseaseDefs
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(744, 611);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.searchGroupBox);
            this.Controls.Add(this.problemsGrid);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDiseaseDefs";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Problems";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProblemDefinitions_FormClosing);
            this.Load += new System.EventHandler(this.FormProblemDefinitions_Load);
            this.searchGroupBox.ResumeLayout(false);
            this.searchGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.Button acceptButton;
        private OpenDental.UI.ODGrid problemsGrid;
		private System.Windows.Forms.GroupBox searchGroupBox;
		private System.Windows.Forms.TextBox snomedTextBox;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.Label snomedLabel;
		private System.Windows.Forms.Label icd10Label;
		private System.Windows.Forms.TextBox icd10TextBox;
		private System.Windows.Forms.Label icd9Label;
		private System.Windows.Forms.TextBox icd9TextBox;
		private System.Windows.Forms.Label descriptionLabel;
		private OpenDental.UI.Button addButton;
		private System.Windows.Forms.CheckBox showHiddenCheckBox;
        private OpenDental.UI.Button deleteButton;
    }
}
