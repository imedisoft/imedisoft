namespace Imedisoft.Forms
{
    partial class FormAccountingAutoPayEdit
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
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.payTypeComboBox = new System.Windows.Forms.ComboBox();
            this.payTypeLabel = new System.Windows.Forms.Label();
            this.accountsListBox = new System.Windows.Forms.ListBox();
            this.accountsLabel = new System.Windows.Forms.Label();
            this.removeButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(432, 204);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(346, 204);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 7;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // payTypeComboBox
            // 
            this.payTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.payTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.payTypeComboBox.FormattingEnabled = true;
            this.payTypeComboBox.Location = new System.Drawing.Point(240, 19);
            this.payTypeComboBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.payTypeComboBox.Name = "payTypeComboBox";
            this.payTypeComboBox.Size = new System.Drawing.Size(236, 21);
            this.payTypeComboBox.TabIndex = 1;
            // 
            // payTypeLabel
            // 
            this.payTypeLabel.AutoSize = true;
            this.payTypeLabel.Location = new System.Drawing.Point(45, 22);
            this.payTypeLabel.Name = "payTypeLabel";
            this.payTypeLabel.Size = new System.Drawing.Size(189, 13);
            this.payTypeLabel.TabIndex = 0;
            this.payTypeLabel.Text = "When this type of payment is entered\r\n";
            // 
            // accountsListBox
            // 
            this.accountsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountsListBox.FormattingEnabled = true;
            this.accountsListBox.IntegralHeight = false;
            this.accountsListBox.Location = new System.Drawing.Point(240, 46);
            this.accountsListBox.Name = "accountsListBox";
            this.accountsListBox.Size = new System.Drawing.Size(236, 120);
            this.accountsListBox.TabIndex = 3;
            this.accountsListBox.SelectedIndexChanged += new System.EventHandler(this.AccountsListBox_SelectedIndexChanged);
            // 
            // accountsLabel
            // 
            this.accountsLabel.Location = new System.Drawing.Point(12, 46);
            this.accountsLabel.Name = "accountsLabel";
            this.accountsLabel.Size = new System.Drawing.Size(222, 120);
            this.accountsLabel.TabIndex = 2;
            this.accountsLabel.Text = "User will get to pick from this list of accounts to deposit into.";
            this.accountsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeButton.Enabled = false;
            this.removeButton.Image = global::Imedisoft.Properties.Resources.IconMinus;
            this.removeButton.Location = new System.Drawing.Point(482, 77);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(30, 25);
            this.removeButton.TabIndex = 5;
            this.removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlus;
            this.addButton.Location = new System.Drawing.Point(482, 46);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 4;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTrash;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(12, 204);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 6;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormAccountingAutoPayEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(524, 241);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.payTypeComboBox);
            this.Controls.Add(this.payTypeLabel);
            this.Controls.Add(this.accountsListBox);
            this.Controls.Add(this.accountsLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAccountingAutoPayEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Auto Pay Entry";
            this.Load += new System.EventHandler(this.FormAccountingAutoPayEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.ComboBox payTypeComboBox;
		private System.Windows.Forms.Label payTypeLabel;
		private System.Windows.Forms.ListBox accountsListBox;
		private System.Windows.Forms.Label accountsLabel;
		private OpenDental.UI.Button removeButton;
		private OpenDental.UI.Button addButton;
		private OpenDental.UI.Button deleteButton;
	}
}
