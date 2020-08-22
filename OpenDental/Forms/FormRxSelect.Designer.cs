namespace Imedisoft.Forms
{
    partial class FormRxSelect
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
            this.acceptButton = new OpenDental.UI.Button();
            this.instructionsLabel = new System.Windows.Forms.Label();
            this.blankButton = new OpenDental.UI.Button();
            this.rxDefsGrid = new OpenDental.UI.ODGrid();
            this.searchGroupBox = new System.Windows.Forms.GroupBox();
            this.searchButton = new OpenDental.UI.Button();
            this.dispLabel = new System.Windows.Forms.Label();
            this.drugLabel = new System.Windows.Forms.Label();
            this.checkControlledOnly = new System.Windows.Forms.CheckBox();
            this.dispTextBox = new System.Windows.Forms.TextBox();
            this.drugTextBox = new System.Windows.Forms.TextBox();
            this.searchGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(872, 604);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(786, 604);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // instructionsLabel
            // 
            this.instructionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.instructionsLabel.AutoSize = true;
            this.instructionsLabel.Location = new System.Drawing.Point(12, 610);
            this.instructionsLabel.Name = "instructionsLabel";
            this.instructionsLabel.Size = new System.Drawing.Size(422, 13);
            this.instructionsLabel.TabIndex = 2;
            this.instructionsLabel.Text = "Please select a Prescription from the list or click Blank to start with a blank p" +
    "rescription.";
            // 
            // blankButton
            // 
            this.blankButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.blankButton.Location = new System.Drawing.Point(450, 604);
            this.blankButton.Name = "blankButton";
            this.blankButton.Size = new System.Drawing.Size(80, 25);
            this.blankButton.TabIndex = 3;
            this.blankButton.Text = "&Blank";
            this.blankButton.Click += new System.EventHandler(this.BlankButton_Click);
            // 
            // rxDefsGrid
            // 
            this.rxDefsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rxDefsGrid.Location = new System.Drawing.Point(12, 12);
            this.rxDefsGrid.Name = "rxDefsGrid";
            this.rxDefsGrid.Size = new System.Drawing.Size(734, 586);
            this.rxDefsGrid.TabIndex = 0;
            this.rxDefsGrid.Title = "Prescriptions";
            this.rxDefsGrid.TranslationName = "TableRxSetup";
            this.rxDefsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.RxDefsGrid_CellDoubleClick);
            // 
            // searchGroupBox
            // 
            this.searchGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchGroupBox.Controls.Add(this.searchButton);
            this.searchGroupBox.Controls.Add(this.dispLabel);
            this.searchGroupBox.Controls.Add(this.drugLabel);
            this.searchGroupBox.Controls.Add(this.checkControlledOnly);
            this.searchGroupBox.Controls.Add(this.dispTextBox);
            this.searchGroupBox.Controls.Add(this.drugTextBox);
            this.searchGroupBox.Location = new System.Drawing.Point(752, 12);
            this.searchGroupBox.Name = "searchGroupBox";
            this.searchGroupBox.Size = new System.Drawing.Size(200, 173);
            this.searchGroupBox.TabIndex = 1;
            this.searchGroupBox.TabStop = false;
            this.searchGroupBox.Text = "Search";
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Location = new System.Drawing.Point(114, 142);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(80, 25);
            this.searchButton.TabIndex = 5;
            this.searchButton.Text = "Search";
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // dispLabel
            // 
            this.dispLabel.AutoSize = true;
            this.dispLabel.Location = new System.Drawing.Point(6, 70);
            this.dispLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.dispLabel.Name = "dispLabel";
            this.dispLabel.Size = new System.Drawing.Size(27, 13);
            this.dispLabel.TabIndex = 2;
            this.dispLabel.Text = "Disp";
            // 
            // drugLabel
            // 
            this.drugLabel.AutoSize = true;
            this.drugLabel.Location = new System.Drawing.Point(6, 26);
            this.drugLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.drugLabel.Name = "drugLabel";
            this.drugLabel.Size = new System.Drawing.Size(30, 13);
            this.drugLabel.TabIndex = 0;
            this.drugLabel.Text = "Drug";
            // 
            // checkControlledOnly
            // 
            this.checkControlledOnly.AutoSize = true;
            this.checkControlledOnly.Location = new System.Drawing.Point(6, 114);
            this.checkControlledOnly.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.checkControlledOnly.Name = "checkControlledOnly";
            this.checkControlledOnly.Size = new System.Drawing.Size(100, 17);
            this.checkControlledOnly.TabIndex = 4;
            this.checkControlledOnly.Text = "Controlled Only";
            this.checkControlledOnly.UseVisualStyleBackColor = true;
            // 
            // dispTextBox
            // 
            this.dispTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dispTextBox.Location = new System.Drawing.Point(6, 86);
            this.dispTextBox.Name = "dispTextBox";
            this.dispTextBox.Size = new System.Drawing.Size(188, 20);
            this.dispTextBox.TabIndex = 3;
            // 
            // drugTextBox
            // 
            this.drugTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.drugTextBox.Location = new System.Drawing.Point(6, 42);
            this.drugTextBox.Name = "drugTextBox";
            this.drugTextBox.Size = new System.Drawing.Size(188, 20);
            this.drugTextBox.TabIndex = 1;
            // 
            // FormRxSelect
            // 
            this.AcceptButton = this.searchButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(964, 641);
            this.Controls.Add(this.searchGroupBox);
            this.Controls.Add(this.rxDefsGrid);
            this.Controls.Add(this.blankButton);
            this.Controls.Add(this.instructionsLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(860, 480);
            this.Name = "FormRxSelect";
            this.ShowInTaskbar = false;
            this.Text = "Select Prescription";
            this.Load += new System.EventHandler(this.FormRxSelect_Load);
            this.searchGroupBox.ResumeLayout(false);
            this.searchGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.Label instructionsLabel;
		private OpenDental.UI.Button blankButton;
		private OpenDental.UI.ODGrid rxDefsGrid;
		private System.Windows.Forms.GroupBox searchGroupBox;
		private System.Windows.Forms.Label dispLabel;
		private System.Windows.Forms.Label drugLabel;
		private System.Windows.Forms.CheckBox checkControlledOnly;
		private System.Windows.Forms.TextBox dispTextBox;
		private System.Windows.Forms.TextBox drugTextBox;
		private OpenDental.UI.Button searchButton;
	}
}
