namespace OpenDental{
	partial class FormMassEmailSend {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMassEmailSend));
			this.butSendEmails = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.userControlEmailTemplate1 = new OpenDental.UserControlEmailTemplate();
			this.textSubject = new OpenDental.ODtextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.labelSendingPatients = new System.Windows.Forms.Label();
			this.checkDisplay = new System.Windows.Forms.CheckBox();
			this.textPatient = new OpenDental.ODtextBox();
			this.butPatientSelect = new OpenDental.UI.Button();
			this.labelReplacedData = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butSendEmails
			// 
			this.butSendEmails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSendEmails.Location = new System.Drawing.Point(818, 604);
			this.butSendEmails.Name = "butSendEmails";
			this.butSendEmails.Size = new System.Drawing.Size(88, 24);
			this.butSendEmails.TabIndex = 3;
			this.butSendEmails.Text = "Send Emails";
			this.butSendEmails.Click += new System.EventHandler(this.butSendEmails_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(912, 604);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// userControlEmailTemplate1
			// 
			this.userControlEmailTemplate1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.userControlEmailTemplate1.Location = new System.Drawing.Point(12, 89);
			this.userControlEmailTemplate1.Name = "userControlEmailTemplate1";
			this.userControlEmailTemplate1.Size = new System.Drawing.Size(975, 496);
			this.userControlEmailTemplate1.TabIndex = 4;
			// 
			// textSubject
			// 
			this.textSubject.AcceptsTab = true;
			this.textSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textSubject.BackColor = System.Drawing.SystemColors.Control;
			this.textSubject.DetectLinksEnabled = false;
			this.textSubject.DetectUrls = false;
			this.textSubject.Location = new System.Drawing.Point(25, 63);
			this.textSubject.Multiline = false;
			this.textSubject.Name = "textSubject";
			this.textSubject.QuickPasteType = OpenDentBusiness.QuickPasteType.Email;
			this.textSubject.ReadOnly = true;
			this.textSubject.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textSubject.Size = new System.Drawing.Size(470, 20);
			this.textSubject.TabIndex = 101;
			this.textSubject.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(22, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(105, 18);
			this.label2.TabIndex = 100;
			this.label2.Text = "Subject";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelSendingPatients
			// 
			this.labelSendingPatients.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSendingPatients.ForeColor = System.Drawing.Color.LimeGreen;
			this.labelSendingPatients.Location = new System.Drawing.Point(22, 9);
			this.labelSendingPatients.Name = "labelSendingPatients";
			this.labelSendingPatients.Size = new System.Drawing.Size(473, 18);
			this.labelSendingPatients.TabIndex = 102;
			this.labelSendingPatients.Text = "Sending the following email template to ### patients";
			this.labelSendingPatients.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkDisplay
			// 
			this.checkDisplay.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDisplay.Location = new System.Drawing.Point(710, 10);
			this.checkDisplay.Name = "checkDisplay";
			this.checkDisplay.Size = new System.Drawing.Size(277, 18);
			this.checkDisplay.TabIndex = 214;
			this.checkDisplay.Text = "Display rendering with replaced data";
			this.checkDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDisplay.UseVisualStyleBackColor = true;
			this.checkDisplay.Click += new System.EventHandler(this.checkDisplay_Click);
			// 
			// textPatient
			// 
			this.textPatient.AcceptsTab = true;
			this.textPatient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textPatient.BackColor = System.Drawing.SystemColors.Control;
			this.textPatient.DetectLinksEnabled = false;
			this.textPatient.DetectUrls = false;
			this.textPatient.Location = new System.Drawing.Point(794, 34);
			this.textPatient.Multiline = false;
			this.textPatient.Name = "textPatient";
			this.textPatient.QuickPasteType = OpenDentBusiness.QuickPasteType.Email;
			this.textPatient.ReadOnly = true;
			this.textPatient.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textPatient.Size = new System.Drawing.Size(112, 20);
			this.textPatient.TabIndex = 215;
			this.textPatient.Text = "";
			// 
			// butPatientSelect
			// 
			this.butPatientSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPatientSelect.Location = new System.Drawing.Point(912, 33);
			this.butPatientSelect.Name = "butPatientSelect";
			this.butPatientSelect.Size = new System.Drawing.Size(36, 20);
			this.butPatientSelect.TabIndex = 216;
			this.butPatientSelect.Text = "...";
			this.butPatientSelect.Click += new System.EventHandler(this.butPatientSelect_Click);
			// 
			// labelReplacedData
			// 
			this.labelReplacedData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelReplacedData.ForeColor = System.Drawing.Color.LimeGreen;
			this.labelReplacedData.Location = new System.Drawing.Point(791, 57);
			this.labelReplacedData.Name = "labelReplacedData";
			this.labelReplacedData.Size = new System.Drawing.Size(196, 18);
			this.labelReplacedData.TabIndex = 217;
			this.labelReplacedData.Text = "With replaced data";
			this.labelReplacedData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormMassEmailSend
			// 
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(999, 640);
			this.Controls.Add(this.labelReplacedData);
			this.Controls.Add(this.butPatientSelect);
			this.Controls.Add(this.textPatient);
			this.Controls.Add(this.checkDisplay);
			this.Controls.Add(this.labelSendingPatients);
			this.Controls.Add(this.textSubject);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.userControlEmailTemplate1);
			this.Controls.Add(this.butSendEmails);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormMassEmailSend";
			this.Text = "Sending Mass Emails";
			this.Load += new System.EventHandler(this.FormMassEmailSend_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butSendEmails;
		private OpenDental.UI.Button butCancel;
		private UserControlEmailTemplate userControlEmailTemplate1;
		private ODtextBox textSubject;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelSendingPatients;
		private System.Windows.Forms.CheckBox checkDisplay;
		private ODtextBox textPatient;
		private UI.Button butPatientSelect;
		private System.Windows.Forms.Label labelReplacedData;
	}
}