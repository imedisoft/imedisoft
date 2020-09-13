namespace Imedisoft.Forms
{
    partial class FormCdsInterventionLabResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCdsInterventionLabResult));
            this.butCancel = new OpenDental.UI.Button();
            this.butOk = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.loincTextBox = new System.Windows.Forms.TextBox();
            this.loincLabel = new System.Windows.Forms.Label();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.loincButton = new OpenDental.UI.Button();
            this.comparatorComboBox = new System.Windows.Forms.ComboBox();
            this.unitsComboBox = new System.Windows.Forms.ComboBox();
            this.numericalResultsGroupBox = new System.Windows.Forms.GroupBox();
            this.valueLabel = new System.Windows.Forms.Label();
            this.microbiologyResultsGroupBox = new System.Windows.Forms.GroupBox();
            this.snomedDescriptionLabel = new System.Windows.Forms.Label();
            this.snomedDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.snomedButton = new OpenDental.UI.Button();
            this.snomedTextBox = new System.Windows.Forms.TextBox();
            this.snomedLabel = new System.Windows.Forms.Label();
            this.allResultsCheckBox = new System.Windows.Forms.CheckBox();
            this.allResultsGroupBox = new System.Windows.Forms.GroupBox();
            this.numericalResultsGroupBox.SuspendLayout();
            this.microbiologyResultsGroupBox.SuspendLayout();
            this.allResultsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Location = new System.Drawing.Point(332, 364);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(80, 25);
            this.butCancel.TabIndex = 0;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // butOk
            // 
            this.butOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOk.Location = new System.Drawing.Point(246, 364);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(80, 25);
            this.butOk.TabIndex = 9;
            this.butOk.Text = "Ok";
            this.butOk.UseVisualStyleBackColor = true;
            this.butOk.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Location = new System.Drawing.Point(12, 364);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 10;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            // 
            // loincTextBox
            // 
            this.loincTextBox.Location = new System.Drawing.Point(120, 12);
            this.loincTextBox.Name = "loincTextBox";
            this.loincTextBox.ReadOnly = true;
            this.loincTextBox.Size = new System.Drawing.Size(100, 20);
            this.loincTextBox.TabIndex = 2;
            // 
            // loincLabel
            // 
            this.loincLabel.AutoSize = true;
            this.loincLabel.Location = new System.Drawing.Point(76, 15);
            this.loincLabel.Name = "loincLabel";
            this.loincLabel.Size = new System.Drawing.Size(38, 13);
            this.loincLabel.TabIndex = 1;
            this.loincLabel.Text = "LOINC";
            // 
            // valueTextBox
            // 
            this.valueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.valueTextBox.Location = new System.Drawing.Point(164, 19);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(154, 20);
            this.valueTextBox.TabIndex = 2;
            this.valueTextBox.TextChanged += new System.EventHandler(this.ValueTextBox_TextChanged);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(54, 41);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 4;
            this.descriptionLabel.Text = "Description";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(120, 38);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(283, 50);
            this.descriptionTextBox.TabIndex = 5;
            // 
            // loincButton
            // 
            this.loincButton.Image = ((System.Drawing.Image)(resources.GetObject("loincButton.Image")));
            this.loincButton.Location = new System.Drawing.Point(226, 12);
            this.loincButton.Name = "loincButton";
            this.loincButton.Size = new System.Drawing.Size(25, 20);
            this.loincButton.TabIndex = 3;
            this.loincButton.UseVisualStyleBackColor = true;
            this.loincButton.Click += new System.EventHandler(this.LoincButton_Click);
            // 
            // comparatorComboBox
            // 
            this.comparatorComboBox.FormattingEnabled = true;
            this.comparatorComboBox.Location = new System.Drawing.Point(108, 19);
            this.comparatorComboBox.Name = "comparatorComboBox";
            this.comparatorComboBox.Size = new System.Drawing.Size(50, 21);
            this.comparatorComboBox.TabIndex = 1;
            // 
            // unitsComboBox
            // 
            this.unitsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.unitsComboBox.FormattingEnabled = true;
            this.unitsComboBox.Location = new System.Drawing.Point(324, 19);
            this.unitsComboBox.Name = "unitsComboBox";
            this.unitsComboBox.Size = new System.Drawing.Size(70, 21);
            this.unitsComboBox.TabIndex = 3;
            // 
            // numericalResultsGroupBox
            // 
            this.numericalResultsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericalResultsGroupBox.Controls.Add(this.valueLabel);
            this.numericalResultsGroupBox.Controls.Add(this.unitsComboBox);
            this.numericalResultsGroupBox.Controls.Add(this.valueTextBox);
            this.numericalResultsGroupBox.Controls.Add(this.comparatorComboBox);
            this.numericalResultsGroupBox.Location = new System.Drawing.Point(12, 150);
            this.numericalResultsGroupBox.Name = "numericalResultsGroupBox";
            this.numericalResultsGroupBox.Size = new System.Drawing.Size(400, 60);
            this.numericalResultsGroupBox.TabIndex = 7;
            this.numericalResultsGroupBox.TabStop = false;
            this.numericalResultsGroupBox.Text = "Numeric Results";
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Location = new System.Drawing.Point(69, 22);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(33, 13);
            this.valueLabel.TabIndex = 0;
            this.valueLabel.Text = "Value";
            // 
            // microbiologyResultsGroupBox
            // 
            this.microbiologyResultsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.microbiologyResultsGroupBox.Controls.Add(this.snomedDescriptionLabel);
            this.microbiologyResultsGroupBox.Controls.Add(this.snomedDescriptionTextBox);
            this.microbiologyResultsGroupBox.Controls.Add(this.snomedButton);
            this.microbiologyResultsGroupBox.Controls.Add(this.snomedTextBox);
            this.microbiologyResultsGroupBox.Controls.Add(this.snomedLabel);
            this.microbiologyResultsGroupBox.Location = new System.Drawing.Point(12, 216);
            this.microbiologyResultsGroupBox.Name = "microbiologyResultsGroupBox";
            this.microbiologyResultsGroupBox.Size = new System.Drawing.Size(400, 120);
            this.microbiologyResultsGroupBox.TabIndex = 8;
            this.microbiologyResultsGroupBox.TabStop = false;
            this.microbiologyResultsGroupBox.Text = "Microbiology Results";
            // 
            // snomedDescriptionLabel
            // 
            this.snomedDescriptionLabel.AutoSize = true;
            this.snomedDescriptionLabel.Location = new System.Drawing.Point(42, 48);
            this.snomedDescriptionLabel.Name = "snomedDescriptionLabel";
            this.snomedDescriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.snomedDescriptionLabel.TabIndex = 3;
            this.snomedDescriptionLabel.Text = "Description";
            // 
            // snomedDescriptionTextBox
            // 
            this.snomedDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.snomedDescriptionTextBox.Location = new System.Drawing.Point(108, 45);
            this.snomedDescriptionTextBox.Multiline = true;
            this.snomedDescriptionTextBox.Name = "snomedDescriptionTextBox";
            this.snomedDescriptionTextBox.Size = new System.Drawing.Size(286, 60);
            this.snomedDescriptionTextBox.TabIndex = 4;
            // 
            // snomedButton
            // 
            this.snomedButton.Image = ((System.Drawing.Image)(resources.GetObject("snomedButton.Image")));
            this.snomedButton.Location = new System.Drawing.Point(315, 19);
            this.snomedButton.Name = "snomedButton";
            this.snomedButton.Size = new System.Drawing.Size(25, 20);
            this.snomedButton.TabIndex = 2;
            this.snomedButton.UseVisualStyleBackColor = true;
            this.snomedButton.Click += new System.EventHandler(this.SnomedButton_Click);
            // 
            // snomedTextBox
            // 
            this.snomedTextBox.Location = new System.Drawing.Point(108, 19);
            this.snomedTextBox.Name = "snomedTextBox";
            this.snomedTextBox.ReadOnly = true;
            this.snomedTextBox.Size = new System.Drawing.Size(200, 20);
            this.snomedTextBox.TabIndex = 1;
            // 
            // snomedLabel
            // 
            this.snomedLabel.AutoSize = true;
            this.snomedLabel.Location = new System.Drawing.Point(37, 22);
            this.snomedLabel.Name = "snomedLabel";
            this.snomedLabel.Size = new System.Drawing.Size(65, 13);
            this.snomedLabel.TabIndex = 0;
            this.snomedLabel.Text = "SNOMED CT";
            // 
            // allResultsCheckBox
            // 
            this.allResultsCheckBox.AutoSize = true;
            this.allResultsCheckBox.Location = new System.Drawing.Point(108, 19);
            this.allResultsCheckBox.Name = "allResultsCheckBox";
            this.allResultsCheckBox.Size = new System.Drawing.Size(252, 17);
            this.allResultsCheckBox.TabIndex = 0;
            this.allResultsCheckBox.Tag = "";
            this.allResultsCheckBox.Text = "Trigger intervention reguardless of result value";
            this.allResultsCheckBox.UseVisualStyleBackColor = true;
            // 
            // allResultsGroupBox
            // 
            this.allResultsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.allResultsGroupBox.Controls.Add(this.allResultsCheckBox);
            this.allResultsGroupBox.Location = new System.Drawing.Point(12, 94);
            this.allResultsGroupBox.Name = "allResultsGroupBox";
            this.allResultsGroupBox.Size = new System.Drawing.Size(400, 50);
            this.allResultsGroupBox.TabIndex = 6;
            this.allResultsGroupBox.TabStop = false;
            this.allResultsGroupBox.Text = "All Results";
            // 
            // FormCdsInterventionLabResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 401);
            this.Controls.Add(this.allResultsGroupBox);
            this.Controls.Add(this.microbiologyResultsGroupBox);
            this.Controls.Add(this.numericalResultsGroupBox);
            this.Controls.Add(this.loincButton);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.loincTextBox);
            this.Controls.Add(this.loincLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.butOk);
            this.Controls.Add(this.butCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCdsInterventionLabResult";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Clinical Decision Support Lab";
            this.Load += new System.EventHandler(this.FormCdsInterventionLabResult_Load);
            this.numericalResultsGroupBox.ResumeLayout(false);
            this.numericalResultsGroupBox.PerformLayout();
            this.microbiologyResultsGroupBox.ResumeLayout(false);
            this.microbiologyResultsGroupBox.PerformLayout();
            this.allResultsGroupBox.ResumeLayout(false);
            this.allResultsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.Button butCancel;
        private OpenDental.UI.Button butOk;
        private OpenDental.UI.Button deleteButton;
        private System.Windows.Forms.TextBox loincTextBox;
        private System.Windows.Forms.Label loincLabel;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private OpenDental.UI.Button loincButton;
        private System.Windows.Forms.ComboBox comparatorComboBox;
        private System.Windows.Forms.ComboBox unitsComboBox;
        private System.Windows.Forms.GroupBox numericalResultsGroupBox;
        private System.Windows.Forms.Label valueLabel;
        private System.Windows.Forms.GroupBox microbiologyResultsGroupBox;
        private System.Windows.Forms.Label snomedDescriptionLabel;
        private System.Windows.Forms.TextBox snomedDescriptionTextBox;
        private OpenDental.UI.Button snomedButton;
        private System.Windows.Forms.TextBox snomedTextBox;
        private System.Windows.Forms.Label snomedLabel;
        private System.Windows.Forms.CheckBox allResultsCheckBox;
        private System.Windows.Forms.GroupBox allResultsGroupBox;
    }
}
