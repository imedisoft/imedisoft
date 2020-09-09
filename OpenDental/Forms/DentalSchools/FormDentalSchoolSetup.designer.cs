namespace Imedisoft.Forms
{
	partial class FormDentalSchoolSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDentalSchoolSetup));
            this.studentsLabel = new System.Windows.Forms.Label();
            this.instructorsLabel = new System.Windows.Forms.Label();
            this.studentsTextBox = new System.Windows.Forms.TextBox();
            this.instructorsTextBox = new System.Windows.Forms.TextBox();
            this.userGroupsInfoLabel = new System.Windows.Forms.Label();
            this.userGroupsGroupBox = new System.Windows.Forms.GroupBox();
            this.studentsButton = new OpenDental.UI.Button();
            this.instructorsButton = new OpenDental.UI.Button();
            this.gradingScalesButton = new OpenDental.UI.Button();
            this.evaluationsButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.userGroupsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // studentsLabel
            // 
            this.studentsLabel.AutoSize = true;
            this.studentsLabel.Location = new System.Drawing.Point(62, 69);
            this.studentsLabel.Name = "studentsLabel";
            this.studentsLabel.Size = new System.Drawing.Size(50, 13);
            this.studentsLabel.TabIndex = 1;
            this.studentsLabel.Text = "Students";
            // 
            // instructorsLabel
            // 
            this.instructorsLabel.AutoSize = true;
            this.instructorsLabel.Location = new System.Drawing.Point(52, 95);
            this.instructorsLabel.Name = "instructorsLabel";
            this.instructorsLabel.Size = new System.Drawing.Size(60, 13);
            this.instructorsLabel.TabIndex = 4;
            this.instructorsLabel.Text = "Instructors";
            // 
            // studentsTextBox
            // 
            this.studentsTextBox.Location = new System.Drawing.Point(118, 66);
            this.studentsTextBox.MaxLength = 255;
            this.studentsTextBox.Name = "studentsTextBox";
            this.studentsTextBox.ReadOnly = true;
            this.studentsTextBox.Size = new System.Drawing.Size(180, 20);
            this.studentsTextBox.TabIndex = 2;
            // 
            // instructorsTextBox
            // 
            this.instructorsTextBox.Location = new System.Drawing.Point(118, 92);
            this.instructorsTextBox.MaxLength = 255;
            this.instructorsTextBox.Name = "instructorsTextBox";
            this.instructorsTextBox.ReadOnly = true;
            this.instructorsTextBox.Size = new System.Drawing.Size(180, 20);
            this.instructorsTextBox.TabIndex = 5;
            // 
            // userGroupsInfoLabel
            // 
            this.userGroupsInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userGroupsInfoLabel.Location = new System.Drawing.Point(6, 23);
            this.userGroupsInfoLabel.Name = "userGroupsInfoLabel";
            this.userGroupsInfoLabel.Size = new System.Drawing.Size(368, 40);
            this.userGroupsInfoLabel.TabIndex = 0;
            this.userGroupsInfoLabel.Text = "Selecting a new user group gives you the opportunity to update all current studen" +
    "ts or instructors to the new group.";
            // 
            // userGroupsGroupBox
            // 
            this.userGroupsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userGroupsGroupBox.Controls.Add(this.studentsLabel);
            this.userGroupsGroupBox.Controls.Add(this.studentsButton);
            this.userGroupsGroupBox.Controls.Add(this.instructorsLabel);
            this.userGroupsGroupBox.Controls.Add(this.userGroupsInfoLabel);
            this.userGroupsGroupBox.Controls.Add(this.instructorsButton);
            this.userGroupsGroupBox.Controls.Add(this.instructorsTextBox);
            this.userGroupsGroupBox.Controls.Add(this.studentsTextBox);
            this.userGroupsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.userGroupsGroupBox.Name = "userGroupsGroupBox";
            this.userGroupsGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.userGroupsGroupBox.Size = new System.Drawing.Size(380, 140);
            this.userGroupsGroupBox.TabIndex = 0;
            this.userGroupsGroupBox.TabStop = false;
            this.userGroupsGroupBox.Text = "Default User Groups";
            // 
            // studentsButton
            // 
            this.studentsButton.Image = ((System.Drawing.Image)(resources.GetObject("studentsButton.Image")));
            this.studentsButton.Location = new System.Drawing.Point(304, 66);
            this.studentsButton.Name = "studentsButton";
            this.studentsButton.Size = new System.Drawing.Size(25, 20);
            this.studentsButton.TabIndex = 3;
            this.studentsButton.Click += new System.EventHandler(this.StudentButton_Click);
            // 
            // instructorsButton
            // 
            this.instructorsButton.Image = ((System.Drawing.Image)(resources.GetObject("instructorsButton.Image")));
            this.instructorsButton.Location = new System.Drawing.Point(304, 92);
            this.instructorsButton.Name = "instructorsButton";
            this.instructorsButton.Size = new System.Drawing.Size(25, 20);
            this.instructorsButton.TabIndex = 6;
            this.instructorsButton.Click += new System.EventHandler(this.InstructorsButton_Click);
            // 
            // gradingScalesButton
            // 
            this.gradingScalesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gradingScalesButton.Location = new System.Drawing.Point(138, 174);
            this.gradingScalesButton.Name = "gradingScalesButton";
            this.gradingScalesButton.Size = new System.Drawing.Size(120, 25);
            this.gradingScalesButton.TabIndex = 2;
            this.gradingScalesButton.Text = "Grading Scales";
            this.gradingScalesButton.Click += new System.EventHandler(this.GradingScalesButton_Click);
            // 
            // evaluationsButton
            // 
            this.evaluationsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.evaluationsButton.Location = new System.Drawing.Point(12, 174);
            this.evaluationsButton.Name = "evaluationsButton";
            this.evaluationsButton.Size = new System.Drawing.Size(120, 25);
            this.evaluationsButton.TabIndex = 1;
            this.evaluationsButton.Text = "Evaluations";
            this.evaluationsButton.Click += new System.EventHandler(this.EvaluationsButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(312, 174);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Close";
            // 
            // FormDentalSchoolSetup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(404, 211);
            this.Controls.Add(this.userGroupsGroupBox);
            this.Controls.Add(this.gradingScalesButton);
            this.Controls.Add(this.evaluationsButton);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDentalSchoolSetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dental School Setup";
            this.Load += new System.EventHandler(this.FormDentalSchoolSetup_Load);
            this.userGroupsGroupBox.ResumeLayout(false);
            this.userGroupsGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label studentsLabel;
		private OpenDental.UI.Button studentsButton;
		private System.Windows.Forms.Label instructorsLabel;
		private OpenDental.UI.Button instructorsButton;
		private System.Windows.Forms.TextBox studentsTextBox;
		private System.Windows.Forms.TextBox instructorsTextBox;
		private System.Windows.Forms.Label userGroupsInfoLabel;
		private OpenDental.UI.Button evaluationsButton;
		private OpenDental.UI.Button gradingScalesButton;
		private System.Windows.Forms.GroupBox userGroupsGroupBox;
	}
}
