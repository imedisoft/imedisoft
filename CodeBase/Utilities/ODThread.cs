using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Forms;

namespace CodeBase
{
    /// <summary>
    /// A wrapper for the c# Thread class.
    /// The purpose of this class is to help implement a well defined pattern throughout our applications.
    /// It also allows us to better document threading where C# lacks documentation.
    /// Since there is no way to get the list of managed threads for an application, the only way we can maintain a list is to do it ourselves.
    /// The advantage of maintaining a list of managed threads is that we can much more easily ensure that all threads are gracefully quit when the program exits.
    /// </summary>
    public class ODThread
	{
		/// <summary>
		/// The C# thread that is used to run ODThread internally.
		/// </summary>
		private Thread _thread = null;
		
		/// <summary>
		/// Sleep timer which can be interrupted elegantly.
		/// </summary>
		private readonly AutoResetEvent _waitEvent = new AutoResetEvent(false);
		
		/// <summary>
		/// The amount of time in milliseconds that this thread will sleep before calling the WorkerDelegate again.
		/// Setting the interval to zero or a negative number will call the WorkerDelegate once and then quit itself.
		/// </summary>
		public int TimeIntervalMS = 0;
		
		/// <summary>
		/// Pointer to the function from the calling code which will perform the majority of this thread's work.
		/// </summary>
		private WorkerDelegate _worker = null;
		
		/// <summary>
		/// Pointer to the function from the calling code which will be alerted when the run function has thrown an unhandled exception.
		/// </summary>
		private ExceptionDelegate _exceptionHandler = null;
		
		/// <summary>
		/// Pointer to the function from the calling code which will be alerted when the run function has completed.
		/// This will NOT fire if Join() times out.
		/// </summary>
		private WorkerDelegate _exitHandler = null;
		
		/// <summary>
		/// Pointer to the function from the calling code which will be run before the main worker delegate starts.
		/// </summary>
		private WorkerDelegate _setupHandler = null;
		
		/// <summary>
		/// Pointer to the function that runs in the constructor of ODThread.
		/// </summary>
		private static WorkerDelegate _onInitialize = null;
		
		/// <summary>
		/// Custom data which can be set before launching the thread and then safely accessed within the WorkerDelegate.
		/// Helps prevent the need to lock objects due to multi-threading, most of the time.
		/// </summary>
		public object Tag = null;
		
		/// <summary>
		/// Custom data which can be used within the WorkerDelegate.
		/// Helps prevent the need to lock objects due to multi-threading, most of the time.
		/// </summary>
		public object[] Parameters = null;
		
		/// <summary>
		/// Used to identify groups of ODThread objects.
		/// Helpful when you need to wait for or quit an entire group of threads.
		/// Initially set to "default".
		/// </summary>
		public string GroupName = "default";
		
		/// <summary>
		/// Global list of all ODThreads which have not been quit.
		/// Used for thread group operations.
		/// </summary>
		private static readonly List<ODThread> _listOdThreads = new List<ODThread>();
		
		/// <summary>
		/// Thread safe lock object. Any time a static variable is accessed, it MUST be wrapped with a lock.
		/// Failing to lock will result in a potential for unsafe access by multiple threads at the same time.
		/// </summary>
		private static readonly object _lockObj = new object();
		
		/// <summary>
		/// Only set when calling Start().
		/// Causes this thread to automatically remove itself from the global list of ODThreads once it has finished doing work.
		/// </summary>
		private bool _isAutoCleanup;
		
		/// <summary>
		/// Used internally by RegisterForUnhandled().
		/// </summary>
		private static Action<Exception, Thread> OnUnhandledException;
		
		/// <summary>
		/// Stores the object that was returned via the func GetDatabaseContext which was invoked in the constructor.
		/// </summary>
		private object _databaseContext;
		
