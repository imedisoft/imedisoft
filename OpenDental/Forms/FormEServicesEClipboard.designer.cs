namespace OpenDental{
	partial class FormEServicesEClipboard{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEServicesEClipboard));
			this.label37 = new System.Windows.Forms.Label();
			this.menuWebSchedVerifyTextTemplate = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.insertReplacementsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.butOK = new OpenDental.UI.Button();
			this.checkEClipboardUseDefaults = new System.Windows.Forms.CheckBox();
			this.groupEClipboardRules = new System.Windows.Forms.GroupBox();
			this.checkEClipboardAllowSelfPortrait = new System.Windows.Forms.CheckBox();
			this.checkEClipboardCreateMissingForms = new System.Windows.Forms.CheckBox();
			this.labelEClipboardMessage = new System.Windows.Forms.Label();
			this.textEClipboardMessage = new System.Windows.Forms.TextBox();
			this.checkEClipboardAllowSheets = new System.Windows.Forms.CheckBox();
			this.checkEClipboardAllowCheckIn = new System.Windows.Forms.CheckBox();
			this.checkEClipboardPopupKiosk = new System.Windows.Forms.CheckBox();
			this.groupEClipboardSheets = new System.Windows.Forms.GroupBox();
			this.butEClipboardAddSheets = new OpenDental.UI.Button();
			this.butEClipboardUp = new OpenDental.UI.Button();
			this.butEClipboardDown = new OpenDental.UI.Button();
			this.butEClipboardRight = new OpenDental.UI.Button();
			this.butEClipboardLeft = new OpenDental.UI.Button();
			this.gridEClipboardSheetsInUse = new OpenDental.UI.ODGrid();
			this.listEClipboardSheetsAvailable = new System.Windows.Forms.ListBox();
			this.labelOps = new System.Windows.Forms.Label();
			this.gridMobileAppDevices = new OpenDental.UI.ODGrid();
			this.label10 = new System.Windows.Forms.Label();
			this.clinicPickerEClipboard = new OpenDental.UI.ComboBoxClinicPicker();
			this.labelEClipboardNotSignedUp = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.menuWebSchedVerifyTextTemplate.SuspendLayout();
			this.groupEClipboardRules.SuspendLayout();
			this.groupEClipboardSheets.SuspendLayout();
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
			// 
			// undoToolStripMenuItem
			// 
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			this.undoToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.undoToolStripMenuItem.Text = "Undo";
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.cutToolStripMenuItem.Text = "Cut";
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.pasteToolStripMenuItem.Text = "Paste";
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.selectAllToolStripMenuItem.Text = "Select All";
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(989, 592);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 23);
			this.butOK.TabIndex = 500;
			this.butOK.Text = "OK";
			this.butOK.UseVisualStyleBackColor = true;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// checkEClipboardUseDefaults
			// 
			this.checkEClipboardUseDefaults.Location = new System.Drawing.Point(11, 33);
			this.checkEClipboardUseDefaults.Name = "checkEClipboardUseDefaults";
			this.checkEClipboardUseDefaults.Size = new System.Drawing.Size(298, 17);
			this.checkEClipboardUseDefaults.TabIndex = 269;
			this.checkEClipboardUseDefaults.Text = "Use Defaults for this clinic";
			this.checkEClipboardUseDefaults.UseVisualStyleBackColor = true;
			this.checkEClipboardUseDefaults.Click += new System.EventHandler(this.CheckEClipboardUseDefaults_Click);
			// 
			// groupEClipboardRules
			// 
			this.groupEClipboardRules.Controls.Add(this.checkEClipboardAllowSelfPortrait);
			this.groupEClipboardRules.Controls.Add(this.checkEClipboardCreateMissingForms);
			this.groupEClipboardRules.Controls.Add(this.labelEClipboardMessage);
			this.groupEClipboardRules.Controls.Add(this.textEClipboardMessage);
			this.groupEClipboardRules.Controls.Add(this.checkEClipboardAllowSheets);
			this.groupEClipboardRules.Controls.Add(this.checkEClipboardAllowCheckIn);
			this.groupEClipboardRules.Controls.Add(this.checkEClipboardPopupKiosk);
			this.groupEClipboardRules.Location = new System.Drawing.Point(11, 53);
			this.groupEClipboardRules.Name = "groupEClipboardRules";
			this.groupEClipboardRules.Size = new System.Drawing.Size(487, 237);
			this.groupEClipboardRules.TabIndex = 265;
			this.groupEClipboardRules.TabStop = false;
			this.groupEClipboardRules.Text = "Behavior Rules";
			// 
			// checkEClipboardAllowSelfPortrait
			// 
			this.checkEClipboardAllowSelfPortrait.Location = new System.Drawing.Point(19, 42);
			this.checkEClipboardAllowSelfPortrait.Name = "checkEClipboardAllowSelfPortrait";
			this.checkEClipboardAllowSelfPortrait.Size = new System.Drawing.Size(458, 17);
			this.checkEClipboardAllowSelfPortrait.TabIndex = 268;
			this.checkEClipboardAllowSelfPortrait.Text = "Allow patients to take self-portrait in mobile app";
			this.checkEClipboardAllowSelfPortrait.UseVisualStyleBackColor = true;
			this.checkEClipboardAllowSelfPortrait.Click += new System.EventHandler(this.CheckEClipboardAllowSelfPortrait_Click);
			// 
			// checkEClipboardCreateMissingForms
			// 
			this.checkEClipboardCreateMissingForms.Location = new System.Drawing.Point(19, 87);
			this.checkEClipboardCreateMissingForms.Name = "checkEClipboardCreateMissingForms";
			this.checkEClipboardCreateMissingForms.Size = new System.Drawing.Size(458, 17);
			this.checkEClipboardCreateMissingForms.TabIndex = 267;
			this.checkEClipboardCreateMissingForms.Text = "Add specified forms upon patient arrival";
			this.checkEClipboardCreateMissingForms.UseVisualStyleBackColor = true;
			this.checkEClipboardCreateMissingForms.Click += new System.EventHandler(this.CheckEClipboardCreateMissingForms_Click);
			// 
			// labelEClipboardMessage
			// 
			this.labelEClipboardMessage.Location = new System.Drawing.Point(16, 133);
			this.labelEClipboardMessage.Name = "labelEClipboardMessage";
			this.labelEClipboardMessage.Size = new System.Drawing.Size(461, 18);
			this.labelEClipboardMessage.TabIndex = 266;
			this.labelEClipboardMessage.Text = "Message to show patients after successful check-in";
			this.labelEClipboardMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textEClipboardMessage
			// 
			this.textEClipboardMessage.Location = new System.Drawing.Point(19, 154);
			this.textEClipboardMessage.Multiline = true;
			this.textEClipboardMessage.Name = "textEClipboardMessage";
			this.textEClipboardMessage.Size = new System.Drawing.Size(458, 66);
			this.textEClipboardMessage.TabIndex = 3;
			this.textEClipboardMessage.TextChanged += new System.EventHandler(this.TextEClipboardMessage_TextChanged);
			// 
			// checkEClipboardAllowSheets
			// 
			this.checkEClipboardAllowSheets.Location = new System.Drawing.Point(19, 64);
			this.checkEClipboardAllowSheets.Name = "checkEClipboardAllowSheets";
			this.checkEClipboardAllowSheets.Size = new System.Drawing.Size(458, 17);
			this.checkEClipboardAllowSheets.TabIndex = 2;
			this.checkEClipboardAllowSheets.Text = "Allow patients to fill out forms in mobile app";
			this.checkEClipboardAllowSheets.UseVisualStyleBackColor = true;
			this.checkEClipboardAllowSheets.Click += new System.EventHandler(this.CheckEClipboardAllowSheets_Click);
			// 
			// checkEClipboardAllowCheckIn
			// 
			this.checkEClipboardAllowCheckIn.Location = new System.Drawing.Point(19, 19);
			this.checkEClipboardAllowCheckIn.Name = "checkEClipboardAllowCheckIn";
			this.checkEClipboardAllowCheckIn.Size = new System.Drawing.Size(458, 17);
			this.checkEClipboardAllowCheckIn.TabIndex = 1;
			this.checkEClipboardAllowCheckIn.Text = "Allow self check-in";
			this.checkEClipboardAllowCheckIn.UseVisualStyleBackColor = true;
			this.checkEClipboardAllowCheckIn.Click += new System.EventHandler(this.CheckEClipboardAllowCheckIn_Click);
			// 
			// checkEClipboardPopupKiosk
			// 
			this.checkEClipboardPopupKiosk.Location = new System.Drawing.Point(19, 110);
			this.checkEClipboardPopupKiosk.Name = "checkEClipboardPopupKiosk";
			this.checkEClipboardPopupKiosk.Size = new System.Drawing.Size(458, 17);
			this.checkEClipboardPopupKiosk.TabIndex = 0;
			this.checkEClipboardPopupKiosk.Text = "Show kiosk manager when staff changes patient status to arrived";
			this.checkEClipboardPopupKiosk.UseVisualStyleBackColor = true;
			this.checkEClipboardPopupKiosk.Click += new System.EventHandler(this.CheckEClipboardPopupKiosk_Click);
			// 
			// groupEClipboardSheets
			// 
			this.groupEClipboardSheets.Controls.Add(this.butEClipboardAddSheets);
			this.groupEClipboardSheets.Controls.Add(this.butEClipboardUp);
			this.groupEClipboardSheets.Controls.Add(this.butEClipboardDown);
			this.groupEClipboardSheets.Controls.Add(this.butEClipboardRight);
			this.groupEClipboardSheets.Controls.Add(this.butEClipboardLeft);
			this.groupEClipboardSheets.Controls.Add(this.gridEClipboardSheetsInUse);
			this.groupEClipboardSheets.Controls.Add(this.listEClipboardSheetsAvailable);
			this.groupEClipboardSheets.Controls.Add(this.labelOps);
			this.groupEClipboardSheets.Location = new System.Drawing.Point(11, 296);
			this.groupEClipboardSheets.Name = "groupEClipboardSheets";
			this.groupEClipboardSheets.Size = new System.Drawing.Size(487, 273);
			this.groupEClipboardSheets.TabIndex = 266;
			this.groupEClipboardSheets.TabStop = false;
			this.groupEClipboardSheets.Text = "Specify which forms are added upon patient arrival";
			// 
			// butEClipboardAddSheets
			// 
			this.butEClipboardAddSheets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butEClipboardAddSheets.Location = new System.Drawing.Point(6, 244);
			this.butEClipboardAddSheets.Name = "butEClipboardAddSheets";
			this.butEClipboardAddSheets.Size = new System.Drawing.Size(170, 23);
			this.butEClipboardAddSheets.TabIndex = 503;
			this.butEClipboardAddSheets.Text = "Add Custom Sheets";
			this.butEClipboardAddSheets.UseVisualStyleBackColor = true;
			this.butEClipboardAddSheets.Click += new System.EventHandler(this.ButEClipboardAddSheets_Click);
			// 
			// butEClipboardUp
			// 
			this.butEClipboardUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butEClipboardUp.Image = global::OpenDental.Properties.Resources.up;
			this.butEClipboardUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEClipboardUp.Location = new System.Drawing.Point(444, 208);
			this.butEClipboardUp.Name = "butEClipboardUp";
			this.butEClipboardUp.Size = new System.Drawing.Size(33, 26);
			this.butEClipboardUp.TabIndex = 267;
			this.butEClipboardUp.Click += new System.EventHandler(this.ButEClipboardUp_Click);
			// 
			// butEClipboardDown
			// 
			this.butEClipboardDown.Image = global::OpenDental.Properties.Resources.down;
			this.butEClipboardDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEClipboardDown.Location = new System.Drawing.Point(444, 240);
			this.butEClipboardDown.Name = "butEClipboardDown";
			this.butEClipboardDown.Size = new System.Drawing.Size(33, 26);
			this.butEClipboardDown.TabIndex = 268;
			this.butEClipboardDown.Click += new System.EventHandler(this.ButEClipboardDown_Click);
			// 
			// butEClipboardRight
			// 
			this.butEClipboardRight.Image = global::OpenDental.Properties.Resources.Right;
			this.butEClipboardRight.Location = new System.Drawing.Point(182, 97);
			this.butEClipboardRight.Name = "butEClipboardRight";
			this.butEClipboardRight.Size = new System.Drawing.Size(35, 26);
			this.butEClipboardRight.TabIndex = 64;
			this.butEClipboardRight.Click += new System.EventHandler(this.ButEClipboardRight_Click);
			// 
			// butEClipboardLeft
			// 
			this.butEClipboardLeft.AdjustImageLocation = new System.Drawing.Point(-1, 0);
			this.butEClipboardLeft.Image = global::OpenDental.Properties.Resources.Left;
			this.butEClipboardLeft.Location = new System.Drawing.Point(182, 131);
			this.butEClipboardLeft.Name = "butEClipboardLeft";
			this.butEClipboardLeft.Size = new System.Drawing.Size(35, 26);
			this.butEClipboardLeft.TabIndex = 63;
			this.butEClipboardLeft.Click += new System.EventHandler(this.ButEClipboardLeft_Click);
			// 
			// gridEClipboardSheetsInUse
			// 
			this.gridEClipboardSheetsInUse.Location = new System.Drawing.Point(223, 22);
			this.gridEClipboardSheetsInUse.Name = "gridEClipboardSheetsInUse";
			this.gridEClipboardSheetsInUse.Size = new System.Drawing.Size(215, 244);
			this.gridEClipboardSheetsInUse.TabIndex = 62;
			this.gridEClipboardSheetsInUse.Title = "Sheets In Use";
			this.gridEClipboardSheetsInUse.TranslationName = "TableAvailableRows";
			this.gridEClipboardSheetsInUse.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.GridEClipboardSheetsInUse_CellDoubleClick);
			// 
			// listEClipboardSheetsAvailable
			// 
			this.listEClipboardSheetsAvailable.Location = new System.Drawing.Point(6, 39);
			this.listEClipboardSheetsAvailable.Name = "listEClipboardSheetsAvailable";
			this.listEClipboardSheetsAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listEClipboardSheetsAvailable.Size = new System.Drawing.Size(170, 199);
			this.listEClipboardSheetsAvailable.TabIndex = 42;
			// 
			// labelOps
			// 
			this.labelOps.Location = new System.Drawing.Point(3, 17);
			this.labelOps.Name = "labelOps";
			this.labelOps.Size = new System.Drawing.Size(214, 18);
			this.labelOps.TabIndex = 41;
			this.labelOps.Text = "Available Sheets (Custom Sheets Only)";
			this.labelOps.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// gridMobileAppDevices
			// 
			this.gridMobileAppDevices.Location = new System.Drawing.Point(504, 53);
			this.gridMobileAppDevices.Name = "gridMobileAppDevices";
			this.gridMobileAppDevices.Size = new System.Drawing.Size(640, 523);
			this.gridMobileAppDevices.TabIndex = 270;
			this.gridMobileAppDevices.Title = "Mobile App Devices";
			this.gridMobileAppDevices.TranslationName = "Checkin Devices";
			this.gridMobileAppDevices.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMobileAppDevices_CellClick);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(823, 31);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(321, 19);
			this.label10.TabIndex = 271;
			this.label10.Text = "To add devices to this list, log in to the mobile app via the device. ";
			// 
			// clinicPickerEClipboard
			// 
			this.clinicPickerEClipboard.HqDescription = "Default";
			this.clinicPickerEClipboard.IncludeUnassigned = true;
			this.clinicPickerEClipboard.Location = new System.Drawing.Point(449, 17);
			this.clinicPickerEClipboard.Name = "clinicPickerEClipboard";
			this.clinicPickerEClipboard.Size = new System.Drawing.Size(200, 21);
			this.clinicPickerEClipboard.TabIndex = 272;
			// 
			// labelEClipboardNotSignedUp
			// 
			this.labelEClipboardNotSignedUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelEClipboardNotSignedUp.Location = new System.Drawing.Point(8, 11);
			this.labelEClipboardNotSignedUp.Name = "labelEClipboardNotSignedUp";
			this.labelEClipboardNotSignedUp.Size = new System.Drawing.Size(321, 19);
			this.labelEClipboardNotSignedUp.TabIndex = 273;
			this.labelEClipboardNotSignedUp.Text = "Go to the Signup Portal to enable this feature.";
			this.labelEClipboardNotSignedUp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(1070, 591);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 501;
			this.butCancel.Text = "Cancel";
			this.butCancel.UseVisualStyleBackColor = true;
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormEServicesEClipboard
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1157, 627);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.labelEClipboardNotSignedUp);
			this.Controls.Add(this.clinicPickerEClipboard);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.gridMobileAppDevices);
			this.Controls.Add(this.groupEClipboardSheets);
			this.Controls.Add(this.groupEClipboardRules);
			this.Controls.Add(this.checkEClipboardUseDefaults);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(0, 0);
			this.Name = "FormEServicesEClipboard";
			this.Text = "eServices EClipboard";
			this.Load += new System.EventHandler(this.FormEServicesEClipboard_Load);
			this.menuWebSchedVerifyTextTemplate.ResumeLayout(false);
			this.groupEClipboardRules.ResumeLayout(false);
			this.groupEClipboardRules.PerformLayout();
			this.groupEClipboardSheets.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private UI.Button butOK;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.ContextMenuStrip menuWebSchedVerifyTextTemplate;
		private System.Windows.Forms.ToolStripMenuItem insertReplacementsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
		private System.Windows.Forms.CheckBox checkEClipboardUseDefaults;
		private System.Windows.Forms.GroupBox groupEClipboardRules;
		private System.Windows.Forms.CheckBox checkEClipboardAllowSelfPortrait;
		private System.Windows.Forms.CheckBox checkEClipboardCreateMissingForms;
		private System.Windows.Forms.Label labelEClipboardMessage;
		private System.Windows.Forms.TextBox textEClipboardMessage;
		private System.Windows.Forms.CheckBox checkEClipboardAllowSheets;
		private System.Windows.Forms.CheckBox checkEClipboardAllowCheckIn;
		private System.Windows.Forms.CheckBox checkEClipboardPopupKiosk;
		private System.Windows.Forms.GroupBox groupEClipboardSheets;
		private UI.Button butEClipboardAddSheets;
		private UI.Button butEClipboardUp;
		private UI.Button butEClipboardDown;
		private UI.Button butEClipboardRight;
		private UI.Button butEClipboardLeft;
		private UI.ODGrid gridEClipboardSheetsInUse;
		private System.Windows.Forms.ListBox listEClipboardSheetsAvailable;
		private System.Windows.Forms.Label labelOps;
		private UI.ODGrid gridMobileAppDevices;
		private System.Windows.Forms.Label label10;
		private UI.ComboBoxClinicPicker clinicPickerEClipboard;
		private System.Windows.Forms.Label labelEClipboardNotSignedUp;
		private UI.Button butCancel;
	}
}