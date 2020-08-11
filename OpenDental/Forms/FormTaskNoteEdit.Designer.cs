namespace Imedisoft.Forms
{
	partial class FormTaskNoteEdit
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
            this.dateTimeLabel = new System.Windows.Forms.Label();
            this.noteTextBox = new OpenDental.ODtextBox();
            this.noteLabel = new System.Windows.Forms.Label();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.dateTimeTextBox = new System.Windows.Forms.TextBox();
            this.deleteButton = new OpenDental.UI.Button();
            this.autoNoteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(426, 304);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(512, 304);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // dateTimeLabel
            // 
            this.dateTimeLabel.AutoSize = true;
            this.dateTimeLabel.Location = new System.Drawing.Point(32, 15);
            this.dateTimeLabel.Name = "dateTimeLabel";
            this.dateTimeLabel.Size = new System.Drawing.Size(62, 13);
            this.dateTimeLabel.TabIndex = 5;
            this.dateTimeLabel.Text = "Date / Time";
            // 
            // noteTextBox
            // 
            this.noteTextBox.AcceptsTab = true;
            this.noteTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noteTextBox.DetectLinksEnabled = false;
            this.noteTextBox.DetectUrls = false;
            this.noteTextBox.HasAutoNotes = true;
            this.noteTextBox.Location = new System.Drawing.Point(100, 64);
            this.noteTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.noteTextBox.Name = "noteTextBox";
            this.noteTextBox.QuickPasteType = OpenDentBusiness.QuickPasteType.Task;
            this.noteTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.noteTextBox.Size = new System.Drawing.Size(492, 230);
            this.noteTextBox.TabIndex = 1;
            this.noteTextBox.Text = "";
            // 
            // noteLabel
            // 
            this.noteLabel.AutoSize = true;
            this.noteLabel.Location = new System.Drawing.Point(64, 67);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.Size = new System.Drawing.Size(30, 13);
            this.noteLabel.TabIndex = 0;
            this.noteLabel.Text = "Note";
            // 
            // userTextBox
            // 
            this.userTextBox.Location = new System.Drawing.Point(100, 38);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.ReadOnly = true;
            this.userTextBox.Size = new System.Drawing.Size(150, 20);
            this.userTextBox.TabIndex = 8;
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(65, 41);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(29, 13);
            this.userLabel.TabIndex = 7;
            this.userLabel.Text = "User";
            // 
            // dateTimeTextBox
            // 
            this.dateTimeTextBox.Location = new System.Drawing.Point(100, 12);
            this.dateTimeTextBox.Name = "dateTimeTextBox";
            this.dateTimeTextBox.Size = new System.Drawing.Size(180, 20);
            this.dateTimeTextBox.TabIndex = 6;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTrash;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(12, 304);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.TabStop = false;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // autoNoteButton
            // 
            this.autoNoteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoNoteButton.Image = global::Imedisoft.Properties.Resources.IconMagic;
            this.autoNoteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.autoNoteButton.Location = new System.Drawing.Point(482, 33);
            this.autoNoteButton.Name = "autoNoteButton";
            this.autoNoteButton.Size = new System.Drawing.Size(110, 25);
            this.autoNoteButton.TabIndex = 9;
            this.autoNoteButton.Text = "Auto Note";
            this.autoNoteButton.Click += new System.EventHandler(this.AutoNoteButton_Click);
            // 
            // FormTaskNoteEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(604, 341);
            this.Controls.Add(this.autoNoteButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.dateTimeTextBox);
            this.Controls.Add(this.userTextBox);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.noteTextBox);
            this.Controls.Add(this.noteLabel);
            this.Controls.Add(this.dateTimeLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTaskNoteEdit";
            this.Text = "Task Note Edit";
            this.Load += new System.EventHandler(this.FormTaskNoteEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label dateTimeLabel;
		private OpenDental.ODtextBox noteTextBox;
		private System.Windows.Forms.Label noteLabel;
		private System.Windows.Forms.TextBox userTextBox;
		private System.Windows.Forms.Label userLabel;
		private System.Windows.Forms.TextBox dateTimeTextBox;
		private OpenDental.UI.Button deleteButton;
		private OpenDental.UI.Button autoNoteButton;
	}
}
