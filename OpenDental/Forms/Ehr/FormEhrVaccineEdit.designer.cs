namespace Imedisoft.Forms
{
	partial class FormEhrVaccineEdit
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
            this.manufacturerComboBox = new System.Windows.Forms.ComboBox();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.manufacturerLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.cvxTextBox = new System.Windows.Forms.TextBox();
            this.cvxLabel = new System.Windows.Forms.Label();
            this.cvxButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // manufacturerComboBox
            // 
            this.manufacturerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.manufacturerComboBox.Location = new System.Drawing.Point(110, 71);
            this.manufacturerComboBox.Name = "manufacturerComboBox";
            this.manufacturerComboBox.Size = new System.Drawing.Size(250, 21);
            this.manufacturerComboBox.TabIndex = 6;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(216, 134);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 7;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(302, 134);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "&Cancel";
            // 
            // manufacturerLabel
            // 
            this.manufacturerLabel.AutoSize = true;
            this.manufacturerLabel.Location = new System.Drawing.Point(32, 74);
            this.manufacturerLabel.Name = "manufacturerLabel";
            this.manufacturerLabel.Size = new System.Drawing.Size(72, 13);
            this.manufacturerLabel.TabIndex = 5;
            this.manufacturerLabel.Text = "Manufacturer";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(110, 45);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(250, 20);
            this.nameTextBox.TabIndex = 4;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(31, 48);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(73, 13);
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "Vaccine Name";
            // 
            // cvxTextBox
            // 
            this.cvxTextBox.Location = new System.Drawing.Point(110, 19);
            this.cvxTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.cvxTextBox.Name = "cvxTextBox";
            this.cvxTextBox.ReadOnly = true;
            this.cvxTextBox.Size = new System.Drawing.Size(200, 20);
            this.cvxTextBox.TabIndex = 1;
            this.cvxTextBox.TabStop = false;
            // 
            // cvxLabel
            // 
            this.cvxLabel.AutoSize = true;
            this.cvxLabel.Location = new System.Drawing.Point(50, 22);
            this.cvxLabel.Name = "cvxLabel";
            this.cvxLabel.Size = new System.Drawing.Size(54, 13);
            this.cvxLabel.TabIndex = 0;
            this.cvxLabel.Text = "CVX Code";
            // 
            // cvxButton
            // 
            this.cvxButton.Location = new System.Drawing.Point(316, 19);
            this.cvxButton.Name = "cvxButton";
            this.cvxButton.Size = new System.Drawing.Size(25, 20);
            this.cvxButton.TabIndex = 2;
            this.cvxButton.Text = "...";
            this.cvxButton.Click += new System.EventHandler(this.CvxButton_Click);
            // 
            // FormVaccineDefEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(394, 171);
            this.Controls.Add(this.cvxButton);
            this.Controls.Add(this.manufacturerLabel);
            this.Controls.Add(this.manufacturerComboBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.cvxTextBox);
            this.Controls.Add(this.cvxLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVaccineDefEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vaccine Definition";
            this.Load += new System.EventHandler(this.FormEhrVaccineEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label manufacturerLabel;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.TextBox cvxTextBox;
		private System.Windows.Forms.Label cvxLabel;
		private OpenDental.UI.Button cvxButton;
		private System.Windows.Forms.ComboBox manufacturerComboBox;
	}
}
