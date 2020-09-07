namespace Imedisoft.Forms
{
    partial class FormAging
    {
		private System.ComponentModel.Container components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
            this.lastDateTextBox = new System.Windows.Forms.TextBox();
            this.lastDateLabel = new System.Windows.Forms.Label();
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.dateTextBox = new System.Windows.Forms.TextBox();
            this.dateLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lastDateTextBox
            // 
            this.lastDateTextBox.Location = new System.Drawing.Point(194, 92);
            this.lastDateTextBox.Name = "lastDateTextBox";
            this.lastDateTextBox.ReadOnly = true;
            this.lastDateTextBox.Size = new System.Drawing.Size(100, 20);
            this.lastDateTextBox.TabIndex = 6;
            // 
            // lastDateLabel
            // 
            this.lastDateLabel.AutoSize = true;
            this.lastDateLabel.Location = new System.Drawing.Point(95, 95);
            this.lastDateLabel.Name = "lastDateLabel";
            this.lastDateLabel.Size = new System.Drawing.Size(93, 13);
            this.lastDateLabel.TabIndex = 5;
            this.lastDateLabel.Text = "Last calculated on";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(312, 184);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(226, 184);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 2;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.BackColor = System.Drawing.SystemColors.Control;
            this.infoLabel.Location = new System.Drawing.Point(12, 9);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(380, 80);
            this.infoLabel.TabIndex = 4;
            this.infoLabel.Text = "If you use monthly billing instead of daily, then this is where you change the ag" +
    "ing date every month. Otherwise, it\'s not necessary to manually run aging. It\'s " +
    "all handled automatically.";
            // 
            // dateTextBox
            // 
            this.dateTextBox.Location = new System.Drawing.Point(194, 118);
            this.dateTextBox.Name = "dateTextBox";
            this.dateTextBox.Size = new System.Drawing.Size(100, 20);
            this.dateTextBox.TabIndex = 1;
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(110, 121);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(78, 13);
            this.dateLabel.TabIndex = 0;
            this.dateLabel.Text = "Calculate as of";
            // 
            // FormAging
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(404, 221);
            this.Controls.Add(this.dateTextBox);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.lastDateTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.lastDateLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAging";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Calculate Aging";
            this.Load += new System.EventHandler(this.FormAging_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.Label lastDateLabel;
		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.Label infoLabel;
		private System.Windows.Forms.TextBox lastDateTextBox;
		private System.Windows.Forms.TextBox dateTextBox;
		private System.Windows.Forms.Label dateLabel;
	}
}
