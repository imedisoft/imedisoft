namespace Imedisoft.Forms
{
    partial class FormDefinitions
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
            this.closeButton = new OpenDental.UI.Button();
            this.helpLabel = new System.Windows.Forms.Label();
            this.helpTextBox = new System.Windows.Forms.TextBox();
            this.editGroupBox = new System.Windows.Forms.GroupBox();
            this.butAlphabetize = new OpenDental.UI.Button();
            this.hideButton = new OpenDental.UI.Button();
            this.butDown = new OpenDental.UI.Button();
            this.upButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.categoryListBox = new System.Windows.Forms.ListBox();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.definitionsGrid = new OpenDental.UI.ODGrid();
            this.editGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(772, 604);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 6;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // helpLabel
            // 
            this.helpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpLabel.AutoSize = true;
            this.helpLabel.Location = new System.Drawing.Point(218, 553);
            this.helpLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(55, 13);
            this.helpLabel.TabIndex = 4;
            this.helpLabel.Text = "Guidelines";
            // 
            // helpTextBox
            // 
            this.helpTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.helpTextBox.Location = new System.Drawing.Point(218, 569);
            this.helpTextBox.Multiline = true;
            this.helpTextBox.Name = "helpTextBox";
            this.helpTextBox.ReadOnly = true;
            this.helpTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.helpTextBox.Size = new System.Drawing.Size(518, 60);
            this.helpTextBox.TabIndex = 5;
            // 
            // editGroupBox
            // 
            this.editGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editGroupBox.Controls.Add(this.butAlphabetize);
            this.editGroupBox.Controls.Add(this.hideButton);
            this.editGroupBox.Controls.Add(this.butDown);
            this.editGroupBox.Controls.Add(this.upButton);
            this.editGroupBox.Controls.Add(this.addButton);
            this.editGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.editGroupBox.Location = new System.Drawing.Point(218, 480);
            this.editGroupBox.Name = "editGroupBox";
            this.editGroupBox.Size = new System.Drawing.Size(634, 60);
            this.editGroupBox.TabIndex = 3;
            this.editGroupBox.TabStop = false;
            this.editGroupBox.Text = "Edit Items";
            // 
            // butAlphabetize
            // 
            this.butAlphabetize.Location = new System.Drawing.Point(254, 19);
            this.butAlphabetize.Name = "butAlphabetize";
            this.butAlphabetize.Size = new System.Drawing.Size(80, 25);
            this.butAlphabetize.TabIndex = 4;
            this.butAlphabetize.Text = "Alphabetize";
            this.butAlphabetize.Click += new System.EventHandler(this.AlphabetizeButton_Click);
            // 
            // hideButton
            // 
            this.hideButton.Location = new System.Drawing.Point(69, 19);
            this.hideButton.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.hideButton.Name = "hideButton";
            this.hideButton.Size = new System.Drawing.Size(80, 25);
            this.hideButton.TabIndex = 1;
            this.hideButton.Text = "&Hide";
            this.hideButton.Click += new System.EventHandler(this.HideButton_Click);
            // 
            // butDown
            // 
            this.butDown.Image = global::Imedisoft.Properties.Resources.IconArrowDown;
            this.butDown.Location = new System.Drawing.Point(218, 19);
            this.butDown.Name = "butDown";
            this.butDown.Size = new System.Drawing.Size(30, 25);
            this.butDown.TabIndex = 3;
            this.butDown.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // upButton
            // 
            this.upButton.Image = global::Imedisoft.Properties.Resources.IconArrowUp;
            this.upButton.Location = new System.Drawing.Point(182, 19);
            this.upButton.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(30, 25);
            this.upButton.TabIndex = 2;
            this.upButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // addButton
            // 
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(6, 19);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 0;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // categoryListBox
            // 
            this.categoryListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.categoryListBox.IntegralHeight = false;
            this.categoryListBox.Location = new System.Drawing.Point(12, 25);
            this.categoryListBox.Name = "categoryListBox";
            this.categoryListBox.Size = new System.Drawing.Size(200, 604);
            this.categoryListBox.Sorted = true;
            this.categoryListBox.TabIndex = 1;
            this.categoryListBox.SelectedIndexChanged += new System.EventHandler(this.CategoryListBox_SelectedIndexChanged);
            // 
            // categoryLabel
            // 
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.Location = new System.Drawing.Point(12, 9);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(84, 13);
            this.categoryLabel.TabIndex = 0;
            this.categoryLabel.Text = "Select Category";
            // 
            // definitionsGrid
            // 
            this.definitionsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.definitionsGrid.Location = new System.Drawing.Point(218, 25);
            this.definitionsGrid.Name = "definitionsGrid";
            this.definitionsGrid.Size = new System.Drawing.Size(634, 449);
            this.definitionsGrid.TabIndex = 2;
            this.definitionsGrid.Title = "Definitions";
            this.definitionsGrid.TranslationName = "TableDefinitions";
            this.definitionsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.DefinitionsGrid_CellDoubleClick);
            // 
            // FormDefinitions
            // 
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(864, 641);
            this.Controls.Add(this.definitionsGrid);
            this.Controls.Add(this.helpLabel);
            this.Controls.Add(this.helpTextBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.editGroupBox);
            this.Controls.Add(this.categoryListBox);
            this.Controls.Add(this.categoryLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "FormDefinitions";
            this.ShowInTaskbar = false;
            this.Text = "Definitions";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormDefinitions_Closing);
            this.Load += new System.EventHandler(this.FormDefinitions_Load);
            this.editGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button closeButton;
		private System.Windows.Forms.Label helpLabel;
		private System.Windows.Forms.TextBox helpTextBox;
		private System.Windows.Forms.GroupBox editGroupBox;
		private System.Windows.Forms.ListBox categoryListBox;
		private System.Windows.Forms.Label categoryLabel;
		private OpenDental.UI.Button addButton;
		private OpenDental.UI.Button upButton;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button hideButton;
		private OpenDental.UI.ODGrid definitionsGrid;
		private OpenDental.UI.Button butAlphabetize;
	}
}
