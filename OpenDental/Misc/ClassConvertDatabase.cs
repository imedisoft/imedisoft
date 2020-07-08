using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class ClassConvertDatabase
	{
		/// <summary>
		/// Return false to indicate exit app. 
		/// Only called when program first starts up at the beginning of FormOpenDental.PrefsStartup.
		/// </summary>
		[Obsolete]
		public bool Convert(string fromVersion, string toVersion, bool isSilent, Form currentForm, bool useDynamicMode)
		{
			return true;
		}
	}
}