		/// <summary>
		/// This func gets invoked within the constructor of this thread.
		/// The goal is to capture the database context of the parent thread.
		/// The object that is returned from this func will get stored in a local variable and then passed into the SetDatabaseContext action within Run() which will officially be executing within this new threads context.
		/// This is so that this new thread can inherit the database context of the parent when a database is being used with ODThread.
		/// </summary>
		public static Func<object> GetDatabaseContextParent;
		
		/// <summary>
		/// This action will get invoked at the beginning of Run() so that calling methods have a chance 
		/// to pass along the database context of the parent thread onto this new thread that was just 
		/// spawned.
		/// </summary>
		public static Action<object> SetDatabaseContextChild;
		
		/// <summary>
		/// Used to indicate if abort has been called on a thread. Used to catch ALL exceptions after a 
		/// thread has been aborted.
		/// </summary>
		private bool _wasAbortAttempted = false;

		/// <summary>
		/// Indicates if ODThread has been scheduled to quit.
		/// Check this from within a resource intensive thread periodically if you want to exit 
		/// gracefully during the course of the WorkerDelegate function.
		/// </summary>
		public bool HasQuit { get; private set; }
		
		/// <summary>
		/// Gets or sets the name of the C# thread to make it easier to find specific threads while debugging.
		/// </summary>
		public string Name
		{
			get => _thread.Name;
			set => _thread.Name = value;
		}

		/// <summary>
		/// Creates a thread that will only run once after Start() is called.
		/// </summary>
		public ODThread(WorkerDelegate worker) : this(worker, null)
		{
		}

		/// <summary>
		/// Creates a thread that will only run once after Start() is called.
		/// </summary>
		public ODThread(WorkerDelegate worker, params object[] parameters) : this(0, worker, parameters)
		{
		}

		/// <summary>
		/// Creates a thread that will continue to run the WorkerDelegate after Start() is called and will stop running once one of the quit methods has been called or the application itself is closing.
		/// timeIntervalMS (in milliseconds) determines how long the thread will wait before executing the WorkerDelegate again.
		/// Set timeIntervalMS to zero or a negative number to have the WorkerDelegate only execute once and then quit itself.
		/// </summary>
		public ODThread(int timeIntervalMS, WorkerDelegate worker, params object[] parameters)
		{
			//The very first thing to do is give the calling method a chance to pass along their current database context to this thread.
			if (GetDatabaseContextParent != null)
			{
				_databaseContext = GetDatabaseContextParent();//Store the database context from the parent to utilize within Run().
			}
			lock (_lockObj)
			{
				_listOdThreads.Add(this);
			}

			_thread = new Thread(new ThreadStart(this.Run));
			TimeIntervalMS = timeIntervalMS;
			_worker += worker;
			Parameters = parameters;
			_onInitialize?.Invoke(this);
		}

		public override string ToString() => Name;

		public void SetApartmentState(ApartmentState aptState)
		{
			_thread.SetApartmentState(aptState);
		}

		/// <summary>
		/// Starts the thread and returns immediately. If the thread is already started or has already finished, then this function will have no effect.
		/// Set isAutoCleanup to true to have this thread automatically remove itself from the global list of ODThreads once it has finished doing work.
		/// </summary>
		public void Start(bool isAutoCleanup = true)
		{
			_isAutoCleanup = isAutoCleanup;
			if (_thread.IsAlive)
			{
				return; // The thread is already running.
			}

			if (HasQuit) return; // The thread has finished.

			_thread.Start();
		}

		/// <summary>
		/// If the thread is currently waiting, this will interrupt the wait and force the thread to continue running instantly.
		/// </summary>
		public void WakeUp() => _waitEvent.Set();

		/// <summary>
		/// Typically when called from outside of ODThread, is used in conjunction with _setupHandler to delay the start of a thread.
		/// Better than using a thread sleep because it can be 'woken up' using Wakeup().
		/// </summary>
		public void Wait(int waitTimeMS)
		{
			//WaitOne is used instead of Sleep so that threads can be 'woken up' in the middle of waiting in order to process pertinent information.
			_waitEvent.WaitOne(waitTimeMS);//Mimics how Run() waits when running a thread on an interval.
		}

