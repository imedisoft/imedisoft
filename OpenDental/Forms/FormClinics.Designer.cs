namespace Imedisoft.Forms
{
    partial class FormClinics
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
            this.moveGroupBox = new System.Windows.Forms.GroupBox();
            this.moveButton = new OpenDental.UI.Button();
            this.pickMoveDestButton = new OpenDental.UI.Button();
            this.moveDestTextBox = new System.Windows.Forms.TextBox();
            this.showHiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.acceptButton = new OpenDental.UI.Button();
            this.clinicsGrid = new OpenDental.UI.ODGrid();
            this.addButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.selectAllButton = new OpenDental.UI.Button();
            this.selectNoneButton = new OpenDental.UI.Button();
            this.moveGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // moveGroupBox
            // 
            this.moveGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveGroupBox.Controls.Add(this.moveButton);
            this.moveGroupBox.Controls.Add(this.pickMoveDestButton);
            this.moveGroupBox.Controls.Add(this.moveDestTextBox);
            this.moveGroupBox.Location = new System.Drawing.Point(652, 43);
            this.moveGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.moveGroupBox.Name = "moveGroupBox";
            this.moveGroupBox.Size = new System.Drawing.Size(280, 90);
            this.moveGroupBox.TabIndex = 1;
            this.moveGroupBox.TabStop = false;
            this.moveGroupBox.Text = "Move patients to...";
            // 
            // moveButton
            // 
            this.moveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveButton.Location = new System.Drawing.Point(194, 47);
            this.moveButton.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(80, 25);
            this.moveButton.TabIndex = 2;
            this.moveButton.Text = "&Move";
            this.moveButton.UseVisualStyleBackColor = true;
            this.moveButton.Click += new System.EventHandler(this.MoveButton_Click);
            // 
            // pickMoveDestButton
            // 
            this.pickMoveDestButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pickMoveDestButton.Location = new System.Drawing.Point(249, 19);
            this.pickMoveDestButton.Name = "pickMoveDestButton";
            this.pickMoveDestButton.Size = new System.Drawing.Size(25, 20);
            this.pickMoveDestButton.TabIndex = 1;
            this.pickMoveDestButton.Text = "...";
            this.pickMoveDestButton.Click += new System.EventHandler(this.PickMoveDestButton_Click);
            // 
            // moveDestTextBox
            // 
            this.moveDestTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveDestTextBox.Location = new System.Drawing.Point(6, 19);
            this.moveDestTextBox.MaxLength = 15;
            this.moveDestTextBox.Name = "moveDestTextBox";
            this.moveDestTextBox.ReadOnly = true;
            this.moveDestTextBox.Size = new System.Drawing.Size(237, 20);
            this.moveDestTextBox.TabIndex = 0;
            // 
            // showHiddenCheckBox
            // 
            this.showHiddenCheckBox.AutoSize = true;
            this.showHiddenCheckBox.Checked = true;
            this.showHiddenCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showHiddenCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.showHiddenCheckBox.Location = new System.Drawing.Point(12, 16);
            this.showHiddenCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.showHiddenCheckBox.Name = "showHiddenCheckBox";
            this.showHiddenCheckBox.Size = new System.Drawing.Size(94, 18);
            this.showHiddenCheckBox.TabIndex = 4;
            this.showHiddenCheckBox.Text = "Show Hidden";
            this.showHiddenCheckBox.UseVisualStyleBackColor = true;
            this.showHiddenCheckBox.CheckedChanged += new System.EventHandler(this.ShowHiddenCheckBox_CheckedChanged);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(852, 493);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 2;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Visible = false;
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // clinicsGrid
            // 
            this.clinicsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clinicsGrid.Location = new System.Drawing.Point(12, 43);
            this.clinicsGrid.Name = "clinicsGrid";
            this.clinicsGrid.Size = new System.Drawing.Size(634, 506);
            this.clinicsGrid.TabIndex = 0;
            this.clinicsGrid.Title = "Clinics";
            this.clinicsGrid.TranslationName = "TableClinics";
            this.clinicsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.ClinicsGrid_CellDoubleClick);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(417, 12);
            this.addButton.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 5;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(852, 524);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Close";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // selectAllButton
            // 
            this.selectAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectAllButton.Location = new System.Drawing.Point(480, 12);
            this.selectAllButton.Name = "selectAllButton";
            this.selectAllButton.Size = new System.Drawing.Size(80, 25);
            this.selectAllButton.TabIndex = 6;
            this.selectAllButton.Text = "Select All";
            this.selectAllButton.UseVisualStyleBackColor = true;
            this.selectAllButton.Visible = false;
            this.selectAllButton.Click += new System.EventHandler(this.SelectAllButton_Click);
            // 
            // selectNoneButton
            // 
            this.selectNoneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectNoneButton.Location = new System.Drawing.Point(566, 12);
            this.selectNoneButton.Name = "selectNoneButton";
            this.selectNoneButton.Size = new System.Drawing.Size(80, 25);
            this.selectNoneButton.TabIndex = 7;
            this.selectNoneButton.Text = "Select None";
            this.selectNoneButton.UseVisualStyleBackColor = true;
            this.selectNoneButton.Visible = false;
            this.selectNoneButton.Click += new System.EventHandler(this.SelectNoneButton_Click);
            // 
            // FormClinics
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(944, 561);
            this.Controls.Add(this.selectNoneButton);
            this.Controls.Add(this.selectAllButton);
            this.Controls.Add(this.moveGroupBox);
            this.Controls.Add(this.showHiddenCheckBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.clinicsGrid);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(780, 480);
            this.Name = "FormClinics";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Clinics";
            this.Load += new System.EventHandler(this.FormClinics_Load);
            this.moveGroupBox.ResumeLayout(false);
            this.moveGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button addButton;
		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.ODGrid clinicsGrid;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.GroupBox moveGroupBox;
		private OpenDental.UI.Button moveButton;
		private OpenDental.UI.Button pickMoveDestButton;
		private System.Windows.Forms.TextBox moveDestTextBox;
		private OpenDental.UI.Button selectAllButton;
		private OpenDental.UI.Button selectNoneButton;
		private System.Windows.Forms.CheckBox showHiddenCheckBox;
	}
}
