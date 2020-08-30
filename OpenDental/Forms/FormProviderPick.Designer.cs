namespace Imedisoft.Forms
{
    partial class FormProviderPick
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
            this.providersGrid = new OpenDental.UI.ODGrid();
            this.acceptButton = new OpenDental.UI.Button();
            this.providerIdLabel = new System.Windows.Forms.Label();
            this.providerIdTextBox = new System.Windows.Forms.TextBox();
            this.dentalSchoolGroupBox = new System.Windows.Forms.GroupBox();
            this.classLabel = new System.Windows.Forms.Label();
            this.classComboBox = new System.Windows.Forms.ComboBox();
            this.lastNameTextBox = new System.Windows.Forms.TextBox();
            this.lastNameLabel = new System.Windows.Forms.Label();
            this.firstNameTextBox = new System.Windows.Forms.TextBox();
            this.firstNameLabel = new System.Windows.Forms.Label();
            this.noneButton = new OpenDental.UI.Button();
            this.showAllCheckBox = new System.Windows.Forms.CheckBox();
            this.dentalSchoolGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(492, 604);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Cancel";
            // 
            // providersGrid
            // 
            this.providersGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.providersGrid.Location = new System.Drawing.Point(12, 12);
            this.providersGrid.Name = "providersGrid";
            this.providersGrid.Size = new System.Drawing.Size(354, 617);
            this.providersGrid.TabIndex = 4;
            this.providersGrid.Title = "Providers";
            this.providersGrid.TranslationName = "TableProviders";
            this.providersGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.ProvidersGrid_CellDoubleClick);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(492, 573);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 2;
            this.acceptButton.Text = "OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // providerIdLabel
            // 
            this.providerIdLabel.AutoSize = true;
            this.providerIdLabel.Location = new System.Drawing.Point(46, 29);
            this.providerIdLabel.Name = "providerIdLabel";
            this.providerIdLabel.Size = new System.Drawing.Size(18, 13);
            this.providerIdLabel.TabIndex = 0;
            this.providerIdLabel.Text = "ID";
            // 
            // providerIdTextBox
            // 
            this.providerIdTextBox.Location = new System.Drawing.Point(70, 26);
            this.providerIdTextBox.MaxLength = 15;
            this.providerIdTextBox.Name = "providerIdTextBox";
            this.providerIdTextBox.Size = new System.Drawing.Size(124, 20);
            this.providerIdTextBox.TabIndex = 1;
            // 
            // dentalSchoolGroupBox
            // 
            this.dentalSchoolGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dentalSchoolGroupBox.Controls.Add(this.classLabel);
            this.dentalSchoolGroupBox.Controls.Add(this.classComboBox);
            this.dentalSchoolGroupBox.Controls.Add(this.lastNameTextBox);
            this.dentalSchoolGroupBox.Controls.Add(this.lastNameLabel);
            this.dentalSchoolGroupBox.Controls.Add(this.firstNameTextBox);
            this.dentalSchoolGroupBox.Controls.Add(this.firstNameLabel);
            this.dentalSchoolGroupBox.Controls.Add(this.providerIdTextBox);
            this.dentalSchoolGroupBox.Controls.Add(this.providerIdLabel);
            this.dentalSchoolGroupBox.Location = new System.Drawing.Point(372, 42);
            this.dentalSchoolGroupBox.Name = "dentalSchoolGroupBox";
            this.dentalSchoolGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.dentalSchoolGroupBox.Size = new System.Drawing.Size(200, 150);
            this.dentalSchoolGroupBox.TabIndex = 0;
            this.dentalSchoolGroupBox.TabStop = false;
            this.dentalSchoolGroupBox.Text = "Dental School Filters";
            // 
            // classLabel
            // 
            this.classLabel.AutoSize = true;
            this.classLabel.Location = new System.Drawing.Point(32, 107);
            this.classLabel.Name = "classLabel";
            this.classLabel.Size = new System.Drawing.Size(32, 13);
            this.classLabel.TabIndex = 6;
            this.classLabel.Text = "Class";
            // 
            // classComboBox
            // 
            this.classComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classComboBox.FormattingEnabled = true;
            this.classComboBox.Location = new System.Drawing.Point(70, 104);
            this.classComboBox.Name = "classComboBox";
            this.classComboBox.Size = new System.Drawing.Size(124, 21);
            this.classComboBox.TabIndex = 7;
            this.classComboBox.SelectionChangeCommitted += new System.EventHandler(this.ClassComboBox_SelectionChangeCommitted);
            // 
            // lastNameTextBox
            // 
            this.lastNameTextBox.Location = new System.Drawing.Point(70, 52);
            this.lastNameTextBox.MaxLength = 15;
            this.lastNameTextBox.Name = "lastNameTextBox";
            this.lastNameTextBox.Size = new System.Drawing.Size(124, 20);
            this.lastNameTextBox.TabIndex = 3;
            // 
            // lastNameLabel
            // 
            this.lastNameLabel.AutoSize = true;
            this.lastNameLabel.Location = new System.Drawing.Point(7, 55);
            this.lastNameLabel.Name = "lastNameLabel";
            this.lastNameLabel.Size = new System.Drawing.Size(57, 13);
            this.lastNameLabel.TabIndex = 2;
            this.lastNameLabel.Text = "Last Name";
            // 
            // firstNameTextBox
            // 
            this.firstNameTextBox.Location = new System.Drawing.Point(70, 78);
            this.firstNameTextBox.MaxLength = 15;
            this.firstNameTextBox.Name = "firstNameTextBox";
            this.firstNameTextBox.Size = new System.Drawing.Size(124, 20);
            this.firstNameTextBox.TabIndex = 5;
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.Location = new System.Drawing.Point(6, 81);
            this.firstNameLabel.Name = "firstNameLabel";
            this.firstNameLabel.Size = new System.Drawing.Size(58, 13);
            this.firstNameLabel.TabIndex = 4;
            this.firstNameLabel.Text = "First Name";
            // 
            // noneButton
            // 
            this.noneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.noneButton.Location = new System.Drawing.Point(492, 515);
            this.noneButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.noneButton.Name = "noneButton";
            this.noneButton.Size = new System.Drawing.Size(80, 25);
            this.noneButton.TabIndex = 1;
            this.noneButton.Text = "None";
            this.noneButton.UseVisualStyleBackColor = true;
            this.noneButton.Click += new System.EventHandler(this.NoneButton_Click);
            // 
            // showAllCheckBox
            // 
            this.showAllCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showAllCheckBox.AutoSize = true;
            this.showAllCheckBox.Location = new System.Drawing.Point(372, 12);
            this.showAllCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.showAllCheckBox.Name = "showAllCheckBox";
            this.showAllCheckBox.Size = new System.Drawing.Size(66, 17);
            this.showAllCheckBox.TabIndex = 5;
            this.showAllCheckBox.Text = "Show All";
            this.showAllCheckBox.UseVisualStyleBackColor = true;
            this.showAllCheckBox.CheckedChanged += new System.EventHandler(this.ShowAllCheckBox_CheckedChanged);
            // 
            // FormProviderPick
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(584, 641);
            this.Controls.Add(this.showAllCheckBox);
            this.Controls.Add(this.noneButton);
            this.Controls.Add(this.dentalSchoolGroupBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.providersGrid);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProviderPick";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Providers";
            this.Load += new System.EventHandler(this.FormProviderSelect_Load);
            this.dentalSchoolGroupBox.ResumeLayout(false);
            this.dentalSchoolGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.ODGrid providersGrid;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.Label providerIdLabel;
		private System.Windows.Forms.TextBox providerIdTextBox;
		private System.Windows.Forms.GroupBox dentalSchoolGroupBox;
		private System.Windows.Forms.TextBox lastNameTextBox;
		private System.Windows.Forms.Label lastNameLabel;
		private System.Windows.Forms.CheckBox showAllCheckBox;
		private System.Windows.Forms.TextBox firstNameTextBox;
		private System.Windows.Forms.Label firstNameLabel;
		private System.Windows.Forms.Label classLabel;
		private System.Windows.Forms.ComboBox classComboBox;
		private OpenDental.UI.Button noneButton;
	}
}
