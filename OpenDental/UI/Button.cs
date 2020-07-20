using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace OpenDental.UI
{
    public class Button : System.Windows.Forms.Button
	{
		private static readonly Color colorBorder = Color.FromArgb(0, 70, 140);
		private static readonly Color colorBorderHighlight = Color.FromArgb(0, 110, 190);
		private static readonly Color colorBorderDisabled = Color.FromArgb(150, 150, 150);
		private static readonly Color colorTop = Color.FromArgb(255, 255, 255);
		private static readonly Color colorBottom = Color.FromArgb(200, 200, 200);
		private static readonly Color colorBottomDisabled = Color.FromArgb(230, 230, 230);
		private static readonly Color colorPressedTop = Color.FromArgb(255, 255, 255);
		private static readonly Color colorPressedBottom = Color.FromArgb(180, 180, 180);
		private static readonly Color colorText = Color.FromArgb(0, 0, 0);
		private static readonly Color colorTextGlow = Color.FromArgb(255, 255, 255);
		private static readonly Color colorTextDisabled = Color.FromArgb(120, 120, 120);

		/// <summary>
		/// Identifies the status of the button.
		/// </summary>
		private enum ButtonState
		{
			Normal,
			Hover,

			/// <summary>
			/// Mouse down. Not a permanent toggle state.
			/// </summary>
			Pressed
		}

		private ButtonState state = ButtonState.Normal;
		private bool canClick = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="Button"/> class.
		/// </summary>
		public Button()
		{
			DoubleBuffered = true;
		}

		/// <summary>
		/// Gets the default size of the button.
		/// </summary>
		protected override Size DefaultSize => new Size(80, 25);

		private StringFormat GetStringFormat(ContentAlignment contentAlignment)
		{
			var stringFormat = new StringFormat();
			switch (contentAlignment)
			{
				case ContentAlignment.MiddleCenter:
					stringFormat.LineAlignment = StringAlignment.Center;
					stringFormat.Alignment = StringAlignment.Center;
					break;

				case ContentAlignment.MiddleLeft:
					stringFormat.LineAlignment = StringAlignment.Center;
					stringFormat.Alignment = StringAlignment.Near;
					break;

				case ContentAlignment.MiddleRight:
					stringFormat.LineAlignment = StringAlignment.Center;
					stringFormat.Alignment = StringAlignment.Far;
					break;

				case ContentAlignment.TopCenter:
					stringFormat.LineAlignment = StringAlignment.Near;
					stringFormat.Alignment = StringAlignment.Center;
					break;

				case ContentAlignment.TopLeft:
					stringFormat.LineAlignment = StringAlignment.Near;
					stringFormat.Alignment = StringAlignment.Near;
					break;

				case ContentAlignment.TopRight:
					stringFormat.LineAlignment = StringAlignment.Near;
					stringFormat.Alignment = StringAlignment.Far;
					break;

				case ContentAlignment.BottomCenter:
					stringFormat.LineAlignment = StringAlignment.Far;
					stringFormat.Alignment = StringAlignment.Center;
					break;

				case ContentAlignment.BottomLeft:
					stringFormat.LineAlignment = StringAlignment.Far;
					stringFormat.Alignment = StringAlignment.Near;
					break;

				case ContentAlignment.BottomRight:
					stringFormat.LineAlignment = StringAlignment.Far;
					stringFormat.Alignment = StringAlignment.Far;
					break;
			}

			stringFormat.HotkeyPrefix = HotkeyPrefix.Show;

			return stringFormat;
		}


		protected override void OnClick(EventArgs ea)
		{
			Capture = false;

			canClick = false;

			var newState = ButtonState.Normal;
			if (ClientRectangle.Contains(PointToClient(MousePosition)))
			{
				newState = ButtonState.Hover;
			}

			if (newState != state)
			{
				state = newState;

				Invalidate();
			}

			base.OnClick(ea);
		}

		protected override void OnMouseEnter(EventArgs ea)
		{
			base.OnMouseEnter(ea);

			state = ButtonState.Hover;

			Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs mea)
		{
			base.OnMouseDown(mea);

			if (mea.Button == MouseButtons.Left)
			{
				canClick = true;
				state = ButtonState.Pressed;

				Invalidate();
			}
		}

		protected override void OnMouseMove(MouseEventArgs mea)
		{
			base.OnMouseMove(mea);

			if (ClientRectangle.Contains(mea.X, mea.Y))
			{
				if (state == ButtonState.Hover && Capture && !canClick)
				{
					canClick = true;

					state = ButtonState.Pressed;

					Invalidate();
				}
			}
			else
			{
				if (state == ButtonState.Pressed)
				{
					canClick = false;

					state = ButtonState.Hover;

					Invalidate();
				}
			}
		}

		protected override void OnMouseLeave(EventArgs ea)
		{
			base.OnMouseLeave(ea);

			state = ButtonState.Normal;

			Invalidate();
		}

		protected override void OnEnabledChanged(EventArgs ea)
		{
			base.OnEnabledChanged(ea);

			state = ButtonState.Normal;

			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs p)
		{
			OnPaintBackground(p);

			var g = p.Graphics;

			g.SmoothingMode = SmoothingMode.HighQuality;

			var outline = new RectangleF(0, 0, Width - 1.2f, Height - 1.2f);

			using (var path = OpenDentBusiness.GraphicsHelper.GetRoundedPath(outline, 3))
			{
				using var pen = new Pen(Enabled ? colorBorder : colorBorderDisabled);

				switch (state)
				{
					case ButtonState.Normal:
						using (var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, Height), colorTop, Enabled ? colorBottom : colorBottomDisabled))
						{
							g.FillPath(brush, path);
						}
						g.DrawPath(pen, path);
						break;

					case ButtonState.Hover:
						using (var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, Height), colorTop, colorBottom))
						{
							g.FillPath(brush, path);
							using (var highlightPen = new Pen(colorBorderHighlight, 1.7f))
                            {
								g.DrawPath(highlightPen, path);
                            }
						}
						g.DrawPath(pen, path);
						break;

					case ButtonState.Pressed:
						using (var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, Height), colorPressedTop, colorPressedBottom))
						{
							g.FillPath(brush, path);

                            using var highlightPen = new Pen(colorBorderHighlight, 1.7f);

                            g.DrawPath(highlightPen, path);
                        }
						g.DrawPath(pen, path);
						break;
				}
			}

			DrawTextAndImage(g);
		}

		private static Point GetImagePosition(ContentAlignment alignment, Image image, Rectangle bounds, Padding padding)
        {
			switch (alignment)
			{
				case ContentAlignment.TopLeft: return new Point(padding.Left, padding.Top);
				case ContentAlignment.TopCenter: return new Point(bounds.Width / 2 - image.Width / 2, padding.Top);
				case ContentAlignment.TopRight: return new Point(bounds.Width - image.Width - padding.Right, padding.Top);
				case ContentAlignment.MiddleLeft: return new Point(padding.Left, bounds.Height / 2 - image.Height / 2);
				case ContentAlignment.MiddleRight: return new Point(bounds.Width - image.Width - padding.Right, bounds.Height / 2 - image.Height / 2);
				case ContentAlignment.BottomLeft: return new Point(padding.Left, bounds.Height - image.Height - padding.Bottom);
				case ContentAlignment.BottomCenter: return new Point(bounds.Width / 2 - image.Width / 2, bounds.Height - image.Height - padding.Bottom);
				case ContentAlignment.BottomRight: return new Point(bounds.Width - image.Width - padding.Right, bounds.Height - image.Height - padding.Bottom);
			}

			// Default to center the image..
			return new Point(
				bounds.Width / 2 - image.Width / 2, 
				bounds.Height / 2 - bounds.Height / 2);
		}

		private static Rectangle GetTextRectangleRelativeToImage(Rectangle rect, Image image, ContentAlignment imageAlignment)
        {
			if (imageAlignment == ContentAlignment.TopLeft ||
				imageAlignment == ContentAlignment.TopCenter ||
				imageAlignment == ContentAlignment.TopRight)
				return Rectangle.FromLTRB(rect.Left, rect.Top - image.Height, rect.Right, rect.Bottom);

			if (imageAlignment == ContentAlignment.MiddleLeft)
				return Rectangle.FromLTRB(rect.Left + image.Width, rect.Top, rect.Right, rect.Bottom);

			if (imageAlignment == ContentAlignment.MiddleRight)
				return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right - image.Width, rect.Bottom);

			if (imageAlignment == ContentAlignment.BottomLeft ||
				imageAlignment == ContentAlignment.BottomCenter ||
				imageAlignment == ContentAlignment.BottomRight)
				return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom - image.Height);

			return rect;
		}

		private void DrawTextAndImage(Graphics g)
		{
			using var stringFormat = GetStringFormat(TextAlign);

			if (Image == null)
			{
				using (var textBrush = new SolidBrush(Enabled ? colorText : colorTextDisabled))
				{
					if (Enabled)
					{
                        using var textGlowBrush = new SolidBrush(colorTextGlow);

                        var textGlowRectangle = new RectangleF(
                            ClientRectangle.X + .5f, ClientRectangle.Y + .5f,
                            ClientRectangle.Width,
                            ClientRectangle.Height);

                        g.DrawString(Text, Font, textGlowBrush, textGlowRectangle, stringFormat);
                    }

					g.DrawString(Text, Font, textBrush, ClientRectangle, stringFormat);

				}

				return;
			}

			// Determine the location for the image and draw it...
			var imagePos = GetImagePosition(ImageAlign, Image, ClientRectangle, Padding);
			if (Enabled)
            {
				g.DrawImage(Image, imagePos);
            }
            else
            {
				ControlPaint.DrawImageDisabled(g, Image, imagePos.X, imagePos.Y, SystemColors.Control);
            }

			// Only draw the text if the image isn't centered...
			if (ImageAlign != ContentAlignment.MiddleCenter)
			{
				var textRectangle = GetTextRectangleRelativeToImage(ClientRectangle, Image, ImageAlign);

				using (var textBrush = new SolidBrush(Enabled ? colorText : colorTextDisabled))
				{
					if (Enabled)
					{
						using var textGlowBrush = new SolidBrush(colorTextGlow);

						var textGlowRectangle = new RectangleF(
							textRectangle.X + .5f, textRectangle.Y + .5f,
							textRectangle.Width, textRectangle.Height);

						g.DrawString(Text, Font, textGlowBrush, textGlowRectangle, stringFormat);
					}

					g.DrawString(Text, Font, textBrush, textRectangle, stringFormat);
				}
			}
		}

		/// <summary>
		/// Used by ODButtonPanel.
		/// Let Button.cs do this drawing because this is where the colors are stored.
		/// No image.
		/// Text in middle center.
		/// No hover effect.
		/// </summary>
		public static void DrawSimpleButton(Graphics g, Rectangle bounds, string text, Font font)
		{
			var outline = new RectangleF(bounds.X, bounds.Y, bounds.Width - 1.2f, bounds.Height - 1.2f);

			using (var path = OpenDentBusiness.GraphicsHelper.GetRoundedPath(outline, 3))
			using (var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, 18), colorTop, colorBottom))
			{
				g.FillPath(brush, path);

				using (var pen = new Pen(colorBorder))
				{
					g.DrawPath(pen, path);
				}
			}

			using (var stringFormat = new StringFormat())
			{
				stringFormat.Alignment = StringAlignment.Center;
				stringFormat.LineAlignment = StringAlignment.Center;

				var rectangleTextGlow = new RectangleF(
					bounds.X + .5f, bounds.Y + .5f, 
					bounds.Width, 
					bounds.Height);

				using (var brushTextGlow = new SolidBrush(colorTextGlow))
				{
					g.DrawString(text, font, brushTextGlow, rectangleTextGlow, stringFormat);
				}

				using (var brushText = new SolidBrush(colorText))
				{
					g.DrawString(text, font, brushText, bounds, stringFormat);
				}
			}
		}
	}
}
