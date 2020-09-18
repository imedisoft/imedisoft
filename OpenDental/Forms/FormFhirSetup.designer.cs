namespace Imedisoft.Forms
{
    partial class FormFhirSetup
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
            this.acceptButton = new OpenDental.UI.Button();
            this.apiKeysGrid = new OpenDental.UI.ODGrid();
            this.permissionsLabel = new System.Windows.Forms.Label();
            this.permissionsListBox = new System.Windows.Forms.ListBox();
            this.addButton = new OpenDental.UI.Button();
            this.enabledCheckBox = new System.Windows.Forms.CheckBox();
            this.intervalInfoLabel = new System.Windows.Forms.Label();
            this.intervalTextBox = new System.Windows.Forms.TextBox();
            this.paymentTypeLabel = new System.Windows.Forms.Label();
            this.paymentTypeComboBox = new System.Windows.Forms.ComboBox();
            this.intervalLabel = new System.Windows.Forms.Label();
            this.refreshButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(692, 484);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "&Close";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // apiKeysGrid
            // 
            this.apiKeysGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.apiKeysGrid.ColorSelectedRow = System.Drawing.SystemColors.Highlight;
            this.apiKeysGrid.Location = new System.Drawing.Point(12, 102);
            this.apiKeysGrid.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.apiKeysGrid.Name = "apiKeysGrid";
            this.apiKeysGrid.Size = new System.Drawing.Size(514, 376);
            this.apiKeysGrid.TabIndex = 7;
            this.apiKeysGrid.Title = "API Keys";
            this.apiKeysGrid.TranslationName = "tableAPIKeys";
            this.apiKeysGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.ApiKeysGrid_CellDoubleClick);
            this.apiKeysGrid.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.ApiKeysGrid_CellClick);
            // 
            // permissionsLabel
            // 
            this.permissionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.permissionsLabel.AutoSize = true;
            this.permissionsLabel.Location = new System.Drawing.Point(529, 86);
            this.permissionsLabel.Name = "permissionsLabel";
            this.permissionsLabel.Size = new System.Drawing.Size(119, 13);
            this.permissionsLabel.TabIndex = 8;
            this.permissionsLabel.Text = "Permissions for API key";
            // 
            // permissionsListBox
            // 
            this.permissionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.permissionsListBox.IntegralHeight = false;
            this.permissionsListBox.Location = new System.Drawing.Point(532, 102);
            this.permissionsListBox.Name = "permissionsListBox";
            this.permissionsListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.permissionsListBox.Size = new System.Drawing.Size(240, 376);
            this.permissionsListBox.TabIndex = 9;
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(12, 484);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(120, 25);
            this.addButton.TabIndex = 10;
            this.addButton.Text = "&Assign Key";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // enabledCheckBox
            // 
            this.enabledCheckBox.AutoSize = true;
            this.enabledCheckBox.Location = new System.Drawing.Point(240, 12);
            this.enabledCheckBox.Name = "enabledCheckBox";
            this.enabledCheckBox.Size = new System.Drawing.Size(64, 17);
            this.enabledCheckBox.TabIndex = 1;
            this.enabledCheckBox.Text = "Enabled";
            // 
            // intervalInfoLabel
            // 
            this.intervalInfoLabel.AutoSize = true;
            this.intervalInfoLabel.Location = new System.Drawing.Point(316, 38);
            this.intervalInfoLabel.Name = "intervalInfoLabel";
            this.intervalInfoLabel.Size = new System.Drawing.Size(182, 13);
            this.intervalInfoLabel.TabIndex = 4;
            this.intervalInfoLabel.Text = "Leave blank to disable subscriptions.";
            // 
            // intervalTextBox
            // 
            this.intervalTextBox.Location = new System.Drawing.Point(240, 35);
            this.intervalTextBox.Name = "intervalTextBox";
            this.intervalTextBox.Size = new System.Drawing.Size(70, 20);
            this.intervalTextBox.TabIndex = 3;
            // 
            // paymentTypeLabel
            // 
            this.paymentTypeLabel.AutoSize = true;
            this.paymentTypeLabel.Location = new System.Drawing.Point(53, 64);
            this.paymentTypeLabel.Name = "paymentTypeLabel";
            this.paymentTypeLabel.Size = new System.Drawing.Size(181, 13);
            this.paymentTypeLabel.TabIndex = 5;
            this.paymentTypeLabel.Text = "Payment type for created payments";
            // 
            // paymentTypeComboBox
            // 
            this.paymentTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.paymentTypeComboBox.Location = new System.Drawing.Point(240, 61);
            this.paymentTypeComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.paymentTypeComboBox.Name = "paymentTypeComboBox";
            this.paymentTypeComboBox.Size = new System.Drawing.Size(200, 21);
            this.paymentTypeComboBox.TabIndex = 6;
            // 
            // intervalLabel
            // 
            this.intervalLabel.AutoSize = true;
            this.intervalLabel.Location = new System.Drawing.Point(40, 38);
            this.intervalLabel.Name = "intervalLabel";
            this.intervalLabel.Size = new System.Drawing.Size(194, 13);
            this.intervalLabel.TabIndex = 2;
            this.intervalLabel.Text = "Process subscription interval in minutes";
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.refreshButton.Image = global::Imedisoft.Properties.Resources.IconSyncAlt;
            this.refreshButton.Location = new System.Drawing.Point(155, 484);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(30, 25);
            this.refreshButton.TabIndex = 11;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // FormFhirSetup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(784, 521);
            this.Controls.Add(this.paymentTypeLabel);
            this.Controls.Add(this.paymentTypeComboBox);
            this.Controls.Add(this.intervalTextBox);
            this.Controls.Add(this.intervalLabel);
            this.Controls.Add(this.intervalInfoLabel);
            this.Controls.Add(this.enabledCheckBox);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.permissionsListBox);
            this.Controls.Add(this.permissionsLabel);
            this.Controls.Add(this.apiKeysGrid);
            this.Controls.Add(this.acceptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFhirSetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FHIR Setup";
            this.Load += new System.EventHandler(this.FormFhirSetup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.Button acceptButton;
        private OpenDental.UI.ODGrid apiKeysGrid;
        private System.Windows.Forms.Label permissionsLabel;
        private System.Windows.Forms.ListBox permissionsListBox;
        private OpenDental.UI.Button addButton;
        private System.Windows.Forms.CheckBox enabledCheckBox;
        private System.Windows.Forms.Label intervalInfoLabel;
        private System.Windows.Forms.TextBox intervalTextBox;
        private System.Windows.Forms.Label paymentTypeLabel;
        private System.Windows.Forms.ComboBox paymentTypeComboBox;
        private System.Windows.Forms.Label intervalLabel;
        private OpenDental.UI.Button refreshButton;
    }
}
