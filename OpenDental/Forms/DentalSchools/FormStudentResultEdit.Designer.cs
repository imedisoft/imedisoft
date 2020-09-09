namespace Imedisoft.Forms
{
    partial class FormStudentResultEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStudentResultEdit));
            this.studentLabel = new System.Windows.Forms.Label();
            this.studentTextBox = new System.Windows.Forms.TextBox();
            this.appointmentTextBox = new System.Windows.Forms.TextBox();
            this.appointmentLabel = new System.Windows.Forms.Label();
            this.patientTextBox = new System.Windows.Forms.TextBox();
            this.patientLabel = new System.Windows.Forms.Label();
            this.completionDateLabel = new System.Windows.Forms.Label();
            this.completionDateTextBox = new System.Windows.Forms.TextBox();
            this.instructorComboBox = new System.Windows.Forms.ComboBox();
            this.instructorLabel = new System.Windows.Forms.Label();
            this.completionDateButton = new OpenDental.UI.Button();
            this.patientDetachButton = new OpenDental.UI.Button();
            this.appointmentDetachButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.requirementTextBox = new System.Windows.Forms.TextBox();
            this.requirementLabel = new System.Windows.Forms.Label();
            this.courseTextBox = new System.Windows.Forms.TextBox();
            this.courseLabel = new System.Windows.Forms.Label();
            this.patientSelectButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // studentLabel
            // 
            this.studentLabel.AutoSize = true;
            this.studentLabel.Location = new System.Drawing.Point(89, 15);
            this.studentLabel.Name = "studentLabel";
            this.studentLabel.Size = new System.Drawing.Size(45, 13);
            this.studentLabel.TabIndex = 0;
            this.studentLabel.Text = "Student";
            // 
            // studentTextBox
            // 
            this.studentTextBox.Location = new System.Drawing.Point(140, 12);
            this.studentTextBox.Name = "studentTextBox";
            this.studentTextBox.ReadOnly = true;
            this.studentTextBox.Size = new System.Drawing.Size(320, 20);
            this.studentTextBox.TabIndex = 1;
            // 
            // appointmentTextBox
            // 
            this.appointmentTextBox.Location = new System.Drawing.Point(140, 142);
            this.appointmentTextBox.Name = "appointmentTextBox";
            this.appointmentTextBox.ReadOnly = true;
            this.appointmentTextBox.Size = new System.Drawing.Size(320, 20);
            this.appointmentTextBox.TabIndex = 14;
            // 
            // appointmentLabel
            // 
            this.appointmentLabel.AutoSize = true;
            this.appointmentLabel.Location = new System.Drawing.Point(66, 145);
            this.appointmentLabel.Name = "appointmentLabel";
            this.appointmentLabel.Size = new System.Drawing.Size(68, 13);
            this.appointmentLabel.TabIndex = 13;
            this.appointmentLabel.Text = "Appointment";
            // 
            // patientTextBox
            // 
            this.patientTextBox.Location = new System.Drawing.Point(140, 116);
            this.patientTextBox.Name = "patientTextBox";
            this.patientTextBox.ReadOnly = true;
            this.patientTextBox.Size = new System.Drawing.Size(320, 20);
            this.patientTextBox.TabIndex = 10;
            // 
            // patientLabel
            // 
            this.patientLabel.AutoSize = true;
            this.patientLabel.Location = new System.Drawing.Point(93, 119);
            this.patientLabel.Name = "patientLabel";
            this.patientLabel.Size = new System.Drawing.Size(41, 13);
            this.patientLabel.TabIndex = 9;
            this.patientLabel.Text = "Patient";
            // 
            // completionDateLabel
            // 
            this.completionDateLabel.AutoSize = true;
            this.completionDateLabel.Location = new System.Drawing.Point(50, 93);
            this.completionDateLabel.Name = "completionDateLabel";
            this.completionDateLabel.Size = new System.Drawing.Size(84, 13);
            this.completionDateLabel.TabIndex = 6;
            this.completionDateLabel.Text = "Date Completed";
            // 
            // completionDateTextBox
            // 
            this.completionDateTextBox.Location = new System.Drawing.Point(140, 90);
            this.completionDateTextBox.Name = "completionDateTextBox";
            this.completionDateTextBox.ReadOnly = true;
            this.completionDateTextBox.Size = new System.Drawing.Size(100, 20);
            this.completionDateTextBox.TabIndex = 7;
            // 
            // instructorComboBox
            // 
            this.instructorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.instructorComboBox.FormattingEnabled = true;
            this.instructorComboBox.Location = new System.Drawing.Point(140, 168);
            this.instructorComboBox.MaxDropDownItems = 25;
            this.instructorComboBox.Name = "instructorComboBox";
            this.instructorComboBox.Size = new System.Drawing.Size(200, 21);
            this.instructorComboBox.TabIndex = 17;
            // 
            // instructorLabel
            // 
            this.instructorLabel.AutoSize = true;
            this.instructorLabel.Location = new System.Drawing.Point(79, 171);
            this.instructorLabel.Name = "instructorLabel";
            this.instructorLabel.Size = new System.Drawing.Size(55, 13);
            this.instructorLabel.TabIndex = 16;
            this.instructorLabel.Text = "Instructor";
            // 
            // completionDateButton
            // 
            this.completionDateButton.Enabled = false;
            this.completionDateButton.Location = new System.Drawing.Point(246, 90);
            this.completionDateButton.Name = "completionDateButton";
            this.completionDateButton.Size = new System.Drawing.Size(60, 20);
            this.completionDateButton.TabIndex = 8;
            this.completionDateButton.Text = "Now";
            this.completionDateButton.Click += new System.EventHandler(this.CompletionDateButton_Click);
            // 
            // patientDetachButton
            // 
            this.patientDetachButton.Enabled = false;
            this.patientDetachButton.Image = ((System.Drawing.Image)(resources.GetObject("patientDetachButton.Image")));
            this.patientDetachButton.Location = new System.Drawing.Point(466, 116);
            this.patientDetachButton.Name = "patientDetachButton";
            this.patientDetachButton.Size = new System.Drawing.Size(25, 20);
            this.patientDetachButton.TabIndex = 11;
            this.patientDetachButton.Click += new System.EventHandler(this.PatientDetachButton_Click);
            // 
            // appointmentDetachButton
            // 
            this.appointmentDetachButton.Enabled = false;
            this.appointmentDetachButton.Image = ((System.Drawing.Image)(resources.GetObject("appointmentDetachButton.Image")));
            this.appointmentDetachButton.Location = new System.Drawing.Point(466, 142);
            this.appointmentDetachButton.Name = "appointmentDetachButton";
            this.appointmentDetachButton.Size = new System.Drawing.Size(25, 20);
            this.appointmentDetachButton.TabIndex = 15;
            this.appointmentDetachButton.Click += new System.EventHandler(this.AppointmentDetachButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Enabled = false;
            this.acceptButton.Location = new System.Drawing.Point(366, 234);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 18;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(452, 234);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 19;
            this.cancelButton.Text = "&Cancel";
            // 
            // requirementTextBox
            // 
            this.requirementTextBox.Location = new System.Drawing.Point(140, 64);
            this.requirementTextBox.Name = "requirementTextBox";
            this.requirementTextBox.ReadOnly = true;
            this.requirementTextBox.Size = new System.Drawing.Size(320, 20);
            this.requirementTextBox.TabIndex = 5;
            // 
            // requirementLabel
            // 
            this.requirementLabel.AutoSize = true;
            this.requirementLabel.Location = new System.Drawing.Point(66, 67);
            this.requirementLabel.Name = "requirementLabel";
            this.requirementLabel.Size = new System.Drawing.Size(68, 13);
            this.requirementLabel.TabIndex = 4;
            this.requirementLabel.Text = "Requirement";
            // 
            // courseTextBox
            // 
            this.courseTextBox.Location = new System.Drawing.Point(140, 38);
            this.courseTextBox.Name = "courseTextBox";
            this.courseTextBox.ReadOnly = true;
            this.courseTextBox.Size = new System.Drawing.Size(320, 20);
            this.courseTextBox.TabIndex = 3;
            // 
            // courseLabel
            // 
            this.courseLabel.AutoSize = true;
            this.courseLabel.Location = new System.Drawing.Point(93, 41);
            this.courseLabel.Name = "courseLabel";
            this.courseLabel.Size = new System.Drawing.Size(41, 13);
            this.courseLabel.TabIndex = 2;
            this.courseLabel.Text = "Course";
            // 
            // patientSelectButton
            // 
            this.patientSelectButton.Enabled = false;
            this.patientSelectButton.Image = global::Imedisoft.Properties.Resources.IconEllipsisSmall;
            this.patientSelectButton.Location = new System.Drawing.Point(497, 116);
            this.patientSelectButton.Name = "patientSelectButton";
            this.patientSelectButton.Size = new System.Drawing.Size(25, 20);
            this.patientSelectButton.TabIndex = 12;
            this.patientSelectButton.Click += new System.EventHandler(this.PatientSelectButton_Click);
            // 
            // FormReqStudentEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(544, 271);
            this.Controls.Add(this.patientSelectButton);
            this.Controls.Add(this.courseTextBox);
            this.Controls.Add(this.courseLabel);
            this.Controls.Add(this.requirementTextBox);
            this.Controls.Add(this.completionDateButton);
            this.Controls.Add(this.completionDateTextBox);
            this.Controls.Add(this.requirementLabel);
            this.Controls.Add(this.completionDateLabel);
            this.Controls.Add(this.instructorComboBox);
            this.Controls.Add(this.instructorLabel);
            this.Controls.Add(this.patientDetachButton);
            this.Controls.Add(this.patientTextBox);
            this.Controls.Add(this.patientLabel);
            this.Controls.Add(this.appointmentDetachButton);
            this.Controls.Add(this.appointmentTextBox);
            this.Controls.Add(this.appointmentLabel);
            this.Controls.Add(this.studentTextBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.studentLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormReqStudentEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Student Result";
            this.Load += new System.EventHandler(this.FormStudentResultEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.Label studentLabel;
		private System.Windows.Forms.TextBox studentTextBox;
		private System.Windows.Forms.TextBox appointmentTextBox;
		private System.Windows.Forms.Label appointmentLabel;
		private OpenDental.UI.Button appointmentDetachButton;
		private OpenDental.UI.Button patientDetachButton;
		private System.Windows.Forms.TextBox patientTextBox;
		private System.Windows.Forms.Label patientLabel;
		private System.Windows.Forms.Label completionDateLabel;
		private System.Windows.Forms.TextBox completionDateTextBox;
		private System.Windows.Forms.ComboBox instructorComboBox;
		private System.Windows.Forms.Label instructorLabel;
		private OpenDental.UI.Button completionDateButton;
		private System.Windows.Forms.TextBox requirementTextBox;
		private System.Windows.Forms.Label requirementLabel;
		private System.Windows.Forms.TextBox courseTextBox;
		private System.Windows.Forms.Label courseLabel;
		private OpenDental.UI.Button patientSelectButton;
	}
}
