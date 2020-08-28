using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using Imedisoft.UI;

namespace OpenDental {
	///<summary></summary>
	public class FormMisc : ODForm {
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private IContainer components;
		private System.Windows.Forms.TextBox textMainWindowTitle;
		private System.Windows.Forms.Label label3;
		private OpenDental.ValidNumber textSigInterval;
		private OpenDental.UI.Button butLanguages;
		private Label label4;
		private ToolTip toolTip1;
		private ComboBox comboShowID;
		private Label label15;
		private Label label17;
		private GroupBox groupBox6;
		private CheckBox checkTitleBarShowSite;
		private TextBox textWebServiceServerName;
		private Label label2;
		private ValidNumber textInactiveSignal;
		private Label label5;
		private CheckBox checkPrefFName;
		private CheckBox checkRefresh;
		private CheckBox checkImeCompositionCompatibility;
		private TextBox textLanguageAndRegion;
		private Label label6;
		private UI.Button butPickLanguageAndRegion;
    private CheckBox checkTimeCardUseLocal;
		private ComboBox comboTrackClinic;
		private Label labelTrackClinic;
		private Label label1;
		private UI.Button butDecimal;
		private TextBox textNumDecimals;
		private Label label7;
		private TextBox textSyncCode;
		private Label label8;
		private UI.Button butClearCode;
		private Label label9;
		private ValidNum textAuditEntries;
		private CheckBox checkAgingIsEnterprise;
		private UI.Button butClearAgingBeginDateT;
		private Label labelAgingBeginDateT;
		private TextBox textAgingBeginDateT;
		private Label labelAutoAgingRunTime;
		private TextBox textAutoAgingRunTime;
		private CheckBox checkUseClinicAbbr;
		private CheckBox checkSubmitExceptions;
		private CheckBox checkTitleBarShowSpecialty;
		private CheckBox checkUserTheme;
		private GroupBox groupSelectPatient;
		private CheckBox checkPatientSelectFilterRestrictedClinics;
		private GroupBox groupTheme;
		private CheckBox checkAlternateIcons;
		private CheckBox checkShowInactivePatientsDefault;
		private ValidNumber textInactiveAlert;
		private Label label10;
		private ValidNumber textAlertInterval;
		private Label label11;
		private List<string> _trackLastClinicBy;
		//private List<Def> posAdjTypes;

