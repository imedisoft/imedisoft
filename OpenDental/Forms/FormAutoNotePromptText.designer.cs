namespace Imedisoft.Forms
{
	partial class FormAutoNotePromptText
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
            this.promptLabel = new System.Windows.Forms.Label();
            this.cancelButton = new OpenDental.UI.Button();
            this.removeButton = new OpenDental.UI.Button();
            this.promptTextBox = new System.Windows.Forms.TextBox();
            this.backButton = new OpenDental.UI.Button();
            this.nextButton = new OpenDental.UI.Button();
            this.skipButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // promptLabel
            // 
            this.promptLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.promptLabel.Location = new System.Drawing.Point(12, 9);
            this.promptLabel.Name = "promptLabel";
            this.promptLabel.Size = new System.Drawing.Size(400, 50);
            this.promptLabel.TabIndex = 0;
            this.promptLabel.Text = "Prompt";
            this.promptLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(332, 324);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Exit";
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeButton.Location = new System.Drawing.Point(130, 265);
            this.removeButton.Margin = new System.Windows.Forms.Padding(0, 0, 1, 3);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(100, 25);
            this.removeButton.TabIndex = 3;
            this.removeButton.Text = "&Remove Prompt";
            this.removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // promptTextBox
            // 
            this.promptTextBox.AcceptsReturn = true;
            this.promptTextBox.AcceptsTab = true;
            this.promptTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.promptTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.promptTextBox.Location = new System.Drawing.Point(12, 62);
            this.promptTextBox.Multiline = true;
            this.promptTextBox.Name = "promptTextBox";
            this.promptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.promptTextBox.Size = new System.Drawing.Size(400, 200);
            this.promptTextBox.TabIndex = 1;
            // 
            // backButton
            // 
            this.backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.backButton.Image = global::Imedisoft.Properties.Resources.IconArrowLeft;
            this.backButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.backButton.Location = new System.Drawing.Point(49, 265);
            this.backButton.Margin = new System.Windows.Forms.Padding(0, 0, 1, 3);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(80, 25);
            this.backButton.TabIndex = 2;
            this.backButton.Text = "&Back";
            this.backButton.Visible = false;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nextButton.Image = global::Imedisoft.Properties.Resources.IconArrowRight;
            this.nextButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nextButton.Location = new System.Drawing.Point(332, 265);
            this.nextButton.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(80, 25);
            this.nextButton.TabIndex = 5;
            this.nextButton.Text = "&Next";
            this.nextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // skipButton
            // 
            this.skipButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skipButton.Location = new System.Drawing.Point(231, 265);
            this.skipButton.Margin = new System.Windows.Forms.Padding(0, 0, 1, 3);
            this.skipButton.Name = "skipButton";
            this.skipButton.Size = new System.Drawing.Size(100, 25);
            this.skipButton.TabIndex = 4;
            this.skipButton.Text = "&Skip For Now";
            this.skipButton.Click += new System.EventHandler(this.SkipButton_Click);
            // 
            // FormAutoNotePromptText
            // 
            this.AcceptButton = this.nextButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(424, 361);
            this.Controls.Add(this.skipButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.promptTextBox);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.promptLabel);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(377, 210);
            this.Name = "FormAutoNotePromptText";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Prompt Text";
            this.Load += new System.EventHandler(this.FormAutoNotePromptText_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label promptLabel;
		private OpenDental.UI.Button removeButton;
		private System.Windows.Forms.TextBox promptTextBox;
		private OpenDental.UI.Button backButton;
		private OpenDental.UI.Button nextButton;
		private OpenDental.UI.Button skipButton;
	}
}
