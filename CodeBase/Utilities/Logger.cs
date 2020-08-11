using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace CodeBase
{
    /// <summary>
    /// Identifies the importance of a log message.
    /// </summary> 
    public enum LogLevel
	{
		Verbose,
		Trace,
		Debug,
		Info,
		Warning,
		Error,
		FatalError,
	};

	public static class Logger
	{
		private static readonly string logDirectory =
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Imedisoft", "Logs");

		private static readonly object logSyncLock = 
			new object();

		public const string DateFormat = "yyyy-MM-dd";
		public const string DateTimeFormat = "yyyy-MM-dd hh:mm:ss";

		/// <summary>
		/// Creates the logs directory if it doesn't exist.
		/// </summary>
		static Logger()
		{
			try
			{
				if (!Directory.Exists(logDirectory))
				{
					Directory.CreateDirectory(logDirectory);
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// Gets or sets the minimum logging level.
		/// </summary>
#if DEBUG
		public static LogLevel MinLogLevel { get; set; } = LogLevel.Verbose;
#else
		public static LogLevel MinLogLevel { get; set; } = LogLevel.Warning;
#endif

		/// <summary>
		/// Convert a severity code into a string.
		/// </summary>
		/// <param name="logLevel">The logging level.</param>
		public static string LogLevelString(LogLevel logLevel)
		{
			switch (logLevel)
			{
				case LogLevel.Verbose: return "VERBOSE";
				case LogLevel.Trace: return "TRACE";
				case LogLevel.Debug: return "DEBUG";
				case LogLevel.Info: return "INFO";
				case LogLevel.Warning: return "WARNING";
				case LogLevel.Error: return "ERROR";
				case LogLevel.FatalError: return "FATAL ERROR";
			}

			return "UNKNOWN SEVERITY";
		}

		/// <summary>
		/// Writes the specified message to the log file.
		/// </summary>
		/// <param name="logLevel">The log message severity.</param>
		/// <param name="message">The log message.</param>
		/// <param name="args">An array of objects to format.</param>
		public static void Write(LogLevel logLevel, string message, params object[] args)
        {
			if (logLevel < MinLogLevel) return;

			var path = Path.Combine(logDirectory, string.Concat(DateTime.Now.ToString(DateFormat), "-Core", ".txt"));

			lock (logSyncLock)
			{
				using (var stream = File.Open(path, FileMode.Append, FileAccess.Write, FileShare.Read))
				using (var streamWriter = new StreamWriter(stream))
				{
					streamWriter.Write(
						$"[{DateTime.Now.ToString(DateTimeFormat)}]" +
						$"[{System.Diagnostics.Process.GetCurrentProcess().Id}]" +
						$"[{LogLevelString(logLevel)}] ");

					streamWriter.WriteLine(string.Format(message, args));
					streamWriter.Flush();
				}
			}
        }

		/// <summary>
		/// Writes a exception to the log file.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		public static void LogException(Exception exception) 
			=> LogError(
				$"{exception.Message}\r\n" +
				$"{exception.StackTrace}");

		public static void LogVerbose(string message, params object[] args)
			=> Write(LogLevel.Verbose, message, args);

		public static void LogTrace(string message, params object[] args)
			=> Write(LogLevel.Trace, message, args);

		public static void LogDebug(string message, params object[] args)
			=> Write(LogLevel.Debug, message, args);

		public static void LogInfo(string message, params object[] args)
			=> Write(LogLevel.Info, message, args);

		public static void LogWarning(string message, params object[] args)
			=> Write(LogLevel.Warning, message, args);

		public static void LogError(string message, params object[] args)
			=> Write(LogLevel.Error, message, args);

		public static void LogFatalError(string message, params object[] args)
			=> Write(LogLevel.FatalError, message, args);

		/// <summary>
		/// Executes the specified action and writes a log entry if a error occurs.
		/// </summary>
		/// <param name="logLevel">The log message severity.</param>
		/// <param name="methodName">The name of the calling method.</param>
		/// <param name="action">The action the execute.</param>
		public static void LogAction(string methodName, Action action)
        {
			LogTrace($"{methodName} START");

			try
            {
				action();
            }
            catch (Exception exception)
            {
				LogError(exception.Message);
            }
            finally
            {
				LogTrace($"{methodName} END");
			}
        }
	}
}
