namespace OpenDental{
	partial class FormOrthoChart {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOrthoChart));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemSetup = new System.Windows.Forms.MenuItem();
            this.butPrint = new OpenDental.UI.Button();
            this.gridOrtho = new OpenDental.UI.ODGrid();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.signatureBoxWrapper = new OpenDental.UI.SignatureBoxWrapper();
            this.butOK = new OpenDental.UI.Button();
            this.butAudit = new OpenDental.UI.Button();
            this.butUseAutoNote = new OpenDental.UI.Button();
            this.butAdd = new OpenDental.UI.Button();
            this.butCancel = new OpenDental.UI.Button();
            this.gridPat = new OpenDental.UI.ODGrid();
            this.gridMain = new OpenDental.UI.ODGrid();
            this.butChangeUser = new OpenDental.UI.Button();
            this.textUser = new System.Windows.Forms.TextBox();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelPermAlert = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSetup});
            // 
            // menuItemSetup
            // 
            this.menuItemSetup.Index = 0;
            this.menuItemSetup.Text = "Setup";
            this.menuItemSetup.Click += new System.EventHandler(this.menuItemSetup_Click);
            // 
            // butPrint
            // 
            this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butPrint.Image = global::Imedisoft.Properties.Resources.butPrintSmall;
            this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butPrint.Location = new System.Drawing.Point(685, 508);
            this.butPrint.Name = "butPrint";
            this.butPrint.Size = new System.Drawing.Size(85, 24);
            this.butPrint.TabIndex = 118;
            this.butPrint.Text = "&Print";
            this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
            // 
            // gridOrtho
            // 
            this.gridOrtho.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gridOrtho.HeadersVisible = false;
            this.gridOrtho.Location = new System.Drawing.Point(600, 5);
            this.gridOrtho.Name = "gridOrtho";
            this.gridOrtho.SelectionMode = OpenDental.UI.GridSelectionMode.None;
            this.gridOrtho.Size = new System.Drawing.Size(364, 148);
            this.gridOrtho.TabIndex = 112;
            this.gridOrtho.Title = "Ortho Info";
            this.gridOrtho.TranslationName = "TableOrthoInfo";
            this.gridOrtho.Visible = false;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Location = new System.Drawing.Point(10, 274);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(954, 23);
            this.tabControl.TabIndex = 111;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_TabIndexChanged);
            this.tabControl.TabIndexChanged += new System.EventHandler(this.tabControl_TabIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(946, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(946, 0);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // signatureBoxWrapper
            // 
            this.signatureBoxWrapper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.signatureBoxWrapper.BackColor = System.Drawing.SystemColors.ControlDark;
            this.signatureBoxWrapper.Location = new System.Drawing.Point(600, 187);
            this.signatureBoxWrapper.Name = "signatureBoxWrapper";
            this.signatureBoxWrapper.SignatureMode = OpenDental.UI.SignatureBoxWrapper.SigMode.OrthoChart;
            this.signatureBoxWrapper.Size = new System.Drawing.Size(364, 81);
            this.signatureBoxWrapper.TabIndex = 110;
            this.signatureBoxWrapper.UserSig = null;
            this.signatureBoxWrapper.ClearSignatureClicked += new System.EventHandler(this.signatureBoxWrapper_ClearSignatureClicked);
            this.signatureBoxWrapper.SignTopazClicked += new System.EventHandler(this.signatureBoxWrapper_SignTopazClicked);
            // 
            // butOK
            // 
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOK.Location = new System.Drawing.Point(806, 509);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 109;
            this.butOK.Text = "&OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butAudit
            // 
            this.butAudit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butAudit.Location = new System.Drawing.Point(447, 509);
            this.butAudit.Name = "butAudit";
            this.butAudit.Size = new System.Drawing.Size(80, 23);
            this.butAudit.TabIndex = 108;
            this.butAudit.Text = "Audit Trail";
            this.butAudit.Click += new System.EventHandler(this.butAudit_Click);
            // 
            // butUseAutoNote
            // 
            this.butUseAutoNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butUseAutoNote.Location = new System.Drawing.Point(96, 509);
            this.butUseAutoNote.Name = "butUseAutoNote";
            this.butUseAutoNote.Size = new System.Drawing.Size(80, 23);
            this.butUseAutoNote.TabIndex = 107;
            this.butUseAutoNote.Text = "Auto Note";
            this.butUseAutoNote.Click += new System.EventHandler(this.butUseAutoNote_Click);
            // 
            // butAdd
            // 
            this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butAdd.Location = new System.Drawing.Point(10, 509);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(80, 23);
            this.butAdd.TabIndex = 9;
            this.butAdd.Text = "Add Date";
            this.butAdd.UseVisualStyleBackColor = true;
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Location = new System.Drawing.Point(887, 509);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 7;
            this.butCancel.Text = "&Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // gridPat
            // 
            this.gridPat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridPat.Location = new System.Drawing.Point(10, 5);
            this.gridPat.Name = "gridPat";
            this.gridPat.Size = new System.Drawing.Size(582, 263);
            this.gridPat.TabIndex = 6;
            this.gridPat.Title = "Patient Fields";
            this.gridPat.TranslationName = "TablePatientFields";
            this.gridPat.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPat_CellDoubleClick);
            // 
            // gridMain
            // 
            this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMain.HScrollVisible = true;
            this.gridMain.Location = new System.Drawing.Point(10, 299);
            this.gridMain.Name = "gridMain";
            this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
            this.gridMain.Size = new System.Drawing.Size(952, 197);
            this.gridMain.TabIndex = 5;
            this.gridMain.Title = "Ortho Chart";
            this.gridMain.TranslationName = "TableOrthoChart";
            this.gridMain.CellSelectionCommitted += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellSelectionCommitted);
            this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
            this.gridMain.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellLeave);
            this.gridMain.CellEnter += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellEnter);
            // 
            // butChangeUser
            // 
            this.butChangeUser.Location = new System.Drawing.Point(754, 167);
            this.butChangeUser.Name = "butChangeUser";
            this.butChangeUser.Size = new System.Drawing.Size(23, 20);
            this.butChangeUser.TabIndex = 185;
            this.butChangeUser.Text = "...";
            this.butChangeUser.Click += new System.EventHandler(this.butChangeUser_Click);
            // 
            // textUser
            // 
            this.textUser.Location = new System.Drawing.Point(636, 167);
            this.textUser.Name = "textUser";
            this.textUser.ReadOnly = true;
            this.textUser.Size = new System.Drawing.Size(116, 20);
            this.textUser.TabIndex = 184;
            // 
            // labelUser
            // 
            this.labelUser.Location = new System.Drawing.Point(593, 168);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(41, 16);
            this.labelUser.TabIndex = 183;
            this.labelUser.Text = "User";
            this.labelUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPermAlert
            // 
            this.labelPermAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPermAlert.ForeColor = System.Drawing.Color.DarkRed;
            this.labelPermAlert.Location = new System.Drawing.Point(598, 271);
            this.labelPermAlert.Name = "labelPermAlert";
            this.labelPermAlert.Size = new System.Drawing.Size(282, 20);
            this.labelPermAlert.TabIndex = 211;
            this.labelPermAlert.Text = "Notes can only be signed by providers.";
            this.labelPermAlert.Visible = false;
            // 
            // FormOrthoChart
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(974, 540);
            this.Controls.Add(this.labelPermAlert);
            this.Controls.Add(this.butChangeUser);
            this.Controls.Add(this.textUser);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.butPrint);
            this.Controls.Add(this.gridOrtho);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.signatureBoxWrapper);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.butAudit);
            this.Controls.Add(this.butUseAutoNote);
            this.Controls.Add(this.butAdd);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.gridPat);
            this.Controls.Add(this.gridMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(727, 446);
            this.Name = "FormOrthoChart";
            this.Text = "Ortho Chart";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOrthoChart_FormClosing);
            this.Load += new System.EventHandler(this.FormOrthoChart_Load);
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private UI.Button butAdd;
		private UI.Button butCancel;
		private UI.ODGrid gridPat;
		private UI.ODGrid gridMain;
		private UI.Button butUseAutoNote;
		private UI.Button butAudit;
		private UI.Button butOK;
		private UI.SignatureBoxWrapper signatureBoxWrapper;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItemSetup;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private UI.ODGrid gridOrtho;
		private UI.Button butPrint;
		private UI.Button butChangeUser;
		private System.Windows.Forms.TextBox textUser;
		private System.Windows.Forms.Label labelUser;
		private System.Windows.Forms.Label labelPermAlert;
	}
}