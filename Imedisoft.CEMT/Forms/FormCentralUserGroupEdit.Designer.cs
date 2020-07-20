namespace Imedisoft.CEMT.Forms
{
    partial class FormCentralUserGroupEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralUserGroupEdit));
            this.deleteButton = new OpenDental.UI.Button();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.CEMT.Properties.Resources.IconRemove;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(13, 93);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.Text = "Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(100, 29);
            this.descriptionTextBox.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(238, 20);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(34, 32);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Description";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(185, 93);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(271, 93);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormCentralUserGroupEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(364, 131);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCentralUserGroupEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Group";
            this.Load += new System.EventHandler(this.FormCentralUserGroupEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.Button deleteButton;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private OpenDental.UI.Button acceptButton;
        private OpenDental.UI.Button cancelButton;
    }
}