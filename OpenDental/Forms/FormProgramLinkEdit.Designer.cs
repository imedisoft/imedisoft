namespace Imedisoft.Forms
{
    partial class FormProgramLinkEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProgramLinkEdit));
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.enabledCheckBox = new System.Windows.Forms.CheckBox();
            this.deleteButton = new OpenDental.UI.Button();
            this.programNameLabel = new System.Windows.Forms.Label();
            this.programNameTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.pathLabel = new System.Windows.Forms.Label();
            this.textCommandLine = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.toolbarsListBox = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonTextTextBox = new System.Windows.Forms.TextBox();
            this.buttonTextLabel = new System.Windows.Forms.Label();
            this.notesTextBox = new System.Windows.Forms.TextBox();
            this.notesLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.propertiesGrid = new OpenDental.UI.ODGrid();
            this.overrideTextBox = new System.Windows.Forms.TextBox();
            this.overrideLabel = new System.Windows.Forms.Label();
            this.butClear = new OpenDental.UI.Button();
            this.butImport = new OpenDental.UI.Button();
            this.buttonImageLabel = new System.Windows.Forms.Label();
            this.buttonImagePictureBox = new System.Windows.Forms.PictureBox();
            this.hideButtonsCheckBox = new System.Windows.Forms.CheckBox();
            this.clinicsButton = new OpenDental.UI.Button();
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            this.clinicsWarningLabel = new System.Windows.Forms.Label();
            this.disableForClinicLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.buttonImagePictureBox)).BeginInit();
            this.buttonsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(672, 584);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(672, 553);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 1;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // enabledCheckBox
            // 
            this.enabledCheckBox.AutoSize = true;
            this.enabledCheckBox.Location = new System.Drawing.Point(340, 64);
            this.enabledCheckBox.Name = "enabledCheckBox";
            this.enabledCheckBox.Size = new System.Drawing.Size(64, 17);
            this.enabledCheckBox.TabIndex = 41;
            this.enabledCheckBox.Text = "Enabled";
            this.enabledCheckBox.CheckedChanged += new System.EventHandler(this.checkEnabled_CheckedChanged);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimes;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(12, 584);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 43;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // programNameLabel
            // 
            this.programNameLabel.AutoSize = true;
            this.programNameLabel.Location = new System.Drawing.Point(259, 15);
            this.programNameLabel.Name = "programNameLabel";
            this.programNameLabel.Size = new System.Drawing.Size(75, 13);
            this.programNameLabel.TabIndex = 44;
            this.programNameLabel.Text = "Internal Name";
            // 
            // programNameTextBox
            // 
            this.programNameTextBox.Location = new System.Drawing.Point(340, 12);
            this.programNameTextBox.Name = "programNameTextBox";
            this.programNameTextBox.ReadOnly = true;
            this.programNameTextBox.Size = new System.Drawing.Size(280, 20);
            this.programNameTextBox.TabIndex = 45;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(340, 38);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(280, 20);
            this.descriptionTextBox.TabIndex = 47;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(274, 41);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 46;
            this.descriptionLabel.Text = "Description";
            // 
            // pathTextBox
            // 
            this.pathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathTextBox.Location = new System.Drawing.Point(337, 110);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(415, 20);
            this.pathTextBox.TabIndex = 49;
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(232, 113);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(99, 13);
            this.pathLabel.TabIndex = 48;
            this.pathLabel.Text = "Path of file to open";
            // 
            // textCommandLine
            // 
            this.textCommandLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textCommandLine.Location = new System.Drawing.Point(337, 162);
            this.textCommandLine.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.textCommandLine.Name = "textCommandLine";
            this.textCommandLine.Size = new System.Drawing.Size(415, 20);
            this.textCommandLine.TabIndex = 52;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(162, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(168, 13);
            this.label4.TabIndex = 51;
            this.label4.Text = "Optional command line arguments";
            // 
            // toolbarsListBox
            // 
            this.toolbarsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolbarsListBox.IntegralHeight = false;
            this.toolbarsListBox.Location = new System.Drawing.Point(6, 98);
            this.toolbarsListBox.Name = "toolbarsListBox";
            this.toolbarsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.toolbarsListBox.Size = new System.Drawing.Size(188, 141);
            this.toolbarsListBox.TabIndex = 53;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 13);
            this.label6.TabIndex = 56;
            this.label6.Text = "Add a button to these toolbars";
            // 
            // buttonTextTextBox
            // 
            this.buttonTextTextBox.Location = new System.Drawing.Point(337, 265);
            this.buttonTextTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.buttonTextTextBox.Name = "buttonTextTextBox";
            this.buttonTextTextBox.Size = new System.Drawing.Size(200, 20);
            this.buttonTextTextBox.TabIndex = 58;
            // 
            // buttonTextLabel
            // 
            this.buttonTextLabel.AutoSize = true;
            this.buttonTextLabel.Location = new System.Drawing.Point(252, 268);
            this.buttonTextLabel.Name = "buttonTextLabel";
            this.buttonTextLabel.Size = new System.Drawing.Size(79, 13);
            this.buttonTextLabel.TabIndex = 57;
            this.buttonTextLabel.Text = "Text on button";
            // 
            // notesTextBox
            // 
            this.notesTextBox.Location = new System.Drawing.Point(337, 464);
            this.notesTextBox.MaxLength = 4000;
            this.notesTextBox.Multiline = true;
            this.notesTextBox.Name = "notesTextBox";
            this.notesTextBox.Size = new System.Drawing.Size(283, 70);
            this.notesTextBox.TabIndex = 59;
            // 
            // notesLabel
            // 
            this.notesLabel.AutoSize = true;
            this.notesLabel.Location = new System.Drawing.Point(296, 467);
            this.notesLabel.Name = "notesLabel";
            this.notesLabel.Size = new System.Drawing.Size(35, 13);
            this.notesLabel.TabIndex = 60;
            this.notesLabel.Text = "Notes";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.Location = new System.Drawing.Point(334, 202);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(418, 60);
            this.label9.TabIndex = 61;
            this.label9.Text = resources.GetString("label9.Text");
            // 
            // propertiesGrid
            // 
            this.propertiesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertiesGrid.Location = new System.Drawing.Point(337, 308);
            this.propertiesGrid.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.propertiesGrid.Name = "propertiesGrid";
            this.propertiesGrid.Size = new System.Drawing.Size(415, 133);
            this.propertiesGrid.TabIndex = 62;
            this.propertiesGrid.Title = "Additional Properties";
            this.propertiesGrid.TranslationName = "TableProperties";
            this.propertiesGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.PropertiesGrid_CellDoubleClick);
            // 
            // overrideTextBox
            // 
            this.overrideTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.overrideTextBox.Location = new System.Drawing.Point(337, 136);
            this.overrideTextBox.Name = "overrideTextBox";
            this.overrideTextBox.Size = new System.Drawing.Size(415, 20);
            this.overrideTextBox.TabIndex = 66;
            // 
            // overrideLabel
            // 
            this.overrideLabel.AutoSize = true;
            this.overrideLabel.Location = new System.Drawing.Point(141, 139);
            this.overrideLabel.Name = "overrideLabel";
            this.overrideLabel.Size = new System.Drawing.Size(190, 13);
            this.overrideLabel.TabIndex = 65;
            this.overrideLabel.Text = "Local path override (usually left blank)";
            // 
            // butClear
            // 
            this.butClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butClear.Location = new System.Drawing.Point(68, 273);
            this.butClear.Name = "butClear";
            this.butClear.Size = new System.Drawing.Size(60, 25);
            this.butClear.TabIndex = 70;
            this.butClear.Text = "Clear";
            this.butClear.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // butImport
            // 
            this.butImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butImport.Location = new System.Drawing.Point(134, 273);
            this.butImport.Name = "butImport";
            this.butImport.Size = new System.Drawing.Size(60, 25);
            this.butImport.TabIndex = 69;
            this.butImport.Text = "Import";
            this.butImport.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // buttonImageLabel
            // 
            this.buttonImageLabel.AutoSize = true;
            this.buttonImageLabel.Location = new System.Drawing.Point(53, 249);
            this.buttonImageLabel.Name = "buttonImageLabel";
            this.buttonImageLabel.Size = new System.Drawing.Size(113, 13);
            this.buttonImageLabel.TabIndex = 68;
            this.buttonImageLabel.Text = "Button Image (22x22)";
            // 
            // buttonImagePictureBox
            // 
            this.buttonImagePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.buttonImagePictureBox.Location = new System.Drawing.Point(172, 245);
            this.buttonImagePictureBox.Name = "buttonImagePictureBox";
            this.buttonImagePictureBox.Size = new System.Drawing.Size(22, 22);
            this.buttonImagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.buttonImagePictureBox.TabIndex = 72;
            this.buttonImagePictureBox.TabStop = false;
            // 
            // hideButtonsCheckBox
            // 
            this.hideButtonsCheckBox.AutoSize = true;
            this.hideButtonsCheckBox.Location = new System.Drawing.Point(338, 87);
            this.hideButtonsCheckBox.Name = "hideButtonsCheckBox";
            this.hideButtonsCheckBox.Size = new System.Drawing.Size(121, 17);
            this.hideButtonsCheckBox.TabIndex = 74;
            this.hideButtonsCheckBox.Text = "Hide Unused Button";
            this.hideButtonsCheckBox.CheckedChanged += new System.EventHandler(this.checkHideButtons_CheckedChanged);
            // 
            // clinicsButton
            // 
            this.clinicsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.clinicsButton.Location = new System.Drawing.Point(6, 19);
            this.clinicsButton.Name = "clinicsButton";
            this.clinicsButton.Size = new System.Drawing.Size(140, 25);
            this.clinicsButton.TabIndex = 76;
            this.clinicsButton.Text = "Hide Button for Clinics";
            this.clinicsButton.Click += new System.EventHandler(this.ClinicsButton_Click);
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.clinicsWarningLabel);
            this.buttonsGroupBox.Controls.Add(this.toolbarsListBox);
            this.buttonsGroupBox.Controls.Add(this.label6);
            this.buttonsGroupBox.Controls.Add(this.butClear);
            this.buttonsGroupBox.Controls.Add(this.clinicsButton);
            this.buttonsGroupBox.Controls.Add(this.buttonImagePictureBox);
            this.buttonsGroupBox.Controls.Add(this.butImport);
            this.buttonsGroupBox.Controls.Add(this.buttonImageLabel);
            this.buttonsGroupBox.Location = new System.Drawing.Point(12, 202);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(200, 304);
            this.buttonsGroupBox.TabIndex = 0;
            this.buttonsGroupBox.TabStop = false;
            this.buttonsGroupBox.Text = "Button Settings";
            // 
            // clinicsWarningLabel
            // 
            this.clinicsWarningLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(140)))));
            this.clinicsWarningLabel.Location = new System.Drawing.Point(6, 47);
            this.clinicsWarningLabel.Name = "clinicsWarningLabel";
            this.clinicsWarningLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.clinicsWarningLabel.Size = new System.Drawing.Size(188, 35);
            this.clinicsWarningLabel.TabIndex = 76;
            this.clinicsWarningLabel.Text = "Program Link button is not visible for some clinics.";
            this.clinicsWarningLabel.Visible = false;
            // 
            // disableForClinicLabel
            // 
            this.disableForClinicLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.disableForClinicLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(140)))));
            this.disableForClinicLabel.Location = new System.Drawing.Point(12, 584);
            this.disableForClinicLabel.Name = "disableForClinicLabel";
            this.disableForClinicLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.disableForClinicLabel.Size = new System.Drawing.Size(740, 25);
            this.disableForClinicLabel.TabIndex = 77;
            this.disableForClinicLabel.Text = "User is Clinic restricted, some functions of this window are disabled. To enable," +
    " contact an administrator.";
            this.disableForClinicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.disableForClinicLabel.Visible = false;
            // 
            // FormProgramLinkEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(764, 621);
            this.Controls.Add(this.hideButtonsCheckBox);
            this.Controls.Add(this.overrideTextBox);
            this.Controls.Add(this.overrideLabel);
            this.Controls.Add(this.propertiesGrid);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.notesLabel);
            this.Controls.Add(this.notesTextBox);
            this.Controls.Add(this.buttonTextTextBox);
            this.Controls.Add(this.textCommandLine);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.programNameTextBox);
            this.Controls.Add(this.buttonTextLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.programNameLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.enabledCheckBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.disableForClinicLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(780, 660);
            this.Name = "FormProgramLinkEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Program Link";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProgramLinkEdit_Closing);
            this.Load += new System.EventHandler(this.FormProgramLinkEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.buttonImagePictureBox)).EndInit();
            this.buttonsGroupBox.ResumeLayout(false);
            this.buttonsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.CheckBox enabledCheckBox;
		private OpenDental.UI.Button deleteButton;
		private System.Windows.Forms.Label programNameLabel;
		private System.Windows.Forms.TextBox programNameTextBox;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.Label pathLabel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox pathTextBox;
		private System.Windows.Forms.TextBox textCommandLine;
		private System.Windows.Forms.ListBox toolbarsListBox;
		private System.Windows.Forms.TextBox buttonTextTextBox;
		private System.Windows.Forms.Label buttonTextLabel;
		private System.Windows.Forms.Label notesLabel;
		private System.Windows.Forms.TextBox notesTextBox;
		private System.Windows.Forms.Label label9;
		private OpenDental.UI.ODGrid propertiesGrid;
		private System.Windows.Forms.TextBox overrideTextBox;
		private System.Windows.Forms.Label overrideLabel;
		private OpenDental.UI.Button butClear;
		private OpenDental.UI.Button butImport;
		private System.Windows.Forms.Label buttonImageLabel;
		private System.Windows.Forms.PictureBox buttonImagePictureBox;
		private System.Windows.Forms.CheckBox hideButtonsCheckBox;
		private OpenDental.UI.Button clinicsButton;
		private System.Windows.Forms.GroupBox buttonsGroupBox;
		private System.Windows.Forms.Label clinicsWarningLabel;
		private System.Windows.Forms.Label disableForClinicLabel;
	}
}
