namespace Imedisoft.Forms
{
    partial class FormProblemDefinitionEdit
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
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.hiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.icd9TextBox = new System.Windows.Forms.TextBox();
            this.icd9Label = new System.Windows.Forms.Label();
            this.snomedTextBox = new System.Windows.Forms.TextBox();
            this.snomedLabel = new System.Windows.Forms.Label();
            this.snomedButton = new OpenDental.UI.Button();
            this.icd9Button = new OpenDental.UI.Button();
            this.icd10Button = new OpenDental.UI.Button();
            this.icd10Label = new System.Windows.Forms.Label();
            this.icd10TextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(392, 164);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(306, 164);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Location = new System.Drawing.Point(120, 90);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(352, 20);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // hiddenCheckBox
            // 
            this.hiddenCheckBox.AutoSize = true;
            this.hiddenCheckBox.Location = new System.Drawing.Point(120, 116);
            this.hiddenCheckBox.Name = "hiddenCheckBox";
            this.hiddenCheckBox.Size = new System.Drawing.Size(59, 17);
            this.hiddenCheckBox.TabIndex = 2;
            this.hiddenCheckBox.Text = "Hidden";
            this.hiddenCheckBox.UseVisualStyleBackColor = true;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(54, 93);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Description";
            // 
            // icd9TextBox
            // 
            this.icd9TextBox.Location = new System.Drawing.Point(120, 12);
            this.icd9TextBox.Name = "icd9TextBox";
            this.icd9TextBox.ReadOnly = true;
            this.icd9TextBox.Size = new System.Drawing.Size(280, 20);
            this.icd9TextBox.TabIndex = 6;
            this.icd9TextBox.TabStop = false;
            // 
            // icd9Label
            // 
            this.icd9Label.AutoSize = true;
            this.icd9Label.Location = new System.Drawing.Point(51, 15);
            this.icd9Label.Name = "icd9Label";
            this.icd9Label.Size = new System.Drawing.Size(63, 13);
            this.icd9Label.TabIndex = 5;
            this.icd9Label.Text = "ICD-9 Code";
            // 
            // snomedTextBox
            // 
            this.snomedTextBox.Location = new System.Drawing.Point(120, 64);
            this.snomedTextBox.Name = "snomedTextBox";
            this.snomedTextBox.ReadOnly = true;
            this.snomedTextBox.Size = new System.Drawing.Size(280, 20);
            this.snomedTextBox.TabIndex = 12;
            this.snomedTextBox.TabStop = false;
            // 
            // snomedLabel
            // 
            this.snomedLabel.AutoSize = true;
            this.snomedLabel.Location = new System.Drawing.Point(21, 67);
            this.snomedLabel.Name = "snomedLabel";
            this.snomedLabel.Size = new System.Drawing.Size(93, 13);
            this.snomedLabel.TabIndex = 11;
            this.snomedLabel.Text = "SNOMED CT Code";
            // 
            // snomedButton
            // 
            this.snomedButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.snomedButton.Location = new System.Drawing.Point(406, 64);
            this.snomedButton.Name = "snomedButton";
            this.snomedButton.Size = new System.Drawing.Size(25, 20);
            this.snomedButton.TabIndex = 13;
            this.snomedButton.Text = "...";
            this.snomedButton.Click += new System.EventHandler(this.SnomedButton_Click);
            // 
            // icd9Button
            // 
            this.icd9Button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.icd9Button.Location = new System.Drawing.Point(406, 12);
            this.icd9Button.Name = "icd9Button";
            this.icd9Button.Size = new System.Drawing.Size(25, 20);
            this.icd9Button.TabIndex = 7;
            this.icd9Button.Text = "...";
            this.icd9Button.Click += new System.EventHandler(this.Icd9Button_Click);
            // 
            // icd10Button
            // 
            this.icd10Button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.icd10Button.Location = new System.Drawing.Point(406, 38);
            this.icd10Button.Name = "icd10Button";
            this.icd10Button.Size = new System.Drawing.Size(25, 20);
            this.icd10Button.TabIndex = 10;
            this.icd10Button.Text = "...";
            this.icd10Button.Click += new System.EventHandler(this.Icd10Button_Click);
            // 
            // icd10Label
            // 
            this.icd10Label.AutoSize = true;
            this.icd10Label.Location = new System.Drawing.Point(45, 41);
            this.icd10Label.Name = "icd10Label";
            this.icd10Label.Size = new System.Drawing.Size(69, 13);
            this.icd10Label.TabIndex = 8;
            this.icd10Label.Text = "ICD-10 Code";
            // 
            // icd10TextBox
            // 
            this.icd10TextBox.Location = new System.Drawing.Point(120, 38);
            this.icd10TextBox.Name = "icd10TextBox";
            this.icd10TextBox.ReadOnly = true;
            this.icd10TextBox.Size = new System.Drawing.Size(280, 20);
            this.icd10TextBox.TabIndex = 9;
            this.icd10TextBox.TabStop = false;
            // 
            // FormDiseaseDefEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(484, 201);
            this.Controls.Add(this.icd10Button);
            this.Controls.Add(this.icd10Label);
            this.Controls.Add(this.icd10TextBox);
            this.Controls.Add(this.icd9Button);
            this.Controls.Add(this.snomedButton);
            this.Controls.Add(this.snomedLabel);
            this.Controls.Add(this.icd9Label);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.hiddenCheckBox);
            this.Controls.Add(this.snomedTextBox);
            this.Controls.Add(this.icd9TextBox);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDiseaseDefEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Problem Definition";
            this.Load += new System.EventHandler(this.FormProblemDefinitionEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.Button acceptButton;
        private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.CheckBox hiddenCheckBox;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.TextBox icd9TextBox;
		private System.Windows.Forms.Label icd9Label;
		private System.Windows.Forms.TextBox snomedTextBox;
		private System.Windows.Forms.Label snomedLabel;
		private OpenDental.UI.Button snomedButton;
		private OpenDental.UI.Button icd9Button;
		private OpenDental.UI.Button icd10Button;
		private System.Windows.Forms.Label icd10Label;
		private System.Windows.Forms.TextBox icd10TextBox;
	}
}
