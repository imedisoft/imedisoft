namespace Imedisoft.Forms
{
    partial class FormTaskEdit
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
            this.repeatPanel = new OpenDental.UI.PanelOD();
            this.repeatDateLabel = new System.Windows.Forms.Label();
            this.repeatIntervalLabel = new System.Windows.Forms.Label();
            this.repeatDateTextBox = new System.Windows.Forms.TextBox();
            this.repeatIntervalComboBox = new System.Windows.Forms.ComboBox();
            this.auditButton = new OpenDental.UI.Button();
            this.colorButton = new System.Windows.Forms.Button();
            this.priorityLabel = new System.Windows.Forms.Label();
            this.priorityComboBox = new System.Windows.Forms.ComboBox();
            this.doneCheckBox = new System.Windows.Forms.RadioButton();
            this.newCheckBox = new System.Windows.Forms.RadioButton();
            this.userPickButton = new OpenDental.UI.Button();
            this.addNoteButton = new OpenDental.UI.Button();
            this.taskListTextBox = new System.Windows.Forms.TextBox();
            this.taskListLabel = new System.Windows.Forms.Label();
            this.sendButton = new OpenDental.UI.Button();
            this.replyLabel = new System.Windows.Forms.Label();
            this.replyButton = new OpenDental.UI.Button();
            this.dateCompletedNowButton = new OpenDental.UI.Button();
            this.dateCompletedTextBox = new System.Windows.Forms.TextBox();
            this.dateCompletedLabel = new System.Windows.Forms.Label();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.deleteButton = new OpenDental.UI.Button();
            this.dateStartNowButton = new OpenDental.UI.Button();
            this.dateStartTextBox = new System.Windows.Forms.TextBox();
            this.dateStartLabel = new System.Windows.Forms.Label();
            this.copyButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.taskDescriptionNotesSplitContainer = new OpenDental.SplitContainerNoFlicker();
            this.editAutoNoteButton = new OpenDental.UI.Button();
            this.autoNoteButton = new OpenDental.UI.Button();
            this.descriptionTextBox = new OpenDental.ODtextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.notesGrid = new OpenDental.UI.ODGrid();
            this.taskChangedLabel = new System.Windows.Forms.Label();
            this.refreshButton = new OpenDental.UI.Button();
            this.dateCreatedTextBox = new System.Windows.Forms.TextBox();
            this.dateCreatedLabel = new System.Windows.Forms.Label();
            this.patientTextBox = new System.Windows.Forms.TextBox();
            this.patientLabel = new System.Windows.Forms.Label();
            this.patientGoButton = new OpenDental.UI.Button();
            this.patientPickButton = new OpenDental.UI.Button();
            this.appointmentTextBox = new System.Windows.Forms.TextBox();
            this.appointmentLabel = new System.Windows.Forms.Label();
            this.appointmentGoButton = new OpenDental.UI.Button();
            this.appointmentPickButton = new OpenDental.UI.Button();
            this.repeatPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.taskDescriptionNotesSplitContainer)).BeginInit();
            this.taskDescriptionNotesSplitContainer.Panel1.SuspendLayout();
            this.taskDescriptionNotesSplitContainer.Panel2.SuspendLayout();
            this.taskDescriptionNotesSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // repeatPanel
            // 
            this.repeatPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.repeatPanel.BorderColor = System.Drawing.Color.Transparent;
            this.repeatPanel.Controls.Add(this.repeatDateLabel);
            this.repeatPanel.Controls.Add(this.repeatIntervalLabel);
            this.repeatPanel.Controls.Add(this.repeatDateTextBox);
            this.repeatPanel.Controls.Add(this.repeatIntervalComboBox);
            this.repeatPanel.Location = new System.Drawing.Point(12, 558);
            this.repeatPanel.Name = "repeatPanel";
            this.repeatPanel.Size = new System.Drawing.Size(280, 60);
            this.repeatPanel.TabIndex = 1;
            // 
            // repeatDateLabel
            // 
            this.repeatDateLabel.AutoSize = true;
            this.repeatDateLabel.Location = new System.Drawing.Point(64, 33);
            this.repeatDateLabel.Name = "repeatDateLabel";
            this.repeatDateLabel.Size = new System.Drawing.Size(30, 13);
            this.repeatDateLabel.TabIndex = 3;
            this.repeatDateLabel.Text = "Date";
            // 
            // repeatIntervalLabel
            // 
            this.repeatIntervalLabel.AutoSize = true;
            this.repeatIntervalLabel.Location = new System.Drawing.Point(52, 6);
            this.repeatIntervalLabel.Name = "repeatIntervalLabel";
            this.repeatIntervalLabel.Size = new System.Drawing.Size(42, 13);
            this.repeatIntervalLabel.TabIndex = 1;
            this.repeatIntervalLabel.Text = "Repeat";
            // 
            // repeatDateTextBox
            // 
            this.repeatDateTextBox.Location = new System.Drawing.Point(100, 30);
            this.repeatDateTextBox.Name = "repeatDateTextBox";
            this.repeatDateTextBox.Size = new System.Drawing.Size(90, 20);
            this.repeatDateTextBox.TabIndex = 4;
            // 
            // repeatIntervalComboBox
            // 
            this.repeatIntervalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.repeatIntervalComboBox.FormattingEnabled = true;
            this.repeatIntervalComboBox.Items.AddRange(new object[] {
            "Never",
            "Once",
            "Daily",
            "Weekly",
            "Monthly"});
            this.repeatIntervalComboBox.Location = new System.Drawing.Point(100, 3);
            this.repeatIntervalComboBox.Name = "repeatIntervalComboBox";
            this.repeatIntervalComboBox.Size = new System.Drawing.Size(150, 21);
            this.repeatIntervalComboBox.TabIndex = 2;
            // 
            // auditButton
            // 
            this.auditButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.auditButton.Location = new System.Drawing.Point(987, 87);
            this.auditButton.Name = "auditButton";
            this.auditButton.Size = new System.Drawing.Size(60, 21);
            this.auditButton.TabIndex = 39;
            this.auditButton.Text = "History";
            this.auditButton.Click += new System.EventHandler(this.AuditButton_Click);
            // 
            // colorButton
            // 
            this.colorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.colorButton.Enabled = false;
            this.colorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorButton.Location = new System.Drawing.Point(956, 87);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(25, 21);
            this.colorButton.TabIndex = 38;
            // 
            // priorityLabel
            // 
            this.priorityLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.priorityLabel.AutoSize = true;
            this.priorityLabel.Location = new System.Drawing.Point(763, 90);
            this.priorityLabel.Name = "priorityLabel";
            this.priorityLabel.Size = new System.Drawing.Size(41, 13);
            this.priorityLabel.TabIndex = 36;
            this.priorityLabel.Text = "Priority";
            // 
            // priorityComboBox
            // 
            this.priorityComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.priorityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priorityComboBox.FormattingEnabled = true;
            this.priorityComboBox.Location = new System.Drawing.Point(810, 87);
            this.priorityComboBox.Name = "priorityComboBox";
            this.priorityComboBox.Size = new System.Drawing.Size(140, 21);
            this.priorityComboBox.TabIndex = 37;
            this.priorityComboBox.SelectedIndexChanged += new System.EventHandler(this.PriorityComboBox_SelectedIndexChanged);
            // 
            // doneCheckBox
            // 
            this.doneCheckBox.AutoSize = true;
            this.doneCheckBox.Location = new System.Drawing.Point(168, 12);
            this.doneCheckBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.doneCheckBox.Name = "doneCheckBox";
            this.doneCheckBox.Size = new System.Drawing.Size(50, 17);
            this.doneCheckBox.TabIndex = 13;
            this.doneCheckBox.Text = "Done";
            this.doneCheckBox.Click += new System.EventHandler(this.DoneCheckBox_Click);
            // 
            // newCheckBox
            // 
            this.newCheckBox.AutoSize = true;
            this.newCheckBox.Checked = true;
            this.newCheckBox.Location = new System.Drawing.Point(119, 12);
            this.newCheckBox.Name = "newCheckBox";
            this.newCheckBox.Size = new System.Drawing.Size(46, 17);
            this.newCheckBox.TabIndex = 12;
            this.newCheckBox.TabStop = true;
            this.newCheckBox.Text = "New";
            this.newCheckBox.Click += new System.EventHandler(this.NewCheckBox_Click);
            // 
            // userPickButton
            // 
            this.userPickButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.userPickButton.Location = new System.Drawing.Point(956, 35);
            this.userPickButton.Name = "userPickButton";
            this.userPickButton.Size = new System.Drawing.Size(25, 20);
            this.userPickButton.TabIndex = 33;
            this.userPickButton.Text = "...";
            this.userPickButton.Click += new System.EventHandler(this.UserPickButton_Click);
            // 
            // addNoteButton
            // 
            this.addNoteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addNoteButton.Image = global::Imedisoft.Properties.Resources.IconPlus;
            this.addNoteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addNoteButton.Location = new System.Drawing.Point(972, 558);
            this.addNoteButton.Name = "addNoteButton";
            this.addNoteButton.Size = new System.Drawing.Size(80, 25);
            this.addNoteButton.TabIndex = 3;
            this.addNoteButton.Text = "Add";
            this.addNoteButton.Click += new System.EventHandler(this.AddNoteButton_Click);
            // 
            // taskListTextBox
            // 
            this.taskListTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.taskListTextBox.Location = new System.Drawing.Point(810, 61);
            this.taskListTextBox.Name = "taskListTextBox";
            this.taskListTextBox.ReadOnly = true;
            this.taskListTextBox.Size = new System.Drawing.Size(140, 20);
            this.taskListTextBox.TabIndex = 35;
            // 
            // taskListLabel
            // 
            this.taskListLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.taskListLabel.AutoSize = true;
            this.taskListLabel.Location = new System.Drawing.Point(756, 64);
            this.taskListLabel.Name = "taskListLabel";
            this.taskListLabel.Size = new System.Drawing.Size(48, 13);
            this.taskListLabel.TabIndex = 34;
            this.taskListLabel.Text = "Task List";
            // 
            // sendButton
            // 
            this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sendButton.Image = global::Imedisoft.Properties.Resources.IconShare;
            this.sendButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.sendButton.Location = new System.Drawing.Point(420, 624);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(90, 25);
            this.sendButton.TabIndex = 7;
            this.sendButton.Text = "&Send To...";
            this.sendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // replyLabel
            // 
            this.replyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.replyLabel.AutoSize = true;
            this.replyLabel.Location = new System.Drawing.Point(296, 630);
            this.replyLabel.Name = "replyLabel";
            this.replyLabel.Size = new System.Drawing.Size(64, 13);
            this.replyLabel.TabIndex = 6;
            this.replyLabel.Text = "(Send to...)";
            // 
            // replyButton
            // 
            this.replyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.replyButton.Image = global::Imedisoft.Properties.Resources.IconReply;
            this.replyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.replyButton.Location = new System.Drawing.Point(200, 624);
            this.replyButton.Name = "replyButton";
            this.replyButton.Size = new System.Drawing.Size(90, 25);
            this.replyButton.TabIndex = 5;
            this.replyButton.Text = "&Reply";
            this.replyButton.Click += new System.EventHandler(this.ReplyButton_Click);
            // 
            // dateCompletedNowButton
            // 
            this.dateCompletedNowButton.Location = new System.Drawing.Point(265, 87);
            this.dateCompletedNowButton.Name = "dateCompletedNowButton";
            this.dateCompletedNowButton.Size = new System.Drawing.Size(50, 20);
            this.dateCompletedNowButton.TabIndex = 22;
            this.dateCompletedNowButton.Text = "Now";
            this.dateCompletedNowButton.Click += new System.EventHandler(this.DateCompletedNowButton_Click);
            // 
            // dateCompletedTextBox
            // 
            this.dateCompletedTextBox.Location = new System.Drawing.Point(119, 87);
            this.dateCompletedTextBox.Name = "dateCompletedTextBox";
            this.dateCompletedTextBox.Size = new System.Drawing.Size(140, 20);
            this.dateCompletedTextBox.TabIndex = 21;
            // 
            // dateCompletedLabel
            // 
            this.dateCompletedLabel.AutoSize = true;
            this.dateCompletedLabel.Location = new System.Drawing.Point(40, 90);
            this.dateCompletedLabel.Name = "dateCompletedLabel";
            this.dateCompletedLabel.Size = new System.Drawing.Size(73, 13);
            this.dateCompletedLabel.TabIndex = 20;
            this.dateCompletedLabel.Text = "Completed on";
            // 
            // userTextBox
            // 
            this.userTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.userTextBox.Location = new System.Drawing.Point(810, 35);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.ReadOnly = true;
            this.userTextBox.Size = new System.Drawing.Size(140, 20);
            this.userTextBox.TabIndex = 32;
            // 
            // userLabel
            // 
            this.userLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(741, 38);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(63, 13);
            this.userLabel.TabIndex = 31;
            this.userLabel.Text = "Assigned to";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTrash;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(12, 624);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // dateStartNowButton
            // 
            this.dateStartNowButton.Location = new System.Drawing.Point(265, 61);
            this.dateStartNowButton.Name = "dateStartNowButton";
            this.dateStartNowButton.Size = new System.Drawing.Size(50, 20);
            this.dateStartNowButton.TabIndex = 19;
            this.dateStartNowButton.Text = "Now";
            this.dateStartNowButton.Click += new System.EventHandler(this.DateStartNowButton_Click);
            // 
            // dateStartTextBox
            // 
            this.dateStartTextBox.Location = new System.Drawing.Point(119, 61);
            this.dateStartTextBox.Name = "dateStartTextBox";
            this.dateStartTextBox.Size = new System.Drawing.Size(140, 20);
            this.dateStartTextBox.TabIndex = 18;
            // 
            // dateStartLabel
            // 
            this.dateStartLabel.AutoSize = true;
            this.dateStartLabel.Location = new System.Drawing.Point(55, 64);
            this.dateStartLabel.Name = "dateStartLabel";
            this.dateStartLabel.Size = new System.Drawing.Size(58, 13);
            this.dateStartLabel.TabIndex = 17;
            this.dateStartLabel.Text = "Started on";
            // 
            // copyButton
            // 
            this.copyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.copyButton.Image = global::Imedisoft.Properties.Resources.IconCopy;
            this.copyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.copyButton.Location = new System.Drawing.Point(886, 558);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(80, 25);
            this.copyButton.TabIndex = 2;
            this.copyButton.Text = "Copy";
            this.copyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(886, 624);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 10;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(972, 624);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // taskDescriptionNotesSplitContainer
            // 
            this.taskDescriptionNotesSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskDescriptionNotesSplitContainer.Location = new System.Drawing.Point(12, 114);
            this.taskDescriptionNotesSplitContainer.Name = "taskDescriptionNotesSplitContainer";
            this.taskDescriptionNotesSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // taskDescriptionNotesSplitContainer.Panel1
            // 
            this.taskDescriptionNotesSplitContainer.Panel1.Controls.Add(this.editAutoNoteButton);
            this.taskDescriptionNotesSplitContainer.Panel1.Controls.Add(this.autoNoteButton);
            this.taskDescriptionNotesSplitContainer.Panel1.Controls.Add(this.descriptionTextBox);
            this.taskDescriptionNotesSplitContainer.Panel1.Controls.Add(this.descriptionLabel);
            this.taskDescriptionNotesSplitContainer.Panel1MinSize = 106;
            // 
            // taskDescriptionNotesSplitContainer.Panel2
            // 
            this.taskDescriptionNotesSplitContainer.Panel2.Controls.Add(this.notesGrid);
            this.taskDescriptionNotesSplitContainer.Panel2MinSize = 50;
            this.taskDescriptionNotesSplitContainer.Size = new System.Drawing.Size(1040, 438);
            this.taskDescriptionNotesSplitContainer.SplitterDistance = 120;
            this.taskDescriptionNotesSplitContainer.TabIndex = 0;
            this.taskDescriptionNotesSplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.TaskDescriptionNotesSplitContainer_SplitterMoved);
            // 
            // editAutoNoteButton
            // 
            this.editAutoNoteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editAutoNoteButton.Location = new System.Drawing.Point(3, 61);
            this.editAutoNoteButton.Name = "editAutoNoteButton";
            this.editAutoNoteButton.Size = new System.Drawing.Size(100, 25);
            this.editAutoNoteButton.TabIndex = 2;
            this.editAutoNoteButton.Text = "Edit Auto Note";
            this.editAutoNoteButton.UseVisualStyleBackColor = true;
            this.editAutoNoteButton.Click += new System.EventHandler(this.EditAutoNoteButton_Click);
            // 
            // autoNoteButton
            // 
            this.autoNoteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.autoNoteButton.Image = global::Imedisoft.Properties.Resources.IconMagic;
            this.autoNoteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.autoNoteButton.Location = new System.Drawing.Point(3, 92);
            this.autoNoteButton.Name = "autoNoteButton";
            this.autoNoteButton.Size = new System.Drawing.Size(100, 25);
            this.autoNoteButton.TabIndex = 3;
            this.autoNoteButton.Text = "Auto Note";
            this.autoNoteButton.Click += new System.EventHandler(this.AutoNoteButton_Click);
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.AcceptsTab = true;
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.descriptionTextBox.DetectLinksEnabled = false;
            this.descriptionTextBox.DetectUrls = false;
            this.descriptionTextBox.HasAutoNotes = true;
            this.descriptionTextBox.Location = new System.Drawing.Point(109, 6);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.QuickPasteType = OpenDentBusiness.QuickPasteType.Task;
            this.descriptionTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.descriptionTextBox.Size = new System.Drawing.Size(928, 111);
            this.descriptionTextBox.TabIndex = 1;
            this.descriptionTextBox.Text = "";
            this.descriptionTextBox.TextChanged += new System.EventHandler(this.DescriptionTextBox_TextChanged);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(43, 9);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Description";
            // 
            // notesGrid
            // 
            this.notesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notesGrid.Location = new System.Drawing.Point(0, 0);
            this.notesGrid.Name = "notesGrid";
            this.notesGrid.Size = new System.Drawing.Size(1040, 314);
            this.notesGrid.TabIndex = 0;
            this.notesGrid.Title = "Notes";
            this.notesGrid.TranslationName = "FormTaskEdit";
            this.notesGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.NotesGrid_CellDoubleClick);
            // 
            // taskChangedLabel
            // 
            this.taskChangedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.taskChangedLabel.ForeColor = System.Drawing.Color.Red;
            this.taskChangedLabel.Location = new System.Drawing.Point(690, 598);
            this.taskChangedLabel.Name = "taskChangedLabel";
            this.taskChangedLabel.Size = new System.Drawing.Size(185, 23);
            this.taskChangedLabel.TabIndex = 8;
            this.taskChangedLabel.Text = "The task has been changed ";
            this.taskChangedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.taskChangedLabel.Visible = false;
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.Image = global::Imedisoft.Properties.Resources.IconSyncAlt;
            this.refreshButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.refreshButton.Location = new System.Drawing.Point(743, 624);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(3, 3, 60, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(80, 25);
            this.refreshButton.TabIndex = 9;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Visible = false;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // dateCreatedTextBox
            // 
            this.dateCreatedTextBox.Location = new System.Drawing.Point(119, 35);
            this.dateCreatedTextBox.Name = "dateCreatedTextBox";
            this.dateCreatedTextBox.ReadOnly = true;
            this.dateCreatedTextBox.Size = new System.Drawing.Size(140, 20);
            this.dateCreatedTextBox.TabIndex = 16;
            // 
            // dateCreatedLabel
            // 
            this.dateCreatedLabel.AutoSize = true;
            this.dateCreatedLabel.Location = new System.Drawing.Point(52, 38);
            this.dateCreatedLabel.Name = "dateCreatedLabel";
            this.dateCreatedLabel.Size = new System.Drawing.Size(61, 13);
            this.dateCreatedLabel.TabIndex = 15;
            this.dateCreatedLabel.Text = "Created on";
            // 
            // patientTextBox
            // 
            this.patientTextBox.Location = new System.Drawing.Point(410, 36);
            this.patientTextBox.Name = "patientTextBox";
            this.patientTextBox.ReadOnly = true;
            this.patientTextBox.Size = new System.Drawing.Size(180, 20);
            this.patientTextBox.TabIndex = 24;
            // 
            // patientLabel
            // 
            this.patientLabel.AutoSize = true;
            this.patientLabel.Location = new System.Drawing.Point(363, 39);
            this.patientLabel.Name = "patientLabel";
            this.patientLabel.Size = new System.Drawing.Size(41, 13);
            this.patientLabel.TabIndex = 23;
            this.patientLabel.Text = "Patient";
            // 
            // patientGoButton
            // 
            this.patientGoButton.Location = new System.Drawing.Point(652, 36);
            this.patientGoButton.Name = "patientGoButton";
            this.patientGoButton.Size = new System.Drawing.Size(50, 20);
            this.patientGoButton.TabIndex = 26;
            this.patientGoButton.Text = "Go To";
            this.patientGoButton.Click += new System.EventHandler(this.PatientGoButton_Click);
            // 
            // patientPickButton
            // 
            this.patientPickButton.Location = new System.Drawing.Point(596, 36);
            this.patientPickButton.Name = "patientPickButton";
            this.patientPickButton.Size = new System.Drawing.Size(50, 20);
            this.patientPickButton.TabIndex = 25;
            this.patientPickButton.Text = "Change";
            this.patientPickButton.Click += new System.EventHandler(this.PatientPickButton_Click);
            // 
            // appointmentTextBox
            // 
            this.appointmentTextBox.Location = new System.Drawing.Point(410, 62);
            this.appointmentTextBox.Name = "appointmentTextBox";
            this.appointmentTextBox.ReadOnly = true;
            this.appointmentTextBox.Size = new System.Drawing.Size(180, 20);
            this.appointmentTextBox.TabIndex = 28;
            // 
            // appointmentLabel
            // 
            this.appointmentLabel.AutoSize = true;
            this.appointmentLabel.Location = new System.Drawing.Point(336, 65);
            this.appointmentLabel.Name = "appointmentLabel";
            this.appointmentLabel.Size = new System.Drawing.Size(68, 13);
            this.appointmentLabel.TabIndex = 27;
            this.appointmentLabel.Text = "Appointment";
            // 
            // appointmentGoButton
            // 
            this.appointmentGoButton.Location = new System.Drawing.Point(652, 62);
            this.appointmentGoButton.Name = "appointmentGoButton";
            this.appointmentGoButton.Size = new System.Drawing.Size(50, 20);
            this.appointmentGoButton.TabIndex = 30;
            this.appointmentGoButton.Text = "Go To";
            this.appointmentGoButton.Click += new System.EventHandler(this.AppointmentGoButton_Click);
            // 
            // appointmentPickButton
            // 
            this.appointmentPickButton.Location = new System.Drawing.Point(596, 62);
            this.appointmentPickButton.Name = "appointmentPickButton";
            this.appointmentPickButton.Size = new System.Drawing.Size(50, 20);
            this.appointmentPickButton.TabIndex = 29;
            this.appointmentPickButton.Text = "Change";
            this.appointmentPickButton.Click += new System.EventHandler(this.AppointmentPickButton_Click);
            // 
            // FormTaskEdit
            // 
            this.ClientSize = new System.Drawing.Size(1064, 661);
            this.Controls.Add(this.appointmentTextBox);
            this.Controls.Add(this.appointmentLabel);
            this.Controls.Add(this.appointmentGoButton);
            this.Controls.Add(this.appointmentPickButton);
            this.Controls.Add(this.patientTextBox);
            this.Controls.Add(this.patientLabel);
            this.Controls.Add(this.dateCreatedLabel);
            this.Controls.Add(this.patientGoButton);
            this.Controls.Add(this.dateCreatedTextBox);
            this.Controls.Add(this.patientPickButton);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.taskChangedLabel);
            this.Controls.Add(this.repeatPanel);
            this.Controls.Add(this.auditButton);
            this.Controls.Add(this.colorButton);
            this.Controls.Add(this.priorityLabel);
            this.Controls.Add(this.priorityComboBox);
            this.Controls.Add(this.doneCheckBox);
            this.Controls.Add(this.newCheckBox);
            this.Controls.Add(this.userPickButton);
            this.Controls.Add(this.addNoteButton);
            this.Controls.Add(this.taskListTextBox);
            this.Controls.Add(this.taskListLabel);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.replyLabel);
            this.Controls.Add(this.replyButton);
            this.Controls.Add(this.dateCompletedNowButton);
            this.Controls.Add(this.dateCompletedTextBox);
            this.Controls.Add(this.dateCompletedLabel);
            this.Controls.Add(this.userTextBox);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.dateStartNowButton);
            this.Controls.Add(this.dateStartTextBox);
            this.Controls.Add(this.dateStartLabel);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.taskDescriptionNotesSplitContainer);
            this.Name = "FormTaskEdit";
            this.Text = "Task";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTaskEdit_FormClosing);
            this.Load += new System.EventHandler(this.FormTaskListEdit_Load);
            this.repeatPanel.ResumeLayout(false);
            this.repeatPanel.PerformLayout();
            this.taskDescriptionNotesSplitContainer.Panel1.ResumeLayout(false);
            this.taskDescriptionNotesSplitContainer.Panel1.PerformLayout();
            this.taskDescriptionNotesSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.taskDescriptionNotesSplitContainer)).EndInit();
            this.taskDescriptionNotesSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.Label repeatDateLabel;
		private System.Windows.Forms.Label repeatIntervalLabel;
		private OpenDental.ODtextBox descriptionTextBox;
		private System.Windows.Forms.TextBox repeatDateTextBox;
		private System.Windows.Forms.Label dateStartLabel;
		private System.Windows.Forms.TextBox dateStartTextBox;
		private OpenDental.UI.Button dateStartNowButton;
		private OpenDental.UI.Button deleteButton;
		private System.Windows.Forms.TextBox userTextBox;
		private System.Windows.Forms.Label userLabel;
		private OpenDental.UI.Button dateCompletedNowButton;
		private System.Windows.Forms.TextBox dateCompletedTextBox;
		private System.Windows.Forms.Label dateCompletedLabel;
		private System.Windows.Forms.Label replyLabel;
		private OpenDental.UI.Button replyButton;
		private OpenDental.UI.Button sendButton;
		private System.Windows.Forms.TextBox taskListTextBox;
		private System.Windows.Forms.Label taskListLabel;
		private System.Windows.Forms.ComboBox repeatIntervalComboBox;
		private OpenDental.UI.ODGrid notesGrid;
		private OpenDental.UI.Button addNoteButton;
		private OpenDental.UI.Button userPickButton;
		private System.Windows.Forms.RadioButton newCheckBox;
		private System.Windows.Forms.RadioButton doneCheckBox;
		private OpenDental.UI.Button copyButton;
		private System.Windows.Forms.ComboBox priorityComboBox;
		private System.Windows.Forms.Label priorityLabel;
		private OpenDental.UI.Button auditButton;
		private OpenDental.UI.PanelOD repeatPanel;
		private OpenDental.SplitContainerNoFlicker taskDescriptionNotesSplitContainer;
		private OpenDental.UI.Button autoNoteButton;
		private System.Windows.Forms.Label taskChangedLabel;
		private System.Windows.Forms.TextBox dateCreatedTextBox;
		private System.Windows.Forms.Label dateCreatedLabel;
		private OpenDental.UI.Button refreshButton;
		private OpenDental.UI.Button editAutoNoteButton;
        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.TextBox patientTextBox;
        private System.Windows.Forms.Label patientLabel;
        private OpenDental.UI.Button patientGoButton;
        private OpenDental.UI.Button patientPickButton;
        private System.Windows.Forms.TextBox appointmentTextBox;
        private System.Windows.Forms.Label appointmentLabel;
        private OpenDental.UI.Button appointmentGoButton;
        private OpenDental.UI.Button appointmentPickButton;
    }
}
