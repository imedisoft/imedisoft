namespace Imedisoft.Forms
{
    partial class FormClinicEdit
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
            this.isHiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.medlabAccountTextBox = new System.Windows.Forms.TextBox();
            this.medlabAccountLabel = new System.Windows.Forms.Label();
            this.abbrTextBox = new System.Windows.Forms.TextBox();
            this.abbrLabel = new System.Windows.Forms.Label();
            this.excludeFromInsVerifyListCheckBox = new System.Windows.Forms.CheckBox();
            this.addressLine1TextBox = new System.Windows.Forms.TextBox();
            this.addressLabel = new System.Windows.Forms.Label();
            this.cityTextBox = new System.Windows.Forms.TextBox();
            this.cityLabel = new System.Windows.Forms.Label();
            this.stateTextBox = new System.Windows.Forms.TextBox();
            this.addressLine2TextBox = new System.Windows.Forms.TextBox();
            this.zipTextBox = new System.Windows.Forms.TextBox();
            this.useBillingAddressOnClaimsCheckBox = new System.Windows.Forms.CheckBox();
            this.billingAddressInfoLabel = new System.Windows.Forms.Label();
            this.billingCityLabel = new System.Windows.Forms.Label();
            this.billingAddressLabel = new System.Windows.Forms.Label();
            this.billingZipTextBox = new System.Windows.Forms.TextBox();
            this.billingAddressLine1TextBox = new System.Windows.Forms.TextBox();
            this.billingStateTextBox = new System.Windows.Forms.TextBox();
            this.billingAddressLine2TextBox = new System.Windows.Forms.TextBox();
            this.billingCityTextBox = new System.Windows.Forms.TextBox();
            this.payToAddressInfoLabel = new System.Windows.Forms.Label();
            this.payToCityLabel = new System.Windows.Forms.Label();
            this.payToZipTextBox = new System.Windows.Forms.TextBox();
            this.payToAddressLabel = new System.Windows.Forms.Label();
            this.payToStateTextBox = new System.Windows.Forms.TextBox();
            this.payToAddressLine1TextBox = new System.Windows.Forms.TextBox();
            this.payToCityTextBox = new System.Windows.Forms.TextBox();
            this.payToAddressLine2TextBox = new System.Windows.Forms.TextBox();
            this.removeSpecialtyButton = new OpenDental.UI.Button();
            this.specialtiesGrid = new OpenDental.UI.ODGrid();
            this.addSpecialtyButton = new OpenDental.UI.Button();
            this.regionComboBox = new System.Windows.Forms.ComboBox();
            this.regionLabel = new System.Windows.Forms.Label();
            this.isMedicalCheckBox = new System.Windows.Forms.CheckBox();
            this.defaultProviderPickButton = new OpenDental.UI.Button();
            this.defaultProviderComboBox = new System.Windows.Forms.ComboBox();
            this.defaultProviderLabel = new System.Windows.Forms.Label();
            this.faxTextBox = new OpenDental.ValidPhone();
            this.faxLabel = new System.Windows.Forms.Label();
            this.insBillingProviderGroupBox = new System.Windows.Forms.GroupBox();
            this.insBillingProviderPickButton = new OpenDental.UI.Button();
            this.insBillingProviderComboBox = new System.Windows.Forms.ComboBox();
            this.insBillingProviderSpecificRadioButton = new System.Windows.Forms.RadioButton();
            this.insBillingProviderTreatingRadioButton = new System.Windows.Forms.RadioButton();
            this.insBillingProviderDefaultRadioButton = new System.Windows.Forms.RadioButton();
            this.emailLabel = new System.Windows.Forms.Label();
            this.placeOfServiceComboBox = new System.Windows.Forms.ComboBox();
            this.placeOfServiceLabel = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.bankNumberTextBox = new System.Windows.Forms.TextBox();
            this.bankNumberLabel = new System.Windows.Forms.Label();
            this.phoneTextBox = new OpenDental.ValidPhone();
            this.phoneLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.emailPickButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.emailNoneButton = new OpenDental.UI.Button();
            this.schedulingNotesTextBox = new System.Windows.Forms.TextBox();
            this.schedulingNotesLabel = new System.Windows.Forms.Label();
            this.procCodeRequiredCheckBox = new System.Windows.Forms.CheckBox();
            this.addressGroupBox = new System.Windows.Forms.GroupBox();
            this.specialtyGroupBox = new System.Windows.Forms.GroupBox();
            this.billingAddressGroupBox = new System.Windows.Forms.GroupBox();
            this.payToAddressGroupBox = new System.Windows.Forms.GroupBox();
            this.insBillingProviderGroupBox.SuspendLayout();
            this.addressGroupBox.SuspendLayout();
            this.specialtyGroupBox.SuspendLayout();
            this.billingAddressGroupBox.SuspendLayout();
            this.payToAddressGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // isHiddenCheckBox
            // 
            this.isHiddenCheckBox.AutoSize = true;
            this.isHiddenCheckBox.Location = new System.Drawing.Point(289, 12);
            this.isHiddenCheckBox.Name = "isHiddenCheckBox";
            this.isHiddenCheckBox.Size = new System.Drawing.Size(71, 17);
            this.isHiddenCheckBox.TabIndex = 36;
            this.isHiddenCheckBox.Text = "Is Hidden";
            // 
            // medlabAccountTextBox
            // 
            this.medlabAccountTextBox.Location = new System.Drawing.Point(210, 444);
            this.medlabAccountTextBox.MaxLength = 255;
            this.medlabAccountTextBox.Name = "medlabAccountTextBox";
            this.medlabAccountTextBox.Size = new System.Drawing.Size(200, 20);
            this.medlabAccountTextBox.TabIndex = 25;
            this.medlabAccountTextBox.Visible = false;
            // 
            // medlabAccountLabel
            // 
            this.medlabAccountLabel.AutoSize = true;
            this.medlabAccountLabel.Location = new System.Drawing.Point(78, 447);
            this.medlabAccountLabel.Name = "medlabAccountLabel";
            this.medlabAccountLabel.Size = new System.Drawing.Size(126, 13);
            this.medlabAccountLabel.TabIndex = 24;
            this.medlabAccountLabel.Text = "MedLab Account Number";
            this.medlabAccountLabel.Visible = false;
            // 
            // abbrTextBox
            // 
            this.abbrTextBox.Location = new System.Drawing.Point(210, 35);
            this.abbrTextBox.Name = "abbrTextBox";
            this.abbrTextBox.Size = new System.Drawing.Size(120, 20);
            this.abbrTextBox.TabIndex = 1;
            // 
            // abbrLabel
            // 
            this.abbrLabel.AutoSize = true;
            this.abbrLabel.Location = new System.Drawing.Point(136, 38);
            this.abbrLabel.Name = "abbrLabel";
            this.abbrLabel.Size = new System.Drawing.Size(68, 13);
            this.abbrLabel.TabIndex = 0;
            this.abbrLabel.Text = "Abbreviation";
            // 
            // excludeFromInsVerifyListCheckBox
            // 
            this.excludeFromInsVerifyListCheckBox.AutoSize = true;
            this.excludeFromInsVerifyListCheckBox.Location = new System.Drawing.Point(210, 166);
            this.excludeFromInsVerifyListCheckBox.Name = "excludeFromInsVerifyListCheckBox";
            this.excludeFromInsVerifyListCheckBox.Size = new System.Drawing.Size(200, 17);
            this.excludeFromInsVerifyListCheckBox.TabIndex = 10;
            this.excludeFromInsVerifyListCheckBox.Text = "Hide From Insurance Verification List";
            // 
            // addressLine1TextBox
            // 
            this.addressLine1TextBox.Location = new System.Drawing.Point(100, 19);
            this.addressLine1TextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.addressLine1TextBox.MaxLength = 255;
            this.addressLine1TextBox.Name = "addressLine1TextBox";
            this.addressLine1TextBox.Size = new System.Drawing.Size(254, 20);
            this.addressLine1TextBox.TabIndex = 1;
            // 
            // addressLabel
            // 
            this.addressLabel.AutoSize = true;
            this.addressLabel.Location = new System.Drawing.Point(48, 22);
            this.addressLabel.Name = "addressLabel";
            this.addressLabel.Size = new System.Drawing.Size(46, 13);
            this.addressLabel.TabIndex = 0;
            this.addressLabel.Text = "Address";
            // 
            // cityTextBox
            // 
            this.cityTextBox.Location = new System.Drawing.Point(100, 65);
            this.cityTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.cityTextBox.MaxLength = 255;
            this.cityTextBox.Name = "cityTextBox";
            this.cityTextBox.Size = new System.Drawing.Size(132, 20);
            this.cityTextBox.TabIndex = 5;
            // 
            // cityLabel
            // 
            this.cityLabel.AutoSize = true;
            this.cityLabel.Location = new System.Drawing.Point(28, 68);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(66, 13);
            this.cityLabel.TabIndex = 4;
            this.cityLabel.Text = "City, ST, Zip";
            // 
            // stateTextBox
            // 
            this.stateTextBox.Location = new System.Drawing.Point(233, 65);
            this.stateTextBox.Margin = new System.Windows.Forms.Padding(1, 0, 1, 3);
            this.stateTextBox.MaxLength = 255;
            this.stateTextBox.Name = "stateTextBox";
            this.stateTextBox.Size = new System.Drawing.Size(50, 20);
            this.stateTextBox.TabIndex = 6;
            // 
            // addressLine2TextBox
            // 
            this.addressLine2TextBox.Location = new System.Drawing.Point(100, 42);
            this.addressLine2TextBox.MaxLength = 255;
            this.addressLine2TextBox.Name = "addressLine2TextBox";
            this.addressLine2TextBox.Size = new System.Drawing.Size(254, 20);
            this.addressLine2TextBox.TabIndex = 3;
            // 
            // zipTextBox
            // 
            this.zipTextBox.Location = new System.Drawing.Point(284, 65);
            this.zipTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.zipTextBox.MaxLength = 255;
            this.zipTextBox.Name = "zipTextBox";
            this.zipTextBox.Size = new System.Drawing.Size(70, 20);
            this.zipTextBox.TabIndex = 7;
            // 
            // useBillingAddressOnClaimsCheckBox
            // 
            this.useBillingAddressOnClaimsCheckBox.AutoSize = true;
            this.useBillingAddressOnClaimsCheckBox.Location = new System.Drawing.Point(100, 44);
            this.useBillingAddressOnClaimsCheckBox.Name = "useBillingAddressOnClaimsCheckBox";
            this.useBillingAddressOnClaimsCheckBox.Size = new System.Drawing.Size(92, 17);
            this.useBillingAddressOnClaimsCheckBox.TabIndex = 1;
            this.useBillingAddressOnClaimsCheckBox.Text = "Use on Claims";
            this.useBillingAddressOnClaimsCheckBox.UseVisualStyleBackColor = true;
            // 
            // billingAddressInfoLabel
            // 
            this.billingAddressInfoLabel.Location = new System.Drawing.Point(6, 16);
            this.billingAddressInfoLabel.Name = "billingAddressInfoLabel";
            this.billingAddressInfoLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.billingAddressInfoLabel.Size = new System.Drawing.Size(348, 25);
            this.billingAddressInfoLabel.TabIndex = 0;
            this.billingAddressInfoLabel.Text = "Optional, for e-Claims. Cannot be a PO Box.";
            this.billingAddressInfoLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // billingCityLabel
            // 
            this.billingCityLabel.AutoSize = true;
            this.billingCityLabel.Location = new System.Drawing.Point(28, 116);
            this.billingCityLabel.Name = "billingCityLabel";
            this.billingCityLabel.Size = new System.Drawing.Size(66, 13);
            this.billingCityLabel.TabIndex = 6;
            this.billingCityLabel.Text = "City, ST, Zip";
            // 
            // billingAddressLabel
            // 
            this.billingAddressLabel.AutoSize = true;
            this.billingAddressLabel.Location = new System.Drawing.Point(48, 70);
            this.billingAddressLabel.Name = "billingAddressLabel";
            this.billingAddressLabel.Size = new System.Drawing.Size(46, 13);
            this.billingAddressLabel.TabIndex = 2;
            this.billingAddressLabel.Text = "Address";
            // 
            // billingZipTextBox
            // 
            this.billingZipTextBox.Location = new System.Drawing.Point(284, 113);
            this.billingZipTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.billingZipTextBox.Name = "billingZipTextBox";
            this.billingZipTextBox.Size = new System.Drawing.Size(70, 20);
            this.billingZipTextBox.TabIndex = 9;
            // 
            // billingAddressLine1TextBox
            // 
            this.billingAddressLine1TextBox.Location = new System.Drawing.Point(100, 67);
            this.billingAddressLine1TextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.billingAddressLine1TextBox.Name = "billingAddressLine1TextBox";
            this.billingAddressLine1TextBox.Size = new System.Drawing.Size(254, 20);
            this.billingAddressLine1TextBox.TabIndex = 3;
            // 
            // billingStateTextBox
            // 
            this.billingStateTextBox.Location = new System.Drawing.Point(233, 113);
            this.billingStateTextBox.Margin = new System.Windows.Forms.Padding(1, 0, 1, 3);
            this.billingStateTextBox.Name = "billingStateTextBox";
            this.billingStateTextBox.Size = new System.Drawing.Size(50, 20);
            this.billingStateTextBox.TabIndex = 8;
            // 
            // billingAddressLine2TextBox
            // 
            this.billingAddressLine2TextBox.Location = new System.Drawing.Point(100, 90);
            this.billingAddressLine2TextBox.Name = "billingAddressLine2TextBox";
            this.billingAddressLine2TextBox.Size = new System.Drawing.Size(254, 20);
            this.billingAddressLine2TextBox.TabIndex = 5;
            // 
            // billingCityTextBox
            // 
            this.billingCityTextBox.Location = new System.Drawing.Point(100, 113);
            this.billingCityTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.billingCityTextBox.Name = "billingCityTextBox";
            this.billingCityTextBox.Size = new System.Drawing.Size(132, 20);
            this.billingCityTextBox.TabIndex = 7;
            // 
            // payToAddressInfoLabel
            // 
            this.payToAddressInfoLabel.Location = new System.Drawing.Point(6, 16);
            this.payToAddressInfoLabel.Name = "payToAddressInfoLabel";
            this.payToAddressInfoLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.payToAddressInfoLabel.Size = new System.Drawing.Size(348, 40);
            this.payToAddressInfoLabel.TabIndex = 0;
            this.payToAddressInfoLabel.Text = "Optional, for e-Claims. Can be a PO Box.\r\nSent in addition to treating or billing" +
    " address.";
            this.payToAddressInfoLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // payToCityLabel
            // 
            this.payToCityLabel.AutoSize = true;
            this.payToCityLabel.Location = new System.Drawing.Point(28, 108);
            this.payToCityLabel.Name = "payToCityLabel";
            this.payToCityLabel.Size = new System.Drawing.Size(66, 13);
            this.payToCityLabel.TabIndex = 5;
            this.payToCityLabel.Text = "City, ST, Zip";
            // 
            // payToZipTextBox
            // 
            this.payToZipTextBox.Location = new System.Drawing.Point(284, 105);
            this.payToZipTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.payToZipTextBox.Name = "payToZipTextBox";
            this.payToZipTextBox.Size = new System.Drawing.Size(70, 20);
            this.payToZipTextBox.TabIndex = 8;
            // 
            // payToAddressLabel
            // 
            this.payToAddressLabel.AutoSize = true;
            this.payToAddressLabel.Location = new System.Drawing.Point(48, 62);
            this.payToAddressLabel.Name = "payToAddressLabel";
            this.payToAddressLabel.Size = new System.Drawing.Size(46, 13);
            this.payToAddressLabel.TabIndex = 1;
            this.payToAddressLabel.Text = "Address";
            // 
            // payToStateTextBox
            // 
            this.payToStateTextBox.Location = new System.Drawing.Point(233, 105);
            this.payToStateTextBox.Margin = new System.Windows.Forms.Padding(1, 0, 1, 3);
            this.payToStateTextBox.Name = "payToStateTextBox";
            this.payToStateTextBox.Size = new System.Drawing.Size(50, 20);
            this.payToStateTextBox.TabIndex = 7;
            // 
            // payToAddressLine1TextBox
            // 
            this.payToAddressLine1TextBox.Location = new System.Drawing.Point(100, 59);
            this.payToAddressLine1TextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.payToAddressLine1TextBox.Name = "payToAddressLine1TextBox";
            this.payToAddressLine1TextBox.Size = new System.Drawing.Size(254, 20);
            this.payToAddressLine1TextBox.TabIndex = 2;
            // 
            // payToCityTextBox
            // 
            this.payToCityTextBox.Location = new System.Drawing.Point(100, 105);
            this.payToCityTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.payToCityTextBox.Name = "payToCityTextBox";
            this.payToCityTextBox.Size = new System.Drawing.Size(132, 20);
            this.payToCityTextBox.TabIndex = 6;
            // 
            // payToAddressLine2TextBox
            // 
            this.payToAddressLine2TextBox.Location = new System.Drawing.Point(100, 82);
            this.payToAddressLine2TextBox.Name = "payToAddressLine2TextBox";
            this.payToAddressLine2TextBox.Size = new System.Drawing.Size(254, 20);
            this.payToAddressLine2TextBox.TabIndex = 4;
            // 
            // removeSpecialtyButton
            // 
            this.removeSpecialtyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeSpecialtyButton.Enabled = false;
            this.removeSpecialtyButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.removeSpecialtyButton.Location = new System.Drawing.Point(324, 50);
            this.removeSpecialtyButton.Name = "removeSpecialtyButton";
            this.removeSpecialtyButton.Size = new System.Drawing.Size(30, 25);
            this.removeSpecialtyButton.TabIndex = 2;
            this.removeSpecialtyButton.UseVisualStyleBackColor = true;
            this.removeSpecialtyButton.Click += new System.EventHandler(this.RemoveSpecialtyButton_Click);
            // 
            // specialtiesGrid
            // 
            this.specialtiesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.specialtiesGrid.Location = new System.Drawing.Point(6, 19);
            this.specialtiesGrid.Name = "specialtiesGrid";
            this.specialtiesGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.specialtiesGrid.Size = new System.Drawing.Size(312, 98);
            this.specialtiesGrid.TabIndex = 0;
            this.specialtiesGrid.Title = "Clinic Specialty";
            this.specialtiesGrid.TranslationName = "TableClinicSpecialty";
            this.specialtiesGrid.SelectionCommitted += new System.EventHandler(this.SpecialtiesGrid_SelectionCommitted);
            // 
            // addSpecialtyButton
            // 
            this.addSpecialtyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addSpecialtyButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addSpecialtyButton.Location = new System.Drawing.Point(324, 19);
            this.addSpecialtyButton.Name = "addSpecialtyButton";
            this.addSpecialtyButton.Size = new System.Drawing.Size(30, 25);
            this.addSpecialtyButton.TabIndex = 1;
            this.addSpecialtyButton.UseVisualStyleBackColor = true;
            this.addSpecialtyButton.Click += new System.EventHandler(this.AddSpecialtyButton_Click);
            // 
            // regionComboBox
            // 
            this.regionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.regionComboBox.FormattingEnabled = true;
            this.regionComboBox.Location = new System.Drawing.Point(210, 139);
            this.regionComboBox.Name = "regionComboBox";
            this.regionComboBox.Size = new System.Drawing.Size(160, 21);
            this.regionComboBox.TabIndex = 9;
            // 
            // regionLabel
            // 
            this.regionLabel.AutoSize = true;
            this.regionLabel.Location = new System.Drawing.Point(164, 142);
            this.regionLabel.Name = "regionLabel";
            this.regionLabel.Size = new System.Drawing.Size(40, 13);
            this.regionLabel.TabIndex = 8;
            this.regionLabel.Text = "Region";
            // 
            // isMedicalCheckBox
            // 
            this.isMedicalCheckBox.AutoSize = true;
            this.isMedicalCheckBox.Location = new System.Drawing.Point(210, 12);
            this.isMedicalCheckBox.Name = "isMedicalCheckBox";
            this.isMedicalCheckBox.Size = new System.Drawing.Size(73, 17);
            this.isMedicalCheckBox.TabIndex = 35;
            this.isMedicalCheckBox.Text = "Is Medical";
            // 
            // defaultProviderPickButton
            // 
            this.defaultProviderPickButton.Location = new System.Drawing.Point(516, 390);
            this.defaultProviderPickButton.Name = "defaultProviderPickButton";
            this.defaultProviderPickButton.Size = new System.Drawing.Size(25, 20);
            this.defaultProviderPickButton.TabIndex = 21;
            this.defaultProviderPickButton.Text = "...";
            this.defaultProviderPickButton.Click += new System.EventHandler(this.DefaultProviderPickButton_Click);
            // 
            // defaultProviderComboBox
            // 
            this.defaultProviderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultProviderComboBox.Location = new System.Drawing.Point(210, 390);
            this.defaultProviderComboBox.Name = "defaultProviderComboBox";
            this.defaultProviderComboBox.Size = new System.Drawing.Size(300, 21);
            this.defaultProviderComboBox.TabIndex = 20;
            // 
            // defaultProviderLabel
            // 
            this.defaultProviderLabel.AutoSize = true;
            this.defaultProviderLabel.Location = new System.Drawing.Point(119, 394);
            this.defaultProviderLabel.Name = "defaultProviderLabel";
            this.defaultProviderLabel.Size = new System.Drawing.Size(85, 13);
            this.defaultProviderLabel.TabIndex = 19;
            this.defaultProviderLabel.Text = "Default Provider";
            // 
            // faxTextBox
            // 
            this.faxTextBox.Location = new System.Drawing.Point(210, 113);
            this.faxTextBox.MaxLength = 255;
            this.faxTextBox.Name = "faxTextBox";
            this.faxTextBox.Size = new System.Drawing.Size(160, 20);
            this.faxTextBox.TabIndex = 7;
            // 
            // faxLabel
            // 
            this.faxLabel.AutoSize = true;
            this.faxLabel.Location = new System.Drawing.Point(179, 116);
            this.faxLabel.Name = "faxLabel";
            this.faxLabel.Size = new System.Drawing.Size(25, 13);
            this.faxLabel.TabIndex = 6;
            this.faxLabel.Text = "Fax";
            // 
            // insBillingProviderGroupBox
            // 
            this.insBillingProviderGroupBox.Controls.Add(this.insBillingProviderPickButton);
            this.insBillingProviderGroupBox.Controls.Add(this.insBillingProviderComboBox);
            this.insBillingProviderGroupBox.Controls.Add(this.insBillingProviderSpecificRadioButton);
            this.insBillingProviderGroupBox.Controls.Add(this.insBillingProviderTreatingRadioButton);
            this.insBillingProviderGroupBox.Controls.Add(this.insBillingProviderDefaultRadioButton);
            this.insBillingProviderGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.insBillingProviderGroupBox.Location = new System.Drawing.Point(210, 264);
            this.insBillingProviderGroupBox.Name = "insBillingProviderGroupBox";
            this.insBillingProviderGroupBox.Size = new System.Drawing.Size(280, 120);
            this.insBillingProviderGroupBox.TabIndex = 18;
            this.insBillingProviderGroupBox.TabStop = false;
            this.insBillingProviderGroupBox.Text = "Default Insurance Billing Provider";
            // 
            // insBillingProviderPickButton
            // 
            this.insBillingProviderPickButton.Enabled = false;
            this.insBillingProviderPickButton.Location = new System.Drawing.Point(232, 87);
            this.insBillingProviderPickButton.Name = "insBillingProviderPickButton";
            this.insBillingProviderPickButton.Size = new System.Drawing.Size(25, 20);
            this.insBillingProviderPickButton.TabIndex = 4;
            this.insBillingProviderPickButton.Text = "...";
            this.insBillingProviderPickButton.Click += new System.EventHandler(this.InsBillingProviderPickButton_Click);
            // 
            // insBillingProviderComboBox
            // 
            this.insBillingProviderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.insBillingProviderComboBox.Enabled = false;
            this.insBillingProviderComboBox.Location = new System.Drawing.Point(6, 87);
            this.insBillingProviderComboBox.Name = "insBillingProviderComboBox";
            this.insBillingProviderComboBox.Size = new System.Drawing.Size(220, 21);
            this.insBillingProviderComboBox.TabIndex = 3;
            // 
            // insBillingProviderSpecificRadioButton
            // 
            this.insBillingProviderSpecificRadioButton.AutoSize = true;
            this.insBillingProviderSpecificRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.insBillingProviderSpecificRadioButton.Location = new System.Drawing.Point(6, 67);
            this.insBillingProviderSpecificRadioButton.Name = "insBillingProviderSpecificRadioButton";
            this.insBillingProviderSpecificRadioButton.Size = new System.Drawing.Size(110, 18);
            this.insBillingProviderSpecificRadioButton.TabIndex = 2;
            this.insBillingProviderSpecificRadioButton.Text = "Specific Provider";
            this.insBillingProviderSpecificRadioButton.CheckedChanged += new System.EventHandler(this.InsBillingProviderSpecificRadioButton_CheckedChanged);
            // 
            // insBillingProviderTreatingRadioButton
            // 
            this.insBillingProviderTreatingRadioButton.AutoSize = true;
            this.insBillingProviderTreatingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.insBillingProviderTreatingRadioButton.Location = new System.Drawing.Point(6, 43);
            this.insBillingProviderTreatingRadioButton.Name = "insBillingProviderTreatingRadioButton";
            this.insBillingProviderTreatingRadioButton.Size = new System.Drawing.Size(114, 18);
            this.insBillingProviderTreatingRadioButton.TabIndex = 1;
            this.insBillingProviderTreatingRadioButton.Text = "Treating Provider";
            // 
            // insBillingProviderDefaultRadioButton
            // 
            this.insBillingProviderDefaultRadioButton.AutoSize = true;
            this.insBillingProviderDefaultRadioButton.Checked = true;
            this.insBillingProviderDefaultRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.insBillingProviderDefaultRadioButton.Location = new System.Drawing.Point(6, 19);
            this.insBillingProviderDefaultRadioButton.Name = "insBillingProviderDefaultRadioButton";
            this.insBillingProviderDefaultRadioButton.Size = new System.Drawing.Size(150, 18);
            this.insBillingProviderDefaultRadioButton.TabIndex = 0;
            this.insBillingProviderDefaultRadioButton.TabStop = true;
            this.insBillingProviderDefaultRadioButton.Text = "Default Practice Provider";
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(131, 215);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(73, 13);
            this.emailLabel.TabIndex = 12;
            this.emailLabel.Text = "Email Address";
            // 
            // placeOfServiceComboBox
            // 
            this.placeOfServiceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.placeOfServiceComboBox.Location = new System.Drawing.Point(210, 417);
            this.placeOfServiceComboBox.MaxDropDownItems = 30;
            this.placeOfServiceComboBox.Name = "placeOfServiceComboBox";
            this.placeOfServiceComboBox.Size = new System.Drawing.Size(300, 21);
            this.placeOfServiceComboBox.TabIndex = 23;
            // 
            // placeOfServiceLabel
            // 
            this.placeOfServiceLabel.AutoSize = true;
            this.placeOfServiceLabel.Location = new System.Drawing.Point(31, 420);
            this.placeOfServiceLabel.Name = "placeOfServiceLabel";
            this.placeOfServiceLabel.Size = new System.Drawing.Size(173, 13);
            this.placeOfServiceLabel.TabIndex = 22;
            this.placeOfServiceLabel.Text = "Default Procedure Place of Service";
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(210, 212);
            this.emailTextBox.MaxLength = 255;
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.ReadOnly = true;
            this.emailTextBox.Size = new System.Drawing.Size(300, 20);
            this.emailTextBox.TabIndex = 13;
            // 
            // bankNumberTextBox
            // 
            this.bankNumberTextBox.Location = new System.Drawing.Point(210, 238);
            this.bankNumberTextBox.MaxLength = 255;
            this.bankNumberTextBox.Name = "bankNumberTextBox";
            this.bankNumberTextBox.Size = new System.Drawing.Size(300, 20);
            this.bankNumberTextBox.TabIndex = 17;
            // 
            // bankNumberLabel
            // 
            this.bankNumberLabel.AutoSize = true;
            this.bankNumberLabel.Location = new System.Drawing.Point(92, 241);
            this.bankNumberLabel.Name = "bankNumberLabel";
            this.bankNumberLabel.Size = new System.Drawing.Size(112, 13);
            this.bankNumberLabel.TabIndex = 16;
            this.bankNumberLabel.Text = "Bank Account Number";
            // 
            // phoneTextBox
            // 
            this.phoneTextBox.Location = new System.Drawing.Point(210, 87);
            this.phoneTextBox.MaxLength = 255;
            this.phoneTextBox.Name = "phoneTextBox";
            this.phoneTextBox.Size = new System.Drawing.Size(160, 20);
            this.phoneTextBox.TabIndex = 5;
            // 
            // phoneLabel
            // 
            this.phoneLabel.AutoSize = true;
            this.phoneLabel.Location = new System.Drawing.Point(167, 90);
            this.phoneLabel.Name = "phoneLabel";
            this.phoneLabel.Size = new System.Drawing.Size(37, 13);
            this.phoneLabel.TabIndex = 4;
            this.phoneLabel.Text = "Phone";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(210, 61);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(300, 20);
            this.descriptionTextBox.TabIndex = 3;
            // 
            // emailPickButton
            // 
            this.emailPickButton.Location = new System.Drawing.Point(516, 212);
            this.emailPickButton.Name = "emailPickButton";
            this.emailPickButton.Size = new System.Drawing.Size(25, 20);
            this.emailPickButton.TabIndex = 14;
            this.emailPickButton.Text = "...";
            this.emailPickButton.Click += new System.EventHandler(this.EmailPickButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(826, 584);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 32;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(912, 584);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 33;
            this.cancelButton.Text = "&Cancel";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(144, 64);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 2;
            this.descriptionLabel.Text = "Description";
            // 
            // emailNoneButton
            // 
            this.emailNoneButton.Enabled = false;
            this.emailNoneButton.Location = new System.Drawing.Point(547, 212);
            this.emailNoneButton.Name = "emailNoneButton";
            this.emailNoneButton.Size = new System.Drawing.Size(40, 20);
            this.emailNoneButton.TabIndex = 15;
            this.emailNoneButton.Text = "None";
            this.emailNoneButton.UseVisualStyleBackColor = true;
            this.emailNoneButton.Click += new System.EventHandler(this.EmailNoneButton_Click);
            // 
            // schedulingNotesTextBox
            // 
            this.schedulingNotesTextBox.Location = new System.Drawing.Point(210, 470);
            this.schedulingNotesTextBox.MaxLength = 255;
            this.schedulingNotesTextBox.Multiline = true;
            this.schedulingNotesTextBox.Name = "schedulingNotesTextBox";
            this.schedulingNotesTextBox.Size = new System.Drawing.Size(300, 70);
            this.schedulingNotesTextBox.TabIndex = 27;
            // 
            // schedulingNotesLabel
            // 
            this.schedulingNotesLabel.AutoSize = true;
            this.schedulingNotesLabel.Location = new System.Drawing.Point(120, 473);
            this.schedulingNotesLabel.Name = "schedulingNotesLabel";
            this.schedulingNotesLabel.Size = new System.Drawing.Size(84, 13);
            this.schedulingNotesLabel.TabIndex = 26;
            this.schedulingNotesLabel.Text = "Scheduling Note";
            // 
            // procCodeRequiredCheckBox
            // 
            this.procCodeRequiredCheckBox.AutoSize = true;
            this.procCodeRequiredCheckBox.Enabled = false;
            this.procCodeRequiredCheckBox.Location = new System.Drawing.Point(210, 189);
            this.procCodeRequiredCheckBox.Name = "procCodeRequiredCheckBox";
            this.procCodeRequiredCheckBox.Size = new System.Drawing.Size(245, 17);
            this.procCodeRequiredCheckBox.TabIndex = 11;
            this.procCodeRequiredCheckBox.Text = "Procedure code required on Rx from this clinic";
            // 
            // addressGroupBox
            // 
            this.addressGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addressGroupBox.Controls.Add(this.cityLabel);
            this.addressGroupBox.Controls.Add(this.cityTextBox);
            this.addressGroupBox.Controls.Add(this.addressLabel);
            this.addressGroupBox.Controls.Add(this.stateTextBox);
            this.addressGroupBox.Controls.Add(this.addressLine1TextBox);
            this.addressGroupBox.Controls.Add(this.zipTextBox);
            this.addressGroupBox.Controls.Add(this.addressLine2TextBox);
            this.addressGroupBox.Location = new System.Drawing.Point(632, 12);
            this.addressGroupBox.Name = "addressGroupBox";
            this.addressGroupBox.Size = new System.Drawing.Size(360, 100);
            this.addressGroupBox.TabIndex = 28;
            this.addressGroupBox.TabStop = false;
            this.addressGroupBox.Text = "Physical Treating Address";
            // 
            // specialtyGroupBox
            // 
            this.specialtyGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.specialtyGroupBox.Controls.Add(this.removeSpecialtyButton);
            this.specialtyGroupBox.Controls.Add(this.specialtiesGrid);
            this.specialtyGroupBox.Controls.Add(this.addSpecialtyButton);
            this.specialtyGroupBox.Location = new System.Drawing.Point(632, 420);
            this.specialtyGroupBox.Name = "specialtyGroupBox";
            this.specialtyGroupBox.Size = new System.Drawing.Size(360, 123);
            this.specialtyGroupBox.TabIndex = 31;
            this.specialtyGroupBox.TabStop = false;
            this.specialtyGroupBox.Text = "Specialty";
            // 
            // billingAddressGroupBox
            // 
            this.billingAddressGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.billingAddressGroupBox.Controls.Add(this.billingCityLabel);
            this.billingAddressGroupBox.Controls.Add(this.useBillingAddressOnClaimsCheckBox);
            this.billingAddressGroupBox.Controls.Add(this.billingZipTextBox);
            this.billingAddressGroupBox.Controls.Add(this.billingAddressInfoLabel);
            this.billingAddressGroupBox.Controls.Add(this.billingStateTextBox);
            this.billingAddressGroupBox.Controls.Add(this.billingAddressLine1TextBox);
            this.billingAddressGroupBox.Controls.Add(this.billingCityTextBox);
            this.billingAddressGroupBox.Controls.Add(this.billingAddressLine2TextBox);
            this.billingAddressGroupBox.Controls.Add(this.billingAddressLabel);
            this.billingAddressGroupBox.Location = new System.Drawing.Point(632, 118);
            this.billingAddressGroupBox.Name = "billingAddressGroupBox";
            this.billingAddressGroupBox.Size = new System.Drawing.Size(360, 150);
            this.billingAddressGroupBox.TabIndex = 29;
            this.billingAddressGroupBox.TabStop = false;
            this.billingAddressGroupBox.Text = "Billing Address";
            // 
            // payToAddressGroupBox
            // 
            this.payToAddressGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.payToAddressGroupBox.Controls.Add(this.payToCityLabel);
            this.payToAddressGroupBox.Controls.Add(this.payToAddressInfoLabel);
            this.payToAddressGroupBox.Controls.Add(this.payToAddressLine1TextBox);
            this.payToAddressGroupBox.Controls.Add(this.payToAddressLabel);
            this.payToAddressGroupBox.Controls.Add(this.payToZipTextBox);
            this.payToAddressGroupBox.Controls.Add(this.payToAddressLine2TextBox);
            this.payToAddressGroupBox.Controls.Add(this.payToCityTextBox);
            this.payToAddressGroupBox.Controls.Add(this.payToStateTextBox);
            this.payToAddressGroupBox.Location = new System.Drawing.Point(632, 274);
            this.payToAddressGroupBox.Name = "payToAddressGroupBox";
            this.payToAddressGroupBox.Size = new System.Drawing.Size(360, 140);
            this.payToAddressGroupBox.TabIndex = 30;
            this.payToAddressGroupBox.TabStop = false;
            this.payToAddressGroupBox.Text = "Pay To Address";
            // 
            // FormClinicEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(1004, 621);
            this.Controls.Add(this.payToAddressGroupBox);
            this.Controls.Add(this.billingAddressGroupBox);
            this.Controls.Add(this.specialtyGroupBox);
            this.Controls.Add(this.addressGroupBox);
            this.Controls.Add(this.procCodeRequiredCheckBox);
            this.Controls.Add(this.schedulingNotesTextBox);
            this.Controls.Add(this.schedulingNotesLabel);
            this.Controls.Add(this.emailNoneButton);
            this.Controls.Add(this.isHiddenCheckBox);
            this.Controls.Add(this.medlabAccountTextBox);
            this.Controls.Add(this.medlabAccountLabel);
            this.Controls.Add(this.abbrTextBox);
            this.Controls.Add(this.abbrLabel);
            this.Controls.Add(this.excludeFromInsVerifyListCheckBox);
            this.Controls.Add(this.regionComboBox);
            this.Controls.Add(this.regionLabel);
            this.Controls.Add(this.isMedicalCheckBox);
            this.Controls.Add(this.defaultProviderPickButton);
            this.Controls.Add(this.defaultProviderComboBox);
            this.Controls.Add(this.defaultProviderLabel);
            this.Controls.Add(this.faxTextBox);
            this.Controls.Add(this.faxLabel);
            this.Controls.Add(this.insBillingProviderGroupBox);
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.placeOfServiceComboBox);
            this.Controls.Add(this.placeOfServiceLabel);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.bankNumberTextBox);
            this.Controls.Add(this.bankNumberLabel);
            this.Controls.Add(this.phoneTextBox);
            this.Controls.Add(this.phoneLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.emailPickButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.descriptionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormClinicEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Clinic";
            this.Load += new System.EventHandler(this.FormClinicEdit_Load);
            this.insBillingProviderGroupBox.ResumeLayout(false);
            this.insBillingProviderGroupBox.PerformLayout();
            this.addressGroupBox.ResumeLayout(false);
            this.addressGroupBox.PerformLayout();
            this.specialtyGroupBox.ResumeLayout(false);
            this.billingAddressGroupBox.ResumeLayout(false);
            this.billingAddressGroupBox.PerformLayout();
            this.payToAddressGroupBox.ResumeLayout(false);
            this.payToAddressGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private OpenDental.ValidPhone phoneTextBox;
		private System.Windows.Forms.Label phoneLabel;
		private System.Windows.Forms.TextBox bankNumberTextBox;
		private System.Windows.Forms.Label bankNumberLabel;
		private System.Windows.Forms.Label placeOfServiceLabel;
		private System.Windows.Forms.ComboBox placeOfServiceComboBox;
		private System.Windows.Forms.GroupBox insBillingProviderGroupBox;
        private System.Windows.Forms.ComboBox insBillingProviderComboBox;
		private System.Windows.Forms.RadioButton insBillingProviderSpecificRadioButton;
		private System.Windows.Forms.RadioButton insBillingProviderTreatingRadioButton;
		private System.Windows.Forms.RadioButton insBillingProviderDefaultRadioButton;
		private OpenDental.ValidPhone faxTextBox;
		private System.Windows.Forms.Label faxLabel;
		private System.Windows.Forms.Label emailLabel;
		private System.Windows.Forms.TextBox emailTextBox;
		private System.Windows.Forms.Label defaultProviderLabel;
		private System.Windows.Forms.ComboBox defaultProviderComboBox;
		private OpenDental.UI.Button defaultProviderPickButton;
		private OpenDental.UI.Button emailPickButton;
		private OpenDental.UI.Button insBillingProviderPickButton;
		private System.Windows.Forms.CheckBox isMedicalCheckBox;
		private System.Windows.Forms.TextBox cityTextBox;
		private System.Windows.Forms.TextBox stateTextBox;
		private System.Windows.Forms.TextBox zipTextBox;
		private System.Windows.Forms.TextBox addressLine2TextBox;
		private System.Windows.Forms.Label cityLabel;
		private System.Windows.Forms.TextBox addressLine1TextBox;
		private System.Windows.Forms.Label addressLabel;
		private System.Windows.Forms.Label payToAddressInfoLabel;
		private System.Windows.Forms.TextBox payToZipTextBox;
		private System.Windows.Forms.TextBox payToStateTextBox;
		private System.Windows.Forms.TextBox payToCityTextBox;
		private System.Windows.Forms.TextBox payToAddressLine2TextBox;
		private System.Windows.Forms.TextBox payToAddressLine1TextBox;
		private System.Windows.Forms.Label payToAddressLabel;
		private System.Windows.Forms.Label payToCityLabel;
		private System.Windows.Forms.Label billingAddressInfoLabel;
		private System.Windows.Forms.TextBox billingZipTextBox;
		private System.Windows.Forms.TextBox billingStateTextBox;
		private System.Windows.Forms.TextBox billingCityTextBox;
		private System.Windows.Forms.TextBox billingAddressLine2TextBox;
		private System.Windows.Forms.TextBox billingAddressLine1TextBox;
		private System.Windows.Forms.Label billingAddressLabel;
		private System.Windows.Forms.Label billingCityLabel;
		private System.Windows.Forms.CheckBox useBillingAddressOnClaimsCheckBox;
		private System.Windows.Forms.Label regionLabel;
		private System.Windows.Forms.ComboBox regionComboBox;
		private System.Windows.Forms.CheckBox excludeFromInsVerifyListCheckBox;
		private System.Windows.Forms.TextBox abbrTextBox;
		private System.Windows.Forms.Label abbrLabel;
		private System.Windows.Forms.TextBox medlabAccountTextBox;
		private System.Windows.Forms.Label medlabAccountLabel;
		private System.Windows.Forms.CheckBox isHiddenCheckBox;
		private OpenDental.UI.Button emailNoneButton;
		private OpenDental.UI.Button addSpecialtyButton;
		private OpenDental.UI.ODGrid specialtiesGrid;
		private OpenDental.UI.Button removeSpecialtyButton;
		private System.Windows.Forms.TextBox schedulingNotesTextBox;
		private System.Windows.Forms.Label schedulingNotesLabel;
        private System.Windows.Forms.GroupBox addressGroupBox;
        private System.Windows.Forms.GroupBox specialtyGroupBox;
        private System.Windows.Forms.GroupBox billingAddressGroupBox;
        private System.Windows.Forms.GroupBox payToAddressGroupBox;
        private System.Windows.Forms.CheckBox procCodeRequiredCheckBox;
    }
}
