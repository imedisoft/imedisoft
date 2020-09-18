namespace Imedisoft.Forms
{
    partial class FormCarrierEdit
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
            this.textCarrierName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textPhone = new OpenDental.ValidPhone();
            this.label3 = new System.Windows.Forms.Label();
            this.textAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textAddress2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.labelElectID = new System.Windows.Forms.Label();
            this.textElectID = new System.Windows.Forms.TextBox();
            this.inUseByGroupBox = new System.Windows.Forms.GroupBox();
            this.comboPlans = new System.Windows.Forms.ComboBox();
            this.textPlans = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textCity = new System.Windows.Forms.TextBox();
            this.textState = new System.Windows.Forms.TextBox();
            this.textZip = new System.Windows.Forms.TextBox();
            this.labelCitySt = new System.Windows.Forms.Label();
            this.isCdaCheckBox = new System.Windows.Forms.CheckBox();
            this.cdaNetGroupBox = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textEncryptionMethod = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.check01 = new System.Windows.Forms.CheckBox();
            this.check03m = new System.Windows.Forms.CheckBox();
            this.check03 = new System.Windows.Forms.CheckBox();
            this.check07 = new System.Windows.Forms.CheckBox();
            this.check06 = new System.Windows.Forms.CheckBox();
            this.check04 = new System.Windows.Forms.CheckBox();
            this.check05 = new System.Windows.Forms.CheckBox();
            this.check02 = new System.Windows.Forms.CheckBox();
            this.check08 = new System.Windows.Forms.CheckBox();
            this.textModemReconcile = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textModemSummary = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textModem = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboNetwork = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textVersion = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkIsHidden = new System.Windows.Forms.CheckBox();
            this.deleteButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.textCarrierNum = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.labelCarrierGroupName = new System.Windows.Forms.Label();
            this.labelColor = new System.Windows.Forms.Label();
            this.comboCarrierGroupName = new OpenDental.UI.ComboBoxPlus();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioBenefitSendsPat = new System.Windows.Forms.RadioButton();
            this.radioBenefitSendsIns = new System.Windows.Forms.RadioButton();
            this.labelSendElectronically = new System.Windows.Forms.Label();
            this.comboSendElectronically = new OpenDental.UI.ComboBoxPlus();
            this.checkRealTimeEligibility = new System.Windows.Forms.CheckBox();
            this.odColorPickerBack = new OpenDental.UI.ODColorPicker();
            this.carrierTabControl = new System.Windows.Forms.TabControl();
            this.generalTabPage = new System.Windows.Forms.TabPage();
            this.canadaTabPage = new System.Windows.Forms.TabPage();
            this.inUseByGroupBox.SuspendLayout();
            this.cdaNetGroupBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.carrierTabControl.SuspendLayout();
            this.generalTabPage.SuspendLayout();
            this.canadaTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // textCarrierName
            // 
            this.textCarrierName.Location = new System.Drawing.Point(200, 32);
            this.textCarrierName.MaxLength = 255;
            this.textCarrierName.Name = "textCarrierName";
            this.textCarrierName.Size = new System.Drawing.Size(220, 20);
            this.textCarrierName.TabIndex = 1;
            this.textCarrierName.TextChanged += new System.EventHandler(this.textCarrierName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(157, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Phone";
            // 
            // textPhone
            // 
            this.textPhone.Location = new System.Drawing.Point(200, 58);
            this.textPhone.MaxLength = 255;
            this.textPhone.Name = "textPhone";
            this.textPhone.Size = new System.Drawing.Size(157, 20);
            this.textPhone.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(117, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Address Line 1";
            // 
            // textAddress
            // 
            this.textAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textAddress.Location = new System.Drawing.Point(200, 84);
            this.textAddress.MaxLength = 255;
            this.textAddress.Name = "textAddress";
            this.textAddress.Size = new System.Drawing.Size(326, 20);
            this.textAddress.TabIndex = 3;
            this.textAddress.TextChanged += new System.EventHandler(this.textAddress_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(117, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Address Line 2";
            // 
            // textAddress2
            // 
            this.textAddress2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textAddress2.Location = new System.Drawing.Point(200, 110);
            this.textAddress2.MaxLength = 255;
            this.textAddress2.Name = "textAddress2";
            this.textAddress2.Size = new System.Drawing.Size(326, 20);
            this.textAddress2.TabIndex = 4;
            this.textAddress2.TextChanged += new System.EventHandler(this.textAddress2_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(124, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Carrier Name";
            // 
            // labelElectID
            // 
            this.labelElectID.AutoSize = true;
            this.labelElectID.Location = new System.Drawing.Point(127, 165);
            this.labelElectID.Name = "labelElectID";
            this.labelElectID.Size = new System.Drawing.Size(67, 13);
            this.labelElectID.TabIndex = 0;
            this.labelElectID.Text = "Electronic ID";
            // 
            // textElectID
            // 
            this.textElectID.Location = new System.Drawing.Point(200, 162);
            this.textElectID.Name = "textElectID";
            this.textElectID.Size = new System.Drawing.Size(59, 20);
            this.textElectID.TabIndex = 8;
            // 
            // inUseByGroupBox
            // 
            this.inUseByGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inUseByGroupBox.Controls.Add(this.comboPlans);
            this.inUseByGroupBox.Controls.Add(this.textPlans);
            this.inUseByGroupBox.Controls.Add(this.label9);
            this.inUseByGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.inUseByGroupBox.Location = new System.Drawing.Point(12, 544);
            this.inUseByGroupBox.Name = "inUseByGroupBox";
            this.inUseByGroupBox.Size = new System.Drawing.Size(540, 50);
            this.inUseByGroupBox.TabIndex = 15;
            this.inUseByGroupBox.TabStop = false;
            this.inUseByGroupBox.Text = "In Use By";
            // 
            // comboPlans
            // 
            this.comboPlans.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboPlans.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPlans.Location = new System.Drawing.Point(206, 19);
            this.comboPlans.MaxDropDownItems = 30;
            this.comboPlans.Name = "comboPlans";
            this.comboPlans.Size = new System.Drawing.Size(328, 21);
            this.comboPlans.TabIndex = 1;
            // 
            // textPlans
            // 
            this.textPlans.BackColor = System.Drawing.Color.White;
            this.textPlans.Location = new System.Drawing.Point(160, 19);
            this.textPlans.Name = "textPlans";
            this.textPlans.ReadOnly = true;
            this.textPlans.Size = new System.Drawing.Size(40, 20);
            this.textPlans.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(136, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Insurance Plan Subscribers";
            // 
            // textCity
            // 
            this.textCity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textCity.Location = new System.Drawing.Point(200, 136);
            this.textCity.MaxLength = 255;
            this.textCity.Name = "textCity";
            this.textCity.Size = new System.Drawing.Size(194, 20);
            this.textCity.TabIndex = 5;
            this.textCity.TextChanged += new System.EventHandler(this.textCity_TextChanged);
            // 
            // textState
            // 
            this.textState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textState.Location = new System.Drawing.Point(400, 136);
            this.textState.MaxLength = 255;
            this.textState.Name = "textState";
            this.textState.Size = new System.Drawing.Size(50, 20);
            this.textState.TabIndex = 6;
            this.textState.TextChanged += new System.EventHandler(this.textState_TextChanged);
            // 
            // textZip
            // 
            this.textZip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textZip.Location = new System.Drawing.Point(456, 136);
            this.textZip.MaxLength = 255;
            this.textZip.Name = "textZip";
            this.textZip.Size = new System.Drawing.Size(70, 20);
            this.textZip.TabIndex = 7;
            // 
            // labelCitySt
            // 
            this.labelCitySt.AutoSize = true;
            this.labelCitySt.Location = new System.Drawing.Point(128, 139);
            this.labelCitySt.Name = "labelCitySt";
            this.labelCitySt.Size = new System.Drawing.Size(66, 13);
            this.labelCitySt.TabIndex = 0;
            this.labelCitySt.Text = "City, ST, Zip";
            // 
            // isCdaCheckBox
            // 
            this.isCdaCheckBox.AutoSize = true;
            this.isCdaCheckBox.Location = new System.Drawing.Point(6, 6);
            this.isCdaCheckBox.Name = "isCdaCheckBox";
            this.isCdaCheckBox.Size = new System.Drawing.Size(111, 17);
            this.isCdaCheckBox.TabIndex = 16;
            this.isCdaCheckBox.Text = "Is CDAnet Carrier";
            this.isCdaCheckBox.Click += new System.EventHandler(this.checkIsCDAnet_Click);
            // 
            // cdaNetGroupBox
            // 
            this.cdaNetGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cdaNetGroupBox.Controls.Add(this.label14);
            this.cdaNetGroupBox.Controls.Add(this.label12);
            this.cdaNetGroupBox.Controls.Add(this.textEncryptionMethod);
            this.cdaNetGroupBox.Controls.Add(this.label13);
            this.cdaNetGroupBox.Controls.Add(this.label11);
            this.cdaNetGroupBox.Controls.Add(this.groupBox3);
            this.cdaNetGroupBox.Controls.Add(this.textModemReconcile);
            this.cdaNetGroupBox.Controls.Add(this.textModemSummary);
            this.cdaNetGroupBox.Controls.Add(this.label8);
            this.cdaNetGroupBox.Controls.Add(this.textModem);
            this.cdaNetGroupBox.Controls.Add(this.label7);
            this.cdaNetGroupBox.Controls.Add(this.comboNetwork);
            this.cdaNetGroupBox.Controls.Add(this.label5);
            this.cdaNetGroupBox.Controls.Add(this.textVersion);
            this.cdaNetGroupBox.Controls.Add(this.label1);
            this.cdaNetGroupBox.Controls.Add(this.label10);
            this.cdaNetGroupBox.Location = new System.Drawing.Point(6, 29);
            this.cdaNetGroupBox.Name = "cdaNetGroupBox";
            this.cdaNetGroupBox.Size = new System.Drawing.Size(520, 469);
            this.cdaNetGroupBox.TabIndex = 17;
            this.cdaNetGroupBox.TabStop = false;
            this.cdaNetGroupBox.Text = "CDAnet";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.Location = new System.Drawing.Point(6, 95);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(508, 40);
            this.label14.TabIndex = 0;
            this.label14.Text = "The values in this section can be set by running database maint after manually se" +
    "tting the carrier identification number above.";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(243, 75);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "(1, 2, or 3)";
            // 
            // textEncryptionMethod
            // 
            this.textEncryptionMethod.Enabled = false;
            this.textEncryptionMethod.Location = new System.Drawing.Point(195, 72);
            this.textEncryptionMethod.Name = "textEncryptionMethod";
            this.textEncryptionMethod.Size = new System.Drawing.Size(42, 20);
            this.textEncryptionMethod.TabIndex = 2;
            this.textEncryptionMethod.TabStop = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(92, 75);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(97, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Encryption Method";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(243, 49);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "(02 or 04)";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.check01);
            this.groupBox3.Controls.Add(this.check03m);
            this.groupBox3.Controls.Add(this.check03);
            this.groupBox3.Controls.Add(this.check07);
            this.groupBox3.Controls.Add(this.check06);
            this.groupBox3.Controls.Add(this.check04);
            this.groupBox3.Controls.Add(this.check05);
            this.groupBox3.Controls.Add(this.check02);
            this.groupBox3.Controls.Add(this.check08);
            this.groupBox3.Location = new System.Drawing.Point(195, 216);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.groupBox3.Size = new System.Drawing.Size(280, 240);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Supported Transaction Types";
            // 
            // check01
            // 
            this.check01.AutoSize = true;
            this.check01.Checked = true;
            this.check01.CheckState = System.Windows.Forms.CheckState.Checked;
            this.check01.Enabled = false;
            this.check01.Location = new System.Drawing.Point(13, 21);
            this.check01.Name = "check01";
            this.check01.Size = new System.Drawing.Size(51, 17);
            this.check01.TabIndex = 0;
            this.check01.Text = "Claim";
            this.check01.UseVisualStyleBackColor = true;
            // 
            // check03m
            // 
            this.check03m.AutoSize = true;
            this.check03m.Enabled = false;
            this.check03m.Location = new System.Drawing.Point(13, 136);
            this.check03m.Name = "check03m";
            this.check03m.Size = new System.Drawing.Size(161, 17);
            this.check03m.TabIndex = 5;
            this.check03m.Text = "Predetermination Multi-page";
            this.check03m.UseVisualStyleBackColor = true;
            // 
            // check03
            // 
            this.check03.AutoSize = true;
            this.check03.Enabled = false;
            this.check03.Location = new System.Drawing.Point(13, 113);
            this.check03.Name = "check03";
            this.check03.Size = new System.Drawing.Size(166, 17);
            this.check03.TabIndex = 4;
            this.check03.Text = "Predetermination Single Page";
            this.check03.UseVisualStyleBackColor = true;
            // 
            // check07
            // 
            this.check07.AutoSize = true;
            this.check07.Enabled = false;
            this.check07.Location = new System.Drawing.Point(13, 67);
            this.check07.Name = "check07";
            this.check07.Size = new System.Drawing.Size(134, 17);
            this.check07.TabIndex = 2;
            this.check07.Text = "COB Claim Transaction";
            this.check07.UseVisualStyleBackColor = true;
            // 
            // check06
            // 
            this.check06.AutoSize = true;
            this.check06.Enabled = false;
            this.check06.Location = new System.Drawing.Point(13, 205);
            this.check06.Name = "check06";
            this.check06.Size = new System.Drawing.Size(196, 17);
            this.check06.TabIndex = 8;
            this.check06.Text = "Request for Payment Reconciliation";
            this.check06.UseVisualStyleBackColor = true;
            // 
            // check04
            // 
            this.check04.AutoSize = true;
            this.check04.Enabled = false;
            this.check04.Location = new System.Drawing.Point(13, 159);
            this.check04.Name = "check04";
            this.check04.Size = new System.Drawing.Size(256, 17);
            this.check04.TabIndex = 6;
            this.check04.Text = "Request for Outstanding Transactions [Mailbox]";
            this.check04.UseVisualStyleBackColor = true;
            // 
            // check05
            // 
            this.check05.AutoSize = true;
            this.check05.Enabled = false;
            this.check05.Location = new System.Drawing.Point(13, 182);
            this.check05.Name = "check05";
            this.check05.Size = new System.Drawing.Size(198, 17);
            this.check05.TabIndex = 7;
            this.check05.Text = "Request for Summary Reconciliation";
            this.check05.UseVisualStyleBackColor = true;
            // 
            // check02
            // 
            this.check02.AutoSize = true;
            this.check02.Enabled = false;
            this.check02.Location = new System.Drawing.Point(13, 90);
            this.check02.Name = "check02";
            this.check02.Size = new System.Drawing.Size(96, 17);
            this.check02.TabIndex = 3;
            this.check02.Text = "Claim Reversal";
            this.check02.UseVisualStyleBackColor = true;
            // 
            // check08
            // 
            this.check08.AutoSize = true;
            this.check08.Enabled = false;
            this.check08.Location = new System.Drawing.Point(13, 44);
            this.check08.Name = "check08";
            this.check08.Size = new System.Drawing.Size(125, 17);
            this.check08.TabIndex = 1;
            this.check08.Text = "Eligibility Transaction";
            this.check08.UseVisualStyleBackColor = true;
            // 
            // textModemReconcile
            // 
            this.textModemReconcile.Location = new System.Drawing.Point(195, 190);
            this.textModemReconcile.Name = "textModemReconcile";
            this.textModemReconcile.Size = new System.Drawing.Size(121, 20);
            this.textModemReconcile.TabIndex = 5;
            this.textModemReconcile.TabStop = false;
            this.textModemReconcile.Visible = false;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 182);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(183, 35);
            this.label10.TabIndex = 0;
            this.label10.Text = "Modem Phone Number - Request for Payment Reconciliation";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label10.Visible = false;
            // 
            // textModemSummary
            // 
            this.textModemSummary.Location = new System.Drawing.Point(195, 164);
            this.textModemSummary.Name = "textModemSummary";
            this.textModemSummary.Size = new System.Drawing.Size(121, 20);
            this.textModemSummary.TabIndex = 4;
            this.textModemSummary.TabStop = false;
            this.textModemSummary.Visible = false;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(183, 35);
            this.label8.TabIndex = 0;
            this.label8.Text = "Modem Phone Number - Request for Summary Reconciliation";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label8.Visible = false;
            // 
            // textModem
            // 
            this.textModem.Location = new System.Drawing.Point(195, 138);
            this.textModem.Name = "textModem";
            this.textModem.Size = new System.Drawing.Size(121, 20);
            this.textModem.TabIndex = 3;
            this.textModem.TabStop = false;
            this.textModem.Visible = false;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(183, 35);
            this.label7.TabIndex = 0;
            this.label7.Text = "Modem Phone Number";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label7.Visible = false;
            // 
            // comboNetwork
            // 
            this.comboNetwork.Enabled = false;
            this.comboNetwork.FormattingEnabled = true;
            this.comboNetwork.Location = new System.Drawing.Point(195, 19);
            this.comboNetwork.Name = "comboNetwork";
            this.comboNetwork.Size = new System.Drawing.Size(169, 21);
            this.comboNetwork.TabIndex = 0;
            this.comboNetwork.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(142, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Network";
            // 
            // textVersion
            // 
            this.textVersion.Enabled = false;
            this.textVersion.Location = new System.Drawing.Point(195, 46);
            this.textVersion.Name = "textVersion";
            this.textVersion.Size = new System.Drawing.Size(42, 20);
            this.textVersion.TabIndex = 1;
            this.textVersion.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(107, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Version Number";
            // 
            // checkIsHidden
            // 
            this.checkIsHidden.AutoSize = true;
            this.checkIsHidden.Location = new System.Drawing.Point(200, 242);
            this.checkIsHidden.Name = "checkIsHidden";
            this.checkIsHidden.Size = new System.Drawing.Size(59, 17);
            this.checkIsHidden.TabIndex = 11;
            this.checkIsHidden.Text = "Hidden";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(12, 604);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 18;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.butDelete_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(386, 604);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 19;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.butOK_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(472, 604);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 20;
            this.cancelButton.Text = "&Cancel";
            // 
            // textCarrierNum
            // 
            this.textCarrierNum.Location = new System.Drawing.Point(200, 6);
            this.textCarrierNum.Name = "textCarrierNum";
            this.textCarrierNum.ReadOnly = true;
            this.textCarrierNum.Size = new System.Drawing.Size(60, 20);
            this.textCarrierNum.TabIndex = 0;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(140, 9);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(54, 13);
            this.label21.TabIndex = 0;
            this.label21.Text = "Carrier ID";
            // 
            // labelCarrierGroupName
            // 
            this.labelCarrierGroupName.AutoSize = true;
            this.labelCarrierGroupName.Location = new System.Drawing.Point(122, 215);
            this.labelCarrierGroupName.Name = "labelCarrierGroupName";
            this.labelCarrierGroupName.Size = new System.Drawing.Size(72, 13);
            this.labelCarrierGroupName.TabIndex = 0;
            this.labelCarrierGroupName.Text = "Carrier Group";
            this.labelCarrierGroupName.Visible = false;
            // 
            // labelColor
            // 
            this.labelColor.AutoSize = true;
            this.labelColor.Location = new System.Drawing.Point(19, 265);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(175, 13);
            this.labelColor.TabIndex = 0;
            this.labelColor.Text = "Appt Text Back Color (black=none)";
            // 
            // comboCarrierGroupName
            // 
            this.comboCarrierGroupName.Location = new System.Drawing.Point(200, 215);
            this.comboCarrierGroupName.Name = "comboCarrierGroupName";
            this.comboCarrierGroupName.Size = new System.Drawing.Size(220, 21);
            this.comboCarrierGroupName.TabIndex = 10;
            this.comboCarrierGroupName.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioBenefitSendsPat);
            this.groupBox2.Controls.Add(this.radioBenefitSendsIns);
            this.groupBox2.Location = new System.Drawing.Point(200, 299);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10, 10, 10, 5);
            this.groupBox2.Size = new System.Drawing.Size(240, 90);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Import Benefit Coinsurance";
            // 
            // radioBenefitSendsPat
            // 
            this.radioBenefitSendsPat.AutoSize = true;
            this.radioBenefitSendsPat.Location = new System.Drawing.Point(13, 26);
            this.radioBenefitSendsPat.Name = "radioBenefitSendsPat";
            this.radioBenefitSendsPat.Size = new System.Drawing.Size(185, 17);
            this.radioBenefitSendsPat.TabIndex = 0;
            this.radioBenefitSendsPat.TabStop = true;
            this.radioBenefitSendsPat.Text = "Carrier sends patient % (default)";
            // 
            // radioBenefitSendsIns
            // 
            this.radioBenefitSendsIns.AutoSize = true;
            this.radioBenefitSendsIns.Location = new System.Drawing.Point(13, 49);
            this.radioBenefitSendsIns.Name = "radioBenefitSendsIns";
            this.radioBenefitSendsIns.Size = new System.Drawing.Size(152, 17);
            this.radioBenefitSendsIns.TabIndex = 1;
            this.radioBenefitSendsIns.TabStop = true;
            this.radioBenefitSendsIns.Text = "Carrier sends insurance %";
            // 
            // labelSendElectronically
            // 
            this.labelSendElectronically.AutoSize = true;
            this.labelSendElectronically.Location = new System.Drawing.Point(98, 188);
            this.labelSendElectronically.Name = "labelSendElectronically";
            this.labelSendElectronically.Size = new System.Drawing.Size(96, 13);
            this.labelSendElectronically.TabIndex = 0;
            this.labelSendElectronically.Text = "Send Electronically";
            // 
            // comboSendElectronically
            // 
            this.comboSendElectronically.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboSendElectronically.Location = new System.Drawing.Point(200, 188);
            this.comboSendElectronically.Name = "comboSendElectronically";
            this.comboSendElectronically.Size = new System.Drawing.Size(326, 21);
            this.comboSendElectronically.TabIndex = 9;
            // 
            // checkRealTimeEligibility
            // 
            this.checkRealTimeEligibility.AutoSize = true;
            this.checkRealTimeEligibility.Location = new System.Drawing.Point(265, 242);
            this.checkRealTimeEligibility.Name = "checkRealTimeEligibility";
            this.checkRealTimeEligibility.Size = new System.Drawing.Size(178, 17);
            this.checkRealTimeEligibility.TabIndex = 21;
            this.checkRealTimeEligibility.Text = "Is trusted for real-time eligibility";
            // 
            // odColorPickerBack
            // 
            this.odColorPickerBack.BackgroundColor = System.Drawing.Color.Empty;
            this.odColorPickerBack.Location = new System.Drawing.Point(200, 265);
            this.odColorPickerBack.Name = "odColorPickerBack";
            this.odColorPickerBack.Size = new System.Drawing.Size(74, 21);
            this.odColorPickerBack.TabIndex = 22;
            // 
            // carrierTabControl
            // 
            this.carrierTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.carrierTabControl.Controls.Add(this.generalTabPage);
            this.carrierTabControl.Controls.Add(this.canadaTabPage);
            this.carrierTabControl.Location = new System.Drawing.Point(12, 12);
            this.carrierTabControl.Name = "carrierTabControl";
            this.carrierTabControl.SelectedIndex = 0;
            this.carrierTabControl.Size = new System.Drawing.Size(540, 530);
            this.carrierTabControl.TabIndex = 23;
            // 
            // generalTabPage
            // 
            this.generalTabPage.Controls.Add(this.textCarrierNum);
            this.generalTabPage.Controls.Add(this.odColorPickerBack);
            this.generalTabPage.Controls.Add(this.label2);
            this.generalTabPage.Controls.Add(this.checkRealTimeEligibility);
            this.generalTabPage.Controls.Add(this.label3);
            this.generalTabPage.Controls.Add(this.labelSendElectronically);
            this.generalTabPage.Controls.Add(this.label4);
            this.generalTabPage.Controls.Add(this.comboSendElectronically);
            this.generalTabPage.Controls.Add(this.label6);
            this.generalTabPage.Controls.Add(this.groupBox2);
            this.generalTabPage.Controls.Add(this.labelElectID);
            this.generalTabPage.Controls.Add(this.comboCarrierGroupName);
            this.generalTabPage.Controls.Add(this.labelColor);
            this.generalTabPage.Controls.Add(this.labelCitySt);
            this.generalTabPage.Controls.Add(this.labelCarrierGroupName);
            this.generalTabPage.Controls.Add(this.textCarrierName);
            this.generalTabPage.Controls.Add(this.textPhone);
            this.generalTabPage.Controls.Add(this.label21);
            this.generalTabPage.Controls.Add(this.textAddress);
            this.generalTabPage.Controls.Add(this.checkIsHidden);
            this.generalTabPage.Controls.Add(this.textAddress2);
            this.generalTabPage.Controls.Add(this.textCity);
            this.generalTabPage.Controls.Add(this.textElectID);
            this.generalTabPage.Controls.Add(this.textState);
            this.generalTabPage.Controls.Add(this.textZip);
            this.generalTabPage.Location = new System.Drawing.Point(4, 22);
            this.generalTabPage.Name = "generalTabPage";
            this.generalTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.generalTabPage.Size = new System.Drawing.Size(532, 504);
            this.generalTabPage.TabIndex = 0;
            this.generalTabPage.Text = "General";
            this.generalTabPage.UseVisualStyleBackColor = true;
            // 
            // canadaTabPage
            // 
            this.canadaTabPage.Controls.Add(this.cdaNetGroupBox);
            this.canadaTabPage.Controls.Add(this.isCdaCheckBox);
            this.canadaTabPage.Location = new System.Drawing.Point(4, 22);
            this.canadaTabPage.Name = "canadaTabPage";
            this.canadaTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.canadaTabPage.Size = new System.Drawing.Size(532, 504);
            this.canadaTabPage.TabIndex = 1;
            this.canadaTabPage.Text = "Canada";
            this.canadaTabPage.UseVisualStyleBackColor = true;
            // 
            // FormCarrierEdit
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(564, 641);
            this.Controls.Add(this.carrierTabControl);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.inUseByGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCarrierEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Carrier";
            this.Load += new System.EventHandler(this.FormCarrierEdit_Load);
            this.inUseByGroupBox.ResumeLayout(false);
            this.inUseByGroupBox.PerformLayout();
            this.cdaNetGroupBox.ResumeLayout(false);
            this.cdaNetGroupBox.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.carrierTabControl.ResumeLayout(false);
            this.generalTabPage.ResumeLayout(false);
            this.generalTabPage.PerformLayout();
            this.canadaTabPage.ResumeLayout(false);
            this.canadaTabPage.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label labelElectID;
		private System.Windows.Forms.TextBox textCarrierName;
		private OpenDental.ValidPhone textPhone;
		private System.Windows.Forms.TextBox textAddress;
		private System.Windows.Forms.TextBox textAddress2;
		private System.Windows.Forms.TextBox textElectID;
		private System.Windows.Forms.GroupBox inUseByGroupBox;
		private System.Windows.Forms.ComboBox comboPlans;
		private System.Windows.Forms.TextBox textPlans;
		private System.Windows.Forms.Label label9;
		private OpenDental.UI.Button deleteButton;
		private System.Windows.Forms.TextBox textCity;
		private System.Windows.Forms.TextBox textState;
		private System.Windows.Forms.TextBox textZip;
		private System.Windows.Forms.Label labelCitySt;
		private System.Windows.Forms.CheckBox isCdaCheckBox;
		private System.Windows.Forms.GroupBox cdaNetGroupBox;
		private System.Windows.Forms.TextBox textModemReconcile;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textModemSummary;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textModem;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox comboNetwork;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textVersion;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox check08;
		private System.Windows.Forms.CheckBox check03;
		private System.Windows.Forms.CheckBox check07;
		private System.Windows.Forms.CheckBox check06;
		private System.Windows.Forms.CheckBox check04;
		private System.Windows.Forms.CheckBox check05;
		private System.Windows.Forms.CheckBox check02;
		private System.Windows.Forms.CheckBox checkIsHidden;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.CheckBox check03m;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox textEncryptionMethod;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.CheckBox check01;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox textCarrierNum;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label labelCarrierGroupName;
		private System.Windows.Forms.Label labelColor;
		private OpenDental.UI.ComboBoxPlus comboCarrierGroupName;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioBenefitSendsIns;
		private System.Windows.Forms.RadioButton radioBenefitSendsPat;
		private System.Windows.Forms.Label labelSendElectronically;
		private OpenDental.UI.ComboBoxPlus comboSendElectronically;
		private OpenDental.UI.ODColorPicker odColorPickerBack;
		private System.Windows.Forms.CheckBox checkRealTimeEligibility;
        private System.Windows.Forms.TabControl carrierTabControl;
        private System.Windows.Forms.TabPage generalTabPage;
        private System.Windows.Forms.TabPage canadaTabPage;
    }
}
