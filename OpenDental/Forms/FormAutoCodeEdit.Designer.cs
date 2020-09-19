namespace Imedisoft.Forms
{
    partial class FormAutoCodeEdit
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
            this.hiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.deleteButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.lessIntrusiveCheckBox = new System.Windows.Forms.CheckBox();
            this.autoCodeItemsGrid = new OpenDental.UI.ODGrid();
            this.editButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // hiddenCheckBox
            // 
            this.hiddenCheckBox.AutoSize = true;
            this.hiddenCheckBox.Location = new System.Drawing.Point(120, 61);
            this.hiddenCheckBox.Name = "hiddenCheckBox";
            this.hiddenCheckBox.Size = new System.Drawing.Size(59, 17);
            this.hiddenCheckBox.TabIndex = 3;
            this.hiddenCheckBox.Text = "Hidden";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(572, 434);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(486, 434);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 9;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(120, 12);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(210, 20);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(54, 15);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 101);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(247, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "You may have duplicate codes in the following list.";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(84, 403);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 8;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(12, 403);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 6;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // lessIntrusiveCheckBox
            // 
            this.lessIntrusiveCheckBox.AutoSize = true;
            this.lessIntrusiveCheckBox.Location = new System.Drawing.Point(120, 38);
            this.lessIntrusiveCheckBox.Name = "lessIntrusiveCheckBox";
            this.lessIntrusiveCheckBox.Size = new System.Drawing.Size(511, 17);
            this.lessIntrusiveCheckBox.TabIndex = 2;
            this.lessIntrusiveCheckBox.Text = "Do not check codes in the procedure edit window, but only use this auto code for " +
    "procedure buttons.";
            this.lessIntrusiveCheckBox.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // autoCodeItemsGrid
            // 
            this.autoCodeItemsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoCodeItemsGrid.ColorSelectedRow = System.Drawing.SystemColors.Highlight;
            this.autoCodeItemsGrid.Location = new System.Drawing.Point(12, 117);
            this.autoCodeItemsGrid.Name = "autoCodeItemsGrid";
            this.autoCodeItemsGrid.Size = new System.Drawing.Size(640, 280);
            this.autoCodeItemsGrid.TabIndex = 5;
            this.autoCodeItemsGrid.Title = "Auto Code Items";
            this.autoCodeItemsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.AutoCodeItemsGrid_CellDoubleClick);
            this.autoCodeItemsGrid.SelectionCommitted += new System.EventHandler(this.AutoCodeItemsGrid_SelectionCommitted);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPenBlack;
            this.editButton.Location = new System.Drawing.Point(48, 403);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(30, 25);
            this.editButton.TabIndex = 7;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // FormAutoCodeEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(664, 471);
            this.Controls.Add(this.autoCodeItemsGrid);
            this.Controls.Add(this.lessIntrusiveCheckBox);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.hiddenCheckBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAutoCodeEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Code";
            this.Load += new System.EventHandler(this.FormAutoCodeEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.CheckBox hiddenCheckBox;
		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.Label label3;
		private OpenDental.UI.Button deleteButton;
		private OpenDental.UI.Button addButton;
		private System.Windows.Forms.CheckBox lessIntrusiveCheckBox;
        private OpenDental.UI.ODGrid autoCodeItemsGrid;
        private OpenDental.UI.Button editButton;
    }
}
