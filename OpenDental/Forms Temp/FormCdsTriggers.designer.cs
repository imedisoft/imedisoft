namespace Imedisoft.Forms
{
    partial class FormCdsTriggers
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
            this.ehrTriggersGrid = new OpenDental.UI.ODGrid();
            this.addButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.setupButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // ehrTriggersGrid
            // 
            this.ehrTriggersGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ehrTriggersGrid.Enabled = false;
            this.ehrTriggersGrid.Location = new System.Drawing.Point(12, 43);
            this.ehrTriggersGrid.Name = "ehrTriggersGrid";
            this.ehrTriggersGrid.Size = new System.Drawing.Size(780, 495);
            this.ehrTriggersGrid.TabIndex = 199;
            this.ehrTriggersGrid.Title = "CDS Triggers";
            this.ehrTriggersGrid.TranslationName = "TableCDSTriggers";
            this.ehrTriggersGrid.WrapText = false;
            this.ehrTriggersGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.EhrTriggersGrid_CellDoubleClick);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Enabled = false;
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(12, 544);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 201;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(712, 544);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Close";
            // 
            // setupButton
            // 
            this.setupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setupButton.Enabled = false;
            this.setupButton.Image = global::Imedisoft.Properties.Resources.IconCog;
            this.setupButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.setupButton.Location = new System.Drawing.Point(692, 12);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(100, 25);
            this.setupButton.TabIndex = 202;
            this.setupButton.Text = "&Setup";
            this.setupButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(48, 544);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 201;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormCdsTriggers
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(804, 581);
            this.Controls.Add(this.setupButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.ehrTriggersGrid);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCdsTriggers";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Clinical Decision Support Triggers";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCdsTriggers_FormClosing);
            this.Load += new System.EventHandler(this.FormCdsTriggers_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.ODGrid ehrTriggersGrid;
        private OpenDental.UI.Button addButton;
        private OpenDental.UI.Button setupButton;
        private OpenDental.UI.Button deleteButton;
    }
}