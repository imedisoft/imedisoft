using CodeBase;
using OpenDentBusiness.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OpenDentBusiness
{
    public class Plugins
	{
		private static readonly List<PluginContainer> plugins = new List<PluginContainer>();

		/// <summary>
		/// Gets a value indicating whether there are plugins loaded.
		/// </summary>
		public static bool PluginsAreLoaded => plugins != null && plugins.Count > 0;

		/// <summary>
		/// Loads all plugins.
		/// </summary>
		/// <param name="host">The host form.</param>
		public static void LoadAllPlugins(Form host)
		{
			// TODO: Need to implement better plugin loaded logic.
		}

		/// <summary>
		/// Will return true if a plugin implements this method, replacing the default behavior.
		/// </summary>
		public static bool HookMethod(object sender, string hookName, params object[] parameters)
		{
			if (plugins.Count == 0) return false;

			foreach (var container in plugins)
            {
                try
                {
					if (container.Plugin.HookMethod(sender, hookName, parameters))
                    {
						return true;
                    }
                }
				catch (Exception exception)
				{
					try
					{
						container.Plugin.HookException(exception);
					}
					catch
					{
						UnhandledException(container, exception);
					}
				}
            }

			return false;
		}

		/// <summary>
		/// Adds code without disrupting existing code.
		/// </summary>
		public static void HookAddCode(object sender, string hookName, params object[] parameters)
		{
			if (plugins.Count == 0) return;

			foreach (var container in plugins)
            {
                try
                {
					container.Plugin.HookAddCode(sender, hookName, parameters);
                }
                catch (Exception exception)
				{
                    try
                    {
						container.Plugin.HookException(exception);
                    }
                    catch 
					{
						UnhandledException(container, exception);
					}
                }
            }
		}

		public static void LaunchToolbarButton(long programNum, long patNum)
		{
			if (plugins == null) return;

			var container = plugins.FirstOrDefault(x => x.ProgramNum == programNum && x.Plugin != null);

			if (container != null)
			{
				try
				{
					container.Plugin.LaunchToolbarButton(patNum);
				}
				catch (Exception exception)
				{
					try
					{
						container.Plugin.HookException(exception);
					}
					catch
					{
						UnhandledException(container, exception);
					}
				}
			}
		}

		private static void UnhandledException(PluginContainer source, Exception exception)
        {
			// TODO: Implement me.
        }
	}
}
