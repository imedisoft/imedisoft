namespace Imedisoft.Forms
{
    partial class FormRxEdit
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
            this.sigTextBox = new System.Windows.Forms.TextBox();
            this.dispTextBox = new System.Windows.Forms.TextBox();
            this.refillsTextBox = new System.Windows.Forms.TextBox();
            this.drugTextBox = new System.Windows.Forms.TextBox();
            this.sigLabel = new System.Windows.Forms.Label();
            this.dispLabel = new System.Windows.Forms.Label();
            this.refillsLabel = new System.Windows.Forms.Label();
            this.notesLabel = new System.Windows.Forms.Label();
            this.drugLabel = new System.Windows.Forms.Label();
            this.dateLabel = new System.Windows.Forms.Label();
            this.dateTextBox = new OpenDental.ValidDate();
            this.printButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.pharmacyLabel = new System.Windows.Forms.Label();
            this.pharmacyButton = new OpenDental.UI.Button();
            this.pharmacyTextBox = new System.Windows.Forms.TextBox();
            this.checkControlled = new System.Windows.Forms.CheckBox();
            this.viewButton = new OpenDental.UI.Button();
            this.viewLabel = new System.Windows.Forms.Label();
            this.sendStatusLabel = new System.Windows.Forms.Label();
            this.sendStatusComboBox = new System.Windows.Forms.ComboBox();
            this.dosageCodeTextBox = new System.Windows.Forms.TextBox();
            this.dosageCodeLabel = new System.Windows.Forms.Label();
            this.providerComboBox = new OpenDental.UI.ComboBoxPlus();
            this.providerButton = new OpenDental.UI.Button();
            this.providerLabel = new System.Windows.Forms.Label();
            this.notesTextBox = new OpenDental.ODtextBox();
            this.cpoeLabel = new System.Windows.Forms.Label();
            this.auditButton = new OpenDental.UI.Button();
            this.pharmacyInfoLabel = new System.Windows.Forms.Label();
            this.pharmacyInfoTextBox = new OpenDental.ODtextBox();
            this.procRequiredCheckBox = new System.Windows.Forms.CheckBox();
            this.procedureComboBox = new System.Windows.Forms.ComboBox();
            this.procedureLabel = new System.Windows.Forms.Label();
            this.daysOfSupplyLabel = new System.Windows.Forms.Label();
            this.daysOfSupplyTextBox = new OpenDental.ValidDouble();
            this.patInstructionsTextBox = new OpenDental.ODtextBox();
            this.patInstructionsLabel = new System.Windows.Forms.Label();
            this.printPatInstructionsButton = new OpenDental.UI.Button();
            this.comboClinic = new OpenDental.UI.ComboBoxClinicPicker();
            this.notesInfoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(552, 654);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 41;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(552, 623);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 40;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // sigTextBox
            // 
            this.sigTextBox.AcceptsReturn = true;
            this.sigTextBox.Location = new System.Drawing.Point(160, 163);
            this.sigTextBox.Multiline = true;
            this.sigTextBox.Name = "sigTextBox";
            this.sigTextBox.Size = new System.Drawing.Size(350, 50);
            this.sigTextBox.TabIndex = 11;
            // 
            // dispTextBox
            // 
            this.dispTextBox.Location = new System.Drawing.Point(160, 219);
            this.dispTextBox.Name = "dispTextBox";
            this.dispTextBox.Size = new System.Drawing.Size(80, 20);
            this.dispTextBox.TabIndex = 13;
            // 
            // refillsTextBox
            // 
            this.refillsTextBox.Location = new System.Drawing.Point(340, 219);
            this.refillsTextBox.Name = "refillsTextBox";
            this.refillsTextBox.Size = new System.Drawing.Size(80, 20);
            this.refillsTextBox.TabIndex = 15;
            // 
            // drugTextBox
            // 
            this.drugTextBox.Location = new System.Drawing.Point(160, 137);
            this.drugTextBox.Name = "drugTextBox";
            this.drugTextBox.Size = new System.Drawing.Size(260, 20);
            this.drugTextBox.TabIndex = 9;
            // 
            // sigLabel
            // 
            this.sigLabel.AutoSize = true;
            this.sigLabel.Location = new System.Drawing.Point(130, 166);
            this.sigLabel.Name = "sigLabel";
            this.sigLabel.Size = new System.Drawing.Size(24, 13);
            this.sigLabel.TabIndex = 10;
            this.sigLabel.Text = "SIG";
            // 
            // dispLabel
            // 
            this.dispLabel.AutoSize = true;
            this.dispLabel.Location = new System.Drawing.Point(104, 222);
            this.dispLabel.Name = "dispLabel";
            this.dispLabel.Size = new System.Drawing.Size(50, 13);
            this.dispLabel.TabIndex = 12;
            this.dispLabel.Text = "Dispense";
            // 
            // refillsLabel
            // 
            this.refillsLabel.AutoSize = true;
            this.refillsLabel.Location = new System.Drawing.Point(299, 222);
            this.refillsLabel.Name = "refillsLabel";
            this.refillsLabel.Size = new System.Drawing.Size(35, 13);
            this.refillsLabel.TabIndex = 14;
            this.refillsLabel.Text = "Refills";
            // 
            // notesLabel
            // 
            this.notesLabel.AutoSize = true;
            this.notesLabel.Location = new System.Drawing.Point(119, 301);
            this.notesLabel.Name = "notesLabel";
            this.notesLabel.Size = new System.Drawing.Size(35, 13);
            this.notesLabel.TabIndex = 21;
            this.notesLabel.Text = "Notes";
            // 
            // drugLabel
            // 
            this.drugLabel.AutoSize = true;
            this.drugLabel.Location = new System.Drawing.Point(124, 140);
            this.drugLabel.Name = "drugLabel";
            this.drugLabel.Size = new System.Drawing.Size(30, 13);
            this.drugLabel.TabIndex = 8;
            this.drugLabel.Text = "Drug";
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(124, 15);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(30, 13);
            this.dateLabel.TabIndex = 0;
            this.dateLabel.Text = "Date";
            // 
            // dateTextBox
            // 
            this.dateTextBox.Location = new System.Drawing.Point(160, 12);
            this.dateTextBox.Name = "dateTextBox";
            this.dateTextBox.Size = new System.Drawing.Size(100, 20);
            this.dateTextBox.TabIndex = 1;
            // 
            // printButton
            // 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.printButton.Image = global::Imedisoft.Properties.Resources.IconPrint;
            this.printButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.printButton.Location = new System.Drawing.Point(324, 654);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(80, 25);
            this.printButton.TabIndex = 38;
            this.printButton.Text = "&Print";
            this.printButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTrash;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(12, 654);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 34;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // pharmacyLabel
            // 
            this.pharmacyLabel.AutoSize = true;
            this.pharmacyLabel.Location = new System.Drawing.Point(100, 529);
            this.pharmacyLabel.Name = "pharmacyLabel";
            this.pharmacyLabel.Size = new System.Drawing.Size(54, 13);
            this.pharmacyLabel.TabIndex = 28;
            this.pharmacyLabel.Text = "Pharmacy";
            // 
            // pharmacyButton
            // 
            this.pharmacyButton.Location = new System.Drawing.Point(366, 526);
            this.pharmacyButton.Name = "pharmacyButton";
            this.pharmacyButton.Size = new System.Drawing.Size(25, 20);
            this.pharmacyButton.TabIndex = 30;
            this.pharmacyButton.Text = "...";
            this.pharmacyButton.Click += new System.EventHandler(this.PharmacyButton_Click);
            // 
            // pharmacyTextBox
            // 
            this.pharmacyTextBox.AcceptsReturn = true;
            this.pharmacyTextBox.Location = new System.Drawing.Point(160, 526);
            this.pharmacyTextBox.Name = "pharmacyTextBox";
            this.pharmacyTextBox.ReadOnly = true;
            this.pharmacyTextBox.Size = new System.Drawing.Size(200, 20);
            this.pharmacyTextBox.TabIndex = 29;
            this.pharmacyTextBox.TabStop = false;
            // 
            // checkControlled
            // 
            this.checkControlled.AutoSize = true;
            this.checkControlled.Location = new System.Drawing.Point(160, 38);
            this.checkControlled.Name = "checkControlled";
            this.checkControlled.Size = new System.Drawing.Size(128, 17);
            this.checkControlled.TabIndex = 2;
            this.checkControlled.Text = "Controlled Substance";
            this.checkControlled.UseVisualStyleBackColor = true;
            // 
            // viewButton
            // 
            this.viewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.viewButton.Image = global::Imedisoft.Properties.Resources.printPreview20;
            this.viewButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.viewButton.Location = new System.Drawing.Point(410, 654);
            this.viewButton.Name = "viewButton";
            this.viewButton.Size = new System.Drawing.Size(80, 25);
            this.viewButton.TabIndex = 39;
            this.viewButton.Text = "&View";
            this.viewButton.Click += new System.EventHandler(this.ViewButton_Click);
            // 
            // viewLabel
            // 
            this.viewLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.viewLabel.AutoSize = true;
            this.viewLabel.Location = new System.Drawing.Point(276, 635);
            this.viewLabel.Name = "viewLabel";
            this.viewLabel.Size = new System.Drawing.Size(169, 13);
            this.viewLabel.TabIndex = 36;
            this.viewLabel.Text = "This Rx has already been printed.";
            // 
            // sendStatusLabel
            // 
            this.sendStatusLabel.AutoSize = true;
            this.sendStatusLabel.Location = new System.Drawing.Point(89, 555);
            this.sendStatusLabel.Name = "sendStatusLabel";
            this.sendStatusLabel.Size = new System.Drawing.Size(65, 13);
            this.sendStatusLabel.TabIndex = 31;
            this.sendStatusLabel.Text = "Send Status";
            // 
            // sendStatusComboBox
            // 
            this.sendStatusComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sendStatusComboBox.FormattingEnabled = true;
            this.sendStatusComboBox.Location = new System.Drawing.Point(160, 552);
            this.sendStatusComboBox.Name = "sendStatusComboBox";
            this.sendStatusComboBox.Size = new System.Drawing.Size(200, 21);
            this.sendStatusComboBox.TabIndex = 32;
            // 
            // dosageCodeTextBox
            // 
            this.dosageCodeTextBox.Location = new System.Drawing.Point(160, 272);
            this.dosageCodeTextBox.Name = "dosageCodeTextBox";
            this.dosageCodeTextBox.Size = new System.Drawing.Size(114, 20);
            this.dosageCodeTextBox.TabIndex = 20;
            // 
            // dosageCodeLabel
            // 
            this.dosageCodeLabel.AutoSize = true;
            this.dosageCodeLabel.Location = new System.Drawing.Point(83, 275);
            this.dosageCodeLabel.Name = "dosageCodeLabel";
            this.dosageCodeLabel.Size = new System.Drawing.Size(71, 13);
            this.dosageCodeLabel.TabIndex = 19;
            this.dosageCodeLabel.Text = "Dosage Code";
            // 
            // providerComboBox
            // 
            this.providerComboBox.Location = new System.Drawing.Point(160, 245);
            this.providerComboBox.Name = "providerComboBox";
            this.providerComboBox.Size = new System.Drawing.Size(260, 20);
            this.providerComboBox.TabIndex = 17;
            // 
            // providerButton
            // 
            this.providerButton.Location = new System.Drawing.Point(426, 245);
            this.providerButton.Name = "providerButton";
            this.providerButton.Size = new System.Drawing.Size(25, 20);
            this.providerButton.TabIndex = 18;
            this.providerButton.Text = "...";
            this.providerButton.Click += new System.EventHandler(this.ProviderButton_Click);
            // 
            // providerLabel
            // 
            this.providerLabel.AutoSize = true;
            this.providerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.providerLabel.Location = new System.Drawing.Point(108, 249);
            this.providerLabel.Name = "providerLabel";
            this.providerLabel.Size = new System.Drawing.Size(46, 13);
            this.providerLabel.TabIndex = 16;
            this.providerLabel.Text = "Provider";
            // 
            // notesTextBox
            // 
            this.notesTextBox.AcceptsTab = true;
            this.notesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.notesTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.notesTextBox.DetectLinksEnabled = false;
            this.notesTextBox.DetectUrls = false;
            this.notesTextBox.Location = new System.Drawing.Point(160, 298);
            this.notesTextBox.Name = "notesTextBox";
            this.notesTextBox.QuickPasteType = OpenDentBusiness.QuickPasteType.Rx;
            this.notesTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.notesTextBox.Size = new System.Drawing.Size(472, 80);
            this.notesTextBox.TabIndex = 23;
            this.notesTextBox.Text = "";
            // 
            // cpoeLabel
            // 
            this.cpoeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cpoeLabel.AutoSize = true;
            this.cpoeLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cpoeLabel.ForeColor = System.Drawing.Color.OrangeRed;
            this.cpoeLabel.Location = new System.Drawing.Point(427, 9);
            this.cpoeLabel.Name = "cpoeLabel";
            this.cpoeLabel.Size = new System.Drawing.Size(205, 13);
            this.cpoeLabel.TabIndex = 42;
            this.cpoeLabel.Text = "Computerized Provider Order Entry";
            this.cpoeLabel.Visible = false;
            // 
            // auditButton
            // 
            this.auditButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.auditButton.Location = new System.Drawing.Point(125, 654);
            this.auditButton.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.auditButton.Name = "auditButton";
            this.auditButton.Size = new System.Drawing.Size(80, 25);
            this.auditButton.TabIndex = 35;
            this.auditButton.Text = "&Audit Trail";
            this.auditButton.Click += new System.EventHandler(this.AuditButton_Click);
            // 
            // pharmacyInfoLabel
            // 
            this.pharmacyInfoLabel.AutoSize = true;
            this.pharmacyInfoLabel.Location = new System.Drawing.Point(55, 473);
            this.pharmacyInfoLabel.Name = "pharmacyInfoLabel";
            this.pharmacyInfoLabel.Size = new System.Drawing.Size(99, 13);
            this.pharmacyInfoLabel.TabIndex = 26;
            this.pharmacyInfoLabel.Text = "eRX Pharmacy Info";
            // 
            // pharmacyInfoTextBox
            // 
            this.pharmacyInfoTextBox.AcceptsTab = true;
            this.pharmacyInfoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pharmacyInfoTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.pharmacyInfoTextBox.DetectLinksEnabled = false;
            this.pharmacyInfoTextBox.DetectUrls = false;
            this.pharmacyInfoTextBox.Location = new System.Drawing.Point(160, 470);
            this.pharmacyInfoTextBox.Name = "pharmacyInfoTextBox";
            this.pharmacyInfoTextBox.QuickPasteType = OpenDentBusiness.QuickPasteType.Rx;
            this.pharmacyInfoTextBox.ReadOnly = true;
            this.pharmacyInfoTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.pharmacyInfoTextBox.Size = new System.Drawing.Size(472, 50);
            this.pharmacyInfoTextBox.TabIndex = 27;
            this.pharmacyInfoTextBox.TabStop = false;
            this.pharmacyInfoTextBox.Text = "";
            // 
            // procRequiredCheckBox
            // 
            this.procRequiredCheckBox.AutoSize = true;
            this.procRequiredCheckBox.Location = new System.Drawing.Point(160, 61);
            this.procRequiredCheckBox.Name = "procRequiredCheckBox";
            this.procRequiredCheckBox.Size = new System.Drawing.Size(133, 17);
            this.procRequiredCheckBox.TabIndex = 3;
            this.procRequiredCheckBox.Text = "Is Procedure Required";
            this.procRequiredCheckBox.UseVisualStyleBackColor = true;
            // 
            // procedureComboBox
            // 
            this.procedureComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.procedureComboBox.FormattingEnabled = true;
            this.procedureComboBox.Location = new System.Drawing.Point(160, 84);
            this.procedureComboBox.Name = "procedureComboBox";
            this.procedureComboBox.Size = new System.Drawing.Size(260, 21);
            this.procedureComboBox.TabIndex = 5;
            // 
            // procedureLabel
            // 
            this.procedureLabel.AutoSize = true;
            this.procedureLabel.Location = new System.Drawing.Point(98, 87);
            this.procedureLabel.Name = "procedureLabel";
            this.procedureLabel.Size = new System.Drawing.Size(56, 13);
            this.procedureLabel.TabIndex = 4;
            this.procedureLabel.Text = "Procedure";
            // 
            // daysOfSupplyLabel
            // 
            this.daysOfSupplyLabel.AutoSize = true;
            this.daysOfSupplyLabel.Location = new System.Drawing.Point(75, 114);
            this.daysOfSupplyLabel.Name = "daysOfSupplyLabel";
            this.daysOfSupplyLabel.Size = new System.Drawing.Size(79, 13);
            this.daysOfSupplyLabel.TabIndex = 6;
            this.daysOfSupplyLabel.Text = "Days of Supply";
            // 
            // daysOfSupplyTextBox
            // 
            this.daysOfSupplyTextBox.Location = new System.Drawing.Point(160, 111);
            this.daysOfSupplyTextBox.MaxVal = 99999999D;
            this.daysOfSupplyTextBox.MinVal = 0D;
            this.daysOfSupplyTextBox.Name = "daysOfSupplyTextBox";
            this.daysOfSupplyTextBox.Size = new System.Drawing.Size(80, 20);
            this.daysOfSupplyTextBox.TabIndex = 7;
            // 
            // patInstructionsTextBox
            // 
            this.patInstructionsTextBox.AcceptsTab = true;
            this.patInstructionsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.patInstructionsTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.patInstructionsTextBox.DetectLinksEnabled = false;
            this.patInstructionsTextBox.DetectUrls = false;
            this.patInstructionsTextBox.Location = new System.Drawing.Point(160, 384);
            this.patInstructionsTextBox.Name = "patInstructionsTextBox";
            this.patInstructionsTextBox.QuickPasteType = OpenDentBusiness.QuickPasteType.Rx;
            this.patInstructionsTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.patInstructionsTextBox.Size = new System.Drawing.Size(472, 80);
            this.patInstructionsTextBox.TabIndex = 25;
            this.patInstructionsTextBox.Text = "";
            // 
            // patInstructionsLabel
            // 
            this.patInstructionsLabel.AutoSize = true;
            this.patInstructionsLabel.Location = new System.Drawing.Point(53, 387);
            this.patInstructionsLabel.Name = "patInstructionsLabel";
            this.patInstructionsLabel.Size = new System.Drawing.Size(101, 13);
            this.patInstructionsLabel.TabIndex = 24;
            this.patInstructionsLabel.Text = "Patient Instructions";
            // 
            // printPatInstructionsButton
            // 
            this.printPatInstructionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.printPatInstructionsButton.Image = global::Imedisoft.Properties.Resources.IconPrint;
            this.printPatInstructionsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.printPatInstructionsButton.Location = new System.Drawing.Point(238, 654);
            this.printPatInstructionsButton.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.printPatInstructionsButton.Name = "printPatInstructionsButton";
            this.printPatInstructionsButton.Size = new System.Drawing.Size(80, 25);
            this.printPatInstructionsButton.TabIndex = 37;
            this.printPatInstructionsButton.Text = "Pat &Instr.";
            this.printPatInstructionsButton.Click += new System.EventHandler(this.PrintPatInstructionsButton_Click);
            // 
            // comboClinic
            // 
            this.comboClinic.ForceShowUnassigned = true;
            this.comboClinic.IncludeUnassigned = true;
            this.comboClinic.Location = new System.Drawing.Point(123, 579);
            this.comboClinic.Name = "comboClinic";
            this.comboClinic.Size = new System.Drawing.Size(238, 21);
            this.comboClinic.TabIndex = 33;
            this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.ClinicComboBox_SelectionChangeCommitted);
            // 
            // notesInfoLabel
            // 
            this.notesInfoLabel.AutoSize = true;
            this.notesInfoLabel.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notesInfoLabel.Location = new System.Drawing.Point(41, 319);
            this.notesInfoLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.notesInfoLabel.Name = "notesInfoLabel";
            this.notesInfoLabel.Size = new System.Drawing.Size(113, 11);
            this.notesInfoLabel.TabIndex = 22;
            this.notesInfoLabel.Text = "(will not show on printout)";
            // 
            // FormRxEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(644, 691);
            this.Controls.Add(this.notesInfoLabel);
            this.Controls.Add(this.comboClinic);
            this.Controls.Add(this.printPatInstructionsButton);
            this.Controls.Add(this.patInstructionsTextBox);
            this.Controls.Add(this.patInstructionsLabel);
            this.Controls.Add(this.daysOfSupplyTextBox);
            this.Controls.Add(this.daysOfSupplyLabel);
            this.Controls.Add(this.procedureLabel);
            this.Controls.Add(this.procedureComboBox);
            this.Controls.Add(this.procRequiredCheckBox);
            this.Controls.Add(this.pharmacyInfoTextBox);
            this.Controls.Add(this.pharmacyInfoLabel);
            this.Controls.Add(this.auditButton);
            this.Controls.Add(this.cpoeLabel);
            this.Controls.Add(this.providerComboBox);
            this.Controls.Add(this.providerButton);
            this.Controls.Add(this.providerLabel);
            this.Controls.Add(this.dosageCodeTextBox);
            this.Controls.Add(this.dosageCodeLabel);
            this.Controls.Add(this.sendStatusComboBox);
            this.Controls.Add(this.sendStatusLabel);
            this.Controls.Add(this.viewLabel);
            this.Controls.Add(this.viewButton);
            this.Controls.Add(this.checkControlled);
            this.Controls.Add(this.pharmacyButton);
            this.Controls.Add(this.pharmacyTextBox);
            this.Controls.Add(this.pharmacyLabel);
            this.Controls.Add(this.notesTextBox);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.dateTextBox);
            this.Controls.Add(this.sigTextBox);
            this.Controls.Add(this.dispTextBox);
            this.Controls.Add(this.refillsTextBox);
            this.Controls.Add(this.drugTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.sigLabel);
            this.Controls.Add(this.dispLabel);
            this.Controls.Add(this.refillsLabel);
            this.Controls.Add(this.notesLabel);
            this.Controls.Add(this.drugLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRxEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Rx";
            this.Load += new System.EventHandler(this.FormRxEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.TextBox sigTextBox;
		private System.Windows.Forms.TextBox dispTextBox;
		private System.Windows.Forms.TextBox refillsTextBox;
		private System.Windows.Forms.TextBox drugTextBox;
		private System.Windows.Forms.Label sigLabel;
		private System.Windows.Forms.Label dispLabel;
		private System.Windows.Forms.Label refillsLabel;
		private System.Windows.Forms.Label notesLabel;
		private System.Windows.Forms.Label drugLabel;
		private System.Windows.Forms.Label dateLabel;
		private OpenDental.ValidDate dateTextBox;
		private OpenDental.UI.Button printButton;
		private OpenDental.UI.Button deleteButton;
		private OpenDental.ODtextBox notesTextBox;
		private System.Windows.Forms.Label pharmacyLabel;
		private OpenDental.UI.Button pharmacyButton;
		private System.Windows.Forms.TextBox pharmacyTextBox;
		private System.Windows.Forms.CheckBox checkControlled;
		private OpenDental.UI.Button viewButton;
		private System.Windows.Forms.Label viewLabel;
		private System.Windows.Forms.Label sendStatusLabel;
		private System.Windows.Forms.ComboBox sendStatusComboBox;
		private System.Windows.Forms.TextBox dosageCodeTextBox;
		private System.Windows.Forms.Label dosageCodeLabel;
		private OpenDental.UI.ComboBoxPlus providerComboBox;
		private OpenDental.UI.Button providerButton;
		private System.Windows.Forms.Label providerLabel;
		private System.Windows.Forms.Label cpoeLabel;
		private OpenDental.UI.Button auditButton;
		private System.Windows.Forms.Label pharmacyInfoLabel;
		private OpenDental.ODtextBox pharmacyInfoTextBox;
		private System.Windows.Forms.CheckBox procRequiredCheckBox;
		private System.Windows.Forms.ComboBox procedureComboBox;
		private System.Windows.Forms.Label procedureLabel;
		private System.Windows.Forms.Label daysOfSupplyLabel;
		private OpenDental.ValidDouble daysOfSupplyTextBox;
		private OpenDental.ODtextBox patInstructionsTextBox;
		private System.Windows.Forms.Label patInstructionsLabel;
		private OpenDental.UI.Button printPatInstructionsButton;
		private OpenDental.UI.ComboBoxClinicPicker comboClinic;
        private System.Windows.Forms.Label notesInfoLabel;
    }
}
