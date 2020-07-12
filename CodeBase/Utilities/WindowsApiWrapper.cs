using System;
using System.Runtime.InteropServices;

namespace CodeBase
{
    public static class WindowsApiWrapper
	{
		//This file is for all windows Desktop App API elements. It can be added to as needed.
		//http://www.pinvoke.net/
		//http://www.pinvoke.net/search.aspx?search=WM.USER&namespace=[All]

		private const int WM_USER = 0x0400;
		private const uint SB_SETPARTS = WM_USER + 4;
		private const uint SB_GETPARTS = WM_USER + 6;
		private const uint SB_GETTEXTLENGTH = WM_USER + 12;
		private const uint SB_GETTEXT = WM_USER + 13;
		public const int SCF_SELECTION = 0x0001;
		public const int EC_LEFTMARGIN = 0x0001;
		public const int EC_RIGHTMARGIN = 0x0002;

		/// <summary>
		/// http://docs.embarcadero.com/products/rad_studio/delphiAndcpp2009/HelpUpdate2/EN/html/delphivclwin32/Messages.html
		/// </summary>
		public enum EM_Rich : int
		{
			EM_GETSEL = 0x00B0,
			EM_SETSEL = 0x00B1,
			EM_GETRECT = 0x00B2,
			EM_SETRECT = 0x00B3,
			EM_SETRECTNP = 0x00B4,
			EM_SCROLL = 0x00B5,
			EM_LINESCROLL = 0x00B6,
			EM_GETMODIFY = 0x00B8,
			EM_SETMODIFY = 0x00B9,
			EM_GETLINECOUNT = 0x00BA,
			EM_LINEINDEX = 0x00BB,
			EM_SETHANDLE = 0x00BC,
			EM_GETHANDLE = 0x00BD,
			EM_GETTHUMB = 0x00BE,
			EM_LINELENGTH = 0x00C1,
			EM_LINEFROMCHAR = 0x00C9,
			EM_GETFIRSTVISIBLELINE = 0x00CE,
			EM_SETMARGINS = 0x00D3,
			EM_GETMARGINS = 0x00D4,
			EM_POSFROMCHAR = 0x00D6,
			EM_CHARFROMPOS = 0x00D7,
			EXGETSEL = WM_USER + 52,
			EXSETSEL = WM_USER + 55,
			GETCHARFORMAT = WM_USER + 58,
			SETCHARFORMAT = WM_USER + 68,
			SETOPTIONS = WM_USER + 77,
			GETOPTIONS = WM_USER + 78,
			GETTEXTEX = WM_USER + 94,
			GETTEXTLENGTHEX = WM_USER + 95,
			SHOWSCROLLBAR = WM_USER + 96,
			SETTEXTEX = WM_USER + 97,
			GETSCROLLPOS = WM_USER + 221,
			SETSCROLLPOS = WM_USER + 222,
		}

		/// <summary>
		/// http://docs.embarcadero.com/products/rad_studio/delphiAndcpp2009/HelpUpdate2/EN/html/delphivclwin32/Messages.html
		/// </summary>
		public enum WinMessagesOther
		{
			WM_PAINT = 0x000F,
			WM_CHAR = 0x0102,
			WM_VSCROLL = 0x115,
		}

		[DllImport("User32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
		public static extern int SendMessage(IntPtr hWnd, int msg, int wparam, int lparam);
	}
}
