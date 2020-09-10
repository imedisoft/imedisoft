namespace Imedisoft.Forms
{
	partial class FormAllergyDefEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAllergyDefEdit));
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.isHiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.medicationLabel = new System.Windows.Forms.Label();
            this.snomedCodeComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.medicationTextBox = new System.Windows.Forms.TextBox();
            this.erhGroupBox = new System.Windows.Forms.GroupBox();
            this.allergenGroupBox = new System.Windows.Forms.GroupBox();
            this.uniiTextBox = new System.Windows.Forms.TextBox();
            this.uniiNoneButton = new OpenDental.UI.Button();
            this.medicationNoneButton = new OpenDental.UI.Button();
            this.uniiSelectButton = new OpenDental.UI.Button();
            this.uniiLabel = new System.Windows.Forms.Label();
            this.medicationSelectButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.erhGroupBox.SuspendLayout();
            this.allergenGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(84, 22);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Description";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(150, 19);
            this.descriptionTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(276, 20);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // isHiddenCheckBox
            // 
            this.isHiddenCheckBox.AutoSize = true;
            this.isHiddenCheckBox.Location = new System.Drawing.Point(150, 45);
            this.isHiddenCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.isHiddenCheckBox.Name = "isHiddenCheckBox";
            this.isHiddenCheckBox.Size = new System.Drawing.Size(71, 17);
            this.isHiddenCheckBox.TabIndex = 2;
            this.isHiddenCheckBox.Text = "Is Hidden";
            this.isHiddenCheckBox.UseVisualStyleBackColor = true;
            // 
            // medicationLabel
            // 
            this.medicationLabel.AutoSize = true;
            this.medicationLabel.Location = new System.Drawing.Point(68, 56);
            this.medicationLabel.Name = "medicationLabel";
            this.medicationLabel.Size = new System.Drawing.Size(58, 13);
            this.medicationLabel.TabIndex = 4;
            this.medicationLabel.Text = "Medication";
            // 
            // snomedCodeComboBox
            // 
            this.snomedCodeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.snomedCodeComboBox.FormattingEnabled = true;
            this.snomedCodeComboBox.Location = new System.Drawing.Point(138, 26);
            this.snomedCodeComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.snomedCodeComboBox.Name = "snomedCodeComboBox";
            this.snomedCodeComboBox.Size = new System.Drawing.Size(340, 21);
            this.snomedCodeComboBox.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(65, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Allergy Type";
            // 
            // medicationTextBox
            // 
            this.medicationTextBox.Location = new System.Drawing.Point(132, 52);
            this.medicationTextBox.Name = "medicationTextBox";
            this.medicationTextBox.ReadOnly = true;
            this.medicationTextBox.Size = new System.Drawing.Size(220, 20);
            this.medicationTextBox.TabIndex = 5;
            // 
            // erhGroupBox
            // 
            this.erhGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.erhGroupBox.Controls.Add(this.allergenGroupBox);
            this.erhGroupBox.Controls.Add(this.snomedCodeComboBox);
            this.erhGroupBox.Controls.Add(this.label3);
            this.erhGroupBox.Location = new System.Drawing.Point(12, 75);
            this.erhGroupBox.Name = "erhGroupBox";
            this.erhGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.erhGroupBox.Size = new System.Drawing.Size(510, 160);
            this.erhGroupBox.TabIndex = 3;
            this.erhGroupBox.TabStop = false;
            this.erhGroupBox.Text = "Only used in EHR for CCDs (most offices can ignore this section)";
            // 
            // allergenGroupBox
            // 
            this.allergenGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.allergenGroupBox.Controls.Add(this.uniiTextBox);
            this.allergenGroupBox.Controls.Add(this.uniiNoneButton);
            this.allergenGroupBox.Controls.Add(this.medicationNoneButton);
            this.allergenGroupBox.Controls.Add(this.uniiSelectButton);
            this.allergenGroupBox.Controls.Add(this.medicationTextBox);
            this.allergenGroupBox.Controls.Add(this.uniiLabel);
            this.allergenGroupBox.Controls.Add(this.medicationSelectButton);
            this.allergenGroupBox.Controls.Add(this.medicationLabel);
            this.allergenGroupBox.Location = new System.Drawing.Point(6, 60);
            this.allergenGroupBox.Name = "allergenGroupBox";
            this.allergenGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.allergenGroupBox.Size = new System.Drawing.Size(498, 94);
            this.allergenGroupBox.TabIndex = 2;
            this.allergenGroupBox.TabStop = false;
            this.allergenGroupBox.Text = "Allergen (only one)";
            // 
            // uniiTextBox
            // 
            this.uniiTextBox.Location = new System.Drawing.Point(132, 26);
            this.uniiTextBox.Name = "uniiTextBox";
            this.uniiTextBox.Size = new System.Drawing.Size(220, 20);
            this.uniiTextBox.TabIndex = 1;
            // 
            // uniiNoneButton
            // 
            this.uniiNoneButton.Enabled = false;
            this.uniiNoneButton.Location = new System.Drawing.Point(389, 26);
            this.uniiNoneButton.Name = "uniiNoneButton";
            this.uniiNoneButton.Size = new System.Drawing.Size(60, 20);
            this.uniiNoneButton.TabIndex = 3;
            this.uniiNoneButton.Text = "None";
            this.uniiNoneButton.Click += new System.EventHandler(this.UniiNoneButton_Click);
            // 
            // medicationNoneButton
            // 
            this.medicationNoneButton.Location = new System.Drawing.Point(389, 52);
            this.medicationNoneButton.Name = "medicationNoneButton";
            this.medicationNoneButton.Size = new System.Drawing.Size(60, 20);
            this.medicationNoneButton.TabIndex = 7;
            this.medicationNoneButton.Text = "None";
            this.medicationNoneButton.Click += new System.EventHandler(this.MedicationNoneButton_Click);
            // 
            // uniiSelectButton
            // 
            this.uniiSelectButton.Enabled = false;
            this.uniiSelectButton.Image = ((System.Drawing.Image)(resources.GetObject("uniiSelectButton.Image")));
            this.uniiSelectButton.Location = new System.Drawing.Point(358, 26);
            this.uniiSelectButton.Name = "uniiSelectButton";
            this.uniiSelectButton.Size = new System.Drawing.Size(25, 20);
            this.uniiSelectButton.TabIndex = 2;
            this.uniiSelectButton.Click += new System.EventHandler(this.UniiSelectButton_Click);
            // 
            // uniiLabel
            // 
            this.uniiLabel.AutoSize = true;
            this.uniiLabel.Location = new System.Drawing.Point(97, 29);
            this.uniiLabel.Name = "uniiLabel";
            this.uniiLabel.Size = new System.Drawing.Size(29, 13);
            this.uniiLabel.TabIndex = 0;
            this.uniiLabel.Text = "UNII";
            // 
            // medicationSelectButton
            // 
            this.medicationSelectButton.Image = ((System.Drawing.Image)(resources.GetObject("medicationSelectButton.Image")));
            this.medicationSelectButton.Location = new System.Drawing.Point(358, 52);
            this.medicationSelectButton.Name = "medicationSelectButton";
            this.medicationSelectButton.Size = new System.Drawing.Size(25, 20);
            this.medicationSelectButton.TabIndex = 6;
            this.medicationSelectButton.Click += new System.EventHandler(this.MedicationSelectButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(442, 264);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(356, 264);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // FormAllergyDefEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(534, 301);
            this.Controls.Add(this.erhGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.isHiddenCheckBox);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.acceptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAllergyDefEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Allergy Definition";
            this.Load += new System.EventHandler(this.FormAllergyEdit_Load);
            this.erhGroupBox.ResumeLayout(false);
            this.erhGroupBox.PerformLayout();
            this.allergenGroupBox.ResumeLayout(false);
            this.allergenGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.CheckBox isHiddenCheckBox;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label medicationLabel;
		private System.Windows.Forms.ComboBox snomedCodeComboBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox medicationTextBox;
		private OpenDental.UI.Button medicationSelectButton;
		private OpenDental.UI.Button medicationNoneButton;
		private System.Windows.Forms.GroupBox erhGroupBox;
		private System.Windows.Forms.Label uniiLabel;
		private System.Windows.Forms.TextBox uniiTextBox;
		private OpenDental.UI.Button uniiSelectButton;
		private OpenDental.UI.Button uniiNoneButton;
		private System.Windows.Forms.GroupBox allergenGroupBox;
	}
}
