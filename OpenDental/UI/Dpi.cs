using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpenDental
{
    public class Dpi
	{
		public enum DPI_AWARENESS_CONTEXT
		{
			DPI_AWARENESS_CONTEXT_DEFAULT = 0,
			DPI_AWARENESS_CONTEXT_UNAWARE = -1,
			DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = -2,
			DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = -3
		}

		[DllImport("User32.dll")]
		public static extern DPI_AWARENESS_CONTEXT SetThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT dpiContext);

		/// <summary>
		/// This is used to set any Form to be unaware of Dpi.
		/// This will cause Windows to handle dpi the old way, by scaling a bitmap of the form.
		/// This will cause everything to get a little blurry, but it will at least all work correctly.
		/// This buys us time to fix any custom drawing. 
		/// Use this just before creating a form (FormExample formExample=new FormExample()). 
		/// Then, right after that line, call Dpi.SetDefault.
		/// </summary>
		public static void SetOld() => SetThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_UNAWARE);

		/// <summary>
		/// Converts a float or int from 96dpi to current screen dpi. Rounds to nearest int.
		/// </summary>
		public static int Scale(Control sender, float val96) 
			=> (int)(Math.Round(val96 / 96 * sender.DeviceDpi));

		/// <summary>
		/// Converts a float or int from current screen dpi to 96dpi. Rounds to nearest int.
		/// </summary>
		public static int Unscale(Control sender, float valScreen) 
			=> (int)(Math.Round(valScreen / sender.DeviceDpi * 96));
	}
}
