using System;
using System.Windows.Forms;

namespace CodeBase
{
	public class ODMessageBox
	{
		/// <summary>
		/// Shows a message to the user.
		/// 
		/// Automatically checks to see if a progress window is showing and will ask the 
		/// progress window to show the message to the user so that the progress window doesn't cover up the question.
		/// </summary>
		public static DialogResult Show(string text) 
			=> ShowHelper(
				(form) => form.MsgBoxShow(text), 
				() => MessageBox.Show(text));

		/// <summary>
		/// Shows a message to the user.
		/// 
		/// Automatically checks to see if a progress window is showing and will ask 
		/// the progress window to show the message to the user so that the progress window doesn't cover up the question.
		/// </summary>
		public static DialogResult Show(string text, string caption)
			=> ShowHelper(
				(form) => form.MsgBoxShow(text, caption), 
				() => MessageBox.Show(text, caption));

		/// <summary>
		/// Shows a message to the user.
		/// 
		/// Automatically checks to see if a progress window is showing and will ask 
		/// the progress window to show the message to the user so that the progress window doesn't cover up the question.
		/// </summary>
		public static DialogResult Show(IWin32Window owner, string text) 
			=> ShowHelper(
				(form) => form.MsgBoxShow(text), 
				() => MessageBox.Show(owner, text));

		/// <summary>
		/// Shows a message to the user.
		/// 
		/// Automatically checks to see if a progress window is showing and will ask 
		/// the progress window to show the message to the user so that the progress window doesn't cover up the question.
		/// </summary>
		public static DialogResult Show(IWin32Window owner, string text, string caption) 
			=> ShowHelper(
				(form) => form.MsgBoxShow(text, caption), 
				() => MessageBox.Show(owner, text, caption));

		/// <summary>
		/// Shows a message to the user.  
		/// 
		/// Automatically checks to see if a progress window is showing and will ask 
		/// the progress window to show the message to the user so that the progress window doesn't cover up the question.
		/// </summary>
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons) 
			=> ShowHelper(
				(form) => form.MsgBoxShow(text, caption, buttons),
				() => MessageBox.Show(text, caption, buttons));

		/// <summary>
		/// Shows a message to the user.
		/// 
		/// Automatically checks to see if a progress window is showing and will ask the 
		/// progress window to show the message to the user so that the progress window doesn't cover up the question.
		/// </summary>
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons) 
			=> ShowHelper(
				(form) => form.MsgBoxShow(text, caption, buttons), 
				() => MessageBox.Show(owner, text, caption, buttons));

		/// <summary>
		/// Shows a message to the user.
		/// 
		/// Automatically checks to see if a progress window is showing and will ask 
		/// the progress window to show the message to the user so that the progress window doesn't cover up the question.
		/// </summary>
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon) 
			=> ShowHelper(
				(form) => form.MsgBoxShow(text, caption, buttons, icon), 
				() => MessageBox.Show(text, caption, buttons, icon));

		/// <summary>
		/// Shows a message to the user.
		/// 
		/// Automatically checks to see if a progress window is showing and will ask 
		/// the progress window to show the message to the user so that the progress window doesn't cover up the question.
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
		/// <param name="funcShowOverProgress">The func that should execute if a progress window is currently showing to the user.</param>
		/// <param name="funcShow">The func that should execute if no progress window is currently showing to the user.</param>
		/// <returns>The dialog result from the func that ended up getting invoked.</returns>
		private static DialogResult ShowHelper(Func<FormProgressBase, DialogResult> funcShowOverProgress, Func<DialogResult> funcShow)
		{
			// Unit tests are not designed to display message boxes.
			// Throw an exception instead of displaying the message so that unit tests cannot get locked up.
			if (ODInitialize.IsRunningInUnitTest)
			{
				throw new ApplicationException("Message boxes are not allowed for unit tests.");
			}

			//Get the active form for the current application.  This property will return null if another application has focus (not our application).
			//This is rare enough that it is acceptable to default the parent of the message box to the progress window (if one is present).
			//This may cause the MessageBox to show up behind a form that HAD focus with a progress window behind it (e.g. Registration Key Edit window).
			Form FormActive = Form.ActiveForm;

			//Get the "active" progress window if one is present.  Utilize a shallow copy so race conditions don't affect us inadvertently.
			FormProgressBase FormPB = ODProgress.FormProgressActive;
			//So that the logic is easier to follow, check for the two scenarios that can cause an immediate kick out.
			if (FormPB == null                               //The easiest scenario is when there is no active progress window.
				|| (FormActive != null && FormActive != FormPB))//There is a progress window but there is another form owned by the application that has focus.
			{
				//There is no progress window showing or there is one showing but we know that a different window of our application has focus.
				return funcShow();//Show the message box like normal and don't override its Parent property with the progress window.
			}

			//There is a progress window present and it could be the active form for the application or another application has focus and we don't know.
			//It is rare enough for applications to leave progress windows open while showing dialogs or new forms to the user.
			//Default to forcing the active progress window to be the parent form of the new message box because that scenario is so rare.
			DialogResult dialogResult = DialogResult.Abort;
			try
			{
				FormPB.InvokeIfRequired(() => dialogResult = funcShowOverProgress(FormPB));
			}
			catch (ObjectDisposedException)
			{
				//Explicitly catch object disposed exceptions due to rare race conditions.
				//The active progress window that was just showing could have been placed on the "invoke stack" for close and disposal prior to our invoke.
				//The active progress window would successfully close and dispose first and then FormPB would be a reference to a disposed window
				//which would cause .InvokeIfRequired() to throw an ObjectDisposedException.
				//No error should be thrown in this scenario and instead we should retry this method because the active progress window could be different.
				//E.g. there will be a different active progress window if multiple progress windows were showing at the same time
				//OR we we eventually get back to the main thread which will not require invoking over to a progress window at all.
				dialogResult = ShowHelper(funcShowOverProgress, funcShow);//Recursive call on purpose.
			}
			return dialogResult;
		}
	}
}
