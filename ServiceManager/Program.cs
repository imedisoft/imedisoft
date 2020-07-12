using System;
using System.Windows.Forms;

namespace ServiceManager
{
    static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			if (args != null && args.Length > 0)
			{
				Application.Run(new FormServiceManage(args[0], true, false));
			}
			else
			{
				Application.Run(new FormMain());
			}
		}
	}
}
