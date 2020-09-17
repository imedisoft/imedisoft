namespace Imedisoft.Forms
{
    partial class FormAlertCategoryEdit
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
            this.cancelButton = new OpenDental.UI.Button();
            this.alertTypesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.alertTypesLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(166, 344);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(252, 344);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // alertTypesCheckedListBox
            // 
            this.alertTypesCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.alertTypesCheckedListBox.CheckOnClick = true;
            this.alertTypesCheckedListBox.FormattingEnabled = true;
            this.alertTypesCheckedListBox.IntegralHeight = false;
            this.alertTypesCheckedListBox.Location = new System.Drawing.Point(12, 58);
            this.alertTypesCheckedListBox.Name = "alertTypesCheckedListBox";
            this.alertTypesCheckedListBox.Size = new System.Drawing.Size(320, 280);
            this.alertTypesCheckedListBox.TabIndex = 3;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Location = new System.Drawing.Point(12, 12);
            this.descriptionTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(320, 20);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // alertTypesLabel
            // 
            this.alertTypesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.alertTypesLabel.AutoSize = true;
            this.alertTypesLabel.Location = new System.Drawing.Point(12, 42);
            this.alertTypesLabel.Name = "alertTypesLabel";
            this.alertTypesLabel.Size = new System.Drawing.Size(219, 13);
            this.alertTypesLabel.TabIndex = 2;
            this.alertTypesLabel.Text = "This category includes the following types...";
            // 
            // FormAlertCategoryEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(344, 381);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.alertTypesLabel);
            this.Controls.Add(this.alertTypesCheckedListBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(344, 284);
            this.Name = "FormAlertCategoryEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Alert Category";
            this.Load += new System.EventHandler(this.FormAlertCategoryEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.Button acceptButton;
        private OpenDental.UI.Button cancelButton;
        private System.Windows.Forms.CheckedListBox alertTypesCheckedListBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label alertTypesLabel;
    }
}
