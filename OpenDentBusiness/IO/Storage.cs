using OpenDentBusiness.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness.IO
{
    public static class Storage
    {
        public static string CombinePaths(params string[] paths)
            => Path.Combine(paths);

        public static IEnumerable<string> EnumerateDirectory(string path)
            => Directory.GetFiles(path);

        public static bool DirectoryExists(string path)
            => Directory.Exists(path);

        public static void OpenDirectory(string path)
            => Run(path);

        public static void CreateDirectory(string path)
            => Directory.CreateDirectory(path);

        public static bool FileExists(string path)
            => File.Exists(path);

        public static void DeleteFile(string path)
            => File.Delete(path);

        public static void Copy(string sourceFileName, string destFileName, bool overwrite = false)
            => File.Copy(sourceFileName, destFileName, overwrite);

        public static void Upload(string sourceFileName, string destFileName)
            => Copy(sourceFileName, destFileName);

        public static Bitmap GetImage(string imagePath)
            => new Bitmap(imagePath);

        public static void Run(string path, string arguments = "")
            => Process.Start(path, arguments);

        public static void RunRelative(string path)
            => Run(CombinePaths(GetRootPath(), path));

        public static void WriteAllText(string path, string contents)
            => File.WriteAllText(path, contents);

        public static string ReadAllText(string path)
            => File.ReadAllText(path);

        public static string GetRootPath()
            => FileAtoZ.GetPreferredAtoZpath();

        public static string GetTempPath()
        {
            var path = Path.Combine(Path.GetTempPath(), "Imedisoft");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetTempFileName()
            => Path.Combine(GetTempPath(), Path.GetRandomFileName());

        public static string GetTempFileName(string extension)
            => Path.Combine(GetTempPath(), string.Concat(Path.GetRandomFileName(), extension));
    }
}
