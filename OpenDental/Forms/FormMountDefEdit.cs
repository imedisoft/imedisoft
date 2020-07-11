using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class FormMountDefEdit : ODForm{
		#region Fields - Public
		public MountDef MountDefCur;
		#endregion Fields - Public

		#region Fields - Private
		private bool _isMouseDown;
		private bool _isWider;
		private List<MountItemDef> _listMountItemDefs;
		/// <summary>The original point where the mouse was down.</summary>
		private Point _pointMouseDownOrig;
		/// <summary>If we are dragging, this is the original location of the item.</summary>
		private Point _pointItemOrig;
		/// <summary>To shrink or enlarge the mount to make it fit in the space available.</summary>
		private float _ratio;
		/// <summary>This is the entire area available for drawing.</summary>
		private Rectangle _rectangleBack;
		/// <summary>This is the outline of the mount.</summary>
		private Rectangle _rectangleMount;
		private int _selectedIndex=-1;
		#endregion Fields - Private

		#region Designer
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butDelete;
		private TextBox textDescription;
		private Label label1;
		private Label label3;
		private ValidNum textWidth;
		private ValidNum textHeight;
		private Label label4;
		private UI.Button butGenerate;
		private Panel panelSplitter;
		private Label label2;
		private UI.Button butDown;
		private UI.Button butUp;
		private GroupBox groupBox1;
		private Label labelWarning;
		private Label label5;
		private Label label7;
		private Label label6;
		private Button butColor;
		private Label labelColor;
		private UI.Button butAdd;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMountDefEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textWidth = new OpenDental.ValidNum();
			this.textHeight = new OpenDental.ValidNum();
			this.label4 = new System.Windows.Forms.Label();
			this.butGenerate = new OpenDental.UI.Button();
			this.panelSplitter = new System.Windows.Forms.Panel();
			this.butColor = new System.Windows.Forms.Button();
			this.labelColor = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.labelWarning = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.panelSplitter.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(909, 634);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 9;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(909, 602);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 8;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(822, 634);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 24);
			this.butDelete.TabIndex = 4;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textDescription
			// 
			this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescription.Location = new System.Drawing.Point(825, 27);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(155, 20);
			this.textDescription.TabIndex = 10;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(822, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 17);
			this.label1.TabIndex = 11;
			this.label1.Text = "Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(848, 50);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(52, 17);
			this.label3.TabIndex = 14;
			this.label3.Text = "Width";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textWidth
			// 
			this.textWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textWidth.Location = new System.Drawing.Point(903, 50);
			this.textWidth.MaxVal = 20000;
			this.textWidth.MinVal = 1;
			this.textWidth.Name = "textWidth";
			this.textWidth.Size = new System.Drawing.Size(48, 20);
			this.textWidth.TabIndex = 27;
			this.textWidth.TextChanged += new System.EventHandler(this.textWidth_TextChanged);
			// 
			// textHeight
			// 
			this.textHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textHeight.Location = new System.Drawing.Point(903, 73);
			this.textHeight.MaxVal = 10000;
			this.textHeight.MinVal = 1;
			this.textHeight.Name = "textHeight";
			this.textHeight.Size = new System.Drawing.Size(48, 20);
			this.textHeight.TabIndex = 29;
			this.textHeight.TextChanged += new System.EventHandler(this.textHeight_TextChanged);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(848, 73);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(52, 17);
			this.label4.TabIndex = 28;
			this.label4.Text = "Height";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// butGenerate
			// 
			this.butGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butGenerate.Location = new System.Drawing.Point(825, 129);
			this.butGenerate.Name = "butGenerate";
			this.butGenerate.Size = new System.Drawing.Size(65, 24);
			this.butGenerate.TabIndex = 30;
			this.butGenerate.Text = "Generate";
			this.butGenerate.Click += new System.EventHandler(this.butGenerate_Click);
			// 
			// panelSplitter
			// 
			this.panelSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelSplitter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelSplitter.Controls.Add(this.butColor);
			this.panelSplitter.Controls.Add(this.labelColor);
			this.panelSplitter.Controls.Add(this.label5);
			this.panelSplitter.Controls.Add(this.labelWarning);
			this.panelSplitter.Location = new System.Drawing.Point(816, 0);
			this.panelSplitter.Name = "panelSplitter";
			this.panelSplitter.Size = new System.Drawing.Size(172, 672);
			this.panelSplitter.TabIndex = 31;
			// 
			// butColor
			// 
			this.butColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColor.Location = new System.Drawing.Point(86, 95);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(30, 20);
			this.butColor.TabIndex = 44;
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			// 
			// labelColor
			// 
			this.labelColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelColor.Location = new System.Drawing.Point(14, 97);
			this.labelColor.Name = "labelColor";
			this.labelColor.Size = new System.Drawing.Size(69, 16);
			this.labelColor.TabIndex = 45;
			this.labelColor.Text = "Back Color";
			this.labelColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(4, 556);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(138, 42);
			this.label5.TabIndex = 43;
			this.label5.Text = "Items get saved as they are added, not when clicking OK here";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelWarning
			// 
			this.labelWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelWarning.ForeColor = System.Drawing.Color.Firebrick;
			this.labelWarning.Location = new System.Drawing.Point(1, 295);
			this.labelWarning.Name = "labelWarning";
			this.labelWarning.Size = new System.Drawing.Size(167, 69);
			this.labelWarning.TabIndex = 43;
			this.labelWarning.Text = "Warning!  At least one item is not showing because it\'s outside the bounds of the" +
    " Mount.  Consider enlarging your mount to find it.";
			this.labelWarning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelWarning.Visible = false;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(893, 123);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(90, 34);
			this.label2.TabIndex = 32;
			this.label2.Text = "(start an entirely new layout)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butDown
			// 
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(9, 49);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(65, 24);
			this.butDown.TabIndex = 40;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(9, 19);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(65, 24);
			this.butUp.TabIndex = 41;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butAdd
			// 
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(825, 168);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(81, 24);
			this.butAdd.TabIndex = 39;
			this.butAdd.Text = "Add Item";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.butUp);
			this.groupBox1.Controls.Add(this.butDown);
			this.groupBox1.Location = new System.Drawing.Point(822, 204);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(142, 80);
			this.groupBox1.TabIndex = 42;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Item Order";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(76, 52);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(58, 18);
			this.label7.TabIndex = 44;
			this.label7.Text = "(higher #)";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(76, 20);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(58, 18);
			this.label6.TabIndex = 43;
			this.label6.Text = "(lower #)";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormMountDefEdit
			// 
			this.ClientSize = new System.Drawing.Size(988, 672);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butGenerate);
			this.Controls.Add(this.textHeight);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textWidth);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.panelSplitter);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = System.Windows.Forms.ImeMode.On;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormMountDefEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Mount Def";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMountDefEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormMountDefEdit_Load);
			this.SizeChanged += new System.EventHandler(this.FormMountDefEdit_SizeChanged);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMountDefEdit_Paint);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMountDefEdit_KeyDown);
			this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FormMountDefEdit_MouseDoubleClick);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMountDefEdit_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormMountDefEdit_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormMountDefEdit_MouseUp);
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.FormMountDefEdit_PreviewKeyDown);
			this.panelSplitter.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer

		#region Constructor
		///<summary></summary>
		public FormMountDefEdit()	{
			//Required for Windows Form Designer support
			InitializeComponent();
			Lan.F(this);
		}
		#endregion Constructor

		#region Methods - EventHandlers - Form
		private void FormMountDefEdit_FormClosing(object sender, FormClosingEventArgs e){
			if(DialogResult==DialogResult.OK){
				return;
			}
			if(MountDefCur.IsNew){
				if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Delete mount?")){
					e.Cancel=true;
					return;
				}
				MountItemDefs.DeleteForMount(MountDefCur.MountDefNum);
				MountDefs.Delete(MountDefCur.MountDefNum);
			}
		}

		private void FormMountDefEdit_KeyDown(object sender, KeyEventArgs e){
			//todo: this is not very reliable.
			if(_selectedIndex==-1){
				return;
			}
			//Each keypress hits the database, but it's a rare setup window and a light hit.
			if(e.KeyCode==Keys.Left){
				_listMountItemDefs[_selectedIndex].Xpos--;
				MountItemDefs.Update(_listMountItemDefs[_selectedIndex]);
				FillItems();
			}
			if(e.KeyCode==Keys.Right){
				_listMountItemDefs[_selectedIndex].Xpos++;
				MountItemDefs.Update(_listMountItemDefs[_selectedIndex]);
				FillItems();
			}
			if(e.KeyCode==Keys.Up){
				_listMountItemDefs[_selectedIndex].Ypos--;
				MountItemDefs.Update(_listMountItemDefs[_selectedIndex]);
				FillItems();
			}
			if(e.KeyCode==Keys.Down){
				_listMountItemDefs[_selectedIndex].Ypos++;
				MountItemDefs.Update(_listMountItemDefs[_selectedIndex]);
				FillItems();
			}
		}

		private void FormMountDefEdit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e){
			
		}

		private void FormMountDefEdit_Load(object sender, System.EventArgs e) {
			CalcRatio();
			textDescription.Text=MountDefCur.Description;
			textWidth.Text=MountDefCur.Width.ToString();
			textHeight.Text=MountDefCur.Height.ToString();
			butColor.BackColor=MountDefCur.ColorBack;
			FillItems();
			ShowWarning();
			if(!MountDefCur.IsNew){
				return;
			}
			FormMountDefGenerate formMountDefGenerate=new FormMountDefGenerate();
			formMountDefGenerate.MountDefCur=MountDefCur;
			formMountDefGenerate.ShowDialog();
			if(formMountDefGenerate.DialogResult!=DialogResult.OK){
				MountDefs.Delete(MountDefCur.MountDefNum);
				DialogResult=DialogResult.OK;//to avoid triggering the msgbox in FormClosing
				return;
			}
			CalcRatio();
			textDescription.Text=MountDefCur.Description;
			textWidth.Text=MountDefCur.Width.ToString();
			textHeight.Text=MountDefCur.Height.ToString();
			butColor.BackColor=MountDefCur.ColorBack;
			FillItems();
			ShowWarning();
		}

		private void FormMountDefEdit_SizeChanged(object sender, EventArgs e){
			//Invalidate();//it already does this
			CalcRatio();
		}
		#endregion Methods - EventHandlers - Form

		#region Methods - EventHandlers - Controls
		private void butAdd_Click(object sender, EventArgs e){
			MountItemDef mountItemDef=new MountItemDef();
			mountItemDef.IsNew=true;
			mountItemDef.MountDefNum=MountDefCur.MountDefNum;
			mountItemDef.Width=MountDefCur.Width/4;
			mountItemDef.Height=MountDefCur.Height/4;
			mountItemDef.ItemOrder=1;
			if(_listMountItemDefs.Count>0){
				mountItemDef.Width=_listMountItemDefs[0].Width;
				mountItemDef.Height=_listMountItemDefs[0].Height;
				mountItemDef.ItemOrder=_listMountItemDefs.Count+1;
			}
			MountItemDefs.Insert(mountItemDef);//don't bother showing the edit window?
			FillItems();
		}

		private void butColor_Click(object sender, System.EventArgs e) {
			ColorDialog colorDialog=new ColorDialog();
			colorDialog.FullOpen=true;
			colorDialog.Color=butColor.BackColor;
			colorDialog.ShowDialog();
			butColor.BackColor=colorDialog.Color;
		}

		private void butGenerate_Click(object sender, EventArgs e){
			FormMountDefGenerate formMountDefGenerate=new FormMountDefGenerate();
			formMountDefGenerate.MountDefCur=MountDefCur;
			formMountDefGenerate.ShowDialog();
			if(formMountDefGenerate.DialogResult!=DialogResult.OK){
				return;
			}
			CalcRatio();
			textWidth.Text=MountDefCur.Width.ToString();
			textHeight.Text=MountDefCur.Height.ToString();
			butColor.BackColor=MountDefCur.ColorBack;
			FillItems();
			ShowWarning();
		}

		private void butDown_Click(object sender, EventArgs e){
			int selectedIdx=_selectedIndex;
			if(selectedIdx==-1) {
				MessageBox.Show("Please select an item first.");
				return;
			}
			if(selectedIdx==_listMountItemDefs.Count-1) {//at bottom
				return;
			}
			MountItemDef mountItemDef=_listMountItemDefs[selectedIdx];
			mountItemDef.ItemOrder++;
			MountItemDefs.Update(mountItemDef);
			MountItemDef mountItemDefBelow=_listMountItemDefs[selectedIdx+1];
			mountItemDefBelow.ItemOrder--;
			MountItemDefs.Update(mountItemDefBelow);
			FillItems();
			_selectedIndex=selectedIdx+1;//visually, this is just the same item as before without moving
		}

		private void butUp_Click(object sender, EventArgs e){
			int selectedIdx=_selectedIndex;
			if(selectedIdx==-1) {
				MessageBox.Show("Please select an item first.");
				return;
			}
			if(selectedIdx==0) {//at top
				return;
			}
			MountItemDef mountItemDef=_listMountItemDefs[selectedIdx];
			mountItemDef.ItemOrder--;
			MountItemDefs.Update(mountItemDef);
			MountItemDef mountItemDefAbove=_listMountItemDefs[selectedIdx-1];
			mountItemDefAbove.ItemOrder++;
			MountItemDefs.Update(mountItemDefAbove);
			FillItems();
			_selectedIndex=selectedIdx-1;//visually, this is just the same item as before without moving
		}

		private void textHeight_TextChanged(object sender, EventArgs e){
			if(textHeight.Text==MountDefCur.Height.ToString()){
				//probably just set at load
				return;
			}
			int height=0;
			try{
				height=PIn.Int(textHeight.Text);
			}
			catch{
				return;
			}
			if(height<1){
				return;
			}
			MountDefCur.Height=height;
			//but don't save to db
			CalcRatio();
			ShowWarning();
			Invalidate();
		}

		private void textWidth_TextChanged(object sender, EventArgs e){
			if(textWidth.Text==MountDefCur.Width.ToString()){
				//probably just set at load
				return;
			}
			int width=0;
			try{
				width=PIn.Int(textWidth.Text);
			}
			catch{
				return;
			}
			if(width<1){
				return;
			}
			MountDefCur.Width=width;
			//but don't save to db
			CalcRatio();
			ShowWarning();
			Invalidate();
		}
		#endregion Methods - EventHandlers - Controls

		#region Methods - EventHandlers - Paint
		private void FormMountDefEdit_Paint(object sender, PaintEventArgs e){
			Graphics g=e.Graphics;//alias
			g.FillRectangle(SystemBrushes.Control,_rectangleBack);
			g.DrawRectangle(Pens.Blue,_rectangleMount);
			RectangleF rectItem;
			Point point;
			string s;
			//first, draw the non-selected items
			for(int i=0;i<_listMountItemDefs.Count;i++){
				if(_selectedIndex==i){
					continue;
				}
				rectItem=new RectangleF(
					_rectangleMount.X+_listMountItemDefs[i].Xpos*_ratio,
					_rectangleMount.Y+_listMountItemDefs[i].Ypos*_ratio,
					_listMountItemDefs[i].Width*_ratio,
					_listMountItemDefs[i].Height*_ratio);
				g.DrawRectangle(Pens.Blue,Rectangle.Round(rectItem));
				s="#"+_listMountItemDefs[i].ItemOrder.ToString()+"\r\nX:"+_listMountItemDefs[i].Xpos.ToString()+"\r\nY:"+_listMountItemDefs[i].Ypos.ToString()
					+"\r\nW:"+_listMountItemDefs[i].Width.ToString()+"\r\nH:"+_listMountItemDefs[i].Height.ToString();
				point=new Point(
					(int)(_rectangleMount.X+_listMountItemDefs[i].Xpos*_ratio+_listMountItemDefs[i].Width*_ratio/2-g.MeasureString(s,Font).Width/2),
					(int)(_rectangleMount.Y+_listMountItemDefs[i].Ypos*_ratio+_listMountItemDefs[i].Height*_ratio/2-g.MeasureString(s,Font).Height/2));
				g.DrawString(s,Font,Brushes.DarkBlue,point);
			}
			//then, draw any selected item so it draws on top
			if(_selectedIndex==-1){
				return;
			}
			rectItem=new RectangleF(
				_rectangleMount.X+_listMountItemDefs[_selectedIndex].Xpos*_ratio,
				_rectangleMount.Y+_listMountItemDefs[_selectedIndex].Ypos*_ratio,
				_listMountItemDefs[_selectedIndex].Width*_ratio,
				_listMountItemDefs[_selectedIndex].Height*_ratio);
			g.DrawRectangle(Pens.Red,Rectangle.Round(rectItem));
			s="#"+_listMountItemDefs[_selectedIndex].ItemOrder.ToString()+"\r\nX:"
				+_listMountItemDefs[_selectedIndex].Xpos.ToString()+"\r\nY:"+_listMountItemDefs[_selectedIndex].Ypos.ToString()
				+"\r\nW:"+_listMountItemDefs[_selectedIndex].Width.ToString()+"\r\nH:"+_listMountItemDefs[_selectedIndex].Height.ToString();
			point=new Point(
				(int)(_rectangleMount.X+_listMountItemDefs[_selectedIndex].Xpos*_ratio+_listMountItemDefs[_selectedIndex].Width*_ratio/2-g.MeasureString(s,Font).Width/2),
				(int)(_rectangleMount.Y+_listMountItemDefs[_selectedIndex].Ypos*_ratio+_listMountItemDefs[_selectedIndex].Height*_ratio/2-g.MeasureString(s,Font).Height/2));
			g.DrawString(s,Font,Brushes.DarkRed,point);
		}
		#endregion Methods - EventHandlers - Paint

		#region Methods - EventHandlers - Mouse
		private void FormMountDefEdit_MouseDoubleClick(object sender, MouseEventArgs e){
			//mouse down will have set selected index
			if(_selectedIndex==-1){
				return;
			}
			FormMountItemDefEdit formMountItemDefEdit=new FormMountItemDefEdit();
			formMountItemDefEdit.MountItemDefCur=_listMountItemDefs[_selectedIndex];
			formMountItemDefEdit.ShowDialog();
			FillItems();
			//If the last item in the array was deleted, we need to update the selectedIndex variable
			if(formMountItemDefEdit.DialogResult==DialogResult.OK && _selectedIndex>=_listMountItemDefs.Count) {
				_selectedIndex=_listMountItemDefs.Count-1;
			}
		}

		private void FormMountDefEdit_MouseDown(object sender, MouseEventArgs e){
			_isMouseDown=true;
			//todo: Still haven't found a way to take focus off textboxes.
			//butOK.Select();//.Focus();//neither of these worked
			//this.Focus();.Select();.ActiveControl=null//didn't work
			_pointMouseDownOrig=e.Location;
			_selectedIndex=-1;
			for(int i=0;i<_listMountItemDefs.Count;i++){
				if(e.X<_rectangleMount.X+_listMountItemDefs[i].Xpos*_ratio){
					continue;
				}
				if(e.Y<_rectangleMount.Y+_listMountItemDefs[i].Ypos*_ratio){
					continue;
				}
				if(e.X>_rectangleMount.X+_listMountItemDefs[i].Xpos*_ratio+_listMountItemDefs[i].Width*_ratio){
					continue;
				}
				if(e.Y>_rectangleMount.Y+_listMountItemDefs[i].Ypos*_ratio+_listMountItemDefs[i].Height*_ratio){
					continue;
				}
				_selectedIndex=i;
			}
			if(_selectedIndex!=-1){
				_pointItemOrig=new Point(
					(int)(_rectangleMount.X+_listMountItemDefs[_selectedIndex].Xpos*_ratio),
					(int)(_rectangleMount.Y+_listMountItemDefs[_selectedIndex].Ypos*_ratio));					
			}
			Invalidate();
		}

		private void FormMountDefEdit_MouseMove(object sender, MouseEventArgs e){
			if(!_isMouseDown){
				return;
			}
			if(_selectedIndex==-1){
				return;
			}
			//we are dragging
			int x=(int)((e.X-_pointMouseDownOrig.X+_pointItemOrig.X-_rectangleMount.X)/_ratio);
			if(x<0){
				x=0;
			}
			if(x > MountDefCur.Width - _listMountItemDefs[_selectedIndex].Width){
				x=MountDefCur.Width - _listMountItemDefs[_selectedIndex].Width;
			}
			_listMountItemDefs[_selectedIndex].Xpos=x;
			int y=(int)((e.Y-_pointMouseDownOrig.Y+_pointItemOrig.Y-_rectangleMount.Y)/_ratio);
			if(y<0){
				y=0;
			}
			if(y > MountDefCur.Height - _listMountItemDefs[_selectedIndex].Height){
				y=MountDefCur.Height - _listMountItemDefs[_selectedIndex].Height;
			}
			_listMountItemDefs[_selectedIndex].Ypos=y;
			//but don't save to db yet.
			Invalidate();
		}

		private void FormMountDefEdit_MouseUp(object sender, MouseEventArgs e){
			if(_isMouseDown && _selectedIndex!=-1){
				//save any movement
				MountItemDefs.Update(_listMountItemDefs[_selectedIndex]);
				FillItems();
				//_selectedIndex remains same
			}
			_isMouseDown=false;

		}
		#endregion Methods - EventHandlers - Mouse	

		#region Methods - EventHandlers - DeleteOkCancel
		private void butDelete_Click(object sender, System.EventArgs e) {
			if(!MountDefCur.IsNew){
				if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Delete mount?")){
					return;
				}
			}
			MountItemDefs.DeleteForMount(MountDefCur.MountDefNum);
			MountDefs.Delete(MountDefCur.MountDefNum);
			if(MountDefCur.IsNew){
				DialogResult=DialogResult.Cancel;
			}
			else{
				DialogResult=DialogResult.OK;
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textDescription.Text==""){
				MessageBox.Show(Lan.g(this,"Description cannot be blank."));
				return;
			}
			if(textWidth.errorProvider1.GetError(textWidth)!=""
				|| textHeight.errorProvider1.GetError(textHeight)!="")
			{
				MessageBox.Show("Please fix data entry errors first.");
				return;
			}
			//Memory issues are the basis of the mount size limit:
			//Limit size of mount to 20k x 10k pixels. =600 MB color
			//For comparison, 4K is 4000 x 2000
			//Good sensor is 1700x1300. FMX mount would then be 10,700 x 3,900 = 125 MB
			MountDefCur.Description=textDescription.Text;
			MountDefCur.Width=PIn.Int(textWidth.Text);
			MountDefCur.Height=PIn.Int(textHeight.Text);
			MountDefCur.ColorBack=butColor.BackColor;
			int intBlack=System.Drawing.Color.Black.ToArgb();
			try{
				MountDefs.Update(MountDefCur);//whether new or not
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


		#endregion Methods - EventHandlers - DeleteOkCancel

		#region Methods
		private void CalcRatio(){
			Rectangle _rectangleBack=new Rectangle(0,0,panelSplitter.Left-1,ClientRectangle.Height-1);
			float ratioWidth=(float)_rectangleBack.Width/MountDefCur.Width;
			float ratioHeight=(float)_rectangleBack.Height/MountDefCur.Height;
			_ratio=ratioWidth;
			_isWider=false;
			if(ratioHeight<ratioWidth){
				_isWider=true;
				_ratio=ratioHeight;
			}
			float xMain=0;
			if(_isWider){
				xMain=(_rectangleBack.Width-MountDefCur.Width*_ratio)/2;
			}
			float yMain=0;
			if(!_isWider){
				yMain=(_rectangleBack.Height-MountDefCur.Height*_ratio)/2;
			}
			_rectangleMount=new Rectangle((int)xMain,(int)yMain,(int)(MountDefCur.Width*_ratio),(int)(MountDefCur.Height*_ratio));
		}

		private void FillItems(){
			_listMountItemDefs=MountItemDefs.GetForMountDef(MountDefCur.MountDefNum);
			for(int i=0;i<_listMountItemDefs.Count;i++){
				if(_listMountItemDefs[i].ItemOrder!=i+1){//happens quite a bit, like when user deletes an item
					_listMountItemDefs[i].ItemOrder=i+1;
					MountItemDefs.Update(_listMountItemDefs[i]);
				}
				//string s="#"+_listMountItemDefs[i].ItemOrder.ToString()+": X:"+_listMountItemDefs[i].Xpos.ToString()+", Y:"+_listMountItemDefs[i].Ypos.ToString()
				//	+": W:"+_listMountItemDefs[i].Width.ToString()+", H:"+_listMountItemDefs[i].Height.ToString();
				//listBoxItems.Items.Add(s);
			}
			Invalidate();
		}

		private void ShowWarning(){
			labelWarning.Visible=false;
			if(_listMountItemDefs==null){
				return;
			}
			for(int i=0;i<_listMountItemDefs.Count;i++){
				if(_listMountItemDefs[i].Xpos>MountDefCur.Width-2
					|| _listMountItemDefs[i].Ypos>MountDefCur.Height-2)
				{
					labelWarning.Visible=true;
				}
			}
		}

		/*
		///<summary>Returns null if no item.  Pass in raw point as obtained from mouse.</summary>
		private MountItemDef HitTest(Point point){

			return null;
		}*/
		#endregion Methods

		

		
	}
}





















