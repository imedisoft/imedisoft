namespace Imedisoft.CEMT.Forms
{
    partial class FormCentralPatientTransfer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralPatientTransfer));
            this.patientsGrid = new OpenDental.UI.ODGrid();
            this.targetDatabasesGrid = new OpenDental.UI.ODGrid();
            this.sourceLabel = new System.Windows.Forms.Label();
            this.exportButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.patientsAddButton = new OpenDental.UI.Button();
            this.databasesAddButton = new OpenDental.UI.Button();
            this.patientsRemoveButton = new OpenDental.UI.Button();
            this.databasesRemoveButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // patientsGrid
            // 
            this.patientsGrid.AllowSortingByColumn = true;
            this.patientsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.patientsGrid.Location = new System.Drawing.Point(13, 36);
            this.patientsGrid.Name = "patientsGrid";
            this.patientsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.patientsGrid.Size = new System.Drawing.Size(450, 401);
            this.patientsGrid.TabIndex = 1;
            this.patientsGrid.Title = "Patients to Export";
            // 
            // targetDatabasesGrid
            // 
            this.targetDatabasesGrid.AllowSortingByColumn = true;
            this.targetDatabasesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.targetDatabasesGrid.Location = new System.Drawing.Point(469, 36);
            this.targetDatabasesGrid.Name = "targetDatabasesGrid";
            this.targetDatabasesGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.targetDatabasesGrid.Size = new System.Drawing.Size(450, 401);
            this.targetDatabasesGrid.TabIndex = 4;
            this.targetDatabasesGrid.Title = "Database to Export patients to";
            // 
            // sourceLabel
            // 
            this.sourceLabel.AutoSize = true;
            this.sourceLabel.Location = new System.Drawing.Point(13, 10);
            this.sourceLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.sourceLabel.Name = "sourceLabel";
            this.sourceLabel.Size = new System.Drawing.Size(101, 13);
            this.sourceLabel.TabIndex = 0;
            this.sourceLabel.Text = "Source database:   ";
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportButton.Location = new System.Drawing.Point(941, 365);
            this.exportButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 50);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(80, 25);
            this.exportButton.TabIndex = 7;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(941, 443);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "&Close";
            // 
            // patientsAddButton
            // 
            this.patientsAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.patientsAddButton.Location = new System.Drawing.Point(13, 443);
            this.patientsAddButton.Name = "patientsAddButton";
            this.patientsAddButton.Size = new System.Drawing.Size(80, 25);
            this.patientsAddButton.TabIndex = 2;
            this.patientsAddButton.Text = "Add";
            this.patientsAddButton.UseVisualStyleBackColor = true;
            this.patientsAddButton.Click += new System.EventHandler(this.PatientsAddButton_Click);
            // 
            // databasesAddButton
            // 
            this.databasesAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.databasesAddButton.Location = new System.Drawing.Point(469, 443);
            this.databasesAddButton.Name = "databasesAddButton";
            this.databasesAddButton.Size = new System.Drawing.Size(80, 25);
            this.databasesAddButton.TabIndex = 5;
            this.databasesAddButton.Text = "Add";
            this.databasesAddButton.UseVisualStyleBackColor = true;
            this.databasesAddButton.Click += new System.EventHandler(this.DatabasesAddButton_Click);
            // 
            // patientsRemoveButton
            // 
            this.patientsRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.patientsRemoveButton.Location = new System.Drawing.Point(99, 443);
            this.patientsRemoveButton.Name = "patientsRemoveButton";
            this.patientsRemoveButton.Size = new System.Drawing.Size(80, 25);
            this.patientsRemoveButton.TabIndex = 3;
            this.patientsRemoveButton.Text = "Remove";
            this.patientsRemoveButton.UseVisualStyleBackColor = true;
            this.patientsRemoveButton.Click += new System.EventHandler(this.PatientsRemoveButton_Click);
            // 
            // databasesRemoveButton
            // 
            this.databasesRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.databasesRemoveButton.Location = new System.Drawing.Point(555, 443);
            this.databasesRemoveButton.Name = "databasesRemoveButton";
            this.databasesRemoveButton.Size = new System.Drawing.Size(80, 25);
            this.databasesRemoveButton.TabIndex = 6;
            this.databasesRemoveButton.Text = "Remove";
            this.databasesRemoveButton.UseVisualStyleBackColor = true;
            this.databasesRemoveButton.Click += new System.EventHandler(this.DatabasesRemoveButton_Click);
            // 
            // FormCentralPatientTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(1034, 481);
            this.Controls.Add(this.databasesRemoveButton);
            this.Controls.Add(this.patientsRemoveButton);
            this.Controls.Add(this.databasesAddButton);
            this.Controls.Add(this.patientsAddButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.sourceLabel);
            this.Controls.Add(this.targetDatabasesGrid);
            this.Controls.Add(this.patientsGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCentralPatientTransfer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Patient Transfer";
            this.Load += new System.EventHandler(this.FormCentralPatientTransfer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.ODGrid patientsGrid;
        private OpenDental.UI.ODGrid targetDatabasesGrid;
        private System.Windows.Forms.Label sourceLabel;
        private OpenDental.UI.Button exportButton;
        private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.Button patientsAddButton;
        private OpenDental.UI.Button databasesAddButton;
        private OpenDental.UI.Button patientsRemoveButton;
        private OpenDental.UI.Button databasesRemoveButton;
    }
}
