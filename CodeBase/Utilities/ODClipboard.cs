using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBase {
	public class ODClipboard {
		///<summary>Writes the text to the user's clipboard.</summary>
		public static void SetClipboard(string text) {
			if(ODBuild.IsWeb()) {
				ODCloudClient.SendDataToBrowser(text,(int)ODCloudClient.BrowserAction.SetClipboard);
			}
			else {
				Clipboard.SetText(text);
			}
		}

		///<summary>Gets the contents of the user's clipboard as text.</summary>
		public static string GetText() {
			if(ODBuild.IsWeb()) {
				return ODCloudClient.SendToBrowserSynchronously("",ODCloudClient.BrowserAction.GetClipboardText);
			}
			return Clipboard.GetText();
		}


		///<summary>Gets the contents of the user's clipboard as an image. Returns null if the clipboard does not contain an image.</summary>
		public static Bitmap GetImage() {
			if(ODBuild.IsWeb()) {
				string base64=ODCloudClient.SendToBrowserSynchronously("",ODCloudClient.BrowserAction.GetClipboardImage,timeoutSecs:30);
				if(base64.IsNullOrEmpty()) {
					return null;
				}
				byte[] rawData=Convert.FromBase64String(base64);
				MemoryStream stream=new MemoryStream(rawData);
				Bitmap image=new Bitmap(stream);
				return image;
			}
			IDataObject iDataObject=Clipboard.GetDataObject();
			Bitmap bitmapPaste=null;
			if(iDataObject.GetDataPresent(DataFormats.Bitmap)) {
				bitmapPaste=(Bitmap)iDataObject.GetData(DataFormats.Bitmap);
			}
			return bitmapPaste;
		}
	}
}
