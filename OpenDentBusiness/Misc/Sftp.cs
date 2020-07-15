using CodeBase;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OpenDentBusiness
{
    public static class Sftp
	{
		private static SftpClient CreateClient(string host, string username, string password, int port = 22) 
			=> new SftpClient(
				new ConnectionInfo(host, port, username, 
					new AuthenticationMethod[] { new PasswordAuthenticationMethod(username, password) }));

		public static async void Upload(string host, string username, string password, int port, string path, string fileName, byte[] contents)
        {
			await System.Threading.Tasks.Task.Run(() =>
            {
                var client = CreateClient(host, username, password, port);

                if (!client.IsConnected)
                {
                    client.Connect();
                }

                CreateDirectories(client, path);

                path = string.Join(path, '/', fileName);
                using (var memoryStream = new MemoryStream(contents))
                {
                    client.UploadFile(memoryStream, path, true);
                }

                client.Disconnect();
            });
		}

		public static bool TestConnection(string host, string username, string password, int port = 22)
		{
			bool result = false;

			try
			{
				var client = CreateClient(host, username, password, port);

				client.Connect();

				if (client.IsConnected)
				{
					result = true;

					client.Disconnect();
				}
			}
			catch
			{
			}

			return result;
		}

		private static void CreateDirectories(SftpClient client, string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return;
			}

			string currentPath = "";

			string[] directories = path.Split("/", StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < directories.Length; i++)
			{
				if (i > 0 || path[0] == '/')
				{
					currentPath += "/";
				}
				currentPath += directories[i];

				try
				{
					// This will throw an exception of SftpPathNotFoundException if the directory does not exist
					SftpFileAttributes attributes = client.GetAttributes(currentPath);

					// Check to see if it's a directory. This will not throw an exception of SftpPathNotFoundException, so we want to break out if it's a file path.
					// This would be a weird permission issue or implementation error, but it doesn't hurt anything.
					if (!attributes.IsDirectory) break;
				}
				catch (SftpPathNotFoundException)
				{
					client.CreateDirectory(currentPath);
				}
			}
		}
	}
}
