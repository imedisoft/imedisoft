using System.Drawing;
using System.Windows.Forms;

namespace CodeBase
{
    public class ODClipboard
	{
		/// <summary>
		/// Writes the text to the user's clipboard.
		/// </summary>
		public static void SetClipboard(string text) => Clipboard.SetText(text);

		/// <summary>
		/// Gets the contents of the user's clipboard as text.
		/// </summary>
		public static string GetText() => Clipboard.GetText();

		/// <summary>
		/// Gets the contents of the user's clipboard as an image.
		/// Returns null if the clipboard does not contain an image.
		/// </summary>
		public static Bitmap GetImage()
		{
			IDataObject iDataObject = Clipboard.GetDataObject();

			Bitmap bitmapPaste = null;
			if (iDataObject.GetDataPresent(DataFormats.Bitmap))
			{
				bitmapPaste = (Bitmap)iDataObject.GetData(DataFormats.Bitmap);
			}

			return bitmapPaste;
		}
	}
}
