namespace Imedisoft.Forms
{
	partial class FormAutoNoteResponsePicker
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
            this.cancelButton = new OpenDental.UI.Button();
            this.responseTextLabel = new System.Windows.Forms.Label();
            this.responseTextTextBox = new System.Windows.Forms.TextBox();
            this.autoNotesGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(226, 484);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(312, 484);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // responseTextLabel
            // 
            this.responseTextLabel.AutoSize = true;
            this.responseTextLabel.Location = new System.Drawing.Point(35, 15);
            this.responseTextLabel.Name = "responseTextLabel";
            this.responseTextLabel.Size = new System.Drawing.Size(79, 13);
            this.responseTextLabel.TabIndex = 1;
            this.responseTextLabel.Text = "Response Text";
            // 
            // responseTextTextBox
            // 
            this.responseTextTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.responseTextTextBox.Location = new System.Drawing.Point(120, 12);
            this.responseTextTextBox.Name = "responseTextTextBox";
            this.responseTextTextBox.Size = new System.Drawing.Size(272, 20);
            this.responseTextTextBox.TabIndex = 2;
            // 
            // autoNotesGrid
            // 
            this.autoNotesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoNotesGrid.ColorSelectedRow = System.Drawing.SystemColors.Highlight;
            this.autoNotesGrid.Location = new System.Drawing.Point(12, 38);
            this.autoNotesGrid.Name = "autoNotesGrid";
            this.autoNotesGrid.Size = new System.Drawing.Size(380, 440);
            this.autoNotesGrid.TabIndex = 3;
            this.autoNotesGrid.Title = "Available Auto Notes";
            this.autoNotesGrid.TranslationName = "FormAutoNoteEdit";
            // 
            // FormAutoNoteResponsePicker
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(404, 521);
            this.Controls.Add(this.autoNotesGrid);
            this.Controls.Add(this.responseTextLabel);
            this.Controls.Add(this.responseTextTextBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAutoNoteResponsePicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Note Response Picker";
            this.Load += new System.EventHandler(this.FormAutoNoteResponsePicker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label responseTextLabel;
		private System.Windows.Forms.TextBox responseTextTextBox;
		private OpenDental.UI.ODGrid autoNotesGrid;
	}
}
