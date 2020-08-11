using System;
using System.Drawing;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using Tao.Platform.Windows;
using OpenDental.UI;
using SparksToothChart;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormGraphics : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private CheckBox checkHardwareAccel;
		private CheckBox checkDoubleBuffering;
		private GroupBox group3DToothChart;
		private OpenDental.UI.ODGrid gridFormats;
		private OpenGLWinFormsControl.PixelFormatValue[] formats=new OpenGLWinFormsControl.PixelFormatValue[0];
		private ToothChartDirectX.DirectXDeviceFormat[] xformats=new ToothChartDirectX.DirectXDeviceFormat[0];
		private int selectedFormatNum=0;
		private string selectedDirectXFormat="";
		private CheckBox checkAllFormats;
		//private bool refreshAllowed=false;
		private RadioButton radioSimpleChart;
		private RadioButton radioOpenGLChart;
		private GroupBox groupFilters;
		private Label label3DChart;
		private Label label2;
		private Label label4;
		private TextBox textSelected;
		private Label label3;
		private RadioButton radioDirectXChart;
		public ComputerPref ComputerPrefCur;
		///<summary>True when we are editing another computers ComputerPrefs.</summary>
		private bool _isRemoteEdit;
		private GroupBox groupBox1;
		private GroupBox groupBox2;
		private GroupBox groupBox3;
		private RadioButton radioDirectX11NotFound;
		private RadioButton radioDirectX11Avail;
		private RadioButton radioDirectX11DontUse;
		private RadioButton radioDirectX11Use;
		private Label label1;
		private Label label5;
		private GroupBox groupBox4;
		private RadioButton radioDirectX11ThisCompUseGlobal;
		private RadioButton radioDirectX11ThisCompYes;
		private RadioButton radioDirectX11ThisCompNo;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		///<summary></summary>
		public FormGraphics(){
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			
		}

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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGraphics));
			this.checkHardwareAccel = new System.Windows.Forms.CheckBox();
			this.checkDoubleBuffering = new System.Windows.Forms.CheckBox();
			this.group3DToothChart = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textSelected = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3DChart = new System.Windows.Forms.Label();
			this.groupFilters = new System.Windows.Forms.GroupBox();
			this.checkAllFormats = new System.Windows.Forms.CheckBox();
			this.gridFormats = new OpenDental.UI.ODGrid();
			this.radioSimpleChart = new System.Windows.Forms.RadioButton();
			this.radioOpenGLChart = new System.Windows.Forms.RadioButton();
			this.radioDirectXChart = new System.Windows.Forms.RadioButton();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.radioDirectX11DontUse = new System.Windows.Forms.RadioButton();
			this.radioDirectX11Use = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.radioDirectX11NotFound = new System.Windows.Forms.RadioButton();
			this.radioDirectX11Avail = new System.Windows.Forms.RadioButton();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.radioDirectX11ThisCompUseGlobal = new System.Windows.Forms.RadioButton();
			this.radioDirectX11ThisCompYes = new System.Windows.Forms.RadioButton();
			this.radioDirectX11ThisCompNo = new System.Windows.Forms.RadioButton();
			this.group3DToothChart.SuspendLayout();
			this.groupFilters.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkHardwareAccel
			// 
			this.checkHardwareAccel.Location = new System.Drawing.Point(6, 15);
			this.checkHardwareAccel.Name = "checkHardwareAccel";
			this.checkHardwareAccel.Size = new System.Drawing.Size(282, 18);
			this.checkHardwareAccel.TabIndex = 2;
			this.checkHardwareAccel.Text = "Hardware Acceleration (checked by default)";
			this.checkHardwareAccel.UseVisualStyleBackColor = true;
			this.checkHardwareAccel.Click += new System.EventHandler(this.checkHardwareAccel_Click);
			// 
			// checkDoubleBuffering
			// 
			this.checkDoubleBuffering.Location = new System.Drawing.Point(6, 32);
			this.checkDoubleBuffering.Name = "checkDoubleBuffering";
			this.checkDoubleBuffering.Size = new System.Drawing.Size(282, 17);
			this.checkDoubleBuffering.TabIndex = 4;
			this.checkDoubleBuffering.Text = "Use Double-Buffering";
			this.checkDoubleBuffering.UseVisualStyleBackColor = true;
			this.checkDoubleBuffering.Click += new System.EventHandler(this.checkDoubleBuffering_Click);
			// 
			// group3DToothChart
			// 
			this.group3DToothChart.Controls.Add(this.label4);
			this.group3DToothChart.Controls.Add(this.textSelected);
			this.group3DToothChart.Controls.Add(this.label3);
			this.group3DToothChart.Controls.Add(this.label2);
			this.group3DToothChart.Controls.Add(this.label3DChart);
			this.group3DToothChart.Controls.Add(this.groupFilters);
			this.group3DToothChart.Controls.Add(this.gridFormats);
			this.group3DToothChart.Location = new System.Drawing.Point(28, 208);
			this.group3DToothChart.Name = "group3DToothChart";
			this.group3DToothChart.Size = new System.Drawing.Size(833, 414);
			this.group3DToothChart.TabIndex = 5;
			this.group3DToothChart.TabStop = false;
			this.group3DToothChart.Text = "Options For Older Technology";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(60, 164);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(608, 16);
			this.label4.TabIndex = 15;
			this.label4.Text = " Formats are listed from most recommended on top to least recommended on bottom.";
			// 
			// textSelected
			// 
			this.textSelected.Location = new System.Drawing.Point(6, 161);
			this.textSelected.Name = "textSelected";
			this.textSelected.ReadOnly = true;
			this.textSelected.Size = new System.Drawing.Size(53, 20);
			this.textSelected.TabIndex = 14;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 143);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(159, 16);
			this.label3.TabIndex = 13;
			this.label3.Text = "Currently selected format number";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 111);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(818, 31);
			this.label2.TabIndex = 12;
			this.label2.Text = resources.GetString("label2.Text");
			// 
			// label3DChart
			// 
			this.label3DChart.Location = new System.Drawing.Point(9, 18);
			this.label3DChart.Name = "label3DChart";
			this.label3DChart.Size = new System.Drawing.Size(818, 20);
			this.label3DChart.TabIndex = 11;
			this.label3DChart.Text = "Most users will never need to change any of these options.  These are only used w" +
    "hen the 3D tooth chart is not working properly.";
			// 
			// groupFilters
			// 
			this.groupFilters.Controls.Add(this.checkHardwareAccel);
			this.groupFilters.Controls.Add(this.checkDoubleBuffering);
			this.groupFilters.Controls.Add(this.checkAllFormats);
			this.groupFilters.Location = new System.Drawing.Point(6, 39);
			this.groupFilters.Name = "groupFilters";
			this.groupFilters.Size = new System.Drawing.Size(295, 68);
			this.groupFilters.TabIndex = 10;
			this.groupFilters.TabStop = false;
			this.groupFilters.Text = "OpenGL filters for list below";
			// 
			// checkAllFormats
			// 
			this.checkAllFormats.Location = new System.Drawing.Point(6, 48);
			this.checkAllFormats.Name = "checkAllFormats";
			this.checkAllFormats.Size = new System.Drawing.Size(282, 17);
			this.checkAllFormats.TabIndex = 9;
			this.checkAllFormats.Text = "Show All Formats";
			this.checkAllFormats.UseVisualStyleBackColor = true;
			this.checkAllFormats.Click += new System.EventHandler(this.checkAllFormats_Click);
			// 
			// gridFormats
			// 
			this.gridFormats.Location = new System.Drawing.Point(6, 184);
			this.gridFormats.Name = "gridFormats";
			this.gridFormats.Size = new System.Drawing.Size(821, 223);
			this.gridFormats.TabIndex = 8;
			this.gridFormats.Title = "Available Graphics Formats";
			this.gridFormats.TranslationName = "TaleFormats";
			this.gridFormats.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridFormats_CellClick);
			// 
			// radioSimpleChart
			// 
			this.radioSimpleChart.Location = new System.Drawing.Point(15, 35);
			this.radioSimpleChart.Name = "radioSimpleChart";
			this.radioSimpleChart.Size = new System.Drawing.Size(146, 19);
			this.radioSimpleChart.TabIndex = 6;
			this.radioSimpleChart.TabStop = true;
			this.radioSimpleChart.Text = "Simple 2D (pie shapes)";
			this.radioSimpleChart.UseVisualStyleBackColor = true;
			this.radioSimpleChart.Click += new System.EventHandler(this.radioSimpleChart_Click);
			// 
			// radioOpenGLChart
			// 
			this.radioOpenGLChart.Location = new System.Drawing.Point(15, 52);
			this.radioOpenGLChart.Name = "radioOpenGLChart";
			this.radioOpenGLChart.Size = new System.Drawing.Size(147, 19);
			this.radioOpenGLChart.TabIndex = 7;
			this.radioOpenGLChart.TabStop = true;
			this.radioOpenGLChart.Text = "OpenGL (no perio)";
			this.radioOpenGLChart.UseVisualStyleBackColor = true;
			this.radioOpenGLChart.Click += new System.EventHandler(this.radioOpenGLChart_Click);
			// 
			// radioDirectXChart
			// 
			this.radioDirectXChart.Location = new System.Drawing.Point(15, 17);
			this.radioDirectXChart.Name = "radioDirectXChart";
			this.radioDirectXChart.Size = new System.Drawing.Size(215, 19);
			this.radioDirectXChart.TabIndex = 8;
			this.radioDirectXChart.TabStop = true;
			this.radioDirectXChart.Text = "DirectX 9 (requires installation of SDK)";
			this.radioDirectXChart.UseVisualStyleBackColor = true;
			this.radioDirectXChart.Click += new System.EventHandler(this.radioDirectXChart_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(684, 628);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(786, 628);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioDirectXChart);
			this.groupBox1.Controls.Add(this.radioSimpleChart);
			this.groupBox1.Controls.Add(this.radioOpenGLChart);
			this.groupBox1.Location = new System.Drawing.Point(604, 129);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(257, 75);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Older Tooth Chart Technology";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.radioDirectX11DontUse);
			this.groupBox2.Controls.Add(this.radioDirectX11Use);
			this.groupBox2.Location = new System.Drawing.Point(28, 28);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(481, 96);
			this.groupBox2.TabIndex = 12;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "DirectX 11 Tooth Chart on all computers";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(12, 19);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(452, 16);
			this.label5.TabIndex = 18;
			this.label5.Text = "This option affects all computers in the office.  All other options are specific " +
    "to this computer.";
			// 
			// radioDirectX11DontUse
			// 
			this.radioDirectX11DontUse.Location = new System.Drawing.Point(15, 58);
			this.radioDirectX11DontUse.Name = "radioDirectX11DontUse";
			this.radioDirectX11DontUse.Size = new System.Drawing.Size(442, 31);
			this.radioDirectX11DontUse.TabIndex = 9;
			this.radioDirectX11DontUse.TabStop = true;
			this.radioDirectX11DontUse.Text = "Do not use.  Advantage is that older or underpowered hardware might work better. " +
    " This is an easy first step for troubleshooting certain graphics problems.";
			this.radioDirectX11DontUse.UseVisualStyleBackColor = true;
			// 
			// radioDirectX11Use
			// 
			this.radioDirectX11Use.Location = new System.Drawing.Point(15, 38);
			this.radioDirectX11Use.Name = "radioDirectX11Use";
			this.radioDirectX11Use.Size = new System.Drawing.Size(442, 19);
			this.radioDirectX11Use.TabIndex = 8;
			this.radioDirectX11Use.TabStop = true;
			this.radioDirectX11Use.Text = "Use if available.  Advantage is that this avoids having to install DirectX9 SDK.";
			this.radioDirectX11Use.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(27, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(394, 16);
			this.label1.TabIndex = 17;
			this.label1.Text = "Changes in this window usually require a restart to take effect";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.radioDirectX11NotFound);
			this.groupBox3.Controls.Add(this.radioDirectX11Avail);
			this.groupBox3.Location = new System.Drawing.Point(236, 129);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(273, 75);
			this.groupBox3.TabIndex = 13;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "DirectX 11 status on this computer";
			// 
			// radioDirectX11NotFound
			// 
			this.radioDirectX11NotFound.AutoCheck = false;
			this.radioDirectX11NotFound.Location = new System.Drawing.Point(12, 35);
			this.radioDirectX11NotFound.Name = "radioDirectX11NotFound";
			this.radioDirectX11NotFound.Size = new System.Drawing.Size(255, 36);
			this.radioDirectX11NotFound.TabIndex = 9;
			this.radioDirectX11NotFound.TabStop = true;
			this.radioDirectX11NotFound.Text = "Sparks3D.dll not found, or Chart not used yet, or unable to load due to graphics " +
    "driver issue";
			this.radioDirectX11NotFound.UseVisualStyleBackColor = true;
			// 
			// radioDirectX11Avail
			// 
			this.radioDirectX11Avail.AutoCheck = false;
			this.radioDirectX11Avail.Location = new System.Drawing.Point(12, 16);
			this.radioDirectX11Avail.Name = "radioDirectX11Avail";
			this.radioDirectX11Avail.Size = new System.Drawing.Size(152, 19);
			this.radioDirectX11Avail.TabIndex = 8;
			this.radioDirectX11Avail.TabStop = true;
			this.radioDirectX11Avail.Text = "Available";
			this.radioDirectX11Avail.UseVisualStyleBackColor = true;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.radioDirectX11ThisCompUseGlobal);
			this.groupBox4.Controls.Add(this.radioDirectX11ThisCompYes);
			this.groupBox4.Controls.Add(this.radioDirectX11ThisCompNo);
			this.groupBox4.Location = new System.Drawing.Point(28, 129);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(180, 75);
			this.groupBox4.TabIndex = 12;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "DirectX 11 Tooth Chart";
			// 
			// radioDirectX11ThisCompUseGlobal
			// 
			this.radioDirectX11ThisCompUseGlobal.Location = new System.Drawing.Point(15, 17);
			this.radioDirectX11ThisCompUseGlobal.Name = "radioDirectX11ThisCompUseGlobal";
			this.radioDirectX11ThisCompUseGlobal.Size = new System.Drawing.Size(159, 19);
			this.radioDirectX11ThisCompUseGlobal.TabIndex = 8;
			this.radioDirectX11ThisCompUseGlobal.TabStop = true;
			this.radioDirectX11ThisCompUseGlobal.Text = "Use the global setting";
			this.radioDirectX11ThisCompUseGlobal.UseVisualStyleBackColor = true;
			// 
			// radioDirectX11ThisCompYes
			// 
			this.radioDirectX11ThisCompYes.Location = new System.Drawing.Point(15, 35);
			this.radioDirectX11ThisCompYes.Name = "radioDirectX11ThisCompYes";
			this.radioDirectX11ThisCompYes.Size = new System.Drawing.Size(146, 19);
			this.radioDirectX11ThisCompYes.TabIndex = 6;
			this.radioDirectX11ThisCompYes.TabStop = true;
			this.radioDirectX11ThisCompYes.Text = "Use if available";
			this.radioDirectX11ThisCompYes.UseVisualStyleBackColor = true;
			// 
			// radioDirectX11ThisCompNo
			// 
			this.radioDirectX11ThisCompNo.Location = new System.Drawing.Point(15, 52);
			this.radioDirectX11ThisCompNo.Name = "radioDirectX11ThisCompNo";
			this.radioDirectX11ThisCompNo.Size = new System.Drawing.Size(147, 19);
			this.radioDirectX11ThisCompNo.TabIndex = 7;
			this.radioDirectX11ThisCompNo.TabStop = true;
			this.radioDirectX11ThisCompNo.Text = "Do not use";
			this.radioDirectX11ThisCompNo.UseVisualStyleBackColor = true;
			// 
			// FormGraphics
			// 
			this.ClientSize = new System.Drawing.Size(892, 665);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.group3DToothChart);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormGraphics";
			this.ShowInTaskbar = false;
			this.Text = "Graphics Preferences";
			this.Load += new System.EventHandler(this.FormGraphics_Load);
			this.group3DToothChart.ResumeLayout(false);
			this.group3DToothChart.PerformLayout();
			this.groupFilters.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormGraphics_Load(object sender,EventArgs e) {
			if(ComputerPrefCur==null) {
				ComputerPrefCur=ComputerPrefs.LocalComputer;
			}
			else {
				if(ComputerPrefCur!=ComputerPrefs.LocalComputer) {
					_isRemoteEdit=true;
				}
				MessageBox.Show("Warning, editing another computers graphical settings should be done from that computer to ensure the selected settings work." 
					+" We do not recommend editing this way. If you make changes for another computer you should still verifiy that they work on that machine.");
					SecurityLogs.MakeLogEntry(Permissions.GraphicsRemoteEdit,0,"Edited graphical settings for "+ComputerPrefCur.ComputerName);
			}
			Text+=" - "+ComputerPrefCur.ComputerName;
			if(ComputerPrefCur.ComputerOS==PlatformOD.Undefined){//Remote computer has not updated yet. 
				MessageBox.Show("Selected computer needs to be updated before being able to remotely change its graphical settings.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(ComputerPrefCur.ComputerOS==PlatformOD.Unix) {//Force simple mode on Unix systems.
				MessageBox.Show("Linux users must use simple tooth chart.");
				radioDirectXChart.Enabled=false;
				radioOpenGLChart.Enabled=false;
				group3DToothChart.Enabled=false;
				return;
			}
			//ComputerPref computerPref=ComputerPrefs.GetForLocalComputer();
			checkHardwareAccel.Checked=ComputerPrefCur.GraphicsUseHardware;
			checkDoubleBuffering.Checked=ComputerPrefCur.GraphicsDoubleBuffering;
			selectedFormatNum=ComputerPrefCur.PreferredPixelFormatNum;
			selectedDirectXFormat=ComputerPrefCur.DirectXFormat;
			textSelected.Text="";
			if(Prefs.GetBool(PrefName.DirectX11ToothChartUseIfAvail)){
				radioDirectX11Use.Checked=true;
			}
			else{
				radioDirectX11DontUse.Checked=true;
			}
			if(ComputerPrefCur.GraphicsUseDirectX11==YN.Yes) {
				radioDirectX11ThisCompYes.Checked=true;
			}
			else if(ComputerPrefCur.GraphicsUseDirectX11==YN.No) {
				radioDirectX11ThisCompNo.Checked=true;
			}
			else {
				radioDirectX11ThisCompUseGlobal.Checked=true;
			}

				radioDirectX11NotFound.Checked=true;
			
			if(ComputerPrefCur.GraphicsSimple==DrawingMode.Simple2D) {
				radioSimpleChart.Checked=true;
				group3DToothChart.Enabled=false;
			}
			else if(ComputerPrefCur.GraphicsSimple==DrawingMode.DirectX) {
				radioDirectXChart.Checked=true;
				group3DToothChart.Enabled=true;
				groupFilters.Enabled=false;
			}
			else{//OpenGL
				radioOpenGLChart.Checked=true;
				group3DToothChart.Enabled=true;
				groupFilters.Enabled=true;
			}
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				radioDirectXChart.Text="Use DirectX Graphics (recommended)";
				radioSimpleChart.Text="Use Simple Graphics";
				radioOpenGLChart.Text="Use OpenGL Graphics";
				group3DToothChart.Text="Options For 3D Graphics";
				label3DChart.Text="Most users will never need to change any of these options.  These are only used when the 3D graphics are not working "
					+"properly.";
			}
			RefreshFormats();
		}

		private void FillGrid() {
			int selectionIndex=-1;
			gridFormats.BeginUpdate();
			gridFormats.ListGridRows.Clear();
			if(this.radioDirectXChart.Checked){
				textSelected.Text="";
				gridFormats.BeginUpdate();
				gridFormats.ListGridColumns.Clear();
				GridColumn col=new GridColumn(Lan.G(this,"FormatNum"),80);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"Adapter"),60);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"Accelerated"),80);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"Buffered"),75);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"ColorBits"),75);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"ColorFormat"),75);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"DepthBits"),75);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"DepthFormat"),75);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"Antialiasing"),75);
				gridFormats.ListGridColumns.Add(col);
				gridFormats.EndUpdate();
				for(int i=0;i<xformats.Length;i++) {
					GridRow row=new GridRow();
					row.Cells.Add((i+1).ToString());
					row.Cells.Add(xformats[i].adapterIndex.ToString());
					row.Cells.Add(xformats[i].IsHardware?"Yes":"No");//Supports hardware acceleration?
					row.Cells.Add("Yes");//Supports double-buffering. All DirectX devices support double-buffering as required.
					row.Cells.Add(ToothChartDirectX.GetFormatBitCount(xformats[i].BackBufferFormat).ToString());//Color bits.
					row.Cells.Add(xformats[i].BackBufferFormat);
					row.Cells.Add(ToothChartDirectX.GetFormatBitCount(xformats[i].DepthStencilFormat).ToString());//Depth buffer bits.
					row.Cells.Add(xformats[i].DepthStencilFormat);
					row.Cells.Add(xformats[i].MultiSampleCount.ToString());
					gridFormats.ListGridRows.Add(row);
					if(xformats[i].ToString()==selectedDirectXFormat) {
					  selectionIndex=i;
						textSelected.Text=(i+1).ToString();
					}
				}
			}
			else if(this.radioOpenGLChart.Checked){
				textSelected.Text=selectedFormatNum.ToString();
				gridFormats.BeginUpdate();
				gridFormats.ListGridColumns.Clear();
				GridColumn col=new GridColumn(Lan.G(this,"FormatNum"),80);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"OpenGL"),60);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"Windowed"),80);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"Bitmapped"),80);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"Palette"),75);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"Accelerated"),80);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"Buffered"),75);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"ColorBits"),75);
				gridFormats.ListGridColumns.Add(col);
				col=new GridColumn(Lan.G(this,"DepthBits"),75);
				gridFormats.ListGridColumns.Add(col);
				gridFormats.EndUpdate();
				for(int i=0;i<formats.Length;i++) {
					GridRow row=new GridRow();
					row.Cells.Add(formats[i].formatNumber.ToString());
					row.Cells.Add(OpenGLWinFormsControl.FormatSupportsOpenGL(formats[i].pfd)?"Yes":"No");
					row.Cells.Add(OpenGLWinFormsControl.FormatSupportsWindow(formats[i].pfd)?"Yes":"No");
					row.Cells.Add(OpenGLWinFormsControl.FormatSupportsBitmap(formats[i].pfd)?"Yes":"No");
					row.Cells.Add(OpenGLWinFormsControl.FormatUsesPalette(formats[i].pfd)?"Yes":"No");
					row.Cells.Add(OpenGLWinFormsControl.FormatSupportsAcceleration(formats[i].pfd)?"Yes":"No");
					row.Cells.Add(OpenGLWinFormsControl.FormatSupportsDoubleBuffering(formats[i].pfd)?"Yes":"No");
					row.Cells.Add(formats[i].pfd.cColorBits.ToString());
					row.Cells.Add(formats[i].pfd.cDepthBits.ToString());
					gridFormats.ListGridRows.Add(row);
					if(formats[i].formatNumber==selectedFormatNum) {
						selectionIndex=i;
					}
				}
			}
			gridFormats.EndUpdate();
			if(selectionIndex>=0) {
				gridFormats.SetSelected(selectionIndex,true);
			}
		}

		///<Summary>Get all formats for the grid based on the current filters.</Summary>
		private void RefreshFormats() {
			this.Cursor=Cursors.WaitCursor;
			gridFormats.ListGridRows.Clear();
			gridFormats.Invalidate();
			textSelected.Text="";
			Application.DoEvents();
			if(this.radioDirectXChart.Checked){
				xformats=ToothChartDirectX.GetStandardDeviceFormats();
			}
			else if(this.radioOpenGLChart.Checked){
				OpenGLWinFormsControl contextFinder=new OpenGLWinFormsControl();
				//Get raw formats.
				Gdi.PIXELFORMATDESCRIPTOR[] rawformats=OpenGLWinFormsControl.GetPixelFormats(contextFinder.GetHDC());
				if(checkAllFormats.Checked){
					formats=new OpenGLWinFormsControl.PixelFormatValue[rawformats.Length];
					for(int i=0;i<rawformats.Length;i++) {
						formats[i]=new OpenGLWinFormsControl.PixelFormatValue();
						formats[i].formatNumber=i+1;
						formats[i].pfd=rawformats[i];
					}
				}
				else{
					//Prioritize formats as requested by the user.
					formats=OpenGLWinFormsControl.PrioritizePixelFormats(rawformats,checkDoubleBuffering.Checked,checkHardwareAccel.Checked);
				}
				contextFinder.Dispose();
			}
			//Update the format grid to reflect possible changes in formats.
			FillGrid();
			this.Cursor=Cursors.Default;
		}

		private void radioSimpleChart_Click(object sender,EventArgs e) {
			group3DToothChart.Enabled=false;
		}

		private void radioDirectXChart_Click(object sender,EventArgs e) {
			group3DToothChart.Enabled=true;
			groupFilters.Enabled=false;
			RefreshFormats();
		}

		private void radioOpenGLChart_Click(object sender,EventArgs e) {
			group3DToothChart.Enabled=true;
			groupFilters.Enabled=true;
			RefreshFormats();
		}

		private void checkHardwareAccel_Click(object sender,EventArgs e) {
			RefreshFormats();
		}

		private void checkDoubleBuffering_Click(object sender,EventArgs e) {
			RefreshFormats();
		}

		private void checkAllFormats_Click(object sender,EventArgs e) {
			checkHardwareAccel.Enabled=!checkAllFormats.Checked;
			checkDoubleBuffering.Enabled=!checkAllFormats.Checked;
			RefreshFormats();
		}

		private void gridFormats_CellClick(object sender,ODGridClickEventArgs e) {
			int formatNum=Convert.ToInt32(gridFormats.ListGridRows[e.Row].Cells[0].Text);
			textSelected.Text=formatNum.ToString();
			if(radioDirectXChart.Checked) {
				selectedDirectXFormat=xformats[formatNum-1].ToString();
			}
			else if(this.radioOpenGLChart.Checked){
				selectedFormatNum=formatNum;
			}
		}

		///<summary>CAUTION: Runs slowly. May take minutes. Returns the string "invalid" if no suitable Direct X format can be found.</summary>
		public static string GetPreferredDirectXFormat(Form callingForm) {
			ToothChartDirectX.DirectXDeviceFormat[] possibleStandardFormats=ToothChartDirectX.GetStandardDeviceFormats();
			for(int i=0;i<possibleStandardFormats.Length;i++) {
				if(TestDirectXFormat(callingForm,possibleStandardFormats[i].ToString())) {
					return possibleStandardFormats[i].ToString();
				}
			}
			return "invalid";
		}

		///<summary>Returns true if the given directXFormat works for a DirectX tooth chart on the local computer.</summary>
		public static bool TestDirectXFormat(Form callingForm,string directXFormat) {
			ToothChartWrapper toothChartTest=new ToothChartWrapper();
			toothChartTest.Visible=false;
			//We add the invisible tooth chart to our form so that the device context will initialize properly
			//and our device creation test will then be accurate.
			callingForm.Controls.Add(toothChartTest);
			toothChartTest.DeviceFormat=new ToothChartDirectX.DirectXDeviceFormat(directXFormat);
			toothChartTest.DrawMode=DrawingMode.DirectX;//Creates the device.
			if(toothChartTest.DrawMode==DrawingMode.Simple2D) {
				//The chart is set back to 2D mode when there is an error initializing.
				callingForm.Controls.Remove(toothChartTest);
				toothChartTest.Dispose();
				return false;
			}
			//Now we check to be sure that one can draw and retrieve a screen shot from a DirectX control
			//using the specified device format.
			try {
				Bitmap screenShot=toothChartTest.GetBitmap();
				screenShot.Dispose();
			} 
			catch {
				callingForm.Controls.Remove(toothChartTest);
				toothChartTest.Dispose();
				return false;
			}
			callingForm.Controls.Remove(toothChartTest);
			toothChartTest.Dispose();
			return true;
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			bool changed=Prefs.Set(PrefName.DirectX11ToothChartUseIfAvail,radioDirectX11Use.Checked);
			//ComputerPrefCur doesn't change until here, so make the old copy exactly when we need it instead of when the form is created.
			ComputerPref computerPrefOld=ComputerPrefCur.Copy();
			if(radioDirectX11ThisCompUseGlobal.Checked) {
				ComputerPrefCur.GraphicsUseDirectX11=YN.Unknown;
			}
			else if(radioDirectX11ThisCompYes.Checked) {
				ComputerPrefCur.GraphicsUseDirectX11=YN.Yes;
			}
			else if(radioDirectX11ThisCompNo.Checked) {
				ComputerPrefCur.GraphicsUseDirectX11=YN.No;
			}
			//ComputerPref computerPref=ComputerPrefs.GetForLocalComputer();
			if(radioDirectXChart.Checked) {
				if(!_isRemoteEdit && !TestDirectXFormat(this,selectedDirectXFormat)){
					MessageBox.Show(Lan.G(this,"Please choose a different device format, "+
						"the selected device format will not support the DirectX 3D tooth chart on this computer"));
					return;
				}
				ComputerPrefCur.GraphicsSimple=DrawingMode.DirectX;
				ComputerPrefCur.DirectXFormat=selectedDirectXFormat;
			}
			else if(radioSimpleChart.Checked) {
				ComputerPrefCur.GraphicsSimple=DrawingMode.Simple2D;
			}
			else { //OpenGL
				OpenGLWinFormsControl contextTester=new OpenGLWinFormsControl();
				try {
					if(!_isRemoteEdit && contextTester.TaoInitializeContexts(selectedFormatNum)!=selectedFormatNum) {
						throw new Exception(Lan.G(this,"Could not initialize pixel format ")+selectedFormatNum.ToString()+".");
					}
				} 
				catch(Exception ex) {
					MessageBox.Show(Lan.G(this,"Please choose a different pixel format, the selected pixel format will not support the 3D tooth chart on this computer: "+ex.Message));
					contextTester.Dispose();
					return;
				}
				contextTester.Dispose();
				ComputerPrefCur.GraphicsUseHardware=checkHardwareAccel.Checked;
				ComputerPrefCur.GraphicsDoubleBuffering=checkDoubleBuffering.Checked;
				ComputerPrefCur.PreferredPixelFormatNum=selectedFormatNum;
				ComputerPrefCur.GraphicsSimple=DrawingMode.OpenGL;
			}
			ComputerPrefs.Update(ComputerPrefCur,computerPrefOld);
			if(changed){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}





















