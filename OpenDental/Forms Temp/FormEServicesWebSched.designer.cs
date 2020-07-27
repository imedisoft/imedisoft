namespace OpenDental{
	partial class FormEServicesWebSched {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEServicesWebSched));
			this.label37 = new System.Windows.Forms.Label();
			this.menuWebSchedVerifyTextTemplate = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.insertReplacementsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.butClose = new OpenDental.UI.Button();
			this.linkLabelAboutWebSched = new System.Windows.Forms.LinkLabel();
			this.labelWebSchedDesc = new System.Windows.Forms.Label();
			this.tabControlWebSched = new System.Windows.Forms.TabControl();
			this.tabWebSchedRecalls = new System.Windows.Forms.TabPage();
			this.groupWebSchedProvRule = new System.Windows.Forms.GroupBox();
			this.butProvRulePickClinic = new OpenDental.UI.Button();
			this.checkUseDefaultProvRule = new System.Windows.Forms.CheckBox();
			this.comboClinicProvRule = new OpenDental.UI.ComboBoxClinicPicker();
			this.listBoxWebSchedProviderPref = new System.Windows.Forms.ListBox();
			this.label21 = new System.Windows.Forms.Label();
			this.checkWSRDoubleBooking = new System.Windows.Forms.CheckBox();
			this.comboWSRConfirmStatus = new OpenDental.UI.ComboBoxPlus();
			this.label36 = new System.Windows.Forms.Label();
			this.label43 = new System.Windows.Forms.Label();
			this.label44 = new System.Windows.Forms.Label();
			this.textWebSchedRecallApptSearchDays = new OpenDental.ValidNumber();
			this.checkRecallAllowProvSelection = new System.Windows.Forms.CheckBox();
			this.groupWebSchedText = new System.Windows.Forms.GroupBox();
			this.labelWebSchedPerBatch = new System.Windows.Forms.Label();
			this.textWebSchedPerBatch = new OpenDental.ValidNumber();
			this.radioDoNotSendText = new System.Windows.Forms.RadioButton();
			this.radioSendText = new System.Windows.Forms.RadioButton();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.listboxWebSchedRecallIgnoreBlockoutTypes = new System.Windows.Forms.ListBox();
			this.butWebSchedRecallBlockouts = new OpenDental.UI.Button();
			this.groupBoxWebSchedAutomation = new System.Windows.Forms.GroupBox();
			this.radioSendToEmailNoPreferred = new System.Windows.Forms.RadioButton();
			this.radioDoNotSend = new System.Windows.Forms.RadioButton();
			this.radioSendToEmailOnlyPreferred = new System.Windows.Forms.RadioButton();
			this.radioSendToEmail = new System.Windows.Forms.RadioButton();
			this.groupWebSchedPreview = new System.Windows.Forms.GroupBox();
			this.butWebSchedPickClinic = new OpenDental.UI.Button();
			this.butWebSchedPickProv = new OpenDental.UI.Button();
			this.label22 = new System.Windows.Forms.Label();
			this.comboWebSchedProviders = new System.Windows.Forms.ComboBox();
			this.butWebSchedToday = new OpenDental.UI.Button();
			this.gridWebSchedTimeSlots = new OpenDental.UI.ODGrid();
			this.textWebSchedDateStart = new OpenDental.ValidDate();
			this.labelWebSchedClinic = new System.Windows.Forms.Label();
			this.labelWebSchedRecallTypes = new System.Windows.Forms.Label();
			this.comboWebSchedClinic = new System.Windows.Forms.ComboBox();
			this.comboWebSchedRecallTypes = new System.Windows.Forms.ComboBox();
			this.gridWebSchedOperatories = new OpenDental.UI.ODGrid();
			this.label35 = new System.Windows.Forms.Label();
			this.butRecallSchedSetup = new OpenDental.UI.Button();
			this.label31 = new System.Windows.Forms.Label();
			this.gridWebSchedRecallTypes = new OpenDental.UI.ODGrid();
			this.label20 = new System.Windows.Forms.Label();
			this.tabWebSchedNewPatAppts = new System.Windows.Forms.TabPage();
			this.checkWSNPDoubleBooking = new System.Windows.Forms.CheckBox();
			this.label38 = new System.Windows.Forms.Label();
			this.groupBoxWSNPHostedURLs = new System.Windows.Forms.GroupBox();
			this.panelHostedURLs = new System.Windows.Forms.FlowLayoutPanel();
			this.checkNewPatAllowProvSelection = new System.Windows.Forms.CheckBox();
			this.checkWebSchedNewPatForcePhoneFormatting = new System.Windows.Forms.CheckBox();
			this.comboWSNPConfirmStatuses = new OpenDental.UI.ComboBoxPlus();
			this.labelWebSchedNewPatConfirmStatus = new System.Windows.Forms.Label();
			this.groupBox13 = new System.Windows.Forms.GroupBox();
			this.textWebSchedNewPatApptMessage = new System.Windows.Forms.TextBox();
			this.groupBox11 = new System.Windows.Forms.GroupBox();
			this.butWSNPRestrictedToReasonsEdit = new OpenDental.UI.Button();
			this.labelWSNPRestrictedToReasons = new System.Windows.Forms.Label();
			this.listboxWSNPARestrictedToReasons = new System.Windows.Forms.ListBox();
			this.listboxWebSchedNewPatIgnoreBlockoutTypes = new System.Windows.Forms.ListBox();
			this.label33 = new System.Windows.Forms.Label();
			this.butWebSchedNewPatBlockouts = new OpenDental.UI.Button();
			this.gridWebSchedNewPatApptOps = new OpenDental.UI.ODGrid();
			this.label42 = new System.Windows.Forms.Label();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.labelWSNPClinic = new System.Windows.Forms.Label();
			this.labelWSNPAApptType = new System.Windows.Forms.Label();
			this.comboWSNPClinics = new System.Windows.Forms.ComboBox();
			this.comboWSNPADefApptType = new OpenDental.UI.ComboBoxPlus();
			this.labelWSNPTimeSlots = new System.Windows.Forms.Label();
			this.butWebSchedNewPatApptsToday = new OpenDental.UI.Button();
			this.gridWebSchedNewPatApptTimeSlots = new OpenDental.UI.ODGrid();
			this.textWebSchedNewPatApptsDateStart = new OpenDental.ValidDate();
			this.gridWSNPAReasons = new OpenDental.UI.ODGrid();
			this.label41 = new System.Windows.Forms.Label();
			this.label40 = new System.Windows.Forms.Label();
			this.textWebSchedNewPatApptSearchDays = new OpenDental.ValidNumber();
			this.tabWebSchedVerify = new System.Windows.Forms.TabPage();
			this.butRestoreWebSchedVerify = new OpenDental.UI.Button();
			this.label28 = new System.Windows.Forms.Label();
			this.checkUseDefaultsVerify = new System.Windows.Forms.CheckBox();
			this.groupBoxASAP = new System.Windows.Forms.GroupBox();
			this.groupBoxASAPTextTemplate = new System.Windows.Forms.GroupBox();
			this.textASAPTextTemplate = new System.Windows.Forms.TextBox();
			this.groupBoxASAPEmail = new System.Windows.Forms.GroupBox();
			this.butAsapEditEmail = new OpenDental.UI.Button();
			this.browserAsapEmailBody = new System.Windows.Forms.WebBrowser();
			this.textASAPEmailSubj = new System.Windows.Forms.TextBox();
			this.groupBoxRadioASAP = new System.Windows.Forms.GroupBox();
			this.radioASAPTextAndEmail = new System.Windows.Forms.RadioButton();
			this.radioASAPEmail = new System.Windows.Forms.RadioButton();
			this.radioASAPText = new System.Windows.Forms.RadioButton();
			this.radioASAPNone = new System.Windows.Forms.RadioButton();
			this.groupBoxNewPat = new System.Windows.Forms.GroupBox();
			this.groupBoxNewPatTextTemplate = new System.Windows.Forms.GroupBox();
			this.textNewPatTextTemplate = new System.Windows.Forms.TextBox();
			this.groupBoxNewPatEmail = new System.Windows.Forms.GroupBox();
			this.butNewPatEditEmail = new OpenDental.UI.Button();
			this.browserNewPatEmailBody = new System.Windows.Forms.WebBrowser();
			this.textNewPatEmailSubj = new System.Windows.Forms.TextBox();
			this.groupBoxRadioNewPat = new System.Windows.Forms.GroupBox();
			this.radioNewPatTextAndEmail = new System.Windows.Forms.RadioButton();
			this.radioNewPatEmail = new System.Windows.Forms.RadioButton();
			this.radioNewPatText = new System.Windows.Forms.RadioButton();
			this.radioNewPatNone = new System.Windows.Forms.RadioButton();
			this.groupBoxRecall = new System.Windows.Forms.GroupBox();
			this.groupBoxRecallTextTemplate = new System.Windows.Forms.GroupBox();
			this.textRecallTextTemplate = new System.Windows.Forms.TextBox();
			this.groupBoxRecallEmail = new System.Windows.Forms.GroupBox();
			this.butRecallEditEmail = new OpenDental.UI.Button();
			this.browserRecallEmailBody = new System.Windows.Forms.WebBrowser();
			this.textRecallEmailSubj = new System.Windows.Forms.TextBox();
			this.groupBoxRadioRecall = new System.Windows.Forms.GroupBox();
			this.radioRecallTextAndEmail = new System.Windows.Forms.RadioButton();
			this.radioRecallEmail = new System.Windows.Forms.RadioButton();
			this.radioRecallText = new System.Windows.Forms.RadioButton();
			this.radioRecallNone = new System.Windows.Forms.RadioButton();
			this.comboClinicVerify = new OpenDental.UI.ComboBoxClinicPicker();
			this.butCancel = new OpenDental.UI.Button();
			this.menuWebSchedVerifyTextTemplate.SuspendLayout();
			this.tabControlWebSched.SuspendLayout();
			this.tabWebSchedRecalls.SuspendLayout();
			this.groupWebSchedProvRule.SuspendLayout();
			this.groupWebSchedText.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBoxWebSchedAutomation.SuspendLayout();
			this.groupWebSchedPreview.SuspendLayout();
			this.tabWebSchedNewPatAppts.SuspendLayout();
			this.groupBoxWSNPHostedURLs.SuspendLayout();
			this.groupBox13.SuspendLayout();
			this.groupBox11.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.tabWebSchedVerify.SuspendLayout();
			this.groupBoxASAP.SuspendLayout();
			this.groupBoxASAPTextTemplate.SuspendLayout();
			this.groupBoxASAPEmail.SuspendLayout();
			this.groupBoxRadioASAP.SuspendLayout();
			this.groupBoxNewPat.SuspendLayout();
			this.groupBoxNewPatTextTemplate.SuspendLayout();
			this.groupBoxNewPatEmail.SuspendLayout();
			this.groupBoxRadioNewPat.SuspendLayout();
			this.groupBoxRecall.SuspendLayout();
			this.groupBoxRecallTextTemplate.SuspendLayout();
			this.groupBoxRecallEmail.SuspendLayout();
			this.groupBoxRadioRecall.SuspendLayout();
			this.SuspendLayout();
			// 
			// label37
			// 
			this.label37.Location = new System.Drawing.Point(0, 0);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(100, 23);
			this.label37.TabIndex = 0;
			// 
			// menuWebSchedVerifyTextTemplate
			// 
			this.menuWebSchedVerifyTextTemplate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertReplacementsToolStripMenuItem,
            this.undoToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.selectAllToolStripMenuItem});
			this.menuWebSchedVerifyTextTemplate.Name = "menuASAPEmailBody";
			this.menuWebSchedVerifyTextTemplate.Size = new System.Drawing.Size(137, 136);
			this.menuWebSchedVerifyTextTemplate.Text = "Insert Replacements";
			// 
			// insertReplacementsToolStripMenuItem
			// 
			this.insertReplacementsToolStripMenuItem.Name = "insertReplacementsToolStripMenuItem";
			this.insertReplacementsToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.insertReplacementsToolStripMenuItem.Text = "Insert Fields";
			this.insertReplacementsToolStripMenuItem.Click += new System.EventHandler(this.WebSchedVerify_ContextMenuReplacementsClick);
			// 
			// undoToolStripMenuItem
			// 
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			this.undoToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.undoToolStripMenuItem.Text = "Undo";
			this.undoToolStripMenuItem.Click += new System.EventHandler(this.WebSchedVerify_ContextMenuUndoClick);
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.cutToolStripMenuItem.Text = "Cut";
			this.cutToolStripMenuItem.Click += new System.EventHandler(this.WebSchedVerify_ContextMenuCutClick);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.WebSchedVerify_ContextMenuCopyClick);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.pasteToolStripMenuItem.Text = "Paste";
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.WebSchedVerify_ContextMenuPasteClick);
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.selectAllToolStripMenuItem.Text = "Select All";
			this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.WebSchedVerify_ContextMenuSelectAllClick);
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(1003, 627);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 23);
			this.butClose.TabIndex = 500;
			this.butClose.Text = "OK";
			this.butClose.UseVisualStyleBackColor = true;
			this.butClose.Click += new System.EventHandler(this.butOK_Click);
			// 
			// linkLabelAboutWebSched
			// 
			this.linkLabelAboutWebSched.Location = new System.Drawing.Point(871, 9);
			this.linkLabelAboutWebSched.Name = "linkLabelAboutWebSched";
			this.linkLabelAboutWebSched.Size = new System.Drawing.Size(31, 28);
			this.linkLabelAboutWebSched.TabIndex = 303;
			this.linkLabelAboutWebSched.TabStop = true;
			this.linkLabelAboutWebSched.Text = "help";
			this.linkLabelAboutWebSched.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.linkLabelAboutWebSched.Click += new System.EventHandler(this.linkLabelAboutWebSched_Click);
			// 
			// labelWebSchedDesc
			// 
			this.labelWebSchedDesc.Location = new System.Drawing.Point(269, 9);
			this.labelWebSchedDesc.Name = "labelWebSchedDesc";
			this.labelWebSchedDesc.Size = new System.Drawing.Size(602, 28);
			this.labelWebSchedDesc.TabIndex = 52;
			this.labelWebSchedDesc.Text = "Web Sched is a separate service that gives your patients an easy way to schedule " +
    "appointments via the web within seconds.";
			this.labelWebSchedDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tabControlWebSched
			// 
			this.tabControlWebSched.Controls.Add(this.tabWebSchedRecalls);
			this.tabControlWebSched.Controls.Add(this.tabWebSchedNewPatAppts);
			this.tabControlWebSched.Controls.Add(this.tabWebSchedVerify);
			this.tabControlWebSched.Location = new System.Drawing.Point(12, 40);
			this.tabControlWebSched.Name = "tabControlWebSched";
			this.tabControlWebSched.SelectedIndex = 0;
			this.tabControlWebSched.Size = new System.Drawing.Size(1148, 575);
			this.tabControlWebSched.TabIndex = 302;
			// 
			// tabWebSchedRecalls
			// 
			this.tabWebSchedRecalls.Controls.Add(this.groupWebSchedProvRule);
			this.tabWebSchedRecalls.Controls.Add(this.checkWSRDoubleBooking);
			this.tabWebSchedRecalls.Controls.Add(this.comboWSRConfirmStatus);
			this.tabWebSchedRecalls.Controls.Add(this.label36);
			this.tabWebSchedRecalls.Controls.Add(this.label43);
			this.tabWebSchedRecalls.Controls.Add(this.label44);
			this.tabWebSchedRecalls.Controls.Add(this.textWebSchedRecallApptSearchDays);
			this.tabWebSchedRecalls.Controls.Add(this.checkRecallAllowProvSelection);
			this.tabWebSchedRecalls.Controls.Add(this.groupWebSchedText);
			this.tabWebSchedRecalls.Controls.Add(this.groupBox6);
			this.tabWebSchedRecalls.Controls.Add(this.groupBoxWebSchedAutomation);
			this.tabWebSchedRecalls.Controls.Add(this.groupWebSchedPreview);
			this.tabWebSchedRecalls.Controls.Add(this.gridWebSchedOperatories);
			this.tabWebSchedRecalls.Controls.Add(this.label35);
			this.tabWebSchedRecalls.Controls.Add(this.butRecallSchedSetup);
			this.tabWebSchedRecalls.Controls.Add(this.label31);
			this.tabWebSchedRecalls.Controls.Add(this.gridWebSchedRecallTypes);
			this.tabWebSchedRecalls.Controls.Add(this.label20);
			this.tabWebSchedRecalls.Location = new System.Drawing.Point(4, 22);
			this.tabWebSchedRecalls.Name = "tabWebSchedRecalls";
			this.tabWebSchedRecalls.Padding = new System.Windows.Forms.Padding(3);
			this.tabWebSchedRecalls.Size = new System.Drawing.Size(1140, 549);
			this.tabWebSchedRecalls.TabIndex = 0;
			this.tabWebSchedRecalls.Text = "Recalls";
			this.tabWebSchedRecalls.UseVisualStyleBackColor = true;
			// 
			// groupWebSchedProvRule
			// 
			this.groupWebSchedProvRule.Controls.Add(this.butProvRulePickClinic);
			this.groupWebSchedProvRule.Controls.Add(this.checkUseDefaultProvRule);
			this.groupWebSchedProvRule.Controls.Add(this.comboClinicProvRule);
			this.groupWebSchedProvRule.Controls.Add(this.listBoxWebSchedProviderPref);
			this.groupWebSchedProvRule.Controls.Add(this.label21);
			this.groupWebSchedProvRule.Location = new System.Drawing.Point(114, 426);
			this.groupWebSchedProvRule.Name = "groupWebSchedProvRule";
			this.groupWebSchedProvRule.Size = new System.Drawing.Size(439, 103);
			this.groupWebSchedProvRule.TabIndex = 333;
			this.groupWebSchedProvRule.TabStop = false;
			this.groupWebSchedProvRule.Text = "Provider Rule";
			// 
			// butProvRulePickClinic
			// 
			this.butProvRulePickClinic.Location = new System.Drawing.Point(414, 12);
			this.butProvRulePickClinic.Name = "butProvRulePickClinic";
			this.butProvRulePickClinic.Size = new System.Drawing.Size(18, 21);
			this.butProvRulePickClinic.TabIndex = 314;
			this.butProvRulePickClinic.Text = "...";
			this.butProvRulePickClinic.Click += new System.EventHandler(this.butProvRulePickClinic_Click);
			// 
			// checkUseDefaultProvRule
			// 
			this.checkUseDefaultProvRule.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseDefaultProvRule.Location = new System.Drawing.Point(92, 12);
			this.checkUseDefaultProvRule.Name = "checkUseDefaultProvRule";
			this.checkUseDefaultProvRule.Size = new System.Drawing.Size(104, 24);
			this.checkUseDefaultProvRule.TabIndex = 312;
			this.checkUseDefaultProvRule.Text = "Use Defaults";
			this.checkUseDefaultProvRule.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseDefaultProvRule.UseVisualStyleBackColor = true;
			this.checkUseDefaultProvRule.Visible = false;
			this.checkUseDefaultProvRule.Click += new System.EventHandler(this.checkUseDefaultProvRule_Click);
			// 
			// comboClinicProvRule
			// 
			this.comboClinicProvRule.HqDescription = "Defaults";
			this.comboClinicProvRule.IncludeUnassigned = true;
			this.comboClinicProvRule.Location = new System.Drawing.Point(209, 12);
			this.comboClinicProvRule.Name = "comboClinicProvRule";
			this.comboClinicProvRule.Size = new System.Drawing.Size(200, 21);
			this.comboClinicProvRule.TabIndex = 311;
			this.comboClinicProvRule.SelectionChangeCommitted += new System.EventHandler(this.comboClinicProvRule_SelectionChangeCommitted);
			// 
			// listBoxWebSchedProviderPref
			// 
			this.listBoxWebSchedProviderPref.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.listBoxWebSchedProviderPref.FormattingEnabled = true;
			this.listBoxWebSchedProviderPref.Items.AddRange(new object[] {
            "First Available",
            "Primary Provider",
            "Secondary Provider",
            "Last Seen Hygienist"});
			this.listBoxWebSchedProviderPref.Location = new System.Drawing.Point(313, 41);
			this.listBoxWebSchedProviderPref.Name = "listBoxWebSchedProviderPref";
			this.listBoxWebSchedProviderPref.Size = new System.Drawing.Size(120, 56);
			this.listBoxWebSchedProviderPref.TabIndex = 309;
			this.listBoxWebSchedProviderPref.SelectedIndexChanged += new System.EventHandler(this.listBoxWebSchedProviderPref_SelectedIndexChanged);
			// 
			// label21
			// 
			this.label21.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label21.Location = new System.Drawing.Point(11, 43);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(301, 56);
			this.label21.TabIndex = 310;
			this.label21.Text = resources.GetString("label21.Text");
			this.label21.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkWSRDoubleBooking
			// 
			this.checkWSRDoubleBooking.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWSRDoubleBooking.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkWSRDoubleBooking.Location = new System.Drawing.Point(115, 529);
			this.checkWSRDoubleBooking.Name = "checkWSRDoubleBooking";
			this.checkWSRDoubleBooking.Size = new System.Drawing.Size(216, 18);
			this.checkWSRDoubleBooking.TabIndex = 329;
			this.checkWSRDoubleBooking.Text = "Prevent double booking";
			this.checkWSRDoubleBooking.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboWSRConfirmStatus
			// 
			this.comboWSRConfirmStatus.Location = new System.Drawing.Point(820, 8);
			this.comboWSRConfirmStatus.Name = "comboWSRConfirmStatus";
			this.comboWSRConfirmStatus.Size = new System.Drawing.Size(191, 21);
			this.comboWSRConfirmStatus.TabIndex = 328;
			// 
			// label36
			// 
			this.label36.Location = new System.Drawing.Point(593, 10);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(221, 17);
			this.label36.TabIndex = 327;
			this.label36.Text = "Web Sched Recall Confirm Status";
			this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label43
			// 
			this.label43.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label43.Location = new System.Drawing.Point(284, 8);
			this.label43.Name = "label43";
			this.label43.Size = new System.Drawing.Size(252, 17);
			this.label43.TabIndex = 332;
			this.label43.Text = "days.  Empty includes all possible openings.";
			this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label44
			// 
			this.label44.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label44.Location = new System.Drawing.Point(70, 8);
			this.label44.Name = "label44";
			this.label44.Size = new System.Drawing.Size(167, 17);
			this.label44.TabIndex = 330;
			this.label44.Text = "Search for openings after";
			this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWebSchedRecallApptSearchDays
			// 
			this.textWebSchedRecallApptSearchDays.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.textWebSchedRecallApptSearchDays.Location = new System.Drawing.Point(243, 7);
			this.textWebSchedRecallApptSearchDays.MaxVal = 365;
			this.textWebSchedRecallApptSearchDays.MinVal = 0;
			this.textWebSchedRecallApptSearchDays.Name = "textWebSchedRecallApptSearchDays";
			this.textWebSchedRecallApptSearchDays.Size = new System.Drawing.Size(38, 20);
			this.textWebSchedRecallApptSearchDays.TabIndex = 331;
			this.textWebSchedRecallApptSearchDays.Leave += new System.EventHandler(this.textWebSchedRecallApptSearchDays_Leave);
			this.textWebSchedRecallApptSearchDays.Validated += new System.EventHandler(this.textWebSchedRecallApptSearchDays_Validated);
			// 
			// checkRecallAllowProvSelection
			// 
			this.checkRecallAllowProvSelection.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRecallAllowProvSelection.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRecallAllowProvSelection.Location = new System.Drawing.Point(337, 528);
			this.checkRecallAllowProvSelection.Name = "checkRecallAllowProvSelection";
			this.checkRecallAllowProvSelection.Size = new System.Drawing.Size(216, 18);
			this.checkRecallAllowProvSelection.TabIndex = 314;
			this.checkRecallAllowProvSelection.Text = "Allow patients to select provider";
			this.checkRecallAllowProvSelection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupWebSchedText
			// 
			this.groupWebSchedText.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupWebSchedText.Controls.Add(this.labelWebSchedPerBatch);
			this.groupWebSchedText.Controls.Add(this.textWebSchedPerBatch);
			this.groupWebSchedText.Controls.Add(this.radioDoNotSendText);
			this.groupWebSchedText.Controls.Add(this.radioSendText);
			this.groupWebSchedText.Location = new System.Drawing.Point(576, 489);
			this.groupWebSchedText.Name = "groupWebSchedText";
			this.groupWebSchedText.Size = new System.Drawing.Size(484, 55);
			this.groupWebSchedText.TabIndex = 313;
			this.groupWebSchedText.TabStop = false;
			this.groupWebSchedText.Text = "Send Text Messages Automatically To";
			// 
			// labelWebSchedPerBatch
			// 
			this.labelWebSchedPerBatch.Location = new System.Drawing.Point(288, 9);
			this.labelWebSchedPerBatch.Name = "labelWebSchedPerBatch";
			this.labelWebSchedPerBatch.Size = new System.Drawing.Size(136, 40);
			this.labelWebSchedPerBatch.TabIndex = 314;
			this.labelWebSchedPerBatch.Text = "Max number of texts sent every 10 minutes per clinic";
			this.labelWebSchedPerBatch.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textWebSchedPerBatch
			// 
			this.textWebSchedPerBatch.Location = new System.Drawing.Point(426, 28);
			this.textWebSchedPerBatch.MaxVal = 100000000;
			this.textWebSchedPerBatch.MinVal = 1;
			this.textWebSchedPerBatch.Name = "textWebSchedPerBatch";
			this.textWebSchedPerBatch.Size = new System.Drawing.Size(39, 20);
			this.textWebSchedPerBatch.TabIndex = 242;
			// 
			// radioDoNotSendText
			// 
			this.radioDoNotSendText.Location = new System.Drawing.Point(7, 16);
			this.radioDoNotSendText.Name = "radioDoNotSendText";
			this.radioDoNotSendText.Size = new System.Drawing.Size(229, 16);
			this.radioDoNotSendText.TabIndex = 77;
			this.radioDoNotSendText.Text = "Do Not Send";
			this.radioDoNotSendText.UseVisualStyleBackColor = true;
			this.radioDoNotSendText.CheckedChanged += new System.EventHandler(this.WebSchedRecallAutoSendRadioButtons_CheckedChanged);
			// 
			// radioSendText
			// 
			this.radioSendText.Location = new System.Drawing.Point(7, 32);
			this.radioSendText.Name = "radioSendText";
			this.radioSendText.Size = new System.Drawing.Size(278, 18);
			this.radioSendText.TabIndex = 0;
			this.radioSendText.Text = "Patients with wireless phone (unless \'Text OK\' = No)";
			this.radioSendText.UseVisualStyleBackColor = true;
			this.radioSendText.CheckedChanged += new System.EventHandler(this.WebSchedRecallAutoSendRadioButtons_CheckedChanged);
			// 
			// groupBox6
			// 
			this.groupBox6.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox6.Controls.Add(this.listboxWebSchedRecallIgnoreBlockoutTypes);
			this.groupBox6.Controls.Add(this.butWebSchedRecallBlockouts);
			this.groupBox6.Location = new System.Drawing.Point(669, 294);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(339, 103);
			this.groupBox6.TabIndex = 312;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Allowed Blockout Types";
			// 
			// listboxWebSchedRecallIgnoreBlockoutTypes
			// 
			this.listboxWebSchedRecallIgnoreBlockoutTypes.FormattingEnabled = true;
			this.listboxWebSchedRecallIgnoreBlockoutTypes.Location = new System.Drawing.Point(213, 13);
			this.listboxWebSchedRecallIgnoreBlockoutTypes.Name = "listboxWebSchedRecallIgnoreBlockoutTypes";
			this.listboxWebSchedRecallIgnoreBlockoutTypes.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listboxWebSchedRecallIgnoreBlockoutTypes.Size = new System.Drawing.Size(120, 82);
			this.listboxWebSchedRecallIgnoreBlockoutTypes.TabIndex = 197;
			// 
			// butWebSchedRecallBlockouts
			// 
			this.butWebSchedRecallBlockouts.Location = new System.Drawing.Point(144, 72);
			this.butWebSchedRecallBlockouts.Name = "butWebSchedRecallBlockouts";
			this.butWebSchedRecallBlockouts.Size = new System.Drawing.Size(68, 23);
			this.butWebSchedRecallBlockouts.TabIndex = 197;
			this.butWebSchedRecallBlockouts.Text = "Edit";
			this.butWebSchedRecallBlockouts.Click += new System.EventHandler(this.butWebSchedRecallBlockouts_Click);
			// 
			// groupBoxWebSchedAutomation
			// 
			this.groupBoxWebSchedAutomation.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBoxWebSchedAutomation.Controls.Add(this.radioSendToEmailNoPreferred);
			this.groupBoxWebSchedAutomation.Controls.Add(this.radioDoNotSend);
			this.groupBoxWebSchedAutomation.Controls.Add(this.radioSendToEmailOnlyPreferred);
			this.groupBoxWebSchedAutomation.Controls.Add(this.radioSendToEmail);
			this.groupBoxWebSchedAutomation.Location = new System.Drawing.Point(576, 401);
			this.groupBoxWebSchedAutomation.Name = "groupBoxWebSchedAutomation";
			this.groupBoxWebSchedAutomation.Size = new System.Drawing.Size(484, 84);
			this.groupBoxWebSchedAutomation.TabIndex = 73;
			this.groupBoxWebSchedAutomation.TabStop = false;
			this.groupBoxWebSchedAutomation.Text = "Send Email Messages Automatically To";
			// 
			// radioSendToEmailNoPreferred
			// 
			this.radioSendToEmailNoPreferred.Location = new System.Drawing.Point(7, 47);
			this.radioSendToEmailNoPreferred.Name = "radioSendToEmailNoPreferred";
			this.radioSendToEmailNoPreferred.Size = new System.Drawing.Size(438, 18);
			this.radioSendToEmailNoPreferred.TabIndex = 1;
			this.radioSendToEmailNoPreferred.Text = "Patients with email address and no other preferred recall method is selected.";
			this.radioSendToEmailNoPreferred.UseVisualStyleBackColor = true;
			this.radioSendToEmailNoPreferred.CheckedChanged += new System.EventHandler(this.WebSchedRecallAutoSendRadioButtons_CheckedChanged);
			// 
			// radioDoNotSend
			// 
			this.radioDoNotSend.Location = new System.Drawing.Point(7, 16);
			this.radioDoNotSend.Name = "radioDoNotSend";
			this.radioDoNotSend.Size = new System.Drawing.Size(438, 16);
			this.radioDoNotSend.TabIndex = 77;
			this.radioDoNotSend.Text = "Do Not Send";
			this.radioDoNotSend.UseVisualStyleBackColor = true;
			// 
			// radioSendToEmailOnlyPreferred
			// 
			this.radioSendToEmailOnlyPreferred.Location = new System.Drawing.Point(7, 63);
			this.radioSendToEmailOnlyPreferred.Name = "radioSendToEmailOnlyPreferred";
			this.radioSendToEmailOnlyPreferred.Size = new System.Drawing.Size(438, 18);
			this.radioSendToEmailOnlyPreferred.TabIndex = 74;
			this.radioSendToEmailOnlyPreferred.Text = "Patients with email address and email is selected as their preferred recall metho" +
    "d.";
			this.radioSendToEmailOnlyPreferred.UseVisualStyleBackColor = true;
			this.radioSendToEmailOnlyPreferred.CheckedChanged += new System.EventHandler(this.WebSchedRecallAutoSendRadioButtons_CheckedChanged);
			// 
			// radioSendToEmail
			// 
			this.radioSendToEmail.Location = new System.Drawing.Point(7, 32);
			this.radioSendToEmail.Name = "radioSendToEmail";
			this.radioSendToEmail.Size = new System.Drawing.Size(438, 16);
			this.radioSendToEmail.TabIndex = 0;
			this.radioSendToEmail.Text = "Patients with email address";
			this.radioSendToEmail.UseVisualStyleBackColor = true;
			this.radioSendToEmail.CheckedChanged += new System.EventHandler(this.WebSchedRecallAutoSendRadioButtons_CheckedChanged);
			// 
			// groupWebSchedPreview
			// 
			this.groupWebSchedPreview.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupWebSchedPreview.Controls.Add(this.butWebSchedPickClinic);
			this.groupWebSchedPreview.Controls.Add(this.butWebSchedPickProv);
			this.groupWebSchedPreview.Controls.Add(this.label22);
			this.groupWebSchedPreview.Controls.Add(this.comboWebSchedProviders);
			this.groupWebSchedPreview.Controls.Add(this.butWebSchedToday);
			this.groupWebSchedPreview.Controls.Add(this.gridWebSchedTimeSlots);
			this.groupWebSchedPreview.Controls.Add(this.textWebSchedDateStart);
			this.groupWebSchedPreview.Controls.Add(this.labelWebSchedClinic);
			this.groupWebSchedPreview.Controls.Add(this.labelWebSchedRecallTypes);
			this.groupWebSchedPreview.Controls.Add(this.comboWebSchedClinic);
			this.groupWebSchedPreview.Controls.Add(this.comboWebSchedRecallTypes);
			this.groupWebSchedPreview.Location = new System.Drawing.Point(114, 253);
			this.groupWebSchedPreview.Name = "groupWebSchedPreview";
			this.groupWebSchedPreview.Size = new System.Drawing.Size(439, 172);
			this.groupWebSchedPreview.TabIndex = 252;
			this.groupWebSchedPreview.TabStop = false;
			this.groupWebSchedPreview.Text = "Available Times For Patients";
			// 
			// butWebSchedPickClinic
			// 
			this.butWebSchedPickClinic.Location = new System.Drawing.Point(414, 139);
			this.butWebSchedPickClinic.Name = "butWebSchedPickClinic";
			this.butWebSchedPickClinic.Size = new System.Drawing.Size(18, 21);
			this.butWebSchedPickClinic.TabIndex = 313;
			this.butWebSchedPickClinic.Text = "...";
			this.butWebSchedPickClinic.Click += new System.EventHandler(this.butWebSchedPickClinic_Click);
			// 
			// butWebSchedPickProv
			// 
			this.butWebSchedPickProv.Location = new System.Drawing.Point(414, 98);
			this.butWebSchedPickProv.Name = "butWebSchedPickProv";
			this.butWebSchedPickProv.Size = new System.Drawing.Size(18, 21);
			this.butWebSchedPickProv.TabIndex = 312;
			this.butWebSchedPickProv.Text = "...";
			this.butWebSchedPickProv.Click += new System.EventHandler(this.butWebSchedPickProv_Click);
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(200, 81);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(182, 14);
			this.label22.TabIndex = 310;
			this.label22.Text = "Provider";
			this.label22.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboWebSchedProviders
			// 
			this.comboWebSchedProviders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboWebSchedProviders.Location = new System.Drawing.Point(200, 98);
			this.comboWebSchedProviders.MaxDropDownItems = 30;
			this.comboWebSchedProviders.Name = "comboWebSchedProviders";
			this.comboWebSchedProviders.Size = new System.Drawing.Size(209, 21);
			this.comboWebSchedProviders.TabIndex = 311;
			this.comboWebSchedProviders.SelectionChangeCommitted += new System.EventHandler(this.comboWebSchedProviders_SelectionChangeCommitted);
			// 
			// butWebSchedToday
			// 
			this.butWebSchedToday.Location = new System.Drawing.Point(334, 16);
			this.butWebSchedToday.Name = "butWebSchedToday";
			this.butWebSchedToday.Size = new System.Drawing.Size(75, 21);
			this.butWebSchedToday.TabIndex = 309;
			this.butWebSchedToday.Text = "Today";
			this.butWebSchedToday.Click += new System.EventHandler(this.butWebSchedToday_Click);
			// 
			// gridWebSchedTimeSlots
			// 
			this.gridWebSchedTimeSlots.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridWebSchedTimeSlots.Location = new System.Drawing.Point(18, 16);
			this.gridWebSchedTimeSlots.Name = "gridWebSchedTimeSlots";
			this.gridWebSchedTimeSlots.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridWebSchedTimeSlots.Size = new System.Drawing.Size(174, 148);
			this.gridWebSchedTimeSlots.TabIndex = 302;
			this.gridWebSchedTimeSlots.Title = "Time Slots";
			this.gridWebSchedTimeSlots.TranslationName = "FormEServicesSetup";
			this.gridWebSchedTimeSlots.WrapText = false;
			// 
			// textWebSchedDateStart
			// 
			this.textWebSchedDateStart.Location = new System.Drawing.Point(203, 16);
			this.textWebSchedDateStart.Name = "textWebSchedDateStart";
			this.textWebSchedDateStart.Size = new System.Drawing.Size(90, 20);
			this.textWebSchedDateStart.TabIndex = 303;
			this.textWebSchedDateStart.Text = "07/08/2015";
			this.textWebSchedDateStart.TextChanged += new System.EventHandler(this.textWebSchedDateStart_TextChanged);
			// 
			// labelWebSchedClinic
			// 
			this.labelWebSchedClinic.Location = new System.Drawing.Point(200, 122);
			this.labelWebSchedClinic.Name = "labelWebSchedClinic";
			this.labelWebSchedClinic.Size = new System.Drawing.Size(182, 14);
			this.labelWebSchedClinic.TabIndex = 264;
			this.labelWebSchedClinic.Text = "Clinic";
			this.labelWebSchedClinic.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelWebSchedRecallTypes
			// 
			this.labelWebSchedRecallTypes.Location = new System.Drawing.Point(200, 40);
			this.labelWebSchedRecallTypes.Name = "labelWebSchedRecallTypes";
			this.labelWebSchedRecallTypes.Size = new System.Drawing.Size(182, 14);
			this.labelWebSchedRecallTypes.TabIndex = 254;
			this.labelWebSchedRecallTypes.Text = "Recall Type";
			this.labelWebSchedRecallTypes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboWebSchedClinic
			// 
			this.comboWebSchedClinic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboWebSchedClinic.Location = new System.Drawing.Point(200, 139);
			this.comboWebSchedClinic.MaxDropDownItems = 30;
			this.comboWebSchedClinic.Name = "comboWebSchedClinic";
			this.comboWebSchedClinic.Size = new System.Drawing.Size(209, 21);
			this.comboWebSchedClinic.TabIndex = 305;
			this.comboWebSchedClinic.SelectionChangeCommitted += new System.EventHandler(this.comboWebSchedClinic_SelectionChangeCommitted);
			// 
			// comboWebSchedRecallTypes
			// 
			this.comboWebSchedRecallTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboWebSchedRecallTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboWebSchedRecallTypes.Location = new System.Drawing.Point(200, 57);
			this.comboWebSchedRecallTypes.MaxDropDownItems = 30;
			this.comboWebSchedRecallTypes.Name = "comboWebSchedRecallTypes";
			this.comboWebSchedRecallTypes.Size = new System.Drawing.Size(209, 21);
			this.comboWebSchedRecallTypes.TabIndex = 304;
			this.comboWebSchedRecallTypes.SelectionChangeCommitted += new System.EventHandler(this.comboWebSchedRecallTypes_SelectionChangeCommitted);
			// 
			// gridWebSchedOperatories
			// 
			this.gridWebSchedOperatories.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.gridWebSchedOperatories.Location = new System.Drawing.Point(114, 48);
			this.gridWebSchedOperatories.Name = "gridWebSchedOperatories";
			this.gridWebSchedOperatories.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridWebSchedOperatories.Size = new System.Drawing.Size(532, 202);
			this.gridWebSchedOperatories.TabIndex = 307;
			this.gridWebSchedOperatories.Title = "Operatories Considered";
			this.gridWebSchedOperatories.TranslationName = "FormEServicesSetup";
			this.gridWebSchedOperatories.WrapText = false;
			this.gridWebSchedOperatories.DoubleClick += new System.EventHandler(this.gridWebSchedOperatories_DoubleClick);
			// 
			// label35
			// 
			this.label35.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label35.Location = new System.Drawing.Point(666, 31);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(345, 15);
			this.label35.TabIndex = 254;
			this.label35.Text = "Double click to edit.";
			this.label35.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// butRecallSchedSetup
			// 
			this.butRecallSchedSetup.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.butRecallSchedSetup.Location = new System.Drawing.Point(905, 258);
			this.butRecallSchedSetup.Name = "butRecallSchedSetup";
			this.butRecallSchedSetup.Size = new System.Drawing.Size(103, 24);
			this.butRecallSchedSetup.TabIndex = 308;
			this.butRecallSchedSetup.Text = "Recall Setup";
			this.butRecallSchedSetup.Click += new System.EventHandler(this.butWebSchedSetup_Click);
			// 
			// label31
			// 
			this.label31.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label31.Location = new System.Drawing.Point(389, 31);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(257, 15);
			this.label31.TabIndex = 254;
			this.label31.Text = "Double click to edit.";
			this.label31.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// gridWebSchedRecallTypes
			// 
			this.gridWebSchedRecallTypes.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.gridWebSchedRecallTypes.Location = new System.Drawing.Point(669, 48);
			this.gridWebSchedRecallTypes.Name = "gridWebSchedRecallTypes";
			this.gridWebSchedRecallTypes.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridWebSchedRecallTypes.Size = new System.Drawing.Size(342, 202);
			this.gridWebSchedRecallTypes.TabIndex = 307;
			this.gridWebSchedRecallTypes.Title = "Recall Types";
			this.gridWebSchedRecallTypes.TranslationName = "FormEServicesSetup";
			this.gridWebSchedRecallTypes.WrapText = false;
			this.gridWebSchedRecallTypes.DoubleClick += new System.EventHandler(this.gridWebSchedRecallTypes_DoubleClick);
			// 
			// label20
			// 
			this.label20.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label20.Location = new System.Drawing.Point(666, 256);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(233, 28);
			this.label20.TabIndex = 247;
			this.label20.Text = "Customize the notification message that will be sent to the patient.";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabWebSchedNewPatAppts
			// 
			this.tabWebSchedNewPatAppts.Controls.Add(this.checkWSNPDoubleBooking);
			this.tabWebSchedNewPatAppts.Controls.Add(this.label38);
			this.tabWebSchedNewPatAppts.Controls.Add(this.groupBoxWSNPHostedURLs);
			this.tabWebSchedNewPatAppts.Controls.Add(this.checkNewPatAllowProvSelection);
			this.tabWebSchedNewPatAppts.Controls.Add(this.checkWebSchedNewPatForcePhoneFormatting);
			this.tabWebSchedNewPatAppts.Controls.Add(this.comboWSNPConfirmStatuses);
			this.tabWebSchedNewPatAppts.Controls.Add(this.labelWebSchedNewPatConfirmStatus);
			this.tabWebSchedNewPatAppts.Controls.Add(this.groupBox13);
			this.tabWebSchedNewPatAppts.Controls.Add(this.groupBox11);
			this.tabWebSchedNewPatAppts.Controls.Add(this.gridWebSchedNewPatApptOps);
			this.tabWebSchedNewPatAppts.Controls.Add(this.label42);
			this.tabWebSchedNewPatAppts.Controls.Add(this.groupBox7);
			this.tabWebSchedNewPatAppts.Controls.Add(this.gridWSNPAReasons);
			this.tabWebSchedNewPatAppts.Controls.Add(this.label41);
			this.tabWebSchedNewPatAppts.Controls.Add(this.label40);
			this.tabWebSchedNewPatAppts.Controls.Add(this.textWebSchedNewPatApptSearchDays);
			this.tabWebSchedNewPatAppts.Location = new System.Drawing.Point(4, 22);
			this.tabWebSchedNewPatAppts.Name = "tabWebSchedNewPatAppts";
			this.tabWebSchedNewPatAppts.Padding = new System.Windows.Forms.Padding(3);
			this.tabWebSchedNewPatAppts.Size = new System.Drawing.Size(1140, 549);
			this.tabWebSchedNewPatAppts.TabIndex = 1;
			this.tabWebSchedNewPatAppts.Text = "New Patient Appts";
			this.tabWebSchedNewPatAppts.UseVisualStyleBackColor = true;
			// 
			// checkWSNPDoubleBooking
			// 
			this.checkWSNPDoubleBooking.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWSNPDoubleBooking.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkWSNPDoubleBooking.Location = new System.Drawing.Point(61, 503);
			this.checkWSNPDoubleBooking.Name = "checkWSNPDoubleBooking";
			this.checkWSNPDoubleBooking.Size = new System.Drawing.Size(216, 18);
			this.checkWSNPDoubleBooking.TabIndex = 331;
			this.checkWSNPDoubleBooking.Text = "Prevent double booking";
			this.checkWSNPDoubleBooking.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label38
			// 
			this.label38.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label38.Location = new System.Drawing.Point(117, 39);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(245, 15);
			this.label38.TabIndex = 330;
			this.label38.Text = "Double click to edit.";
			this.label38.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// groupBoxWSNPHostedURLs
			// 
			this.groupBoxWSNPHostedURLs.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBoxWSNPHostedURLs.Controls.Add(this.panelHostedURLs);
			this.groupBoxWSNPHostedURLs.Location = new System.Drawing.Point(390, 201);
			this.groupBoxWSNPHostedURLs.Name = "groupBoxWSNPHostedURLs";
			this.groupBoxWSNPHostedURLs.Size = new System.Drawing.Size(738, 204);
			this.groupBoxWSNPHostedURLs.TabIndex = 329;
			this.groupBoxWSNPHostedURLs.TabStop = false;
			this.groupBoxWSNPHostedURLs.Text = "Hosted URLs will be where new patients need to visit in order to create an appoin" +
    "tment. Use Signup to enable.";
			// 
			// panelHostedURLs
			// 
			this.panelHostedURLs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelHostedURLs.AutoScroll = true;
			this.panelHostedURLs.Location = new System.Drawing.Point(0, 13);
			this.panelHostedURLs.Name = "panelHostedURLs";
			this.panelHostedURLs.Size = new System.Drawing.Size(738, 186);
			this.panelHostedURLs.TabIndex = 0;
			// 
			// checkNewPatAllowProvSelection
			// 
			this.checkNewPatAllowProvSelection.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.checkNewPatAllowProvSelection.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkNewPatAllowProvSelection.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNewPatAllowProvSelection.Location = new System.Drawing.Point(61, 479);
			this.checkNewPatAllowProvSelection.Name = "checkNewPatAllowProvSelection";
			this.checkNewPatAllowProvSelection.Size = new System.Drawing.Size(216, 18);
			this.checkNewPatAllowProvSelection.TabIndex = 328;
			this.checkNewPatAllowProvSelection.Text = "Allow patients to select provider";
			this.checkNewPatAllowProvSelection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkWebSchedNewPatForcePhoneFormatting
			// 
			this.checkWebSchedNewPatForcePhoneFormatting.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.checkWebSchedNewPatForcePhoneFormatting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWebSchedNewPatForcePhoneFormatting.Location = new System.Drawing.Point(555, 38);
			this.checkWebSchedNewPatForcePhoneFormatting.Name = "checkWebSchedNewPatForcePhoneFormatting";
			this.checkWebSchedNewPatForcePhoneFormatting.Size = new System.Drawing.Size(237, 19);
			this.checkWebSchedNewPatForcePhoneFormatting.TabIndex = 327;
			this.checkWebSchedNewPatForcePhoneFormatting.Text = "Force US phone number format";
			this.checkWebSchedNewPatForcePhoneFormatting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWebSchedNewPatForcePhoneFormatting.UseVisualStyleBackColor = true;
			this.checkWebSchedNewPatForcePhoneFormatting.Click += new System.EventHandler(this.checkWebSchedNewPatForcePhoneFormatting_Click);
			// 
			// comboWSNPConfirmStatuses
			// 
			this.comboWSNPConfirmStatuses.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.comboWSNPConfirmStatuses.Location = new System.Drawing.Point(774, 11);
			this.comboWSNPConfirmStatuses.Name = "comboWSNPConfirmStatuses";
			this.comboWSNPConfirmStatuses.Size = new System.Drawing.Size(291, 21);
			this.comboWSNPConfirmStatuses.TabIndex = 326;
			// 
			// labelWebSchedNewPatConfirmStatus
			// 
			this.labelWebSchedNewPatConfirmStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.labelWebSchedNewPatConfirmStatus.Location = new System.Drawing.Point(562, 12);
			this.labelWebSchedNewPatConfirmStatus.Name = "labelWebSchedNewPatConfirmStatus";
			this.labelWebSchedNewPatConfirmStatus.Size = new System.Drawing.Size(206, 17);
			this.labelWebSchedNewPatConfirmStatus.TabIndex = 325;
			this.labelWebSchedNewPatConfirmStatus.Text = "Confirm Status";
			this.labelWebSchedNewPatConfirmStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox13
			// 
			this.groupBox13.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox13.Controls.Add(this.textWebSchedNewPatApptMessage);
			this.groupBox13.Location = new System.Drawing.Point(390, 406);
			this.groupBox13.Name = "groupBox13";
			this.groupBox13.Size = new System.Drawing.Size(304, 137);
			this.groupBox13.TabIndex = 324;
			this.groupBox13.TabStop = false;
			this.groupBox13.Text = "Appointment Message";
			// 
			// textWebSchedNewPatApptMessage
			// 
			this.textWebSchedNewPatApptMessage.Location = new System.Drawing.Point(6, 17);
			this.textWebSchedNewPatApptMessage.Multiline = true;
			this.textWebSchedNewPatApptMessage.Name = "textWebSchedNewPatApptMessage";
			this.textWebSchedNewPatApptMessage.Size = new System.Drawing.Size(292, 114);
			this.textWebSchedNewPatApptMessage.TabIndex = 313;
			// 
			// groupBox11
			// 
			this.groupBox11.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox11.Controls.Add(this.butWSNPRestrictedToReasonsEdit);
			this.groupBox11.Controls.Add(this.labelWSNPRestrictedToReasons);
			this.groupBox11.Controls.Add(this.listboxWSNPARestrictedToReasons);
			this.groupBox11.Controls.Add(this.listboxWebSchedNewPatIgnoreBlockoutTypes);
			this.groupBox11.Controls.Add(this.label33);
			this.groupBox11.Controls.Add(this.butWebSchedNewPatBlockouts);
			this.groupBox11.Location = new System.Drawing.Point(700, 406);
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.Size = new System.Drawing.Size(428, 137);
			this.groupBox11.TabIndex = 323;
			this.groupBox11.TabStop = false;
			this.groupBox11.Text = "Allowed Blockout Types";
			// 
			// butWSNPRestrictedToReasonsEdit
			// 
			this.butWSNPRestrictedToReasonsEdit.Location = new System.Drawing.Point(172, 107);
			this.butWSNPRestrictedToReasonsEdit.Name = "butWSNPRestrictedToReasonsEdit";
			this.butWSNPRestrictedToReasonsEdit.Size = new System.Drawing.Size(68, 24);
			this.butWSNPRestrictedToReasonsEdit.TabIndex = 226;
			this.butWSNPRestrictedToReasonsEdit.Text = "Edit";
			this.butWSNPRestrictedToReasonsEdit.UseVisualStyleBackColor = true;
			this.butWSNPRestrictedToReasonsEdit.Click += new System.EventHandler(this.butWSNPRestrictedToReasonsEdit_Click);
			// 
			// labelWSNPRestrictedToReasons
			// 
			this.labelWSNPRestrictedToReasons.Location = new System.Drawing.Point(169, 16);
			this.labelWSNPRestrictedToReasons.Name = "labelWSNPRestrictedToReasons";
			this.labelWSNPRestrictedToReasons.Size = new System.Drawing.Size(253, 20);
			this.labelWSNPRestrictedToReasons.TabIndex = 225;
			this.labelWSNPRestrictedToReasons.Text = "Restricted to Reasons";
			this.labelWSNPRestrictedToReasons.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// listboxWSNPARestrictedToReasons
			// 
			this.listboxWSNPARestrictedToReasons.FormattingEnabled = true;
			this.listboxWSNPARestrictedToReasons.Location = new System.Drawing.Point(172, 37);
			this.listboxWSNPARestrictedToReasons.Name = "listboxWSNPARestrictedToReasons";
			this.listboxWSNPARestrictedToReasons.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listboxWSNPARestrictedToReasons.Size = new System.Drawing.Size(250, 69);
			this.listboxWSNPARestrictedToReasons.TabIndex = 224;
			// 
			// listboxWebSchedNewPatIgnoreBlockoutTypes
			// 
			this.listboxWebSchedNewPatIgnoreBlockoutTypes.FormattingEnabled = true;
			this.listboxWebSchedNewPatIgnoreBlockoutTypes.Location = new System.Drawing.Point(6, 37);
			this.listboxWebSchedNewPatIgnoreBlockoutTypes.Name = "listboxWebSchedNewPatIgnoreBlockoutTypes";
			this.listboxWebSchedNewPatIgnoreBlockoutTypes.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listboxWebSchedNewPatIgnoreBlockoutTypes.Size = new System.Drawing.Size(146, 69);
			this.listboxWebSchedNewPatIgnoreBlockoutTypes.TabIndex = 197;
			// 
			// label33
			// 
			this.label33.Location = new System.Drawing.Point(6, 14);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(149, 20);
			this.label33.TabIndex = 223;
			this.label33.Text = "Generally Allowed";
			this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butWebSchedNewPatBlockouts
			// 
			this.butWebSchedNewPatBlockouts.Location = new System.Drawing.Point(6, 108);
			this.butWebSchedNewPatBlockouts.Name = "butWebSchedNewPatBlockouts";
			this.butWebSchedNewPatBlockouts.Size = new System.Drawing.Size(68, 23);
			this.butWebSchedNewPatBlockouts.TabIndex = 197;
			this.butWebSchedNewPatBlockouts.Text = "Edit";
			this.butWebSchedNewPatBlockouts.Click += new System.EventHandler(this.butWebSchedNewPatBlockouts_Click);
			// 
			// gridWebSchedNewPatApptOps
			// 
			this.gridWebSchedNewPatApptOps.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.gridWebSchedNewPatApptOps.Location = new System.Drawing.Point(390, 57);
			this.gridWebSchedNewPatApptOps.Name = "gridWebSchedNewPatApptOps";
			this.gridWebSchedNewPatApptOps.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridWebSchedNewPatApptOps.Size = new System.Drawing.Size(738, 142);
			this.gridWebSchedNewPatApptOps.TabIndex = 309;
			this.gridWebSchedNewPatApptOps.Title = "Operatories Considered";
			this.gridWebSchedNewPatApptOps.TranslationName = "FormEServicesSetup";
			this.gridWebSchedNewPatApptOps.WrapText = false;
			this.gridWebSchedNewPatApptOps.DoubleClick += new System.EventHandler(this.gridWebSchedNewPatApptOps_DoubleClick);
			// 
			// label42
			// 
			this.label42.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label42.Location = new System.Drawing.Point(820, 39);
			this.label42.Name = "label42";
			this.label42.Size = new System.Drawing.Size(245, 15);
			this.label42.TabIndex = 308;
			this.label42.Text = "Double click to edit.";
			this.label42.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// groupBox7
			// 
			this.groupBox7.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox7.Controls.Add(this.labelWSNPClinic);
			this.groupBox7.Controls.Add(this.labelWSNPAApptType);
			this.groupBox7.Controls.Add(this.comboWSNPClinics);
			this.groupBox7.Controls.Add(this.comboWSNPADefApptType);
			this.groupBox7.Controls.Add(this.labelWSNPTimeSlots);
			this.groupBox7.Controls.Add(this.butWebSchedNewPatApptsToday);
			this.groupBox7.Controls.Add(this.gridWebSchedNewPatApptTimeSlots);
			this.groupBox7.Controls.Add(this.textWebSchedNewPatApptsDateStart);
			this.groupBox7.Location = new System.Drawing.Point(6, 201);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(378, 228);
			this.groupBox7.TabIndex = 304;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Available Times For Patients";
			// 
			// labelWSNPClinic
			// 
			this.labelWSNPClinic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelWSNPClinic.Location = new System.Drawing.Point(195, 77);
			this.labelWSNPClinic.Name = "labelWSNPClinic";
			this.labelWSNPClinic.Size = new System.Drawing.Size(179, 16);
			this.labelWSNPClinic.TabIndex = 324;
			this.labelWSNPClinic.Text = "Clinic";
			this.labelWSNPClinic.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelWSNPAApptType
			// 
			this.labelWSNPAApptType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelWSNPAApptType.Location = new System.Drawing.Point(195, 129);
			this.labelWSNPAApptType.Name = "labelWSNPAApptType";
			this.labelWSNPAApptType.Size = new System.Drawing.Size(179, 17);
			this.labelWSNPAApptType.TabIndex = 320;
			this.labelWSNPAApptType.Text = "Reason";
			// 
			// comboWSNPClinics
			// 
			this.comboWSNPClinics.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboWSNPClinics.Location = new System.Drawing.Point(195, 96);
			this.comboWSNPClinics.MaxDropDownItems = 30;
			this.comboWSNPClinics.Name = "comboWSNPClinics";
			this.comboWSNPClinics.Size = new System.Drawing.Size(177, 21);
			this.comboWSNPClinics.TabIndex = 323;
			this.comboWSNPClinics.SelectionChangeCommitted += new System.EventHandler(this.comboWSNPClinics_SelectionChangeCommitted);
			// 
			// comboWSNPADefApptType
			// 
			this.comboWSNPADefApptType.Location = new System.Drawing.Point(195, 148);
			this.comboWSNPADefApptType.Name = "comboWSNPADefApptType";
			this.comboWSNPADefApptType.Size = new System.Drawing.Size(177, 21);
			this.comboWSNPADefApptType.TabIndex = 319;
			this.comboWSNPADefApptType.SelectionChangeCommitted += new System.EventHandler(this.comboWebSchedNewPatApptsApptType_SelectionChangeCommitted);
			// 
			// labelWSNPTimeSlots
			// 
			this.labelWSNPTimeSlots.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelWSNPTimeSlots.Location = new System.Drawing.Point(12, 196);
			this.labelWSNPTimeSlots.Name = "labelWSNPTimeSlots";
			this.labelWSNPTimeSlots.Size = new System.Drawing.Size(354, 26);
			this.labelWSNPTimeSlots.TabIndex = 318;
			this.labelWSNPTimeSlots.Text = "Select a clinic to view its available time slots.";
			// 
			// butWebSchedNewPatApptsToday
			// 
			this.butWebSchedNewPatApptsToday.Location = new System.Drawing.Point(204, 46);
			this.butWebSchedNewPatApptsToday.Name = "butWebSchedNewPatApptsToday";
			this.butWebSchedNewPatApptsToday.Size = new System.Drawing.Size(79, 21);
			this.butWebSchedNewPatApptsToday.TabIndex = 309;
			this.butWebSchedNewPatApptsToday.Text = "Today";
			this.butWebSchedNewPatApptsToday.Click += new System.EventHandler(this.butWebSchedNewPatApptsToday_Click);
			// 
			// gridWebSchedNewPatApptTimeSlots
			// 
			this.gridWebSchedNewPatApptTimeSlots.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridWebSchedNewPatApptTimeSlots.Location = new System.Drawing.Point(15, 20);
			this.gridWebSchedNewPatApptTimeSlots.Name = "gridWebSchedNewPatApptTimeSlots";
			this.gridWebSchedNewPatApptTimeSlots.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridWebSchedNewPatApptTimeSlots.Size = new System.Drawing.Size(174, 169);
			this.gridWebSchedNewPatApptTimeSlots.TabIndex = 302;
			this.gridWebSchedNewPatApptTimeSlots.Title = "Time Slots";
			this.gridWebSchedNewPatApptTimeSlots.TranslationName = "FormEServicesSetup";
			this.gridWebSchedNewPatApptTimeSlots.WrapText = false;
			// 
			// textWebSchedNewPatApptsDateStart
			// 
			this.textWebSchedNewPatApptsDateStart.Location = new System.Drawing.Point(204, 20);
			this.textWebSchedNewPatApptsDateStart.Name = "textWebSchedNewPatApptsDateStart";
			this.textWebSchedNewPatApptsDateStart.Size = new System.Drawing.Size(79, 20);
			this.textWebSchedNewPatApptsDateStart.TabIndex = 303;
			this.textWebSchedNewPatApptsDateStart.Text = "07/08/2015";
			this.textWebSchedNewPatApptsDateStart.TextChanged += new System.EventHandler(this.textWebSchedNewPatApptsDateStart_TextChanged);
			// 
			// gridWSNPAReasons
			// 
			this.gridWSNPAReasons.AllowSelection = false;
			this.gridWSNPAReasons.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.gridWSNPAReasons.Location = new System.Drawing.Point(6, 57);
			this.gridWSNPAReasons.Name = "gridWSNPAReasons";
			this.gridWSNPAReasons.Size = new System.Drawing.Size(378, 142);
			this.gridWSNPAReasons.TabIndex = 303;
			this.gridWSNPAReasons.Title = "Appointment Types";
			this.gridWSNPAReasons.TranslationName = "FormEServicesSetup";
			this.gridWSNPAReasons.DoubleClick += new System.EventHandler(this.gridWSNPAReasons_DoubleClick);
			// 
			// label41
			// 
			this.label41.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label41.Location = new System.Drawing.Point(270, 12);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(252, 17);
			this.label41.TabIndex = 244;
			this.label41.Text = "days.  Empty includes all possible openings.";
			this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label40
			// 
			this.label40.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label40.Location = new System.Drawing.Point(56, 12);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(167, 17);
			this.label40.TabIndex = 242;
			this.label40.Text = "Search for openings after";
			this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWebSchedNewPatApptSearchDays
			// 
			this.textWebSchedNewPatApptSearchDays.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.textWebSchedNewPatApptSearchDays.Location = new System.Drawing.Point(229, 11);
			this.textWebSchedNewPatApptSearchDays.MaxVal = 365;
			this.textWebSchedNewPatApptSearchDays.MinVal = 0;
			this.textWebSchedNewPatApptSearchDays.Name = "textWebSchedNewPatApptSearchDays";
			this.textWebSchedNewPatApptSearchDays.Size = new System.Drawing.Size(38, 20);
			this.textWebSchedNewPatApptSearchDays.TabIndex = 243;
			this.textWebSchedNewPatApptSearchDays.Leave += new System.EventHandler(this.textWebSchedNewPatApptSearchDays_Leave);
			this.textWebSchedNewPatApptSearchDays.Validated += new System.EventHandler(this.textWebSchedNewPatApptSearchDays_Validated);
			// 
			// tabWebSchedVerify
			// 
			this.tabWebSchedVerify.Controls.Add(this.butRestoreWebSchedVerify);
			this.tabWebSchedVerify.Controls.Add(this.label28);
			this.tabWebSchedVerify.Controls.Add(this.checkUseDefaultsVerify);
			this.tabWebSchedVerify.Controls.Add(this.groupBoxASAP);
			this.tabWebSchedVerify.Controls.Add(this.groupBoxNewPat);
			this.tabWebSchedVerify.Controls.Add(this.groupBoxRecall);
			this.tabWebSchedVerify.Controls.Add(this.comboClinicVerify);
			this.tabWebSchedVerify.Location = new System.Drawing.Point(4, 22);
			this.tabWebSchedVerify.Name = "tabWebSchedVerify";
			this.tabWebSchedVerify.Padding = new System.Windows.Forms.Padding(3);
			this.tabWebSchedVerify.Size = new System.Drawing.Size(1140, 549);
			this.tabWebSchedVerify.TabIndex = 2;
			this.tabWebSchedVerify.Text = "Notify";
			this.tabWebSchedVerify.UseVisualStyleBackColor = true;
			// 
			// butRestoreWebSchedVerify
			// 
			this.butRestoreWebSchedVerify.Location = new System.Drawing.Point(1059, 6);
			this.butRestoreWebSchedVerify.Name = "butRestoreWebSchedVerify";
			this.butRestoreWebSchedVerify.Size = new System.Drawing.Size(75, 23);
			this.butRestoreWebSchedVerify.TabIndex = 304;
			this.butRestoreWebSchedVerify.Text = "Undo All";
			this.butRestoreWebSchedVerify.UseVisualStyleBackColor = true;
			this.butRestoreWebSchedVerify.Click += new System.EventHandler(this.WebSchedVerify_butUndoClick);
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(646, 3);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(407, 28);
			this.label28.TabIndex = 269;
			this.label28.Text = "Right-click on any text box to choose from a list of valid template fields.";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkUseDefaultsVerify
			// 
			this.checkUseDefaultsVerify.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUseDefaultsVerify.Location = new System.Drawing.Point(384, 10);
			this.checkUseDefaultsVerify.Name = "checkUseDefaultsVerify";
			this.checkUseDefaultsVerify.Size = new System.Drawing.Size(105, 19);
			this.checkUseDefaultsVerify.TabIndex = 267;
			this.checkUseDefaultsVerify.Text = "Use Defaults";
			this.checkUseDefaultsVerify.CheckedChanged += new System.EventHandler(this.WebSchedVerify_CheckUseDefaultsChanged);
			// 
			// groupBoxASAP
			// 
			this.groupBoxASAP.Controls.Add(this.groupBoxASAPTextTemplate);
			this.groupBoxASAP.Controls.Add(this.groupBoxASAPEmail);
			this.groupBoxASAP.Controls.Add(this.groupBoxRadioASAP);
			this.groupBoxASAP.Location = new System.Drawing.Point(762, 32);
			this.groupBoxASAP.Name = "groupBoxASAP";
			this.groupBoxASAP.Size = new System.Drawing.Size(372, 497);
			this.groupBoxASAP.TabIndex = 2;
			this.groupBoxASAP.TabStop = false;
			this.groupBoxASAP.Text = "ASAP";
			// 
			// groupBoxASAPTextTemplate
			// 
			this.groupBoxASAPTextTemplate.Controls.Add(this.textASAPTextTemplate);
			this.groupBoxASAPTextTemplate.Location = new System.Drawing.Point(6, 139);
			this.groupBoxASAPTextTemplate.Name = "groupBoxASAPTextTemplate";
			this.groupBoxASAPTextTemplate.Size = new System.Drawing.Size(360, 120);
			this.groupBoxASAPTextTemplate.TabIndex = 121;
			this.groupBoxASAPTextTemplate.TabStop = false;
			this.groupBoxASAPTextTemplate.Text = "Text Message";
			// 
			// textASAPTextTemplate
			// 
			this.textASAPTextTemplate.ContextMenuStrip = this.menuWebSchedVerifyTextTemplate;
			this.textASAPTextTemplate.Location = new System.Drawing.Point(6, 19);
			this.textASAPTextTemplate.Multiline = true;
			this.textASAPTextTemplate.Name = "textASAPTextTemplate";
			this.textASAPTextTemplate.Size = new System.Drawing.Size(348, 95);
			this.textASAPTextTemplate.TabIndex = 314;
			this.textASAPTextTemplate.Tag = OpenDentBusiness.PrefName.WebSchedVerifyASAPText;
			this.textASAPTextTemplate.Leave += new System.EventHandler(this.WebSchedVerify_TextLeave);
			// 
			// groupBoxASAPEmail
			// 
			this.groupBoxASAPEmail.Controls.Add(this.butAsapEditEmail);
			this.groupBoxASAPEmail.Controls.Add(this.browserAsapEmailBody);
			this.groupBoxASAPEmail.Controls.Add(this.textASAPEmailSubj);
			this.groupBoxASAPEmail.Location = new System.Drawing.Point(6, 259);
			this.groupBoxASAPEmail.Name = "groupBoxASAPEmail";
			this.groupBoxASAPEmail.Size = new System.Drawing.Size(360, 232);
			this.groupBoxASAPEmail.TabIndex = 122;
			this.groupBoxASAPEmail.TabStop = false;
			this.groupBoxASAPEmail.Text = "E-mail Subject and Body";
			// 
			// butAsapEditEmail
			// 
			this.butAsapEditEmail.Location = new System.Drawing.Point(284, 209);
			this.butAsapEditEmail.Name = "butAsapEditEmail";
			this.butAsapEditEmail.Size = new System.Drawing.Size(70, 20);
			this.butAsapEditEmail.TabIndex = 318;
			this.butAsapEditEmail.Text = "Edit";
			this.butAsapEditEmail.UseVisualStyleBackColor = true;
			this.butAsapEditEmail.Click += new System.EventHandler(this.WebSchedVerify_AsapEditEmailClick);
			// 
			// browserAsapEmailBody
			// 
			this.browserAsapEmailBody.AllowWebBrowserDrop = false;
			this.browserAsapEmailBody.Location = new System.Drawing.Point(7, 45);
			this.browserAsapEmailBody.MinimumSize = new System.Drawing.Size(20, 20);
			this.browserAsapEmailBody.Name = "browserAsapEmailBody";
			this.browserAsapEmailBody.Size = new System.Drawing.Size(347, 158);
			this.browserAsapEmailBody.TabIndex = 317;
			this.browserAsapEmailBody.WebBrowserShortcutsEnabled = false;
			// 
			// textASAPEmailSubj
			// 
			this.textASAPEmailSubj.ContextMenuStrip = this.menuWebSchedVerifyTextTemplate;
			this.textASAPEmailSubj.Location = new System.Drawing.Point(6, 19);
			this.textASAPEmailSubj.Multiline = true;
			this.textASAPEmailSubj.Name = "textASAPEmailSubj";
			this.textASAPEmailSubj.Size = new System.Drawing.Size(348, 20);
			this.textASAPEmailSubj.TabIndex = 314;
			this.textASAPEmailSubj.Tag = OpenDentBusiness.PrefName.WebSchedVerifyASAPEmailSubj;
			this.textASAPEmailSubj.Leave += new System.EventHandler(this.WebSchedVerify_TextLeave);
			// 
			// groupBoxRadioASAP
			// 
			this.groupBoxRadioASAP.Controls.Add(this.radioASAPTextAndEmail);
			this.groupBoxRadioASAP.Controls.Add(this.radioASAPEmail);
			this.groupBoxRadioASAP.Controls.Add(this.radioASAPText);
			this.groupBoxRadioASAP.Controls.Add(this.radioASAPNone);
			this.groupBoxRadioASAP.Location = new System.Drawing.Point(6, 18);
			this.groupBoxRadioASAP.Name = "groupBoxRadioASAP";
			this.groupBoxRadioASAP.Size = new System.Drawing.Size(360, 115);
			this.groupBoxRadioASAP.TabIndex = 1;
			this.groupBoxRadioASAP.TabStop = false;
			this.groupBoxRadioASAP.Tag = OpenDentBusiness.PrefName.WebSchedVerifyASAPType;
			this.groupBoxRadioASAP.Text = "Communication Method";
			// 
			// radioASAPTextAndEmail
			// 
			this.radioASAPTextAndEmail.Location = new System.Drawing.Point(7, 89);
			this.radioASAPTextAndEmail.Name = "radioASAPTextAndEmail";
			this.radioASAPTextAndEmail.Size = new System.Drawing.Size(175, 17);
			this.radioASAPTextAndEmail.TabIndex = 3;
			this.radioASAPTextAndEmail.TabStop = true;
			this.radioASAPTextAndEmail.Tag = OpenDentBusiness.WebSchedVerifyType.TextAndEmail;
			this.radioASAPTextAndEmail.Text = "Text and E-mail";
			this.radioASAPTextAndEmail.UseVisualStyleBackColor = true;
			this.radioASAPTextAndEmail.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioASAPEmail
			// 
			this.radioASAPEmail.Location = new System.Drawing.Point(7, 66);
			this.radioASAPEmail.Name = "radioASAPEmail";
			this.radioASAPEmail.Size = new System.Drawing.Size(175, 17);
			this.radioASAPEmail.TabIndex = 2;
			this.radioASAPEmail.TabStop = true;
			this.radioASAPEmail.Tag = OpenDentBusiness.WebSchedVerifyType.Email;
			this.radioASAPEmail.Text = "E-mail";
			this.radioASAPEmail.UseVisualStyleBackColor = true;
			this.radioASAPEmail.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioASAPText
			// 
			this.radioASAPText.Location = new System.Drawing.Point(7, 43);
			this.radioASAPText.Name = "radioASAPText";
			this.radioASAPText.Size = new System.Drawing.Size(175, 17);
			this.radioASAPText.TabIndex = 1;
			this.radioASAPText.TabStop = true;
			this.radioASAPText.Tag = OpenDentBusiness.WebSchedVerifyType.Text;
			this.radioASAPText.Text = "Text";
			this.radioASAPText.UseVisualStyleBackColor = true;
			this.radioASAPText.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioASAPNone
			// 
			this.radioASAPNone.Location = new System.Drawing.Point(7, 20);
			this.radioASAPNone.Name = "radioASAPNone";
			this.radioASAPNone.Size = new System.Drawing.Size(175, 17);
			this.radioASAPNone.TabIndex = 0;
			this.radioASAPNone.TabStop = true;
			this.radioASAPNone.Tag = OpenDentBusiness.WebSchedVerifyType.None;
			this.radioASAPNone.Text = "None";
			this.radioASAPNone.UseVisualStyleBackColor = true;
			this.radioASAPNone.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// groupBoxNewPat
			// 
			this.groupBoxNewPat.Controls.Add(this.groupBoxNewPatTextTemplate);
			this.groupBoxNewPat.Controls.Add(this.groupBoxNewPatEmail);
			this.groupBoxNewPat.Controls.Add(this.groupBoxRadioNewPat);
			this.groupBoxNewPat.Location = new System.Drawing.Point(384, 32);
			this.groupBoxNewPat.Name = "groupBoxNewPat";
			this.groupBoxNewPat.Size = new System.Drawing.Size(372, 497);
			this.groupBoxNewPat.TabIndex = 1;
			this.groupBoxNewPat.TabStop = false;
			this.groupBoxNewPat.Text = "New Patient";
			// 
			// groupBoxNewPatTextTemplate
			// 
			this.groupBoxNewPatTextTemplate.Controls.Add(this.textNewPatTextTemplate);
			this.groupBoxNewPatTextTemplate.Location = new System.Drawing.Point(6, 139);
			this.groupBoxNewPatTextTemplate.Name = "groupBoxNewPatTextTemplate";
			this.groupBoxNewPatTextTemplate.Size = new System.Drawing.Size(360, 120);
			this.groupBoxNewPatTextTemplate.TabIndex = 121;
			this.groupBoxNewPatTextTemplate.TabStop = false;
			this.groupBoxNewPatTextTemplate.Text = "Text Message";
			// 
			// textNewPatTextTemplate
			// 
			this.textNewPatTextTemplate.ContextMenuStrip = this.menuWebSchedVerifyTextTemplate;
			this.textNewPatTextTemplate.Location = new System.Drawing.Point(6, 19);
			this.textNewPatTextTemplate.Multiline = true;
			this.textNewPatTextTemplate.Name = "textNewPatTextTemplate";
			this.textNewPatTextTemplate.Size = new System.Drawing.Size(348, 95);
			this.textNewPatTextTemplate.TabIndex = 314;
			this.textNewPatTextTemplate.Tag = OpenDentBusiness.PrefName.WebSchedVerifyNewPatText;
			this.textNewPatTextTemplate.Leave += new System.EventHandler(this.WebSchedVerify_TextLeave);
			// 
			// groupBoxNewPatEmail
			// 
			this.groupBoxNewPatEmail.Controls.Add(this.butNewPatEditEmail);
			this.groupBoxNewPatEmail.Controls.Add(this.browserNewPatEmailBody);
			this.groupBoxNewPatEmail.Controls.Add(this.textNewPatEmailSubj);
			this.groupBoxNewPatEmail.Location = new System.Drawing.Point(6, 259);
			this.groupBoxNewPatEmail.Name = "groupBoxNewPatEmail";
			this.groupBoxNewPatEmail.Size = new System.Drawing.Size(360, 232);
			this.groupBoxNewPatEmail.TabIndex = 122;
			this.groupBoxNewPatEmail.TabStop = false;
			this.groupBoxNewPatEmail.Text = "E-mail Subject and Body";
			// 
			// butNewPatEditEmail
			// 
			this.butNewPatEditEmail.Location = new System.Drawing.Point(284, 209);
			this.butNewPatEditEmail.Name = "butNewPatEditEmail";
			this.butNewPatEditEmail.Size = new System.Drawing.Size(70, 20);
			this.butNewPatEditEmail.TabIndex = 316;
			this.butNewPatEditEmail.Text = "Edit";
			this.butNewPatEditEmail.UseVisualStyleBackColor = true;
			this.butNewPatEditEmail.Click += new System.EventHandler(this.WebSchedVerify_NewPatEditEmailClick);
			// 
			// browserNewPatEmailBody
			// 
			this.browserNewPatEmailBody.AllowWebBrowserDrop = false;
			this.browserNewPatEmailBody.Location = new System.Drawing.Point(7, 45);
			this.browserNewPatEmailBody.MinimumSize = new System.Drawing.Size(20, 20);
			this.browserNewPatEmailBody.Name = "browserNewPatEmailBody";
			this.browserNewPatEmailBody.Size = new System.Drawing.Size(347, 158);
			this.browserNewPatEmailBody.TabIndex = 315;
			this.browserNewPatEmailBody.WebBrowserShortcutsEnabled = false;
			// 
			// textNewPatEmailSubj
			// 
			this.textNewPatEmailSubj.ContextMenuStrip = this.menuWebSchedVerifyTextTemplate;
			this.textNewPatEmailSubj.Location = new System.Drawing.Point(6, 19);
			this.textNewPatEmailSubj.Multiline = true;
			this.textNewPatEmailSubj.Name = "textNewPatEmailSubj";
			this.textNewPatEmailSubj.Size = new System.Drawing.Size(348, 20);
			this.textNewPatEmailSubj.TabIndex = 314;
			this.textNewPatEmailSubj.Tag = OpenDentBusiness.PrefName.WebSchedVerifyNewPatEmailSubj;
			this.textNewPatEmailSubj.Leave += new System.EventHandler(this.WebSchedVerify_TextLeave);
			// 
			// groupBoxRadioNewPat
			// 
			this.groupBoxRadioNewPat.Controls.Add(this.radioNewPatTextAndEmail);
			this.groupBoxRadioNewPat.Controls.Add(this.radioNewPatEmail);
			this.groupBoxRadioNewPat.Controls.Add(this.radioNewPatText);
			this.groupBoxRadioNewPat.Controls.Add(this.radioNewPatNone);
			this.groupBoxRadioNewPat.Location = new System.Drawing.Point(6, 18);
			this.groupBoxRadioNewPat.Name = "groupBoxRadioNewPat";
			this.groupBoxRadioNewPat.Size = new System.Drawing.Size(360, 115);
			this.groupBoxRadioNewPat.TabIndex = 1;
			this.groupBoxRadioNewPat.TabStop = false;
			this.groupBoxRadioNewPat.Tag = OpenDentBusiness.PrefName.WebSchedVerifyNewPatType;
			this.groupBoxRadioNewPat.Text = "Communication Method";
			// 
			// radioNewPatTextAndEmail
			// 
			this.radioNewPatTextAndEmail.Location = new System.Drawing.Point(7, 89);
			this.radioNewPatTextAndEmail.Name = "radioNewPatTextAndEmail";
			this.radioNewPatTextAndEmail.Size = new System.Drawing.Size(175, 17);
			this.radioNewPatTextAndEmail.TabIndex = 3;
			this.radioNewPatTextAndEmail.TabStop = true;
			this.radioNewPatTextAndEmail.Tag = OpenDentBusiness.WebSchedVerifyType.TextAndEmail;
			this.radioNewPatTextAndEmail.Text = "Text and E-mail";
			this.radioNewPatTextAndEmail.UseVisualStyleBackColor = true;
			this.radioNewPatTextAndEmail.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioNewPatEmail
			// 
			this.radioNewPatEmail.Location = new System.Drawing.Point(7, 66);
			this.radioNewPatEmail.Name = "radioNewPatEmail";
			this.radioNewPatEmail.Size = new System.Drawing.Size(175, 17);
			this.radioNewPatEmail.TabIndex = 2;
			this.radioNewPatEmail.TabStop = true;
			this.radioNewPatEmail.Tag = OpenDentBusiness.WebSchedVerifyType.Email;
			this.radioNewPatEmail.Text = "E-mail";
			this.radioNewPatEmail.UseVisualStyleBackColor = true;
			this.radioNewPatEmail.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioNewPatText
			// 
			this.radioNewPatText.Location = new System.Drawing.Point(7, 43);
			this.radioNewPatText.Name = "radioNewPatText";
			this.radioNewPatText.Size = new System.Drawing.Size(175, 17);
			this.radioNewPatText.TabIndex = 1;
			this.radioNewPatText.TabStop = true;
			this.radioNewPatText.Tag = OpenDentBusiness.WebSchedVerifyType.Text;
			this.radioNewPatText.Text = "Text";
			this.radioNewPatText.UseVisualStyleBackColor = true;
			this.radioNewPatText.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioNewPatNone
			// 
			this.radioNewPatNone.Location = new System.Drawing.Point(7, 20);
			this.radioNewPatNone.Name = "radioNewPatNone";
			this.radioNewPatNone.Size = new System.Drawing.Size(175, 17);
			this.radioNewPatNone.TabIndex = 0;
			this.radioNewPatNone.TabStop = true;
			this.radioNewPatNone.Tag = OpenDentBusiness.WebSchedVerifyType.None;
			this.radioNewPatNone.Text = "None";
			this.radioNewPatNone.UseVisualStyleBackColor = true;
			this.radioNewPatNone.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// groupBoxRecall
			// 
			this.groupBoxRecall.Controls.Add(this.groupBoxRecallTextTemplate);
			this.groupBoxRecall.Controls.Add(this.groupBoxRecallEmail);
			this.groupBoxRecall.Controls.Add(this.groupBoxRadioRecall);
			this.groupBoxRecall.Location = new System.Drawing.Point(6, 32);
			this.groupBoxRecall.Name = "groupBoxRecall";
			this.groupBoxRecall.Size = new System.Drawing.Size(372, 497);
			this.groupBoxRecall.TabIndex = 0;
			this.groupBoxRecall.TabStop = false;
			this.groupBoxRecall.Text = "Recall";
			// 
			// groupBoxRecallTextTemplate
			// 
			this.groupBoxRecallTextTemplate.Controls.Add(this.textRecallTextTemplate);
			this.groupBoxRecallTextTemplate.Location = new System.Drawing.Point(6, 139);
			this.groupBoxRecallTextTemplate.Name = "groupBoxRecallTextTemplate";
			this.groupBoxRecallTextTemplate.Size = new System.Drawing.Size(360, 120);
			this.groupBoxRecallTextTemplate.TabIndex = 121;
			this.groupBoxRecallTextTemplate.TabStop = false;
			this.groupBoxRecallTextTemplate.Text = "Text Message";
			// 
			// textRecallTextTemplate
			// 
			this.textRecallTextTemplate.ContextMenuStrip = this.menuWebSchedVerifyTextTemplate;
			this.textRecallTextTemplate.Location = new System.Drawing.Point(6, 19);
			this.textRecallTextTemplate.Multiline = true;
			this.textRecallTextTemplate.Name = "textRecallTextTemplate";
			this.textRecallTextTemplate.Size = new System.Drawing.Size(348, 95);
			this.textRecallTextTemplate.TabIndex = 314;
			this.textRecallTextTemplate.Tag = OpenDentBusiness.PrefName.WebSchedVerifyRecallText;
			this.textRecallTextTemplate.Leave += new System.EventHandler(this.WebSchedVerify_TextLeave);
			// 
			// groupBoxRecallEmail
			// 
			this.groupBoxRecallEmail.Controls.Add(this.butRecallEditEmail);
			this.groupBoxRecallEmail.Controls.Add(this.browserRecallEmailBody);
			this.groupBoxRecallEmail.Controls.Add(this.textRecallEmailSubj);
			this.groupBoxRecallEmail.Location = new System.Drawing.Point(6, 259);
			this.groupBoxRecallEmail.Name = "groupBoxRecallEmail";
			this.groupBoxRecallEmail.Size = new System.Drawing.Size(360, 232);
			this.groupBoxRecallEmail.TabIndex = 122;
			this.groupBoxRecallEmail.TabStop = false;
			this.groupBoxRecallEmail.Text = "E-mail Subject and Body";
			// 
			// butRecallEditEmail
			// 
			this.butRecallEditEmail.Location = new System.Drawing.Point(284, 209);
			this.butRecallEditEmail.Name = "butRecallEditEmail";
			this.butRecallEditEmail.Size = new System.Drawing.Size(70, 20);
			this.butRecallEditEmail.TabIndex = 318;
			this.butRecallEditEmail.Text = "Edit";
			this.butRecallEditEmail.UseVisualStyleBackColor = true;
			this.butRecallEditEmail.Click += new System.EventHandler(this.WebSchedVerify_RecallEditEmailClick);
			// 
			// browserRecallEmailBody
			// 
			this.browserRecallEmailBody.AllowWebBrowserDrop = false;
			this.browserRecallEmailBody.Location = new System.Drawing.Point(7, 45);
			this.browserRecallEmailBody.MinimumSize = new System.Drawing.Size(20, 20);
			this.browserRecallEmailBody.Name = "browserRecallEmailBody";
			this.browserRecallEmailBody.Size = new System.Drawing.Size(347, 158);
			this.browserRecallEmailBody.TabIndex = 317;
			this.browserRecallEmailBody.WebBrowserShortcutsEnabled = false;
			// 
			// textRecallEmailSubj
			// 
			this.textRecallEmailSubj.ContextMenuStrip = this.menuWebSchedVerifyTextTemplate;
			this.textRecallEmailSubj.Location = new System.Drawing.Point(6, 19);
			this.textRecallEmailSubj.Multiline = true;
			this.textRecallEmailSubj.Name = "textRecallEmailSubj";
			this.textRecallEmailSubj.Size = new System.Drawing.Size(348, 20);
			this.textRecallEmailSubj.TabIndex = 314;
			this.textRecallEmailSubj.Tag = OpenDentBusiness.PrefName.WebSchedVerifyRecallEmailSubj;
			this.textRecallEmailSubj.Leave += new System.EventHandler(this.WebSchedVerify_TextLeave);
			// 
			// groupBoxRadioRecall
			// 
			this.groupBoxRadioRecall.Controls.Add(this.radioRecallTextAndEmail);
			this.groupBoxRadioRecall.Controls.Add(this.radioRecallEmail);
			this.groupBoxRadioRecall.Controls.Add(this.radioRecallText);
			this.groupBoxRadioRecall.Controls.Add(this.radioRecallNone);
			this.groupBoxRadioRecall.Location = new System.Drawing.Point(6, 18);
			this.groupBoxRadioRecall.Name = "groupBoxRadioRecall";
			this.groupBoxRadioRecall.Size = new System.Drawing.Size(360, 115);
			this.groupBoxRadioRecall.TabIndex = 1;
			this.groupBoxRadioRecall.TabStop = false;
			this.groupBoxRadioRecall.Tag = OpenDentBusiness.PrefName.WebSchedVerifyRecallType;
			this.groupBoxRadioRecall.Text = "Communication Method";
			// 
			// radioRecallTextAndEmail
			// 
			this.radioRecallTextAndEmail.Location = new System.Drawing.Point(7, 89);
			this.radioRecallTextAndEmail.Name = "radioRecallTextAndEmail";
			this.radioRecallTextAndEmail.Size = new System.Drawing.Size(175, 17);
			this.radioRecallTextAndEmail.TabIndex = 3;
			this.radioRecallTextAndEmail.TabStop = true;
			this.radioRecallTextAndEmail.Tag = OpenDentBusiness.WebSchedVerifyType.TextAndEmail;
			this.radioRecallTextAndEmail.Text = "Text and E-mail";
			this.radioRecallTextAndEmail.UseVisualStyleBackColor = true;
			this.radioRecallTextAndEmail.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioRecallEmail
			// 
			this.radioRecallEmail.Location = new System.Drawing.Point(7, 66);
			this.radioRecallEmail.Name = "radioRecallEmail";
			this.radioRecallEmail.Size = new System.Drawing.Size(175, 17);
			this.radioRecallEmail.TabIndex = 2;
			this.radioRecallEmail.TabStop = true;
			this.radioRecallEmail.Tag = OpenDentBusiness.WebSchedVerifyType.Email;
			this.radioRecallEmail.Text = "E-mail";
			this.radioRecallEmail.UseVisualStyleBackColor = true;
			this.radioRecallEmail.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioRecallText
			// 
			this.radioRecallText.Location = new System.Drawing.Point(7, 43);
			this.radioRecallText.Name = "radioRecallText";
			this.radioRecallText.Size = new System.Drawing.Size(175, 17);
			this.radioRecallText.TabIndex = 1;
			this.radioRecallText.TabStop = true;
			this.radioRecallText.Tag = OpenDentBusiness.WebSchedVerifyType.Text;
			this.radioRecallText.Text = "Text";
			this.radioRecallText.UseVisualStyleBackColor = true;
			this.radioRecallText.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioRecallNone
			// 
			this.radioRecallNone.Location = new System.Drawing.Point(7, 20);
			this.radioRecallNone.Name = "radioRecallNone";
			this.radioRecallNone.Size = new System.Drawing.Size(175, 17);
			this.radioRecallNone.TabIndex = 0;
			this.radioRecallNone.TabStop = true;
			this.radioRecallNone.Tag = OpenDentBusiness.WebSchedVerifyType.None;
			this.radioRecallNone.Text = "None";
			this.radioRecallNone.UseVisualStyleBackColor = true;
			this.radioRecallNone.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// comboClinicVerify
			// 
			this.comboClinicVerify.HqDescription = "Default";
			this.comboClinicVerify.IncludeUnassigned = true;
			this.comboClinicVerify.Location = new System.Drawing.Point(150, 8);
			this.comboClinicVerify.Name = "comboClinicVerify";
			this.comboClinicVerify.Size = new System.Drawing.Size(216, 21);
			this.comboClinicVerify.TabIndex = 268;
			this.comboClinicVerify.SelectedIndexChanged += new System.EventHandler(this.WebSchedVerify_ComboClinicSelectedIndexChanged);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(1084, 626);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 501;
			this.butCancel.Text = "Cancel";
			this.butCancel.UseVisualStyleBackColor = true;
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormEServicesWebSched
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1171, 662);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.linkLabelAboutWebSched);
			this.Controls.Add(this.labelWebSchedDesc);
			this.Controls.Add(this.tabControlWebSched);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(0, 0);
			this.Name = "FormEServicesWebSched";
			this.Text = "eServices Web Sched";
			this.Load += new System.EventHandler(this.FormEServicesWebSched_Load);
			this.menuWebSchedVerifyTextTemplate.ResumeLayout(false);
			this.tabControlWebSched.ResumeLayout(false);
			this.tabWebSchedRecalls.ResumeLayout(false);
			this.tabWebSchedRecalls.PerformLayout();
			this.groupWebSchedProvRule.ResumeLayout(false);
			this.groupWebSchedText.ResumeLayout(false);
			this.groupWebSchedText.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBoxWebSchedAutomation.ResumeLayout(false);
			this.groupWebSchedPreview.ResumeLayout(false);
			this.groupWebSchedPreview.PerformLayout();
			this.tabWebSchedNewPatAppts.ResumeLayout(false);
			this.tabWebSchedNewPatAppts.PerformLayout();
			this.groupBoxWSNPHostedURLs.ResumeLayout(false);
			this.groupBox13.ResumeLayout(false);
			this.groupBox13.PerformLayout();
			this.groupBox11.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			this.tabWebSchedVerify.ResumeLayout(false);
			this.groupBoxASAP.ResumeLayout(false);
			this.groupBoxASAPTextTemplate.ResumeLayout(false);
			this.groupBoxASAPTextTemplate.PerformLayout();
			this.groupBoxASAPEmail.ResumeLayout(false);
			this.groupBoxASAPEmail.PerformLayout();
			this.groupBoxRadioASAP.ResumeLayout(false);
			this.groupBoxNewPat.ResumeLayout(false);
			this.groupBoxNewPatTextTemplate.ResumeLayout(false);
			this.groupBoxNewPatTextTemplate.PerformLayout();
			this.groupBoxNewPatEmail.ResumeLayout(false);
			this.groupBoxNewPatEmail.PerformLayout();
			this.groupBoxRadioNewPat.ResumeLayout(false);
			this.groupBoxRecall.ResumeLayout(false);
			this.groupBoxRecallTextTemplate.ResumeLayout(false);
			this.groupBoxRecallTextTemplate.PerformLayout();
			this.groupBoxRecallEmail.ResumeLayout(false);
			this.groupBoxRecallEmail.PerformLayout();
			this.groupBoxRadioRecall.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private UI.Button butClose;
		private System.Windows.Forms.Label labelWebSchedDesc;
		private UI.Button butRecallSchedSetup;
		private System.Windows.Forms.Label label20;
		private UI.ODGrid gridWebSchedRecallTypes;
		private System.Windows.Forms.Label label35;
		private UI.ODGrid gridWebSchedTimeSlots;
		private System.Windows.Forms.GroupBox groupWebSchedPreview;
		private System.Windows.Forms.Label labelWebSchedClinic;
		private System.Windows.Forms.Label labelWebSchedRecallTypes;
		private System.Windows.Forms.ComboBox comboWebSchedClinic;
		private System.Windows.Forms.ComboBox comboWebSchedRecallTypes;
		private ValidDate textWebSchedDateStart;
		private UI.Button butWebSchedToday;
		private UI.ODGrid gridWebSchedOperatories;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.ListBox listBoxWebSchedProviderPref;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.ComboBox comboWebSchedProviders;
		private UI.Button butWebSchedPickClinic;
		private UI.Button butWebSchedPickProv;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.TabControl tabControlWebSched;
		private System.Windows.Forms.TabPage tabWebSchedRecalls;
		private System.Windows.Forms.TabPage tabWebSchedNewPatAppts;
		private System.Windows.Forms.Label label41;
		private ValidNumber textWebSchedNewPatApptSearchDays;
		private System.Windows.Forms.Label label40;
		private UI.ODGrid gridWebSchedNewPatApptOps;
		private System.Windows.Forms.Label label42;
		private System.Windows.Forms.GroupBox groupBox7;
		private UI.Button butWebSchedNewPatApptsToday;
		private UI.ODGrid gridWebSchedNewPatApptTimeSlots;
		private ValidDate textWebSchedNewPatApptsDateStart;
		private UI.ODGrid gridWSNPAReasons;
		private System.Windows.Forms.Label labelWSNPTimeSlots;
		private System.Windows.Forms.LinkLabel linkLabelAboutWebSched;
		private System.Windows.Forms.GroupBox groupBoxWebSchedAutomation;
		private System.Windows.Forms.RadioButton radioDoNotSend;
		private System.Windows.Forms.RadioButton radioSendToEmailOnlyPreferred;
		private System.Windows.Forms.RadioButton radioSendToEmailNoPreferred;
		private System.Windows.Forms.RadioButton radioSendToEmail;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.ListBox listboxWebSchedRecallIgnoreBlockoutTypes;
		private UI.Button butWebSchedRecallBlockouts;
		private System.Windows.Forms.GroupBox groupBox11;
		private System.Windows.Forms.ListBox listboxWebSchedNewPatIgnoreBlockoutTypes;
		private System.Windows.Forms.Label label33;
		private UI.Button butWebSchedNewPatBlockouts;
		private System.Windows.Forms.GroupBox groupWebSchedText;
		private System.Windows.Forms.RadioButton radioDoNotSendText;
		private System.Windows.Forms.RadioButton radioSendText;
		private System.Windows.Forms.Label labelWebSchedPerBatch;
		private ValidNumber textWebSchedPerBatch;
		private UI.ComboBoxPlus comboWSNPConfirmStatuses;
		private System.Windows.Forms.Label labelWebSchedNewPatConfirmStatus;
		private System.Windows.Forms.CheckBox checkRecallAllowProvSelection;
		private System.Windows.Forms.TabPage tabWebSchedVerify;
		private System.Windows.Forms.CheckBox checkUseDefaultsVerify;
		private System.Windows.Forms.GroupBox groupBoxASAP;
		private System.Windows.Forms.GroupBox groupBoxASAPTextTemplate;
		private System.Windows.Forms.TextBox textASAPTextTemplate;
		private System.Windows.Forms.GroupBox groupBoxASAPEmail;
		private System.Windows.Forms.TextBox textASAPEmailSubj;
		private System.Windows.Forms.GroupBox groupBoxRadioASAP;
		private System.Windows.Forms.RadioButton radioASAPTextAndEmail;
		private System.Windows.Forms.RadioButton radioASAPEmail;
		private System.Windows.Forms.RadioButton radioASAPText;
		private System.Windows.Forms.RadioButton radioASAPNone;
		private System.Windows.Forms.GroupBox groupBoxNewPat;
		private System.Windows.Forms.GroupBox groupBoxNewPatTextTemplate;
		private System.Windows.Forms.TextBox textNewPatTextTemplate;
		private System.Windows.Forms.GroupBox groupBoxNewPatEmail;
		private System.Windows.Forms.TextBox textNewPatEmailSubj;
		private System.Windows.Forms.GroupBox groupBoxRadioNewPat;
		private System.Windows.Forms.RadioButton radioNewPatTextAndEmail;
		private System.Windows.Forms.RadioButton radioNewPatText;
		private System.Windows.Forms.RadioButton radioNewPatNone;
		private System.Windows.Forms.GroupBox groupBoxRecall;
		private System.Windows.Forms.GroupBox groupBoxRecallTextTemplate;
		private System.Windows.Forms.TextBox textRecallTextTemplate;
		private System.Windows.Forms.GroupBox groupBoxRecallEmail;
		private System.Windows.Forms.TextBox textRecallEmailSubj;
		private System.Windows.Forms.GroupBox groupBoxRadioRecall;
		private System.Windows.Forms.RadioButton radioRecallTextAndEmail;
		private System.Windows.Forms.RadioButton radioRecallEmail;
		private System.Windows.Forms.RadioButton radioRecallText;
		private System.Windows.Forms.RadioButton radioRecallNone;
		private UI.ComboBoxClinicPicker comboClinicVerify;
		private System.Windows.Forms.RadioButton radioNewPatEmail;
		private System.Windows.Forms.ContextMenuStrip menuWebSchedVerifyTextTemplate;
		private System.Windows.Forms.ToolStripMenuItem insertReplacementsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
		private System.Windows.Forms.Label label28;
		private UI.Button butRestoreWebSchedVerify;
		private System.Windows.Forms.CheckBox checkWebSchedNewPatForcePhoneFormatting;
		private System.Windows.Forms.CheckBox checkNewPatAllowProvSelection;
		private UI.ComboBoxPlus comboWSRConfirmStatus;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label labelWSNPAApptType;
		private UI.ComboBoxPlus comboWSNPADefApptType;
		private System.Windows.Forms.GroupBox groupBoxWSNPHostedURLs;
		private System.Windows.Forms.FlowLayoutPanel panelHostedURLs;
		private System.Windows.Forms.Label labelWSNPClinic;
		private System.Windows.Forms.ComboBox comboWSNPClinics;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.GroupBox groupBox13;
		private System.Windows.Forms.TextBox textWebSchedNewPatApptMessage;
		private System.Windows.Forms.CheckBox checkWSRDoubleBooking;
		private System.Windows.Forms.CheckBox checkWSNPDoubleBooking;
		private UI.Button butAsapEditEmail;
		private System.Windows.Forms.WebBrowser browserAsapEmailBody;
		private UI.Button butNewPatEditEmail;
		private System.Windows.Forms.WebBrowser browserNewPatEmailBody;
		private UI.Button butRecallEditEmail;
		private System.Windows.Forms.WebBrowser browserRecallEmailBody;
		private System.Windows.Forms.Label label43;
		private System.Windows.Forms.Label label44;
		private ValidNumber textWebSchedRecallApptSearchDays;
		private System.Windows.Forms.GroupBox groupWebSchedProvRule;
		private System.Windows.Forms.CheckBox checkUseDefaultProvRule;
		private UI.ComboBoxClinicPicker comboClinicProvRule;
		private UI.Button butProvRulePickClinic;
		private System.Windows.Forms.ListBox listboxWSNPARestrictedToReasons;
		private UI.Button butWSNPRestrictedToReasonsEdit;
		private System.Windows.Forms.Label labelWSNPRestrictedToReasons;
		private UI.Button butCancel;
	}
}