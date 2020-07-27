using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;
using Imedisoft.Forms;
using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormConnectionLost : FormBase
	{
		private readonly EventCategory eventCategory;
        private readonly EventInfo firedEvent;
		private readonly Delegate firedEventDelegate;
		private readonly Func<bool> closeWindowFunc;
		private bool closeWindow = false;
		private Thread monitorThread;

		/// <summary>
		/// funcShouldWindowClose should return a boolean indicating if this window should close or not.
		/// Optionally set errorMessage to override the label text that is displayed to the user.
		/// Optionally set a custom eventType in order to listen for specific types of ODEvent.Fired events.
		/// Defaults to DataConnectionEvent.
		/// </summary>
		public FormConnectionLost(Func<bool> closeWindowFunc, EventCategory eventCategory = EventCategory.Undefined, string message = "")
		{
			InitializeComponent();

			messageLabel.Text = message;

			this.closeWindowFunc = closeWindowFunc;
			this.eventCategory = eventCategory;

			firedEvent = typeof(DataConnectionEvent).GetEvent("Fired");
			if (firedEvent == null)
            {
				throw new ApplicationException(
					$"'{typeof(DataConnectionEvent).FullName}' does not have a 'Fired' event.");
			}

			firedEventDelegate = Delegate.CreateDelegate(firedEvent.EventHandlerType, this, nameof(DataConnectionEventFired));
			firedEvent.GetAddMethod().Invoke(this, new object[] { firedEventDelegate });
		}

		private void FormConnectionLost_Load(object sender, EventArgs e)
		{
			if (closeWindowFunc != null)
			{
				if (monitorThread != null) return;

				monitorThread = new Thread(() =>
				{
                    try
                    {
						while (true)
						{
							if (closeWindow)
                            {
								Invoke(new Action(() => DialogResult = DialogResult.OK));

								break;
                            }

							Thread.Sleep(500);
						}
					}
					catch (ThreadAbortException)
                    {
                    }
				});

				monitorThread.Start();
			}
		}

		private void DataConnectionEventFired(DataConnectionEventArgs e)
		{
			try
			{
				if (eventCategory != EventCategory.Undefined && eventCategory != e.EventType)
				{
					return;
				}

				if (e.IsConnectionRestored) closeWindow = true;
			}
			catch
			{
			}
		}

		private void RetryButton_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;

			bool isConnected = closeWindowFunc?.Invoke() ?? false;

			Cursor = Cursors.Default;

			if (isConnected)
			{
				DialogResult = DialogResult.OK;
			}
		}

		private void ExitButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void FormConnectionLost_FormClosing(object sender, FormClosingEventArgs e)
		{
            try
            {
				monitorThread?.Abort();
			}
            catch { }

			firedEvent.GetRemoveMethod().Invoke(this, new object[] { firedEventDelegate });
		}
	}
}
