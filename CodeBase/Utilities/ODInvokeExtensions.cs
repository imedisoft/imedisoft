using System;
using System.Windows.Forms;

namespace CodeBase
{
	public static class ODInvokeExtensions
	{
		/// <summary>
		/// Invoke an action on a control.
		/// </summary>
		public static void Invoke(this Control control, Action action) 
			=> control.Invoke(action);

		/// <summary>
		/// BeginInvoke an action on a control.
		/// </summary>
		public static void BeginInvoke(this Control control, Action action)
			=> control.BeginInvoke(action);

		/// <summary>
		/// Invoke an action on a control if InvokeRequired is true.
		/// </summary>
		public static void InvokeIfRequired(this Control control, Action action)
		{
			if (control.InvokeRequired)
			{
				control.Invoke(action);
			}
			else
			{
				action();
			}
		}

		/// <summary>
		/// Invoke an action on a control. If the control is disposing or disposed, will return without performing the action.
		/// </summary>
		public static void InvokeIfNotDisposed(this Control control, Action action)
		{
			if (control.Disposing || control.IsDisposed)
			{
				return;
			}

			bool invokeSuccessful = false;

			try
			{
				control.Invoke(() =>
				{
					invokeSuccessful = true;

					action();
				});
			}
			catch 
			{
				if (invokeSuccessful)
				{
					throw;
				}
			}
		}
	}
}
