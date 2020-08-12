namespace Imedisoft.Forms
{
    partial class FormAbout
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

        private void InitializeComponent()
        {
            this.versionLabel = new System.Windows.Forms.Label();
            this.closeButton = new OpenDental.UI.Button();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.licenseLabel = new System.Windows.Forms.Label();
            this.licenseButton = new OpenDental.UI.Button();
            this.serviceHostnameLabel = new System.Windows.Forms.Label();
            this.serviceNameLabel = new System.Windows.Forms.Label();
            this.serviceVersionLabel = new System.Windows.Forms.Label();
            this.serviceCommentLabel = new System.Windows.Forms.Label();
            this.splitterLabel = new System.Windows.Forms.Label();
            this.connectionGroupBox = new System.Windows.Forms.GroupBox();
            this.machineNameCaptionLabel = new System.Windows.Forms.Label();
            this.serviceNameCaptionLabel = new System.Windows.Forms.Label();
            this.machineNameLabel = new System.Windows.Forms.Label();
            this.serviceHostnameCaptionLabel = new System.Windows.Forms.Label();
            this.serviceCommentCaptionLabel = new System.Windows.Forms.Label();
            this.serviceVersionCaptionLabel = new System.Windows.Forms.Label();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.connectionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.versionLabel.Location = new System.Drawing.Point(242, 9);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(230, 20);
            this.versionLabel.TabIndex = 1;
            this.versionLabel.Text = "Version";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.closeButton.Location = new System.Drawing.Point(392, 294);
            this.closeButton.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "&Close";
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(12, 247);
            this.copyrightLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(238, 13);
            this.copyrightLabel.TabIndex = 4;
            this.copyrightLabel.Text = "Copyright 2003-2007, Jordan S. Sparks, D.M.D.";
            // 
            // licenseLabel
            // 
            this.licenseLabel.AutoSize = true;
            this.licenseLabel.Location = new System.Drawing.Point(12, 265);
            this.licenseLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.licenseLabel.Name = "licenseLabel";
            this.licenseLabel.Size = new System.Drawing.Size(141, 13);
            this.licenseLabel.TabIndex = 5;
            this.licenseLabel.Text = "Main software license is GPL";
            // 
            // licenseButton
            // 
            this.licenseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.licenseButton.Location = new System.Drawing.Point(289, 294);
            this.licenseButton.Name = "licenseButton";
            this.licenseButton.Size = new System.Drawing.Size(90, 25);
            this.licenseButton.TabIndex = 6;
            this.licenseButton.Text = "View Licenses";
            this.licenseButton.Click += new System.EventHandler(this.LicenseButton_Click);
            // 
            // serviceHostnameLabel
            // 
            this.serviceHostnameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceHostnameLabel.AutoSize = true;
            this.serviceHostnameLabel.Location = new System.Drawing.Point(200, 51);
            this.serviceHostnameLabel.Name = "serviceHostnameLabel";
            this.serviceHostnameLabel.Size = new System.Drawing.Size(58, 13);
            this.serviceHostnameLabel.TabIndex = 3;
            this.serviceHostnameLabel.Text = "(unknown)";
            this.serviceHostnameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serviceNameLabel
            // 
            this.serviceNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceNameLabel.AutoSize = true;
            this.serviceNameLabel.Location = new System.Drawing.Point(200, 74);
            this.serviceNameLabel.Name = "serviceNameLabel";
            this.serviceNameLabel.Size = new System.Drawing.Size(58, 13);
            this.serviceNameLabel.TabIndex = 5;
            this.serviceNameLabel.Text = "(unknown)";
            this.serviceNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serviceVersionLabel
            // 
            this.serviceVersionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceVersionLabel.AutoSize = true;
            this.serviceVersionLabel.Location = new System.Drawing.Point(200, 97);
            this.serviceVersionLabel.Name = "serviceVersionLabel";
            this.serviceVersionLabel.Size = new System.Drawing.Size(58, 13);
            this.serviceVersionLabel.TabIndex = 7;
            this.serviceVersionLabel.Text = "(unknown)";
            this.serviceVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serviceCommentLabel
            // 
            this.serviceCommentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceCommentLabel.AutoSize = true;
            this.serviceCommentLabel.Location = new System.Drawing.Point(200, 120);
            this.serviceCommentLabel.Name = "serviceCommentLabel";
            this.serviceCommentLabel.Size = new System.Drawing.Size(58, 13);
            this.serviceCommentLabel.TabIndex = 9;
            this.serviceCommentLabel.Text = "(unknown)";
            this.serviceCommentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitterLabel
            // 
            this.splitterLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitterLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitterLabel.Location = new System.Drawing.Point(12, 235);
            this.splitterLabel.Name = "splitterLabel";
            this.splitterLabel.Size = new System.Drawing.Size(460, 2);
            this.splitterLabel.TabIndex = 3;
            // 
            // connectionGroupBox
            // 
            this.connectionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionGroupBox.Controls.Add(this.machineNameCaptionLabel);
            this.connectionGroupBox.Controls.Add(this.serviceNameCaptionLabel);
            this.connectionGroupBox.Controls.Add(this.machineNameLabel);
            this.connectionGroupBox.Controls.Add(this.serviceHostnameCaptionLabel);
            this.connectionGroupBox.Controls.Add(this.serviceNameLabel);
            this.connectionGroupBox.Controls.Add(this.serviceCommentCaptionLabel);
            this.connectionGroupBox.Controls.Add(this.serviceHostnameLabel);
            this.connectionGroupBox.Controls.Add(this.serviceVersionCaptionLabel);
            this.connectionGroupBox.Controls.Add(this.serviceCommentLabel);
            this.connectionGroupBox.Controls.Add(this.serviceVersionLabel);
            this.connectionGroupBox.Location = new System.Drawing.Point(12, 75);
            this.connectionGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.connectionGroupBox.Name = "connectionGroupBox";
            this.connectionGroupBox.Padding = new System.Windows.Forms.Padding(10, 15, 10, 10);
            this.connectionGroupBox.Size = new System.Drawing.Size(460, 150);
            this.connectionGroupBox.TabIndex = 2;
            this.connectionGroupBox.TabStop = false;
            this.connectionGroupBox.Text = "Current Connection";
            // 
            // machineNameCaptionLabel
            // 
            this.machineNameCaptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.machineNameCaptionLabel.AutoSize = true;
            this.machineNameCaptionLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.machineNameCaptionLabel.Location = new System.Drawing.Point(70, 28);
            this.machineNameCaptionLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.machineNameCaptionLabel.Name = "machineNameCaptionLabel";
            this.machineNameCaptionLabel.Size = new System.Drawing.Size(124, 13);
            this.machineNameCaptionLabel.TabIndex = 0;
            this.machineNameCaptionLabel.Text = "Client Machine Name";
            this.machineNameCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serviceNameCaptionLabel
            // 
            this.serviceNameCaptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceNameCaptionLabel.AutoSize = true;
            this.serviceNameCaptionLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serviceNameCaptionLabel.Location = new System.Drawing.Point(110, 74);
            this.serviceNameCaptionLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.serviceNameCaptionLabel.Name = "serviceNameCaptionLabel";
            this.serviceNameCaptionLabel.Size = new System.Drawing.Size(84, 13);
            this.serviceNameCaptionLabel.TabIndex = 4;
            this.serviceNameCaptionLabel.Text = "Service Name";
            this.serviceNameCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // machineNameLabel
            // 
            this.machineNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.machineNameLabel.AutoSize = true;
            this.machineNameLabel.Location = new System.Drawing.Point(200, 28);
            this.machineNameLabel.Name = "machineNameLabel";
            this.machineNameLabel.Size = new System.Drawing.Size(58, 13);
            this.machineNameLabel.TabIndex = 1;
            this.machineNameLabel.Text = "(unknown)";
            this.machineNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serviceHostnameCaptionLabel
            // 
            this.serviceHostnameCaptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceHostnameCaptionLabel.AutoSize = true;
            this.serviceHostnameCaptionLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serviceHostnameCaptionLabel.Location = new System.Drawing.Point(114, 51);
            this.serviceHostnameCaptionLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.serviceHostnameCaptionLabel.Name = "serviceHostnameCaptionLabel";
            this.serviceHostnameCaptionLabel.Size = new System.Drawing.Size(80, 13);
            this.serviceHostnameCaptionLabel.TabIndex = 2;
            this.serviceHostnameCaptionLabel.Text = "Server Name";
            this.serviceHostnameCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serviceCommentCaptionLabel
            // 
            this.serviceCommentCaptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceCommentCaptionLabel.AutoSize = true;
            this.serviceCommentCaptionLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serviceCommentCaptionLabel.Location = new System.Drawing.Point(87, 120);
            this.serviceCommentCaptionLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.serviceCommentCaptionLabel.Name = "serviceCommentCaptionLabel";
            this.serviceCommentCaptionLabel.Size = new System.Drawing.Size(107, 13);
            this.serviceCommentCaptionLabel.TabIndex = 8;
            this.serviceCommentCaptionLabel.Text = "Service Comment";
            this.serviceCommentCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serviceVersionCaptionLabel
            // 
            this.serviceVersionCaptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceVersionCaptionLabel.AutoSize = true;
            this.serviceVersionCaptionLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serviceVersionCaptionLabel.Location = new System.Drawing.Point(100, 97);
            this.serviceVersionCaptionLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.serviceVersionCaptionLabel.Name = "serviceVersionCaptionLabel";
            this.serviceVersionCaptionLabel.Size = new System.Drawing.Size(94, 13);
            this.serviceVersionCaptionLabel.TabIndex = 6;
            this.serviceVersionCaptionLabel.Text = "Service Version";
            this.serviceVersionCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Image = global::Imedisoft.Properties.Resources.LogoSmall;
            this.logoPictureBox.Location = new System.Drawing.Point(12, 12);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(200, 50);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // FormAbout
            // 
            this.AcceptButton = this.closeButton;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(484, 331);
            this.Controls.Add(this.logoPictureBox);
            this.Controls.Add(this.connectionGroupBox);
            this.Controls.Add(this.splitterLabel);
            this.Controls.Add(this.licenseButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.licenseLabel);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.versionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.connectionGroupBox.ResumeLayout(false);
            this.connectionGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label versionLabel;
        private OpenDental.UI.Button closeButton;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Label licenseLabel;
        private OpenDental.UI.Button licenseButton;
        private System.Windows.Forms.Label serviceHostnameLabel;
        private System.Windows.Forms.Label serviceNameLabel;
        private System.Windows.Forms.Label serviceVersionLabel;
        private System.Windows.Forms.Label serviceCommentLabel;
        private System.Windows.Forms.Label splitterLabel;
        private System.Windows.Forms.GroupBox connectionGroupBox;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label machineNameLabel;
        private System.Windows.Forms.Label machineNameCaptionLabel;
        private System.Windows.Forms.Label serviceNameCaptionLabel;
        private System.Windows.Forms.Label serviceHostnameCaptionLabel;
        private System.Windows.Forms.Label serviceCommentCaptionLabel;
        private System.Windows.Forms.Label serviceVersionCaptionLabel;
    }
}
