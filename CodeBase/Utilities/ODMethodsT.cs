namespace CodeBase
{
    public class ODMethodsT
	{
		/// <summary>
		/// Returns a new instance of T if input is null.
		/// </summary>
		public static T Coalesce<T>(T input) where T : new()
		{
			if (input != null)
			{
				return input;
			}
			return new T();
		}
	}
}
