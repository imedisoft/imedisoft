using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;

namespace OpenDentHL7
{
    static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
#if DEBUG
			string serviceName = "OpenDentHL7";

			var serviceControllers = ServiceController.GetServices();
			foreach (var serviceController in serviceControllers)
			{
				if (serviceController.ServiceName.StartsWith("OpenDent"))
				{
					serviceName = serviceController.ServiceName;
					break;
				}
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FormDebug(serviceName));
#else
			EventLog.WriteEntry("OpenDentHL7.Main", DateTime.Now.ToLongTimeString() + " - Service main method starting...");

			var serviceHL7 = new ServiceHL7();
			serviceHL7.ServiceName = "OpenDentalHL7";

			// Get all installed services.
			var serviceControllers = new List<ServiceController>();
			foreach (var serviceController in ServiceController.GetServices())
			{
				if (serviceController.ServiceName.StartsWith("OpenDent"))
				{
					serviceControllers.Add(serviceController);
				}
			}

			string executingDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

			// Get the service that is installed from the same directory as the current directory.
			for (int i = 0; i < serviceControllers.Count; i++)
			{
				var serviceRegKey = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Services\" + serviceControllers[i].ServiceName);

				var serviceDirectory = Path.GetDirectoryName(
					serviceRegKey.GetValue("ImagePath").ToString().Replace("\"", ""));

				if (serviceDirectory == executingDirectory)
				{
					serviceHL7.ServiceName = serviceControllers[i].ServiceName;
					break;
				}
			}

			ServiceBase.Run(serviceHL7);

			EventLog.WriteEntry("OpenDentHL7.Main", DateTime.Now.ToLongTimeString() + " - Service main method exiting...");
#endif
		}
	}
}
