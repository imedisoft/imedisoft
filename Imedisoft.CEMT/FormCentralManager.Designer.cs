namespace CentralManager
{
    partial class FormCentralManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralManager));
            this.filterConnectionTextBox = new System.Windows.Forms.TextBox();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemLogoff = new System.Windows.Forms.MenuItem();
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItemPassword = new System.Windows.Forms.MenuItem();
            this.menuItemSetup = new System.Windows.Forms.MenuItem();
            this.menuItemConnections = new System.Windows.Forms.MenuItem();
            this.menuItemDisplayFields = new System.Windows.Forms.MenuItem();
            this.menuItemGroups = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemSecurity = new System.Windows.Forms.MenuItem();
            this.menuItemReports = new System.Windows.Forms.MenuItem();
            this.menuItemAnnualPI = new System.Windows.Forms.MenuItem();
            this.menuTransfer = new System.Windows.Forms.MenuItem();
            this.menuTransferPatient = new System.Windows.Forms.MenuItem();
            this.connectionGroupLabel = new System.Windows.Forms.Label();
            this.connectionGroupComboBox = new System.Windows.Forms.ComboBox();
            this.filterProviderLabel = new System.Windows.Forms.Label();
            this.filterClinicLabel = new System.Windows.Forms.Label();
            this.filterConnectionLabel = new System.Windows.Forms.Label();
            this.filterProviderTextBox = new System.Windows.Forms.TextBox();
            this.filterClinicTextBox = new System.Windows.Forms.TextBox();
            this.gridMainRightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.versionLabel = new System.Windows.Forms.Label();
            this.connectionsGrid = new OpenDental.UI.ODGrid();
            this.filterButton = new OpenDental.UI.Button();
            this.butRefreshStatuses = new OpenDental.UI.Button();
            this.labelFetch = new System.Windows.Forms.Label();
            this.checkLimit = new System.Windows.Forms.CheckBox();
            this.butSearchPats = new OpenDental.UI.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textClinicPatSearch = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.checkGuarantors = new System.Windows.Forms.CheckBox();
            this.textConnPatSearch = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textCountry = new System.Windows.Forms.TextBox();
            this.labelCountry = new System.Windows.Forms.Label();
            this.textEmail = new System.Windows.Forms.TextBox();
            this.labelEmail = new System.Windows.Forms.Label();
            this.textSubscriberID = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBirthdate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkHideArchived = new System.Windows.Forms.CheckBox();
            this.textChartNumber = new System.Windows.Forms.TextBox();
            this.textSSN = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textPatNum = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textState = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textCity = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkHideInactive = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textAddress = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textPhone = new OpenDental.ValidPhone();
            this.label15 = new System.Windows.Forms.Label();
            this.textFName = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textLName = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.patientsGrid = new OpenDental.UI.ODGrid();
            this.filterGroupBox = new System.Windows.Forms.GroupBox();
            this.gridMainRightClickMenu.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.filterGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // filterConnectionTextBox
            // 
            this.filterConnectionTextBox.Location = new System.Drawing.Point(100, 17);
            this.filterConnectionTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.filterConnectionTextBox.Name = "filterConnectionTextBox";
            this.filterConnectionTextBox.Size = new System.Drawing.Size(160, 20);
            this.filterConnectionTextBox.TabIndex = 211;
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemLogoff,
            this.menuItemFile,
            this.menuItemSetup,
            this.menuItemReports,
            this.menuTransfer});
            // 
            // menuItemLogoff
            // 
            this.menuItemLogoff.Index = 0;
            this.menuItemLogoff.Text = "Logoff";
            this.menuItemLogoff.Click += new System.EventHandler(this.LogoffMenuItem_Click);
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 1;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPassword});
            this.menuItemFile.Text = "File";
            // 
            // menuItemPassword
            // 
            this.menuItemPassword.Index = 0;
            this.menuItemPassword.Text = "Change Password";
            this.menuItemPassword.Click += new System.EventHandler(this.PasswordMenuItem_Click);
            // 
            // menuItemSetup
            // 
            this.menuItemSetup.Index = 2;
            this.menuItemSetup.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemConnections,
            this.menuItemDisplayFields,
            this.menuItemGroups,
            this.menuItem1,
            this.menuItemSecurity});
            this.menuItemSetup.Text = "Setup";
            // 
            // menuItemConnections
            // 
            this.menuItemConnections.Index = 0;
            this.menuItemConnections.Text = "Connections";
            this.menuItemConnections.Click += new System.EventHandler(this.ConnectionsMenuItem_Click);
            // 
            // menuItemDisplayFields
            // 
            this.menuItemDisplayFields.Index = 1;
            this.menuItemDisplayFields.Text = "Display Fields";
            this.menuItemDisplayFields.Click += new System.EventHandler(this.DisplayFieldsMenuItem_Click);
            // 
            // menuItemGroups
            // 
            this.menuItemGroups.Index = 2;
            this.menuItemGroups.Text = "Groups";
            this.menuItemGroups.Click += new System.EventHandler(this.GroupsMenuItem_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "Report Permissions";
            this.menuItem1.Click += new System.EventHandler(this.ReportSetupMenuItem_Click);
            // 
            // menuItemSecurity
            // 
            this.menuItemSecurity.Index = 4;
            this.menuItemSecurity.Text = "Security";
            this.menuItemSecurity.Click += new System.EventHandler(this.SecurityMenuItem_Click);
            // 
            // menuItemReports
            // 
            this.menuItemReports.Index = 3;
            this.menuItemReports.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAnnualPI});
            this.menuItemReports.Text = "Reports";
            // 
            // menuItemAnnualPI
            // 
            this.menuItemAnnualPI.Index = 0;
            this.menuItemAnnualPI.Text = "Production and Income";
            this.menuItemAnnualPI.Click += new System.EventHandler(this.ProductionIncomeMenuItem_Click);
            // 
            // menuTransfer
            // 
            this.menuTransfer.Index = 4;
            this.menuTransfer.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuTransferPatient});
            this.menuTransfer.Text = "Transfer";
            // 
            // menuTransferPatient
            // 
            this.menuTransferPatient.Index = 0;
            this.menuTransferPatient.Text = "Patient";
            this.menuTransferPatient.Click += new System.EventHandler(this.TransferPatientMenuItem_Click);
            // 
            // connectionGroupLabel
            // 
            this.connectionGroupLabel.AutoSize = true;
            this.connectionGroupLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.connectionGroupLabel.Location = new System.Drawing.Point(21, 16);
            this.connectionGroupLabel.Name = "connectionGroupLabel";
            this.connectionGroupLabel.Size = new System.Drawing.Size(93, 13);
            this.connectionGroupLabel.TabIndex = 213;
            this.connectionGroupLabel.Text = "Connection Group";
            // 
            // connectionGroupComboBox
            // 
            this.connectionGroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.connectionGroupComboBox.FormattingEnabled = true;
            this.connectionGroupComboBox.Location = new System.Drawing.Point(120, 13);
            this.connectionGroupComboBox.MaxDropDownItems = 20;
            this.connectionGroupComboBox.Name = "connectionGroupComboBox";
            this.connectionGroupComboBox.Size = new System.Drawing.Size(169, 21);
            this.connectionGroupComboBox.TabIndex = 214;
            this.connectionGroupComboBox.SelectionChangeCommitted += new System.EventHandler(this.ConnectionGroupComboBox_SelectionChangeCommitted);
            // 
            // filterProviderLabel
            // 
            this.filterProviderLabel.AutoSize = true;
            this.filterProviderLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filterProviderLabel.Location = new System.Drawing.Point(47, 62);
            this.filterProviderLabel.Name = "filterProviderLabel";
            this.filterProviderLabel.Size = new System.Drawing.Size(47, 13);
            this.filterProviderLabel.TabIndex = 226;
            this.filterProviderLabel.Text = "Provider";
            // 
            // filterClinicLabel
            // 
            this.filterClinicLabel.AutoSize = true;
            this.filterClinicLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filterClinicLabel.Location = new System.Drawing.Point(63, 41);
            this.filterClinicLabel.Name = "filterClinicLabel";
            this.filterClinicLabel.Size = new System.Drawing.Size(31, 13);
            this.filterClinicLabel.TabIndex = 224;
            this.filterClinicLabel.Text = "Clinic";
            // 
            // filterConnectionLabel
            // 
            this.filterConnectionLabel.AutoSize = true;
            this.filterConnectionLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filterConnectionLabel.Location = new System.Drawing.Point(33, 20);
            this.filterConnectionLabel.Name = "filterConnectionLabel";
            this.filterConnectionLabel.Size = new System.Drawing.Size(61, 13);
            this.filterConnectionLabel.TabIndex = 225;
            this.filterConnectionLabel.Text = "Connection";
            // 
            // filterProviderTextBox
            // 
            this.filterProviderTextBox.Location = new System.Drawing.Point(100, 59);
            this.filterProviderTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.filterProviderTextBox.Name = "filterProviderTextBox";
            this.filterProviderTextBox.Size = new System.Drawing.Size(160, 20);
            this.filterProviderTextBox.TabIndex = 213;
            // 
            // filterClinicTextBox
            // 
            this.filterClinicTextBox.Location = new System.Drawing.Point(100, 38);
            this.filterClinicTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.filterClinicTextBox.Name = "filterClinicTextBox";
            this.filterClinicTextBox.Size = new System.Drawing.Size(160, 20);
            this.filterClinicTextBox.TabIndex = 212;
            // 
            // gridMainRightClickMenu
            // 
            this.gridMainRightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.gridMainRightClickMenu.Name = "gridMainRightClickMenu";
            this.gridMainRightClickMenu.Size = new System.Drawing.Size(114, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(113, 22);
            this.toolStripMenuItem1.Text = "Refresh";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.RefreshStatusesButton_Click);
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.versionLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.Location = new System.Drawing.Point(13, 93);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(49, 13);
            this.versionLabel.TabIndex = 227;
            this.versionLabel.Text = "Version";
            // 
            // connectionsGrid
            // 
            this.connectionsGrid.AllowSortingByColumn = true;
            this.connectionsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.connectionsGrid.ContextMenuStrip = this.gridMainRightClickMenu;
            this.connectionsGrid.Location = new System.Drawing.Point(13, 109);
            this.connectionsGrid.Name = "connectionsGrid";
            this.connectionsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.connectionsGrid.Size = new System.Drawing.Size(500, 588);
            this.connectionsGrid.TabIndex = 5;
            this.connectionsGrid.Title = "Connections - Double-click to Launch";
            this.connectionsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.ConnectionsGrid_CellDoubleClick);
            // 
            // filterButton
            // 
            this.filterButton.Location = new System.Drawing.Point(282, 56);
            this.filterButton.Name = "filterButton";
            this.filterButton.Size = new System.Drawing.Size(56, 24);
            this.filterButton.TabIndex = 216;
            this.filterButton.Text = "Filter";
            this.filterButton.UseVisualStyleBackColor = true;
            this.filterButton.Click += new System.EventHandler(this.FilterButton_Click);
            // 
            // butRefreshStatuses
            // 
            this.butRefreshStatuses.Location = new System.Drawing.Point(180, 40);
            this.butRefreshStatuses.Name = "butRefreshStatuses";
            this.butRefreshStatuses.Size = new System.Drawing.Size(110, 25);
            this.butRefreshStatuses.TabIndex = 227;
            this.butRefreshStatuses.Text = "Refresh Statuses";
            this.butRefreshStatuses.UseVisualStyleBackColor = true;
            this.butRefreshStatuses.Click += new System.EventHandler(this.RefreshStatusesButton_Click);
            // 
            // labelFetch
            // 
            this.labelFetch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFetch.AutoSize = true;
            this.labelFetch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFetch.ForeColor = System.Drawing.Color.Red;
            this.labelFetch.Location = new System.Drawing.Point(1107, 581);
            this.labelFetch.Name = "labelFetch";
            this.labelFetch.Size = new System.Drawing.Size(109, 13);
            this.labelFetch.TabIndex = 235;
            this.labelFetch.Text = "Fetching Results...";
            this.labelFetch.Visible = false;
            // 
            // checkLimit
            // 
            this.checkLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkLimit.AutoSize = true;
            this.checkLimit.Checked = true;
            this.checkLimit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkLimit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkLimit.Location = new System.Drawing.Point(6, 436);
            this.checkLimit.Name = "checkLimit";
            this.checkLimit.Size = new System.Drawing.Size(184, 18);
            this.checkLimit.TabIndex = 234;
            this.checkLimit.Text = "Limit 30 patients per connection";
            // 
            // butSearchPats
            // 
            this.butSearchPats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butSearchPats.Location = new System.Drawing.Point(1251, 575);
            this.butSearchPats.Name = "butSearchPats";
            this.butSearchPats.Size = new System.Drawing.Size(80, 25);
            this.butSearchPats.TabIndex = 233;
            this.butSearchPats.Text = "Search";
            this.butSearchPats.Click += new System.EventHandler(this.SearchPatientsButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textClinicPatSearch);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.checkGuarantors);
            this.groupBox2.Controls.Add(this.checkLimit);
            this.groupBox2.Controls.Add(this.textConnPatSearch);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.textCountry);
            this.groupBox2.Controls.Add(this.labelCountry);
            this.groupBox2.Controls.Add(this.textEmail);
            this.groupBox2.Controls.Add(this.labelEmail);
            this.groupBox2.Controls.Add(this.textSubscriberID);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.textBirthdate);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.checkHideArchived);
            this.groupBox2.Controls.Add(this.textChartNumber);
            this.groupBox2.Controls.Add(this.textSSN);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.textPatNum);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.textState);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textCity);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.checkHideInactive);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textAddress);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.textPhone);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.textFName);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.textLName);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(1101, 109);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(230, 460);
            this.groupBox2.TabIndex = 232;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search Criteria";
            // 
            // textClinicPatSearch
            // 
            this.textClinicPatSearch.Location = new System.Drawing.Point(120, 333);
            this.textClinicPatSearch.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textClinicPatSearch.Name = "textClinicPatSearch";
            this.textClinicPatSearch.Size = new System.Drawing.Size(100, 20);
            this.textClinicPatSearch.TabIndex = 50;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(83, 336);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(31, 13);
            this.label18.TabIndex = 51;
            this.label18.Text = "Clinic";
            // 
            // checkGuarantors
            // 
            this.checkGuarantors.AutoSize = true;
            this.checkGuarantors.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkGuarantors.Location = new System.Drawing.Point(6, 373);
            this.checkGuarantors.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.checkGuarantors.Name = "checkGuarantors";
            this.checkGuarantors.Size = new System.Drawing.Size(140, 18);
            this.checkGuarantors.TabIndex = 49;
            this.checkGuarantors.Text = "Show Guarantors Only";
            // 
            // textConnPatSearch
            // 
            this.textConnPatSearch.Location = new System.Drawing.Point(120, 312);
            this.textConnPatSearch.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textConnPatSearch.Name = "textConnPatSearch";
            this.textConnPatSearch.Size = new System.Drawing.Size(100, 20);
            this.textConnPatSearch.TabIndex = 47;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(53, 315);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(61, 13);
            this.label14.TabIndex = 48;
            this.label14.Text = "Connection";
            // 
            // textCountry
            // 
            this.textCountry.Location = new System.Drawing.Point(120, 291);
            this.textCountry.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textCountry.Name = "textCountry";
            this.textCountry.Size = new System.Drawing.Size(100, 20);
            this.textCountry.TabIndex = 12;
            // 
            // labelCountry
            // 
            this.labelCountry.AutoSize = true;
            this.labelCountry.Location = new System.Drawing.Point(68, 294);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(46, 13);
            this.labelCountry.TabIndex = 46;
            this.labelCountry.Text = "Country";
            // 
            // textEmail
            // 
            this.textEmail.Location = new System.Drawing.Point(120, 270);
            this.textEmail.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textEmail.Name = "textEmail";
            this.textEmail.Size = new System.Drawing.Size(100, 20);
            this.textEmail.TabIndex = 11;
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(79, 273);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(35, 13);
            this.labelEmail.TabIndex = 43;
            this.labelEmail.Text = "E-mail";
            // 
            // textSubscriberID
            // 
            this.textSubscriberID.Location = new System.Drawing.Point(120, 249);
            this.textSubscriberID.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textSubscriberID.Name = "textSubscriberID";
            this.textSubscriberID.Size = new System.Drawing.Size(100, 20);
            this.textSubscriberID.TabIndex = 10;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(43, 252);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(71, 13);
            this.label13.TabIndex = 41;
            this.label13.Text = "Subscriber ID";
            // 
            // textBirthdate
            // 
            this.textBirthdate.Location = new System.Drawing.Point(120, 228);
            this.textBirthdate.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textBirthdate.Name = "textBirthdate";
            this.textBirthdate.Size = new System.Drawing.Size(100, 20);
            this.textBirthdate.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 231);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Birthdate";
            // 
            // checkHideArchived
            // 
            this.checkHideArchived.AutoSize = true;
            this.checkHideArchived.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkHideArchived.Location = new System.Drawing.Point(6, 415);
            this.checkHideArchived.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.checkHideArchived.Name = "checkHideArchived";
            this.checkHideArchived.Size = new System.Drawing.Size(149, 18);
            this.checkHideArchived.TabIndex = 25;
            this.checkHideArchived.Text = "Hide Archived/Deceased";
            // 
            // textChartNumber
            // 
            this.textChartNumber.Location = new System.Drawing.Point(120, 207);
            this.textChartNumber.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textChartNumber.Name = "textChartNumber";
            this.textChartNumber.Size = new System.Drawing.Size(100, 20);
            this.textChartNumber.TabIndex = 8;
            // 
            // textSSN
            // 
            this.textSSN.Location = new System.Drawing.Point(120, 165);
            this.textSSN.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textSSN.Name = "textSSN";
            this.textSSN.Size = new System.Drawing.Size(100, 20);
            this.textSSN.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(88, 168);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "SSN";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(40, 210);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Chart Number";
            // 
            // textPatNum
            // 
            this.textPatNum.Location = new System.Drawing.Point(120, 186);
            this.textPatNum.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textPatNum.Name = "textPatNum";
            this.textPatNum.Size = new System.Drawing.Size(100, 20);
            this.textPatNum.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(34, 189);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Patient Number";
            // 
            // textState
            // 
            this.textState.Location = new System.Drawing.Point(120, 144);
            this.textState.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textState.Name = "textState";
            this.textState.Size = new System.Drawing.Size(100, 20);
            this.textState.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(81, 147);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "State";
            // 
            // textCity
            // 
            this.textCity.Location = new System.Drawing.Point(120, 123);
            this.textCity.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textCity.Name = "textCity";
            this.textCity.Size = new System.Drawing.Size(100, 20);
            this.textCity.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(88, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "City";
            // 
            // checkHideInactive
            // 
            this.checkHideInactive.AutoSize = true;
            this.checkHideInactive.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkHideInactive.Location = new System.Drawing.Point(6, 394);
            this.checkHideInactive.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.checkHideInactive.Name = "checkHideInactive";
            this.checkHideInactive.Size = new System.Drawing.Size(137, 18);
            this.checkHideInactive.TabIndex = 44;
            this.checkHideInactive.Text = "Hide Inactive Patients";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Hint: enter values in multiple boxes.";
            // 
            // textAddress
            // 
            this.textAddress.Location = new System.Drawing.Point(120, 102);
            this.textAddress.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textAddress.Name = "textAddress";
            this.textAddress.Size = new System.Drawing.Size(100, 20);
            this.textAddress.TabIndex = 3;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(68, 105);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 13);
            this.label11.TabIndex = 9;
            this.label11.Text = "Address";
            // 
            // textPhone
            // 
            this.textPhone.Location = new System.Drawing.Point(120, 81);
            this.textPhone.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textPhone.Name = "textPhone";
            this.textPhone.Size = new System.Drawing.Size(100, 20);
            this.textPhone.TabIndex = 2;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(48, 84);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(66, 13);
            this.label15.TabIndex = 7;
            this.label15.Text = "Phone (any)";
            // 
            // textFName
            // 
            this.textFName.Location = new System.Drawing.Point(120, 60);
            this.textFName.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textFName.Name = "textFName";
            this.textFName.Size = new System.Drawing.Size(100, 20);
            this.textFName.TabIndex = 1;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(56, 63);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(58, 13);
            this.label16.TabIndex = 5;
            this.label16.Text = "First Name";
            // 
            // textLName
            // 
            this.textLName.Location = new System.Drawing.Point(120, 38);
            this.textLName.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.textLName.Name = "textLName";
            this.textLName.Size = new System.Drawing.Size(100, 20);
            this.textLName.TabIndex = 0;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(57, 41);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(57, 13);
            this.label17.TabIndex = 3;
            this.label17.Text = "Last Name";
            // 
            // patientsGrid
            // 
            this.patientsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.patientsGrid.HScrollVisible = true;
            this.patientsGrid.Location = new System.Drawing.Point(519, 109);
            this.patientsGrid.Name = "patientsGrid";
            this.patientsGrid.Size = new System.Drawing.Size(576, 588);
            this.patientsGrid.TabIndex = 231;
            this.patientsGrid.Title = "Patients - Double-click to Launch Connection";
            this.patientsGrid.TranslationName = "FormPatientSelect";
            this.patientsGrid.WrapText = false;
            this.patientsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.PatientsGrid_CellDoubleClick);
            // 
            // filterGroupBox
            // 
            this.filterGroupBox.Controls.Add(this.filterConnectionTextBox);
            this.filterGroupBox.Controls.Add(this.filterClinicTextBox);
            this.filterGroupBox.Controls.Add(this.filterProviderTextBox);
            this.filterGroupBox.Controls.Add(this.filterProviderLabel);
            this.filterGroupBox.Controls.Add(this.filterConnectionLabel);
            this.filterGroupBox.Controls.Add(this.filterButton);
            this.filterGroupBox.Controls.Add(this.filterClinicLabel);
            this.filterGroupBox.Location = new System.Drawing.Point(303, 13);
            this.filterGroupBox.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.filterGroupBox.Name = "filterGroupBox";
            this.filterGroupBox.Size = new System.Drawing.Size(344, 90);
            this.filterGroupBox.TabIndex = 236;
            this.filterGroupBox.TabStop = false;
            this.filterGroupBox.Text = "Filter Connections";
            // 
            // FormCentralManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 710);
            this.Controls.Add(this.filterGroupBox);
            this.Controls.Add(this.labelFetch);
            this.Controls.Add(this.butSearchPats);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.patientsGrid);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.butRefreshStatuses);
            this.Controls.Add(this.connectionGroupComboBox);
            this.Controls.Add(this.connectionGroupLabel);
            this.Controls.Add(this.connectionsGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(1006, 572);
            this.Name = "FormCentralManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Central Enterprise Management Tool (CEMT)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCentralManager_FormClosing);
            this.Load += new System.EventHandler(this.FormCentralManager_Load);
            this.gridMainRightClickMenu.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.filterGroupBox.ResumeLayout(false);
            this.filterGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.ODGrid connectionsGrid;
        private System.Windows.Forms.TextBox filterConnectionTextBox;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem menuItemSetup;
        private System.Windows.Forms.MenuItem menuItemReports;
        private System.Windows.Forms.MenuItem menuItemConnections;
        private System.Windows.Forms.MenuItem menuItemSecurity;
        private System.Windows.Forms.MenuItem menuItemAnnualPI;
        private System.Windows.Forms.MenuItem menuItemGroups;
        private System.Windows.Forms.Label connectionGroupLabel;
        private System.Windows.Forms.ComboBox connectionGroupComboBox;
        private System.Windows.Forms.MenuItem menuItemLogoff;
        private System.Windows.Forms.MenuItem menuItemFile;
        private System.Windows.Forms.MenuItem menuItemPassword;
        private OpenDental.UI.Button filterButton;
        private System.Windows.Forms.Label filterProviderLabel;
        private System.Windows.Forms.Label filterClinicLabel;
        private System.Windows.Forms.Label filterConnectionLabel;
        private System.Windows.Forms.TextBox filterProviderTextBox;
        private System.Windows.Forms.TextBox filterClinicTextBox;
        private OpenDental.UI.Button butRefreshStatuses;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItemDisplayFields;
        private System.Windows.Forms.ContextMenuStrip gridMainRightClickMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.MenuItem menuTransfer;
        private System.Windows.Forms.MenuItem menuTransferPatient;
        private System.Windows.Forms.Label labelFetch;
        private System.Windows.Forms.CheckBox checkLimit;
        private OpenDental.UI.Button butSearchPats;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkGuarantors;
        private System.Windows.Forms.TextBox textConnPatSearch;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textCountry;
        private System.Windows.Forms.Label labelCountry;
        private System.Windows.Forms.TextBox textEmail;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.TextBox textSubscriberID;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBirthdate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkHideArchived;
        private System.Windows.Forms.TextBox textChartNumber;
        private System.Windows.Forms.TextBox textSSN;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textPatNum;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textState;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textCity;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkHideInactive;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textAddress;
        private System.Windows.Forms.Label label11;
        private OpenDental.ValidPhone textPhone;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textFName;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textLName;
        private System.Windows.Forms.Label label17;
        private OpenDental.UI.ODGrid patientsGrid;
        private System.Windows.Forms.TextBox textClinicPatSearch;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.GroupBox filterGroupBox;
    }
}