		/// <summary>
		/// Main thread loop that executes the WorkerDelegate and sleeps for the designated timeIntervalMS (in milliseconds) if one was set.
		/// </summary>
		private void Run()
		{
			try
			{
				// The very first thing we want to do, even before _setupHandler is invoked, is to preserve the parent thread's database context if desired.
				SetDatabaseContextChild?.Invoke(_databaseContext);

				// Now that our database context is set correctly, let the setup handler have its turn.
				_setupHandler?.Invoke(this);
			}
			catch (Exception e)
			{
				if (!WorkerExceptionHandler(e))
				{
					return;
				}
			}

			while (!HasQuit)
			{
				try
				{
					_worker(this);
				}
				catch (Exception e)
				{
					if (!WorkerExceptionHandler(e))
					{
						return;
					}
				}
				if (TimeIntervalMS > 0)
				{
					if (!HasQuit)
					{
						Wait(TimeIntervalMS);
					}
				}
				else if (TimeIntervalMS <= 0)
				{
					// Interval was set to zero or a negative number, so do work once and then quits the thread.
					HasQuit = true;
				}
			}

			_exitHandler?.Invoke(this);

			if (_isAutoCleanup)
			{
				lock (_lockObj)
				{
					_listOdThreads.Remove(this);
				}
			}
		}

		/// <summary>
		/// Calls the appropriate exception handler for this exception. Returns false if the thread needs to quit.
		/// </summary>
		private bool WorkerExceptionHandler(Exception e)
		{
			// If _wasAbortAttempted is true, the thread has been aborted so catch all types of exceptions. Before, we only caught exceptions of type 
			// ThreadAbortException. The problem with that is that while a thread cannot catch a ThreadAbortException, if another exception throws 
			// inside of a catch for a different circumstance, that exception will be thrown instead of the ThreadAbortException. To solve, catch them all.
			if (_wasAbortAttempted || e is ThreadAbortException)
			{
				//We know that a join failed by exceeding the allotted timeout.
				HasQuit = true;
				return false;
			}

			// An exception was unhandled by the worker delegate. Alert the caller if they have subscribed to this event.
			if (_exceptionHandler != null)
			{ 
				// Do not set the quit flag, just call the handler and let the thread continue living.
				try
				{
					_exceptionHandler(e);
				}
				catch (Exception ex)
				{
					//The exception handler for the thread didn't actually handle its exception. Give OnUnhandledException a chance to handle the exception.
					return HandleUnhandledExceptionOrThrow(ex);
				}
			}
			else
			{
				// Caller has not explicitly registered for this thread's exception handler so stop program execution and alert end user that something unforeseen has failed.
				return HandleUnhandledExceptionOrThrow(e);
			}

			return true;
		}

		/// <summary>
		/// If the program has registered for catching unhandled exception, that handler will be invoked.
		/// Otherwise, the exception will be thrown which will hard crash the program.
		/// </summary>
		private bool HandleUnhandledExceptionOrThrow(Exception e)
		{
			// In this case it is safe to quit this thread because the application is probably about to either hard crash or exit gracefully.
			HasQuit = true;
			if (OnUnhandledException != null)
			{
				OnUnhandledException(e, _thread);
				return false;
			}

			// If we get here the entire program will shutdown without warning and only leave a vague reference to KERNELBASE.dll in the event viewer.
			throw e;
		}

