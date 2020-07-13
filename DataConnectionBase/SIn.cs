using CodeBase;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace DataConnectionBase
{
    /// <summary>
    /// Converts strings coming in from user input into the appropriate type.
    /// </summary>
    public class SIn
	{
		public static Bitmap Bitmap(string value)
		{
			if (value == null || value.Length < 0x32) return null;

			try
			{
				byte[] rawData = Convert.FromBase64String(value);

				using (var stream = new MemoryStream(rawData))
				{
					return new Bitmap(stream);
				}
			}
			catch
			{
				return null;
			}
		}

		public static bool Bool(string value)
		{
			if (value == "" || value == "0" || value.ToLower() == "false")
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Set has exceptions to false to supress exceptions and return 0 if the input string is not an byte.
		/// </summary>
		public static byte Byte(string value, bool hasExceptions = true)
		{
			if (!string.IsNullOrEmpty(value))
			{
				try
				{
					return Convert.ToByte(value);
				}
				catch
				{
					if (hasExceptions)
					{
						throw;
					}
				}
			}

			return 0;
		}

		/// <summary>
		/// Some versions of MySQL return a GROUP_CONCAT as a string, and other versions return it as a byte array.
		/// This method handles either way, making it work smoothly with different versions.
		/// </summary>
		public static string ByteArray(object obj)
		{
			if (obj is byte[] bytes)
            {
				var stringBuilder = new StringBuilder();

				for (int i = 0; i < bytes.Length; i++)
				{
					stringBuilder.Append((char)bytes[i]);
				}

				return stringBuilder.ToString();
			}

			return obj.ToString();
		}

		/// <summary>
		/// Processes dates incoming from db that look like "4/29/2013", and dates from textboxes where 
		/// users entered and which have usually been validated.
		/// </summary>
		public static DateTime Date(string value)
		{
			if (string.IsNullOrEmpty(value)) return DateTime.MinValue;

			try
			{
				return DateTime.Parse(value);
			}
			catch
			{
				return DateTime.MinValue;
			}
		}

		[Obsolete("Use Date() instead, it is exactly the same...")]
		public static DateTime DateT(string value)
		{
			if (string.IsNullOrEmpty(value)) return DateTime.MinValue;

			try
			{
				return DateTime.Parse(value);
			}
			catch
			{
				return DateTime.MinValue;
			}
		}

		/// <summary>
		/// If blank or invalid, returns 0. Otherwise, parses.
		/// </summary>
		public static decimal Decimal(string value)
		{
			if (value == "")
			{
				return 0;
			}
			else
			{
				try
				{
					return Convert.ToDecimal(value);
				}
				catch 
				{
					return 0;
				}
			}
		}

		/// <summary>
		/// If blank or invalid, returns 0. Otherwise, parses.
		/// </summary>
		/// <param name="doUseEnUSFormat">
		/// If false, will use the computer's current settings to parse. 
		/// If true, will use the en-US setting where a "." separates the decimal portion and a "," separates groups.
		/// </param>
		public static double Double(string value, bool doUseEnUSFormat = false)
		{
			if (!string.IsNullOrEmpty(value))
			{
				try
				{
					if (doUseEnUSFormat)
					{
						var numberFormatInfo = new NumberFormatInfo
						{
							NumberDecimalSeparator = ".",
							NumberGroupSeparator = ","
						};

						return Convert.ToDouble(value, numberFormatInfo);
					}

					return Convert.ToDouble(value); // In Europe, comes in as a comma, parsed according to culture.
				}
				catch
				{
				}
			}

			return 0;
		}

		/// <summary>
		/// Set has exceptions to false to supress exceptions and return 0 if the input string is not an int.
		/// </summary>
		public static int Int(string value, bool hasExceptions = true)
		{
			if (!string.IsNullOrEmpty(value))
			{
				try
				{
					return Convert.ToInt32(value);
				}
				catch
				{
					if (hasExceptions)
					{
						throw;
					}
				}
			}

			return 0;
		}

		public static float Float(string value)
		{
			if (value == "")
			{
				return 0;
			}

			try
			{
				return Convert.ToSingle(value);
			}
			catch 
			{
				// because this will fail when getting the mysql version on startup, which always comes back with a period.
				return Convert.ToSingle(value, CultureInfo.InvariantCulture);
			}
		}

		/// <summary>
		/// Set has exceptions to false to supress exceptions and return 0 if the input string is not a long.
		/// </summary>
		public static long Long(string value, bool hasExceptions = true)
		{
			if (!string.IsNullOrEmpty(value))
			{
				try
				{
					return Convert.ToInt64(value);
				}
				catch
				{
					if (hasExceptions)
					{
						throw;
					}
				}
			}

			return 0;
		}

		/// <summary>
		/// Strongly types the value provided to the enumeration value of declared enum type (T).
		/// When isEnumAsString is false, value should be the integer value of the desired enum item.  E.g. T = ApptStatus, value = "5", retVal = ApptStatus.Broken
		/// When isEnumAsString is true, value must be the enum item name.  E.g. T = ProgramName, value = programCur.ProgName, retVal = ProgramName.Podium
		/// By default, defaultEnumOption will give you the Enum option containing a value of 0 (either the first in the set, or the first one explicitly set to 0)
		/// To default a non-0 Enum option, set defaultEnumOption accordingly.
		/// This will mainly get called to circumvent double casting. 
		/// E.g. ApptStatus stat=(ApptStatus)SIn(table["ApptStatus"].ToString());
		/// </summary>
		public static T Enum<T>(string value, bool isEnumAsString = false, T defaultEnumOption = default) where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
			{
				throw new Exception("T must be an enumeration.");
			}

			T result;
            if (isEnumAsString)
			{
				System.Enum.TryParse(value, out result);
			}
			else
			{
				int valueAsInt = Int(value);
				result = Enum(valueAsInt, defaultEnumOption);
			}

			return result;
		}

		/// <summary>
		/// Strongly types the integer provided to the enumeration value of the declared enum type (T).
		/// </summary>
		public static T Enum<T>(int value, T defaultValue = default) where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
			{
				throw new Exception("T must be an enumeration.");
			}

			T result = defaultValue;

			ODException.SwallowAnyException(() =>
			{
				if (typeof(T).IsDefined(typeof(FlagsAttribute), true))
				{
					for (int i = 1; i <= value; i *= 2)
					{
						if ((i & value) == i && !System.Enum.IsDefined(typeof(T), i))
						{
							return;
						}

						if (i - 1 == int.MaxValue)
						{
							break;
						}
					}
				}
				else
				{
					if (!System.Enum.IsDefined(typeof(T), value))
					{
						return;
					}
				}

				result = (T)System.Enum.ToObject(typeof(T), value);
			});

			return result;
		}

		/// <summary>
		/// Saves the string representation of a sound into a .wav file.
		/// The timing of this is different than with the other "P" functions, and is only used by the export button in FormSigElementDefEdit
		/// </summary>
		public static void Sound(string sound, string filename)
		{
			if (!filename.ToLower().EndsWith(".wav"))
			{
				throw new ApplicationException("Filename must end with .wav");
			}

			byte[] rawData = Convert.FromBase64String(sound);

			using (var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
			{
				fileStream.Write(rawData, 0, rawData.Length);
				fileStream.Close();
			}
		}

		/// <summary>
		/// Currently does nothing.
		/// </summary>
		public static string String(string value) => value;

		[Obsolete("Use Time() instead, it is exactly the same...")]
		public static TimeSpan TSpan(string value)
		{
			if (string.IsNullOrEmpty(value)) return TimeSpan.Zero;

			try
			{
				return TimeSpan.Parse(value);
			}
			catch
			{
				return TimeSpan.Zero;
			}
		}

		public static TimeSpan Time(string value)
		{
			if (string.IsNullOrEmpty(value)) return TimeSpan.Zero;

			try
			{
				return TimeSpan.Parse(value);
			}
			catch
			{
				return TimeSpan.Zero;
			}
		}
	}
}
