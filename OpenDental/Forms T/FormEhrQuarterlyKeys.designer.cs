namespace Imedisoft.Forms
{
	partial class FormEhrQuarterlyKeys
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEhrQuarterlyKeys));
            this.cancelButton = new OpenDental.UI.Button();
            this.ehrQuarterlyKeysGrid = new OpenDental.UI.ODGrid();
            this.infoLabel = new System.Windows.Forms.Label();
            this.practiceTitleLabel = new System.Windows.Forms.Label();
            this.practiceTitleTextBox = new System.Windows.Forms.TextBox();
            this.addButton = new OpenDental.UI.Button();
            this.editButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(452, 444);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Close";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ehrQuarterlyKeysGrid
            // 
            this.ehrQuarterlyKeysGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ehrQuarterlyKeysGrid.Location = new System.Drawing.Point(12, 125);
            this.ehrQuarterlyKeysGrid.Name = "ehrQuarterlyKeysGrid";
            this.ehrQuarterlyKeysGrid.Size = new System.Drawing.Size(520, 313);
            this.ehrQuarterlyKeysGrid.TabIndex = 4;
            this.ehrQuarterlyKeysGrid.Title = "Keys";
            this.ehrQuarterlyKeysGrid.TranslationName = "TableKeys";
            this.ehrQuarterlyKeysGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.EhrQuarterlyKeysGrid_CellDoubleClick);
            this.ehrQuarterlyKeysGrid.SelectionCommitted += new System.EventHandler(this.EhrQuarterlyKeysGrid_SelectionCommitted);
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(12, 9);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(520, 80);
            this.infoLabel.TabIndex = 1;
            this.infoLabel.Text = resources.GetString("infoLabel.Text");
            // 
            // practiceTitleLabel
            // 
            this.practiceTitleLabel.AutoSize = true;
            this.practiceTitleLabel.Location = new System.Drawing.Point(36, 95);
            this.practiceTitleLabel.Name = "practiceTitleLabel";
            this.practiceTitleLabel.Size = new System.Drawing.Size(68, 13);
            this.practiceTitleLabel.TabIndex = 2;
            this.practiceTitleLabel.Text = "Practice Title";
            // 
            // practiceTitleTextBox
            // 
            this.practiceTitleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.practiceTitleTextBox.Location = new System.Drawing.Point(110, 92);
            this.practiceTitleTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.practiceTitleTextBox.Name = "practiceTitleTextBox";
            this.practiceTitleTextBox.ReadOnly = true;
            this.practiceTitleTextBox.Size = new System.Drawing.Size(422, 20);
            this.practiceTitleTextBox.TabIndex = 3;
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(12, 444);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 5;
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPenBlack;
            this.editButton.Location = new System.Drawing.Point(48, 444);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(30, 25);
            this.editButton.TabIndex = 6;
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(84, 444);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 7;
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormEhrQuarterlyKeys
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(544, 481);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.practiceTitleTextBox);
            this.Controls.Add(this.practiceTitleLabel);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.ehrQuarterlyKeysGrid);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEhrQuarterlyKeys";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EHR Quarterly Keys";
            this.Load += new System.EventHandler(this.FormEhrQuarterlyKeys_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.ODGrid ehrQuarterlyKeysGrid;
		private System.Windows.Forms.Label infoLabel;
		private System.Windows.Forms.Label practiceTitleLabel;
		private System.Windows.Forms.TextBox practiceTitleTextBox;
		private OpenDental.UI.Button addButton;
        private OpenDental.UI.Button editButton;
        private OpenDental.UI.Button deleteButton;
    }
}
