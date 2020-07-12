using System.Drawing;
using System.Windows.Forms;

namespace CodeBase
{
    public class ODClipboard
	{
		/// <summary>
		/// Gets or sets the clipboard value of the user as text.
		/// </summary>
		public static string Text
        {
			get => Clipboard.GetText();
			set => Clipboard.SetText(value);
        }

		/// <summary>
		/// Gets the contents of the clipboard as an image.
		/// </summary>
		public static Bitmap Image
        {
            get
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
}
