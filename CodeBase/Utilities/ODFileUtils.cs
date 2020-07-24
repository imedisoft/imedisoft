using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace CodeBase
{
	public class ODFileUtils
	{
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreebytesAvailable, out ulong lpTotalNumberOfBytes, out ulong lpTotalNumberOfFreeBytes);

		/// <summary>
		/// This is a class scope variable in order to ensure that the random value is only seeded once for each time OD is launched.
		/// Otherwise, if instantiated more often, then the same random numbers are generated over and over again.
		/// </summary>
		private static readonly Random _rand = new Random();

		/// <summary>
		/// Removes a trailing path separator from the given string if one exists.
		/// </summary>
		public static string RemoveTrailingSeparators(string path)
		{
			while (path != null && path.Length > 0 && (path[path.Length - 1] == '\\' || path[path.Length - 1] == '/'))
			{
				path = path.Substring(0, path.Length - 1);
			}

			return path;
		}

		/// <summary>
		/// OS independent path cominations. Ensures that each of the given path pieces are separated by the correct path 
		/// separator for the current operating system. There is guaranteed not to be a trailing path separator at the 
		/// end of the returned string (to accomodate file paths), unless the last specified path piece in the array is 
		/// the empty string.
		/// </summary>
		public static string CombinePaths(params string[] paths) 
			=> Path.Combine(paths);

		/// <summary>
		/// This function takes a valid folder path. 
		/// Accepts UNC paths as well. freeBytesAvail will contain the free space in bytes of the drive containing the folder.
		/// It returns false if the function fails.
		/// </summary>
		public static bool GetDiskFreeSpace(string folder, out ulong freeBytesAvail)
		{
            if (!folder.EndsWith("\\")) folder += "\\";

			return GetDiskFreeSpaceEx(folder, out freeBytesAvail, out _, out _);
        }

		/// <summary>
		/// Creates a new randomly named file in the given directory path with the given extension and returns the full path to the new file.
		/// The file name will include the local date and time down to the second.
		/// </summary>
		public static string CreateRandomFile(string dir, string ext, string prefix = "")
		{
			if (ext.Length > 0 && ext[0] != '.')
			{
				ext = '.' + ext;
			}
			bool fileCreated = false;
			const string randChrs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			string filePath;
			do
			{
				string fileName = prefix;
				for (int i = 0; i < 6; i++)
				{
					fileName += randChrs[_rand.Next(0, randChrs.Length - 1)];
				}

				fileName += DateTime.Now.ToString("yyyyMMddhhmmss");
				filePath = CombinePaths(dir, fileName + ext);

				try
				{
					FileStream fs = File.Create(filePath);
					fs.Dispose();
					fileCreated = true;
				}
				catch
				{
				}

			} while (!fileCreated);

			return filePath;
		}

		/// <summary>
		/// Throws exceptions when there are permission issues. 
		/// Creates a new randomly named subdirectory inside the given directory path and returns the full path to the new subfolder.
		/// </summary>
		public static string CreateRandomFolder(string dir)
		{
			bool isFolderCreated = false;
            const string randChrs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string folderPath;
            do
            {
                string subDirName = "";
                for (int i = 0; i < 6; i++)
                {
                    subDirName += randChrs[_rand.Next(0, randChrs.Length - 1)];
                }
                subDirName += DateTime.Now.ToString("yyyyMMddhhmmss");
                folderPath = CombinePaths(dir, subDirName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    isFolderCreated = true;
                }
            } while (!isFolderCreated);
            return folderPath;
		}

		/// <summary>
		/// Appends the suffix at the end of the file name but before the extension.
		/// </summary>
		public static string AppendSuffix(string path, string suffix) 
			=> CombinePaths(
				Path.GetDirectoryName(path),
				string.Concat(Path.GetFileNameWithoutExtension(path), suffix, Path.GetExtension(path)));

		/// <summary>
		/// Removes invalid characters from the passed in file name.
		/// </summary>
		public static string CleanFileName(string fileName) 
			=> string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));

		/// <summary>
		/// Start the given process.  
		/// </summary>
		public static void ProcessStart(Process process) 
			=> process.Start();

		/// <summary>
		/// Start a new process with the given path and arguments.  
		/// </summary>
		/// <param name="createDirIfNeeded">If included, will create the directory if it doesn't exist.</param>
		public static Process ProcessStart(string path, string commandLineArgs = "", string createDirIfNeeded = "")
		{
			if (!string.IsNullOrEmpty(createDirIfNeeded) && !Directory.Exists(createDirIfNeeded))
			{
				Directory.CreateDirectory(createDirIfNeeded);
			}
			return Process.Start(path, commandLineArgs);
		}

		public static void WriteAllText(string path, string text) 
			=> File.WriteAllText(path, text);

		public static Process WriteAllTextThenStart(string path, string text, string processPath, string commandLine = "") 
			=> WriteAllTextThenStart(path, text, Encoding.UTF8, processPath, commandLine);

		public static Process WriteAllTextThenStart(string path, string text, Encoding encoding, string processPath, string commandLine) 
			=> WriteAllBytesThenStart(path, encoding.GetBytes(text), processPath, commandLine);

		/// <summary>Write the given filebytes and launches a file.</summary>
		/// <param name="path">The location to write the bytes to.</param>
		/// <param name="bytes">The bytes to write to the file.</param>
		/// <param name="processPath">The path of the file to launch.</param>
		/// <param name="commandLine">Command line arguments to pass to processPath.</param>
		public static Process WriteAllBytesThenStart(string path, byte[] bytes, string processPath, string commandLine)
		{
			File.WriteAllBytes(path, bytes);

			if (!string.IsNullOrEmpty(processPath)) path = processPath;

			return Process.Start(path, commandLine);
		}

		/// <summary>
		/// Creates a list of all filepaths found in the given text. If path is to specific file (ex.- ~/test/test.txt)
		/// then the parent directory will be returned (~/test).
		/// Overview: Files can start with \\ or, drive colon slash (ex. C:\ or F:/). 
		/// Filepaths that end with folder (no extension) must end with slash followed by either a space (or return), period, comma, semi-colon, or end-of-file
		/// Filepaths that end with an extension (.txt) can be followed with a space (or return), period, comma or semi-colon, or end-of-file and stil be found
		/// Regex breakdown: Split into two groups: \\ and X:\ (where X is any mapped drive letter)
		///		All capturing groups will be found based on the existence of a space (or return), period, comma, semi-colon, or end-of-file
		/// First alternative capuring group: 
		///		(\\\\\w(([\w. \\-]*?)(?=(\\)[\s,.;]|(\\)\z) -- start with \\ and end in slash
		///		| [\w. \\-]*?(\.[a-zA-Z]{1,4}(?=[\s,.;]|\z)))) -- still staring with \\ but ending in a file extension (i.e .txt)
		/// Second alternative capturing group: 
		///		([a-zA-Z]\:(\\|\/) -- start with letter drive immediately followed by colon and a slash.
		///		((\s|\z) | \w[\w-. \\\/]*?((?=(\\|\/)[\s,.;](\\|\/)\z) -- Find all paths without extensions
		///		| (\.[a-zA-Z]{1,4}(?=[\s,.;]))))) -- OR find all paths with a file extension
		/// </summary>
		/// <param name="text">Plain text that could contain filepaths.</param>
		/// <returns>List of UNC Paths</returns>
		public static List<string> GetFilePathsFromText(string text)
		{
			List<string> listStringMatches = Regex.Matches(text,
				@"(\\\\\w(([\w. \\-]*?)(?=(\\)[\s,.;]|(\\)\z)|[\w. \\-]*?(\.[a-zA-Z]{1,4}(?=[\s,.;]|\z))))|([a-zA-Z]{1}\:(\\|\/)((\s|\z)|\w[\w-. \\\/]*((?=(\\|\/)[\s,.;]|(\\|\/)\z)|(\.[a-zA-Z]{1,4}(?=[\s,.;]|\z)))))")
				.OfType<Match>().Select(m => m.Groups[0].Value).Distinct().ToList();
			List<string> folderPathsOnly = new List<String>(listStringMatches.Count);
			foreach (string match in listStringMatches)
			{
				string folderPath = match;
				try
				{
					//In regex we pick up extra white space but we don't want to open the file explorer with a file path with white space attached 
					folderPath = Regex.Replace(folderPath, @"[\s]+$", "");
					//If string has extension, assuming specific file
					if (Path.GetExtension(folderPath) != "")
					{
						//If text is a specific file, truncate to the parent directory
						folderPath = new FileInfo(folderPath).Directory.FullName;
					}
				}
				catch
				{
					//We don't want this method to throw any errors. If the path doesn't exist we want to preserve what was found and throw when the 
					//user clicks to navigate to the selected path. See OpenFileExplorer().
				}
				folderPathsOnly.Add(folderPath);
			}
			return folderPathsOnly;
		}
	}
}
