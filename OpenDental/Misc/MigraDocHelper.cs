using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using GdiColor = System.Drawing.Color;
using GdiFont = System.Drawing.Font;
using MigraDocColor = MigraDoc.DocumentObjectModel.Color;
using MigraDocFont = MigraDoc.DocumentObjectModel.Font;
using PdfFont = PdfSharp.Drawing.XFont;

namespace OpenDental
{
    /// <summary>
    /// Used to add functionality to the MigraDoc framework.
    /// Specifically, it helps with absolute positioning.
    /// </summary>
    public class MigraDocHelper
	{
		/// <summary>
		/// Creates a container frame in a section.
		/// This allows other objects to be placed absolutely within a given area by adding more textframes within this one.
		/// This frame is full width and is automatically placed after previous elements.
		/// </summary>
		public static TextFrame CreateContainer(Section section)
		{
			TextFrame framebig = section.AddTextFrame();
			framebig.RelativeVertical = RelativeVertical.Line;
			framebig.RelativeHorizontal = RelativeHorizontal.Page;
			framebig.MarginLeft = Unit.Zero;
			framebig.MarginTop = Unit.Zero;
			framebig.Top = TopPosition.Parse("0 in");
			framebig.Left = LeftPosition.Parse("0 in");

			if (CultureInfo.CurrentCulture.Name == "en-US")
			{
				framebig.Width = Unit.FromInch(8.5);
			}
			else
			{
				framebig.Width = Unit.FromInch(8.3);
			}

			return framebig;
		}

		/// <summary>
		/// Creates a somewhat smaller container at an absolute position on the page.
		/// </summary>
		public static TextFrame CreateContainer(Section section, float x, float y, float width, float height)
		{
			TextFrame framebig = section.AddTextFrame();
			framebig.RelativeVertical = RelativeVertical.Page;
			framebig.RelativeHorizontal = RelativeHorizontal.Page;
			framebig.MarginLeft = Unit.Zero;
			framebig.MarginTop = Unit.Zero;
			framebig.Top = TopPosition.Parse((y / 100).ToString() + " in");
			framebig.Left = LeftPosition.Parse((x / 100).ToString() + " in");
			framebig.Width = Unit.FromInch(width / 100);
			framebig.Height = Unit.FromInch(height / 100);
			return framebig;
		}

		/// <summary>
		/// In 100ths of an inch
		/// </summary>
		public static int GetDocWidth()
		{
			if (CultureInfo.CurrentCulture.Name == "en-US")
			{
				return 850;
			}
			else
			{
				return 830;
			}
		}

		public static void DrawString(TextFrame container, string text, MigraDocFont font, RectangleF rect, ParagraphAlignment alignment = ParagraphAlignment.Left)
		{
			var bottom = Unit.FromInch(rect.Bottom / 100f);
			if (container.Height < bottom)
			{
				container.Height = bottom;
			}

			var textFrame = new TextFrame
			{
				RelativeVertical = RelativeVertical.Page,
				RelativeHorizontal = RelativeHorizontal.Page,
				MarginLeft = Unit.FromInch(rect.Left / 100),
				MarginTop = Unit.FromInch(rect.Top / 100),
				Top = TopPosition.Parse("0 in"),
				Left = LeftPosition.Parse("0 in"),
				Width = Unit.FromInch(rect.Right / 100f),
				Height = container.Height
			};
			
			var par = textFrame.AddParagraph();
			par.Format.Font = font.Clone();
			par.Format.Alignment = alignment;
			par.AddText(text);

			container.Elements.Add(textFrame);
		}

		public static void DrawString(TextFrame container, string text, MigraDocFont font, float x, float y)
		{
			var fontInfo = new GdiFont(
				font.Name, (float)font.Size.Point, 
				font.Bold ? FontStyle.Bold : FontStyle.Regular);

			var bottom = Unit.FromInch((y + fontInfo.Height) / 100);
			if (container.Height < bottom)
			{
				container.Height = bottom;
			}

			var textFrame = new TextFrame
            {
                RelativeVertical = RelativeVertical.Page,
                RelativeHorizontal = RelativeHorizontal.Page,
                MarginLeft = Unit.FromInch(x / 100),
                MarginTop = Unit.FromInch(y / 100),
                Top = TopPosition.Parse("0 in"),
                Left = LeftPosition.Parse("0 in"),
                Width = container.Width,
				Height = container.Height
			};

            var paragraph = textFrame.AddParagraph();
			paragraph.Format.Font = font.Clone();
			paragraph.AddText(text);

			container.Elements.Add(textFrame);
		}

