namespace Imedisoft.CEMT.Forms
{
	partial class FormCentralConnectionGroupEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralConnectionGroupEdit));
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.acceptButton = new OpenDental.UI.Button();
            this.mainGrid = new OpenDental.UI.ODGrid();
            this.deleteButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.removeButton = new OpenDental.UI.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.availableGrid = new OpenDental.UI.ODGrid();
            this.cancelButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(22, 16);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(92, 13);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Group Description";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(120, 13);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(237, 20);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(845, 523);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 8;
            this.acceptButton.Text = "OK";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // mainGrid
            // 
            this.mainGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.mainGrid.Location = new System.Drawing.Point(13, 39);
            this.mainGrid.Name = "mainGrid";
            this.mainGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.mainGrid.Size = new System.Drawing.Size(450, 478);
            this.mainGrid.TabIndex = 3;
            this.mainGrid.Title = "Connections for this Group";
            this.mainGrid.TranslationName = "TableConnections";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.CEMT.Properties.Resources.IconRemove;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(13, 523);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 7;
            this.deleteButton.Text = "Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // addButton
            // 
            this.addButton.Image = global::Imedisoft.CEMT.Properties.Resources.IconLeft;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(472, 220);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 25);
            this.addButton.TabIndex = 4;
            this.addButton.Text = "&Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Image = global::Imedisoft.CEMT.Properties.Resources.IconRight;
            this.removeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.removeButton.Location = new System.Drawing.Point(472, 251);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(80, 25);
            this.removeButton.TabIndex = 5;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(363, 16);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(209, 13);
            this.infoLabel.TabIndex = 2;
            this.infoLabel.Text = "Connections can belong to multiple groups";
            // 
            // availableGrid
            // 
            this.availableGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.availableGrid.Location = new System.Drawing.Point(561, 39);
            this.availableGrid.Name = "availableGrid";
            this.availableGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.availableGrid.Size = new System.Drawing.Size(450, 478);
            this.availableGrid.TabIndex = 6;
            this.availableGrid.Title = "Available Connections";
            this.availableGrid.TranslationName = "TableConnections";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(931, 523);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // FormCentralConnectionGroupEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(1024, 561);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.availableGrid);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.mainGrid);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCentralConnectionGroupEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connection Group";
            this.Load += new System.EventHandler(this.FormCentralConnectionGroupEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private OpenDental.UI.ODGrid mainGrid;
		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button deleteButton;
		private OpenDental.UI.Button addButton;
		private OpenDental.UI.Button removeButton;
		private System.Windows.Forms.Label infoLabel;
		private OpenDental.UI.ODGrid availableGrid;
		private OpenDental.UI.Button cancelButton;
	}
}
