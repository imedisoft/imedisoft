namespace Imedisoft.Forms
{
    partial class FormPatientSelect
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
            this.components = new System.ComponentModel.Container();
            this.textLName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupAddPt = new System.Windows.Forms.GroupBox();
            this.butAddAll = new OpenDental.UI.Button();
            this.butAddPt = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.searchGroupBox = new System.Windows.Forms.GroupBox();
            this.textInvoiceNumber = new System.Windows.Forms.TextBox();
            this.labelInvoiceNumber = new System.Windows.Forms.Label();
            this.checkShowMerged = new System.Windows.Forms.CheckBox();
            this.clinicComboBox = new OpenDental.UI.ComboBoxClinicPicker();
            this.textEmail = new System.Windows.Forms.TextBox();
            this.labelEmail = new System.Windows.Forms.Label();
            this.textSubscriberID = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.siteComboBox = new System.Windows.Forms.ComboBox();
            this.siteLabel = new System.Windows.Forms.Label();
            this.billingTypeComboBox = new System.Windows.Forms.ComboBox();
            this.textBirthdate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkShowArchived = new System.Windows.Forms.CheckBox();
            this.textChartNumber = new System.Windows.Forms.TextBox();
            this.textSSN = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.billingTypeLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textPatNum = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textState = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textCity = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkGuarantors = new System.Windows.Forms.CheckBox();
            this.checkShowInactive = new System.Windows.Forms.CheckBox();
            this.textAddress = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textPhone = new OpenDental.ValidPhone();
            this.label4 = new System.Windows.Forms.Label();
            this.textFName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.refreshCheckBox = new System.Windows.Forms.CheckBox();
            this.butGetAll = new OpenDental.UI.Button();
            this.butSearch = new OpenDental.UI.Button();
            this.patientsGrid = new OpenDental.UI.ODGrid();
            this.patientsGridFillTimer = new System.Windows.Forms.Timer(this.components);
            this.groupAddPt.SuspendLayout();
            this.searchGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textLName
            // 
            this.textLName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textLName.Location = new System.Drawing.Point(114, 19);
            this.textLName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.textLName.Name = "textLName";
            this.textLName.Size = new System.Drawing.Size(160, 20);
            this.textLName.TabIndex = 1;
            this.textLName.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textLName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Last Name";
            // 
            // groupAddPt
            // 
            this.groupAddPt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupAddPt.Controls.Add(this.butAddAll);
            this.groupAddPt.Controls.Add(this.butAddPt);
            this.groupAddPt.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupAddPt.Location = new System.Drawing.Point(792, 579);
            this.groupAddPt.Name = "groupAddPt";
            this.groupAddPt.Size = new System.Drawing.Size(280, 60);
            this.groupAddPt.TabIndex = 2;
            this.groupAddPt.TabStop = false;
            this.groupAddPt.Text = "Add New Family";
            // 
            // butAddAll
            // 
            this.butAddAll.Location = new System.Drawing.Point(143, 24);
            this.butAddAll.Name = "butAddAll";
            this.butAddAll.Size = new System.Drawing.Size(80, 25);
            this.butAddAll.TabIndex = 1;
            this.butAddAll.Text = "Add Many";
            this.butAddAll.Click += new System.EventHandler(this.AddManyButton_Click);
            // 
            // butAddPt
            // 
            this.butAddPt.Location = new System.Drawing.Point(57, 24);
            this.butAddPt.Name = "butAddPt";
            this.butAddPt.Size = new System.Drawing.Size(80, 25);
            this.butAddPt.TabIndex = 0;
            this.butAddPt.Text = "&Add Patient";
            this.butAddPt.Click += new System.EventHandler(this.AddPatientButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(906, 664);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(992, 664);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // searchGroupBox
            // 
            this.searchGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchGroupBox.Controls.Add(this.textInvoiceNumber);
            this.searchGroupBox.Controls.Add(this.labelInvoiceNumber);
            this.searchGroupBox.Controls.Add(this.checkShowMerged);
            this.searchGroupBox.Controls.Add(this.clinicComboBox);
            this.searchGroupBox.Controls.Add(this.textEmail);
            this.searchGroupBox.Controls.Add(this.labelEmail);
            this.searchGroupBox.Controls.Add(this.textSubscriberID);
            this.searchGroupBox.Controls.Add(this.label13);
            this.searchGroupBox.Controls.Add(this.siteComboBox);
            this.searchGroupBox.Controls.Add(this.siteLabel);
            this.searchGroupBox.Controls.Add(this.billingTypeComboBox);
            this.searchGroupBox.Controls.Add(this.textBirthdate);
            this.searchGroupBox.Controls.Add(this.label2);
            this.searchGroupBox.Controls.Add(this.checkShowArchived);
            this.searchGroupBox.Controls.Add(this.textChartNumber);
            this.searchGroupBox.Controls.Add(this.textSSN);
            this.searchGroupBox.Controls.Add(this.label12);
            this.searchGroupBox.Controls.Add(this.billingTypeLabel);
            this.searchGroupBox.Controls.Add(this.label10);
            this.searchGroupBox.Controls.Add(this.textPatNum);
            this.searchGroupBox.Controls.Add(this.label9);
            this.searchGroupBox.Controls.Add(this.textState);
            this.searchGroupBox.Controls.Add(this.label8);
            this.searchGroupBox.Controls.Add(this.textCity);
            this.searchGroupBox.Controls.Add(this.label7);
            this.searchGroupBox.Controls.Add(this.checkGuarantors);
            this.searchGroupBox.Controls.Add(this.checkShowInactive);
            this.searchGroupBox.Controls.Add(this.textAddress);
            this.searchGroupBox.Controls.Add(this.label5);
            this.searchGroupBox.Controls.Add(this.textPhone);
            this.searchGroupBox.Controls.Add(this.label4);
            this.searchGroupBox.Controls.Add(this.textFName);
            this.searchGroupBox.Controls.Add(this.label3);
            this.searchGroupBox.Controls.Add(this.textLName);
            this.searchGroupBox.Controls.Add(this.label1);
            this.searchGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.searchGroupBox.Location = new System.Drawing.Point(792, 12);
            this.searchGroupBox.Name = "searchGroupBox";
            this.searchGroupBox.Size = new System.Drawing.Size(280, 480);
            this.searchGroupBox.TabIndex = 0;
            this.searchGroupBox.TabStop = false;
            this.searchGroupBox.Text = "Search Criteria";
            this.searchGroupBox.Enter += new System.EventHandler(this.searchGroupBox_Enter);
            // 
            // textInvoiceNumber
            // 
            this.textInvoiceNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textInvoiceNumber.Location = new System.Drawing.Point(114, 271);
            this.textInvoiceNumber.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.textInvoiceNumber.Name = "textInvoiceNumber";
            this.textInvoiceNumber.Size = new System.Drawing.Size(160, 20);
            this.textInvoiceNumber.TabIndex = 25;
            this.textInvoiceNumber.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textInvoiceNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // labelInvoiceNumber
            // 
            this.labelInvoiceNumber.AutoSize = true;
            this.labelInvoiceNumber.Location = new System.Drawing.Point(26, 274);
            this.labelInvoiceNumber.Name = "labelInvoiceNumber";
            this.labelInvoiceNumber.Size = new System.Drawing.Size(82, 13);
            this.labelInvoiceNumber.TabIndex = 24;
            this.labelInvoiceNumber.Text = "Invoice Number";
            // 
            // checkShowMerged
            // 
            this.checkShowMerged.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkShowMerged.AutoSize = true;
            this.checkShowMerged.Location = new System.Drawing.Point(6, 457);
            this.checkShowMerged.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.checkShowMerged.Name = "checkShowMerged";
            this.checkShowMerged.Size = new System.Drawing.Size(133, 17);
            this.checkShowMerged.TabIndex = 34;
            this.checkShowMerged.Text = "Show Merged Patients";
            this.checkShowMerged.Visible = false;
            this.checkShowMerged.CheckedChanged += new System.EventHandler(this.OnDataEntered);
            // 
            // clinicComboBox
            // 
            this.clinicComboBox.IncludeAll = true;
            this.clinicComboBox.Location = new System.Drawing.Point(77, 358);
            this.clinicComboBox.Name = "clinicComboBox";
            this.clinicComboBox.Size = new System.Drawing.Size(195, 21);
            this.clinicComboBox.TabIndex = 30;
            this.clinicComboBox.SelectionChangeCommitted += new System.EventHandler(this.OnDataEntered);
            // 
            // textEmail
            // 
            this.textEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textEmail.Location = new System.Drawing.Point(114, 250);
            this.textEmail.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.textEmail.Name = "textEmail";
            this.textEmail.Size = new System.Drawing.Size(160, 20);
            this.textEmail.TabIndex = 23;
            this.textEmail.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textEmail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(73, 253);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(35, 13);
            this.labelEmail.TabIndex = 22;
            this.labelEmail.Text = "E-mail";
            // 
            // textSubscriberID
            // 
            this.textSubscriberID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSubscriberID.Location = new System.Drawing.Point(114, 229);
            this.textSubscriberID.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.textSubscriberID.Name = "textSubscriberID";
            this.textSubscriberID.Size = new System.Drawing.Size(160, 20);
            this.textSubscriberID.TabIndex = 21;
            this.textSubscriberID.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textSubscriberID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(37, 232);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(71, 13);
            this.label13.TabIndex = 20;
            this.label13.Text = "Subscriber ID";
            // 
            // siteComboBox
            // 
            this.siteComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.siteComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.siteComboBox.Location = new System.Drawing.Point(114, 331);
            this.siteComboBox.MaxDropDownItems = 40;
            this.siteComboBox.Name = "siteComboBox";
            this.siteComboBox.Size = new System.Drawing.Size(158, 21);
            this.siteComboBox.TabIndex = 29;
            this.siteComboBox.SelectionChangeCommitted += new System.EventHandler(this.OnDataEntered);
            // 
            // siteLabel
            // 
            this.siteLabel.AutoSize = true;
            this.siteLabel.Location = new System.Drawing.Point(83, 334);
            this.siteLabel.Name = "siteLabel";
            this.siteLabel.Size = new System.Drawing.Size(25, 13);
            this.siteLabel.TabIndex = 28;
            this.siteLabel.Text = "Site";
            // 
            // billingTypeComboBox
            // 
            this.billingTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.billingTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.billingTypeComboBox.FormattingEnabled = true;
            this.billingTypeComboBox.Location = new System.Drawing.Point(114, 304);
            this.billingTypeComboBox.Name = "billingTypeComboBox";
            this.billingTypeComboBox.Size = new System.Drawing.Size(158, 21);
            this.billingTypeComboBox.TabIndex = 27;
            this.billingTypeComboBox.SelectionChangeCommitted += new System.EventHandler(this.OnDataEntered);
            // 
            // textBirthdate
            // 
            this.textBirthdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBirthdate.Location = new System.Drawing.Point(114, 208);
            this.textBirthdate.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.textBirthdate.Name = "textBirthdate";
            this.textBirthdate.Size = new System.Drawing.Size(160, 20);
            this.textBirthdate.TabIndex = 19;
            this.textBirthdate.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textBirthdate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 211);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Birthdate";
            // 
            // checkShowArchived
            // 
            this.checkShowArchived.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkShowArchived.AutoSize = true;
            this.checkShowArchived.Location = new System.Drawing.Point(6, 437);
            this.checkShowArchived.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.checkShowArchived.Name = "checkShowArchived";
            this.checkShowArchived.Size = new System.Drawing.Size(217, 17);
            this.checkShowArchived.TabIndex = 33;
            this.checkShowArchived.Text = "Show Archived/Deceased/Hidden Clinics";
            this.checkShowArchived.CheckedChanged += new System.EventHandler(this.ShowArchivedCheckBox_CheckedChanged);
            // 
            // textChartNumber
            // 
            this.textChartNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textChartNumber.Location = new System.Drawing.Point(114, 187);
            this.textChartNumber.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.textChartNumber.Name = "textChartNumber";
            this.textChartNumber.Size = new System.Drawing.Size(160, 20);
            this.textChartNumber.TabIndex = 17;
            this.textChartNumber.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textChartNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // textSSN
            // 
            this.textSSN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSSN.Location = new System.Drawing.Point(114, 145);
            this.textSSN.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.textSSN.Name = "textSSN";
            this.textSSN.Size = new System.Drawing.Size(160, 20);
            this.textSSN.TabIndex = 13;
            this.textSSN.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textSSN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(82, 148);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "SSN";
            // 
            // billingTypeLabel
            // 
            this.billingTypeLabel.AutoSize = true;
            this.billingTypeLabel.Location = new System.Drawing.Point(48, 307);
            this.billingTypeLabel.Name = "billingTypeLabel";
            this.billingTypeLabel.Size = new System.Drawing.Size(60, 13);
            this.billingTypeLabel.TabIndex = 26;
            this.billingTypeLabel.Text = "Billing Type";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(34, 190);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Chart Number";
            // 
            // textPatNum
            // 
            this.textPatNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPatNum.Location = new System.Drawing.Point(114, 166);
            this.textPatNum.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.textPatNum.Name = "textPatNum";
            this.textPatNum.Size = new System.Drawing.Size(160, 20);
            this.textPatNum.TabIndex = 15;
            this.textPatNum.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textPatNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(27, 169);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Patient Number";
            // 
            // textState
            // 
            this.textState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textState.Location = new System.Drawing.Point(114, 124);
            this.textState.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.textState.Name = "textState";
            this.textState.Size = new System.Drawing.Size(160, 20);
            this.textState.TabIndex = 11;
            this.textState.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textState.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(75, 127);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "State";
            // 
            // textCity
            // 
            this.textCity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textCity.Location = new System.Drawing.Point(114, 103);
            this.textCity.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.textCity.Name = "textCity";
            this.textCity.Size = new System.Drawing.Size(160, 20);
            this.textCity.TabIndex = 9;
            this.textCity.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textCity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(82, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "City";
            // 
            // checkGuarantors
            // 
            this.checkGuarantors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkGuarantors.AutoSize = true;
            this.checkGuarantors.Location = new System.Drawing.Point(6, 397);
            this.checkGuarantors.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.checkGuarantors.Name = "checkGuarantors";
            this.checkGuarantors.Size = new System.Drawing.Size(105, 17);
            this.checkGuarantors.TabIndex = 31;
            this.checkGuarantors.Text = "Guarantors Only";
            this.checkGuarantors.CheckedChanged += new System.EventHandler(this.OnDataEntered);
            // 
            // checkShowInactive
            // 
            this.checkShowInactive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkShowInactive.AutoSize = true;
            this.checkShowInactive.Location = new System.Drawing.Point(6, 417);
            this.checkShowInactive.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.checkShowInactive.Name = "checkShowInactive";
            this.checkShowInactive.Size = new System.Drawing.Size(136, 17);
            this.checkShowInactive.TabIndex = 32;
            this.checkShowInactive.Text = "Show Inactive Patients";
            this.checkShowInactive.CheckedChanged += new System.EventHandler(this.OnDataEntered);
            // 
            // textAddress
            // 
            this.textAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textAddress.Location = new System.Drawing.Point(114, 82);
            this.textAddress.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.textAddress.Name = "textAddress";
            this.textAddress.Size = new System.Drawing.Size(160, 20);
            this.textAddress.TabIndex = 7;
            this.textAddress.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Address";
            // 
            // textPhone
            // 
            this.textPhone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPhone.Location = new System.Drawing.Point(114, 61);
            this.textPhone.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.textPhone.Name = "textPhone";
            this.textPhone.Size = new System.Drawing.Size(160, 20);
            this.textPhone.TabIndex = 5;
            this.textPhone.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textPhone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Phone (any)";
            // 
            // textFName
            // 
            this.textFName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textFName.Location = new System.Drawing.Point(114, 40);
            this.textFName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.textFName.Name = "textFName";
            this.textFName.Size = new System.Drawing.Size(160, 20);
            this.textFName.TabIndex = 3;
            this.textFName.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textFName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "First Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.refreshCheckBox);
            this.groupBox1.Controls.Add(this.butGetAll);
            this.groupBox1.Controls.Add(this.butSearch);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(792, 498);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 75);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search";
            // 
            // refreshCheckBox
            // 
            this.refreshCheckBox.AutoSize = true;
            this.refreshCheckBox.Location = new System.Drawing.Point(6, 52);
            this.refreshCheckBox.Name = "refreshCheckBox";
            this.refreshCheckBox.Size = new System.Drawing.Size(124, 17);
            this.refreshCheckBox.TabIndex = 2;
            this.refreshCheckBox.Text = "Refresh while typing";
            this.refreshCheckBox.UseVisualStyleBackColor = true;
            this.refreshCheckBox.Click += new System.EventHandler(this.RefreshCheckBox_Click);
            // 
            // butGetAll
            // 
            this.butGetAll.Location = new System.Drawing.Point(143, 19);
            this.butGetAll.Name = "butGetAll";
            this.butGetAll.Size = new System.Drawing.Size(80, 25);
            this.butGetAll.TabIndex = 1;
            this.butGetAll.Text = "Get All";
            this.butGetAll.Click += new System.EventHandler(this.GetAllButton_Click);
            // 
            // butSearch
            // 
            this.butSearch.Location = new System.Drawing.Point(57, 19);
            this.butSearch.Name = "butSearch";
            this.butSearch.Size = new System.Drawing.Size(80, 25);
            this.butSearch.TabIndex = 0;
            this.butSearch.Text = "&Search";
            this.butSearch.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // patientsGrid
            // 
            this.patientsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.patientsGrid.HScrollVisible = true;
            this.patientsGrid.Location = new System.Drawing.Point(12, 12);
            this.patientsGrid.Name = "patientsGrid";
            this.patientsGrid.Size = new System.Drawing.Size(774, 677);
            this.patientsGrid.TabIndex = 5;
            this.patientsGrid.Title = "Select Patient";
            this.patientsGrid.TranslationName = "FormPatientSelect";
            this.patientsGrid.WrapText = false;
            this.patientsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.PatientsGrid_CellDoubleClick);
            this.patientsGrid.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.PatientsGrid_CellClick);
            this.patientsGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PatientsGrid_MouseDown);
            // 
            // patientsGridFillTimer
            // 
            this.patientsGridFillTimer.Interval = 1;
            this.patientsGridFillTimer.Tick += new System.EventHandler(this.OnDataEntered);
            // 
            // FormPatientSelect
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(1084, 701);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.patientsGrid);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.searchGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.groupAddPt);
            this.Name = "FormPatientSelect";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Patient";
            this.Load += new System.EventHandler(this.FormSelectPatient_Load);
            this.groupAddPt.ResumeLayout(false);
            this.searchGroupBox.ResumeLayout(false);
            this.searchGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button butAddPt;
		private System.Windows.Forms.GroupBox searchGroupBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textLName;
		private System.Windows.Forms.TextBox textFName;
		private System.Windows.Forms.TextBox textAddress;
		private OpenDental.ValidPhone textPhone;
		private System.Windows.Forms.CheckBox checkShowInactive;
		private System.Windows.Forms.GroupBox groupAddPt;
		private System.Windows.Forms.CheckBox checkGuarantors;
		private System.Windows.Forms.TextBox textCity;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textState;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textPatNum;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox textChartNumber;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label billingTypeLabel;
		private System.Windows.Forms.TextBox textSSN;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.GroupBox groupBox1;
		private OpenDental.UI.Button butSearch;
		private OpenDental.UI.ODGrid patientsGrid;
		private System.Windows.Forms.CheckBox checkShowArchived;
		private System.Windows.Forms.TextBox textBirthdate;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox billingTypeComboBox;
		private OpenDental.UI.Button butGetAll;
		private System.Windows.Forms.CheckBox refreshCheckBox;
		private OpenDental.UI.Button butAddAll;
		private System.Windows.Forms.ComboBox siteComboBox;
		private System.Windows.Forms.Label siteLabel;
		private System.Windows.Forms.TextBox selectedTxtBox;
		private System.Windows.Forms.TextBox textSubscriberID;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox textEmail;
		private System.Windows.Forms.Label labelEmail;
		private OpenDental.UI.ComboBoxClinicPicker clinicComboBox;
		private System.Windows.Forms.CheckBox checkShowMerged;
		private System.Windows.Forms.TextBox textInvoiceNumber;
		private System.Windows.Forms.Label labelInvoiceNumber;
		private System.Windows.Forms.Timer patientsGridFillTimer;
	}
}