		///<summary></summary>
		public FormMisc(){
			InitializeComponent();
			
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMisc));
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.textAuditEntries = new OpenDental.ValidNum();
			this.label9 = new System.Windows.Forms.Label();
			this.butClearCode = new OpenDental.UI.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.textSyncCode = new System.Windows.Forms.TextBox();
			this.butDecimal = new OpenDental.UI.Button();
			this.textNumDecimals = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.comboTrackClinic = new System.Windows.Forms.ComboBox();
			this.labelTrackClinic = new System.Windows.Forms.Label();
			this.checkTimeCardUseLocal = new System.Windows.Forms.CheckBox();
			this.butPickLanguageAndRegion = new OpenDental.UI.Button();
			this.textLanguageAndRegion = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.checkImeCompositionCompatibility = new System.Windows.Forms.CheckBox();
			this.checkRefresh = new System.Windows.Forms.CheckBox();
			this.checkPrefFName = new System.Windows.Forms.CheckBox();
			this.textInactiveSignal = new OpenDental.ValidNumber();
			this.label5 = new System.Windows.Forms.Label();
			this.textWebServiceServerName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.checkTitleBarShowSpecialty = new System.Windows.Forms.CheckBox();
			this.checkUseClinicAbbr = new System.Windows.Forms.CheckBox();
			this.textMainWindowTitle = new System.Windows.Forms.TextBox();
			this.checkTitleBarShowSite = new System.Windows.Forms.CheckBox();
			this.label15 = new System.Windows.Forms.Label();
			this.comboShowID = new System.Windows.Forms.ComboBox();
			this.label17 = new System.Windows.Forms.Label();
			this.groupSelectPatient = new System.Windows.Forms.GroupBox();
			this.checkShowInactivePatientsDefault = new System.Windows.Forms.CheckBox();
			this.checkPatientSelectFilterRestrictedClinics = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.butLanguages = new OpenDental.UI.Button();
			this.textSigInterval = new OpenDental.ValidNumber();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.checkAgingIsEnterprise = new System.Windows.Forms.CheckBox();
			this.butClearAgingBeginDateT = new OpenDental.UI.Button();
			this.labelAgingBeginDateT = new System.Windows.Forms.Label();
			this.textAgingBeginDateT = new System.Windows.Forms.TextBox();
			this.labelAutoAgingRunTime = new System.Windows.Forms.Label();
			this.textAutoAgingRunTime = new System.Windows.Forms.TextBox();
			this.checkSubmitExceptions = new System.Windows.Forms.CheckBox();
			this.checkUserTheme = new System.Windows.Forms.CheckBox();
			this.groupTheme = new System.Windows.Forms.GroupBox();
			this.checkAlternateIcons = new System.Windows.Forms.CheckBox();
			this.textInactiveAlert = new OpenDental.ValidNumber();
			this.label10 = new System.Windows.Forms.Label();
			this.textAlertInterval = new OpenDental.ValidNumber();
			this.label11 = new System.Windows.Forms.Label();
			this.groupBox6.SuspendLayout();
			this.groupSelectPatient.SuspendLayout();
			this.groupTheme.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolTip1
			// 
			this.toolTip1.AutomaticDelay = 0;
			this.toolTip1.AutoPopDelay = 600000;
			this.toolTip1.InitialDelay = 0;
			this.toolTip1.IsBalloon = true;
			this.toolTip1.ReshowDelay = 0;
			this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.toolTip1.ToolTipTitle = "Help";
			// 
			// textAuditEntries
			// 
			this.textAuditEntries.Location = new System.Drawing.Point(375, 497);
			this.textAuditEntries.MaxLength = 5;
			this.textAuditEntries.MaxVal = 10000;
			this.textAuditEntries.MinVal = 1;
			this.textAuditEntries.Name = "textAuditEntries";
			this.textAuditEntries.Size = new System.Drawing.Size(74, 20);
			this.textAuditEntries.TabIndex = 225;
			this.textAuditEntries.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(6, 498);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(367, 17);
			this.label9.TabIndex = 224;
			this.label9.Text = "Number of Audit Trail entries displayed";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butClearCode
			// 
			this.butClearCode.Location = new System.Drawing.Point(455, 474);
			this.butClearCode.Name = "butClearCode";
			this.butClearCode.Size = new System.Drawing.Size(43, 21);
			this.butClearCode.TabIndex = 222;
			this.butClearCode.Text = "Clear";
			this.butClearCode.Click += new System.EventHandler(this.butClearCode_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(6, 475);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(367, 17);
			this.label8.TabIndex = 221;
			this.label8.Text = "Sync code for CEMT.  Clear if you want to sync from a different source";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSyncCode
			// 
			this.textSyncCode.Location = new System.Drawing.Point(375, 474);
			this.textSyncCode.Name = "textSyncCode";
			this.textSyncCode.ReadOnly = true;
			this.textSyncCode.Size = new System.Drawing.Size(74, 20);
			this.textSyncCode.TabIndex = 220;
			// 
			// butDecimal
			// 
			this.butDecimal.Location = new System.Drawing.Point(455, 384);
			this.butDecimal.Name = "butDecimal";
			this.butDecimal.Size = new System.Drawing.Size(23, 21);
			this.butDecimal.TabIndex = 219;
			this.butDecimal.Text = "...";
			this.butDecimal.Click += new System.EventHandler(this.butDecimal_Click);
			// 
			// textNumDecimals
			// 
			this.textNumDecimals.Location = new System.Drawing.Point(402, 385);
			this.textNumDecimals.Name = "textNumDecimals";
			this.textNumDecimals.ReadOnly = true;
			this.textNumDecimals.Size = new System.Drawing.Size(47, 20);
			this.textNumDecimals.TabIndex = 217;
			this.textNumDecimals.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(6, 386);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(394, 17);
			this.label7.TabIndex = 218;
			this.label7.Text = "Currency number of digits after decimal";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTrackClinic
			// 
			this.comboTrackClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTrackClinic.FormattingEnabled = true;
			this.comboTrackClinic.Location = new System.Drawing.Point(319, 449);
			this.comboTrackClinic.Name = "comboTrackClinic";
			this.comboTrackClinic.Size = new System.Drawing.Size(130, 21);
			this.comboTrackClinic.TabIndex = 215;
			// 
			// labelTrackClinic
			// 
			this.labelTrackClinic.Location = new System.Drawing.Point(6, 452);
			this.labelTrackClinic.Name = "labelTrackClinic";
			this.labelTrackClinic.Size = new System.Drawing.Size(311, 17);
			this.labelTrackClinic.TabIndex = 216;
			this.labelTrackClinic.Text = "Track Last Clinic By";
			this.labelTrackClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkTimeCardUseLocal
			// 
			this.checkTimeCardUseLocal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTimeCardUseLocal.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTimeCardUseLocal.Location = new System.Drawing.Point(6, 200);
			this.checkTimeCardUseLocal.Name = "checkTimeCardUseLocal";
			this.checkTimeCardUseLocal.Size = new System.Drawing.Size(443, 17);
			this.checkTimeCardUseLocal.TabIndex = 214;
			this.checkTimeCardUseLocal.Text = "Time Cards use local time";
			this.checkTimeCardUseLocal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPickLanguageAndRegion
			// 
			this.butPickLanguageAndRegion.Location = new System.Drawing.Point(455, 362);
			this.butPickLanguageAndRegion.Name = "butPickLanguageAndRegion";
			this.butPickLanguageAndRegion.Size = new System.Drawing.Size(23, 21);
			this.butPickLanguageAndRegion.TabIndex = 206;
			this.butPickLanguageAndRegion.Text = "...";
			this.butPickLanguageAndRegion.Click += new System.EventHandler(this.butPickLanguageAndRegion_Click);
			// 
			// textLanguageAndRegion
			// 
			this.textLanguageAndRegion.Location = new System.Drawing.Point(284, 363);
			this.textLanguageAndRegion.Name = "textLanguageAndRegion";
			this.textLanguageAndRegion.ReadOnly = true;
			this.textLanguageAndRegion.Size = new System.Drawing.Size(165, 20);
			this.textLanguageAndRegion.TabIndex = 204;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(6, 361);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(276, 17);
			this.label6.TabIndex = 205;
			this.label6.Text = "Language and region used by program";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkImeCompositionCompatibility
			// 
			this.checkImeCompositionCompatibility.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkImeCompositionCompatibility.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkImeCompositionCompatibility.Location = new System.Drawing.Point(6, 408);
			this.checkImeCompositionCompatibility.Name = "checkImeCompositionCompatibility";
			this.checkImeCompositionCompatibility.Size = new System.Drawing.Size(443, 17);
			this.checkImeCompositionCompatibility.TabIndex = 203;
			this.checkImeCompositionCompatibility.Text = "Text boxes use foreign language Input Method Editor (IME) composition";
			this.checkImeCompositionCompatibility.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkRefresh
			// 
			this.checkRefresh.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRefresh.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRefresh.Location = new System.Drawing.Point(55, 13);
			this.checkRefresh.Name = "checkRefresh";
			this.checkRefresh.Size = new System.Drawing.Size(382, 17);
			this.checkRefresh.TabIndex = 202;
			this.checkRefresh.Text = "New Computers default to refresh while typing in Select Patient window";
			this.checkRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRefresh.UseVisualStyleBackColor = true;
			// 
			// checkPrefFName
			// 
			this.checkPrefFName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPrefFName.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPrefFName.Location = new System.Drawing.Point(55, 29);
			this.checkPrefFName.Name = "checkPrefFName";
			this.checkPrefFName.Size = new System.Drawing.Size(382, 17);
			this.checkPrefFName.TabIndex = 79;
			this.checkPrefFName.Text = "Search for preferred name in first name field in Select Patient window";
			this.checkPrefFName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInactiveSignal
			// 
			this.textInactiveSignal.Location = new System.Drawing.Point(375, 249);
			this.textInactiveSignal.MaxVal = 1000000;
			this.textInactiveSignal.MinVal = 1;
			this.textInactiveSignal.Name = "textInactiveSignal";
			this.textInactiveSignal.Size = new System.Drawing.Size(74, 20);
			this.textInactiveSignal.TabIndex = 201;
			this.textInactiveSignal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(6, 245);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(367, 27);
			this.label5.TabIndex = 200;
			this.label5.Text = "Disable signal interval after this many minutes of user inactivity\r\nLeave blank t" +
    "o disable";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWebServiceServerName
			// 
			this.textWebServiceServerName.Location = new System.Drawing.Point(284, 427);
			this.textWebServiceServerName.Name = "textWebServiceServerName";
			this.textWebServiceServerName.Size = new System.Drawing.Size(165, 20);
			this.textWebServiceServerName.TabIndex = 197;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 428);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(276, 17);
			this.label2.TabIndex = 198;
			this.label2.Text = "Update Server Name";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(453, 17);
			this.label1.TabIndex = 196;
			this.label1.Text = "See Setup | Module Preferences for setup options that were previously in this win" +
    "dow.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.checkTitleBarShowSpecialty);
			this.groupBox6.Controls.Add(this.checkUseClinicAbbr);
			this.groupBox6.Controls.Add(this.textMainWindowTitle);
			this.groupBox6.Controls.Add(this.checkTitleBarShowSite);
			this.groupBox6.Controls.Add(this.label15);
			this.groupBox6.Controls.Add(this.comboShowID);
			this.groupBox6.Controls.Add(this.label17);
			this.groupBox6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox6.Location = new System.Drawing.Point(12, 24);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(453, 113);
			this.groupBox6.TabIndex = 195;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Main Window Title";
			// 
			// checkTitleBarShowSpecialty
			// 
			this.checkTitleBarShowSpecialty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTitleBarShowSpecialty.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTitleBarShowSpecialty.Location = new System.Drawing.Point(6, 60);
			this.checkTitleBarShowSpecialty.Name = "checkTitleBarShowSpecialty";
			this.checkTitleBarShowSpecialty.Size = new System.Drawing.Size(431, 17);
			this.checkTitleBarShowSpecialty.TabIndex = 234;
			this.checkTitleBarShowSpecialty.Text = "Show patient specialty in main title bar and account patient select";
			this.checkTitleBarShowSpecialty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkUseClinicAbbr
			// 
			this.checkUseClinicAbbr.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseClinicAbbr.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUseClinicAbbr.Location = new System.Drawing.Point(6, 94);
			this.checkUseClinicAbbr.Name = "checkUseClinicAbbr";
			this.checkUseClinicAbbr.Size = new System.Drawing.Size(431, 17);
			this.checkUseClinicAbbr.TabIndex = 233;
			this.checkUseClinicAbbr.Text = "Use clinic abbreviation in main title bar (clinics must be turned on)";
			this.checkUseClinicAbbr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMainWindowTitle
			// 
			this.textMainWindowTitle.Location = new System.Drawing.Point(170, 13);
			this.textMainWindowTitle.Name = "textMainWindowTitle";
			this.textMainWindowTitle.Size = new System.Drawing.Size(267, 20);
			this.textMainWindowTitle.TabIndex = 38;
			// 
			// checkTitleBarShowSite
			// 
			this.checkTitleBarShowSite.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTitleBarShowSite.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTitleBarShowSite.Location = new System.Drawing.Point(6, 77);
			this.checkTitleBarShowSite.Name = "checkTitleBarShowSite";
			this.checkTitleBarShowSite.Size = new System.Drawing.Size(431, 17);
			this.checkTitleBarShowSite.TabIndex = 74;
			this.checkTitleBarShowSite.Text = "Show Site (public health must also be turned on)";
			this.checkTitleBarShowSite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(6, 14);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(163, 17);
			this.label15.TabIndex = 39;
			this.label15.Text = "Title Text";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboShowID
			// 
			this.comboShowID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboShowID.FormattingEnabled = true;
			this.comboShowID.Location = new System.Drawing.Point(307, 35);
			this.comboShowID.Name = "comboShowID";
			this.comboShowID.Size = new System.Drawing.Size(130, 21);
			this.comboShowID.TabIndex = 72;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(6, 37);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(299, 17);
			this.label17.TabIndex = 73;
			this.label17.Text = "Show ID in title bar";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupSelectPatient
			// 
			this.groupSelectPatient.Controls.Add(this.checkShowInactivePatientsDefault);
			this.groupSelectPatient.Controls.Add(this.checkPatientSelectFilterRestrictedClinics);
			this.groupSelectPatient.Controls.Add(this.checkRefresh);
			this.groupSelectPatient.Controls.Add(this.checkPrefFName);
			this.groupSelectPatient.Location = new System.Drawing.Point(12, 602);
			this.groupSelectPatient.Name = "groupSelectPatient";
			this.groupSelectPatient.Size = new System.Drawing.Size(453, 83);
			this.groupSelectPatient.TabIndex = 235;
			this.groupSelectPatient.TabStop = false;
			this.groupSelectPatient.Text = "Select Patient Window";
			// 
			// checkShowInactivePatientsDefault
			// 
			this.checkShowInactivePatientsDefault.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowInactivePatientsDefault.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowInactivePatientsDefault.Location = new System.Drawing.Point(55, 61);
			this.checkShowInactivePatientsDefault.Name = "checkShowInactivePatientsDefault";
			this.checkShowInactivePatientsDefault.Size = new System.Drawing.Size(382, 18);
			this.checkShowInactivePatientsDefault.TabIndex = 292;
			this.checkShowInactivePatientsDefault.Text = "Show Inactive Patients by default";
			this.checkShowInactivePatientsDefault.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPatientSelectFilterRestrictedClinics
			// 
			this.checkPatientSelectFilterRestrictedClinics.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPatientSelectFilterRestrictedClinics.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPatientSelectFilterRestrictedClinics.Location = new System.Drawing.Point(55, 45);
			this.checkPatientSelectFilterRestrictedClinics.Name = "checkPatientSelectFilterRestrictedClinics";
			this.checkPatientSelectFilterRestrictedClinics.Size = new System.Drawing.Size(382, 18);
			this.checkPatientSelectFilterRestrictedClinics.TabIndex = 291;
			this.checkPatientSelectFilterRestrictedClinics.Text = "Hide patients from restricted clinics when viewing \"All\" clinics";
			this.checkPatientSelectFilterRestrictedClinics.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPatientSelectFilterRestrictedClinics.Visible = false;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 340);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(352, 17);
			this.label4.TabIndex = 64;
			this.label4.Text = "Languages used by patients";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butLanguages
			// 
			this.butLanguages.Location = new System.Drawing.Point(360, 337);
			this.butLanguages.Name = "butLanguages";
			this.butLanguages.Size = new System.Drawing.Size(88, 24);
			this.butLanguages.TabIndex = 63;
			this.butLanguages.Text = "Edit Languages";
			this.butLanguages.Click += new System.EventHandler(this.butLanguages_Click);
			// 
			// textSigInterval
			// 
			this.textSigInterval.Location = new System.Drawing.Point(375, 219);
			this.textSigInterval.MaxVal = 1000000;
			this.textSigInterval.MinVal = 1;
			this.textSigInterval.Name = "textSigInterval";
			this.textSigInterval.Size = new System.Drawing.Size(74, 20);
			this.textSigInterval.TabIndex = 57;
			this.textSigInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(455, 700);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 8;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(374, 700);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 7;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 216);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(367, 27);
			this.label3.TabIndex = 56;
			this.label3.Text = "Process signal interval in seconds.  Usually every 6 to 20 seconds\r\nLeave blank t" +
    "o disable autorefresh";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAgingIsEnterprise
			// 
			this.checkAgingIsEnterprise.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAgingIsEnterprise.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAgingIsEnterprise.Location = new System.Drawing.Point(6, 542);
			this.checkAgingIsEnterprise.Name = "checkAgingIsEnterprise";
			this.checkAgingIsEnterprise.Size = new System.Drawing.Size(443, 17);
			this.checkAgingIsEnterprise.TabIndex = 226;
			this.checkAgingIsEnterprise.Text = "Aging for enterprise Galera cluster environments using FamAging table";
			this.checkAgingIsEnterprise.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAgingIsEnterprise.Click += new System.EventHandler(this.checkUseFamAgingTable_Click);
			// 
			// butClearAgingBeginDateT
			// 
			this.butClearAgingBeginDateT.Location = new System.Drawing.Point(455, 563);
			this.butClearAgingBeginDateT.Name = "butClearAgingBeginDateT";
			this.butClearAgingBeginDateT.Size = new System.Drawing.Size(43, 21);
			this.butClearAgingBeginDateT.TabIndex = 229;
			this.butClearAgingBeginDateT.Text = "Clear";
			this.butClearAgingBeginDateT.Click += new System.EventHandler(this.butClearAgingBeginDateT_Click);
			// 
			// labelAgingBeginDateT
			// 
			this.labelAgingBeginDateT.Location = new System.Drawing.Point(6, 560);
			this.labelAgingBeginDateT.Name = "labelAgingBeginDateT";
			this.labelAgingBeginDateT.Size = new System.Drawing.Size(291, 27);
			this.labelAgingBeginDateT.TabIndex = 228;
			this.labelAgingBeginDateT.Text = "DateTime the currently running aging started\r\nUsually blank";
			this.labelAgingBeginDateT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAgingBeginDateT
			// 
			this.textAgingBeginDateT.Location = new System.Drawing.Point(299, 563);
			this.textAgingBeginDateT.Name = "textAgingBeginDateT";
			this.textAgingBeginDateT.ReadOnly = true;
			this.textAgingBeginDateT.Size = new System.Drawing.Size(150, 20);
			this.textAgingBeginDateT.TabIndex = 227;
			this.textAgingBeginDateT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelAutoAgingRunTime
			// 
			this.labelAutoAgingRunTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelAutoAgingRunTime.Location = new System.Drawing.Point(6, 517);
			this.labelAutoAgingRunTime.Name = "labelAutoAgingRunTime";
			this.labelAutoAgingRunTime.Size = new System.Drawing.Size(367, 27);
			this.labelAutoAgingRunTime.TabIndex = 231;
			this.labelAutoAgingRunTime.Text = "Automated aging run time, only applies if using Daily aging \r\nLeave blank to disa" +
    "ble";
			this.labelAutoAgingRunTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAutoAgingRunTime
			// 
			this.textAutoAgingRunTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textAutoAgingRunTime.Location = new System.Drawing.Point(375, 520);
			this.textAutoAgingRunTime.Name = "textAutoAgingRunTime";
			this.textAutoAgingRunTime.Size = new System.Drawing.Size(74, 20);
			this.textAutoAgingRunTime.TabIndex = 230;
			this.textAutoAgingRunTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// checkSubmitExceptions
			// 
			this.checkSubmitExceptions.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSubmitExceptions.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSubmitExceptions.Location = new System.Drawing.Point(6, 587);
			this.checkSubmitExceptions.Name = "checkSubmitExceptions";
			this.checkSubmitExceptions.Size = new System.Drawing.Size(443, 17);
			this.checkSubmitExceptions.TabIndex = 232;
			this.checkSubmitExceptions.Text = "Automatically submit unhandled exceptions";
			this.checkSubmitExceptions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkUserTheme
			// 
			this.checkUserTheme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkUserTheme.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUserTheme.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUserTheme.Location = new System.Drawing.Point(98, 33);
			this.checkUserTheme.Name = "checkUserTheme";
			this.checkUserTheme.Size = new System.Drawing.Size(120, 17);
			this.checkUserTheme.TabIndex = 234;
			this.checkUserTheme.Text = "Change by User";
			this.checkUserTheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUserTheme.UseVisualStyleBackColor = true;
			// 
			// groupTheme
			// 
			this.groupTheme.Controls.Add(this.checkAlternateIcons);
			this.groupTheme.Controls.Add(this.checkUserTheme);
			this.groupTheme.Location = new System.Drawing.Point(231, 143);
			this.groupTheme.Name = "groupTheme";
			this.groupTheme.Size = new System.Drawing.Size(234, 54);
			this.groupTheme.TabIndex = 236;
			this.groupTheme.TabStop = false;
			this.groupTheme.Text = "Module buttons use alternate icons";
			// 
			// checkAlternateIcons
			// 
			this.checkAlternateIcons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkAlternateIcons.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAlternateIcons.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAlternateIcons.Location = new System.Drawing.Point(78, 15);
			this.checkAlternateIcons.Name = "checkAlternateIcons";
			this.checkAlternateIcons.Size = new System.Drawing.Size(140, 17);
			this.checkAlternateIcons.TabIndex = 235;
			this.checkAlternateIcons.Text = "Use Alternate Icons";
			this.checkAlternateIcons.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAlternateIcons.UseVisualStyleBackColor = true;
			// 
			// textInactiveAlert
			// 
			this.textInactiveAlert.Location = new System.Drawing.Point(375, 309);
			this.textInactiveAlert.MaxVal = 1000000;
			this.textInactiveAlert.MinVal = 1;
			this.textInactiveAlert.Name = "textInactiveAlert";
			this.textInactiveAlert.Size = new System.Drawing.Size(74, 20);
			this.textInactiveAlert.TabIndex = 240;
			this.textInactiveAlert.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(6, 305);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(367, 27);
			this.label10.TabIndex = 239;
			this.label10.Text = "Disable alert interval after this many minutes of user inactivity\r\nLeave blank to" +
    " disable";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAlertInterval
			// 
			this.textAlertInterval.Location = new System.Drawing.Point(375, 279);
			this.textAlertInterval.MaxVal = 1000000;
			this.textAlertInterval.MinVal = 1;
			this.textAlertInterval.Name = "textAlertInterval";
			this.textAlertInterval.Size = new System.Drawing.Size(74, 20);
			this.textAlertInterval.TabIndex = 238;
			this.textAlertInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(6, 276);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(367, 27);
			this.label11.TabIndex = 237;
			this.label11.Text = "Check alert interval in seconds.  Leave blank to disable";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormMisc
			// 
			this.ClientSize = new System.Drawing.Size(542, 730);
			this.Controls.Add(this.textInactiveAlert);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.textAlertInterval);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.groupTheme);
			this.Controls.Add(this.groupSelectPatient);
			this.Controls.Add(this.checkSubmitExceptions);
			this.Controls.Add(this.butClearAgingBeginDateT);
			this.Controls.Add(this.labelAgingBeginDateT);
			this.Controls.Add(this.textAgingBeginDateT);
			this.Controls.Add(this.checkAgingIsEnterprise);
			this.Controls.Add(this.textAuditEntries);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.butClearCode);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textSyncCode);
			this.Controls.Add(this.butDecimal);
			this.Controls.Add(this.textNumDecimals);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.comboTrackClinic);
			this.Controls.Add(this.labelTrackClinic);
			this.Controls.Add(this.checkTimeCardUseLocal);
			this.Controls.Add(this.butPickLanguageAndRegion);
			this.Controls.Add(this.textLanguageAndRegion);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.checkImeCompositionCompatibility);
			this.Controls.Add(this.textInactiveSignal);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textWebServiceServerName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butLanguages);
			this.Controls.Add(this.textSigInterval);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.labelAutoAgingRunTime);
			this.Controls.Add(this.textAutoAgingRunTime);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(558, 650);
			this.Name = "FormMisc";
			this.ShowInTaskbar = false;
			this.Text = "Miscellaneous Setup";
			this.Load += new System.EventHandler(this.FormMisc_Load);
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupSelectPatient.ResumeLayout(false);
			this.groupTheme.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormMisc_Load(object sender, System.EventArgs e) {
			if(Prefs.GetLong(PrefName.ProcessSigsIntervalInSecs)==0){
				textSigInterval.Text="";
			}
			else{
				textSigInterval.Text=Prefs.GetLong(PrefName.ProcessSigsIntervalInSecs).ToString();
			}
			if(Prefs.GetLong(PrefName.SignalInactiveMinutes)==0) {
				textInactiveSignal.Text="";
			}
			else {
				textInactiveSignal.Text=Prefs.GetLong(PrefName.SignalInactiveMinutes).ToString();
			}
			checkAlternateIcons.Checked=Prefs.GetBool(PrefName.ColorTheme);
			checkUserTheme.Checked=Prefs.GetBool(PrefName.ThemeSetByUser);
      checkTimeCardUseLocal.Checked=Prefs.GetBool(PrefName.LocalTimeOverridesServerTime);
			checkRefresh.Checked=!Prefs.GetBool(PrefName.PatientSelectUsesSearchButton);
			checkPrefFName.Checked=Prefs.GetBool(PrefName.PatientSelectUseFNameForPreferred);
			if(PrefC.HasClinicsEnabled){
				checkPatientSelectFilterRestrictedClinics.Visible=true;
				checkPatientSelectFilterRestrictedClinics.Checked=Prefs.GetBool(PrefName.PatientSelectFilterRestrictedClinics);
			}
			checkShowInactivePatientsDefault.Checked=Prefs.GetBool(PrefName.PatientSelectShowInactive);
			textMainWindowTitle.Text=Prefs.GetString(PrefName.MainWindowTitle);
			checkUseClinicAbbr.Checked=Prefs.GetBool(PrefName.TitleBarClinicUseAbbr);
			checkTitleBarShowSpecialty.Checked=Prefs.GetBool(PrefName.TitleBarShowSpecialty);
			comboShowID.Items.Add("None");
			comboShowID.Items.Add("PatNum");
			comboShowID.Items.Add("ChartNumber");
			comboShowID.Items.Add("Birthdate");
			comboShowID.SelectedIndex=PrefC.GetInt(PrefName.ShowIDinTitleBar);
			checkImeCompositionCompatibility.Checked=Prefs.GetBool(PrefName.ImeCompositionCompatibility);
			checkTitleBarShowSite.Checked=Prefs.GetBool(PrefName.TitleBarShowSite);
			textWebServiceServerName.Text=Prefs.GetString(PrefName.WebServiceServerName);
			if(Prefs.GetLong(PrefName.AlertCheckFrequencySeconds)==0) {
				textAlertInterval.Text="";
			}
			else {
				textAlertInterval.Text=Prefs.GetString(PrefName.AlertCheckFrequencySeconds);
			}
			if(Prefs.GetLong(PrefName.AlertInactiveMinutes)==0) {
				textInactiveAlert.Text="";
			}
			else {
				textInactiveAlert.Text=Prefs.GetString(PrefName.AlertInactiveMinutes);
			}
			if(Prefs.GetString(PrefName.LanguageAndRegion)!="") {
				textLanguageAndRegion.Text=PrefC.GetLanguageAndRegion().DisplayName;
			}
			else {
				textLanguageAndRegion.Text="None";
			}
			textNumDecimals.Text=CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
			_trackLastClinicBy=new List<string> { "None","Workstation","User" };//must be in english because these values are stored in DB.
			_trackLastClinicBy.ForEach(x => comboTrackClinic.Items.Add(x));//translation is for display only.
			comboTrackClinic.SelectedIndex=_trackLastClinicBy.FindIndex(x => x==Prefs.GetString(PrefName.ClinicTrackLast));
			if(comboTrackClinic.SelectedIndex==-1) {
				comboTrackClinic.SelectedIndex=0;
			}
			if(!PrefC.HasClinicsEnabled) {
				labelTrackClinic.Visible=false;
				comboTrackClinic.Visible=false;
				checkUseClinicAbbr.Visible=false;
			}
			textSyncCode.Text=Prefs.GetString(PrefName.CentralManagerSyncCode);
			textAuditEntries.Text=Prefs.GetString(PrefName.AuditTrailEntriesDisplayed);
			if(Prefs.GetBool(PrefName.AgingIsEnterprise)) {
				//Enterprise
				checkAgingIsEnterprise.Checked=true;
				DateTime agingBeginDateT=PrefC.GetDate(PrefName.AgingBeginDateTime);
				if(agingBeginDateT>DateTime.MinValue) {
					textAgingBeginDateT.Text=agingBeginDateT.ToString();
				}
			}
			else {
				//Classic
				labelAgingBeginDateT.Visible=false;
				textAgingBeginDateT.Visible=false;
				butClearAgingBeginDateT.Visible=false;
			}
			if(Prefs.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)) {
				textAutoAgingRunTime.Enabled=false;
			}
			else {
				DateTime agingDateTDue=PrefC.GetDate(PrefName.AgingServiceTimeDue);
				if(agingDateTDue!=DateTime.MinValue) {
					textAutoAgingRunTime.Text=agingDateTDue.ToShortTimeString();
				}
			}
			checkSubmitExceptions.Checked=Prefs.GetBool(PrefName.SendUnhandledExceptionsToHQ);
		}

		private void butLanguages_Click(object sender,EventArgs e) {
			FormLanguagesUsed FormL=new FormLanguagesUsed();
			FormL.ShowDialog();
			if(FormL.DialogResult==DialogResult.OK){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}

		private void butPickLanguageAndRegion_Click(object sender,EventArgs e) {
			FormLanguageAndRegion FormLAR=new FormLanguageAndRegion();//FormLanguageAndRegion saves pref to DB.
			FormLAR.ShowDialog();
			if(Prefs.GetString(PrefName.LanguageAndRegion)!="") {
				textLanguageAndRegion.Text=PrefC.GetLanguageAndRegion().DisplayName;
			}
			else {
				textLanguageAndRegion.Text="None";
			}
		}

		private void butDecimal_Click(object sender,EventArgs e) {
			FormDecimalSettings FormDS=new FormDecimalSettings();
			FormDS.ShowDialog();
		}

		private void butClearCode_Click(object sender,EventArgs e) {
			textSyncCode.Text="";
		}

		private void checkUseFamAgingTable_Click(object sender,EventArgs e) {
			if(checkAgingIsEnterprise.Checked && !Prefs.GetBool(PrefName.AgingIsEnterprise)) {
				//Enabling feature that is turned off according to pref table.
				string msgTxt="In order to enable enterprise aging, enter the password from our manual below.";
				if(Prefs.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)) {
					//Conditional warning that only affects Monthly Aging customers.
					msgTxt+="\r\n"+"Note: This will change your aging from calculated monthly to calculated daily.";
				}
				InputBox iBox=new InputBox(msgTxt) { Text="Enter Password" };
				iBox.ShowDialog();
				if(iBox.DialogResult!=DialogResult.OK) {
					checkAgingIsEnterprise.Checked=false;
					return;
				}
				if(iBox.textResult.Text!="abracadabra"){//to keep non-enterprise customers from unwittingly enabling this feature 
					checkAgingIsEnterprise.Checked=false;
					MessageBox.Show("Incorrect password.  Refer to the online manual or contact Open Dental support for assistance.");
					return;
				}
			}
		}
		
		///<summary>This button and the associated label and textbox are only visible if AgingIsEnterprise is set in the database.  When the user first
		///enables the AgingIsEnterprise feature, there will be no need to clear the DateTime since it will already be blank.  Once the form closes and
		///AgingIsEnterprise is enabled, the next time the form is accessed they may have the option to clear the DateTime.  Requires SecurityAdmin permission.</summary>
		private void butClearAgingBeginDateT_Click(object sender,EventArgs e) {
			//user doesn't have permission or the pref is already cleared
			if(!Security.IsAuthorized(Permissions.SecurityAdmin) || PrefC.GetDate(PrefName.AgingBeginDateTime)==DateTime.MinValue) {//blank=DateTime.MinValue
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"This will override the lock on the famaging table, potentially allowing a second connection to start "
				+"the aging calculations which could cause both calculations to fail.  If this happens, this flag will need to be cleared again and aging "
				+"started by a single connection.\r\nAre you sure you want to clear this value?"))
			{
				return;
			}
			textAgingBeginDateT.Text="";
			Prefs.Set(PrefName.AgingBeginDateTime,"");
			DataValid.SetInvalid(InvalidType.Prefs);
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textAuditEntries.errorProvider1.GetError(textAuditEntries)!="" || !textAlertInterval.IsValid || !textInactiveAlert.IsValid) {
				MessageBox.Show("Please fix data entry errors first.");
				return;
			}
			if(string.IsNullOrWhiteSpace(textSigInterval.Text) && Prefs.GetLong(PrefName.ProcessSigsIntervalInSecs)!=0) {
				bool proceed=MsgBox.Show(sender,MsgBoxButtons.YesNo,"Disabling the process signal interval prevents the use of kiosks.\r\n"
					+"This should not be done if there are multiple workstations in the office.\r\n"
					+"Proceed?");
				if (!proceed) {
					textSigInterval.Text=Prefs.GetLong(PrefName.ProcessSigsIntervalInSecs).ToString();
					return;
				}
			}
			if(PIn.Long(textSigInterval.Text)>=(5+(PIn.Long(textInactiveSignal.Text)*60)) && PIn.Long(textInactiveSignal.Text)!=0) {//Signal Refresh time is less than or equal to 5 seconds plus the number of seconds in textSigInterval
				string question="The inactive signal time is less than or equal to the signal refresh time."+"\r\n"
					+"This could inadvertently cause signals to not correctly refresh.  Continue?";
				if(MessageBox.Show(question,"",MessageBoxButtons.YesNo)!=DialogResult.Yes) {
					return;
				}
			}
			DateTime autoAgingRunTime=DateTime.MinValue;
			if(!string.IsNullOrWhiteSpace(textAutoAgingRunTime.Text)
				&& !Prefs.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)
				&& !DateTime.TryParse(textAutoAgingRunTime.Text,out autoAgingRunTime))
			{
				MessageBox.Show("Automated Aging Run Time must be blank or a valid time of day.");
				return;
			}
			if(comboTrackClinic.SelectedIndex<0) {
				comboTrackClinic.SelectedIndex=0;
			}
			bool changed=false;
			if( Prefs.Set(PrefName.MainWindowTitle,textMainWindowTitle.Text)
				| Prefs.Set(PrefName.ShowIDinTitleBar,comboShowID.SelectedIndex)
				| Prefs.Set(PrefName.TitleBarShowSite, checkTitleBarShowSite.Checked)
				| Prefs.Set(PrefName.WebServiceServerName,textWebServiceServerName.Text)
        | Prefs.Set(PrefName.LocalTimeOverridesServerTime,checkTimeCardUseLocal.Checked)
				| Prefs.Set(PrefName.PatientSelectUseFNameForPreferred,checkPrefFName.Checked)
				| Prefs.Set(PrefName.PatientSelectUsesSearchButton,!checkRefresh.Checked)
				| Prefs.Set(PrefName.ImeCompositionCompatibility,checkImeCompositionCompatibility.Checked)
				| Prefs.Set(PrefName.ClinicTrackLast,_trackLastClinicBy[comboTrackClinic.SelectedIndex])
				| Prefs.Set(PrefName.CentralManagerSyncCode,textSyncCode.Text)
				| Prefs.Set(PrefName.AuditTrailEntriesDisplayed,textAuditEntries.Text)
				| Prefs.Set(PrefName.AgingIsEnterprise,checkAgingIsEnterprise.Checked)
				| Prefs.Set(PrefName.TitleBarClinicUseAbbr,checkUseClinicAbbr.Checked)
				| Prefs.Set(PrefName.TitleBarShowSpecialty,checkTitleBarShowSpecialty.Checked)
				| Prefs.Set(PrefName.SendUnhandledExceptionsToHQ,checkSubmitExceptions.Checked)
				| Prefs.Set(PrefName.ThemeSetByUser,checkUserTheme.Checked)
				| Prefs.Set(PrefName.PatientSelectFilterRestrictedClinics,checkPatientSelectFilterRestrictedClinics.Checked)
				| Prefs.Set(PrefName.PatientSelectShowInactive,checkShowInactivePatientsDefault.Checked)
			)
			{
				changed=true;
			}
			if(Prefs.Set(PrefName.ColorTheme,checkAlternateIcons.Checked)) {
				changed=true;
			}
			if(textSigInterval.Text==""){
				if(Prefs.Set(PrefName.ProcessSigsIntervalInSecs,0)){
					changed=true;
				}
			}
			else{
				if(Prefs.Set(PrefName.ProcessSigsIntervalInSecs,PIn.Long(textSigInterval.Text))){
					changed=true;
				}
			}
			if(textInactiveSignal.Text=="") {
				if(Prefs.Set(PrefName.SignalInactiveMinutes,0)) {
					changed=true;
				}
			}
			else {
				if(Prefs.Set(PrefName.SignalInactiveMinutes,PIn.Long(textInactiveSignal.Text))) {
					changed=true;
				}
			}
			if(textAlertInterval.Text=="") {
				changed|=Prefs.Set(PrefName.AlertCheckFrequencySeconds,0);
			}
			else {
				changed|=Prefs.Set(PrefName.AlertCheckFrequencySeconds,PIn.Long(textAlertInterval.Text));
			}
			if(textInactiveAlert.Text=="") {
				changed|=Prefs.Set(PrefName.AlertInactiveMinutes,0);
			}
			else {
				changed|=Prefs.Set(PrefName.AlertInactiveMinutes,PIn.Long(textInactiveAlert.Text));
			}
			//if AgingIsEnterprise, change the aging to daily aging if it is currently set to monthly.  AgingIsEnterprise requires daily aging.
			if(checkAgingIsEnterprise.Checked && Prefs.Set(PrefName.AgingCalculatedMonthlyInsteadOfDaily,false)) {
				changed=true;
			}
			if(autoAgingRunTime==DateTime.MinValue) {
				if(Prefs.Set(PrefName.AgingServiceTimeDue,"")) {
					changed=true;
				}
			}
			else {
				if(Prefs.Set(PrefName.AgingServiceTimeDue,POut.DateT(autoAgingRunTime,false))) {
					changed=true;
				}
			}
			if(changed){
				//ComputerPrefs may not need to be invalidated here, since task computer settings moved to FormTaskSetup.  Leaving here for now just in case.
				DataValid.SetInvalid(InvalidType.Prefs, InvalidType.Computers);
				ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}





