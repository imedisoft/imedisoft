namespace Imedisoft.CEMT.Forms
{
    partial class FormCentralReportSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralReportSetup));
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.reportSetupTabControl = new System.Windows.Forms.TabControl();
            this.reportPermissionsGroupBox = new System.Windows.Forms.TabPage();
            this.reportSetupUserControl = new OpenDental.User_Controls.UserControlReportSetup();
            this.reportSetupTabControl.SuspendLayout();
            this.reportPermissionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(395, 663);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 5;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(481, 663);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // reportSetupTabControl
            // 
            this.reportSetupTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportSetupTabControl.Controls.Add(this.reportPermissionsGroupBox);
            this.reportSetupTabControl.Location = new System.Drawing.Point(13, 13);
            this.reportSetupTabControl.Name = "reportSetupTabControl";
            this.reportSetupTabControl.SelectedIndex = 0;
            this.reportSetupTabControl.Size = new System.Drawing.Size(548, 644);
            this.reportSetupTabControl.TabIndex = 217;
            // 
            // reportPermissionsGroupBox
            // 
            this.reportPermissionsGroupBox.Controls.Add(this.reportSetupUserControl);
            this.reportPermissionsGroupBox.Location = new System.Drawing.Point(4, 22);
            this.reportPermissionsGroupBox.Name = "reportPermissionsGroupBox";
            this.reportPermissionsGroupBox.Size = new System.Drawing.Size(540, 618);
            this.reportPermissionsGroupBox.TabIndex = 3;
            this.reportPermissionsGroupBox.Text = "Security Permissions";
            this.reportPermissionsGroupBox.UseVisualStyleBackColor = true;
            // 
            // reportSetupUserControl
            // 
            this.reportSetupUserControl.Location = new System.Drawing.Point(2, 2);
            this.reportSetupUserControl.Name = "reportSetupUserControl";
            this.reportSetupUserControl.Size = new System.Drawing.Size(525, 607);
            this.reportSetupUserControl.TabIndex = 2;
            // 
            // FormCentralReportSetup
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(574, 701);
            this.Controls.Add(this.reportSetupTabControl);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCentralReportSetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Report Setup";
            this.Load += new System.EventHandler(this.FormCentralReportSetup_Load);
            this.reportSetupTabControl.ResumeLayout(false);
            this.reportPermissionsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenDental.UI.Button acceptButton;
        private OpenDental.UI.Button cancelButton;
        private System.Windows.Forms.TabControl reportSetupTabControl;
        private System.Windows.Forms.TabPage reportPermissionsGroupBox;
        private OpenDental.User_Controls.UserControlReportSetup reportSetupUserControl;
    }
}
