using System;

namespace CodeBase
{
	/// <summary>
	/// This class is useful when testing in order to mock DateTime.Now.
	/// In the real code, substitute DateTime.Now with DateTime_.Now, andin the tests, override DateTime_GetNow to change the current time.
	/// </summary>
	public static class DateTime_
	{
		/// <summary>
		/// Set this func to return a custom value for DateTime_.Now. 
		/// If not overridden, will return the real DateTime.Now.
		/// </summary>
		private static Func<DateTime> _getNow = () => DateTime.Now;

		/// <summary>
		/// True if DateTime_.Now will return a custom value.
		/// </summary>
		public static bool IsNowModified
		{
			get; private set;
		}

		/// <summary>
		/// The Now time based on DateTime_.GetNow.
		/// </summary>
		public static DateTime Now => _getNow();

		/// <summary>
		/// The Today date based on DateTime_.GetNow.
		/// </summary>
		public static DateTime Today => _getNow().Date;
	}
}
