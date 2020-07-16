namespace OpenDental
{
	partial class FormConnectionLost
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConnectionLost));
            this.messageLabel = new System.Windows.Forms.Label();
            this.retryButton = new OpenDental.UI.Button();
            this.exitButton = new OpenDental.UI.Button();
            this.containerPanel = new System.Windows.Forms.Panel();
            this.containerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageLabel
            // 
            this.messageLabel.Location = new System.Drawing.Point(11, 8);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(376, 151);
            this.messageLabel.TabIndex = 0;
            this.messageLabel.Text = "Error Message";
            // 
            // retryButton
            // 
            this.retryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.retryButton.Location = new System.Drawing.Point(221, 162);
            this.retryButton.Name = "retryButton";
            this.retryButton.Size = new System.Drawing.Size(80, 25);
            this.retryButton.TabIndex = 1;
            this.retryButton.Text = "Retry";
            this.retryButton.Click += new System.EventHandler(this.RetryButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.Location = new System.Drawing.Point(307, 162);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(80, 25);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "Exit Program";
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // containerPanel
            // 
            this.containerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.containerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.containerPanel.Controls.Add(this.messageLabel);
            this.containerPanel.Controls.Add(this.exitButton);
            this.containerPanel.Controls.Add(this.retryButton);
            this.containerPanel.Location = new System.Drawing.Point(0, 0);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(400, 200);
            this.containerPanel.TabIndex = 0;
            // 
            // FormConnectionLost
            // 
            this.ClientSize = new System.Drawing.Size(400, 200);
            this.Controls.Add(this.containerPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 200);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "FormConnectionLost";
            this.ShowInTaskbar = false;
            this.Text = "Connection Lost";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormConnectionLost_FormClosing);
            this.Load += new System.EventHandler(this.FormConnectionLost_Load);
            this.containerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button retryButton;
		private OpenDental.UI.Button exitButton;
		private System.Windows.Forms.Label messageLabel;
		private System.Windows.Forms.Panel containerPanel;
	}
}