		public static void DrawBitmap(TextFrame container, Bitmap bitmap, float x, float y)
		{
			var bottom = Unit.FromInch((y + (double)bitmap.Height) / 100);
			if (container.Height < bottom)
			{
				container.Height = bottom;
			}

            var textFrame = new TextFrame
            {
                RelativeVertical = RelativeVertical.Page,
                RelativeHorizontal = RelativeHorizontal.Page,
                MarginLeft = Unit.FromInch(x / 100),
                MarginTop = Unit.FromInch(y / 100),
                Top = TopPosition.Parse("0 in"),
                Left = LeftPosition.Parse("0 in"),
                Width = container.Width,
                Height = container.Height
            };

            string tempImageFileName = Storage.GetTempFileName(".tmp");
			bitmap.SetResolution(100, 100);
			bitmap.Save(tempImageFileName);
			textFrame.AddImage(tempImageFileName);

			container.Elements.Add(textFrame);
		}

		public static MigraDocFont CreateFont(float fontSize) 
			=> new MigraDocFont
			{
                Name = "Arial",
                Size = Unit.FromPoint(fontSize)
            };

		public static MigraDocFont CreateFont(float fontSize, bool bold) 
			=> new MigraDocFont
			{
                Name = "Arial",
                Size = Unit.FromPoint(fontSize),
                Bold = bold
            };

		public static MigraDocFont CreateFont(float fontSize, bool bold, GdiColor color) 
			=> new MigraDocFont
			{
                Name = "Arial",
                Size = Unit.FromPoint(fontSize),
                Bold = bold,
                Color = ConvertColor(color)
			};

		public static MigraDocColor ConvertColor(GdiColor color) => 
			new MigraDocColor(
				color.A, 
				color.R, 
				color.G, 
				color.B);

		public static void FillRectangle(TextFrame container, GdiColor color, float x, float y, float width, float height)
		{
			var bottom = Unit.FromInch((y + height) / 100);
			if (container.Height < bottom)
			{
				container.Height = bottom;
			}

			var textFrame = new TextFrame
            {
                RelativeVertical = RelativeVertical.Page,
                RelativeHorizontal = RelativeHorizontal.Page,
                MarginLeft = Unit.FromInch(x / 100),
                MarginTop = Unit.FromInch(y / 100),
                Top = TopPosition.Parse("0 in"),
                Left = LeftPosition.Parse("0 in"),
                Width = container.Width,
				Height = container.Height
			};

			var textFrameRect = new TextFrame
			{
				Height = Unit.FromInch(height / 100),
				Width = Unit.FromInch(width / 100)
			};

			textFrameRect.FillFormat.Visible = true;
			textFrameRect.FillFormat.Color = ConvertColor(color);
			textFrame.Elements.Add(textFrameRect);

			container.Elements.Add(textFrame);
		}

		public static void DrawRectangle(TextFrame container, GdiColor color, float x, float y, float width, float height)
		{
			var bottom = Unit.FromInch((y + height) / 100);
			if (container.Height < bottom)
			{
				container.Height = bottom;
			}

			var textFrame = new TextFrame
            {
                RelativeVertical = RelativeVertical.Page,
                RelativeHorizontal = RelativeHorizontal.Page,
                MarginLeft = Unit.FromInch(x / 100),
                MarginTop = Unit.FromInch(y / 100),
                Top = TopPosition.Parse("0 in"),
                Left = LeftPosition.Parse("0 in"),
                Width = container.Width,
                Height = container.Height
            };

			var textFrameRect = new TextFrame
			{
				Height = Unit.FromInch(height / 100),
				Width = Unit.FromInch(width / 100)
			};

			textFrameRect.LineFormat.Color = ConvertColor(color);
			textFrame.Elements.Add(textFrameRect);

			container.Elements.Add(textFrame);
		}

