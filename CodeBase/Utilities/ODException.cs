using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace CodeBase
{
	public class ODException : ApplicationException
	{
        /// <summary>
        /// Contains query text when an ErrorCode in the 700s was thrown.
        /// 
        /// This is the query that was attempted prior to an exception.
        /// </summary>
        private readonly string query = "";

        /// <summary>
        /// Gets the error code associated to this exception.
        /// 
        /// Defaults to 0 if no error code was explicitly set.
        /// </summary>		
        public int ErrorCode { get; } = 0;

        /// <summary>
        /// Contains query text when an ErrorCode in the 700s was thrown. This is the query that was attempted prior to an exception.
        /// </summary>
        public string Query => query ?? "";

		/// <summary>
		/// Convert an int to an Enum typed ErrorCode. 
		/// Returns NotDefined if the input errorCode is not defined in ErrorCodes.
		/// </summary>		
		public static ErrorCodes GetErrorCodeAsEnum(int errorCode)
		{
			if (!Enum.IsDefined(typeof(ErrorCodes), errorCode))
			{
				return ErrorCodes.NotDefined;
			}

			return (ErrorCodes)errorCode;
		}

		/// <summary>
		/// Gets the pre-defined error code associated to this exception.  
		/// Defaults to NotDefined if the error code (int) specified is not defined in ErrorCodes enum.
		/// </summary>		
		public ErrorCodes ErrorCodeAsEnum
		{
			get
			{
				return GetErrorCodeAsEnum(ErrorCode);
			}
		}

		public ODException()
		{
		}

		public ODException(int errorCode) : this("", errorCode)
		{
		}

		public ODException(string message) : this(message, 0)
		{
		}

		public ODException(string message, ErrorCodes errorCodeAsEnum) 
			: this(message, (int)errorCodeAsEnum)
		{
		}

		public ODException(string message, int errorCode) : base(message)
		{
			ErrorCode = errorCode;
		}

		public ODException(string message, string query, Exception ex) : base(message, ex)
		{
			this.query = query;
			ErrorCode = (int)ErrorCodes.DbQueryError;
		}

		public ODException(string message, Exception ex) : base(message, ex)
		{
		}

		/// <summary>
		/// Wrap the given action in a try/catch and swallow any exceptions that are thrown. 
		/// This should be used sparingly as we typically want to handle the exception or let it bubble up to the UI but sometimes you just want to ignore it.
		/// </summary>
		public static void SwallowAnyException(Action a)
		{
			try
			{
				a();
			}
			catch
			{
			}
		}

		/// <summary>
		/// Does nothing if the exception passed in is null. 
		/// Preserves the callstack of the exception passed in.
		/// Typically used when a work thread throws an exception and we want to wait until we are back on the main thread in order to throw the exception.
		/// Calling this when there is no worker thread involved is harmless and unnecessary but will still preserve the call stack.
		/// </summary>
		public static void TryThrowPreservedCallstack(Exception ex)
		{
			if (ex == null)
			{
				return;
			}

			//We are back in the main thread context so throw a new exception which contains our actual exception (from the worker) as the innner exception.
			//Simply throwing ex here would cause the stack trace to be lost. https://stackoverflow.com/q/3403501
			throw new Exception(ex.Message, ex);
		}

		/// <summary>
		/// Predefined ODException.ErrorCode field values. 
		/// ErrorCode field is not limited to these values but this is a convenient way defined known error types.
		/// These values must be converted to/from int in order to be stored in ODException.ErrorCode.
		/// Number ranges are arbitrary but should reserve plenty of padding for the future of a given range.
		/// Each number range should share a similar prefix between all of it's elements.
		/// </summary>
		public enum ErrorCodes
		{
			/// <summary>
			/// 0 is the default. If the given (int) ErrorCode is not defined here, it will be returned at 0 - NotDefined.
			/// </summary>
			NotDefined = 0,

			OtkArgsInvalid = 200,
			MaxRequestDataExceeded = 202,
			XWebProgramProperties = 203,
			PayConnectProgramProperties = 204,

			/// <summary>
			/// DoseSpot user not authorized to perform action.
			/// </summary>
			DoseSpotNotAuthorized = 206,

			/// <summary>
			/// An API request to XWeb DTG was failed by XWeb.
			/// </summary>
			XWebDTGFailed = 207,

			/// <summary>
			/// No patient found that matches the specified parameters.
			/// </summary>
			NoPatientFound = 400,

			/// <summary>
			/// No operatories have been set up for Web Sched.
			/// </summary>
			NoOperatoriesSetup = 406,

			FormClosed = 500,

			/// <summary>
			/// After successfully logging in to Open Dental, a middle tier call to Userods.CheckUserAndPassword returned an 
			/// "Invalid user or password" error.
			/// </summary>
			CheckUserAndPasswordFailed = 600,

			/// <summary>
			/// Generic database command failed to execute.
			/// </summary>
			DbQueryError = 700,

			/// <summary>
			/// Error occurred when attempting to archive old claims.
			/// </summary>
			ClaimArchiveFailed = 4002,
		}
	}
}