		/// <summary>
		/// Forces the calling thread to synchronously wait for the current thread to finish doing work.
		/// Pass Timeout.Infinite into timeoutMS if you wish to wait as long as necessary for the thread to join.
		/// The thread will be aborted if the timeout was reached and then will return false.
		/// </summary>
		public bool Join(int timeoutMS)
		{
			if (_thread.ThreadState == ThreadState.Unstarted) return true; // Thread has not even started yet to we cannot join.

			bool hasJoined = _thread.Join(timeoutMS);
			if (!hasJoined)
			{
				// The timeout expired and the thread is still alive so we want to abort it manually.
				// Abort exceptions will be swallowed within Run()
				// wasAbortAttempted should be set first as it is being set on the main thread. If abort is quick, it may not be set in time.
				_wasAbortAttempted = true;
				_thread.Abort();
			}

			return hasJoined;
		}

		/// <summary>
		/// Raises onExit when all thread's from the given groupName have exited. Returns immediately.
		/// </summary>
		public static void AddGroupNameExitHandler(string groupName, EventHandler onExit)
		{
			new ODThread(new WorkerDelegate((ODThread thread) =>
			{
				JoinThreadsByGroupName(Timeout.Infinite, groupName, false);
				onExit(groupName, new EventArgs());
			})).Start(true);
		}

		/// <summary>
		/// Synchronously waits for all threads in the specified group to finish doing work.
		/// Pass Timeout.Infinite into timeoutMS if you wish to wait as long as necessary for all threads to join.
		/// Set doRemoveThreads to true to remove all threads from the global list of threads.
		/// </summary>
		public static void JoinThreadsByGroupName(int timeoutMS, string groupName, bool doRemoveThreads = false)
		{
			var groupThreads = GetThreadsByGroupName(groupName);
			foreach (var thread in groupThreads)
            {
				thread.Join(timeoutMS);
            }

			if (doRemoveThreads) // Remove all threads from the global list of ODThreads.
			{
				foreach (var thread in groupThreads)
                {
					thread.QuitAsync();
                }
			}
		}

		/// <summary>
		/// Immediately returns after flagging the thread to quit itself asynchronously.
		/// The thread may execute a bit longer.
		/// If the thread has been forgotten, it will be forcefully quit on closing of the main application.
		/// </summary>
		public void QuitAsync() => QuitAsync(true);

		/// <summary>
		/// Immediately returns after flagging the thread to quit itself asynchronously.
		/// The thread may execute a bit longer.
		/// If the thread has been forgotten, it will be forcefully quit on closing of the main application.
		/// Set removeThread false if you want this thread to stay in the global list of ODThreads.
		/// </summary>
		public void QuitAsync(bool removeThread)
		{
			HasQuit = true;

			// If thread is in idle due to wait event, then wake it immediately so we can more quickly quit.  Helps the thread quit within timeoutMS.
			WakeUp();
			if (removeThread)
			{
				lock (_lockObj)
				{
					_listOdThreads.Remove(this);
				}
			}
		}

		/// <summary>
		/// Waits for this thread to quit itself before returning.
		/// If the thread has been forgotten, it will be forcefully quit on closing of the main application.
		/// </summary>
		public void QuitSync(int timeoutMS)
		{
			HasQuit = true;

			//If thread is in waiting on wait event, wake it can quit gracefully.
			WakeUp();
			try
			{
				Join(timeoutMS);//Wait for allotted time before throwing ThreadAbortException.
			}
			catch
			{
				//Guards against re-entrance into this function just in case the main thread called QuitSyncAllOdThreads() and this thread itself called QuitSync() at the same time.
				//This will be very rare and if we get to this point, we know that the thread has already been joined or aborted and thus has already finished doing work so it is fine to remove.
			}
			finally
			{
				lock (_lockObj)
				{
					_listOdThreads.Remove(this);
				}
			}
		}

		/// <summary>
		/// Asynchronously quits all threads that have the passed in group name.
		/// Optionally have this quit method remove the threads from the global list of threads.
		/// </summary>
		public static void QuitAsyncThreadsByGroupName(string groupName, bool doRemoveThreads = false)
		{
			var groupThreads = GetThreadsByGroupName(groupName).ToList();

			foreach (var thread in groupThreads)
            {
				thread.QuitAsync(doRemoveThreads);
            }
		}

