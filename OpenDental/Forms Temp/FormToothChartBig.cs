using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Tao.Platform.Windows;
using SparksToothChart;
using OpenDentBusiness;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormToothChartingBig:ODForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private bool ShowBySelectedTeeth;
		private List<ToothInitial> ToothInitialList;
		private ToothChartWrapper toothChartWrapper;
		private ToothChartRelay _toothChartRelay;
		///<summary>This is the new Sparks3D toothChart.</summary>
		private Control toothChart;
		private List<DataRow> ProcList;

		///<summary></summary>
		public FormToothChartingBig(bool showBySelectedTeeth,List<ToothInitial> toothInitialList,List<DataRow> procList)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			ShowBySelectedTeeth=showBySelectedTeeth;
			ToothInitialList=toothInitialList;
			ProcList=procList;
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
			SparksToothChart.ToothChartData toothChartData1 = new SparksToothChart.ToothChartData();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormToothChartingBig));
			this.toothChartWrapper = new SparksToothChart.ToothChartWrapper();
			this.SuspendLayout();
			// 
			// toothChartWrapper
			// 
			this.toothChartWrapper.AutoFinish = false;
			this.toothChartWrapper.ColorBackground = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
			this.toothChartWrapper.Cursor = System.Windows.Forms.Cursors.Default;
			this.toothChartWrapper.CursorTool = SparksToothChart.CursorTool.Pointer;
			this.toothChartWrapper.DeviceFormat = null;
			this.toothChartWrapper.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toothChartWrapper.DrawMode = OpenDentBusiness.DrawingMode.Simple2D;
			this.toothChartWrapper.Location = new System.Drawing.Point(0, 0);
			this.toothChartWrapper.Name = "toothChartWrapper";
			this.toothChartWrapper.PerioMode = false;
			this.toothChartWrapper.PreferredPixelFormatNumber = 0;
			this.toothChartWrapper.Size = new System.Drawing.Size(926, 858);
			this.toothChartWrapper.TabIndex = 0;
			toothChartData1.SizeControl = new System.Drawing.Size(926, 858);
			this.toothChartWrapper.TcData = toothChartData1;
			this.toothChartWrapper.UseHardware = false;
			this.toothChartWrapper.Visible = false;
			// 
			// FormToothChartingBig
			// 
			this.ClientSize = new System.Drawing.Size(926, 858);
			this.Controls.Add(this.toothChartWrapper);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormToothChartingBig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormToothChartingBig_FormClosed);
			this.Load += new System.EventHandler(this.FormToothChartingBig_Load);
			this.ResizeEnd += new System.EventHandler(this.FormToothChartingBig_ResizeEnd);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormToothChartingBig_Load(object sender,EventArgs e) {
			_toothChartRelay= new ToothChartRelay();
			_toothChartRelay.SetToothChartWrapper(toothChartWrapper);

				toothChartWrapper.Visible=true;
				//ComputerPref computerPref=ComputerPrefs.GetForLocalComputer();
				toothChartWrapper.UseHardware=ComputerPrefs.LocalComputer.GraphicsUseHardware;
				toothChartWrapper.PreferredPixelFormatNumber=ComputerPrefs.LocalComputer.PreferredPixelFormatNum;
				//Must be last preference set, last so that all settings are caried through in the reinitialization this line triggers.
				if(ComputerPrefs.LocalComputer.GraphicsSimple==DrawingMode.Simple2D) {
					toothChartWrapper.DrawMode=DrawingMode.Simple2D;
				}
				else if(ComputerPrefs.LocalComputer.GraphicsSimple==DrawingMode.DirectX) {
					toothChartWrapper.DeviceFormat=new ToothChartDirectX.DirectXDeviceFormat(ComputerPrefs.LocalComputer.DirectXFormat);
					toothChartWrapper.DrawMode=DrawingMode.DirectX;
				}
				else{
					toothChartWrapper.DrawMode=DrawingMode.OpenGL;
				}
				//The preferred pixel format number changes to the selected pixel format number after a context is chosen.
				ComputerPrefs.LocalComputer.PreferredPixelFormatNum=toothChartWrapper.PreferredPixelFormatNumber;
				ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			
			FillToothChart();
			//toothChart.Refresh();
		}

		private void FormToothChartingBig_ResizeEnd(object sender,EventArgs e) {
			FillToothChart();
			//toothChart.Refresh();
		}

		///<summary>This is, of course, called when module refreshed.  But it's also called when user sets missing teeth or tooth movements.  In that case, the Progress notes are not refreshed, so it's a little faster.  This also fills in the movement amounts.</summary>
		private void FillToothChart(){
			Cursor=Cursors.WaitCursor;
			_toothChartRelay.BeginUpdate();
			_toothChartRelay.SetToothNumberingNomenclature((ToothNumberingNomenclature)PrefC.GetInt(PrefName.UseInternationalToothNumbers));
			List<Definition> listDefs=Definitions.GetDefsForCategory(DefinitionCategory.ChartGraphicColors);
			_toothChartRelay.ColorBackgroundMain=listDefs[10].Color;
			_toothChartRelay.ColorText=listDefs[11].Color;
			_toothChartRelay.ColorTextHighlightFore=listDefs[12].Color;
			_toothChartRelay.ColorTextHighlightBack=listDefs[13].Color;
			//remember which teeth were selected
			List<string> selectedTeeth=new List<string>(toothChartWrapper.SelectedTeeth);
			//ArrayList selectedTeeth=new ArrayList();//integers 1-32
			//for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
			//	selectedTeeth.Add(Tooth.ToInt(toothChart.SelectedTeeth[i]));
			//}
			_toothChartRelay.ResetTeeth();
			//if(PatCur==null) {
				//toothChart.ResumeLayout();
				//FillMovementsAndHidden();
				//Cursor=Cursors.Default;
				//return;
			//}
			if(ShowBySelectedTeeth) {
				for(int i=0;i<selectedTeeth.Count;i++) {
					_toothChartRelay.SetSelected(selectedTeeth[i],true);
				}
			}
			//first, primary.  That way, you can still set a primary tooth missing afterwards.
			for(int i=0;i<ToothInitialList.Count;i++) {
				if(ToothInitialList[i].InitialType==ToothInitialType.Primary) {
					_toothChartRelay.SetPrimary(ToothInitialList[i].ToothNum);
				}
			}
			for(int i=0;i<ToothInitialList.Count;i++) {
				switch(ToothInitialList[i].InitialType) {
					case ToothInitialType.Missing:
						_toothChartRelay.SetMissing(ToothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Hidden:
						_toothChartRelay.SetHidden(ToothInitialList[i].ToothNum);
						break;
					//case ToothInitialType.Primary:
					//	break;
					case ToothInitialType.Rotate:
						_toothChartRelay.MoveTooth(ToothInitialList[i].ToothNum,ToothInitialList[i].Movement,0,0,0,0,0);
						break;
					case ToothInitialType.TipM:
						_toothChartRelay.MoveTooth(ToothInitialList[i].ToothNum,0,ToothInitialList[i].Movement,0,0,0,0);
						break;
					case ToothInitialType.TipB:
						_toothChartRelay.MoveTooth(ToothInitialList[i].ToothNum,0,0,ToothInitialList[i].Movement,0,0,0);
						break;
					case ToothInitialType.ShiftM:
						_toothChartRelay.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,ToothInitialList[i].Movement,0,0);
						break;
					case ToothInitialType.ShiftO:
						_toothChartRelay.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,0,ToothInitialList[i].Movement,0);
						break;
					case ToothInitialType.ShiftB:
						_toothChartRelay.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,0,0,ToothInitialList[i].Movement);
						break;
					case ToothInitialType.Drawing:
						_toothChartRelay.AddDrawingSegment(ToothInitialList[i].Copy());
						break;
				}
			}
			DrawProcGraphics();
			_toothChartRelay.EndUpdate();
			//FillMovementsAndHidden();
			Cursor=Cursors.Default;
		}

		private void DrawProcGraphics() {
			//this requires: ProcStatus, ProcCode, ToothNum, HideGraphics, Surf, and ToothRange.  All need to be raw database values.
			string[] teeth;
			Color cLight=Color.White;
			Color cDark=Color.White;
			List<Definition> listDefs=Definitions.GetDefsForCategory(DefinitionCategory.ChartGraphicColors,true);
			for(int i=0;i<ProcList.Count;i++) {
				if(ProcList[i]["HideGraphics"].ToString()=="1") {
					continue;
				}
				if(ProcedureCodes.GetProcCode(ProcList[i]["ProcCode"].ToString()).PaintType==ToothPaintingType.Extraction && (
					PIn.Long(ProcList[i]["ProcStatus"].ToString())==(int)ProcStat.C
					|| PIn.Long(ProcList[i]["ProcStatus"].ToString())==(int)ProcStat.EC
					|| PIn.Long(ProcList[i]["ProcStatus"].ToString())==(int)ProcStat.EO
					)) {
					continue;//prevents the red X. Missing teeth already handled.
				}
				if(ProcedureCodes.GetProcCode(ProcList[i]["ProcCode"].ToString()).GraphicColor.ToArgb()==Color.FromArgb(0).ToArgb()) {
					switch((ProcStat)PIn.Long(ProcList[i]["ProcStatus"].ToString())) {
						case ProcStat.C:
							cDark=listDefs[1].Color;
							cLight=listDefs[6].Color;
							break;
						case ProcStat.TP:
							cDark=listDefs[0].Color;
							cLight=listDefs[5].Color;
							break;
						case ProcStat.EC:
							cDark=listDefs[2].Color;
							cLight=listDefs[7].Color;
							break;
						case ProcStat.EO:
							cDark=listDefs[3].Color;
							cLight=listDefs[8].Color;
							break;
						case ProcStat.R:
							cDark=listDefs[4].Color;
							cLight=listDefs[9].Color;
							break;
						case ProcStat.Cn:
							cDark=listDefs[16].Color;
							cLight=listDefs[17].Color;
							break;
						case ProcStat.D://Can happen with invalidated locked procs.
						default:
							continue;//Don't draw.
					}
				}
				else {
					cDark=ProcedureCodes.GetProcCode(ProcList[i]["ProcCode"].ToString()).GraphicColor;
					cLight=ProcedureCodes.GetProcCode(ProcList[i]["ProcCode"].ToString()).GraphicColor;
				}
				switch(ProcedureCodes.GetProcCode(ProcList[i]["ProcCode"].ToString()).PaintType) {
					case ToothPaintingType.BridgeDark:
						if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,ProcList[i]["ToothNum"].ToString())) {
							_toothChartRelay.SetPontic(ProcList[i]["ToothNum"].ToString(),cDark);
						}
						else {
							_toothChartRelay.SetCrown(ProcList[i]["ToothNum"].ToString(),cDark);
						}
						break;
					case ToothPaintingType.BridgeLight:
						if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,ProcList[i]["ToothNum"].ToString())) {
							_toothChartRelay.SetPontic(ProcList[i]["ToothNum"].ToString(),cLight);
						}
						else {
							_toothChartRelay.SetCrown(ProcList[i]["ToothNum"].ToString(),cLight);
						}
						break;
					case ToothPaintingType.CrownDark:
						_toothChartRelay.SetCrown(ProcList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.CrownLight:
						_toothChartRelay.SetCrown(ProcList[i]["ToothNum"].ToString(),cLight);
						break;
					case ToothPaintingType.DentureDark:
						if(ProcList[i]["Surf"].ToString()=="U") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+2).ToString();
							}
						}
						else if(ProcList[i]["Surf"].ToString()=="L") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+18).ToString();
							}
						}
						else {
							teeth=ProcList[i]["ToothRange"].ToString().Split(new char[] { ',' });
						}
						for(int t=0;t<teeth.Length;t++) {
							if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,teeth[t])) {
								_toothChartRelay.SetPontic(teeth[t],cDark);
							}
							else {
								_toothChartRelay.SetCrown(teeth[t],cDark);
							}
						}
						break;
					case ToothPaintingType.DentureLight:
						if(ProcList[i]["Surf"].ToString()=="U") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+2).ToString();
							}
						}
						else if(ProcList[i]["Surf"].ToString()=="L") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+18).ToString();
							}
						}
						else {
							teeth=ProcList[i]["ToothRange"].ToString().Split(new char[] { ',' });
						}
						for(int t=0;t<teeth.Length;t++) {
							if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,teeth[t])) {
								_toothChartRelay.SetPontic(teeth[t],cLight);
							}
							else {
								_toothChartRelay.SetCrown(teeth[t],cLight);
							}
						}
						break;
					case ToothPaintingType.Extraction:
						_toothChartRelay.SetBigX(ProcList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.FillingDark:
						_toothChartRelay.SetSurfaceColors(ProcList[i]["ToothNum"].ToString(),ProcList[i]["Surf"].ToString(),cDark);
						break;
					case ToothPaintingType.FillingLight:
						_toothChartRelay.SetSurfaceColors(ProcList[i]["ToothNum"].ToString(),ProcList[i]["Surf"].ToString(),cLight);
						break;
					case ToothPaintingType.Implant:
						_toothChartRelay.SetImplant(ProcList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.PostBU:
						_toothChartRelay.SetBU(ProcList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.RCT:
						_toothChartRelay.SetRCT(ProcList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.Sealant:
						_toothChartRelay.SetSealant(ProcList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.Veneer:
						_toothChartRelay.SetVeneer(ProcList[i]["ToothNum"].ToString(),cLight);
						break;
					case ToothPaintingType.Watch:
						_toothChartRelay.SetWatch(ProcList[i]["ToothNum"].ToString(),cDark);
						break;
				}
			}
		}

		private void FormToothChartingBig_FormClosed(object sender,FormClosedEventArgs e) {
			//This helps ensure that the tooth chart wrapper is properly disposed of.
			//This step is necessary so that graphics memory does not fill up.
			Dispose();
		}	


	}
}






















