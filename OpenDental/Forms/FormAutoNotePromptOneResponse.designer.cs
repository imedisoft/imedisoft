namespace Imedisoft.Forms
{
	partial class FormAutoNotePromptOneResponse
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
            this.promptListBox = new System.Windows.Forms.ListBox();
            this.previewButton = new OpenDental.UI.Button();
            this.removeButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.backButton = new OpenDental.UI.Button();
            this.nextButton = new OpenDental.UI.Button();
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
            // promptListBox
            // 
            this.promptListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.promptListBox.FormattingEnabled = true;
            this.promptListBox.HorizontalScrollbar = true;
            this.promptListBox.IntegralHeight = false;
            this.promptListBox.Location = new System.Drawing.Point(12, 62);
            this.promptListBox.MultiColumn = true;
            this.promptListBox.Name = "promptListBox";
            this.promptListBox.Size = new System.Drawing.Size(400, 200);
            this.promptListBox.TabIndex = 1;
            this.promptListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PromptListBox_MouseDoubleClick);
            // 
            // previewButton
            // 
            this.previewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.previewButton.Location = new System.Drawing.Point(130, 265);
            this.previewButton.Margin = new System.Windows.Forms.Padding(0, 0, 1, 3);
            this.previewButton.Name = "previewButton";
            this.previewButton.Size = new System.Drawing.Size(100, 25);
            this.previewButton.TabIndex = 3;
            this.previewButton.Text = "&Preview";
            this.previewButton.Click += new System.EventHandler(this.PreviewButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeButton.Location = new System.Drawing.Point(231, 265);
            this.removeButton.Margin = new System.Windows.Forms.Padding(0, 0, 1, 3);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(100, 25);
            this.removeButton.TabIndex = 4;
            this.removeButton.Text = "&Remove Prompt";
            this.removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
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
            // FormAutoNotePromptOneResp
            // 
            this.AcceptButton = this.nextButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(424, 361);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.previewButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.promptListBox);
            this.Controls.Add(this.promptLabel);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(377, 210);
            this.Name = "FormAutoNotePromptOneResp";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Prompt One Response";
            this.Load += new System.EventHandler(this.FormAutoNotePromptOneResp_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label promptLabel;
		private System.Windows.Forms.ListBox promptListBox;
		private OpenDental.UI.Button removeButton;
		private OpenDental.UI.Button previewButton;
		private OpenDental.UI.Button backButton;
		private OpenDental.UI.Button nextButton;
	}
}
