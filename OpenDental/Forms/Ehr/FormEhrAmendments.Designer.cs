namespace Imedisoft.Forms
{
	partial class FormEhrAmendments
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
            this.ehrAmendmentsGrid = new OpenDental.UI.ODGrid();
            this.cancelButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.editButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // ehrAmendmentsGrid
            // 
            this.ehrAmendmentsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ehrAmendmentsGrid.ColorSelectedRow = System.Drawing.SystemColors.Highlight;
            this.ehrAmendmentsGrid.Location = new System.Drawing.Point(12, 52);
            this.ehrAmendmentsGrid.Name = "ehrAmendmentsGrid";
            this.ehrAmendmentsGrid.Size = new System.Drawing.Size(600, 286);
            this.ehrAmendmentsGrid.TabIndex = 25;
            this.ehrAmendmentsGrid.Title = "Amendments";
            this.ehrAmendmentsGrid.TranslationName = "TableAmendments";
            this.ehrAmendmentsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.EhrAmendmentsGrid_CellDoubleClick);
            this.ehrAmendmentsGrid.SelectionCommitted += new System.EventHandler(this.EhrAmendmentsGrid_SelectionCommitted);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(532, 344);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 28;
            this.cancelButton.Text = "&Close";
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(12, 344);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 29;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(12, 9);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(600, 40);
            this.infoLabel.TabIndex = 130;
            this.infoLabel.Text = "Patients can bring in documents and request that they be appended to their record" +
    ".\r\nThey can then be scanned or the hardcopy filed.";
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPenBlack;
            this.editButton.Location = new System.Drawing.Point(48, 344);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(30, 25);
            this.editButton.TabIndex = 29;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(84, 344);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 29;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormEhrAmendments
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(624, 381);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.ehrAmendmentsGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEhrAmendments";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Amendments";
            this.Load += new System.EventHandler(this.FormEhrAmendments_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.ODGrid ehrAmendmentsGrid;
		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button addButton;
		private System.Windows.Forms.Label infoLabel;
        private OpenDental.UI.Button editButton;
        private OpenDental.UI.Button deleteButton;
    }
}
