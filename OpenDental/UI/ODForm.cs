using CodeBase;
using Google.Apis.Util;
using Imedisoft.UI;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    /// <summary>
    /// Most OD forms extend this class. It does help and signal processing. 
    /// Jordan is the only one allowed to alter this file.
    /// </summary>
    public class ODForm : Form
	{
        ///<summary>Keeps track of window state changes.  We use it to restore minimized forms to their previous state.</summary>
        private FormWindowState _windowStateOld;

		///<summary>List of controls in the form that are used to filter something in the form.</summary>
		private List<Control> _listFilterControls = new List<Control>();
		///<summary>The given action to run after filter input is commited for FilterCommitMs.</summary>
		private Action _filterAction;
		///<summary>The thread that is ran to check if filter controls have had their changes commited.  If a single control is considered to have commited changes then the thread will only fire the _filterAction once and then will wait for more input.</summary>
		private ODThread _threadFilter;
		///<summary></summary>
		private DateTime _timeLastModified = DateTime.MaxValue;
		///<summary>The number of milliseconds to wait after the last user input on one of the specified filter controls to wait before calling _filterAction.  After some testing, 1 second felt most natural.</summary>
		protected int _filterCommitMs = 1000;
		private static List<ODForm> _listSubscribedForms = new List<ODForm>();

		/// <summary>
		/// Initializes a new instance of the <see cref="ODForm"/> class.
		/// </summary>
		public ODForm()
		{
			Shown += new EventHandler(ODForm_Shown);
			FormClosing += new FormClosingEventHandler(ODForm_FormClosing);
			Resize += new EventHandler(ODForm_Resize);
		}

		#region Properties

		/// <summary>
		/// Override and set to false if you want your form not to respond to F1 key for help.
		/// </summary>
		[Browsable(false)]
		protected virtual bool HasHelpKey => true;

        /// <summary>
        /// True when form has been shown by the system.
        /// </summary>
        [Browsable(false)]
        public bool HasShown { get; private set; } = false;

        /// <summary>
		/// Only true if FormClosed has been called by the system.
		/// </summary>
        [Browsable(false)]
        public bool HasClosed { get; private set; } = false;

        /// <summary>
		/// This is here so that the default and reset show properly. 
		/// Also, this still shows in designer in spite of being set to Browsable false. 
		/// The value doesn't matter at all because this property is deprecated with new high dpi support.
		/// </summary>
        [Browsable(false)]
		[DefaultValue(AutoScaleMode.None)]
		public new AutoScaleMode AutoScaleMode
		{
			get
			{
				return base.AutoScaleMode;
			}
			set
			{
				base.AutoScaleMode = value;
			}
		}

		#endregion Properties

		#region Methods - Event Handlers
		///<summary>Fires first for all FormClosing events of this form.</summary>
		private void ODForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			//FormClosed event added to list of closing events as late as possible.
			//This allows the implementing form to set another FormClosing event to be fired before our base event here.
			//The advantage is that HasClosed will only be true if ALL FormClosing events have fired for this form.
			this.FormClosed += new FormClosedEventHandler(this.ODForm_FormClosed);
		}

		private void ODForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (_threadFilter != null)
			{
				_threadFilter.QuitAsync();//It's fine if our thread loop finishes, it protects against unhandled exceptions.
				_threadFilter = null;
				//We don't want an enumeration exception here so don't clear _listFilterControls. It will get garbage collected anyways.
			}
			HasClosed = true;
		}



		/// <summary>
		/// Gets the help subject.
		/// </summary>
		public virtual string HelpSubject { get; private set; }

        protected override void OnLoad(EventArgs e)
        {
			var helpAttribute = GetType().GetCustomAttribute<HelpSubjectAttribute>();
			if (helpAttribute != null)
            {
				HelpSubject = helpAttribute.Subject;
				if (string.IsNullOrEmpty(HelpSubject))
                {
					HelpSubject = Name;
                }

				HelpButton = true;
            }

            base.OnLoad(e);
        }

		protected override void OnHelpButtonClicked(CancelEventArgs e)
		{
			base.OnHelpButtonClicked(e);

			if (!e.Cancel)
			{
				// TODO: Implement me.

				e.Cancel = true;
			}
		}



		private void ODForm_Shown(object sender, EventArgs e)
		{
			HasShown = true;//Occurs after Load(...)
			this.FormClosed += delegate
			{
				_listSubscribedForms.Remove(this);
			};
			_listSubscribedForms.Add(this);
			//This form has just invoked the "Shown" event which probably means it is important and needs to actually show to the user.
			//There are times in the application that a progress window (e.g. splash screen) will be showing to the user and a new form is trying to show.
			//Therefore, forcefully invoke "Activate" if there is a progress window currently on the screen.
			//Invoking Activate will cause the new form to show above the progress window (if TopMost=false) even though it is in another thread.
			if (ODProgress.FormProgressActive != null)
			{
				this.Activate();
			}

		}

		private void ODForm_Resize(object sender, EventArgs e)
		{
			if (WindowState != FormWindowState.Minimized)
			{
				_windowStateOld = WindowState;
			}
		}


		#endregion Methods - Event Handlers

		#region Methods - Public
		///<summary>Sets the entire form into "read only" mode by disabling all controls on the form. Pass in any controls that should say enabled (e.g. Cancel button). This can be used to stop users from clicking items they do not have permission for.</summary>
		public void DisableAllExcept(params Control[] enabledControls)
		{
			foreach (Control ctrl in this.Controls)
			{
				if (enabledControls.Contains(ctrl))
				{
					continue;
				}
				//Attempt to disable the control.
				try
				{
					ctrl.Enabled = false;
				}
				catch
				{
					//Some controls do not support being disabled.  E.g. the WebBrowser control will throw an exception here.
				}
			}
		}

		///<summary>Returns true if the form passed in has been disposed or if it extends ODForm and HasClosed is true.</summary>
		public static bool IsDisposedOrClosed(Form form)
		{
			if (form.IsDisposed)
			{//Usually the system will set IsDisposed to true after a form has closed.  Not true for FormHelpBrowser.
				return true;
			}
			if (form.GetType().GetProperty("HasClosed") != null)
			{//Is a Form and has property HasClosed => Assume is an ODForm.
			 //Difficult to compare type to ODForm, because it is a template class.
				if ((bool)form.GetType().GetProperty("HasClosed").GetValue(form))
				{//This is how we know FormHelpBrowser has closed.
					return true;
				}
			}
			return false;
		}

		public void Restore()
		{
			if (WindowState == FormWindowState.Minimized)
			{
				WindowState = _windowStateOld;
			}
		}
		#endregion Methods - Public

		#region Signal Processing

		public void ProcessSignals(List<Signalod> signals)
			=> Logger.LogAction("ODForm.ProcessSignals", () => OnProcessSignals(signals));

		/// <summary>
		/// Override this if your form cares about signal processing.
		/// </summary>
		public virtual void OnProcessSignals(List<Signalod> signals)
		{
		}

		/// <summary>
		/// Spawns a new thread to retrieve new signals from the DB, update caches, and broadcast signals to all subscribed forms.
		/// </summary>
		public static void SignalsTick(Action onShutdown, Action<List<ODForm>, List<Signalod>> onProcess, Action onDone)
		{
			var signals = new List<Signalod>();

			ODThread threadRefreshSignals = new ODThread((o) =>
			{
				// Get new signals from DB.
				Logger.LogAction("RefreshTimed", () => signals = Signalods.RefreshTimed(Signalods.SignalLastRefreshed));

				// Only update the time stamp with signals retreived from the DB. Do NOT use listLocalSignals to set timestamp.
				if (signals.Count > 0)
				{
					Signalods.SignalLastRefreshed = signals.Max(x => x.SigDateTime);
					Signalods.ApptSignalLastRefreshed = Signalods.SignalLastRefreshed;
				}

				Logger.LogVerbose("Found " + signals.Count.ToString() + " signals");
				if (signals.Count == 0)
				{
					return;
				}

				// Logger.LogVerbose("Signal count(s)", string.Join(" - ", listSignals.GroupBy(x => x.IType).Select(x => x.Key.ToString() + ": " + x.Count())));
				if (signals.Exists(x => x.IType == InvalidType.ShutDownNow))
				{
					onShutdown();
					return;
				}

				var invalidTypes = signals
					.FindAll(x => x.FKey == 0 && x.FKeyType == KeyType.Undefined)
					.Select(x => x.IType)
					.Distinct()
					.ToArray();

				Cache.Refresh(invalidTypes);

				onProcess(_listSubscribedForms, signals);
			});


			threadRefreshSignals.AddExceptionHandler((e) =>
			{
				DateTime dateTimeRefreshed;
				try
				{
					//Signal processing should always use the server's time.
					dateTimeRefreshed = MiscData.GetNowDateTime();
				}
				catch
				{
					//If the server cannot be reached, we still need to move the signal processing forward so use local time as a fail-safe.
					dateTimeRefreshed = DateTime.Now;
				}
				Signalods.SignalLastRefreshed = dateTimeRefreshed;
				Signalods.ApptSignalLastRefreshed = dateTimeRefreshed;
			});

			threadRefreshSignals.AddExitHandler((o) =>
			{
				onDone();
			});

			threadRefreshSignals.Name = "SignalsTick";
			threadRefreshSignals.Start(true);
		}

		#endregion Signal Processing





		#region Filtering
		///<summary>Call before form is Shown. Adds the given controls to the list of filter controls. We will loop through all the controls in the list to identify the first control that has had its filter change commited for FilterCommitMs.  Once a filter is commited, the filter action will be invoked and the thread will wait for the next filter change to start the thread again.  Controls which are not text-based will commit immediately and will not use a thread (ex checkboxes).  filterCommitMs: The number of milliseconds to wait after the last user input on one of the specified filter controls to wait before calling _filterAction.</summary>
		protected void SetFilterControlsAndAction(Action action, int filterCommitMs, params Control[] arrayControls)
		{
			SetFilterControlsAndAction(action, arrayControls);
			_filterCommitMs = filterCommitMs;
		}

		///<summary>Call before form is Shown. Adds the given controls to the list of filter controls. We will loop through all the controls in the list to identify the first control that has had its filter change commited for FilterCommitMs. Once a filter is commited, the filter action will be invoked and the thread will wait for the next filter change to start the thread again. Controls which are not text-based will commit immediately and will not use a thread (ex checkboxes).</summary>
		protected void SetFilterControlsAndAction(Action action, params Control[] arrayControls)
		{
			if (HasShown)
			{
				return;
			}
			_filterAction = action;
			foreach (Control control in arrayControls)
			{
				//Keep the following if/else block in alphabetical order to it is easy to see which controls are supported.
				if (control.GetType().IsSubclassOf(typeof(CheckBox)) || control.GetType() == typeof(CheckBox))
				{
					CheckBox checkbox = (CheckBox)control;
					checkbox.CheckedChanged += Control_FilterCommitImmediate;
				}
				else if (control.GetType().IsSubclassOf(typeof(ComboBox)) || control.GetType() == typeof(ComboBox))
				{
					ComboBox comboBox = (ComboBox)control;
					comboBox.SelectionChangeCommitted += Control_FilterCommitImmediate;
				}
				else if (control.GetType().IsSubclassOf(typeof(ComboBoxMulti)) || control.GetType() == typeof(ComboBoxMulti))
				{
					ComboBoxMulti comboBoxMulti = (ComboBoxMulti)control;
					comboBoxMulti.SelectionChangeCommitted += Control_FilterCommitImmediate;
				}
				else if (control.GetType().IsSubclassOf(typeof(ODDateRangePicker)) || control.GetType() == typeof(ODDateRangePicker))
				{
					ODDateRangePicker dateRangePicker = (ODDateRangePicker)control;
					dateRangePicker.CalendarSelectionChanged += Control_FilterCommitImmediate;
				}
				else if (control.GetType().IsSubclassOf(typeof(ODDatePicker)) || control.GetType() == typeof(ODDatePicker))
				{
					ODDatePicker datePicker = (ODDatePicker)control;
					datePicker.DateTextChanged += Control_FilterChange;
				}
				else if (control.GetType().IsSubclassOf(typeof(TextBoxBase)) || control.GetType() == typeof(TextBoxBase))
				{
					//This includes TextBox and RichTextBox, therefore also includes ODtextBox, ValidNum, ValidNumber, ValidDouble.
					control.TextChanged += Control_FilterChange;
				}
				else if (control.GetType().IsSubclassOf(typeof(ListBox)) || control.GetType() == typeof(ListBox))
				{
					control.MouseUp += Control_FilterChange;
				}
				else if (control.GetType().IsSubclassOf(typeof(ComboBoxClinicPicker)) || control.GetType() == typeof(ComboBoxClinicPicker))
				{
					((ComboBoxClinicPicker)control).SelectionChangeCommitted += Control_FilterCommitImmediate;
				}
				else if (control.GetType().IsSubclassOf(typeof(ComboBoxPlus)) || control.GetType() == typeof(ComboBoxPlus))
				{
					((ComboBoxPlus)control).SelectionChangeCommitted += Control_FilterCommitImmediate;
				}
				else
				{
					throw new NotImplementedException("Filter control of type " + control.GetType().Name + " is undefined.  Define it in ODForm.AddFilterControl().");
				}
				_listFilterControls.Add(control);
			}
		}

		///<summary>A typical try-get, with an additional check to see if the form is disposed or control is disposed.</summary>
		private bool TryGetFilterInfo(Control control)
		{
			if (this.Disposing || this.IsDisposed || control.IsDisposed)
			{
				return false;
			}
			return true;
		}

		///<summary>Commits the filter action immediately.</summary>
		private void Control_FilterCommitImmediate(object sender, EventArgs e)
		{
			if (!HasShown)
			{
				//Form has not finished the Load(...) function.
				//Odds are the form is initializing a filter in the form load and the TextChanged, CheckChanged, etc fired prematurely.
				return;
			}
			_timeLastModified = DateTime.Now;
			FilterActionCommit();//Immediately commit checkbox changes.
		}

		///<summary>Commits the filter action according to the delayed interval and input wakeup algorithm which uses FilterCommitMs.</summary>
		private void Control_FilterChange(object sender, EventArgs e)
		{
			if (!HasShown)
			{
				//Form has not finished the Load(...) function.
				//Odds are the form is initializing a filter in the form load and the TextChanged, CheckChanged, etc fired prematurely.
				return;
			}
			Control control = (Control)sender;
			if (!TryGetFilterInfo(control))
			{
				return;
			}
			if (IsDisposedOrClosed(this))
			{
				//FormClosed even has already occurred.  Can happen if a control in _listFilterControls has a filter action subscribed to an event that occurs after the 
				//FormClosed event, ex CellLeave in FormQueryParser triggers TextBox.TextChanged when closing via shortcut keys (Alt+O).
				return;
			}
			_timeLastModified = DateTime.Now;
			if (_threadFilter == null)
			{//Ignore if we are already running the thread to perform a refresh.
			 //The thread does not ever run in a form where the user has not modified the filters.
				#region Init _threadFilter      
				this.FormClosed += new FormClosedEventHandler(this.ODForm_FormClosed); //Wait until closed event so inheritor has a chance to cancel closing event.
																					   //No need to add thread waiting. We will take care of this with FilterCommitMs within our own thread when it runs.
				_threadFilter = new ODThread(1, ((t) => { ThreadCheckFilterChangeCommited(t); }));
				_threadFilter.Name = "ODFormFilterThread_" + Name;
				//Do not add an exception handler here. It would inadvertantly swallow real exceptions as thrown by the Main thread.
				_threadFilter.Start(false);//We will quit the thread ourselves so we can track other variables.
				#endregion
			}
			else
			{
				_threadFilter.WakeUp();
			}
		}

		///<summary>The thread belonging to Control_FilterChange.</summary>
		private void ThreadCheckFilterChangeCommited(ODThread thread)
		{
			//Might be running after FormClosing()
			foreach (Control control in _listFilterControls)
			{
				if (thread.HasQuit)
				{//In case the thread is executing when the user closes the form and QuitSync() is called in FormClosing().
					return;
				}
				if (!TryGetFilterInfo(control))
				{//Just in case.
					continue;
				}
				double diff = (DateTime.Now - _timeLastModified).TotalMilliseconds;
				if (diff <= _filterCommitMs)
				{//Time elapsed is less than specified time.
					continue;//Check again later.
				}
				FilterActionCommit();
				thread.Wait(int.MaxValue);//Wait forever... or until Control_FilterChange(...) wakes the thread up or until form is closed.
				break;//Do not check other controls since we just called the filters action.
			}
		}

		private void FilterActionCommit()
		{
			Exception ex = null;
			//Synchronously invoke the "Refresh"/filter action function for the form on the main thread and invoke to prevent thread access violation exceptions.
			this.InvokeIfNotDisposed(() =>
			{
				//Only invoke if action handler has been set.
				try
				{
					_filterAction?.Invoke();
				}
				catch (Exception e)
				{
					//Simply throwing here would replace the stack trace with this thread's stack. 
					//Provide this exception as the inner exception below once we are out of the main thread's invoke to preserve both.
					ex = e;
				}
			});
			//Throw any errors that happened within the worker delegate while we were in a threaded context.
			ODException.TryThrowPreservedCallstack(ex);
		}


		#endregion Filtering
	}
}

//2020-04-27- Issues for later:
//ODProgress and Help both have UI in a separate thread from the main OD thread.
//This is bad.  Both should be deprecated and rewritten.
//ODProgress has tentacles, including ODMessageBox and various code inside this class.
//As long as ODProgress exists, none of the related code can be damaged.
//Another issue to be aware of is how OD forces closing.
//It must force various open windows to close, 
//so there is a fair amount of code scattered around that tries to close windows and then kills them.
//This code will generally stay in place since it's good.
//Signal processing code doesn't really need to be in this class, so we'll move it eventually.