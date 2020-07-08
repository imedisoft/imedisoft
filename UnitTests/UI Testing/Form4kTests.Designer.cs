namespace UnitTests
{
	partial class Form4kTest
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4kTest));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.odGrid1 = new OpenDental.UI.ODGrid();
			this.odGrid2 = new OpenDental.UI.ODGrid();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemSetup = new System.Windows.Forms.MenuItem();
			this.menuItemLock = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemGL = new System.Windows.Forms.MenuItem();
			this.menuItemBalSheet = new System.Windows.Forms.MenuItem();
			this.menuItemProfitLoss = new System.Windows.Forms.MenuItem();
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.button3 = new OpenDental.UI.Button();
			this.button2 = new OpenDental.UI.Button();
			this.button1 = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.checkBox1);
			this.groupBox1.Controls.Add(this.radioButton1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(93, 463);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 100);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "groupBox1";
			// 
			// checkBox1
			// 
			this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBox1.Location = new System.Drawing.Point(23, 68);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(104, 20);
			this.checkBox1.TabIndex = 2;
			this.checkBox1.Text = "checkBox1";
			this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// radioButton1
			// 
			this.radioButton1.Location = new System.Drawing.Point(23, 42);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(104, 20);
			this.radioButton1.TabIndex = 1;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "radioButton1";
			this.radioButton1.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(20, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 18);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			// 
			// listBox1
			// 
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.listBox1.FormattingEnabled = true;
			this.listBox1.IntegralHeight = false;
			this.listBox1.Items.AddRange(new object[] {
            "Item1",
            "Item2",
            "Item3",
            "Item4",
            "Item5",
            "Item6"});
			this.listBox1.Location = new System.Drawing.Point(102, 579);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(120, 82);
			this.listBox1.TabIndex = 4;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(93, 36);
			this.gridMain.Name = "gridMain";
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(865, 332);
			this.gridMain.TabIndex = 0;
			this.gridMain.Title = "Test Grid";
			this.gridMain.TranslationName = "test";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.Location = new System.Drawing.Point(706, 505);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.odGrid1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.odGrid2);
			this.splitContainer1.Size = new System.Drawing.Size(182, 77);
			this.splitContainer1.SplitterDistance = 60;
			this.splitContainer1.TabIndex = 59;
			// 
			// odGrid1
			// 
			this.odGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.odGrid1.Location = new System.Drawing.Point(3, 3);
			this.odGrid1.Name = "odGrid1";
			this.odGrid1.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.odGrid1.Size = new System.Drawing.Size(52, 69);
			this.odGrid1.TabIndex = 60;
			this.odGrid1.Title = "Test Grid";
			this.odGrid1.TranslationName = "test";
			// 
			// odGrid2
			// 
			this.odGrid2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.odGrid2.Location = new System.Drawing.Point(3, 3);
			this.odGrid2.Name = "odGrid2";
			this.odGrid2.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.odGrid2.Size = new System.Drawing.Size(110, 69);
			this.odGrid2.TabIndex = 61;
			this.odGrid2.Title = "Test Grid";
			this.odGrid2.TranslationName = "test";
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSetup,
            this.menuItemLock,
            this.menuItem1});
			// 
			// menuItemSetup
			// 
			this.menuItemSetup.Index = 0;
			this.menuItemSetup.Text = "Setup";
			// 
			// menuItemLock
			// 
			this.menuItemLock.Index = 1;
			this.menuItemLock.Text = "Lock";
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 2;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemGL,
            this.menuItemBalSheet,
            this.menuItemProfitLoss});
			this.menuItem1.Text = "Reports";
			// 
			// menuItemGL
			// 
			this.menuItemGL.Index = 0;
			this.menuItemGL.Text = "General Ledger Detail";
			// 
			// menuItemBalSheet
			// 
			this.menuItemBalSheet.Index = 1;
			this.menuItemBalSheet.Text = "Balance Sheet";
			// 
			// menuItemProfitLoss
			// 
			this.menuItemProfitLoss.Index = 2;
			this.menuItemProfitLoss.Text = "Profit and Loss";
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0, "Add.gif");
			this.imageListMain.Images.SetKeyName(1, "editPencil.gif");
			this.imageListMain.Images.SetKeyName(2, "butExport.gif");
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.ImageList = this.imageListMain;
			this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(974, 25);
			this.ToolBarMain.TabIndex = 61;
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button3.Location = new System.Drawing.Point(887, 639);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 24);
			this.button3.TabIndex = 60;
			this.button3.Text = "button3";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Image = global::UnitTests.Properties.Resources.deleteX;
			this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button2.Location = new System.Drawing.Point(12, 65);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 24);
			this.button2.TabIndex = 2;
			this.button2.Text = "Delete";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 36);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 24);
			this.button1.TabIndex = 1;
			this.button1.Text = "Launch";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click_1);
			// 
			// Form4kTest
			// 
			this.ClientSize = new System.Drawing.Size(974, 675);
			this.Controls.Add(this.ToolBarMain);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.gridMain);
			this.Location = new System.Drawing.Point(100, 100);
			this.Menu = this.mainMenu1;
			this.Name = "Form4kTest";
			this.Text = "4K Tests";
			this.Load += new System.EventHandler(this.FormGridTest_Load);
			this.DpiChanged += new System.Windows.Forms.DpiChangedEventHandler(this.Form4kTest_DpiChanged);
			this.ResizeEnd += new System.EventHandler(this.Form4kTest_ResizeEnd);
			this.SizeChanged += new System.EventHandler(this.Form4kTest_SizeChanged);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.Form4kTest_Layout);
			this.DpiChangedBeforeParent += new System.EventHandler(this.Form4kTest_DpiChangedBeforeParent);
			this.DpiChangedAfterParent += new System.EventHandler(this.Form4kTest_DpiChangedAfterParent);
			this.Resize += new System.EventHandler(this.Form4kTest_Resize);
			this.groupBox1.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.Button button1;
		private OpenDental.UI.Button button2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private OpenDental.UI.ODGrid odGrid1;
		private OpenDental.UI.ODGrid odGrid2;
		private OpenDental.UI.Button button3;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItemSetup;
		private System.Windows.Forms.MenuItem menuItemLock;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItemGL;
		private System.Windows.Forms.MenuItem menuItemBalSheet;
		private System.Windows.Forms.MenuItem menuItemProfitLoss;
		private OpenDental.UI.ODToolBar ToolBarMain;
		private System.Windows.Forms.ImageList imageListMain;
	}
}