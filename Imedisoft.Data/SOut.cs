using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DataConnectionBase
{
    /// <summary>
    /// Converts various datatypes into strings formatted correctly for MySQL. 
    /// </summary>
    public class SOut
	{
		public static string Bool(bool value) 
			=> value ? "1" : "0";

		public static string Byte(byte value) 
			=> value.ToString();

		/// <summary>
		/// Always encapsulates the result, depending on the current database connection.
		/// </summary>
		public static string DateT(DateTime value, bool encapsulate = true)
		{
			if (value.Year < 1880) value = DateTime.MinValue;

			try
			{
				string result = value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);//new DateTimeFormatInfo());
				string frontCap = "'";
				string backCap = "'";

				if (encapsulate)
				{
					result = frontCap + result + backCap;
				}

				return result;
			}
			catch
			{
				return "";
			}
		}

		/// <summary>
		/// Converts a date to yyyy-MM-dd format which is the format required by MySQL.
		/// myDate is the date you want to convert. 
		/// encapsulate is true for the first overload, making the result look like this: 'yyyy-MM-dd' for MySQL.
		/// </summary>
		public static string Date(DateTime value, bool encapsulate = true)
		{
			try
			{
				string result = value.ToString("yyyy-MM-dd", new DateTimeFormatInfo());
				string frontCap = "'";
				string backCap = "'";

				if (encapsulate)
				{
					result = frontCap + result + backCap;
				}

				return result;
			}
			catch
			{
				return "";
			}
		}

		/// <summary>
		/// Timespans that might be invalid time of day.
		/// Can be + or - and can be up to 800+ hours.
		/// Never encapsulates
		/// </summary>
		public static string TSpan(TimeSpan value)
		{
			if (value == TimeSpan.Zero) return "00:00:00"; ;

			try
			{
				string result = "";
				if (value < TimeSpan.Zero)
				{
					result += "-";
					value = value.Duration();
				}

				int hours = (value.Days * 24) + value.Hours;
				
				result += hours.ToString().PadLeft(2, '0') + ":" + value.Minutes.ToString().PadLeft(2, '0') + ":" + value.Seconds.ToString().PadLeft(2, '0');
				return result;
			}
			catch 
			{
				return "00:00:00";
			}
		}

		/// <summary>
		/// Timespans that are guaranteed to always be a valid time of day.
		/// No negatives or hours over 24.  Stored in Oracle as datetime.
		/// Encapsulated by default.
		/// </summary>
		public static string Time(TimeSpan value, bool encapsulate = true)
		{
			string retval =
				value.Hours.ToString().PadLeft(2, '0') + ":" +
				value.Minutes.ToString().PadLeft(2, '0') + ":" +
				value.Seconds.ToString().PadLeft(2, '0');

			if (encapsulate)
			{
				return "'" + retval + "'";
			}
			else
			{
				return retval;
			}
		}

		/// <summary>
		/// By default, rounds input up to max of 2 decimal places. EG: .0047 will return "0.00"; .0051 will return "0.01".
		/// Set doRounding false when the double passed in needs to be Multiple Precision Floating-Point Reliable (MPFR).
		/// Set doUseEnUSFormat to true to use a period no matter what region.
		/// </summary>
		public static string Double(double value, bool doRounding = true, bool doUseEnUSFormat = false)
		{
			try
			{
				if (doRounding)
				{
					// Because decimal is a comma in Europe, this sends it to db with period instead 
					return value.ToString("f", CultureInfo.InvariantCulture);
				}
				else if (doUseEnUSFormat)
				{
                    var numberFormatInfo = new NumberFormatInfo
                    {
                        NumberDecimalSeparator = ".",
                        NumberGroupSeparator = ","
                    };

                    return value.ToString(numberFormatInfo);
				}

				// This will send the double to the database with a comma for some countries.  E.g. Europe uses commas instead of periods.
				return value.ToString();
			}
			catch
			{
				return "0";
			}
		}

		/// <summary>
		/// Rounds input up to max of 2 decimal places.
		/// EG: .0047 will return "0.00"; .0051 will return "0.01".
		/// </summary>
		public static string Decimal(decimal value)
		{
			try
			{
				// because decimal is a comma in Europe, this sends it to db with period instead 
				return value.ToString("f", CultureInfo.InvariantCulture);
			}
			catch
			{
				return "0";
			}
		}

		public static string Long(long value) 
			=> value.ToString();

		public static string Int(int value) 
			=> value.ToString();

		public static string Enum<T>(T value) where T : Enum 
			=> Int((int)(object)value);

		public static string Float(float value) 
			=> value.ToString(CultureInfo.InvariantCulture); // sends as comma in Europe.  (comes back from mysql later as a period)

		public static string String(string value)
		{
			if (value == null)
			{
				return "";
			}

			value = StringScrub(value);

			var stringBuilder = new StringBuilder();
			for (int i = 0; i < value.Length; i++)
			{
				switch (value.Substring(i, 1))
				{
					// note. When using binary data, must escape ',",\, and nul(? haven't done nul)
					// _ and % are special characters in LIKE clauses.  But they need not be escaped.  Only a potential problem when using LIKE.

					case "'":  stringBuilder.Append(@"\'");  break; // ' replaced by \'
					case "\"": stringBuilder.Append("\\\""); break; // " replaced by \"
					case @"\": stringBuilder.Append(@"\\");  break; // single \ replaced by \\
					case "\r": stringBuilder.Append(@"\r");  break; // carriage return(usually followed by new line)
					case "\n": stringBuilder.Append(@"\n");  break; // new line
					case "\t": stringBuilder.Append(@"\t");  break; // tab

					default: 
						stringBuilder.Append(value.Substring(i, 1)); break;
				}
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Should never be used outside of the crud.
		/// Used for large columns (i.e. text, mediumtext, longtext) where it is possible to enter too many consecutive new line characters for the windows control to draw.
		/// This can cause a graphics memory error. 
		/// If there are more than 50 consecutive new line characters, this will replace them with a single new line.
		/// It will do the same with tabs.
		/// Any null characters will also removed.
		/// </summary>
		/// <param name="doEscapeCharacters">
		/// Only needs to be true when using parameters to construct the query. When true, will call POut.String.
		/// </param>
		public static string StringNote(string value, bool doEscapeCharacters = false)
		{
			if (value == null) return "";

			value = StringScrub(value);
			value = value.Replace("\r\n", "\n");
			value = value.Replace("\r", "\n");
			value = Regex.Replace(value, @"[\s]{100,}", " ");//{100,} means 100 or more. \s is any whitespace.
			value = Regex.Replace(value, @"[\0]", "");//take out null character. Any other characters that cause problems should be added here.
			value = value.Replace("\n", "\r\n");

			if (doEscapeCharacters)
			{
				return String(value);
			}

			return value;
		}

		/// <summary>
		/// Should never be used outside of the crud.
		/// Scrubs unwanted unicode symbols from SQL parameters.
		/// </summary>
		public static string StringParam(string value)
		{
			if (value == null) return "";

			return StringScrub(value);
		}

		public static string Bitmap(System.Drawing.Bitmap bitmap, ImageFormat imageFormat)
		{
			if (bitmap == null) return "";

			using (var memoryStream = new MemoryStream())
			{
				bitmap.Save(memoryStream, imageFormat);//was Bmp, then Png, then user defined.  So there will be a mix of different kinds.
				byte[] rawData = memoryStream.ToArray();

				return Convert.ToBase64String(rawData);
			}
		}

		/// <summary>
		/// Converts the specified wav file into a string representation.
		/// The timing of this is a little different than with the other "P" functions and is only used by the import button in FormSigElementDefEdit.
		/// After that, the wav spends the rest of it's life as a string until "played" or exported.
		/// </summary>
		public static string Sound(string filename)
		{
			if (!File.Exists(filename))
			{
				throw new ApplicationException("File does not exist.");
			}

			if (!filename.ToLower().EndsWith(".wav"))
			{
				throw new ApplicationException("Filename must end with .wav");
			}

			FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			byte[] rawData = new byte[stream.Length];
			stream.Read(rawData, 0, (int)stream.Length);
			return Convert.ToBase64String(rawData);
		}

		/// <summary>
		/// Removes unsupported UTF-8 characters from the string so that inserting into the database can preserve as much of the string as possible.
		/// </summary>
		private static string StringScrub(string value)
		{
			if (value == null) return "";

			// Return whatever was passed in if only white space was passed in to preserve old behavior (e.g. myString set to many tabs in a row).
			if (string.IsNullOrWhiteSpace(value)) return value;

			// Had an issue with emojis in text described in TaskNum #1196599. Any text after an emoji would be truncated when saved to db.
			// The following regular expressions check the entered text for any characters falling within specific unicode ranges and removes them.
			// Hex codes \u2600 to \u26FF is the range for Miscellaneous Symbols.
			// Hex codes \u2700 to \u27BF is the range for Dingbats. (there are no hex codes inbetween Misc Symbols and Dingbats)
			// Hex codes \u27C0 to \u27EF is the range for Miscellaneous Mathementical Symbols-A
			// Hex codes \u27F0 to \u27FF is the range for Supplemental Arrows-A
			value = Regex.Replace(value, @"[\u2600-\u27FF]", "\uFFFD");

			// Hex code \uFFFD is the "replacement character" 
			// (The replacement character is the diamond with a ? in it to denote a symbol was entered that the program doesn't know how to interpret)
			// myString=Regex.Replace(myString,@"\uFFFD","");
			// The following regular expressions are for unicode symbols that are surrogate pairs
			// Documentation for surrogates in unicode: https://goo.gl/H9tCKq (MSDN docs link shortened with Google URL shortener)
			// Surrogate pair, D83C points to a set of unicode symbols, [\uDC00-\uDFFF] is a range of symbols within that set
			// The D83C surrogate range loosely translates to hex codes \u1F000 to \u1F3FF which covers the following unicode blocks:
			// Mahjong Tiles, Domino Tiles, Playing Cards, Enclosed Alphanumeric Supplement, Enclosed Ideographic Supplement
			// and half of Miscellaneous Symbols And Pictographs
			// The D83D surrogate range loosely translates to hex codes \u1F400 to \u1F7FF which covers the following unicode blocks:
			// 2nd half of Miscellaneous Symbols And Pictographs, Emoticons, Ornamental Dingbats, Transport And Map Symbols,
			// Alchemical Symbols and Geometric Shapes Extended
			// The D83E surrogate range loosely translates to hex codes \u1F910 to \u1F9C0 which covers the Supplemental Symbols And Pictographs blocks:
			value = Regex.Replace(value, @"\uD83C[\uDC00-\uDFFF]", "\uFFFD");
			value = Regex.Replace(value, @"\uD83D[\uDC00-\uDFFF]", "\uFFFD");
			value = Regex.Replace(value, @"\uD83E[\uDD10-\uDDC0]", "\uFFFD");

			return value;
		}

		/// <summary>
		/// Returns true if some characters would be stripped out of str before appending to a query.
		/// </summary>
		public static bool HasInjectionChars(string str) => str != String(str);
	}
}
