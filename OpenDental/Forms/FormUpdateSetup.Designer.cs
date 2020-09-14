namespace Imedisoft.Forms
{
    partial class FormUpdateSetup
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
            this.registrationKeyTextBox = new System.Windows.Forms.TextBox();
            this.registrationKeyLabel = new System.Windows.Forms.Label();
            this.updateServerTextBox = new System.Windows.Forms.TextBox();
            this.updateServerLabel = new System.Windows.Forms.Label();
            this.registrationKeyButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // registrationKeyTextBox
            // 
            this.registrationKeyTextBox.Enabled = false;
            this.registrationKeyTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registrationKeyTextBox.Location = new System.Drawing.Point(120, 45);
            this.registrationKeyTextBox.Name = "registrationKeyTextBox";
            this.registrationKeyTextBox.ReadOnly = true;
            this.registrationKeyTextBox.Size = new System.Drawing.Size(250, 20);
            this.registrationKeyTextBox.TabIndex = 4;
            // 
            // registrationKeyLabel
            // 
            this.registrationKeyLabel.AutoSize = true;
            this.registrationKeyLabel.Location = new System.Drawing.Point(28, 48);
            this.registrationKeyLabel.Name = "registrationKeyLabel";
            this.registrationKeyLabel.Size = new System.Drawing.Size(86, 13);
            this.registrationKeyLabel.TabIndex = 3;
            this.registrationKeyLabel.Text = "Registration Key";
            // 
            // updateServerTextBox
            // 
            this.updateServerTextBox.Location = new System.Drawing.Point(120, 19);
            this.updateServerTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.updateServerTextBox.Name = "updateServerTextBox";
            this.updateServerTextBox.Size = new System.Drawing.Size(360, 20);
            this.updateServerTextBox.TabIndex = 2;
            // 
            // updateServerLabel
            // 
            this.updateServerLabel.AutoSize = true;
            this.updateServerLabel.Location = new System.Drawing.Point(37, 22);
            this.updateServerLabel.Name = "updateServerLabel";
            this.updateServerLabel.Size = new System.Drawing.Size(77, 13);
            this.updateServerLabel.TabIndex = 1;
            this.updateServerLabel.Text = "Update Server";
            // 
            // registrationKeyButton
            // 
            this.registrationKeyButton.Enabled = false;
            this.registrationKeyButton.Location = new System.Drawing.Point(376, 45);
            this.registrationKeyButton.Name = "registrationKeyButton";
            this.registrationKeyButton.Size = new System.Drawing.Size(60, 20);
            this.registrationKeyButton.TabIndex = 5;
            this.registrationKeyButton.Text = "Change";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(326, 104);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 6;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(412, 104);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormUpdateSetup
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(504, 141);
            this.Controls.Add(this.registrationKeyButton);
            this.Controls.Add(this.updateServerTextBox);
            this.Controls.Add(this.updateServerLabel);
            this.Controls.Add(this.registrationKeyTextBox);
            this.Controls.Add(this.registrationKeyLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUpdateSetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Update Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUpdateSetup_FormClosing);
            this.Load += new System.EventHandler(this.FormUpdateSetup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.Button acceptButton;
        private System.Windows.Forms.TextBox registrationKeyTextBox;
        private System.Windows.Forms.Label registrationKeyLabel;
        private System.Windows.Forms.TextBox updateServerTextBox;
        private System.Windows.Forms.Label updateServerLabel;
        private OpenDental.UI.Button registrationKeyButton;
    }
}
