using System;
using System.Windows.Forms;

namespace CodeBase
{
	public class ODMessageBox
	{
		/// <summary>
		/// Displays a message box in front of the specified object and with the specified text.
		/// </summary>
		public static DialogResult Show(string text)
			=> Show(text, "Imedisoft", MessageBoxButtons.OK, MessageBoxIcon.Information);

		/// <summary>
		/// Displays a message box in front of the specified object and with the specified text and caption.
		/// </summary>
		public static DialogResult Show(string text, string caption)
			=> Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);

		/// <summary>
		/// Displays a message box in front of the specified object and with the specified text, caption and buttons.
		/// </summary>
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
			=> Show(text, caption, buttons, MessageBoxIcon.Information);

		/// <summary>
		/// Displays a message box in front of the specified object and with the specified text, caption, buttons, and icon.
		/// </summary>
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
			=> ShowHelper(
				(form) => form.MsgBoxShow(text, caption, buttons, icon),
				() => MessageBox.Show(text, caption, buttons, icon));

		/// <summary>
		/// Displays a message box in front of the specified object and with the specified text.
		/// </summary>
		public static DialogResult Show(IWin32Window owner, string text)
			=> Show(owner, text, "Imedisoft", MessageBoxButtons.OK, MessageBoxIcon.Information);

		/// <summary>
		/// Displays a message box in front of the specified object and with the specified text and caption.
		/// </summary>
		public static DialogResult Show(IWin32Window owner, string text, string caption) 
			=> Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);

		/// <summary>
		/// Displays a message box in front of the specified object and with the specified text, caption and buttons.
		/// </summary>
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
			=> Show(owner, text, caption, buttons, MessageBoxIcon.Information);

		/// <summary>
		/// Displays a message box in front of the specified object and with the specified text, caption, buttons, and icon.
		/// </summary>
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon) 
			=> ShowHelper(
				(form) => form.MsgBoxShow(text, caption, buttons, icon), 
				() => MessageBox.Show(owner, text, caption, buttons, icon));

		/// <summary>
		/// Invokes one of the funcs passed in based on if there are any active progress windows showing and has focus.
		/// 
		/// Will invoke funcShowProgress if a progress window is active.  Otherwise; invokes funcShow.
		/// Recursively calls itself as needed if the active progress window was in the middle of closing when this method was invoked.
		/// </summary>
		/// <param name="updateProgressBoxFunc">The func that should execute if a progress window is currently showing to the user.</param>
		/// <param name="funcShow">The func that should execute if no progress window is currently showing to the user.</param>
		/// <returns>The dialog result from the func that ended up getting invoked.</returns>
		private static DialogResult ShowHelper(Func<FormProgressBase, DialogResult> updateProgressBoxFunc, Func<DialogResult> messageBoxFunc)
		{
			if (ODInitialize.IsRunningInUnitTest) throw new ApplicationException("Message boxes are not allowed for unit tests.");

			// Get the active form.
			var form = Form.ActiveForm;

			// If there is no progress dialog shown or the progress dialog is not the active form, show a message box.
			var formProgressBase = ODProgress.FormProgressActive;
			if (formProgressBase == null || (form != null && form != formProgressBase))
			{
				return messageBoxFunc();
			}

			// If the active form is a progress dialog, run the func to update the progress dialog.
			var dialogResult = DialogResult.Abort;
			try
			{
				if (formProgressBase.InvokeRequired)
                {
					formProgressBase.Invoke(() =>
					{
						dialogResult = updateProgressBoxFunc(formProgressBase);
					});
                }
                else
                {
					dialogResult = updateProgressBoxFunc(formProgressBase);
                }
			}
			catch (ObjectDisposedException)
			{
				dialogResult = ShowHelper(updateProgressBoxFunc, messageBoxFunc);
			}

			return dialogResult;
		}
	}
}
