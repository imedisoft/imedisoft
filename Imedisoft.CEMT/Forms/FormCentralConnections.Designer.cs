namespace Imedisoft.CEMT.Forms
{
    partial class FormCentralConnections
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralConnections));
            this.searchLabel = new System.Windows.Forms.Label();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.connectionGroupComboBox = new System.Windows.Forms.ComboBox();
            this.connectionGroupLabel = new System.Windows.Forms.Label();
            this.alphabetizeButton = new OpenDental.UI.Button();
            this.upButton = new OpenDental.UI.Button();
            this.downButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.connectionsGrid = new OpenDental.UI.ODGrid();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.searchLabel.Location = new System.Drawing.Point(383, 16);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(40, 13);
            this.searchLabel.TabIndex = 4;
            this.searchLabel.Text = "Search";
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(429, 13);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(190, 20);
            this.searchTextBox.TabIndex = 5;
            this.searchTextBox.TextChanged += new System.EventHandler(this.SearchTextBox_TextChanged);
            // 
            // connectionGroupComboBox
            // 
            this.connectionGroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.connectionGroupComboBox.FormattingEnabled = true;
            this.connectionGroupComboBox.Location = new System.Drawing.Point(170, 13);
            this.connectionGroupComboBox.MaxDropDownItems = 20;
            this.connectionGroupComboBox.Name = "connectionGroupComboBox";
            this.connectionGroupComboBox.Size = new System.Drawing.Size(190, 21);
            this.connectionGroupComboBox.TabIndex = 3;
            this.connectionGroupComboBox.SelectionChangeCommitted += new System.EventHandler(this.ConnectionGroupComboBox_SelectionChangeCommitted);
            // 
            // connectionGroupLabel
            // 
            this.connectionGroupLabel.AutoSize = true;
            this.connectionGroupLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.connectionGroupLabel.Location = new System.Drawing.Point(66, 16);
            this.connectionGroupLabel.Name = "connectionGroupLabel";
            this.connectionGroupLabel.Size = new System.Drawing.Size(98, 13);
            this.connectionGroupLabel.TabIndex = 2;
            this.connectionGroupLabel.Text = "Connection Groups";
            // 
            // alphabetizeButton
            // 
            this.alphabetizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.alphabetizeButton.Location = new System.Drawing.Point(641, 197);
            this.alphabetizeButton.Name = "alphabetizeButton";
            this.alphabetizeButton.Size = new System.Drawing.Size(80, 25);
            this.alphabetizeButton.TabIndex = 10;
            this.alphabetizeButton.Text = "Alphabetize";
            this.alphabetizeButton.Click += new System.EventHandler(this.AlphabetizeButton_Click);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Image = global::Imedisoft.CEMT.Properties.Resources.IconUp;
            this.upButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.upButton.Location = new System.Drawing.Point(641, 118);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(80, 25);
            this.upButton.TabIndex = 8;
            this.upButton.Text = "&Up";
            this.upButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Image = global::Imedisoft.CEMT.Properties.Resources.IconDown;
            this.downButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.downButton.Location = new System.Drawing.Point(641, 149);
            this.downButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(80, 25);
            this.downButton.TabIndex = 9;
            this.downButton.Text = "&Down";
            this.downButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::Imedisoft.CEMT.Properties.Resources.IconPlus;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(641, 40);
            this.addButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 50);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 25);
            this.addButton.TabIndex = 7;
            this.addButton.Text = "&Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // connectionsGrid
            // 
            this.connectionsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionsGrid.Location = new System.Drawing.Point(13, 40);
            this.connectionsGrid.Name = "connectionsGrid";
            this.connectionsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.connectionsGrid.Size = new System.Drawing.Size(622, 448);
            this.connectionsGrid.TabIndex = 6;
            this.connectionsGrid.Title = "Connections";
            this.connectionsGrid.TranslationName = "TableConnections";
            this.connectionsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.ConnectionsGrid_CellDoubleClick);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(641, 432);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "OK";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(641, 463);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // FormCentralConnections
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(734, 501);
            this.Controls.Add(this.alphabetizeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.connectionGroupComboBox);
            this.Controls.Add(this.connectionGroupLabel);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.connectionsGrid);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCentralConnections";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connections";
            this.Load += new System.EventHandler(this.FormCentralConnections_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.Button acceptButton;
        private OpenDental.UI.ODGrid connectionsGrid;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.ComboBox connectionGroupComboBox;
        private System.Windows.Forms.Label connectionGroupLabel;
        private OpenDental.UI.Button addButton;
        private OpenDental.UI.Button upButton;
        private OpenDental.UI.Button downButton;
        private OpenDental.UI.Button alphabetizeButton;
    }
}
