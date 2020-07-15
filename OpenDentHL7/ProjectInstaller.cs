using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace OpenDentServer
{
    [RunInstaller(true)]
	public class MyProjectInstaller : Installer
	{
		private readonly ServiceInstaller serviceInstaller;
		private readonly ServiceProcessInstaller serviceProcessInstaller;

		public MyProjectInstaller()
		{
			serviceProcessInstaller = new ServiceProcessInstaller();
			serviceInstaller = new ServiceInstaller();
			serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
			serviceInstaller.StartType = ServiceStartMode.Automatic;
			serviceInstaller.ServiceName = "OpenDentHL7";

			string[] args = Environment.GetCommandLineArgs();
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].StartsWith("/ServiceName"))
				{
					serviceInstaller.ServiceName = args[i].Substring(13);
				}
			}

			Installers.Add(serviceInstaller);
			Installers.Add(serviceProcessInstaller);
		}
	}
}
