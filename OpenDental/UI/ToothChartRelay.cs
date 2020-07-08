using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using SparksToothChart;

namespace OpenDental{
	///<summary>Relays commands to either the old SparksToothChart.ToothChartWrapper or the new Sparks3d.ToothChart.</summary>
	public class ToothChartRelay{
		///<summary>This is set when the program starts up.  If true, it will load up the new tooth chart in many places.</summary>
		public static bool IsSparks3DPresent;
		private Sparks3DInterface _sparks3DInterface;
		private ToothChartWrapper _toothChartWrapper;

		public ToothChartRelay(bool hasHwnd=true){
			if(IsSparks3DPresent){
				try{
					_sparks3DInterface=new Sparks3DInterface(hasHwnd);
				}
				catch{
					//Probably older version of Windows that does not support DirectX 11.1.
					//which means Win7 instead of Win8+
					IsSparks3DPresent=false;
					return;
				}
				_sparks3DInterface.SegmentDrawn += _sparks3DInterface_SegmentDrawn;
				_sparks3DInterface.ToothSelectionsChanged += _sparks3DInterface_ToothSelectionsChanged;
			}
		}

		#region Events
		public event SparksToothChart.ToothChartDrawEventHandler SegmentDrawn=null;

		private void _sparks3DInterface_SegmentDrawn(object sender, ToothChartDrawEventArgs e){
			SegmentDrawn?.Invoke(sender,e);//simple bubble
		}

		public event SparksToothChart.ToothChartSelectionEventHandler ToothSelectionsChanged=null;

		private void _sparks3DInterface_ToothSelectionsChanged(object sender){
			ToothSelectionsChanged?.Invoke(sender);//simple bubble
		}
		#endregion Events

		#region Properties
		public int Bottom{
			get{
				if(IsSparks3DPresent){
					return _sparks3DInterface.Bottom;
				}
				else{
					return _toothChartWrapper.Bottom;
				}
			}
		}

		public Color ColorBackgroundMain{
			set{
				if(IsSparks3DPresent){
					_sparks3DInterface.ColorBackgroundMain=value;
				}
				else{
					_toothChartWrapper.ColorBackground=value;
				}
			}
		}

		public Color ColorDrawing{
			set{
				if(IsSparks3DPresent){
					_sparks3DInterface.ColorDrawingCurrently=value;
				}
				else{
					_toothChartWrapper.ColorDrawing=value;
				}
			}
		}

		public Color ColorText{
			set{
				if(IsSparks3DPresent){
					_sparks3DInterface.ColorText=value;
				}
				else{
					_toothChartWrapper.ColorText=value;
				}
			}
		}

		public Color ColorTextHighlightBack{
			set{
				if(IsSparks3DPresent){
					_sparks3DInterface.ColorTextHighlightBack=value;
				}
				else{
					_toothChartWrapper.ColorBackHighlight=value;
				}
			}
		}

		public Color ColorTextHighlightFore{
			set{
				if(IsSparks3DPresent){
					_sparks3DInterface.ColorTextHighlightFore=value;
				}
				else{
					_toothChartWrapper.ColorTextHighlight=value;
				}
			}
		}

		public CursorTool CursorTool{
			get{
				if(IsSparks3DPresent){
					return _sparks3DInterface.CursorTool;
				}
				else{
					return _toothChartWrapper.CursorTool;
				}
			}
			set{
				if(IsSparks3DPresent){
					_sparks3DInterface.CursorTool=value;
				}
				else{
					_toothChartWrapper.CursorTool=value;
				}
			}
		}

		public bool Enabled{
			set{
				if(IsSparks3DPresent){
					_sparks3DInterface.Enabled=value;
				}
				else{
					_toothChartWrapper.Enabled=value;
				}
			}
		}

		public int Height{
			get{
				if(IsSparks3DPresent){
					return _sparks3DInterface.Height;
				}
				else{
					return _toothChartWrapper.Height;
				}
			}
		}

