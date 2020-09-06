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
		private static readonly Color colorBorderHighlight = Color.FromArgb(160, 0, 110, 190);
		private static readonly Color colorBorderDisabled = Color.FromArgb(150, 150, 150);
		private static readonly Color colorTop = Color.FromArgb(255, 255, 255);
		private static readonly Color colorBottom = Color.FromArgb(200, 200, 200);
		private static readonly Color colorBottomDisabled = Color.FromArgb(230, 230, 230);
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
		/// Gets the default padding of the button.
		/// </summary>
		protected override Padding DefaultPadding => new Padding(3);

        /// <summary>
        /// Gets the default size of the button.
        /// </summary>
        protected override Size DefaultSize => new Size(80, 25);

		private TextFormatFlags GetTextFormatFlags(ContentAlignment contentAlignment)
		{
            return contentAlignment switch
            {
                ContentAlignment.MiddleLeft => TextFormatFlags.VerticalCenter | TextFormatFlags.Left,
                ContentAlignment.MiddleRight => TextFormatFlags.VerticalCenter | TextFormatFlags.Right,
                ContentAlignment.TopCenter => TextFormatFlags.Top | TextFormatFlags.HorizontalCenter,
                ContentAlignment.TopLeft => TextFormatFlags.Top | TextFormatFlags.Left,
                ContentAlignment.TopRight => TextFormatFlags.Top | TextFormatFlags.Right,
                ContentAlignment.BottomCenter => TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter,
                ContentAlignment.BottomLeft => TextFormatFlags.Bottom | TextFormatFlags.Left,
                ContentAlignment.BottomRight => TextFormatFlags.Bottom | TextFormatFlags.Right,
                _ => TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
            };
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
							var clip = g.Clip;

							g.FillPath(brush, path);
							g.Clip = new Region(path);

                            using var highlightPen = new Pen(colorBorderHighlight, 3.5f);

                            g.DrawPath(highlightPen, path);
							g.Clip = clip;
                        }
						g.DrawPath(pen, path);
						break;

					case ButtonState.Pressed:
						using (var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, Height), colorBottom, colorTop))
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
            return alignment switch
            {
                ContentAlignment.TopLeft => new Point(padding.Left, padding.Top),
                ContentAlignment.TopCenter => new Point(bounds.Width / 2 - image.Width / 2, padding.Top),
                ContentAlignment.TopRight => new Point(bounds.Width - image.Width - padding.Right, padding.Top),
                ContentAlignment.MiddleLeft => new Point(padding.Left, bounds.Height / 2 - image.Height / 2),
                ContentAlignment.MiddleRight => new Point(bounds.Width - image.Width - padding.Right, bounds.Height / 2 - image.Height / 2),
                ContentAlignment.BottomLeft => new Point(padding.Left, bounds.Height - image.Height - padding.Bottom),
                ContentAlignment.BottomCenter => new Point(bounds.Width / 2 - image.Width / 2, bounds.Height - image.Height - padding.Bottom),
                ContentAlignment.BottomRight => new Point(bounds.Width - image.Width - padding.Right, bounds.Height - image.Height - padding.Bottom),

                // Default to center the image..
                _ => new Point(bounds.Width / 2 - image.Width / 2, bounds.Height / 2 - image.Height / 2),
            };
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
			var textFormatFlags = GetTextFormatFlags(TextAlign);

			if (Image == null)
			{
                if (Enabled)
                {
                    var textGlowRectangle = new Rectangle(
						ClientRectangle.X + 1, ClientRectangle.Y + 1,
						ClientRectangle.Width,
						ClientRectangle.Height);

					TextRenderer.DrawText(g, Text, Font, textGlowRectangle, colorTextGlow, textFormatFlags);
				}

				TextRenderer.DrawText(g, Text, Font, ClientRectangle, Enabled ? colorText : colorTextDisabled, textFormatFlags);

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

                if (Enabled)
                {
                    var textGlowRectangle = new Rectangle(
                        textRectangle.X + 1, textRectangle.Y + 1,
                        textRectangle.Width, textRectangle.Height);

					TextRenderer.DrawText(g, Text, Font, textGlowRectangle, colorTextGlow, textFormatFlags);
				}

				TextRenderer.DrawText(g, Text, Font, textRectangle, Enabled ? colorText : colorTextDisabled, textFormatFlags);
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

                using var pen = new Pen(colorBorder);

                g.DrawPath(pen, path);
            }

            using var stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var rectangleTextGlow = new RectangleF(
                bounds.X + .5f, bounds.Y + .5f,
                bounds.Width,
                bounds.Height);

			using var brushTextGlow = new SolidBrush(colorTextGlow);
			using var brushText = new SolidBrush(colorText);

            g.DrawString(text, font, brushTextGlow, rectangleTextGlow, stringFormat);
            g.DrawString(text, font, brushText, bounds, stringFormat);
        }
	}
}
