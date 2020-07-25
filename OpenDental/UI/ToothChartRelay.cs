using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using SparksToothChart;

namespace OpenDental
{
	///<summary>Relays commands to either the old SparksToothChart.ToothChartWrapper or the new Sparks3d.ToothChart.</summary>
	public class ToothChartRelay
	{
		private ToothChartWrapper _toothChartWrapper;

		public ToothChartRelay(bool hasHwnd = true)
		{
		}

		#region Events
		public event SparksToothChart.ToothChartDrawEventHandler SegmentDrawn = null;

		public event SparksToothChart.ToothChartSelectionEventHandler ToothSelectionsChanged = null;

		#endregion Events

		#region Properties
		public int Bottom
		{
			get
			{
				return _toothChartWrapper.Bottom;
			}
		}

		public Color ColorBackgroundMain
		{
			set
			{
				_toothChartWrapper.ColorBackground = value;
			}
		}

		public Color ColorDrawing
		{
			set
			{

					_toothChartWrapper.ColorDrawing = value;
				
			}
		}

		public Color ColorText
		{
			set
			{
				_toothChartWrapper.ColorText = value;
			}
		}

		public Color ColorTextHighlightBack
		{
			set
			{
				_toothChartWrapper.ColorBackHighlight = value;
			}
		}

		public Color ColorTextHighlightFore
		{
			set
			{
				_toothChartWrapper.ColorTextHighlight = value;
			}
		}

		public CursorTool CursorTool
		{
			get
			{
				return _toothChartWrapper.CursorTool;
			}
			set
			{
				_toothChartWrapper.CursorTool = value;
			}
		}

		public bool Enabled
		{
			set
			{
				_toothChartWrapper.Enabled = value;
			}
		}

		public int Height
		{
			get
			{
				return _toothChartWrapper.Height;
			}
		}

		public bool IsPerioMode
		{
			set
			{
				_toothChartWrapper.PerioMode = true;
			}
		}

		public List<string> SelectedTeeth
		{
			get
			{
				return _toothChartWrapper.SelectedTeeth;
			}
		}

		public int Width
		{
			get
			{
				return _toothChartWrapper.Width;
			}
		}

		#endregion Properties

		#region Methods
		public void AddDrawingSegment(ToothInitial drawingSegment)
		{
			_toothChartWrapper.AddDrawingSegment(drawingSegment);
		}

		public void AddPerioMeasure(int intTooth, PerioSequenceType sequenceType, int mb, int b, int db, int ml, int l, int dl)
		{

			_toothChartWrapper.AddPerioMeasure(intTooth, sequenceType, mb, b, db, ml, l, dl);

		}

		///<summary>Sparks3D.BeginUpdate and toothChartWrapper.SuspendLayout</summary>
		public void BeginUpdate()
		{

			_toothChartWrapper.SuspendLayout();//badly named

		}

		public void DisposeControl()
		{

			_toothChartWrapper.Dispose();

		}

		///<summary>Sparks3D.EndUpdate and toothChartWrapper.ResumeLayout</summary>
		public void EndUpdate()
		{

			_toothChartWrapper.ResumeLayout();//badly named

		}

		public Bitmap GetBitmap()
		{
			return _toothChartWrapper.GetBitmap();
		}

		//public Control GetToothChart()
		//{
		//	//_toothChartWrapper;

		//	//return _sparks3DInterface.GetToothChart();

		//	return null;
		//}

		public void MoveTooth(string toothID, float rotate, float tipM, float tipB, float shiftM, float shiftO, float shiftB)
		{

			_toothChartWrapper.MoveTooth(toothID, rotate, tipM, tipB, shiftM, shiftO, shiftB);

		}

		public void ResetTeeth()
		{

			_toothChartWrapper.ResetTeeth();

		}

		public void SetBigX(string toothID, Color color)
		{

			_toothChartWrapper.SetBigX(toothID, color);

		}

		public void SetBU(string toothID, Color color)
		{

			_toothChartWrapper.SetBU(toothID, color);

		}

		public void SetCrown(string toothID, Color color)
		{

			_toothChartWrapper.SetCrown(toothID, color);

		}

		public void SetHidden(string toothID)
		{

			_toothChartWrapper.SetHidden(toothID);

		}

		public void SetImplant(string toothID, Color color)
		{

			_toothChartWrapper.SetImplant(toothID, color);

		}

		public void SetMissing(string toothID)
		{

			_toothChartWrapper.SetMissing(toothID);

		}

		public void SetMobility(string toothID, string mobility, Color color)
		{
			_toothChartWrapper.SetMobility(toothID, mobility, color);
		}

		public void SetPerioColors(Color colorBleeding, Color colorSuppuration, Color colorProbing, Color colorProbingRed, Color colorGM, Color colorCAL, Color colorMGJ, Color colorFurcations, Color colorFurcationsRed, int redLimitProbing, int redLimitFurcations)
		{
			_toothChartWrapper.ColorBleeding = colorBleeding;
			_toothChartWrapper.ColorSuppuration = colorSuppuration;
			_toothChartWrapper.ColorProbing = colorProbing;
			_toothChartWrapper.ColorProbingRed = colorProbingRed;
			_toothChartWrapper.ColorGingivalMargin = colorGM;
			_toothChartWrapper.ColorCAL = colorCAL;
			_toothChartWrapper.ColorMGJ = colorMGJ;
			_toothChartWrapper.ColorFurcations = colorFurcations;
			_toothChartWrapper.ColorFurcationsRed = colorFurcationsRed;
			_toothChartWrapper.RedLimitProbing = redLimitProbing;
			_toothChartWrapper.RedLimitFurcations = redLimitFurcations;
		}

		public void SetPontic(string toothID, Color color)
		{
			_toothChartWrapper.SetPontic(toothID, color);
		}

		public void SetPrimary(string toothID)
		{
			_toothChartWrapper.SetPrimary(toothID);
		}

		public void SetRCT(string toothID, Color color)
		{
			_toothChartWrapper.SetRCT(toothID, color);
		}

		public void SetSealant(string toothID, Color color)
		{
			_toothChartWrapper.SetSealant(toothID, color);
		}

		public void SetSelected(string toothID, bool setValue)
		{
			_toothChartWrapper.SetSelected(toothID, setValue);
		}

		public void SetSurfaceColors(string toothID, string surfaces, Color color)
		{
			_toothChartWrapper.SetSurfaceColors(toothID, surfaces, color);
		}

		public void SetToothChartWrapper(ToothChartWrapper toothChartWrapper)
		{
			_toothChartWrapper = toothChartWrapper;
		}

		public void SetToothNumberingNomenclature(ToothNumberingNomenclature toothNumberingNomenclature)
		{
			_toothChartWrapper.SetToothNumberingNomenclature(toothNumberingNomenclature);
		}

		public void SetVeneer(string toothID, Color color)
		{
			_toothChartWrapper.SetVeneer(toothID, color);
		}

		public void SetWatch(string toothID, Color color)
		{
			_toothChartWrapper.SetWatch(toothID, color);
		}

		#endregion Methods
	}
}
