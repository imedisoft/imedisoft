namespace Imedisoft.Forms
{
	partial class FormEhrPatientVaccineObservationEdit
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
            this.valueTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.questionLabel = new System.Windows.Forms.Label();
            this.questionComboBox = new System.Windows.Forms.ComboBox();
            this.valueLabel = new System.Windows.Forms.Label();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.codeSystemLabel = new System.Windows.Forms.Label();
            this.codeSystemComboBox = new System.Windows.Forms.ComboBox();
            this.valueUnitsLabel = new System.Windows.Forms.Label();
            this.dateObservedLabel = new System.Windows.Forms.Label();
            this.methodCodeTextBox = new System.Windows.Forms.TextBox();
            this.methodCodeLabel = new System.Windows.Forms.Label();
            this.valueUnitsComboBox = new System.Windows.Forms.ComboBox();
            this.dateObservedTextBox = new System.Windows.Forms.TextBox();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.dateObservedButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // valueTypeComboBox
            // 
            this.valueTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.valueTypeComboBox.FormattingEnabled = true;
            this.valueTypeComboBox.Location = new System.Drawing.Point(150, 39);
            this.valueTypeComboBox.Name = "valueTypeComboBox";
            this.valueTypeComboBox.Size = new System.Drawing.Size(130, 21);
            this.valueTypeComboBox.TabIndex = 3;
            this.valueTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(84, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Value Type";
            // 
            // questionLabel
            // 
            this.questionLabel.AutoSize = true;
            this.questionLabel.Location = new System.Drawing.Point(32, 15);
            this.questionLabel.Name = "questionLabel";
            this.questionLabel.Size = new System.Drawing.Size(112, 13);
            this.questionLabel.TabIndex = 0;
            this.questionLabel.Text = "Observation Question";
            // 
            // questionComboBox
            // 
            this.questionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.questionComboBox.FormattingEnabled = true;
            this.questionComboBox.Location = new System.Drawing.Point(150, 12);
            this.questionComboBox.Name = "questionComboBox";
            this.questionComboBox.Size = new System.Drawing.Size(200, 21);
            this.questionComboBox.TabIndex = 1;
            this.questionComboBox.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommitted);
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Location = new System.Drawing.Point(111, 96);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(33, 13);
            this.valueLabel.TabIndex = 6;
            this.valueLabel.Text = "Value";
            // 
            // valueTextBox
            // 
            this.valueTextBox.Location = new System.Drawing.Point(150, 93);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(120, 20);
            this.valueTextBox.TabIndex = 7;
            // 
            // codeSystemLabel
            // 
            this.codeSystemLabel.AutoSize = true;
            this.codeSystemLabel.Enabled = false;
            this.codeSystemLabel.Location = new System.Drawing.Point(45, 69);
            this.codeSystemLabel.Name = "codeSystemLabel";
            this.codeSystemLabel.Size = new System.Drawing.Size(99, 13);
            this.codeSystemLabel.TabIndex = 4;
            this.codeSystemLabel.Text = "Value Code System";
            // 
            // codeSystemComboBox
            // 
            this.codeSystemComboBox.Enabled = false;
            this.codeSystemComboBox.FormattingEnabled = true;
            this.codeSystemComboBox.Items.AddRange(new object[] {
            "CVX",
            "HL70064",
            "SCT"});
            this.codeSystemComboBox.Location = new System.Drawing.Point(150, 66);
            this.codeSystemComboBox.Name = "codeSystemComboBox";
            this.codeSystemComboBox.Size = new System.Drawing.Size(130, 21);
            this.codeSystemComboBox.TabIndex = 5;
            // 
            // valueUnitsLabel
            // 
            this.valueUnitsLabel.AutoSize = true;
            this.valueUnitsLabel.Enabled = false;
            this.valueUnitsLabel.Location = new System.Drawing.Point(84, 122);
            this.valueUnitsLabel.Name = "valueUnitsLabel";
            this.valueUnitsLabel.Size = new System.Drawing.Size(60, 13);
            this.valueUnitsLabel.TabIndex = 8;
            this.valueUnitsLabel.Text = "Value Units";
            // 
            // dateObservedLabel
            // 
            this.dateObservedLabel.AutoSize = true;
            this.dateObservedLabel.Location = new System.Drawing.Point(64, 149);
            this.dateObservedLabel.Name = "dateObservedLabel";
            this.dateObservedLabel.Size = new System.Drawing.Size(80, 13);
            this.dateObservedLabel.TabIndex = 10;
            this.dateObservedLabel.Text = "Date Observed";
            // 
            // methodCodeTextBox
            // 
            this.methodCodeTextBox.Enabled = false;
            this.methodCodeTextBox.Location = new System.Drawing.Point(150, 172);
            this.methodCodeTextBox.Name = "methodCodeTextBox";
            this.methodCodeTextBox.Size = new System.Drawing.Size(113, 20);
            this.methodCodeTextBox.TabIndex = 14;
            // 
            // methodCodeLabel
            // 
            this.methodCodeLabel.AutoSize = true;
            this.methodCodeLabel.Enabled = false;
            this.methodCodeLabel.Location = new System.Drawing.Point(73, 175);
            this.methodCodeLabel.Name = "methodCodeLabel";
            this.methodCodeLabel.Size = new System.Drawing.Size(71, 13);
            this.methodCodeLabel.TabIndex = 13;
            this.methodCodeLabel.Text = "Method Code";
            // 
            // valueUnitsComboBox
            // 
            this.valueUnitsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.valueUnitsComboBox.Enabled = false;
            this.valueUnitsComboBox.FormattingEnabled = true;
            this.valueUnitsComboBox.Location = new System.Drawing.Point(150, 119);
            this.valueUnitsComboBox.Name = "valueUnitsComboBox";
            this.valueUnitsComboBox.Size = new System.Drawing.Size(130, 21);
            this.valueUnitsComboBox.TabIndex = 9;
            // 
            // dateObservedTextBox
            // 
            this.dateObservedTextBox.Location = new System.Drawing.Point(150, 146);
            this.dateObservedTextBox.Name = "dateObservedTextBox";
            this.dateObservedTextBox.Size = new System.Drawing.Size(80, 20);
            this.dateObservedTextBox.TabIndex = 11;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(206, 234);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 15;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(292, 234);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 16;
            this.cancelButton.Text = "&Cancel";
            // 
            // dateObservedButton
            // 
            this.dateObservedButton.Location = new System.Drawing.Point(236, 146);
            this.dateObservedButton.Name = "dateObservedButton";
            this.dateObservedButton.Size = new System.Drawing.Size(50, 20);
            this.dateObservedButton.TabIndex = 12;
            this.dateObservedButton.Text = "Today";
            this.dateObservedButton.Click += new System.EventHandler(this.DateObservedButton_Click);
            // 
            // FormVaccineObsEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(384, 271);
            this.Controls.Add(this.dateObservedButton);
            this.Controls.Add(this.valueUnitsComboBox);
            this.Controls.Add(this.methodCodeTextBox);
            this.Controls.Add(this.methodCodeLabel);
            this.Controls.Add(this.dateObservedTextBox);
            this.Controls.Add(this.dateObservedLabel);
            this.Controls.Add(this.valueUnitsLabel);
            this.Controls.Add(this.codeSystemLabel);
            this.Controls.Add(this.codeSystemComboBox);
            this.Controls.Add(this.valueTextBox);
            this.Controls.Add(this.valueLabel);
            this.Controls.Add(this.questionComboBox);
            this.Controls.Add(this.questionLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.valueTypeComboBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(100, 100);
            this.Name = "FormVaccineObsEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vaccine Observation";
            this.Load += new System.EventHandler(this.FormEhrPatientVaccineObservationEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.ComboBox valueTypeComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label questionLabel;
		private System.Windows.Forms.ComboBox questionComboBox;
		private System.Windows.Forms.Label valueLabel;
		private System.Windows.Forms.TextBox valueTextBox;
		private System.Windows.Forms.Label codeSystemLabel;
		private System.Windows.Forms.ComboBox codeSystemComboBox;
		private System.Windows.Forms.Label valueUnitsLabel;
		private System.Windows.Forms.Label dateObservedLabel;
		private System.Windows.Forms.TextBox dateObservedTextBox;
		private System.Windows.Forms.TextBox methodCodeTextBox;
		private System.Windows.Forms.Label methodCodeLabel;
		private System.Windows.Forms.ComboBox valueUnitsComboBox;
		private OpenDental.UI.Button dateObservedButton;
	}
}
