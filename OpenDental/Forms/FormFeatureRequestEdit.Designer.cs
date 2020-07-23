namespace Imedisoft.Forms
{
	partial class FormRequestEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRequestEdit));
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.detailsLabel = new System.Windows.Forms.Label();
            this.detailsTextBox = new System.Windows.Forms.TextBox();
            this.difficultyLabel = new System.Windows.Forms.Label();
            this.difficultyTextBox = new System.Windows.Forms.TextBox();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.pledgeGroupBox = new System.Windows.Forms.GroupBox();
            this.pledgeInfoLabel = new System.Windows.Forms.Label();
            this.pledgeTextBox = new System.Windows.Forms.TextBox();
            this.pledgeLabel = new System.Windows.Forms.Label();
            this.criticalCheckBox = new System.Windows.Forms.CheckBox();
            this.bountyTextBox = new System.Windows.Forms.TextBox();
            this.bountyLabel = new System.Windows.Forms.Label();
            this.submitterTextBox = new System.Windows.Forms.TextBox();
            this.submitterLabel = new System.Windows.Forms.Label();
            this.priorityTextBox = new System.Windows.Forms.TextBox();
            this.priorityLabel = new System.Windows.Forms.Label();
            this.difficultyMaxLabel = new System.Windows.Forms.Label();
            this.noteLabel = new System.Windows.Forms.Label();
            this.noteTextBox = new System.Windows.Forms.TextBox();
            this.saveNoteButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.notesGrid = new OpenDental.UI.ODGrid();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.idLabel = new System.Windows.Forms.Label();
            this.priorityMaxLabel = new System.Windows.Forms.Label();
            this.pledgeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(25, 15);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(89, 13);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Short Description";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Location = new System.Drawing.Point(120, 12);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(428, 30);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // detailsLabel
            // 
            this.detailsLabel.AutoSize = true;
            this.detailsLabel.Location = new System.Drawing.Point(75, 51);
            this.detailsLabel.Name = "detailsLabel";
            this.detailsLabel.Size = new System.Drawing.Size(39, 13);
            this.detailsLabel.TabIndex = 2;
            this.detailsLabel.Text = "Details";
            // 
            // detailsTextBox
            // 
            this.detailsTextBox.AcceptsReturn = true;
            this.detailsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.detailsTextBox.Location = new System.Drawing.Point(120, 48);
            this.detailsTextBox.Multiline = true;
            this.detailsTextBox.Name = "detailsTextBox";
            this.detailsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.detailsTextBox.Size = new System.Drawing.Size(428, 70);
            this.detailsTextBox.TabIndex = 3;
            // 
            // difficultyLabel
            // 
            this.difficultyLabel.AutoSize = true;
            this.difficultyLabel.Location = new System.Drawing.Point(65, 153);
            this.difficultyLabel.Name = "difficultyLabel";
            this.difficultyLabel.Size = new System.Drawing.Size(49, 13);
            this.difficultyLabel.TabIndex = 6;
            this.difficultyLabel.Text = "Difficulty";
            // 
            // difficultyTextBox
            // 
            this.difficultyTextBox.Location = new System.Drawing.Point(120, 150);
            this.difficultyTextBox.Name = "difficultyTextBox";
            this.difficultyTextBox.ReadOnly = true;
            this.difficultyTextBox.Size = new System.Drawing.Size(40, 20);
            this.difficultyTextBox.TabIndex = 7;
            this.difficultyTextBox.TabStop = false;
            this.difficultyTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(120, 176);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.Size = new System.Drawing.Size(200, 20);
            this.statusTextBox.TabIndex = 17;
            this.statusTextBox.TabStop = false;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(76, 179);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(38, 13);
            this.statusLabel.TabIndex = 16;
            this.statusLabel.Text = "Status";
            // 
            // pledgeGroupBox
            // 
            this.pledgeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pledgeGroupBox.Controls.Add(this.pledgeInfoLabel);
            this.pledgeGroupBox.Controls.Add(this.pledgeTextBox);
            this.pledgeGroupBox.Controls.Add(this.pledgeLabel);
            this.pledgeGroupBox.Controls.Add(this.criticalCheckBox);
            this.pledgeGroupBox.Location = new System.Drawing.Point(554, 12);
            this.pledgeGroupBox.Name = "pledgeGroupBox";
            this.pledgeGroupBox.Size = new System.Drawing.Size(228, 132);
            this.pledgeGroupBox.TabIndex = 18;
            this.pledgeGroupBox.TabStop = false;
            this.pledgeGroupBox.Text = "Pledge";
            // 
            // pledgeInfoLabel
            // 
            this.pledgeInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pledgeInfoLabel.Location = new System.Drawing.Point(6, 16);
            this.pledgeInfoLabel.Name = "pledgeInfoLabel";
            this.pledgeInfoLabel.Size = new System.Drawing.Size(216, 60);
            this.pledgeInfoLabel.TabIndex = 0;
            this.pledgeInfoLabel.Text = "Pledges are neither required nor requested. They are for unusual situations where" +
    " a feature is extremely important to someone.";
            // 
            // pledgeTextBox
            // 
            this.pledgeTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.pledgeTextBox.Location = new System.Drawing.Point(110, 79);
            this.pledgeTextBox.Name = "pledgeTextBox";
            this.pledgeTextBox.Size = new System.Drawing.Size(70, 20);
            this.pledgeTextBox.TabIndex = 2;
            // 
            // pledgeLabel
            // 
            this.pledgeLabel.AutoSize = true;
            this.pledgeLabel.Location = new System.Drawing.Point(16, 82);
            this.pledgeLabel.Name = "pledgeLabel";
            this.pledgeLabel.Size = new System.Drawing.Size(88, 13);
            this.pledgeLabel.TabIndex = 1;
            this.pledgeLabel.Text = "Pledge Amount $";
            // 
            // criticalCheckBox
            // 
            this.criticalCheckBox.AutoSize = true;
            this.criticalCheckBox.Location = new System.Drawing.Point(110, 105);
            this.criticalCheckBox.Name = "criticalCheckBox";
            this.criticalCheckBox.Size = new System.Drawing.Size(70, 17);
            this.criticalCheckBox.TabIndex = 3;
            this.criticalCheckBox.Text = "Is Critical";
            this.criticalCheckBox.UseVisualStyleBackColor = true;
            this.criticalCheckBox.Click += new System.EventHandler(this.CriticalCheckBox_Click);
            // 
            // bountyTextBox
            // 
            this.bountyTextBox.Location = new System.Drawing.Point(485, 150);
            this.bountyTextBox.Name = "bountyTextBox";
            this.bountyTextBox.ReadOnly = true;
            this.bountyTextBox.Size = new System.Drawing.Size(63, 20);
            this.bountyTextBox.TabIndex = 15;
            this.bountyTextBox.TabStop = false;
            this.bountyTextBox.Visible = false;
            // 
            // bountyLabel
            // 
            this.bountyLabel.AutoSize = true;
            this.bountyLabel.Location = new System.Drawing.Point(429, 153);
            this.bountyLabel.Name = "bountyLabel";
            this.bountyLabel.Size = new System.Drawing.Size(50, 13);
            this.bountyLabel.TabIndex = 14;
            this.bountyLabel.Text = "Bounty $";
            this.bountyLabel.Visible = false;
            // 
            // submitterTextBox
            // 
            this.submitterTextBox.Location = new System.Drawing.Point(120, 124);
            this.submitterTextBox.Name = "submitterTextBox";
            this.submitterTextBox.Size = new System.Drawing.Size(428, 20);
            this.submitterTextBox.TabIndex = 5;
            this.submitterTextBox.Visible = false;
            // 
            // submitterLabel
            // 
            this.submitterLabel.AutoSize = true;
            this.submitterLabel.Location = new System.Drawing.Point(61, 127);
            this.submitterLabel.Name = "submitterLabel";
            this.submitterLabel.Size = new System.Drawing.Size(53, 13);
            this.submitterLabel.TabIndex = 4;
            this.submitterLabel.Text = "Submitter";
            this.submitterLabel.Visible = false;
            // 
            // priorityTextBox
            // 
            this.priorityTextBox.Location = new System.Drawing.Point(250, 150);
            this.priorityTextBox.Name = "priorityTextBox";
            this.priorityTextBox.ReadOnly = true;
            this.priorityTextBox.Size = new System.Drawing.Size(40, 20);
            this.priorityTextBox.TabIndex = 10;
            this.priorityTextBox.TabStop = false;
            this.priorityTextBox.Visible = false;
            // 
            // priorityLabel
            // 
            this.priorityLabel.AutoSize = true;
            this.priorityLabel.Location = new System.Drawing.Point(203, 153);
            this.priorityLabel.Name = "priorityLabel";
            this.priorityLabel.Size = new System.Drawing.Size(41, 13);
            this.priorityLabel.TabIndex = 9;
            this.priorityLabel.Text = "Priority";
            this.priorityLabel.Visible = false;
            // 
            // difficultyMaxLabel
            // 
            this.difficultyMaxLabel.AutoSize = true;
            this.difficultyMaxLabel.Location = new System.Drawing.Point(163, 153);
            this.difficultyMaxLabel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.difficultyMaxLabel.Name = "difficultyMaxLabel";
            this.difficultyMaxLabel.Size = new System.Drawing.Size(23, 13);
            this.difficultyMaxLabel.TabIndex = 8;
            this.difficultyMaxLabel.Text = "/10";
            // 
            // noteLabel
            // 
            this.noteLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.noteLabel.AutoSize = true;
            this.noteLabel.Location = new System.Drawing.Point(202, 550);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.Size = new System.Drawing.Size(582, 13);
            this.noteLabel.TabIndex = 20;
            this.noteLabel.Text = "This discussion is very leisurely. Nobody necessarily checks it for new messages." +
    " Try to prepend your name to the note.";
            // 
            // noteTextBox
            // 
            this.noteTextBox.AcceptsReturn = true;
            this.noteTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noteTextBox.BackColor = System.Drawing.Color.White;
            this.noteTextBox.Location = new System.Drawing.Point(205, 566);
            this.noteTextBox.Multiline = true;
            this.noteTextBox.Name = "noteTextBox";
            this.noteTextBox.Size = new System.Drawing.Size(491, 90);
            this.noteTextBox.TabIndex = 22;
            // 
            // saveNoteButton
            // 
            this.saveNoteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveNoteButton.Image = global::Imedisoft.Properties.Resources.IconPlus;
            this.saveNoteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.saveNoteButton.Location = new System.Drawing.Point(139, 566);
            this.saveNoteButton.Name = "saveNoteButton";
            this.saveNoteButton.Size = new System.Drawing.Size(60, 25);
            this.saveNoteButton.TabIndex = 21;
            this.saveNoteButton.Text = "Save";
            this.saveNoteButton.Click += new System.EventHandler(this.SaveNoteButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTrash;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(12, 631);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 23;
            this.deleteButton.Text = "Delete";
            this.deleteButton.Visible = false;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // notesGrid
            // 
            this.notesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.notesGrid.Location = new System.Drawing.Point(12, 204);
            this.notesGrid.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.notesGrid.Name = "notesGrid";
            this.notesGrid.Size = new System.Drawing.Size(770, 336);
            this.notesGrid.TabIndex = 19;
            this.notesGrid.Title = "Discussion";
            this.notesGrid.TranslationName = "TableDiscussion";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(702, 600);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 24;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(702, 631);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 25;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // idTextBox
            // 
            this.idTextBox.Location = new System.Drawing.Point(360, 150);
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.ReadOnly = true;
            this.idTextBox.Size = new System.Drawing.Size(40, 20);
            this.idTextBox.TabIndex = 13;
            this.idTextBox.TabStop = false;
            // 
            // idLabel
            // 
            this.idLabel.AutoSize = true;
            this.idLabel.Location = new System.Drawing.Point(336, 153);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(18, 13);
            this.idLabel.TabIndex = 12;
            this.idLabel.Text = "ID";
            // 
            // priorityMaxLabel
            // 
            this.priorityMaxLabel.AutoSize = true;
            this.priorityMaxLabel.Location = new System.Drawing.Point(293, 153);
            this.priorityMaxLabel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.priorityMaxLabel.Name = "priorityMaxLabel";
            this.priorityMaxLabel.Size = new System.Drawing.Size(23, 13);
            this.priorityMaxLabel.TabIndex = 11;
            this.priorityMaxLabel.Text = "/10";
            // 
            // FormRequestEdit
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(794, 668);
            this.Controls.Add(this.bountyTextBox);
            this.Controls.Add(this.bountyLabel);
            this.Controls.Add(this.idTextBox);
            this.Controls.Add(this.idLabel);
            this.Controls.Add(this.noteTextBox);
            this.Controls.Add(this.saveNoteButton);
            this.Controls.Add(this.noteLabel);
            this.Controls.Add(this.difficultyTextBox);
            this.Controls.Add(this.priorityMaxLabel);
            this.Controls.Add(this.difficultyMaxLabel);
            this.Controls.Add(this.priorityTextBox);
            this.Controls.Add(this.priorityLabel);
            this.Controls.Add(this.submitterLabel);
            this.Controls.Add(this.submitterTextBox);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.pledgeGroupBox);
            this.Controls.Add(this.notesGrid);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.difficultyLabel);
            this.Controls.Add(this.detailsLabel);
            this.Controls.Add(this.detailsTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormRequestEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Feature Request";
            this.Load += new System.EventHandler(this.FormRequestEdit_Load);
            this.pledgeGroupBox.ResumeLayout(false);
            this.pledgeGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.Label detailsLabel;
		private System.Windows.Forms.TextBox detailsTextBox;
		private System.Windows.Forms.Label difficultyLabel;
		private System.Windows.Forms.TextBox difficultyTextBox;
		private System.Windows.Forms.TextBox statusTextBox;
		private System.Windows.Forms.Label statusLabel;
		private OpenDental.UI.ODGrid notesGrid;
		private System.Windows.Forms.GroupBox pledgeGroupBox;
		private System.Windows.Forms.Label pledgeInfoLabel;
		private System.Windows.Forms.TextBox pledgeTextBox;
		private System.Windows.Forms.Label pledgeLabel;
		private System.Windows.Forms.CheckBox criticalCheckBox;
		private OpenDental.UI.Button deleteButton;
		private System.Windows.Forms.TextBox submitterTextBox;
		private System.Windows.Forms.Label submitterLabel;
		private System.Windows.Forms.TextBox priorityTextBox;
		private System.Windows.Forms.Label priorityLabel;
		private System.Windows.Forms.Label difficultyMaxLabel;
		private System.Windows.Forms.Label noteLabel;
		private OpenDental.UI.Button saveNoteButton;
		private System.Windows.Forms.TextBox noteTextBox;
		private System.Windows.Forms.TextBox idTextBox;
		private System.Windows.Forms.Label idLabel;
		private System.Windows.Forms.TextBox bountyTextBox;
		private System.Windows.Forms.Label bountyLabel;
        private System.Windows.Forms.Label priorityMaxLabel;
    }
}