		public bool IsPerioMode{
			set{
				if(IsSparks3DPresent){
					_sparks3DInterface.IsPerioMode=true;
				}
				else{
					_toothChartWrapper.PerioMode=true;
				}
			}
		}

		public List<string> SelectedTeeth{
			get{
				if(IsSparks3DPresent){
					return _sparks3DInterface.SelectedTeeth;
				}
				else{
					return _toothChartWrapper.SelectedTeeth;
				}
			}
		}

		public int Width{
			get{
				if(IsSparks3DPresent){
					return _sparks3DInterface.Width;
				}
				else{
					return _toothChartWrapper.Width;
				}
			}
		}
		#endregion Properties

		#region Methods
		public void AddDrawingSegment(ToothInitial drawingSegment){
			if(IsSparks3DPresent){
				_sparks3DInterface.AddDrawingSegment(drawingSegment.DrawingSegment,drawingSegment.ColorDraw);
			}
			else{
				_toothChartWrapper.AddDrawingSegment(drawingSegment);
			}
		}

		public void AddPerioMeasure(int intTooth,PerioSequenceType sequenceType,int mb,int b,int db,int ml,int l, int dl){
			if(IsSparks3DPresent){
				_sparks3DInterface.AddPerioMeasure(intTooth,sequenceType,mb,b,db,ml,l,dl);
			}
			else{
				_toothChartWrapper.AddPerioMeasure(intTooth,sequenceType,mb,b,db,ml,l,dl);
			}
		}

		///<summary>Sparks3D.BeginUpdate and toothChartWrapper.SuspendLayout</summary>
		public void BeginUpdate(){
			if(IsSparks3DPresent){
				_sparks3DInterface.BeginUpdate();
			}
			else{
				_toothChartWrapper.SuspendLayout();//badly named
			}
		}

		public void DisposeControl(){
			if(IsSparks3DPresent){
				_sparks3DInterface?.Dispose();
			}
			else{
				_toothChartWrapper.Dispose();
			}
		}

		///<summary>Sparks3D.EndUpdate and toothChartWrapper.ResumeLayout</summary>
		public void EndUpdate(){
			if(IsSparks3DPresent){
				_sparks3DInterface.EndUpdate();
			}
			else{
				_toothChartWrapper.ResumeLayout();//badly named
			}
		}

		public Bitmap GetBitmap(){
			if(IsSparks3DPresent){
				return _sparks3DInterface.GetBitmap();
			}
			else{
				return _toothChartWrapper.GetBitmap();
			}
		}

		public Control GetToothChart(){
			return _sparks3DInterface.GetToothChart();
		}

		public void MoveTooth(string toothID,float rotate,float tipM,float tipB,float shiftM,float shiftO,float shiftB){
			if(IsSparks3DPresent){
				_sparks3DInterface.MoveTooth(toothID,rotate,tipM,tipB,shiftM,shiftO,shiftB);
			}
			else{
				_toothChartWrapper.MoveTooth(toothID,rotate,tipM,tipB,shiftM,shiftO,shiftB);
			}
		}

		public void ResetTeeth(){
			if(IsSparks3DPresent){
				_sparks3DInterface.ResetTeeth();
			}
			else{
				_toothChartWrapper.ResetTeeth();
			}
		}

		public void SetBigX(string toothID,Color color){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetBigX(toothID,color);
			}
			else{
				_toothChartWrapper.SetBigX(toothID,color);
			}
		}

