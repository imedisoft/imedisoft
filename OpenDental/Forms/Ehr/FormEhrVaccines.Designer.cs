namespace Imedisoft.Forms
{
	partial class FormEhrVaccines
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
            this.submitButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.exportButton = new OpenDental.UI.Button();
            this.exportLabel = new System.Windows.Forms.Label();
            this.vaccinesGrid = new OpenDental.UI.ODGrid();
            this.exportNoRadioButton = new System.Windows.Forms.RadioButton();
            this.exportYesRadioButton = new System.Windows.Forms.RadioButton();
            this.exportUnknownRadioButton = new System.Windows.Forms.RadioButton();
            this.editButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // submitButton
            // 
            this.submitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.submitButton.Image = global::Imedisoft.Properties.Resources.IconShare;
            this.submitButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.submitButton.Location = new System.Drawing.Point(253, 444);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(100, 25);
            this.submitButton.TabIndex = 5;
            this.submitButton.Text = "Submit HL7";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(12, 444);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(592, 444);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Close";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exportButton.Image = global::Imedisoft.Properties.Resources.IconExport;
            this.exportButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.exportButton.Location = new System.Drawing.Point(147, 444);
            this.exportButton.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(100, 25);
            this.exportButton.TabIndex = 4;
            this.exportButton.Text = "Export HL7";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // exportLabel
            // 
            this.exportLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportLabel.AutoSize = true;
            this.exportLabel.Location = new System.Drawing.Point(313, 14);
            this.exportLabel.Name = "exportLabel";
            this.exportLabel.Size = new System.Drawing.Size(192, 13);
            this.exportLabel.TabIndex = 7;
            this.exportLabel.Text = "Patient allows exporting to other EHRs";
            // 
            // vaccinesGrid
            // 
            this.vaccinesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vaccinesGrid.Location = new System.Drawing.Point(12, 35);
            this.vaccinesGrid.Name = "vaccinesGrid";
            this.vaccinesGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.vaccinesGrid.Size = new System.Drawing.Size(660, 403);
            this.vaccinesGrid.TabIndex = 0;
            this.vaccinesGrid.Title = "Vaccines";
            this.vaccinesGrid.TranslationName = "TableVaccines";
            this.vaccinesGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.VaccinesGrid_CellDoubleClick);
            this.vaccinesGrid.SelectionCommitted += new System.EventHandler(this.VaccinesGrid_SelectionCommitted);
            // 
            // exportNoRadioButton
            // 
            this.exportNoRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportNoRadioButton.AutoSize = true;
            this.exportNoRadioButton.Location = new System.Drawing.Point(559, 12);
            this.exportNoRadioButton.Name = "exportNoRadioButton";
            this.exportNoRadioButton.Size = new System.Drawing.Size(38, 17);
            this.exportNoRadioButton.TabIndex = 9;
            this.exportNoRadioButton.Text = "No";
            this.exportNoRadioButton.UseVisualStyleBackColor = true;
            this.exportNoRadioButton.CheckedChanged += new System.EventHandler(this.ExportRadioButton_CheckedChanged);
            // 
            // exportYesRadioButton
            // 
            this.exportYesRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportYesRadioButton.AutoSize = true;
            this.exportYesRadioButton.Location = new System.Drawing.Point(511, 12);
            this.exportYesRadioButton.Name = "exportYesRadioButton";
            this.exportYesRadioButton.Size = new System.Drawing.Size(42, 17);
            this.exportYesRadioButton.TabIndex = 8;
            this.exportYesRadioButton.Text = "Yes";
            this.exportYesRadioButton.UseVisualStyleBackColor = true;
            this.exportYesRadioButton.CheckedChanged += new System.EventHandler(this.ExportRadioButton_CheckedChanged);
            // 
            // exportUnknownRadioButton
            // 
            this.exportUnknownRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportUnknownRadioButton.AutoSize = true;
            this.exportUnknownRadioButton.Checked = true;
            this.exportUnknownRadioButton.Location = new System.Drawing.Point(603, 12);
            this.exportUnknownRadioButton.Name = "exportUnknownRadioButton";
            this.exportUnknownRadioButton.Size = new System.Drawing.Size(69, 17);
            this.exportUnknownRadioButton.TabIndex = 10;
            this.exportUnknownRadioButton.TabStop = true;
            this.exportUnknownRadioButton.Text = "Unknown";
            this.exportUnknownRadioButton.UseVisualStyleBackColor = true;
            this.exportUnknownRadioButton.CheckedChanged += new System.EventHandler(this.ExportRadioButton_CheckedChanged);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPenBlack;
            this.editButton.Location = new System.Drawing.Point(48, 444);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(30, 25);
            this.editButton.TabIndex = 2;
            this.editButton.Text = "Add";
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
            this.deleteButton.TabIndex = 3;
            this.deleteButton.Text = "Add";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormEhrVaccines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(684, 481);
            this.Controls.Add(this.exportYesRadioButton);
            this.Controls.Add(this.exportUnknownRadioButton);
            this.Controls.Add(this.exportNoRadioButton);
            this.Controls.Add(this.exportLabel);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.vaccinesGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEhrVaccines";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vaccines";
            this.Load += new System.EventHandler(this.FormVaccines_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button submitButton;
		private OpenDental.UI.Button addButton;
		private OpenDental.UI.ODGrid vaccinesGrid;
		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button exportButton;
		private System.Windows.Forms.Label exportLabel;
        private System.Windows.Forms.RadioButton exportNoRadioButton;
        private System.Windows.Forms.RadioButton exportYesRadioButton;
        private System.Windows.Forms.RadioButton exportUnknownRadioButton;
        private OpenDental.UI.Button editButton;
        private OpenDental.UI.Button deleteButton;
    }
}
