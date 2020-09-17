namespace Imedisoft.Forms
{
    partial class FormAlertCategorySetup
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
            this.duplicateButton = new OpenDental.UI.Button();
            this.copyButton = new OpenDental.UI.Button();
            this.internalGrid = new OpenDental.UI.ODGrid();
            this.customGrid = new OpenDental.UI.ODGrid();
            this.cancelButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.editButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // duplicateButton
            // 
            this.duplicateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.duplicateButton.Enabled = false;
            this.duplicateButton.Image = global::Imedisoft.Properties.Resources.IconCopyGreen;
            this.duplicateButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.duplicateButton.Location = new System.Drawing.Point(591, 424);
            this.duplicateButton.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.duplicateButton.Name = "duplicateButton";
            this.duplicateButton.Size = new System.Drawing.Size(80, 25);
            this.duplicateButton.TabIndex = 6;
            this.duplicateButton.Text = "Duplicate";
            this.duplicateButton.Click += new System.EventHandler(this.DuplicateButton_Click);
            // 
            // copyButton
            // 
            this.copyButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.copyButton.Image = global::Imedisoft.Properties.Resources.IconArrowRight;
            this.copyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.copyButton.Location = new System.Drawing.Point(402, 210);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(80, 25);
            this.copyButton.TabIndex = 2;
            this.copyButton.Text = "Copy";
            this.copyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // internalGrid
            // 
            this.internalGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.internalGrid.Location = new System.Drawing.Point(12, 12);
            this.internalGrid.Name = "internalGrid";
            this.internalGrid.Size = new System.Drawing.Size(370, 406);
            this.internalGrid.TabIndex = 1;
            this.internalGrid.Title = "Internal";
            this.internalGrid.TranslationName = "TableInternal";
            this.internalGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.InternalGrid_CellDoubleClick);
            // 
            // customGrid
            // 
            this.customGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customGrid.Location = new System.Drawing.Point(502, 12);
            this.customGrid.Name = "customGrid";
            this.customGrid.Size = new System.Drawing.Size(370, 406);
            this.customGrid.TabIndex = 3;
            this.customGrid.Title = "Custom";
            this.customGrid.TranslationName = "TableCustom";
            this.customGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.CustomGrid_CellDoubleClick);
            this.customGrid.SelectionCommitted += new System.EventHandler(this.CustomGrid_SelectionCommitted);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(792, 424);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Close";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(538, 424);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPenBlack;
            this.editButton.Location = new System.Drawing.Point(502, 424);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(30, 25);
            this.editButton.TabIndex = 4;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // FormAlertCategorySetup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.duplicateButton);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.internalGrid);
            this.Controls.Add(this.customGrid);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAlertCategorySetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Alert Categories";
            this.Load += new System.EventHandler(this.FormAlertCategorySetup_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenDental.UI.Button duplicateButton;
        private OpenDental.UI.Button copyButton;
        private OpenDental.UI.ODGrid internalGrid;
        private OpenDental.UI.ODGrid customGrid;
        private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.Button deleteButton;
        private OpenDental.UI.Button editButton;
    }
}
