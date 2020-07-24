using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace CodeBase
{
    /// <summary>
    /// A base window designed to run in a separate thread so that the progress bar can smoothly spin without waiting on the main thread.
    /// 
    /// Takes care of registering and unregistering for the ODEvent passed into the constructor.
    /// 
    /// Also takes care of making sure that this window does not get "stuck" open by spawning a thread that monitors if CloseGracefully has been called.
    /// 
    /// Extending classes are supposed to take care of the desired UI.  Does not extend ODForm on purpose.
    /// </summary>
    public class FormProgressBase : Form
	{
		private readonly EventCategory eventCategory;
		private bool formHasClosed;

		/// <summary>
		/// An indicator owned by Open Dental indicating that this progress window needs to close regardless if it is done computing or not.
		/// Set to true by the entity that instantiated this progress form to gracefully close when the long computation has finished.
		/// </summary>
		public bool ForceClose { get; set; }

		/// <summary>
		/// The "Fired" event that is currently registered to this form.
		/// This is used when registering to custom ODEvents.
		/// It is necessary to keep track of so that we can unregister it when this progress window is closed.
		/// </summary>
		private EventInfo firedEvent;

		public FormProgressBase() : this(EventCategory.Undefined, null)
		{
		}

		public FormProgressBase(EventCategory eventCategory, Type eventType)
		{
			this.eventCategory = eventCategory;

			eventType ??= typeof(ODEvent);

			firedEvent = eventType.GetEvent("Fired");
			if (firedEvent == null)
			{
				throw new ApplicationException(
					$"The 'eventType' passed into {nameof(FormProgressBase)} does not have a 'Fired' event.\r\n" +
					$"Type passed in: {eventType.GetType()}");
			}

			// Spawn the monitor thread when the form is displayed...
			Shown += StartMonitorThread;

			var firedEventDelegate = Delegate.CreateDelegate(this.firedEvent.EventHandlerType, this, nameof(ODEvent_Fired));

			// Register our handler for the 'Fired' event.
			firedEvent.GetAddMethod().Invoke(this, new object[] { firedEventDelegate });

			// When the form is closed, remove our handler from the 'Fired' event.
			FormClosing += (s, e) 
				=> firedEvent.GetRemoveMethod().Invoke(this, new object[] { firedEventDelegate });

			// Keep track of when the form is closed to the monitor thread can quit when it happens.
			FormClosed += (s, e) => formHasClosed = true;
		}

		/// <summary>
		///		<para>
		///			Safely closes the form. Safe to call from another thread.
		///		</para>
		/// </summary>
		private void CloseSafely()
        {
			if (InvokeRequired)
            {
				Invoke(new Action(() => CloseSafely()));
            }
            else
            {
				DialogResult = DialogResult.OK;

				try
				{
					Close();
				}
				catch { }
			}
        }

		private void StartMonitorThread(object sender, EventArgs e)
		{
            // Spawn a separate thread that will monitor if this progress form should has indicated that it needs to close.
            // This thread will be a fail-safe in the sense that it will constantly monitor a separate indicator that this window should close.
            var forceCloseMonitorThread = new Thread(() =>
            {
                while (true)
                {
                    if (formHasClosed)
                    {
                        return;
                    }

                    if (ForceClose) // Something triggered the fact that this form should close.
                    {
						CloseSafely();

                        return;
                    }

                    Thread.Sleep(100);
                }
            })
            {
                Name = "FormProgressStatusMonitor_" + DateTime.Now.Ticks
            };

            forceCloseMonitorThread.Start();
		}

		/// <summary>
		/// Shows a message to the user with this progress window as the owner.
		/// </summary>
		public DialogResult MsgBoxShow(string text, string caption = "")
		{
			return MessageBox.Show(this, text, caption);
		}

		/// <summary>
		/// Shows a message to the user with this progress window as the owner.
		/// </summary>
		public DialogResult MsgBoxShow(string text, string caption, MessageBoxButtons buttons)
		{
			return MessageBox.Show(this, text, caption, buttons);
		}

		/// <summary>
		/// Shows a message to the user with this progress window as the owner.
		/// </summary>
		public DialogResult MsgBoxShow(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			return MessageBox.Show(this, text, caption, buttons, icon);
		}

		public void ODEvent_Fired(ODEventArgs e)
		{
			try
			{
				// We don't know what thread will cause a progress status change, so invoke this method as a delegate if necessary.
				if (InvokeRequired)
				{
					Invoke((Action)delegate () { ODEvent_Fired(e); });

					return;
				}

				// If Tag on the ODEvent is null then there is nothing to for this event handler to do.
				if (e.Tag == null) return;

				// Check to see if an ODEventType was set otherwise check the ODEventName to make sure this is an event that this instance cares to process.
				if (eventCategory != EventCategory.Undefined)
				{
					//Always use ODEventType if one was explicitly set regardless of what _odEventName is set to.
					if (eventCategory != e.EventType)
					{
						return;
					}
				}

				ProgressBarHelper progHelper = new ProgressBarHelper("");
				bool hasProgHelper = false;
				string status = "";

				if (e.Tag is string)
				{
					status = (string)e.Tag;
				}
				else if (e.Tag is ProgressBarHelper)
				{
					progHelper = (ProgressBarHelper)e.Tag;

					status = progHelper.LabelValue;

					hasProgHelper = true;
				}
				else
				{
					return;
				}

				UpdateProgress(status, progHelper, hasProgHelper);
			}
			catch
			{
			}
		}

		/// <summary>
		/// Extending classes are required to implement this method.
		/// This class was originally an abstract class which made this fact apparent but Visual Studio's designer doesn't play nicely.
		/// </summary>
		public virtual void UpdateProgress(string status, ProgressBarHelper progHelper, bool hasProgHelper)
			=> throw new NotImplementedException();
	}
}
