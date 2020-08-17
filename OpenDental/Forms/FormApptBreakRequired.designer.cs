namespace Imedisoft.Forms
{
	partial class FormApptBreakRequired
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
		///<summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
            this.missedButton = new OpenDental.UI.Button();
            this.cancelledButton = new OpenDental.UI.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.cancelButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // missedButton
            // 
            this.missedButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.missedButton.Location = new System.Drawing.Point(52, 70);
            this.missedButton.Name = "missedButton";
            this.missedButton.Size = new System.Drawing.Size(150, 25);
            this.missedButton.TabIndex = 2;
            this.missedButton.Text = "&Missed";
            this.missedButton.Click += new System.EventHandler(this.MissedButton_Click);
            // 
            // cancelledButton
            // 
            this.cancelledButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cancelledButton.Location = new System.Drawing.Point(52, 101);
            this.cancelledButton.Name = "cancelledButton";
            this.cancelledButton.Size = new System.Drawing.Size(150, 25);
            this.cancelledButton.TabIndex = 3;
            this.cancelledButton.Text = "&Cancelled";
            this.cancelledButton.Click += new System.EventHandler(this.CancelledButton_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.AllowDrop = true;
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(12, 9);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(230, 50);
            this.infoLabel.TabIndex = 1;
            this.infoLabel.Text = "Before an appointment can be moved or deleted it must first be broken. Please spe" +
    "cify whether it was missed or cancelled.";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(162, 154);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormApptBreakRequired
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(254, 191);
            this.ControlBox = false;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.missedButton);
            this.Controls.Add(this.cancelledButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormApptBreakRequired";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Break Appointment";
            this.ResumeLayout(false);

		}
		#endregion

		private OpenDental.UI.Button missedButton;
		private OpenDental.UI.Button cancelledButton;
		private System.Windows.Forms.Label infoLabel;
		private OpenDental.UI.Button cancelButton;
	}
}
