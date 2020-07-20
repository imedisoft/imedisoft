using System;
using System.Windows.Forms;

namespace OpenDental
{
    public class Dpi
	{
		/// <summary>
		/// Converts a float or int from 96dpi to current screen dpi. Rounds to nearest int.
		/// </summary>
		public static int Scale(Control sender, float val96) 
			=> (int)Math.Round(val96 / 96 * sender.DeviceDpi);

		/// <summary>
		/// Converts a float or int from current screen dpi to 96dpi. Rounds to nearest int.
		/// </summary>
		public static int Unscale(Control sender, float valScreen) 
			=> (int)Math.Round(valScreen / sender.DeviceDpi * 96);
	}
}