		/// <summary>
		/// Only supports horizontal and vertical lines. Assumes single width, no dashes.
		/// </summary>
		public static void DrawLine(TextFrame frameContainer, GdiColor color, float x1, float y1, float x2, float y2)
		{
            MigraDocColor colorx = ConvertColor(color);
			var textFrameRect = new TextFrame();
			var textFrame = new TextFrame();

			textFrameRect.LineFormat.Color = colorx;
			if (x1 == x2) // Vertical
			{
				textFrameRect.Width = Unit.FromPoint(.01);
				textFrame.MarginLeft = Unit.FromInch(x1 / 100);

				if (y2 > y1) // Down
				{
					textFrameRect.Height = Unit.FromInch((y2 - y1) / 100);
					textFrame.MarginTop = Unit.FromInch(y1 / 100);
				}
				else // Up
				{
					textFrameRect.Height = Unit.FromInch((y1 - y2) / 100);
					textFrame.MarginTop = Unit.FromInch(y2 / 100);
				}
			}
			else if (y1 == y2) // Horizontal
			{
				textFrameRect.Height = Unit.FromPoint(.01);
				textFrame.MarginTop = Unit.FromInch(y1 / 100);

				if (x2 > x1) // Right
				{
					textFrameRect.Width = Unit.FromInch((x2 - x1) / 100);
					textFrame.MarginLeft = Unit.FromInch(x1 / 100);
				}
				else // Left
				{
					textFrameRect.Width = Unit.FromInch((x1 - x2) / 100);
					textFrame.MarginLeft = Unit.FromInch(x2 / 100);
				}
			}
			else return; // Diagonal lines not supported.

			var bottom = (y1 > y2) ? Unit.FromInch(y1 / 100) : Unit.FromInch(y2 / 100);
			if (frameContainer.Height < bottom)
			{
				frameContainer.Height = bottom;
			}

			textFrame.Elements.Add(textFrameRect);
			textFrame.RelativeVertical = RelativeVertical.Page;
			textFrame.RelativeHorizontal = RelativeHorizontal.Page;
			textFrame.Top = TopPosition.Parse("0 in");
			textFrame.Left = LeftPosition.Parse("0 in");
			textFrame.Width = frameContainer.Width;
			textFrame.Height = frameContainer.Height;

			frameContainer.Elements.Add(textFrame);
		}

		/// <summary>
		/// Draws a small simple table at a fixed location within a frame container. 
		/// Not intended for tables with variable number of rows.
		/// </summary>
		public static Table DrawTable(TextFrame container, float x, float y, float height)
		{
			var bottom = Unit.FromInch((y + height) / 100);
			if (container.Height < bottom)
			{
				container.Height = bottom;
			}

			var textFrame = new TextFrame
            {
                RelativeVertical = RelativeVertical.Page,
                RelativeHorizontal = RelativeHorizontal.Page,
                MarginLeft = Unit.FromInch(x / 100),
                MarginTop = Unit.FromInch(y / 100),
                Top = TopPosition.Parse("0 in"),
                Left = LeftPosition.Parse("0 in"),
                Width = container.Width,
				Height = container.Height
			};

			Table table = new Table();
			textFrame.Elements.Add(table);

			container.Elements.Add(textFrame);

			return table;
		}

		/// <summary>
		/// Vertical spacer.
		/// </summary>
		public static void InsertSpacer(Section section, int space) 
			=> section.AddTextFrame().Height = Unit.FromInch((double)space / 100);

