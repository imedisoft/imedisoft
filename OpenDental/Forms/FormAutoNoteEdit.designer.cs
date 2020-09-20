namespace Imedisoft.Forms
{
	partial class FormAutoNoteEdit
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
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.mainTextTextBox = new System.Windows.Forms.TextBox();
            this.mainTextLabel = new System.Windows.Forms.Label();
            this.addButton = new OpenDental.UI.Button();
            this.autoNotePromptsGrid = new OpenDental.UI.ODGrid();
            this.insertButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.editButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Location = new System.Drawing.Point(100, 12);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(460, 20);
            this.nameTextBox.TabIndex = 1;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(60, 15);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(34, 13);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Name";
            // 
            // mainTextTextBox
            // 
            this.mainTextTextBox.AcceptsReturn = true;
            this.mainTextTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTextTextBox.HideSelection = false;
            this.mainTextTextBox.Location = new System.Drawing.Point(12, 61);
            this.mainTextTextBox.Multiline = true;
            this.mainTextTextBox.Name = "mainTextTextBox";
            this.mainTextTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mainTextTextBox.Size = new System.Drawing.Size(548, 510);
            this.mainTextTextBox.TabIndex = 3;
            this.mainTextTextBox.Leave += new System.EventHandler(this.MainTextTextBox_Leave);
            // 
            // mainTextLabel
            // 
            this.mainTextLabel.AutoSize = true;
            this.mainTextLabel.Location = new System.Drawing.Point(12, 45);
            this.mainTextLabel.Name = "mainTextLabel";
            this.mainTextLabel.Size = new System.Drawing.Size(29, 13);
            this.mainTextLabel.TabIndex = 2;
            this.mainTextLabel.Text = "Text";
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(602, 30);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 5;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // autoNotePromptsGrid
            // 
            this.autoNotePromptsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoNotePromptsGrid.ColorSelectedRow = System.Drawing.SystemColors.Highlight;
            this.autoNotePromptsGrid.Location = new System.Drawing.Point(602, 61);
            this.autoNotePromptsGrid.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.autoNotePromptsGrid.Name = "autoNotePromptsGrid";
            this.autoNotePromptsGrid.Size = new System.Drawing.Size(250, 510);
            this.autoNotePromptsGrid.TabIndex = 8;
            this.autoNotePromptsGrid.Title = "Available Prompts";
            this.autoNotePromptsGrid.TranslationName = "FormAutoNoteEdit";
            this.autoNotePromptsGrid.SelectionCommitted += new System.EventHandler(this.AutoNotePromptsGrid_SelectionCommitted);
            // 
            // insertButton
            // 
            this.insertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.insertButton.Image = global::Imedisoft.Properties.Resources.IconArrowLeft;
            this.insertButton.Location = new System.Drawing.Point(566, 280);
            this.insertButton.Name = "insertButton";
            this.insertButton.Size = new System.Drawing.Size(30, 25);
            this.insertButton.TabIndex = 4;
            this.insertButton.Click += new System.EventHandler(this.InsertButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(686, 584);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 9;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(772, 584);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "&Cancel";
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPenBlack;
            this.editButton.Location = new System.Drawing.Point(786, 30);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(30, 25);
            this.editButton.TabIndex = 6;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(822, 30);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 7;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormAutoNoteEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(864, 621);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.mainTextLabel);
            this.Controls.Add(this.mainTextTextBox);
            this.Controls.Add(this.autoNotePromptsGrid);
            this.Controls.Add(this.insertButton);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAutoNoteEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Note";
            this.Load += new System.EventHandler(this.FormAutoNoteEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.Label nameLabel;
		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button insertButton;
		private OpenDental.UI.ODGrid autoNotePromptsGrid;
		private System.Windows.Forms.TextBox mainTextTextBox;
		private System.Windows.Forms.Label mainTextLabel;
		private OpenDental.UI.Button addButton;
        private OpenDental.UI.Button editButton;
        private OpenDental.UI.Button deleteButton;
    }
}
