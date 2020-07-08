namespace OpenDental{
	partial class FormEServicesSetup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEServicesSetup));
			this.butCancel = new OpenDental.UI.Button();
			this.butTextingServices = new OpenDental.UI.Button();
			this.butWebSched = new OpenDental.UI.Button();
			this.butMassEmail = new OpenDental.UI.Button();
			this.butPatPortal = new OpenDental.UI.Button();
			this.butMobileWeb = new OpenDental.UI.Button();
			this.butMisc = new OpenDental.UI.Button();
			this.butEConnector = new OpenDental.UI.Button();
			this.butEClipboard = new OpenDental.UI.Button();
			this.butECR = new OpenDental.UI.Button();
			this.butSignup = new OpenDental.UI.Button();
			this.label23 = new System.Windows.Forms.Label();
			this.labelECR = new System.Windows.Forms.Label();
			this.labelSignup = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(433, 360);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 12;
			this.butCancel.Text = "Close";
			this.butCancel.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butTextingServices
			// 
			this.butTextingServices.Location = new System.Drawing.Point(19, 217);
			this.butTextingServices.Name = "butTextingServices";
			this.butTextingServices.Size = new System.Drawing.Size(130, 24);
			this.butTextingServices.TabIndex = 6;
			this.butTextingServices.Text = "Texting Services";
			this.butTextingServices.UseVisualStyleBackColor = true;
			this.butTextingServices.Click += new System.EventHandler(this.butTextingServices_Click);
			// 
			// butWebSched
			// 
			this.butWebSched.Location = new System.Drawing.Point(19, 186);
			this.butWebSched.Name = "butWebSched";
			this.butWebSched.Size = new System.Drawing.Size(130, 24);
			this.butWebSched.TabIndex = 5;
			this.butWebSched.Text = "Web Sched";
			this.butWebSched.UseVisualStyleBackColor = true;
			this.butWebSched.Click += new System.EventHandler(this.butWebSched_Click);
			// 
			// butMassEmail
			// 
			this.butMassEmail.Location = new System.Drawing.Point(19, 339);
			this.butMassEmail.Name = "butMassEmail";
			this.butMassEmail.Size = new System.Drawing.Size(130, 24);
			this.butMassEmail.TabIndex = 10;
			this.butMassEmail.Text = "Mass Email";
			this.butMassEmail.UseVisualStyleBackColor = true;
			this.butMassEmail.Click += new System.EventHandler(this.butMassEmail_Click);
			// 
			// butPatPortal
			// 
			this.butPatPortal.Location = new System.Drawing.Point(19, 155);
			this.butPatPortal.Name = "butPatPortal";
			this.butPatPortal.Size = new System.Drawing.Size(130, 24);
			this.butPatPortal.TabIndex = 4;
			this.butPatPortal.Text = "Patient Portal";
			this.butPatPortal.UseVisualStyleBackColor = true;
			this.butPatPortal.Click += new System.EventHandler(this.butPatPortal_Click);
			// 
			// butMobileWeb
			// 
			this.butMobileWeb.Location = new System.Drawing.Point(19, 124);
			this.butMobileWeb.Name = "butMobileWeb";
			this.butMobileWeb.Size = new System.Drawing.Size(130, 24);
			this.butMobileWeb.TabIndex = 3;
			this.butMobileWeb.Text = "Mobile Web";
			this.butMobileWeb.UseVisualStyleBackColor = true;
			this.butMobileWeb.Click += new System.EventHandler(this.butMobileWeb_Click);
			// 
			// butMisc
			// 
			this.butMisc.Location = new System.Drawing.Point(19, 308);
			this.butMisc.Name = "butMisc";
			this.butMisc.Size = new System.Drawing.Size(130, 24);
			this.butMisc.TabIndex = 9;
			this.butMisc.Text = "Miscellaneous";
			this.butMisc.UseVisualStyleBackColor = true;
			this.butMisc.Click += new System.EventHandler(this.butMisc_Click);
			// 
			// butEConnector
			// 
			this.butEConnector.Location = new System.Drawing.Point(19, 93);
			this.butEConnector.Name = "butEConnector";
			this.butEConnector.Size = new System.Drawing.Size(130, 24);
			this.butEConnector.TabIndex = 2;
			this.butEConnector.Text = "eConnector Service";
			this.butEConnector.UseVisualStyleBackColor = true;
			this.butEConnector.Click += new System.EventHandler(this.butEConnector_Click);
			// 
			// butEClipboard
			// 
			this.butEClipboard.Location = new System.Drawing.Point(19, 277);
			this.butEClipboard.Name = "butEClipboard";
			this.butEClipboard.Size = new System.Drawing.Size(130, 24);
			this.butEClipboard.TabIndex = 8;
			this.butEClipboard.Text = "eClipboard";
			this.butEClipboard.UseVisualStyleBackColor = true;
			this.butEClipboard.Click += new System.EventHandler(this.butEClipboard_Click);
			// 
			// butECR
			// 
			this.butECR.Location = new System.Drawing.Point(19, 247);
			this.butECR.Name = "butECR";
			this.butECR.Size = new System.Drawing.Size(130, 24);
			this.butECR.TabIndex = 7;
			this.butECR.Text = "Automated Messaging";
			this.butECR.UseVisualStyleBackColor = true;
			this.butECR.Click += new System.EventHandler(this.butECR_Click);
			// 
			// butSignup
			// 
			this.butSignup.Location = new System.Drawing.Point(19, 63);
			this.butSignup.Name = "butSignup";
			this.butSignup.Size = new System.Drawing.Size(130, 24);
			this.butSignup.TabIndex = 1;
			this.butSignup.Text = "Signup";
			this.butSignup.UseVisualStyleBackColor = true;
			this.butSignup.Click += new System.EventHandler(this.butSignup_Click);
			// 
			// label23
			// 
			this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label23.Location = new System.Drawing.Point(9, 9);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(499, 26);
			this.label23.TabIndex = 245;
			this.label23.Text = "eServices refer to Open Dental features that can be delivered electronically via " +
    "the internet.  \r\nAll eServices hosted by Open Dental use the eConnector Service." +
    "";
			this.label23.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// labelECR
			// 
			this.labelECR.Location = new System.Drawing.Point(155, 251);
			this.labelECR.Name = "labelECR";
			this.labelECR.Size = new System.Drawing.Size(353, 16);
			this.labelECR.TabIndex = 247;
			this.labelECR.Text = "Automated eReminders, eConfirmations, Thank-Yous, && Arrivals";
			this.labelECR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelSignup
			// 
			this.labelSignup.Location = new System.Drawing.Point(155, 67);
			this.labelSignup.Name = "labelSignup";
			this.labelSignup.Size = new System.Drawing.Size(87, 16);
			this.labelSignup.TabIndex = 246;
			this.labelSignup.Text = "Get Started Here";
			this.labelSignup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormEServicesSetup
			// 
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(520, 396);
			this.Controls.Add(this.labelECR);
			this.Controls.Add(this.labelSignup);
			this.Controls.Add(this.butSignup);
			this.Controls.Add(this.label23);
			this.Controls.Add(this.butTextingServices);
			this.Controls.Add(this.butWebSched);
			this.Controls.Add(this.butMassEmail);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butPatPortal);
			this.Controls.Add(this.butEConnector);
			this.Controls.Add(this.butMobileWeb);
			this.Controls.Add(this.butECR);
			this.Controls.Add(this.butMisc);
			this.Controls.Add(this.butEClipboard);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormEServicesSetup";
			this.Text = "eServices Setup";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEServicesSetup2_FormClosing);
			this.Load += new System.EventHandler(this.FormEServicesSetup2_Load);
			this.ResumeLayout(false);

		}

		#endregion
		private OpenDental.UI.Button butCancel;
		private UI.Button butTextingServices;
		private UI.Button butWebSched;
		private UI.Button butPatPortal;
		private UI.Button butMobileWeb;
		private UI.Button butEConnector;
		private UI.Button butSignup;
		private UI.Button butMassEmail;
		private UI.Button butMisc;
		private UI.Button butEClipboard;
		private UI.Button butECR;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label labelECR;
		private System.Windows.Forms.Label labelSignup;
	}
}