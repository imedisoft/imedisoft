using OpenDentBusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace SparksToothChart
{
    public partial class ToothChart2D : UserControl
	{
		/// <summary>
		/// This is a reference to the TcData object that's at the wrapper level.
		/// </summary>
		public ToothChartData TcData;

		private bool MouseIsDown;
		
		/// <summary>
		/// Mouse move causes this variable to be updated with the current tooth that the mouse is hovering over.
		/// </summary>
		private string hotTooth;
		
		/// <summary>
		/// The previous hotTooth. 
		/// If this is different than hotTooth, then mouse has just now moved to a new tooth. 
		/// Can be 0 to represent no previous.
		/// </summary>
		private string hotToothOld;


		[Category("Action"), Description("Occurs when the mouse goes up ending a drawing segment.")]
		public event ToothChartDrawEventHandler SegmentDrawn = null;

		[Category("Action"), Description("Occurs when the mouse goes up committing tooth selection.")]
		public event ToothChartSelectionEventHandler ToothSelectionsChanged = null;

		/// <summary>
		/// GDI+ handle to this control. Used for line drawing and font measurement.
		/// </summary>
		private Graphics g = null;

		private List<string> _listSelectedTeethOld = new List<string>();

		public ToothChart2D()
		{
			InitializeComponent();
		}

		public void InitializeGraphics()
		{
			g = CreateGraphics();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (DesignMode)
			{
				e.Graphics.DrawImage(pictBox.Image, new Rectangle(0, 0, this.Width, this.Height));
				return;
			}

			// Our strategy here will be to draw on a new bitmap.
			Bitmap bitmap = new Bitmap(Width, Height);
			Graphics g = Graphics.FromImage(bitmap);
			g.Clear(TcData.ColorBackground);

			// Draw a copy of the tooth chart background
			g.DrawImage(pictBox.Image, TcData.RectTarget);
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			// Loop through each tooth
			for (int t = 0; t < TcData.ListToothGraphics.Count; t++)
			{
				if (TcData.ListToothGraphics[t].ToothID == "implant")
				{
					// This is not an actual tooth.
					continue;
				}

				DrawFacialView(TcData.ListToothGraphics[t], g);
				DrawOcclusalView(TcData.ListToothGraphics[t], g);
			}

			DrawWatches(g);
			DrawNumbers(g);
			DrawDrawingSegments(g);

			e.Graphics.DrawImage(bitmap, 0, 0);

			g.Dispose();
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		private void DrawFacialView(ToothGraphic toothGraphic, Graphics g)
		{
			if (toothGraphic.DrawBigX)
			{
				float x = TcData.GetTransXpix(toothGraphic.ToothID);
				float y = TcData.GetTransYfacialPix(toothGraphic.ToothID);
				float halfw = 6f * TcData.PixelScaleRatio;
				float halfh = 29f * TcData.PixelScaleRatio;

				g.DrawLine(new Pen(toothGraphic.colorX, 2f * TcData.PixelScaleRatio), x - halfw, y - halfh, x + halfw, y + halfh);
				g.DrawLine(new Pen(toothGraphic.colorX, 2f * TcData.PixelScaleRatio), x + halfw, y - halfh, x - halfw, y + halfh);
			}
		}

		private void DrawOcclusalView(ToothGraphic toothGraphic, Graphics g)
		{
			if (toothGraphic.Visible // Might not be visible if an implant
				|| (toothGraphic.IsCrown && toothGraphic.IsImplant)// A crown on an implant will paint
																   // pontics won't paint, because tooth is invisible
																   // but, unlike the regular toothchart, we do want pontics to paint here
				|| toothGraphic.IsPontic)
			{
				DrawToothOcclusal(toothGraphic, g);
			}
		}

		private void DrawToothOcclusal(ToothGraphic toothGraphic, Graphics g)
		{
			ToothGroup group;
			float x, y;

			Pen outline = new Pen(Color.Gray);
			for (int i = 0; i < toothGraphic.Groups.Count; i++)
			{
				group = toothGraphic.Groups[i];
				if (!group.Visible)
				{
					continue;
				}

				x = TcData.GetTransXpix(toothGraphic.ToothID);
				y = TcData.GetTransYocclusalPix(toothGraphic.ToothID);
				float sqB = 4; // Half the size of the central square. B for Big.
				float cirB = 9.5f; // Radius of outer circle
				float sqS = 3; // S for small
				float cirS = 8f;
				GraphicsPath path;
				SolidBrush brush = new SolidBrush(group.PaintColor);
				string dir;
				switch (group.GroupType)
				{
					case ToothGroupType.O:
						g.FillRectangle(brush, x - sqB, y - sqB, 2f * sqB, 2f * sqB);
						g.DrawRectangle(outline, x - sqB, y - sqB, 2f * sqB, 2f * sqB);
						break;

					case ToothGroupType.I:
						g.FillRectangle(brush, x - sqS, y - sqS, 2f * sqS, 2f * sqS);
						g.DrawRectangle(outline, x - sqS, y - sqS, 2f * sqS, 2f * sqS);
						break;

					case ToothGroupType.B:
						if (ToothGraphic.IsMaxillary(toothGraphic.ToothID))
						{
							path = GetPath("U", x, y, sqB, cirB);
						}
						else
						{
							path = GetPath("D", x, y, sqB, cirB);
						}
						g.FillPath(brush, path);
						g.DrawPath(outline, path);
						break;

					case ToothGroupType.F:
						if (ToothGraphic.IsMaxillary(toothGraphic.ToothID))
						{
							path = GetPath("U", x, y, sqS, cirS);
						}
						else
						{
							path = GetPath("D", x, y, sqS, cirS);
						}
						g.FillPath(brush, path);
						g.DrawPath(outline, path);
						break;

					case ToothGroupType.L:
						if (ToothGraphic.IsMaxillary(toothGraphic.ToothID))
						{
							dir = "D";
						}
						else
						{
							dir = "U";
						}
						if (ToothGraphic.IsAnterior(toothGraphic.ToothID))
						{
							path = GetPath(dir, x, y, sqS, cirS);
						}
						else
						{
							path = GetPath(dir, x, y, sqB, cirB);
						}
						g.FillPath(brush, path);
						g.DrawPath(outline, path);
						break;

					case ToothGroupType.M:
						if (ToothGraphic.IsRight(toothGraphic.ToothID))
						{
							dir = "R";
						}
						else
						{
							dir = "L";
						}
						if (ToothGraphic.IsAnterior(toothGraphic.ToothID))
						{
							path = GetPath(dir, x, y, sqS, cirS);
						}
						else
						{
							path = GetPath(dir, x, y, sqB, cirB);
						}
						g.FillPath(brush, path);
						g.DrawPath(outline, path);
						break;

					case ToothGroupType.D:
						if (ToothGraphic.IsRight(toothGraphic.ToothID))
						{
							dir = "L";
						}
						else
						{
							dir = "R";
						}
						if (ToothGraphic.IsAnterior(toothGraphic.ToothID))
						{
							path = GetPath(dir, x, y, sqS, cirS);
						}
						else
						{
							path = GetPath(dir, x, y, sqB, cirB);
						}
						g.FillPath(brush, path);
						g.DrawPath(outline, path);
						break;
				}
			}
		}

		/// <summary>
		/// Gets a path for the pie shape that represents a tooth surface. 
		/// sq and cir refer to the radius of those two elements.
		/// </summary>
		private GraphicsPath GetPath(string UDLR, float x, float y, float sq, float cir)
		{
			GraphicsPath path = new GraphicsPath();
			float pt = cir * 0.7071f; // The x or y dist to the point where the circle is at 45 degrees.
			switch (UDLR)
			{
				case "U":
					path.AddLine(x - sq, y - sq, x + sq, y - sq);
					path.AddLine(x + sq, y - sq, x + pt, y - pt);
					path.AddArc(x - cir, y - cir, cir * 2f, cir * 2f, 360 - 45, -90);
					path.AddLine(x - pt, y - pt, x - sq, y - sq);
					break;
				case "D":
					path.AddLine(x + sq, y + sq, x - sq, y + sq);
					path.AddLine(x - sq, y + sq, x - pt, y + pt);
					path.AddArc(x - cir, y - cir, cir * 2f, cir * 2f, 90 + 45, -90);
					path.AddLine(x + pt, y + pt, x + sq, y + sq);
					break;
				case "L":
					path.AddLine(x - sq, y + sq, x - sq, y - sq);
					path.AddLine(x - sq, y - sq, x - pt, y - pt);
					path.AddArc(x - cir, y - cir, cir * 2f, cir * 2f, 180 + 45, -90);
					path.AddLine(x - pt, y + pt, x - sq, y + sq);
					break;
				case "R":
					path.AddLine(x + sq, y - sq, x + sq, y + sq);
					path.AddLine(x + sq, y + sq, x + pt, y + pt);
					path.AddArc(x - cir, y - cir, cir * 2f, cir * 2f, 45, -90);
					path.AddLine(x + pt, y - pt, x + sq, y - sq);
					break;
			}
			return path;
		}

		private void DrawWatches(Graphics g)
		{
			Hashtable watchTeeth = new Hashtable(TcData.ListToothGraphics.Count);

			// Loop through each adult tooth
			for (int t = 0; t < TcData.ListToothGraphics.Count; t++)
			{
				ToothGraphic toothGraphic = TcData.ListToothGraphics[t];

				// If a tooth is marked to be watched then it is always visible, even if the tooth is missing/hidden.
				if (toothGraphic.ToothID == "implant" || !toothGraphic.Watch || Tooth.IsPrimary(toothGraphic.ToothID))
				{
					continue;
				}

				watchTeeth[toothGraphic.ToothID] = toothGraphic;
			}

			// Loop through each primary tooth
			for (int t = 0; t < TcData.ListToothGraphics.Count; t++)
			{
				var toothGraphic = TcData.ListToothGraphics[t];
				
				// If a tooth is marked to be watched then it is always visible, even if the tooth is missing/hidden.
				if (toothGraphic.ToothID == "implant" || !toothGraphic.Watch || !Tooth.IsPrimary(toothGraphic.ToothID) || !toothGraphic.Visible)
				{
					continue;
				}

				watchTeeth[Tooth.PriToPerm(toothGraphic.ToothID)] = toothGraphic;
			}

			foreach (DictionaryEntry toothGraphic in watchTeeth)
			{
				RenderToothWatch(g, (ToothGraphic)toothGraphic.Value);
			}
		}

		private void RenderToothWatch(Graphics g, ToothGraphic toothGraphic)
		{
            SolidBrush brush = new SolidBrush(toothGraphic.colorWatch);
			
			// Drawing a white silhouette around the colored watch W doesn't make sense here because unerupted teeth do not change color in this chart.
			if (ToothGraphic.IsRight(toothGraphic.ToothID))
			{
				if (ToothGraphic.IsMaxillary(toothGraphic.ToothID))
				{
					g.DrawString("W", Font, brush, new PointF(TcData.GetTransXpix(toothGraphic.ToothID) + toothGraphic.ShiftM - 6f, 0));
				}
				else
				{
					g.DrawString("W", Font, brush, new PointF(TcData.GetTransXpix(toothGraphic.ToothID) + toothGraphic.ShiftM - 7f, Height - Font.Size - 8f));
				}
			}
			else
			{
				if (ToothGraphic.IsMaxillary(toothGraphic.ToothID))
				{
					g.DrawString("W", Font, brush, new PointF(TcData.GetTransXpix(toothGraphic.ToothID) - toothGraphic.ShiftM - 6f, 0));
				}
				else
				{
					g.DrawString("W", Font, brush, new PointF(TcData.GetTransXpix(toothGraphic.ToothID) - toothGraphic.ShiftM - 7f, Height - Font.Size - 8f));
				}
			}
			brush.Dispose();
		}

		private void DrawNumbers(Graphics g)
		{
			if (DesignMode)
			{
				return;
			}

			string tooth_id;
			for (int i = 1; i <= 52; i++)
			{
				tooth_id = Tooth.FromOrdinal(i);
				if (TcData.SelectedTeeth.Contains(tooth_id))
				{
					DrawNumber(tooth_id, true, true, g);
				}
				else
				{
					DrawNumber(tooth_id, false, true, g);
				}
			}
		}

		/// <summary>
		/// Draws the number and the rectangle behind it. 
		/// Draws in the appropriate color
		/// </summary>
		private void DrawNumber(string tooth_id, bool isSelected, bool isFullRedraw, Graphics g)
		{
			if (DesignMode || TcData == null) return;
			

			if (!Tooth.IsValidDB(tooth_id)) return;
			

			if (TcData.ListToothGraphics[tooth_id] == null)
			{
				return; // For some reason, it's still getting to here in DesignMode
			}

			if (isFullRedraw)
			{
				if (TcData.ListToothGraphics[tooth_id].HideNumber)
				{
					return;
				}

				if (Tooth.IsPrimary(tooth_id) && !TcData.ListToothGraphics[Tooth.PriToPerm(tooth_id)].ShowPrimaryLetter)//but not set to show primary letters
				{
					return;
				}
			}

			string displayNum = Tooth.GetToothLabelGraphic(tooth_id, TcData.ToothNumberingNomenclature);
            float labelWidthMm = g.MeasureString(displayNum, Font).Width / TcData.ScaleMmToPix;
            SizeF labelSizeF = new SizeF(labelWidthMm, Font.Height / TcData.ScaleMmToPix);
			Rectangle rec = TcData.GetNumberRecPix(tooth_id, labelSizeF);

			if (isSelected)
			{
				g.FillRectangle(new SolidBrush(TcData.ColorBackHighlight), rec);
			}
			else
			{
				g.FillRectangle(new SolidBrush(TcData.ColorBackground), rec);
			}

			if (TcData.ListToothGraphics[tooth_id].HideNumber)
			{
				// If number is hidden do not print string
			}
			else if (Tooth.IsPrimary(tooth_id) && !TcData.ListToothGraphics[Tooth.PriToPerm(tooth_id)].ShowPrimaryLetter)
			{
				// Do not print string
			}
			else if (isSelected)
			{
				g.DrawString(displayNum, Font, new SolidBrush(TcData.ColorTextHighlight), rec.X, rec.Y);
			}
			else
			{
				g.DrawString(displayNum, Font, new SolidBrush(TcData.ColorText), rec.X, rec.Y);
			}
		}

		private void DrawDrawingSegments(Graphics g)
		{
			string[] pointStr;
			List<PointF> points;
			PointF pointf;
			string[] xy;
			float x;
			float y;
			Pen pen;

			for (int s = 0; s < TcData.DrawingSegmentList.Count; s++)
			{
				pen = new Pen(TcData.DrawingSegmentList[s].ColorDraw, 2.2f * TcData.PixelScaleRatio);
				pointStr = TcData.DrawingSegmentList[s].DrawingSegment.Split(';');
				points = new List<PointF>();
				for (int p = 0; p < pointStr.Length; p++)
				{
					xy = pointStr[p].Split(',');
					if (xy.Length == 2)
					{
						x = TcData.RectTarget.X + float.Parse(xy[0]) * TcData.PixelScaleRatio;
						y = TcData.RectTarget.Y + float.Parse(xy[1]) * TcData.PixelScaleRatio;
						pointf = new PointF(x, y);
						points.Add(pointf);
					}
				}
				if (points.Count < 2)
				{
					// Can't draw a line with less than 2 points.
					continue;
				}
				g.DrawLines(pen, points.ToArray());
			}
		}

		/// <summary>
		/// Returns a bitmap of what is showing in the control. Used for printing.
		/// </summary>
		public Bitmap GetBitmap()
		{
			Bitmap bmp = new Bitmap(this.Width, this.Height);
			Graphics gfx = Graphics.FromImage(bmp);
			PaintEventArgs e = new PaintEventArgs(gfx, new Rectangle(0, 0, bmp.Width, bmp.Height));
			OnPaint(e);
			return bmp;
		}

		#region Mouse And Selections

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			MouseIsDown = true;

			if (TcData.ListToothGraphics.Count == 0)
			{
				// Still starting up?
				return;
			}

			if (TcData.CursorTool == CursorTool.Pointer)
			{
				_listSelectedTeethOld = TcData.SelectedTeeth.FindAll(x => x != null);//Make a copy of the list.  No elements should ever be null (copy all).
				string toothClicked = TcData.GetToothAtPoint(e.Location);
				if (TcData.SelectedTeeth.Contains(toothClicked))
				{
					SetSelected(toothClicked, false);
				}
				else
				{
					SetSelected(toothClicked, true);
				}
			}
			else if (TcData.CursorTool == CursorTool.Pen)
			{
				TcData.PointList.Add(new PointF(e.X, e.Y));
			}
			else if (TcData.CursorTool == CursorTool.Eraser)
			{
				// Do nothing
			}
			else if (TcData.CursorTool == CursorTool.ColorChanger)
			{
				// Look for any lines near the "wand".
				// Since the line segments are so short, it's sufficient to check end points.
				string[] xy;
				string[] pointStr;
				float x;
				float y;
				float dist; // The distance between the point being tested and the center of the eraser circle.
				float radius = 2f; // By trial and error to achieve best feel.
				PointF pointMouseScaled = TcData.GetPointMouseScaled(e.X, e.Y, Size);
				for (int i = 0; i < TcData.DrawingSegmentList.Count; i++)
				{
					pointStr = TcData.DrawingSegmentList[i].DrawingSegment.Split(';');
					for (int p = 0; p < pointStr.Length; p++)
					{
						xy = pointStr[p].Split(',');
						if (xy.Length == 2)
						{
							x = float.Parse(xy[0]);
							y = float.Parse(xy[1]);
							dist = (float)Math.Sqrt(Math.Pow(Math.Abs(x - pointMouseScaled.X), 2) + Math.Pow(Math.Abs(y - pointMouseScaled.Y), 2));
							if (dist <= radius)
							{
								// Testing circle intersection here
								OnSegmentDrawn(TcData.DrawingSegmentList[i].DrawingSegment);
								TcData.DrawingSegmentList[i].ColorDraw = TcData.ColorDrawing;
								Invalidate();
								return; ;
							}
						}
					}
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (TcData.ListToothGraphics.Count == 0)
			{
				return;
			}
			if (TcData.CursorTool == CursorTool.Pointer)
			{
				hotTooth = TcData.GetToothAtPoint(e.Location);
				if (hotTooth == hotToothOld)
				{
					// Mouse has not moved to another tooth
					return;
				}

				hotToothOld = hotTooth;
				if (MouseIsDown)
				{
					// Drag action
					if (TcData.SelectedTeeth.Contains(hotTooth))
					{
						SetSelected(hotTooth, false);
					}
					else
					{
						SetSelected(hotTooth, true);
					}
				}
			}
			else if (TcData.CursorTool == CursorTool.Pen)
			{
				if (!MouseIsDown)
				{
					return;
				}
				TcData.PointList.Add(new PointF(e.X, e.Y));

				// Just add the last line segment instead of redrawing the whole thing.
				g.SmoothingMode = SmoothingMode.HighQuality;
				Pen pen = new Pen(TcData.ColorDrawing, 2.2f * TcData.PixelScaleRatio);
				int i = TcData.PointList.Count - 1;

				g.DrawLine(pen, TcData.PointList[i - 1].X, TcData.PointList[i - 1].Y, TcData.PointList[i].X, TcData.PointList[i].Y);
			}
			else if (TcData.CursorTool == CursorTool.Eraser)
			{
				if (!MouseIsDown)
				{
					return;
				}

				// Look for any lines that intersect the "eraser".
				// Since the line segments are so short, it's sufficient to check end points.
				string[] xy;
				string[] pointStr;
				float x;
				float y;
				float dist; // The distance between the point being tested and the center of the eraser circle.
				float radius = 8f; // By trial and error to achieve best feel.
				PointF eraserPt = TcData.GetPointMouseScaled(e.X + 8.49f, e.Y + 8.49f, Size);
				for (int i = 0; i < TcData.DrawingSegmentList.Count; i++)
				{
					pointStr = TcData.DrawingSegmentList[i].DrawingSegment.Split(';');
					for (int p = 0; p < pointStr.Length; p++)
					{
						xy = pointStr[p].Split(',');
						if (xy.Length == 2)
						{
							x = float.Parse(xy[0]);
							y = float.Parse(xy[1]);
							dist = (float)Math.Sqrt(Math.Pow(Math.Abs(x - eraserPt.X), 2) + Math.Pow(Math.Abs(y - eraserPt.Y), 2));
							if (dist <= radius)
							{
								// Testing circle intersection here
								OnSegmentDrawn(TcData.DrawingSegmentList[i].DrawingSegment); // Triggers a deletion from db.
								TcData.DrawingSegmentList.RemoveAt(i);
								Invalidate();
								return; ;
							}
						}
					}
				}
			}
			else if (TcData.CursorTool == CursorTool.ColorChanger)
			{
				// Do nothing
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			MouseIsDown = false;
			if (TcData.CursorTool == CursorTool.Pointer)
			{
				if (TcData.HasSelectedTeethChanged(_listSelectedTeethOld))
				{
					OnToothSelectionsChanged();
				}
			}
			else if (TcData.CursorTool == CursorTool.Pen)
			{
				string drawingSegment = "";
				for (int i = 0; i < TcData.PointList.Count; i++)
				{
					if (i > 0)
					{
						drawingSegment += ";";
					}
					PointF pointMouseScaled = TcData.GetPointMouseScaled(TcData.PointList[i].X, TcData.PointList[i].Y, Size);

					// I could compensate to center point here:
					drawingSegment += pointMouseScaled.X + "," + pointMouseScaled.Y;
				}

				OnSegmentDrawn(drawingSegment);

				TcData.PointList = new List<PointF>();
			}
			else if (TcData.CursorTool == CursorTool.Eraser)
			{
				// Do nothing
			}
			else if (TcData.CursorTool == CursorTool.ColorChanger)
			{
				// Do nothing
			}
		}

		protected void OnSegmentDrawn(string drawingSegment) 
			=> SegmentDrawn?.Invoke(this, new ToothChartDrawEventArgs(drawingSegment));
        
		protected void OnToothSelectionsChanged() 
			=> ToothSelectionsChanged?.Invoke(this);

		/// <summary>
		/// Used by mousedown and mouse move to set teeth selected or unselected.
		/// A similar method is used externally in the wrapper to set teeth selected.
		/// This private method might be faster since it only draws the changes.
		/// </summary>
		private void SetSelected(string tooth_id, bool setValue)
		{
			if (setValue)
			{
				TcData.SelectedTeeth.Add(tooth_id);
				DrawNumber(tooth_id, true, false, g);
			}
			else
			{
				TcData.SelectedTeeth.Remove(tooth_id);
				DrawNumber(tooth_id, false, false, g);
			}

			Invalidate();

			Application.DoEvents();
		}

		#endregion Mouse And Selections
	}
}
