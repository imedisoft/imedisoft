using CodeBase;
using Imedisoft.Forms;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows;

namespace OpenDental
{
    public partial class FormFriendlyException : FormBase
	{
		private readonly string message;
		private readonly Exception exception;
		private int detailsHeight;
		private int pagesPrinted;

		protected override bool HasHelpKey => false;

		public FormFriendlyException(string message, Exception exception)
		{
			InitializeComponent();

			this.message = message;
			this.exception = exception;

			Text += " - " + DateTime.Now.ToString(); //Append to title of form.
		}

		private void FormFriendlyException_Load(object sender, EventArgs e)
		{
			messageLabel.Text = message;

			stackTraceTextBox.Text = MiscUtils.GetExceptionText(exception);

			if (exception is ODException odex)
            {
				var query = odex.Query ?? "";

				if (string.IsNullOrEmpty(query))
                {
					detailsTabControl.TabPages.RemoveByKey(nameof(queryTabPage));
				}
                else
                {
					queryTextBox.Text = query;
                }
            }

			detailsHeight = detailsTabControl.Height;

			DetailsLinkLabel_Click(this, EventArgs.Empty);
		}

		private void DetailsLinkLabel_Click(object sender, EventArgs e)
		{
			if (detailsTabControl.Visible)
			{
				detailsHeight = detailsTabControl.Height;
				detailsTabControl.Visible = false;
				copyAllButton.Visible = false;
				printButton.Visible = false;

				Height -= detailsHeight;
			}
			else
			{
				detailsTabControl.Visible = true;
				copyAllButton.Visible = true;
				printButton.Visible = true;

				Height += detailsHeight;
			}
		}

		private void CopyAllButton_Click(object sender, EventArgs e)
		{
			try
			{
				Clipboard.SetText(Text + "\r\n" + stackTraceTextBox.Text + GetQueryText());
			}
			catch
			{
				ShowError("Could not copy contents to the clipboard. Please try again.");
			}
		}

		private string GetQueryText() 
			=> string.IsNullOrEmpty(queryTextBox.Text) ? "" : "\r\n-------------------------------------------\r\n" + queryTextBox.Text;

		private void PrintButton_Click(object sender, EventArgs e)
		{
			pagesPrinted = 0;

			ODprintout.InvalidMinDefaultPageWidth = 0;

			// No print previews, since this form is in and of itself a print preview.
			PrinterL.TryPrint(PrintPage, margins: new Margins(50, 50, 50, 50), duplex: Duplex.Horizontal);
		}

		/// <summary>
		/// Called for each page to be printed.
		/// </summary>
		private void PrintPage(object sender, PrintPageEventArgs e) => e.HasMorePages = !Print(e.Graphics, pagesPrinted++, e.MarginBounds);

		/// <summary>
		/// Prints one page. Returns true if pageToPrint is the last page in this print job.
		/// </summary>
		private bool Print(Graphics g, int pageToPrint, Rectangle margins)
		{
			// Messages may span multiple pages. We print the header on each page as well as the page number.
			float baseY = margins.Top;
			string text = "Page " + (pageToPrint + 1);
			Font font = Font;
			SizeF textSize = g.MeasureString(text, font);
			g.DrawString(text, font, Brushes.Black, margins.Right - textSize.Width, baseY);
			baseY += textSize.Height;
			text = Text;
			font = new Font(Font.FontFamily, 16, System.Drawing.FontStyle.Bold);
			textSize = g.MeasureString(text, font);
			g.DrawString(text, font, Brushes.Black, (margins.Width - textSize.Width) / 2, baseY);
			baseY += textSize.Height;
			font.Dispose();

			string[] messageLines = (stackTraceTextBox.Text + GetQueryText()).Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

			font = Font;
			bool isLastPage = false;

            for (int curPage = 0, msgLine = 0; curPage <= pageToPrint; curPage++)
			{
                //Set y to its initial value for the current page (right after the header).
                float y = curPage * (margins.Bottom - baseY);
                while (msgLine < messageLines.Length)
				{
					//If a line is blank, we need to make sure that is counts for some vertical space.
					if (messageLines[msgLine] == "")
					{
						textSize = g.MeasureString("A", font);
					}
					else
					{
						textSize = g.MeasureString(messageLines[msgLine], font);
					}
					//Would the current text line go past the bottom margin?
					if (y + textSize.Height > (curPage + 1) * (margins.Bottom - baseY))
					{
						break;
					}
					if (curPage == pageToPrint)
					{
						g.DrawString(messageLines[msgLine], font, Brushes.Black, margins.Left, baseY + y - curPage * (margins.Bottom - baseY));
						if (msgLine == messageLines.Length - 1)
						{
							isLastPage = true;
						}
					}
					y += textSize.Height;
					msgLine++;
				}
			}

			return isLastPage;
		}

		private void CloseButton_Click(object sender, EventArgs e) => Close();
	}

	public class FriendlyException
	{
		public static void Show(string message, Exception exception, string closeButtonText = "")
		{
			if (ODInitialize.IsRunningInUnitTest) throw new Exception(message, exception);

			try
			{
				new FormFriendlyException(message, exception).ShowDialog();
			}
			catch (Exception e)
			{
                ODMessageBox.Show(
					$"Error encountered: {exception.Message}\r\n\r\n" +
					$"Additional error: {e.Message}");
			}
		}
	}
}
