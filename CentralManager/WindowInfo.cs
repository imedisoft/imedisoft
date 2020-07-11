using System;

namespace CentralManager
{
    public class WindowInfo
	{
		/// <summary>
		/// This isn't constant because it will be the most recent active popup for the ProcessID. 
		/// It gets reset frequently.
		/// </summary>
		public IntPtr HWnd;
		
		public long CentralConnectionNum;
		
		/// <summary>
		/// This is constant for the life of the application, so it's a great foundation for finding and controlling a window.
		/// </summary>
		public int ProcessId;
		
		/// <summary>
		/// This window should be maximized when restored instead of set to normal.
		/// Set true whenever a window is found in a maximized state.
		/// Set false whenever a window is found in normal state.
		/// Left alone in minimized state.
		/// </summary>
		public bool WasMaximized;
		
		/// <summary>
		/// This window is currently minimized.
		/// </summary>
		public bool IsMinimized;

		public string GetStringState()
		{
			if (IsMinimized)
			{
				return "mimimized";
			}

			return "showing";
		}
	}
}