		public void SetBU(string toothID,Color color){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetBU(toothID,color);
			}
			else{
				_toothChartWrapper.SetBU(toothID,color);
			}
		}

		public void SetCrown(string toothID,Color color){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetCrown(toothID,color);
			}
			else{
				_toothChartWrapper.SetCrown(toothID,color);
			}
		}

		public void SetHidden(string toothID){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetHidden(toothID);
			}
			else{
				_toothChartWrapper.SetHidden(toothID);
			}
		}

		public void SetImplant(string toothID,Color color){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetImplant(toothID,color);
			}
			else{
				_toothChartWrapper.SetImplant(toothID,color);
			}
		}

		public void SetMissing(string toothID){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetMissing(toothID);
			}
			else{
				_toothChartWrapper.SetMissing(toothID);
			}
		}

		public void SetMobility(string toothID,string mobility,Color color){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetMobility(toothID,mobility,color);
			}
			else{
				_toothChartWrapper.SetMobility(toothID,mobility,color);
			}
		}

		public void SetPerioColors(Color colorBleeding,Color colorSuppuration,Color colorProbing,Color colorProbingRed,Color colorGM,Color colorCAL,
			Color colorMGJ,Color colorFurcations,Color colorFurcationsRed,int redLimitProbing,int redLimitFurcations){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetPerioColors(colorBleeding,colorSuppuration,colorProbing,colorProbingRed,colorGM,colorCAL,
					colorMGJ,colorFurcations,colorFurcationsRed,redLimitProbing,redLimitFurcations);
			}
			else{
				_toothChartWrapper.ColorBleeding=colorBleeding;
				_toothChartWrapper.ColorSuppuration=colorSuppuration;
				_toothChartWrapper.ColorProbing=colorProbing;
				_toothChartWrapper.ColorProbingRed=colorProbingRed;
				_toothChartWrapper.ColorGingivalMargin=colorGM;
				_toothChartWrapper.ColorCAL=colorCAL;
				_toothChartWrapper.ColorMGJ=colorMGJ;
				_toothChartWrapper.ColorFurcations=colorFurcations;
				_toothChartWrapper.ColorFurcationsRed=colorFurcationsRed;
				_toothChartWrapper.RedLimitProbing=redLimitProbing;
				_toothChartWrapper.RedLimitFurcations=redLimitFurcations;
			}
		}

		public void SetPontic(string toothID,Color color){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetPontic(toothID,color);
			}
			else{
				_toothChartWrapper.SetPontic(toothID,color);
			}
		}

		public void SetPrimary(string toothID){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetPrimary(toothID);
			}
			else{
				_toothChartWrapper.SetPrimary(toothID);
			}
		}

		public void SetRCT(string toothID,Color color){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetRCT(toothID,color);
			}
			else{
				_toothChartWrapper.SetRCT(toothID,color);
			}
		}

		public void SetSealant(string toothID,Color color){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetSealant(toothID,color);
			}
			else{
				_toothChartWrapper.SetSealant(toothID,color);
			}
		}

		public void SetSelected(string toothID,bool setValue){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetSelected(toothID,setValue);
			}
			else{
				_toothChartWrapper.SetSelected(toothID,setValue);
			}
		}

		public void SetSurfaceColors(string toothID,string surfaces,Color color){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetSurfaceColors(toothID,surfaces,color);
			}
			else{
				_toothChartWrapper.SetSurfaceColors(toothID,surfaces,color);
			}
		}

		public void SetToothChartWrapper(ToothChartWrapper toothChartWrapper){
			_toothChartWrapper=toothChartWrapper;
		}

		public void SetToothNumberingNomenclature(ToothNumberingNomenclature toothNumberingNomenclature){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetToothNumberingNomenclature(toothNumberingNomenclature);
			}
			else{
				_toothChartWrapper.SetToothNumberingNomenclature(toothNumberingNomenclature);
			}
		}

		public void SetVeneer(string toothID,Color color){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetVeneer(toothID,color);
			}
			else{
				_toothChartWrapper.SetVeneer(toothID,color);
			}
		}

		public void SetWatch(string toothID,Color color){
			if(IsSparks3DPresent){
				_sparks3DInterface.SetWatch(toothID,color);
			}
			else{
				_toothChartWrapper.SetWatch(toothID,color);
			}
		}
		#endregion Methods

	}

	
}
