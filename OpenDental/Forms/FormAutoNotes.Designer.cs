namespace Imedisoft.Forms
{
    partial class FormAutoNotes
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
            this.components = new System.ComponentModel.Container();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.autoNotesTreeView = new System.Windows.Forms.TreeView();
            this.cancelButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.collapseCheckBox = new System.Windows.Forms.CheckBox();
            this.exportButton = new OpenDental.UI.Button();
            this.importButton = new OpenDental.UI.Button();
            this.editButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // imageListTree
            // 
            this.imageListTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListTree.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // autoNotesTreeView
            // 
            this.autoNotesTreeView.AllowDrop = true;
            this.autoNotesTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoNotesTreeView.HideSelection = false;
            this.autoNotesTreeView.ImageIndex = 0;
            this.autoNotesTreeView.ImageList = this.imageListTree;
            this.autoNotesTreeView.Indent = 12;
            this.autoNotesTreeView.Location = new System.Drawing.Point(12, 43);
            this.autoNotesTreeView.Name = "autoNotesTreeView";
            this.autoNotesTreeView.SelectedImageIndex = 0;
            this.autoNotesTreeView.Size = new System.Drawing.Size(360, 475);
            this.autoNotesTreeView.TabIndex = 0;
            this.autoNotesTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.AutoNotesTreeView_ItemDrag);
            this.autoNotesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AutoNotesTreeView_AfterSelect);
            this.autoNotesTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.AutoNotesTreeView_MouseDoubleClick);
            this.autoNotesTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.AutoNotesTreeView_DragDrop);
            this.autoNotesTreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.AutoNotesTreeView_DragEnter);
            this.autoNotesTreeView.DragOver += new System.Windows.Forms.DragEventHandler(this.AutoNotesTreeView_DragOver);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(292, 524);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Close";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(12, 524);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 1;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // collapseCheckBox
            // 
            this.collapseCheckBox.AutoSize = true;
            this.collapseCheckBox.Location = new System.Drawing.Point(12, 17);
            this.collapseCheckBox.Name = "collapseCheckBox";
            this.collapseCheckBox.Size = new System.Drawing.Size(80, 17);
            this.collapseCheckBox.TabIndex = 3;
            this.collapseCheckBox.Text = "Collapse All";
            this.collapseCheckBox.UseVisualStyleBackColor = true;
            this.collapseCheckBox.CheckedChanged += new System.EventHandler(this.CollapseCheckBox_CheckedChanged);
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportButton.Image = global::Imedisoft.Properties.Resources.IconUpload;
            this.exportButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.exportButton.Location = new System.Drawing.Point(292, 12);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(80, 25);
            this.exportButton.TabIndex = 5;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // importButton
            // 
            this.importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importButton.Image = global::Imedisoft.Properties.Resources.IconDownload;
            this.importButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.importButton.Location = new System.Drawing.Point(206, 12);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(80, 25);
            this.importButton.TabIndex = 4;
            this.importButton.Text = "Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPenBlack;
            this.editButton.Location = new System.Drawing.Point(48, 524);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(30, 25);
            this.editButton.TabIndex = 1;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(84, 524);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 1;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormAutoNotes
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(384, 561);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.collapseCheckBox);
            this.Controls.Add(this.autoNotesTreeView);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.addButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAutoNotes";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Notes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAutoNotes_FormClosing);
            this.Load += new System.EventHandler(this.FormAutoNotes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.TreeView autoNotesTreeView;
		private OpenDental.UI.Button addButton;
		private System.Windows.Forms.ImageList imageListTree;
		private System.Windows.Forms.CheckBox collapseCheckBox;
		private OpenDental.UI.Button exportButton;
		private OpenDental.UI.Button importButton;
        private OpenDental.UI.Button editButton;
        private OpenDental.UI.Button deleteButton;
    }
}
