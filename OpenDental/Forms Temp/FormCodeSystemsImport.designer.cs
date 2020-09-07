namespace Imedisoft.Forms
{
    partial class FormCodeSystemsImport
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
            this.codeSystemsGrid = new OpenDental.UI.ODGrid();
            this.checkUpdatesButton = new OpenDental.UI.Button();
            this.downloadButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.keepDescriptionsCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // codeSystemsGrid
            // 
            this.codeSystemsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.codeSystemsGrid.EditableAcceptsCR = true;
            this.codeSystemsGrid.Location = new System.Drawing.Point(12, 43);
            this.codeSystemsGrid.Name = "codeSystemsGrid";
            this.codeSystemsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.codeSystemsGrid.Size = new System.Drawing.Size(760, 335);
            this.codeSystemsGrid.TabIndex = 4;
            this.codeSystemsGrid.Title = "Code Systems Available";
            this.codeSystemsGrid.TranslationName = "TableCodeSystems";
            // 
            // checkUpdatesButton
            // 
            this.checkUpdatesButton.Image = global::Imedisoft.Properties.Resources.IconSyncAlt;
            this.checkUpdatesButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkUpdatesButton.Location = new System.Drawing.Point(12, 12);
            this.checkUpdatesButton.Name = "checkUpdatesButton";
            this.checkUpdatesButton.Size = new System.Drawing.Size(130, 25);
            this.checkUpdatesButton.TabIndex = 1;
            this.checkUpdatesButton.Text = "Check for Updates";
            this.checkUpdatesButton.Click += new System.EventHandler(this.CheckUpdatesButton_Click);
            // 
            // downloadButton
            // 
            this.downloadButton.Enabled = false;
            this.downloadButton.Location = new System.Drawing.Point(148, 12);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(130, 25);
            this.downloadButton.TabIndex = 2;
            this.downloadButton.Text = "Download Updates";
            this.downloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(692, 384);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Close";
            // 
            // keepDescriptionsCheckBox
            // 
            this.keepDescriptionsCheckBox.AutoSize = true;
            this.keepDescriptionsCheckBox.Enabled = false;
            this.keepDescriptionsCheckBox.Location = new System.Drawing.Point(284, 17);
            this.keepDescriptionsCheckBox.Name = "keepDescriptionsCheckBox";
            this.keepDescriptionsCheckBox.Size = new System.Drawing.Size(127, 17);
            this.keepDescriptionsCheckBox.TabIndex = 3;
            this.keepDescriptionsCheckBox.Text = "Keep old descriptions";
            this.keepDescriptionsCheckBox.UseVisualStyleBackColor = true;
            // 
            // FormCodeSystemsImport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(784, 421);
            this.Controls.Add(this.keepDescriptionsCheckBox);
            this.Controls.Add(this.checkUpdatesButton);
            this.Controls.Add(this.codeSystemsGrid);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCodeSystemsImport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Code Systems";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCodeSystemsImport_FormClosing);
            this.Load += new System.EventHandler(this.FormCodeSystemsImport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.Button downloadButton;
        private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.ODGrid codeSystemsGrid;
        private OpenDental.UI.Button checkUpdatesButton;
        private System.Windows.Forms.CheckBox keepDescriptionsCheckBox;
    }
}
