using System;

namespace CodeBase
{
    public abstract class ODThreadAbs : IODThread
	{
        public ODThread ODThread { get; set; } = null;

        public bool IsInit { get; set; }

        /// <summary>
        /// The default level at which this thread should log.
        /// </summary>
        public LogLevel LogLevelThread { get; set; }

        /// <summary>
        /// The directory in \OpenDental\OpenDentalService\Logger that holds the log files that are written to by CodeBase.Logger.cs
        /// </summary>
        public abstract string GetLogDirectoryName();

		/// <summary>
		/// The name of the thread for debugging/development purposes.
		/// </summary>
		public abstract string GetThreadName();

		/// <summary>
		/// The interval of time in milliseconds to wait between calling OnThreadRun
		/// </summary>
		public abstract int GetThreadRunIntervalMS();

		/// <summary>
		/// What the thread does every time the specified interval of time has passed.
		/// </summary>
		public abstract void OnThreadRun(ODThread odThread);

		/// <summary>
		/// What the thread does when it catches an unhandled exception.
		/// </summary>
		public void OnThreadException(Exception e)
		{
			Logger.WriteException(e, GetLogDirectoryName());
		}

		/// <summary>
		/// What the thread does right before it dies.
		/// </summary>
		public virtual void OnThreadExit(ODThread odThread)
		{
		}

		/// <summary>
		/// Asynchronously stops the thread.  Guards against re-entrance.
		/// </summary>
		public void Stop()
		{
			if (ODThread != null)
			{
				ODThread.QuitAsync();
			}
		}
	}
}
