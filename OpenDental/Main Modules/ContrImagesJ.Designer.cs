namespace OpenDental
{
	partial class ContrImagesJ
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContrImagesJ));
			this.imageListTools2 = new System.Windows.Forms.ImageList(this.components);
			this.menuForms = new System.Windows.Forms.ContextMenu();
			this.menuMounts = new System.Windows.Forms.ContextMenu();
			this.treeMain = new System.Windows.Forms.TreeView();
			this.imageListTree = new System.Windows.Forms.ImageList(this.components);
			this.menuTree = new System.Windows.Forms.ContextMenu();
			this.menuItemTreePrint = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.panelNote = new System.Windows.Forms.Panel();
			this.labelInvalidSig = new System.Windows.Forms.Label();
			this.sigBox = new OpenDental.UI.SignatureBox();
			this.label15 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textNote = new System.Windows.Forms.TextBox();
			this.toolBarPaint = new OpenDental.UI.ODToolBar();
			this.zoomSlider = new OpenDental.UI.ZoomSlider();
			this.panelMain = new OpenDental.UI.PanelImagesModule();
			this.windowingSlider = new OpenDental.UI.WindowingSlider();
			this.toolBarMain = new OpenDental.UI.ODToolBar();
			this.timerTreeClick = new System.Windows.Forms.Timer(this.components);
			this.panelNote.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageListTools2
			// 
			this.imageListTools2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTools2.ImageStream")));
			this.imageListTools2.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListTools2.Images.SetKeyName(0, "Pat.gif");
			this.imageListTools2.Images.SetKeyName(1, "print.gif");
			this.imageListTools2.Images.SetKeyName(2, "deleteX.gif");
			this.imageListTools2.Images.SetKeyName(3, "info.gif");
			this.imageListTools2.Images.SetKeyName(4, "scan.gif");
			this.imageListTools2.Images.SetKeyName(5, "import.gif");
			this.imageListTools2.Images.SetKeyName(6, "paste.gif");
			this.imageListTools2.Images.SetKeyName(7, "");
			this.imageListTools2.Images.SetKeyName(8, "ZoomIn.gif");
			this.imageListTools2.Images.SetKeyName(9, "ZoomOut.gif");
			this.imageListTools2.Images.SetKeyName(10, "Hand.gif");
			this.imageListTools2.Images.SetKeyName(11, "flip.gif");
			this.imageListTools2.Images.SetKeyName(12, "rotateL.gif");
			this.imageListTools2.Images.SetKeyName(13, "rotateR.gif");
			this.imageListTools2.Images.SetKeyName(14, "scanDoc.gif");
			this.imageListTools2.Images.SetKeyName(15, "scanPhoto.gif");
			this.imageListTools2.Images.SetKeyName(16, "scanXray.gif");
			this.imageListTools2.Images.SetKeyName(17, "copy.gif");
			this.imageListTools2.Images.SetKeyName(18, "ScanMulti.gif");
			this.imageListTools2.Images.SetKeyName(19, "Export.gif");
			this.imageListTools2.Images.SetKeyName(20, "arrowsAll.png");
			// 
			// treeMain
			// 
			this.treeMain.AllowDrop = true;
			this.treeMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.treeMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.treeMain.ContextMenu = this.menuTree;
			this.treeMain.FullRowSelect = true;
			this.treeMain.HideSelection = false;
			this.treeMain.ImageIndex = 2;
			this.treeMain.ImageList = this.imageListTree;
			this.treeMain.Indent = 5;
			this.treeMain.Location = new System.Drawing.Point(0, 50);
			this.treeMain.Name = "treeMain";
			this.treeMain.SelectedImageIndex = 2;
			this.treeMain.ShowLines = false;
			this.treeMain.Size = new System.Drawing.Size(228, 546);
			this.treeMain.TabIndex = 19;
			this.treeMain.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.TreeMain_AfterCollapse);
			this.treeMain.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.TreeMain_AfterExpand);
			this.treeMain.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeMain_DragDrop);
			this.treeMain.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeMain_DragEnter);
			this.treeMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeMain_MouseClick);
			this.treeMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeMain_MouseDoubleClick);
			this.treeMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeMain_MouseDown);
			this.treeMain.MouseLeave += new System.EventHandler(this.treeMain_MouseLeave);
			this.treeMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeMain_MouseMove);
			this.treeMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeMain_MouseUp);
			// 
			// imageListTree
			// 
			this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
			this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListTree.Images.SetKeyName(0, "");
			this.imageListTree.Images.SetKeyName(1, "");
			this.imageListTree.Images.SetKeyName(2, "");
			this.imageListTree.Images.SetKeyName(3, "");
			this.imageListTree.Images.SetKeyName(4, "");
			this.imageListTree.Images.SetKeyName(5, "");
			this.imageListTree.Images.SetKeyName(6, "");
			this.imageListTree.Images.SetKeyName(7, "");
			// 
			// menuTree
			// 
			this.menuTree.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemTreePrint,
            this.menuItem8,
            this.menuItem9});
			// 
			// menuItemTreePrint
			// 
			this.menuItemTreePrint.Index = 0;
			this.menuItemTreePrint.Text = "Print";
			this.menuItemTreePrint.Click += new System.EventHandler(this.menuTree_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 1;
			this.menuItem8.Text = "Delete";
			this.menuItem8.Click += new System.EventHandler(this.menuTree_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 2;
			this.menuItem9.Text = "Info";
			this.menuItem9.Click += new System.EventHandler(this.menuTree_Click);
			// 
			// panelNote
			// 
			this.panelNote.Controls.Add(this.labelInvalidSig);
			this.panelNote.Controls.Add(this.sigBox);
			this.panelNote.Controls.Add(this.label15);
			this.panelNote.Controls.Add(this.label1);
			this.panelNote.Controls.Add(this.textNote);
			this.panelNote.Location = new System.Drawing.Point(230, 523);
			this.panelNote.Name = "panelNote";
			this.panelNote.Size = new System.Drawing.Size(834, 64);
			this.panelNote.TabIndex = 23;
			this.panelNote.Visible = false;
			this.panelNote.DoubleClick += new System.EventHandler(this.panelNote_DoubleClick);
			// 
			// labelInvalidSig
			// 
			this.labelInvalidSig.BackColor = System.Drawing.SystemColors.Window;
			this.labelInvalidSig.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelInvalidSig.Location = new System.Drawing.Point(398, 31);
			this.labelInvalidSig.Name = "labelInvalidSig";
			this.labelInvalidSig.Size = new System.Drawing.Size(196, 59);
			this.labelInvalidSig.TabIndex = 94;
			this.labelInvalidSig.Text = "Invalid Signature -  Document or note has changed since it was signed.";
			this.labelInvalidSig.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelInvalidSig.DoubleClick += new System.EventHandler(this.labelInvalidSig_DoubleClick);
			// 
			// sigBox
			// 
			this.sigBox.Location = new System.Drawing.Point(308, 20);
			this.sigBox.Name = "sigBox";
			this.sigBox.Size = new System.Drawing.Size(362, 79);
			this.sigBox.TabIndex = 90;
			this.sigBox.DoubleClick += new System.EventHandler(this.sigBox_DoubleClick);
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(305, 0);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(63, 18);
			this.label15.TabIndex = 87;
			this.label15.Text = "Signature";
			this.label15.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.label15.DoubleClick += new System.EventHandler(this.label15_DoubleClick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 18);
			this.label1.TabIndex = 1;
			this.label1.Text = "Note";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.label1.DoubleClick += new System.EventHandler(this.label1_DoubleClick);
			// 
			// textNote
			// 
			this.textNote.BackColor = System.Drawing.SystemColors.Window;
			this.textNote.Location = new System.Drawing.Point(0, 20);
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.ReadOnly = true;
			this.textNote.Size = new System.Drawing.Size(302, 79);
			this.textNote.TabIndex = 0;
			this.textNote.DoubleClick += new System.EventHandler(this.textNote_DoubleClick);
			// 
			// toolBarPaint
			// 
			this.toolBarPaint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.toolBarPaint.ImageList = this.imageListTools2;
			this.toolBarPaint.Location = new System.Drawing.Point(393, 25);
			this.toolBarPaint.Name = "toolBarPaint";
			this.toolBarPaint.Size = new System.Drawing.Size(674, 25);
			this.toolBarPaint.TabIndex = 15;
			this.toolBarPaint.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.toolBarPaint_ButtonClick);
			// 
			// zoomSlider
			// 
			this.zoomSlider.Location = new System.Drawing.Point(162, 25);
			this.zoomSlider.Name = "zoomSlider";
			this.zoomSlider.Size = new System.Drawing.Size(231, 25);
			this.zoomSlider.TabIndex = 21;
			this.zoomSlider.Text = "zoomSlider1";
			this.zoomSlider.FitPressed += new System.EventHandler(this.zoomSlider_FitPressed);
			this.zoomSlider.Zoomed += new System.EventHandler(this.zoomSlider_Zoomed);
			// 
			// panelMain
			// 
			this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelMain.BackColor = System.Drawing.Color.White;
			this.panelMain.Location = new System.Drawing.Point(229, 56);
			this.panelMain.Name = "panelMain";
			this.panelMain.Size = new System.Drawing.Size(836, 425);
			this.panelMain.TabIndex = 20;
			this.panelMain.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMain_Paint);
			this.panelMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelMain_MouseDown);
			this.panelMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelMain_MouseMove);
			this.panelMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelMain_MouseUp);
			// 
			// windowingSlider
			// 
			this.windowingSlider.Enabled = false;
			this.windowingSlider.Location = new System.Drawing.Point(4, 27);
			this.windowingSlider.MaxVal = 255;
			this.windowingSlider.MinVal = 0;
			this.windowingSlider.Name = "windowingSlider";
			this.windowingSlider.Size = new System.Drawing.Size(154, 20);
			this.windowingSlider.TabIndex = 16;
			this.windowingSlider.Text = "contrWindowingSlider1";
			this.windowingSlider.Scroll += new System.EventHandler(this.windowingSlider_Scroll);
			this.windowingSlider.ScrollComplete += new System.EventHandler(this.windowingSlider_ScrollComplete);
			// 
			// toolBarMain
			// 
			this.toolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.toolBarMain.ImageList = this.imageListTools2;
			this.toolBarMain.Location = new System.Drawing.Point(0, 0);
			this.toolBarMain.Name = "toolBarMain";
			this.toolBarMain.Size = new System.Drawing.Size(1067, 25);
			this.toolBarMain.TabIndex = 11;
			this.toolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.toolBarMain_ButtonClick);
			// 
			// timerTreeClick
			// 
			this.timerTreeClick.Interval = 1000;
			this.timerTreeClick.Tick += new System.EventHandler(this.timerTreeClick_Tick);
			// 
			// ContrImagesJ
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panelNote);
			this.Controls.Add(this.toolBarPaint);
			this.Controls.Add(this.zoomSlider);
			this.Controls.Add(this.panelMain);
			this.Controls.Add(this.treeMain);
			this.Controls.Add(this.windowingSlider);
			this.Controls.Add(this.toolBarMain);
			this.DoubleBuffered = true;
			this.Name = "ContrImagesJ";
			this.Size = new System.Drawing.Size(1067, 595);
			this.Resize += new System.EventHandler(this.ContrImages_Resize);
			this.panelNote.ResumeLayout(false);
			this.panelNote.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private UI.ODToolBar toolBarMain;
		private System.Windows.Forms.ContextMenu menuForms;
		private UI.ODToolBar toolBarPaint;
		private System.Windows.Forms.ContextMenu menuMounts;
		private UI.WindowingSlider windowingSlider;
		private System.Windows.Forms.ImageList imageListTools2;
		private System.Windows.Forms.TreeView treeMain;
		private OpenDental.UI.PanelImagesModule panelMain;
		private System.Windows.Forms.ContextMenu menuTree;
		private System.Windows.Forms.MenuItem menuItemTreePrint;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.ImageList imageListTree;
		private UI.ZoomSlider zoomSlider;
		private System.Windows.Forms.Panel panelNote;
		private System.Windows.Forms.Label labelInvalidSig;
		private UI.SignatureBox sigBox;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textNote;
		private System.Windows.Forms.Timer timerTreeClick;
	}
}