		public static void DrawGrid(Section section, ODGrid grid)
		{
			// First, calculate width of dummy column that will push the grid over just enough to center it on the page.
			double pageW = 0;
			if (CultureInfo.CurrentCulture.Name == "en-US")
			{
				pageW = (section.PageSetup.Orientation == MigraDoc.DocumentObjectModel.Orientation.Landscape) ? 1100 : 850;
			}
			else
			{
				pageW = (section.PageSetup.Orientation == MigraDoc.DocumentObjectModel.Orientation.Landscape) ? 1080 : 830;
			}

			//in 100ths/inch
			double widthAllColumns = (double)grid.WidthAllColumns / .96;
			double lmargin = section.Document.DefaultPageSetup.LeftMargin.Inch * 100;
			double dummyColW = (pageW - widthAllColumns) / 2 - lmargin;
			Table table = new Table();
			Column col;
			col = table.AddColumn(Unit.FromInch(dummyColW / 100));
			for (int i = 0; i < grid.ListGridColumns.Count; i++)
			{
				col = table.AddColumn(Unit.FromInch((double)grid.ListGridColumns[i].ColWidth / 96));
				col.LeftPadding = Unit.FromInch(.01);
				col.RightPadding = Unit.FromInch(.01);
			}
			Row row;
			row = table.AddRow();
			row.HeadingFormat = true;
			row.TopPadding = Unit.FromInch(0);
			row.BottomPadding = Unit.FromInch(-.01);
			Cell cell;
			Paragraph par;
			//dummy column:
			cell = row.Cells[0];
			//cell.Shading.Color=Colors.LightGray;
			//format dummy cell?
			MigraDoc.DocumentObjectModel.Font fontHead = new MigraDoc.DocumentObjectModel.Font("Arial", Unit.FromPoint(8.5));
			fontHead.Bold = true;
			PdfDocument pdfd = new PdfDocument();
			PdfPage pg = pdfd.AddPage();
			XGraphics gx = XGraphics.FromPdfPage(pg);//A dummy graphics object for measuring the text
			for (int i = 0; i < grid.ListGridColumns.Count; i++)
			{
				cell = row.Cells[i + 1];
				par = cell.AddParagraph();
				par.AddFormattedText(grid.ListGridColumns[i].Heading, fontHead);
				par.Format.Alignment = ParagraphAlignment.Center;
				cell.Format.Alignment = ParagraphAlignment.Center;
				cell.Borders.Width = Unit.FromPoint(1);
				cell.Borders.Color = Colors.Black;
				cell.Shading.Color = Colors.LightGray;
			}
            MigraDocFont fontBody = null;//=new MigraDoc.DocumentObjectModel.Font("Arial",Unit.FromPoint(8.5));
			bool isBold;
            GdiColor color;
			int edgeRows = 1;
			for (int i = 0; i < grid.ListGridRows.Count; i++, edgeRows++)
			{
				row = table.AddRow();
				row.TopPadding = Unit.FromInch(.01);
				row.BottomPadding = Unit.FromInch(0);
				for (int j = 0; j < grid.ListGridColumns.Count; j++)
				{
					cell = row.Cells[j + 1];
					par = cell.AddParagraph();
					if (grid.ListGridRows[i].Cells[j].Bold == YN.Unknown)
					{
						isBold = grid.ListGridRows[i].Bold;
					}
					else if (grid.ListGridRows[i].Cells[j].Bold == YN.Yes)
					{
						isBold = true;
					}
					else
					{// if(grid.Rows[i].Cells[j].Bold==YN.No){
						isBold = false;
					}
					if (grid.ListGridRows[i].Cells[j].ColorText == System.Drawing.Color.Empty)
					{
						color = grid.ListGridRows[i].ColorText;
					}
					else
					{
						color = grid.ListGridRows[i].Cells[j].ColorText;
					}
					fontBody = CreateFont(8.5f, isBold, color);
					XFont xFont;
					if (isBold)
					{
						xFont = new XFont("Arial", 13.00);//Since we are using a dummy graphics object to measure the string, '13.00' is pretty much a guess-and-check
														  //value that looks about right.
					}
					else
					{
						xFont = new XFont("Arial", 11.65);//Yep, a guess-and-check value here too.
					}
					int colWidth = grid.ListGridColumns[j].ColWidth;
					string cellText = grid.ListGridRows[i].Cells[j].Text;
					List<string> listWords = cellText.Split(new[] { " ", "\t", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
						 .Where(x => !string.IsNullOrWhiteSpace(x)).ToList();//PdfSharp.MeasureString sometimes throws an exception when measuring whitespace
					bool isAnyWordWiderThanColumn = listWords.Any(x => gx.MeasureString(x, xFont).Width > colWidth);
					if (!isAnyWordWiderThanColumn)
					{
						//Let MigraDoc format line breaks
						par.AddFormattedText(cellText, fontBody);
					}
					else
					{
						//Do our own line splitting and word splitting
						DrawTextWithWordSplits(par, fontBody, colWidth, cellText, gx, xFont);
					}
					if (grid.ListGridColumns[j].TextAlign == HorizontalAlignment.Center)
					{
						cell.Format.Alignment = ParagraphAlignment.Center;
					}
					if (grid.ListGridColumns[j].TextAlign == HorizontalAlignment.Left)
					{
						cell.Format.Alignment = ParagraphAlignment.Left;
					}
					if (grid.ListGridColumns[j].TextAlign == HorizontalAlignment.Right)
					{
						cell.Format.Alignment = ParagraphAlignment.Right;
					}
					cell.Borders.Color = new MigraDoc.DocumentObjectModel.Color(180, 180, 180);
					if (grid.ListGridRows[i].ColorLborder != System.Drawing.Color.Empty)
					{
						cell.Borders.Bottom.Color = ConvertColor(grid.ListGridRows[i].ColorLborder);
					}
				}
				if (grid.ListGridRows[i].Note != null && grid.ListGridRows[i].Note != "" && grid.NoteSpanStop > 0 && grid.NoteSpanStart < grid.ListGridColumns.Count)
				{
					row = table.AddRow();
					row.TopPadding = Unit.FromInch(.01);
					row.BottomPadding = Unit.FromInch(0);
					cell = row.Cells[grid.NoteSpanStart + 1];
					par = cell.AddParagraph();
					par.AddFormattedText(grid.ListGridRows[i].Note, fontBody);
					cell.Format.Alignment = ParagraphAlignment.Left;
					cell.Borders.Color = new MigraDoc.DocumentObjectModel.Color(180, 180, 180);
					cell.MergeRight = grid.ListGridColumns.Count - 1 - grid.NoteSpanStart;
					edgeRows++;
				}
			}
			table.SetEdge(1, 0, grid.ListGridColumns.Count, edgeRows, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 1, Colors.Black);
			section.Add(table);
		}

		/// <summary>
		/// Draws the text and splits words that are too long for the column into multiple lines.
		/// </summary>
		private static void DrawTextWithWordSplits(Paragraph par, MigraDocFont fontBody, int colWidth, string cellText, XGraphics gx, PdfFont xFont)
		{
			string line = "";
			string word = "";
			//cellText=cellText.Replace("\r\n","\n").Replace("\n","\r\n");//Make sure all the line breaks are uniform
			for (int c = 0; c < cellText.Length; c++)
			{
				char letter = cellText[c];
				word += letter;
				if (c == cellText.Length - 1 || (!char.IsWhiteSpace(letter) && char.IsWhiteSpace(cellText[c + 1])))
				{//We have reached the end of the word.
					if ((line + word).All(x => char.IsWhiteSpace(x)))
					{//Sometimes gx.MeasureString will throw an exception if the text is all whitespace.
						par.AddFormattedText(line + word, fontBody);
						line = "";
						word = "";
						continue;
					}
					if (DoesTextFit(line + word, colWidth, gx, xFont))
					{
						line += word;
						if (IsLastWord(c, cellText))
						{
							par.AddFormattedText(line, fontBody);
						}
					}
					else
					{//The line plus the word do not fit.
						if (line == "")
						{//The word by itself does not fit.
							DrawWordChunkByChunk(word, colWidth, par, fontBody, gx, xFont);
						}
						else
						{
							par.AddFormattedText(line, fontBody);
							if (DoesTextFit(word, colWidth, gx, xFont))
							{
								if (IsLastWord(c, cellText))
								{
									par.AddFormattedText(word, fontBody);
								}
								line = word;
							}
							else
							{//The word by itself does not fit.
								DrawWordChunkByChunk(word, colWidth, par, fontBody, gx, xFont);
								line = "";
							}
						}
					}
					word = "";
				}
			}
		}

		/// <summary>
		/// Draws the word on multiple lines if it is too long to fit on one line.
		/// </summary>
		private static void DrawWordChunkByChunk(string word, int columnWidth, Paragraph paragraph, MigraDocFont fontBody, XGraphics gx, PdfFont xFont)
		{
			string chunk = "";

			for (int i = 0; i < word.Length; i++)
			{
				char letter = word[i];

				if ((chunk + letter).All(x => char.IsWhiteSpace(x)))
				{//Sometimes gx.MeasureString will throw an exception if the text is all whitespace.
					paragraph.AddFormattedText(chunk + letter, fontBody);
					continue;
				}

				if (DoesTextFit(chunk + letter, columnWidth, gx, xFont))
				{
					if (i == word.Length - 1)
					{//Reached the end of the word
						paragraph.AddFormattedText(chunk + letter, fontBody);
						return;
					}
					chunk += letter;
					continue;
				}

				paragraph.AddFormattedText(chunk, fontBody);
				chunk = "" + letter;
			}
		}

		/// <summary>
		/// Returns true if there are no other words past the index. This will only work if the index is the last character in a previous word.
		/// </summary>
		private static bool IsLastWord(int index, string cellText)
		{
			for (int i = index + 1; i < cellText.Length; i++)
			{
				if (!char.IsWhiteSpace(cellText[i]))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns true if the text fits in the specified width.
		/// </summary>
		private static bool DoesTextFit(string text, int columnWidth, XGraphics graphics, PdfFont font) 
			=> graphics.MeasureString(text, font).Width < columnWidth;
	}
}
