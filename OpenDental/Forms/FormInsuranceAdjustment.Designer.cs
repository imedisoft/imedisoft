namespace Imedisoft.Forms
{
    partial class FormInsuranceAdjustment
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
            this.dateLabel = new System.Windows.Forms.Label();
            this.deductibleUsedLabel = new System.Windows.Forms.Label();
            this.insuranceUsedLabel = new System.Windows.Forms.Label();
            this.dateTextBox = new System.Windows.Forms.TextBox();
            this.insuranceUsedTextBox = new System.Windows.Forms.TextBox();
            this.deductibleUsedTextBox = new System.Windows.Forms.TextBox();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(114, 55);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(30, 13);
            this.dateLabel.TabIndex = 1;
            this.dateLabel.Text = "Date";
            // 
            // deductibleUsedLabel
            // 
            this.deductibleUsedLabel.AutoSize = true;
            this.deductibleUsedLabel.Location = new System.Drawing.Point(60, 107);
            this.deductibleUsedLabel.Name = "deductibleUsedLabel";
            this.deductibleUsedLabel.Size = new System.Drawing.Size(84, 13);
            this.deductibleUsedLabel.TabIndex = 5;
            this.deductibleUsedLabel.Text = "Deductible Used";
            // 
            // insuranceUsedLabel
            // 
            this.insuranceUsedLabel.AutoSize = true;
            this.insuranceUsedLabel.Location = new System.Drawing.Point(62, 81);
            this.insuranceUsedLabel.Name = "insuranceUsedLabel";
            this.insuranceUsedLabel.Size = new System.Drawing.Size(82, 13);
            this.insuranceUsedLabel.TabIndex = 3;
            this.insuranceUsedLabel.Text = "Insurance Used";
            // 
            // dateTextBox
            // 
            this.dateTextBox.Location = new System.Drawing.Point(150, 52);
            this.dateTextBox.Name = "dateTextBox";
            this.dateTextBox.Size = new System.Drawing.Size(100, 20);
            this.dateTextBox.TabIndex = 2;
            // 
            // insuranceUsedTextBox
            // 
            this.insuranceUsedTextBox.Location = new System.Drawing.Point(150, 78);
            this.insuranceUsedTextBox.Name = "insuranceUsedTextBox";
            this.insuranceUsedTextBox.Size = new System.Drawing.Size(70, 20);
            this.insuranceUsedTextBox.TabIndex = 4;
            // 
            // deductibleUsedTextBox
            // 
            this.deductibleUsedTextBox.Location = new System.Drawing.Point(150, 104);
            this.deductibleUsedTextBox.Name = "deductibleUsedTextBox";
            this.deductibleUsedTextBox.Size = new System.Drawing.Size(70, 20);
            this.deductibleUsedTextBox.TabIndex = 6;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(252, 143);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 8;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(252, 174);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "&Cancel";
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(12, 9);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(320, 40);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Make sure the date you use falls within the correct benefit year.";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(12, 174);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 7;
            this.deleteButton.Text = "Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormInsAdj
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(344, 211);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.deductibleUsedTextBox);
            this.Controls.Add(this.insuranceUsedTextBox);
            this.Controls.Add(this.dateTextBox);
            this.Controls.Add(this.insuranceUsedLabel);
            this.Controls.Add(this.deductibleUsedLabel);
            this.Controls.Add(this.dateLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormInsAdj";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Insurance Adjustments";
            this.Load += new System.EventHandler(this.FormInsuranceAdjustment_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.Label dateLabel;
		private System.Windows.Forms.Label deductibleUsedLabel;
		private System.Windows.Forms.Label insuranceUsedLabel;
		private System.Windows.Forms.TextBox dateTextBox;
		private System.Windows.Forms.TextBox insuranceUsedTextBox;
		private System.Windows.Forms.TextBox deductibleUsedTextBox;
		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label infoLabel;
		private OpenDental.UI.Button deleteButton;
	}
}
