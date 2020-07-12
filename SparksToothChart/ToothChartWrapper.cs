using OpenDentBusiness;
using SparksToothChart.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SparksToothChart
{
    /// <summary>
    /// This is the old tooth chart control.
    /// It "wraps" three different old tooth charts: OpenGL, DirectX9, and 2D.
    /// </summary>
    public partial class ToothChartWrapper : UserControl
	{
        private ToothChartOpenGL toothChartOpenGL;
		private ToothChartDirectX toothChartDirectX;
        private DrawingMode drawMode;
		private ToothChartData chartData;

		[Category("Action"), Description("Occurs when the mouse goes up ending a drawing segment.")]
		public event ToothChartDrawEventHandler SegmentDrawn = null;

		[Category("Action"), Description("Occurs when the mouse goes up committing tooth selection.")]
		public event ToothChartSelectionEventHandler ToothSelectionsChanged = null;

		public ToothChartWrapper()
		{
			drawMode = DrawingMode.Simple2D;
			chartData = new ToothChartData();
			InitializeComponent();
			ResetControls();
		}

		#region Properties

		public DrawingMode DrawMode
		{
			get
			{
				return drawMode;
			}
			set
			{
				if (Environment.OSVersion.Platform == PlatformID.Unix)
				{
					return;//disallow changing from simpleMode if platform is Unix
				}
				//do not break out if not changing mode.  ContrChart.InitializeOnStartup assumes this code will always run.
				if (drawMode == DrawingMode.DirectX && value != DrawingMode.DirectX)
				{
					//If switching from from DirectX to another drawing mode,
					//then we need to cleanup DirectX resources in case the 
					//chart is never switched back to DirectX mode.
					toothChartDirectX.Dispose();//Calls CleanupDirectX() and device.Dispose().
					toothChartDirectX = null;
				}
				try
				{
					drawMode = value;
					ResetControls();
				}
				catch
				{
					drawMode = DrawingMode.Simple2D;
					ResetControls();
				}
			}
		}

		/// <summary>
		/// This data object holds nearly all information about what to draw.
		/// It is normally acted on by public methods of the wrapper instead of being accessed directly.
		/// </summary>
		public ToothChartData TcData
		{
			get
			{
				return chartData;
			}
			set
			{
				if (drawMode == DrawingMode.Simple2D)
				{
					toothChart2D.TcData = value;
				}
				else if (drawMode == DrawingMode.DirectX)
				{
					if (chartData != null)
					{
						chartData.CleanupDirectX();//Clean up old tc data DirectX objects to free video memory.
					}
					toothChartDirectX.TcData = value;
					toothChartDirectX.TcData.PrepareForDirectX(toothChartDirectX.device);
				}
				else if (drawMode == DrawingMode.OpenGL)
				{
					toothChartOpenGL.TcData = value;
				}

				chartData = value;
			}
		}

		/// <summary>
		/// Valid values are 1-32 and A-Z.
		/// </summary>
		public List<string> SelectedTeeth => chartData.SelectedTeeth;


		[Browsable(false)]
		public Color ColorBackground
		{
			get => chartData.ColorBackground;
			set
			{
				chartData.ColorBackground = value;
				Invalidate();
			}
		}

		[Browsable(false)]
		public Color ColorText
		{
			set
			{
				chartData.ColorText = value;
				Invalidate();
			}
		}

		[Browsable(false)]
		public Color ColorTextHighlight
		{
			set
			{
				chartData.ColorTextHighlight = value;
				Invalidate();
			}
		}

		[Browsable(false)]
		public Color ColorBackHighlight
		{
			set
			{
				chartData.ColorBackHighlight = value;
				Invalidate();
			}
		}

        /// <summary>
		/// Set to true when using hardware rendering in OpenGL, and false otherwise. 
		/// This will have no effect when in simple 2D graphics mode.
		/// </summary>
        [Browsable(false)]
        public bool UseHardware { get; set; } = false;

        [Browsable(false)]
		public bool AutoFinish
		{
			get
			{
				if (drawMode == DrawingMode.OpenGL)
				{
					return toothChartOpenGL.autoFinish;
				}
				else
				{
					return false;
				}
			}
			set
			{
				if (drawMode == DrawingMode.OpenGL)
				{
					toothChartOpenGL.autoFinish = value;
				}
			}
		}

        [Browsable(false)]
        public int PreferredPixelFormatNumber { get; set; }

        [Browsable(false)]
        public ToothChartDirectX.DirectXDeviceFormat DeviceFormat { get; set; } = null;

		private static Cursor GetCursor(byte[] cursor)
        {
			using (var memoryStream = new MemoryStream(cursor))
            {
				return new Cursor(memoryStream);
            }
        }

        [Browsable(false)]
		public CursorTool CursorTool
		{
			get
			{
				return chartData.CursorTool;
			}
			set
			{
				chartData.CursorTool = value;
				if (chartData.CursorTool == CursorTool.Pointer)
				{
					Cursor = Cursors.Default;
				}
				if (chartData.CursorTool == CursorTool.Pen)
				{
					Cursor = GetCursor(Resources.CursorPen);
				}
				if (chartData.CursorTool == CursorTool.Eraser)
				{
					Cursor = GetCursor(Resources.CursorEraseCircle);
				}
				if (chartData.CursorTool == CursorTool.ColorChanger)
				{
					Cursor = GetCursor(Resources.CursorColorChanger);
				}
			}
		}

		/// <summary>
		/// For the freehand drawing tool.
		/// </summary>
		[Browsable(false)]
		public Color ColorDrawing
		{
			get => chartData.ColorDrawing;
			set
			{
				chartData.ColorDrawing = value;
			}
		}

		[Browsable(false)]
		public bool PerioMode
		{
			get
			{
				return chartData.PerioMode;
			}
			set
			{
				if (drawMode != DrawingMode.DirectX && value == true)
				{
					throw new Exception("Only allowed in DirectX");
				}
				chartData.PerioMode = value;
				Invalidate();
			}
		}

		[Browsable(false)]
		public Color ColorBleeding
		{
			get => chartData.ColorBleeding;
			set
			{
				chartData.ColorBleeding = value;
			}
		}

		[Browsable(false)]
		public Color ColorSuppuration
		{
			get => chartData.ColorSuppuration;
			set
			{
				chartData.ColorSuppuration = value;
			}
		}

		[Browsable(false)]
		public Color ColorFurcations
		{
			get => chartData.ColorFurcations;
			set
			{
				chartData.ColorFurcations = value;
			}
		}

		[Browsable(false)]
		public Color ColorFurcationsRed
		{
			get => chartData.ColorFurcationsRed;
			set
			{
				chartData.ColorFurcationsRed = value;
			}
		}

		[Browsable(false)]
		public Color ColorGingivalMargin
		{
			get => chartData.ColorGingivalMargin;
			set
			{
				chartData.ColorGingivalMargin = value;
			}
		}

		[Browsable(false)]
		public Color ColorCAL
		{
			get => chartData.ColorCAL;
			set
			{
				chartData.ColorCAL = value;
			}
		}

		[Browsable(false)]
		public Color ColorMGJ
		{
			get => chartData.ColorMGJ;
			set
			{
				chartData.ColorMGJ = value;
			}
		}

		[Browsable(false)]
		public Color ColorProbing
		{
			get => chartData.ColorProbing;
			set
			{
				chartData.ColorProbing = value;
			}
		}

		[Browsable(false)]
		public Color ColorProbingRed
		{
			get => chartData.ColorProbingRed;
			set
			{
				chartData.ColorProbingRed = value;
			}
		}

		[Browsable(false)]
		public int RedLimitProbing
		{
			get => chartData.RedLimitProbing;
			set
			{
				chartData.RedLimitProbing = value;
			}
		}

		[Browsable(false)]
		public int RedLimitFurcations
		{
			get => chartData.RedLimitFurcations;
			set
			{
				chartData.RedLimitFurcations = value;
			}
		}

		#endregion Properties

		protected override void OnInvalidated(InvalidateEventArgs e)
		{
			base.OnInvalidated(e);
			if (drawMode == DrawingMode.Simple2D)
			{
				toothChart2D.Invalidate();
			}
			else if (drawMode == DrawingMode.DirectX)
			{
				toothChartDirectX.Invalidate();
			}
			else if (drawMode == DrawingMode.OpenGL)
			{
				toothChartOpenGL.Invalidate();
				//toothChartOpenGL.TaoDraw();
			}
		}

		private void ResetControls()
		{
			Controls.Clear();

			if (drawMode == DrawingMode.Simple2D)
			{
                toothChart2D = new ToothChart2D
                {
                    Dock = DockStyle.Fill,
                    Location = new Point(0, 0),
                    Name = "toothChart2D"
                };
                toothChart2D.SegmentDrawn += new ToothChartDrawEventHandler(ToothChart_SegmentDrawn);
				toothChart2D.ToothSelectionsChanged += new ToothChartSelectionEventHandler(ToothChart_ToothSelectionsChanged);
				toothChart2D.TcData = chartData;
				toothChart2D.SuspendLayout();
				Controls.Add(toothChart2D);

				ResetTeeth();
				toothChart2D.InitializeGraphics();
				toothChart2D.ResumeLayout();
			}
			else if (drawMode == DrawingMode.DirectX)
			{
				// I noticed that this code executes when the program starts, then also when the Chart module is selected for the first time.
				// Thus the Chart graphic was loading twice before the user could see it.
				bool isInitialized = true;
				if (toothChartDirectX == null)
				{
					isInitialized = false;
				}

				if (isInitialized)
				{
					// Since the control is already initialized, reuse it.  This helps the load time of the Chart module and helps to prevent an issue.
					// This flag helps prevent a red X on the tooth chart when the Chart module is left open and the Windows user is switched then switched back.
				}
				else
				{
					toothChartDirectX = new ToothChartDirectX();
				}

				toothChartDirectX.Dock = DockStyle.Fill;
				toothChartDirectX.Location = new Point(0, 0);
				toothChartDirectX.Name = "toothChartDirectX";
				toothChartDirectX.SegmentDrawn += new ToothChartDrawEventHandler(ToothChart_SegmentDrawn);
				toothChartDirectX.ToothSelectionsChanged += new ToothChartSelectionEventHandler(ToothChart_ToothSelectionsChanged);
				toothChartDirectX.TcData = chartData;
				toothChartDirectX.SuspendLayout(); // Might help with the MDA debug error we used to get (if the option wasn't disabled in our compilers).
				Controls.Add(toothChartDirectX);

				ResetTeeth();
				toothChartDirectX.deviceFormat = DeviceFormat;
				if (!isInitialized)
				{
					toothChartDirectX.InitializeGraphics();
				}
				toothChartDirectX.ResumeLayout(); // Might help with the MDA debug error we used to get (if the option wasn't disabled in our compilers).
			}
			else if (drawMode == DrawingMode.OpenGL)
			{
				toothChartOpenGL = new ToothChartOpenGL(UseHardware, PreferredPixelFormatNumber);
				PreferredPixelFormatNumber = toothChartOpenGL.SelectedPixelFormatNumber;
				toothChartOpenGL.Dock = DockStyle.Fill;
				toothChartOpenGL.Location = new Point(0, 0);
				toothChartOpenGL.Name = "toothChartOpenGL";
				toothChartOpenGL.TcData = chartData;
				toothChartOpenGL.SegmentDrawn += new ToothChartDrawEventHandler(ToothChart_SegmentDrawn);
				toothChartOpenGL.ToothSelectionsChanged += new ToothChartSelectionEventHandler(ToothChart_ToothSelectionsChanged);
				toothChartOpenGL.SuspendLayout();
				Controls.Add(toothChartOpenGL);

				ResetTeeth();
				toothChartOpenGL.InitializeGraphics();
				toothChartOpenGL.ResumeLayout();
			}
		}

		#region Public Methods

		/// <summary>
		/// If ListToothGraphics is empty, then this fills it, including the complex process of loading all drawing points from local resources.
		/// Or if not empty, then this resets all 32+20 teeth to default postitions, no restorations, etc. Primary teeth set to visible false.
		/// Also clears selected.  Should surround with SuspendLayout / ResumeLayout.
		/// </summary>
		public void ResetTeeth()
		{
			// This will only happen once when program first loads.
			// Unfortunately, there is no way to tell what the drawMode is going to be when loading the graphics from the file.
			// So any other initialization must happen in resetControls.
			if (chartData.ListToothGraphics.Count == 0)
			{
				chartData.ListToothGraphics.Clear();
				ToothGraphic tooth;
				for (int i = 1; i <= 32; i++)
				{
                    tooth = new ToothGraphic(i.ToString())
                    {
                        Visible = true
                    };

                    chartData.ListToothGraphics.Add(tooth);

					// Primary
					if (Tooth.PermToPri(i.ToString()) != "")
					{
                        tooth = new ToothGraphic(Tooth.PermToPri(i.ToString()))
                        {
                            Visible = false
                        };

                        chartData.ListToothGraphics.Add(tooth);
					}
				}
				tooth = new ToothGraphic("implant");
				chartData.ListToothGraphics.Add(tooth);
			}
			else
			{
				// List was already initially filled, but now user needs to reset it.
				for (int i = 0; i < chartData.ListToothGraphics.Count; i++)
				{
					// Loop through all perm and pri teeth.
					chartData.ListToothGraphics[i].Reset();
				}
			}

			chartData.SelectedTeeth.Clear();
			chartData.DrawingSegmentList = new List<ToothInitial>();
			chartData.PointList = new List<PointF>();
			Invalidate();
		}

		/// <summary>
		/// Moves position of tooth. 
		/// Rotations first in order listed, then translations. 
		/// Tooth doesn't get moved immediately, just when painting. 
		/// All changes are cumulative and are in addition to any previous translations and rotations.
		/// </summary>
		public void MoveTooth(string toothID, float rotate, float tipM, float tipB, float shiftM, float shiftO, float shiftB)
		{
			if (!ToothGraphic.IsValidToothID(toothID)) return;

			chartData.ListToothGraphics[toothID].ShiftM += shiftM;
			chartData.ListToothGraphics[toothID].ShiftO += shiftO;
			chartData.ListToothGraphics[toothID].ShiftB += shiftB;
			chartData.ListToothGraphics[toothID].Rotate += rotate;
			chartData.ListToothGraphics[toothID].TipM += tipM;
			chartData.ListToothGraphics[toothID].TipB += tipB;
			Invalidate();
		}

		/// <summary>
		/// Sets the specified permanent tooth to primary. 
		/// Works as follows: Sets ShowPrimaryLetter to true for the perm tooth. 
		/// Makes pri tooth visible=true.  Also repositions perm tooth by translating -Y. 
		/// Moves primary tooth slightly to M or D sometimes for better alignment. 
		/// And if 2nd primary molar, then because of the larger size, it must move all perm molars to distal.
		/// </summary>
		public void SetPrimary(string toothID)
		{
			if (!ToothGraphic.IsValidToothID(toothID) || Tooth.IsPrimary(toothID)) return;
			
			chartData.ListToothGraphics[toothID].ShiftO -= 12;
			if (ToothGraphic.IsValidToothID(Tooth.PermToPri(toothID)))
			{
				// If there's a primary tooth at this location
				chartData.ListToothGraphics[Tooth.PermToPri(toothID)].Visible = true;//show the primary.
				chartData.ListToothGraphics[toothID].ShowPrimaryLetter = true;
			}

			// First pri mand molars, shift slightly to M
			if (toothID == "21")
			{
				chartData.ListToothGraphics["J"].ShiftM += 0.5f;
			}

			if (toothID == "28")
			{
				chartData.ListToothGraphics["S"].ShiftM += 0.5f;
			}

			// Second pri molars are huge, so shift distally for space and move all the perm molars distally too
			if (toothID == "4")
			{
				chartData.ListToothGraphics["A"].ShiftM -= 0.5f;
				chartData.ListToothGraphics["1"].ShiftM -= 1;
				chartData.ListToothGraphics["2"].ShiftM -= 1;
				chartData.ListToothGraphics["3"].ShiftM -= 1;
			}

			if (toothID == "13")
			{
				chartData.ListToothGraphics["J"].ShiftM -= 0.5f;
				chartData.ListToothGraphics["14"].ShiftM -= 1;
				chartData.ListToothGraphics["15"].ShiftM -= 1;
				chartData.ListToothGraphics["16"].ShiftM -= 1;
			}

			if (toothID == "20")
			{
				chartData.ListToothGraphics["K"].ShiftM -= 1.2f;
				chartData.ListToothGraphics["17"].ShiftM -= 2.3f;
				chartData.ListToothGraphics["18"].ShiftM -= 2.3f;
				chartData.ListToothGraphics["19"].ShiftM -= 2.3f;
			}

			if (toothID == "29")
			{
				chartData.ListToothGraphics["T"].ShiftM -= 1.2f;
				chartData.ListToothGraphics["30"].ShiftM -= 2.3f;
				chartData.ListToothGraphics["31"].ShiftM -= 2.3f;
				chartData.ListToothGraphics["32"].ShiftM -= 2.3f;
			}

			Invalidate();
		}

		/// <summary>
		/// This is used for crowns and for retainers. 
		/// Crowns will be visible on missing teeth with implants. 
		/// Crowns are visible on F and O views, unlike ponics which are only visible on F view. 
		/// If the tooth is not visible, that should be set before this call, because then, this will set the root invisible.
		/// Tooth numbers 1-32 or A-T.  Supernumeraries not supported here yet.
		/// </summary>
		public void SetCrown(string toothID, Color color)
		{
			if (!ToothGraphic.IsValidToothID(toothID)) return;
			
			chartData.ListToothGraphics[toothID].IsCrown = true;
			if (!chartData.ListToothGraphics[toothID].Visible)
			{
				// Tooth not visible, so set root invisible.
				chartData.ListToothGraphics[toothID].SetGroupVisibility(ToothGroupType.Cementum, false);
			}

			chartData.ListToothGraphics[toothID].SetSurfaceColors("MODBLFIV", color);
			chartData.ListToothGraphics[toothID].SetGroupColor(ToothGroupType.Enamel, color);
			chartData.ListToothGraphics[toothID].SetGroupColor(ToothGroupType.EnamelF, color);

			Invalidate();
		}

		/// <summary>
		/// A series of color settings will result in the last ones entered overriding earlier entries.
		/// </summary>
		public void SetSurfaceColors(string toothID, string surfaces, Color color)
		{
			if (!ToothGraphic.IsValidToothID(toothID)) return;
			
			chartData.ListToothGraphics[toothID].SetSurfaceColors(surfaces, color);
			Invalidate();
		}

		/// <summary>
		/// Used for missing teeth. 
		/// This should always be done before setting restorations, because a pontic will cause the tooth
		/// to become visible again except for the root.  So if setMissing after a pontic, then the 
		/// pontic can't show.
		/// </summary>
		public void SetMissing(string toothID)
		{
			if (!ToothGraphic.IsValidToothID(toothID)) return;
			
			chartData.ListToothGraphics[toothID].Visible = false;
			Invalidate();
		}

		/// <summary>
		/// This is just the same as SetMissing, except that it also hides the number from showing. 
		/// This is used, for example, if premolars are missing, and ortho has completely closed the space. 
		/// User will not be able to select this tooth because the number is hidden.
		/// </summary>
		public void SetHidden(string toothID)
		{
			if (!ToothGraphic.IsValidToothID(toothID)) return;
			
			chartData.ListToothGraphics[toothID].Visible = false;
			chartData.ListToothGraphics[toothID].HideNumber = true;
			Invalidate();
		}

		/// <summary>
		/// This is used for any pontic, including bridges, full dentures, and partials. 
		/// It is usually used on a tooth that has already been set invisible. 
		/// This routine cuases the tooth to show again, but the root needs to be invisible. 
		/// Then, it sets the entire crown to the specified color. 
		/// If the tooth is already visible, then it does not set the root invisible.
		/// Tooth numbers 1-32 or A-J or T-K. Supernumeraries not supported here yet.
		/// </summary>
		public void SetPontic(string toothID, Color color)
		{
			if (!ToothGraphic.IsValidToothID(toothID)) return;

			chartData.ListToothGraphics[toothID].IsPontic = true;
			if (!chartData.ListToothGraphics[toothID].Visible)
			{
				// Tooth not visible, but since IsPontic changes the visibility behavior of the tooth, we need to set the root invisible.
				chartData.ListToothGraphics[toothID].SetGroupVisibility(ToothGroupType.Cementum, false);
			}

			chartData.ListToothGraphics[toothID].SetSurfaceColors("MODBLFIV", color);
			chartData.ListToothGraphics[toothID].SetGroupColor(ToothGroupType.Enamel, color);
			chartData.ListToothGraphics[toothID].SetGroupColor(ToothGroupType.EnamelF, color);
			Invalidate();
		}

		/// <summary>
		/// Root canals are initially not visible. 
		/// This routine sets the canals visible, changes the color to the one specified, and also sets 
		/// the cementum for the tooth to be semitransparent so that the canals can be seen. 
		/// Also sets the IsRCT flag for the tooth to true.
		/// </summary>
		public void SetRCT(string toothID, Color color)
		{
			if (!ToothGraphic.IsValidToothID(toothID)) return;

			chartData.ListToothGraphics[toothID].IsRCT = true;
			chartData.ListToothGraphics[toothID].colorRCT = color;
			Invalidate();
		}

		/// <summary>
		/// This draws a big red extraction X right on top of the tooth. 
		/// It's up to the calling application to figure out when it's appropriate to do this. 
		/// Even if the tooth has been marked invisible, there's a good chance that this will still get 
		/// drawn because a tooth can be set visible again for the drawing the pontic.  So the calling 
		/// application needs to figure out when it's appropriate to draw the X, and not set this 
		/// otherwise.
		/// </summary>
		public void SetBigX(string toothID, Color color)
		{
			if (!ToothGraphic.IsValidToothID(toothID)) return;
			
			chartData.ListToothGraphics[toothID].DrawBigX = true;
			chartData.ListToothGraphics[toothID].colorX = color;
			Invalidate();
		}

		/// <summary>
		/// Set this tooth to show a BU or post.
		/// </summary>
		public void SetBU(string toothID, Color color)
		{
			if (!ToothGraphic.IsValidToothID(toothID)) return;
			

			// Buildups are now just another group, so
			chartData.ListToothGraphics[toothID].SetGroupVisibility(ToothGroupType.Buildup, true);
			chartData.ListToothGraphics[toothID].SetGroupColor(ToothGroupType.Buildup, color);
			Invalidate();
		}

		/// <summary>
		/// Set this tooth to show an implant.
		/// </summary>
		public void SetImplant(string toothID, Color color)
		{
			if (!ToothGraphic.IsValidToothID(toothID)) return;
			
			chartData.ListToothGraphics[toothID].IsImplant = true;
			chartData.ListToothGraphics[toothID].colorImplant = color;
			Invalidate();
		}

		/// <summary>
		/// Set this tooth to show a sealant.
		/// </summary>
		public void SetSealant(string toothID, Color color)
		{
			if (!ToothGraphic.IsValidToothID(toothID)) return;
			
			chartData.ListToothGraphics[toothID].IsSealant = true;
			chartData.ListToothGraphics[toothID].colorSealant = color;
			Invalidate();
		}

		/// <summary>
		/// This will mostly only be successful on certain anterior teeth. 
		/// For others, it will just show F coloring.
		/// </summary>
		public void SetVeneer(string toothID, Color color)
		{
			if (!ToothGraphic.IsValidToothID(toothID)) return;

			chartData.ListToothGraphics[toothID].SetSurfaceColors("BFV", color);
			chartData.ListToothGraphics[toothID].SetGroupColor(ToothGroupType.EnamelF, color);
			chartData.ListToothGraphics[toothID].SetGroupColor(ToothGroupType.DF, color);
			chartData.ListToothGraphics[toothID].SetGroupColor(ToothGroupType.MF, color);
			chartData.ListToothGraphics[toothID].SetGroupColor(ToothGroupType.IF, color);
			Invalidate();
		}

		/// <summary>
		/// Set this tooth to show a 'W' to indicate that the tooth is being watched.
		/// </summary>
		public void SetWatch(string toothID, Color color)
		{
			if (!ToothGraphic.IsValidToothID(toothID)) return;
			
			chartData.ListToothGraphics[toothID].Watch = true;
			chartData.ListToothGraphics[toothID].colorWatch = color;
			Invalidate();
		}

		public void AddDrawingSegment(ToothInitial drawingSegment)
		{
			bool alreadyAdded = false;
			for (int i = 0; i < chartData.DrawingSegmentList.Count; i++)
			{
				if (chartData.DrawingSegmentList[i].DrawingSegment == drawingSegment.DrawingSegment)
				{
					alreadyAdded = true;
					break;
				}
			}

			if (!alreadyAdded)
			{
				chartData.DrawingSegmentList.Add(drawingSegment);
			}

			Invalidate();
		}

		/// <summary>
		/// Returns a bitmap of what is showing in the control. Used for printing.
		/// </summary>
		public Bitmap GetBitmap()
		{
			if (drawMode == DrawingMode.Simple2D)
			{
				return toothChart2D.GetBitmap();
			}

			if (drawMode == DrawingMode.OpenGL)
			{
				return toothChartOpenGL.GetBitmap();
			}

			if (drawMode == DrawingMode.DirectX)
			{
				return toothChartDirectX.GetBitmap();
			}

			return null;
		}

		public void SetToothNumberingNomenclature(ToothNumberingNomenclature nomenclature)
		{
			chartData.ToothNumberingNomenclature = nomenclature;
			Invalidate();
		}

		public void SetMobility(string toothID, string mobility, Color color)
		{
			if (!ToothGraphic.IsValidToothID(toothID)) return;

			chartData.ListToothGraphics[toothID].Mobility = mobility;
			chartData.ListToothGraphics[toothID].colorMobility = color;
			Invalidate();
		}

		public void AddPerioMeasure(int intTooth, PerioSequenceType sequenceType, int mb, int b, int db, int ml, int l, int dl)
		{
            chartData.ListPerioMeasure.Add(new PerioMeasure
			{
				MBvalue = mb,
				Bvalue = b,
				DBvalue = db,
				MLvalue = ml,
				Lvalue = l,
				DLvalue = dl,
				IntTooth = intTooth,
				SequenceType = sequenceType
			});
		}

		public void AddPerioMeasure(PerioMeasure pm)
		{
			chartData.ListPerioMeasure.Add(pm);
		}

		#endregion

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			chartData.SizeControl = Size;
			Invalidate();

			if (drawMode == DrawingMode.DirectX)
			{
				// Fire the resize event for the DirectX tooth chart.
				// For some reason the Resize() and Size() events don't fire on the DirectX control
				// if you create them through the designer. Perhaps there is something wrong, but this
				// works for now.
				toothChartDirectX.SetSize(Size);
			}
		}

		public void SetSelected(string tooth_id, bool setValue)
		{
			chartData.SetSelected(tooth_id, setValue);
			Invalidate();
		}

		protected void OnSegmentDrawn(string drawingSegment) 
			=> SegmentDrawn?.Invoke(this, new ToothChartDrawEventArgs(drawingSegment));
        
		protected void OnToothSelectionsChanged() 
			=> ToothSelectionsChanged?.Invoke(this);
   
		private void ToothChart_SegmentDrawn(object sender, ToothChartDrawEventArgs e) 
			=> OnSegmentDrawn(e.DrawingSegement);

		private void ToothChart_ToothSelectionsChanged(object sender) 
			=> OnToothSelectionsChanged();
	}

	public enum CursorTool
	{
		Pointer,
		Pen,
		Eraser,
		ColorChanger
	}

	public delegate void ToothChartDrawEventHandler(object sender, ToothChartDrawEventArgs e);

	public delegate void ToothChartSelectionEventHandler(object sender);

	public class ToothChartDrawEventArgs
	{
		public ToothChartDrawEventArgs(string drawingSeg)
		{
			DrawingSegement = drawingSeg;
		}

		public string DrawingSegement { get; }
	}
}
