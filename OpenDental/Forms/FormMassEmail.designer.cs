namespace OpenDental{
	partial class FormMassEmail {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMassEmail));
			this.butClose = new OpenDental.UI.Button();
			this.labelPatsSelected = new System.Windows.Forms.Label();
			this.labelNumberPats = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPatients = new System.Windows.Forms.TabPage();
			this.groupBoxFilters = new System.Windows.Forms.GroupBox();
			this.labelPatBillingType = new System.Windows.Forms.Label();
			this.listBoxPatBillingType = new System.Windows.Forms.ListBox();
			this.checkHideSeenSince = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textAgeTo = new OpenDental.ValidNum();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textAgeFrom = new OpenDental.ValidNum();
			this.comboClinicPatient = new OpenDental.UI.ComboBoxClinicPicker();
			this.checkHideNotSeenSince = new System.Windows.Forms.CheckBox();
			this.labelRefreshNeeded = new System.Windows.Forms.Label();
			this.butRefreshPatientFilters = new OpenDental.UI.Button();
			this.checkHiddenFutureAppt = new System.Windows.Forms.CheckBox();
			this.labelPatStatus = new System.Windows.Forms.Label();
			this.labelContact = new System.Windows.Forms.Label();
			this.groupBoxRecipients = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lableDays = new System.Windows.Forms.Label();
			this.textNumDays = new OpenDental.ValidNum();
			this.checkExcludeWithin = new System.Windows.Forms.CheckBox();
			this.listBoxPatStatus = new System.Windows.Forms.ListBox();
			this.listBoxContactMethod = new System.Windows.Forms.ListBox();
			this.datePickerNotSeenSince = new OpenDental.UI.ODDatePicker();
			this.datePickerSeenSince = new OpenDental.UI.ODDatePicker();
			this.butSelectAllSelected = new OpenDental.UI.Button();
			this.butSelectAllAvailable = new OpenDental.UI.Button();
			this.butMoveToAvailable = new OpenDental.UI.Button();
			this.butMoveToSelected = new OpenDental.UI.Button();
			this.gridSelectedPatients = new OpenDental.UI.ODGrid();
			this.gridAvailablePatients = new OpenDental.UI.ODGrid();
			this.tabTemplate = new System.Windows.Forms.TabPage();
			this.userControlEmailTemplate1 = new OpenDental.UserControlEmailTemplate();
			this.butEditTemplate = new OpenDental.UI.Button();
			this.butImport = new OpenDental.UI.Button();
			this.butNewTemplate = new OpenDental.UI.Button();
			this.butCopy = new OpenDental.UI.Button();
			this.butDeleteTemplate = new OpenDental.UI.Button();
			this.gridTemplates = new OpenDental.UI.ODGrid();
			this.tabAnalytics = new System.Windows.Forms.TabPage();
			this.comboClinicAnalytics = new OpenDental.UI.ComboBoxClinicPicker();
			this.butRefreshAnalytics = new OpenDental.UI.Button();
			this.gridAnalytics = new OpenDental.UI.ODGrid();
			this.dateRangeAnalytics = new OpenDental.UI.ODDateRangePicker();
			this.butSendEmails = new OpenDental.UI.Button();
			this.labelPleaseActivate = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabPatients.SuspendLayout();
			this.groupBoxFilters.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBoxRecipients.SuspendLayout();
			this.tabTemplate.SuspendLayout();
			this.tabAnalytics.SuspendLayout();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(1138, 728);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(80, 24);
			this.butClose.TabIndex = 10;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// labelPatsSelected
			// 
			this.labelPatsSelected.Location = new System.Drawing.Point(2, 19);
			this.labelPatsSelected.Name = "labelPatsSelected";
			this.labelPatsSelected.Size = new System.Drawing.Size(150, 16);
			this.labelPatsSelected.TabIndex = 92;
			this.labelPatsSelected.Text = "# of Selected Patients:";
			this.labelPatsSelected.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelNumberPats
			// 
			this.labelNumberPats.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelNumberPats.ForeColor = System.Drawing.Color.LimeGreen;
			this.labelNumberPats.Location = new System.Drawing.Point(154, 17);
			this.labelNumberPats.Name = "labelNumberPats";
			this.labelNumberPats.Size = new System.Drawing.Size(59, 17);
			this.labelNumberPats.TabIndex = 128;
			this.labelNumberPats.Text = "0";
			this.labelNumberPats.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPatients);
			this.tabControl1.Controls.Add(this.tabTemplate);
			this.tabControl1.Controls.Add(this.tabAnalytics);
			this.tabControl1.Location = new System.Drawing.Point(12, 67);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1208, 643);
			this.tabControl1.TabIndex = 129;
			// 
			// tabPatients
			// 
			this.tabPatients.BackColor = System.Drawing.SystemColors.Control;
			this.tabPatients.Controls.Add(this.groupBoxFilters);
			this.tabPatients.Controls.Add(this.butSelectAllSelected);
			this.tabPatients.Controls.Add(this.butSelectAllAvailable);
			this.tabPatients.Controls.Add(this.butMoveToAvailable);
			this.tabPatients.Controls.Add(this.butMoveToSelected);
			this.tabPatients.Controls.Add(this.gridSelectedPatients);
			this.tabPatients.Controls.Add(this.gridAvailablePatients);
			this.tabPatients.Location = new System.Drawing.Point(4, 22);
			this.tabPatients.Name = "tabPatients";
			this.tabPatients.Padding = new System.Windows.Forms.Padding(3);
			this.tabPatients.Size = new System.Drawing.Size(1200, 617);
			this.tabPatients.TabIndex = 0;
			this.tabPatients.Text = "Patients";
			// 
			// groupBoxFilters
			// 
			this.groupBoxFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxFilters.Controls.Add(this.labelPatBillingType);
			this.groupBoxFilters.Controls.Add(this.listBoxPatBillingType);
			this.groupBoxFilters.Controls.Add(this.checkHideSeenSince);
			this.groupBoxFilters.Controls.Add(this.groupBox1);
			this.groupBoxFilters.Controls.Add(this.comboClinicPatient);
			this.groupBoxFilters.Controls.Add(this.checkHideNotSeenSince);
			this.groupBoxFilters.Controls.Add(this.labelRefreshNeeded);
			this.groupBoxFilters.Controls.Add(this.butRefreshPatientFilters);
			this.groupBoxFilters.Controls.Add(this.checkHiddenFutureAppt);
			this.groupBoxFilters.Controls.Add(this.labelPatStatus);
			this.groupBoxFilters.Controls.Add(this.labelContact);
			this.groupBoxFilters.Controls.Add(this.groupBoxRecipients);
			this.groupBoxFilters.Controls.Add(this.listBoxPatStatus);
			this.groupBoxFilters.Controls.Add(this.listBoxContactMethod);
			this.groupBoxFilters.Controls.Add(this.datePickerNotSeenSince);
			this.groupBoxFilters.Controls.Add(this.datePickerSeenSince);
			this.groupBoxFilters.Location = new System.Drawing.Point(9, 6);
			this.groupBoxFilters.Name = "groupBoxFilters";
			this.groupBoxFilters.Size = new System.Drawing.Size(1182, 166);
			this.groupBoxFilters.TabIndex = 212;
			this.groupBoxFilters.TabStop = false;
			this.groupBoxFilters.Text = "Filters";
			// 
			// labelPatBillingType
			// 
			this.labelPatBillingType.Location = new System.Drawing.Point(342, 16);
			this.labelPatBillingType.Name = "labelPatBillingType";
			this.labelPatBillingType.Size = new System.Drawing.Size(154, 16);
			this.labelPatBillingType.TabIndex = 221;
			this.labelPatBillingType.Text = "Patient Billing Type";
			this.labelPatBillingType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listBoxPatBillingType
			// 
			this.listBoxPatBillingType.Location = new System.Drawing.Point(342, 35);
			this.listBoxPatBillingType.Name = "listBoxPatBillingType";
			this.listBoxPatBillingType.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxPatBillingType.Size = new System.Drawing.Size(154, 121);
			this.listBoxPatBillingType.TabIndex = 220;
			this.listBoxPatBillingType.SelectedIndexChanged += new System.EventHandler(this.listBoxPatBillingType_SelectedIndexChanged);
			// 
			// checkHideSeenSince
			// 
			this.checkHideSeenSince.Location = new System.Drawing.Point(505, 140);
			this.checkHideSeenSince.Name = "checkHideSeenSince";
			this.checkHideSeenSince.Size = new System.Drawing.Size(160, 18);
			this.checkHideSeenSince.TabIndex = 219;
			this.checkHideSeenSince.Text = "Exclude patients seen since";
			this.checkHideSeenSince.UseVisualStyleBackColor = true;
			this.checkHideSeenSince.Click += new System.EventHandler(this.checkBoxHideSeenSince_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textAgeTo);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.textAgeFrom);
			this.groupBox1.Location = new System.Drawing.Point(783, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(176, 62);
			this.groupBox1.TabIndex = 217;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Patient Age";
			// 
			// textAgeTo
			// 
			this.textAgeTo.Location = new System.Drawing.Point(124, 26);
			this.textAgeTo.MaxVal = 255;
			this.textAgeTo.MinVal = 0;
			this.textAgeTo.Name = "textAgeTo";
			this.textAgeTo.Size = new System.Drawing.Size(39, 20);
			this.textAgeTo.TabIndex = 209;
			this.textAgeTo.TextChanged += new System.EventHandler(this.textAgeTo_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 26);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(44, 17);
			this.label3.TabIndex = 208;
			this.label3.Text = "From";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(101, 26);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(23, 17);
			this.label4.TabIndex = 207;
			this.label4.Text = "To";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAgeFrom
			// 
			this.textAgeFrom.Location = new System.Drawing.Point(51, 26);
			this.textAgeFrom.MaxVal = 255;
			this.textAgeFrom.MinVal = 0;
			this.textAgeFrom.Name = "textAgeFrom";
			this.textAgeFrom.Size = new System.Drawing.Size(39, 20);
			this.textAgeFrom.TabIndex = 130;
			this.textAgeFrom.TextChanged += new System.EventHandler(this.textAgeFrom_TextChanged);
			// 
			// comboClinicPatient
			// 
			this.comboClinicPatient.IncludeAll = true;
			this.comboClinicPatient.Location = new System.Drawing.Point(976, 22);
			this.comboClinicPatient.Name = "comboClinicPatient";
			this.comboClinicPatient.Size = new System.Drawing.Size(200, 22);
			this.comboClinicPatient.TabIndex = 216;
			this.comboClinicPatient.SelectionChangeCommitted += new System.EventHandler(this.comboClinicPatient_SelectionChangeCommitted);
			// 
			// checkHideNotSeenSince
			// 
			this.checkHideNotSeenSince.Location = new System.Drawing.Point(505, 113);
			this.checkHideNotSeenSince.Name = "checkHideNotSeenSince";
			this.checkHideNotSeenSince.Size = new System.Drawing.Size(195, 18);
			this.checkHideNotSeenSince.TabIndex = 218;
			this.checkHideNotSeenSince.Text = "Exclude patients not seen since";
			this.checkHideNotSeenSince.UseVisualStyleBackColor = true;
			this.checkHideNotSeenSince.Click += new System.EventHandler(this.checkHideNotSeenSince_Click);
			// 
			// labelRefreshNeeded
			// 
			this.labelRefreshNeeded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelRefreshNeeded.ForeColor = System.Drawing.Color.Firebrick;
			this.labelRefreshNeeded.Location = new System.Drawing.Point(982, 112);
			this.labelRefreshNeeded.Name = "labelRefreshNeeded";
			this.labelRefreshNeeded.Size = new System.Drawing.Size(194, 19);
			this.labelRefreshNeeded.TabIndex = 215;
			this.labelRefreshNeeded.Text = "Filters changed, refresh needed";
			this.labelRefreshNeeded.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelRefreshNeeded.Visible = false;
			// 
			// butRefreshPatientFilters
			// 
			this.butRefreshPatientFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefreshPatientFilters.Location = new System.Drawing.Point(1091, 136);
			this.butRefreshPatientFilters.Name = "butRefreshPatientFilters";
			this.butRefreshPatientFilters.Size = new System.Drawing.Size(85, 24);
			this.butRefreshPatientFilters.TabIndex = 214;
			this.butRefreshPatientFilters.Text = "Refresh";
			this.butRefreshPatientFilters.Click += new System.EventHandler(this.butRefreshPatientFilters_Click);
			// 
			// checkHiddenFutureAppt
			// 
			this.checkHiddenFutureAppt.Location = new System.Drawing.Point(505, 86);
			this.checkHiddenFutureAppt.Name = "checkHiddenFutureAppt";
			this.checkHiddenFutureAppt.Size = new System.Drawing.Size(288, 18);
			this.checkHiddenFutureAppt.TabIndex = 213;
			this.checkHiddenFutureAppt.Text = "Hide patients with future appointments";
			this.checkHiddenFutureAppt.UseVisualStyleBackColor = true;
			this.checkHiddenFutureAppt.Click += new System.EventHandler(this.checkHiddenFutureAppt_Click);
			// 
			// labelPatStatus
			// 
			this.labelPatStatus.Location = new System.Drawing.Point(181, 16);
			this.labelPatStatus.Name = "labelPatStatus";
			this.labelPatStatus.Size = new System.Drawing.Size(154, 16);
			this.labelPatStatus.TabIndex = 212;
			this.labelPatStatus.Text = "Patient Status";
			this.labelPatStatus.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelContact
			// 
			this.labelContact.Location = new System.Drawing.Point(13, 16);
			this.labelContact.Name = "labelContact";
			this.labelContact.Size = new System.Drawing.Size(154, 16);
			this.labelContact.TabIndex = 211;
			this.labelContact.Text = "Preferred Contact Method";
			this.labelContact.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupBoxRecipients
			// 
			this.groupBoxRecipients.Controls.Add(this.label1);
			this.groupBoxRecipients.Controls.Add(this.lableDays);
			this.groupBoxRecipients.Controls.Add(this.textNumDays);
			this.groupBoxRecipients.Controls.Add(this.checkExcludeWithin);
			this.groupBoxRecipients.Location = new System.Drawing.Point(502, 16);
			this.groupBoxRecipients.Name = "groupBoxRecipients";
			this.groupBoxRecipients.Size = new System.Drawing.Size(275, 62);
			this.groupBoxRecipients.TabIndex = 210;
			this.groupBoxRecipients.TabStop = false;
			this.groupBoxRecipients.Text = "Mass Email Recipients";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(32, 39);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(61, 18);
			this.label1.TabIndex = 208;
			this.label1.Text = "In the last";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lableDays
			// 
			this.lableDays.Location = new System.Drawing.Point(137, 39);
			this.lableDays.Name = "lableDays";
			this.lableDays.Size = new System.Drawing.Size(61, 18);
			this.lableDays.TabIndex = 207;
			this.lableDays.Text = "days";
			this.lableDays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textNumDays
			// 
			this.textNumDays.Location = new System.Drawing.Point(93, 38);
			this.textNumDays.MaxVal = 255;
			this.textNumDays.MinVal = 0;
			this.textNumDays.Name = "textNumDays";
			this.textNumDays.Size = new System.Drawing.Size(39, 20);
			this.textNumDays.TabIndex = 130;
			this.textNumDays.TextChanged += new System.EventHandler(this.textNumDays_TextChanged);
			// 
			// checkExcludeWithin
			// 
			this.checkExcludeWithin.Location = new System.Drawing.Point(6, 19);
			this.checkExcludeWithin.Name = "checkExcludeWithin";
			this.checkExcludeWithin.Size = new System.Drawing.Size(266, 18);
			this.checkExcludeWithin.TabIndex = 129;
			this.checkExcludeWithin.Text = "Exclude patients who received a mass email\r\n";
			this.checkExcludeWithin.UseVisualStyleBackColor = true;
			this.checkExcludeWithin.Click += new System.EventHandler(this.checkExcludeWithin_Click);
			// 
			// listBoxPatStatus
			// 
			this.listBoxPatStatus.Location = new System.Drawing.Point(181, 35);
			this.listBoxPatStatus.Name = "listBoxPatStatus";
			this.listBoxPatStatus.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxPatStatus.Size = new System.Drawing.Size(154, 121);
			this.listBoxPatStatus.TabIndex = 209;
			this.listBoxPatStatus.SelectedIndexChanged += new System.EventHandler(this.listBoxPatStatus_SelectedIndexChanged);
			// 
			// listBoxContactMethod
			// 
			this.listBoxContactMethod.Location = new System.Drawing.Point(13, 35);
			this.listBoxContactMethod.Name = "listBoxContactMethod";
			this.listBoxContactMethod.Size = new System.Drawing.Size(161, 121);
			this.listBoxContactMethod.TabIndex = 208;
			this.listBoxContactMethod.SelectedIndexChanged += new System.EventHandler(this.listBoxContactMethod_SelectedIndexChanged);
			// 
			// datePickerNotSeenSince
			// 
			this.datePickerNotSeenSince.BackColor = System.Drawing.Color.Transparent;
			this.datePickerNotSeenSince.DefaultDateTime = new System.DateTime(2020, 1, 1, 0, 0, 0, 0);
			this.datePickerNotSeenSince.Location = new System.Drawing.Point(637, 110);
			this.datePickerNotSeenSince.MaximumSize = new System.Drawing.Size(0, 184);
			this.datePickerNotSeenSince.MinimumSize = new System.Drawing.Size(227, 23);
			this.datePickerNotSeenSince.Name = "datePickerNotSeenSince";
			this.datePickerNotSeenSince.Size = new System.Drawing.Size(227, 23);
			this.datePickerNotSeenSince.TabIndex = 222;
			// 
			// datePickerSeenSince
			// 
			this.datePickerSeenSince.BackColor = System.Drawing.Color.Transparent;
			this.datePickerSeenSince.DefaultDateTime = new System.DateTime(2020, 1, 1, 0, 0, 0, 0);
			this.datePickerSeenSince.Location = new System.Drawing.Point(637, 138);
			this.datePickerSeenSince.MaximumSize = new System.Drawing.Size(0, 184);
			this.datePickerSeenSince.MinimumSize = new System.Drawing.Size(227, 23);
			this.datePickerSeenSince.Name = "datePickerSeenSince";
			this.datePickerSeenSince.Size = new System.Drawing.Size(227, 23);
			this.datePickerSeenSince.TabIndex = 223;
			// 
			// butSelectAllSelected
			// 
			this.butSelectAllSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSelectAllSelected.Location = new System.Drawing.Point(729, 519);
			this.butSelectAllSelected.Name = "butSelectAllSelected";
			this.butSelectAllSelected.Size = new System.Drawing.Size(85, 24);
			this.butSelectAllSelected.TabIndex = 211;
			this.butSelectAllSelected.Text = "Select All";
			this.butSelectAllSelected.Click += new System.EventHandler(this.butSelectAllSelected_Click);
			// 
			// butSelectAllAvailable
			// 
			this.butSelectAllAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSelectAllAvailable.Location = new System.Drawing.Point(9, 519);
			this.butSelectAllAvailable.Name = "butSelectAllAvailable";
			this.butSelectAllAvailable.Size = new System.Drawing.Size(85, 24);
			this.butSelectAllAvailable.TabIndex = 210;
			this.butSelectAllAvailable.Text = "Select All";
			this.butSelectAllAvailable.Click += new System.EventHandler(this.butSelectAllAvailable_Click);
			// 
			// butMoveToAvailable
			// 
			this.butMoveToAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butMoveToAvailable.Image = global::Imedisoft.Properties.Resources.Left;
			this.butMoveToAvailable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butMoveToAvailable.Location = new System.Drawing.Point(690, 347);
			this.butMoveToAvailable.Name = "butMoveToAvailable";
			this.butMoveToAvailable.Size = new System.Drawing.Size(33, 23);
			this.butMoveToAvailable.TabIndex = 209;
			this.butMoveToAvailable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.butMoveToAvailable.Click += new System.EventHandler(this.butMoveToAvailable_Click);
			// 
			// butMoveToSelected
			// 
			this.butMoveToSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butMoveToSelected.Image = global::Imedisoft.Properties.Resources.Right;
			this.butMoveToSelected.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butMoveToSelected.Location = new System.Drawing.Point(690, 254);
			this.butMoveToSelected.Name = "butMoveToSelected";
			this.butMoveToSelected.Size = new System.Drawing.Size(33, 23);
			this.butMoveToSelected.TabIndex = 208;
			this.butMoveToSelected.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.butMoveToSelected.Click += new System.EventHandler(this.butMoveToSelected_Click);
			// 
			// gridSelectedPatients
			// 
			this.gridSelectedPatients.AllowSortingByColumn = true;
			this.gridSelectedPatients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridSelectedPatients.Location = new System.Drawing.Point(729, 178);
			this.gridSelectedPatients.Name = "gridSelectedPatients";
			this.gridSelectedPatients.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridSelectedPatients.Size = new System.Drawing.Size(462, 335);
			this.gridSelectedPatients.TabIndex = 2;
			this.gridSelectedPatients.Title = "Selected Patients";
			this.gridSelectedPatients.TranslationName = "TableSelectedPatients";
			// 
			// gridAvailablePatients
			// 
			this.gridAvailablePatients.AllowSortingByColumn = true;
			this.gridAvailablePatients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridAvailablePatients.HasAutoWrappedHeaders = true;
			this.gridAvailablePatients.HasMultilineHeaders = true;
			this.gridAvailablePatients.Location = new System.Drawing.Point(10, 178);
			this.gridAvailablePatients.Name = "gridAvailablePatients";
			this.gridAvailablePatients.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridAvailablePatients.Size = new System.Drawing.Size(674, 335);
			this.gridAvailablePatients.TabIndex = 1;
			this.gridAvailablePatients.Title = "Available Patients";
			this.gridAvailablePatients.TranslationName = "TableAvailablePatients";
			// 
			// tabTemplate
			// 
			this.tabTemplate.BackColor = System.Drawing.SystemColors.Control;
			this.tabTemplate.Controls.Add(this.userControlEmailTemplate1);
			this.tabTemplate.Controls.Add(this.butEditTemplate);
			this.tabTemplate.Controls.Add(this.butImport);
			this.tabTemplate.Controls.Add(this.butNewTemplate);
			this.tabTemplate.Controls.Add(this.butCopy);
			this.tabTemplate.Controls.Add(this.butDeleteTemplate);
			this.tabTemplate.Controls.Add(this.gridTemplates);
			this.tabTemplate.Location = new System.Drawing.Point(4, 22);
			this.tabTemplate.Name = "tabTemplate";
			this.tabTemplate.Padding = new System.Windows.Forms.Padding(3);
			this.tabTemplate.Size = new System.Drawing.Size(1200, 617);
			this.tabTemplate.TabIndex = 1;
			this.tabTemplate.Text = "Templates";
			// 
			// userControlEmailTemplate1
			// 
			this.userControlEmailTemplate1.Location = new System.Drawing.Point(352, 59);
			this.userControlEmailTemplate1.Name = "userControlEmailTemplate1";
			this.userControlEmailTemplate1.Size = new System.Drawing.Size(842, 510);
			this.userControlEmailTemplate1.TabIndex = 98;
			// 
			// butEditTemplate
			// 
			this.butEditTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEditTemplate.Location = new System.Drawing.Point(216, 20);
			this.butEditTemplate.Name = "butEditTemplate";
			this.butEditTemplate.Size = new System.Drawing.Size(101, 26);
			this.butEditTemplate.TabIndex = 22;
			this.butEditTemplate.Text = "Edit Template";
			this.butEditTemplate.Click += new System.EventHandler(this.butEditTemplate_Click);
			// 
			// butImport
			// 
			this.butImport.Location = new System.Drawing.Point(135, 21);
			this.butImport.Name = "butImport";
			this.butImport.Size = new System.Drawing.Size(75, 25);
			this.butImport.TabIndex = 96;
			this.butImport.Text = "Import";
			this.butImport.Click += new System.EventHandler(this.butImport_Click);
			// 
			// butNewTemplate
			// 
			this.butNewTemplate.Image = global::Imedisoft.Properties.Resources.Add;
			this.butNewTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNewTemplate.Location = new System.Drawing.Point(15, 21);
			this.butNewTemplate.Name = "butNewTemplate";
			this.butNewTemplate.Size = new System.Drawing.Size(114, 25);
			this.butNewTemplate.TabIndex = 94;
			this.butNewTemplate.Text = "New Template";
			this.butNewTemplate.Click += new System.EventHandler(this.butNewTemplate_Click);
			// 
			// butCopy
			// 
			this.butCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butCopy.Location = new System.Drawing.Point(267, 575);
			this.butCopy.Name = "butCopy";
			this.butCopy.Size = new System.Drawing.Size(75, 25);
			this.butCopy.TabIndex = 22;
			this.butCopy.Text = "Copy";
			this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
			// 
			// butDeleteTemplate
			// 
			this.butDeleteTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDeleteTemplate.Image = global::Imedisoft.Properties.Resources.deleteX;
			this.butDeleteTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDeleteTemplate.Location = new System.Drawing.Point(14, 575);
			this.butDeleteTemplate.Name = "butDeleteTemplate";
			this.butDeleteTemplate.Size = new System.Drawing.Size(75, 26);
			this.butDeleteTemplate.TabIndex = 7;
			this.butDeleteTemplate.Text = "Delete";
			this.butDeleteTemplate.Click += new System.EventHandler(this.butDeleteTemplate_Click);
			// 
			// gridTemplates
			// 
			this.gridTemplates.AllowSortingByColumn = true;
			this.gridTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridTemplates.Location = new System.Drawing.Point(15, 59);
			this.gridTemplates.Name = "gridTemplates";
			this.gridTemplates.Size = new System.Drawing.Size(327, 510);
			this.gridTemplates.TabIndex = 3;
			this.gridTemplates.Title = "Saved Templates";
			this.gridTemplates.TranslationName = "TableSavedTemplates";
			this.gridTemplates.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTemplates_CellDoubleClick);
			this.gridTemplates.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTemplates_CellClick);
			// 
			// tabAnalytics
			// 
			this.tabAnalytics.BackColor = System.Drawing.SystemColors.Control;
			this.tabAnalytics.Controls.Add(this.comboClinicAnalytics);
			this.tabAnalytics.Controls.Add(this.butRefreshAnalytics);
			this.tabAnalytics.Controls.Add(this.gridAnalytics);
			this.tabAnalytics.Controls.Add(this.dateRangeAnalytics);
			this.tabAnalytics.Location = new System.Drawing.Point(4, 22);
			this.tabAnalytics.Name = "tabAnalytics";
			this.tabAnalytics.Padding = new System.Windows.Forms.Padding(3);
			this.tabAnalytics.Size = new System.Drawing.Size(1200, 617);
			this.tabAnalytics.TabIndex = 2;
			this.tabAnalytics.Text = "Analytics";
			// 
			// comboClinicAnalytics
			// 
			this.comboClinicAnalytics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboClinicAnalytics.IncludeAll = true;
			this.comboClinicAnalytics.Location = new System.Drawing.Point(838, 19);
			this.comboClinicAnalytics.Name = "comboClinicAnalytics";
			this.comboClinicAnalytics.Size = new System.Drawing.Size(200, 21);
			this.comboClinicAnalytics.TabIndex = 3;
			// 
			// butRefreshAnalytics
			// 
			this.butRefreshAnalytics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefreshAnalytics.Location = new System.Drawing.Point(1095, 16);
			this.butRefreshAnalytics.Name = "butRefreshAnalytics";
			this.butRefreshAnalytics.Size = new System.Drawing.Size(87, 24);
			this.butRefreshAnalytics.TabIndex = 2;
			this.butRefreshAnalytics.Text = "Refresh";
			this.butRefreshAnalytics.UseVisualStyleBackColor = true;
			this.butRefreshAnalytics.Click += new System.EventHandler(this.butRefreshAnalytics_Click);
			// 
			// gridAnalytics
			// 
			this.gridAnalytics.AllowSelection = false;
			this.gridAnalytics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAnalytics.Location = new System.Drawing.Point(16, 51);
			this.gridAnalytics.Name = "gridAnalytics";
			this.gridAnalytics.Size = new System.Drawing.Size(1166, 549);
			this.gridAnalytics.TabIndex = 1;
			this.gridAnalytics.Title = "Analytics";
			this.gridAnalytics.TranslationName = "GridAnalytics";
			// 
			// dateRangeAnalytics
			// 
			this.dateRangeAnalytics.BackColor = System.Drawing.Color.Transparent;
			this.dateRangeAnalytics.DefaultDateTimeFrom = new System.DateTime(((long)(0)));
			this.dateRangeAnalytics.DefaultDateTimeTo = new System.DateTime(((long)(0)));
			this.dateRangeAnalytics.Location = new System.Drawing.Point(16, 16);
			this.dateRangeAnalytics.MinimumSize = new System.Drawing.Size(453, 22);
			this.dateRangeAnalytics.Name = "dateRangeAnalytics";
			this.dateRangeAnalytics.Size = new System.Drawing.Size(453, 24);
			this.dateRangeAnalytics.TabIndex = 0;
			// 
			// butSendEmails
			// 
			this.butSendEmails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSendEmails.Location = new System.Drawing.Point(1007, 728);
			this.butSendEmails.Name = "butSendEmails";
			this.butSendEmails.Size = new System.Drawing.Size(113, 24);
			this.butSendEmails.TabIndex = 130;
			this.butSendEmails.Text = "Send Emails";
			this.butSendEmails.Click += new System.EventHandler(this.butSendEmails_Click);
			// 
			// labelPleaseActivate
			// 
			this.labelPleaseActivate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelPleaseActivate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPleaseActivate.Location = new System.Drawing.Point(795, 48);
			this.labelPleaseActivate.Name = "labelPleaseActivate";
			this.labelPleaseActivate.Size = new System.Drawing.Size(421, 16);
			this.labelPleaseActivate.TabIndex = 131;
			this.labelPleaseActivate.Text = "Please activate Mass Email in the eServices Setup Window";
			this.labelPleaseActivate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelPleaseActivate.Visible = false;
			// 
			// FormMassEmail
			// 
			this.ClientSize = new System.Drawing.Size(1229, 774);
			this.Controls.Add(this.labelPleaseActivate);
			this.Controls.Add(this.butSendEmails);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.labelNumberPats);
			this.Controls.Add(this.labelPatsSelected);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(990, 713);
			this.Name = "FormMassEmail";
			this.Text = "Send Mass Email";
			this.Load += new System.EventHandler(this.FormMassEmail_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPatients.ResumeLayout(false);
			this.groupBoxFilters.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBoxRecipients.ResumeLayout(false);
			this.groupBoxRecipients.PerformLayout();
			this.tabTemplate.ResumeLayout(false);
			this.tabAnalytics.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
    private UI.Button butClose;
		private System.Windows.Forms.Label labelPatsSelected;
		private System.Windows.Forms.Label labelNumberPats;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPatients;
		private UI.ODGrid gridSelectedPatients;
		private UI.ODGrid gridAvailablePatients;
		private UI.Button butSendEmails;
		private UI.Button butMoveToSelected;
		private UI.Button butSelectAllAvailable;
		private UI.Button butMoveToAvailable;
		private UI.Button butSelectAllSelected;
		private System.Windows.Forms.TabPage tabTemplate;
		private UI.ODGrid gridTemplates;
		private UI.Button butDeleteTemplate;
		private UI.Button butCopy;
		private System.Windows.Forms.TabPage tabAnalytics;
		private UI.ODGrid gridAnalytics;
		private UI.ODDateRangePicker dateRangeAnalytics;
		private UI.Button butRefreshAnalytics;
		private UI.ComboBoxClinicPicker comboClinicAnalytics;
		private System.Windows.Forms.GroupBox groupBoxFilters;
		private UI.Button butRefreshPatientFilters;
		private System.Windows.Forms.CheckBox checkHiddenFutureAppt;
		private System.Windows.Forms.Label labelPatStatus;
		private System.Windows.Forms.Label labelContact;
		private System.Windows.Forms.GroupBox groupBoxRecipients;
		private System.Windows.Forms.Label lableDays;
		private ValidNum textNumDays;
		private System.Windows.Forms.CheckBox checkExcludeWithin;
		private System.Windows.Forms.ListBox listBoxPatStatus;
		private System.Windows.Forms.ListBox listBoxContactMethod;
		private System.Windows.Forms.Label label1;
		private UI.Button butNewTemplate;
		private System.Windows.Forms.Label labelRefreshNeeded;
		private UI.Button butEditTemplate;
		private UI.Button butImport;
		private UI.ComboBoxClinicPicker comboClinicPatient;
		private System.Windows.Forms.Label labelPleaseActivate;
		private System.Windows.Forms.GroupBox groupBox1;
		private ValidNum textAgeTo;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private ValidNum textAgeFrom;
		private UserControlEmailTemplate userControlEmailTemplate1;
		private System.Windows.Forms.CheckBox checkHideSeenSince;
		private System.Windows.Forms.CheckBox checkHideNotSeenSince;
		private System.Windows.Forms.Label labelPatBillingType;
		private System.Windows.Forms.ListBox listBoxPatBillingType;
		private UI.ODDatePicker datePickerNotSeenSince;
		private UI.ODDatePicker datePickerSeenSince;
	}
}