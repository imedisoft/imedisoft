namespace Imedisoft.Forms
{
	partial class FormRxNorms
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
            this.codeLabel = new System.Windows.Forms.Label();
            this.codeTextBox = new System.Windows.Forms.TextBox();
            this.rxNormsGrid = new OpenDental.UI.ODGrid();
            this.searchExactButton = new OpenDental.UI.Button();
            this.searchSimilarButton = new OpenDental.UI.Button();
            this.noneButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.ignoreCheckBox = new System.Windows.Forms.CheckBox();
            this.clearButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // codeLabel
            // 
            this.codeLabel.AutoSize = true;
            this.codeLabel.Location = new System.Drawing.Point(71, 15);
            this.codeLabel.Name = "codeLabel";
            this.codeLabel.Size = new System.Drawing.Size(101, 13);
            this.codeLabel.TabIndex = 1;
            this.codeLabel.Text = "Code or Description";
            // 
            // codeTextBox
            // 
            this.codeTextBox.Location = new System.Drawing.Point(178, 12);
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.Size = new System.Drawing.Size(100, 20);
            this.codeTextBox.TabIndex = 2;
            // 
            // rxNormsGrid
            // 
            this.rxNormsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rxNormsGrid.Location = new System.Drawing.Point(12, 38);
            this.rxNormsGrid.Name = "rxNormsGrid";
            this.rxNormsGrid.Size = new System.Drawing.Size(650, 580);
            this.rxNormsGrid.TabIndex = 7;
            this.rxNormsGrid.Title = "RxNorm Codes";
            this.rxNormsGrid.TranslationName = "TableCodes";
            this.rxNormsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.RxNormsGrid_CellDoubleClick);
            // 
            // searchExactButton
            // 
            this.searchExactButton.Location = new System.Drawing.Point(360, 12);
            this.searchExactButton.Name = "searchExactButton";
            this.searchExactButton.Size = new System.Drawing.Size(70, 20);
            this.searchExactButton.TabIndex = 4;
            this.searchExactButton.Text = "Exact";
            this.searchExactButton.Click += new System.EventHandler(this.SearchExactButton_Click);
            // 
            // searchSimilarButton
            // 
            this.searchSimilarButton.Location = new System.Drawing.Point(284, 12);
            this.searchSimilarButton.Name = "searchSimilarButton";
            this.searchSimilarButton.Size = new System.Drawing.Size(70, 20);
            this.searchSimilarButton.TabIndex = 3;
            this.searchSimilarButton.Text = "Similar";
            this.searchSimilarButton.Click += new System.EventHandler(this.SearchSimilarButton_Click);
            // 
            // noneButton
            // 
            this.noneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.noneButton.Location = new System.Drawing.Point(12, 624);
            this.noneButton.Name = "noneButton";
            this.noneButton.Size = new System.Drawing.Size(80, 25);
            this.noneButton.TabIndex = 8;
            this.noneButton.Text = "&None";
            this.noneButton.Click += new System.EventHandler(this.NoneButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(496, 624);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 9;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(582, 624);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // ignoreCheckBox
            // 
            this.ignoreCheckBox.AutoSize = true;
            this.ignoreCheckBox.Location = new System.Drawing.Point(515, 14);
            this.ignoreCheckBox.Name = "ignoreCheckBox";
            this.ignoreCheckBox.Size = new System.Drawing.Size(103, 17);
            this.ignoreCheckBox.TabIndex = 6;
            this.ignoreCheckBox.Text = "Ignore Numbers";
            this.ignoreCheckBox.UseVisualStyleBackColor = true;
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(436, 12);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(70, 20);
            this.clearButton.TabIndex = 5;
            this.clearButton.Text = "Clear";
            this.clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // FormRxNorms
            // 
            this.AcceptButton = this.searchSimilarButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(674, 661);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.ignoreCheckBox);
            this.Controls.Add(this.searchExactButton);
            this.Controls.Add(this.searchSimilarButton);
            this.Controls.Add(this.codeLabel);
            this.Controls.Add(this.codeTextBox);
            this.Controls.Add(this.rxNormsGrid);
            this.Controls.Add(this.noneButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 520);
            this.Name = "FormRxNorms";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RxNorms";
            this.Load += new System.EventHandler(this.FormRxNorms_Load);
            this.Shown += new System.EventHandler(this.FormRxNorms_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button noneButton;
		private OpenDental.UI.ODGrid rxNormsGrid;
		private OpenDental.UI.Button searchSimilarButton;
		private System.Windows.Forms.Label codeLabel;
		private System.Windows.Forms.TextBox codeTextBox;
		private OpenDental.UI.Button searchExactButton;
		private System.Windows.Forms.CheckBox ignoreCheckBox;
		private OpenDental.UI.Button clearButton;
	}
}
