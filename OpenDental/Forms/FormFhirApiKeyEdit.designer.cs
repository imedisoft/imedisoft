namespace Imedisoft.Forms
{
	partial class FormFhirApiKeyEdit
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
            this.acceptButton = new OpenDental.UI.Button();
            this.keyTextBox = new System.Windows.Forms.TextBox();
            this.phoneTextBox = new OpenDental.ValidPhone();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.keyLabel = new System.Windows.Forms.Label();
            this.phoneLabel = new System.Windows.Forms.Label();
            this.emailLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.dateDisabledLabel = new System.Windows.Forms.Label();
            this.dateDisabledTextBox = new System.Windows.Forms.TextBox();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.disableButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.acceptButton.Location = new System.Drawing.Point(432, 224);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "&Close";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // keyTextBox
            // 
            this.keyTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keyTextBox.Location = new System.Drawing.Point(160, 19);
            this.keyTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.keyTextBox.MaxLength = 100;
            this.keyTextBox.Name = "keyTextBox";
            this.keyTextBox.ReadOnly = true;
            this.keyTextBox.Size = new System.Drawing.Size(320, 22);
            this.keyTextBox.TabIndex = 2;
            this.keyTextBox.TabStop = false;
            // 
            // phoneTextBox
            // 
            this.phoneTextBox.Location = new System.Drawing.Point(160, 129);
            this.phoneTextBox.MaxLength = 100;
            this.phoneTextBox.Name = "phoneTextBox";
            this.phoneTextBox.ReadOnly = true;
            this.phoneTextBox.Size = new System.Drawing.Size(160, 20);
            this.phoneTextBox.TabIndex = 10;
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(160, 101);
            this.emailTextBox.MaxLength = 100;
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.ReadOnly = true;
            this.emailTextBox.Size = new System.Drawing.Size(250, 20);
            this.emailTextBox.TabIndex = 8;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(160, 73);
            this.nameTextBox.MaxLength = 100;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ReadOnly = true;
            this.nameTextBox.Size = new System.Drawing.Size(250, 20);
            this.nameTextBox.TabIndex = 6;
            // 
            // keyLabel
            // 
            this.keyLabel.AutoSize = true;
            this.keyLabel.Location = new System.Drawing.Point(129, 22);
            this.keyLabel.Name = "keyLabel";
            this.keyLabel.Size = new System.Drawing.Size(25, 13);
            this.keyLabel.TabIndex = 1;
            this.keyLabel.Text = "Key";
            // 
            // phoneLabel
            // 
            this.phoneLabel.AutoSize = true;
            this.phoneLabel.Location = new System.Drawing.Point(65, 132);
            this.phoneLabel.Name = "phoneLabel";
            this.phoneLabel.Size = new System.Drawing.Size(89, 13);
            this.phoneLabel.TabIndex = 9;
            this.phoneLabel.Text = "Developer Phone";
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(71, 104);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(83, 13);
            this.emailLabel.TabIndex = 7;
            this.emailLabel.Text = "Developer Email";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(68, 76);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(86, 13);
            this.nameLabel.TabIndex = 5;
            this.nameLabel.Text = "Developer Name";
            // 
            // dateDisabledLabel
            // 
            this.dateDisabledLabel.AutoSize = true;
            this.dateDisabledLabel.Location = new System.Drawing.Point(81, 159);
            this.dateDisabledLabel.Name = "dateDisabledLabel";
            this.dateDisabledLabel.Size = new System.Drawing.Size(73, 13);
            this.dateDisabledLabel.TabIndex = 11;
            this.dateDisabledLabel.Text = "Date Disabled";
            // 
            // dateDisabledTextBox
            // 
            this.dateDisabledTextBox.Location = new System.Drawing.Point(160, 156);
            this.dateDisabledTextBox.MaxLength = 100;
            this.dateDisabledTextBox.Name = "dateDisabledTextBox";
            this.dateDisabledTextBox.ReadOnly = true;
            this.dateDisabledTextBox.Size = new System.Drawing.Size(80, 20);
            this.dateDisabledTextBox.TabIndex = 12;
            this.dateDisabledTextBox.TabStop = false;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(160, 47);
            this.statusTextBox.MaxLength = 100;
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.Size = new System.Drawing.Size(160, 20);
            this.statusTextBox.TabIndex = 4;
            this.statusTextBox.TabStop = false;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(116, 50);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(38, 13);
            this.statusLabel.TabIndex = 3;
            this.statusLabel.Text = "Status";
            // 
            // disableButton
            // 
            this.disableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.disableButton.Image = global::Imedisoft.Properties.Resources.IconToggleOff;
            this.disableButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.disableButton.Location = new System.Drawing.Point(12, 224);
            this.disableButton.Name = "disableButton";
            this.disableButton.Size = new System.Drawing.Size(80, 25);
            this.disableButton.TabIndex = 13;
            this.disableButton.Text = "&Disable";
            this.disableButton.Click += new System.EventHandler(this.DisableButton_Click);
            // 
            // FormFhirApiKeyEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.acceptButton;
            this.ClientSize = new System.Drawing.Size(524, 261);
            this.Controls.Add(this.disableButton);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.dateDisabledTextBox);
            this.Controls.Add(this.dateDisabledLabel);
            this.Controls.Add(this.keyTextBox);
            this.Controls.Add(this.phoneTextBox);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.keyLabel);
            this.Controls.Add(this.phoneLabel);
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.acceptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFhirApiKeyEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FHIR API Key";
            this.Load += new System.EventHandler(this.FormFhirApiKeyEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.TextBox keyTextBox;
		private OpenDental.ValidPhone phoneTextBox;
		private System.Windows.Forms.TextBox emailTextBox;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.Label keyLabel;
		private System.Windows.Forms.Label phoneLabel;
		private System.Windows.Forms.Label emailLabel;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.Label dateDisabledLabel;
		private System.Windows.Forms.TextBox dateDisabledTextBox;
		private System.Windows.Forms.TextBox statusTextBox;
		private System.Windows.Forms.Label statusLabel;
		private OpenDental.UI.Button disableButton;
	}
}