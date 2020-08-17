namespace Imedisoft.Forms
{
	partial class FormApptBreak
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
            this.procedureTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.missedRadioButton = new System.Windows.Forms.RadioButton();
            this.cancelledRadioButton = new System.Windows.Forms.RadioButton();
            this.unschedButton = new OpenDental.UI.Button();
            this.PinboardButton = new OpenDental.UI.Button();
            this.ApptBookButton = new OpenDental.UI.Button();
            this.procedureTypeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(122, 184);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // procedureTypeGroupBox
            // 
            this.procedureTypeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.procedureTypeGroupBox.Controls.Add(this.missedRadioButton);
            this.procedureTypeGroupBox.Controls.Add(this.cancelledRadioButton);
            this.procedureTypeGroupBox.Location = new System.Drawing.Point(12, 12);
            this.procedureTypeGroupBox.Name = "procedureTypeGroupBox";
            this.procedureTypeGroupBox.Size = new System.Drawing.Size(190, 50);
            this.procedureTypeGroupBox.TabIndex = 1;
            this.procedureTypeGroupBox.TabStop = false;
            this.procedureTypeGroupBox.Text = "Broken Procedure Type";
            // 
            // missedRadioButton
            // 
            this.missedRadioButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.missedRadioButton.AutoSize = true;
            this.missedRadioButton.Location = new System.Drawing.Point(28, 22);
            this.missedRadioButton.Name = "missedRadioButton";
            this.missedRadioButton.Size = new System.Drawing.Size(57, 17);
            this.missedRadioButton.TabIndex = 0;
            this.missedRadioButton.TabStop = true;
            this.missedRadioButton.Text = "Missed";
            this.missedRadioButton.UseVisualStyleBackColor = true;
            // 
            // cancelledRadioButton
            // 
            this.cancelledRadioButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cancelledRadioButton.AutoSize = true;
            this.cancelledRadioButton.Location = new System.Drawing.Point(91, 22);
            this.cancelledRadioButton.Name = "cancelledRadioButton";
            this.cancelledRadioButton.Size = new System.Drawing.Size(71, 17);
            this.cancelledRadioButton.TabIndex = 1;
            this.cancelledRadioButton.TabStop = true;
            this.cancelledRadioButton.Text = "Cancelled";
            this.cancelledRadioButton.UseVisualStyleBackColor = true;
            // 
            // unschedButton
            // 
            this.unschedButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.unschedButton.Location = new System.Drawing.Point(32, 75);
            this.unschedButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.unschedButton.Name = "unschedButton";
            this.unschedButton.Size = new System.Drawing.Size(150, 25);
            this.unschedButton.TabIndex = 2;
            this.unschedButton.Text = "Send to Unscheduled List";
            this.unschedButton.UseVisualStyleBackColor = true;
            this.unschedButton.Click += new System.EventHandler(this.UnschedButton_Click);
            // 
            // PinboardButton
            // 
            this.PinboardButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PinboardButton.Location = new System.Drawing.Point(32, 106);
            this.PinboardButton.Name = "PinboardButton";
            this.PinboardButton.Size = new System.Drawing.Size(150, 25);
            this.PinboardButton.TabIndex = 3;
            this.PinboardButton.Text = "Copy to Pinboard";
            this.PinboardButton.UseVisualStyleBackColor = true;
            this.PinboardButton.Click += new System.EventHandler(this.PinboardButton_Click);
            // 
            // ApptBookButton
            // 
            this.ApptBookButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ApptBookButton.Location = new System.Drawing.Point(32, 137);
            this.ApptBookButton.Name = "ApptBookButton";
            this.ApptBookButton.Size = new System.Drawing.Size(150, 25);
            this.ApptBookButton.TabIndex = 4;
            this.ApptBookButton.Text = "Leave in Appt Module";
            this.ApptBookButton.UseVisualStyleBackColor = true;
            this.ApptBookButton.Click += new System.EventHandler(this.ApptBookButton_Click);
            // 
            // FormApptBreak
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(214, 221);
            this.ControlBox = false;
            this.Controls.Add(this.ApptBookButton);
            this.Controls.Add(this.PinboardButton);
            this.Controls.Add(this.unschedButton);
            this.Controls.Add(this.procedureTypeGroupBox);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormApptBreak";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Broken Appointment Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormApptBreak_FormClosing);
            this.Load += new System.EventHandler(this.FormApptBreak_Load);
            this.procedureTypeGroupBox.ResumeLayout(false);
            this.procedureTypeGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.GroupBox procedureTypeGroupBox;
		private System.Windows.Forms.RadioButton cancelledRadioButton;
		private System.Windows.Forms.RadioButton missedRadioButton;
		private OpenDental.UI.Button unschedButton;
		private OpenDental.UI.Button PinboardButton;
		private OpenDental.UI.Button ApptBookButton;
	}
}
