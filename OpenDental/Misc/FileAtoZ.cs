using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace OpenDental
{
    /// <summary>
    /// This class is used to access files in the AtoZ folder.
    /// 
    /// Depending on the storage type in use, it will read/write to a local location or it will download/upload from the cloud.
    /// </summary>
	[Obsolete("Use the OpenDentBusiness.FileIO.FileAtoZ class instead of this one")]
    public class FileAtoZ
	{
		/// <summary>
		/// Returns the string contents of the file.
		/// </summary>
		public static string ReadAllText(string fileName) 
			=> File.ReadAllText(fileName);

		/// <summary>
		/// Writes or uploads the text to the specified file name.
		/// </summary>
		public static void WriteAllText(string fileName, string textForFile) 
			=> File.WriteAllText(fileName, textForFile);

		/// <summary>
		/// Gets a list of the files in the specified directory.
		/// An absolute path is required, call GetFilesInDirectoryRelative() for passing in a relative AtoZ folder path.
		/// </summary>
		public static List<string> GetFilesInDirectory(string folderFullPath) 
			=> OpenDentBusiness.FileIO.FileAtoZ.GetFilesInDirectory(folderFullPath);

		/// <summary>
		/// Copies or downloads the file and opens it. acutalFileName should be a full path, displayedFileName should be a file name only.
		/// </summary>
		public static void OpenFile(string path, string displayedFileName = "")
		{
			try
			{
				string tempPath;
				if (string.IsNullOrEmpty(displayedFileName))
				{
					tempPath = ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(), Path.GetFileName(path));
				}
				else
				{
					tempPath = ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(), displayedFileName);
				}
	
				File.Copy(path, tempPath, true);

				Process.Start(tempPath);

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Use this instead of ODFileUtils.CombinePaths when the path is in the A to Z folder.
		/// </summary>
		public static string CombinePaths(params string[] paths) 
			=> OpenDentBusiness.FileIO.FileAtoZ.CombinePaths(paths);

		/// <summary>
		/// Use this instead of ODFileUtils.AppendSuffix when the path is in the A to Z folder.
		/// </summary>
		public static string AppendSuffix(string path, string suffix) 
			=> OpenDentBusiness.FileIO.FileAtoZ.AppendSuffix(path, suffix);

		/// <summary>
		/// Returns true if the file exists. If cloud, checks if the file exists in the cloud.
		/// </summary>
		public static bool Exists(string path) 
			=> OpenDentBusiness.FileIO.FileAtoZ.Exists(path);
		
		/// <summary>
		/// Returns null if the the image could not be downloaded or the user canceled the download.
		/// </summary>
		public static Bitmap GetImage(string imagePath) 
			=> new Bitmap(imagePath);

		/// <summary>
		/// This function will throw if the Process fails to start. Catch in calling class. Runs the file.
		/// </summary>
		public static void StartProcess(string path) 
			=> Process.Start(path);
		
		/// <summary>
		/// Runs the file.  f necessary, downloads the file from the cloud to the temp directory.
		/// This method will dynamically prepend the correct AtoZ path to the folder path provided.
		/// E.g. passing in 'wiki' as the value for folder might open a file within the folder '\\server\OpenDentImages\wiki\[fileName]'.
		/// folder can be set to a relative path like 'wiki\lists\images' which opens '\\server\OpenDentImages\wiki\lists\images\[fileName]'.
		/// </summary>
		public static void StartProcessRelative(string folder, string fileName) 
			=> StartProcess(CombinePaths(OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath(), folder, fileName));

		/// <summary>
		/// The first parameter, 'sourceFileName', must be a file that exists.
		/// </summary>
		public static void Copy(string sourceFileName, string destFileName, bool overwrite = false) 
			=> File.Copy(sourceFileName, destFileName, overwrite);

		/// <summary>
		/// Deletes the file.
		/// </summary>
		public static void Delete(string fileName) 
			=> OpenDentBusiness.FileIO.FileAtoZ.Delete(fileName);

		/// <summary>
		/// Returns true if the directory exists.
		/// </summary>
		public static bool DirectoryExists(string path) 
			=> OpenDentBusiness.FileIO.FileAtoZ.DirectoryExists(path);

		/// <summary>
		/// Opens the directory. If cloud, opens the directory in FormFilePicker.
		/// </summary>
		public static void OpenDirectory(string path) 
			=> Process.Start(path);

		/// <summary>
		/// Uploads an A to Z file to the local machine.
		/// </summary>
		public static void Download(string AtoZFilePath, string localFilePath) 
			=> Copy(AtoZFilePath, localFilePath);

		/// <summary>
		/// Uploads a local file to the A to Z folder.
		/// </summary>
		public static void Upload(string sourceFileName, string destFileName) 
			=> Copy(sourceFileName, destFileName);
	}
}