		/// <summary>
		/// Waits for ALL threads in the group to finish doing work before returning.
		/// Each thread will be given the timeoutMS to quit.
		/// Try to keep in mind how many threads are going to be quitting when setting the milliseconds for the timeout.
		/// If the thread has been forgotten, it will be forcefully quit on closing of the main application.
		/// Removes all threads from the global list of ODThreads after the threads have quit.
		/// </summary>
		public static void QuitSyncThreadsByGroupName(int timeoutMS, string groupName = "")
		{
			QuitAsyncThreadsByGroupName(groupName);

			// Wait for all threads to end or timeout, whichever comes first.
			JoinThreadsByGroupName(timeoutMS, groupName, true);
		}

		/// <summary>
		/// Returns the specified group of threads in the same order they were created.
		/// If groupName is empty, then returns the list of all current ODThreads.
		/// </summary>
		public static IEnumerable<ODThread> GetThreadsByGroupName(string groupName)
		{
			lock (_lockObj)
            {
				foreach (var thread in _listOdThreads)
                {
					if (thread.GroupName.Equals(groupName, StringComparison.InvariantCultureIgnoreCase))
						yield return thread;
                }
            }
		}

		/// <summary>
		/// Add an exception handler to be alerted of unhandled exceptions in the work delegate.
		/// </summary>
		public void AddExceptionHandler(ExceptionDelegate exceptionHandler) 
			=> _exceptionHandler += exceptionHandler;

		/// <summary>
		/// Add an exit handler that will get fired once the thread loop has exited.
		/// Fires in the context of this thread not the context of the calling / creating thread.
		/// Make sure to use Invoke or BeginInvoke if you are going to be manipulating UI elements from this handler.
		/// </summary>
		public void AddExitHandler(WorkerDelegate exitHandler) 
			=> _exitHandler += exitHandler;

		/// <summary>
		/// Add a delegate that will get called before the main worker delegate starts.
		/// If this is a thread that runs repeatedly at an interval, this delegate will only run before the first time the thread is run. 
		/// It is implied that this is invoked from within the thread context.
		/// Make sure to use Invoke or BeginInvoke if you are going to be manipulating UI elements from this handler.
		/// </summary>
		public void AddSetupHandler(WorkerDelegate setupHandler) 
			=> _setupHandler += setupHandler;


		///<summary>Spread the given actions over the given numThreads. Blocks until threads have completed or timeout is reached.
		///If numThreads is not provided then numThreads will default to Environment.ProcessorCount. This is typically what you should let happen.
		///If onException is provided then one and only one onException event will be raised when any number of exceptions occur.
		///All actions will run to completion regardless if any/all throw unhandled exceptions.
		///If the timeout is reached, all threads will be killed and their corresponding actions will not complete.  This can leave data in an 
		///undefined state, for example, if an action times out before instantiating an object, the object will be null.
		///Throws exception on main thread if any action throws and unhandled exception and no onException was provided.</summary>
		public static void RunParallel(List<Action> listActions, TimeSpan timeout, int numThreads = 0, ExceptionDelegate exceptionHandler = null)
		{
			RunParallel(listActions, (int)timeout.TotalMilliseconds, numThreads, exceptionHandler);
		}

