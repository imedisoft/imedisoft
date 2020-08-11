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
            this.label6 = new System.Windows.Forms.Label();
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
            this.textLName.Location = new System.Drawing.Point(166, 55);
            this.textLName.Name = "textLName";
            this.textLName.Size = new System.Drawing.Size(90, 20);
            this.textLName.TabIndex = 0;
            this.textLName.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textLName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Last Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupAddPt
            // 
            this.groupAddPt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupAddPt.Controls.Add(this.butAddAll);
            this.groupAddPt.Controls.Add(this.butAddPt);
            this.groupAddPt.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupAddPt.Location = new System.Drawing.Point(672, 562);
            this.groupAddPt.Name = "groupAddPt";
            this.groupAddPt.Size = new System.Drawing.Size(262, 45);
            this.groupAddPt.TabIndex = 2;
            this.groupAddPt.TabStop = false;
            this.groupAddPt.Text = "Add New Family:";
            // 
            // butAddAll
            // 
            this.butAddAll.Location = new System.Drawing.Point(148, 16);
            this.butAddAll.Name = "butAddAll";
            this.butAddAll.Size = new System.Drawing.Size(75, 23);
            this.butAddAll.TabIndex = 1;
            this.butAddAll.Text = "Add Many";
            this.butAddAll.Click += new System.EventHandler(this.AddManyButton_Click);
            // 
            // butAddPt
            // 
            this.butAddPt.Location = new System.Drawing.Point(42, 16);
            this.butAddPt.Name = "butAddPt";
            this.butAddPt.Size = new System.Drawing.Size(75, 23);
            this.butAddPt.TabIndex = 0;
            this.butAddPt.Text = "&Add Pt";
            this.butAddPt.Click += new System.EventHandler(this.AddPatientButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(766, 659);
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
            this.cancelButton.Location = new System.Drawing.Point(852, 659);
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
            this.searchGroupBox.Controls.Add(this.label6);
            this.searchGroupBox.Controls.Add(this.textAddress);
            this.searchGroupBox.Controls.Add(this.label5);
            this.searchGroupBox.Controls.Add(this.textPhone);
            this.searchGroupBox.Controls.Add(this.label4);
            this.searchGroupBox.Controls.Add(this.textFName);
            this.searchGroupBox.Controls.Add(this.label3);
            this.searchGroupBox.Controls.Add(this.textLName);
            this.searchGroupBox.Controls.Add(this.label1);
            this.searchGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.searchGroupBox.Location = new System.Drawing.Point(672, 2);
            this.searchGroupBox.Name = "searchGroupBox";
            this.searchGroupBox.Size = new System.Drawing.Size(262, 499);
            this.searchGroupBox.TabIndex = 0;
            this.searchGroupBox.TabStop = false;
            this.searchGroupBox.Text = "Search Criteria";
            // 
            // textInvoiceNumber
            // 
            this.textInvoiceNumber.Location = new System.Drawing.Point(166, 295);
            this.textInvoiceNumber.Name = "textInvoiceNumber";
            this.textInvoiceNumber.Size = new System.Drawing.Size(90, 20);
            this.textInvoiceNumber.TabIndex = 12;
            this.textInvoiceNumber.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textInvoiceNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // labelInvoiceNumber
            // 
            this.labelInvoiceNumber.Location = new System.Drawing.Point(11, 296);
            this.labelInvoiceNumber.Name = "labelInvoiceNumber";
            this.labelInvoiceNumber.Size = new System.Drawing.Size(156, 17);
            this.labelInvoiceNumber.TabIndex = 53;
            this.labelInvoiceNumber.Text = "Invoice Number";
            this.labelInvoiceNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkShowMerged
            // 
            this.checkShowMerged.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkShowMerged.Location = new System.Drawing.Point(11, 477);
            this.checkShowMerged.Name = "checkShowMerged";
            this.checkShowMerged.Size = new System.Drawing.Size(236, 17);
            this.checkShowMerged.TabIndex = 21;
            this.checkShowMerged.Text = "Show Merged Patients";
            this.checkShowMerged.Visible = false;
            this.checkShowMerged.CheckedChanged += new System.EventHandler(this.OnDataEntered);
            // 
            // clinicComboBox
            // 
            this.clinicComboBox.IncludeAll = true;
            this.clinicComboBox.Location = new System.Drawing.Point(61, 398);
            this.clinicComboBox.Name = "clinicComboBox";
            this.clinicComboBox.Size = new System.Drawing.Size(195, 21);
            this.clinicComboBox.TabIndex = 17;
            this.clinicComboBox.SelectionChangeCommitted += new System.EventHandler(this.OnDataEntered);
            // 
            // textEmail
            // 
            this.textEmail.Location = new System.Drawing.Point(166, 275);
            this.textEmail.Name = "textEmail";
            this.textEmail.Size = new System.Drawing.Size(90, 20);
            this.textEmail.TabIndex = 11;
            this.textEmail.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textEmail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // labelEmail
            // 
            this.labelEmail.Location = new System.Drawing.Point(11, 279);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(156, 12);
            this.labelEmail.TabIndex = 43;
            this.labelEmail.Text = "E-mail";
            this.labelEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textSubscriberID
            // 
            this.textSubscriberID.Location = new System.Drawing.Point(166, 255);
            this.textSubscriberID.Name = "textSubscriberID";
            this.textSubscriberID.Size = new System.Drawing.Size(90, 20);
            this.textSubscriberID.TabIndex = 10;
            this.textSubscriberID.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textSubscriberID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(11, 259);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(156, 12);
            this.label13.TabIndex = 41;
            this.label13.Text = "Subscriber ID";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // siteComboBox
            // 
            this.siteComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.siteComboBox.Location = new System.Drawing.Point(98, 377);
            this.siteComboBox.MaxDropDownItems = 40;
            this.siteComboBox.Name = "siteComboBox";
            this.siteComboBox.Size = new System.Drawing.Size(158, 21);
            this.siteComboBox.TabIndex = 16;
            this.siteComboBox.SelectionChangeCommitted += new System.EventHandler(this.OnDataEntered);
            // 
            // siteLabel
            // 
            this.siteLabel.Location = new System.Drawing.Point(11, 381);
            this.siteLabel.Name = "siteLabel";
            this.siteLabel.Size = new System.Drawing.Size(86, 14);
            this.siteLabel.TabIndex = 38;
            this.siteLabel.Text = "Site";
            this.siteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // billingTypeComboBox
            // 
            this.billingTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.billingTypeComboBox.FormattingEnabled = true;
            this.billingTypeComboBox.Location = new System.Drawing.Point(98, 356);
            this.billingTypeComboBox.Name = "billingTypeComboBox";
            this.billingTypeComboBox.Size = new System.Drawing.Size(158, 21);
            this.billingTypeComboBox.TabIndex = 15;
            this.billingTypeComboBox.SelectionChangeCommitted += new System.EventHandler(this.OnDataEntered);
            // 
            // textBirthdate
            // 
            this.textBirthdate.Location = new System.Drawing.Point(166, 235);
            this.textBirthdate.Name = "textBirthdate";
            this.textBirthdate.Size = new System.Drawing.Size(90, 20);
            this.textBirthdate.TabIndex = 9;
            this.textBirthdate.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textBirthdate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(11, 239);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 12);
            this.label2.TabIndex = 27;
            this.label2.Text = "Birthdate";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkShowArchived
            // 
            this.checkShowArchived.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkShowArchived.Location = new System.Drawing.Point(11, 460);
            this.checkShowArchived.Name = "checkShowArchived";
            this.checkShowArchived.Size = new System.Drawing.Size(245, 16);
            this.checkShowArchived.TabIndex = 20;
            this.checkShowArchived.Text = "Show Archived/Deceased/Hidden Clinics";
            this.checkShowArchived.CheckedChanged += new System.EventHandler(this.ShowArchivedCheckBox_CheckedChanged);
            // 
            // textChartNumber
            // 
            this.textChartNumber.Location = new System.Drawing.Point(166, 215);
            this.textChartNumber.Name = "textChartNumber";
            this.textChartNumber.Size = new System.Drawing.Size(90, 20);
            this.textChartNumber.TabIndex = 8;
            this.textChartNumber.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textChartNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // textSSN
            // 
            this.textSSN.Location = new System.Drawing.Point(166, 175);
            this.textSSN.Name = "textSSN";
            this.textSSN.Size = new System.Drawing.Size(90, 20);
            this.textSSN.TabIndex = 6;
            this.textSSN.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textSSN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(11, 179);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(155, 12);
            this.label12.TabIndex = 24;
            this.label12.Text = "SSN";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // billingTypeLabel
            // 
            this.billingTypeLabel.Location = new System.Drawing.Point(11, 360);
            this.billingTypeLabel.Name = "billingTypeLabel";
            this.billingTypeLabel.Size = new System.Drawing.Size(87, 14);
            this.billingTypeLabel.TabIndex = 21;
            this.billingTypeLabel.Text = "Billing Type";
            this.billingTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(11, 219);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(156, 12);
            this.label10.TabIndex = 20;
            this.label10.Text = "Chart Number";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textPatNum
            // 
            this.textPatNum.Location = new System.Drawing.Point(166, 195);
            this.textPatNum.Name = "textPatNum";
            this.textPatNum.Size = new System.Drawing.Size(90, 20);
            this.textPatNum.TabIndex = 7;
            this.textPatNum.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textPatNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(11, 199);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(156, 12);
            this.label9.TabIndex = 18;
            this.label9.Text = "Patient Number";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textState
            // 
            this.textState.Location = new System.Drawing.Point(166, 155);
            this.textState.Name = "textState";
            this.textState.Size = new System.Drawing.Size(90, 20);
            this.textState.TabIndex = 5;
            this.textState.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textState.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(11, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(154, 12);
            this.label8.TabIndex = 16;
            this.label8.Text = "State";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textCity
            // 
            this.textCity.Location = new System.Drawing.Point(166, 135);
            this.textCity.Name = "textCity";
            this.textCity.Size = new System.Drawing.Size(90, 20);
            this.textCity.TabIndex = 4;
            this.textCity.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textCity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(11, 137);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(152, 14);
            this.label7.TabIndex = 14;
            this.label7.Text = "City";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkGuarantors
            // 
            this.checkGuarantors.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkGuarantors.Location = new System.Drawing.Point(11, 426);
            this.checkGuarantors.Name = "checkGuarantors";
            this.checkGuarantors.Size = new System.Drawing.Size(245, 16);
            this.checkGuarantors.TabIndex = 18;
            this.checkGuarantors.Text = "Guarantors Only";
            this.checkGuarantors.CheckedChanged += new System.EventHandler(this.OnDataEntered);
            // 
            // checkShowInactive
            // 
            this.checkShowInactive.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkShowInactive.Location = new System.Drawing.Point(11, 443);
            this.checkShowInactive.Name = "checkShowInactive";
            this.checkShowInactive.Size = new System.Drawing.Size(245, 16);
            this.checkShowInactive.TabIndex = 19;
            this.checkShowInactive.Text = "Show Inactive Patients";
            this.checkShowInactive.CheckedChanged += new System.EventHandler(this.OnDataEntered);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(11, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(245, 14);
            this.label6.TabIndex = 10;
            this.label6.Text = "Hint: enter values in multiple boxes.";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textAddress
            // 
            this.textAddress.Location = new System.Drawing.Point(166, 115);
            this.textAddress.Name = "textAddress";
            this.textAddress.Size = new System.Drawing.Size(90, 20);
            this.textAddress.TabIndex = 3;
            this.textAddress.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(11, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "Address";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textPhone
            // 
            this.textPhone.Location = new System.Drawing.Point(166, 95);
            this.textPhone.Name = "textPhone";
            this.textPhone.Size = new System.Drawing.Size(90, 20);
            this.textPhone.TabIndex = 2;
            this.textPhone.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textPhone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(11, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Phone (any)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textFName
            // 
            this.textFName.Location = new System.Drawing.Point(166, 75);
            this.textFName.Name = "textFName";
            this.textFName.Size = new System.Drawing.Size(90, 20);
            this.textFName.TabIndex = 1;
            this.textFName.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textFName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(11, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "First Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.refreshCheckBox);
            this.groupBox1.Controls.Add(this.butGetAll);
            this.groupBox1.Controls.Add(this.butSearch);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(672, 501);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 61);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search";
            // 
            // refreshCheckBox
            // 
            this.refreshCheckBox.AutoSize = true;
            this.refreshCheckBox.Location = new System.Drawing.Point(11, 41);
            this.refreshCheckBox.Name = "refreshCheckBox";
            this.refreshCheckBox.Size = new System.Drawing.Size(124, 17);
            this.refreshCheckBox.TabIndex = 72;
            this.refreshCheckBox.Text = "Refresh while typing";
            this.refreshCheckBox.UseVisualStyleBackColor = true;
            this.refreshCheckBox.Click += new System.EventHandler(this.RefreshCheckBox_Click);
            // 
            // butGetAll
            // 
            this.butGetAll.Location = new System.Drawing.Point(148, 15);
            this.butGetAll.Name = "butGetAll";
            this.butGetAll.Size = new System.Drawing.Size(75, 23);
            this.butGetAll.TabIndex = 1;
            this.butGetAll.Text = "Get All";
            this.butGetAll.Click += new System.EventHandler(this.GetAllButton_Click);
            // 
            // butSearch
            // 
            this.butSearch.Location = new System.Drawing.Point(42, 15);
            this.butSearch.Name = "butSearch";
            this.butSearch.Size = new System.Drawing.Size(75, 23);
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
            this.patientsGrid.Size = new System.Drawing.Size(654, 672);
            this.patientsGrid.TabIndex = 9;
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
            this.ClientSize = new System.Drawing.Size(944, 696);
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
		private System.Windows.Forms.Label label6;
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
