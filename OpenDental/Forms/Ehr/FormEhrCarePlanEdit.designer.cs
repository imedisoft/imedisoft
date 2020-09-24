namespace Imedisoft.Forms
{
	partial class FormEhrCarePlanEdit
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
            this.snomedTextBox = new System.Windows.Forms.TextBox();
            this.snomedLabel = new System.Windows.Forms.Label();
            this.instructionsTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateLabel = new System.Windows.Forms.Label();
            this.dateTextBox = new System.Windows.Forms.TextBox();
            this.snomedButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // snomedTextBox
            // 
            this.snomedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.snomedTextBox.Location = new System.Drawing.Point(110, 45);
            this.snomedTextBox.Name = "snomedTextBox";
            this.snomedTextBox.ReadOnly = true;
            this.snomedTextBox.Size = new System.Drawing.Size(291, 20);
            this.snomedTextBox.TabIndex = 3;
            // 
            // snomedLabel
            // 
            this.snomedLabel.AutoSize = true;
            this.snomedLabel.Location = new System.Drawing.Point(15, 48);
            this.snomedLabel.Name = "snomedLabel";
            this.snomedLabel.Size = new System.Drawing.Size(89, 13);
            this.snomedLabel.TabIndex = 2;
            this.snomedLabel.Text = "SNOMED CT Goal";
            // 
            // instructionsTextBox
            // 
            this.instructionsTextBox.AcceptsReturn = true;
            this.instructionsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.instructionsTextBox.Location = new System.Drawing.Point(110, 71);
            this.instructionsTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.instructionsTextBox.Multiline = true;
            this.instructionsTextBox.Name = "instructionsTextBox";
            this.instructionsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.instructionsTextBox.Size = new System.Drawing.Size(322, 160);
            this.instructionsTextBox.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Instructions";
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(74, 22);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(30, 13);
            this.dateLabel.TabIndex = 0;
            this.dateLabel.Text = "Date";
            // 
            // dateTextBox
            // 
            this.dateTextBox.Location = new System.Drawing.Point(110, 19);
            this.dateTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.dateTextBox.Name = "dateTextBox";
            this.dateTextBox.Size = new System.Drawing.Size(94, 20);
            this.dateTextBox.TabIndex = 1;
            // 
            // snomedButton
            // 
            this.snomedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.snomedButton.Location = new System.Drawing.Point(407, 45);
            this.snomedButton.Name = "snomedButton";
            this.snomedButton.Size = new System.Drawing.Size(25, 20);
            this.snomedButton.TabIndex = 4;
            this.snomedButton.Text = "...";
            this.snomedButton.Click += new System.EventHandler(this.SnomedButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(266, 244);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 7;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(352, 244);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormEhrCarePlanEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(444, 281);
            this.Controls.Add(this.dateTextBox);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.instructionsTextBox);
            this.Controls.Add(this.snomedTextBox);
            this.Controls.Add(this.snomedButton);
            this.Controls.Add(this.snomedLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEhrCarePlanEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Care Plan";
            this.Load += new System.EventHandler(this.FormEhrCarePlanEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.TextBox snomedTextBox;
		private OpenDental.UI.Button snomedButton;
		private System.Windows.Forms.Label snomedLabel;
		private System.Windows.Forms.TextBox instructionsTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label dateLabel;
		private System.Windows.Forms.TextBox dateTextBox;
	}
}
