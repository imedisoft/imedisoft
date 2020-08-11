namespace Imedisoft.Forms
{
	partial class FormBackupReminder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBackupReminder));
            this.acceptButton = new OpenDental.UI.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.a5CheckBox = new System.Windows.Forms.CheckBox();
            this.a1CheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.a2CheckBox = new System.Windows.Forms.CheckBox();
            this.a4CheckBox = new System.Windows.Forms.CheckBox();
            this.a3CheckBox = new System.Windows.Forms.CheckBox();
            this.b2CheckBox = new System.Windows.Forms.CheckBox();
            this.b1CheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.b3CheckBox = new System.Windows.Forms.CheckBox();
            this.c3CheckBox = new System.Windows.Forms.CheckBox();
            this.c2CheckBox = new System.Windows.Forms.CheckBox();
            this.c1CheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(442, 474);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 15;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(12, 9);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(510, 110);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = resources.GetString("infoLabel.Text");
            // 
            // a5CheckBox
            // 
            this.a5CheckBox.AutoSize = true;
            this.a5CheckBox.Location = new System.Drawing.Point(39, 242);
            this.a5CheckBox.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.a5CheckBox.Name = "a5CheckBox";
            this.a5CheckBox.Size = new System.Drawing.Size(81, 17);
            this.a5CheckBox.TabIndex = 6;
            this.a5CheckBox.Text = "No backups";
            this.a5CheckBox.UseVisualStyleBackColor = true;
            // 
            // a1CheckBox
            // 
            this.a1CheckBox.AutoSize = true;
            this.a1CheckBox.Location = new System.Drawing.Point(39, 150);
            this.a1CheckBox.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.a1CheckBox.Name = "a1CheckBox";
            this.a1CheckBox.Size = new System.Drawing.Size(56, 17);
            this.a1CheckBox.TabIndex = 2;
            this.a1CheckBox.Text = "Online";
            this.a1CheckBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 129);
            this.label3.Margin = new System.Windows.Forms.Padding(20, 10, 3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(281, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Do you make backups every single day?  Backup method:";
            // 
            // a2CheckBox
            // 
            this.a2CheckBox.AutoSize = true;
            this.a2CheckBox.Location = new System.Drawing.Point(39, 173);
            this.a2CheckBox.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.a2CheckBox.Name = "a2CheckBox";
            this.a2CheckBox.Size = new System.Drawing.Size(222, 17);
            this.a2CheckBox.TabIndex = 3;
            this.a2CheckBox.Text = "Removable (external HD, USB drive, etc)";
            this.a2CheckBox.UseVisualStyleBackColor = true;
            // 
            // a4CheckBox
            // 
            this.a4CheckBox.AutoSize = true;
            this.a4CheckBox.Location = new System.Drawing.Point(39, 219);
            this.a4CheckBox.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.a4CheckBox.Name = "a4CheckBox";
            this.a4CheckBox.Size = new System.Drawing.Size(130, 17);
            this.a4CheckBox.TabIndex = 5;
            this.a4CheckBox.Text = "Other backup method";
            this.a4CheckBox.UseVisualStyleBackColor = true;
            // 
            // a3CheckBox
            // 
            this.a3CheckBox.AutoSize = true;
            this.a3CheckBox.Location = new System.Drawing.Point(39, 196);
            this.a3CheckBox.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.a3CheckBox.Name = "a3CheckBox";
            this.a3CheckBox.Size = new System.Drawing.Size(242, 17);
            this.a3CheckBox.TabIndex = 4;
            this.a3CheckBox.Text = "Network (to another computer in your office)";
            this.a3CheckBox.UseVisualStyleBackColor = true;
            // 
            // b2CheckBox
            // 
            this.b2CheckBox.AutoSize = true;
            this.b2CheckBox.Location = new System.Drawing.Point(39, 316);
            this.b2CheckBox.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.b2CheckBox.Name = "b2CheckBox";
            this.b2CheckBox.Size = new System.Drawing.Size(187, 17);
            this.b2CheckBox.TabIndex = 9;
            this.b2CheckBox.Text = "Run backup from a second server";
            this.b2CheckBox.UseVisualStyleBackColor = true;
            // 
            // b1CheckBox
            // 
            this.b1CheckBox.AutoSize = true;
            this.b1CheckBox.Location = new System.Drawing.Point(39, 293);
            this.b1CheckBox.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.b1CheckBox.Name = "b1CheckBox";
            this.b1CheckBox.Size = new System.Drawing.Size(256, 17);
            this.b1CheckBox.TabIndex = 8;
            this.b1CheckBox.Text = "Restore to home computer at least once a week";
            this.b1CheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 272);
            this.label2.Margin = new System.Windows.Forms.Padding(20, 10, 3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(300, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "What proof do you have that your recent backups are good?";
            // 
            // b3CheckBox
            // 
            this.b3CheckBox.AutoSize = true;
            this.b3CheckBox.Location = new System.Drawing.Point(39, 339);
            this.b3CheckBox.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.b3CheckBox.Name = "b3CheckBox";
            this.b3CheckBox.Size = new System.Drawing.Size(68, 17);
            this.b3CheckBox.TabIndex = 10;
            this.b3CheckBox.Text = "No proof";
            this.b3CheckBox.UseVisualStyleBackColor = true;
            // 
            // c3CheckBox
            // 
            this.c3CheckBox.AutoSize = true;
            this.c3CheckBox.Location = new System.Drawing.Point(39, 436);
            this.c3CheckBox.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.c3CheckBox.Name = "c3CheckBox";
            this.c3CheckBox.Size = new System.Drawing.Size(83, 17);
            this.c3CheckBox.TabIndex = 14;
            this.c3CheckBox.Text = "No strategy";
            this.c3CheckBox.UseVisualStyleBackColor = true;
            // 
            // c2CheckBox
            // 
            this.c2CheckBox.AutoSize = true;
            this.c2CheckBox.Location = new System.Drawing.Point(39, 413);
            this.c2CheckBox.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.c2CheckBox.Name = "c2CheckBox";
            this.c2CheckBox.Size = new System.Drawing.Size(173, 17);
            this.c2CheckBox.TabIndex = 13;
            this.c2CheckBox.Text = "Saved hardcopy paper reports";
            this.c2CheckBox.UseVisualStyleBackColor = true;
            // 
            // c1CheckBox
            // 
            this.c1CheckBox.AutoSize = true;
            this.c1CheckBox.Location = new System.Drawing.Point(39, 390);
            this.c1CheckBox.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.c1CheckBox.Name = "c1CheckBox";
            this.c1CheckBox.Size = new System.Drawing.Size(310, 17);
            this.c1CheckBox.TabIndex = 12;
            this.c1CheckBox.Text = "Completely separate archives stored offsite (DVD, HD, etc)";
            this.c1CheckBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 369);
            this.label4.Margin = new System.Windows.Forms.Padding(20, 10, 3, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(383, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "What secondary long-term mechanism do you use to ensure minimal data loss?";
            // 
            // FormBackupReminder
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(534, 511);
            this.ControlBox = false;
            this.Controls.Add(this.c3CheckBox);
            this.Controls.Add(this.c2CheckBox);
            this.Controls.Add(this.c1CheckBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.b3CheckBox);
            this.Controls.Add(this.b2CheckBox);
            this.Controls.Add(this.b1CheckBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.a4CheckBox);
            this.Controls.Add(this.a3CheckBox);
            this.Controls.Add(this.a2CheckBox);
            this.Controls.Add(this.a1CheckBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.a5CheckBox);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.acceptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormBackupReminder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Backup Reminder";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBackupReminder_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.Label infoLabel;
		private System.Windows.Forms.CheckBox a5CheckBox;
		private System.Windows.Forms.CheckBox a1CheckBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox a2CheckBox;
		private System.Windows.Forms.CheckBox a4CheckBox;
		private System.Windows.Forms.CheckBox a3CheckBox;
		private System.Windows.Forms.CheckBox b2CheckBox;
		private System.Windows.Forms.CheckBox b1CheckBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox b3CheckBox;
		private System.Windows.Forms.CheckBox c3CheckBox;
		private System.Windows.Forms.CheckBox c2CheckBox;
		private System.Windows.Forms.CheckBox c1CheckBox;
		private System.Windows.Forms.Label label4;
	}
}
