using CodeBase;
using OpenDentBusiness;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace OpenDental
{
	static class ProgramEntry
	{
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			try
			{
				// The default SecurityProtocol is "Ssl3|Tls".  We must add Tls12 in order to support Tls1.2 web reference handshakes, 
				// without breaking any web references using Ssl3 or Tls. This is necessary for XWeb payments.
				ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
			}
			catch (Exception e)
			{
				FriendlyException.Show("Critical Error: " + e.Message, e, "Quit");

				return;
			}

			//Register an EventHandler which handles unhandled exceptions.
			//AppDomain.CurrentDomain.UnhandledException+=new UnhandledExceptionEventHandler(OnUnhandeledExceptionPolicy);



			bool isSecondInstance = false;

			foreach (var process in Process.GetProcesses())
            {
				if (process.Id == Process.GetCurrentProcess().Id)
                {
					continue;
                }

				if (process.ProcessName == Process.GetCurrentProcess().ProcessName)
                {
					isSecondInstance = true;

					break;
                }
            }

			Application.DoEvents();




			string[] cla = new string[args.Length];
			args.CopyTo(cla, 0);
			FormOpenDental formOD = new FormOpenDental(cla);
			Exception submittedException = null;
			Action<Exception, string> onUnhandled = new Action<Exception, string>((e, threadName) =>
			{
				//Try to automatically submit a bug report to HQ.
				string displayMsg = "";
				try
				{
					//We want to submit a maximum of one exception per instance of OD.
					if (submittedException == null)
					{
						submittedException = e;
						// TODO: BugSubmissions.SubmitException(e,out displayMsg,threadName,FormOpenDental.CurPatNum,formOD.GetSelectedModuleName());
					}
				}
				catch
				{

				}
				FriendlyException.Show((displayMsg.IsNullOrEmpty()) ? "Critical Error: " + e.Message : displayMsg, e, "Quit");
				formOD.ProcessKillCommand();
			});

			CodeBase.ODThread.RegisterForUnhandledExceptions(formOD, onUnhandled);
			formOD.IsSecondInstance = isSecondInstance;
			Application.AddMessageFilter(new ODGlobalUserActiveHandler());
			Application.ThreadException += new ThreadExceptionEventHandler((object s, ThreadExceptionEventArgs e) =>
			{
				onUnhandled(e.Exception, "ProgramEntry");
			});

			Application.Run(formOD);
		}
	}
}
