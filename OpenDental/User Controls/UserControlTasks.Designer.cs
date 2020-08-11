namespace OpenDental {
	partial class UserControlTasks {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlTasks));
            this.taskListsTree = new System.Windows.Forms.TreeView();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.imageListMain = new System.Windows.Forms.ImageList(this.components);
            this.menuEdit = new System.Windows.Forms.ContextMenu();
            this.menuItemDone = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItemEdit = new System.Windows.Forms.MenuItem();
            this.menuItemPriority = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItemCut = new System.Windows.Forms.MenuItem();
            this.menuItemCopy = new System.Windows.Forms.MenuItem();
            this.menuItemPaste = new System.Windows.Forms.MenuItem();
            this.menuItemDelete = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItemSubscribe = new System.Windows.Forms.MenuItem();
            this.menuItemUnsubscribe = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItemSendToMe = new System.Windows.Forms.MenuItem();
            this.menuItemSendAndGoto = new System.Windows.Forms.MenuItem();
            this.menuItemGoto = new System.Windows.Forms.MenuItem();
            this.menuItemMarkRead = new System.Windows.Forms.MenuItem();
            this.menuNavJob = new System.Windows.Forms.MenuItem();
            this.menuDeleteTaken = new System.Windows.Forms.MenuItem();
            this.menuArchive = new System.Windows.Forms.MenuItem();
            this.menuUnarchive = new System.Windows.Forms.MenuItem();
            this.menuTask = new System.Windows.Forms.ContextMenu();
            this.menuItemTaskReminder = new System.Windows.Forms.MenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tasksGrid = new OpenDental.UI.ODGrid();
            this.menuFilter = new System.Windows.Forms.ContextMenu();
            this.menuItemFilterDefault = new System.Windows.Forms.MenuItem();
            this.menuItemFilterNone = new System.Windows.Forms.MenuItem();
            this.menuItemFilterClinic = new System.Windows.Forms.MenuItem();
            this.menuItemFilterRegion = new System.Windows.Forms.MenuItem();
            this.mainToolBar = new OpenDental.UI.ODToolBar();
            this.showArchivedCheckBox = new System.Windows.Forms.CheckBox();
            this.showMineCheckBox = new System.Windows.Forms.CheckBox();
            this.statusComboBox = new System.Windows.Forms.ComboBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // taskListsTree
            // 
            this.taskListsTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskListsTree.HideSelection = false;
            this.taskListsTree.ImageIndex = 0;
            this.taskListsTree.ImageList = this.imageListTree;
            this.taskListsTree.ItemHeight = 18;
            this.taskListsTree.Location = new System.Drawing.Point(0, 26);
            this.taskListsTree.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.taskListsTree.Name = "taskListsTree";
            this.taskListsTree.Scrollable = false;
            this.taskListsTree.SelectedImageIndex = 0;
            this.taskListsTree.ShowPlusMinus = false;
            this.taskListsTree.Size = new System.Drawing.Size(216, 484);
            this.taskListsTree.TabIndex = 7;
            // 
            // imageListTree
            // 
            this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
            this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTree.Images.SetKeyName(0, "TaskList.gif");
            this.imageListTree.Images.SetKeyName(1, "checkBoxChecked.gif");
            this.imageListTree.Images.SetKeyName(2, "checkBoxUnchecked.gif");
            this.imageListTree.Images.SetKeyName(3, "TaskListHighlight.gif");
            this.imageListTree.Images.SetKeyName(4, "checkBoxNew.gif");
            // 
            // imageListMain
            // 
            this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
            this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMain.Images.SetKeyName(0, "TaskListAdd.gif");
            this.imageListMain.Images.SetKeyName(1, "Add.gif");
            // 
            // menuEdit
            // 
            this.menuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemDone,
            this.menuItem4,
            this.menuItemEdit,
            this.menuItemPriority,
            this.menuItem6,
            this.menuItemCut,
            this.menuItemCopy,
            this.menuItemPaste,
            this.menuItemDelete,
            this.menuItem2,
            this.menuItemSubscribe,
            this.menuItemUnsubscribe,
            this.menuItem3,
            this.menuItemSendToMe,
            this.menuItemSendAndGoto,
            this.menuItemGoto,
            this.menuItemMarkRead,
            this.menuNavJob,
            this.menuDeleteTaken,
            this.menuArchive,
            this.menuUnarchive});
            this.menuEdit.Popup += new System.EventHandler(this.menuEdit_Popup);
            // 
            // menuItemDone
            // 
            this.menuItemDone.Index = 0;
            this.menuItemDone.Text = "Done (affects all users)";
            this.menuItemDone.Click += new System.EventHandler(this.menuItemDone_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.Text = "-";
            // 
            // menuItemEdit
            // 
            this.menuItemEdit.Index = 2;
            this.menuItemEdit.Text = "Edit Properties";
            this.menuItemEdit.Click += new System.EventHandler(this.menuItemEdit_Click);
            // 
            // menuItemPriority
            // 
            this.menuItemPriority.Index = 3;
            this.menuItemPriority.Text = "Set Priority";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 4;
            this.menuItem6.Text = "-";
            // 
            // menuItemCut
            // 
            this.menuItemCut.Index = 5;
            this.menuItemCut.Text = "Cut";
            this.menuItemCut.Click += new System.EventHandler(this.menuItemCut_Click);
            // 
            // menuItemCopy
            // 
            this.menuItemCopy.Index = 6;
            this.menuItemCopy.Text = "Copy";
            this.menuItemCopy.Click += new System.EventHandler(this.menuItemCopy_Click);
            // 
            // menuItemPaste
            // 
            this.menuItemPaste.Index = 7;
            this.menuItemPaste.Text = "Paste";
            this.menuItemPaste.Click += new System.EventHandler(this.menuItemPaste_Click);
            // 
            // menuItemDelete
            // 
            this.menuItemDelete.Index = 8;
            this.menuItemDelete.Text = "Delete";
            this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 9;
            this.menuItem2.Text = "-";
            // 
            // menuItemSubscribe
            // 
            this.menuItemSubscribe.Index = 10;
            this.menuItemSubscribe.Text = "Subscribe";
            this.menuItemSubscribe.Click += new System.EventHandler(this.menuItemSubscribe_Click);
            // 
            // menuItemUnsubscribe
            // 
            this.menuItemUnsubscribe.Index = 11;
            this.menuItemUnsubscribe.Text = "Unsubscribe";
            this.menuItemUnsubscribe.Click += new System.EventHandler(this.menuItemUnsubscribe_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 12;
            this.menuItem3.Text = "-";
            // 
            // menuItemSendToMe
            // 
            this.menuItemSendToMe.Index = 13;
            this.menuItemSendToMe.Text = "Send to Me";
            this.menuItemSendToMe.Click += new System.EventHandler(this.menuItemSendToMe_Click);
            // 
            // menuItemSendAndGoto
            // 
            this.menuItemSendAndGoto.Index = 14;
            this.menuItemSendAndGoto.Text = "Send to Me && Go To";
            this.menuItemSendAndGoto.Click += new System.EventHandler(this.menuItemSendAndGoto_Click);
            // 
            // menuItemGoto
            // 
            this.menuItemGoto.Index = 15;
            this.menuItemGoto.Text = "Go To";
            this.menuItemGoto.Click += new System.EventHandler(this.menuItemGoto_Click);
            // 
            // menuItemMarkRead
            // 
            this.menuItemMarkRead.Index = 16;
            this.menuItemMarkRead.Text = "Mark as Read";
            this.menuItemMarkRead.Click += new System.EventHandler(this.menuItemMarkRead_Click);
            // 
            // menuNavJob
            // 
            this.menuNavJob.Index = 17;
            this.menuNavJob.Text = "Navigate to Job";
            this.menuNavJob.Visible = false;
            // 
            // menuDeleteTaken
            // 
            this.menuDeleteTaken.Index = 18;
            this.menuDeleteTaken.Text = "Delete Task Taken";
            this.menuDeleteTaken.Visible = false;
            this.menuDeleteTaken.Click += new System.EventHandler(this.menuDeleteTaken_Click);
            // 
            // menuArchive
            // 
            this.menuArchive.Index = 19;
            this.menuArchive.Text = "Archive";
            this.menuArchive.Visible = false;
            this.menuArchive.Click += new System.EventHandler(this.menuArchive_Click);
            // 
            // menuUnarchive
            // 
            this.menuUnarchive.Index = 20;
            this.menuUnarchive.Text = "Unarchive";
            this.menuUnarchive.Visible = false;
            this.menuUnarchive.Click += new System.EventHandler(this.menuUnarchive_Click);
            // 
            // menuTask
            // 
            this.menuTask.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemTaskReminder});
            // 
            // menuItemTaskReminder
            // 
            this.menuItemTaskReminder.Index = 0;
            this.menuItemTaskReminder.Text = "Reminder";
            this.menuItemTaskReminder.Click += new System.EventHandler(this.menuItemTaskReminder_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // tasksGrid
            // 
            this.tasksGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tasksGrid.Location = new System.Drawing.Point(217, 48);
            this.tasksGrid.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.tasksGrid.Name = "tasksGrid";
            this.tasksGrid.Size = new System.Drawing.Size(724, 462);
            this.tasksGrid.TabIndex = 9;
            this.tasksGrid.Title = "Tasks";
            this.tasksGrid.TranslationName = "TableTasks";
            this.tasksGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
            this.tasksGrid.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
            this.tasksGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridMain_MouseDown);
            // 
            // menuFilter
            // 
            this.menuFilter.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFilterDefault,
            this.menuItemFilterNone,
            this.menuItemFilterClinic,
            this.menuItemFilterRegion});
            // 
            // menuItemFilterDefault
            // 
            this.menuItemFilterDefault.Index = 0;
            this.menuItemFilterDefault.Text = "Default";
            // 
            // menuItemFilterNone
            // 
            this.menuItemFilterNone.Index = 1;
            this.menuItemFilterNone.Text = "None";
            // 
            // menuItemFilterClinic
            // 
            this.menuItemFilterClinic.Index = 2;
            this.menuItemFilterClinic.Text = "Clinic";
            // 
            // menuItemFilterRegion
            // 
            this.menuItemFilterRegion.Index = 3;
            this.menuItemFilterRegion.Text = "Region";
            // 
            // mainToolBar
            // 
            this.mainToolBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainToolBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.mainToolBar.ImageList = this.imageListMain;
            this.mainToolBar.Location = new System.Drawing.Point(0, 0);
            this.mainToolBar.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.mainToolBar.Name = "mainToolBar";
            this.mainToolBar.Size = new System.Drawing.Size(941, 25);
            this.mainToolBar.TabIndex = 2;
            this.mainToolBar.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
            // 
            // showArchivedCheckBox
            // 
            this.showArchivedCheckBox.AutoSize = true;
            this.showArchivedCheckBox.Location = new System.Drawing.Point(400, 28);
            this.showArchivedCheckBox.Name = "showArchivedCheckBox";
            this.showArchivedCheckBox.Size = new System.Drawing.Size(125, 17);
            this.showArchivedCheckBox.TabIndex = 10;
            this.showArchivedCheckBox.Text = "Show archived tasks";
            this.showArchivedCheckBox.UseVisualStyleBackColor = true;
            // 
            // showMineCheckBox
            // 
            this.showMineCheckBox.AutoSize = true;
            this.showMineCheckBox.Checked = true;
            this.showMineCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showMineCheckBox.Location = new System.Drawing.Point(217, 28);
            this.showMineCheckBox.Name = "showMineCheckBox";
            this.showMineCheckBox.Size = new System.Drawing.Size(177, 17);
            this.showMineCheckBox.TabIndex = 11;
            this.showMineCheckBox.Text = "Only show tasks assigned to me";
            this.showMineCheckBox.UseVisualStyleBackColor = true;
            // 
            // statusComboBox
            // 
            this.statusComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.statusComboBox.FormattingEnabled = true;
            this.statusComboBox.Location = new System.Drawing.Point(791, 26);
            this.statusComboBox.Margin = new System.Windows.Forms.Padding(3, 1, 0, 1);
            this.statusComboBox.Name = "statusComboBox";
            this.statusComboBox.Size = new System.Drawing.Size(150, 21);
            this.statusComboBox.TabIndex = 12;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(748, 29);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(37, 13);
            this.statusLabel.TabIndex = 13;
            this.statusLabel.Text = "Status";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(548, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(20, 0, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "-- TODO: Date Range Filter --";
            // 
            // UserControlTasks
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.statusComboBox);
            this.Controls.Add(this.showMineCheckBox);
            this.Controls.Add(this.showArchivedCheckBox);
            this.Controls.Add(this.tasksGrid);
            this.Controls.Add(this.taskListsTree);
            this.Controls.Add(this.mainToolBar);
            this.Name = "UserControlTasks";
            this.Size = new System.Drawing.Size(941, 510);
            this.Load += new System.EventHandler(this.UserControlTasks_Load);
            this.Resize += new System.EventHandler(this.UserControlTasks_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.ODToolBar mainToolBar;
		private System.Windows.Forms.TreeView taskListsTree;
		private System.Windows.Forms.ImageList imageListMain;
		private System.Windows.Forms.ContextMenu menuEdit;
		private System.Windows.Forms.MenuItem menuItemEdit;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItemCut;
		private System.Windows.Forms.MenuItem menuItemCopy;
		private System.Windows.Forms.MenuItem menuItemPaste;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItemGoto;
		private System.Windows.Forms.ImageList imageListTree;
		private System.Windows.Forms.MenuItem menuItemSubscribe;
		private System.Windows.Forms.MenuItem menuItemUnsubscribe;
		private System.Windows.Forms.MenuItem menuItem3;
		private OpenDental.UI.ODGrid tasksGrid;
		private System.Windows.Forms.MenuItem menuItemDone;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItemSendToMe;
		private System.Windows.Forms.ContextMenu menuTask;
		private System.Windows.Forms.MenuItem menuItemTaskReminder;
		private System.Windows.Forms.MenuItem menuItemSendAndGoto;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.MenuItem menuNavJob;
		private System.Windows.Forms.MenuItem menuItemPriority;
		private System.Windows.Forms.MenuItem menuDeleteTaken;
		private System.Windows.Forms.MenuItem menuItemMarkRead;
		private System.Windows.Forms.ContextMenu menuFilter;
		private System.Windows.Forms.MenuItem menuItemFilterNone;
		private System.Windows.Forms.MenuItem menuItemFilterClinic;
		private System.Windows.Forms.MenuItem menuItemFilterRegion;
		private System.Windows.Forms.MenuItem menuItemFilterDefault;
		private System.Windows.Forms.MenuItem menuArchive;
		private System.Windows.Forms.MenuItem menuUnarchive;
        private System.Windows.Forms.CheckBox showArchivedCheckBox;
        private System.Windows.Forms.CheckBox showMineCheckBox;
        private System.Windows.Forms.ComboBox statusComboBox;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label label1;
    }
}
