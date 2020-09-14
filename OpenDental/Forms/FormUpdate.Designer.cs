namespace Imedisoft.Forms
{
    partial class FormUpdate
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
            this.versionLabel = new System.Windows.Forms.Label();
            this.checkForUpdatesButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.previousVersionsButton = new OpenDental.UI.Button();
            this.setupButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.Location = new System.Drawing.Point(12, 9);
            this.versionLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(95, 13);
            this.versionLabel.TabIndex = 1;
            this.versionLabel.Text = "Current Version";
            // 
            // checkForUpdatesButton
            // 
            this.checkForUpdatesButton.Image = global::Imedisoft.Properties.Resources.IconSyncAlt;
            this.checkForUpdatesButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkForUpdatesButton.Location = new System.Drawing.Point(12, 35);
            this.checkForUpdatesButton.Name = "checkForUpdatesButton";
            this.checkForUpdatesButton.Size = new System.Drawing.Size(140, 25);
            this.checkForUpdatesButton.TabIndex = 2;
            this.checkForUpdatesButton.Text = "Check for Updates";
            this.checkForUpdatesButton.Visible = false;
            this.checkForUpdatesButton.Click += new System.EventHandler(this.CheckForUpdatesButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(397, 101);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Close";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // previousVersionsButton
            // 
            this.previousVersionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.previousVersionsButton.Image = global::Imedisoft.Properties.Resources.IconHistory;
            this.previousVersionsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.previousVersionsButton.Location = new System.Drawing.Point(226, 35);
            this.previousVersionsButton.Name = "previousVersionsButton";
            this.previousVersionsButton.Size = new System.Drawing.Size(160, 25);
            this.previousVersionsButton.TabIndex = 3;
            this.previousVersionsButton.Text = "Show Previous Versions";
            this.previousVersionsButton.Click += new System.EventHandler(this.PreviousVersionsButton_Click);
            // 
            // setupButton
            // 
            this.setupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setupButton.Image = global::Imedisoft.Properties.Resources.IconCog;
            this.setupButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.setupButton.Location = new System.Drawing.Point(392, 35);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(80, 25);
            this.setupButton.TabIndex = 4;
            this.setupButton.Text = "&Setup";
            this.setupButton.Click += new System.EventHandler(this.SetupButton_Click);
            // 
            // FormUpdate
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(484, 141);
            this.Controls.Add(this.setupButton);
            this.Controls.Add(this.previousVersionsButton);
            this.Controls.Add(this.checkForUpdatesButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.versionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUpdate";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update";
            this.Load += new System.EventHandler(this.FormUpdate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private OpenDental.UI.Button cancelButton;
        private System.Windows.Forms.Label versionLabel;
        private OpenDental.UI.Button checkForUpdatesButton;
        private OpenDental.UI.Button previousVersionsButton;
        private OpenDental.UI.Button setupButton;
    }
}
