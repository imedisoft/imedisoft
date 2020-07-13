using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDentBusiness.FileIO
{
	/// <summary>
	/// This class is used to access files in the A to Z folder.
	/// 
	/// Depending on the storage type in use, it will read/write to a local location or it will download/upload from the cloud. All methods are synchronous.
	/// </summary>
	public class FileAtoZ
	{
		/// <summary>
		/// Remembers the computerpref.AtoZpath. 
		/// Set to empty string on startup. 
		/// If set to something else, this path will override all other paths.
		/// </summary>
		public static string LocalAtoZpath = null;

		/// <summary>
		/// Only makes a call to the database on startup.
		/// After that, just uses cached data.
		/// Does not validate that the path exists except if the main one is used.
		/// </summary>
		public static string GetPreferredAtoZpath()
		{
			if (LocalAtoZpath == null)
			{
				try
				{
					LocalAtoZpath = ComputerPrefs.LocalComputer.AtoZpath;
				}
				catch
				{
					LocalAtoZpath = "";
				}
			}

			// Override path. Because it overrides all other paths, we evaluate it first.
			if (!string.IsNullOrEmpty(LocalAtoZpath)) return LocalAtoZpath.Trim();

			// Use this to handle possible multiple paths separated by semicolons.
			return GetValidPathFromString(PrefC.GetString(PrefName.DocPath))?.Trim();
		}

		/// <summary>
		/// Returns the first valid AtoZ path from the paths passed in.
		/// 
		/// Set documentPaths to a single path or a semicolon delimited string representing multiple paths.
		/// A valid path is considered as the first path that contains a folder named 'A'.
		/// </summary>
		public static string GetValidPathFromString(string documentPaths)
		{
			foreach (string path in documentPaths.Split(new char[] { ';' }))
			{
				string tryPath = Path.Combine(path, "A");

				if (Directory.Exists(tryPath))
				{
					return path;
				}
			}

			return null;
		}

		/// <summary>
		/// Creates the specified directory.
		/// </summary>
		public static void CreateDirectory(string path) 
			=> Directory.CreateDirectory(path);

		/// <summary>
		/// Creates the specified directory.  Throws exceptions.
		/// 
		/// This method will dynamically prepend the correct AtoZ path to the folder path provided.
		/// E.g. passing in 'wiki' as the value for folder might create the folder '\\server\OpenDentImages\wiki\'.
		/// folder can also be set to a relative path like 'wiki\lists\images' which creates '\\server\OpenDentImages\wiki\lists\images'.
		/// </summary>
		public static void CreateDirectoryRelative(string folder) 
			=> Directory.CreateDirectory(CombinePaths(GetPreferredAtoZpath(), folder));
		
		/// <summary>
		/// Returns the string contents of the file.
		/// </summary>
		public static string ReadAllText(string path) 
			=> File.ReadAllText(path);
			
		/// <summary>
		/// Returns the byte contents of the file.
		/// </summary>
		public static byte[] ReadAllBytes(string fileFullPath) 
			=> File.ReadAllBytes(fileFullPath);

		/// <summary>
		/// Writes or uploads the text to the specified file name.
		/// </summary>
		public static void WriteAllText(string fileFullPath, string contents) 
			=> File.WriteAllText(fileFullPath, contents);

		/// <summary>
		/// Writes or uploads the text to the specified file name.
		/// 
		/// This method will dynamically prepend the correct AtoZ path to the folder path provided.
		/// E.g. passing in 'wiki' as the value for folder might create a file within the folder '\\server\OpenDentImages\wiki\[fileName]'.
		/// folder can be set to a relative path like 'wiki\lists\images' which creates '\\server\OpenDentImages\wiki\lists\images\[fileName]'.
		/// </summary>
		public static void WriteAllTextRelative(string folder, string fileName, string contents) 
			=> WriteAllText(CombinePaths(GetPreferredAtoZpath(), folder, fileName), contents);

		/// <summary>
		/// Writes or uploads the bytes to the specified file name.
		/// </summary>
		public static void WriteAllBytes(string fileFullPath, byte[] byteArray) 
			=> File.WriteAllBytes(fileFullPath, byteArray);

		/// <summary>
		/// Gets a list of the files in the specified directory.
		/// </summary>
		public static List<string> GetFilesInDirectory(string folderFullPath) 
			=> Directory.GetFiles(folderFullPath).ToList();

		/// <summary>
		/// Use this instead of ODFileUtils.CombinePaths when the path is in the A to Z folder.
		/// </summary>
		public static string CombinePaths(params string[] paths) 
			=> Path.Combine(paths);

		/// <summary>
		/// Use this instead of ODFileUtils.AppendSuffix when the path is in the A to Z folder.
		/// </summary>
		public static string AppendSuffix(string path, string suffix) 
			=> ODFileUtils.AppendSuffix(path, suffix);

		/// <summary>
		/// Returns true if the file exists.
		/// </summary>
		public static bool Exists(string path) 
			=> File.Exists(path);
		
		/// <summary>
		/// Returns true if the file exists.
		/// 
		/// This method will dynamically prepend the correct AtoZ path to the folder path provided.
		/// E.g. passing in 'wiki' as the value for folder might create the folder '\\server\OpenDentImages\wiki\'.
		/// folder can also be set to a relative path like 'wiki\lists\images' which creates '\\server\OpenDentImages\wiki\lists\images'.
		/// </summary>
		public static bool ExistsRelative(string folder, string fileName) 
			=> Exists(CombinePaths(GetPreferredAtoZpath(), folder, fileName));
		
		public static void Copy(string sourceFileName, string destFileName, bool overwrite = false) 
			=> File.Copy(sourceFileName, destFileName, overwrite);

		public static void Move(string sourceFileName, string destFileName) 
			=> File.Move(sourceFileName, destFileName);

		public static void Delete(string path) 
			=> File.Delete(path);
			
		public static bool DirectoryExists(string path) 
			=> Directory.Exists(path);

		/// <summary>
		/// Returns true if the directory exists.
		/// 
		/// This method will dynamically prepend the correct AtoZ path to the folder path provided.
		/// E.g. passing in 'wiki' as the value for folder might search for the folder '\\server\OpenDentImages\wiki\'.
		/// folder can also be set to a relative path like 'wiki\lists\images' which searches for '\\server\OpenDentImages\wiki\lists\images'.
		/// </summary>
		public static bool DirectoryExistsRelative(string folder) 
			=> DirectoryExists(CombinePaths(GetPreferredAtoZpath(), folder));

		/// <summary>
		/// Returns null if the the image could not be downloaded.
		/// </summary>
		public static Bitmap GetImage(string imagePath) 
			=> new Bitmap(Image.FromFile(imagePath));
	}
}

namespace OpenDentBusiness
{
	///<summary>Used to specify where the files are coming from and going when copying.</summary>
	public enum FileAtoZSourceDestination
	{
		///<summary>Copying a local file to AtoZ folder. Equivalent to 'upload.'</summary>
		LocalToAtoZ,
		///<summary>Copying an AtoZ file to a local file. Equivalent to 'download'.</summary>
		AtoZToLocal,
		///<summary>Copying an AtoZ file to another AtoZ file. Equivalent to 'download' then 'upload'.</summary>
		AtoZToAtoZ
	}
}