		///<summary>Spread the given actions over the given numThreads. Blocks until threads have completed or timeout is reached.
		///If numThreads is not provided then numThreads will default to Environment.ProcessorCount. This is typically what you should let happen.
		///If onException is provided then one and only one onException event will be raised when any number of exceptions occur.
		///All actions will run to completion regardless if any/all throw unhandled exceptions.
		///If the timeout is reached, all threads will be killed and their corresponding actions will not complete.  This can leave data in an 
		///undefined state, for example, if an action times out before instantiating an object, the object will be null.
		///Throws exception on main thread if any action throws and unhandled exception and no onException was provided.</summary>
		public static void RunParallel(List<Action> actions, int timeoutMS = Timeout.Infinite, int maxThreads = 0, ExceptionDelegate exceptionHandler = null)
		{
			if (actions == null || actions.Count == 0) return;

			var threadGroup = "ODThread-" + (Guid.NewGuid().ToString());
			var threadActions = new List<Action>();
			var threadId = 1;
			var threadSyncLock = new object();

			if (maxThreads <= 0)
            {
				maxThreads = Environment.ProcessorCount;
				if (maxThreads < 8)
                {
					maxThreads = 8;
				}
			}

			if (maxThreads > actions.Count) maxThreads = actions.Count;

			static void ExecuteThreadActions(ODThread thread)
            {
				if (thread.Tag is List<Action> actions)
                {
					foreach (var action in actions)
                    {
						action?.Invoke();
                    }
                }
            }

			Exception firstUnhandledException = null;

			void DispatchActions()
            {
				if (threadActions.Count > 0)
				{
					var thread = new ODThread(ExecuteThreadActions)
					{
						Tag = new List<Action>(threadActions),
						Name = threadGroup + "-" + threadId,
						GroupName = threadGroup
					};

					thread.AddExceptionHandler(exception =>
					{
						lock (threadSyncLock)
						{
							firstUnhandledException ??= exception;
						}
					});

					threadId++;
				}

				threadActions.Clear();
			}

			var maxActionsPerThread = (int)Math.Ceiling((double)actions.Count / maxThreads);

			foreach (var action in actions)
            {
				threadActions.Add(action);
				if (threadActions.Count == maxActionsPerThread)
                {
					DispatchActions();
                }
            }

			if (threadActions.Count > 0) DispatchActions();

			// Wait for all threads to finish.
			JoinThreadsByGroupName(timeoutMS, threadGroup, true);

			// Did one of the actions throw an unhandled exception?
			if (firstUnhandledException != null)
			{
				if (exceptionHandler == null)
				{
					ExceptionDispatchInfo.Capture(firstUnhandledException).Throw();
				}
				else
				{
					exceptionHandler(firstUnhandledException);
				}
			}
		}

		/// <summary>
		/// Pointer delegate to the method that does the work for this thread.
		/// The worker method has to take an ODThread as a parameter so that it has access to Tag and other variables when needed.
		/// </summary>
		public delegate void WorkerDelegate(ODThread odThread);

		/// <summary>
		/// Pointer delegate to the method that gets called when the worker delegate throws an unhandled exception.
		/// </summary>
		public delegate void ExceptionDelegate(Exception e);

		/// <summary>
		/// Program entry of any application using ODThread call this method and provide the Application.Run's form/control.
		/// Any unhandled exception originating from an ODThread will be passed along through this handler.
		/// The handler instance is responsible for joining back to the main thread, reporting the error, and exiting the program. 
		/// Failing to register here in your application will result in unhandled exceptions in ODThread killing your program without any on-screen feedback and
		/// a vague event blaming KERNELBASE.dll will be posted to the event viewer.
		/// </summary>
		public static void RegisterForUnhandledExceptions(Control mainThreadControl = null, Action<Exception, string> exceptionHandler = null)
		{
			OnUnhandledException = new Action<Exception, Thread>((e, thread) =>
			{
				if (mainThreadControl == null)
				{
					exceptionHandler?.Invoke(e, thread.Name);
					return;
				}

				// BeginInvoke allows the thread to continue. Simply using Invoke() would cause a thread dead-lock.
				mainThreadControl.BeginInvoke(() =>
				{
					// We are back on the main thread and the offending ODThread is now continuing and exiting so it is safe to join here.
					thread.Join();
					if (exceptionHandler != null)
					{ 
						// Registered application would like to handle this on their own.
						exceptionHandler(e, thread.Name);
						return;
					}

					// Guaranteed to kill any threads which are still running. 999 means 'Unknown Problem' per the OD manual.
					Application.Exit();
				});
			});
		}
	}
}
