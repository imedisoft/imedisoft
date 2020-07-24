using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeBase
{
    public static class ODPrimitiveExtensions
	{
		/// <summary>
		/// Used to check if a decimal number is "equal" to zero based on some epsilon. 
		/// Epsilon is 0.0000001M and will return true if the absolute value of the decimal is less than that.
		/// </summary>
		public static bool IsZero(this decimal val) 
			=> Math.Abs(val) <= 0.0000001M;

		/// <summary>
		/// Used to check if a floating point number is "equal" to zero based on some epsilon. 
		/// Epsilon is 0.0000001f and will return true if the absolute value of the double is less than that.
		/// </summary>
		public static bool IsZero(this double val) 
			=> Math.Abs(val) <= 0.0000001f;

		public static bool IsEqual(this double val, double val2) 
			=> IsZero(val - val2);

		/// <summary>
		/// Used to check if a double is "less than" another double based on some epsilon.
		/// </summary>
		public static bool IsLessThan(this double val, double val2) 
			=> val2 - val > 0.0000001f;

		/// <summary>
		/// Used to check if a double is "greater than" another double based on some epsilon.
		/// </summary>
		public static bool IsGreaterThan(this double val, double val2) 
			=> val - val2 > 0.0000001f;

		/// <summary>
		/// Used to check if a decimal number is "less than" zero based on some epsilon.
		/// </summary>
		public static bool IsLessThanZero(this decimal val) 
			=> val < -0.0000001M;

		/// <summary>
		/// Used to check if a double number is "less than" zero based on some epsilon.
		/// </summary>
		public static bool IsLessThanZero(this double val) 
			=> val < -0.0000001f;

		/// <summary>
		/// Used to check if a double number is "less than" zero based on some epsilon.
		/// </summary>
		public static bool IsLessThanOrEqualToZero(this double val) 
			=> val < -0.0000001f || Math.Abs(val) <= 0.0000001f;

		/// <summary>
		/// Used to check if a decimal number is "less than" zero based on some epsilon.
		/// </summary>
		public static bool IsLessThanOrEqualToZero(this decimal val) 
			=> val < -0.0000001M || Math.Abs(val) <= 0.0000001M;

		/// <summary>
		/// Used to check if a double number is "greater than" zero based on some epsilon.
		/// </summary>
		public static bool IsGreaterThanOrEqualToZero(this double val) 
			=> val > 0.0000001f || Math.Abs(val) <= 0.0000001f;

		/// <summary>
		/// Used to check if a decimal number is "greater than" zero based on some epsilon.
		/// </summary>
		public static bool IsGreaterThanOrEqualToZero(this decimal val) 
			=> val > 0.0000001M || Math.Abs(val) <= 0.0000001M;

		/// <summary>
		/// Used to check if a decimal is "greater than" another decimal based on some epsilon.
		/// </summary>
		public static bool IsGreaterThan(this decimal val, decimal val2) 
			=> val - val2 > 0.0000001M;

		/// <summary>
		/// Used to check if a decimal number is "greater than" zero based on some epsilon.
		/// </summary>
		public static bool IsGreaterThanZero(this decimal val) 
			=> val > 0.0000001M;

		/// <summary>
		/// Used to check if a double number is "greater than" zero based on some epsilon.
		/// </summary>
		public static bool IsGreaterThanZero(this double val) 
			=> val > 0.0000001f;

		public static bool IsEqual(this decimal val, decimal val2) 
			=> Math.Abs(val - val2) < 0.0000001M;

		public static string Left(this string s, int maxCharacters, bool hasElipsis = false)
		{
			if (s == null || string.IsNullOrEmpty(s) || maxCharacters < 1)
			{
				return "";
			}

			if (s.Length > maxCharacters)
			{
				if (hasElipsis && maxCharacters > 4)
				{
					return s.Substring(0, maxCharacters - 3) + "...";
				}
				return s.Substring(0, maxCharacters);
			}

			return s;
		}

		public static string Right(this string s, int maxCharacters)
		{
			if (s == null || string.IsNullOrEmpty(s) || maxCharacters < 1)
			{
				return "";
			}

			if (s.Length > maxCharacters)
			{
				return s.Substring(s.Length - maxCharacters, maxCharacters);
			}

			return s;
		}

		/// <summary>
		/// Returns the Description attribute, if available. If not, returns enum.ToString().
		/// Pass it through translation after retrieving from here.
		/// </summary>
		public static string GetDescription(this Enum value, bool useShortVersionIfAvailable = false)
		{
			Type type = value.GetType();

			string name = Enum.GetName(type, value);
			if (name == null)
			{
				return value.ToString();
			}

			FieldInfo fieldInfo = type.GetField(name);
			if (fieldInfo == null)
			{
				return value.ToString();
			}

			if (useShortVersionIfAvailable)
			{
				ShortDescriptionAttribute attrShort = (ShortDescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(ShortDescriptionAttribute));
				if (attrShort != null)
				{
					return attrShort.ShortDesc;
				}
			}

			DescriptionAttribute attr = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
			if (attr == null)
			{
				return value.ToString();
			}

			return attr.Description;
		}

		/// <summary>
		/// Returns the attribute for the enum value if available. If not, returns the default value for the attribute.
		/// </summary>
		public static T GetAttributeOrDefault<T>(this Enum value) where T : Attribute, new()
		{
			Type type = value.GetType();

			string name = Enum.GetName(type, value);
			if (name == null)
			{
				return new T();
			}

			FieldInfo field = type.GetField(name);
			if (field == null)
			{
				return new T();
			}

			if (!(Attribute.GetCustomAttribute(field, typeof(T)) is T attribute))
			{
				return new T();
			}

			return attribute;
		}

		/// <summary>
		/// Returns the enum value with the passed in flags added.
		/// </summary>
		public static T AddFlag<T>(this Enum value, params T[] flags)
		{
			long valLong = Convert.ToInt64(value);
			foreach (T flagToAdd in flags)
			{
				valLong = valLong | Convert.ToInt64(flagToAdd);
			}
			return (T)Enum.ToObject(typeof(T), valLong);
		}

		/// <summary>
		/// Returns the enum value with the passed in flags removed.
		/// </summary>
		public static T RemoveFlag<T>(this Enum value, params T[] flags)
		{
			long valLong = Convert.ToInt64(value);
			foreach (T flagToRemove in flags)
			{
				valLong = valLong & ~Convert.ToInt64(flagToRemove);
			}
			return (T)Enum.ToObject(typeof(T), valLong);
		}

		/// <summary>
		/// Returns a list of flags that this enum value has.
		/// Ignores 0b0 flag if defined.
		/// </summary>
		public static IEnumerable<T> GetFlags<T>(this T value) where T : Enum
		{
			foreach (T flag in Enum.GetValues(value.GetType()).Cast<T>().Where(x => Convert.ToInt64(x) != 0))
			{
				if (value.HasFlag(flag))
				{
					yield return flag;
				}
			}
		}

		public static string ToStringDH(this TimeSpan timeSpan)
			=> string.Format("{0:%d} Days {0:%h} Hours", timeSpan);

		public static DateTime ToBeginningOfMinute(this DateTime dateTime) 
			=> new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, dateTime.Kind);

		public static DateTime ToBeginningOfMonth(this DateTime dateTime) 
			=> new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, dateTime.Kind);

		public static DateTime ToEndOfMonth(this DateTime dateTime)
			=> new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month), 23, 59, 59, dateTime.Kind);

		public static DateTime ToEndOfMinute(this DateTime dateTime)
			=> new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 59, dateTime.Kind).AddMilliseconds(999);

		/// <summary>
		/// Adds x number of week days to the given DateTime. This assumes Saturday and Sunday are the weekend days.
		/// </summary>
		/// <param name="numberOfDays">The number of week days to add.</param>
		public static DateTime AddWeekDays(this DateTime dateTime, int numberOfDays)
		{
			int numberOfDaysToAdd = 0;

			for (int i = 0; i < numberOfDays; i++)
			{
				numberOfDaysToAdd++;

				if (dateTime.AddDays(numberOfDaysToAdd).DayOfWeek == DayOfWeek.Saturday ||
					dateTime.AddDays(numberOfDaysToAdd).DayOfWeek == DayOfWeek.Sunday)
				{
					i--;
				}
			}
			dateTime = dateTime.AddDays(numberOfDaysToAdd);

			return dateTime;
		}

		/// <summary>Use regular expressions to do an in-situ string replacement. Default behavior is case insensitive.</summary>
		/// <param name="pattern">Must be a REGEX compatible pattern.</param>
		/// <param name="replacement">The string that should be used to replace each occurance of the pattern.</param>
		/// <param name="regexOptions">IgnoreCase by default, allows others.</param>
		public static void RegReplace(this StringBuilder value, string pattern, string replacement, RegexOptions regexOptions = RegexOptions.IgnoreCase)
		{
			string newVal = Regex.Replace(value.ToString(), pattern, replacement, regexOptions);
			value.Clear();
			value.Append(newVal);
		}

		/// <summary>
		/// Convert the first char in the string to upper case. The rest of the string will be lower case.
		/// </summary>
		public static string ToUpperFirstOnly(this string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			if (value.Length == 1)
			{
				return value.ToUpper();
			}
			return value.Substring(0, 1).ToUpper() + value.Substring(1, value.Length - 1).ToLower();
		}

		/// <summary>
		/// Removes all characters from the string that are not digits.
		/// </summary>
		public static string StripNonDigits(this string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			return new string(Array.FindAll(value.ToCharArray(), y => char.IsDigit(y)));
		}

		/// <summary>
		/// Adds a new line if the string is not empty and appends the addition.
		/// </summary>
		public static string AppendLine(this string value, string addition)
		{
			if (value != "")
			{
				value += "\r\n";
			}
			return value + addition;
		}

		/// <summary>
		/// When you want to specify StringSplitOptions and you just want to split using a string.
		/// </summary>
		public static string[] Split(this string stringToSplit, string separator, StringSplitOptions splitOptions)
		{
			return stringToSplit.Split(new string[] { separator }, splitOptions);
		}

		/// <summary>
		/// Returns true if the specified <see cref="IEnumerable{T}"/> is null or empty.
		/// </summary>
		/// <param name="enumerable">The enumerable.</param>
		public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> enumerable)
			=> enumerable == null || enumerable.Count() == 0;

		/// <summary>
		/// Convert to single items of type TTarge to a List of type TTarget containing 1 element: item.
		/// </summary>
		public static List<T> SingleItemToList<T>(this T self) => new List<T>() { self };

		/// <summary>
		/// Deep copy each list items of the source list to a new list instance of the target return type.
		/// Property values will be shallow-copied if specified.
		/// </summary>
		public static List<TTarget> DeepCopyList<TSource, TTarget>(this List<TSource> source, bool shallowCopyProperties = true) where TTarget : TSource
		{
			PropertyInfo[] properties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
			FieldInfo[] fields = typeof(TSource).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
			List<TTarget> ret = new List<TTarget>();
			foreach (TSource sourceObj in source)
			{
				ret.Add(sourceObj.DeepCopy<TSource, TTarget>(properties: properties, fields: fields));
			}
			return ret;
		}

		/// <summary>
		/// Deep copy of the source tiem to a new instance of the target return type.
		/// Property values will be shallow-copied if specified.
		/// </summary>
		public static TTarget DeepCopy<TSource, TTarget>(this TSource sourceObj, bool shallowCopyProperties = true, PropertyInfo[] properties = null, FieldInfo[] fields = null) where TTarget : TSource
		{
			properties = properties ?? typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
			fields = fields ?? typeof(TSource).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
			TTarget targetObj = (TTarget)Activator.CreateInstance(typeof(TTarget));
			if (shallowCopyProperties)
			{
				foreach (PropertyInfo property in properties)
				{
					try
					{
						property.SetValue(sourceObj, property.GetValue(sourceObj, null), null);
					}
					catch
					{ //For Get-only-properties.
					}
				}
			}
			foreach (FieldInfo field in fields)
			{
				field.SetValue(targetObj, field.GetValue(sourceObj));
			}
			return targetObj;
		}

		/// <summary>
		/// Allows for custom comparison of TSource. Implements IEqualityComparer, which is required by LINQ for inline comparisons.
		/// </summary>
		public class ODEqualityComparer<T> : IEqualityComparer<T>
		{
			private readonly Func<T, T, bool> predicate;

			public ODEqualityComparer(Func<T, T, bool> predicate)
			{
				this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
			}

			public bool Equals(T x, T y) => predicate(x, y);

			public int GetHashCode(T obj) => 0;
		}
	}

	public class ShortDescriptionAttribute : Attribute
	{
		public ShortDescriptionAttribute()
		{
		}

		public ShortDescriptionAttribute(string shortDesc)
		{
			ShortDesc = shortDesc;
		}

		public string ShortDesc { get; set; } = "";
	}
}
