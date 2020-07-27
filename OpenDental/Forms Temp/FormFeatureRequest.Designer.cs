namespace Imedisoft.Forms
{
    partial class FormFeatureRequest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFeatureRequest));
            this.voteLabel = new System.Windows.Forms.Label();
            this.searchLabel = new System.Windows.Forms.Label();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.searchFirstLabel = new System.Windows.Forms.Label();
            this.searchButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.requestsGrid = new OpenDental.UI.ODGrid();
            this.cancelButton = new OpenDental.UI.Button();
            this.editButton = new OpenDental.UI.Button();
            this.mineCheckBox = new System.Windows.Forms.CheckBox();
            this.myVotesCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // voteLabel
            // 
            this.voteLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.voteLabel.Location = new System.Drawing.Point(12, 9);
            this.voteLabel.Name = "voteLabel";
            this.voteLabel.Size = new System.Drawing.Size(940, 20);
            this.voteLabel.TabIndex = 0;
            this.voteLabel.Text = "Vote for your favorite features here. Please remember that we cannot ever give an" +
    "y time estimates.";
            this.voteLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Location = new System.Drawing.Point(23, 35);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(72, 13);
            this.searchLabel.TabIndex = 1;
            this.searchLabel.Text = "Search Terms";
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(101, 32);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(167, 20);
            this.searchTextBox.TabIndex = 2;
            // 
            // searchFirstLabel
            // 
            this.searchFirstLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.searchFirstLabel.AutoSize = true;
            this.searchFirstLabel.Location = new System.Drawing.Point(98, 630);
            this.searchFirstLabel.Name = "searchFirstLabel";
            this.searchFirstLabel.Size = new System.Drawing.Size(124, 13);
            this.searchFirstLabel.TabIndex = 8;
            this.searchFirstLabel.Text = "A search is required first";
            this.searchFirstLabel.Visible = false;
            // 
            // searchButton
            // 
            this.searchButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.searchButton.Location = new System.Drawing.Point(274, 29);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(80, 25);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = "Search";
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.Add;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(12, 624);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 25);
            this.addButton.TabIndex = 7;
            this.addButton.Text = "Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // requestsGrid
            // 
            this.requestsGrid.AllowSortingByColumn = true;
            this.requestsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.requestsGrid.HasMultilineHeaders = true;
            this.requestsGrid.Location = new System.Drawing.Point(12, 60);
            this.requestsGrid.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.requestsGrid.Name = "requestsGrid";
            this.requestsGrid.Size = new System.Drawing.Size(940, 551);
            this.requestsGrid.TabIndex = 6;
            this.requestsGrid.Title = "Feature Requests";
            this.requestsGrid.TranslationName = "TableRequests";
            this.requestsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.RequestsGrid_CellDoubleClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(872, 624);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "&Close";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editButton.Location = new System.Drawing.Point(228, 624);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(80, 25);
            this.editButton.TabIndex = 9;
            this.editButton.Text = "Edit";
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // mineCheckBox
            // 
            this.mineCheckBox.AutoSize = true;
            this.mineCheckBox.Location = new System.Drawing.Point(360, 34);
            this.mineCheckBox.Name = "mineCheckBox";
            this.mineCheckBox.Size = new System.Drawing.Size(48, 17);
            this.mineCheckBox.TabIndex = 4;
            this.mineCheckBox.Text = "Mine";
            this.mineCheckBox.UseVisualStyleBackColor = true;
            this.mineCheckBox.CheckedChanged += new System.EventHandler(this.MineCheckBox_CheckedChanged);
            // 
            // myVotesCheckBox
            // 
            this.myVotesCheckBox.AutoSize = true;
            this.myVotesCheckBox.Location = new System.Drawing.Point(414, 34);
            this.myVotesCheckBox.Name = "myVotesCheckBox";
            this.myVotesCheckBox.Size = new System.Drawing.Size(70, 17);
            this.myVotesCheckBox.TabIndex = 5;
            this.myVotesCheckBox.Text = "My Votes";
            this.myVotesCheckBox.UseVisualStyleBackColor = true;
            this.myVotesCheckBox.CheckedChanged += new System.EventHandler(this.MyVotesCheckBox_CheckedChanged);
            // 
            // FormFeatureRequest
            // 
            this.AcceptButton = this.searchButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(964, 661);
            this.Controls.Add(this.myVotesCheckBox);
            this.Controls.Add(this.mineCheckBox);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchFirstLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.requestsGrid);
            this.Controls.Add(this.voteLabel);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 520);
            this.Name = "FormFeatureRequest";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Feature Requests";
            this.Load += new System.EventHandler(this.FormFeatureRequest_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label voteLabel;
		private System.Windows.Forms.Label searchLabel;
		private System.Windows.Forms.TextBox searchTextBox;
		private OpenDental.UI.ODGrid requestsGrid;
		private OpenDental.UI.Button addButton;
		private System.Windows.Forms.Label searchFirstLabel;
		private OpenDental.UI.Button searchButton;
		private OpenDental.UI.Button editButton;
		private System.Windows.Forms.CheckBox mineCheckBox;
		private System.Windows.Forms.CheckBox myVotesCheckBox;
	}
}
